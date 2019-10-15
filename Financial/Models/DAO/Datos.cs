using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Financial.Models
{
    public class Datos
    {
        SqlConnection conexion = new SqlConnection();

        public Datos()
        {
            conexion.ConnectionString = ConfigurationManager.ConnectionStrings["conn"].ConnectionString;
        }


        public Modelreg registrar(string name, string email)
        {
            Modelreg registro = new Modelreg();
            int IdNuevo = 0;

            string sql = "INSERT INTO [dbo].[registro]"
           + "([name]"
           + ",[email]"
           + ",[idtrasa]"
           + ",[ended]"
           + ",[userattender])  VALUES"
           + "('" + name
           + "', '" + email + "'"
           + ", CONCAT(rand(), '')"
           + ", 0"
           + ", 1) SELECT SCOPE_IDENTITY()";

            SqlCommand command = new SqlCommand();
            command.Connection = conexion;
            command.CommandType = CommandType.Text;
            command.CommandText = sql;


            try
            {
                conexion.Open();
                var IdNueva = command.ExecuteScalar();
                IdNuevo = Convert.ToInt32(IdNueva);

            }
            catch (SqlException ex)
            {
                // error here
            }
            finally
            {
                conexion.Close();
            }

            registro = getRegistro(IdNuevo);
            return registro;
        }

        public void finChat(int idReg)
        {
            string sql = "UPDATE [dbo].[registro]  SET [ended] = 1"
                        + " WHERE[Id] = " + idReg;

            SqlCommand command = new SqlCommand();
            command.Connection = conexion;
            command.CommandType = CommandType.Text;
            command.CommandText = sql;


            try
            {
                conexion.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                // error here
            }
            finally
            {
                conexion.Close();
            }
        }
        public void updFuncionario(int idfun, int idReg)
        {
            string sql = "UPDATE [dbo].[registro]  SET[userattender] = " + idfun
                         + " WHERE[Id] = " + idReg;

            SqlCommand command = new SqlCommand();
            command.Connection = conexion;
            command.CommandType = CommandType.Text;
            command.CommandText = sql;


            try
            {
                conexion.Open();
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                // error here
            }
            finally
            {
                conexion.Close();
            }


        }

        public userAttender getUserAttenderbyTraza(string traza)
        {
            userAttender usr = new userAttender();

            conexion.Open();
            string sql = " SELECT  a.[Id], a.[name] ,[username] ,[pass] " +
                 "FROM[challenge].[dbo].[userAttender] a INNER JOIN[challenge].[dbo].[registro] r" +
                 "On a.Id = r.userattender" +
                  "WHERE r.idtrasa like '´" + traza + "'";
            SqlCommand command = new SqlCommand(sql, conexion);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    usr.Id = Convert.ToInt32(reader["Id"].ToString());
                    usr.name = reader["name"].ToString();
                    usr.username = reader["username"].ToString();

                }
            }

            conexion.Close();
            return usr;
        }



        public List<Modelreg> getUsersSA()
        {
            List<Modelreg> users = new List<Modelreg>();

            conexion.Open();
            string sql = "SELECT *  FROM [challenge].[dbo].[registro]"
                        + "WHERE[ended] = 0 AND[userattender] = 1";

            SqlCommand command = new SqlCommand(sql, conexion);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Modelreg usr = new Modelreg();

                    usr.Id = Convert.ToInt32(reader["Id"].ToString());
                    usr.name = reader["name"].ToString();
                    usr.email = reader["email"].ToString();
                    usr.idTraza = reader["idtrasa"].ToString();
                    users.Add(usr);
                }
            }

            conexion.Close();
            return users;
        }

        public userAttender Login(string username, string pass)
        {
            userAttender usr = new userAttender();

            conexion.Open();
            string sql = " SELECT  [Id],[name],[username],[pass]"
                         + "FROM [challenge].[dbo].[userAttender]"
                         + "Where [username] like '" + username + "' AND[pass] like'" + pass + "'";
            SqlCommand command = new SqlCommand(sql, conexion);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    usr.Id = Convert.ToInt32(reader["Id"].ToString());
                    usr.name = reader["name"].ToString();
                    usr.username = reader["username"].ToString();

                }
            }

            conexion.Close();
            return usr;
        }

        public Modelreg getRegistro(int id)
        {
            Modelreg registro = new Modelreg();

            conexion.Open();

            SqlCommand command = new SqlCommand("SELECT * FROM [challenge].[dbo].[registro] WHERE[Id] = " + id, conexion);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    registro.Id = id;
                    registro.email = reader["email"].ToString();
                    registro.name = reader["name"].ToString();
                    registro.idTraza = reader["idtrasa"].ToString();
                    registro.busy = Convert.ToBoolean(reader["ended"].ToString());
                    registro.attender = Convert.ToInt32(reader["userattender"].ToString());

                }
            }

            conexion.Close();

            return registro;
        }

    }
}