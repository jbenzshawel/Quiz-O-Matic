CREATE OR REPLACE FUNCTION list_quizes (
	p_only_visible BOOLEAN DEFAULT true
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
	IF (p_only_visible) THEN
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
			quizes.is_visible = p_only_visible;
	ELSE
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
			public.quizes LEFT OUTER JOIN public.quiz_type ON quizes.type_id = quiz_type.type_id;
	END IF;
END; $$ 
 
LANGUAGE 'plpgsql';