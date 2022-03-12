using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI.HtmlControls;

namespace reading_management_system
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected String myConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ReadingDBConnectionString"].ConnectionString;
        protected DataTable listsTable = new DataTable();
        protected DataTable genresTable = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Load_Top_Lists();
                Load_Top_Genres();
            }
        }

        protected void Load_Top_Lists()
        {
            String selectListsCommand = "SELECT TOP(5) ReadingList.ReadingListName, COUNT(ReadingListAffiliations.FK_BookID) AS BookCount" +
                    " FROM ReadingList" +
                    " INNER JOIN ReadingListAffiliations ON ReadingList.ReadingListID = ReadingListAffiliations.FK_ReadingListID" +
                    " GROUP BY ReadingList.ReadingListName" +
                    " ORDER BY BookCount DESC";

            using (SqlConnection myConnection = new SqlConnection(myConnectionString))
            {
                using (SqlCommand myCommand = new SqlCommand(selectListsCommand, myConnection))
                {
                    SqlDataAdapter listsReader = new SqlDataAdapter(myCommand);

                    DataSet listsDS = new DataSet();
                    listsReader.Fill(listsDS);

                    listsTable = listsDS.Tables[0].Copy();

                    // Default html anchors
                    HtmlAnchor[] defaultControls = { BrowseListItem01, BrowseListItem02, BrowseListItem03, BrowseListItem04, BrowseListItem05 };

                    // If the table does not have any rows, we will populate the links with a default value
                    if (listsTable.Rows.Count == 0)
                    {
                        for (int i = 0; i < defaultControls.Length; i++)
                        {
                            defaultControls[i].InnerHtml = "Not Enough Data";
                        }
                    }
                    // If the table has some rows but not 5, we will populate what we can, then populate the rest with a default value
                    else if (listsTable.Rows.Count < 5)
                    {
                        // Add the ones that can be added
                        int nextToAdd = 0;
                        for (int i = 0; i < listsTable.Rows.Count; i++)
                        {
                            defaultControls[i].InnerHtml = (String)listsTable.Rows[i]["ReadingListName"];
                            nextToAdd = i + 1;
                        }

                        // Add defaults for the others
                        for (int i = nextToAdd; i < defaultControls.Length; i++)
                        {
                            defaultControls[i].InnerHtml = "Not Enough Data";
                        }
                    }
                    // If the table has 5 or more rows, just use those rows
                    else
                    {
                        for (int i = 0; i < defaultControls.Length; i++)
                        {
                            defaultControls[i].InnerHtml = (String)listsTable.Rows[i]["ReadingListName"];
                        }
                    }
                }
            }
        }

        protected void Load_Top_Genres()
        {
            String selectGenresCommand = "SELECT Genre.GenreName, COUNT(GenreAffiliations.FK_BookID) AS BookCount" +
                    " FROM Genre" +
                    " INNER JOIN GenreAffiliations ON Genre.GenreID = GenreAffiliations.FK_GenreID" +
                    " GROUP BY Genre.GenreName HAVING(NOT (Genre.GenreName = 'Non Fiction')) AND(NOT(Genre.GenreName = 'Fiction'))" +
                    " ORDER BY BookCount DESC";

            using (SqlConnection myConnection = new SqlConnection(myConnectionString))
            {
                using (SqlCommand myCommand = new SqlCommand(selectGenresCommand, myConnection))
                {
                    SqlDataAdapter listsReader = new SqlDataAdapter(myCommand);

                    DataSet listsDS = new DataSet();
                    listsReader.Fill(listsDS);

                    genresTable = listsDS.Tables[0].Copy();

                    BrowseGenreItem01.InnerHtml = (String)genresTable.Rows[0]["GenreName"];
                    BrowseGenreItem02.InnerHtml = (String)genresTable.Rows[1]["GenreName"];
                    BrowseGenreItem03.InnerHtml = (String)genresTable.Rows[2]["GenreName"];
                    BrowseGenreItem04.InnerHtml = (String)genresTable.Rows[3]["GenreName"];
                    BrowseGenreItem05.InnerHtml = (String)genresTable.Rows[4]["GenreName"];
                }
            }
        }
    }
}