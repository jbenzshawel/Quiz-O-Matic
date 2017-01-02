CREATE OR REPLACE FUNCTION save_question (
   p_title Text, 
   p_attributes Text, 
   p_quiz_id UUID, 
   p_question_id Integer DEFAULT NULL
) 
 RETURNS BigInt 
AS $$
	DECLARE affected_rows Integer DEFAULT 0;
BEGIN

 IF (p_question_id IS NOT NULL AND p_question_id > 0 AND 
 	(SELECT COUNT(*) from questions WHERE questions.question_id = p_question_id) > 0) THEN 
 	UPDATE public."questions" 
    	SET "title" = p_title, "attributes" = p_attributes, "quiz_id" = p_quiz_id
        WHERE questions.question_id = p_question_id;
 ELSE -- Insert new row 
 	INSERT 
    	INTO public."questions"("title", "attributes", "quiz_id")
    	VALUES (p_title, p_attributes, p_quiz_id);
 END IF; 
 
 GET DIAGNOSTICS affected_rows = ROW_COUNT;
 RETURN affected_rows;
END; $$ 
 
LANGUAGE 'plpgsql';