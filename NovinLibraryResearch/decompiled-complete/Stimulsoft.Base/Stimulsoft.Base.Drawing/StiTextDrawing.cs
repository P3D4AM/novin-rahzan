using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace Stimulsoft.Base.Drawing;

public class StiTextDrawing
{
	private struct Range
	{
		public string Text;

		public PointD Pos;

		public SizeD Size;

		public bool IsStart;

		public bool IsEnd;

		public bool NewLineForm;

		public Range(string text, SizeD size, bool newLineForm)
		{
			Pos = new PointD(0.0, 0.0);
			Text = text;
			Size = size;
			IsStart = false;
			IsEnd = false;
			NewLineForm = newLineForm;
		}
	}

	private struct Word
	{
		public string Text;

		public bool IsEnter;

		public static Word[] GetTexts(string str)
		{
			List<Word> list = new List<Word>();
			int i = 0;
			bool flag = true;
			string text = "";
			for (; i < str.Length; i++)
			{
				if (str[i] == '\n')
				{
					list.Add(new Word(text, isEnter: true));
					text = "";
					flag = true;
				}
				else if (!flag && str[i] == ' ')
				{
					list.Add(new Word(text, isEnter: false));
					text = "";
					flag = true;
				}
				else
				{
					text += str[i];
					flag = false;
				}
			}
			if (i == str.Length)
			{
				list.Add(new Word(text, isEnter: false));
			}
			return list.ToArray();
		}

		private Word(string text, bool isEnter)
		{
			Text = text;
			IsEnter = isEnter;
		}
	}

	private struct Line
	{
		public int Start;

		public int Count;

		public bool IsEnter;

		public Line(int start, int count, bool isEnter)
		{
			Start = start;
			Count = count;
			IsEnter = isEnter;
		}
	}

	private class MeasurableInfo
	{
		private string text;

		private Graphics g;

		private Font font;

		private StringFormat sf;

		private RectangleF rect;

		private RectangleF?[] rects;

		private double defaultLineHeight;

		public double LineHeight
		{
			get
			{
				if (defaultLineHeight == 0.0)
				{
					CharacterRange[] array = (CharacterRange[])(object)new CharacterRange[1];
					((CharacterRange)(ref array[0])).First = 0;
					((CharacterRange)(ref array[0])).Length = 1;
					sf.SetMeasurableCharacterRanges(array);
					defaultLineHeight = g.MeasureCharacterRanges("\u00a0", font, rect, sf)[0].GetBounds(g).Height;
				}
				return defaultLineHeight;
			}
		}

		public void SetText(string st)
		{
			text = st;
			rects = new RectangleF?[st.Length];
		}

		public RectangleF GetRect(int index)
		{
			if (!rects[index].HasValue)
			{
				int num = index & 0xFFFFE0;
				int num2 = text.Length - num;
				if (num2 > 32)
				{
					num2 = 32;
				}
				CharacterRange[] array = (CharacterRange[])(object)new CharacterRange[num2];
				for (int i = 0; i < num2; i++)
				{
					((CharacterRange)(ref array[i])).First = num + i;
					((CharacterRange)(ref array[i])).Length = 1;
				}
				sf.SetMeasurableCharacterRanges(array);
				Region[] array2;
				try
				{
					array2 = g.MeasureCharacterRanges(text, font, rect, sf);
				}
				catch
				{
					for (int j = 0; j < num2; j++)
					{
						((CharacterRange)(ref array[j])).First = j;
						((CharacterRange)(ref array[j])).Length = 1;
					}
					sf.SetMeasurableCharacterRanges(array);
					array2 = g.MeasureCharacterRanges(text.Substring(num, num2), font, rect, sf);
				}
				for (int k = 0; k < num2; k++)
				{
					rects[num + k] = array2[k].GetBounds(g);
				}
			}
			return rects[index].Value;
		}

		public MeasurableInfo(Graphics g, Font font, RectangleF rect, StringFormat sf)
		{
			this.g = g;
			this.font = font;
			this.rect = rect;
			this.sf = sf;
		}
	}

	private static Hashtable hashStringFormats = new Hashtable();

	public static bool MeasureTrailingSpaces { get; set; }

	private static void CorrectSize(ref Range range, RectangleD rect)
	{
		if (range.Pos.X + range.Size.Width > rect.Right)
		{
			range.Size.Width = rect.Right - range.Pos.X;
		}
		if (range.Pos.Y + range.Size.Height > rect.Bottom)
		{
			range.Size.Height = rect.Bottom - range.Pos.Y;
		}
	}

	private static Range[] GetRange(Graphics g, RectangleD rect, string text, Font font, StringFormat sf, double lineHeight = 1.0)
	{
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Invalid comparison between Unknown and I4
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Invalid comparison between Unknown and I4
		bool flag = text.EndsWith(StiTextRenderer.StiForceWidthAlignTag, StringComparison.InvariantCulture);
		if (flag)
		{
			text = text.Substring(0, text.Length - StiTextRenderer.StiForceWidthAlignTag.Length);
		}
		text = text.TrimEnd(new char[1] { ' ' });
		List<Range> list = new List<Range>();
		Word[] texts = Word.GetTexts(text);
		List<Line> list2 = new List<Line>();
		double num = 0.0;
		Font val = StiFontUtils.ChangeFontSize(font, 1024f);
		try
		{
			num = (double)((float)val.Height * font.Size / val.Size) * lineHeight;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		for (int i = 0; i < texts.Length; i++)
		{
			SizeF sizeF = g.MeasureString(texts[i].Text, font);
			list.Add(new Range(texts[i].Text, new SizeD(sizeF.Width, sizeF.Height), texts[i].IsEnter));
		}
		double num2 = rect.Left;
		double num3 = rect.Top;
		int num4 = 1;
		int num5 = 0;
		int start = 0;
		bool flag2 = false;
		Range[] array = list.ToArray();
		for (int j = 0; j < array.Length; j++)
		{
			num2 += array[j].Size.Width;
			if (flag2 || (num2 >= rect.Right && num5 >= 1))
			{
				num2 = rect.Left + array[j].Size.Width;
				num4++;
				num3 += num;
				list2.Add(new Line(start, num5, flag2));
				start = j;
				num5 = 0;
			}
			num5++;
			array[j].Pos.Y = num3;
			flag2 = array[j].NewLineForm;
		}
		list2.Add(new Line(start, num5, !flag));
		Line[] array2 = list2.ToArray();
		for (int k = 0; k < array2.Length; k++)
		{
			int start2 = array2[k].Start;
			int num6 = array2[k].Start + array2[k].Count - 1;
			array[start2].IsStart = true;
			array[num6].IsEnd = true;
			if (array2[k].IsEnter)
			{
				num2 = rect.Left;
				for (int l = start2; l <= num6; l++)
				{
					array[l].Pos.X = num2;
					num2 += array[l].Size.Width;
				}
				continue;
			}
			if (array2[k].Count == 1)
			{
				array[start2].Pos.X = rect.Left;
				continue;
			}
			array[start2].Pos.X = rect.Left;
			array[num6].Pos.X = rect.Right - array[num6].Size.Width;
			double num7 = 0.0;
			if (array2[k].Count > 2)
			{
				double num8 = 0.0;
				for (int m = start2 + 1; m < num6; m++)
				{
					num8 += array[m].Size.Width;
				}
				num7 = (rect.Width - array[start2].Size.Width - array[num6].Size.Width - num8) / (double)(array2[k].Count - 1);
			}
			num2 = array[start2].Size.Width + rect.Left + num7;
			for (int n = start2 + 1; n < num6; n++)
			{
				array[n].Pos.X = num2;
				num2 += num7 + array[n].Size.Width;
			}
		}
		if ((int)sf.LineAlignment != 0)
		{
			double num9 = num * ((double)array2.Length + 0.05);
			double num10 = 0.0;
			if ((int)sf.LineAlignment == 2)
			{
				num10 = rect.Height - num9;
			}
			if ((int)sf.LineAlignment == 1)
			{
				num10 = (rect.Height - num9) / 2.0;
			}
			for (int num11 = 0; num11 < array.Length; num11++)
			{
				array[num11].Pos.Y += num10;
			}
		}
		for (int num12 = 0; num12 < array.Length; num12++)
		{
			CorrectSize(ref array[num12], rect);
		}
		return array;
	}

	public static void DrawStringWidth(Graphics g, string text, Font font, Brush brush, RectangleD rect, StringFormat stringFormat, double lineHeight = 1.0)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Invalid comparison between Unknown and I4
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		Region clip = g.Clip;
		g.SetClip(rect.ToRectangleF(), (CombineMode)1);
		if (!string.IsNullOrEmpty(text))
		{
			if ((stringFormat.FormatFlags & 0x1000) > 0)
			{
				stringFormat.FormatFlags = (StringFormatFlags)(stringFormat.FormatFlags ^ 0x1000);
			}
			Range[] range = GetRange(g, rect, text, font, stringFormat, lineHeight);
			stringFormat.LineAlignment = (StringAlignment)0;
			stringFormat.FormatFlags = (StringFormatFlags)(stringFormat.FormatFlags | 0x2000);
			for (int i = 0; i < range.Length; i++)
			{
				if (range[i].IsStart)
				{
					stringFormat.Alignment = (StringAlignment)0;
				}
				else if (range[i].IsEnd)
				{
					stringFormat.Alignment = (StringAlignment)2;
				}
				else
				{
					stringFormat.Alignment = (StringAlignment)1;
				}
				g.DrawString(range[i].Text, font, brush, new RectangleD(range[i].Pos, range[i].Size).ToRectangleF(), stringFormat);
			}
		}
		g.SetClip(clip, (CombineMode)0);
	}

	public static void DrawStringWidth(Graphics g, string text, Font font, Brush brush, RectangleD rect, StringFormat stringFormat, float angle, double lineHeight = 1.0)
	{
		angle %= 360f;
		if (angle < 0f)
		{
			angle = 360f + angle;
		}
		if (angle != 0f)
		{
			Region clip = g.Clip;
			g.SetClip(rect.ToRectangleF(), (CombineMode)1);
			GraphicsState val = g.Save();
			g.TranslateTransform((float)(rect.Left + rect.Width / 2.0), (float)(rect.Top + rect.Height / 2.0));
			g.RotateTransform(0f - angle);
			rect.X = (0.0 - rect.Width) / 2.0;
			rect.Y = (0.0 - rect.Height) / 2.0;
			RectangleD rect2 = new RectangleD(rect.X, rect.Y, rect.Width, rect.Height);
			if ((angle > 45f && angle < 135f) || (angle > 225f && angle < 315f))
			{
				rect2 = new RectangleD(rect.Y, rect.X, rect.Height, rect.Width);
			}
			DrawStringWidth(g, text, font, brush, rect2, stringFormat, lineHeight);
			g.Restore(val);
			g.SetClip(clip, (CombineMode)0);
		}
		else
		{
			DrawStringWidth(g, text, font, brush, rect, stringFormat, lineHeight);
		}
	}

	public static void DrawString(Graphics g, string text, Font font, Brush brush, RectangleF rect, StringFormat stringFormat, float angle, double lineHeight = 1.0)
	{
		DrawString(g, text, font, brush, RectangleD.CreateFromRectangle(rect), stringFormat, angle);
	}

	public static void DrawString(Graphics g, string text, Font font, Brush brush, RectangleD rect, StringFormat stringFormat, float angle, double lineHeight = 1.0)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}
		angle %= 360f;
		if (angle < 0f)
		{
			angle = 360f + angle;
		}
		try
		{
			if (angle != 0f)
			{
				List<StiTextRenderer.LineInfo> list = null;
				if (lineHeight != 1.0)
				{
					list = SplitTextWordwrap(text, g, font, ((angle > 45f && angle < 135f) || (angle > 225f && angle < 315f)) ? new RectangleD(0.0, 0.0, rect.Height, rect.Width) : new RectangleD(0.0, 0.0, rect.Width, rect.Height), stringFormat);
				}
				Region clip = g.Clip;
				g.SetClip(rect.ToRectangleF(), (CombineMode)1);
				GraphicsState val = g.Save();
				g.TranslateTransform((float)(rect.Left + rect.Width / 2.0), (float)(rect.Top + rect.Height / 2.0));
				g.RotateTransform(0f - angle);
				rect.X = (0.0 - rect.Width) / 2.0;
				rect.Y = (0.0 - rect.Height) / 2.0;
				RectangleD rect2 = new RectangleD(rect.X, rect.Y, rect.Width, rect.Height);
				if ((angle > 45f && angle < 135f) || (angle > 225f && angle < 315f))
				{
					rect2 = new RectangleD(rect.Y, rect.X, rect.Height, rect.Width);
				}
				if (angle == 0f || angle == 90f || angle == 180f || angle == 270f)
				{
					if (lineHeight == 1.0)
					{
						g.DrawString(text, font, brush, rect2.ToRectangleF(), stringFormat);
					}
					else
					{
						DrawStringLineHeight(g, text, font, brush, rect2, stringFormat, lineHeight, list);
					}
				}
				else
				{
					stringFormat.SetTabStops(20f, new float[3] { 30f, 30f, 30f });
					stringFormat.Alignment = (StringAlignment)1;
					stringFormat.LineAlignment = (StringAlignment)1;
					if (lineHeight == 1.0)
					{
						g.DrawString(text, font, brush, (float)(rect2.X + rect2.Width / 2.0), (float)(rect2.Y + rect2.Height / 2.0), stringFormat);
					}
					else
					{
						DrawStringLineHeight(g, text, font, brush, rect2, stringFormat, lineHeight, list);
					}
				}
				g.Restore(val);
				g.SetClip(clip, (CombineMode)0);
			}
			else if (lineHeight == 1.0)
			{
				g.DrawString(text, font, brush, rect.ToRectangleF(), stringFormat);
			}
			else
			{
				Region clip2 = g.Clip;
				g.SetClip(rect.ToRectangleF(), (CombineMode)1);
				DrawStringLineHeight(g, text, font, brush, rect, stringFormat, lineHeight);
				g.SetClip(clip2, (CombineMode)0);
			}
		}
		catch
		{
		}
	}

	public static void DrawString(Graphics g, string text, Font font, Color color, RectangleD rect, StringFormat stringFormat)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		SolidBrush val = new SolidBrush(color);
		try
		{
			DrawString(g, text, font, (Brush)(object)val, rect, stringFormat, 0f);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static void DrawString(Graphics g, string text, Font font, Brush brush, RectangleD rect, StringFormat stringFormat)
	{
		DrawString(g, text, font, brush, rect, stringFormat, 0f);
	}

	public static void DrawString(Graphics g, string text, Font font, Brush brush, RectangleD rect, StiTextOptions textOptions, StiTextHorAlignment ha, StiVertAlignment va)
	{
		DrawString(g, text, font, brush, rect, textOptions, ha, va, antialiasing: false, 1f);
	}

	public static void DrawString(Graphics g, string text, Font font, Brush brush, RectangleD rect, StiTextOptions textOptions, StiTextHorAlignment ha, StiVertAlignment va, bool antialiasing, float zoom, double lineHeight = 1.0)
	{
		StringFormat stringFormat = GetStringFormat(textOptions, ha, va, antialiasing, zoom);
		try
		{
			if (ha == StiTextHorAlignment.Width)
			{
				DrawStringWidth(g, text, font, brush, rect, stringFormat, textOptions.Angle, lineHeight);
			}
			else
			{
				DrawString(g, text, font, brush, rect, stringFormat, textOptions.Angle, lineHeight);
			}
		}
		finally
		{
			((IDisposable)stringFormat)?.Dispose();
		}
	}

	private static void DrawStringLineHeight(Graphics g, string text, Font font, Brush brush, RectangleD rect, StringFormat sf, double lineHeight = 1.0, List<StiTextRenderer.LineInfo> list = null)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Invalid comparison between Unknown and I4
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Invalid comparison between Unknown and I4
		if (list == null)
		{
			list = SplitTextWordwrap(text, g, font, rect, sf);
		}
		float width = (((Enum)sf.FormatFlags).HasFlag((Enum)(object)(StringFormatFlags)4096) ? 0f : ((float)rect.Width));
		SizeF sizeF = g.MeasureString(text, font, new SizeF(width, 999999f), sf);
		bool flag = false;
		double num = (double)(sizeF.Height / (float)list.Count) * lineHeight;
		if ((double)sizeF.Height * lineHeight > rect.Height * 1.05)
		{
			num = list[0].LineHeight * lineHeight;
			flag = true;
		}
		StringFormat val = new StringFormat(sf);
		val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 0x1000);
		double num2 = 0.0;
		if (!flag)
		{
			if ((int)val.LineAlignment == 1)
			{
				num2 = (rect.Height - (double)sizeF.Height * lineHeight) / 2.0;
			}
			if ((int)val.LineAlignment == 2)
			{
				num2 = rect.Height - (double)sizeF.Height * lineHeight;
			}
		}
		RectangleF rectangleF = rect.ToRectangleF();
		rectangleF.Height = (float)num;
		rectangleF.Y += (float)num2;
		foreach (StiTextRenderer.LineInfo item in list)
		{
			g.DrawString(item.Text, font, brush, rectangleF, val);
			rectangleF.Y += (float)num;
			if (flag && (double)rectangleF.Y > rect.Bottom)
			{
				break;
			}
		}
	}

	public static List<StiTextRenderer.LineInfo> SplitTextWordwrap(string text, Graphics g, Font font, RectangleD rect, StiTextOptions textOptions, StiTextHorAlignment ha, bool typographic)
	{
		if (textOptions == null)
		{
			textOptions = new StiTextOptions();
		}
		float zoom = 10f;
		StringFormat stringFormat = GetStringFormat(textOptions, StiTextHorAlignment.Left, StiVertAlignment.Top, typographic, zoom);
		return SplitTextWordwrap(text, g, font, rect, stringFormat, ha == StiTextHorAlignment.Width);
	}

	public static List<StiTextRenderer.LineInfo> SplitTextWordwrap(string text, Graphics g, Font font, RectangleD rect, StringFormat sf, bool horAlignWidth = false)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		List<StiTextRenderer.LineInfo> list = new List<StiTextRenderer.LineInfo>();
		float num = 10f;
		double num2 = (((Enum)sf.FormatFlags).HasFlag((Enum)(object)(StringFormatFlags)4096) ? 999999.0 : rect.Width);
		double num3 = rect.Height * (double)num;
		RectangleF rect2 = new RectangleF(0f, 0f, (float)(num2 * (double)num), 999999f);
		Font font2 = StiFontUtils.ChangeFontSize(font, font.Size * num);
		MeasurableInfo measurableInfo = new MeasurableInfo(g, font2, rect2, sf);
		bool flag = false;
		List<string> list2 = SplitString(text, removeControl: true);
		for (int i = 0; i < list2.Count; i++)
		{
			string text2 = list2[i];
			if (text2.Length == 0)
			{
				list.Add(new StiTextRenderer.LineInfo
				{
					Text = string.Empty,
					LineHeight = measurableInfo.LineHeight / 10.0
				});
				continue;
			}
			measurableInfo.SetText(text2);
			if (!((Enum)sf.FormatFlags).HasFlag((Enum)(object)(StringFormatFlags)4096) && !flag)
			{
				RectangleF rect3 = measurableInfo.GetRect(0);
				double num4 = (double)rect3.Top + (double)rect3.Height / 2.0;
				int num5 = 0;
				int num6 = 0;
				for (int j = 1; j < text2.Length; j++)
				{
					if (num6 == 0)
					{
						int num7 = 15;
						if (j + num7 > text2.Length - 1)
						{
							num7 = text2.Length - j - 1;
						}
						if (num7 > 0)
						{
							rect3 = measurableInfo.GetRect(j + num7);
							if ((double)rect3.Top < num4 && !rect3.IsEmpty)
							{
								j += num7;
								continue;
							}
							num6 = num7;
						}
					}
					else
					{
						num6--;
					}
					rect3 = measurableInfo.GetRect(j);
					if ((double)rect3.Top > num4)
					{
						list.Add(MakeLineInfo(text2, num5, j - num5, measurableInfo, horAlignWidth, flag));
						num5 = j;
						num4 = (double)rect3.Top + (double)rect3.Height / 2.0;
						num6 = 0;
						if ((double)(list.Count - 1) * measurableInfo.LineHeight > num3)
						{
							flag = true;
							break;
						}
					}
				}
				if (num5 < text2.Length)
				{
					list.Add(MakeLineInfo(text2, num5, text2.Length - num5, measurableInfo, needWidthAligh: false, flag));
				}
			}
			else
			{
				list.Add(MakeLineInfo(text2, 0, text2.Length, measurableInfo, needWidthAligh: false, flag));
			}
		}
		for (int k = 0; k < list.Count; k++)
		{
			string text3 = list[k].Text;
			if (!string.IsNullOrWhiteSpace(text3) && text3.IndexOf('\u00ad') != -1)
			{
				string text4 = text3.Replace("\u00ad", "");
				if (text3[text3.Length - 1] == '\u00ad')
				{
					text4 += "\u00ad";
				}
				list[k].Text = text4;
			}
		}
		return list;
	}

	private static StiTextRenderer.LineInfo MakeLineInfo(string st, int begin, int length, MeasurableInfo info, bool needWidthAligh, bool needCut)
	{
		if (!needCut)
		{
			while (length > 0 && info.GetRect(begin + length - 1).IsEmpty)
			{
				length--;
			}
		}
		StiTextRenderer.LineInfo lineInfo = new StiTextRenderer.LineInfo();
		lineInfo.Text = st.Substring(begin, length);
		lineInfo.NeedWidthAlign = needWidthAligh;
		lineInfo.Widths = new double[length];
		if (!needCut)
		{
			for (int i = 0; i < length; i++)
			{
				lineInfo.Widths[i] = info.GetRect(begin + i).Width / 10f;
			}
		}
		lineInfo.LineHeight = info.GetRect(begin).Height / 10f;
		return lineInfo;
	}

	public static List<string> SplitString(string inputString, bool removeControl)
	{
		List<string> list = new List<string>();
		if (inputString == null)
		{
			inputString = string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		string text = inputString;
		foreach (char c in text)
		{
			if (c == '\n')
			{
				list.Add(stringBuilder.ToString().TrimEnd(new char[0]));
				stringBuilder.Length = 0;
			}
			else if (!removeControl || !char.IsControl(c) || c == '\t')
			{
				stringBuilder.Append(c);
			}
		}
		if (stringBuilder.Length > 0)
		{
			list.Add(stringBuilder.ToString().TrimEnd(new char[0]));
		}
		if (list.Count == 0)
		{
			list.Add(string.Empty);
		}
		return list;
	}

	public static string CutLineLimit(ref string text, Graphics g, Font font, RectangleD rect, StiTextOptions textOptions, bool typographic, bool isWindows = true)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrWhiteSpace(text))
		{
			return text;
		}
		if (textOptions == null)
		{
			textOptions = new StiTextOptions();
		}
		float num = (isWindows ? 10f : 1f);
		float num2 = (isWindows ? 1f : 1.23f);
		StringFormat stringFormat = GetStringFormat(textOptions, StiTextHorAlignment.Left, StiVertAlignment.Top, typographic, num);
		double num3 = (((Enum)stringFormat.FormatFlags).HasFlag((Enum)(object)(StringFormatFlags)4096) ? 999999.0 : rect.Width);
		double num4 = rect.Height * (double)num / (double)num2;
		RectangleF rect2 = new RectangleF(0f, 0f, (float)(num3 * (double)num), 999999f);
		Font font2 = StiFontUtils.ChangeFontSize(font, font.Size * num);
		MeasurableInfo measurableInfo = new MeasurableInfo(g, font2, rect2, stringFormat);
		string text2 = text;
		if (!isWindows)
		{
			text = text.Replace("\r", "");
			text2 = text.Replace("\n", "\n\u200b");
		}
		measurableInfo.SetText(text2);
		RectangleF rect3 = measurableInfo.GetRect(text.Length - 1);
		if (!rect3.IsEmpty && (double)rect3.Bottom < num4)
		{
			return text;
		}
		if ((double)measurableInfo.GetRect(0).Bottom > num4)
		{
			return string.Empty;
		}
		int num5 = 0;
		int num6 = text.Length - 1;
		while (num6 - num5 > 1)
		{
			int num7 = (num5 + num6) / 2;
			rect3 = measurableInfo.GetRect(num7);
			if (!isWindows && rect3.IsEmpty)
			{
				if (!char.IsWhiteSpace(text, num7))
				{
					rect3.Height = (float)(num4 + 1.0);
				}
				else
				{
					int num8 = num7;
					while (rect3.IsEmpty && char.IsWhiteSpace(text, num8) && num8 >= num5)
					{
						num8--;
						rect3 = measurableInfo.GetRect(num8);
					}
				}
			}
			if ((double)rect3.Bottom > num4)
			{
				num6 = num7;
			}
			else
			{
				num5 = num7;
			}
		}
		if (num6 - num5 == 1 && (double)measurableInfo.GetRect(num5).Bottom > num4)
		{
			return text.Substring(0, num5);
		}
		string result = text.Substring(0, num6);
		text = text.Substring(num6);
		return result;
	}

	public static string BreakTextWidth(Graphics g, ref string text, Font font, RectangleD rect, StringFormat stringFormat, double lineHeight = 1.0)
	{
		if (string.IsNullOrEmpty(text))
		{
			return string.Empty;
		}
		double num = 0.0;
		Font val = StiFontUtils.ChangeFontSize(font, 1024f);
		try
		{
			num = (double)((float)val.Height * font.Size / val.Size) * lineHeight;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		double num2 = rect.Left;
		double num3 = rect.Top;
		int num4 = 0;
		bool flag = false;
		Word[] texts = Word.GetTexts(text.TrimEnd(new char[1] { ' ' }));
		StringBuilder stringBuilder = new StringBuilder();
		int num5 = 0;
		for (num5 = 0; num5 < texts.Length; num5++)
		{
			SizeF sizeF = g.MeasureString(texts[num5].Text, font);
			num2 += (double)sizeF.Width;
			if (flag || (num2 >= rect.Right && num4 >= 1))
			{
				num2 = rect.Left + (double)sizeF.Width;
				num3 += num;
				num4 = 0;
			}
			num4++;
			if (num3 + (double)sizeF.Height > rect.Bottom)
			{
				break;
			}
			flag = texts[num5].IsEnter;
			stringBuilder.Append(texts[num5].Text + (texts[num5].IsEnter ? "\n" : " "));
		}
		if (num5 == 0)
		{
			return null;
		}
		if (num5 == texts.Length)
		{
			string result = text;
			text = string.Empty;
			return result;
		}
		StringBuilder stringBuilder2 = new StringBuilder();
		for (int i = num5; i < texts.Length; i++)
		{
			stringBuilder2.Append(texts[i].Text + (texts[i].IsEnter ? "\n" : " "));
		}
		text = stringBuilder2.ToString();
		return stringBuilder.ToString() + (flag ? "" : StiTextRenderer.StiForceWidthAlignTag);
	}

	public static List<string> SplitTextWordwrapWidth(string text, Graphics g, Font font, RectangleD rect)
	{
		bool flag = text.EndsWith(StiTextRenderer.StiForceWidthAlignTag, StringComparison.InvariantCulture);
		if (flag)
		{
			text = text.Substring(0, text.Length - StiTextRenderer.StiForceWidthAlignTag.Length);
		}
		text = text.TrimEnd(new char[1] { ' ' });
		List<Range> list = new List<Range>();
		Word[] texts = Word.GetTexts(text);
		List<Line> list2 = new List<Line>();
		double num = font.Height;
		for (int i = 0; i < texts.Length; i++)
		{
			SizeF sizeF = g.MeasureString(texts[i].Text, font);
			list.Add(new Range(texts[i].Text, new SizeD(sizeF.Width, sizeF.Height), texts[i].IsEnter));
		}
		double num2 = rect.Left;
		double num3 = rect.Top;
		int num4 = 1;
		int num5 = 0;
		int start = 0;
		bool flag2 = false;
		Range[] array = list.ToArray();
		for (int j = 0; j < array.Length; j++)
		{
			num2 += array[j].Size.Width;
			if (flag2 || (num2 >= rect.Right && num5 >= 1))
			{
				num2 = rect.Left + array[j].Size.Width;
				num4++;
				num3 += num;
				list2.Add(new Line(start, num5, flag2));
				start = j;
				num5 = 0;
			}
			num5++;
			array[j].Pos.Y = num3;
			flag2 = array[j].NewLineForm;
		}
		list2.Add(new Line(start, num5, !flag));
		List<string> list3 = new List<string>();
		foreach (Line item in list2)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num6 = item.Start + item.Count;
			for (int k = item.Start; k < num6; k++)
			{
				stringBuilder.Append(array[k].Text);
				if (k == num6 - 1)
				{
					stringBuilder.Append(item.IsEnter ? "" : "\a");
				}
				else
				{
					stringBuilder.Append(" ");
				}
			}
			list3.Add(stringBuilder.ToString());
		}
		return list3;
	}

	public static SizeF MeasureStringWidth(Graphics g, string text, Font font, SizeD size, StringFormat stringFormat, double lineHeight = 1.0)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Invalid comparison between Unknown and I4
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		if (!string.IsNullOrEmpty(text))
		{
			if ((stringFormat.FormatFlags & 0x1000) > 0)
			{
				stringFormat.FormatFlags = (StringFormatFlags)(stringFormat.FormatFlags ^ 0x1000);
			}
			Range[] range = GetRange(g, new RectangleD(0.0, 0.0, size.Width, size.Height), text, font, stringFormat, lineHeight);
			if (range.Length != 0)
			{
				Range range2 = range[^1];
				float height = (float)(range2.Pos.Y + range2.Size.Height - range[0].Pos.Y);
				return new SizeF((float)size.Width, height);
			}
		}
		return default(SizeF);
	}

	public static SizeF MeasureString(Graphics g, string text, Font font)
	{
		return MeasureString(g, text, font, (StringAlignment)1, (StringAlignment)1, 0f);
	}

	public static SizeF MeasureString(Graphics g, string text, Font font, StringAlignment horizontalAligment, StringAlignment vertiacalAligment, float angle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Invalid comparison between Unknown and I4
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		StiTextHorAlignment ha = (((int)horizontalAligment == 1) ? StiTextHorAlignment.Center : (((int)horizontalAligment == 2) ? StiTextHorAlignment.Right : StiTextHorAlignment.Left));
		StiVertAlignment va = (((int)vertiacalAligment == 1) ? StiVertAlignment.Center : (((int)vertiacalAligment == 2) ? StiVertAlignment.Bottom : StiVertAlignment.Top));
		return MeasureString(g, text, font, ha, va, angle).ToSizeF();
	}

	public static SizeD MeasureString(Graphics g, string text, Font font, StiTextHorAlignment ha, StiVertAlignment va, float angle)
	{
		return MeasureString(g, text, font, ha, va, antialiasing: false, angle);
	}

	public static SizeD MeasureString(Graphics g, string text, Font font, StiTextHorAlignment ha, StiVertAlignment va, bool antialiasing, float angle)
	{
		StiTextOptions stiTextOptions = new StiTextOptions();
		stiTextOptions.Angle = angle;
		return MeasureString(g, text, font, 0.0, stiTextOptions, ha, va, antialiasing);
	}

	public static SizeD MeasureString(Graphics g, string text, Font font, double width, StiTextOptions textOptions, StiTextHorAlignment ha, StiVertAlignment va, bool antialiasing, double lineHeight = 1.0)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Expected O, but got Unknown
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Expected O, but got Unknown
		int hashCode = textOptions.GetHashCode();
		hashCode = (hashCode * 397) ^ (int)ha;
		hashCode = (hashCode * 397) ^ (int)va;
		hashCode = (hashCode * 397) ^ (antialiasing ? 1231 : 1237);
		StringFormat val = (StringFormat)hashStringFormats[hashCode];
		if (val == null)
		{
			val = GetStringFormat(textOptions, ha, va, antialiasing, 1f);
			lock (hashStringFormats)
			{
				hashStringFormats[hashCode] = val;
			}
		}
		double num = width;
		if (!textOptions.WordWrap)
		{
			width = 0.0;
		}
		SizeF sizeF;
		try
		{
			if (ha == StiTextHorAlignment.Width)
			{
				sizeF = MeasureStringWidth(g, text, font, new SizeD((float)num, 999999.0), val, lineHeight);
			}
			else
			{
				sizeF = g.MeasureString(text, font, new SizeF((float)width, 999999f), val);
				sizeF.Height *= (float)lineHeight;
			}
		}
		catch
		{
			if (text.Length > 16384)
			{
				try
				{
					sizeF = g.MeasureString(text.Substring(0, 16384), font, new SizeF((float)width, 999999f), val);
					sizeF.Height *= (float)lineHeight;
				}
				catch
				{
					sizeF = new SizeF((float)width, 0f);
				}
			}
			else
			{
				sizeF = new SizeF((float)width, 0f);
			}
		}
		float num2 = textOptions.Angle % 360f;
		if (num2 < 0f)
		{
			num2 = 360f + num2;
		}
		if (num2 == 90f || num2 == 270f)
		{
			float width2 = sizeF.Width;
			sizeF.Width = sizeF.Height;
			sizeF.Height = width2;
		}
		else if (num2 != 0f)
		{
			float num3 = sizeF.Width / 2f;
			float num4 = sizeF.Height / 2f;
			PointF[] array = new PointF[4]
			{
				new PointF(0f - num3, 0f - num4),
				new PointF(num3, 0f - num4),
				new PointF(num3, num4),
				new PointF(0f - num3, num4)
			};
			Matrix val2 = new Matrix();
			val2.Rotate(0f - num2);
			val2.TransformPoints(array);
			int num5 = 0;
			PointF[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				PointF pointF = array2[i];
				double num6 = pointF.X;
				double num7 = pointF.Y;
				array[num5++] = new PointF((float)(num6 + (double)(sizeF.Width / 2f)), (float)(num7 + (double)(sizeF.Height / 2f)));
			}
			GraphicsPath val3 = new GraphicsPath();
			try
			{
				val3.AddPolygon(array);
				RectangleF bounds = val3.GetBounds();
				return new SizeD(bounds.Width, bounds.Height);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		return new SizeD(sizeF.Width, sizeF.Height);
	}

	public static StringFormat GetStringFormat(StiTextOptions textOptions, StiTextHorAlignment ha, StiVertAlignment va, float zoom)
	{
		return GetStringFormat(textOptions, ha, va, antialiasing: false, zoom);
	}

	public static StringAlignment GetAlignment(StiTextHorAlignment alignment)
	{
		switch (alignment)
		{
		case StiTextHorAlignment.Center:
		case StiTextHorAlignment.Width:
			return (StringAlignment)1;
		case StiTextHorAlignment.Right:
			return (StringAlignment)2;
		default:
			return (StringAlignment)0;
		}
	}

	public static StringAlignment GetAlignment(StiVertAlignment alignment)
	{
		return (StringAlignment)(alignment switch
		{
			StiVertAlignment.Center => 1, 
			StiVertAlignment.Bottom => 2, 
			_ => 0, 
		});
	}

	public static StringFormat GetStringFormat(StiTextOptions textOptions, StiTextHorAlignment ha, StiVertAlignment va, bool antialiasing, float zoom)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		StringFormat stringFormat = textOptions.GetStringFormat(antialiasing, zoom);
		stringFormat.Alignment = GetAlignment(ha);
		stringFormat.LineAlignment = GetAlignment(va);
		if (MeasureTrailingSpaces)
		{
			stringFormat.FormatFlags = (StringFormatFlags)(stringFormat.FormatFlags | 0x800);
		}
		return stringFormat;
	}
}
