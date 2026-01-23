<%@ Page Language="C#" AutoEventWireup="true" CodeFile="main.aspx.cs" Inherits="MovieRatingIS.main" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Movie Rating System - Home</title>
    <style>
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
        
        .search-container {
            margin-bottom: 20px;
            margin: 15px 0;
            display: flex;
            gap: 10px;
            align-items: center;
            flex-wrap: wrap;
        }
        
        .search-container input {
            padding: 12px 15px;
            border: 1px solid #ddd;
            border-radius: 6px;
            outline: none;
            font-size: 16px;
            transition: all 0.3s ease;
            flex: 1;
            min-width: 200px;
        }
        
        .search-container input:focus {
            border-color: #9face6;
            box-shadow: 0 0 8px rgba(159, 172, 230, 0.3);
        }
        
        .search-container button {
            background-color: #4f66ca;
            color: white;
            border: none;
            padding: 12px 15px;
            border-radius: 6px;
            cursor: pointer;
            font-size: 16px;
            font-weight: 500;
            transition: background-color 0.3s, transform 0.2s;
        }
        
        .search-container button:hover {
            background-color: #4447a2;
            transform: translateY(-2px);
        }
        
        .grid-view {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
            background: white;
            border-radius: 8px;
            overflow: hidden;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.05);
        }
        
        .grid-view th, .grid-view td {
            border: 1px solid #e0e0e0;
            padding: 12px;
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
        
        .pager {
            margin-top: 15px;
            text-align: center;
        }
        
        .pager a, .pager span {
            margin-right: 8px;
            text-decoration: none;
            color: #333;
            padding: 8px 12px;
            border-radius: 4px;
            transition: all 0.3s ease;
        }
        
        .pager a:hover {
            background-color: #e9ecfd;
            color: #4f66ca;
        }
        
        .pager .disabled {
            color: #ccc !important;
            pointer-events: none;
            cursor: default;
        }
        
        .username {
            position: absolute;
            right: 20px;
            top: 50%;
            transform: translateY(-50%);
            display: flex;
            align-items: center;
            gap: 15px;
        }
        
        .link-button {
            color: #4f66ca;
            text-decoration: none;
            font-weight: 500;
            transition: color 0.3s ease;
        }
        
        .link-button:hover {
            color: #4447a2;
        }
    </style>
    <script type="text/javascript">
        function SelectSingleRadioButton(rdbtn) {
            // 1. 获取 GridView 控件
            var grid = document.getElementById('<%= gvMovies.ClientID %>');

            // 2. 获取 GridView 内所有的 input 元素
            var inputs = grid.getElementsByTagName("input");

            // 3. 遍历并取消选中其他单选框
            for (var i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "radio") {
                    if (inputs[i] != rdbtn) {
                        inputs[i].checked = false;
                    }
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <h1>Movie Rating System</h1>
            <div class="username">
                Welcome, <asp:Label ID="lblUsername" runat="server" Text=""></asp:Label>
                <asp:LinkButton ID="lnkChangePassword" runat="server" OnClick="lnkChangePassword_Click" Style="margin-left: 15px; padding: 8px 12px; text-decoration: none; color: #4f66ca; font-weight: 500; transition: color 0.3s ease;">Change Password</asp:LinkButton>
                <asp:LinkButton ID="lnkLogout" runat="server" OnClick="lnkLogout_Click" Style="margin-left: 15px; padding: 8px 12px; text-decoration: none; color: #4f66ca; font-weight: 500; transition: color 0.3s ease;">Logout</asp:LinkButton>
            </div>
        </div>
        
        <div class="container">
            <div class="search-container">
                <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" />
      
                <asp:Button ID="btnRateMovie" runat="server" Text="Rate Selected" OnClick="btnRateMovie_Click" 
                    Style="margin-left: 10px; background-color: #4CAF50; color: white; border: none; padding: 5px 15px; border-radius: 3px; cursor: pointer;" />
                <asp:Button ID="btnFavorite" runat="server" Text="Add to Favorites" OnClick="btnFavorite_Click" 
                    Style="margin-left: 10px; background-color: #2196F3; color: white; border: none; padding: 5px 15px; border-radius: 3px; cursor: pointer;" />
                <asp:Button ID="btnViewFavorites" runat="server" Text="My Favorites" OnClick="btnViewFavorites_Click" 
                    Style="margin-left: 10px; background-color: #FF9800; color: white; border: none; padding: 5px 15px; border-radius: 3px; cursor: pointer;" />
                <asp:Button ID="btnViewRatings" runat="server" Text="View Ratings" OnClick="btnViewRatings_Click" 
                    Style="margin-left: 10px; background-color: #9C27B0; color: white; border: none; padding: 5px 15px; border-radius: 3px; cursor: pointer;" />
            </div>
            
            <div class="grid-container">
                <asp:GridView ID="gvMovies" runat="server" 
                    AutoGenerateColumns="False" 
                    CssClass="grid-view"
                    AllowPaging="True"
                    PageSize="5"
                    OnPageIndexChanging="gvMovies_PageIndexChanging"
                    DataKeyNames="MovieID">
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <ItemTemplate>
                                <asp:RadioButton ID="rbSelectMovie" runat="server" onclick="SelectSingleRadioButton(this)" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="MovieID" HeaderText="Movie ID" Visible="False" />
                        <asp:BoundField DataField="Title" HeaderText="Title" />
                        <asp:BoundField DataField="Genre" HeaderText="Genre" />
                        <asp:BoundField DataField="ReleaseYear" HeaderText="Release Year" />
                        <%-- 1. 将原 Rating 改名为时�?--%>
                        <asp:BoundField DataField="Duration" HeaderText="Duration" />
                        <%-- 2. 新增平均评分�?--%>
                        <asp:BoundField DataField="AvgRating" HeaderText="Rating" DataFormatString="{0:F1}" />
                    </Columns>
                    <PagerSettings Visible="False" />
                </asp:GridView>

                <div class="pager">
                    <asp:LinkButton ID="lnkFirst" runat="server" OnClick="lnkFirst_Click">First</asp:LinkButton>
                    <asp:LinkButton ID="lnkPrev" runat="server" OnClick="lnkPrev_Click">Prev</asp:LinkButton>
                    <span>Page <asp:Label ID="lblCurrentPage" runat="server" Text="1"></asp:Label> of <asp:Label ID="lblTotalPages" runat="server" Text="1"></asp:Label></span>
                    <asp:LinkButton ID="lnkNext" runat="server" OnClick="lnkNext_Click">Next</asp:LinkButton>
                    <asp:LinkButton ID="lnkLast" runat="server" OnClick="lnkLast_Click">Last</asp:LinkButton>
                </div>
            </div>
        </div>
    </form>
</body>
</html>