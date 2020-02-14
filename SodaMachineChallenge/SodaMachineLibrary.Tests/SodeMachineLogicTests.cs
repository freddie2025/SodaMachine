using Xunit;
using SodaMachineLibrary.Logic;
using SodaMachineLibrary.Models;
using SodaMachineLibrary.Tests.Mocks;
using System.Collections.Generic;
using System.Linq;

namespace SodaMachineLibrary.Tests
{
	public class SodeMachineLogicTests
	{
		[Fact]
		public void AddToCoinInventory_ShouldWork()
		{
			MockDataAccess da = new MockDataAccess();
			SodaMachineLogic logic = new SodaMachineLogic(da);

			List<CoinModel> coins = new List<CoinModel>
			{
				new CoinModel{ Name = "Quarter", Amount = 0.25M },
				new CoinModel{ Name = "Quarter", Amount = 0.25M },
				new CoinModel{ Name = "Dime", Amount = 0.1M }
			};

			logic.AddToCoinInventory(coins);

			int expected = 6;
			int actual = da.CoinInventory.Where(x => x.Name == "Quarter").Count();

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void AddToSodaInventory_ShouldWork()
		{
			MockDataAccess da = new MockDataAccess();
			SodaMachineLogic logic = new SodaMachineLogic(da);

			List<SodaModel> sodas = new List<SodaModel>
			{
				new SodaModel{ Name = "Coke", SlotOccupied = "1" },
				new SodaModel{ Name = "Coke", SlotOccupied = "1" },
			};

			logic.AddToSodaInventory(sodas);

			int expected = 7;
			int actual = da.SodaInventory.Where(x => x.Name == "Coke").Count();

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void EmptyMoneyFromMachine_ShouldWork()
		{
			MockDataAccess da = new MockDataAccess();
			SodaMachineLogic logic = new SodaMachineLogic(da);

			var (_, expected, _) = da.MachineInfo;
			decimal actual = logic.EmptyMoneyFromMachine();

			Assert.Equal(expected, actual);

			expected = 0;
			(_, actual, _) = da.MachineInfo;

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void GetCoinInventory_ShouldWork()
		{
			MockDataAccess da = new MockDataAccess();
			SodaMachineLogic logic = new SodaMachineLogic(da);

			var coins = logic.GetCoinInventory();

			var expected = da.CoinInventory.Where(x => x.Name == "Quarter").Count();
			var actual = coins.Where(x => x.Name == "Quarter").Count();

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void GetCurrentIncome_ShouldWork()
		{
			MockDataAccess da = new MockDataAccess();
			SodaMachineLogic logic = new SodaMachineLogic(da);

			var (_, expected, _) = da.MachineInfo;
			decimal actual = logic.GetCurrentIncome();

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void GetMoneyInsertedTotal_ShouldWork()
		{
			MockDataAccess da = new MockDataAccess();
			SodaMachineLogic logic = new SodaMachineLogic(da);

			decimal expected = 0.65M;
			da.UserCredit.Add("test", expected);

			decimal actual = logic.GetMoneyInsertedTotal("test");

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void GetSodaInventory_ShouldWork()
		{
			MockDataAccess da = new MockDataAccess();
			SodaMachineLogic logic = new SodaMachineLogic(da);

			int actual = logic.GetSodaInventory().Count();
			int expected = 8;

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void GetSodaPrice_ShouldWork()
		{
			MockDataAccess da = new MockDataAccess();
			SodaMachineLogic logic = new SodaMachineLogic(da);

			decimal actual = logic.GetSodaPrice();
			decimal expected = da.MachineInfo.sodaPrice;

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void GetTotalIncome_ShouldWork()
		{
			MockDataAccess da = new MockDataAccess();
			SodaMachineLogic logic = new SodaMachineLogic(da);

			decimal actual = logic.GetTotalIncome();
			decimal expected = da.MachineInfo.sodaPrice;

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void IssueFullRefund_ShouldWork()
		{
			MockDataAccess da = new MockDataAccess();
			SodaMachineLogic logic = new SodaMachineLogic(da);
			string user = "test";

			da.UserCredit[user] = 0.65M;

			logic.IssueFullRefund(user);

			Assert.True(da.UserCredit[user] == 0);
		}

		[Fact]
		public void ListTypesOfSoda_ShouldWork()
		{
			MockDataAccess da = new MockDataAccess();
			SodaMachineLogic logic = new SodaMachineLogic(da);

			var expected = 3;
			int actual = logic.ListTypesOfSoda().Count();

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void MoneyInsert_SingleUserShouldHaveCorrectAmount()
		{
			MockDataAccess da = new MockDataAccess();
			SodaMachineLogic logic = new SodaMachineLogic(da);
			string user = "test";
			decimal money = 0.25M;

			logic.MoneyInserted(user, money);

			decimal expected = money;
			decimal actual = da.UserCredit[user];

			Assert.Equal(expected, actual);

			logic.MoneyInserted(user, money);

			expected += money;
			actual = da.UserCredit[user];

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void MoneyInsert_User2ShouldHaveCorrectAmount()
		{
			MockDataAccess da = new MockDataAccess();
			SodaMachineLogic logic = new SodaMachineLogic(da);
			string user1 = "test";
			string user2 = "fred";
			decimal money = 0.25M;
			decimal user2Money = 0.1M;

			logic.MoneyInserted(user1, money);
			logic.MoneyInserted(user2, user2Money);
			logic.MoneyInserted(user1, money);

			decimal expected = user2Money;
			decimal actual = da.UserCredit[user2];

			Assert.Equal(expected, actual);
		}

		[Fact]
		public void RequestSoda_ShouldReturnSodaWithNoChange()
		{
			MockDataAccess da = new MockDataAccess();
			SodaMachineLogic logic = new SodaMachineLogic(da);

			string user = "test";
			SodaModel expcetedSoda = new SodaModel { Name = "Coke", SlotOccupied = "1" };
			var initialState = da.MachineInfo;

			da.UserCredit[user] = 0.75M;

			var results = logic.RequestSoda(expcetedSoda);

			Assert.Equal(expcetedSoda.Name, results.soda.Name);
			Assert.Equal(expcetedSoda.SlotOccupied, results.soda.SlotOccupied);

			Assert.Equal(0, da.UserCredit[user]);

			Assert.Equal(initialState.cashOnHand + 0.75M, da.MachineInfo.cashOnHand);
			Assert.Equal(initialState.totalIncome + 0.75M, da.MachineInfo.totalIncome);

			Assert.True(string.IsNullOrWhiteSpace(results.errorMessage));

			Assert.True(results.change.Count() == 0);
		}

		[Fact]
		public void RequestSoda_ShouldSayNotEnoughMoney()
		{
			MockDataAccess da = new MockDataAccess();
			SodaMachineLogic logic = new SodaMachineLogic(da);
			
			string user = "test";
			SodaModel expcetedSoda = new SodaModel { Name = "Coke", SlotOccupied = "1" };
			var initialState = da.MachineInfo;

			da.UserCredit[user] = 0.5M;

			var results = logic.RequestSoda(expcetedSoda);

			Assert.Null(results.soda);

			Assert.Equal(0.5M, da.UserCredit[user]);

			Assert.Equal(initialState.cashOnHand, da.MachineInfo.cashOnHand);
			Assert.Equal(initialState.totalIncome, da.MachineInfo.totalIncome);

			Assert.False(string.IsNullOrWhiteSpace(results.errorMessage));

			Assert.True(results.change.Count() == 0);
		}

		[Fact]
		public void RequestSoda_ShouldSayOutOfStock()
		{
			MockDataAccess da = new MockDataAccess();
			SodaMachineLogic logic = new SodaMachineLogic(da);

			string user = "test";
			SodaModel expcetedSoda = new SodaModel { Name = "Fanta", SlotOccupied = "4" };
			var initialState = da.MachineInfo;

			da.UserCredit[user] = 0.75M;

			var results = logic.RequestSoda(expcetedSoda);

			Assert.Null(results.soda);

			Assert.Equal(0.75M, da.UserCredit[user]);

			Assert.Equal(initialState.cashOnHand, da.MachineInfo.cashOnHand);
			Assert.Equal(initialState.totalIncome, da.MachineInfo.totalIncome);

			Assert.False(string.IsNullOrWhiteSpace(results.errorMessage));

			Assert.True(results.change.Count() == 0);
		}

		[Fact]
		public void RequestSoda_ShouldReturnSodaWithChange()
		{
			MockDataAccess da = new MockDataAccess();
			SodaMachineLogic logic = new SodaMachineLogic(da);

			string user = "test";
			SodaModel expcetedSoda = new SodaModel { Name = "Coke", SlotOccupied = "1" };
			var initialState = da.MachineInfo;

			da.UserCredit[user] = 1M;

			var results = logic.RequestSoda(expcetedSoda);

			Assert.Equal(expcetedSoda.Name, results.soda.Name);
			Assert.Equal(expcetedSoda.SlotOccupied, results.soda.SlotOccupied);

			Assert.Equal(0, da.UserCredit[user]);

			Assert.Equal(initialState.cashOnHand + 1M, da.MachineInfo.cashOnHand);
			Assert.Equal(initialState.totalIncome + 1M, da.MachineInfo.totalIncome);

			Assert.True(string.IsNullOrWhiteSpace(results.errorMessage));

			Assert.True(results.change.Count() == 1);
		}

		[Fact]
		public void RequestSoda_ShouldReturnSodaWithAlternativeChange()
		{
			MockDataAccess da = new MockDataAccess();
			SodaMachineLogic logic = new SodaMachineLogic(da);

			string user = "test";
			SodaModel expcetedSoda = new SodaModel { Name = "Coke", SlotOccupied = "1" };
			var initialState = da.MachineInfo;

			da.UserCredit[user] = 1M;
			da.CoinInventory.RemoveAll(x => x.Amount == 0.25M);

			var results = logic.RequestSoda(expcetedSoda);

			Assert.Equal(expcetedSoda.Name, results.soda.Name);
			Assert.Equal(expcetedSoda.SlotOccupied, results.soda.SlotOccupied);

			Assert.Equal(0, da.UserCredit[user]);

			Assert.Equal(initialState.cashOnHand + 1M, da.MachineInfo.cashOnHand);
			Assert.Equal(initialState.totalIncome + 1M, da.MachineInfo.totalIncome);

			Assert.True(string.IsNullOrWhiteSpace(results.errorMessage));

			// Dime + Dime + Nickle = Quarter
			Assert.True(results.change.Count() == 3);
		}

	}
}
