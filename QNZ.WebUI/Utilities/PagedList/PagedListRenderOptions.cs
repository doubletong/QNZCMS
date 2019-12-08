﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace QNZCMS.Utilities.PagedList
{
    ///<summary>
    /// Options for configuring the output of <see cref = "HtmlHelper" />.
    ///</summary>
    public partial class PagedListRenderOptions
    {
        ///<summary>
        /// The default settings render all navigation links and no descriptive text.
        ///</summary>
        public PagedListRenderOptions()
        {
            DisplayLinkToFirstPage = PagedListDisplayMode.IfNeeded;
            DisplayLinkToLastPage = PagedListDisplayMode.IfNeeded;
            DisplayLinkToPreviousPage = PagedListDisplayMode.IfNeeded;
            DisplayLinkToNextPage = PagedListDisplayMode.IfNeeded;
            DisplayLinkToIndividualPages = true;
            DisplayPageCountAndCurrentLocation = false;
            MaximumPageNumbersToDisplay = 10;
            DisplayEllipsesWhenNotShowingAllPageNumbers = true;
            EllipsesFormat = "&#8230;";
            LinkToFirstPageFormat = "««";
            LinkToPreviousPageFormat = "«";
            LinkToIndividualPageFormat = "{0}";
            LinkToNextPageFormat = "»";
            LinkToLastPageFormat = "»»";
            PageCountAndCurrentLocationFormat = "Page {0} of {1}.";
            ItemSliceAndTotalFormat = "Showing items {0} through {1} of {2}.";
            ClassToApplyToFirstListItemInPager = null;
            ClassToApplyToLastListItemInPager = null;
            ContainerHtmlTag = "nav";
            UlElementClasses = new[] { "pagination" };
            LiElementClasses = Enumerable.Empty<string>();
            AhrefElementClasses = Enumerable.Empty<string>();
            ActiveElementClasses = new[] { "active" };
            DisabledElementClasses = new[] { "disabled" };
        }

        public string ContainerHtmlTag { get; set; }

        ///<summary>
        /// CSSClasses to append to the &lt;ul&gt; element in the paging control.
        ///</summary>
        public IEnumerable<string> UlElementClasses { get; set; }

        ///<summary>
        /// CSS Classes to append to every &lt;li&gt; element in the paging control.
        ///</summary>
        public IEnumerable<string> LiElementClasses { get; set; }

        public IEnumerable<string> AhrefElementClasses { get; set; }

        public IEnumerable<string> ActiveElementClasses { get; set; }

        public IEnumerable<string> DisabledElementClasses { get; set; }

        ///<summary>
        /// Specifies a CSS class to append to the first list item in the pager. If null or whitespace is defined, no additional class is added to first list item in list.
        ///</summary>
        public string ClassToApplyToFirstListItemInPager { get; set; }

        ///<summary>
        /// Specifies a CSS class to append to the last list item in the pager. If null or whitespace is defined, no additional class is added to last list item in list.
        ///</summary>
        public string ClassToApplyToLastListItemInPager { get; set; }

        /// <summary>
        /// If set to Always, always renders the paging control. If set to IfNeeded, render the paging control when there is more than one page.
        /// </summary>
        public PagedListDisplayMode Display { get; set; }

        ///<summary>
        /// If set to Always, render a hyperlink to the first page in the list. If set to IfNeeded, render the hyperlink only when the first page isn't visible in the paging control.
        ///</summary>
        public PagedListDisplayMode DisplayLinkToFirstPage { get; set; }

        ///<summary>
        /// If set to Always, render a hyperlink to the last page in the list. If set to IfNeeded, render the hyperlink only when the last page isn't visible in the paging control.
        ///</summary>
        public PagedListDisplayMode DisplayLinkToLastPage { get; set; }

        ///<summary>
        /// If set to Always, render a hyperlink to the previous page of the list. If set to IfNeeded, render the hyperlink only when there is a previous page in the list.
        ///</summary>
        public PagedListDisplayMode DisplayLinkToPreviousPage { get; set; }

        ///<summary>
        /// If set to Always, render a hyperlink to the next page of the list. If set to IfNeeded, render the hyperlink only when there is a next page in the list.
        ///</summary>
        public PagedListDisplayMode DisplayLinkToNextPage { get; set; }

        ///<summary>
        /// When true, includes hyperlinks for each page in the list.
        ///</summary>
        public bool DisplayLinkToIndividualPages { get; set; }

        ///<summary>
        /// When true, shows the current page number and the total number of pages in the list.
        ///</summary>
        ///<example>
        /// "Page 3 of 8."
        ///</example>
        public bool DisplayPageCountAndCurrentLocation { get; set; }

        ///<summary>
        /// When true, shows the one-based index of the first and last items on the page, and the total number of items in the list.
        ///</summary>
        ///<example>
        /// "Showing items 75 through 100 of 183."
        ///</example>
        public bool DisplayItemSliceAndTotal { get; set; }

        ///<summary>
        /// The maximum number of page numbers to display. Null displays all page numbers.
        ///</summary>
        public int? MaximumPageNumbersToDisplay { get; set; }

        ///<summary>
        /// If true, adds an ellipsis where not all page numbers are being displayed.
        ///</summary>
        ///<example>
        /// "1 2 3 4 5 ...",
        /// "... 6 7 8 9 10 ...",
        /// "... 11 12 13 14 15"
        ///</example>
        public bool DisplayEllipsesWhenNotShowingAllPageNumbers { get; set; }

        ///<summary>
        /// The pre-formatted text to display when not all page numbers are displayed at once.
        ///</summary>
        ///<example>
        /// "..."
        ///</example>
        public string EllipsesFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display inside the hyperlink to the first page. The one-based index of the page (always 1 in this case) is passed into the formatting function - use {0} to reference it.
        ///</summary>
        ///<example>
        /// "&lt;&lt; First"
        ///</example>
        public string LinkToFirstPageFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display inside the hyperlink to the previous page. The one-based index of the page is passed into the formatting function - use {0} to reference it.
        ///</summary>
        ///<example>
        /// "&lt; Previous"
        ///</example>
        public string LinkToPreviousPageFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display inside the hyperlink to each individual page. The one-based index of the page is passed into the formatting function - use {0} to reference it.
        ///</summary>
        ///<example>
        /// "{0}"
        ///</example>
        public string LinkToIndividualPageFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display inside the hyperlink to the next page. The one-based index of the page is passed into the formatting function - use {0} to reference it.
        ///</summary>
        ///<example>
        /// "Next &gt;"
        ///</example>
        public string LinkToNextPageFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display inside the hyperlink to the last page. The one-based index of the page is passed into the formatting function - use {0} to reference it.
        ///</summary>
        ///<example>
        /// "Last &gt;&gt;"
        ///</example>
        public string LinkToLastPageFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display when DisplayPageCountAndCurrentLocation is true. Use {0} to reference the current page and {1} to reference the total number of pages.
        ///</summary>
        ///<example>
        /// "Page {0} of {1}."
        ///</example>
        public string PageCountAndCurrentLocationFormat { get; set; }

        ///<summary>
        /// The pre-formatted text to display when DisplayItemSliceAndTotal is true. Use {0} to reference the first item on the page, {1} for the last item on the page, and {2} for the total number of items across all pages.
        ///</summary>
        ///<example>
        /// "Showing items {0} through {1} of {2}."
        ///</example>
        public string ItemSliceAndTotalFormat { get; set; }

        /// <summary>
        /// Text that will appear between each page number. If null or whitespace is specified, no delimiter will be shown.
        /// </summary>
        public string DelimiterBetweenPageNumbers { get; set; }
    }
}