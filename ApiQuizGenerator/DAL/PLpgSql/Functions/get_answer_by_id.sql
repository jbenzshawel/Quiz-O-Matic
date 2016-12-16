CREATE OR REPLACE FUNCTION get_answer_by_id (
	p_answer_id integer
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
	public.answers 
WHERE
	answer_id.answers = p_answer_id
END; $$ 
 
LANGUAGE 'plpgsql';