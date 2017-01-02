CREATE OR REPLACE FUNCTION delete_answer (
   p_answer_id integer
) 
 RETURNS bigint 
AS $$
   DECLARE affected_rows Integer DEFAULT 0;
BEGIN

   IF (p_answer_id > 0 AND 
 	  (SELECT COUNT(*) from public."answers" WHERE answers.answer_id = p_answer_id) > 0) THEN 
 	  DELETE FROM public."answers" WHERE answers.answer_id = p_answer_id;
   END IF; 

   GET DIAGNOSTICS affected_rows = ROW_COUNT;
   RETURN affected_rows;
END; $$ 
 
LANGUAGE 'plpgsql';