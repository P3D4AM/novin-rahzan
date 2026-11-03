using System.Collections;
using System.Drawing;
using System.Globalization;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report;

public static class StiViewerFitTextHelper
{
	public class StiFitTextInfo
	{
		public Hashtable HashText = new Hashtable();

		public Hashtable HashComponent = new Hashtable();

		private Hashtable HashFontString = new Hashtable();

		public string GetFontString(Font font)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			object obj = HashFontString[font];
			if (obj != null)
			{
				return (string)obj;
			}
			string text = $"{font.Name}*{font.Style}*";
			HashFontString[font] = text;
			return text;
		}

		public object GetFontSizeObject(StiText textBox, RectangleD rect, string text, ref float fontSize, ref string hashSt)
		{
			object obj = HashComponent[textBox];
			if (obj == null)
			{
				hashSt = text + "*" + ((RectangleD)(ref rect)).Width.ToString(CultureInfo.InvariantCulture) + "*" + ((RectangleD)(ref rect)).Height.ToString(CultureInfo.InvariantCulture) + "*" + GetFontString(textBox.Font) + "*" + fontSize.ToString(CultureInfo.InvariantCulture);
				obj = HashText[hashSt];
				if (obj != null)
				{
					HashComponent[textBox] = obj;
				}
			}
			if (obj is float)
			{
				fontSize = (float)obj;
			}
			return obj;
		}

		public void Clear()
		{
			HashText.Clear();
			HashComponent.Clear();
			HashFontString.Clear();
		}
	}

	private static Hashtable hashes = new Hashtable();

	public static bool Enabled { get; set; }

	public static void AddReport(StiReport report)
	{
		if (Enabled && report != null)
		{
			hashes[report.CompiledReport ?? report] = new StiFitTextInfo();
		}
	}

	public static void RemoveReport(StiReport report)
	{
		if (report != null)
		{
			((StiFitTextInfo)hashes[report])?.Clear();
			hashes.Remove(report);
		}
	}

	public static void ClearReportInfo(StiReport report)
	{
		if (report != null)
		{
			((StiFitTextInfo)hashes[report])?.Clear();
		}
	}

	public static StiFitTextInfo GetReportInfo(StiReport report)
	{
		if (Enabled && report != null)
		{
			object obj = hashes[report];
			if (obj != null)
			{
				return (StiFitTextInfo)obj;
			}
		}
		return null;
	}

	public static void Clear()
	{
		hashes.Clear();
	}
}
