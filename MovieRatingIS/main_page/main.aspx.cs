﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace MovieRatingIS
{
    public partial class main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
            }

            if (Session["Username"] != null)
            {
                lblUsername.Text = Session["Username"].ToString();
            }
            else
            {
                Response.Redirect("~/start_page/login.html");
            }
        }

        private void BindGridView()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;
            string searchTerm = txtSearch.Text.Trim();
            string query = @"
                SELECT 
                    m.Mno as MovieID, 
                    m.Mname as Title, 
                    m.Mtype as Genre, 
                    m.Myear as ReleaseYear, 
                    m.Mtime as Duration, 
                    AVG(CAST(r.Rating AS FLOAT)) as AvgRating 
                FROM Movie m
                LEFT JOIN (
                    SELECT Mno, Rating FROM Rate 
                ) r ON m.Mno = r.Mno";

            // 构建过滤条件
            string whereClause = "";
            if (!string.IsNullOrEmpty(searchTerm))
            {
                whereClause = " WHERE m.Mname LIKE @SearchTerm OR m.Mtype LIKE @SearchTerm OR m.Myear LIKE @SearchTerm";
            }

            // 分组以计算平均值
            string groupBy = " GROUP BY m.Mno, m.Mname, m.Mtype, m.Myear, m.Mtime";

            query += whereClause + groupBy;

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

                    UpdatePaginationInfo();
                }
            }
        }

        /* main.aspx.cs */

        private void UpdatePaginationInfo()
        {
            if (lblCurrentPage != null && lblTotalPages != null)
            {
                // 显示当前页码
                lblCurrentPage.Text = (gvMovies.PageIndex + 1).ToString();
                // 显示总页数
                lblTotalPages.Text = (gvMovies.PageCount > 0 ? gvMovies.PageCount : 1).ToString();
            }

            // 逻辑：如果是第一页，禁用 "First" 和 "Prev"
            bool isFirstPage = (gvMovies.PageIndex == 0);
            // 逻辑：如果是最后一页，禁用 "Next" 和 "Last"
            bool isLastPage = (gvMovies.PageIndex >= gvMovies.PageCount - 1);

            if (lnkFirst != null) 
            {
                lnkFirst.Enabled = !isFirstPage;
                // 关键修复：手动添加 CSS 类，让它变灰
                lnkFirst.CssClass = isFirstPage ? "disabled" : ""; 
            }

            if (lnkPrev != null) 
            {
                lnkPrev.Enabled = !isFirstPage;
                lnkPrev.CssClass = isFirstPage ? "disabled" : "";
            }

            if (lnkNext != null) 
            {
                lnkNext.Enabled = !isLastPage;
                lnkNext.CssClass = isLastPage ? "disabled" : "";
            }

            if (lnkLast != null) 
            {
                lnkLast.Enabled = !isLastPage;
                lnkLast.CssClass = isLastPage ? "disabled" : "";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvMovies.PageIndex = 0;
            BindGridView();
        }

        protected void gvMovies_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvMovies.PageIndex = e.NewPageIndex;
            BindGridView();
        }

        protected void lnkFirst_Click(object sender, EventArgs e)
        {
            gvMovies.PageIndex = 0;
            BindGridView();
        }

        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            if (gvMovies.PageIndex > 0)
            {
                gvMovies.PageIndex--;
                BindGridView();
            }
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
            if (gvMovies.PageIndex < gvMovies.PageCount - 1)
            {
                gvMovies.PageIndex++;
                BindGridView();
            }
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (gvMovies.PageCount > 0)
            {
                gvMovies.PageIndex = gvMovies.PageCount - 1;
                BindGridView();
            }
        }

        protected void btnRateMovie_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvMovies.Rows)
            {
                RadioButton rb = (RadioButton)row.FindControl("rbSelectMovie");
                if (rb != null && rb.Checked)
                {
                    // 1. 安全获取 MovieID
                    string movieIdStr = gvMovies.DataKeys[row.RowIndex].Value.ToString();

                    // 2. 获取 UserID (直接作为字符串处理，保持 '00001' 格式)
                    string userId = Session["UserID"] != null ? Session["UserID"].ToString() : "";

                    string movieTitle = row.Cells[2].Text; 

                    // 3. 传参
                    Response.Redirect("../rating_page/rating.html?movieId=" + movieIdStr + "&movieTitle=" + Server.UrlEncode(movieTitle) + "&userId=" + userId);
                    return;
                }
            }
            Response.Write("<script>alert('Please select a movie first!');</script>");
        }

        protected void btnFavorite_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvMovies.Rows)
            {
                RadioButton rb = (RadioButton)row.FindControl("rbSelectMovie");
                if (rb != null && rb.Checked)
                {
                    // 1. 直接获取字符串形式的 MovieID
                    string movieId = gvMovies.DataKeys[row.RowIndex].Value.ToString();
                    string movieTitle = row.Cells[2].Text;

                    if (Session["UserID"] == null)
                    {
                        Response.Write("<script>alert('Please login first!');window.location.href='../start_page/login.html';</script>");
                        return;
                    }

                    // 2. 直接获取字符串形式的 UserID (兼容 '00001')
                    string userId = Session["UserID"].ToString();

                    // 3. 调用修改后的 AddFavorite
                    bool success = AddFavorite(userId, movieId);

                    if (success)
                    {
                        Response.Write("<script>alert('Movie \"" + movieTitle + "\" added to favorites successfully!');</script>");
                    }
                    else
                    {
                        Response.Write("<script>alert('Failed to add favorite. The movie might already be in your favorites!');</script>");
                    }
                    return;
                }
            }
            Response.Write("<script>alert('Please select a movie first!');</script>");
        }

        private bool AddFavorite(string userId, string movieId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string checkQuery = "SELECT COUNT(*) FROM Favorite WHERE Uno = @Uno AND Mno = @Mno";
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    // 这里直接传字符串，不要 Convert.ToInt32
                    checkCommand.Parameters.AddWithValue("@Uno", userId);
                    checkCommand.Parameters.AddWithValue("@Mno", movieId);
                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                    if (count > 0)
                    {
                        return false; 
                    }
                }

                string insertQuery = "INSERT INTO Favorite (Uno, Mno, Type) VALUES (@Uno, @Mno, @Type)";
                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@Uno", userId);
                    insertCommand.Parameters.AddWithValue("@Mno", movieId);
                    insertCommand.Parameters.AddWithValue("@Type", "favorite");

                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

                protected void btnViewFavorites_Click(object sender, EventArgs e)
        {
            Response.Redirect("/favorite_page/favorite.aspx");
        }

        protected void btnViewRatings_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvMovies.Rows)
            {
                RadioButton rb = (RadioButton)row.FindControl("rbSelectMovie");
                if (rb != null && rb.Checked)
                {
                    // 获取 MovieID 和电影标题
                    string movieIdStr = gvMovies.DataKeys[row.RowIndex].Value.ToString();
                    string movieTitle = row.Cells[2].Text;

                    // 跳转到评分查看页面
                    Response.Redirect("../view_rating_page/view_rating.aspx?movieId=" + movieIdStr + "&movieTitle=" + Server.UrlEncode(movieTitle));
                    return;
                }
            }
            Response.Write("<script>alert('Please select a movie first!');</script>");
        }

                protected void lnkChangePassword_Click(object sender, EventArgs e)
                {
                    Response.Redirect("~/changepwd_page/change_password.aspx");
                }

                protected void lnkLogout_Click(object sender, EventArgs e)
                {
                    Session.Clear();
                    Session.Abandon();
                    Response.Redirect("~/start_page/login.html");
                }
            }
        }