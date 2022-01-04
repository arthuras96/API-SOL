using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Evaluation.Models
{
    public class EvaluationQuestionModel
    {
        public int idquestion { get; set; }
        public int idevaluationquestion { get; set; }
        public int idevaluationanswer { get; set; }
        public string dsquestion { get; set; }
        public int sequence { get; set; }
        public int totalrating { get; set; }
        public bool nullfied { get; set; }
        public string studentgrade { get; set; }
    }
}
