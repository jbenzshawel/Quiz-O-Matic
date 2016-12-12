CREATE OR REPLACE FUNCTION delete_question (
   p_question_id integer
) 
 RETURNS void 
AS $$
BEGIN

 IF (p_question_id > 0 AND 
 	(SELECT COUNT(*) from questions WHERE questions.question_id = p_question_id) > 0) THEN 
 	DELETE FROM public."questions" WHERE questions.question_id = p_question_id;
 END IF; 
 
 RETURN;
END; $$ 
 
LANGUAGE 'plpgsql';