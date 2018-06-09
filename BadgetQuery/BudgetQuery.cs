using System;
using System.Collections.Generic;
using System.Linq;

namespace BadgetQuery
{
    public class BudgetQuery
    {
        private readonly IBudgetRepostory _budgetRepostory;
        private List<int> monthList;

        public BudgetQuery(IBudgetRepostory budgetRepostory)
        {
            _budgetRepostory = budgetRepostory;
        }

        public decimal Query(DateTime startDate, DateTime endDate)
        {
            var budgets = _budgetRepostory.GetBudgets();
            if (startDate.Year == endDate.Year && startDate.Month != endDate.Month)
            {
                var totalBudget = 0m;
                monthList = Enumerable.Range(startDate.Month, endDate.Month - startDate.Month + 1).ToList();


                foreach (var month in monthList)
                {
                    var monthStartDate = IsFirstMonth(month)
                        ? startDate
                        : new DateTime(startDate.Year, month, 1);
                    var monthEndDate = IsLastMonth(month)
                        ? endDate
                        : new DateTime(startDate.Year, month, DateTime.DaysInMonth(startDate.Year, month));


                    totalBudget += QueryInMonthBudgetAmount(monthStartDate, monthEndDate, budgets);
                }


                return totalBudget;
            }

            if (startDate.Month == endDate.Month && startDate.Year == endDate.Year) return QueryInMonthBudgetAmount(startDate, endDate, budgets);


            return 10m;
        }

        private bool IsLastMonth(int month)
        {
            return month == monthList.Last();
        }

        private bool IsFirstMonth(int month)
        {
            return month == monthList.First();
        }

        private static decimal QueryInMonthBudgetAmount(DateTime startDate, DateTime endDate, List<Budget> budgets)
        {
            var dayDiff = new TimeSpan(endDate.Ticks - startDate.Ticks).Days + 1;
            var currentMonthBudget = budgets.FirstOrDefault(it => it.YearMonth == new DateTime(startDate.Year, startDate.Month, 1))?.Amount ?? 0;
            var currentMonthDays = DateTime.DaysInMonth(startDate.Year, startDate.Month);
            var oneDayBudgetAmount = currentMonthBudget / currentMonthDays;
            return dayDiff * oneDayBudgetAmount;
        }
    }
}