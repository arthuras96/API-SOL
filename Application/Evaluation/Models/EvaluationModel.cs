using Application.General.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Evaluation.Models
{
    public class EvaluationModel
    {
        public int idevaluation { get; set; }
        public string dsevaluation { get; set; }
        public string groups { get; set; }
        public int totalcount { get; set; }
        public int ratedcount { get; set; }
        public List<string> disciplines { get; set; }
        public List<LabelValueModel> disciplinesid { get; set; }
    }
}
