using JwtIdentityPoc.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JwtIdentityPoc.Repositories
{
    public static class UserRepository
    {
        public static User Get(string username, string password)
        {
            var users = new List<User>();

            string _connString = "Server=(localdb)\\mssqllocaldb;Database=JwtIdentityPocContext-d1d9cd27-fffa-4963-a184-7dbf51773045;Trusted_Connection=True;MultipleActiveResultSets=true";
            try
            {
                using (SqlConnection connection = new SqlConnection(_connString))
                {                    
                    using (SqlCommand command = new SqlCommand())
                    {

                        command.Connection = connection;
                        command.CommandText = "Select * From [dbo].[User]";
                        connection.Open();
                        using (var READER = command.ExecuteReader())
                        {
                            if (READER.HasRows)
                            {
                                while (READER.Read())
                                {
                                    users.Add(new User { Id = 1, Username = READER["Username"].ToString(), Password = READER["Password"].ToString(), Role = READER["Role"].ToString() });
                                }
                            }
                        }
                        connection.Close();
                        return users.Where(x => x.Username.ToLower() == username.ToLower() && x.Password == x.Password).FirstOrDefault();

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            //users.Add(new User { Id = 1, Username = "batman", Password = "batman", Role = "manager" });
            //users.Add(new User { Id = 2, Username = "robin", Password = "robin", Role = "employee" });
            
        }
    }
}
