using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Maps.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Report.Export;

public static class StiMapSvgHelper
{
	public static void DrawMap(XmlTextWriter xmlsWriter, StiMap map, double x, double y, double width, double height, bool animated)
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		StiMapLoader.DeleteAllCustomMaps();
		StiCustomMapFinder.Clear();
		StiMapSvgContainer stiMapSvgContainer = StiMapLoader.LoadResource(map.Report, map.MapIdent, map.Language);
		if (stiMapSvgContainer != null)
		{
			double num = ((width / (double)stiMapSvgContainer.Width < height / (double)stiMapSvgContainer.Height) ? (width / (double)stiMapSvgContainer.Width) : (height / (double)stiMapSvgContainer.Height));
			xmlsWriter.WriteStartElement("rect");
			if (x != 0.0)
			{
				xmlsWriter.WriteAttributeString("x", NormalizeDecimal(x));
			}
			if (y != 0.0)
			{
				xmlsWriter.WriteAttributeString("y", NormalizeDecimal(y));
			}
			xmlsWriter.WriteAttributeString("width", NormalizeDecimal(width));
			xmlsWriter.WriteAttributeString("height", NormalizeDecimal(height));
			Color color = ((StiSolidBrush)map.GetStyleBackground()).Color;
			if (map.Brush is StiSolidBrush && ((StiSolidBrush)map.Brush).Color != Color.Transparent)
			{
				color = ((StiSolidBrush)map.Brush).Color;
			}
			xmlsWriter.WriteAttributeString("style", string.Format("fill:rgb({0},{1},{2});fill-opacity:{3};", color.R, color.G, color.B, Math.Round((float)(int)color.A / 255f, 3).ToString().Replace(",", ".")));
			xmlsWriter.WriteEndElement();
			xmlsWriter.WriteStartElement("g");
			xmlsWriter.WriteAttributeString("transform", $"translate({NormalizeDecimal((float)((width - (double)stiMapSvgContainer.Width * num) / 2.0 + x))},{NormalizeDecimal((float)((height - (double)stiMapSvgContainer.Height * num) / 2.0 + y))})");
			Render(map, xmlsWriter, animated, num);
			xmlsWriter.WriteEndElement();
		}
	}

	public static void Render(StiMap map, XmlTextWriter xmlsWriter, bool animated, double sScale)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_0a0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a15: Expected O, but got Unknown
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Expected O, but got Unknown
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0584: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Expected I4, but got Unknown
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Expected I4, but got Unknown
		StiGdiMapContextPainter stiGdiMapContextPainter = new StiGdiMapContextPainter(map);
		stiGdiMapContextPainter.MapStyle = StiMap.GetMapStyle(map);
		stiGdiMapContextPainter.DataTable = map.DataTable;
		stiGdiMapContextPainter.PrepareDataColumns();
		stiGdiMapContextPainter.UpdateGroupedData();
		stiGdiMapContextPainter.UpdateHeatmapWithGroup();
		List<StiMapData> mapData = stiGdiMapContextPainter.MapData;
		StiMapStyle stiMapStyle = null;
		if (!string.IsNullOrEmpty(map.ComponentStyle))
		{
			stiMapStyle = map.Report.Styles[map.ComponentStyle] as StiMapStyle;
		}
		if (stiMapStyle == null)
		{
			stiMapStyle = StiMap.GetMapStyle(map.MapStyle);
		}
		stiGdiMapContextPainter.DefaultBrush = new SolidBrush(stiMapStyle.DefaultColor);
		stiGdiMapContextPainter.DefaultBrush1 = new StiSolidBrush(stiMapStyle.DefaultColor);
		StiMapSvgContainer stiMapSvgContainer = StiMapLoader.LoadResource(map.Report, map.MapIdent, map.Language);
		Hashtable hashtable = new Hashtable();
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				if (stiMapSvgContainer != null)
				{
					int num = 0;
					int count = stiMapSvgContainer.HashPaths.Keys.Count;
					bool flag = (!map.ShowValue && map.DisplayNameType == StiDisplayNameType.None) || !(sScale >= 0.2);
					float num3 = (stiGdiMapContextPainter.IndividualStep = 0.5f / (float)count);
					List<string> list = new List<string>(stiMapSvgContainer.HashPaths.Keys);
					list.Sort();
					foreach (string item in list)
					{
						StiMapData stiMapData = mapData[0];
						foreach (StiMapData item2 in mapData)
						{
							if (item2.Key == item)
							{
								stiMapData = item2;
							}
						}
						Brush geomBrush = stiGdiMapContextPainter.GetGeomBrush(stiMapData);
						stiGdiMapContextPainter.IndividualStep += num3;
						xmlsWriter.WriteStartElement("path");
						xmlsWriter.WriteAttributeString("d", stiMapSvgContainer.HashPaths[item].Data);
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append(GetFillBrush((SolidBrush)(object)((geomBrush is SolidBrush) ? geomBrush : null)));
						string text = GetBorderStroke(stiMapStyle.BorderColor) ?? "";
						stringBuilder.Append(text + ";stroke-width:" + NormalizeDecimal(stiMapStyle.BorderSize) + ";");
						xmlsWriter.WriteAttributeString("style", stringBuilder.ToString());
						xmlsWriter.WriteAttributeString("transform", "scale(" + NormalizeDecimal(sScale) + ")");
						xmlsWriter.WriteAttributeString("_ismap", "true");
						xmlsWriter.WriteAttributeString("_text1", GetToolTipIdent(stiMapData));
						string text2 = GetToolTipValueText(map, stiMapData.Value);
						string toolTipTotalText = GetToolTipTotalText(map, mapData, stiMapData);
						if (!string.IsNullOrEmpty(toolTipTotalText))
						{
							text2 = StiLocalization.Get("FormFormatEditor", "BooleanValue").Replace(":", string.Empty) + " = " + text2 + "<div style='margin-top: 5px;'>" + toolTipTotalText + "<div>";
						}
						xmlsWriter.WriteAttributeString("_text2", text2);
						xmlsWriter.WriteAttributeString("elementargument", item);
						xmlsWriter.WriteAttributeString("elementident", item);
						xmlsWriter.WriteAttributeString("elementvalue", (stiMapData.Value != null) ? stiMapData.Value.ToString() : string.Empty);
						Color color = ((SolidBrush)((geomBrush is SolidBrush) ? geomBrush : null)).Color;
						xmlsWriter.WriteAttributeString("_color", $"#{color.R:X2}{color.G:X2}{color.B:X2}");
						if (animated)
						{
							xmlsWriter.WriteAttributeString("opacity", "0");
							xmlsWriter.WriteAttributeString("_animation", string.Format("{{\"actions\":[[\"opacity\", 0, 1, \"\"], [\"scale\", {2}, {2},\"\"]], \"begin\":{0}, \"duration\":{1}}}", NormalizeDecimal(200 / count * num), "100", NormalizeDecimal(sScale)));
						}
						xmlsWriter.WriteEndElement();
						if (!flag)
						{
							hashtable[item] = stiMapData;
						}
						num++;
					}
					if (hashtable.Keys.Count > 0)
					{
						float num4 = 19f;
						if (stiMapSvgContainer.TextScale.HasValue)
						{
							num4 *= (float)stiMapSvgContainer.TextScale.GetValueOrDefault();
							flag = false;
						}
						Font val3 = new Font("Calibri", num4);
						Color[] array = new Color[2]
						{
							Color.FromArgb(180, 251, 251, 251),
							Color.FromArgb(255, 37, 37, 37)
						};
						foreach (string key in hashtable.Keys)
						{
							StiMapSvg stiMapSvg = stiMapSvgContainer.HashPaths[key];
							bool flag2 = false;
							if ((map.DisplayNameType != StiDisplayNameType.Short) ? stiMapSvg.SkipText : ((!stiMapSvg.SkipTextIso.HasValue) ? stiMapSvg.SkipText : stiMapSvg.SkipTextIso.GetValueOrDefault()))
							{
								continue;
							}
							StiMapData stiMapData2 = hashtable[key] as StiMapData;
							string pathText = GetPathText(map, stiMapData2, stiMapSvg, key);
							if (string.IsNullOrEmpty(pathText))
							{
								continue;
							}
							Rectangle pathRect = GetPathRect(map, stiMapSvg);
							SizeF sizeF = ((map.DisplayNameType == StiDisplayNameType.Full && stiMapSvg.SetMaxWidth) ? val2.MeasureString(pathText, val3, pathRect.Width) : val2.MeasureString(pathText, val3));
							float num5 = 0f;
							float num6 = 0f;
							StiTextHorAlignment pathHorAlignment = GetPathHorAlignment(map, stiMapSvg);
							switch ((int)pathHorAlignment)
							{
							case 0:
							case 3:
								num5 = pathRect.X;
								break;
							case 2:
								num5 = (float)pathRect.Right - sizeF.Width;
								break;
							case 1:
								num5 = (float)pathRect.X + ((float)pathRect.Width - sizeF.Width) / 2f;
								break;
							}
							num5 += 25f;
							StiVertAlignment pathVertAlignment = GetPathVertAlignment(map, stiMapSvg);
							switch ((int)pathVertAlignment)
							{
							case 0:
								num6 = pathRect.Y;
								break;
							case 2:
								num6 = (float)pathRect.Bottom - sizeF.Height;
								break;
							case 1:
								num6 = (float)pathRect.Y + ((float)pathRect.Height - sizeF.Height) / 2f;
								break;
							}
							num6 += 30f;
							if (map.ShowValue && !string.IsNullOrEmpty(stiMapData2?.Value) && pathText.Contains(Environment.NewLine))
							{
								num6 += 8f;
							}
							for (int i = 0; i < 2; i++)
							{
								xmlsWriter.WriteStartElement("text");
								xmlsWriter.WriteAttributeString("font-size", NormalizeDecimal(val3.Size * (float)sScale));
								xmlsWriter.WriteAttributeString("font-family", "Calibri");
								if (animated)
								{
									xmlsWriter.WriteAttributeString("opacity", "0");
									xmlsWriter.WriteAttributeString("_animation", string.Format("{{\"actions\":[[\"opacity\", 0, 1, \"\"], [\"scale\", {2}, {2},\"\"]], \"begin\":{0}, \"duration\":{1}}}", NormalizeDecimal(200 / count * num), "100", NormalizeDecimal(sScale)));
								}
								string text3 = string.Empty;
								if (i == 1)
								{
									string arg = $"#{array[0].R:X2}{array[0].G:X2}{array[0].B:X2}";
									text3 = string.Format("text-shadow: -1px -1px 1px {0},-1px 1px 1px {0},1px -1px 1px {0}, 1px 1px 1px {0};", arg);
								}
								xmlsWriter.WriteAttributeString("transform", $"translate({NormalizeDecimal((double)(num5 - (float)i) * sScale)}, {NormalizeDecimal((double)(num6 - (float)i) * sScale)})");
								xmlsWriter.WriteAttributeString("style", $"fill:#{array[i].R:X2}{array[i].G:X2}{array[i].B:X2}; pointer-events:none;{text3}");
								xmlsWriter.WriteAttributeString("font-weight", "bold");
								if (stiMapSvg.SetMaxWidth)
								{
									string[] array2 = Regex.Split(pathText, "[ ]|\\r\\n");
									int num7 = 0;
									string text4 = "";
									for (int j = 0; j < array2.Length; j++)
									{
										text4 += ((text4 != "") ? (" " + array2[j]) : array2[j]);
										if (val2.MeasureString((j < array2.Length - 1) ? (text4 + " " + array2[j + 1]) : text4, val3).Width > sizeF.Width + 20f || j == array2.Length - 1 || text4.EndsWith("\r"))
										{
											xmlsWriter.WriteStartElement("tspan");
											xmlsWriter.WriteAttributeString("x", "0");
											if (num7 == 0)
											{
												xmlsWriter.WriteAttributeString("y", "0");
											}
											else
											{
												xmlsWriter.WriteAttributeString("y", NormalizeDecimal((double)((float)num7 * val3.Size) * sScale));
											}
											xmlsWriter.WriteString(text4);
											xmlsWriter.WriteEndElement();
											num7++;
											text4 = "";
										}
									}
								}
								else
								{
									string[] array3 = Regex.Split(pathText, "\\r\\n");
									for (int k = 0; k < array3.Length; k++)
									{
										xmlsWriter.WriteStartElement("tspan");
										xmlsWriter.WriteAttributeString("x", "0");
										if (k == 0)
										{
											xmlsWriter.WriteAttributeString("y", "0");
										}
										else
										{
											xmlsWriter.WriteAttributeString("y", NormalizeDecimal((double)((float)k * val3.Size) * sScale));
										}
										xmlsWriter.WriteString(array3[k]);
										xmlsWriter.WriteEndElement();
									}
								}
								xmlsWriter.WriteEndElement();
							}
						}
					}
					if (map.ShowBubble)
					{
						SolidBrush brush = new SolidBrush(Color.FromArgb(150, stiMapStyle.BubbleBackColor));
						Color color2 = Color.FromArgb(170, stiMapStyle.BubbleBorderColor);
						foreach (string item3 in list)
						{
							float bubbleSize = GetBubbleSize(mapData, item3);
							if (bubbleSize > 0f)
							{
								StiMapSvg stiMapSvg2 = stiMapSvgContainer.HashPaths[item3];
								Point point = new Point(stiMapSvg2.RectIso.X + stiMapSvg2.RectIso.Width / 2, stiMapSvg2.RectIso.Y + stiMapSvg2.RectIso.Height / 2);
								xmlsWriter.WriteStartElement("ellipse");
								xmlsWriter.WriteAttributeString("cx", NormalizeDecimal((double)point.X * sScale));
								xmlsWriter.WriteAttributeString("cy", NormalizeDecimal((double)point.Y * sScale));
								xmlsWriter.WriteAttributeString("rx", NormalizeDecimal((double)(bubbleSize / 2f) * sScale));
								xmlsWriter.WriteAttributeString("ry", NormalizeDecimal((double)(bubbleSize / 2f) * sScale));
								xmlsWriter.WriteAttributeString("style", GetFillBrush(brush) + GetBorderStroke(color2) + " stroke-width:2; pointer-events:none;");
								xmlsWriter.WriteEndElement();
							}
						}
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
			((IDisposable)val)?.Dispose();
		}
		mapData?.Clear();
	}

	private static double GetScaleStep(double factor)
	{
		if (factor < 1.5)
		{
			return 1.0;
		}
		if (factor < 2.0)
		{
			return 1.5;
		}
		return 2.0;
	}

	private static string GetPathText(StiMap map, StiMapData info, StiMapSvg path, string key)
	{
		string text = null;
		switch (map.DisplayNameType)
		{
		case StiDisplayNameType.Full:
			text = ((info != null) ? info.Name : path.EnglishName);
			break;
		case StiDisplayNameType.Short:
			text = StiMapHelper.PrepareIsoCode(path.ISOCode);
			break;
		}
		if (map.ShowValue && info != null && info.Value != null)
		{
			string text2 = null;
			if (map.ShortValue)
			{
				double result = 0.0;
				if (double.TryParse(info.Value, out result))
				{
					text2 = StiAbbreviationNumberFormatHelper.Format(result);
				}
			}
			if (text2 == null)
			{
				text2 = info.Value;
			}
			if (text == null)
			{
				text = text2;
			}
			else
			{
				text += Environment.NewLine;
				text += text2;
			}
		}
		return text;
	}

	private static Rectangle GetPathRect(StiMap map, StiMapSvg path)
	{
		if (map.DisplayNameType == StiDisplayNameType.Short)
		{
			return path.RectIso.IsEmpty ? path.Rect : path.RectIso;
		}
		return path.Rect;
	}

	private static StiTextHorAlignment GetPathHorAlignment(StiMap map, StiMapSvg path)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (map.DisplayNameType == StiDisplayNameType.Short)
		{
			if (!path.HorAlignmentIso.HasValue)
			{
				return path.HorAlignment;
			}
			return path.HorAlignmentIso.GetValueOrDefault();
		}
		return path.HorAlignment;
	}

	private static StiVertAlignment GetPathVertAlignment(StiMap map, StiMapSvg path)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (map.DisplayNameType == StiDisplayNameType.Short)
		{
			if (!path.VertAlignmentIso.HasValue)
			{
				return path.VertAlignment;
			}
			return path.VertAlignmentIso.GetValueOrDefault();
		}
		return path.VertAlignment;
	}

	private static string GetToolTipIdent(StiMapData data)
	{
		if (!string.IsNullOrEmpty(data.Name))
		{
			return data.Name;
		}
		return data.Key;
	}

	private static string GetToolTipValueText(StiMap map, string value)
	{
		string text = string.Empty;
		if (map.ShowValue && !string.IsNullOrEmpty(value))
		{
			if (map.ShortValue && double.TryParse(value, out var result))
			{
				text = StiAbbreviationNumberFormatHelper.Format(result);
			}
			if (string.IsNullOrEmpty(text))
			{
				text = value;
			}
		}
		return text;
	}

	private static string GetToolTipTotalText(StiMap map, List<StiMapData> mapData, StiMapData data)
	{
		if (map.MapType == StiMapType.Group || map.MapType == StiMapType.HeatmapWithGroup)
		{
			IEnumerable<StiMapData> enumerable = mapData.Where((StiMapData x) => x.Group == data.Group && x.Value != null);
			double num = 0.0;
			foreach (StiMapData item in enumerable)
			{
				if (double.TryParse(item.Value, out var result))
				{
					num += result;
				}
			}
			string text = StiAbbreviationNumberFormatHelper.Format(num);
			return StiLocalization.Get("PropertyMain", "Total") + " = " + text;
		}
		return null;
	}

	private static string NormalizeDecimal(double value)
	{
		return value.ToString().Replace(",", ".");
	}

	private static string GetBorderStroke(Color color)
	{
		string text = $"stroke:rgb({color.R},{color.G},{color.B});";
		double num = Math.Round((float)(int)color.A / 255f, 3);
		if (num != 1.0)
		{
			text = text + "stroke-opacity:" + num.ToString().Replace(",", ".") + ";";
		}
		return text;
	}

	public static string GetFillBrush(SolidBrush brush)
	{
		Color color = brush.Color;
		return string.Format("fill:rgb({0},{1},{2});fill-opacity:{3};", color.R, color.G, color.B, Math.Round((float)(int)color.A / 255f, 3).ToString().Replace(",", "."));
	}

	private static float GetBubbleSize(List<StiMapData> mapData, string key)
	{
		int num = 14;
		int num2 = 130;
		double num3 = 0.0;
		double num4 = 0.0;
		double num5 = 0.0;
		float num6 = num2 - num;
		StiMapData stiMapData = mapData.FirstOrDefault((StiMapData x) => x.Key == key);
		bool flag = true;
		foreach (StiMapData mapDatum in mapData)
		{
			if (mapDatum.Value == null)
			{
				continue;
			}
			double? value = mapDatum.GetValue();
			if (value.HasValue)
			{
				if (flag)
				{
					flag = false;
					num3 = value.GetValueOrDefault();
					num4 = value.GetValueOrDefault();
				}
				else if (value < num3)
				{
					num3 = value.GetValueOrDefault();
				}
				else if (value > num4)
				{
					num4 = value.GetValueOrDefault();
				}
			}
		}
		num5 = num4 - num3;
		if (stiMapData != null && stiMapData.Value != null)
		{
			double? value2 = stiMapData.GetValue();
			if (value2.HasValue)
			{
				double num7 = (value2.GetValueOrDefault() - num3) / num5;
				return (float)((double)num + (double)num6 * num7);
			}
		}
		return 0f;
	}
}
