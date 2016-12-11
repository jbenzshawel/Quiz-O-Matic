CREATE OR REPLACE FUNCTION list_questions (
	p_quiz_id UUID
) 
 RETURNS TABLE (
 question_id integer,
 quiz_id UUID,
 title Text,
 attributes Text
) 
AS $$
BEGIN
 RETURN QUERY SELECT 
 questions.question_id,
 questions.quiz_id,
 questions.title,
 questions.attributes
FROM 
	public.questions
WHERE
	questions.quiz_id = p_quiz_id;
END; $$ 
 
LANGUAGE 'plpgsql';