using System.Collections;
using System.Drawing;
using System.IO;
using System.Text;

namespace Stimulsoft.Editor;

public class RtfExporter : FmtExporter
{
	private class ColorComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			return (int)((DictionaryEntry)x).Value - (int)((DictionaryEntry)y).Value;
		}
	}

	private const string rtfHeader = "{\\rtf1\\ansi ";

	private const string rtfGroupBegin = "{";

	private const string rtfGroupEnd = "}";

	private const string rtfFontTbl = "\\fonttbl ";

	private const string rtfColorTbl = "\\colortbl ";

	private const string rtfF1 = "\\f1 ";

	private const string rtfSize = "\\fs";

	private const string rtfLine = "\\line";

	private const string rtfCf = "\\cf";

	private const string rtfHighlight = "\\highlight";

	private const string rtfB = "\\b";

	private const string rtfI = "\\i";

	private const string rtfUl = "\\ul";

	private const string rtfStrike = "\\strike";

	private const string rtfB0 = "\\b0";

	private const string rtfI0 = "\\i0";

	private const string rtfUlnone = "\\ulnone";

	private const string rtfStrike0 = "\\strike0";

	private const string rtfTab = "\\tab";

	private const string rtfRed = "\\red";

	private const string rtfGreen = "\\green";

	private const string rtfBlue = "\\blue";

	private StringBuilder sBuilder;

	private StringBuilder rBuilder;

	private Hashtable colorTable;

	private bool needSpace;

	public RtfExporter()
	{
		colorTable = new Hashtable();
		sBuilder = new StringBuilder();
		rBuilder = new StringBuilder();
	}

	private void WriteString(string str)
	{
		if (str != string.Empty)
		{
			if (needSpace)
			{
				sBuilder.Append(' ');
			}
			needSpace = false;
			sBuilder.Append(str);
		}
	}

	private void WriteString(string str, bool newLine)
	{
		WriteString(str);
		if (newLine)
		{
			sBuilder.Append("\r\n");
			needSpace = false;
		}
	}

	private void WriteTag(string tag)
	{
		sBuilder.Append(tag);
		needSpace = true;
	}

	private int AddColor(Color color)
	{
		object obj = colorTable[color];
		if (obj != null)
		{
			return (int)obj;
		}
		int count = colorTable.Count;
		colorTable.Add(color, count);
		return count;
	}

	private void WriteHeader()
	{
		writer.WriteLine("{\\rtf1\\ansi ");
	}

	private void WriteFontTable()
	{
		writer.Write("{\\fonttbl ");
		writer.Write("{\\f1 " + owner.Font.Name + ";}");
		writer.WriteLine("}");
	}

	private void WriteColorTable()
	{
		if (colorTable.Count <= 0)
		{
			return;
		}
		ArrayList arrayList = new ArrayList();
		writer.Write("{\\colortbl ");
		IDictionaryEnumerator enumerator = colorTable.GetEnumerator();
		enumerator.Reset();
		while (enumerator.MoveNext())
		{
			arrayList.Add(enumerator.Entry);
		}
		arrayList.Sort(new ColorComparer());
		foreach (DictionaryEntry item in arrayList)
		{
			writer.Write(ColorToRtf((Color)item.Key) + ";");
		}
		writer.WriteLine("}");
	}

	private string StringToRtf(string str, ref bool firstTab, ref bool lastTab)
	{
		if (str == string.Empty || str == null)
		{
			return str;
		}
		rBuilder.Length = 0;
		firstTab = str[0] == '\t';
		lastTab = false;
		for (int i = 0; i < str.Length; i++)
		{
			char c = str[i];
			switch (c)
			{
			case '\\':
			case '{':
			case '}':
				lastTab = false;
				rBuilder.Append("\\" + c);
				continue;
			case '\t':
				lastTab = true;
				rBuilder.Append("\\tab");
				continue;
			}
			if (lastTab)
			{
				rBuilder.Append(' ');
			}
			lastTab = false;
			rBuilder.Append(c);
		}
		return rBuilder.ToString();
	}

	protected string ColorToRtf(Color color)
	{
		return "\\red" + color.R + "\\green" + color.G + "\\blue" + color.B;
	}

	protected override void StartDocument(TextWriter writer)
	{
		base.StartDocument(writer);
		AddColor(foreColor);
	}

	protected override void EndDocument()
	{
		WriteHeader();
		WriteFontTable();
		WriteColorTable();
		writer.Write("\\f1 ");
		writer.Write("\\fs" + (int)owner.Font.Size * 2, ' ');
		writer.Write(sBuilder.ToString());
		writer.Write("}");
	}

	protected override void WriteForeColor(Color foreColor)
	{
		WriteTag("\\cf" + AddColor(foreColor));
	}

	protected override void WriteBackColor(Color backColor)
	{
		WriteTag("\\highlight" + AddColor(backColor));
	}

	protected override void RemoveFontStyle(FontStyle style)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected I4, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		switch (style - 1)
		{
		case 0:
			WriteTag("\\b0");
			return;
		case 1:
			WriteTag("\\i0");
			return;
		case 3:
			WriteTag("\\ulnone");
			return;
		case 2:
			return;
		}
		if ((int)style == 8)
		{
			WriteTag("\\strike0");
		}
	}

	protected override void AddFontStyle(FontStyle style)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected I4, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		switch (style - 1)
		{
		case 0:
			WriteTag("\\b");
			return;
		case 1:
			WriteTag("\\i");
			return;
		case 3:
			WriteTag("\\ul");
			return;
		case 2:
			return;
		}
		if ((int)style == 8)
		{
			WriteTag("\\strike");
		}
	}

	protected override void StartLine()
	{
		if (!firstLine)
		{
			WriteString(string.Empty, newLine: true);
			WriteTag("\\line");
		}
	}

	protected override void WriteText(int pos, string text)
	{
		bool firstTab = false;
		bool lastTab = false;
		string str = StringToRtf(text, ref firstTab, ref lastTab);
		if (firstTab)
		{
			needSpace = false;
		}
		WriteString(str);
		if (lastTab)
		{
			needSpace = true;
		}
	}
}
