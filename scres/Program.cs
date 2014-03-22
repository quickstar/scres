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
		private const string VariablesTxt = "Variables.txt";
		private const string D3PrefsTxt = "D3Prefs.txt";

		private static void Main(string[] args)
		{
			string myDocumentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			string scFolderPath = Path.Combine(myDocumentsPath, "StarCraft II");
			string diabloFolderPath = Path.Combine(myDocumentsPath, "Diablo III");

			string scConfigContent = ReadFileContent(scFolderPath, VariablesTxt);
			string diabloConfigContent = ReadFileContent(diabloFolderPath, D3PrefsTxt);
			int monitorHeight = Screen.PrimaryScreen.Bounds.Height;

			Console.ForegroundColor = ConsoleColor.Red;			

			Regex scRegex = new Regex(@"(height)(=)()(\d+)");
			Regex diabloRegex = new Regex(@"(DisplayModeUIOptHeight)( )("")(\d+)\""");

			Match scMatch = scRegex.Match(scConfigContent);
			Match dibloMatch = diabloRegex.Match(diabloConfigContent);

			ReplaceMatch(scMatch, monitorHeight, scConfigContent, scFolderPath, VariablesTxt);
			ReplaceMatch(dibloMatch, monitorHeight, diabloConfigContent, diabloFolderPath, D3PrefsTxt);

			string starCraftInstallPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\StarCraft II\", "InstallLocation", string.Empty).ToString();
			if (!string.IsNullOrEmpty(starCraftInstallPath))
			{
				starCraftInstallPath = Path.Combine(starCraftInstallPath, "StarCraft II.exe");
				Process.Start(starCraftInstallPath);
			}
		}

		private static void ReplaceMatch(Match scMatch, int monitorHeight, string scConfigContent, string scFolderPath, string fileName)
		{
			if (scMatch.Success)
			{
				string fullString = scMatch.Captures[0].Value;

				string variablesName = scMatch.Groups[1].Captures[0].Value;
				string delimiter = scMatch.Groups[2].Captures[0].Value;
				string valueEnclosingCharacter = scMatch.Groups[3].Captures[0].Value;
				int configHeight = int.Parse(scMatch.Groups[4].Captures[0].Value);

				if (configHeight != monitorHeight)
				{
					Console.WriteLine("Current Height: {0}", configHeight);
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine("Changing to {0}", monitorHeight);
					scConfigContent = scConfigContent.Replace(fullString, string.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}{2}", variablesName, delimiter, valueEnclosingCharacter, monitorHeight));
					Console.ForegroundColor = ConsoleColor.Green;
					WriteAllText(scFolderPath, fileName, scConfigContent);
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
		}

		private static void WriteAllText(string path, string fileName, string content)
		{
			string fullVariablesPath = Path.Combine(path, fileName);
			File.WriteAllText(fullVariablesPath, content);
		}

		private static string ReadFileContent(string path, string fileName)
		{
			string fullVariablesPath = Path.Combine(path, fileName);
			return File.ReadAllText(fullVariablesPath);
		}
	}
}