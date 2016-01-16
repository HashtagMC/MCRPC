using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MCRPCGUI
{
	public class VersionList
	{
		public String[] versions { get; set; }

		public VersionList ()
		{
			// ---- Get path to game folder ---- //
			var gamepath = System.Environment.GetEnvironmentVariable ("AppData").ToString () + @"\.minecraft\";
			try {
				// ---- Get all versions by enumerating all 1.*.json files ---- //
				var versions1 = from file in Directory.EnumerateFiles(gamepath + @"versions\", "1.*.json", SearchOption.AllDirectories)
								from line in File.ReadLines(file)
								where line.Contains("{")
								select new
								{
								File = file,
								Line = line
								};

				int number = 0;

				foreach(var f in versions1) {
					number += 1;
				}

				List<String> versions2 = new List<String>();

				foreach(var f in versions1) {
					versions2.Add(f.File.ToString());
				}

				for (int i = 0; i < versions2.Count; i++) {
					versions2[i] = versions2[i].Replace(gamepath + @"versions\", "");
					versions2[i] = versions2[i].Replace(".json", "");

					Regex regex = new Regex(@".*\\", RegexOptions.IgnoreCase);
					versions2[i] = regex.Replace(versions2[i], "");
				}

				this.versions = versions2.Distinct().ToArray();

				//versions2 = versions2.ToArray();

				Random rnd = new Random();
				int r = rnd.Next(versions.Length);
				//MessageBox.Show(string.Join(",", versions2));

			} catch (UnauthorizedAccessException UAEx) {
				MessageBox.Show (UAEx.Message);
			} catch (PathTooLongException PathEx) {
				MessageBox.Show (PathEx.Message);
			} catch (Exception e) {
				MessageBox.Show (e.Message);
			}
		}
	}
}

