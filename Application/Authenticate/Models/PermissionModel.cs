using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authenticate.Models
{
    public class PermissionModel
    {
        public int permission { get; set; }
        public int idaccount { get; set; }
        public bool unrestricted { get; set; }
    }
}
