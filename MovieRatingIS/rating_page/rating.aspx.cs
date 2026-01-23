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
            string movieIdStr = Request.Form["movieId"];
            string userIdStr = Request.Form["userId"];
            string ratingValueStr = Request.Form["rating"];
            string comment = Request.Form["comment"];
    
            string movieId = string.IsNullOrEmpty(movieIdStr) ? "" : movieIdStr.Trim();
            string userId = string.IsNullOrEmpty(userIdStr) ? "" : userIdStr.Trim();
    
            decimal ratingValue;
            bool isRatingValid = decimal.TryParse(ratingValueStr, out ratingValue);
    
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

                string insertRatingQuery = "INSERT INTO Rate (Mno, Uno, Rating, Comment, [Time]) " +
                                          "VALUES (@Mno, @Uno, @Rating, @Comment, @Time)";

                using (SqlCommand command = new SqlCommand(insertRatingQuery, connection))
                {
                    command.Parameters.AddWithValue("@Mno", movieId);
    
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
        catch (SqlException ex)
        {
            if (ex.Number == 2627)
            {
                Response.Write("<script>alert('You have already rated this movie!');window.location.href='../main_page/main.aspx';</script>");
            }
            else
            {
                lblMessage.Text = "SQL Error: " + ex.Message;
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error: " + ex.Message;
        }
    }

    private void UpdateMovieAverageRating(SqlConnection connection, string movieId)
    {
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