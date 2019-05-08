DROP TABLE IF EXISTS "Questions" CASCADE;
DROP TABLE IF EXISTS "Answers" CASCADE;
DROP TABLE IF EXISTS "Categories" CASCADE;
DROP TABLE IF EXISTS "QuestionCategory" CASCADE;

DROP ROLE IF EXISTS trivia;
create user trivia with encrypted password 'control*88';
grant all privileges on database trivia to trivia;

CREATE TABLE "Questions" (
	"Id" serial NOT NULL PRIMARY KEY,
	"Text" text NOT NULL
);

CREATE TABLE "Answers" (
	"Id" serial NOT NULL PRIMARY KEY,
	"QuestionId" int NOT NULL REFERENCES "Questions"("Id"),	"Text" text NOT NULL,
	"Count" int NOT NULL
);

CREATE TABLE "Categories" (
	"Id" serial NOT NULL PRIMARY KEY,
	"Name" varchar(100) NOT NULL
);

CREATE TABLE "QuestionCategory" (
	"QuestionId" int REFERENCES "Questions"("Id"),
	"CategoryId" int REFERENCES "Categories"("Id")
);