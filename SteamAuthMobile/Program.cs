using System;
using System.Collections.Generic;
using System.Linq;
using SteamAuth;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace SteamAuthCode
{
	class Program
	{
		static void Main(string[] args)
		{
			ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => { return true; };
			
			if (args.Length > 0){
				string guardFile = System.IO.File.ReadAllText(args[0] + ".maFile");
				SteamGuardAccount guard = JsonConvert.DeserializeObject<SteamGuardAccount>(guardFile);
				switch (args[1])
				{
				case "code":
					{
						while (true)
						{
							Console.WriteLine("{0}", guard.GenerateSteamGuardCode());
							System.Threading.Thread.Sleep(30000);
						}
					}
				case "trades":
					{
						while (true)
						{
							if (guard != null)
								guard.RefreshSession();
							Confirmation[] tradeRequests = guard.FetchConfirmations();
							foreach (Confirmation conf in tradeRequests)
							{
								guard.AcceptConfirmation(conf);
							}
							System.Threading.Thread.Sleep(20000);
						}
					}
				case "remove":
					{
						guard.DeactivateAuthenticator();
						Console.Write("Authenticator remvoed successfully");
						break;
					}
				}
			}
		}
	}
}
