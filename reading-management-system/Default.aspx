<%@ Page Title="RMS | Home"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Default.aspx.cs"
    Inherits="reading_management_system.Default" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PageHead" runat="server">
</asp:Content>

<asp:Content ID="MainContentHeader" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>My Books</h1>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Show all the books in the database -->
    <div class="table-container">
        <!-- Feedback for when an attempt is made to add a book to a list -->
        <asp:Literal ID="AddToListFeedbackLiteral" runat="server"></asp:Literal>

        <!-- Gridview to show all the books in the database -->
        <asp:GridView ID="allBooksGV" runat="server"
            CssClass="all-books-table"
            AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" ItemStyle-CssClass="col-hidden" HeaderStyle-CssClass="col-hidden" />
                <asp:BoundField DataField="ISBN" HeaderText="ISBN" SortExpression="ISBN" />
                <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                <asp:BoundField DataField="Authors" HeaderText="Authors" SortExpression="Authors" />
                <asp:BoundField DataField="Genres" HeaderText="Genres" SortExpression="Genres" />
                <asp:BoundField DataField="Summary" HeaderText="Summary" SortExpression="Summary" ItemStyle-CssClass="col-hidden" HeaderStyle-CssClass="col-hidden" />
                <asp:BoundField DataField="PageCount" HeaderText="PageCount" SortExpression="PageCount" ItemStyle-CssClass="col-hidden" HeaderStyle-CssClass="col-hidden" />
            </Columns>
        </asp:GridView>
    </div>

    <!-- Modal for adding a book to a reading list -->
    <div class="modal">
        <div class="modal-header">
            <div class="mh-label-header">
                <small>Title:</small>
                <h1 id="modalBookTitle" runat="server"></h1>
            </div>
            <input type="button" class="close-button" value="&times;" />
        </div>
        <hr />

        <div class="modal-content">
            <div class="mc-book-meta">
                <div class="smc-isbn">
                    <small>ISBN-13:</small>
                    <h5 id="modalBookISBN" runat="server"></h5>
                </div>
                <div class="smc-pagecount">
                    <small>Page Count:</small>
                    <h5 id="modalBookPageCount" runat="server"></h5>
                </div>
            </div>

            <div class="mc-author">
                <small>Author(s):</small>
                <h3 id="modalBookAuthors" runat="server"></h3>
            </div>
            <div class="mc-genre">
                <small>Genre(s):</small>
                <h3 id="modalBookGenres" runat="server"></h3>
            </div>
            <div class="mc-summary">
                <small>Summary:</small>
                <p id="modalBookSummary" runat="server"></p>
            </div>
        </div>

        <div class="modal-footer">
            <asp:SqlDataSource ID="readingListsSDS" runat="server"
                ConnectionString='<%$ ConnectionStrings:ReadingDBConnectionString %>'
                SelectCommand="SELECT * FROM [ReadingList]">
            </asp:SqlDataSource>
            <div class="mf-selectbutton">
                <asp:DropDownList ID="readingListsDDL" runat="server"
                    DataSourceID="readingListsSDS"
                    DataTextField="ReadingListName"
                    DataValueField="ReadingListID"
                    AppendDataBoundItems="true">
                    <asp:ListItem Selected="True" Text="--Select One--" Value="0"></asp:ListItem>
                </asp:DropDownList>

                <asp:LinkButton ID="AddToListButton" runat="server" OnClick="AddToListButton_ServerClick">Add To List</asp:LinkButton>
            </div>

            <!-- A hidden text box to hold the selected book when a postback is made after clicking the add to list button -->
            <div class="mf-feedback">
                <asp:TextBox ID="selectedBookIDTextBox" runat="server" CssClass="col-hidden"></asp:TextBox>
            </div>
        </div>
    </div>
</asp:Content>
