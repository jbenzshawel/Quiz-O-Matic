CREATE OR REPLACE FUNCTION get_quiz_by_id (
    p_quiz_id UUID
) 
 RETURNS TABLE (
 quiz_id UUID,
 name Text,
 description Text,
 type Text,
 type_id smallint,
 created timestamp with time zone,
 updated timestamp with time zone,
 attributes JSON
) 
AS $$
BEGIN
 RETURN QUERY SELECT 
 quizes.quiz_id, 
 quizes.name, 
 quizes.description, 
 quiz_type.type,
 quiz_type.type_id,
 quizes.created,
 quizes.updated,
 quizes.attributes
FROM 
	public.quizes LEFT OUTER JOIN public.quiz_type ON quizes.type_id = quiz_type.type_id
WHERE
	quizes.quiz_id = p_quiz_id;
END; $$ 
 
LANGUAGE 'plpgsql';