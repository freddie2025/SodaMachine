using SodaMachineLibrary.DataAccess;
using SodaMachineLibrary.Models;
using System;
using System.Collections.Generic;

namespace SodaMachineLibrary.Logic
{
	public class SodaMachineLogic : ISodaMachineLogic
	{
		private readonly IDataAccess _db;

		public SodaMachineLogic(IDataAccess dataAccess)
		{
			_db = dataAccess;
		}

		public void AddToCoinInventory(List<CoinModel> coins)
		{
			_db.CoinInventory_AddCoins(coins);
		}

		public void AddToSodaInventory(List<SodaModel> sodas)
		{
			_db.SodaInventory_AddSoda(sodas);
		}

		public decimal EmptyMoneyFromMachine()
		{
			return _db.MachineInfo_EmptyCash();
		}

		public List<CoinModel> GetCoinInventory()
		{
			return _db.CoinInventory_GetAll();
		}

		public decimal GetCurrentIncome()
		{
			return _db.MachineInfo_CashOnHand();
		}

		public List<SodaModel> GetSodaInventory()
		{
			return _db.SodaInventory_GetAll();
		}

		public decimal GetMoneyInsertedTotal(string userId)
		{
			return _db.UserCredit_Total(userId);
		}

		public decimal GetSodaPrice()
		{
			return _db.MachineInfo_SodaPrice();
		}

		public decimal GetTotalIncome()
		{
			return _db.MachineInfo_TotalIncome();
		}

		public void IssueFullRefund(string userId)
		{
			_db.UserCredit_Clear(userId);
		}

		public List<SodaModel> ListTypesOfSoda()
		{
			return _db.SodaInventory_GetTypes();
		}

		public decimal MoneyInserted(string userId, decimal monetaryAmount)
		{
			_db.UserCredit_Insert(userId, monetaryAmount);

			return _db.UserCredit_Total(userId);
		}

		public (SodaModel soda, List<CoinModel> change, string errorMessage) RequestSoda(SodaModel soda, string userId)
		{
			(SodaModel soda, List<CoinModel> change, string errorMessage) output = (null, new List<CoinModel>(), "An unexpected error occurred.");
			decimal userCredit = _db.UserCredit_Total(userId);
			decimal sodaCost = _db.MachineInfo_SodaPrice();

			if (userCredit >= sodaCost) // No change needed
			{
				bool sodaInStock = _db.SodaInventory_CheckIfSodaInStock(soda);

				if (sodaInStock == false)
				{
					output.errorMessage = "Soda not in inventory.";
				}
				else // Get Change
				{
					try
					{
						var change = GetChange(sodaCost, userCredit);

						var sodaToReturn = _db.SodaInventory_GetSoda(soda, userCredit);
						_db.UserCredit_Clear(userId);
						output = (sodaToReturn, change, "");
					}
					catch (Exception ex)
					{
						output.errorMessage =  "There is not enough coins to give you change.";
					}
				}
			}
			else // Not enough money
			{
				output = (null, new List<CoinModel>(), "User did not provide enough change.");
			}

			return output;
		}

		private List<CoinModel> GetChange(decimal sodaCost, decimal userCredit)
		{
			decimal difference = userCredit - sodaCost;
			List<CoinModel> output = new List<CoinModel>();

			if (difference > 0)
			{
				int quarterCount = (int)Math.Floor(difference / 0.25M);
				var quarters = _db.CoinInventory_WithdrawCoin(0.25M, quarterCount);
				output.AddRange(quarters);
				difference -= (quarters.Count * 0.25M);

				int dimeCount = (int)Math.Floor(difference / 0.1M);
				var dimes = _db.CoinInventory_WithdrawCoin(0.1M, dimeCount);
				output.AddRange(dimes);
				difference -= (dimes.Count * 0.1M);

				int nickleCount = (int)Math.Floor(difference / 0.05M);
				var nickles = _db.CoinInventory_WithdrawCoin(0.05M, nickleCount);
				output.AddRange(nickles);
				difference -= (nickles.Count * 0.05M);

				if (difference > 0)
				{
					throw new Exception("Could not make proper change.");
				}
			}

			return output;
		}
	}
}
