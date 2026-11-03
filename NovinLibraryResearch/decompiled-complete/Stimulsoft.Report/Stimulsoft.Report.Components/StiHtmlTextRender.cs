using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Text;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components;

public class StiHtmlTextRender
{
	public class StiRune
	{
		public double X;

		public double Y;

		public double Height;

		public string Text;

		public double[] Widths;

		public double[] SumWidths;

		public double[] Scales;

		public int IndexBegin;

		public int IndexEnd;

		public int FontIndex;

		public StiHtmlTagsState State;

		public double Width => ((IndexEnd > 0) ? SumWidths[IndexEnd - 1] : 0.0) - ((IndexBegin > 0) ? SumWidths[IndexBegin - 1] : 0.0);

		public StiRune Clone()
		{
			return MemberwiseClone() as StiRune;
		}
	}

	public class StiOutlineTextMetric
	{
		public int otmAscent;

		public int otmDescent;

		public int otmExternalLeading;

		public int otmsStrikeoutSize;

		public int otmsStrikeoutPosition;

		public int otmsUnderscoreSize;

		public int otmsUnderscorePosition;

		public Font BaseFont;

		public int[] Widths;

		internal Graphics otmGraphics;

		public int GetWidth(int sym)
		{
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Expected O, but got Unknown
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			if (Widths[sym] == -1)
			{
				lock (lockOutlineTextMetricsCache)
				{
					StringFormat val = new StringFormat(StringFormat.GenericTypographic);
					try
					{
						val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 0x1000);
						int num = 32;
						int num2 = sym & 0xFFE0;
						int num3 = num2 + num;
						if (StiOptions.Export.Pdf.AllowImportSystemLibraries)
						{
							StringBuilder stringBuilder = new StringBuilder();
							for (int i = num2; i < num3; i++)
							{
								stringBuilder.Append((char)i);
							}
							string text = stringBuilder.ToString();
							CharacterRange[] array = (CharacterRange[])(object)new CharacterRange[num];
							for (int j = 0; j < num; j++)
							{
								((CharacterRange)(ref array[j])).First = j;
								((CharacterRange)(ref array[j])).Length = 1;
							}
							val.SetMeasurableCharacterRanges(array);
							Region[] array2 = (Region[])(object)new Region[num];
							try
							{
								otmGraphics.MeasureCharacterRanges(text, BaseFont, new RectangleF(0f, 0f, 10000000f, 10000000f), val).CopyTo(array2, 0);
							}
							catch
							{
								array = (CharacterRange[])(object)new CharacterRange[1];
								((CharacterRange)(ref array[0])).First = 0;
								((CharacterRange)(ref array[0])).Length = 1;
								val.SetMeasurableCharacterRanges(array);
								for (int k = 0; k < num; k++)
								{
									try
									{
										otmGraphics.MeasureCharacterRanges(text.Substring(k, 1), BaseFont, new RectangleF(0f, 0f, 10000000f, 10000000f), val).CopyTo(array2, k);
									}
									catch
									{
										array2[k] = new Region(new Rectangle(0, 0, 1000, 1000));
									}
								}
							}
							for (int l = num2; l < num3; l++)
							{
								double a = array2[l - num2].GetBounds(otmGraphics).Width;
								Widths[l] = (int)Math.Round(a);
							}
						}
						else
						{
							double num4 = otmGraphics.MeasureString("DD", BaseFont, 10000000, val).Width;
							for (int m = num2; m < num3; m++)
							{
								double num5 = (double)otmGraphics.MeasureString("D" + (char)m + "D", BaseFont, 10000000, val).Width - num4;
								if (char.GetUnicodeCategory((char)m) == UnicodeCategory.NonSpacingMark)
								{
									num5 = 0.0;
								}
								Widths[m] = (int)Math.Round(num5 * 0.99);
							}
						}
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			return Widths[sym];
		}

		public StiOutlineTextMetric(Graphics gr)
		{
			otmGraphics = gr;
			Widths = new int[65536];
			for (int i = 0; i < 65536; i++)
			{
				Widths[i] = -1;
			}
		}
	}

	public class StiFontState2 : StiFontState
	{
		public StiOutlineTextMetric Otm;

		public static StiFontState2 CreateFrom(StiFontState state)
		{
			return new StiFontState2
			{
				Ascend = state.Ascend,
				Descend = state.Descend,
				EmValue = state.EmValue,
				FontBase = state.FontBase,
				LineHeight = state.LineHeight,
				FontName = state.FontName
			};
		}
	}

	public class StiHtmlParser
	{
		private List<StiFontState> fontList;

		private StiHtmlState[] stateList;

		private List<StiRune> outputRunes;

		private List<StiRune> currentLineRunes;

		private double positionX;

		private double positionY;

		private double correctX;

		private RectangleD bounds;

		private double breakHeight;

		private bool needBreakText;

		private int lastLineBeginStateIndex;

		private int lastLineBeginRuneIndex = -1;

		private Graphics otmGraphics;

		public SizeD Parse(StiText textbox, string text, out List<RunInfo> outRunsList, out List<StiFontState> outFontsList, double breakHeight = 0.0, List<string> breakParts = null)
		{
			//IL_0aef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_020d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0223: Unknown result type (might be due to invalid IL or missing references)
			//IL_0379: Unknown result type (might be due to invalid IL or missing references)
			//IL_046f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0474: Unknown result type (might be due to invalid IL or missing references)
			//IL_0476: Unknown result type (might be due to invalid IL or missing references)
			//IL_049c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0897: Unknown result type (might be due to invalid IL or missing references)
			//IL_089d: Invalid comparison between Unknown and I4
			//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_08be: Unknown result type (might be due to invalid IL or missing references)
			//IL_08c4: Invalid comparison between Unknown and I4
			//IL_060c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0611: Unknown result type (might be due to invalid IL or missing references)
			//IL_07b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0682: Unknown result type (might be due to invalid IL or missing references)
			//IL_0684: Unknown result type (might be due to invalid IL or missing references)
			//IL_0689: Unknown result type (might be due to invalid IL or missing references)
			//IL_068f: Invalid comparison between Unknown and I4
			//IL_08f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_098e: Unknown result type (might be due to invalid IL or missing references)
			//IL_09a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_09be: Unknown result type (might be due to invalid IL or missing references)
			//IL_09d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a72: Unknown result type (might be due to invalid IL or missing references)
			//IL_0763: Unknown result type (might be due to invalid IL or missing references)
			//IL_0765: Unknown result type (might be due to invalid IL or missing references)
			//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_06bc: Invalid comparison between Unknown and I4
			//IL_0a24: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a30: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a3c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a58: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aa0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0773: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_06d4: Invalid comparison between Unknown and I4
			//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_07f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_07fd: Invalid comparison between Unknown and I4
			//IL_07ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0801: Unknown result type (might be due to invalid IL or missing references)
			//IL_080b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0812: Invalid comparison between Unknown and I4
			//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_082c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0820: Unknown result type (might be due to invalid IL or missing references)
			//IL_0711: Unknown result type (might be due to invalid IL or missing references)
			//IL_072f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0736: Unknown result type (might be due to invalid IL or missing references)
			otmGraphics = ((textbox.Report != null) ? textbox.GetMeasureGraphics() : OtmStaticGraphics);
			outRunsList = new List<RunInfo>();
			outFontsList = new List<StiFontState>();
			if (textbox.Page != null)
			{
				bounds = textbox.Page.Unit.ConvertToHInches(textbox.ClientRectangle);
			}
			else
			{
				bounds = textbox.GetPaintRectangle(convertToHInches: true, convertByZoom: false);
			}
			bounds = textbox.ConvertTextMargins(bounds, convert: false);
			bounds = textbox.ConvertTextBorders(bounds, convert: false);
			correctX = otmGraphics.MeasureString(" ", textbox.Font).Width / 5f;
			ref RectangleD reference = ref bounds;
			((RectangleD)(ref reference)).X = ((RectangleD)(ref reference)).X + correctX;
			ref RectangleD reference2 = ref bounds;
			((RectangleD)(ref reference2)).Width = ((RectangleD)(ref reference2)).Width - correctX * 2.0;
			float angle = textbox.Angle;
			if ((angle > 45f && angle < 135f) || (angle > 215f && angle < 315f))
			{
				double width = ((RectangleD)(ref bounds)).Width;
				((RectangleD)(ref bounds)).Width = ((RectangleD)(ref bounds)).Height;
				((RectangleD)(ref bounds)).Height = width;
			}
			this.breakHeight = breakHeight;
			needBreakText = breakParts != null;
			bool forceWidthAlign = text?.EndsWith(StiTextRenderer.StiForceWidthAlignTag, StringComparison.InvariantCulture) ?? false;
			SizeD result = default(SizeD);
			((SizeD)(ref result))._002Ector(0.0, 0.0);
			double num = ((RectangleD)(ref bounds)).Right;
			double num2 = 0.0;
			try
			{
				StiHtmlTagsState val = default(StiHtmlTagsState);
				((StiHtmlTagsState)(ref val))._002Ector(textbox.Font.Bold, textbox.Font.Italic, textbox.Font.Underline, textbox.Font.Strikeout, textbox.Font.SizeInPoints, textbox.Font.Name, StiBrush.ToColor(textbox.TextBrush), StiBrush.ToColor(textbox.Brush), false, false, 0.0, 0.0, (double)textbox.LineSpacing, textbox.HorAlignment);
				StiHtmlState val2 = default(StiHtmlState);
				((StiHtmlState)(ref val2))._002Ector(val, 0);
				stateList = StiTextRenderer.ParseHtmlToStates(text, val2, true).ToArray();
				fontList = new List<StiFontState>();
				for (int i = 0; i < stateList.Length; i++)
				{
					float fontSize = stateList[i].TS.FontSize;
					stateList[i].FontIndex = StiTextRenderer.GetFontIndex(stateList[i].TS.FontName, fontSize, stateList[i].TS.Bold, stateList[i].TS.Italic, stateList[i].TS.Underline, stateList[i].TS.Strikeout, stateList[i].TS.Superscript || stateList[i].TS.Subscript, fontList);
				}
				for (int j = 0; j < fontList.Count; j++)
				{
					StiFontState2 stiFontState = StiFontState2.CreateFrom(fontList[j]);
					((StiFontState)stiFontState).EmValue = ((StiFontState)stiFontState).FontBase.Size;
					StiOutlineTextMetric outlineTextMetricsCached = GetOutlineTextMetricsCached(((StiFontState)stiFontState).FontName, ((StiFontState)stiFontState).FontBase.Style, otmGraphics);
					double num3 = (double)((float)(outlineTextMetricsCached.otmAscent + outlineTextMetricsCached.otmDescent + outlineTextMetricsCached.otmExternalLeading) / StiTextRenderer.MaxFontSize) * ((StiFontState)stiFontState).EmValue;
					double ascend = (double)((float)outlineTextMetricsCached.otmAscent / StiTextRenderer.MaxFontSize) * ((StiFontState)stiFontState).EmValue;
					double descend = (double)((float)outlineTextMetricsCached.otmDescent / StiTextRenderer.MaxFontSize) * ((StiFontState)stiFontState).EmValue;
					double typoOffset = 0.0;
					double lineHeight = num3;
					((StiFontState)stiFontState).LineHeight = lineHeight;
					((StiFontState)stiFontState).Ascend = ascend;
					((StiFontState)stiFontState).Descend = descend;
					((StiFontState)stiFontState).TypoOffset = typoOffset;
					stiFontState.Otm = outlineTextMetricsCached;
					fontList[j] = (StiFontState)(object)stiFontState;
				}
				outputRunes = new List<StiRune>();
				currentLineRunes = new List<StiRune>();
				for (int k = 0; k < stateList.Length; k++)
				{
					StiHtmlState val3 = stateList[k];
					if (val3.Text.Length == 0)
					{
						continue;
					}
					if (needBreakText && positionY > breakHeight)
					{
						break;
					}
					if (val3.Text.Length == 1 && val3.Text[0] == '\n')
					{
						CompleteCurrentLine(k);
						continue;
					}
					StiRune stiRune = GetRune(val3);
					while (positionX + stiRune.Width > ((RectangleD)(ref bounds)).Width && textbox.WordWrap)
					{
						StiRune stiRune2 = BreakRune(stiRune, ((RectangleD)(ref bounds)).Width - positionX);
						if (stiRune2 != null)
						{
							AddRuneToCurrentLine(stiRune);
							stiRune = stiRune2;
							CompleteCurrentLine(k);
						}
						else
						{
							CompleteCurrentLine((k > 0) ? (k - 1) : 0);
						}
						if (needBreakText && positionY > breakHeight)
						{
							break;
						}
					}
					AddRuneToCurrentLine(stiRune);
				}
				if (currentLineRunes.Count > 0)
				{
					CompleteCurrentLine(stateList.Length - 1, forceWidthAlign);
				}
				if (needBreakText)
				{
					if (positionY < breakHeight)
					{
						breakParts.Add(text);
						breakParts.Add(string.Empty);
					}
					else if (lastLineBeginStateIndex == 0 && lastLineBeginRuneIndex < 0)
					{
						breakParts.Add(string.Empty);
						breakParts.Add(text);
					}
					else
					{
						StiRune stiRune3 = outputRunes[lastLineBeginRuneIndex];
						int num4 = lastLineBeginStateIndex;
						StiHtmlState val4 = stateList[num4];
						int startIndex = stiRune3.IndexEnd;
						bool flag = false;
						string text2 = null;
						string text3 = null;
						if (stiRune3.IndexEnd != stiRune3.Text.Length)
						{
							string text4 = stiRune3.Text.Substring(0, stiRune3.IndexEnd);
							if (lastLineBeginRuneIndex + 1 < outputRunes.Count)
							{
								startIndex = outputRunes[lastLineBeginRuneIndex + 1].IndexBegin;
							}
							if ((int)val4.TS.TextAlign == 3)
							{
								flag = true;
							}
							text2 = string.Concat(str1: (num4 <= 0 || (int)stateList[num4 - 1].TS.Tag.Tag != 20 || (int)val4.TS.Tag.Tag != 20) ? StiTextRenderer.StateToHtml(val4, val4, text4, 0) : ((stateList[num4 - 1].ListLevels != null && val4.ListLevels != null && stateList[num4 - 1].ListLevels.Count != val4.ListLevels.Count) ? (((val4.ListLevels[val4.ListLevels.Count - 1] > 0) ? "<ol>" : "<ul>") + text4) : ("<li>" + text4)), str0: text.Substring(0, val4.PosBegin), str2: flag ? StiTextRenderer.StiForceWidthAlignTag : "");
						}
						else
						{
							StiHtmlState val5 = ((num4 + 1 < stateList.Length) ? stateList[num4 + 1] : val4);
							text2 = text.Substring(0, val5.PosBegin) + (flag ? StiTextRenderer.StiForceWidthAlignTag : "");
						}
						int num5 = num4 + 1;
						text3 = StiTextRenderer.StateToHtml(val4, (((int)val4.TS.Tag.Tag == 20 || (int)val4.TS.Tag.Tag == 16) && num5 < stateList.Length) ? stateList[num5] : val4, stiRune3.Text.Substring(startIndex), 0);
						if (num5 < stateList.Length)
						{
							text3 += text.Substring(stateList[num5].PosBegin);
						}
						breakParts.Add(text2);
						breakParts.Add(text3);
					}
				}
				else
				{
					double num6 = 0.0;
					if ((int)textbox.VertAlignment == 1)
					{
						num6 = (((RectangleD)(ref bounds)).Height - positionY) / 2.0;
					}
					if ((int)textbox.VertAlignment == 2)
					{
						num6 = ((RectangleD)(ref bounds)).Height - positionY;
					}
					foreach (StiRune outputRune in outputRunes)
					{
						RunInfo val6 = default(RunInfo);
						val6.Text = outputRune.Text.Substring(outputRune.IndexBegin, outputRune.IndexEnd - outputRune.IndexBegin);
						val6.XPos = outputRune.X;
						val6.YPos = outputRune.Y + num6;
						val6.TextColor = outputRune.State.FontColor;
						val6.BackColor = outputRune.State.BackColor;
						val6.FontIndex = outputRune.FontIndex;
						val6.Href = outputRune.State.Href;
						val6.Widths = new int[val6.Text.Length];
						val6.GlyphWidths = new int[val6.Text.Length];
						val6.ScaleList = new double[val6.Text.Length];
						val6.GlyphIndexList = new int[val6.Text.Length];
						int num7 = 0;
						double num8 = 0.0;
						for (int l = 0; l < val6.Widths.Length; l++)
						{
							num8 += outputRune.Widths[outputRune.IndexBegin + l];
							int num9 = (int)Math.Round(num8 - (double)num7);
							val6.Widths[l] = num9;
							val6.GlyphWidths[l] = num9;
							val6.ScaleList[l] = outputRune.Scales[outputRune.IndexBegin + l];
							val6.GlyphIndexList[l] = -1;
							num7 += num9;
						}
						num = Math.Min(num, outputRune.X);
						num2 = Math.Max(num2, outputRune.X + num8);
						outRunsList.Add(val6);
					}
					outFontsList.AddRange(fontList);
				}
			}
			catch
			{
			}
			finally
			{
				((SizeD)(ref result)).Width = num2 - num;
				((SizeD)(ref result)).Height = positionY;
			}
			return result;
		}

		private void AddRuneToCurrentLine(StiRune rune)
		{
			rune.X = positionX;
			rune.Y = positionY;
			currentLineRunes.Add(rune);
			positionX += rune.Width;
		}

		private void CompleteCurrentLine(int stateIndex, bool forceWidthAlign = false)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Invalid comparison between Unknown and I4
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Invalid comparison between Unknown and I4
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f4: Invalid comparison between Unknown and I4
			//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
			StiHtmlState val = stateList[stateIndex];
			int fontIndex = val.FontIndex;
			double num = 0.0;
			int num2 = 0;
			foreach (StiRune currentLineRune in currentLineRunes)
			{
				if (fontList[fontIndex].EmValue < fontList[currentLineRune.FontIndex].EmValue)
				{
					fontIndex = currentLineRune.FontIndex;
				}
				else if (fontList[fontIndex].LineHeight < fontList[currentLineRune.FontIndex].LineHeight)
				{
					fontIndex = currentLineRune.FontIndex;
				}
				num += currentLineRune.Width;
				for (int i = currentLineRune.IndexBegin; i < currentLineRune.IndexEnd; i++)
				{
					char c = currentLineRune.Text[i];
					if (char.IsWhiteSpace(c) && c != '\u00a0')
					{
						num2++;
					}
				}
			}
			double lineHeight = fontList[fontIndex].LineHeight;
			StiTextHorAlignment textAlign = val.TS.TextAlign;
			if (currentLineRunes.Count > 0)
			{
				textAlign = currentLineRunes[currentLineRunes.Count - 1].State.TextAlign;
			}
			double num3 = 0.0;
			bool flag = forceWidthAlign;
			if ((int)textAlign == 3 && currentLineRunes.Count > 0)
			{
				StiRune stiRune = currentLineRunes[currentLineRunes.Count - 1];
				if (stiRune.IndexEnd != stiRune.Text.Length)
				{
					flag = true;
				}
			}
			if (flag && num2 > 0)
			{
				num3 = (((RectangleD)(ref bounds)).Width - num) / (double)num2;
			}
			double num4 = 0.0;
			if ((int)textAlign == 1)
			{
				num4 = (((RectangleD)(ref bounds)).Width - num) / 2.0;
			}
			if ((int)textAlign == 2)
			{
				num4 = ((RectangleD)(ref bounds)).Width - num;
			}
			double num5 = lineHeight - fontList[fontIndex].Descend;
			double num6 = 0.0;
			foreach (StiRune currentLineRune2 in currentLineRunes)
			{
				currentLineRune2.X += num4 + correctX + num6;
				if (currentLineRune2.State.Superscript || currentLineRune2.State.Subscript)
				{
					if (currentLineRune2.State.Superscript)
					{
						currentLineRune2.Y += lineHeight - fontList[fontList[currentLineRune2.FontIndex].ParentFontIndex].LineHeight;
					}
					else
					{
						currentLineRune2.Y += num5 + fontList[fontList[currentLineRune2.FontIndex].ParentFontIndex].Descend - currentLineRune2.Height;
					}
				}
				else
				{
					currentLineRune2.Y += num5 + fontList[currentLineRune2.FontIndex].Descend - currentLineRune2.Height;
				}
				if (!flag)
				{
					continue;
				}
				for (int j = currentLineRune2.IndexBegin; j < currentLineRune2.IndexEnd; j++)
				{
					char c2 = currentLineRune2.Text[j];
					if (char.IsWhiteSpace(c2) && c2 != '\u00a0')
					{
						double num7 = currentLineRune2.Widths[j] + num3;
						currentLineRune2.Scales[j] = num7 / currentLineRune2.Widths[j];
						currentLineRune2.Widths[j] = num7;
						num6 += num3;
					}
				}
			}
			outputRunes.AddRange(currentLineRunes);
			positionX = 0.0;
			positionY += lineHeight * val.TS.LineHeight;
			currentLineRunes.Clear();
			if (needBreakText && !(positionY > breakHeight))
			{
				lastLineBeginStateIndex = stateIndex;
				lastLineBeginRuneIndex = outputRunes.Count - 1;
			}
		}

		private StiRune GetRune(StiHtmlState state)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			string text = StiTextRenderer.PrepareStateText(state.Text).ToString();
			StiRune stiRune = new StiRune();
			stiRune.Text = text;
			stiRune.Widths = new double[text.Length];
			stiRune.SumWidths = new double[text.Length];
			stiRune.Scales = new double[text.Length];
			stiRune.IndexBegin = 0;
			stiRune.IndexEnd = text.Length;
			stiRune.State = state.TS;
			stiRune.FontIndex = state.FontIndex;
			StiFontState2 stiFontState = fontList[state.FontIndex] as StiFontState2;
			double num = ((StiFontState)stiFontState).EmValue * state.TS.LetterSpacing;
			double num2 = ((StiFontState)stiFontState).EmValue * state.TS.WordSpacing;
			double num3 = 0.0;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				double num4 = (double)((float)stiFontState.Otm.GetWidth(c) / StiTextRenderer.MaxFontSize) * ((StiFontState)stiFontState).EmValue;
				double num5 = num4 + num;
				if (char.IsWhiteSpace(c) && c != '\u00a0')
				{
					num5 += num2;
				}
				num3 += num5;
				stiRune.Widths[i] = num5;
				stiRune.SumWidths[i] = num3;
				stiRune.Scales[i] = ((num4 == 0.0) ? 1.0 : (num5 / num4));
			}
			stiRune.Height = fontList[state.FontIndex].LineHeight;
			return stiRune;
		}

		private StiRune BreakRune(StiRune baseRune, double size)
		{
			int i = baseRune.IndexBegin;
			for (double num = ((i > 0) ? baseRune.SumWidths[i - 1] : 0.0); baseRune.SumWidths[i] - num < size; i++)
			{
			}
			int num2 = i;
			while (!IsWrapPoint(baseRune.Text, num2) && num2 > baseRune.IndexBegin)
			{
				num2--;
			}
			StiRune stiRune = null;
			if (num2 == baseRune.IndexBegin)
			{
				if (currentLineRunes.Count > 0)
				{
					stiRune = null;
				}
				else
				{
					if (i == baseRune.IndexBegin)
					{
						i++;
					}
					stiRune = baseRune.Clone();
					stiRune.IndexBegin = i;
					baseRune.IndexEnd = i;
				}
			}
			else
			{
				while (num2 > baseRune.IndexBegin && char.IsWhiteSpace(baseRune.Text, num2 - 1))
				{
					num2--;
				}
				int j;
				for (j = num2; j < baseRune.IndexEnd && char.IsWhiteSpace(baseRune.Text, j); j++)
				{
				}
				stiRune = baseRune.Clone();
				stiRune.IndexBegin = j;
				baseRune.IndexEnd = num2;
			}
			return stiRune;
		}

		private bool IsWrapPoint(string st, int pos)
		{
			char c = st[pos];
			if (c == ' ')
			{
				return true;
			}
			if (char.GetUnicodeCategory(c) == UnicodeCategory.SpaceSeparator && c != '\u00a0')
			{
				return true;
			}
			if (pos > 0)
			{
				char c2 = st[pos - 1];
				if (c2 == ' ')
				{
					return true;
				}
				if (isCJKSymbol(c2) && isCJKSymbol(c))
				{
					return true;
				}
				if (c2 == '!' || c2 == '%' || c2 == ')' || c2 == '}' || c2 == '-' || c2 == '?' || c2 == '）' || c2 == '：' || c2 == '、' || c2 == '，' || c2 == '。')
				{
					return true;
				}
			}
			return false;
		}

		private static bool isCJKSymbol(int sym)
		{
			if (sym < 19968 || sym > 40908)
			{
				if (sym >= 13312)
				{
					return sym <= 19893;
				}
				return false;
			}
			return true;
		}
	}

	private static Hashtable outlineTextMetricsCache = new Hashtable();

	private static object lockOutlineTextMetricsCache = new object();

	private static Graphics otmStaticGraphics;

	private static Graphics OtmStaticGraphics
	{
		get
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Expected O, but got Unknown
			if (otmStaticGraphics == null)
			{
				Image val = (Image)new Bitmap(1, 1);
				otmStaticGraphics = Graphics.FromImage(val);
			}
			return otmStaticGraphics;
		}
	}

	private static StiOutlineTextMetric GetOutlineTextMetricsCached(string fontName, FontStyle fontStyle, Graphics otmGraphics)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		string key = fontName + "*" + (char)(48 + fontStyle);
		object obj = outlineTextMetricsCache[key];
		if (obj == null)
		{
			StiOutlineTextMetric stiOutlineTextMetric = new StiOutlineTextMetric(otmGraphics);
			lock (lockOutlineTextMetricsCache)
			{
				try
				{
					stiOutlineTextMetric.BaseFont = StiFontCollection.CreateFont(fontName, (float)((double)StiTextRenderer.MaxFontSize * StiDpiHelper.GraphicsScale), fontStyle);
					StringFormat val = new StringFormat(StringFormat.GenericTypographic);
					try
					{
						val.FormatFlags = (StringFormatFlags)(val.FormatFlags | 0x1000);
						string text = "0\n1";
						val.SetMeasurableCharacterRanges((CharacterRange[])(object)new CharacterRange[1]
						{
							new CharacterRange(2, 1)
						});
						Region[] array = otmGraphics.MeasureCharacterRanges(text, stiOutlineTextMetric.BaseFont, new RectangleF(0f, 0f, 10000000f, 10000000f), val);
						RectangleF bounds = array[0].GetBounds(otmGraphics);
						double num = bounds.Height;
						double num2 = bounds.Y;
						double num3 = 0.0;
						if (!StiOptions.Export.Pdf.AllowImportSystemLibraries)
						{
							num = otmGraphics.MeasureString("D\r\nD\r\nD\r\nD\r\nD\r\nD\r\nD\r\nD\r\nD\r\nD", stiOutlineTextMetric.BaseFont, 10000000, val).Height / 10f;
							num2 = num;
							num3 = otmGraphics.MeasureString("DD", stiOutlineTextMetric.BaseFont, 10000000, val).Width;
						}
						if (StiOptions.Engine.TextLineSpacingScale != 1f)
						{
							num *= (double)StiOptions.Engine.TextLineSpacingScale;
							num2 *= (double)StiOptions.Engine.TextLineSpacingScale;
						}
						stiOutlineTextMetric.otmAscent = (int)(num * 0.8);
						stiOutlineTextMetric.otmDescent = (int)(num * 0.2);
						stiOutlineTextMetric.otmExternalLeading = (int)(num2 - num);
						stiOutlineTextMetric.otmsStrikeoutPosition = (int)(num * 0.25);
						stiOutlineTextMetric.otmsStrikeoutSize = (int)(num * 0.05);
						stiOutlineTextMetric.otmsUnderscorePosition = -(int)(num * 0.1);
						stiOutlineTextMetric.otmsUnderscoreSize = (int)(num * 0.07);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
				catch
				{
				}
				outlineTextMetricsCache[key] = stiOutlineTextMetric;
				return stiOutlineTextMetric;
			}
		}
		StiOutlineTextMetric stiOutlineTextMetric2 = (StiOutlineTextMetric)obj;
		stiOutlineTextMetric2.otmGraphics = otmGraphics;
		return stiOutlineTextMetric2;
	}

	public static void DrawString(Graphics g, RectangleD rect, string text, StiText textBox, double scale = 1.0)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		StiHtmlParser stiHtmlParser = new StiHtmlParser();
		stiHtmlParser.Parse(textBox, text, out var outRunsList, out var outFontsList);
		double num = textBox.Page.Zoom * scale;
		double num2 = num * StiScale.Factor;
		if (StiScale.IsNoScaling && !textBox.IsPrinting && StiDpiHelper.NeedDeviceCapsScale)
		{
			num *= StiDpiHelper.DeviceCapsScale;
		}
		GraphicsState val = g.Save();
		try
		{
			g.SetClip(((RectangleD)(ref rect)).ToRectangleF());
			StringFormat val2 = new StringFormat(StringFormat.GenericTypographic);
			try
			{
				val2.FormatFlags = (StringFormatFlags)(val2.FormatFlags | 0x1000);
				foreach (RunInfo item in outRunsList)
				{
					int num3 = item.Widths.Sum();
					StiFontState val3 = outFontsList[item.FontIndex];
					if (num != 1.0 && val3.FontScaled == null)
					{
						val3.FontScaled = new Font(val3.FontBase.FontFamily, (float)(val3.EmValue * num), val3.FontBase.Style);
					}
					Font val4 = ((num == 1.0) ? val3.FontBase : val3.FontScaled);
					Rectangle rectangle = new Rectangle((int)(((RectangleD)(ref rect)).X + item.XPos * num2), (int)(((RectangleD)(ref rect)).Y + item.YPos * num2), (int)((double)num3 * num2), (int)(val3.LineHeight * num2));
					SolidBrush val5 = new SolidBrush(item.BackColor);
					try
					{
						SolidBrush val6 = new SolidBrush(item.TextColor);
						try
						{
							g.FillRectangle((Brush)(object)val5, rectangle);
							bool flag = false;
							double[] scaleList = item.ScaleList;
							foreach (double num4 in scaleList)
							{
								if (num4 < 0.99 || num4 > 1.01)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								g.DrawString(item.Text, val4, (Brush)(object)val6, new PointF((float)(((RectangleD)(ref rect)).X + item.XPos * num2), (float)(((RectangleD)(ref rect)).Y + item.YPos * num2)), val2);
								continue;
							}
							double num5 = ((RectangleD)(ref rect)).X + item.XPos * num2;
							double num6 = ((RectangleD)(ref rect)).Y + item.YPos * num2;
							for (int j = 0; j < item.Text.Length; j++)
							{
								g.DrawString(item.Text[j].ToString(), val4, (Brush)(object)val6, new PointF((float)num5, (float)num6), val2);
								num5 += (double)item.Widths[j] * num2;
							}
						}
						finally
						{
							((IDisposable)val6)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			g.Restore(val);
		}
	}

	public static SizeD MeasureString(StiText textBox)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		StiHtmlParser stiHtmlParser = new StiHtmlParser();
		List<RunInfo> outRunsList;
		List<StiFontState> outFontsList;
		SizeD result = stiHtmlParser.Parse(textBox, textBox.Text, out outRunsList, out outFontsList);
		if (textBox.Report != null && textBox.Report.ReportUnit != StiReportUnitType.HundredthsOfInch)
		{
			StiUnit unit = textBox.Report.Unit;
			double num = ((SizeD)(ref result)).Width + textBox.Margins.Left + textBox.Margins.Right + textBox.Border.Size;
			if (unit.ConvertToHInches(Math.Round(unit.ConvertFromHInches(num), 2)) < num)
			{
				((SizeD)(ref result)).Width = ((SizeD)(ref result)).Width + unit.ConvertToHInches(0.01);
			}
		}
		if (textBox.Angle == 90f || textBox.Angle == 180f)
		{
			((SizeD)(ref result)).Width = ((SizeD)(ref result)).Width * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorTypographic;
		}
		else
		{
			((SizeD)(ref result)).Height = ((SizeD)(ref result)).Height * StiOptions.Engine.TextDrawingMeasurement.MeasurementFactorTypographic;
		}
		return result;
	}

	public static string BreakText(RectangleD rect, ref string text, StiText textBox)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		List<string> list = new List<string>();
		StiHtmlParser stiHtmlParser = new StiHtmlParser();
		stiHtmlParser.Parse(textBox, textBox.Text, out var _, out var _, ((RectangleD)(ref rect)).Height, list);
		text = list[1];
		return list[0];
	}

	public static void DrawTextForOutput(StiText textBox, out List<RunInfo> outRunsList, out List<StiFontState> outFontsList)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		StiHtmlParser stiHtmlParser = new StiHtmlParser();
		stiHtmlParser.Parse(textBox, textBox.Text, out outRunsList, out outFontsList);
	}
}
