CREATE OR REPLACE FUNCTION save_quiz (
   p_name Text, 
   p_description Text, 
   p_type_id integer, 
   p_quiz_id UUID DEFAULT uuid_nil()
) 
 RETURNS void 
AS $$
BEGIN

 IF (p_quiz_id <> uuid_nil() AND 
 	(SELECT COUNT(*) from quizes WHERE quizes.quiz_id = p_quiz_id) > 0) THEN -- update row 
 	UPDATE public."quizes" 
    	SET "name" = p_name, "description" = p_description, "type_id" = p_type_id, "updated" = current_timestamp
        WHERE quizes.quiz_id = p_quiz_id;
 ELSE -- Insert new row 
 	INSERT 
    	INTO public."quizes"("quiz_id", "name", "description", "type_id")
    	VALUES (uuid_generate_v4(), p_name, p_description, p_type_id);
 END IF; 
 
 RETURN;
END; $$ 
 
LANGUAGE 'plpgsql';