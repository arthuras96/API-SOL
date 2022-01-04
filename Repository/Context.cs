using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Repository
{
    public class Context : IDisposable
    {
        private readonly SqlConnection myConnection;
        public Context()
        {
            myConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
            myConnection.Open();
        }

        // select
        public DataTable RunCommandDT(string strQuery)
        {
            var adapter = new SqlDataAdapter(strQuery, myConnection);
            adapter.SelectCommand.CommandTimeout = 0;
            DataSet ds = new DataSet();
            adapter.Fill(ds);

            return ds.Tables[0];
        }

        // insert, update or delete with return
        public SqlDataReader RunCommandRetID(string strQuery)
        {
            var comando = new SqlCommand(strQuery, myConnection);
            return comando.ExecuteReader();
        }

        // insert, update or delete
        public void RunCommand(string strQuery)
        {
            var comando = new SqlCommand(strQuery, myConnection);
            comando.ExecuteNonQuery();
        }

        public void Dispose()
        {
            if (myConnection.State == ConnectionState.Open)
            {
                myConnection.Dispose();
                myConnection.Close();
            }
        }
    }
}
