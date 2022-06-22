--...
-- t_dlg

TRUNCATE t_dlg RESTART IDENTITY CASCADE;

SELECT *
FROM t_dlg;

INSERT INTO t_dlg (dl_rc_id, dl_proj_us_id, dl_exe_us_id, dl_date_init, dl_ph_id, dl_te_id, dl_livraison, dl_version)
VALUES (1, 1, 2, CURRENT_DATE, 2, 3, 1, 1),
       (1, 1, 2, CURRENT_DATE, 2, 3, 1, 2),
       (1, 1, 2, CURRENT_DATE, 2, 2, 2, 1),
       (2, 1, 2, CURRENT_DATE, 3, 2, 2, 1),
       (3, 1, 2, CURRENT_DATE, 3, 3, 1, 1),
       (4, 1, 2, CURRENT_DATE, 3, 3, 1, 2),
       (4, 1, 2, CURRENT_DATE, 2, 2, 2, 1);

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

SELECT add_export_to_dlg(2, 5);

-- Trigger pour mettre dans la t_export
create or replace function tr_add_export_to_dlg() RETURNS trigger AS $BODY$
begin
    INSERT INTO "GeoTools"."t_exports" (ex_dl_id, ex_et_id)
    VALUES (NEW.dl_id, 1);
    RETURN NEW;
end;
$BODY$
    LANGUAGE plpgsql VOLATILE
                     COST 100;

create trigger tr_add_export_to_dlg after insert on "GeoTools"."t_dlg"
FOR EACH ROW EXECUTE PROCEDURE "GeoTools".tr_add_export_to_dlg();
