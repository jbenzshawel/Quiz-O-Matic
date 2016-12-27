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

 -- Table: public.quizes

-- DROP TABLE public.quizes;

CREATE TABLE public.quizes
(
    quiz_id uuid NOT NULL,
    name text COLLATE pg_catalog."default" NOT NULL,
    description text COLLATE pg_catalog."default",
    type_id integer,
    created timestamp with time zone DEFAULT now(),
    updated timestamp with time zone,
    CONSTRAINT "QuizIdPK" PRIMARY KEY (quiz_id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.quizes
    OWNER to "QuizGeneratorAdmin";

-- Table: public.quiz_type

-- DROP TABLE public.quiz_type;

CREATE TABLE public.quiz_type
(
    type text COLLATE pg_catalog."default" NOT NULL,
    type_id smallint NOT NULL DEFAULT nextval('"QuizType_Id_seq"'::regclass),
    CONSTRAINT "QuizTypePK" PRIMARY KEY (type_id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.quiz_type
    OWNER to "QuizGeneratorAdmin";

-- Table: public.questions

-- DROP TABLE public.questions;

CREATE TABLE public.questions
(
    question_id integer NOT NULL DEFAULT nextval('"Qustions_Id_seq"'::regclass),
    quiz_id uuid NOT NULL,
    title text COLLATE pg_catalog."default" NOT NULL,
    attributes text COLLATE pg_catalog."default",
    CONSTRAINT "Qustions_pkey" PRIMARY KEY (question_id),
    CONSTRAINT "QuestionsQuizFK" FOREIGN KEY (quiz_id)
        REFERENCES public.quizes (quiz_id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.questions
    OWNER to "QuizGeneratorAdmin";

-- Index: fki_QuestionsQuizFK

-- DROP INDEX public."fki_QuestionsQuizFK";

CREATE INDEX "fki_QuestionsQuizFK"
    ON public.questions USING btree
    (quiz_id)
    TABLESPACE pg_default;

-- Table: public.answers

-- DROP TABLE public.answers;

CREATE TABLE public.answers
(
    answer_id integer NOT NULL DEFAULT nextval('"Answers_Id_seq"'::regclass),
    question_id integer NOT NULL,
    content text COLLATE pg_catalog."default" NOT NULL,
    identifier text COLLATE pg_catalog."default",
    attributes text COLLATE pg_catalog."default",
    image bytea,
    CONSTRAINT "Answers_pkey" PRIMARY KEY (answer_id),
    CONSTRAINT "AnswerQuestionFK" FOREIGN KEY (question_id)
        REFERENCES public.questions (question_id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.answers
    OWNER to "QuizGeneratorAdmin";

-- Table: public.responses

-- DROP TABLE public.responses;

CREATE TABLE public.responses
(
    response_id bigint NOT NULL DEFAULT nextval('responses_response_id_seq'::regclass),
    quiz_id uuid NOT NULL,
    question_id integer NOT NULL,
    value text COLLATE pg_catalog."default" NOT NULL,
    created timestamp with time zone NOT NULL,
    CONSTRAINT responses_pkey PRIMARY KEY (response_id),
    -- CONSTRAINT fk_response_question_id FOREIGN KEY (response_id)
    --     REFERENCES public.questions (question_id) MATCH SIMPLE
    --     ON UPDATE CASCADE
    --     ON DELETE CASCADE,
    CONSTRAINT fk_response_quiz_id FOREIGN KEY (quiz_id)
        REFERENCES public.quizes (quiz_id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.responses
    OWNER to postgres;