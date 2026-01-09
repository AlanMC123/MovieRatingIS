using System;
using System.Web.UI;

public partial class test : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            lblMessage.Text = "测试页面加载成功！";
        }
    }
}