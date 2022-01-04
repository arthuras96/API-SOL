using Application.Authenticate.Entities;
using Application.Authenticate.Models;
using Application.General.Models;
using Functions;
using Newtonsoft.Json;
using Repository;
using System;
using System.Collections.Generic;


namespace Application.Authenticate
{
    public class AuthenticateService
    {
        private Context context;
        public UserModel AuthenticateUser(string userName, string password)
        {
            string stQuery = "";
            QueryUtils queryUtils = new QueryUtils();
            UserModel user = new UserModel();
            user.iduser = 0;

            stQuery = "select top 1" +
                    "	 TB_user.iduser" +
                    "	,TB_user.name" +
                    "	,TB_user.email" +
                    "	,TB_user.iduser" +
                    "	,TB_user.FK_idusertype           as idprofile" +
                    "   ,TB_usertype.dsusertype          as dsprofile" +
                    "	,TB_usersession.registrationdate as lastlogin" +
                    " from TB_user" +
                    " join TB_usersession on TB_usersession.FK_iduser = TB_user.iduser" +
                    " join TB_usertype    on TB_usertype.idusertype   = TB_user.FK_idusertype" +
                    " where" +
                    "	(" +
                    "          TB_user.userlogin = " + queryUtils.InsertSingleQuotes(userName) +
                    "       OR TB_user.email     = " + queryUtils.InsertSingleQuotes(userName) +
                    "   )" +
                    "   AND" +
                    "       TB_user.userpassword = " + queryUtils.InsertSingleQuotes(password) +
                    " order by " +
                    "	TB_usersession.registrationdate desc;";

            context = new Context();
            try
            {
                var retDT = context.RunCommandDT(stQuery);
                if (retDT.Rows.Count > 0)
                    user = queryUtils.DataTableToObject<UserModel>(retDT);
            }
            catch (Exception e)
            {
                user.iduser = -1;
            }
            context.Dispose();

            if (user.iduser <= 0)
                return user;

            stQuery = "select" +
                    "	 TB_permission.enum as permission" +
                    "	,TB_userpermission.FK_idaccount as idaccount" +
                    "	,unrestricted" +
                    " from TB_userpermission" +
                    " join TB_permission on TB_permission.idpermission = TB_userpermission.FK_idpermission" +
                    " where" +
                    "	TB_userpermission.FK_iduser = " + user.iduser + ";";

            context = new Context();
            try
            {
                var retDT = context.RunCommandDT(stQuery);
                if (retDT.Rows.Count > 0)
                    user.permissions = queryUtils.DataTableToList<PermissionModel>(retDT);
            }
            catch (Exception e)
            {
                
            }
            context.Dispose();

            stQuery = "select" +
                    "	 TB_account.dsaccount as label" +
                    "	,TB_account.idaccount as value" +
                    " from TB_useraccount" +
                    " join TB_account on TB_account.idaccount = TB_useraccount.FK_idaccount" +
                    " where" +
                    "	TB_useraccount.FK_iduser = " + user.iduser + ";";

            context = new Context();
            try
            {
                var retDT = context.RunCommandDT(stQuery);
                if (retDT.Rows.Count > 0)
                    user.idaccounts = queryUtils.DataTableToList<LabelValueModel>(retDT);
            }
            catch (Exception e)
            {

            }
            context.Dispose();

            return user;
        }
    }
}
