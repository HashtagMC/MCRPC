using System;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace MCRPC
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// ---- Init ---- //
			Console.Title = "MCRPC";
			Console.WriteLine ("Welcome to the Minecraft Resource Pack Creator 'MCRPC'");
			Console.WriteLine ("");

			// ---- Get the necessary data ---- //

			// ---- Name ---- //
			Console.Write ("Please enter the name of your resource pack: ");
			var name = Console.ReadLine ();
			Console.WriteLine (""); //Newline

			// ---- Remove invalid chars from foldername ---- //
			string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

			foreach (char c in invalid)
			{
				name = name.Replace(c.ToString(), "");
			}

			// ---- Description ---- //
			Console.Write (@"Please enter the description (use \n as newline) of your resource pack: ");
			var desc = Console.ReadLine ();
			Console.WriteLine ("");

			// ---- Pack format ---- //
			Console.Write("Please enter the pack format (1 for 1.8 or earlier, 2 for 1.9 or later). Leave blank to use the default value: ");
			String packformat = Console.ReadLine ();
			Console.WriteLine ("");

			// ---- Version ---- //
			var version = "";

			switch (packformat) {
				case "1":
					version = "1.8.x or earlier";
					break;
			case "2":
				version = "1.9.x or later";
				break;
			}

			// ---- Summary ---- //
			Console.WriteLine ("-- Summary --\nName: {0}\nDescription: {1}\nVersion: {2}", name, desc, version);
			Console.WriteLine ("");

			// ---- Verify / sanitize pack format ---- //
			if (packformat.Equals ("1") || packformat.Equals ("2")) {

			} else {
				packformat = "1";
			}

			// ---- Path & Files ---- //
			var appdata = System.Environment.GetEnvironmentVariable ("AppData"); //get appdata path
			var rppath = appdata + @"\.minecraft\resourcepacks\"; //path to resourcepack folder
			var packpath = rppath + name + @"\"; //path to resource pack

			// ---- Don't overwrite anything ---- //
			try {
				if (Directory.Exists(packpath)){
					Console.WriteLine ("Directory {0} already exists!", packpath);
					return;
				}

				//Everything's ok? Create folder + assets folder
				DirectoryInfo packdir = Directory.CreateDirectory(packpath);
				DirectoryInfo assetsdir = Directory.CreateDirectory(packpath + @"\assets");

				//Write pack.mcmeta
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

			// ---- Extract minecraft jar ---- //
			var mcjar = appdata + @"\.minecraft\versions\1.8\1.8.jar";

			Console.WriteLine ("Extracting 1.8.jar");
			ZipFile.ExtractToDirectory (mcjar, packpath);

			// ---- Delete unnessecary files, such as .class or META-INF ---- //
			Console.WriteLine("Deleting unnessecary files");
			var directoryPath = new DirectoryInfo (packpath);

			foreach (var file in directoryPath.EnumerateFiles("*.class")) {
				file.Delete();
			}

			foreach (var file in directoryPath.EnumerateFiles("log*.xml")) {
				file.Delete();
			}

			Directory.Delete (packpath + @"\META-INF", true);

			Directory.Delete (packpath + @"\net", true);

			// ---- Done! ---- //
			Console.WriteLine("Done!");
		}
	}
}
