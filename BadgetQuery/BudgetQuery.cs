using System;

namespace BadgetQuery
{
    public class BudgetQuery
    {
        private readonly IBudgetRepostory _budgetRepostory;

        public BudgetQuery(IBudgetRepostory budgetRepostory)
        {
            _budgetRepostory = budgetRepostory;
        }

        public decimal Query(DateTime dateTime, DateTime dateTime1)
        {
            throw new NotImplementedException();
        }
    }
}