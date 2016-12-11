/**
 * Generate Script for QuizGenerator DB and Tables
 * SqlType: PostgreSql
 */

-- Database: QuizGenerator

-- DROP DATABASE "QuizGenerator";

CREATE DATABASE "QuizGenerator"
    WITH 
    OWNER = "QuizGeneratorAdmin"
    ENCODING = 'UTF8'
    LC_COLLATE = 'C'
    LC_CTYPE = 'C'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

 -- Table: public."Quizes"

-- DROP TABLE public."Quizes";

CREATE TABLE public."Quizes"
(
    "Id" uuid NOT NULL,
    "Name" character(1) COLLATE pg_catalog."default" NOT NULL,
    "Description" text COLLATE pg_catalog."default",
    "Type" integer NOT NULL,
    CONSTRAINT "QuizIdPK" PRIMARY KEY ("Id"),
    CONSTRAINT "TypeUnique" UNIQUE ("Type"),
    CONSTRAINT "QuizQuizTypeFK" FOREIGN KEY ("Type")
        REFERENCES public."QuizType" ("Id") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."Quizes"
    OWNER to "QuizGeneratorAdmin";

-- Table: public."QuizType"

-- DROP TABLE public."QuizType";

CREATE TABLE public."QuizType"
(
    "Type" character(500) COLLATE pg_catalog."default" NOT NULL,
    "Id" smallint NOT NULL DEFAULT nextval('"QuizType_Id_seq"'::regclass),
    CONSTRAINT "QuizTypePK" PRIMARY KEY ("Id")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."QuizType"
    OWNER to "QuizGeneratorAdmin";

-- Table: public."Qustions"

-- DROP TABLE public."Qustions";

CREATE TABLE public."Qustions"
(
    "Id" integer NOT NULL DEFAULT nextval('"Qustions_Id_seq"'::regclass),
    "QuizId" uuid NOT NULL,
    "Title" text COLLATE pg_catalog."default" NOT NULL,
    "Attributes" text[] COLLATE pg_catalog."default",
    CONSTRAINT "Qustions_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "QuestionsQuizFK" FOREIGN KEY ("QuizId")
        REFERENCES public."Quizes" ("Id") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."Qustions"
    OWNER to "QuizGeneratorAdmin";

-- Index: fki_QuestionsQuizFK

-- DROP INDEX public."fki_QuestionsQuizFK";

CREATE INDEX "fki_QuestionsQuizFK"
    ON public."Qustions" USING btree
    (QuizId)
    TABLESPACE pg_default;
-- Table: public."Answers"

-- DROP TABLE public."Answers";

CREATE TABLE public."Answers"
(
    "Id" bigint NOT NULL DEFAULT nextval('"Answers_Id_seq"'::regclass),
    "QuestionId" bigint NOT NULL,
    "Content" text COLLATE pg_catalog."default" NOT NULL,
    "Identifier" character(50) COLLATE pg_catalog."default",
    "Attributes" text[] COLLATE pg_catalog."default",
    CONSTRAINT "Answers_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "AnswerQuestionFK" FOREIGN KEY ("QuestionId")
        REFERENCES public."Qustions" ("Id") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."Answers"
    OWNER to "QuizGeneratorAdmin";