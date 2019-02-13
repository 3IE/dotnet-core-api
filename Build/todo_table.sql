DROP TABLE IF EXISTS "public"."TodoItems";
-- This script only contains the table creation statements and does not fully represent the table in the database. It's still missing: indices, triggers. Do not use it as a backup.

-- Sequence and defined type
CREATE SEQUENCE IF NOT EXISTS "TodoItems_id_seq";

-- Table Definition
CREATE TABLE "public"."TodoItems" (
    "Id" int4 NOT NULL DEFAULT nextval('"TodoItems_id_seq"'::regclass),
    "Name" varchar NOT NULL,
    "IsComplete" bool NOT NULL DEFAULT false,
    "UserId" int4 NOT NULL,
    CONSTRAINT "TodoItems_UserId_fkey" FOREIGN KEY ("UserId") REFERENCES "public"."Users"("Id"),
    PRIMARY KEY ("Id")
);

INSERT INTO "public"."TodoItems" ("Id", "Name", "IsComplete", "UserId") VALUES ('2', 'Walk Cat', 't', '1'),
('3', 'get diner', 'f', '2'),
('5', 'get groceries', 't', '1'),
('6', 'get coffee with Dane', 'f', '1');
