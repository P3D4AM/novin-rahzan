using System.Drawing;
using System.IO;
using System.Text;

namespace Stimulsoft.Editor;

public class HtmlExporter : FmtExporter
{
	private const string htmlB = "b";

	private const string htmlI = "i";

	private const string htmlU = "u";

	private const string htmlStrike = "strike";

	private const string htmlHtml = "html";

	private const string htmlBody = "body";

	private const string htmlHead = "head";

	private const string htmlFont = "font";

	private const string htmlName = "name";

	private const string htmlColor = "color";

	private const string htmlBr = "br";

	private const string htmlLt = "&lt;";

	private const string htmlQt = "&gt;";

	private const string htmlAmp = "&amp;";

	private const string htmlQuot = "&quot;";

	private const string htmlNbsp = "&nbsp;";

	private bool colorChanged;

	private StringBuilder rBuilder;

	public HtmlExporter()
	{
		rBuilder = new StringBuilder();
	}

	private void WriteTag(string tag)
	{
		writer.Write("<" + tag + ">");
	}

	private void WriteTag(string tag, string name, string value)
	{
		writer.Write("<" + tag + " " + name + "=" + value + ">");
	}

	private void WriteEndTag(string tag)
	{
		writer.Write("</" + tag + ">");
	}

	private string ColorToHtml(Color color)
	{
		return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
	}

	private string StringToHtml(int pos, string str)
	{
		if (str == string.Empty || str == null)
		{
			return str;
		}
		rBuilder.Length = 0;
		bool flag = true;
		foreach (char c in str)
		{
			switch (c)
			{
			case ' ':
				if (flag)
				{
					rBuilder.Append("&nbsp;");
				}
				else
				{
					rBuilder.Append(c);
				}
				break;
			case '\t':
			{
				int tabStop = owner.Lines.GetTabStop(pos);
				if (flag)
				{
					for (int j = 0; j < tabStop; j++)
					{
						rBuilder.Append("&nbsp;");
					}
				}
				else
				{
					rBuilder.Append(new string(' ', tabStop));
				}
				pos += tabStop;
				break;
			}
			case '<':
				rBuilder.Append("&lt;");
				break;
			case '>':
				rBuilder.Append("&gt;");
				break;
			case '&':
				rBuilder.Append("&amp;");
				break;
			case '"':
				rBuilder.Append("&quot;");
				break;
			default:
				rBuilder.Append(c);
				break;
			}
			if (c != ' ' && c != '\t')
			{
				flag = false;
			}
			if (c != '\t')
			{
				pos++;
			}
		}
		return rBuilder.ToString();
	}

	protected override void StartDocument(TextWriter writer)
	{
		base.StartDocument(writer);
		WriteTag("html");
		base.writer.WriteLine();
		WriteTag("head");
		base.writer.Write(owner.Source.FileName);
		WriteEndTag("head");
		base.writer.WriteLine();
		WriteTag("body");
		base.writer.WriteLine();
		WriteTag("font", "name", owner.Font.Name);
		base.writer.WriteLine();
	}

	protected override void EndDocument()
	{
		if (colorChanged)
		{
			WriteEndTag("font");
		}
		WriteFontStyle((FontStyle)0);
		WriteEndTag("font");
		writer.WriteLine();
		WriteEndTag("body");
		writer.WriteLine();
		WriteEndTag("html");
	}

	protected override void WriteForeColor(Color foreColor)
	{
		if (colorChanged)
		{
			WriteEndTag("font");
		}
		WriteTag("font", "color", ColorToHtml(foreColor));
		colorChanged = true;
	}

	protected override void WriteBackColor(Color backColor)
	{
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
			WriteEndTag("b");
			return;
		case 1:
			WriteEndTag("i");
			return;
		case 3:
			WriteEndTag("u");
			return;
		case 2:
			return;
		}
		if ((int)style == 8)
		{
			WriteEndTag("strike");
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
			WriteTag("b");
			return;
		case 1:
			WriteTag("i");
			return;
		case 3:
			WriteTag("u");
			return;
		case 2:
			return;
		}
		if ((int)style == 8)
		{
			WriteTag("strike");
		}
	}

	protected override void StartLine()
	{
		if (!firstLine)
		{
			writer.WriteLine();
			WriteTag("br");
		}
	}

	protected override void WriteText(int pos, string text)
	{
		writer.Write(StringToHtml(pos, text));
	}
}
