CREATE OR REPLACE FUNCTION delete_quiz (
   p_quiz_id UUID
) 
 RETURNS void 
AS $$
BEGIN

 IF (p_quiz_id <> uuid_nil() AND 
 	(SELECT COUNT(*) from quizes WHERE quizes.quiz_id = p_quiz_id) > 0) THEN 
 	DELETE FROM public."quizes" WHERE quizes.quiz_id = p_quiz_id;
 END IF; 
 
 RETURN;
END; $$ 
 
LANGUAGE 'plpgsql';