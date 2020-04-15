using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SodaMachineLibrary.Logic;
using SodaMachineLibrary.Models;

namespace SodaMachineRazorUI.Pages
{
    [Authorize(Roles = "Admin")]
    public class CoinInventoryModel : PageModel
    {
        private readonly ISodaMachineLogic sodaMachine;

        [BindProperty]
        public CoinModel CoinToAdd { get; set; } = new CoinModel();

        public List<CoinModel> Coins { get; set; }

        public CoinInventoryModel(ISodaMachineLogic sodaMachine)
        {
            this.sodaMachine = sodaMachine;
        }
        public void OnGet()
        {
            Coins = this.sodaMachine.GetCoinInventory().OrderBy(x => x.Name).ToList();
        }

        public IActionResult OnPost()
        {
            this.sodaMachine.AddToCoinInventory(new List<CoinModel> { CoinToAdd });
            return RedirectToPage();

        }
    }
}
