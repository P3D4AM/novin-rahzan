using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Report.Controls;

internal class StiHtmlTextHelper : IStiHtmlTextHelper
{
	public static string DefaultFontName = "Arial";

	public static float DefaultFontSize = 12f;

	public string SetFont(string text, Font font, Color defaultColor)
	{
		StiRichBoxControl stiRichBoxControl = new StiRichBoxControl();
		try
		{
			SetHtmlText(text, stiRichBoxControl, defaultColor);
			((TextBoxBase)stiRichBoxControl).SelectAll();
			((RichTextBox)stiRichBoxControl).SelectionFont = font;
			return GetHtmlText(stiRichBoxControl, defaultColor);
		}
		finally
		{
			((IDisposable)stiRichBoxControl)?.Dispose();
		}
	}

	public string SetFontName(string text, string fontName, Color defaultColor)
	{
		StiRichBoxControl stiRichBoxControl = new StiRichBoxControl();
		try
		{
			SetHtmlText(text, stiRichBoxControl, defaultColor);
			((TextBoxBase)stiRichBoxControl).SelectAll();
			stiRichBoxControl.SetSelectionFont(fontName);
			return GetHtmlText(stiRichBoxControl, defaultColor);
		}
		finally
		{
			((IDisposable)stiRichBoxControl)?.Dispose();
		}
	}

	public string SetFontSize(string text, float fontSize, Color defaultColor)
	{
		StiRichBoxControl stiRichBoxControl = new StiRichBoxControl();
		try
		{
			SetHtmlText(text, stiRichBoxControl, defaultColor);
			((TextBoxBase)stiRichBoxControl).SelectAll();
			stiRichBoxControl.SetSelectionSize((int)Math.Round(fontSize));
			return GetHtmlText(stiRichBoxControl, defaultColor);
		}
		finally
		{
			((IDisposable)stiRichBoxControl)?.Dispose();
		}
	}

	public string GrowFontSize(string text, Color defaultColor)
	{
		StiRichBoxControl stiRichBoxControl = new StiRichBoxControl();
		try
		{
			SetHtmlText(text, stiRichBoxControl, defaultColor);
			((TextBoxBase)stiRichBoxControl).SelectAll();
			stiRichBoxControl.SetSelectionSize((int)Math.Round(GetSelectionOrFirstFont(stiRichBoxControl).Size + 1f));
			return GetHtmlText(stiRichBoxControl, defaultColor);
		}
		finally
		{
			((IDisposable)stiRichBoxControl)?.Dispose();
		}
	}

	public string ShrinkFontSize(string text, Color defaultColor)
	{
		StiRichBoxControl stiRichBoxControl = new StiRichBoxControl();
		try
		{
			SetHtmlText(text, stiRichBoxControl, defaultColor);
			((TextBoxBase)stiRichBoxControl).SelectAll();
			if (GetSelectionOrFirstFont(stiRichBoxControl).Size - 1f > 0f)
			{
				stiRichBoxControl.SetSelectionSize((int)Math.Round(GetSelectionOrFirstFont(stiRichBoxControl).Size - 1f));
			}
			return GetHtmlText(stiRichBoxControl, defaultColor);
		}
		finally
		{
			((IDisposable)stiRichBoxControl)?.Dispose();
		}
	}

	public string SetFontBoldStyle(string text, bool isBold, Color defaultColor)
	{
		StiRichBoxControl stiRichBoxControl = new StiRichBoxControl();
		try
		{
			SetHtmlText(text, stiRichBoxControl, defaultColor);
			((TextBoxBase)stiRichBoxControl).SelectAll();
			stiRichBoxControl.SetSelectionBold(isBold);
			return GetHtmlText(stiRichBoxControl, defaultColor);
		}
		finally
		{
			((IDisposable)stiRichBoxControl)?.Dispose();
		}
	}

	public string SetFontItalicStyle(string text, bool isItalic, Color defaultColor)
	{
		StiRichBoxControl stiRichBoxControl = new StiRichBoxControl();
		try
		{
			SetHtmlText(text, stiRichBoxControl, defaultColor);
			((TextBoxBase)stiRichBoxControl).SelectAll();
			stiRichBoxControl.SetSelectionItalic(isItalic);
			return GetHtmlText(stiRichBoxControl, defaultColor);
		}
		finally
		{
			((IDisposable)stiRichBoxControl)?.Dispose();
		}
	}

	public string SetFontUnderlineStyle(string text, bool isUnderline, Color defaultColor)
	{
		StiRichBoxControl stiRichBoxControl = new StiRichBoxControl();
		try
		{
			SetHtmlText(text, stiRichBoxControl, defaultColor);
			((TextBoxBase)stiRichBoxControl).SelectAll();
			stiRichBoxControl.SetSelectionUnderlined(isUnderline);
			return GetHtmlText(stiRichBoxControl, defaultColor);
		}
		finally
		{
			((IDisposable)stiRichBoxControl)?.Dispose();
		}
	}

	public string SetColor(string text, Color color)
	{
		StiRichBoxControl stiRichBoxControl = new StiRichBoxControl();
		try
		{
			SetHtmlText(text, stiRichBoxControl, color);
			((TextBoxBase)stiRichBoxControl).SelectAll();
			((RichTextBox)stiRichBoxControl).SelectionColor = color;
			return GetHtmlText(stiRichBoxControl, color);
		}
		finally
		{
			((IDisposable)stiRichBoxControl)?.Dispose();
		}
	}

	public string SetHorAlignment(string text, StiTextHorAlignment alignment, Color defaultColor)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return SetHorAlignment(text, alignment, defaultColor, DefaultFontName, DefaultFontSize);
	}

	public string SetHorAlignment(string text, StiTextHorAlignment alignment, Color defaultColor, string defaultFontName, float defaultFontSize)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		StiRichBoxControl stiRichBoxControl = new StiRichBoxControl();
		try
		{
			SetHtmlText(text, stiRichBoxControl, defaultColor, defaultFontName, defaultFontSize);
			((TextBoxBase)stiRichBoxControl).SelectAll();
			stiRichBoxControl.SelectionAlignment = GetAlignment(alignment);
			return GetHtmlText(stiRichBoxControl, defaultColor, defaultFontName, defaultFontSize);
		}
		finally
		{
			((IDisposable)stiRichBoxControl)?.Dispose();
		}
	}

	public Font GetFont(string text, Color defaultColor)
	{
		return GetFont(text, defaultColor, DefaultFontName, DefaultFontSize);
	}

	public Font GetFont(string text, Color defaultColor, string defaultFontName, float defaultFontSize)
	{
		StiRichBoxControl stiRichBoxControl = new StiRichBoxControl();
		try
		{
			SetHtmlText(text, stiRichBoxControl, defaultColor, defaultFontName, defaultFontSize);
			((TextBoxBase)stiRichBoxControl).SelectAll();
			return GetSelectionOrFirstFont(stiRichBoxControl);
		}
		finally
		{
			((IDisposable)stiRichBoxControl)?.Dispose();
		}
	}

	private Font GetSelectionOrFirstFont(StiRichBoxControl richTextBox)
	{
		Font selectionFont = ((RichTextBox)richTextBox).SelectionFont;
		if (selectionFont != null)
		{
			return selectionFont;
		}
		((TextBoxBase)richTextBox).Select(0, 1);
		return ((RichTextBox)richTextBox).SelectionFont;
	}

	public Color GetColor(string text, Color defaultColor)
	{
		StiRichBoxControl stiRichBoxControl = new StiRichBoxControl();
		try
		{
			SetHtmlText(text, stiRichBoxControl, defaultColor);
			((TextBoxBase)stiRichBoxControl).SelectAll();
			return ((RichTextBox)stiRichBoxControl).SelectionColor;
		}
		finally
		{
			((IDisposable)stiRichBoxControl)?.Dispose();
		}
	}

	public StiTextHorAlignment GetHorAlign(string text, Color defaultColor)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		StiRichBoxControl stiRichBoxControl = new StiRichBoxControl();
		try
		{
			SetHtmlText(text, stiRichBoxControl, defaultColor);
			((TextBoxBase)stiRichBoxControl).SelectAll();
			return GetAlignment(stiRichBoxControl.SelectionAlignment);
		}
		finally
		{
			((IDisposable)stiRichBoxControl)?.Dispose();
		}
	}

	public void SetHtmlText(string htmlText, StiRichBoxControl richTextBox, Color defaultColor)
	{
		SetHtmlText(htmlText, richTextBox, defaultColor, DefaultFontName, DefaultFontSize);
	}

	public void SetHtmlText(string htmlText, StiRichBoxControl richTextBox, Color defaultColor, string defaultFontName, float defaultFontSize)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00cc: Expected O, but got Unknown
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Expected O, but got Unknown
		//IL_023d: Expected O, but got Unknown
		((TextBoxBase)richTextBox).SelectAll();
		richTextBox.SetSelectionFont(defaultFontName);
		richTextBox.SetSelectionSize((int)defaultFontSize);
		StiHtmlTagsState val = default(StiHtmlTagsState);
		((StiHtmlTagsState)(ref val))._002Ector(false, false, false, false, defaultFontSize, defaultFontName, defaultColor, Color.White, false, false, 0.0, 0.0, 0.0, (StiTextHorAlignment)1);
		Font val2 = null;
		StiHtmlState val3 = default(StiHtmlState);
		((StiHtmlState)(ref val3))._002Ector(val, 0);
		List<StiHtmlState> list = StiTextRenderer.ParseHtmlToStates(htmlText, val3, false);
		float num = defaultFontSize;
		if (list.Count > 1)
		{
			num = list[0].TS.FontSize;
			foreach (StiHtmlState item in list)
			{
				if (item.TS.FontSize != num)
				{
					Font val4 = new Font(item.TS.FontName, item.TS.FontSize);
					Font val5 = val4;
					((Control)richTextBox).Font = val4;
					val2 = val5;
					num = item.TS.FontSize;
					break;
				}
			}
		}
		if (list.Count > 2)
		{
			StiHtmlState value = list[list.Count - 1];
			value.TS.TextAlign = list[list.Count - 2].TS.TextAlign;
			list[list.Count - 1] = value;
		}
		foreach (StiHtmlState item2 in list)
		{
			int length = ((Control)richTextBox).Text.Length;
			((TextBoxBase)richTextBox).Select(length, 0);
			string text = null;
			text = (((TextBoxBase)richTextBox).SelectedText = (string.IsNullOrEmpty(item2.TS.Href) ? item2.Text.ToString() : $"<a href=\"{item2.TS.Href}\">{item2.Text}</a>"));
			((TextBoxBase)richTextBox).Select(length, text.Length);
			FontStyle val6 = (FontStyle)0;
			if (item2.TS.Bold)
			{
				val6 = (FontStyle)(val6 | 1);
			}
			if (item2.TS.Italic)
			{
				val6 = (FontStyle)(val6 | 2);
			}
			if (item2.TS.Underline)
			{
				val6 = (FontStyle)(val6 | 4);
			}
			string fontName = item2.TS.FontName;
			float fontSize = item2.TS.FontSize;
			if (val2 == null)
			{
				Font val7 = new Font(fontName, fontSize);
				Font val5 = val7;
				((Control)richTextBox).Font = val7;
				val2 = val5;
			}
			((RichTextBox)richTextBox).SelectionFont = StiFontUtils.ChangeFontStyle(fontName, num, val6);
			((RichTextBox)richTextBox).SelectionColor = item2.TS.FontColor;
			richTextBox.SelectionAlignment = GetAlignment(item2.TS.TextAlign);
		}
		((TextBoxBase)richTextBox).Select(0, 0);
	}

	public string GetHtmlText(StiRichBoxControl richTextBox, Color defaultColor)
	{
		return GetHtmlText(richTextBox, defaultColor, DefaultFontName, DefaultFontSize);
	}

	public string GetHtmlText(StiRichBoxControl richTextBox, Color defaultColor, string defaultFontName, float defaultFontSize)
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		Color color = defaultColor;
		StringBuilder stringBuilder = new StringBuilder();
		((TextBoxBase)richTextBox).SelectAll();
		Font selectionOrFirstFont = GetSelectionOrFirstFont(richTextBox);
		string arg = ((selectionOrFirstFont != null) ? selectionOrFirstFont.Name : defaultFontName);
		float num = ((selectionOrFirstFont != null) ? selectionOrFirstFont.SizeInPoints : defaultFontSize);
		StiRichTextAlignment selectionAlignment = richTextBox.SelectionAlignment;
		stringBuilder = stringBuilder.Append($"<font face=\"{arg}\" size=\"{num}\">");
		stringBuilder = stringBuilder.Append($"<text-align=\"{selectionAlignment}\">");
		bool flag4 = false;
		for (int i = 0; i < ((Control)richTextBox).Text.Length; i++)
		{
			((TextBoxBase)richTextBox).Select(i, 1);
			if (!flag4)
			{
				Font selectionFont = ((RichTextBox)richTextBox).SelectionFont;
				bool flag5 = selectionFont != null && ((Enum)selectionFont.Style).HasFlag((Enum)(object)(FontStyle)1);
				bool flag6 = selectionFont != null && ((Enum)selectionFont.Style).HasFlag((Enum)(object)(FontStyle)2);
				bool flag7 = selectionFont != null && ((Enum)selectionFont.Style).HasFlag((Enum)(object)(FontStyle)4);
				Color selectionColor = ((RichTextBox)richTextBox).SelectionColor;
				if (((Control)richTextBox).Text[i] == '\n')
				{
					stringBuilder = stringBuilder.Append("<br />");
				}
				if (flag5 && !flag)
				{
					flag = true;
					stringBuilder = stringBuilder.Append("<b>");
				}
				if (!flag5 && flag)
				{
					flag = false;
					stringBuilder = stringBuilder.Append("</b>");
				}
				if (flag6 && !flag2)
				{
					flag2 = true;
					stringBuilder = stringBuilder.Append("<i>");
				}
				if (!flag6 && flag2)
				{
					flag2 = false;
					stringBuilder = stringBuilder.Append("</i>");
				}
				if (flag7 && !flag3)
				{
					flag3 = true;
					stringBuilder = stringBuilder.Append("<u>");
				}
				if (!flag7 && flag3)
				{
					flag3 = false;
					stringBuilder = stringBuilder.Append("</u>");
				}
				if (color != selectionColor)
				{
					if (color == defaultColor)
					{
						stringBuilder = stringBuilder.Append("<font-color=\"" + ColorTranslator.ToHtml(selectionColor) + "\">");
						color = selectionColor;
					}
					else if (selectionColor == defaultColor)
					{
						stringBuilder = stringBuilder.Append("</font-color>");
						color = selectionColor;
					}
					else
					{
						stringBuilder = stringBuilder.Append("</font-color>");
						stringBuilder = stringBuilder.Append("<font-color=\"" + ColorTranslator.ToHtml(selectionColor) + "\">");
						color = selectionColor;
					}
				}
			}
			if (((Control)richTextBox).Text[i] == '{')
			{
				flag4 = true;
			}
			if (((Control)richTextBox).Text[i] == '}')
			{
				flag4 = false;
			}
			stringBuilder = stringBuilder.Append(((Control)richTextBox).Text[i]);
		}
		if (flag4)
		{
			stringBuilder = stringBuilder.Append("}");
		}
		if (color != defaultColor)
		{
			stringBuilder = stringBuilder.Append("</font-color>");
		}
		if (flag3)
		{
			stringBuilder = stringBuilder.Append("</u>");
		}
		if (flag2)
		{
			stringBuilder = stringBuilder.Append("</i>");
		}
		if (flag)
		{
			stringBuilder = stringBuilder.Append("</b>");
		}
		stringBuilder = stringBuilder.Append("</text-align>");
		stringBuilder = stringBuilder.Append("</font>");
		return stringBuilder.ToString();
	}

	private static StiRichTextAlignment GetAlignment(StiTextHorAlignment align)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected I4, but got Unknown
		return (int)align switch
		{
			0 => StiRichTextAlignment.Left, 
			1 => StiRichTextAlignment.Center, 
			2 => StiRichTextAlignment.Right, 
			3 => StiRichTextAlignment.Justify, 
			_ => throw new NotSupportedException(), 
		};
	}

	private static StiTextHorAlignment GetAlignment(StiRichTextAlignment align)
	{
		return (StiTextHorAlignment)(align switch
		{
			StiRichTextAlignment.Left => 0, 
			StiRichTextAlignment.Center => 1, 
			StiRichTextAlignment.Right => 2, 
			StiRichTextAlignment.Justify => 3, 
			_ => throw new NotSupportedException(), 
		});
	}

	public string GetSimpleText(string htmlText, Color defaultColor)
	{
		StiRichBoxControl stiRichBoxControl = new StiRichBoxControl();
		try
		{
			SetHtmlText(htmlText, stiRichBoxControl, defaultColor);
			return ((Control)stiRichBoxControl).Text;
		}
		finally
		{
			((IDisposable)stiRichBoxControl)?.Dispose();
		}
	}
}
