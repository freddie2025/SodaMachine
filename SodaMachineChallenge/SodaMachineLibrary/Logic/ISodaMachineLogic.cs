using SodaMachineLibrary.Models;
using System.Collections.Generic;

namespace SodaMachineLibrary.Logic
{
	public interface ISodaMachineLogic
	{
		List<SodaModel> ListTypesOfSoda();

		// Takes in an amount and returns the total deposited so far
		decimal MoneyInserted(decimal monetaryAmount);

		// Get the overall price for sodas - there is no individual pricing
		decimal GetSodaPrice();

		// SodaModel (or null), List(of Coin) Change, string ErrorMessage
		(SodaModel soda, List<CoinModel> change, string errorMessage) RequestSoda(SodaModel soda);

		void IssueFullRefund();

		void GetMoneyInsertedTotal();

		void AddToInventory(List<SodaModel> sodas);

		List<SodaModel> GetInventory();

		decimal EmptyMoneyFromMachine();

		List<CoinModel> GetCoinInventory();

		void AddToCoinInventory(List<CoinModel> coins);

		decimal GetCurrentIncome();

		decimal GetTotalincome();
	}
}
