CREATE OR REPLACE FUNCTION delete_quiz (
   p_quiz_id UUID
) 
 RETURNS bigint 
AS $$
   DECLARE affected_rows Integer DEFAULT 0;
BEGIN

   IF (p_quiz_id <> uuid_nil() AND 
 	  (SELECT COUNT(*) from quizes WHERE quizes.quiz_id = p_quiz_id) > 0) THEN 
     	DELETE FROM public."quizes" WHERE quizes.quiz_id = p_quiz_id;
   END IF; 
 
   GET DIAGNOSTICS affected_rows = ROW_COUNT;
   RETURN affected_rows;
END; $$ 
 
LANGUAGE 'plpgsql';