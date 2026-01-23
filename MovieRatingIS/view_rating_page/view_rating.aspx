<%@ Page Language="C#" AutoEventWireup="true" CodeFile="view_rating.aspx.cs" Inherits="MovieRatingIS.view_rating" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Movie Ratings - Movie Rating System</title>
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
        
        .btn-back {
            background-color: #4f66ca;
            color: white;
            border: none;
            padding: 12px 15px;
            border-radius: 6px;
            cursor: pointer;
            margin-bottom: 20px;
            text-decoration: none;
            display: inline-block;
            font-size: 16px;
            font-weight: 500;
            transition: background-color 0.3s, transform 0.2s;
        }
        
        .btn-back:hover {
            background-color: #4447a2;
            transform: translateY(-2px);
        }
        
        .movie-info {
            background-color: #f8f9fa;
            padding: 20px;
            border-radius: 12px;
            margin-bottom: 20px;
            border: 1px solid #e0e0e0;
        }
        
        .movie-title {
            font-size: 24px;
            font-weight: 600;
            margin-bottom: 10px;
            color: #333;
        }
        
        .movie-details {
            font-size: 16px;
            color: #666;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <h1>Movie Rating System</h1>
        </div>
        
        <div class="container">
            <asp:HyperLink ID="lnkBack" runat="server" Text="Back to Movie List" CssClass="btn-back" NavigateUrl="~/main_page/main.aspx"></asp:HyperLink>
            
            <div class="movie-info">
                <div class="movie-title">
                    <asp:Label ID="lblMovieTitle" runat="server" Text=""></asp:Label>
                </div>
                <div class="movie-details">
                    <asp:Label ID="lblMovieID" runat="server" Text=""></asp:Label>
                </div>
            </div>
            
            <h2>Ratings and Comments</h2>
            
            <asp:GridView ID="gvRatings" runat="server" 
                AutoGenerateColumns="False" 
                CssClass="grid-view"
                AllowPaging="True"
                PageSize="10"
                OnPageIndexChanging="gvRatings_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="Time" HeaderText="Time" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                    <asp:BoundField DataField="Rating" HeaderText="Rating" DataFormatString="{0:F1}" />
                    <asp:BoundField DataField="Comment" HeaderText="Comment" />
                </Columns>
                <PagerStyle HorizontalAlign="Center" ForeColor="#333" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>