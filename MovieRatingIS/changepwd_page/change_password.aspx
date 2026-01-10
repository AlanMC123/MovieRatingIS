<%@ Page Language="C#" AutoEventWireup="true" CodeFile="change_password.aspx.cs" Inherits="change_password" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>修改密码</title>
    <style type="text/css">
        .container {
            width: 50%;
            margin: 0 auto;
            padding: 20px;
            border: 1px solid #ccc;
            border-radius: 5px;
            margin-top: 50px;
        }
        .form-group {
            margin-bottom: 15px;
        }
        .form-group label {
            display: block;
            margin-bottom: 5px;
        }
        .form-group input {
            width: 100%;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 3px;
        }
        .btn-submit {
            background-color: #4CAF50;
            color: white;
            padding: 10px 15px;
            border: none;
            border-radius: 3px;
            cursor: pointer;
        }
        .btn-submit:hover {
            background-color: #45a049;
        }
        .message {
            color: red;
            margin-bottom: 15px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="header-container">
                <h1>修改密码</h1>
                <div class="user-info">
                    欢迎，<asp:Label ID="lblUsername" runat="server"></asp:Label>
                </div>
            </div>
            <asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>
            
            <div class="form-group">
                <label for="txtOldPassword">原密码：</label>
                <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <label for="txtNewPassword">新密码：</label>
                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <label for="txtConfirmPassword">确认新密码：</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
            </div>
            
            <asp:Button ID="btnSubmit" runat="server" Text="提交" OnClick="btnSubmit_Click" CssClass="btn-submit" />
            <asp:LinkButton ID="lnkBack" runat="server" OnClick="lnkBack_Click">返回主页面</asp:LinkButton>
        </div>
    </form>
</body>
</html>