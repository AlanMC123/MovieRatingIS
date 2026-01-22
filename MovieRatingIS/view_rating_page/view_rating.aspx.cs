using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace MovieRatingIS
{
    public partial class view_rating : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["movieId"] != null && Request.QueryString["movieTitle"] != null)
                {
                    string movieId = Request.QueryString["movieId"];
                    string movieTitle = Server.UrlDecode(Request.QueryString["movieTitle"]);
                    
                    lblMovieTitle.Text = movieTitle;
                    lblMovieID.Text = "Movie ID: " + movieId;
                    
                    BindRatingsGrid(movieId);
                }
                else
                {
                    Response.Redirect("~/main_page/main.aspx");
                }
            }
        }
        
        private void BindRatingsGrid(string movieId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;
            string query = @"
                SELECT 
                    Time,
                    Rating,
                    Comment
                FROM Rate 
                WHERE Mno = @MovieId
                ORDER BY Time DESC";
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MovieId", movieId);
                    
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    
                    gvRatings.DataSource = dataTable;
                    gvRatings.DataBind();
                }
            }
        }
        
        protected void gvRatings_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvRatings.PageIndex = e.NewPageIndex;
            string movieId = Request.QueryString["movieId"];
            BindRatingsGrid(movieId);
        }
    }
}