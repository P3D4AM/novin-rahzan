using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Base.Drawing;

public class StiTextOptionsHelper : TypeConverter
{
	public static string ConvertTextOptionsToString(StiTextOptions op, char separator)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		string format = ((op.FirstTabOffset == 40f && op.DistanceBetweenTabs == 20f) ? "{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}" : "{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}{0}{8}{0}");
		object[] obj = new object[9] { separator, null, null, null, null, null, null, null, null };
		string text;
		if ((int)op.HotkeyPrefix == 0)
		{
			text = string.Empty;
		}
		else
		{
			HotkeyPrefix hotkeyPrefix = op.HotkeyPrefix;
			text = "HotkeyPrefix=" + ((object)(HotkeyPrefix)(ref hotkeyPrefix)).ToString();
		}
		obj[1] = text;
		obj[2] = (op.LineLimit ? "LineLimit=True" : string.Empty);
		obj[3] = (op.RightToLeft ? "RightToLeft=True" : string.Empty);
		string text2;
		if ((int)op.Trimming == 0)
		{
			text2 = string.Empty;
		}
		else
		{
			StringTrimming trimming = op.Trimming;
			text2 = "Trimming=" + ((object)(StringTrimming)(ref trimming)).ToString();
		}
		obj[4] = text2;
		obj[5] = (op.WordWrap ? "WordWrap=True" : string.Empty);
		obj[6] = ((op.Angle == 0f) ? "A=0" : ("Angle=" + op.Angle));
		obj[7] = ((op.FirstTabOffset == 40f) ? "O=40" : ("FirstTabOffset=" + op.FirstTabOffset));
		obj[8] = ((op.DistanceBetweenTabs == 20f) ? "D=20" : ("DistanceBetweenTabs=" + op.DistanceBetweenTabs));
		return string.Format(format, obj);
	}

	public static string ConvertTextOptionsToLocalizedString(StiTextOptions op, bool wordWrap)
	{
		char separator = CultureInfo.CurrentCulture.TextInfo.ListSeparator[0];
		return ConvertTextOptionsToLocalizedString(op, separator, wordWrap);
	}

	public static string ConvertTextOptionsToLocalizedString(StiTextOptions op, char separator, bool wordWrap)
	{
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		string text = Loc.Get("PropertyEnum", "boolTrue");
		string text2 = Loc.Get("PropertyEnum", "boolFalse");
		HotkeyPrefix hotkeyPrefix;
		StringTrimming trimming;
		if (wordWrap)
		{
			object[] obj = new object[16]
			{
				Loc.Get("PropertyMain", "HotkeyPrefix"),
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null
			};
			hotkeyPrefix = op.HotkeyPrefix;
			obj[1] = Loc.Get("PropertyEnum", "HotkeyPrefix" + ((object)(HotkeyPrefix)(ref hotkeyPrefix)).ToString());
			obj[2] = Loc.Get("PropertyMain", "LineLimit");
			obj[3] = (op.LineLimit ? text : text2);
			obj[4] = Loc.Get("PropertyMain", "RightToLeft");
			obj[5] = (op.RightToLeft ? text : text2);
			obj[6] = Loc.Get("PropertyMain", "Trimming");
			trimming = op.Trimming;
			obj[7] = Loc.Get("PropertyEnum", "StringTrimming" + ((object)(StringTrimming)(ref trimming)).ToString());
			obj[8] = Loc.Get("PropertyMain", "WordWrap");
			obj[9] = (op.WordWrap ? text : text2);
			obj[10] = Loc.Get("PropertyMain", "Angle");
			obj[11] = op.Angle;
			obj[12] = Loc.Get("PropertyMain", "FirstTabOffset");
			obj[13] = op.FirstTabOffset;
			obj[14] = Loc.Get("PropertyMain", "DistanceBetweenTabs");
			obj[15] = op.DistanceBetweenTabs;
			return string.Format("{0}={1}\n{2}={3}\n{4}={5}\n{6}={7}\n{8}={9}\n{10}={11}\n{12}={13}\n{14}={15}", obj);
		}
		object[] array = new object[23];
		array[0] = Loc.Get("PropertyMain", "HotkeyPrefix");
		hotkeyPrefix = op.HotkeyPrefix;
		array[1] = Loc.Get("PropertyEnum", "HotkeyPrefix" + ((object)(HotkeyPrefix)(ref hotkeyPrefix)).ToString());
		array[2] = separator;
		array[3] = Loc.Get("PropertyMain", "LineLimit");
		array[4] = (op.LineLimit ? text : text2);
		array[5] = separator;
		array[6] = Loc.Get("PropertyMain", "RightToLeft");
		array[7] = (op.RightToLeft ? text : text2);
		array[8] = separator;
		array[9] = Loc.Get("PropertyMain", "Trimming");
		trimming = op.Trimming;
		array[10] = Loc.Get("PropertyEnum", "StringTrimming" + ((object)(StringTrimming)(ref trimming)).ToString());
		array[11] = separator;
		array[12] = Loc.Get("PropertyMain", "WordWrap");
		array[13] = (op.WordWrap ? text : text2);
		array[14] = separator;
		array[15] = Loc.Get("PropertyMain", "Angle");
		array[16] = op.Angle;
		array[17] = separator;
		array[18] = Loc.Get("PropertyMain", "FirstTabOffset");
		array[19] = op.FirstTabOffset;
		array[20] = separator;
		array[21] = Loc.Get("PropertyMain", "DistanceBetweenTabs");
		array[22] = op.DistanceBetweenTabs;
		return string.Format("{0}={1}{2} {3}={4}{5} {6}={7}{8} {9}={10}{11} {12}={13}{14} {15}={16}{17} {18}={19}{20} {21}={22}", array);
	}

	public static StiTextOptions ConvertStringToTextOptions(string str, char separator)
	{
		string[] array = str.Split(new char[1] { separator });
		StiTextOptions stiTextOptions = new StiTextOptions();
		if (array[0].IndexOf("Hide", StringComparison.InvariantCulture) >= 0)
		{
			stiTextOptions.HotkeyPrefix = (HotkeyPrefix)2;
		}
		else if (array[0].IndexOf("Show", StringComparison.InvariantCulture) >= 0)
		{
			stiTextOptions.HotkeyPrefix = (HotkeyPrefix)1;
		}
		else
		{
			stiTextOptions.HotkeyPrefix = (HotkeyPrefix)0;
		}
		stiTextOptions.LineLimit = array[1].IndexOf("True", StringComparison.InvariantCulture) >= 0;
		stiTextOptions.RightToLeft = array[2].IndexOf("True", StringComparison.InvariantCulture) >= 0;
		if (array[3].IndexOf("EllipsisCharacter", StringComparison.InvariantCulture) >= 0)
		{
			stiTextOptions.Trimming = (StringTrimming)3;
		}
		else if (array[3].IndexOf("Character", StringComparison.InvariantCulture) >= 0)
		{
			stiTextOptions.Trimming = (StringTrimming)1;
		}
		else if (array[3].IndexOf("EllipsisPath", StringComparison.InvariantCulture) >= 0)
		{
			stiTextOptions.Trimming = (StringTrimming)5;
		}
		else if (array[3].IndexOf("EllipsisWord", StringComparison.InvariantCulture) >= 0)
		{
			stiTextOptions.Trimming = (StringTrimming)4;
		}
		else if (array[3].IndexOf("None", StringComparison.InvariantCulture) >= 0)
		{
			stiTextOptions.Trimming = (StringTrimming)0;
		}
		else if (array[3].IndexOf("Word", StringComparison.InvariantCulture) >= 0)
		{
			stiTextOptions.Trimming = (StringTrimming)2;
		}
		stiTextOptions.WordWrap = array[4].IndexOf("True", StringComparison.InvariantCulture) >= 0;
		string[] array2 = array[5].Split(new char[1] { '=' });
		stiTextOptions.Angle = float.Parse(array2[1].Trim());
		if (array.Length > 6)
		{
			string[] array3 = array[6].Split(new char[1] { '=' });
			stiTextOptions.FirstTabOffset = float.Parse(array3[1].Trim());
			string[] array4 = array[7].Split(new char[1] { '=' });
			stiTextOptions.DistanceBetweenTabs = float.Parse(array4[1].Trim());
		}
		return stiTextOptions;
	}
}
