using SodaMachineLibrary.Models;
using System.Collections.Generic;

namespace SodaMachineLibrary.DataAccess
{
	public interface IDataAccess
	{
		List<SodaModel> SodaInventory_GetTypes();
		SodaModel SodaInventory_GetSoda(SodaModel soda);
		void SodaInventory_AddSoda(List<SodaModel> sodas);
		List<SodaModel> SodaInventory_GetAll();
		void UserCredit_Insert(string userId, decimal amount);
		void UserCredit_Clear(string userId);
		decimal UserCredit_Total(string userId);
		void UserCredit_Deposit(string userId);
		decimal MachineInfo_SodaPrice();
		decimal MachineInfo_EmptyCash();
		decimal MachineInfo_CashOnHand();
		decimal MachineInfo_TotalIncome();
		List<CoinModel> CoinInventory_WithdrawCoin(decimal coinValue, int quantity);
		List<CoinModel> CoinInventory_GetAll();
		void CoinInventory_AddCoins(List<CoinModel> coins);
	}
}
