using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class change_password : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // 检查用户是否已登录
        if (Session["UserID"] == null)
        {
            Response.Redirect("~/start_page/login.html");
        }
        
        // 显示当前用户名
        if (Session["Username"] != null)
        {
            lblUsername.Text = Session["Username"].ToString();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string oldPassword = txtOldPassword.Text.Trim();
        string newPassword = txtNewPassword.Text.Trim();
        string confirmPassword = txtConfirmPassword.Text.Trim();

        // 验证输入
        if (string.IsNullOrEmpty(oldPassword))
        {
            lblMessage.Text = "请输入原密码";
            return;
        }

        if (string.IsNullOrEmpty(newPassword))
        {
            lblMessage.Text = "请输入新密码";
            return;
        }

        if (string.IsNullOrEmpty(confirmPassword))
        {
            lblMessage.Text = "请确认新密码";
            return;
        }

        if (newPassword != confirmPassword)
        {
            lblMessage.Text = "新密码和确认密码不匹配";
            return;
        }

        // 获取用户ID（假设已存储在Session中）
        int userId = Convert.ToInt32(Session["UserID"]);
        string connectionString = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // 检查原密码是否正确
                string checkPasswordQuery = "SELECT Uno FROM Users WHERE Uno = @UserID AND Upassword = @OldPassword";
                using (SqlCommand checkCommand = new SqlCommand(checkPasswordQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@UserID", userId);
                    checkCommand.Parameters.AddWithValue("@OldPassword", oldPassword);

                    object result = checkCommand.ExecuteScalar();
                    if (result == null)
                    {
                        lblMessage.Text = "原密码不正确";
                        return;
                    }
                }

                // 更新密码
                string updatePasswordQuery = "UPDATE Users SET Upassword = @NewPassword WHERE Uno = @UserID";
                using (SqlCommand updateCommand = new SqlCommand(updatePasswordQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@NewPassword", newPassword);
                    updateCommand.Parameters.AddWithValue("@UserID", userId);

                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        lblMessage.Text = "密码修改成功";
                        // 清空输入框
                        txtOldPassword.Text = "";
                        txtNewPassword.Text = "";
                        txtConfirmPassword.Text = "";
                    }
                    else
                    {
                        lblMessage.Text = "密码修改失败";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "发生错误：" + ex.Message;
        }
    }

    protected void lnkBack_Click(object sender, EventArgs e)
    {
        // 返回主页面
        Response.Redirect("~/main_page/main.aspx");
    }
}