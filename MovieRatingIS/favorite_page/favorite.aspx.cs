using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class favorite_page_favorite : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // 1. 核心修复：Session 取值需与登录时存入的 Key 完全一致
        // 且由于 ID 是 '00001' 格式，必须检查 ToString() 是否为空
        if (Session["UserID"] == null || Session["Username"] == null)
        {
            Response.Redirect("~/start_page/login.html");
            return;
        }

        lblUsername.Text = Session["Username"].ToString();

        if (!IsPostBack)
        {
            BindFavoritesGridView();
        }
    }

    private void BindFavoritesGridView()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;
        
        string userId = Session["UserID"].ToString();

        string query = @"
            SELECT 
                m.Mno as MovieID, 
                m.Mname as Title, 
                m.Mtype as Genre, 
                m.Myear as ReleaseYear, 
                m.Mtime as LastingTime, 
                (SELECT AVG(CAST(Rating AS FLOAT)) FROM Rate WHERE Mno = m.Mno) as AvgRating
            FROM Favorite f
            INNER JOIN Movie m ON f.Mno = m.Mno
            WHERE f.Uno = @Uno
            ORDER BY m.Mno ASC";
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
            // 5. 修复：ID 为 m001 格式，应作为 string 处理
            string movieId = e.CommandArgument.ToString();
            string userId = Session["UserID"].ToString();

            bool success = RemoveFavorite(userId, movieId);
            if (success)
            {
                // 使用客户端脚本弹出提示，避免页面刷新中断
                Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Removed from favorites!');", true);
                BindFavoritesGridView();
            }
        }
    }

    private bool RemoveFavorite(string userId, string movieId)
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
                return command.ExecuteNonQuery() > 0;
            }
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/main_page/main.aspx");
    }

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Session.Abandon();
        Response.Redirect("~/start_page/login.html");
    }
}