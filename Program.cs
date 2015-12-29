using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using MCRPCGUI;

namespace MCRPCGUI
{
	class Window
	{
		//Close console window
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool FreeConsole();

		[STAThread]
		public static void Main ()
		{
			FreeConsole ();
			Application.EnableVisualStyles();

			// ---- Icon ---- //
			var iconpath = System.Reflection.Assembly.GetExecutingAssembly ().GetManifestResourceStream ("icon");
			Icon icon = new Icon (iconpath);

			// ---- GUI ---- //
			Form form = new Form();
			form.Text = "MCRPC";
			form.Width = 256;
			form.Height = 350;
			form.MaximizeBox = false;
			form.MinimizeBox = false;
			form.FormBorderStyle = FormBorderStyle.FixedSingle;
			form.Icon = icon;

			TextBox namefield = new TextBox ();
			namefield.Text = "Name";
			namefield.Width = 128;
			namefield.Height = 20;
			namefield.TabIndex = 1;
			namefield.Name = "namefield";

			Button startbutton = new Button ();
			startbutton.Text = "Create Pack";
			startbutton.Width = 112;
			startbutton.Height = 20;
			startbutton.Location = new Point (128, 0);
			startbutton.TabIndex = 5;
			startbutton.Click += new EventHandler (startbutton_Click);

			GroupBox packformatgroup = new GroupBox ();

			RadioButton packformat1 = new RadioButton ();
			packformat1.Text = "pre 1.9";
			packformat1.Height = 20;
			packformat1.Width = 64;
			packformat1.Location = new Point (8, 100);
			packformat1.TabIndex = 3;
			packformat1.Name = "packformat1";
			packformat1.Checked = true;

			RadioButton packformat2 = new RadioButton ();
			packformat2.Text = "post 1.9";
			packformat2.Height = 20;
			packformat2.Width = 80;
			packformat2.Location = new Point (72, 100);
			packformat2.TabIndex = 4;
			packformat2.Name = "packformat2";

			packformatgroup.Controls.Add (packformat1);
			packformatgroup.Controls.Add (packformat2);

			TextBox descriptionfield = new TextBox ();
			descriptionfield.Text = "Description";
			descriptionfield.Multiline = true;
			descriptionfield.Height = 80;
			descriptionfield.Width = 256;
			descriptionfield.Location = new Point (0, 20);
			descriptionfield.TabIndex = 2;
			descriptionfield.Name = "descriptionfield";

			TextBox outputfield = new TextBox ();
			outputfield.Multiline = true;
			outputfield.Width = 256;
			outputfield.Height = 192;
			outputfield.Location = new Point (0, 120);
			outputfield.ReadOnly = true;
			outputfield.Name = "outputfield";

			form.Controls.Add (namefield);
			form.Controls.Add (startbutton);
			form.Controls.Add (packformat1);
			form.Controls.Add (packformat2);
			form.Controls.Add (descriptionfield);
			form.Controls.Add (outputfield);
			form.Show();

			// ---- Run ---- //
			Application.Run(form);
		}

		public static void startbutton_Click(object sender, EventArgs eventArgs)
		{
			// ---- Get all necessary GUI items ---- //
			Button button = sender as Button;
			Form form = button.FindForm () as Form;
			TextBox outputfield = form.Controls ["outputfield"] as TextBox;
			TextBox namefield = form.Controls ["namefield"] as TextBox;
			TextBox descriptionfield = form.Controls ["descriptionfield"] as TextBox;
			RadioButton packformat1 = form.Controls ["packformat1"] as RadioButton;
			RadioButton packformat2 = form.Controls ["packformat2"] as RadioButton;

			// ---- pack_format, name, description, version ---- //
			var pack_format = "1";
			var name = namefield.Text;
			var description = descriptionfield.Text;
			var version = "1.8.x or earlier";
			if (packformat2.Checked)
			{
				pack_format = "2";
				version = "1.9.x or later";
			}

			// ---- Remove invalid chars from name ---- //
			string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

			foreach (char c in invalid)
			{
				name = name.Replace(c.ToString(), "");
			}

			// ---- summary ---- //
			output ("-- Summary --" + Environment.NewLine + "Name: " + name + Environment.NewLine + "Description: " + Environment.NewLine + description + Environment.NewLine + "Version: " + version, outputfield);

			// ---- Create pack ---- //
			new MCRPC (name, description, pack_format, outputfield);
		}

		// ---- Output function, nothing more than a substitute for Control.Text += text ---- //
		public static void output(string text, TextBox output)
		{
			output.Text += text;
		}
	}
}