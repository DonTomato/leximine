CREATE TABLE "book" (
	"id"	INTEGER,
	"title"	TEXT NOT NULL,
	"author"	TEXT,
	"words_count"	INTEGER NOT NULL DEFAULT 0,
	"sentence_count"	INTEGER NOT NULL DEFAULT 0,
	"unique_words_count"	INTEGER NOT NULL DEFAULT 0,
	PRIMARY KEY("id" AUTOINCREMENT)
);

CREATE TABLE "sentence" (
	"id"	INTEGER,
	"book_id"	INTEGER NOT NULL,
	"sentence"	TEXT NOT NULL,
	"words_count"	INTEGER NOT NULL DEFAULT 0,
	"hash"	TEXT NOT NULL,
	FOREIGN KEY("book_id") REFERENCES "book"("id"),
	PRIMARY KEY("id" AUTOINCREMENT)
);

CREATE TABLE "word" (
	"word_id"	TEXT,
	"word_example"	TEXT,
	"total_count"	INTEGER NOT NULL DEFAULT 0,
	PRIMARY KEY("word_id")
);

CREATE TABLE "word_form" (
	"id"	TEXT,
	"word_id"	TEXT NOT NULL,
	FOREIGN KEY("word_id") REFERENCES "word"("word_id"),
	PRIMARY KEY("id")
);

CREATE TABLE "book_word" (
	"id"	INTEGER,
	"book_id"	INTEGER NOT NULL,
	"word_id"	TEXT NOT NULL,
	"sentence_id"	INTEGER NOT NULL,
	"count"	INTEGER NOT NULL DEFAULT 0,
	FOREIGN KEY("book_id") REFERENCES "book"("id"),
	FOREIGN KEY("word_id") REFERENCES "word"("word_id"),
	FOREIGN KEY("sentence_id") REFERENCES "sentence"("id"),
	PRIMARY KEY("id" AUTOINCREMENT)
);
