//--------------------------------------------------------------------------
// File:    Program.MenuHandlers.cs
// Content:	Console Menu Handlers
// Author:	Andreas Börcsök
// Website:	http://smartexpert.de
// Copyright © 2012 Andreas Börcsök
//--------------------------------------------------------------------------
#region Using directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using SmartExpert.CUI;
using SmartExpert.Linq;
using SmartExpert.Drawing;
using SmartExpert.Reflection;

#endregion

namespace SmartExpert.Examples
{
	///<summary>Console App Menu Handlers</summary>
	partial class Program
	{
		[Description("Using Color Extensions")]
		internal static void MenuItem1Handler()
		{
			
			var color1 = ColorRGB.FromColor(Colors.Green);
			ConsoleHelper.WriteLine(color1, ConsoleColor.White);
			ConsoleHelper.NewLine();
			ConsoleHelper.WriteLine(color1.ToString("X", null), ConsoleColor.Green);
			ConsoleHelper.NewLine();
			ConsoleHelper.WriteLine(color1.ToGrayColorFromLumaRec709(), ConsoleColor.Gray);
			ConsoleHelper.NewLine();

			var hslColor = ColorHSL.FromColor(Colors.Green); 
			ConsoleHelper.WriteLine(hslColor, ConsoleColor.Yellow);
			ConsoleHelper.NewLine();
			var hsvColor = ColorHSV.FromColor(Colors.Green);
			ConsoleHelper.WriteLine(hsvColor, ConsoleColor.Yellow);
			ConsoleHelper.NewLine();
			ConsoleHelper.WriteLine(new ColorCMYK(color1), ConsoleColor.Cyan);

			ConsoleHelper.HR();

			var color2 = Color.FromRgb(47, 47, 250);
			var color2Rgb = ColorRGB.FromColor(color2);
			var hsvColor2 = ColorHSV.FromColor(color2);
			
			ConsoleHelper.WriteLine(color2Rgb, ConsoleColor.White);
			ConsoleHelper.NewLine();
			ConsoleHelper.WriteLine(color2Rgb.ToString("X", null), ConsoleColor.Green);
			ConsoleHelper.NewLine();
			ConsoleHelper.WriteLine(hsvColor2, ConsoleColor.Yellow);
			ConsoleHelper.NewLine();
			ConsoleHelper.WriteLine(color2Rgb.ToLumaRec709(), ConsoleColor.Gray);
			ConsoleHelper.WriteLine(color2Rgb.ToGrayColorFromLumaRec709(), ConsoleColor.Gray);
			ConsoleHelper.NewLine();

			var color3 = Color.FromRgb(97, 3, 13);
			var color3Rgb = ColorRGB.FromColor(color3);
			
			ConsoleHelper.HR();

			var hsvColor3 = ColorHSV.FromColor(color3);

			ConsoleHelper.WriteLine(color3Rgb, ConsoleColor.White);
			ConsoleHelper.NewLine();
			ConsoleHelper.WriteLine(color3Rgb.ToString("X", null), ConsoleColor.Green);
			ConsoleHelper.NewLine();
			ConsoleHelper.WriteLine(hsvColor3, ConsoleColor.Yellow);
			ConsoleHelper.NewLine();

			ConsoleHelper.WriteLine(color3Rgb.ToLumaRec709(), ConsoleColor.Gray);
			ConsoleHelper.WriteLine(color3Rgb.ToGrayColorFromLumaRec709(), ConsoleColor.Gray);
			ConsoleHelper.NewLine();

			ConsoleHelper.WriteLine(color3Rgb.ToLabL(), ConsoleColor.Gray);
			ConsoleHelper.WriteLine(color3Rgb.ToGrayColorFromLabL(), ConsoleColor.Gray);
			ConsoleHelper.NewLine();
			
		}

		[Description("PeReader + IsManaged Eval")]
		internal static void MenuItem2Handler()
		{
			var file1 = @"C:\Users\anbo42\Documents\WindowsPowerShell\Modules\SmartExpertPack\SmartExpert.BCL.CoreMin.dll";
			var peInfo1 = new SmartExpert.Reflection.PeReader.PeFileInfo(file1);
			peInfo1.ReadPeFileHeaders();
			bool isManaged1 = AssemblyHelper.IsManaged(file1);

			var file2 = @"C:\Windows\Notepad.exe";
			var peInfo2 = new SmartExpert.Reflection.PeReader.PeFileInfo(file2);
			peInfo2.ReadPeFileHeaders();
			bool isManaged2 = AssemblyHelper.IsManaged(file2);

		}
	}
}
