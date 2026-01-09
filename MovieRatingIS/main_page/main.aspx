<%@ Page Language="C#" AutoEventWireup="true" CodeFile="main.aspx.cs" Inherits="main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>电影评分系统</title>
    <style type="text/css">
        .container {
            width: 80%;
            margin: 0 auto;
            padding: 20px;
        }
        .search-container {
            text-align: right;
            margin-bottom: 20px;
        }
        .grid-container {
            display: flex;
            justify-content: center;
        }
        .grid-view {
            width: 100%;
            border-collapse: collapse;
        }
        .grid-view th,
        .grid-view td {
            padding: 10px;
            border: 1px solid #ccc;
            text-align: left;
        }
        .grid-view th {
            background-color: #f2f2f2;
            font-weight: bold;
        }
        .grid-view tr:nth-child(even) {
            background-color: #f9f9f9;
        }
        .grid-view tr:hover {
            background-color: #e9e9e9;
        }
        .pager {
            text-align: center;
            margin-top: 20px;
        }
        .pager a {
            margin: 0 5px;
            padding: 5px 10px;
            background-color: #f2f2f2;
            border: 1px solid #ccc;
            text-decoration: none;
            color: #333;
        }
        .pager a:hover {
            background-color: #e9e9e9;
        }
        .pager span {
            margin: 0 5px;
            padding: 5px 10px;
            background-color: #ddd;
            border: 1px solid #ccc;
            color: #333;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h1>电影评分系统</h1>
            
            <div class="search-container">
                <asp:TextBox ID="txtSearch" runat="server" placeholder="输入搜索关键词"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" />
            </div>
            
            <div class="grid-container">
                ds</div>
        </div>
    </form>
</body>
</html>