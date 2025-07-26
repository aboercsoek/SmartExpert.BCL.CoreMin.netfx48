using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SmartExpert.Drawing
{
	/// <summary>
	/// A helper class containing named colors.
	/// </summary>
	[Serializable]
	public class KnownColors : Dictionary<string, string>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="KnownColors"/> class.
		/// </summary>
		/// <param name="info">The info.</param>
		/// <param name="context">The context.</param>
		protected KnownColors(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="KnownColors" /> class.
		/// </summary>
		public KnownColors()
			: base(150, StringComparer.OrdinalIgnoreCase)
		{
			Add("AliceBlue", "#FFF0F8FF");
			Add("AntiqueWhite", "#FFFAEBD7");
			Add("Aqua", "#FF00FFFF");
			Add("Aquamarine", "#FF7FFFD4");
			Add("Azure", "#FFF0FFFF");
			Add("Beige", "#FFF5F5DC");
			Add("Bisque", "#FFFFE4C4");
			Add("Black", "#FF000000");
			Add("BlanchedAlmond", "#FFFFEBCD");
			Add("Blue", "#FF0000FF");
			Add("BlueViolet", "#FF8A2BE2");
			Add("Brown", "#FFA52A2A");
			Add("BurlyWood", "#FFDEB887");
			Add("CadetBlue", "#FF5F9EA0");
			Add("Chartreuse", "#FF7FFF00");
			Add("Chocolate", "#FFD2691E");
			Add("Coral", "#FFFF7F50");
			Add("CornflowerBlue", "#FF6495ED");
			Add("Cornsilk", "#FFFFF8DC");
			Add("Crimson", "#FFDC143C");
			Add("Cyan", "#FF00FFFF");
			Add("DarkBlue", "#FF00008B");
			Add("DarkCyan", "#FF008B8B");
			Add("DarkGoldenrod", "#FFB8860B");
			Add("DarkGray", "#FFA9A9A9");
			Add("DarkGreen", "#FF006400");
			Add("DarkKhaki", "#FFBDB76B");
			Add("DarkMagenta", "#FF8B008B");
			Add("DarkOliveGreen", "#FF556B2F");
			Add("DarkOrange", "#FFFF8C00");
			Add("DarkOrchid", "#FF9932CC");
			Add("DarkRed", "#FF8B0000");
			Add("DarkSalmon", "#FFE9967A");
			Add("DarkSeaGreen", "#FF8FBC8F");
			Add("DarkSlateBlue", "#FF483D8B");
			Add("DarkSlateGray", "#FF2F4F4F");
			Add("DarkTurquoise", "#FF00CED1");
			Add("DarkViolet", "#FF9400D3");
			Add("DeepPink", "#FFFF1493");
			Add("DeepSkyBlue", "#FF00BFFF");
			Add("DimGray", "#FF696969");
			Add("DodgerBlue", "#FF1E90FF");
			Add("Firebrick", "#FFB22222");
			Add("FloralWhite", "#FFFFFAF0");
			Add("ForestGreen", "#FF228B22");
			Add("Fuchsia", "#FFFF00FF");
			Add("Gainsboro", "#FFDCDCDC");
			Add("GhostWhite", "#FFF8F8FF");
			Add("Gold", "#FFFFD700");
			Add("Goldenrod", "#FFDAA520");
			Add("Gray", "#FF808080");
			Add("Green", "#FF008000");
			Add("GreenYellow", "#FFADFF2F");
			Add("Honeydew", "#FFF0FFF0");
			Add("HotPink", "#FFFF69B4");
			Add("IndianRed", "#FFCD5C5C");
			Add("Indigo", "#FF4B0082");
			Add("Ivory", "#FFFFFFF0");
			Add("Khaki", "#FFF0E68C");
			Add("Lavender", "#FFE6E6FA");
			Add("LavenderBlush", "#FFFFF0F5");
			Add("LawnGreen", "#FF7CFC00");
			Add("LemonChiffon", "#FFFFFACD");
			Add("LightBlue", "#FFADD8E6");
			Add("LightCoral", "#FFF08080");
			Add("LightCyan", "#FFE0FFFF");
			Add("LightGoldenrodYellow", "#FFFAFAD2");
			Add("LightGray", "#FFD3D3D3");
			Add("LightGreen", "#FF90EE90");
			Add("LightPink", "#FFFFB6C1");
			Add("LightSalmon", "#FFFFA07A");
			Add("LightSeaGreen", "#FF20B2AA");
			Add("LightSkyBlue", "#FF87CEFA");
			Add("LightSlateGray", "#FF778899");
			Add("LightSteelBlue", "#FFB0C4DE");
			Add("LightYellow", "#FFFFFFE0");
			Add("Lime", "#FF00FF00");
			Add("LimeGreen", "#FF32CD32");
			Add("Linen", "#FFFAF0E6");
			Add("Magenta", "#FFFF00FF");
			Add("Maroon", "#FF800000");
			Add("MediumAquamarine", "#FF66CDAA");
			Add("MediumBlue", "#FF0000CD");
			Add("MediumOrchid", "#FFBA55D3");
			Add("MediumPurple", "#FF9370DB");
			Add("MediumSeaGreen", "#FF3CB371");
			Add("MediumSlateBlue", "#FF7B68EE");
			Add("MediumSpringGreen", "#FF00FA9A");
			Add("MediumTurquoise", "#FF48D1CC");
			Add("MediumVioletRed", "#FFC71585");
			Add("MidnightBlue", "#FF191970");
			Add("MintCream", "#FFF5FFFA");
			Add("MistyRose", "#FFFFE4E1");
			Add("Moccasin", "#FFFFE4B5");
			Add("NavajoWhite", "#FFFFDEAD");
			Add("Navy", "#FF000080");
			Add("OldLace", "#FFFDF5E6");
			Add("Olive", "#FF808000");
			Add("OliveDrab", "#FF6B8E23");
			Add("Orange", "#FFFFA500");
			Add("OrangeRed", "#FFFF4500");
			Add("Orchid", "#FFDA70D6");
			Add("PaleGoldenrod", "#FFEEE8AA");
			Add("PaleGreen", "#FF98FB98");
			Add("PaleTurquoise", "#FFAFEEEE");
			Add("PaleVioletRed", "#FFDB7093");
			Add("PapayaWhip", "#FFFFEFD5");
			Add("PeachPuff", "#FFFFDAB9");
			Add("Peru", "#FFCD853F");
			Add("Pink", "#FFFFC0CB");
			Add("Plum", "#FFDDA0DD");
			Add("PowderBlue", "#FFB0E0E6");
			Add("Purple", "#FF800080");
			Add("Red", "#FFFF0000");
			Add("RosyBrown", "#FFBC8F8F");
			Add("RoyalBlue", "#FF4169E1");
			Add("SaddleBrown", "#FF8B4513");
			Add("Salmon", "#FFFA8072");
			Add("SandyBrown", "#FFF4A460");
			Add("SeaGreen", "#FF2E8B57");
			Add("SeaShell", "#FFFFF5EE");
			Add("Sienna", "#FFA0522D");
			Add("Silver", "#FFC0C0C0");
			Add("SkyBlue", "#FF87CEEB");
			Add("SlateBlue", "#FF6A5ACD");
			Add("SlateGray", "#FF708090");
			Add("Snow", "#FFFFFAFA");
			Add("SpringGreen", "#FF00FF7F");
			Add("SteelBlue", "#FF4682B4");
			Add("Tan", "#FFD2B48C");
			Add("Teal", "#FF008080");
			Add("Thistle", "#FFD8BFD8");
			Add("Tomato", "#FFFF6347");
			Add("Transparent", "#00FFFFFF");
			Add("Turquoise", "#FF40E0D0");
			Add("Violet", "#FFEE82EE");
			Add("Wheat", "#FFF5DEB3");
			Add("White", "#FFFFFFFF");
			Add("WhiteSmoke", "#FFF5F5F5");
			Add("Yellow", "#FFFFFF00");
			Add("YellowGreen", "#FF9ACD32");
		}

		/// <summary>
		/// 
		/// </summary>
		public string AliceBlue
		{
			get
			{
				const string key = "AliceBlue";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string AntiqueWhite
		{
			get
			{
				const string key = "AntiqueWhite";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Aqua
		{
			get
			{
				const string key = "Aqua";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Aquamarine
		{
			get
			{
				const string key = "Aquamarine";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Azure
		{
			get
			{
				const string key = "Azure";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Beige
		{
			get
			{
				const string key = "Beige";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Bisque
		{
			get
			{
				const string key = "Bisque";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Black
		{
			get
			{
				const string key = "Black";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string BlanchedAlmond
		{
			get
			{
				const string key = "BlanchedAlmond";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Blue
		{
			get
			{
				const string key = "Blue";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string BlueViolet
		{
			get
			{
				const string key = "BlueViolet";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Brown
		{
			get
			{
				const string key = "Brown";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string BurlyWood
		{
			get
			{
				const string key = "BurlyWood";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string CadetBlue
		{
			get
			{
				const string key = "CadetBlue";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Chartreuse
		{
			get
			{
				const string key = "Chartreuse";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Chocolate
		{
			get
			{
				const string key = "Chocolate";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Coral
		{
			get
			{
				const string key = "Coral";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string CornflowerBlue
		{
			get
			{
				const string key = "CornflowerBlue";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Cornsilk
		{
			get
			{
				const string key = "Cornsilk";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Crimson
		{
			get
			{
				const string key = "Crimson";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Cyan
		{
			get
			{
				const string key = "Cyan";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DarkBlue
		{
			get
			{
				const string key = "DarkBlue";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DarkCyan
		{
			get
			{
				const string key = "DarkCyan";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DarkGoldenrod
		{
			get
			{
				const string key = "DarkGoldenrod";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DarkGray
		{
			get
			{
				const string key = "DarkGray";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DarkGreen
		{
			get
			{
				const string key = "DarkGreen";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DarkKhaki
		{
			get
			{
				const string key = "DarkKhaki";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DarkMagenta
		{
			get
			{
				const string key = "DarkMagenta";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DarkOliveGreen
		{
			get
			{
				const string key = "DarkOliveGreen";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DarkOrange
		{
			get
			{
				const string key = "DarkOrange";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DarkOrchid
		{
			get
			{
				const string key = "DarkOrchid";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DarkRed
		{
			get
			{
				const string key = "DarkRed";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DarkSalmon
		{
			get
			{
				const string key = "DarkSalmon";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DarkSeaGreen
		{
			get
			{
				const string key = "DarkSeaGreen";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DarkSlateBlue
		{
			get
			{
				const string key = "DarkSlateBlue";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DarkSlateGray
		{
			get
			{
				const string key = "DarkSlateGray";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DarkTurquoise
		{
			get
			{
				const string key = "DarkTurquoise";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DarkViolet
		{
			get
			{
				const string key = "DarkViolet";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DeepPink
		{
			get
			{
				const string key = "DeepPink";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DeepSkyBlue
		{
			get
			{
				const string key = "DeepSkyBlue";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DimGray
		{
			get
			{
				const string key = "DimGray";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string DodgerBlue
		{
			get
			{
				const string key = "DodgerBlue";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Firebrick
		{
			get
			{
				const string key = "Firebrick";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string FloralWhite
		{
			get
			{
				const string key = "FloralWhite";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string ForestGreen
		{
			get
			{
				const string key = "ForestGreen";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Fuchsia
		{
			get
			{
				const string key = "Fuchsia";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Gainsboro
		{
			get
			{
				const string key = "Gainsboro";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string GhostWhite
		{
			get
			{
				const string key = "GhostWhite";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Gold
		{
			get
			{
				const string key = "Gold";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Goldenrod
		{
			get
			{
				const string key = "Goldenrod";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Gray
		{
			get
			{
				const string key = "Gray";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Green
		{
			get
			{
				const string key = "Green";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string GreenYellow
		{
			get
			{
				const string key = "GreenYellow";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Honeydew
		{
			get
			{
				const string key = "Honeydew";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string HotPink
		{
			get
			{
				const string key = "HotPink";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string IndianRed
		{
			get
			{
				const string key = "IndianRed";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Indigo
		{
			get
			{
				const string key = "Indigo";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Ivory
		{
			get
			{
				const string key = "Ivory";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Khaki
		{
			get
			{
				const string key = "Khaki";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Lavender
		{
			get
			{
				const string key = "Lavender";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string LavenderBlush
		{
			get
			{
				const string key = "LavenderBlush";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string LawnGreen
		{
			get
			{
				const string key = "LawnGreen";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string LemonChiffon
		{
			get
			{
				const string key = "LemonChiffon";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string LightBlue
		{
			get
			{
				const string key = "LightBlue";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string LightCoral
		{
			get
			{
				const string key = "LightCoral";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string LightCyan
		{
			get
			{
				const string key = "LightCyan";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string LightGoldenrodYellow
		{
			get
			{
				const string key = "LightGoldenrodYellow";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string LightGray
		{
			get
			{
				const string key = "LightGray";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string LightGreen
		{
			get
			{
				const string key = "LightGreen";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string LightPink
		{
			get
			{
				const string key = "LightPink";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string LightSalmon
		{
			get
			{
				const string key = "LightSalmon";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string LightSeaGreen
		{
			get
			{
				const string key = "LightSeaGreen";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string LightSkyBlue
		{
			get
			{
				const string key = "LightSkyBlue";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string LightSlateGray
		{
			get
			{
				const string key = "LightSlateGray";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string LightSteelBlue
		{
			get
			{
				const string key = "LightSteelBlue";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string LightYellow
		{
			get
			{
				const string key = "LightYellow";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Lime
		{
			get
			{
				const string key = "Lime";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string LimeGreen
		{
			get
			{
				const string key = "LimeGreen";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Linen
		{
			get
			{
				const string key = "Linen";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Magenta
		{
			get
			{
				const string key = "Magenta";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Maroon
		{
			get
			{
				const string key = "Maroon";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string MediumAquamarine
		{
			get
			{
				const string key = "MediumAquamarine";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string MediumBlue
		{
			get
			{
				const string key = "MediumBlue";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string MediumOrchid
		{
			get
			{
				const string key = "MediumOrchid";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string MediumPurple
		{
			get
			{
				const string key = "MediumPurple";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string MediumSeaGreen
		{
			get
			{
				const string key = "MediumSeaGreen";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string MediumSlateBlue
		{
			get
			{
				const string key = "MediumSlateBlue";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string MediumSpringGreen
		{
			get
			{
				const string key = "MediumSpringGreen";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string MediumTurquoise
		{
			get
			{
				const string key = "MediumTurquoise";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string MediumVioletRed
		{
			get
			{
				const string key = "MediumVioletRed";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string MidnightBlue
		{
			get
			{
				const string key = "MidnightBlue";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string MintCream
		{
			get
			{
				const string key = "MintCream";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string MistyRose
		{
			get
			{
				const string key = "MistyRose";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Moccasin
		{
			get
			{
				const string key = "Moccasin";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string NavajoWhite
		{
			get
			{
				const string key = "NavajoWhite";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Navy
		{
			get
			{
				const string key = "Navy";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string OldLace
		{
			get
			{
				const string key = "OldLace";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Olive
		{
			get
			{
				const string key = "Olive";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string OliveDrab
		{
			get
			{
				const string key = "OliveDrab";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Orange
		{
			get
			{
				const string key = "Orange";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string OrangeRed
		{
			get
			{
				const string key = "OrangeRed";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Orchid
		{
			get
			{
				const string key = "Orchid";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string PaleGoldenrod
		{
			get
			{
				const string key = "PaleGoldenrod";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string PaleGreen
		{
			get
			{
				const string key = "PaleGreen";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string PaleTurquoise
		{
			get
			{
				const string key = "PaleTurquoise";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string PaleVioletRed
		{
			get
			{
				const string key = "PaleVioletRed";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string PapayaWhip
		{
			get
			{
				const string key = "PapayaWhip";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string PeachPuff
		{
			get
			{
				const string key = "PeachPuff";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Peru
		{
			get
			{
				const string key = "Peru";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Pink
		{
			get
			{
				const string key = "Pink";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Plum
		{
			get
			{
				const string key = "Plum";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string PowderBlue
		{
			get
			{
				const string key = "PowderBlue";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Purple
		{
			get
			{
				const string key = "Purple";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Red
		{
			get
			{
				const string key = "Red";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string RosyBrown
		{
			get
			{
				const string key = "RosyBrown";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string RoyalBlue
		{
			get
			{
				const string key = "RoyalBlue";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string SaddleBrown
		{
			get
			{
				const string key = "SaddleBrown";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Salmon
		{
			get
			{
				const string key = "Salmon";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string SandyBrown
		{
			get
			{
				const string key = "SandyBrown";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string SeaGreen
		{
			get
			{
				const string key = "SeaGreen";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string SeaShell
		{
			get
			{
				const string key = "SeaShell";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Sienna
		{
			get
			{
				const string key = "Sienna";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Silver
		{
			get
			{
				const string key = "Silver";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string SkyBlue
		{
			get
			{
				const string key = "SkyBlue";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string SlateBlue
		{
			get
			{
				const string key = "SlateBlue";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string SlateGray
		{
			get
			{
				const string key = "SlateGray";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Snow
		{
			get
			{
				const string key = "Snow";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string SpringGreen
		{
			get
			{
				const string key = "SpringGreen";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string SteelBlue
		{
			get
			{
				const string key = "SteelBlue";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Tan
		{
			get
			{
				const string key = "Tan";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Teal
		{
			get
			{
				const string key = "Teal";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Thistle
		{
			get
			{
				const string key = "Thistle";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Tomato
		{
			get
			{
				const string key = "Tomato";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Transparent
		{
			get
			{
				const string key = "Transparent";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Turquoise
		{
			get
			{
				const string key = "Turquoise";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Violet
		{
			get
			{
				const string key = "Violet";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Wheat
		{
			get
			{
				const string key = "Wheat";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string White
		{
			get
			{
				const string key = "White";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string WhiteSmoke
		{
			get
			{
				const string key = "WhiteSmoke";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Yellow
		{
			get
			{
				const string key = "Yellow";
				return GetKey(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string YellowGreen
		{
			get
			{
				const string key = "YellowGreen";
				return GetKey(key);
			}
		}

		private string GetKey(string key)
		{
			if (ContainsKey(key))
			{
				return this[key];
			}

			throw new KeyNotFoundException(string.Format("{0} key not found!", key));
		}
	}
}