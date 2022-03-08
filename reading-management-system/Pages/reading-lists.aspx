<%@ Page Title="RMS | My Reading Lists"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="reading-lists.aspx.cs"
    Inherits="reading_management_system.Pages.reading_lists" %>

<asp:Content ID="PageHead" ContentPlaceHolderID="PageHead" runat="server">
    <link href="../Styles/Pages_Styles/reading-lists.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="HeaderContent" ContentPlaceHolderID="MainContentHeader" runat="server">
    <h1>Reading Lists</h1>
</asp:Content>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Select a reading list to display -->
    <div class="rl-select-content">
        <asp:SqlDataSource ID="readingListSDS" runat="server"
            ConnectionString="<%$ ConnectionStrings:ReadingDBConnectionString %>"
            SelectCommand="SELECT * FROM [ReadingList]">
        </asp:SqlDataSource>
        <asp:Label ID="selectReadingListLabel" runat="server" Text="Select a List:"></asp:Label>
        <asp:DropDownList ID="readingListDDL" runat="server"
            AppendDataBoundItems="True"
            DataSourceID="readingListSDS"
            DataTextField="ReadingListName"
            DataValueField="ReadingListID">
            <asp:ListItem Selected="True" Text="--Select One--" Value="-1"></asp:ListItem>
            <asp:ListItem Text="New List" Value="0"></asp:ListItem>
        </asp:DropDownList>
        <asp:TextBox ID="newReadingListTextBox" runat="server"></asp:TextBox>
        <asp:Button ID="submitButton" runat="server" Text="Show List" />
    </div>

    <!-- Feedback for when a new list is created -->
    <asp:Literal ID="newListFeedbackLiteral" runat="server"></asp:Literal>

    <!-- Table to show the selected data -->
    <div class="rl-list-content">
        <asp:SqlDataSource ID="readingListBooksSDS" runat="server"></asp:SqlDataSource>
        <asp:SqlDataSource ID="readingListDefaultSDS" runat="server"
            ConnectionString="<%$ ConnectionStrings:ReadingDBConnectionString %>"
            SelectCommand="SELECT ReadingList.ReadingListID, ReadingList.ReadingListName AS ListName, COUNT(ReadingListAffiliations.FK_BookID) AS BookCount
                FROM ReadingList
                LEFT OUTER JOIN ReadingListAffiliations ON ReadingList.ReadingListID = ReadingListAffiliations.FK_ReadingListID
                GROUP BY ReadingList.ReadingListName, ReadingList.ReadingListID
                ORDER BY ReadingList.ReadingListID">
        </asp:SqlDataSource>
        <asp:GridView ID="readingListDefaultGV" runat="server"
            CssClass="reading-lists-table"
            AutoGenerateColumns="false"
            DataSourceID="readingListDefaultSDS"
            DataKeyNames="ReadingListID">
            <Columns>
                <asp:BoundField DataField="ReadingListID" HeaderText="ReadingListID" SortExpression="ReadingListID" ReadOnly="True" ItemStyle-CssClass="col-hidden" HeaderStyle-CssClass="col-hidden" />
                <asp:BoundField DataField="ListName" HeaderText="ListName" SortExpression="ListName" />
                <asp:BoundField DataField="BookCount" HeaderText="BookCount" ReadOnly="True" SortExpression="BookCount" />
            </Columns>
        </asp:GridView>
        <asp:GridView ID="readingListSelectedGV" runat="server"
            AutoGenerateColumns="false"
            Visible="false"
            CssClass="reading-lists-table">
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
</asp:Content>