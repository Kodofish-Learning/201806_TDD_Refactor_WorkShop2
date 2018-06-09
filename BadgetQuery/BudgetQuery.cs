using System;
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
            if (startDate.Month == endDate.Month && startDate.Year == endDate.Year)
            {
                var dayDiff = new TimeSpan(endDate.Ticks-startDate.Ticks).Days+1;
                var currentMonthBudget = budgets.First(it => it.YearMonth == new DateTime(startDate.Year, startDate.Month, 1));
                var currentMonthDays = DateTime.DaysInMonth(startDate.Year, startDate.Month);
                var oneDayBudgetAmount = currentMonthBudget.Amount / currentMonthDays;
                return dayDiff * oneDayBudgetAmount;
            }

            return 10m;
        }
    }
}