using Application.Evaluation.Models;
using Application.General.Models;
using Functions;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Evaluation
{
    public class EvaluationService
    {
        private Context context;

        public List<EvaluationModel> GetEvaluations(int idaccount, int iduser, bool unrestricted)
        {
            List<EvaluationModel> evaluations = new List<EvaluationModel>();
            string stQuery = "", completeWhere = "", completeJoin = "";
            QueryUtils queryUtils = new QueryUtils();

            if (!unrestricted)
            {
                completeWhere   = " AND TB_evaluationuserauthorize.FK_iduser = " + queryUtils.InsertSingleQuotes(iduser.ToString());
                completeJoin    = " join TB_evaluationuserauthorize	on TB_evaluationuserauthorize.FK_idevaluation	= TB_evaluation.idevaluation";
            }

            stQuery = "select" +
                    "	 TB_evaluation.idevaluation" +
                    "	,TB_evaluation.dsevaluation" +
                    "	,STRING_AGG(TB_group.dsgroup, ', ') as 'groups'" +
                    "   ,(select count(*) from TB_evaluationstudent where FK_idevaluation = TB_evaluation.idevaluation) as totalcount" +
                    "   ,(select count(*) from TB_evaluationstudent where FK_idevaluation = TB_evaluation.idevaluation and israted = 1) as ratedcount" +
                    " from TB_evaluation" +
                    " join TB_groupevaluation		on TB_groupevaluation.FK_idevaluation		= TB_evaluation.idevaluation" +
                    " join TB_group					on TB_group.idgroup							= TB_groupevaluation.FK_idgroup" +
                    completeJoin +
                    " where" +
                    "		    TB_evaluation.FK_idaccount = " + idaccount +
                    completeWhere +
                    " group by " +
                    "	 TB_evaluation.idevaluation" +
                    "	,TB_evaluation.dsevaluation";

            context = new Context();
            try
            {
                var retDT = context.RunCommandDT(stQuery);
                evaluations = queryUtils.DataTableToList<EvaluationModel>(retDT);
            }
            catch(Exception e)
            {

            }
            context.Dispose();

            context = new Context();
            for(int index = 0; index < evaluations.Count; index++)
            {
                stQuery = "select" +
                        "	 TB_discipline.iddiscipline as value" +
                        "	,TB_discipline.dsdiscipline as label" +
                        " from TB_discipline" +
                        " join TB_evaluationdiscipline on TB_evaluationdiscipline.FK_iddiscipline = TB_discipline.iddiscipline" +
                        " where" +
                        "	TB_evaluationdiscipline.FK_idevaluation = " + evaluations[index].idevaluation;

                evaluations[index].disciplines = new List<string>();
                evaluations[index].disciplinesid = new List<General.Models.LabelValueModel>();

                try
                {
                    var retDT = context.RunCommandDT(stQuery);
                    foreach(DataRow dt in retDT.Rows)
                    {
                        evaluations[index].disciplines.Add(Convert.ToString(dt[1]));
                        evaluations[index].disciplinesid.Add(new General.Models.LabelValueModel()
                        {
                            label = Convert.ToString(dt[1]),
                            value = Convert.ToString(dt[0])
                        });
                    }
                }
                catch (Exception e)
                {

                }
                
            }
            context.Dispose();

            return evaluations;
        }

        public List<EvaluationStudentModel> GetEvaluationsStudent(int idevaluation, int selectedAccount, int iduser, bool unrestricted)
        {
            List<EvaluationStudentModel> evaluations = new List<EvaluationStudentModel>();
            string stQuery = "", completeWhere = "", completeJoin = "";
            QueryUtils queryUtils = new QueryUtils();

            if (!unrestricted)
            {
                //completeWhere = " AND TB_evaluationuserauthorize.FK_iduser = " + queryUtils.InsertSingleQuotes(iduser.ToString());
                //completeJoin = " join TB_evaluationuserauthorize	on TB_evaluationuserauthorize.FK_idevaluation	= TB_evaluation.idevaluation";
            }
            string path = AppDomain.CurrentDomain.BaseDirectory + @"F51E2EC95866455482975B7D52FD9\" + idevaluation;
            DirectoryInfo directory = new DirectoryInfo(path);
            FileInfo[] files = directory.GetFiles("*.*");
            
            stQuery = "SELECT" +
                    "	 TB_evaluationstudent.FK_iduser AS idstudent" +
                    "	,TB_evaluationversion.idevaluationversion" +
                    "	,TB_evaluationversion.dsevaluationversion" +
                    "	,TB_user.name AS namestudent" +
                    "   ,TB_evaluationstudent.idevaluationstudent" +
                    "	,'' AS studentphoto" +
                    "	,CONCAT (" +
                    "		'['" +
                    "		,STRING_AGG(CONCAT (" +
                    "				'{'" +
                    "				,'\"idquestion\":'" +
                    "				,TB_evaluationquestion.FK_idquestion" +
                    "				,','" +
                    "				,'\"idevaluationquestion\": '" +
                    "				,TB_evaluationquestion.idevaluationquestion" +
                    "				,','" +
                    "				,'\"idevaluationanswer\": '" +
                    "				,CASE " +
                    "					WHEN LEN(TB_evaluationanswer.idevaluationanswer) > 0 THEN TB_evaluationanswer.idevaluationanswer" +
                    "					else 0" +
                    "				 END" +
                    "				,','" +
                    "				,'\"dsquestion\": \"'" +
                    "				,TB_question.dsquestion" +
                    "				,'\",'" +
                    "				,'\"sequence\": '" +
                    "				,TB_evaluationquestion.sequence" +
                    "				,','" +
                    "				,'\"totalrating\": '" +
                    "				,TB_evaluationquestion.totalrating" +
                    "				,','" +
                    "				,'\"nullified\": '" +
                    "				,TB_question.nullified" +
                    "				,','" +
                    "				,'\"studentgrade\": '" +
                    "				,CASE " +
                    "					WHEN LEN(TB_evaluationanswer.rating) > 0 THEN CONCAT('\"', TB_evaluationanswer.rating, '\"')" +
                    "					else '\"\"'" +
                    "				 END" +
                    "				,'}'" +
                    "				), ',')" +
                    "		,']'" +
                    "		) AS jsonquestions" +
                    " FROM TB_evaluationstudent" +
                    " JOIN TB_evaluationversion ON TB_evaluationversion.idevaluationversion = TB_evaluationstudent.FK_idevaluationversion" +
                    " JOIN TB_user ON TB_user.iduser = TB_evaluationstudent.FK_iduser" +
                    " JOIN TB_evaluationquestion ON TB_evaluationquestion.FK_idevaluationversion = TB_evaluationversion.idevaluationversion" +
                    " JOIN TB_question ON TB_question.idquestion = TB_evaluationquestion.FK_idquestion" +
                    " LEFT JOIN TB_evaluationanswer ON" +
                    "		TB_evaluationanswer.FK_idevaluationquestion = TB_evaluationquestion.idevaluationquestion" +
                    "	AND TB_evaluationanswer.FK_idevaluationstudent = TB_evaluationstudent.idevaluationstudent" +
                    " WHERE " +
                    "	TB_evaluationstudent.FK_idevaluation = " + idevaluation +
                    " GROUP BY" +
                    "	 TB_evaluationstudent.FK_iduser" +
                    "	,TB_evaluationversion.idevaluationversion" +
                    "	,TB_evaluationversion.dsevaluationversion" +
                    "	,TB_user.name" +
                    "   ,TB_evaluationstudent.idevaluationstudent";

            context = new Context();
            try
            {
                var retDT = context.RunCommandDT(stQuery);
                //evaluations = queryUtils.DataTableToList<EvaluationStudentModel>(retDT);
                foreach(DataRow dr in retDT.Rows)
                {
                    List<EvaluationImageModel> filePathStudent = new List<EvaluationImageModel>();
                    foreach (FileInfo fileinfo in files)
                    {
                        string idstudentevaluation = fileinfo.FullName.Split('\\')[fileinfo.FullName.Split('\\').Length - 1].Split('.')[0];
                        int page = Convert.ToInt32(fileinfo.FullName.Split('\\')[fileinfo.FullName.Split('\\').Length - 1].Split('.')[1]);
                        string imageextension = fileinfo.FullName.Split('\\')[fileinfo.FullName.Split('\\').Length - 1].Split('.')[2];

                        if (idstudentevaluation == Convert.ToString(dr[0]))
                        {
                            Byte[] bytes = File.ReadAllBytes(fileinfo.FullName);
                            //filePathStudent.Add(new EvaluationImageModel { page = page, image = "data:image/jpg;base64," + Convert.ToBase64String(bytes) });
                            filePathStudent.Add(new EvaluationImageModel { page = page, image = "data:image/" + imageextension + ";base64," + Convert.ToBase64String(bytes) });
                        }
                    }

                    evaluations.Add(new EvaluationStudentModel
                    {
                        idstudent = Convert.ToInt32(dr[0]),
                        idevaluationversion = Convert.ToInt32(dr[1]),
                        dsevaluationversion = Convert.ToString(dr[2]),
                        namestudent = Convert.ToString(dr[3]),
                        idevaluationstudent = Convert.ToInt32(dr[4]),
                        studentphoto = Convert.ToString(dr[5]),
                        questions = JsonConvert.DeserializeObject<List<EvaluationQuestionModel>>(Convert.ToString(dr[6])),
                        sendemail = false,
                        evaluations = filePathStudent
                    });
                }
            }
            catch (Exception e)
            {

            }
            context.Dispose();

            return evaluations;
        }

        public GenericReturnModel PutStudentGrade(EvaluationStudentModel evaluation, int selectedAccount, int iduser, int idevaluation)
        {
            GenericReturnModel statusReturn = new GenericReturnModel();
            QueryUtils queryUtils = new QueryUtils();

            List<string> stQuerys = new List<string>();

            foreach (EvaluationQuestionModel question in evaluation.questions)
            {
                if (question.idevaluationanswer == 0)
                    stQuerys.Add(
                        "INSERT INTO TB_evaluationanswer" +
                        " (" +
                        "	 FK_idevaluationquestion" +
                        "   ,FK_idevaluationstudent" +
                        "   ,FK_iduserbroker" +
                        "   ,answer" +
                        "   ,rating" +
                        " )" +
                        " VALUES" +
                        " (" +
                        "    " + question.idevaluationquestion + //<FK_idevaluationquestion, bigint,>" +
                        "   ," + evaluation.idevaluationstudent +//<FK_idevaluationstudent, bigint,>" +
                        "   ," + iduser + //<FK_iduserbroker, bigint,>" +
                        "   ," + queryUtils.InsertSingleQuotes("") + //<answer, varchar(1000),>" +
                        "   ," + question.studentgrade + //<rating, float,>" +
                        " )"
                        );
                else
                    stQuerys.Add("update TB_evaluationanswer set rating = " + question.studentgrade + " where idevaluationanswer = " + question.idevaluationanswer);
            }

            ContextTransaction contextTransaction = new ContextTransaction();

            try
            {
                contextTransaction.RunTransaction(stQuerys);
                statusReturn.statuscode = 100;
            }
            catch (Exception e)
            {
                statusReturn.statuscode = 500;
            }

            if(statusReturn.statuscode == 100)
            {
                try 
                { 
                    string path = AppDomain.CurrentDomain.BaseDirectory + @"F51E2EC95866455482975B7D52FD9\" + idevaluation;
                    DirectoryInfo directory = new DirectoryInfo(path);
                    FileInfo[] files = directory.GetFiles("*.*");

                    foreach (FileInfo fileinfo in files)
                    {
                        string idstudentevaluation = fileinfo.FullName.Split('\\')[fileinfo.FullName.Split('\\').Length - 1].Split('.')[0];
                        if (idstudentevaluation == Convert.ToString(evaluation.idstudent))
                            File.Delete(fileinfo.FullName);
                    }

                    foreach(EvaluationImageModel evaluationImage in evaluation.evaluations)
                    {
                        string base64file = evaluationImage.image.Split(',')[1];
                        string fileextension = evaluationImage.image.Split(',')[0].Split('/')[1].Split(';')[0];
                        string filename = evaluation.idstudent + "." + evaluationImage.page + "." + fileextension;
                        File.WriteAllBytes(path + "\\" + filename, Convert.FromBase64String(base64file));
                    }

                    statusReturn.statuscode = 201;
                }
                catch(Exception e)
                {
                    statusReturn.statuscode = 500;
                }
            }

            return statusReturn;
        }
    }
}
