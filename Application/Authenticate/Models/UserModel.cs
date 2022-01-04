using Application.General.Models;
using System;
using System.Collections.Generic;

namespace Application.Authenticate.Models
{
    public class UserModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public List<PermissionModel> permissions { get; set; }
        public DateTime lastlogin { get; set; }
        public int iduser { get; set; }
        public int idprofile { get; set; }
        public string dsprofile { get; set; }
        public List<LabelValueModel> idaccounts { get; set; }
    }
}
