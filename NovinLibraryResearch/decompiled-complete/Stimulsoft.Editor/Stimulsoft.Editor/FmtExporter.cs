using System.Drawing;
using System.IO;

namespace Stimulsoft.Editor;

public class FmtExporter
{
	protected ISyntaxEdit owner;

	protected ISyntaxStrings lines;

	protected TextWriter writer;

	protected Color foreColor = Consts.DefaultControlForeColor;

	protected Color backColor = Consts.DefaultControlBackColor;

	protected FontStyle fontStyle;

	protected bool firstLine = true;

	protected virtual void StartDocument(TextWriter writer)
	{
		this.writer = writer;
	}

	protected virtual void EndDocument()
	{
	}

	protected virtual void WriteForeColor(Color foreColor)
	{
	}

	protected virtual void WriteBackColor(Color backColor)
	{
	}

	protected virtual void RemoveFontStyle(FontStyle style)
	{
	}

	protected virtual void AddFontStyle(FontStyle style)
	{
	}

	protected virtual void StartLine()
	{
	}

	protected virtual void EndLine()
	{
	}

	protected virtual void WriteText(int pos, string text)
	{
	}

	protected virtual void WriteFontStyle(FontStyle style)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		FontStyle val = (FontStyle)(fontStyle & ~style);
		FontStyle val2 = (FontStyle)(style & ~fontStyle);
		if ((val & 8) != 0)
		{
			RemoveFontStyle((FontStyle)8);
		}
		if ((val & 4) != 0)
		{
			RemoveFontStyle((FontStyle)4);
		}
		if ((val & 2) != 0)
		{
			RemoveFontStyle((FontStyle)2);
		}
		if ((val & 1) != 0)
		{
			RemoveFontStyle((FontStyle)1);
		}
		if ((val2 & 1) != 0)
		{
			AddFontStyle((FontStyle)1);
		}
		if ((val2 & 2) != 0)
		{
			AddFontStyle((FontStyle)2);
		}
		if ((val2 & 4) != 0)
		{
			AddFontStyle((FontStyle)4);
		}
		if ((val2 & 8) != 0)
		{
			AddFontStyle((FontStyle)8);
		}
	}

	protected virtual void ApplyStyle(FontStyle style, Color foreColor, Color backColor)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		if (foreColor != this.foreColor && foreColor != Color.Empty)
		{
			WriteForeColor(foreColor);
			this.foreColor = foreColor;
		}
		if (backColor != this.backColor && backColor != Color.Empty)
		{
			WriteBackColor(backColor);
			this.backColor = backColor;
		}
		if (style != fontStyle)
		{
			WriteFontStyle(style);
			fontStyle = style;
		}
	}

	protected virtual void WriteLine(string line, int style, int start, int end)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		ColorFlags state = ColorFlags.None;
		SyntaxEdit syntaxEdit = owner as SyntaxEdit;
		SyntaxPaint syntaxPaint = syntaxEdit.SyntaxPaint as SyntaxPaint;
		ILexStyle lexStyle = syntaxPaint.GetLexStyle(style, ref state);
		FontStyle style2;
		Color fontColor;
		Color color;
		if (lexStyle != null)
		{
			style2 = syntaxPaint.GetFontStyle(lexStyle.FontStyle, state);
			fontColor = syntaxPaint.GetFontColor(lexStyle.ForeColor, state);
			color = ((!(lexStyle.BackColor != Color.Empty)) ? owner.BackColor : lexStyle.BackColor);
		}
		else
		{
			style2 = syntaxPaint.GetFontStyle(owner.Font.Style, state);
			fontColor = syntaxPaint.GetFontColor(owner.ForeColor, state);
			color = owner.BackColor;
		}
		ApplyStyle(style2, fontColor, color);
		WriteText(start, line.Substring(start, end - start + 1));
	}

	protected virtual void WriteContent()
	{
		bool flag = owner.Source.Lexer != null;
		if (flag && lines.Source != null)
		{
			lines.Source.ParseToString(lines.Count - 1);
		}
		SyntaxPaint syntaxPaint = ((SyntaxEdit)owner).SyntaxPaint as SyntaxPaint;
		firstLine = true;
		for (int i = 0; i < lines.Count; i++)
		{
			IStrItem item = lines.GetItem(i);
			int length = item.String.Length;
			StartLine();
			firstLine = false;
			if (flag && item.String != string.Empty)
			{
				int num = item.ColorData[0];
				int num2 = num;
				int num3 = 0;
				for (int j = 1; j < length; j++)
				{
					num = item.ColorData[j];
					if (num != num2 && !syntaxPaint.EqualStyles(num2, num, useColors: true))
					{
						WriteLine(item.String, num2, num3, j - 1);
						num2 = num;
						num3 = j;
					}
				}
				if (num3 < length)
				{
					WriteLine(item.String, num2, num3, length - 1);
				}
			}
			else
			{
				WriteLine(item.String, -1, 0, length - 1);
			}
			EndLine();
		}
	}

	public void Write(ISyntaxEdit owner, ISyntaxStrings lines, TextWriter writer)
	{
		this.owner = owner;
		this.lines = lines;
		StartDocument(writer);
		WriteContent();
		EndDocument();
	}
}
