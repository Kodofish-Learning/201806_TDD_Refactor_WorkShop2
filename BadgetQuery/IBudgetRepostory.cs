using System.Collections.Generic;

namespace BadgetQuery
{
    public interface IBudgetRepostory
    {
        List<Budget> GetBudgets();
    }
}