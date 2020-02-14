using SodaMachineLibrary.DataAccess;
using SodaMachineLibrary.Models;
using System;
using System.Collections.Generic;

namespace SodaMachineLibrary.Logic
{
	public class SodaMachineLogic : ISodaMachineLogic
	{
		public SodaMachineLogic(IDataAccess dataAccess)
		{

		}

		public void AddToCoinInventory(List<CoinModel> coins)
		{
			throw new NotImplementedException();
		}

		public void AddToSodaInventory(List<SodaModel> sodas)
		{
			throw new NotImplementedException();
		}

		public decimal EmptyMoneyFromMachine()
		{
			throw new NotImplementedException();
		}

		public List<CoinModel> GetCoinInventory()
		{
			throw new NotImplementedException();
		}

		public decimal GetCurrentIncome()
		{
			throw new NotImplementedException();
		}

		public List<SodaModel> GetSodaInventory()
		{
			throw new NotImplementedException();
		}

		public decimal GetMoneyInsertedTotal(string userId)
		{
			throw new NotImplementedException();
		}

		public decimal GetSodaPrice()
		{
			throw new NotImplementedException();
		}

		public decimal GetTotalIncome()
		{
			throw new NotImplementedException();
		}

		public void IssueFullRefund(string userId)
		{
			throw new NotImplementedException();
		}

		public List<SodaModel> ListTypesOfSoda()
		{
			throw new NotImplementedException();
		}

		public decimal MoneyInserted(string userId, decimal monetaryAmount)
		{
			throw new NotImplementedException();
		}

		public (SodaModel soda, List<CoinModel> change, string errorMessage) RequestSoda(SodaModel soda)
		{
			throw new NotImplementedException();
		}
	}
}
