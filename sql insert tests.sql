insert into TB_connectsoftware (dsconnectsoftware) values ('SOFT1')

INSERT INTO [TB_account]
           ([FK_idconnectsoftware]
           ,[dsaccount]
           ,[isexternal]
           ,[idexternal]
           ,[masterlogin]
           ,[masterpassword]
           ,[idaccountconnectsoftware])
     VALUES
           (1
           ,'Exemplo de Conta'
           ,1
           ,'101'
           ,'objpp'
           ,'batatinhafrita123'
           ,'101')
		   
		   
		   
INSERT INTO [dbo].[TB_user]
           ([FK_idconnectsoftware]
           ,[FK_idusertype]
           ,[email]
           ,[name]
           ,[userlogin]
           ,[userpassword]
           ,[iduserconnectsoftware]
           )
     VALUES
           (1
           ,3
           ,'arthur.silva@seudominio.com.br'
           ,'Arthur Alencar Silva'
           ,'arthur.silva'
           ,'teste'
           ,'197087'
           )
		   
INSERT INTO [dbo].[TB_user]
           ([FK_idconnectsoftware]
           ,[FK_idusertype]
           ,[email]
           ,[name]
           ,[userlogin]
           ,[userpassword]
           ,[iduserconnectsoftware]
           )
     VALUES
           (1
           ,5
           ,'teste.aluno@seudominio.com.br'
           ,'Teste Aluno 1'
           ,'teste.aluno'
           ,'testealuno'
           ,''
           )

INSERT INTO [dbo].[TB_user]
           ([FK_idconnectsoftware]
           ,[FK_idusertype]
           ,[email]
           ,[name]
           ,[userlogin]
           ,[userpassword]
           ,[iduserconnectsoftware]
           )
     VALUES
           (1
           ,5
           ,'teste.aluno2@seudominio.com.br'
           ,'Teste Aluno 12'
           ,'teste.aluno2'
           ,'testealuno'
           ,''
           )


insert into TB_useraccount (FK_iduser, FK_idaccount) values (1, 1);
insert into TB_userpermission (FK_iduser, FK_idpermission, FK_idaccount, unrestricted) values (1, 1, 1, 1);

select * from TB_permission;
select * from TB_user;
select * from TB_account;
select * from TB_useraccount;
select * from TB_userpermission;


INSERT INTO TB_evaluation
	(
		[FK_idusercreated]
	   ,[FK_idevaluationcategory]
	   ,[FK_idaccount]
	   ,[FK_idevaluationorigin]
	   ,[dsevaluation])
 VALUES
	(
		1
	   ,2
	   ,1
	   ,1
	   ,'Avaliação de Teste'
	);

insert into TB_question (FK_idquestiontype, dsquestion, nullified) values (2, '', 0);
insert into TB_question (FK_idquestiontype, dsquestion, nullified) values (2, '', 0);
insert into TB_question (FK_idquestiontype, dsquestion, nullified) values (2, '', 0);

insert into TB_evaluationquestion(FK_idquestion, FK_idevaluationversion, sequence, nullfied, totalrating) values (1, 1, 1, 0, 3)
insert into TB_evaluationquestion(FK_idquestion, FK_idevaluationversion, sequence, nullfied, totalrating) values (2, 1, 2, 0, 3)
insert into TB_evaluationquestion(FK_idquestion, FK_idevaluationversion, sequence, nullfied, totalrating) values (3, 1, 3, 0, 3)

insert into TB_evaluationstudent (FK_idevaluationversion, FK_iduser, FK_idevaluation) values (1,2,1)
insert into TB_evaluationstudent (FK_idevaluationversion, FK_iduser, FK_idevaluation) values (1,3,1)

insert into TB_evaluationdiscipline (FK_idevaluation, FK_iddiscipline) values (1, 2)
insert into TB_evaluationdiscipline (FK_idevaluation, FK_iddiscipline) values (1, 3)

insert into TB_group(FK_idconnectsoftware, FK_ideducationlevel, FK_idaccount, dsgroup, idgroupconnectsoftware, yeargroup, enabled)values(1, 3, 1, 'EM3A', 'EM3A', 2021, 1);
insert into TB_group(FK_idconnectsoftware, FK_ideducationlevel, FK_idaccount, dsgroup, idgroupconnectsoftware, yeargroup, enabled)values(1, 3, 1, 'EM3B', 'EM3B', 2021, 1);

insert into TB_groupevaluation (FK_idgroup, FK_idevaluation) values (1,1);
insert into TB_groupevaluation (FK_idgroup, FK_idevaluation) values (2,1);