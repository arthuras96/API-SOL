using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authenticate.Entities
{
    public class Permissions
    {
        //public static class Evaluation
        //{
        //    public const string Add = "evaluation.add";
        //    public const string Correct = "evaluation.correct";
        //    public const string Edit = "evaluation.edit";
        //}
        public enum PermissionEnum
        {
            evaluationcorrect = 1,
            evaluationrecord = 2
        }
    }
}
