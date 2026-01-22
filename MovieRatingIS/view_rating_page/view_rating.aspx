<%@ Page Language="C#" AutoEventWireup="true" CodeFile="view_rating.aspx.cs" Inherits="MovieRatingIS.view_rating" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Movie Ratings - Movie Rating System</title>
    <style>
        body { font-family: Arial, sans-serif; background-color: #f4f4f4; margin: 0; padding: 0; }
        .header { 
            background-color: #333; 
            color: white;
            padding: 10px 0; 
            text-align: center; 
            position: relative;
        }
        .container { width: 80%; margin: 0 auto; padding: 20px; }
        .grid-view { width: 100%; border-collapse: collapse; margin-top: 20px; }
        .grid-view th, .grid-view td { border: 1px solid #ddd; padding: 12px; text-align: left; }
        .grid-view th { background-color: #333; color: white; }
        .grid-view tr:nth-child(even) { background-color: #f2f2f2; }
        .grid-view tr:hover { background-color: #ddd; }
        .btn-back { background-color: #4CAF50; color: white; border: none; padding: 8px 15px; border-radius: 3px; cursor: pointer; margin-bottom: 20px; text-decoration: none; display: inline-block; }
        .btn-back:hover { background-color: #45a049; }
        .movie-info { background-color: white; padding: 20px; border-radius: 5px; margin-bottom: 20px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }
        .movie-title { font-size: 24px; font-weight: bold; margin-bottom: 10px; }
        .movie-details { font-size: 16px; color: #666; }
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