CREATE OR REPLACE FUNCTION delete_answer (
   p_answer_id integer
) 
 RETURNS void 
AS $$
BEGIN

 IF (p_answer_id > 0 AND 
 	(SELECT COUNT(*) from public."answers" WHERE answers.answer_id = p_answer_id) > 0) THEN 
 	DELETE FROM public."answers" WHERE answers.answer_id = p_answer_id;
 END IF; 
 
 RETURN;
END; $$ 
 
LANGUAGE 'plpgsql';