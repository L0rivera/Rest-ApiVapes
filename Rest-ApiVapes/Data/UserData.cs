using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using Rest_ApiVapes.Models;
using BCrypt.Net;
using BCrypt;

namespace Rest_ApiVapes.Data
{
    public class UserData
    {
        private readonly string _connectionString;

        public UserData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SQLConnection");
        }

        //Se crea un metodo de tipo lista con el modelo User como parametro generico 
        public List<User> GetUsers()
        {
            //Se crea una lista
            var oList = new List<User>();

            //Usando la anterior conexion creamos una nueva conexion con sql llamando al metodo que devuelve el string de conexion
            using (SqlConnection Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();
                //Llamamos al stored procedure
                SqlCommand cmd = new SqlCommand("Get_Users1", Connection);
                //Declaramos que es un comando de tipo stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                //Ejecutamos el SP y leemos el resultado
                using(var dr = cmd.ExecuteReader())
                {
                    //Mientras se lean resultados se crea un nuevo objeto de tipo User y se agrega a la lista
                    while (dr.Read())
                    {
                        oList.Add(new User()
                        {
                            UserId = Convert.ToInt32(dr["UserId"]),
                            name = dr["name"].ToString(),
                            lastname = dr["lastname"].ToString(),
                            email = dr["email"].ToString(),
                            password = dr["password"].ToString(),
                            state = dr["state"].ToString()
                        });
                    }
                }
            }
            //Se devuelve la lista
            return oList.ToList();
        }

        public bool addUser(User user)
        {
            bool res;

            try
            {
                using (SqlConnection Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();
                    SqlCommand cmd = new SqlCommand("Create_user", Connection);
                    cmd.Parameters.AddWithValue("name", user.name);
                    cmd.Parameters.AddWithValue("lastname", user.lastname);
                    cmd.Parameters.AddWithValue("email", user.email);
                    cmd.Parameters.AddWithValue("password", user.password);
                    cmd.Parameters.AddWithValue("state", user.state);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }

                res = true;
            }
            catch (Exception e)
            {
                string msg = e.Message;
                res = false;
            }

            return res;
        }

        public bool login(string email, string password)
        {
            bool res = false;

            try
            {
                using (SqlConnection Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();

                    SqlCommand cmd = new SqlCommand("Login_user", Connection);
                    cmd.Parameters.AddWithValue("email", email);
                    cmd.CommandType = CommandType.StoredProcedure;

                    //Using SqlDataReader we execute the stored procedure and read the result
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            //We take the result using the reader variable and turn it to string
                            string Hashedpassword = reader["password"].ToString();

                            //To then compare it with the password from the controller
                            res = BCrypt.Net.BCrypt.Verify(password, Hashedpassword);

                        }
                    }
                }

            }
            catch (Exception e)
            {
                string msg = e.Message;
                res = false;
            }

            return res;
        }
    }
}
