using System;
using System.Collections.Generic;
using System.Linq;

namespace BadgetQuery
{
    public class BudgetQuery
    {
        private readonly IBudgetRepostory _budgetRepostory;
        private List<int> _monthList;

        public BudgetQuery(IBudgetRepostory budgetRepostory)
        {
            _budgetRepostory = budgetRepostory;
        }

        public decimal Query(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate) throw new ArgumentException();
            var totalBudget = 0m;
            _monthList = GetMonthRange(startDate, endDate);

            foreach (var month in _monthList)
            {
                var monthStartDate = IsFirstMonth(month)
                    ? startDate
                    : GetMonthFirstDate(startDate, month);
                var monthEndDate = IsLastMonth(month)
                    ? endDate
                    : GetMonthLastDate(startDate, month);


                totalBudget += QueryInMonthBudgetAmount(monthStartDate, monthEndDate);
            }


            return totalBudget;
        }

        private static DateTime GetMonthLastDate(DateTime startDate, int month)
        {
            return new DateTime(startDate.Year, month, DateTime.DaysInMonth(startDate.Year, month));
        }

        private static DateTime GetMonthFirstDate(DateTime startDate, int month)
        {
            return new DateTime(startDate.Year, month, 1);
        }

        private static List<int> GetMonthRange(DateTime startDate, DateTime endDate)
        {
            return Enumerable.Range(startDate.Month, endDate.Month - startDate.Month + 1).ToList();
        }

        private bool IsLastMonth(int month)
        {
            return month == _monthList.Last();
        }

        private bool IsFirstMonth(int month)
        {
            return month == _monthList.First();
        }

        private decimal QueryInMonthBudgetAmount(DateTime startDate, DateTime endDate)
        {
            var budgets = _budgetRepostory.GetBudgets();
            var dayDiff = new TimeSpan(endDate.Ticks - startDate.Ticks).Days + 1;
            var currentMonthBudget = budgets.FirstOrDefault(it => it.YearMonth == new DateTime(startDate.Year, startDate.Month, 1))?.Amount ?? 0;
            var currentMonthDays = DateTime.DaysInMonth(startDate.Year, startDate.Month);
            var oneDayBudgetAmount = currentMonthBudget / currentMonthDays;
            return dayDiff * oneDayBudgetAmount;
        }
    }
}