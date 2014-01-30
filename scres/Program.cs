using System;
using System.IO;

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

			if (variablesContent.Contains("height=1200"))
			{
				Console.WriteLine("Current Height: 1200");
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Changing to 1080");
				variablesContent = variablesContent.Replace("height=1200", "height=1080");
			}
			else
			{
				Console.WriteLine("Current Height: 1080");
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Changing to 1200");
				variablesContent = variablesContent.Replace("height=1080", "height=1200");
			}

			Console.ForegroundColor = ConsoleColor.Green;
			File.WriteAllText(fullVariablesPath, variablesContent);

			Console.WriteLine("Variables.txt successfully changed");
		}
	}
}