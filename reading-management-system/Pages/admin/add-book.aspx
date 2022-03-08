<%@ Page Title="RMS | Add Book"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="add-book.aspx.cs"
    Inherits="reading_management_system.Pages.admin.add_book" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PageHead" runat="server">
    <link href="../../Styles/Pages_Styles/add-book.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Add Book</h1>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- FORM TO ADD A NEW BOOK -->
    <div class="add-book-form">
        <div class="form-section book-isbn-page-section">
            <div class="form-item">
                <div class="form-item-label">
                    <asp:Label ID="bookNameLabel" runat="server" AssociatedControlID="bookNameTextBox" Text="Name:"></asp:Label>
                    <asp:Literal ID="bookNameLiteral" runat="server"></asp:Literal>
                </div>
                <asp:TextBox ID="bookNameTextBox" runat="server"></asp:TextBox>
            </div>
            
            <div class="form-item">
                <div class="form-item-label">
                    <asp:Label ID="bookISBNLabel" runat="server" AssociatedControlID="bookISBNTextBox" Text="ISBN:"></asp:Label>
                    <asp:Literal ID="bookISBNLiteral" runat="server"></asp:Literal>
                </div>
                <asp:TextBox ID="bookISBNTextBox" runat="server" placeholder="ISBN-13"></asp:TextBox>
            </div>
            
            <div class="form-item">
                <div class="form-item-label">
                    <asp:Label ID="bookPageCountLabel" runat="server" AssociatedControlID="bookPageCountTextBox" Text="Page Count:"></asp:Label>
                    <asp:Literal ID="bookPageCountLiteral" runat="server"></asp:Literal>
                </div>
                <asp:TextBox ID="bookPageCountTextBox" runat="server" TextMode="Number"></asp:TextBox>
            </div>
        </div>

        <div class="form-section author-genre-section">
            <div class="form-item">
                <div class="form-item-label">
                    <asp:Label ID="bookAuthorsLabel" runat="server" AssociatedControlID="bookAuthorsTextBox" Text="Author(s):"></asp:Label>
                    <asp:Literal ID="bookAuthorsLiteral" runat="server"></asp:Literal>
                </div>
                <asp:TextBox ID="bookAuthorsTextBox" runat="server" placeholder="Comma Separated Names"></asp:TextBox>
            </div>
            
            <div class="form-item">
                <div class="form-item-label">
                    <asp:Label ID="bookGenresLabel" runat="server" AssociatedControlID="bookGenresTextBox" Text="Genre(s):"></asp:Label>
                    <asp:Literal ID="bookGenresLiteral" runat="server"></asp:Literal>
                </div>
                <asp:TextBox ID="bookGenresTextBox" runat="server" placeholder="Comma Separated Genres"></asp:TextBox>
            </div>
        </div>

        <div class="form-section summary-section">
            <div class="form-item">
                <div class="form-item-label">
                    <asp:Label ID="bookSummaryLabel" runat="server" AssociatedControlID="bookSummaryTextBox" Text="Summary:"></asp:Label>
                    <asp:Literal ID="bookSummaryLiteral" runat="server"></asp:Literal>
                </div>
                <asp:TextBox ID="bookSummaryTextBox" runat="server" TextMode="MultiLine" Rows="10" placeholder="Max 2,000 Characters" MaxLength="1000"></asp:TextBox>
            </div>
        </div>

        <div class="form-section submit-section">
            <div class="form-item">
                <asp:Button ID="newBookSubmitButton" runat="server" Text="Add New Book" OnClick="newBookSubmitButton_Click" />
            </div>
        </div>

        <div class="form-section feedback-section">
            <div class="form-item">
                <asp:Literal ID="newBookFeedbackLiteral" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
</asp:Content>
