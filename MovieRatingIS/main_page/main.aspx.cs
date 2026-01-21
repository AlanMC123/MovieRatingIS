using System;
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
        
        // 鏄剧ず褰撳墠鐢ㄦ埛鍚?
        if (Session["Username"] != null)
        {
            lblUsername.Text = Session["Username"].ToString();
        }
        else
        {
            // 濡傛灉娌℃湁鐢ㄦ埛鍚嶄俊鎭紝閲嶅畾鍚戝埌鐧诲綍椤甸潰
            Response.Redirect("~/start_page/login.html");
        }
    }

    private void BindGridView()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;
        string searchTerm = txtSearch.Text.Trim();
        string query = "SELECT Mno,Mname,Mtype,Myear,Mtime,Distributer FROM Movie";//杩涜浜嗕慨鏀癸紝鐩墠Movie琛ㄧ粨鏋勫涓?

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query += " WHERE Mname LIKE @SearchTerm OR Mtype LIKE @SearchTerm OR Myear LIKE @SearchTerm";//鏍规嵁琛ㄧ粨鏋勶紝鐜板湪浠呮敮鎸佸熀浜庣數褰卞悕銆佺數褰辩被鍨嬪拰涓婃槧鏃堕棿鐨勬煡璇?
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

                // 鏇存柊椤甸潰淇℃伅
                UpdatePaginationInfo();
            }
        }
    }

    private void UpdatePaginationInfo()
    {
        // 鏇存柊褰撳墠椤电爜鍜屾€婚〉鏁?
        lblCurrentPage.Text = (gvMovies.PageIndex + 1).ToString();
        lblTotalPages.Text = gvMovies.PageCount.ToString();

        // 鍚敤/绂佺敤鍒嗛〉鎸夐挳
        lnkFirst.Enabled = (gvMovies.PageIndex > 0);
        lnkPrev.Enabled = (gvMovies.PageIndex > 0);
        lnkNext.Enabled = (gvMovies.PageIndex < gvMovies.PageCount - 1);
        lnkLast.Enabled = (gvMovies.PageIndex < gvMovies.PageCount - 1);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // 閲嶇疆鍒嗛〉绱㈠紩
        gvMovies.PageIndex = 0;
        BindGridView();
    }

    protected void gvMovies_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        // 璁剧疆鏂扮殑鍒嗛〉绱㈠紩
        gvMovies.PageIndex = e.NewPageIndex;
        BindGridView();
    }

    protected void lnkFirst_Click(object sender, EventArgs e)
    {
        // 璺宠浆鍒扮涓€椤?
        gvMovies.PageIndex = 0;
        BindGridView();
    }

    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        // 璺宠浆鍒颁笂涓€椤?
        if (gvMovies.PageIndex > 0)
        {
            gvMovies.PageIndex--;
            BindGridView();
        }
    }

    protected void btnRateMovie_Click(object sender, EventArgs e)
    {
        // 閬嶅巻GridView锛屾壘鍒拌閫変腑鐨勭數褰?
        foreach (GridViewRow row in gvMovies.Rows)
        {
            RadioButton rb = (RadioButton)row.FindControl("rbSelectMovie");
            if (rb != null && rb.Checked)
            {
                // 鑾峰彇閫変腑鐨勭數褰盜D鍜屽悕绉?
                int movieId = Convert.ToInt32(gvMovies.DataKeys[row.RowIndex].Value);
                string movieTitle = row.Cells[2].Text; // 鐢靛奖鍚嶇О鍦ㄧ2鍒?

                // 璺宠浆鍒拌瘎鍒嗛〉闈?
                string userId = Session["UserID"] != null ? Session["UserID"].ToString() : "";
                Response.Redirect("../rating_page/rating.html?movieId=" + movieId + "&movieTitle=" + Server.UrlEncode(movieTitle) + "&userId=" + userId);
                return;
            }
        }

        // 濡傛灉娌℃湁閫変腑鐢靛奖锛屾彁绀虹敤鎴?
        Response.Write("<script>alert('璇峰厛閫夋嫨涓€閮ㄧ數褰憋紒');</script>");
    }

    protected void btnFavorite_Click(object sender, EventArgs e)
    {
        // 閬嶅巻GridView锛屾壘鍒拌閫変腑鐨勭數褰?
        foreach (GridViewRow row in gvMovies.Rows)
        {
            RadioButton rb = (RadioButton)row.FindControl("rbSelectMovie");
            if (rb != null && rb.Checked)
            {
                // 鑾峰彇閫変腑鐨勭數褰盜D
                int movieId = Convert.ToInt32(gvMovies.DataKeys[row.RowIndex].Value);
                string movieTitle = row.Cells[2].Text;

                // 鑾峰彇鐢ㄦ埛ID
                if (Session["UserID"] == null)
                {
                    Response.Write("<script>alert('璇峰厛鐧诲綍锛?);window.location.href='../start_page/login.html';</script>");
                    return;
                }
                int userId = Convert.ToInt32(Session["UserID"]);

                // 鏀惰棌鐢靛奖
                bool success = AddFavorite(userId, movieId);
                if (success)
                {
                    Response.Write("<script>alert('鐢靛奖\"" + movieTitle + "\"宸叉垚鍔熸敹钘忥紒');</script>");
                }
                else
                {
                    Response.Write("<script>alert('鏀惰棌澶辫触锛岃鐢靛奖鍙兘宸茶鏀惰棌锛?);</script>");
                }
                return;
            }
        }

        // 濡傛灉娌℃湁閫変腑鐢靛奖锛屾彁绀虹敤鎴?
        Response.Write("<script>alert('璇峰厛閫夋嫨涓€閮ㄧ數褰憋紒');</script>");
    }

    private bool AddFavorite(int userId, int movieId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // 妫€鏌ユ槸鍚﹀凡缁忔敹钘?
            string checkQuery = "SELECT COUNT(*) FROM Favorite WHERE Uno = @Uno AND Mno = @Mno";
            using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
            {
                checkCommand.Parameters.AddWithValue("@Uno", userId);
                checkCommand.Parameters.AddWithValue("@Mno", movieId);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                if (count > 0)
                {
                    return false; // 宸叉敹钘?
                }
            }

            // 娣诲姞鏀惰棌
            string insertQuery = "INSERT INTO Favorite (Uno, Mno, Type) VALUES (@Uno, @Mno, @Type)";
            using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection))
            {
                insertCommand.Parameters.AddWithValue("@Uno", userId);
                insertCommand.Parameters.AddWithValue("@Mno", movieId);
                insertCommand.Parameters.AddWithValue("@Type", "favorite"); // 鍥哄畾鏀惰棌绫诲瀷涓篺avorite

                int rowsAffected = insertCommand.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }

    protected void btnViewFavorites_Click(object sender, EventArgs e)
    {
        // 璺宠浆鍒版敹钘忛〉闈?
        Response.Redirect("../favorite_page/favorite.aspx");
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        // 璺宠浆鍒颁笅涓€椤?
        if (gvMovies.PageIndex < gvMovies.PageCount - 1)
        {
            gvMovies.PageIndex++;
            BindGridView();
        }
    }

    protected void lnkLast_Click(object sender, EventArgs e)
    {
        // 璺宠浆鍒版渶鍚庝竴椤?
        gvMovies.PageIndex = gvMovies.PageCount - 1;
        BindGridView();
    }

    protected void lnkChangePassword_Click(object sender, EventArgs e)
    {
        // 澶勭悊淇敼瀵嗙爜鍔熻兘
        Response.Redirect("~/changepwd_page/change_password.aspx");
    }

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        // 澶勭悊閫€鍑虹櫥褰曞姛鑳?
        // 娓呴櫎浼氳瘽
        Session.Clear();
        Session.Abandon();
        
        // 閲嶅畾鍚戝埌鐧诲綍椤甸潰
        Response.Redirect("~/start_page/login.html");
    }
}
}