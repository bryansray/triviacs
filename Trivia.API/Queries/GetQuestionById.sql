SELECT * 
FROM questions 
    INNER JOIN answers 
        ON questions.id = answers.question_id 
WHERE questions.id = @id