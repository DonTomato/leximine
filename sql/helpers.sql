-- Find sentences for some word

SELECT sentence, s.words_count FROM word_sentence ws
	INNER JOIN sentence s ON ws.sentence_id = s.id
WHERE ws.word_id = 'alpash'
ORDER BY s.words_count ASC