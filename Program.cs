using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace MCRPC
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			//Init
			Console.Title = "MCRPC";
			Console.WriteLine ("Welcome to the Minecraft Resource Pack Creator 'MCRPC'");
			Console.WriteLine ("");

			//Get the necessary data
			Console.Write ("Please enter the name of your resource pack: "); //Name
			var name = Console.ReadLine (); //Input

			//Remove invalid chars from foldername
			string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

			foreach (char c in invalid)
			{
				name = name.Replace(c.ToString(), "");
			}

			Console.WriteLine (""); //Newline
			Console.Write (@"Please enter the description (use \n as newline) of your resource pack: "); //Description
			var desc = Console.ReadLine (); //Input
			Console.WriteLine (""); //Newline
			Console.Write("Please enter the pack format (1 for 1.8 or earlier, 2 for 1.9 or later). Leave blank to use the default value: "); //pack format
			String packformat = Console.ReadLine ();
			Console.WriteLine (""); //Newline
			Console.WriteLine ("Summary: \n{0}\n{1}\n{2}", name, desc, packformat); //Summary
			Console.WriteLine (""); //Newline

			if (packformat.Equals ("1") || packformat.Equals ("2")) {

			} else {
				packformat = "1";
			}

			//Path & Files
			var appdata = System.Environment.GetEnvironmentVariable ("AppData"); //get appdata path
			var rppath = appdata + @"\.minecraft\resourcepacks\";
			var packpath = rppath + name + @"\";

			try {
				if (Directory.Exists(packpath)){
					Console.WriteLine ("Directory {0} already exists!", packpath);
					return;
				}

				DirectoryInfo packdir = Directory.CreateDirectory(packpath);

				try {
					StreamWriter mcmeta = new StreamWriter (packpath + "pack.mcmeta", true);
					mcmeta.Write ("{\n  \"pack\": {\n    \"pack_format\": " + packformat + ",\n    \"description\": \"" + desc + "\"\n  }\n}");
					mcmeta.Close ();
				} catch (Exception e) {
					Console.WriteLine ("Execution failed: {0}", e.ToString());
				}


			} catch(Exception e) {
				Console.WriteLine ("Execution failed: {0}", e.ToString ());
			}
		}
	}
}
