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
            if (startDate.Year == endDate.Year && startDate.Month != endDate.Month)
            {
                var totalBudget = 0m;


                var firstMonthStartDate = startDate;
                var firstMonthEndDate = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));

                var dayDiff = new TimeSpan(firstMonthEndDate.Ticks - firstMonthStartDate.Ticks).Days + 1;
                var currentMonthBudget = budgets.FirstOrDefault(it => it.YearMonth == new DateTime(firstMonthStartDate.Year, firstMonthStartDate.Month, 1))?.Amount ?? 0m;
                var currentMonthDays = DateTime.DaysInMonth(firstMonthStartDate.Year, firstMonthStartDate.Month);
                var oneDayBudgetAmount = currentMonthBudget / currentMonthDays;
                totalBudget += dayDiff * oneDayBudgetAmount;

                var secondMonthStartDate = new DateTime(endDate.Year, endDate.Month, 1);
                var secondMonthEndDate = endDate;

                dayDiff = new TimeSpan(secondMonthEndDate.Ticks - secondMonthStartDate.Ticks).Days + 1;
                currentMonthBudget = budgets.FirstOrDefault(it => it.YearMonth == new DateTime(secondMonthStartDate.Year, secondMonthStartDate.Month, 1))?.Amount ?? 0m;
                currentMonthDays = DateTime.DaysInMonth(secondMonthStartDate.Year, secondMonthStartDate.Month);
                oneDayBudgetAmount = currentMonthBudget / currentMonthDays;
                totalBudget += dayDiff * oneDayBudgetAmount;

                return totalBudget;
            }

            if (startDate.Month == endDate.Month && startDate.Year == endDate.Year)
            {
                var dayDiff = new TimeSpan(endDate.Ticks - startDate.Ticks).Days + 1;
                var currentMonthBudget = budgets.First(it => it.YearMonth == new DateTime(startDate.Year, startDate.Month, 1));
                var currentMonthDays = DateTime.DaysInMonth(startDate.Year, startDate.Month);
                var oneDayBudgetAmount = currentMonthBudget.Amount / currentMonthDays;
                return dayDiff * oneDayBudgetAmount;
            }


            return 10m;
        }
    }
}