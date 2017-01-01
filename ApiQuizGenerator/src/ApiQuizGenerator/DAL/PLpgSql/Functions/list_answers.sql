CREATE OR REPLACE FUNCTION list_answers (
	p_quiz_id UUID
) 
 RETURNS TABLE (
 "answer_id" Integer, 
 "question_id" Integer,
 "content" Text,
 "identifier" Text,
 "attributes" Text
) 
AS $$
BEGIN
 RETURN QUERY SELECT 
 answers.answer_id, 
 answers.question_id, 
 answers.content,
 answers.identifier,
 answers.attributes
FROM 
	public.answers INNER JOIN public.questions ON answer.question_id = questions.question_id
WHERE
	question.quiz_id = p_quiz_id;
END; $$ 
 
LANGUAGE 'plpgsql';