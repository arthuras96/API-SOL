using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace Repository
{
    public class ContextTransaction
    {
        private SqlConnection connection;
        private string connString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public bool RunTransaction(List<string> Transactions)
        {
            connection = new SqlConnection(connString);

            //SqlCommand command = connection.CreateCommand();
            SqlTransaction transaction = null;

            try
            {
                // BeginTransaction() Requires Open Connection
                connection.Open();
                transaction = connection.BeginTransaction();

                foreach (string query in Transactions)
                {
                    SqlCommand command = new SqlCommand(query, connection, transaction);
                    command.ExecuteNonQuery();
                }

                transaction.Commit();
                connection.Close();
                return true;

            }
            catch (Exception)
            {
                transaction.Rollback();
                connection.Close();
                return false;
            }
            //finally
            //{
            //    connection.Close();
            //}
        }
    }
}
