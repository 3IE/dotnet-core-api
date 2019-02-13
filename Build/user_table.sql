DROP TABLE IF EXISTS "public"."Users";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Sequence and defined type
CREATE SEQUENCE IF NOT EXISTS "User_id_seq";

-- Table Definition
CREATE TABLE "public"."Users" (
    "Id" int4 NOT NULL DEFAULT nextval('"User_id_seq"'::regclass),
    "Name" varchar NOT NULL,
    PRIMARY KEY ("Id")
);

INSERT INTO "public"."Users" ("Id", "Name") VALUES ('1', 'John Doe'),
('2', 'Mike Smith'),
('3', 'Cedric James');
