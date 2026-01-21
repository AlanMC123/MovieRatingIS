<%@ Page Language="C#" AutoEventWireup="true" CodeFile="favorite.aspx.cs" Inherits="favorite_page_favorite" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>我的收藏 - 电影评分系统</title>
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
        .user-menu a, .user-menu button {
            padding: 5px 10px;
            text-decoration: none;
            color: #333;
            border: 1px solid #ccc;
            border-radius: 3px;
            background-color: #f2f2f2;
            cursor: pointer;
        }
        .user-menu a:hover, .user-menu button:hover {
            background-color: #e9e9e9;
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
        .btn-back {
            background-color: #FF9800;
            color: white;
            border: none;
            padding: 8px 16px;
            border-radius: 3px;
            cursor: pointer;
            font-size: 14px;
        }
        .btn-back:hover {
            background-color: #F57C00;
        }
        .btn-remove {
            background-color: #f44336;
            color: white;
            border: none;
            padding: 5px 10px;
            border-radius: 3px;
            cursor: pointer;
            font-size: 12px;
        }
        .btn-remove:hover {
            background-color: #d32f2f;
        }
        .empty-message {
            text-align: center;
            color: #666;
            padding: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="header-container">
                <h1>我的收藏</h1>
                <div class="user-info">
                    欢迎，<asp:Label ID="lblUsername" runat="server"></asp:Label>
                </div>
                <div class="user-menu">
                    <asp:Button ID="btnBack" runat="server" Text="返回电影列表" OnClick="btnBack_Click" CssClass="btn-back" />
                    <asp:LinkButton ID="lnkLogout" runat="server" OnClick="lnkLogout_Click">退出登录</asp:LinkButton>
                </div>
            </div>

            <div class="grid-container">
                <asp:GridView ID="gvFavorites" runat="server" 
                    AutoGenerateColumns="False" 
                    CssClass="grid-view"
                    DataKeyNames="MovieID"
                    OnRowCommand="gvFavorites_RowCommand">
                    
                    <Columns>
                        <asp:BoundField DataField="MovieID" HeaderText="ID" />
                        <asp:BoundField DataField="Title" HeaderText="电影名称" />
                        <asp:BoundField DataField="Genre" HeaderText="类型" />
                        <asp:BoundField DataField="ReleaseYear" HeaderText="上映年份" />
                        <asp:BoundField DataField="LastingTime" HeaderText="时长" />
                        <asp:BoundField DataField="Distributor" HeaderText="发行商" />
                        <asp:BoundField DataField="Rating" HeaderText="评分" DataFormatString="{0:N1}" />
                        <asp:BoundField DataField="FavoriteTime" HeaderText="收藏时间" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                        <asp:TemplateField HeaderText="操作">
                            <ItemTemplate>
                                <asp:Button ID="btnRemove" runat="server" Text="取消收藏" 
                                    CommandName="RemoveFavorite" 
                                    CommandArgument='<%# Eval("MovieID") %>'
                                    CssClass="btn-remove" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:Label ID="lblEmptyMessage" runat="server" Text="暂无收藏的电影" CssClass="empty-message" Visible="False"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>