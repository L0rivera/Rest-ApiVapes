using System.Data.SqlClient;
using System.Data;
using Rest_ApiVapes.Models;

namespace Rest_ApiVapes.Data
{
    public class VapeData
    {
        private readonly string _connectionString;

        public VapeData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SQLConnection");
        }

        public List<Vape> GetVapes()
        {
            var oList = new List<Vape>();

            using (SqlConnection Connection = new SqlConnection(_connectionString))
            {
                Connection.Open();
                SqlCommand cmd = new SqlCommand("get_Vapes", Connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oList.Add(new Vape()
                        {
                            VapeId = Convert.ToInt32(dr["VapeId"]),
                            name = dr["name"].ToString(),
                            price = Convert.ToDecimal(dr["price"]),
                            status = Convert.ToBoolean(dr["status"]),
                            brand = dr["brand"].ToString(),
                            flavor = dr["flavor"].ToString(),
                            public_id = dr["public_id"].ToString(),
                            secure_url = dr["secure_url"].ToString()
                        });
                    }
                }

            }

            return oList.ToList();
        }

        public bool addVape(Vape vape)
        {
            bool res;

            try
            {
                using(SqlConnection Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();
                    SqlCommand cmd = new SqlCommand("add_Vape", Connection);
                    cmd.Parameters.AddWithValue("name", vape.name);
                    cmd.Parameters.AddWithValue("description", vape.description);
                    cmd.Parameters.AddWithValue("price", vape.price);
                    cmd.Parameters.AddWithValue("status", vape.status);
                    cmd.Parameters.AddWithValue("brand", vape.brand);
                    cmd.Parameters.AddWithValue("flavor", vape.flavor);
                    cmd.Parameters.AddWithValue("public_id", vape.public_id);
                    cmd.Parameters.AddWithValue("secure_url", vape.secure_url);
                    cmd.Parameters.AddWithValue("nicotine", vape.nicotine);
                    cmd.Parameters.AddWithValue("e_liquid", vape.e_liquid);
                    cmd.Parameters.AddWithValue("mAh", vape.mAh);
                    cmd.Parameters.AddWithValue("uses", vape.uses);
                    cmd.Parameters.AddWithValue("rechargable", vape.rechargable);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }
                res = true;
            }
            catch(Exception e)
            {
                string msg = e.Message;
                res = false;
            }
            return res;
        }

        public bool DeleteVape(int VapeId)
        {
            bool res;

            try
            {
                using(SqlConnection Connection = new SqlConnection(_connectionString))
                {
                    Connection.Open();
                    SqlCommand cmd = new SqlCommand("delete_vape", Connection);
                    cmd.Parameters.AddWithValue("VapeId", VapeId);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }

                res = true;
            }
            catch(Exception e)
            {               
                res = false;
            }

            return res;
        }
    }
}
