using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;

namespace DevComponents.DotNetBar.Metro.ColorTables;

[TypeConverter(typeof(MetroColorGeneratorParametersConverter))]
public struct MetroColorGeneratorParameters
{
	private static readonly Color _1393 = Color.White;

	private Color __1394;

	private static readonly Color _1395 = ColorScheme.GetColor("FFA31A");

	private Color __1396;

	private string __1397;

	[Description("Indicates color of the metro Canvas.")]
	[Category("Columns")]
	public Color CanvasColor
	{
		get
		{
			return __1394;
		}
		set
		{
			__1394 = value;
		}
	}

	[Description("Indicates base color of the Metro style.")]
	[Category("Columns")]
	public Color BaseColor
	{
		get
		{
			return __1396;
		}
		set
		{
			__1396 = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string ThemeName
	{
		get
		{
			return __1397;
		}
		set
		{
			__1397 = value;
		}
	}

	public static MetroColorGeneratorParameters Default => new MetroColorGeneratorParameters(Color.White, ColorScheme.GetColor("2B579A"), "Default");

	public static MetroColorGeneratorParameters Purple => new MetroColorGeneratorParameters(ColorScheme.GetColor("FFFFFF"), ColorScheme.GetColor("6A3889"), "Purple");

	public static MetroColorGeneratorParameters Red => new MetroColorGeneratorParameters(ColorScheme.GetColor("FFFFFF"), ColorScheme.GetColor("CF4926"), "Red");

	public static MetroColorGeneratorParameters Green => new MetroColorGeneratorParameters(ColorScheme.GetColor("FFFFFF"), ColorScheme.GetColor("217346"), "Green");

	public static MetroColorGeneratorParameters Blue => new MetroColorGeneratorParameters(ColorScheme.GetColor("FFFFFF"), ColorScheme.GetColor("0072C6"), "Blue");

	public static MetroColorGeneratorParameters DarkPurple => new MetroColorGeneratorParameters(ColorScheme.GetColor("FFFFFF"), ColorScheme.GetColor("80397B"), "DarkPurple");

	public static MetroColorGeneratorParameters RedAmplified => new MetroColorGeneratorParameters(ColorScheme.GetColor("FFFFFF"), ColorScheme.GetColor("D24726"), "RedAmplified");

	public static MetroColorGeneratorParameters ForestGreen => new MetroColorGeneratorParameters(ColorScheme.GetColor("FFFFFF"), ColorScheme.GetColor("077568"), "ForestGreen");

	public static MetroColorGeneratorParameters Brown => new MetroColorGeneratorParameters(ColorScheme.GetColor(15195840), ColorScheme.GetColor("6D4320"), "Brown");

	public static MetroColorGeneratorParameters DarkRed => new MetroColorGeneratorParameters(ColorScheme.GetColor("F2F2F2"), ColorScheme.GetColor("A4373A"), "Dark Red");

	public static MetroColorGeneratorParameters Orange => new MetroColorGeneratorParameters(ColorScheme.GetColor("FFFFFF"), ColorScheme.GetColor("ED8E00"), "Orange");

	public static MetroColorGeneratorParameters BlackMaroon => new MetroColorGeneratorParameters(ColorScheme.GetColor("1C190F"), ColorScheme.GetColor("8A2F25"), "Black Maroon");

	public static MetroColorGeneratorParameters BlackSky => new MetroColorGeneratorParameters(Color.Black, ColorScheme.GetColor("00A7FF"), "Black Sky");

	public static MetroColorGeneratorParameters BlackClouds => new MetroColorGeneratorParameters(ColorScheme.GetColor("000000"), ColorScheme.GetColor("9B96A3"), "Black Clouds");

	public static MetroColorGeneratorParameters BlackLilac => new MetroColorGeneratorParameters(ColorScheme.GetColor("191919"), ColorScheme.GetColor("D8CAFF"), "Black Lilac");

	public static MetroColorGeneratorParameters Bordeaux => new MetroColorGeneratorParameters(ColorScheme.GetColor("F2EBDC"), ColorScheme.GetColor("960E00"), "Bordeaux");

	public static MetroColorGeneratorParameters Rust => new MetroColorGeneratorParameters(ColorScheme.GetColor("B4B48D"), ColorScheme.GetColor("A94D2D"), "Rust");

	public static MetroColorGeneratorParameters Espresso => new MetroColorGeneratorParameters(ColorScheme.GetColor("AB9667"), ColorScheme.GetColor("383025"), "Espresso");

	public static MetroColorGeneratorParameters PowderRed => new MetroColorGeneratorParameters(ColorScheme.GetColor("D0D9D6"), ColorScheme.GetColor("BF0404"), "Powder Red");

	public static MetroColorGeneratorParameters BlueishBrown => new MetroColorGeneratorParameters(ColorScheme.GetColor("D5D5E0"), ColorScheme.GetColor("3D1429"), "Blueish Brown");

	public static MetroColorGeneratorParameters SilverGreen => new MetroColorGeneratorParameters(ColorScheme.GetColor("F2F2F2"), ColorScheme.GetColor("588C3F"), "Silver Green");

	public static MetroColorGeneratorParameters MaroonSilver => new MetroColorGeneratorParameters(ColorScheme.GetColor("260B01"), ColorScheme.GetColor("F2F2F2"), "Maroon Silver");

	public static MetroColorGeneratorParameters SkyGreen => new MetroColorGeneratorParameters(ColorScheme.GetColor("BDD5D0"), ColorScheme.GetColor("04ADBF"), "Sky Green");

	public static MetroColorGeneratorParameters DarkBrown => new MetroColorGeneratorParameters(ColorScheme.GetColor("D9B573"), ColorScheme.GetColor("261105"), "Dark Brown");

	public static MetroColorGeneratorParameters Latte => new MetroColorGeneratorParameters(ColorScheme.GetColor("B18D5A"), ColorScheme.GetColor("851A1D"), "Latte");

	public static MetroColorGeneratorParameters Cherry => new MetroColorGeneratorParameters(ColorScheme.GetColor("DECDAE"), ColorScheme.GetColor("5A0D12"), "Cherry");

	public static MetroColorGeneratorParameters GrayOrange => new MetroColorGeneratorParameters(ColorScheme.GetColor("E8E8DC"), ColorScheme.GetColor("FF8600"), "Gray Orange");

	public static MetroColorGeneratorParameters EarlyRed => new MetroColorGeneratorParameters(ColorScheme.GetColor("FCFCF0"), ColorScheme.GetColor("C93C00"), "Early Red");

	public static MetroColorGeneratorParameters EarlyMaroon => new MetroColorGeneratorParameters(ColorScheme.GetColor("FCFCF0"), ColorScheme.GetColor("730046"), "Early Maroon");

	public static MetroColorGeneratorParameters EarlyOrange => new MetroColorGeneratorParameters(ColorScheme.GetColor("FCFCF0"), ColorScheme.GetColor("E88801"), "Early Orange");

	public static MetroColorGeneratorParameters SilverBlues => new MetroColorGeneratorParameters(ColorScheme.GetColor("dcdcdc"), ColorScheme.GetColor("002395"), "Silver Blues");

	public static MetroColorGeneratorParameters RetroBlue => new MetroColorGeneratorParameters(ColorScheme.GetColor("FEFEFE"), ColorScheme.GetColor("3E788F"), "Retro Blue");

	public static MetroColorGeneratorParameters LatteRed => new MetroColorGeneratorParameters(ColorScheme.GetColor("A77A51"), ColorScheme.GetColor("8C170D"), "Latte Red");

	public static MetroColorGeneratorParameters LatteDarkSteel => new MetroColorGeneratorParameters(ColorScheme.GetColor("A77A51"), ColorScheme.GetColor("2C2D40"), "Latte Dark Steel");

	public static MetroColorGeneratorParameters SimplyBlue => new MetroColorGeneratorParameters(ColorScheme.GetColor("E2E2E2"), ColorScheme.GetColor("215BA6"), "Simply Blue");

	public static MetroColorGeneratorParameters WashedBlue => new MetroColorGeneratorParameters(ColorScheme.GetColor("B8CDD1"), ColorScheme.GetColor("B4090A"), "Washed Blue");

	public static MetroColorGeneratorParameters WashedWhite => new MetroColorGeneratorParameters(ColorScheme.GetColor("FEFFF8"), ColorScheme.GetColor("92AAB1"), "Washed White");

	public static MetroColorGeneratorParameters NapaRed => new MetroColorGeneratorParameters(ColorScheme.GetColor("F2F2F2"), ColorScheme.GetColor("A64B29"), "Napa Red");

	public static MetroColorGeneratorParameters Magenta => new MetroColorGeneratorParameters(ColorScheme.GetColor("FFFFFF"), ColorScheme.GetColor("7F3B78"), "Magenta");

	public static MetroColorGeneratorParameters DarkBlue => new MetroColorGeneratorParameters(ColorScheme.GetColor("FFFFFF"), ColorScheme.GetColor("2B569A"), "Dark Blue");

	public static MetroColorGeneratorParameters VisualStudio2012Light => new MetroColorGeneratorParameters(ColorScheme.GetColor("EFEFF2"), ColorScheme.GetColor("007ACC"), "Visual Studio 2012 Light");

	public static MetroColorGeneratorParameters VisualStudio2012Dark => new MetroColorGeneratorParameters(ColorScheme.GetColor("2D2D30"), ColorScheme.GetColor("007ACC"), "Visual Studio 2012 Dark");

	public static MetroColorGeneratorParameters Office2016Blue => new MetroColorGeneratorParameters(ColorScheme.GetColor("FFFFFF"), ColorScheme.GetColor("0173C7"), "Office 2016 Blue");

	public static MetroColorGeneratorParameters Office2016DarkBlue => new MetroColorGeneratorParameters(ColorScheme.GetColor("FFFFFF"), ColorScheme.GetColor("2A579A"), "Office 2016 Dark Blue");

	public static MetroColorGeneratorParameters Office2016Green => new MetroColorGeneratorParameters(ColorScheme.GetColor("FFFFFF"), ColorScheme.GetColor("227447"), "Office 2016 Green");

	public static MetroColorGeneratorParameters Office2016Orange => new MetroColorGeneratorParameters(ColorScheme.GetColor("FFFFFF"), ColorScheme.GetColor("B7472A"), "Office 2016 Orange");

	public static MetroColorGeneratorParameters Office2016Red => new MetroColorGeneratorParameters(ColorScheme.GetColor("FFFFFF"), ColorScheme.GetColor("A4373A"), "Office 2016 Red");

	public static MetroColorGeneratorParameters Office2016Purple => new MetroColorGeneratorParameters(ColorScheme.GetColor("FFFFFF"), ColorScheme.GetColor("80397B"), "Office 2016 Purple");

	public MetroColorGeneratorParameters(Color canvasColor, Color baseColor)
	{
		if (canvasColor.IsEmpty)
		{
			canvasColor = _1393;
		}
		if (baseColor.IsEmpty)
		{
			baseColor = _1395;
		}
		__1394 = canvasColor;
		__1396 = baseColor;
		__1397 = "";
	}

	public MetroColorGeneratorParameters(Color canvasColor, Color baseColor, string themeName)
	{
		__1394 = canvasColor;
		__1396 = baseColor;
		__1397 = themeName;
	}

	public static MetroColorGeneratorParameters[] GetAllPredefinedThemes()
	{
		Type typeFromHandle = typeof(MetroColorGeneratorParameters);
		PropertyInfo[] properties = typeFromHandle.GetProperties(BindingFlags.Static | BindingFlags.Public);
		List<MetroColorGeneratorParameters> list = new List<MetroColorGeneratorParameters>();
		PropertyInfo[] array = properties;
		foreach (PropertyInfo propertyInfo in array)
		{
			if ((object)propertyInfo.PropertyType == typeFromHandle)
			{
				list.Add((MetroColorGeneratorParameters)propertyInfo.GetValue(null, null));
			}
		}
		return list.ToArray();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeCanvasColor()
	{
		return __1394 != _1393;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetCanvasColor()
	{
		CanvasColor = _1393;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBaseColor()
	{
		return __1396 != _1395;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBaseColor()
	{
		BaseColor = _1395;
	}
}
