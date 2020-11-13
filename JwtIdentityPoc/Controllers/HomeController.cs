using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using JwtIdentityPoc.Models;
using JwtIdentityPoc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtIdentityPoc.Controllers
{
    [ApiController]
    [Route("v1/account")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("teste")]
        public string teste()
        {
            return "funcionou";
        }

        [HttpPost]
        [Route("usuario")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<dynamic>> Usuario([FromBody]User model)
        {
            string _connString = "Server=(localdb)\\mssqllocaldb;Database=JwtIdentityPocContext-d1d9cd27-fffa-4963-a184-7dbf51773045;Trusted_Connection=True;MultipleActiveResultSets=true";
            try
            {
                using (SqlConnection connection = new SqlConnection(_connString))
                {

                    using (SqlCommand command = new SqlCommand())
                    {
                        connection.Open();
                        command.Connection = connection;
                        //command.Parameters.AddWithValue("@Username", );
                        //command.Parameters.AddWithValue("@Password",model.Password);
                        //command.Parameters.AddWithValue("@Role", model.Role);
                        command.CommandText = "INSERT INTO [dbo].[User] (Username, Password, Role) VALUES ('" + model.Username + "', '" + model.Password + "', '" + model.Role + "')";
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // Retorna os dados
            return new { message = "Usuario criado" };
        }


        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody]User model)
        {
            // Recupera o usuário
            var users = new List<User>();

                string _connString = "Server=(localdb)\\mssqllocaldb;Database=JwtIdentityPocContext-d1d9cd27-fffa-4963-a184-7dbf51773045;Trusted_Connection=True;MultipleActiveResultSets=true";
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connString))
                    {
                        using (SqlCommand command = new SqlCommand())
                        {

                            command.Connection = connection;
                            command.CommandText = "Select * From [dbo].[User] where Username = '"+model.Username+"' and Password = '"+model.Password+"' ";
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
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            // Verifica se o usuário existe
            if (users.Count<1)
                return NotFound(new { message = "Usuário ou senha inválidos" });

            // Gera o Token
            var token = TokenService.GenerateToken(users[0]);
            DateTime Created = DateTime.Now;
            // Oculta a senha
            users[0].Password = "";

            // Retorna os dados
            return new
            {
                Authenticated = true,
                User = users[0],
                AccessToken = token,
                Created = Created,
                Expiration = Created.AddMinutes(2),
                Message = "Token gerado com sucesso"
            };
        }

        [HttpGet]
        [Route("anonymous")]
        [AllowAnonymous]
        public string Anonymous() => "Anônimo";

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public string Authenticated() => String.Format("Autenticado - {0}", User.Identity.Name);

        [HttpGet]
        [Route("employee")]
        [Authorize(Roles = "employee,manager")]
        public string Employee() => "Funcionário";

        [HttpGet]
        [Route("manager")]
        [Authorize(Roles = "manager")]
        public string Manager() => "Gerente";
    }
}