using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace reading_management_system.Pages
{
    public partial class reading_lists : System.Web.UI.Page
    {
        // Instance variables
        protected static String myConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ReadingDBConnectionString"].ConnectionString;
        protected DataSet modifiedDataSet = new DataSet();

        /**
         * When the page is loaded, we want to populate the table based on the selected value
         * in the drop down list. By default, the drop down list should have the selected
         * value as -1.
         */
        protected void Page_Load(object sender, EventArgs e)
        {
            // When the page is loaded, load the table with values that need to be shown
            if (readingListDDL.SelectedValue == "-1")
            {
                readingListSelectedGV.Visible = false;
                readingListDefaultGV.Visible = true;

                // Make sure the feedback literal is not shown
                newListFeedbackLiteral.Text = "";
            }
            else if (readingListDDL.SelectedValue == "0")
            {
                // If the textbox is empty, then just display the lists that are currently in the database
                if (newReadingListTextBox.Text.Length == 0)
                {
                    readingListSelectedGV.Visible = false;
                    readingListDefaultGV.Visible = true;

                    // Make sure the feedback literal provides feeback
                    newListFeedbackLiteral.Text = "<small class='error-small'>Enter the Name of Your New List</small>";
                }
                else
                {
                    // Add the new list to the database
                    String insertNewListCommand = "INSERT INTO ReadingList (ReadingListName) VALUES (@listItem)";
                    using (SqlConnection myConnection = new SqlConnection(myConnectionString))
                    {
                        using (SqlCommand myCommand = new SqlCommand(insertNewListCommand, myConnection))
                        {
                            // Add the parameters
                            myCommand.Parameters.AddWithValue("listItem", newReadingListTextBox.Text);

                            // Open the connection
                            myConnection.Open();

                            // Execute the command
                            int rowsAffected = myCommand.ExecuteNonQuery();

                            // Close the connection
                            myConnection.Close();

                            // Set the feedback literal based on the rows affected
                            if (rowsAffected == 0)
                            {
                                newListFeedbackLiteral.Text = "<small class='error-small'>Insertion Unsuccessful</small>";
                            }
                            else
                            {
                                newListFeedbackLiteral.Text = "<small class='success-small'>Insertion Successful</small>";
                            }
                        }
                    }

                    // Display the updated reading lists
                    readingListSelectedGV.Visible = false;
                    readingListDefaultGV.Visible = true;
                }
            }
            else
            {
                // Select command
                String selectCommand = "SELECT Book.BookID AS ID, Book.BookISBN AS ISBN, Book.BookName AS Title, Author.AuthorName AS Authors, Genre.GenreName AS Genres, Book.BookSummary AS Summary, Book.BookPageCount AS PageCount" +
                    " FROM Author" +
                    " INNER JOIN AuthorAffiliations ON Author.AuthorID = AuthorAffiliations.FK_AuthorID" +
                    " INNER JOIN Book ON AuthorAffiliations.FK_BookID = Book.BookID" +
                    " INNER JOIN GenreAffiliations ON Book.BookID = GenreAffiliations.FK_BookID" +
                    " INNER JOIN Genre ON GenreAffiliations.FK_GenreID = Genre.GenreID" +
                    " INNER JOIN ReadingListAffiliations ON Book.BookID = ReadingListAffiliations.FK_BookID" +
                    " INNER JOIN ReadingList ON ReadingListAffiliations.FK_ReadingListID = ReadingList.ReadingListID" +
                    " WHERE(ReadingList.ReadingListID = @selectedList)";

                // Initialize the sql connection using the connection string
                using (SqlConnection myConnection = new SqlConnection(myConnectionString))
                {
                    // Initialize the sql command using the selectCommand and the connection
                    using (SqlCommand myCommand = new SqlCommand(selectCommand, myConnection))
                    {
                        // Add the select parameters
                        myCommand.Parameters.AddWithValue("selectedList", readingListDDL.SelectedValue);

                        // Construct an SqlAdapter
                        SqlDataAdapter originalDataReader = new SqlDataAdapter(myCommand);

                        // Construct a DataSet from the sqldataadapter and fill it
                        DataSet originalData = new DataSet();
                        originalDataReader.Fill(originalData);

                        // Construct a DataSet and a new DataTable that will have the modified data
                        DataTable modifiedTable = new DataTable();

                        // Set up all of the columns for the DataTable
                        setTableColumns(modifiedTable);

                        // Construct a string to hold all the new tables column names
                        String[] columnNames = { "ID", "ISBN", "Title", "Authors", "Genres", "Summary", "PageCount" };

                        // First, loop through all of the tables in the DataTable object
                        foreach (DataTable table in originalData.Tables)
                        {
                            // Construct a DataRowCollection object that represents a collection of rows from the table DataTable
                            DataRowCollection oldDataRows = table.Rows;

                            // This variable represents the insert position for the new DataTable. This is needed because the positions of the two tables will not
                            // be parallel becuase we will be skipping rows in the original data table
                            int newDataRowsIndex = 0;

                            // For every row in the original data set data table. A for loop is used becuase we need to know the exact position in the original data table
                            for (int oldDataRowsIndex = 0; oldDataRowsIndex < oldDataRows.Count; oldDataRowsIndex++)
                            {
                                // If i is the very last row in the original data table, we will just add that row into the new data table
                                if (oldDataRowsIndex == oldDataRows.Count - 1)
                                {
                                    // Construct a new DataRow
                                    DataRow newRow = modifiedTable.NewRow();

                                    // Construct the string that holds the genre and author names
                                    String genresCollection = oldDataRows[oldDataRowsIndex]["Genres"].ToString();
                                    String authorsCollection = oldDataRows[oldDataRowsIndex]["Authors"].ToString();

                                    // Create the new row and add it to the new table
                                    createRow(modifiedTable, oldDataRows, columnNames, genresCollection, authorsCollection, oldDataRowsIndex, newDataRowsIndex);

                                    // increment the shadowI variable to show that the new table has one more row than before adding the new row
                                    newDataRowsIndex++;
                                }
                                // If i is not the very last row in the original data table, we will either check for successive like rows or add the row if not
                                else
                                {
                                    // If the row at the i position in the original data set table has the same 'BookID' as the next row, we will start looping through all the books after
                                    // the current i position
                                    if (oldDataRows[oldDataRowsIndex]["ISBN"].Equals(oldDataRows[oldDataRowsIndex + 1]["ISBN"]))
                                    {
                                        // Construct strings to hold all of the genres and authors that describe the BookID at the current position in the original data set table
                                        String genresCollection = oldDataRows[oldDataRowsIndex]["Genres"].ToString();
                                        String authorsCollection = oldDataRows[oldDataRowsIndex]["Authors"].ToString();

                                        // For every row after the current i position 
                                        for (int j = oldDataRowsIndex + 1; j < oldDataRows.Count; j++)
                                        {
                                            // Check if the current i positions "BookID" is the same as the next position. This holds the i position constant while changing the j position
                                            if (oldDataRows[oldDataRowsIndex]["ISBN"].Equals(oldDataRows[j]["ISBN"]))
                                            {
                                                // Check if the genre string already has the genre in it. If not add it.
                                                if (!genresCollection.Contains(oldDataRows[j]["Genres"].ToString()))
                                                {
                                                    // If the two positions have the same book, then we want to concatenate the genre names that are in each row
                                                    genresCollection += ", " + oldDataRows[j]["Genres"].ToString();
                                                }

                                                // Check if the author string already has the author in it. If not add it.
                                                if (!authorsCollection.Contains(oldDataRows[j]["Authors"].ToString()))
                                                {
                                                    // If the two positions have the same book, then we want to concatenate the author names that are in each row
                                                    authorsCollection += ", " + oldDataRows[j]["Authors"].ToString();
                                                }

                                                // Change the i position (which points to the position of the original table) to the j position, effectively skipping all of the rows that were the same
                                                oldDataRowsIndex = j;
                                            }
                                            // If the current i positions "BookID" is not the same as the next position, we will break from the loop because there is no reason to check the other rows
                                            else
                                            {
                                                break;
                                            }
                                        }

                                        // Add the new row
                                        createRow(modifiedTable, oldDataRows, columnNames, genresCollection, authorsCollection, oldDataRowsIndex, newDataRowsIndex);

                                        // Increment the shadowI variable to show that the new table has one more row than before adding the new row
                                        newDataRowsIndex++;
                                    }
                                    // Otherwise, the row at position i is the only row in the table with the same "BookID"
                                    else
                                    {
                                        // the string that holds the genre name
                                        String genresCollection = oldDataRows[oldDataRowsIndex]["Genres"].ToString();

                                        // the string that holds the author name
                                        String authorsCollection = oldDataRows[oldDataRowsIndex]["Authors"].ToString();

                                        // Add the new row
                                        createRow(modifiedTable, oldDataRows, columnNames, genresCollection, authorsCollection, oldDataRowsIndex, newDataRowsIndex);

                                        // Increment the shadowI variable to show that the new table has one more row than before adding the new row
                                        newDataRowsIndex++;
                                    }
                                }
                            }
                        }

                        // Add the new table to the DataSet
                        modifiedDataSet.Tables.Add(modifiedTable);

                        // If the table does not have anything in it, then just add one row with a value in the first column
                        if (modifiedDataSet.Tables[0].Rows.Count == 0)
                        {
                            // Constructs a new DataRow object
                            DataRow newRow = modifiedDataSet.Tables[0].NewRow();

                            // Insert column values for each column in the DatatRow
                            newRow[columnNames[0]] = "-";               // BookID
                            newRow[columnNames[1]] = "Empty";           // BookISBN
                            newRow[columnNames[2]] = "-";               // BookName
                            newRow[columnNames[3]] = "-";               // Authors
                            newRow[columnNames[4]] = "-";               // Genres
                            newRow[columnNames[5]] = "-";               // BookSummary
                            newRow[columnNames[6]] = 0;                 // BookPageCount

                            // Insert the row at a given index in the table
                            modifiedDataSet.Tables[0].Rows.InsertAt(newRow, 0);
                        }

                        // Set the data source of the gridview and databind it
                        readingListSelectedGV.DataSource = modifiedDataSet;
                        readingListSelectedGV.DataBind();

                        readingListSelectedGV.Visible = true;
                        readingListDefaultGV.Visible = false;

                        // Make sure the feedback literal is not shown
                        newListFeedbackLiteral.Text = "";
                    }
                }
            }
        }

        /**
         * Set and add the table columns that should be found on a DataTable object
         * @param table the DataTable to add the new columns to
         */
        protected void setTableColumns(DataTable table)
        {
            // Add a column for the book isbn. The column should be a string value, and the name should be "BookISBN"
            table.Columns.Add(createColumn("System.String", "ID", true, true));

            // Add a column for the book isbn. The column should be a string value, and the name should be "BookISBN"
            table.Columns.Add(createColumn("System.String", "ISBN", true, true));

            // Add a column for the book name. The column should be a string value, and the name should be "BookName"
            table.Columns.Add(createColumn("System.String", "Title", true, false));

            // Add a column for the genre names. The column should be a string value, and the name should be "Authors"
            table.Columns.Add(createColumn("System.String", "Authors", true, false));

            // Add a column for the genre names. The column should be a string value, and the name should be "Genres"
            table.Columns.Add(createColumn("System.String", "Genres", true, false));

            // Add a column for the book isbn. The column should be a string value, and the name should be "BookISBN"
            table.Columns.Add(createColumn("System.String", "Summary", true, false));

            // Add a column for the book isbn. The column should be a string value, and the name should be "BookISBN"
            table.Columns.Add(createColumn("System.Int32", "PageCount", true, false));
        }

        /**
         * Creates a DataColumn object based on the passed parameters
         * @param dataType the data type for the column
         * @param columnName the column name
         * @param readOnly whether the column is read only or not (true, false)
         * @param unique whether the column value should be unique or not (true, false)
         */
        protected DataColumn createColumn(String dataType, String columnName, bool readOnly, bool unique)
        {
            // Construct a DataColumn object that represents a column
            DataColumn column = new DataColumn();

            // Set the properties for the column
            column.DataType = System.Type.GetType(dataType);
            column.ColumnName = columnName;
            column.ReadOnly = readOnly;
            column.Unique = unique;

            // return the DataColumn object
            return column;
        }

        /**
         * Creates a new row in a given data table whose values are derived from another table
         * @param table the data table to add the new row to
         * @param oldDataRows the rows of data from the old table
         * @param columnNames the column names (both the table and data rows need to have the same column names)
         * @param genresCollection the concatenated string to represent the genres for this row
         * @param getPosition the position from which the data rows' values are derived from, the row position in the old table
         * @param insertPosition the position at which the new row is to be inserted in the new table
         */
        protected void createRow(DataTable table, DataRowCollection oldDataRows, String[] columnNames, String genresCollection, String authorsCollection, int getPosition, int insertPosition)
        {
            // Constructs a new DataRow object
            DataRow newRow = table.NewRow();

            // Insert column values for each column in the DatatRow
            newRow[columnNames[0]] = oldDataRows[getPosition][columnNames[0]];          // BookID
            newRow[columnNames[1]] = oldDataRows[getPosition][columnNames[1]];          // BookISBN
            newRow[columnNames[2]] = oldDataRows[getPosition][columnNames[2]];          // BookName
            newRow[columnNames[3]] = authorsCollection;                                 // Authors
            newRow[columnNames[4]] = genresCollection;                                  // Genres
            newRow[columnNames[5]] = oldDataRows[getPosition][columnNames[5]];          // BookSummary
            newRow[columnNames[6]] = oldDataRows[getPosition][columnNames[6]];          // BookPageCount

            // Insert the row at a given index in the table
            table.Rows.InsertAt(newRow, insertPosition);
        }
    }
}