using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.General.Models
{
    public class GenericReturnModel
    {
        public int statuscode { get; set; }
        public int id { get; set; }
        public string message { get; set; }
    }
}
