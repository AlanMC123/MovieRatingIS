<%@ Page Language="C#" AutoEventWireup="true" CodeFile="change_password.aspx.cs" Inherits="change_password" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change Password</title>
    <style type="text/css">
        * {
            box-sizing: border-box;
        }
        
        body {
            background: linear-gradient(135deg, #74ebd5 0%, #9face6 100%);
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            margin: 0;
            padding: 0;
            min-height: 100vh;
        }
        
        .header {
            background-color: white;
            color: #333;
            padding: 15px 0;
            text-align: center;
            position: relative;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
        }
        
        .container {
            width: 50%;
            margin: 20px auto;
            padding: 20px;
            background: white;
            border-radius: 12px;
            box-shadow: 0 15px 25px rgba(0, 0, 0, 0.1);
        }
        
        .header-container {
            margin-bottom: 20px;
        }
        
        .header-container h1 {
            color: #333;
            text-align: center;
            margin-bottom: 10px;
        }
        
        .user-info {
            text-align: center;
            font-weight: 600;
            color: #333;
            margin-bottom: 20px;
        }
        
        .form-group {
            margin-bottom: 20px;
        }
        
        .form-group label {
            display: block;
            margin-bottom: 8px;
            font-weight: 500;
            color: #333;
        }
        
        .form-group input {
            width: 100%;
            padding: 12px 15px;
            border: 1px solid #ddd;
            border-radius: 6px;
            outline: none;
            font-size: 16px;
            transition: all 0.3s ease;
        }
        
        .form-group input:focus {
            border-color: #9face6;
            box-shadow: 0 0 8px rgba(159, 172, 230, 0.3);
        }
        
        .btn-submit {
            width: 75%;
            padding: 12px;
            margin: 20px auto;
            display: block;
            background-color: #4f66ca;
            color: white;
            border: none;
            border-radius: 6px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
            transition: background-color 0.3s, transform 0.2s;
        }
        
        .btn-submit:hover {
            background-color: #4447a2;
            transform: translateY(-2px);
        }
        
        .message {
            color: #f44336;
            margin-bottom: 15px;
            text-align: center;
            font-weight: 500;
        }
        
        .link-back {
            display: block;
            text-align: center;
            margin-top: 10px;
            color: #4f66ca;
            text-decoration: none;
            font-weight: 500;
            transition: color 0.3s ease;
        }
        
        .link-back:hover {
            color: #4447a2;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <h1>Movie Rating System</h1>
        </div>
        
        <div class="container">
            <div class="header-container">
                <h1>Change Password</h1>
                <div class="user-info">
                    Welcome, <asp:Label ID="lblUsername" runat="server"></asp:Label>
                </div>
            </div>
            <asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>
            
            <div class="form-group">
                <label for="txtOldPassword">Old Password:</label>
                <asp:TextBox ID="txtOldPassword" runat="server" TextMode="Password"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <label for="txtNewPassword">New Password:</label>
                <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password"></asp:TextBox>
            </div>
            
            <div class="form-group">
                <label for="txtConfirmPassword">Confirm New Password:</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
            </div>
            
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="btn-submit" />
            <asp:LinkButton ID="lnkBack" runat="server" OnClick="lnkBack_Click" CssClass="link-back">Back to Main Page</asp:LinkButton>
        </div>
    </form>
</body>
</html>