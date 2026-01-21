using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class rating_page_rating : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.HttpMethod == "POST")
        {
            ProcessRating();
        }
    }

    private void ProcessRating()
    {
        try
        {
            // 获取表单数据
            string movieId = Request.Form["movieId"];
            string userId = Request.Form["userId"];
            string ratingValue = Request.Form["rating"];
            string comment = Request.Form["comment"];

            // 验证必填字段
            if (string.IsNullOrEmpty(movieId) || string.IsNullOrEmpty(ratingValue))
            {
                lblMessage.Text = "缺少必要的评分信息";
                return;
            }

            // 获取数据库连接字符串
            string connectionString = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // 写入评分数据到Rate表
                string insertRatingQuery = "INSERT INTO Rate (Mno, Uno, Rating, Comment, Time) " +
                                          "VALUES (@Mno, @Uno, @Rating, @Comment, @Time)";

                using (SqlCommand command = new SqlCommand(insertRatingQuery, connection))
                {
                    command.Parameters.AddWithValue("@Mno", Convert.ToInt32(movieId));
                    command.Parameters.AddWithValue("@Uno", !string.IsNullOrEmpty(userId) ? Convert.ToInt32(userId) : DBNull.Value);
                    command.Parameters.AddWithValue("@Rating", Convert.ToDecimal(ratingValue));
                    command.Parameters.AddWithValue("@Comment", string.IsNullOrEmpty(comment) ? DBNull.Value : (object)comment);
                    command.Parameters.AddWithValue("@Time", DateTime.Now);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // 更新Movie表中的平均评分
                        UpdateMovieAverageRating(connection, Convert.ToInt32(movieId));

                        // 评分成功，跳回电影列表
                        Response.Write("<script>alert('评分成功！');window.location.href='../main_page/main.aspx';</script>");
                    }
                    else
                    {
                        lblMessage.Text = "评分提交失败，请重试";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "处理评分时发生错误：" + ex.Message;
        }
    }

    private void UpdateMovieAverageRating(SqlConnection connection, int movieId)
    {
        // 计算平均评分
        string calculateAverageQuery = "SELECT AVG(Rating) FROM Rate WHERE Mno = @Mno";
        decimal averageRating = 0;

        using (SqlCommand command = new SqlCommand(calculateAverageQuery, connection))
        {
            command.Parameters.AddWithValue("@Mno", movieId);
            object result = command.ExecuteScalar();
            if (result != DBNull.Value)
            {
                averageRating = Convert.ToDecimal(result);
            }
        }

        // 注意：Movie表中没有Rating列，暂时注释掉更新操作
        // 如需实现此功能，请先在Movie表中添加Rating列
        /*
        string updateMovieQuery = "UPDATE Movie SET Rating = @AverageRating WHERE Mno = @Mno";
        using (SqlCommand command = new SqlCommand(updateMovieQuery, connection))
        {
            command.Parameters.AddWithValue("@AverageRating", averageRating);
            command.Parameters.AddWithValue("@Mno", movieId);
            command.ExecuteNonQuery();
        }
        */
    }
}