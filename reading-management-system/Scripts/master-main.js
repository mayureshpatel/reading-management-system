$(document).ready(function () {
    var navSearchForm = $("#nav-search-form");
    var navUL = $("#nav-ul");
    var cancelButton = $("#ResetSearchButton");
    var browseButton = $("#BrowseButton");
    var browseDropDown = $("#BrowseDropDown");

    // When the search button is clicked, toggle the search bar only if the document width is 576px or less
    $("#searchBookButton").click(function () {
        var windowSize = $(window).width();

        if (windowSize <= 576) {
            // when the button is clicked and the form is not up, we want to make the ul disappear and the form appear
            if (navSearchForm.css("display").localeCompare("none") == 0) {
                // toggle the navbar off first immediately
                navUL.hide();

                // now slide down the search form
                navSearchForm.slideDown(150);

                // change the values of the logo to the gotten value
                var displayOutput = navSearchForm.css("display");

                // change the display value to flex if it is block
                if (displayOutput.localeCompare("block") == 0) {
                    navSearchForm.css({ "display": "flex" });
                }
            }
            else if (navSearchForm.css("display").localeCompare("flex") == 0) {
                // toggle the searchbar off first immediately
                navSearchForm.hide();

                // now slide down the nav ul
                navUL.slideDown(150);
            }
        }
    });

    // Need to handle when the toggle search bar is up and the window is resized to greater than 576px
    $(window).resize(function () {
        var windowSize = $(window).width();

        if (windowSize > 576 && navSearchForm.css("display").localeCompare("flex") == 0) {
            // toggle the form
            navSearchForm.toggle();

            // toggle the nav
            navUL.slideToggle(350);
        }
    });

    // When the cancel button is clicked, toggle the search bar off and toggle the nav ul back on
    $(cancelButton).click(function () {
        // toggle the form
        navSearchForm.toggle();

        // toggle the nav
        navUL.toggle(250);
    });

    // When the browse button is clicked, toggle the browse dropdown menu
    $(browseButton).click(function () {
        // toggle the browse drop down
        browseDropDown.slideToggle(150);

        // change the values of the logo to the gotten value
        var displayOutput = browseDropDown.css("display");

        // change the display value to flex if it is block
        if (displayOutput.localeCompare("block") == 0) {
            browseDropDown.css({ "display": "grid" });
        }
    });

    // Be able to click anywhere to hide the Browse drop down
    $("body").click(function (evt) {
        if (evt.target.id == "#BrowseDropDown") {
            return;
        }
            
        //For descendants of menu_content being clicked, remove this check if you do not want to put constraint on descendants.
        if ($(evt.target).closest("#BrowseDropDown").length) {
            return;
        }

        //Do processing of click event here for every element except with id menu_content
        if (evt.target.id == "" && browseDropDown.css("display").localeCompare("grid") == 0) {
            browseDropDown.slideToggle(150);
        }
    });

    // Set the current year in the footer span
    setCopyrightYear();

    // When a row in the table is clicked, popup a modal that will add a book into a list
    $(".all-books-table tr").click(function () {
        // Check if the modal is already open, if it is, don't allow any click functionality
        if (!$(".modal").css("display").localeCompare("block") == 0) {
            // First set a variable for the row that was clicked
            let currentRow = $(this);

            // If the row was not a header row, we want to grab the ISBN value of that row
            if (this.children[0].tagName === "TD") {
                // Get the selected rows ISBN value
                let rowID = currentRow.find("td:eq(0)").text();
                let rowISBN = currentRow.find("td:eq(1)").text();
                let rowTitle = currentRow.find("td:eq(2)").text();
                let rowAuthors = currentRow.find("td:eq(3)").text();
                let rowGenres = currentRow.find("td:eq(4)").text();
                let rowSummary = currentRow.find("td:eq(5)").text();
                let rowPageCount = currentRow.find("td:eq(6)").text();

                // Set the text of the appropriate tags in the modal for each information block
                $("#MainContent_modalBookTitle").text(rowTitle);
                $("#MainContent_modalBookISBN").text(rowISBN);
                $("#MainContent_modalBookAuthors").text(rowAuthors);
                $("#MainContent_modalBookGenres").text(rowGenres);
                $("#MainContent_modalBookSummary").text(rowSummary);
                $("#MainContent_modalBookPageCount").text(rowPageCount);
                
                // Set the text of the hidden p tag so that it reflected the selected value
                $("#MainContent_SelectedBookID").text(rowID);
                $("#MainContent_selectedBookIDTextBox").val(rowID);

                // Launch the modal after dimming the background
                if ($(".modal").css("display").localeCompare("none") == 0) {
                    $("body").css("background-color", "rgba(0, 0, 0, 0.20)");
                    $(".modal").slideToggle(300);
                }
                else {
                    $("body").css("background-color", "rgb(255, 255, 255)");
                    $(".modal").slideToggle(300);
                }
            }
            else {
                $("#MainContent_SelectedISBN").text(0);
            }
        }
    });

    // When the close button on the modal is clicked, close the modal
    $(".close-button").click(function () {
        // slide toggle the modal
        $(".modal").slideToggle(300);
    });

    // When the drop down list in the reading lists' selected value is changed, turn the textbox on and off
    $("#MainContent_readingListDDL").change(function () {
        // Get the select element
        let readingListDDL = $(this);

        // Get the selected value
        let selectedValue = readingListDDL.val();

        // If the selected value is 0, then turn on the textbox
        if (selectedValue === "0") {
            $("#MainContent_newReadingListTextBox").css("display", "block");
        }
        else {
            $("#MainContent_newReadingListTextBox").css("display", "none");
        }
    });
});

// Function to get the current year to display in the copyright section
function setCopyrightYear() {
    var currentDate = new Date();
    var currentYear = currentDate.getFullYear();

    document.getElementById("footer-year").innerHTML = currentYear;
}