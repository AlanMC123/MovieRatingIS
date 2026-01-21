using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class main : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGridView();
        }
        
        // 显示当前用户名
        if (Session["Username"] != null)
        {
            lblUsername.Text = Session["Username"].ToString();
        }
        else
        {
            // 如果没有用户名信息，重定向到登录页面
            Response.Redirect("~/start_page/login.html");
        }
    }

    private void BindGridView()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;
        string searchTerm = txtSearch.Text.Trim();
        string query = "SELECT Mno,Mname,Mtype,Myear,Mtime,Distributer FROM Movie";//进行了修改，目前Movie表结构如下

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query += " WHERE Mname LIKE @SearchTerm OR Mtype LIKE @SearchTerm OR Myear LIKE @SearchTerm";//根据表结构，现在仅支持基于电影名、电影类型和上映时间的查询
        }

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                gvMovies.DataSource = dataTable;
                gvMovies.DataBind();

                // 更新页面信息
                UpdatePaginationInfo();
            }
        }
    }

    private void UpdatePaginationInfo()
    {
        // 更新当前页码和总页数
        lblCurrentPage.Text = (gvMovies.PageIndex + 1).ToString();
        lblTotalPages.Text = gvMovies.PageCount.ToString();

        // 启用/禁用分页按钮
        lnkFirst.Enabled = (gvMovies.PageIndex > 0);
        lnkPrev.Enabled = (gvMovies.PageIndex > 0);
        lnkNext.Enabled = (gvMovies.PageIndex < gvMovies.PageCount - 1);
        lnkLast.Enabled = (gvMovies.PageIndex < gvMovies.PageCount - 1);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // 重置分页索引
        gvMovies.PageIndex = 0;
        BindGridView();
    }

    protected void gvMovies_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        // 设置新的分页索引
        gvMovies.PageIndex = e.NewPageIndex;
        BindGridView();
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        // 跳转到第一页
        gvMovies.PageIndex = 0;
        BindGridView();
    }

    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        // 跳转到上一页
        if (gvMovies.PageIndex > 0)
        {
            gvMovies.PageIndex--;
            BindGridView();
        }
    }

    protected void btnRateMovie_Click(object sender, EventArgs e)
    {
        // 遍历GridView，找到被选中的电影
        foreach (GridViewRow row in gvMovies.Rows)
        {
            RadioButton rb = (RadioButton)row.FindControl("rbSelectMovie");
            if (rb != null && rb.Checked)
            {
                // 获取选中的电影ID和名称
                int movieId = Convert.ToInt32(gvMovies.DataKeys[row.RowIndex].Value);
                string movieTitle = row.Cells[2].Text; // 电影名称在第2列

                // 跳转到评分页面
                string userId = Session["UserID"] != null ? Session["UserID"].ToString() : "";
                Response.Redirect($"../rating_page/rating.html?movieId={movieId}&movieTitle={Server.UrlEncode(movieTitle)}&userId={userId}");
                return;
            }
        }

        // 如果没有选中电影，提示用户
        Response.Write("<script>alert('请先选择一部电影！');</script>");
    }

    protected void btnFavorite_Click(object sender, EventArgs e)
    {
        // 遍历GridView，找到被选中的电影
        foreach (GridViewRow row in gvMovies.Rows)
        {
            RadioButton rb = (RadioButton)row.FindControl("rbSelectMovie");
            if (rb != null && rb.Checked)
            {
                // 获取选中的电影ID
                int movieId = Convert.ToInt32(gvMovies.DataKeys[row.RowIndex].Value);
                string movieTitle = row.Cells[2].Text;

                // 获取用户ID
                if (Session["UserID"] == null)
                {
                    Response.Write("<script>alert('请先登录！');window.location.href='../start_page/login.html';</script>");
                    return;
                }
                int userId = Convert.ToInt32(Session["UserID"]);

                // 收藏电影
                bool success = AddFavorite(userId, movieId);
                if (success)
                {
                    Response.Write($"<script>alert('电影\"{movieTitle}\"已成功收藏！');</script>");
                }
                else
                {
                    Response.Write("<script>alert('收藏失败，该电影可能已被收藏！');</script>");
                }
                return;
            }
        }

        // 如果没有选中电影，提示用户
        Response.Write("<script>alert('请先选择一部电影！');</script>");
    }

    private bool AddFavorite(int userId, int movieId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // 检查是否已经收藏
            string checkQuery = "SELECT COUNT(*) FROM Favorite WHERE Uno = @Uno AND Mno = @Mno";
            using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@Uno", userId);
                checkCommand.Parameters.AddWithValue("@Mno", movieId);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                if (count > 0)
                {
                    return false; // 已收藏
                }
            }

            // 添加收藏
            string insertQuery = "INSERT INTO Favorite (Uno, Mno, FavoriteTime) VALUES (@Uno, @Mno, @FavoriteTime)";
            using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
            {
                insertCommand.Parameters.AddWithValue("@Uno", userId);
                insertCommand.Parameters.AddWithValue("@Mno", movieId);
                insertCommand.Parameters.AddWithValue("@FavoriteTime", DateTime.Now);

                int rowsAffected = insertCommand.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }

    protected void btnViewFavorites_Click(object sender, EventArgs e)
    {
        // 跳转到收藏页面
        Response.Redirect("../favorite_page/favorite.aspx");
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        // 跳转到下一页
        if (gvMovies.PageIndex < gvMovies.PageCount - 1)
        {
            gvMovies.PageIndex++;
            BindGridView();
        }
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        // 跳转到最后一页
        gvMovies.PageIndex = gvMovies.PageCount - 1;
        BindGridView();
    }

    protected void lnkChangePassword_Click(object sender, EventArgs e)
    {
        // 处理修改密码功能
        Response.Redirect("~/changepwd_page/change_password.aspx");
    }

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        // 处理退出登录功能
        // 清除会话
        Session.Clear();
        Session.Abandon();
        
        // 重定向到登录页面
        Response.Redirect("~/start_page/login.html");
    }
}