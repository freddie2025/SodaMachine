using SodaMachineLibrary.DataAccess;
using SodaMachineLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SodaMachineLibrary.Tests.Mocks
{
	public class MockDataAccess : IDataAccess
	{
		public List<CoinModel> CoinInventory { get; set; } = new List<CoinModel>();
		public (decimal sodaPrice, decimal cashOnHand, decimal totalIncome) MachineInfo { get; set; }
		public List<SodaModel> SodaInventory { get; set; } = new List<SodaModel>();
		public Dictionary<string, decimal> UserCredit { get; set; } = new Dictionary<string, decimal>();

		public MockDataAccess()
		{
			CoinInventory.Add(new CoinModel { Name = "Quarter", Amount = 0.25M });
			CoinInventory.Add(new CoinModel { Name = "Quarter", Amount = 0.25M });
			CoinInventory.Add(new CoinModel { Name = "Quarter", Amount = 0.25M });
			CoinInventory.Add(new CoinModel { Name = "Quarter", Amount = 0.25M });
			CoinInventory.Add(new CoinModel { Name = "Dime", Amount = 0.1M });
			CoinInventory.Add(new CoinModel { Name = "Dime", Amount = 0.1M });
			CoinInventory.Add(new CoinModel { Name = "Nickle", Amount = 0.05M });
			CoinInventory.Add(new CoinModel { Name = "Nickle", Amount = 0.05M });
			CoinInventory.Add(new CoinModel { Name = "Nickle", Amount = 0.05M });
			CoinInventory.Add(new CoinModel { Name = "Nickle", Amount = 0.05M });
			CoinInventory.Add(new CoinModel { Name = "Nickle", Amount = 0.05M });

			MachineInfo = (0.75M, 25.65M, 201.50M);

			SodaInventory.Add(new SodaModel { Name = "Coke", SlotOccupied = "1" });
			SodaInventory.Add(new SodaModel { Name = "Coke", SlotOccupied = "1" });
			SodaInventory.Add(new SodaModel { Name = "Coke", SlotOccupied = "1" });
			SodaInventory.Add(new SodaModel { Name = "Coke", SlotOccupied = "1" });
			SodaInventory.Add(new SodaModel { Name = "Coke", SlotOccupied = "1" });
			SodaInventory.Add(new SodaModel { Name = "Diet Coke", SlotOccupied = "2" });
			SodaInventory.Add(new SodaModel { Name = "Sprite", SlotOccupied = "3" });
			SodaInventory.Add(new SodaModel { Name = "Sprite", SlotOccupied = "3" });
		}

		public void CoinInventory_AddCoins(List<CoinModel> coins)
		{
			CoinInventory.AddRange(coins);
		}

		public List<CoinModel> CoinInventory_GetAll()
		{
			return CoinInventory;
		}

		public List<CoinModel> CoinInventory_WithdrawCoin(decimal coinValue, int quantity)
		{
			var coins = CoinInventory.Where(x => x.Amount == coinValue).Take(quantity).ToList();

			coins.ForEach(x => CoinInventory.Remove(x));

			return coins;
		}

		public decimal MachineInfo_CashOnHand()
		{
			return MachineInfo.cashOnHand;
		}

		public decimal MachineInfo_EmptyCash()
		{
			var val = MachineInfo;
			decimal output = val.cashOnHand;

			val.cashOnHand = 0;
			MachineInfo = val;

			return output;
		}

		public decimal MachineInfo_SodaPrice()
		{
			return MachineInfo.sodaPrice;
		}

		public decimal MachineInfo_TotalIncome()
		{
			return MachineInfo.totalIncome;
		}

		public void SodaInventory_AddSoda(List<SodaModel> sodas)
		{
			SodaInventory.AddRange(sodas);
		}

		public List<SodaModel> SodaInventory_GetAll()
		{
			return SodaInventory;
		}

		public bool SodaInventory_CheckIfSodaInStock(SodaModel soda)
		{
			var outputSoda = SodaInventory.Where(x => x.Name == soda.Name).FirstOrDefault();

			return outputSoda != null;
		}

		public SodaModel SodaInventory_GetSoda(SodaModel soda, decimal amount)
		{
			var outputSoda = SodaInventory.Where(x => x.Name == soda.Name).FirstOrDefault();

			if (outputSoda != null)
			{ 
				var info = MachineInfo;
				info.cashOnHand += amount;
				info.totalIncome += amount;
				MachineInfo = info;
			}

			return outputSoda;
		}

		public List<SodaModel> SodaInventory_GetTypes()
		{
			return SodaInventory.GroupBy(x => x.Name)
								.Select(x => x.First())
								.ToList();
		}

		public void UserCredit_Clear(string userId)
		{
			if (UserCredit.ContainsKey(userId))
			{
				UserCredit[userId] = 0;
			}
		}

		public void UserCredit_Deposit(string userId)
		{
			if (UserCredit.ContainsKey(userId) == false)
			{
				throw new Exception("User not found for deposit");
			}
			
			var info = MachineInfo;
			info.cashOnHand += UserCredit[userId];
			info.totalIncome += UserCredit[userId];

			UserCredit.Remove(userId);
		}

		public void UserCredit_Insert(string userId, decimal amount)
		{
			UserCredit.TryGetValue(userId, out decimal currentCredit);
			UserCredit[userId] = currentCredit + amount;
		}

		public decimal UserCredit_Total(string userId)
		{
			UserCredit.TryGetValue(userId, out decimal currentCredit);
			return currentCredit;  
		}
	}
}
