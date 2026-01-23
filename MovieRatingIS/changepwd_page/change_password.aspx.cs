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

        // Validate input
        if (string.IsNullOrEmpty(oldPassword))
        {
            lblMessage.Text = "Please enter your old password";
            return;
        }

        if (string.IsNullOrEmpty(newPassword))
        {
            lblMessage.Text = "Please enter your new password";
            return;
        }

        if (string.IsNullOrEmpty(confirmPassword))
        {
            lblMessage.Text = "Please confirm your new password";
            return;
        }

        if (newPassword != confirmPassword)
        {
            lblMessage.Text = "New password and confirmation password do not match";
            return;
        }

        // Get user ID (assuming it's stored in Session)
        string userId = Session["UserID"].ToString();
        string connectionString = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;

        try
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Check if old password is correct
                string checkPasswordQuery = "SELECT Uno FROM Users WHERE Uno = @UserID AND Upassword = @OldPassword";
                using (SqlCommand checkCommand = new SqlCommand(checkPasswordQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@UserID", userId);
                    checkCommand.Parameters.AddWithValue("@OldPassword", oldPassword);

                    object result = checkCommand.ExecuteScalar();
                    if (result == null)
                    {
                        lblMessage.Text = "Old password is incorrect";
                        return;
                    }
                }

                // Update password
                string updatePasswordQuery = "UPDATE Users SET Upassword = @NewPassword WHERE Uno = @UserID";
                using (SqlCommand updateCommand = new SqlCommand(updatePasswordQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@NewPassword", newPassword);
                    updateCommand.Parameters.AddWithValue("@UserID", userId);

                    int rowsAffected = updateCommand.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        lblMessage.Text = "Password changed successfully";
                        // Clear input fields
                        txtOldPassword.Text = "";
                        txtNewPassword.Text = "";
                        txtConfirmPassword.Text = "";
                    }
                    else
                    {
                        lblMessage.Text = "Failed to change password";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error: " + ex.Message;
        }
    }

    protected void lnkBack_Click(object sender, EventArgs e)
    {
        // Return to main page
        Response.Redirect("~/main_page/main.aspx");
    }
}