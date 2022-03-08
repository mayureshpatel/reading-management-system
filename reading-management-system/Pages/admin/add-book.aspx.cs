using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace reading_management_system.Pages.admin
{
    public partial class add_book : System.Web.UI.Page
    {
        // Instance Variable to hold the connection string
        protected String myConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ReadingDBConnectionString"].ConnectionString;

        /**
         * Code to run when the page is loaded upon landing on the page or postback
         */
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /**
         * Code to run when the submit button is clicked
         */
        protected void newBookSubmitButton_Click(object sender, EventArgs e)
        {
            // Once the submit button is clicked, the input will be validated. If the validation succeeds, then we will
            // try to insert the data into the database
            if (ValidateInput())
            {
                // Get the authors and genres input string and process them
                String[] authorsList = GenerateInputList(bookAuthorsTextBox.Text);
                String[] genresList = GenerateInputList(bookGenresTextBox.Text);

                // Lists to hold authors and geners that need to be added to the database
                List<String> addAuthorsList = new List<String>();
                List<String> addGenresList = new List<String>();

                // Populate the addAuthorsList by checking if the author already exists in the database,
                // if it doesn't exist, then add that author to the list
                for (int i = 0; i < authorsList.Length; i++)
                {
                    if (!IsInAuthorTable(authorsList[i]))
                    {
                        addAuthorsList.Add(authorsList[i]);
                    }
                }

                // Populate the addGenresList by chekcing if the genre already exists in the database,
                // if it doesn't exist, then add that genre to the list
                for (int i = 0; i < genresList.Length; i++)
                {
                    if (!IsInGenreTable(genresList[i]))
                    {
                        addGenresList.Add(genresList[i]);
                    }
                }

                // If the add author list has authors in it, then add those authors to the database
                if (addAuthorsList.Count > 0)
                {
                    InsertNewAuthors(addAuthorsList);
                }

                // If the add genre list has genres in it, then add those genres to the database
                if (addGenresList.Count > 0)
                {
                    InsertNewGenres(addGenresList);
                }

                // Add the book to the book table if it is not already in there
                if (!IsInBookTable(bookISBNTextBox.Text))
                {
                    InsertNewBook(authorsList, genresList);
                    newBookFeedbackLiteral.Text = "<small class='success-small'>Successfully Added Book</small>";
                }
                else
                {
                    newBookFeedbackLiteral.Text = "<small class='error-small'>Book Already Exists</small>";
                }
            }
            else
            {
                newBookFeedbackLiteral.Text = "<small class='error-small'>Errors in Input</small>";
            }
        }

        /**
         * Validates the form when the submit button is clicked. Each input should have values inside
         * of it. The ISBN should be a 13 digit number, the page count should be a number, and the
         * author names and genre names should be comma separated values.
         */
        protected bool ValidateInput()
        {
            // Set the Text values of all literals to empty strings
            SetErrorLiteralText("");

            // Set the borders for all input boxes back to none
            InitInputBorders();

            // Flag to test if the input is valid (cannot be set back to true)
            bool validFlag = true;

            // Validates the book name input (cannot be empty)
            if (bookNameTextBox.Text.Length == 0)
            {
                // Set the error literal associated with the book name
                bookNameLiteral.Text = "<small class='error-small'>cannot be blank</small>";

                // Add a css class to the textbox that will change the border to red
                bookNameTextBox.Style.Add("border", "1px solid red");

                // Set the valid flag to false
                validFlag = false;
            }
            else
            {
                // Add a css class to the textbox that will change the border to green
                bookNameTextBox.Style.Add("border", "1px solid green");
            }

            // Validates the book isbn input (must be a 13 digit number)
            Regex isbnPattern = new Regex(@"^\d{13}$");
            if (!isbnPattern.IsMatch(bookISBNTextBox.Text))
            {
                // Set the error literal associated with the book isbn
                bookISBNLiteral.Text = "<small class='error-small'>must be a 13 digit number</small>";

                // Add a css class to the textbox that will change the border to red
                bookISBNTextBox.Style.Add("border", "1px solid red");

                // Set the valid flag to false
                validFlag = false;
            }
            else
            {
                // Add a css class to the textbox that will change the border to green
                bookISBNTextBox.Style.Add("border", "1px solid green");
            }

            // Validates the page count input (cannot be blank)
            if (bookPageCountTextBox.Text.Length == 0)
            {
                // Set the error literal associated with the book page count
                bookPageCountLiteral.Text = "<small class='error-small'>cannot be blank</small>";

                // Add a css class to the textbox that will change the border to red
                bookPageCountTextBox.Style.Add("border", "1px solid red");

                // Set the valid flag to false
                validFlag = false;
            }
            else
            {
                // Add a css class to the textbox that will change the border to green
                bookPageCountTextBox.Style.Add("border", "1px solid green");
            }

            // Validates the authors name input (cannot be blank and must be comma separated)
            if (Regex.Matches(bookAuthorsTextBox.Text, @"\w[^,]+").Count == 0 || bookAuthorsTextBox.Text.Length == 0)
            {
                // Set error literal associated with the author names
                bookAuthorsLiteral.Text = "<small class='error-small'>cannot be blank</small>";

                // Add a css class to the textbox that will change the border to green
                bookAuthorsTextBox.Style.Add("border", "1px solid red");

                // Set the valid flag to false
                validFlag = false;
            }
            else
            {
                // Add a css class to the textbox that will change the border to green
                bookAuthorsTextBox.Style.Add("border", "1px solid green");
            }

            // Validates the genre names input (cannot be blank and must be comma separated)
            if (Regex.Matches(bookGenresTextBox.Text, @"\w[^,]+").Count == 0 || bookGenresTextBox.Text.Length == 0)
            {
                // Set error literal associated with the author names
                bookGenresLiteral.Text = "<small class='error-small'>cannot be blank</small>";

                // Add a css class to the textbox that will change the border to red
                bookGenresTextBox.Style.Add("border", "1px solid red");

                // Set the valid flag to false
                validFlag = false;
            }
            else
            {
                // Add a css class to the textbox that will change the border to green
                bookGenresTextBox.Style.Add("border", "1px solid green");
            }

            // Validates the book summary input (cannot be blank)
            if (bookSummaryTextBox.Text.Length == 0)
            {
                // Set the error literal associated with the book summary
                bookSummaryLiteral.Text = "<small class='error-small'>cannot be blank</small>";

                // Add a css class to the textbox that will change the border to red
                bookSummaryTextBox.Style.Add("border", "1px solid red");

                // Set the valid flag to false
                validFlag = false;
            }
            else
            {
                // Add a css class to the textbox that will change the border to green
                bookSummaryTextBox.Style.Add("border", "1px solid green");
            }

            // Return the valid flag's value
            return validFlag;
        }

        /**
         * Sets the Text values of all error literals to a given text
         * @param text the text to set the literal Text value to
         */
        protected void SetErrorLiteralText(String text)
        {
            bookNameLiteral.Text = text;
            bookISBNLiteral.Text = text;
            bookPageCountLiteral.Text = text;
            bookAuthorsLiteral.Text = text;
            bookGenresLiteral.Text = text;
            bookSummaryLiteral.Text = text;
        }

        /**
         * Resets the border property for all the input boxes
         */
        protected void InitInputBorders()
        {
            bookNameTextBox.Style.Remove("border");
            bookISBNTextBox.Style.Remove("border");
            bookPageCountTextBox.Style.Remove("border");
            bookAuthorsTextBox.Style.Remove("border");
            bookGenresTextBox.Style.Remove("border");
            bookSummaryTextBox.Style.Remove("border");
        }

        /**
         * Generates a list of strings form the input using the split method
         * @param input the input string
         * @return inputList an array of strings that represent the input values
         */
        protected String[] GenerateInputList(String input)
        {
            // Split the text input by commas
            String[] inputList = input.Split(',');

            // Process each element in the list
            for (int i = 0; i < inputList.Length; i++)
            {
                // Trim the leading and trailing whitespace
                inputList[i] = inputList[i].Trim();

                // Split the elements contents by spaces
                String[] inputListSplit = inputList[i].Split();

                // Capitalize each word in the input
                for (int j = 0; j < inputListSplit.Length; j++)
                {
                    inputListSplit[j] = inputListSplit[j].Trim().ToLower().Substring(0, 1).ToUpper() + inputListSplit[j].Trim().ToLower().Substring(1);
                }

                // Join the list back into one string
                inputList[i] = String.Join(" ", inputListSplit);
            }

            // Return an array of string after processing the strings
            return inputList;
        }

        /**
         * Checks if the given author is in the Author table
         * @param authorName the author name
         * @return true if the author is in the Authors table otherwise, false
         */
        protected bool IsInAuthorTable(String authorName)
        {
            // Construct the select command
            String authorSelectCommand = "SELECT AuthorName FROM Author WHERE AuthorName = @authorName";

            // Boolean flag to keep track whether the author was found in the table
            bool isInDB = false;

            // Initialize the sql connection
            using (SqlConnection myConnection = new SqlConnection(myConnectionString))
            {
                // Initialize the sql command
                using (SqlCommand myCommand = new SqlCommand(authorSelectCommand, myConnection))
                {
                    // Add the parameters
                    myCommand.Parameters.AddWithValue("authorName", authorName);

                    // Open the connection
                    myConnection.Open();

                    // Execute the command
                    String resultAuthor = (String)myCommand.ExecuteScalar();

                    // Close the connection
                    myConnection.Close();

                    // Set the boolean flag to true if the author was found in the table
                    if (resultAuthor != null)
                    {
                        isInDB = true;
                    }
                }
            }

            // Return the boolean flag
            return isInDB;
        }

        /**
         * Checks if the given genre is in the Genres table
         * @param genreName the name of the genre
         * @return true if the genre is in the Genres table otherwise, false
         */
        protected bool IsInGenreTable(String genreName)
        {
            // Construct the select command
            String genreSelectCommand = "SELECT GenreName FROM Genre WHERE GenreName = @genreName";

            // Construct a boolean flag that keeps track of whether the genre is in the table or not
            bool isInDB = false;

            // Initialize the sql connection
            using (SqlConnection myConnection = new SqlConnection(myConnectionString))
            {
                // Initialize the sql command
                using (SqlCommand myCommand = new SqlCommand(genreSelectCommand, myConnection))
                {
                    // Add the select parameters
                    myCommand.Parameters.AddWithValue("genreName", genreName);

                    // Open the connection
                    myConnection.Open();

                    // Execute the command
                    String resultGenre = (String)myCommand.ExecuteScalar();

                    // Close the connection
                    myConnection.Close();

                    // Set the isInDB boolean flag to true if found in the table
                    if (resultGenre != null)
                    {
                        isInDB = true;
                    }
                }
            }

            // Return the boolean flag
            return isInDB;
        }

        /**
         * Inserts the given authors into the Authors table
         * @param newAuthorsList the list of new authors to add
         */
        protected void InsertNewAuthors(List<String> newAuthorsList)
        {
            // Construct the insert command
            String authorInsertCommand = "INSERT INTO Author (AuthorName) Values (@authorName)";

            // Insert each author in the list into the Authors table
            foreach (String authorName in newAuthorsList)
            {
                // Initialize the sql connection
                using (SqlConnection myConnection = new SqlConnection(myConnectionString))
                {
                    // Initialize the sql command
                    using (SqlCommand myCommand = new SqlCommand(authorInsertCommand, myConnection))
                    {
                        // Add the parameters
                        myCommand.Parameters.AddWithValue("authorName", authorName);

                        // Open the connection
                        myConnection.Open();

                        // Execute the command
                        myCommand.ExecuteNonQuery();

                        // Close the connection
                        myConnection.Close();
                    }
                }
            }
        }

        /**
         * Inserts the given genres into the Genres table
         * @param newGenresList the list of new genres to add
         */
        protected void InsertNewGenres(List<String> newGenresList)
        {
            // Construct the insert command
            String genreInsertCommand = "INSERT INTO Genre (GenreName) Values (@genreName)";

            // Loop through each genre in the newGenresList
            foreach (String genreName in newGenresList)
            {
                // Initialize the sql connection
                using (SqlConnection myConnection = new SqlConnection(myConnectionString))
                {
                    // Initialize the sql command
                    using (SqlCommand myCommand = new SqlCommand(genreInsertCommand, myConnection))
                    {
                        // Add the parameters
                        myCommand.Parameters.AddWithValue("genreName", genreName);

                        // Open the connection
                        myConnection.Open();

                        // Execute the command
                        myCommand.ExecuteNonQuery();

                        // Close the connection
                        myConnection.Close();
                    }
                }
            }
        }

        /**
         * Checks if a given book in in the Book table of the database
         * @param bookISBN the book ISBN to check
         * @return true if the book is in the table, otherwise false
         */
        protected bool IsInBookTable(String bookISBN)
        {
            // The select statement
            String selectBookCommand = "SELECT BookISBN FROM Book WHERE BookISBN = @bookISBN";

            // Boolean flag to know if the book is in the table
            bool isInDB = false;

            // Initialize the sql connection
            using (SqlConnection myConnection = new SqlConnection(myConnectionString))
            {
                // Initialize the sql command
                using (SqlCommand myCommand = new SqlCommand(selectBookCommand, myConnection))
                {
                    // Add the parameters
                    myCommand.Parameters.AddWithValue("bookISBN", bookISBN);

                    // Open the connection
                    myConnection.Open();

                    // Execute the command
                    String resultISBN = (String)myCommand.ExecuteScalar();

                    // Close the connection
                    myConnection.Close();

                    // Set the boolean flag to true if the resultISBN is not null
                    if (resultISBN != null)
                    {
                        isInDB = true;
                    }
                }
            }

            // Return the isInDB boolean flag
            return isInDB;
        }

        /**
         * Inserts a new book into the Book table
         * @param authorsList the authors of the book
         * @param genresList the genres of the book
         */
        protected void InsertNewBook(String[] authorsList, String[] genresList)
        {
            // Insert into the Book table first
            InsertBook();

            // Insert the author and genre affiliations
            InsertAuthorAffils(authorsList, GetBookID(bookISBNTextBox.Text));
            InsertGenreAffils(genresList, GetBookID(bookISBNTextBox.Text));

            // Reset all form fields
            bookNameTextBox.Text = "";
            bookISBNTextBox.Text = "";
            bookPageCountTextBox.Text = "";
            bookAuthorsTextBox.Text = "";
            bookGenresTextBox.Text = "";
            bookSummaryTextBox.Text = "";
        }

        /**
         * Insert a book into the Book table taking the parameters form the form input values
         */
        protected void InsertBook()
        {
            // Construct the insert command
            String insertBookCommand = "INSERT INTO Book (BookISBN, BookName, BookSummary, BookPageCount)" +
                " VALUES (@bookISBN, @bookName, @bookSummary, @bookPageCount)";

            // Initialize the sql connection
            using (SqlConnection myConnection = new SqlConnection(myConnectionString))
            {
                // Initialize the sql command
                using (SqlCommand myCommand = new SqlCommand(insertBookCommand, myConnection))
                {
                    // Set up the insert parameters
                    myCommand.Parameters.AddWithValue("bookISBN", bookISBNTextBox.Text);
                    myCommand.Parameters.AddWithValue("bookName", bookNameTextBox.Text);
                    myCommand.Parameters.AddWithValue("bookSummary", bookSummaryTextBox.Text);
                    myCommand.Parameters.AddWithValue("bookPageCount", bookPageCountTextBox.Text);

                    // Open the connection
                    myConnection.Open();

                    // Execute the command
                    int rowsAffected = myCommand.ExecuteNonQuery();

                    // Close the connection
                    myConnection.Close();
                }
            }
        }

        /**
         * Inserts the author affiliations for the given book
         * @param authorList the authors of the book
         * @param bookID the book
         */
        protected void InsertAuthorAffils(String[] authorsList, int bookID)
        {
            // Loop through each author in the authorList
            foreach (String authorName in authorsList)
            {
                // Construct the insert command
                String insertAuthorAffilsCommand = "INSERT INTO AuthorAffiliations (FK_BookID, FK_AuthorID)" +
                    " VALUES (@bookID, @authorID)";

                // Initialize the sql connection
                using (SqlConnection myConnection = new SqlConnection(myConnectionString))
                {
                    // Initialize the sql command
                    using (SqlCommand myCommand = new SqlCommand(insertAuthorAffilsCommand, myConnection))
                    {
                        // Set up the insert parameters
                        myCommand.Parameters.AddWithValue("bookID", bookID);
                        myCommand.Parameters.AddWithValue("authorID", GetAuthorID(authorName));

                        // Open the connection
                        myConnection.Open();

                        // Execute the command
                        int rowsAffected = myCommand.ExecuteNonQuery();

                        // Close the connection
                        myConnection.Close();
                    }
                }
            }
        }

        /**
         * Inserts the genre affiliations for the given book
         * @param genreList the genres of the book
         * @param bookID the book
         */
        protected void InsertGenreAffils(String[] genresList, int bookID)
        {
            // Loop through all of the genres in the list
            foreach (String genreName in genresList)
            {
                // Construct the insert command
                String insertGenreAffilsCommand = "INSERT INTO GenreAffiliations (FK_BookID, FK_GenreID)" +
                    " VALUES (@bookID, @genreID)";

                // Initialize the sql connection
                using (SqlConnection myConnection = new SqlConnection(myConnectionString))
                {
                    // Initialize the sql command
                    using (SqlCommand myCommand = new SqlCommand(insertGenreAffilsCommand, myConnection))
                    {
                        // Set up the insert parameters
                        myCommand.Parameters.AddWithValue("bookID", bookID);
                        myCommand.Parameters.AddWithValue("genreID", GetGenreID(genreName));

                        // Open the connection
                        myConnection.Open();

                        // Execute the command
                        int rowsAffected = myCommand.ExecuteNonQuery();

                        // Close the connection
                        myConnection.Close();
                    }
                }
            }
        }

        /**
         * Gets the bookID of a book given the book's ISBN
         * @param bookISBN the ISBN of the book
         * @return bookID the id value of the book in the Book table, 0 if not found
         */
        protected int GetBookID(String bookISBN)
        {
            // Construct the select command
            String selectBookIDCommand = "SELECT BookID FROM Book WHERE BookISBN = @bookISBN";

            // Initialize the bookID to be 0 (which is not possible in the Book table)
            int bookID = 0;

            // Initialize the sql connection
            using (SqlConnection myConnection = new SqlConnection(myConnectionString))
            {
                // Initialize the sql command
                using (SqlCommand myCommand = new SqlCommand(selectBookIDCommand, myConnection))
                {
                    // Set up the parameters
                    myCommand.Parameters.AddWithValue("bookISBN", bookISBN);

                    // Open the connection
                    myConnection.Open();

                    // Execute the query
                    bookID = (int)myCommand.ExecuteScalar();

                    // Close the connection
                    myConnection.Close();
                }
            }

            // Return the bookID
            return bookID;
        }

        /**
         * Gets the genreID of a genre name in the Genre table
         * @param genreName the genre whose id is being retrieved
         * @return genreID the id of the genre, 0 if not found
         */
        protected int GetGenreID(String genreName)
        {
            // Construct the select command
            String selectGenreIDCommand = "SELECT GenreID FROM Genre WHERE GenreName = @genreName";

            // Initialize the genreID to be 0 (which is not possible in the Genre table)
            int genreID = 0;

            // Initialize the sql connection
            using (SqlConnection myConnection = new SqlConnection(myConnectionString))
            {
                // Initialize the sql command
                using (SqlCommand myCommand = new SqlCommand(selectGenreIDCommand, myConnection))
                {
                    // Set up the parameters
                    myCommand.Parameters.AddWithValue("genreName", genreName);

                    // Open the connection
                    myConnection.Open();

                    // Execute the query
                    genreID = (int)myCommand.ExecuteScalar();

                    // Close the connection
                    myConnection.Close();
                }
            }

            // Return the genreID
            return genreID;
        }

        /**
         * Gets the authorID of an author name in the Author table
         * @param authorName the author whose id is being retrieved
         * @return authorID the id of the author, 0 if not found
         */
        protected int GetAuthorID(String authorName)
        {
            // Construct the select command
            String selectAuthorIDCommand = "SELECT AuthorID FROM Author WHERE AuthorName = @authorName";

            // Initialze the authorID to be 0 (which is not possible in the Author table)
            int authorID = 0;

            // Initialize the sql connection
            using (SqlConnection myConnection = new SqlConnection(myConnectionString))
            {
                // Initialize the sql command
                using (SqlCommand myCommand = new SqlCommand(selectAuthorIDCommand, myConnection))
                {
                    // Set up the parameters
                    myCommand.Parameters.AddWithValue("authorName", authorName);

                    // Open the connection
                    myConnection.Open();

                    // Execute the query
                    authorID = (int)myCommand.ExecuteScalar();

                    // Close the connection
                    myConnection.Close();
                }
            }

            // Return the authorID
            return authorID;
        }
    }
}