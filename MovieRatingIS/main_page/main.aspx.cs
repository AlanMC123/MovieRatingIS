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
        string query = "SELECT MovieID, Title, Director, ReleaseYear, Genre, Rating FROM Movies";

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query += " WHERE Title LIKE @SearchTerm OR Director LIKE @SearchTerm OR Genre LIKE @SearchTerm";
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