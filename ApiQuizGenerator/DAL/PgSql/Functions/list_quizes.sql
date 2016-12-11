CREATE OR REPLACE FUNCTION list_quizes () 
 RETURNS TABLE (
 quiz_id UUID,
 name Text,
 description Text,
 type Text,
 type_id smallint
) 
AS $$
BEGIN
 RETURN QUERY SELECT 
 quizes.quiz_id, 
 quizes.name, 
 quizes.description, 
 quiz_type.type,
 quiz_type.type_id
FROM 
	public.quizes LEFT OUTER JOIN public.quiz_type ON quizes.type_id = quiz_type.type_id;
END; $$ 
 
LANGUAGE 'plpgsql';