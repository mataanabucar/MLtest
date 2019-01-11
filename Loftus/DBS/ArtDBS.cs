using Loftus.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using System.Data;

namespace Loftus.DBS
{
    public class ArtDBS
    {

        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public ArtDBS()
        {
        }

        //public ArtDBS(string connectionString)
        //{
        //    this.connectionString = connectionString;
        //}

        public int AddArtwork(string title, string filename, string year, string dems, string medium, string category, string addInfo)
        {

            int rowsAffected = db.Execute(@"INSERT INTO artDetails values(@title, @filename, @year, @dems, @medium, @category, @addInfo)",
                new { title, filename, year, dems, medium, category, addInfo });

            return rowsAffected;
        }

        public int EditArtwork(string title, string year, string dems, string medium, string category, string addInfo, int id)
        {
            string qString = @"update artDetails set ";
            bool comma = false;

            if (title!=null)
            {
                if (title.Trim().Length > 0)
                {
                    qString += @"title = @title ";
                    comma=true;
                }
            }

            if (year!=null)
            {
                if (year.Trim().Length > 0)
                {
                    if(comma)
                    {
                        qString += ",";
                    }
                    qString += @"year = @year ";
                    comma=true;
                }


            }
            if (dems!=null)
            {
                if (dems.Trim().Length > 0)
                {
                    if(comma)
                    {
                        qString += ",";
                    }
                    qString += @"deminsions = @dems ";
                    comma=true;
                }
            }
            if (medium!=null)
            {
                if (medium.Trim().Length > 0)
                {
                    if(comma)
                    {
                        qString += ",";
                    }
                    qString += @"medium = @medium ";
                    comma=true;
                }
            }
            if (category!=null)
            {
                if (category.Trim().Length > 0)
                {
                    if(comma)
                    {
                        qString += ",";
                    }
                    qString += @"catagory = @category ";
                    comma=true;
                }
            }
            if (addInfo!=null)
            {
                if (addInfo.Trim().Length > 0)
                {
                    if(comma)
                    {
                        qString += ",";
                    }
                    qString += @"addInfo = @addInfo ";
                }
            }

            qString += @"where id=@id";

            int rowsAffected = db.Execute(qString, new { title, year, dems, medium, category, addInfo, id });
            //int rowsAffected = 1;

            //int rowsAffected = db.Execute(@"update artDetails set title=@title, year=@year, deminsions= @dems, medium=@medium,catagory=@category, addInfo=@addInfo where id = 1031",
            //    new { title, year, dems, medium, category, addInfo });

            return rowsAffected;
        }

        public User GetUser(string email, string password)
        {
            User user = db.Query<User>(@"
                select * from UserInfo
                where email = @email and password = @password
                ", new { email, password }).FirstOrDefault();
                

            return user;
        }

        public int IntCount()
        {
            int currentCount = 0;
            int newCount = 0;
            string getCount = @"select * from Hits";
            string setCount = @"update Hits set HitCount = ";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(getCount, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    currentCount = reader.GetInt32(0);
                }
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                newCount = currentCount + 1;
                SqlCommand cmd = new SqlCommand(setCount + newCount.ToString(), conn);
                cmd.ExecuteNonQuery();

            }


            return newCount;
        }

        public int CurrentCount()
        {
            int currentCount = 0;
            string getCount = @"select * from Hits";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(getCount, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    currentCount = reader.GetInt32(0);
                }
            }


            return currentCount;
        }

        public List<Artwork> GetAllArt()
        {
            string sql = "Select * from artDetails";
            List<Artwork> art = new List<Artwork>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    art.Add(GetArtwork(reader));
                }

            }

            return art;

        }




        private Artwork GetArtwork(SqlDataReader reader)
        {
            Artwork a = new Artwork()
            {
                ID = Convert.ToInt32(reader["id"]),
                Filename = Convert.ToString(reader["filename"]),
                Title = Convert.ToString(reader["title"]),
                Year = Convert.ToString(reader["year"]),
                Deminsions = Convert.ToString(reader["deminsions"]),
                Medium = Convert.ToString(reader["medium"]),
                Category = Convert.ToString(reader["catagory"]),
                AddInfo = Convert.ToString(reader["addInfo"]),
            };
            return a;
        }

        private User GetUser(SqlDataReader reader)
        {
            User a = new User()
            {
                Id = Convert.ToInt32(reader["id"]),
                Email = Convert.ToString(reader["email"]),
                Password = Convert.ToString(reader["password"]),
                IsAdmin = Convert.ToBoolean(reader["isAdmin"]),
            };
            return a;
        }

        public void SendNewMEssage(Message msg)
        {
           



        }












    }
}