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
    INSERT INTO t_dlg (dl_proj_us_id, dl_rc_id, dl_date_init,
                       dl_ph_id, dl_te_id, dl_livraison, dl_version)
    SELECT proj_us_id, rc_id, date_init, ph_id, te_id, livraison, version
    WHERE NOT EXISTS(SELECT dl_id
                     FROM t_dlg
                     WHERE dl_rc_id = dl_rc_id
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
                      FROM t_users
                      WHERE us_guid = proj);
    rc_id      int = (SELECT rc_id
                      FROM data.l_refcode
                      WHERE rc_refcode3 = refcode3);
    ph_id      int = (SELECT ph_id
                      FROM l_phases
                      WHERE ph_nom = ph);
    te_id      int = (SELECT te_id
                      FROM l_type_export
                      WHERE te_nom = te);
begin
    INSERT INTO t_dlg (dl_proj_us_id, dl_rc_id, dl_date_init,
                       dl_ph_id, dl_te_id, dl_livraison, dl_version)
    SELECT proj_us_id,
           rc_id,
           date_init,
           ph_id,
           te_id,
           livraison,
           version
    WHERE NOT EXISTS(SELECT dl_id
                     FROM t_dlg
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
    INSERT INTO t_exports (ex_dl_id, ex_et_id)
    VALUES (dlg_id, etat_id);
end;
$BODY$
    LANGUAGE plpgsql VOLATILE
                     COST 100;

SELECT add_export_to_dlg(4, 6);

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

-- ...
-- v_dlg
SELECT *
FROM t_dlg;

