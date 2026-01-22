using System;
using System.Data;
using MovieRatingIS.DAL;
using MovieRatingIS.Model;

public partial class codes_register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.HttpMethod == "POST")
        {
            string username = Request.Form["username"];
            string genderInput = Request.Form["gender"];
            string gender = "男"; // 设置默认值
            if (genderInput == "female")
            {
                gender = "女";
            }
            else if (genderInput == "other")
            {
                gender = "密"; // 或者是数据库允许的其他短字符
            }
            else
            {
                gender = "男";
            }
            string tele = Request.Form["tele"];
            string password = Request.Form["password"];

            User user = new User();
            user.Username = username;
            user.Password = password;
            user.Gender = gender;
            user.Tele = tele;

            UserDAL userDal = new UserDAL();
            DataTable checkUser = userDal.GetUserByUsername(username);
            if (checkUser.Rows.Count > 0)
            {
                // 用户名已存在
                Response.Write("<script>alert('用户名已被注册！');window.location.href='/start_page/register.html';</script>");
                return;
            }

            // 用户注册
            bool isSuccess = userDal.AddUser(user);
            if (isSuccess)
            {
                // 注册成功，跳转到登录页
                Response.Write("<script>alert('注册成功！请登录');window.location.href='/start_page/login.html';</script>");
            }
            else
            {
                // 注册失败
                Response.Write("<script>alert('注册失败！请重试');window.location.href='/start_page/register.html';</script>");
            }
        }
    }
}