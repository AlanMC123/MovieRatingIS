using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MovieRatingIS.DAL
{
    public class UserDAL
    {
        // 1根据用户名查询用户信息,用于登录校验、修改密码校验
        public DataTable GetUserByUsername(string username)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;
            string query = "SELECT Uno, Uname, Upassword FROM Users WHERE Uname = @Username";
            DataTable userTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(userTable);
                    }
                }
                catch (Exception ex)
                {
                    // 抛出异常
                    throw new Exception("查询用户信息失败：" + ex.Message);
                }
            }
            return userTable;
        }

        // 修改用户密码
        public bool UpdatePassword(int userId, string oldPwd, string newPwd)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;
            string query = "UPDATE Users SET Upassword = @NewPwd WHERE Uno = @UserID AND Upassword = @OldPwd";
            int affectedRows = 0; 
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@OldPwd", oldPwd);
                        cmd.Parameters.AddWithValue("@NewPwd", newPwd);
                        affectedRows = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("修改密码失败：" + ex.Message);
                }
            }
            return affectedRows > 0;
        }

        // 用户注册方法
        public bool AddUser(MovieRatingIS.Model.User user)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;
            string query = "INSERT INTO Users (Uname, Upassword, Usex, Utelephone) VALUES (@Uname, @Upassword, @Usex, @Utelephone)";
            int affectedRows = 0;
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Uname", user.Username);
                        cmd.Parameters.AddWithValue("@Upassword", user.Password);
                        cmd.Parameters.AddWithValue("@Usex", user.Gender);
                        cmd.Parameters.AddWithValue("@Utelephone", user.Tele);
                        affectedRows = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("用户注册失败：" + ex.Message);
                }
            }
            return affectedRows > 0;
        }
    }
}