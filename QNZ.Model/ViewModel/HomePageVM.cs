using System;
using System.Collections.Generic;
using System.Text;

namespace QNZ.Model.Admin.ViewModel
{
    public class HomePageVM
    {
        public int FeedbackTodayCount { get; set; }
        public int FeedbackCount { get; set; }

        public int CustomerXCXTodayCount { get; set; }
        public int CustomerXCXCount { get; set; }
        public int CustomerGZHTodayCount { get; set; }
        public int CustomerGZHCount { get; set; }

        public decimal TodayRevenue { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class StatisticsVM
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int CustomerXCXTodayCount { get; set; }
        public int CustomerXCXCount { get; set; }
        public int CustomerXCXCustomCount { get; set; }
        public int CustomerGZHTodayCount { get; set; }
        public int CustomerGZHCount { get; set; }
        public int CustomerGZHCustomCount { get; set; }

        public decimal TodayRevenue { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal CustomRevenue { get; set; }


    }

}
