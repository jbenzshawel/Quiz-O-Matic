CREATE OR REPLACE FUNCTION save_response (
   p_quiz_id UUID,
   p_question_id Integer, 
   p_value Text, 
   p_user_id Text DEFAULT NULL,
   p_response_id Integer DEFAULT NULL
) 
 RETURNS bigint 
AS $$
   	DECLARE affected_rows Integer DEFAULT 0;
BEGIN

   IF (p_response_id IS NOT NULL AND 
 	(SELECT COUNT(*) from public.responses WHERE responses.response_id = p_response_id) > 0) THEN -- update row 
 	   UPDATE public."responses" 
          SET "quiz_id" = p_quiz_id, "question_id" = p_question_id, "value" = p_value, "user_id" = p_user_id
          WHERE responses.response_id = p_response_id;
   ELSE -- Insert new row 
 	   INSERT 
          INTO public."responses"("quiz_id", "question_id", "value", "user_id", "created")
    	  VALUES (p_quiz_id, p_question_id, p_value, p_user_id, current_timestamp);
   END IF; 
 
   GET DIAGNOSTICS affected_rows = ROW_COUNT;
   RETURN affected_rows;
END; $$ 
 
LANGUAGE 'plpgsql';