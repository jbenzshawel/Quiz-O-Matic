CREATE OR REPLACE FUNCTION delete_question (
   p_question_id integer
) 
 RETURNS bigint
AS $$
   DECLARE affected_rows Integer DEFAULT 0;
BEGIN
   IF (p_question_id > 0 AND 
 	  (SELECT COUNT(*) from questions WHERE questions.question_id = p_question_id) > 0) THEN 
 	  DELETE FROM public."questions" WHERE questions.question_id = p_question_id;
   END IF; 
 
   GET DIAGNOSTICS affected_rows = ROW_COUNT;
   RETURN affected_rows;
END; $$ 
 
LANGUAGE 'plpgsql';