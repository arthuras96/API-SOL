using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Evaluation.Models
{
    public class EvaluationEmailModel
    {
        public int idevaluationemail { get; set; }
        public int idevaluation { get; set; }
        public string dsevaluation { get; set; }
        public string brokeremail { get; set; }
        public string brokername { get; set; }
        public int idstudent { get; set; }
        public string studentemail { get; set; }
        public string studentname { get; set; }
    }
}
