using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using Shouldly;

namespace BadgetQuery
{
    [TestFixture]
    public class BugetQueryTests
    {
        private BudgetQuery _target;

        [SetUp]
        public void Setup()
        {
            IBudgetRepostory _budgetRepostory = NSubstitute.Substitute.For<IBudgetRepostory>();
            _budgetRepostory.GetBudgets().Returns(new List<Budget>()
            {
                new Budget()
                {
                    YearMonth = new DateTime(2018,6, 1),
                    Amount = 300m
                },
                new Budget()
                {
                    YearMonth = new DateTime(2018, 8, 1)
                    , Amount = 3100m
                }

            });

            _target = new BudgetQuery(_budgetRepostory);

        }


        [Test]
        public void Query_Query_from_20180601_to_20180601_One_day_in_june_Should_10()
        {
            decimal actual = _target.Query(new DateTime(2018, 6, 1), new DateTime(2018, 6, 1));
            actual.ShouldBe(10m);
        }
    }
}
