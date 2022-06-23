--...
-- t_dlg

TRUNCATE t_dlg RESTART IDENTITY CASCADE;

SELECT *
FROM t_dlg;

-- INSERT INTO t_dlg (dl_proj_us_id, dl_exe_us_id, dl_rc_id, dl_date_init, dl_ph_id, dl_te_id, dl_livraison, dl_version)
-- VALUES (2, 1, 1, CURRENT_DATE, 2, 3, 1, 1),
--        (2, 1, 1, CURRENT_DATE, 2, 3, 1, 2),
--        (2, 1, 1, CURRENT_DATE, 2, 2, 2, 1),
--        (2, 1, 2, CURRENT_DATE, 3, 2, 2, 1),
--        (2, 1, 3, CURRENT_DATE, 3, 3, 1, 1),
--        (2, 1, 4, CURRENT_DATE, 3, 3, 1, 2),
--        (2, 1, 4, CURRENT_DATE, 2, 2, 2, 1);

-- //TODO: A faire
-- Procédure
create or replace function _add_dlg(proj_us_id int, rc_id int, date_init DATE, ph_id int, te_id int,
                                    livraison int, version int) returns void
as
$BODY$
begin
    INSERT INTO "GeoTools".t_dlg (dl_proj_us_id, dl_rc_id, dl_date_init,
                       dl_ph_id, dl_te_id, dl_livraison, dl_version)
    SELECT proj_us_id, rc_id, date_init, ph_id, te_id, livraison, version
    WHERE NOT EXISTS(SELECT dl_id
                     FROM "GeoTools".t_dlg
                     WHERE dl_rc_id = rc_id
                       AND dl_ph_id = ph_id
                       AND dl_te_id = te_id
                       AND dl_livraison = livraison
                       AND dl_version = version);
end;
$BODY$
    LANGUAGE plpgsql VOLATILE
                     COST 100;

SELECT _add_dlg(2, 1, CURRENT_DATE, 2, 3, 1, 1);


-- V2
create or replace function add_dlg(proj varchar, refcode3 varchar, date_init DATE, ph varchar, te varchar,
                                   livraison int, version int) returns void
as
$BODY$
declare
    proj_us_id int = (SELECT us_id
                      FROM "GeoTools".t_users
                      WHERE us_guid = proj);
    rc_id      int = (SELECT rc_id
                      FROM "data".l_refcode
                      WHERE rc_refcode3 = refcode3);
    ph_id      int = (SELECT ph_id
                      FROM "GeoTools".l_phases
                      WHERE ph_nom = ph);
    te_id      int = (SELECT te_id
                      FROM "GeoTools".l_type_export
                      WHERE te_nom = te);
begin
    INSERT INTO "GeoTools".t_dlg (dl_proj_us_id, dl_rc_id, dl_date_init,
                       dl_ph_id, dl_te_id, dl_livraison, dl_version)
    SELECT proj_us_id,
           rc_id,
           date_init,
           ph_id,
           te_id,
           livraison,
           version
    WHERE NOT EXISTS(SELECT dl_id
                     FROM "GeoTools".t_dlg
                     WHERE dl_rc_id = rc_id
                       AND dl_ph_id = ph_id
                       AND dl_te_id = te_id
                       AND dl_livraison = livraison
                       AND dl_version = version);
end;
$BODY$
    LANGUAGE plpgsql VOLATILE
                     COST 100;

SELECT add_dlg('XD5965', 'BIMI', CURRENT_DATE, 'EXE', 'TRANSPORT ET DISTRIBUTION', 1, 1);



--...
-- t_exports

-- Procédure
create or replace function add_export_to_dlg(dlg_id int, etat_id int) returns void
as
$BODY$
begin
    INSERT INTO "GeoTools".t_exports (ex_dl_id, ex_et_id)
    VALUES (dlg_id, etat_id);
end;
$BODY$
    LANGUAGE plpgsql VOLATILE
                     COST 100;

SELECT add_export_to_dlg(5, 3);

-- Trigger pour mettre dans la t_export
create or replace function tr_add_export_to_dlg() RETURNS trigger AS
$BODY$
begin
    INSERT INTO "GeoTools"."t_exports" (ex_dl_id, ex_et_id)
    VALUES (NEW.dl_id, 1);
    RETURN NEW;
end;
$BODY$
    LANGUAGE plpgsql VOLATILE
                     COST 100;

create trigger tr_add_export_to_dlg
    after insert
    on "GeoTools"."t_dlg"
    FOR EACH ROW
EXECUTE PROCEDURE "GeoTools".tr_add_export_to_dlg();



SELECT *
FROM t_exports;

SELECT *
FROM t_dlg dl;

-- ...
-- v_dlg
create or replace view "GeoTools".v_dlg as
WITH dlg AS (SELECT dl_id                              as id,
                    usp.us_guid                        as guid_projeteur,
                    usp.us_nom || ' ' || usp.us_prenom as projeteur,
                    use.us_guid                        as guid_executant,
                    use.us_nom || ' ' || use.us_prenom as executant,
                    rc.rc_refcode1                     as refcode1,
                    rc.rc_refcode2                     as refcode2,
                    rc.rc_refcode3                     as refcode3,
                    dl.dl_date_init                    as date_initial,
                    ph.ph_nom                          as phase,
                    te.te_nom                          as type_export,
                    dl.dl_livraison                    as livraison,
                    dl.dl_version                      as version,
                    (SELECT ex_id
                     FROM "GeoTools".t_exports
                     WHERE ex_dl_id = dl.dl_id
                     ORDER BY ex_date DESC
                     LIMIT 1)                          as id_export
             FROM "GeoTools".t_dlg dl
                      LEFT JOIN "GeoTools".t_users usp
                                ON usp.us_id = dl.dl_proj_us_id
                      LEFT JOIN "GeoTools".t_users use
                                ON use.us_id = dl.dl_exe_us_id
                      INNER JOIN data.l_refcode rc
                                 ON rc.rc_id = dl.dl_rc_id
                      INNER JOIN "GeoTools".l_phases ph
                                 ON ph.ph_id = dl.dl_ph_id
                      INNER JOIN "GeoTools".l_type_export te
                                 ON te.te_id = dl.dl_te_id)
SELECT dlg.*, et.et_code as code_etat, et.et_nom as nom_etat, et.et_color as couleur_etat, ex.ex_date as date_etat
FROM dlg
         INNER JOIN "GeoTools".t_exports ex
                    ON ex.ex_id = dlg.id_export
         INNER JOIN "GeoTools".l_etats et
                    ON et.et_id = ex.ex_et_id;

-- v_exports
create or replace view "GeoTools".v_exports as
SELECT ex.ex_id as id,
       ex.ex_dl_id as dlg_id,
       et.et_code as code_etat,
       et.et_nom as nom_etat,
       et_code as couleur_etat
FROM "GeoTools".t_exports ex
         INNER JOIN "GeoTools".l_etats et
                    ON et.et_id = ex.ex_et_id
ORDER BY ex.ex_dl_id, ex.ex_id;


-- ...
-- Procédures
create or replace function get_dlg_exports(dlg int) returns setof "GeoTools".v_exports
as
$BODY$
begin
    RETURN QUERY (SELECT * FROM "GeoTools".v_exports WHERE dlg_id = dlg ORDER BY dlg_id, id);
end;
$BODY$
    LANGUAGE plpgsql VOLATILE
                     COST 100;

SELECT * FROM get_dlg_exports(2);



create or replace function get_dlg_by_date(dt date) returns setof "GeoTools".v_dlg
as
$BODY$
begin
    RETURN QUERY (SELECT * FROM "GeoTools".v_dlg WHERE DATE(date_initial) = dt);
end
$BODY$
    LANGUAGE plpgsql VOLATILE
                     COST 100;

SELECT * FROM get_dlg_by_date('2022-06-23');
