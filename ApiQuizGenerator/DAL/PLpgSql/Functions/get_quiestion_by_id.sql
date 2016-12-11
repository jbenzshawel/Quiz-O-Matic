CREATE OR REPLACE FUNCTION get_question_by_id (
	p_question_id integer
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
 question.attributes
FROM 
	public.questions
WHERE
	questions.question_id = p_question_id;
END; $$ 
 
LANGUAGE 'plpgsql';