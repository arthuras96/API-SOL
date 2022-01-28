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
                    "       ,CASE" +
                    "           WHEN TB_evaluationemail.wassent = 0 THEN 1" +
                    "           ELSE 0" +
                    "       END as sentemail" +
                    " FROM TB_evaluationstudent" +
                    " JOIN TB_evaluationversion ON TB_evaluationversion.idevaluationversion = TB_evaluationstudent.FK_idevaluationversion" +
                    " JOIN TB_user ON TB_user.iduser = TB_evaluationstudent.FK_iduser" +
                    " JOIN TB_evaluationquestion ON TB_evaluationquestion.FK_idevaluationversion = TB_evaluationversion.idevaluationversion" +
                    " JOIN TB_question ON TB_question.idquestion = TB_evaluationquestion.FK_idquestion" +
                    " LEFT JOIN TB_evaluationanswer ON" +
                    "		TB_evaluationanswer.FK_idevaluationquestion = TB_evaluationquestion.idevaluationquestion" +
                    "	AND TB_evaluationanswer.FK_idevaluationstudent = TB_evaluationstudent.idevaluationstudent" +
                    " LEFT JOIN TB_evaluationemail ON TB_evaluationemail.FK_idevaluationstudent = TB_evaluationstudent.idevaluationstudent" +
                    " WHERE " +
                    "	TB_evaluationstudent.FK_idevaluation = " + idevaluation +
                    " GROUP BY" +
                    "	 TB_evaluationstudent.FK_iduser" +
                    "	,TB_evaluationversion.idevaluationversion" +
                    "	,TB_evaluationversion.dsevaluationversion" +
                    "	,TB_user.name" +
                    "   ,TB_evaluationstudent.idevaluationstudent" +
                    "   ,TB_evaluationemail.wassent";

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
                        sendemail = Convert.ToBoolean(dr[7]),
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

            stQuerys.Add("update TB_evaluationstudent set israted = 1 where idevaluationstudent = " + evaluation.idevaluationstudent);

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

            if(statusReturn.statuscode == 201 && evaluation.sendemail)
            {
                string stQuery = "select idevaluationemail from TB_evaluationemail where FK_idevaluationstudent = " + evaluation.idevaluationstudent + " and wassent = 0";
                int idevaluationemail = 0;

                context = new Context();
                try
                {
                    var retDT = context.RunCommandDT(stQuery);
                    if (retDT.Rows.Count > 0)
                        idevaluationemail = Convert.ToInt32(retDT.Rows[0][0]);
                }
                catch(Exception e)
                {
                    idevaluationemail = -1;
                    statusReturn.statuscode = 500;
                }
                context.Dispose();

                if(statusReturn.statuscode == 201 && idevaluationemail == 0)
                {
                    stQuery = "insert into TB_evaluationemail (FK_idevaluationstudent, FK_iduserbroker) values (" + evaluation.idevaluationstudent + ", " + iduser +")";
                    context = new Context();
                    try
                    {
                        context.RunCommand(stQuery);
                    }
                    catch (Exception e)
                    {
                        idevaluationemail = -1;
                        statusReturn.statuscode = 500;
                    }
                    context.Dispose();
                }
                else if (statusReturn.statuscode == 201 && idevaluationemail > 0)
                {
                    stQuery = "update TB_evaluationemail set FK_iduserbroker = " + iduser + ", registrationdate = getdate() where idevaluationemail = " + evaluation.idevaluationstudent;
                    context = new Context();
                    try
                    {
                        context.RunCommand(stQuery);
                    }
                    catch (Exception e)
                    {
                        idevaluationemail = -1;
                        statusReturn.statuscode = 500;
                    }
                    context.Dispose();
                }
            }

            return statusReturn;
        }

        public void SendEmails()
        {
            List<EvaluationEmailModel> evaluations = new List<EvaluationEmailModel>();
            QueryUtils queryUtils = new QueryUtils();

            string stQuery = "select" +
                            "	 TB_evaluationemail.idevaluationemail" +
                            "	,TB_evaluationstudent.FK_idevaluation as idevaluation" +
                            "	,TB_evaluation.dsevaluation" +
                            "	,userbroker.email as brokeremail" +
                            "	,userbroker.name as brokername" +
                            "	,TB_evaluationstudent.FK_iduser as idstudent" +
                            "	,student.email as studentemail" +
                            "	,student.name as studentname" +
                            " from TB_evaluationemail" +
                            " join TB_user as userbroker on TB_evaluationemail.FK_iduserbroker = userbroker.iduser" +
                            " join TB_evaluationstudent on TB_evaluationemail.FK_idevaluationstudent = TB_evaluationstudent.idevaluationstudent" +
                            " join TB_evaluation on TB_evaluationstudent.FK_idevaluation = TB_evaluation.idevaluation" +
                            " join TB_user as student on TB_evaluationstudent.FK_iduser = student.iduser" +
                            " where" +
                            "	TB_evaluationemail.wassent = 0" +
                            " order by " +
                            "    TB_evaluationemail.idevaluationemail" +
                            "   ,TB_evaluationstudent.FK_iduser";
            

            string path = AppDomain.CurrentDomain.BaseDirectory + @"F51E2EC95866455482975B7D52FD9\";

            context = new Context();
            try
            {
                var retDT = context.RunCommandDT(stQuery);
                if (retDT.Rows.Count > 0)
                    evaluations = queryUtils.DataTableToList<EvaluationEmailModel>(retDT);
            }
            catch (Exception e)
            {
                
            }
            context.Dispose();

            Utils utils = new Utils();
            ImagePDFConverter converterPDF = new ImagePDFConverter();

            string tempPath = path + utils.RandomAlphanumeric(20) + @"\";

            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);

            foreach(EvaluationEmailModel evaluation in evaluations)
            {
                DirectoryInfo directory = new DirectoryInfo(path + evaluation.idevaluation);
                FileInfo[] files = directory.GetFiles("*.*");

                string pdfTitle = tempPath + evaluation.dsevaluation + " - " + evaluation.studentname + " - " + evaluation.idstudent + ".pdf";

                List<string> filePathStudent = new List<string>();
                foreach (FileInfo fileinfo in files)
                {
                    string idstudentevaluation = fileinfo.FullName.Split('\\')[fileinfo.FullName.Split('\\').Length - 1].Split('.')[0];
                    
                    if (idstudentevaluation == Convert.ToString(evaluation.idstudent))
                        filePathStudent.Add(fileinfo.FullName);
                }

                string imgTitle = tempPath + evaluation.idstudent + ".jpg";

                if (filePathStudent.Count > 0 && SaveGradeImage(evaluation.idevaluation, evaluation.idstudent, imgTitle, evaluation.brokername))
                {
                    filePathStudent.Add(imgTitle);

                    if (converterPDF.ConvertImageToPDF(filePathStudent, pdfTitle, 1000))
                    {
                        List<string> stringsBody = new List<string>();
                        List<string> receivers = new List<string>();
                        
                        stringsBody.Add("Olá, " + evaluation.studentname.Split(' ')[0] + ",");
                        stringsBody.Add("Segue anexo a correção da sua avaliação.");
                        stringsBody.Add("Qualquer duvida, entrar em contrato com o professor(a) " + evaluation.brokername + " pelo e-mail " + evaluation.brokeremail + ".");

                        receivers.Add("arthur.silva@objetivobaixada.com.br");

                        var fileByte = System.IO.File.ReadAllBytes(pdfTitle);
                        string dataString = Convert.ToBase64String(fileByte);

                        string bodyEmail = utils.GetBodyMail(evaluation.dsevaluation + " - Correção", stringsBody, "", "", evaluation.brokername);
                        if(utils.SendMSGraphMail("0e9825e3-d081-4918-93f9-1402ebb7b947", receivers, evaluation.dsevaluation, bodyEmail, "pdf", pdfTitle.Split('\\')[pdfTitle.Split('\\').Length - 1], dataString, evaluation.studentemail, ""))
                        {
                            stQuery = "update TB_evaluationemail set wassent = 1 where idevaluationemail = " + evaluation.idevaluationemail;
                            context = new Context();
                            try
                            {
                                context.RunCommand(stQuery);
                            }
                            catch(Exception e)
                            {

                            }
                            context.Dispose();
                        }
                    }
                }

            }

            if (Directory.Exists(tempPath))
                Directory.Delete(tempPath, true);
        }

        public bool SaveGradeImage(int idevaluation, int idstudent, string path, string brokername)
        {
            string stQuery = "";
            stQuery = "SELECT" +
                    "	CONCAT (" +
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
                    "	    TB_evaluationstudent.FK_idevaluation = " + idevaluation +
                    "   AND TB_evaluationstudent.FK_iduser       = " + idstudent +
                    " GROUP BY" +
                    "	 TB_evaluationstudent.FK_iduser" +
                    "	,TB_evaluationversion.idevaluationversion" +
                    "	,TB_evaluationversion.dsevaluationversion" +
                    "	,TB_user.name" +
                    "   ,TB_evaluationstudent.idevaluationstudent";

            List<EvaluationQuestionModel> questions = new List<EvaluationQuestionModel>();

            context = new Context();
            try
            {
                var retDT = context.RunCommandDT(stQuery);
                if (retDT.Rows.Count > 0)
                    questions = JsonConvert.DeserializeObject<List<EvaluationQuestionModel>>(Convert.ToString(retDT.Rows[0][0]));
            }
            catch(Exception e)
            {
           
            }
            context.Dispose();

            if (questions.Count == 0)
                return false;

            double studentGrade = 0;
            double maxGrade = 0;
            string tableBody = "";

            foreach(EvaluationQuestionModel question in questions)
            {
                studentGrade += Convert.ToDouble(question.studentgrade);
                maxGrade += Convert.ToDouble(question.totalrating);
                tableBody += "  <tr>" +
                             "    <td class=\"tg-z7id\">" + question.sequence + "</td>" +
                             "    <td class=\"tg-z7id\">" + question.studentgrade + "</td>" +
                             "    <td class=\"tg-z7id\">" + question.totalrating + "</td>" +
                             "  </tr>";
            }

            string html = "<style type=\"text/css\">" +
                            "	.assinatura{display: flex;justify-content: end;} .tg  {border-collapse:collapse;border-spacing:0;margin:0px auto;}.tg td{border-color:black;border-style:solid;border-width:1px;font-family:Arial, sans-serif;font-size:14px;overflow:hidden;padding:10px 5px;word-break:normal;}.tg th{border-color:black;border-style:solid;border-width:1px;font-family:Arial, sans-serif;font-size:14px;font-weight:normal;overflow:hidden;padding:10px 5px;word-break:normal;}.tg .tg-e7lt{border-color:inherit;font-family:\"Times New Roman\", Times, serif !important;;font-size:14px;font-weight:bold;text-align:center;vertical-align:top}.tg .tg-234o{font-family:\"Times New Roman\", Times, serif !important;;font-size:14px;font-weight:bold;text-align:center;vertical-align:top}.tg .tg-z7id{font-family:\"Times New Roman\", Times, serif !important;;font-size:12px;text-align:center;vertical-align:top}" +
                            "</style>" +
                            "<table class=\"tg\">" +
                            "<thead>" +
                            "  <tr>" +
                            "    <th class=\"tg-e7lt\">Questão</th>" +
                            "    <th class=\"tg-e7lt\">Sua Pontuação</th>" +
                            "    <th class=\"tg-234o\">Pontuação Máxima</th>" +
                            "  </tr>" +
                            "</thead>" +
                            "<tbody>" +

                            tableBody +

                            "  <tr>" +
                            "    <th class=\"tg-e7lt\">TOTAL</th>" +
                            "    <th class=\"tg-e7lt\">" + studentGrade + "</th>" +
                            "    <th class=\"tg-234o\">" + maxGrade + "</th>" +
                            "  </tr>" +
                            "<td>Corrigido por:</td><td colspan=\"2\">" + brokername + "</td>" +
                            "</tbody>" +
                            "</table>";

            Utils utils = new Utils();
            return utils.CreateImageFromHtml(path, html, "jpeg");
        }
    }
}
