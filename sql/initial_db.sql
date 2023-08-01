CREATE TABLE "book" (
	"id"	INTEGER,
	"lang"	TEXT NOT NULL,
	"title"	TEXT NOT NULL,
	"author"	TEXT,
	"words_count"	INTEGER NOT NULL DEFAULT 0,
	"sentence_count"	INTEGER NOT NULL DEFAULT 0,
	"unique_words_count"	INTEGER NOT NULL DEFAULT 0,
	PRIMARY KEY("id" AUTOINCREMENT)
);

CREATE TABLE "en_sentence" (
	"id"	INTEGER,
	"book_id"	INTEGER NOT NULL,
	"sentence"	TEXT NOT NULL,
	"words_count"	INTEGER NOT NULL DEFAULT 0,
	"hash"	TEXT NOT NULL,
	PRIMARY KEY("id" AUTOINCREMENT),
	FOREIGN KEY("book_id") REFERENCES "book"("id")
);
