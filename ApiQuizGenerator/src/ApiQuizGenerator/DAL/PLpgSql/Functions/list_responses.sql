CREATE OR REPLACE FUNCTION list_responses (
    p_quiz_id UUID
) 
 RETURNS TABLE (
 response_id BIGINT,
 user_id Text,
 quiz_id UUID,
 question_id INT,
 value Text,
 created timestamp with time zone
) 
AS $$
BEGIN
 RETURN QUERY SELECT 
 responses.response_id,
 responses.user_id,
 responses.quiz_id,
 responses.question_id,
 responses.value,
 responses.created
FROM 
	public.responses
WHERE
	responses.quiz_id = p_quiz_id;
END; $$ 
 
LANGUAGE 'plpgsql';