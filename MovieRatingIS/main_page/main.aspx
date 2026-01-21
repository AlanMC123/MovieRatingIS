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
        .header-container {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
        }
        .user-info {
            margin-right: auto;
            font-weight: bold;
        }
        .user-menu {
            display: flex;
            gap: 15px;
        }
        .user-menu a {
            padding: 5px 10px;
            text-decoration: none;
            color: #333;
            border: 1px solid #ccc;
            border-radius: 3px;
            background-color: #f2f2f2;
        }
        .user-menu a:hover {
            background-color: #e9e9e9;
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
            <div class="header-container">
                <h1>电影评分系统</h1>
                <div class="user-info">
                    欢迎，<asp:Label ID="lblUsername" runat="server"></asp:Label>
                </div>
                <div class="user-menu">
                    <asp:LinkButton ID="lnkChangePassword" runat="server" OnClick="lnkChangePassword_Click">修改密码</asp:LinkButton>
                    <asp:LinkButton ID="lnkLogout" runat="server" OnClick="lnkLogout_Click">退出登录</asp:LinkButton>
                </div>
            </div>
            
            <div class="search-container">
                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="搜索" OnClick="btnSearch_Click" />
                <asp:Button ID="btnRateMovie" runat="server" Text="评价所选电影" OnClick="btnRateMovie_Click" 
                    Style="margin-left: 10px; background-color: #4CAF50; color: white; border: none; padding: 5px 15px; border-radius: 3px; cursor: pointer;" />
                <asp:Button ID="btnFavorite" runat="server" Text="收藏所选电影" OnClick="btnFavorite_Click" 
                    Style="margin-left: 10px; background-color: #2196F3; color: white; border: none; padding: 5px 15px; border-radius: 3px; cursor: pointer;" />
                <asp:Button ID="btnViewFavorites" runat="server" Text="我的收藏" OnClick="btnViewFavorites_Click" 
                    Style="margin-left: 10px; background-color: #FF9800; color: white; border: none; padding: 5px 15px; border-radius: 3px; cursor: pointer;" />
            </div>
            
            <div class="grid-container">
                <asp:GridView ID="gvMovies" runat="server" 
                    AutoGenerateColumns="False" 
                    CssClass="grid-view"
                    AllowPaging="True" 
                    PageSize="10"
                    OnPageIndexChanging="gvMovies_PageIndexChanging"
                    DataKeyNames="MovieID">
                    
                    <Columns>
                        <asp:TemplateField HeaderText="选择">
                            <ItemTemplate>
                                <asp:RadioButton ID="rbSelectMovie" runat="server" GroupName="SelectMovie" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="MovieID" HeaderText="ID" />
                        <asp:BoundField DataField="Title" HeaderText="电影名称" />
                        <asp:BoundField DataField="Genre" HeaderText="类型" />
                        <asp:BoundField DataField="ReleaseYear" HeaderText="上映年份" />
                        <asp:BoundField DataField="LastingTime" HeaderText="时长" />
                        <asp:BoundField DataField="Distributor" HeaderText="发行商" />
                        <asp:BoundField DataField="Rating" HeaderText="评分" DataFormatString="{0:N1}" />
                    </Columns>

                    <PagerStyle Visible="false" />
                </asp:GridView>

                <div class="pager">
                    <asp:LinkButton ID="lnkFirst" runat="server" OnClick="lnkFirst_Click">首页</asp:LinkButton>
                    <asp:LinkButton ID="lnkPrev" runat="server" OnClick="lnkPrev_Click">上一页</asp:LinkButton>
                    <span>第 <asp:Label ID="lblCurrentPage" runat="server"></asp:Label> 页 / 共 <asp:Label ID="lblTotalPages" runat="server"></asp:Label> 页</span>
                    <asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click">下一页</asp:LinkButton>
                    <asp:LinkButton ID="lnkLast" runat="server" OnClick="lnkLast_Click">末页</asp:LinkButton>
                </div>
</div>
        </div>
    </form>
</body>
</html>