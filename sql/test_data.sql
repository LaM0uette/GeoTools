-- ******** TABLES ******** --
-- t_dlg
TRUNCATE t_dlg RESTART IDENTITY CASCADE;

SELECT *
FROM t_dlg;

INSERT INTO t_dlg (dl_proj_us_id, dl_exe_us_id, dl_rc_id, dl_date_init, dl_ph_id, dl_te_id, dl_livraison, dl_version)
VALUES (2, 1, 1, CURRENT_DATE, 2, 3, 1, 1),
       (2, 1, 1, CURRENT_DATE, 2, 3, 1, 2),
       (2, 1, 1, CURRENT_DATE, 2, 2, 2, 1),
       (2, 1, 2, CURRENT_DATE, 3, 2, 2, 1),
       (2, 1, 3, CURRENT_DATE, 3, 3, 1, 1),
       (2, 1, 4, CURRENT_DATE, 3, 3, 1, 2),
       (2, 1, 4, CURRENT_DATE, 2, 2, 2, 1);
-- ...


-- ******** VUES ******** --
-- v_l_refcode
create or replace view "data"."v_refcode" as
SELECT *
FROM "data".l_refcode;


-- v_dlg
create or replace view "GeoTools"."v_dlg" as
WITH dlg AS (SELECT dl.dl_id                                               AS id,
                    usp.us_guid                                            AS guid_projeteur,
                    (usp.us_nom::text || ' '::text) || usp.us_prenom::text AS projeteur,
                    use.us_guid                                            AS guid_executant,
                    (use.us_nom::text || ' '::text) || use.us_prenom::text AS executant,
                    rc.rc_nro                                              AS nro,
                    rc.rc_sro                                              AS sro,
                    rc.rc_refcode1                                         AS refcode1,
                    rc.rc_refcode2                                         AS refcode2,
                    rc.rc_refcode3                                         AS refcode3,
                    dl.dl_date_init                                        AS date_initial,
                    extract('week' from dl.dl_date_init)                   AS semaine,
                    extract('month' from dl.dl_date_init)                   AS mois,
                    extract('year' from dl.dl_date_init)                   AS annee,
                    ph.ph_nom                                              AS phase,
                    te.te_code                                             AS code_type_export,
                    te.te_nom                                              AS type_export,
                    dl.dl_livraison                                        AS livraison,
                    dl.dl_version                                          AS version,
                    (SELECT ex.ex_id
                     FROM "GeoTools"."t_exports" ex
                     WHERE ex.ex_dl_id = dl.dl_id
                     ORDER BY ex.ex_date DESC
                     LIMIT 1)                                              AS id_export
             FROM "GeoTools"."t_dlg" dl
                      LEFT JOIN "GeoTools"."t_users" usp ON usp.us_id = dl.dl_proj_us_id
                      LEFT JOIN "GeoTools"."t_users" use ON use.us_id = dl.dl_exe_us_id
                      JOIN "data"."l_refcode" rc ON rc.rc_id = dl.dl_rc_id::double precision
                      JOIN "GeoTools"."l_phases" ph ON ph.ph_id = dl.dl_ph_id
                      JOIN "GeoTools"."l_type_export" te ON te.te_id = dl.dl_te_id)
SELECT dlg.id,
       dlg.guid_projeteur,
       dlg.projeteur,
       dlg.guid_executant,
       dlg.executant,
       dlg.nro,
       dlg.sro,
       dlg.refcode1,
       dlg.refcode2,
       dlg.refcode3,
       dlg.date_initial,
       dlg.semaine,
       dlg.mois,
       dlg.annee,
       dlg.phase,
       dlg.type_export,
       dlg.livraison,
       dlg.version,
       dlg.id_export,
       CASE
           WHEN et.et_id IN (1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 12)
               THEN 1
           WHEN et.et_id IN (11, 13)
               THEN 2
           ELSE 0
           END                                                                              AS "id_etat",
       et.et_code                                                                           AS code_etat,
       et.et_nom                                                                            AS nom_etat,
       et.et_color                                                                          AS couleur_etat,
       ex.ex_date                                                                           AS date_etat,
       LEFT(dlg.phase, 3) || '-DLG-' || dlg.refcode1 || '-' || dlg.refcode2 || '-' || dlg.refcode3 || '-' ||
       to_char(dlg.livraison, 'fm00') || '-V' || dlg.version                                as dlg,
       'NRO' || dlg.nro || '-' || 'PM' || dlg.sro || '|' || dlg.refcode2 || '-' || dlg.refcode3 || '-' ||
       LEFT(dlg.phase, 3) || '|' ||
       dlg.code_type_export || '-' || to_char(dlg.livraison, 'fm00') || '-V' || dlg.version as dlg_infos
FROM dlg
         JOIN "GeoTools"."t_exports" ex ON ex.ex_id = dlg.id_export
         JOIN "GeoTools"."l_etats" et ON et.et_id = ex.ex_et_id;
-- ...


-- v_exports
create or replace view "GeoTools"."v_exports" as
SELECT ex.ex_id    as id,
       ex.ex_dl_id as dlg_id,
       et.et_code  as code_etat,
       et.et_nom   as nom_etat,
       et_code     as couleur_etat
FROM "GeoTools"."t_exports" ex
         INNER JOIN "GeoTools".l_etats et
                    ON et.et_id = ex.ex_et_id
ORDER BY ex.ex_dl_id, ex.ex_id;
-- ...


-- ******** PROCEDURES ******** --


-- Ajouter un nouveau DLG avec des int
-- create or replace function _add_dlg(
--     proj_us_id int, rc_id int, date_init DATE, ph_id int, te_id int, livraison int, version int) returns void
-- as
-- $BODY$
-- begin
--     INSERT INTO "GeoTools".t_dlg (dl_proj_us_id, dl_rc_id, dl_date_init,
--                                   dl_ph_id, dl_te_id, dl_livraison, dl_version)
--     SELECT proj_us_id, rc_id, date_init, ph_id, te_id, livraison, version
--     WHERE NOT EXISTS(SELECT dl_id
--                      FROM "GeoTools".t_dlg
--                      WHERE dl_rc_id = rc_id
--                        AND dl_ph_id = ph_id
--                        AND dl_te_id = te_id
--                        AND dl_livraison = livraison
--                        AND dl_version = version);
-- end;
-- $BODY$
--     LANGUAGE plpgsql STABLE
--                      COST 100;
--
-- SELECT _add_dlg(2, 1, CURRENT_DATE, 2, 3, 1, 1);
-- ...


-- Ajouter un nouveau DLG
create or replace function add_dlg(
    proj varchar, refcode3 varchar, date_init DATE, ph varchar, te varchar, livraison int, version int) returns void
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
    LANGUAGE plpgsql STABLE
                     COST 100;

SELECT add_dlg('XD5965', 'NISY', CURRENT_DATE, 'EXE', 'TRANSPORT ET DISTRIBUTION', 1, 1);
-- ...


-- Ajouter un nouvel export à un DLG
create or replace function add_export_to_dlg(dlg_id int, etat_id int) returns void
as
$BODY$
begin
    INSERT INTO "GeoTools".t_exports (ex_dl_id, ex_et_id)
    VALUES (dlg_id, etat_id);
end;
$BODY$
    LANGUAGE plpgsql STABLE
                     COST 100;

SELECT add_export_to_dlg(5, 3);
-- ...


-- Obtenir un seul DLG
create or replace function get_dlg(i int) returns setof "GeoTools".v_dlg
as
$BODY$
begin
    RETURN QUERY (SELECT * FROM "GeoTools"."v_dlg" WHERE id = i);
end
$BODY$
    LANGUAGE plpgsql STABLE
                     COST 100;

SELECT * FROM get_dlg(2);
-- ...


-- Obtenir la liste des DLG à une date précise
create or replace function get_dlg_by_date(dt date) returns setof "GeoTools".v_dlg
as
$BODY$
begin
    RETURN QUERY (SELECT * FROM "GeoTools".v_dlg WHERE DATE(date_initial) = dt);
end
$BODY$
    LANGUAGE plpgsql STABLE
                     COST 100;

SELECT *
FROM get_dlg_by_date('2022-06-23');
-- ...


-- Obtenir la liste des DLG à une semaine donnée
create or replace function get_dlg_by_week(week int, year int) returns setof "GeoTools".v_dlg
as
$BODY$
begin
    RETURN QUERY (SELECT * FROM "GeoTools".v_dlg WHERE semaine = week AND annee = year ORDER BY date_initial);
end
$BODY$
    LANGUAGE plpgsql STABLE
                     COST 100;

SELECT *
FROM get_dlg_by_week(25, 2022);
-- ...


-- Obtenir la liste des DLG à un mois donné
create or replace function get_dlg_by_month(month int, year int) returns setof "GeoTools".v_dlg
as
$BODY$
begin
    RETURN QUERY (SELECT * FROM "GeoTools".v_dlg WHERE mois = month AND annee = year ORDER BY date_initial);
end
$BODY$
    LANGUAGE plpgsql STABLE
                     COST 100;

SELECT *
FROM get_dlg_by_month(6, 2022);
-- ...


-- Obtenir les exports d'un DLG
create or replace function get_dlg_exports(dlg int) returns setof "GeoTools".v_exports
as
$BODY$
begin
    RETURN QUERY (SELECT * FROM "GeoTools".v_exports WHERE dlg_id = dlg ORDER BY dlg_id, id);
end;
$BODY$
    LANGUAGE plpgsql STABLE
                     COST 100;

SELECT *
FROM get_dlg_exports(2);
-- ...


-- ******** TRIGGERS ******** --
-- Export a faire à la création d'un DLG
create or replace function tr_add_export_to_dlg() RETURNS trigger AS
$BODY$
begin
    INSERT INTO "GeoTools"."t_exports" (ex_dl_id, ex_et_id)
    VALUES (NEW.dl_id, 1);
    RETURN NEW;
end;
$BODY$
    LANGUAGE plpgsql STABLE
                     COST 100;

create trigger tr_add_export_to_dlg
    after insert
    on "GeoTools"."t_dlg"
    FOR EACH ROW
EXECUTE PROCEDURE "GeoTools".tr_add_export_to_dlg();
-- ...
