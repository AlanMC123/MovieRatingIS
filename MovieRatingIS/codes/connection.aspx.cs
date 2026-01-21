using System;
using System.Data;
using MovieRatingIS.DAL;
public partial class codes_connection : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.HttpMethod == "POST")
        {
            string username = Request.Form["username"]; 
            string password = Request.Form["password"]; 
            UserDAL userDal = new UserDAL();
            DataTable userTable = userDal.GetUserByUsername(username);
            // 校验登录
            if (userTable.Rows.Count > 0) 
            {
                string dbPwd = userTable.Rows[0]["Upassword"].ToString().Trim(); // 去除char类型的尾随空格
                if (dbPwd == password) 
                {
                    // 登录成功：存用户名和UserID到Session，跳转到main.aspx
                    Session["Username"] = username;
                    Session["UserID"] = userTable.Rows[0]["Uno"].ToString();
                    Response.Redirect("~/main_page/main.aspx");
                    Response.End();
                }
                else
                {
                    // 密码错误，提示并返回登录页
                    Response.Write("<script>alert('密码错误！');window.location.href='../start_page/login.html';</script>");
                    Response.End();
                }
            }
            else
            {
                // 用户名不存在，提示并返回登录页
                Response.Write("<script>alert('用户名不存在！');window.location.href='../start_page/login.html';</script>");
                Response.End();
            }
        }
    }
}