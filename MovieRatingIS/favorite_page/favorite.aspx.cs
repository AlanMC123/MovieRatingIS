using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class favorite_page_favorite : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // 检查用户是否登录
        if (Session["UserID"] == null || Session["Username"] == null)
        {
            Response.Redirect("~/start_page/login.html");
            return;
        }

        // 显示当前用户名
        lblUsername.Text = Session["Username"].ToString();

        if (!IsPostBack)
        {
            BindFavoritesGridView();
        }
    }

    private void BindFavoritesGridView()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;
        int userId = Convert.ToInt32(Session["UserID"]);

        // 查询用户收藏的电影
        string query = "SELECT m.Mno as MovieID, m.Mname as Title, m.Mtype as Genre, m.Myear as ReleaseYear, " +
                       "m.Mtime as LastingTime, m.Distributer as Distributor, m.Rating as Rating, " +
                       "f.FavoriteTime as FavoriteTime " +
                       "FROM Favorite f " +
                       "INNER JOIN Movie m ON f.Mno = m.Mno " +
                       "WHERE f.Uno = @Uno " +
                       "ORDER BY f.FavoriteTime DESC";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Uno", userId);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    gvFavorites.DataSource = dataTable;
                    gvFavorites.DataBind();
                    lblEmptyMessage.Visible = false;
                }
                else
                {
                    gvFavorites.DataSource = null;
                    gvFavorites.DataBind();
                    lblEmptyMessage.Visible = true;
                }
            }
        }
    }

    protected void gvFavorites_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "RemoveFavorite")
        {
            int movieId = Convert.ToInt32(e.CommandArgument);
            int userId = Convert.ToInt32(Session["UserID"]);

            bool success = RemoveFavorite(userId, movieId);
            if (success)
            {
                Response.Write("<script>alert('已取消收藏！');</script>");
                BindFavoritesGridView();
            }
            else
            {
                Response.Write("<script>alert('取消收藏失败！');</script>");
            }
        }
    }

    private bool RemoveFavorite(int userId, int movieId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string deleteQuery = "DELETE FROM Favorite WHERE Uno = @Uno AND Mno = @Mno";
            using (SqlCommand command = new SqlCommand(deleteQuery, connection))
            {
                command.Parameters.AddWithValue("@Uno", userId);
                command.Parameters.AddWithValue("@Mno", movieId);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/main_page/main.aspx");
    }

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        // 清除会话
        Session.Clear();
        Session.Abandon();

        // 重定向到登录页面
        Response.Redirect("~/start_page/login.html");
    }
}