using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SodaMachineLibrary.DataAccess;
using SodaMachineLibrary.Logic;

namespace SodaMachineConsoleUI
{
	class Program
	{
		private static IServiceProvider _serviceProvider;

		static void Main(string[] args)
		{
			RegisterServices();
			string userSelection = "";

			Console.WriteLine("Welcome to our Soda Machine.");

			do
			{
				userSelection = ShowMenu();

				switch (userSelection)
				{
					case "1": // Show Soda Price
						break;
					case "2": // List Soda Options
						break;
					case "3": // Show Amount Deposited
						break;
					case "4": // Deposit Money
						break;
					case "5": // Cancel Transaction
						break;
					case "6": // Request Soda
						break;
					case "9": // Close Machine
						// Don't do anything - allow this to go to the while, which will close the loop. 
						break;
					default:
						// Don't do anything - allow this to go to the while, which will restart the loop.
						break;
				}

				Console.Clear();

			} while (userSelection != "9");

			Console.WriteLine("Thanks, have a nice day.");
			Console.WriteLine("Press return to quit...");

			Console.ReadLine();
		}

		private static string ShowMenu()
		{
			Console.WriteLine("Please make a selection from the following options:");
			Console.WriteLine("1: Show Soda Price");
			Console.WriteLine("2: List Soda Options");
			Console.WriteLine("3: Show Amount Deposited");
			Console.WriteLine("4: Deposit Money");
			Console.WriteLine("5: Cancel Transaction");
			Console.WriteLine("6: Request Soda");
			Console.WriteLine("9: Close Machine");

			return Console.ReadLine();
		}

		private static void RegisterServices()
		{
			var collection = new ServiceCollection();

			IConfiguration config = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", true, true)
				.Build();

			collection.AddSingleton(config);
			collection.AddTransient<IDataAccess, TextFileDataAccess>();
			collection.AddTransient<ISodaMachineLogic, SodaMachineLogic>();

			_serviceProvider = collection.BuildServiceProvider();
		}
	}
}
