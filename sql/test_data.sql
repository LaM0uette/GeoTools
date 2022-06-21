--...
-- t_dlg

TRUNCATE t_dlg RESTART IDENTITY CASCADE;

SELECT *
FROM t_dlg;

INSERT INTO t_dlg (dl_rc_id, dl_proj_us_id, dl_exe_us_id, dl_date_init, dl_ph_id, dl_te_id, dl_livraison, dl_version)
VALUES (1, 'XD5965', 'XD5965', CURRENT_DATE, 2, 3, 1, 1),
       (1, 'XD5965', 'XD5965', CURRENT_DATE, 2, 3, 1, 2),
       (1, 'XD5965', 'XD5965', CURRENT_DATE, 2, 2, 2, 1),
       (2, 'XD5965', 'XD5965', CURRENT_DATE, 3, 2, 2, 1),
       (3, 'XD5965', 'XD5965', CURRENT_DATE, 3, 3, 1, 1),
       (4, 'XD5965', 'XD5965', CURRENT_DATE, 3, 3, 1, 2),
       (4, 'XD5965', 'XD5965', CURRENT_DATE, 2, 2, 2, 1);

-- Procédure

create or replace procedure add_export_to_dlg(dlg_id int, etat_id int)
as
$$
begin
    INSERT INTO t_exports (ex_dl_id, ex_et_id, ex_date)
    VALUES (dlg_id, etat_id, CURRENT_TIMESTAMP);
end;
$$;

-- Trigger pour mettre dans la t_export

create or replace trigger ai_t_dlg
    after insert
    on t_dlg
execute procedure add_export_to_dlg(dlg_id := :NEW.dl_id, etat_id := 0, proj_id := _, exe_id := NULL);
