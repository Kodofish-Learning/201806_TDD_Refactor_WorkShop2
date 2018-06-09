using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Shouldly;

namespace BadgetQuery
{
    [TestFixture]
    public class BugetQueryTests
    {
        [SetUp]
        public void Setup()
        {
            var _budgetRepostory = Substitute.For<IBudgetRepostory>();
            _budgetRepostory.GetBudgets().Returns(new List<Budget>
            {
                new Budget
                {
                    YearMonth = new DateTime(2018, 6, 1),
                    Amount = 300m
                },
                new Budget
                {
                    YearMonth = new DateTime(2018, 8, 1),
                    Amount = 3100m
                }
            });

            _target = new BudgetQuery(_budgetRepostory);
        }

        private BudgetQuery _target;
        private decimal actual;

        private void AssertBudgetShouldBe(decimal expected)
        {
            actual.ShouldBe(expected);
        }

        private void WhenQueryBudget(DateTime startDate, DateTime endDate)
        {
            actual = _target.Query(startDate, endDate);
        }


        [Test]
        public void Query_from_20180518_to_20180711_cross_3month_should_300()
        {
            WhenQueryBudget(new DateTime(2018, 5, 18), new DateTime(2018, 7, 11));

            AssertBudgetShouldBe(300m);
        }


        [Test]
        public void Query_from_20180601_to_20180601_One_day_in_june_Should_10()
        {
            WhenQueryBudget(new DateTime(2018, 6, 1), new DateTime(2018, 6, 1));
            AssertBudgetShouldBe(10m);
        }


        [Test]
        public void Query_from_20180601_to_20180630_full_month_should_300()
        {
            WhenQueryBudget(new DateTime(2018, 6, 1), new DateTime(2018, 6, 30));

            AssertBudgetShouldBe(300m);
        }


        [Test]
        public void Query_from_20180620_to_20180705_cross_2month_should_110()
        {
            WhenQueryBudget(new DateTime(2018, 6, 20), new DateTime(2018, 7, 5));

            AssertBudgetShouldBe(110m);
        }


        [Test]
        public void Query_from_20180722_to_201808015_cross_2month_should_1500()
        {
            WhenQueryBudget(new DateTime(2018, 7, 22), new DateTime(2018, 8, 15));

            AssertBudgetShouldBe(1500m);
        }
    }
}