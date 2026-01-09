using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class main : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindGridView();
        }
    }

    private void BindGridView()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MovieRatingConnection"].ConnectionString;
        string searchTerm = txtSearch.Text.Trim();
        string query = "SELECT MovieID, Title, Director, ReleaseYear, Genre, Rating FROM Movies";

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query += " WHERE Title LIKE @SearchTerm OR Director LIKE @SearchTerm OR Genre LIKE @SearchTerm";
        }

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                }

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                gvMovies.DataSource = dataTable;
                gvMovies.DataBind();
            }
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        // 重置分页索引
        gvMovies.PageIndex = 0;
        BindGridView();
    }

    protected void gvMovies_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        // 设置新的分页索引
        gvMovies.PageIndex = e.NewPageIndex;
        BindGridView();
    }
}
}