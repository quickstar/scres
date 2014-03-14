using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;

namespace scres
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			string scFolderPath = Path.Combine(myDocumentsPath, "StarCraft II");
			string fullVariablesPath = Path.Combine(scFolderPath, "Variables.txt");
			string variablesContent = File.ReadAllText(fullVariablesPath);
			Console.ForegroundColor = ConsoleColor.Red;

			int monitorHeight = Screen.PrimaryScreen.Bounds.Height;

			Regex regex = new Regex(@"height=(\d+)");
			Match match = regex.Match(variablesContent);

			if (match.Success)
			{
				string fullString = match.Captures[0].Value;
				int configHeight = int.Parse(match.Groups[1].Captures[0].Value);

				if (configHeight != monitorHeight)
				{
					Console.WriteLine("Current Height: {0}", configHeight);
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("Changing to {0}", monitorHeight);
					variablesContent = variablesContent.Replace(fullString, string.Format(CultureInfo.InvariantCulture, "height={0}", monitorHeight));
					Console.ForegroundColor = ConsoleColor.Green;
					File.WriteAllText(fullVariablesPath, variablesContent);
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("Everthing is already goooood to gooo!!!");
				}
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Something bad happened to the config file... go check it!");
				Console.ReadLine();
			}

			string starCraftInstallPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\StarCraft II\", "InstallLocation", string.Empty).ToString();
			if (!string.IsNullOrEmpty(starCraftInstallPath))
			{
				starCraftInstallPath = Path.Combine(starCraftInstallPath, "StarCraft II.exe");
				Process.Start(starCraftInstallPath);
			}
		}
	}
}