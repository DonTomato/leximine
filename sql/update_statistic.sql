UPDATE word
SET total_count = (
    SELECT SUM(book_word.count)
    FROM book_word
    WHERE book_word.word_id = word.word_id
);

UPDATE word_form
SET count = (
    SELECT SUM(book_word_form.count)
    FROM book_word_form
    WHERE book_word_form.word_form_id = word_form.id
);
