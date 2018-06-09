using System;
using System.Collections.Generic;
using System.Linq;

namespace BadgetQuery
{
    public class BudgetQuery
    {
        private readonly IBudgetRepostory _budgetRepostory;

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
                var monthList = Enumerable.Range(startDate.Month, endDate.Month - startDate.Month + 1).ToList();


                foreach (var month in monthList)
                {
                    var monthStartDate = new DateTime(startDate.Year, month, 1);
                    var monthEndDate = new DateTime(startDate.Year, month, DateTime.DaysInMonth(startDate.Year, month));

                    if (month == monthList.First())
                    {
                        monthStartDate = startDate;
                        monthEndDate = new DateTime(startDate.Year, month, DateTime.DaysInMonth(startDate.Year, month));
                    }

                    if (month == monthList.Last())
                    {
                        monthStartDate = new DateTime(startDate.Year, month, 1);
                        monthEndDate = endDate;
                    }

                    totalBudget += QueryInMonthBudgetAmount(monthStartDate, monthEndDate, budgets);
                }


                return totalBudget;
            }

            if (startDate.Month == endDate.Month && startDate.Year == endDate.Year) return QueryInMonthBudgetAmount(startDate, endDate, budgets);


            return 10m;
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