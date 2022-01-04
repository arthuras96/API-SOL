using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Evaluation.Models
{
    public class EvaluationStudentModel
    {
        public int idstudent { get; set; }
        public int idevaluationversion { get; set; }
        public int idevaluationstudent { get; set; }
        public string dsevaluationversion { get; set; }
        public string namestudent { get; set; }
        public string studentphoto { get; set; }
        public bool sendemail { get; set; }
        public List<EvaluationImageModel> evaluations { get; set; }
        public List<EvaluationQuestionModel> questions { get; set; }
    }
}
