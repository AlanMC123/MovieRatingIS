using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MovieRatingIS.DAL
{
    public class MovieDAL
    {
        // 根据搜索关键词查询电影列表
        public DataTable GetMovies(string searchTerm)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;
            string query = "SELECT Mno as MovieID, Mname as Title, Mtype as Genre, Myear as ReleaseYear, Distributer as Director FROM Movie";
            DataTable movieTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        // 有搜索关键词时，添加模糊查询条件
                        if (!string.IsNullOrEmpty(searchTerm))
                        {
                            query += " WHERE Mname LIKE @SearchTerm OR Mtype LIKE @SearchTerm OR Distributer LIKE @SearchTerm";
                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                        }
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(movieTable);
                    }
                }
                catch (Exception ex)
                {
                    // 异常提示
                    throw new Exception("查询电影列表失败：" + ex.Message);
                }
            }
            return movieTable;
        }

        // 获取符合条件的电影总数（用于分页计算）
        public int GetMovieTotalCount(string searchTerm)
        {
            string connStr = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;
            string query = "SELECT COUNT(*) FROM Movie";
            int totalCount = 0;

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(searchTerm))
                        {
                            query += " WHERE Mname LIKE @SearchTerm OR Mtype LIKE @SearchTerm OR Distributer LIKE @SearchTerm";
                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                        }
                        totalCount = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("查询电影总数失败：" + ex.Message);
                }
            }
            return totalCount;
        }
    }
}