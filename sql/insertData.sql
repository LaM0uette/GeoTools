-- Insertion d'une nouvelle zone (100% nouveau)
INSERT INTO t_zone_dlg (zo_marche,zo_nro,zo_pm,zo_refcode2,zo_refcode3)
SELECT 24, 22, 8, (SELECT cz_refcode2 FROM l_code_zone WHERE cz_refcode3 = 'BIMI'), 'BIMI'
WHERE NOT EXISTS(
    SELECT zo_refcode3
    FROM t_zone_dlg
    WHERE zo_refcode3 = 'BIMI'
    );

-- Insertion d'un nouveau dlg (100% nouveau)
INSERT INTO t_dlg (dl_zo_id,dl_init_date,dl_phase,dl_td,dl_no_livraison,dl_no_version)
SELECT (SELECT dlg.zo_id FROM t_zone_dlg dlg WHERE dlg.zo_refcode3 = 'BIMI') AS zo, '29-03-2022', 'EXE', 'TD', 1, 1
WHERE NOT EXISTS(
    SELECT dl_phase,dl_td,dl_no_livraison,dl_no_version
    FROM t_dlg
    WHERE dl_zo_id = zo
    AND dl_phase = 'EXE'
    AND dl_td = 'TD'
    AND dl_no_livraison = 1
    AND dl_no_version = 1
    );

-- Insertion d'un nouvel export (100% nouveau)
INSERT INTO t_exports (ex_dl_id,ex_no_export,ex_date,ex_et_id)
SELECT 1, 1, datetime('now', 'localtime'), 1;


-- Select le dernier export d'un dlg
SELECT *
FROM v_exports_en_cours
WHERE ex_dl_id = 2
AND ex_date = (SELECT MAX(ex_date) FROM v_exports_en_cours WHERE ex_dl_id = 2);


-- Select tables
SELECT * FROM t_zone_dlg;
SELECT * FROM t_dlg;
SELECT * FROM t_exports;

-- Select vues
SELECT * FROM v_dlg;
SELECT * FROM v_dlg_a_faire;
SELECT * FROM v_dlg_fait;
SELECT * FROM v_exports_en_cours;




-- Delete col
DELETE FROM t_zone_dlg;
UPDATE sqlite_sequence SET seq = 0 WHERE name = 't_zone_dlg';

DELETE FROM t_dlg;
UPDATE sqlite_sequence SET seq = 0 WHERE name = 't_dlg';

DELETE FROM t_exports;
UPDATE sqlite_sequence SET seq = 0 WHERE name = 't_exports';


-- Trigger insert t_dlg
CREATE TRIGGER IF NOT EXISTS tr_insert_t_dlg
   AFTER INSERT
   ON t_dlg
BEGIN
    INSERT INTO t_exports (ex_dl_id,ex_no_export,ex_date,ex_et_id)
    VALUES ((SELECT MAX(dl_id) FROM t_dlg), 1, datetime('now', 'localtime'), 1);
END;






-- Insertion t_etats
-- INSERT INTO t_etats (et_code,et_nom) VALUES ('AFA', 'A FAIRE');
-- INSERT INTO t_etats (et_code,et_nom) VALUES ('GEX', 'GO EXPORT');
-- INSERT INTO t_etats (et_code,et_nom) VALUES ('CMA', 'CRASH MAJEURE');
-- INSERT INTO t_etats (et_code,et_nom) VALUES ('CCH', 'CRASH CHECK');
-- INSERT INTO t_etats (et_code,et_nom) VALUES ('EDL', 'ERREURS DLG');
-- INSERT INTO t_etats (et_code,et_nom) VALUES ('EOK', 'EXPORT OK');
-- INSERT INTO t_etats (et_code,et_nom) VALUES ('DOK', 'DJANGO OK');
-- INSERT INTO t_etats (et_code,et_nom) VALUES ('DNO', 'DJANGO NOK');
-- INSERT INTO t_etats (et_code,et_nom) VALUES ('POK', 'PDB OK');
-- INSERT INTO t_etats (et_code,et_nom) VALUES ('PNO', 'PDB NOK');
-- INSERT INTO t_etats (et_code,et_nom) VALUES ('LCL', 'LIVRAISON CLIENT');
-- INSERT INTO t_etats (et_code,et_nom) VALUES ('PAU', 'PAUSE');
-- INSERT INTO t_etats (et_code,et_nom) VALUES ('ANN', 'ANNULE');

-- INSERT INTO t_phase (ph_nom) VALUES  ('PRO');
-- INSERT INTO t_phase (ph_nom) VALUES  ('DOE');
-- INSERT INTO t_phase (ph_nom) VALUES  ('EXE');


-- CREATE VIEW v_dlg_fait as
-- select DISTINCT dl.dl_id,zo.zo_marche,zo.zo_nro,zo.zo_pm,zo.zo_refcode3,
--        zo.zo_marche || '_NRO' || zo.zo_nro || '_PM' || zo.zo_pm  || '_' || zo.zo_refcode3 AS zo_ext_id,
--        dl.dl_init_date,dl.dl_phase,dl.dl_td,dl.dl_no_livraison,dl.dl_no_version,
--        SUBSTR(dl.dl_phase, 1, 3) || '-DLG-' || zo.zo_marche || '-' || zo.zo_refcode2  || '-' || zo.zo_refcode3 || '-' || PRINTF('%02d', dl.dl_no_livraison) || '-V' || dl.dl_no_version AS dlg,
--                 ex.ex_et_id
-- from t_zone_dlg zo
-- INNER JOIN t_dlg dl
--     ON dl.dl_zo_id = zo.zo_id
-- INNER JOIN t_exports ex
--     ON ex.ex_dl_id = dl.dl_id
-- WHERE ex.ex_et_id IN(11, 12)
-- ORDER BY dl.dl_init_date, dl_no_livraison, dl_no_version;


SELECT *
FROM v_exports_en_cours
WHERE ex_dl_id = 2
AND ex_date = (SELECT MAX(ex_date) FROM v_exports_en_cours WHERE ex_dl_id = 2);

