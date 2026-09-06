CREATE TABLE "person"(
    "id" BIGINT NOT NULL,
    "name" VARCHAR(255) NOT NULL,
    "nick_name" VARCHAR(255) NOT NULL,
    "birthday" DATE NOT NULL,
    "fr_identity" BIGINT NOT NULL,
    "native_language" BIGINT NOT NULL
);

ALTER TABLE "person" ADD PRIMARY KEY("id");


CREATE TABLE "identity"(
    "id" BIGINT NOT NULL,
    "email" VARCHAR(255) NOT NULL,
    "phone_number" VARCHAR(255) NOT NULL,
    "fr_nationality" BIGINT NOT NULL
);

ALTER TABLE "identity" ADD PRIMARY KEY("id");

CREATE TABLE "nationality"(
    "id" BIGINT NOT NULL,
    "name_country" VARCHAR(255) NOT NULL
);
ALTER TABLE "nationality" ADD PRIMARY KEY("id");

CREATE TABLE "languages"(
    "id" BIGINT NOT NULL,
    "language" VARCHAR(255) NOT NULL
);
ALTER TABLE "languages" ADD PRIMARY KEY("id");


CREATE TABLE "lavel_languge"(
    "id" BIGINT NOT NULL,
    "lavel_Name" VARCHAR(255) NOT NULL
);
ALTER TABLE "lavel_languge" ADD PRIMARY KEY("id");

CREATE TABLE "info_Target_languge"(
    "id" BIGINT NOT NULL,
    "FK_Languge" BIGINT NOT NULL,
    "FK_lavel_languge" BIGINT NOT NULL
);
ALTER TABLE "info_Target_languge" ADD PRIMARY KEY("id");


CREATE TABLE "student"(
    "id" BIGINT NOT NULL,
    "join_date" DATE NOT NULL,
    "FK_person" BIGINT NOT NULL,
    "FK_interest" BIGINT NOT NULL,
    "password" VARCHAR(255) NOT NULL,
    "login" VARCHAR(255) NOT NULL,
    "FK_info_Target_languge" BIGINT NOT NULL
);
ALTER TABLE "student" ADD PRIMARY KEY("id");


CREATE TABLE "adminer"(
    "id" BIGINT NOT NULL,
    "fr_person" BIGINT NOT NULL,
    "password" VARCHAR(255) NOT NULL,
    "permission" BIGINT NOT NULL
);
ALTER TABLE "adminer" ADD PRIMARY KEY("id");


CREATE TABLE "match_interactions"(
    "id" BIGINT NOT NULL,
    "fr_pair1" BIGINT NOT NULL,
    "fr_pair2" BIGINT NOT NULL,
    "date_of_meet" DATE NOT NULL
);
ALTER TABLE "match_interactions" ADD PRIMARY KEY("id");

CREATE TABLE "adminer_interactions"(
    "id" BIGINT NOT NULL,
    "fr_student" BIGINT NOT NULL,
    "fr_adminer" BIGINT NOT NULL,
     "date_of_meet" DATE NOT NULL
);
ALTER TABLE "adminer_interactions" ADD PRIMARY KEY("id");

CREATE TABLE "conversations"(
    "id" BIGINT NOT NULL,
    "fr_student1" BIGINT NOT NULL,
    "fr_student2" BIGINT NOT NULL
);
ALTER TABLE "conversations" ADD PRIMARY KEY("id");


CREATE TABLE "messages"(
    "id" BIGINT NOT NULL,
    "fr_sender" BIGINT NOT NULL,
    "fr_conversation" BIGINT NOT NULL,
    "message" TEXT NOT NULL
);
ALTER TABLE "messages" ADD PRIMARY KEY("id");


CREATE TABLE "ai_suggestion_subject"(
    "id" BIGINT NOT NULL,
    "fr_match_interactions" BIGINT NOT NULL,
    "suggested_subject" TEXT NOT NULL
);
ALTER TABLE "ai_suggestion_subject" ADD PRIMARY KEY("id");

-----------------------------
CREATE TABLE "interests"(
    "id" BIGINT NOT NULL,
    "interest_name" VARCHAR(255) NOT NULL,
    "value_to_extract" BIGINT NOT NULL
);
ALTER TABLE "interests" ADD PRIMARY KEY("id");

CREATE TABLE "friends"(
    "id" BIGINT NOT NULL,
    "fr_student1" BIGINT NOT NULL,
    "fr_student2" BIGINT NOT NULL
);
ALTER TABLE "friends" ADD PRIMARY KEY("id");

-- =========================================================
-- Foreign key constraints
-- =========================================================

ALTER TABLE "person"
    ADD CONSTRAINT "person_fr_identity_foreign" FOREIGN KEY("fr_identity") REFERENCES "identity"("id");
ALTER TABLE "person"
    ADD CONSTRAINT "person_native_language_foreign" FOREIGN KEY("native_language") REFERENCES "languages"("id");

ALTER TABLE "identity"
    ADD CONSTRAINT "identity_fr_nationality_foreign" FOREIGN KEY("fr_nationality") REFERENCES "nationality"("id");

ALTER TABLE "student"
    ADD CONSTRAINT "student_fk_person_foreign"
    FOREIGN KEY("FK_person") REFERENCES "person"("id");

ALTER TABLE "student"
    ADD CONSTRAINT "student_fk_info_target_languge_foreign"
    FOREIGN KEY("FK_info_Target_languge")
    REFERENCES "info_Target_languge"("id");

ALTER TABLE "student"
    ADD CONSTRAINT "FK_interest_foreign"
    FOREIGN KEY("FK_interest")
    REFERENCES "interests"("id");
    
ALTER TABLE "info_Target_languge"
    ADD CONSTRAINT "info_target_languge_fk_languge_foreign" FOREIGN KEY("FK_Languge") REFERENCES "languages"("id");
ALTER TABLE "info_Target_languge"
    ADD CONSTRAINT "info_target_languge_fk_lavel_languge_foreign" FOREIGN KEY("FK_lavel_languge") REFERENCES "lavel_languge"("id");

ALTER TABLE "adminer"
    ADD CONSTRAINT "adminer_fr_person_foreign" FOREIGN KEY("fr_person") REFERENCES "person"("id");

ALTER TABLE "match_interactions"
    ADD CONSTRAINT "match_interactions_fr_pair1_foreign" FOREIGN KEY("fr_pair1") REFERENCES "student"("id");
ALTER TABLE "match_interactions"
    ADD CONSTRAINT "match_interactions_fr_pair2_foreign" FOREIGN KEY("fr_pair2") REFERENCES "student"("id");

ALTER TABLE "adminer_interactions"
    ADD CONSTRAINT "adminer_interactions_fr_student_foreign" FOREIGN KEY("fr_student") REFERENCES "student"("id");
ALTER TABLE "adminer_interactions"
    ADD CONSTRAINT "adminer_interactions_fr_adminer_foreign" FOREIGN KEY("fr_adminer") REFERENCES "adminer"("id");

ALTER TABLE "conversations"
    ADD CONSTRAINT "conversations_fr_student1_foreign" FOREIGN KEY("fr_student1") REFERENCES "student"("id");
ALTER TABLE "conversations"
    ADD CONSTRAINT "conversations_fr_student2_foreign" FOREIGN KEY("fr_student2") REFERENCES "student"("id");

ALTER TABLE "messages"
    ADD CONSTRAINT "messages_fr_sender_foreign" FOREIGN KEY("fr_sender") REFERENCES "student"("id");
ALTER TABLE "messages"
    ADD CONSTRAINT "messages_fr_conversation_foreign" FOREIGN KEY("fr_conversation") REFERENCES "conversations"("id");

ALTER TABLE "ai_suggestion_subject"
    ADD CONSTRAINT "ai_suggestion_subject_fr_match_interactions_foreign" FOREIGN KEY("fr_match_interactions") REFERENCES "match_interactions"("id");

ALTER TABLE "friends"
    ADD CONSTRAINT "friends_fr_student1_foreign" FOREIGN KEY("fr_student1") REFERENCES "student"("id");
ALTER TABLE "friends"
    ADD CONSTRAINT "friends_fr_student2_foreign" FOREIGN KEY("fr_student2") REFERENCES "student"("id");