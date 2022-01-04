-- ------------------------------------------------------------
-- Origens possíveis:
-- 
-- SIS1, SIS2, SIS3
-- ------------------------------------------------------------

CREATE TABLE TB_evaluationorigin (
  idevaluationorigin BIGINT  NOT NULL   IDENTITY ,
  dsevaluationorigin VARCHAR(45)  NOT NULL    ,
PRIMARY KEY(idevaluationorigin));
GO


INSERT INTO TB_evaluationorigin (dsevaluationorigin) VALUES('SIS1');
INSERT INTO TB_evaluationorigin (dsevaluationorigin) VALUES('SIS2');
INSERT INTO TB_evaluationorigin (dsevaluationorigin) VALUES('SIS3');

GO




CREATE TABLE TB_temptoken (
  idtemptoken BIGINT  NOT NULL   IDENTITY ,
  accesstoken VARCHAR(6000)  NOT NULL  ,
  registrationdate DATETIME  NOT NULL DEFAULT GETDATE()   ,
PRIMARY KEY(idtemptoken));
GO




-- ------------------------------------------------------------
-- Categorias possíveis: Simulado, Avaliação Padrão.
-- ------------------------------------------------------------

CREATE TABLE TB_evaluationcategory (
  idevaluationcategory BIGINT  NOT NULL   IDENTITY ,
  dscategory VARCHAR(45)  NOT NULL    ,
PRIMARY KEY(idevaluationcategory));
GO


INSERT INTO TB_evaluationcategory (dscategory) VALUES ('Simulated')
INSERT INTO TB_evaluationcategory (dscategory) VALUES ('Standard Evaluation')

GO




CREATE TABLE TB_permission (
  idpermission BIGINT  NOT NULL   IDENTITY ,
  dspermission VARCHAR(40)  NOT NULL  ,
  enum INT  NOT NULL    ,
PRIMARY KEY(idpermission));
GO


insert into TB_permission (dspermission, enum) values ('evaluationcorrect', 1);
insert into TB_permission (dspermission, enum) values ('evaluationrecord', 2);

GO




-- ------------------------------------------------------------
-- Imagem, texto, formula matemática.
-- ------------------------------------------------------------

CREATE TABLE TB_questiondescriptiontype (
  idquestiondescriptiontype BIGINT  NOT NULL   IDENTITY ,
  dsquestiondescriptiontype VARCHAR(45)      ,
PRIMARY KEY(idquestiondescriptiontype));
GO


insert into TB_questiondescriptiontype (dsquestiondescriptiontype) values ('Image')
insert into TB_questiondescriptiontype (dsquestiondescriptiontype) values ('Text')
insert into TB_questiondescriptiontype (dsquestiondescriptiontype) values ('Mathematical formula')

GO




-- ------------------------------------------------------------
-- Dissertativa, Alternativa, Multipla Alternativa (mais de uma correta).
-- ------------------------------------------------------------

CREATE TABLE TB_questiontype (
  idquestiontype BIGINT  NOT NULL   IDENTITY ,
  dsquestiontype VARCHAR(45)      ,
PRIMARY KEY(idquestiontype));
GO


insert into TB_questiontype (dsquestiontype) values ('Essay');
insert into TB_questiontype (dsquestiontype) values ('Alternative');
insert into TB_questiontype (dsquestiontype) values ('Multiple Alternative');

GO




-- ------------------------------------------------------------
-- Aluno, Professor, Gestor.
-- ------------------------------------------------------------

CREATE TABLE TB_usertype (
  idusertype BIGINT  NOT NULL   IDENTITY ,
  dsusertype VARCHAR(20)  NOT NULL    ,
PRIMARY KEY(idusertype));
GO


insert into TB_usertype (dsusertype) values ('superadmin');
insert into TB_usertype (dsusertype) values ('admin');
insert into TB_usertype (dsusertype) values ('academic');
insert into TB_usertype (dsusertype) values ('broker');
insert into TB_usertype (dsusertype) values ('student');

GO




CREATE TABLE TB_discipline (
  iddiscipline BIGINT  NOT NULL   IDENTITY ,
  dsdiscipline VARCHAR(45)  NOT NULL    ,
PRIMARY KEY(iddiscipline));
GO


INSERT INTO TB_discipline (dsdiscipline) VALUES ('Arts')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Biology')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Chemistry')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Computing')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Domains Human Sciences')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('English')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Financial Education')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Geography')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Geopolitics')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('History')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Law and Citizenship')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Literature')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Math')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Music')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Natural Science Domains')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Nature and Culture')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Philosophy')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Physical Education')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Physics')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Portuguese')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Redaction')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Robotics')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Science')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Sociology')
INSERT INTO TB_discipline (dsdiscipline) VALUES ('Spanish')

GO




CREATE TABLE TB_connectsoftware (
  idconnectsoftware BIGINT  NOT NULL   IDENTITY ,
  dsconnectsoftware VARCHAR(45)      ,
PRIMARY KEY(idconnectsoftware));
GO




-- ------------------------------------------------------------
-- Aberto (TODOS), EM1A, EF1A
-- ------------------------------------------------------------

CREATE TABLE TB_educationlevel (
  ideducationlevel BIGINT  NOT NULL   IDENTITY ,
  dseducationlevel VARCHAR(45)  NOT NULL    ,
PRIMARY KEY(ideducationlevel));
GO


INSERT INTO TB_educationlevel (dseducationlevel) VALUES ('Elementary School I')
INSERT INTO TB_educationlevel (dseducationlevel) VALUES ('Elementary School II')
INSERT INTO TB_educationlevel (dseducationlevel) VALUES ('High School')
INSERT INTO TB_educationlevel (dseducationlevel) VALUES ('Course')

GO




CREATE TABLE TB_question (
  idquestion BIGINT  NOT NULL   IDENTITY ,
  FK_idquestiontype BIGINT  NOT NULL  ,
  dsquestion VARCHAR(255)  NOT NULL  ,
  nullified BIT  NOT NULL DEFAULT 0   ,
PRIMARY KEY(idquestion)  ,
  FOREIGN KEY(FK_idquestiontype)
    REFERENCES TB_questiontype(idquestiontype));
GO


CREATE INDEX TB_question_FKIndex1 ON TB_question (FK_idquestiontype);
GO


CREATE INDEX IFK_Rel_14 ON TB_question (FK_idquestiontype);
GO


CREATE TABLE TB_account (
  idaccount BIGINT  NOT NULL   IDENTITY ,
  FK_idconnectsoftware BIGINT    ,
  dsaccount VARCHAR(255)  NOT NULL  ,
  isexternal BIT  NOT NULL  ,
  idexternal VARCHAR(255)    ,
  masterlogin VARCHAR(45)  NOT NULL  ,
  masterpassword VARCHAR(45)  NOT NULL  ,
  idaccountconnectsoftware VARCHAR(20)      ,
PRIMARY KEY(idaccount)  ,
  FOREIGN KEY(FK_idconnectsoftware)
    REFERENCES TB_connectsoftware(idconnectsoftware));
GO


CREATE INDEX TB_account_FKIndex1 ON TB_account (FK_idconnectsoftware);
GO


CREATE INDEX IFK_Rel_36 ON TB_account (FK_idconnectsoftware);
GO


CREATE TABLE TB_studysuggestion (
  idstudysuggestion BIGINT  NOT NULL   IDENTITY ,
  FK_iddiscipline BIGINT  NOT NULL  ,
  dsstudysuggestion VARCHAR(1500)  NOT NULL  ,
  module VARCHAR(20)    ,
  notebook VARCHAR(20)      ,
PRIMARY KEY(idstudysuggestion)  ,
  FOREIGN KEY(FK_iddiscipline)
    REFERENCES TB_discipline(iddiscipline));
GO


CREATE INDEX TB_studysuggestion_FKIndex1 ON TB_studysuggestion (FK_iddiscipline);
GO


CREATE INDEX IFK_Rel_36 ON TB_studysuggestion (FK_iddiscipline);
GO


CREATE TABLE TB_questionoption (
  idquestionoption BIGINT  NOT NULL   IDENTITY ,
  FK_idquestion BIGINT  NOT NULL  ,
  dsquestionoption VARCHAR(255)  NOT NULL  ,
  letter CHAR(1)  NOT NULL  ,
  correctanswer BIT  NOT NULL    ,
PRIMARY KEY(idquestionoption)  ,
  FOREIGN KEY(FK_idquestion)
    REFERENCES TB_question(idquestion));
GO


CREATE INDEX TB_questionoption_FKIndex1 ON TB_questionoption (FK_idquestion);
GO


CREATE INDEX IFK_Rel_19 ON TB_questionoption (FK_idquestion);
GO


CREATE TABLE TB_user (
  iduser BIGINT  NOT NULL   IDENTITY ,
  FK_idconnectsoftware BIGINT    ,
  FK_idusertype BIGINT  NOT NULL  ,
  email VARCHAR(255)  NOT NULL  ,
  name VARCHAR(255)    ,
  userlogin VARCHAR(45)  NOT NULL  ,
  userpassword VARCHAR(255)  NOT NULL  ,
  iduserconnectsoftware VARCHAR(20)    ,
  enabled BIT  NOT NULL DEFAULT 1   ,
PRIMARY KEY(iduser)    ,
  FOREIGN KEY(FK_idusertype)
    REFERENCES TB_usertype(idusertype),
  FOREIGN KEY(FK_idconnectsoftware)
    REFERENCES TB_connectsoftware(idconnectsoftware));
GO


CREATE INDEX TB_user_FKIndex1 ON TB_user (FK_idusertype);
GO
CREATE INDEX TB_user_FKIndex2 ON TB_user (FK_idconnectsoftware);
GO


CREATE INDEX IFK_Rel_14 ON TB_user (FK_idusertype);
GO
CREATE INDEX IFK_Rel_38 ON TB_user (FK_idconnectsoftware);
GO


CREATE TABLE TB_questiondescription (
  idquestiondescription BIGINT  NOT NULL   IDENTITY ,
  FK_idquestiondescriptiontype BIGINT  NOT NULL  ,
  FK_idquestion BIGINT  NOT NULL  ,
  questiondescriptionbody VARCHAR(255)  NOT NULL  ,
  sequence INT  NOT NULL    ,
PRIMARY KEY(idquestiondescription)    ,
  FOREIGN KEY(FK_idquestion)
    REFERENCES TB_question(idquestion),
  FOREIGN KEY(FK_idquestiondescriptiontype)
    REFERENCES TB_questiondescriptiontype(idquestiondescriptiontype));
GO


CREATE INDEX TB_questiondescription_FKIndex1 ON TB_questiondescription (FK_idquestion);
GO
CREATE INDEX TB_questiondescription_FKIndex2 ON TB_questiondescription (FK_idquestiondescriptiontype);
GO


CREATE INDEX IFK_Rel_17 ON TB_questiondescription (FK_idquestion);
GO
CREATE INDEX IFK_Rel_18 ON TB_questiondescription (FK_idquestiondescriptiontype);
GO


CREATE TABLE TB_educationlevelstudysuggestion (
  FK_ideducationlevel BIGINT  NOT NULL  ,
  FK_idstudysuggestion BIGINT  NOT NULL    ,
PRIMARY KEY(FK_ideducationlevel, FK_idstudysuggestion)    ,
  FOREIGN KEY(FK_ideducationlevel)
    REFERENCES TB_educationlevel(ideducationlevel),
  FOREIGN KEY(FK_idstudysuggestion)
    REFERENCES TB_studysuggestion(idstudysuggestion));
GO


CREATE INDEX TB_educationlevel_has_TB_studysuggestion_FKIndex1 ON TB_educationlevelstudysuggestion (FK_ideducationlevel);
GO
CREATE INDEX TB_educationlevel_has_TB_studysuggestion_FKIndex2 ON TB_educationlevelstudysuggestion (FK_idstudysuggestion);
GO


CREATE INDEX IFK_Rel_35 ON TB_educationlevelstudysuggestion (FK_ideducationlevel);
GO
CREATE INDEX IFK_Rel_36 ON TB_educationlevelstudysuggestion (FK_idstudysuggestion);
GO


CREATE TABLE TB_disciplinequestion (
  FK_iddiscipline BIGINT  NOT NULL  ,
  FK_idquestion BIGINT  NOT NULL    ,
PRIMARY KEY(FK_iddiscipline, FK_idquestion)    ,
  FOREIGN KEY(FK_iddiscipline)
    REFERENCES TB_discipline(iddiscipline),
  FOREIGN KEY(FK_idquestion)
    REFERENCES TB_question(idquestion));
GO


CREATE INDEX TB_discipline_has_TB_question_FKIndex1 ON TB_disciplinequestion (FK_iddiscipline);
GO
CREATE INDEX TB_discipline_has_TB_question_FKIndex2 ON TB_disciplinequestion (FK_idquestion);
GO


CREATE INDEX IFK_Rel_15 ON TB_disciplinequestion (FK_iddiscipline);
GO
CREATE INDEX IFK_Rel_16 ON TB_disciplinequestion (FK_idquestion);
GO


CREATE TABLE TB_userpermission (
  FK_iduser BIGINT  NOT NULL  ,
  FK_idpermission BIGINT  NOT NULL  ,
  FK_idaccount BIGINT  NOT NULL  ,
  unrestricted BIT  NOT NULL    ,
PRIMARY KEY(FK_iduser, FK_idpermission)      ,
  FOREIGN KEY(FK_iduser)
    REFERENCES TB_user(iduser),
  FOREIGN KEY(FK_idpermission)
    REFERENCES TB_permission(idpermission),
  FOREIGN KEY(FK_idaccount)
    REFERENCES TB_account(idaccount));
GO


CREATE INDEX TB_user_has_TB_permission_FKIndex1 ON TB_userpermission (FK_iduser);
GO
CREATE INDEX TB_user_has_TB_permission_FKIndex2 ON TB_userpermission (FK_idpermission);
GO
CREATE INDEX TB_userpermission_FKIndex3 ON TB_userpermission (FK_idaccount);
GO


CREATE INDEX IFK_Rel_39 ON TB_userpermission (FK_iduser);
GO
CREATE INDEX IFK_Rel_40 ON TB_userpermission (FK_idpermission);
GO
CREATE INDEX IFK_Rel_41 ON TB_userpermission (FK_idaccount);
GO


-- ------------------------------------------------------------
-- Equivalente a turma.
-- ------------------------------------------------------------

CREATE TABLE TB_group (
  idgroup BIGINT  NOT NULL   IDENTITY ,
  FK_idconnectsoftware BIGINT  NOT NULL  ,
  FK_ideducationlevel BIGINT  NOT NULL  ,
  FK_idaccount BIGINT  NOT NULL  ,
  dsgroup VARCHAR(100)  NOT NULL  ,
  idgroupconnectsoftware VARCHAR(20)    ,
  yeargroup INT    ,
  enabled BIT  NOT NULL DEFAULT 1   ,
PRIMARY KEY(idgroup)      ,
  FOREIGN KEY(FK_idaccount)
    REFERENCES TB_account(idaccount),
  FOREIGN KEY(FK_ideducationlevel)
    REFERENCES TB_educationlevel(ideducationlevel),
  FOREIGN KEY(FK_idconnectsoftware)
    REFERENCES TB_connectsoftware(idconnectsoftware));
GO


CREATE INDEX TB_group_FKIndex1 ON TB_group (FK_idaccount);
GO
CREATE INDEX TB_group_FKIndex2 ON TB_group (FK_ideducationlevel);
GO
CREATE INDEX TB_group_FKIndex3 ON TB_group (FK_idconnectsoftware);
GO


CREATE INDEX IFK_Rel_21 ON TB_group (FK_idaccount);
GO
CREATE INDEX IFK_Rel_22 ON TB_group (FK_ideducationlevel);
GO
CREATE INDEX IFK_Rel_35 ON TB_group (FK_idconnectsoftware);
GO


CREATE TABLE TB_evaluation (
  idevaluation BIGINT  NOT NULL   IDENTITY ,
  FK_idusercreated BIGINT  NOT NULL  ,
  FK_idevaluationcategory BIGINT  NOT NULL  ,
  FK_idaccount BIGINT  NOT NULL  ,
  FK_idevaluationorigin BIGINT  NOT NULL  ,
  dsevaluation VARCHAR(45)  NOT NULL  ,
  registrationdate DATETIME  NOT NULL DEFAULT GETDATE()   ,
PRIMARY KEY(idevaluation)        ,
  FOREIGN KEY(FK_idevaluationorigin)
    REFERENCES TB_evaluationorigin(idevaluationorigin),
  FOREIGN KEY(FK_idaccount)
    REFERENCES TB_account(idaccount),
  FOREIGN KEY(FK_idevaluationcategory)
    REFERENCES TB_evaluationcategory(idevaluationcategory),
  FOREIGN KEY(FK_idusercreated)
    REFERENCES TB_user(iduser));
GO


CREATE INDEX TB_evaluation_FKIndex1 ON TB_evaluation (FK_idevaluationorigin);
GO
CREATE INDEX TB_evaluation_FKIndex2 ON TB_evaluation (FK_idaccount);
GO
CREATE INDEX TB_evaluation_FKIndex3 ON TB_evaluation (FK_idevaluationcategory);
GO
CREATE INDEX TB_evaluation_FKIndex4 ON TB_evaluation (FK_idusercreated);
GO


CREATE INDEX IFK_Rel_11 ON TB_evaluation (FK_idevaluationorigin);
GO
CREATE INDEX IFK_Rel_12 ON TB_evaluation (FK_idaccount);
GO
CREATE INDEX IFK_Rel_17 ON TB_evaluation (FK_idevaluationcategory);
GO
CREATE INDEX IFK_Rel_18 ON TB_evaluation (FK_idusercreated);
GO


CREATE TABLE TB_usersession (
  idusersession BIGINT  NOT NULL   IDENTITY ,
  FK_iduser BIGINT  NOT NULL  ,
  ip VARCHAR(20)    ,
  registrationdate DATETIME  NOT NULL DEFAULT GETDATE()   ,
PRIMARY KEY(idusersession)  ,
  FOREIGN KEY(FK_iduser)
    REFERENCES TB_user(iduser));
GO


CREATE INDEX TB_usersession_FKIndex1 ON TB_usersession (FK_iduser);
GO


CREATE INDEX IFK_Rel_38 ON TB_usersession (FK_iduser);
GO


CREATE TABLE TB_evaluationversion (
  idevaluationversion BIGINT  NOT NULL   IDENTITY ,
  FK_idevaluation BIGINT  NOT NULL  ,
  dsevaluationversion VARCHAR(20)  NOT NULL    ,
PRIMARY KEY(idevaluationversion)  ,
  FOREIGN KEY(FK_idevaluation)
    REFERENCES TB_evaluation(idevaluation));
GO


CREATE INDEX TB_evaluationversion_FKIndex1 ON TB_evaluationversion (FK_idevaluation);
GO


CREATE INDEX IFK_Rel_23 ON TB_evaluationversion (FK_idevaluation);
GO


CREATE TABLE TB_useraccount (
  FK_iduser BIGINT  NOT NULL  ,
  FK_idaccount BIGINT  NOT NULL    ,
PRIMARY KEY(FK_iduser, FK_idaccount)    ,
  FOREIGN KEY(FK_iduser)
    REFERENCES TB_user(iduser),
  FOREIGN KEY(FK_idaccount)
    REFERENCES TB_account(idaccount));
GO


CREATE INDEX TB_user_has_TB_account_FKIndex1 ON TB_useraccount (FK_iduser);
GO
CREATE INDEX TB_user_has_TB_account_FKIndex2 ON TB_useraccount (FK_idaccount);
GO


CREATE INDEX IFK_Rel_09 ON TB_useraccount (FK_iduser);
GO
CREATE INDEX IFK_Rel_10 ON TB_useraccount (FK_idaccount);
GO


CREATE TABLE TB_evaluationquestion (
  idevaluationquestion BIGINT  NOT NULL   IDENTITY ,
  FK_idquestion BIGINT  NOT NULL  ,
  FK_idevaluationversion BIGINT  NOT NULL  ,
  sequence INT  NOT NULL  ,
  nullfied BIT  NOT NULL DEFAULT 0 ,
  totalrating INT  NOT NULL    ,
PRIMARY KEY(idevaluationquestion)    ,
  FOREIGN KEY(FK_idevaluationversion)
    REFERENCES TB_evaluationversion(idevaluationversion),
  FOREIGN KEY(FK_idquestion)
    REFERENCES TB_question(idquestion));
GO


CREATE INDEX TB_evaluationquestion_FKIndex1 ON TB_evaluationquestion (FK_idevaluationversion);
GO
CREATE INDEX TB_evaluationquestion_FKIndex2 ON TB_evaluationquestion (FK_idquestion);
GO


CREATE INDEX IFK_Rel_28 ON TB_evaluationquestion (FK_idevaluationversion);
GO
CREATE INDEX IFK_Rel_29 ON TB_evaluationquestion (FK_idquestion);
GO


CREATE TABLE TB_evaluationdiscipline (
  FK_idevaluation BIGINT  NOT NULL  ,
  FK_iddiscipline BIGINT  NOT NULL    ,
PRIMARY KEY(FK_idevaluation, FK_iddiscipline)    ,
  FOREIGN KEY(FK_idevaluation)
    REFERENCES TB_evaluation(idevaluation),
  FOREIGN KEY(FK_iddiscipline)
    REFERENCES TB_discipline(iddiscipline));
GO


CREATE INDEX TB_evaluation_has_TB_discipline_FKIndex1 ON TB_evaluationdiscipline (FK_idevaluation);
GO
CREATE INDEX TB_evaluation_has_TB_discipline_FKIndex2 ON TB_evaluationdiscipline (FK_iddiscipline);
GO


CREATE INDEX IFK_Rel_15 ON TB_evaluationdiscipline (FK_idevaluation);
GO
CREATE INDEX IFK_Rel_16 ON TB_evaluationdiscipline (FK_iddiscipline);
GO


CREATE TABLE TB_groupevaluation (
  FK_idgroup BIGINT  NOT NULL  ,
  FK_idevaluation BIGINT  NOT NULL    ,
PRIMARY KEY(FK_idgroup, FK_idevaluation)    ,
  FOREIGN KEY(FK_idgroup)
    REFERENCES TB_group(idgroup),
  FOREIGN KEY(FK_idevaluation)
    REFERENCES TB_evaluation(idevaluation));
GO


CREATE INDEX TB_group_has_TB_evaluation_FKIndex1 ON TB_groupevaluation (FK_idgroup);
GO
CREATE INDEX TB_group_has_TB_evaluation_FKIndex2 ON TB_groupevaluation (FK_idevaluation);
GO


CREATE INDEX IFK_Rel_36 ON TB_groupevaluation (FK_idgroup);
GO
CREATE INDEX IFK_Rel_37 ON TB_groupevaluation (FK_idevaluation);
GO


CREATE TABLE TB_usergroup (
  FK_idgroup BIGINT  NOT NULL  ,
  FK_iduser BIGINT  NOT NULL    ,
PRIMARY KEY(FK_idgroup, FK_iduser)    ,
  FOREIGN KEY(FK_idgroup)
    REFERENCES TB_group(idgroup),
  FOREIGN KEY(FK_iduser)
    REFERENCES TB_user(iduser));
GO


CREATE INDEX TB_group_has_TB_user_FKIndex1 ON TB_usergroup (FK_idgroup);
GO
CREATE INDEX TB_group_has_TB_user_FKIndex2 ON TB_usergroup (FK_iduser);
GO


CREATE INDEX IFK_Rel_15 ON TB_usergroup (FK_idgroup);
GO
CREATE INDEX IFK_Rel_16 ON TB_usergroup (FK_iduser);
GO


CREATE TABLE TB_evaluationuserauthorize (
  FK_iduser BIGINT  NOT NULL  ,
  FK_idevaluation BIGINT  NOT NULL    ,
PRIMARY KEY(FK_iduser, FK_idevaluation)    ,
  FOREIGN KEY(FK_iduser)
    REFERENCES TB_user(iduser),
  FOREIGN KEY(FK_idevaluation)
    REFERENCES TB_evaluation(idevaluation));
GO


CREATE INDEX TB_user_has_TB_evaluation_FKIndex1 ON TB_evaluationuserauthorize (FK_iduser);
GO
CREATE INDEX TB_user_has_TB_evaluation_FKIndex2 ON TB_evaluationuserauthorize (FK_idevaluation);
GO


CREATE INDEX IFK_Rel_09 ON TB_evaluationuserauthorize (FK_iduser);
GO
CREATE INDEX IFK_Rel_10 ON TB_evaluationuserauthorize (FK_idevaluation);
GO


CREATE TABLE TB_evaluationstudent (
  idevaluationstudent BIGINT  NOT NULL   IDENTITY ,
  FK_idevaluationversion BIGINT  NOT NULL  ,
  FK_iduser BIGINT  NOT NULL  ,
  FK_idevaluation BIGINT  NOT NULL  ,
  israted BIT  NOT NULL DEFAULT 0   ,
PRIMARY KEY(idevaluationstudent)      ,
  FOREIGN KEY(FK_idevaluation)
    REFERENCES TB_evaluation(idevaluation),
  FOREIGN KEY(FK_iduser)
    REFERENCES TB_user(iduser),
  FOREIGN KEY(FK_idevaluationversion)
    REFERENCES TB_evaluationversion(idevaluationversion));
GO


CREATE INDEX TB_evaluationstudent_FKIndex1 ON TB_evaluationstudent (FK_idevaluation);
GO
CREATE INDEX TB_evaluationstudent_FKIndex2 ON TB_evaluationstudent (FK_iduser);
GO
CREATE INDEX TB_evaluationstudent_FKIndex3 ON TB_evaluationstudent (FK_idevaluationversion);
GO


CREATE INDEX IFK_Rel_31 ON TB_evaluationstudent (FK_idevaluation);
GO
CREATE INDEX IFK_Rel_32 ON TB_evaluationstudent (FK_iduser);
GO
CREATE INDEX IFK_Rel_35 ON TB_evaluationstudent (FK_idevaluationversion);
GO


CREATE TABLE TB_evaluationanswer (
  idevaluationanswer BIGINT  NOT NULL   IDENTITY ,
  FK_idevaluationstudent BIGINT  NOT NULL  ,
  FK_idevaluationquestion BIGINT  NOT NULL  ,
  FK_iduserbroker BIGINT  NOT NULL  ,
  answer VARCHAR(1000)  NOT NULL  ,
  rating FLOAT  NOT NULL  ,
  registrationdate DATETIME  NOT NULL DEFAULT GETDATE()   ,
PRIMARY KEY(idevaluationanswer)      ,
  FOREIGN KEY(FK_iduserbroker)
    REFERENCES TB_user(iduser),
  FOREIGN KEY(FK_idevaluationquestion)
    REFERENCES TB_evaluationquestion(idevaluationquestion),
  FOREIGN KEY(FK_idevaluationstudent)
    REFERENCES TB_evaluationstudent(idevaluationstudent));
GO


CREATE INDEX TB_evaluationanswer_FKIndex1 ON TB_evaluationanswer (FK_iduserbroker);
GO
CREATE INDEX TB_evaluationanswer_FKIndex2 ON TB_evaluationanswer (FK_idevaluationquestion);
GO
CREATE INDEX TB_evaluationanswer_FKIndex3 ON TB_evaluationanswer (FK_idevaluationstudent);
GO


CREATE INDEX IFK_Rel_26 ON TB_evaluationanswer (FK_iduserbroker);
GO
CREATE INDEX IFK_Rel_30 ON TB_evaluationanswer (FK_idevaluationquestion);
GO
CREATE INDEX IFK_Rel_42 ON TB_evaluationanswer (FK_idevaluationstudent);
GO


