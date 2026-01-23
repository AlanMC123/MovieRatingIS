<%@ Page Language="C#" AutoEventWireup="true" CodeFile="favorite.aspx.cs" Inherits="favorite_page_favorite" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Favorites - Movie Rating System</title>
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
            width: 80%;
            margin: 20px auto;
            padding: 20px;
            background: white;
            border-radius: 12px;
            box-shadow: 0 15px 25px rgba(0, 0, 0, 0.1);
        }
        
        .header-container {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 20px;
            flex-wrap: wrap;
            gap: 15px;
        }
        
        .user-info {
            margin-right: auto;
            font-weight: 600;
            color: #333;
        }
        
        .user-menu {
            display: flex;
            gap: 10px;
        }
        
        .user-menu a, .user-menu button {
            padding: 12px 15px;
            text-decoration: none;
            color: white;
            background-color: #4f66ca;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-size: 16px;
            font-weight: 500;
            transition: background-color 0.3s, transform 0.2s;
        }
        
        .user-menu a:hover, .user-menu button:hover {
            background-color: #4447a2;
            transform: translateY(-2px);
        }
        
        .grid-container {
            display: flex;
            justify-content: center;
        }
        
        .grid-view {
            width: 100%;
            border-collapse: collapse;
            background: white;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
        }
        
        .grid-view th,
        .grid-view td {
            padding: 12px;
            border: 1px solid #e0e0e0;
            text-align: left;
        }
        
        .grid-view th {
            background-color: #f8f9fa;
            color: #333;
            font-weight: 600;
        }
        
        .grid-view tr:nth-child(even) {
            background-color: #f8f9fa;
        }
        
        .grid-view tr:hover {
            background-color: #e9ecfd;
        }
        
        .btn-back {
            background-color: #4f66ca;
            color: white;
            border: none;
            padding: 12px 16px;
            border-radius: 6px;
            cursor: pointer;
            font-size: 16px;
            transition: background-color 0.3s, transform 0.2s;
        }
        
        .btn-back:hover {
            background-color: #4447a2;
            transform: translateY(-2px);
        }
        
        .btn-remove {
            background-color: #f44336;
            color: white;
            border: none;
            padding: 8px 12px;
            border-radius: 6px;
            cursor: pointer;
            font-size: 14px;
            transition: background-color 0.3s, transform 0.2s;
        }
        
        .btn-remove:hover {
            background-color: #d32f2f;
            transform: translateY(-2px);
        }
        
        .empty-message {
            text-align: center;
            color: #666;
            padding: 40px;
            font-size: 16px;
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
                <h2>My Favorites</h2>
                <div class="user-info">
                    Welcome, <asp:Label ID="lblUsername" runat="server"></asp:Label>
                </div>
                <div class="user-menu">
                    <asp:Button ID="btnBack" runat="server" Text="Back to Movie List" OnClick="btnBack_Click" CssClass="btn-back" />
                    <asp:LinkButton ID="lnkLogout" runat="server" OnClick="lnkLogout_Click">Logout</asp:LinkButton>
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
                        <asp:BoundField DataField="Title" HeaderText="Title" />
                        <asp:BoundField DataField="Genre" HeaderText="Genre" />
                        <asp:BoundField DataField="ReleaseYear" HeaderText="Release Year" />
                        <asp:BoundField DataField="LastingTime" HeaderText="Duration" />
                        <asp:BoundField DataField="AvgRating" HeaderText="Rating" DataFormatString="{0:F1}" NullDisplayText="暂无评分" />
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <asp:Button ID="btnRemove" runat="server" 
                                    Text="Remove from Favorites" 
                                    CommandName="RemoveFavorite" 
                                    CommandArgument='<%# Eval("MovieID") %>' 
                                    CssClass="btn-remove" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:Label ID="lblEmptyMessage" runat="server" Text="No favorite movies yet" CssClass="empty-message" Visible="False"></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>