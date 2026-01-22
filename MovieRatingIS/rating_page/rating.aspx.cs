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

    // 
// [rating.aspx.cs]

// [rating.aspx.cs]
// [rating.aspx.cs]
    private void ProcessRating()
    {
        try
        {
            // 1. 获取原始字符串数据
            string movieIdStr = Request.Form["movieId"];
            string userIdStr = Request.Form["userId"];
            string ratingValueStr = Request.Form["rating"];
            string comment = Request.Form["comment"];

            // 2. 数据清洗：去除可能的空格 (针对 'm001 ' 这种情况)
            string movieId = string.IsNullOrEmpty(movieIdStr) ? "" : movieIdStr.Trim();
            string userId = string.IsNullOrEmpty(userIdStr) ? "" : userIdStr.Trim();

            // 3. 验证评分 (评分仍然必须是数字)
            decimal ratingValue;
            bool isRatingValid = decimal.TryParse(ratingValueStr, out ratingValue);

            // 4. 验证必填项 (ID不再验证是否为数字，只验证是否有值)
            if (string.IsNullOrEmpty(movieId) || !isRatingValid)
            {
                string errorDetails = "";
                if (string.IsNullOrEmpty(movieId)) 
                {
                    errorDetails += "MovieID is empty ";
                }
                if (!isRatingValid) 
                {
                    errorDetails += "Invalid rating (received value: '" + ratingValueStr + "')";
                }

                lblMessage.Text = "Submission failed details: " + errorDetails;
                return;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string insertRatingQuery = "INSERT INTO Rate (Mno, Uno, Rating, Comment, Time) " +
                                          "VALUES (@Mno, @Uno, @Rating, @Comment, @Time)";

                using (SqlCommand command = new SqlCommand(insertRatingQuery, connection))
                {
                    // 【修正】直接传入字符串类型的 ID
                    command.Parameters.AddWithValue("@Mno", movieId);

                    // 处理 UserID (也作为字符串处理，兼容 '00001' 这种格式)
                    if (!string.IsNullOrEmpty(userId))
                    {
                        command.Parameters.AddWithValue("@Uno", userId);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Uno", DBNull.Value);
                    }

                    command.Parameters.AddWithValue("@Rating", ratingValue);
                    command.Parameters.AddWithValue("@Comment", string.IsNullOrEmpty(comment) ? DBNull.Value : (object)comment);
                    command.Parameters.AddWithValue("@Time", DateTime.Now);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // 注意：UpdateMovieAverageRating 内部可能也需要修改为接收 string
                        UpdateMovieAverageRating(connection, movieId); 

                        Response.Write("<script>alert('Rating submitted successfully!');window.location.href='../main_page/main.aspx';</script>");
                    }
                    else
                    {
                        lblMessage.Text = "Fail to submit. Check your input data and try again.";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error: " + ex.Message;
        }
    }

    // 【额外修正】你需要同时修改下面这个辅助方法的签名，把 int movieId 改成 string movieId
    private void UpdateMovieAverageRating(SqlConnection connection, string movieId)
    {
        string calculateAverageQuery = "SELECT AVG(Rating) FROM Rate WHERE Mno = @Mno";
        // ... (内部代码不用变，Command 会自动处理 string 参数)
        using (SqlCommand command = new SqlCommand(calculateAverageQuery, connection))
        {
            command.Parameters.AddWithValue("@Mno", movieId);
            object result = command.ExecuteScalar();
            // ... (后续逻辑保持不变)
        }
    }
    
        private void UpdateMovieAverageRating(SqlConnection connection, int movieId)
        {
            // Calculate average rating
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
        }
    }