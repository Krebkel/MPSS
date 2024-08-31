DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'mpss') THEN
CREATE SCHEMA mpss;
END IF;
END $EF$;
CREATE TABLE IF NOT EXISTS mpss."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
    );

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'mpss') THEN
CREATE SCHEMA mpss;
END IF;
END $EF$;

CREATE TABLE mpss."Counteragents" (
                                      "Id" integer GENERATED BY DEFAULT AS IDENTITY,
                                      "Name" character varying(100) NOT NULL,
                                      "Contact" character varying(100) NOT NULL,
                                      "Phone" character varying(20) NOT NULL,
                                      "INN" character varying(10),
                                      "OGRN" character varying(15),
                                      "AccountNumber" character varying(20),
                                      "BIK" character varying(9),
                                      CONSTRAINT "PK_Counteragents" PRIMARY KEY ("Id")
);

CREATE TABLE mpss."Employees" (
                                  "Id" integer GENERATED BY DEFAULT AS IDENTITY,
                                  "Name" character varying(100) NOT NULL,
                                  "Phone" character varying(20) NOT NULL,
                                  "IsDriver" boolean NOT NULL,
                                  "Passport" character varying(10),
                                  "DateOfBirth" timestamp with time zone NOT NULL,
                                  "INN" character varying(12),
                                  "AccountNumber" character varying(20),
                                  "BIK" character varying(9),
                                  CONSTRAINT "PK_Employees" PRIMARY KEY ("Id")
);

CREATE TABLE mpss."Products" (
                                 "Id" integer GENERATED BY DEFAULT AS IDENTITY,
                                 "Name" character varying(100) NOT NULL,
                                 "Cost" numeric(18,2) NOT NULL,
                                 "Type" text NOT NULL,
                                 CONSTRAINT "PK_Products" PRIMARY KEY ("Id")
);

CREATE TABLE mpss."Projects" (
                                 "Id" integer GENERATED BY DEFAULT AS IDENTITY,
                                 "Name" text NOT NULL,
                                 "Address" text NOT NULL,
                                 "DeadlineDate" timestamp with time zone NOT NULL,
                                 "StartDate" timestamp with time zone NOT NULL,
                                 "CounteragentId" integer,
                                 "ResponsibleEmployeeId" integer NOT NULL,
                                 "ManagerShare" real NOT NULL,
                                 "Note" text,
                                 "ProjectStatus" text NOT NULL,
                                 CONSTRAINT "PK_Projects" PRIMARY KEY ("Id"),
                                 CONSTRAINT "FK_Projects_Counteragents_CounteragentId" FOREIGN KEY ("CounteragentId") REFERENCES mpss."Counteragents" ("Id"),
                                 CONSTRAINT "FK_Projects_Employees_ResponsibleEmployeeId" FOREIGN KEY ("ResponsibleEmployeeId") REFERENCES mpss."Employees" ("Id") ON DELETE CASCADE
);

CREATE TABLE mpss."ProductComponents" (
                                          "Id" integer GENERATED BY DEFAULT AS IDENTITY,
                                          "ProductId" integer NOT NULL,
                                          "Name" character varying(100) NOT NULL,
                                          "Quantity" integer,
                                          "Weight" double precision,
                                          CONSTRAINT "PK_ProductComponents" PRIMARY KEY ("Id"),
                                          CONSTRAINT "FK_ProductComponents_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES mpss."Products" ("Id") ON DELETE CASCADE
);

CREATE TABLE mpss."EmployeeShifts" (
                                       "Id" integer GENERATED BY DEFAULT AS IDENTITY,
                                       "ProjectId" integer NOT NULL,
                                       "EmployeeId" integer NOT NULL,
                                       "Date" timestamp with time zone NOT NULL,
                                       "Arrival" timestamp with time zone,
                                       "Departure" timestamp with time zone,
                                       "TravelTime" float,
                                       "ConsiderTravel" boolean NOT NULL,
                                       "ISN" integer,
                                       CONSTRAINT "PK_EmployeeShifts" PRIMARY KEY ("Id"),
                                       CONSTRAINT "FK_EmployeeShifts_Employees_EmployeeId" FOREIGN KEY ("EmployeeId") REFERENCES mpss."Employees" ("Id") ON DELETE CASCADE,
                                       CONSTRAINT "FK_EmployeeShifts_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES mpss."Projects" ("Id") ON DELETE CASCADE
);

CREATE TABLE mpss."Expenses" (
                                 "Id" integer GENERATED BY DEFAULT AS IDENTITY,
                                 "ProjectId" integer NOT NULL,
                                 "Name" text NOT NULL,
                                 "Amount" numeric(18,2),
                                 "Description" character varying(500),
                                 "Type" text NOT NULL,
                                 "IsPaidByCompany" boolean NOT NULL,
                                 CONSTRAINT "PK_Expenses" PRIMARY KEY ("Id"),
                                 CONSTRAINT "FK_Expenses_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES mpss."Projects" ("Id") ON DELETE CASCADE
);

CREATE TABLE mpss."ProjectProducts" (
                                        "Id" integer GENERATED BY DEFAULT AS IDENTITY,
                                        "ProjectId" integer NOT NULL,
                                        "ProductId" integer NOT NULL,
                                        "Quantity" integer NOT NULL,
                                        "Markup" numeric(18,2) NOT NULL,
                                        CONSTRAINT "PK_ProjectProducts" PRIMARY KEY ("Id"),
                                        CONSTRAINT "FK_ProjectProducts_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES mpss."Products" ("Id") ON DELETE CASCADE,
                                        CONSTRAINT "FK_ProjectProducts_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES mpss."Projects" ("Id") ON DELETE CASCADE
);

CREATE TABLE mpss."ProjectSuspensions" (
                                           "Id" integer GENERATED BY DEFAULT AS IDENTITY,
                                           "ProjectId" integer NOT NULL,
                                           "DateSuspended" timestamp with time zone NOT NULL,
                                           CONSTRAINT "PK_ProjectSuspensions" PRIMARY KEY ("Id"),
                                           CONSTRAINT "FK_ProjectSuspensions_Projects_ProjectId" FOREIGN KEY ("ProjectId") REFERENCES mpss."Projects" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_EmployeeShifts_EmployeeId" ON mpss."EmployeeShifts" ("EmployeeId");

CREATE INDEX "IX_EmployeeShifts_ProjectId" ON mpss."EmployeeShifts" ("ProjectId");

CREATE INDEX "IX_Expenses_ProjectId" ON mpss."Expenses" ("ProjectId");

CREATE INDEX "IX_ProductComponents_ProductId" ON mpss."ProductComponents" ("ProductId");

CREATE INDEX "IX_ProjectProducts_ProductId" ON mpss."ProjectProducts" ("ProductId");

CREATE INDEX "IX_ProjectProducts_ProjectId" ON mpss."ProjectProducts" ("ProjectId");

CREATE INDEX "IX_Projects_CounteragentId" ON mpss."Projects" ("CounteragentId");

CREATE INDEX "IX_Projects_ResponsibleEmployeeId" ON mpss."Projects" ("ResponsibleEmployeeId");

CREATE INDEX "IX_ProjectSuspensions_ProjectId" ON mpss."ProjectSuspensions" ("ProjectId");

INSERT INTO mpss."__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240831151943_Initial', '8.0.7');

COMMIT;
