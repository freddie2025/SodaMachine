using Microsoft.Extensions.Configuration;
using SodaMachineLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SodaMachineLibrary.DataAccess
{
	public class TextFileDataAccess : IDataAccess
	{
		private readonly IConfiguration _config;
		private readonly string coinTextFile;
		private readonly string sodaTextFile;
		private readonly string machineInfoTextFile;
		private readonly string userCreditTextFile;

		public TextFileDataAccess(IConfiguration config)
		{
			_config = config;
			coinTextFile = _config.GetValue<string>("TextFileStorage.Coins");
			sodaTextFile = _config.GetValue<string>("TextFileStorage.Soda");
			machineInfoTextFile = _config.GetValue<string>("TextFileStorage.MachineInfo");
			userCreditTextFile = _config.GetValue<string>("TextFileStorage.UserCredit");
		}

		private List<CoinModel> RetrieveCoins()
		{
			List<CoinModel> output = new List<CoinModel>();

			var lines = File.ReadAllLines(coinTextFile);

			try
			{
				foreach (var line in lines)
				{
					var data = line.Split(',');
					output.Add(new CoinModel
					{
						Name = data[0],
						Amount = decimal.Parse(data[1])
					});
				}
			}
			catch (IndexOutOfRangeException ex)
			{
				throw new Exception("Invalid data in the Coin text file.", ex);
			}
			catch (FormatException ex)
			{
				throw new Exception("The data in the Coin text file has been corrupted.", ex);
			}
			catch 
			{
				throw;
			}

			return output;
		}

		private void SaveCoins(List<CoinModel> coins)
		{
			File.WriteAllLines(coinTextFile, coins.Select(x => $"{ x.Name }, { x.Amount }"));
		}

		public void CoinInventory_AddCoins(List<CoinModel> coins)
		{
			coins.AddRange(RetrieveCoins());

			SaveCoins(coins);
		}

		public List<CoinModel> CoinInventory_GetAll()
		{
			return RetrieveCoins();
		}

		public List<CoinModel> CoinInventory_WithdrawCoin(decimal coinValue, int quantity)
		{
			var coins = RetrieveCoins();

			var coinsToReturn = coins.Where(x => x.Amount == coinValue).Take(quantity).ToList();

			coinsToReturn.ForEach(x => coins.Remove(x));

			SaveCoins(coins);

			return coinsToReturn;
		}

		private (decimal sodaPrice, decimal cashOnHand, decimal totalIncome) RetrieveMachineInfo()
		{
			(decimal sodaPrice, decimal cashOnHand, decimal totalIncome) output;
			var lines = File.ReadAllLines(machineInfoTextFile);

			try
			{
				output.sodaPrice = decimal.Parse(lines[0]);
				output.cashOnHand = decimal.Parse(lines[1]);
				output.totalIncome = decimal.Parse(lines[2]);
			}
			catch (IndexOutOfRangeException ex)
			{
				throw new Exception("Invalid data in the Machine Info text file.", ex);
			}
			catch (FormatException ex)
			{
				throw new Exception("The data in the Machine Info text file has been corrupted.", ex);
			}
			catch
			{
				throw;
			}

			return output;
		}

		private void SaveMachineInfo(decimal sodaPrice, decimal cashOnHand, decimal totalIncome)
		{
			string[] infoToSave = new string[] { sodaPrice.ToString(), cashOnHand.ToString(), totalIncome.ToString() };

			File.WriteAllLines(machineInfoTextFile , infoToSave);
		}

		public decimal MachineInfo_CashOnHand()
		{
			var info = RetrieveMachineInfo();

			return info.cashOnHand;
		}

		public decimal MachineInfo_EmptyCash()
		{
			var info = RetrieveMachineInfo();

			SaveMachineInfo(info.sodaPrice, 0, info.totalIncome);

			return info.cashOnHand;
		}

		public decimal MachineInfo_SodaPrice()
		{
			var info = RetrieveMachineInfo();

			return info.sodaPrice;
		}

		public decimal MachineInfo_TotalIncome()
		{
			var info = RetrieveMachineInfo();

			return info.totalIncome;
		}

		private List<SodaModel> RetrieveSodas()
		{
			List<SodaModel> output = new List<SodaModel>();

			var lines = File.ReadAllLines(sodaTextFile);

			try
			{
				foreach (var line in lines)
				{
					var data = line.Split(',');
					output.Add(new SodaModel
					{
						Name = data[0],
						SlotOccupied  = data[1]
					});
				}
			}
			catch (IndexOutOfRangeException ex)
			{
				throw new Exception("Invalid data in the Soda text file.", ex);
			}
			catch (FormatException ex)
			{
				throw new Exception("The data in the Soda text file has been corrupted.", ex);
			}
			catch
			{
				throw;
			}

			return output;
		}

		private void SaveSodas(List<SodaModel> sodas)
		{
			File.WriteAllLines(sodaTextFile, sodas.Select(x => $"{ x.Name },{ x.SlotOccupied }"));
		}

		public void SodaInventory_AddSoda(List<SodaModel> sodas)
		{
			sodas.AddRange(RetrieveSodas());
			SaveSodas(sodas);
		}

		public bool SodaInventory_CheckIfSodaInStock(SodaModel soda)
		{
			var sodas = RetrieveSodas();
			var outputSodas = sodas.Where(x => x.Name == soda.Name && x.SlotOccupied == soda.SlotOccupied).FirstOrDefault();

			return outputSodas != null;
		}

		public List<SodaModel> SodaInventory_GetAll()
		{
			return RetrieveSodas();
		}

		public SodaModel SodaInventory_GetSoda(SodaModel soda, decimal amount)
		{
			var sodas = RetrieveSodas();
			var outputSoda = sodas.Where(x => x.Name == soda.Name && x.SlotOccupied == soda.SlotOccupied).FirstOrDefault();

			if (outputSoda != null)
			{
				var info = RetrieveMachineInfo();
				info.cashOnHand += amount;
				info.totalIncome += amount;
				SaveMachineInfo(info.sodaPrice, info.cashOnHand, info.totalIncome);
			}

			return outputSoda;
		}

		public List<SodaModel> SodaInventory_GetTypes()
		{
			var sodas = RetrieveSodas();

			return sodas.GroupBy(x => x.Name)
						.Select(x => x.First())
						.ToList();
		}

		private Dictionary<string, decimal> RetrieveUserCredit()
		{
			Dictionary<string, decimal> output = new Dictionary<string, decimal>();

			var lines = File.ReadAllLines(userCreditTextFile);

			try
			{
				foreach (var line in lines)
				{
					var data = line.Split(',');
					output.Add(data[0], decimal.Parse(data[1]));
				}
			}
			catch (IndexOutOfRangeException ex)
			{
				throw new Exception("Invalid data in the User Credit text file.", ex);
			}
			catch (FormatException ex)
			{
				throw new Exception("The data in the User Credit text file has been corrupted.", ex);
			}
			catch
			{
				throw;
			}

			return output;
		}

		private void SaveUserCredit(Dictionary<string, decimal> credits)
		{
			File.WriteAllLines(userCreditTextFile, credits.Select(x => $"{ x.Key }, { x.Value }"));
		}

		public void UserCredit_Clear(string userId)
		{
			var credits = RetrieveUserCredit();
			credits[userId] = 0;
			SaveUserCredit(credits);
		}

		public void UserCredit_Deposit(string userId)
		{
			var credits = RetrieveUserCredit();

			if (credits.ContainsKey(userId) == false)
			{
				throw new Exception("User not found for deposit");
			}

			var info = RetrieveMachineInfo();
			info.cashOnHand += credits[userId];
			info.totalIncome += credits[userId];

			SaveMachineInfo(info.sodaPrice, info.cashOnHand, info.totalIncome);

			credits.Remove(userId);

			SaveUserCredit(credits);
		}

		public void UserCredit_Insert(string userId, decimal amount)
		{
			var credits = RetrieveUserCredit();

			credits.TryGetValue(userId, out decimal currentCredit);
			credits[userId] = currentCredit + amount;

			SaveUserCredit(credits);
		}

		public decimal UserCredit_Total(string userId)
		{
			var credits = RetrieveUserCredit();

			credits.TryGetValue(userId, out decimal currentCredit);
			return currentCredit;
		}
	}
}
