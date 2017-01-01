CREATE OR REPLACE FUNCTION save_answer (
   p_question_id Integer, 
   p_content Text, 
   p_identifier Text
   p_attributes Text, 
   p_answer_id Integer DEFAULT NULL
) 
 RETURNS void 
AS $$
BEGIN

 IF (p_answer_id IS NOT NULL AND 
 	(SELECT COUNT(*) from public.answers WHERE answers.answer_id = p_answer_id) > 0) THEN -- update row 
 	UPDATE public."answers" 
    	SET "question_id" = p_question_id, "content" = p_content, "identifier" = p_identifier, "attributes" = p_attributes
        WHERE answers.answer_id = p_answer_id;
 ELSE -- Insert new row 
 	INSERT 
    	INTO public."answers"("question_id", "content", "identifier", "attributes")
    	VALUES (p_question_id, p_content, p_identifier, p_attributes);
 END IF; 
 
 RETURN;
END; $$ 
 
LANGUAGE 'plpgsql';