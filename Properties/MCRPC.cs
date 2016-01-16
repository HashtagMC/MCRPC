﻿using System;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

using MCRPCGUI;

namespace MCRPCGUI
{
	public class MCRPC
	{
		public MCRPC (String name, String desc, String pack_format, TextBox outputfield, String version)
		{
			// ---- Path & Files ---- //
			var appdata = System.Environment.GetEnvironmentVariable ("AppData"); //get appdata path
			var rppath = appdata + @"\.minecraft\resourcepacks\"; //path to resourcepack folder
			var packpath = rppath + name + @"\"; //path to resource pack

			// ---- Don't overwrite anything ---- //
			try {
				if (Directory.Exists(packpath)){
					MessageBox.Show ("Directory \"" + packpath + "\" already exists!");
					return;
				}

				//Everything's ok? Create folder + assets folder
				DirectoryInfo packdir = Directory.CreateDirectory(packpath);
				DirectoryInfo assetsdir = Directory.CreateDirectory(packpath + @"\assets");

				//Write pack.mcmeta
				try {
					StreamWriter mcmeta = new StreamWriter (packpath + "pack.mcmeta", true);
					mcmeta.Write ("{\n  \"pack\": {\n    \"pack_format\": " + pack_format + ",\n    \"description\": \"" + desc + "\"\n  }\n}");
					mcmeta.Close ();
				} catch (Exception e) {
					MessageBox.Show ("Execution failed: {0}", e.ToString());
				}

			} catch(Exception e) {
				MessageBox.Show ("Execution failed: {0}", e.ToString ());
			}

			// ---- Extract minecraft jar ---- //
			var mcjar = appdata + @"\.minecraft\versions\" + version + ".jar";

			output (Environment.NewLine + "Extracting " + version + ".jar", outputfield);
			try {
				ZipFile.ExtractToDirectory (mcjar, packpath);
				// ---- Delete unnessecary files, such as .class or META-INF ---- //
				output(Environment.NewLine + "Deleting unnessecary files", outputfield);
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
				output(Environment.NewLine + "Done!", outputfield);
			} catch (Exception e) {
				output(Environment.NewLine + "Execution failed: Make sure you have Minecraft 1.8 installed" + Environment.NewLine + Environment.NewLine + e.ToString(), outputfield);
				MessageBox.Show ("Execution failed: Make sure you have Minecraft 1.8 installed!");
				Directory.Delete (packpath, true);
			}

		}

		// ---- Output function, nothing more than a substitute for Control.Text += text ---- //
		public static void output(string text, TextBox output)
		{
			output.Text += text;
		}
	}
}

