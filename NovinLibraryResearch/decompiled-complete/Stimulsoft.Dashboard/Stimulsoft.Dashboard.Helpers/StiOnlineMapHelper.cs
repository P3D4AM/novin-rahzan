using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;
using Stimulsoft.Base.Meters;
using Stimulsoft.Dashboard.Components.OnlineMap;
using Stimulsoft.Data.Comparers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps.Helpers;

namespace Stimulsoft.Dashboard.Helpers;

public class StiOnlineMapHelper
{
	private class LatLon
	{
		public static byte MAX_SIZE = 100;

		public decimal Lat;

		public decimal Lon;

		public byte Weight;

		public LatLon(decimal lat, decimal lon, byte weight)
		{
			Lat = lat;
			Lon = lon;
			Weight = weight;
		}
	}

	private static readonly string ICON = Convert.ToBase64String(Encoding.UTF8.GetBytes("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"M12 2C8.13 2 5 5.13 5 9c0 5.25 7 13 7 13s7-7.75 7-13c0-3.87-3.13-7-7-7zm0 9.5c-1.38 0-2.5-1.12-2.5-2.5s1.12-2.5 2.5-2.5 2.5 1.12 2.5 2.5-1.12 2.5-2.5 2.5z\" fill=\"#af6682\" stroke=\"#663a4c\"/><circle cx=\"12\" cy=\"9\" r=\"3\" fill=\"#f7f4f2\"/></svg>"));

	private static readonly string EMPTY_ICON = Convert.ToBase64String(Encoding.UTF8.GetBytes("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"1\" height=\"1\" ></svg>"));

	private static readonly int MAX_COORDINATES_LOCATIONS = 10000;

	private static readonly int MAX_LOCATIONS = 1000;

	private static readonly int MAX_CHART_SECTORS = 100;

	private static readonly int MIN_RADIUS = 3;

	private static readonly int MAX_RADIUS = 15;

	private static readonly int CHART_RADIUS = 26;

	private static readonly int DELTA_RADIUS = MAX_RADIUS - MIN_RADIUS;

	private static Hashtable htmlNameToColor;

	private static object lockHtmlNameToColor = new object();

	public static string GetBingMapScript(IStiElement element, bool showTitle)
	{
		StiDataTable dataTable = GetDataTable(element);
		Hashtable hashtable = CalculateMapData(dataTable, element as StiOnlineMapElement);
		if (showTitle)
		{
			StiPadding padding = ((IStiPadding)element).Padding;
			hashtable["outerStyle"] = "display: table;\r\n" + $"padding:{padding.Top}px {padding.Right}px {padding.Bottom}px {padding.Left}px;";
			IStiTitle title = ((IStiTitleElement)element).Title;
			if (title.Visible)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (title.Font.Bold)
				{
					stringBuilder.Append("font-weight: bold;");
				}
				if (title.Font.Italic)
				{
					stringBuilder.Append("font-style: italic;");
				}
				if (title.Font.Strikeout || title.Font.Underline)
				{
					string text = ((!title.Font.Underline) ? (title.Font.Strikeout ? "line-through" : "none") : (title.Font.Strikeout ? "underline line-through" : "underline"));
					stringBuilder.Append("text-decoration: " + text + ";");
				}
				hashtable["title"] = $"font-family: {title.Font.Name}; font-size: {title.Font.Size}pt; color: {ColorTranslator.ToHtml((title.ForeColor == Color.Transparent) ? StiDashboardStyleHelper.GetTitleForeColor(element) : title.ForeColor)};" + $"{stringBuilder}background-color:{ColorTranslator.ToHtml((title.BackColor == Color.Transparent) ? StiDashboardStyleHelper.GetBackColor(element) : title.BackColor)};" + "text-align:" + title.HorAlignment.ToString().ToLowerInvariant() + "'>" + title.Text + "</div>";
			}
		}
		return StiBingMapHelper.GetScript(hashtable);
	}

	private static StiDataTable GetDataTable(IStiElement element)
	{
		StiDataTable stiDataTable = (element as IStiManuallyEnteredData)?.GetManuallyEnteredDataTable();
		if (stiDataTable != null)
		{
			return stiDataTable;
		}
		return StiElementDataCache.TryToGetOrCreate(element);
	}

	public static Hashtable CalculateMapData(StiDataTable dataTable, StiOnlineMapElement onlineMapElement)
	{
		Hashtable hashtable = new Hashtable();
		int longitudeMeterIndex = GetLongitudeMeterIndex(dataTable);
		int latitudeMeterIndex = GetLatitudeMeterIndex(dataTable);
		int locationMeterIndex = GetLocationMeterIndex(dataTable);
		int locationColorMeterIndex = GetLocationColorMeterIndex(dataTable);
		int locationArgumentMeterIndex = GetLocationArgumentMeterIndex(dataTable);
		int locationValueMeterIndex = GetLocationValueMeterIndex(dataTable);
		bool flag = StiDashboardStyleHelper.IsDarkStyle(onlineMapElement.Dashboard);
		if (dataTable != null)
		{
			if (latitudeMeterIndex != -1 && longitudeMeterIndex != -1)
			{
				List<Hashtable> list = new List<Hashtable>();
				List<LatLon> list2 = Approximate(dataTable);
				foreach (LatLon item2 in list2)
				{
					string value = item2.Lat.ToString(CultureInfo.InvariantCulture).Replace(",", ".");
					string value2 = item2.Lon.ToString(CultureInfo.InvariantCulture).Replace(",", ".");
					if (onlineMapElement.ValueViewMode == StiOnlineMapValueViewMode.Heatmap)
					{
						list.Add(new Hashtable
						{
							["lat"] = value,
							["lon"] = value2
						});
					}
					else if (onlineMapElement.CustomIcon == null)
					{
						list.Add(new Hashtable
						{
							["lat"] = value,
							["lon"] = value2,
							["fontIcon"] = (int)StiFontIconsHelper.GetContent(onlineMapElement.Icon)[0],
							["fontIconColor"] = $"rgb({onlineMapElement.IconColor.R}, {onlineMapElement.IconColor.G}, {onlineMapElement.IconColor.B})"
						});
					}
					else
					{
						list.Add(new Hashtable
						{
							["lat"] = value,
							["lon"] = value2,
							["anchorX"] = MAX_RADIUS,
							["anchorY"] = MAX_RADIUS * 2
						});
					}
				}
				hashtable["pushpins"] = list;
			}
			else if (locationMeterIndex != -1)
			{
				List<Color> list3 = null;
				Hashtable hashtable2 = null;
				if (onlineMapElement.LocationColorType == StiOnlineMapLocationColorType.ColorEach)
				{
					list3 = GetColors(Math.Min(dataTable.Rows.Count, MAX_LOCATIONS));
				}
				else if (onlineMapElement.LocationColorType == StiOnlineMapLocationColorType.Group && locationColorMeterIndex != -1)
				{
					List<string> list4 = new List<string>();
					for (int i = 0; i < dataTable.Rows.Count; i++)
					{
						string item = ((dataTable.Rows[i][locationColorMeterIndex] != null) ? dataTable.Rows[i][locationColorMeterIndex].ToString() : "");
						if (!list4.Contains(item))
						{
							list4.Add(item);
						}
					}
					List<Color> colors = GetColors(list4.Count);
					hashtable2 = new Hashtable();
					for (int j = 0; j < list4.Count; j++)
					{
						hashtable2[list4[j]] = colors[j];
					}
				}
				SortedSet<decimal> sortedSet = new SortedSet<decimal>();
				if (locationValueMeterIndex >= 0)
				{
					for (int k = 0; k < Math.Min(dataTable.Rows.Count, MAX_LOCATIONS); k++)
					{
						sortedSet.Add(StiValueHelper.TryToDecimal(dataTable.Rows[k][locationValueMeterIndex]));
					}
				}
				List<decimal> list5 = sortedSet.ToList();
				decimal num = ((list5.Count > 0) ? list5.Max() : 0m);
				decimal num2 = ((list5.Count > 0) ? list5.Min() : 0m);
				Hashtable hashtable3 = new Hashtable();
				int num3 = 0;
				while (num3 < dataTable.Rows.Count && num3 < MAX_LOCATIONS)
				{
					object obj = dataTable.Rows[num3][locationMeterIndex];
					if (obj == null || string.IsNullOrEmpty(obj.ToString()))
					{
						num3++;
						continue;
					}
					Color color = Color.LimeGreen;
					string text = null;
					if (onlineMapElement.LocationColorType == StiOnlineMapLocationColorType.Single)
					{
						color = onlineMapElement.LocationColor;
						text = ((onlineMapElement.LocationColor == Color.Transparent) ? "Transparent" : null);
					}
					else if (onlineMapElement.LocationColorType == StiOnlineMapLocationColorType.ColorEach)
					{
						color = list3[num3];
					}
					else if ((onlineMapElement.LocationColorType == StiOnlineMapLocationColorType.Group || onlineMapElement.LocationColorType == StiOnlineMapLocationColorType.Value) && locationColorMeterIndex >= 0)
					{
						string text2 = ((dataTable.Rows[num3][locationColorMeterIndex] != null) ? dataTable.Rows[num3][locationColorMeterIndex].ToString() : "");
						if (onlineMapElement.LocationColorType == StiOnlineMapLocationColorType.Group && hashtable2.ContainsKey(text2))
						{
							color = (Color)hashtable2[text2];
						}
						if (onlineMapElement.LocationColorType == StiOnlineMapLocationColorType.Value && !string.IsNullOrEmpty(text2))
						{
							text = text2;
						}
					}
					if (onlineMapElement.ValueViewMode == StiOnlineMapValueViewMode.Heatmap)
					{
						text = "Transparent";
					}
					hashtable3[obj] = new Hashtable
					{
						["fillStyle"] = text ?? $"rgba({color.R}, {color.G}, {color.B}, 0.5)",
						["strokeStyle"] = ((text == "Transparent") ? text : "#d3d3d3")
					};
					if (locationValueMeterIndex >= 0 || onlineMapElement.ValueViewMode == StiOnlineMapValueViewMode.Icon)
					{
						if (onlineMapElement.ValueViewMode == StiOnlineMapValueViewMode.Bubble)
						{
							double num4 = (double)list5.IndexOf(StiValueHelper.TryToDecimal(dataTable.Rows[num3][locationValueMeterIndex])) / (double)list5.Count;
							int num5 = MAX_RADIUS * 2 + 2;
							double num6 = (double)MIN_RADIUS + Math.Ceiling(num4 * (double)DELTA_RADIUS);
							int num7 = MAX_RADIUS + 1;
							Color color2 = StiColorUtils.Dark((text != null) ? ParseColor(text) : color, 70);
							string s = $"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{num5}\" height=\"{num5}\">" + $"<circle cx=\"{num7}\" cy=\"{num7}\" r=\"{num6}\" stroke=\"rgba({color2.R}, {color2.G}, {color2.B}, {color2.A})\" " + "stroke-width=\"1\" fill=\"" + (text ?? $"rgba({color.R}, {color.G}, {color.B}, 0.5)") + "\"/></svg>";
							(hashtable3[obj] as Hashtable)["icon"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
							(hashtable3[obj] as Hashtable)["anchorX"] = num7;
							(hashtable3[obj] as Hashtable)["anchorY"] = num7;
						}
						else if (onlineMapElement.ValueViewMode == StiOnlineMapValueViewMode.Value)
						{
							(hashtable3[obj] as Hashtable)["icon"] = EMPTY_ICON;
							(hashtable3[obj] as Hashtable)["title"] = ((dataTable.Rows[num3][locationValueMeterIndex] != null) ? dataTable.Rows[num3][locationValueMeterIndex].ToString() : "");
							(hashtable3[obj] as Hashtable)["anchorX"] = 1;
							(hashtable3[obj] as Hashtable)["anchorY"] = 7;
						}
						else if (onlineMapElement.ValueViewMode == StiOnlineMapValueViewMode.Icon)
						{
							if (onlineMapElement.CustomIcon == null)
							{
								(hashtable3[obj] as Hashtable)["fontIcon"] = (int)StiFontIconsHelper.GetContent(onlineMapElement.Icon)[0];
								(hashtable3[obj] as Hashtable)["fontIconColor"] = $"rgb({onlineMapElement.IconColor.R}, {onlineMapElement.IconColor.G}, {onlineMapElement.IconColor.B})";
							}
							if (locationValueMeterIndex > 0)
							{
								(hashtable3[obj] as Hashtable)["title"] = ((dataTable.Rows[num3][locationValueMeterIndex] != null) ? dataTable.Rows[num3][locationValueMeterIndex].ToString() : "");
							}
						}
						else if (onlineMapElement.ValueViewMode == StiOnlineMapValueViewMode.Chart)
						{
							(hashtable3[obj] as Hashtable)["chart"] = GetChartData(dataTable, locationMeterIndex, locationValueMeterIndex, locationArgumentMeterIndex, dataTable.Rows[num3][locationMeterIndex]);
							(hashtable3[obj] as Hashtable)["icon"] = EMPTY_ICON;
						}
						else if (onlineMapElement.ValueViewMode == StiOnlineMapValueViewMode.Heatmap)
						{
							decimal num8 = StiValueHelper.TryToDecimal(dataTable.Rows[num3][locationValueMeterIndex]);
							(hashtable3[obj] as Hashtable)["weight"] = ((num == num2) ? 1m : Math.Max((num8 - num2) / (num - num2), 0.2m));
						}
					}
					num3++;
				}
				hashtable["locations"] = hashtable3;
				hashtable["locationType"] = onlineMapElement.LocationType;
			}
		}
		if ((onlineMapElement.ValueViewMode == StiOnlineMapValueViewMode.Icon || (latitudeMeterIndex != -1 && longitudeMeterIndex != -1)) && onlineMapElement.CustomIcon != null)
		{
			GetCustomIcon(onlineMapElement.CustomIcon, hashtable);
		}
		if (onlineMapElement.ValueViewMode == StiOnlineMapValueViewMode.Heatmap)
		{
			GetHeatmapGradient(onlineMapElement, hashtable);
		}
		hashtable["theme"] = (flag ? "canvasDark" : "canvasLight");
		hashtable["culture"] = onlineMapElement.Culture.ToString().Replace("_", "-");
		Color color3 = StiDashboardStyleHelper.GetBackColor(onlineMapElement);
		if (color3 == Color.Transparent && onlineMapElement.Report != null && onlineMapElement.Report.Info.FillComponent && onlineMapElement.IsDesigning)
		{
			color3 = Color.FromArgb(153, Color.White);
		}
		hashtable["backgroundColor"] = ColorTranslator.ToHtml(color3);
		return hashtable;
	}

	private static List<LatLon> Approximate(StiDataTable dataTable)
	{
		List<LatLon> result = new List<LatLon>();
		double num = (double)MAX_COORDINATES_LOCATIONS / (double)dataTable.Rows.Count();
		int latIndex = GetLatitudeMeterIndex(dataTable);
		int lonIndex = GetLongitudeMeterIndex(dataTable);
		if (num < 1.0)
		{
			int num2 = (int)((double)dataTable.Rows.Count() * 0.0008);
			decimal num3 = dataTable.Rows.Where((object[] r) => r[latIndex] != null).Max((object[] r) => StiValueHelper.TryToDecimal(r[latIndex]));
			decimal num4 = dataTable.Rows.Where((object[] r) => r[latIndex] != null).Min((object[] r) => StiValueHelper.TryToDecimal(r[latIndex]));
			decimal num5 = (num3 - num4) / (decimal)num2;
			decimal num6 = dataTable.Rows.Where((object[] r) => r[lonIndex] != null).Max((object[] r) => StiValueHelper.TryToDecimal(r[lonIndex]));
			decimal num7 = dataTable.Rows.Where((object[] r) => r[lonIndex] != null).Min((object[] r) => StiValueHelper.TryToDecimal(r[lonIndex]));
			decimal num8 = (num6 - num7) / (decimal)num2;
			Hashtable hashtable = new Hashtable();
			foreach (object[] row in dataTable.Rows)
			{
				if (row[latIndex] != null && row[lonIndex] != null)
				{
					decimal num9 = StiValueHelper.TryToDecimal(row[latIndex]);
					decimal num10 = StiValueHelper.TryToDecimal(row[lonIndex]);
					int num11 = (int)((num9 - num4) / num5);
					int num12 = (int)((num10 - num7) / num8);
					if (!hashtable.ContainsKey(num11))
					{
						hashtable[num11] = new Hashtable();
					}
					if ((hashtable[num11] as Hashtable)[num12] == null)
					{
						(hashtable[num11] as Hashtable)[num12] = new List<LatLon>();
					}
					List<LatLon> list = (List<LatLon>)(hashtable[num11] as Hashtable)[num12];
					if (list.Count() == 0 || list[list.Count() - 1].Weight == LatLon.MAX_SIZE)
					{
						list.Add(new LatLon(num9, num10, 1));
						continue;
					}
					LatLon latLon = list[list.Count() - 1];
					latLon.Lat = (latLon.Lat - num9) / 2m + num9;
					latLon.Lon = (latLon.Lon - num10) / 2m + num10;
					latLon.Weight++;
				}
			}
			foreach (object key in hashtable.Keys)
			{
				foreach (object value in (hashtable[key] as Hashtable).Values)
				{
					result.AddRange(value as List<LatLon>);
				}
			}
		}
		else
		{
			dataTable.Rows.ToList().ForEach(delegate(object[] r)
			{
				result.Add(new LatLon(StiValueHelper.TryToDecimal(r[latIndex]), StiValueHelper.TryToDecimal(r[lonIndex]), LatLon.MAX_SIZE));
			});
		}
		return result;
	}

	private static void GetHeatmapGradient(StiOnlineMapElement onlineMapElement, Hashtable mapData)
	{
		string value = string.Empty;
		switch (onlineMapElement.HeatmapColorGradientType)
		{
		case StiOnlineMapHeatmapColorGradientType.BlackAquaWhite:
			value = "{'0': 'Black','0.5': 'Aqua','1': 'White'}";
			break;
		case StiOnlineMapHeatmapColorGradientType.BlueRed:
			value = "{'0.0': 'blue','1': 'red'}";
			break;
		case StiOnlineMapHeatmapColorGradientType.DeepSea:
			value = "{'0.0': 'rgb(0, 0, 0)','0.6': 'rgb(24, 53, 103)','0.75': 'rgb(46, 100, 158)','0.9': 'rgb(23, 173, 203)','1.0': 'rgb(0, 250, 250)'}";
			break;
		case StiOnlineMapHeatmapColorGradientType.ColorSpectrum:
			value = "{'0': 'Navy','0.25': 'Blue','0.5': 'Green','0.75': 'Yellow','1': 'Red'}";
			break;
		case StiOnlineMapHeatmapColorGradientType.Incandescent:
			value = "{'0': 'Black','0.33': 'DarkRed','0.66': 'Yellow','1': 'White'}";
			break;
		case StiOnlineMapHeatmapColorGradientType.HeatedMetal:
			value = "{'0': 'Black','0.4': 'Purple','0.6': 'Red','0.8': 'Yellow','1': 'White'}";
			break;
		case StiOnlineMapHeatmapColorGradientType.Sunrise:
			value = "{'0': 'Red','0.66': 'Yellow','1': 'White'}";
			break;
		case StiOnlineMapHeatmapColorGradientType.SteppedColors:
			value = "{'0': 'Navy', '0.25': 'Navy','0.26': 'Green','0.5': 'Green','0.51': 'Yellow','0.75': 'Yellow','0.76': 'Red','1': 'Red'}";
			break;
		case StiOnlineMapHeatmapColorGradientType.VisibleSpectrum:
			value = "{'0.00': 'rgb(255,0,255)','0.25': 'rgb(0,0,255)','0.50': 'rgb(0,255,0)','0.75': 'rgb(255,255,0)','1.00': 'rgb(255,0,0)'}";
			break;
		case StiOnlineMapHeatmapColorGradientType.Custom:
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			for (int i = 0; i < onlineMapElement.HeatmapGradientRanges.Count; i++)
			{
				IStiHeatmapGradientRange stiHeatmapGradientRange = onlineMapElement.HeatmapGradientRanges[i];
				float num = StiValueHelper.ParseFloat(StiReportParser.Parse(stiHeatmapGradientRange.StartExpression, onlineMapElement)) / 100f;
				stringBuilder.Append($"'{num}': '{ColorTranslator.ToHtml(stiHeatmapGradientRange.Color)}'");
				if (i < onlineMapElement.HeatmapGradientRanges.Count - 1)
				{
					stringBuilder.Append(", ");
				}
			}
			stringBuilder.Append("}");
			value = stringBuilder.ToString();
			break;
		}
		}
		mapData["heatmapRadius"] = onlineMapElement.HeatmapRadius;
		mapData["heatmapGradient"] = value;
	}

	private static Hashtable GetChartData(StiDataTable dataTable, int locIndex, int locValueIndex, int locAgrumentIndex, object locValue)
	{
		Hashtable hashtable = new Hashtable();
		try
		{
			if (locIndex >= 0 && locValueIndex >= 0 && locAgrumentIndex >= 0)
			{
				int cHART_RADIUS = CHART_RADIUS;
				IStiMeter stiMeter = dataTable.Meters[locAgrumentIndex];
				IStiMeter stiMeter2 = dataTable.Meters[locValueIndex];
				IStiMeter stiMeter3 = dataTable.Meters[locIndex];
				IEnumerable<object[]> source = dataTable.Rows.Where((object[] row) => row[locIndex] == locValue);
				List<object> list = (from row in source.GroupBy((object[] row) => new object[1] { row[locAgrumentIndex] }, new StiArrayEqualityComparer())
					select row.Key.ToArray()[0]).ToList();
				List<decimal> list2 = new List<decimal>();
				foreach (object argument in list)
				{
					list2.Add((from row in source
						where row[locAgrumentIndex] == argument
						select row[locValueIndex]).Sum((object i) => StiValueHelper.TryToDecimal(i)));
				}
				decimal num = list2.Sum();
				List<Color> colors = GetColors(Math.Min(list.Count, MAX_CHART_SECTORS));
				double num2 = 0.0;
				List<Hashtable> list3 = new List<Hashtable>();
				for (int j = 0; j < Math.Min(list.Count, MAX_CHART_SECTORS); j++)
				{
					object obj = list[j];
					decimal num3 = list2[j];
					double num4 = (double)(num3 / num * 6.28318530716m);
					double number = (double)(cHART_RADIUS + 1) + (double)cHART_RADIUS * Math.Cos(num2);
					double number2 = (double)(cHART_RADIUS + 1) + (double)cHART_RADIUS * Math.Sin(num2);
					double number3 = (double)(cHART_RADIUS + 1) + (double)cHART_RADIUS * Math.Cos(num2 + (num4 - ((list.Count == 1) ? 0.001 : 0.0)));
					double number4 = (double)(cHART_RADIUS + 1) + (double)cHART_RADIUS * Math.Sin(num2 + (num4 - ((list.Count == 1) ? 0.001 : 0.0)));
					int num5 = ((num3 / num > 0.5m) ? 1 : 0);
					Color color = StiColorUtils.Dark(colors[j], 100);
					list3.Add(new Hashtable
					{
						["stroke"] = $"rgb({color.R}, {color.G}, {color.B})",
						["fill"] = $"rgb({colors[j].R}, {colors[j].G}, {colors[j].B})",
						["_color"] = ColorTranslator.ToHtml(colors[j]),
						["d"] = string.Format("M {0} {1} {2} {3} {4} A {5} {6} 0 {7} 1 {8} {9}z", cHART_RADIUS + 1, cHART_RADIUS + 1, (list.Count == 1) ? "M" : "L", ToUnits(number), ToUnits(number2), cHART_RADIUS, cHART_RADIUS, num5, ToUnits(number3), ToUnits(number4)),
						["_text0"] = stiMeter3.Label,
						["_text1"] = locValue?.ToString(),
						["_text2"] = stiMeter.Label,
						["_text3"] = obj?.ToString(),
						["_text4"] = stiMeter2.Label,
						["_text5"] = num3.ToString()
					});
					num2 += num4;
				}
				hashtable["paths"] = list3;
				hashtable["anchor"] = cHART_RADIUS;
			}
		}
		catch (Exception)
		{
		}
		return hashtable;
	}

	public static string ToUnits(double number)
	{
		return Math.Round(number, 2).ToString().Replace(",", ".");
	}

	private static void GetCustomIcon(byte[] bytes, Hashtable mapData)
	{
		try
		{
			int num = 0;
			int num2 = 0;
			if (!StiSvgHelper.IsSvg(bytes))
			{
				MemoryStream memoryStream = new MemoryStream(bytes);
				Image val = Image.FromStream((Stream)memoryStream);
				double num3 = Math.Min(1.0, (double)MAX_RADIUS * 2.0 / (double)Math.Max(val.Width, val.Height));
				num = (int)Math.Round((double)val.Width * num3);
				num2 = (int)Math.Round((double)val.Height * num3);
			}
			else
			{
				object document = StiSvgHelper.OpenSvg(bytes);
				SizeF svgSize = StiSvgHelper.GetSvgSize(document);
				double num4 = Math.Min(1.0, (double)MAX_RADIUS * 2.0 / (double)Math.Max(svgSize.Width, svgSize.Height));
				num = (int)Math.Round((double)svgSize.Width * num4);
				num2 = (int)Math.Round((double)svgSize.Height * num4);
				Image val = StiSvgHelper.ConvertSvgToImage(bytes, num, num2, stretch: true, aspectRatio: true);
				bytes = StiImageConverter.ImageToBytes(val);
			}
			mapData["customIconAnchorX"] = num / 2;
			mapData["customIconAnchorY"] = num2;
			string s = $"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{num}\" height=\"{num2}\"><image width=\"{num}\" height=\"{num2}\" href=\"{ImageBytesToBase64String(bytes)}\"></image></svg>";
			mapData["customIcon"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
		}
		catch (Exception)
		{
		}
	}

	private static string ImageBytesToBase64String(byte[] image)
	{
		string text = "data:image/png;base64,";
		if (StiImageHelper.IsMetafile(image))
		{
			text = "data:image/x-wmf;base64,";
		}
		else if (StiImageHelper.IsBmp(image))
		{
			text = "data:image/bmp;base64,";
		}
		else if (StiImageHelper.IsJpeg(image))
		{
			text = "data:image/jpeg;base64,";
		}
		else if (StiImageHelper.IsGif(image))
		{
			text = "data:image/gif;base64,";
		}
		else if (StiSvgHelper.IsSvg(image))
		{
			text = "data:image/svg+xml;base64,";
		}
		return text + Convert.ToBase64String(image);
	}

	private static List<Color> GetColors(int count)
	{
		List<Color> list = new List<Color>();
		byte b = 10;
		bool flag = true;
		Color[] colors = StiMapHelper.GetColors();
		list.AddRange(colors.ToList());
		while (list.Count < count)
		{
			if (flag)
			{
				Color[] array = colors;
				foreach (Color baseColor in array)
				{
					list.Add(StiColorUtils.Dark(baseColor, b));
				}
			}
			else
			{
				Color[] array2 = colors;
				foreach (Color baseColor2 in array2)
				{
					list.Add(StiColorUtils.Light(baseColor2, b));
				}
				b += 10;
			}
			flag = !flag;
		}
		return list;
	}

	protected static int GetLongitudeMeterIndex(StiDataTable dataTable)
	{
		if (dataTable == null)
		{
			return -1;
		}
		IStiMeter stiMeter = dataTable.Meters.FirstOrDefault((IStiMeter f) => f is StiLongitudeMapMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return dataTable.Meters.IndexOf(stiMeter);
	}

	protected static int GetGisMeterIndex(StiDataTable dataTable)
	{
		if (dataTable == null)
		{
			return -1;
		}
		IStiMeter stiMeter = dataTable.Meters.FirstOrDefault((IStiMeter f) => f is StiGisMapMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return dataTable.Meters.IndexOf(stiMeter);
	}

	protected static int GetLatitudeMeterIndex(StiDataTable dataTable)
	{
		if (dataTable == null)
		{
			return -1;
		}
		IStiMeter stiMeter = dataTable.Meters.FirstOrDefault((IStiMeter f) => f is StiLatitudeMapMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return dataTable.Meters.IndexOf(stiMeter);
	}

	protected static int GetLocationMeterIndex(StiDataTable dataTable)
	{
		if (dataTable == null)
		{
			return -1;
		}
		IStiMeter stiMeter = dataTable.Meters.FirstOrDefault((IStiMeter f) => f is StiLocationMapMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return dataTable.Meters.IndexOf(stiMeter);
	}

	protected static int GetLocationColorMeterIndex(StiDataTable dataTable)
	{
		if (dataTable == null)
		{
			return -1;
		}
		IStiMeter stiMeter = dataTable.Meters.FirstOrDefault((IStiMeter f) => f is StiLocationColorMapMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return dataTable.Meters.IndexOf(stiMeter);
	}

	protected static int GetLocationArgumentMeterIndex(StiDataTable dataTable)
	{
		if (dataTable == null)
		{
			return -1;
		}
		IStiMeter stiMeter = dataTable.Meters.FirstOrDefault((IStiMeter f) => f is StiLocationArgumentMapMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return dataTable.Meters.IndexOf(stiMeter);
	}

	protected static int GetLocationValueMeterIndex(StiDataTable dataTable)
	{
		if (dataTable == null)
		{
			return -1;
		}
		IStiMeter stiMeter = dataTable.Meters.FirstOrDefault((IStiMeter f) => f is StiLocationValueMapMeter);
		if (stiMeter == null)
		{
			return -1;
		}
		return dataTable.Meters.IndexOf(stiMeter);
	}

	private static Color ParseColor(string colorAttribute)
	{
		Color result = Color.Black;
		try
		{
			if (colorAttribute.Length > 1)
			{
				if (colorAttribute[0] == '#')
				{
					string text = colorAttribute.Substring(1).ToLowerInvariant();
					StringBuilder stringBuilder = new StringBuilder();
					string text2 = text;
					foreach (char c in text2)
					{
						if (c == '0' || c == '1' || c == '2' || c == '3' || c == '4' || c == '5' || c == '6' || c == '7' || c == '8' || c == '9' || c == 'a' || c == 'b' || c == 'c' || c == 'd' || c == 'e' || c == 'f')
						{
							stringBuilder.Append(c);
						}
					}
					text = ((stringBuilder.Length != 3) ? stringBuilder.ToString() : string.Format("{0}{0}{1}{1}{2}{2}", stringBuilder[0], stringBuilder[1], stringBuilder[2]));
					if (text.Length == 6)
					{
						int num = Convert.ToInt32(text, 16);
						result = Color.FromArgb(255, (num >> 16) & 0xFF, (num >> 8) & 0xFF, num & 0xFF);
					}
				}
				else if (colorAttribute.StartsWith("rgb", StringComparison.InvariantCulture))
				{
					string[] array = colorAttribute.Trim().Substring(4, colorAttribute.Length - 5).Split(new char[1] { ',' });
					if (array.Length == 3)
					{
						int[] array2 = new int[3];
						if (array[0].EndsWith("%", StringComparison.InvariantCulture))
						{
							array2[0] = (int)Math.Round((double)StiValueHelper.TryToInt(array[0].Substring(0, array[0].Length - 1)) * 2.55);
						}
						else
						{
							array2[0] = StiValueHelper.TryToInt(array[0]);
						}
						if (array[1].EndsWith("%", StringComparison.InvariantCulture))
						{
							array2[1] = (int)Math.Round((double)StiValueHelper.TryToInt(array[1].Substring(0, array[1].Length - 1)) * 2.55);
						}
						else
						{
							array2[1] = StiValueHelper.TryToInt(array[1]);
						}
						if (array[2].EndsWith("%", StringComparison.InvariantCulture))
						{
							array2[2] = (int)Math.Round((double)StiValueHelper.TryToInt(array[2].Substring(0, array[2].Length - 1)) * 2.55);
						}
						else
						{
							array2[2] = StiValueHelper.TryToInt(array[2]);
						}
						result = Color.FromArgb(255, array2[0], array2[1], array2[2]);
					}
					else if (array.Length == 4)
					{
						int[] array3 = new int[3];
						double num2 = 1.0;
						array[0] = array[0].Replace("(", "");
						if (array[0].EndsWith("%", StringComparison.InvariantCulture))
						{
							array3[0] = (int)Math.Round((double)StiValueHelper.TryToInt(array[0].Substring(0, array[0].Length - 1)) * 2.55);
						}
						else
						{
							array3[0] = StiValueHelper.TryToInt(array[0]);
						}
						if (array[1].EndsWith("%", StringComparison.InvariantCulture))
						{
							array3[1] = (int)Math.Round((double)StiValueHelper.TryToInt(array[1].Substring(0, array[1].Length - 1)) * 2.55);
						}
						else
						{
							array3[1] = StiValueHelper.TryToInt(array[1]);
						}
						if (array[2].EndsWith("%", StringComparison.InvariantCulture))
						{
							array3[2] = (int)Math.Round((double)StiValueHelper.TryToInt(array[2].Substring(0, array[2].Length - 1)) * 2.55);
						}
						else
						{
							array3[2] = StiValueHelper.TryToInt(array[2]);
						}
						num2 = ((!array[3].EndsWith("%", StringComparison.InvariantCulture)) ? StiValueHelper.TryToDouble(array[3]) : (StiValueHelper.TryToDouble(array[3].Substring(0, array[3].Length - 1)) * 2.55));
						result = Color.FromArgb((int)Math.Round(num2 * 255.0), array3[0], array3[1], array3[2]);
					}
				}
				else
				{
					lock (lockHtmlNameToColor)
					{
						if (htmlNameToColor == null)
						{
							string[,] array4 = new string[147, 2]
							{
								{ "AliceBlue", "#F0F8FF" },
								{ "AntiqueWhite", "#FAEBD7" },
								{ "Aqua", "#00FFFF" },
								{ "Aquamarine", "#7FFFD4" },
								{ "Azure", "#F0FFFF" },
								{ "Beige", "#F5F5DC" },
								{ "Bisque", "#FFE4C4" },
								{ "Black", "#000000" },
								{ "BlanchedAlmond", "#FFEBCD" },
								{ "Blue", "#0000FF" },
								{ "BlueViolet", "#8A2BE2" },
								{ "Brown", "#A52A2A" },
								{ "BurlyWood", "#DEB887" },
								{ "CadetBlue", "#5F9EA0" },
								{ "Chartreuse", "#7FFF00" },
								{ "Chocolate", "#D2691E" },
								{ "Coral", "#FF7F50" },
								{ "CornflowerBlue", "#6495ED" },
								{ "Cornsilk", "#FFF8DC" },
								{ "Crimson", "#DC143C" },
								{ "Cyan", "#00FFFF" },
								{ "DarkBlue", "#00008B" },
								{ "DarkCyan", "#008B8B" },
								{ "DarkGoldenRod", "#B8860B" },
								{ "DarkGray", "#A9A9A9" },
								{ "DarkGrey", "#A9A9A9" },
								{ "DarkGreen", "#006400" },
								{ "DarkKhaki", "#BDB76B" },
								{ "DarkMagenta", "#8B008B" },
								{ "DarkOliveGreen", "#556B2F" },
								{ "Darkorange", "#FF8C00" },
								{ "DarkOrchid", "#9932CC" },
								{ "DarkRed", "#8B0000" },
								{ "DarkSalmon", "#E9967A" },
								{ "DarkSeaGreen", "#8FBC8F" },
								{ "DarkSlateBlue", "#483D8B" },
								{ "DarkSlateGray", "#2F4F4F" },
								{ "DarkSlateGrey", "#2F4F4F" },
								{ "DarkTurquoise", "#00CED1" },
								{ "DarkViolet", "#9400D3" },
								{ "DeepPink", "#FF1493" },
								{ "DeepSkyBlue", "#00BFFF" },
								{ "DimGray", "#696969" },
								{ "DimGrey", "#696969" },
								{ "DodgerBlue", "#1E90FF" },
								{ "FireBrick", "#B22222" },
								{ "FloralWhite", "#FFFAF0" },
								{ "ForestGreen", "#228B22" },
								{ "Fuchsia", "#FF00FF" },
								{ "Gainsboro", "#DCDCDC" },
								{ "GhostWhite", "#F8F8FF" },
								{ "Gold", "#FFD700" },
								{ "GoldenRod", "#DAA520" },
								{ "Gray", "#808080" },
								{ "Grey", "#808080" },
								{ "Green", "#008000" },
								{ "GreenYellow", "#ADFF2F" },
								{ "HoneyDew", "#F0FFF0" },
								{ "HotPink", "#FF69B4" },
								{ "IndianRed", "#CD5C5C" },
								{ "Indigo", "#4B0082" },
								{ "Ivory", "#FFFFF0" },
								{ "Khaki", "#F0E68C" },
								{ "Lavender", "#E6E6FA" },
								{ "LavenderBlush", "#FFF0F5" },
								{ "LawnGreen", "#7CFC00" },
								{ "LemonChiffon", "#FFFACD" },
								{ "LightBlue", "#ADD8E6" },
								{ "LightCoral", "#F08080" },
								{ "LightCyan", "#E0FFFF" },
								{ "LightGoldenRodYellow", "#FAFAD2" },
								{ "LightGray", "#D3D3D3" },
								{ "LightGrey", "#D3D3D3" },
								{ "LightGreen", "#90EE90" },
								{ "LightPink", "#FFB6C1" },
								{ "LightSalmon", "#FFA07A" },
								{ "LightSeaGreen", "#20B2AA" },
								{ "LightSkyBlue", "#87CEFA" },
								{ "LightSlateGray", "#778899" },
								{ "LightSlateGrey", "#778899" },
								{ "LightSteelBlue", "#B0C4DE" },
								{ "LightYellow", "#FFFFE0" },
								{ "Lime", "#00FF00" },
								{ "LimeGreen", "#32CD32" },
								{ "Linen", "#FAF0E6" },
								{ "Magenta", "#FF00FF" },
								{ "Maroon", "#800000" },
								{ "MediumAquaMarine", "#66CDAA" },
								{ "MediumBlue", "#0000CD" },
								{ "MediumOrchid", "#BA55D3" },
								{ "MediumPurple", "#9370D8" },
								{ "MediumSeaGreen", "#3CB371" },
								{ "MediumSlateBlue", "#7B68EE" },
								{ "MediumSpringGreen", "#00FA9A" },
								{ "MediumTurquoise", "#48D1CC" },
								{ "MediumVioletRed", "#C71585" },
								{ "MidnightBlue", "#191970" },
								{ "MintCream", "#F5FFFA" },
								{ "MistyRose", "#FFE4E1" },
								{ "Moccasin", "#FFE4B5" },
								{ "NavajoWhite", "#FFDEAD" },
								{ "Navy", "#000080" },
								{ "OldLace", "#FDF5E6" },
								{ "Olive", "#808000" },
								{ "OliveDrab", "#6B8E23" },
								{ "Orange", "#FFA500" },
								{ "OrangeRed", "#FF4500" },
								{ "Orchid", "#DA70D6" },
								{ "PaleGoldenRod", "#EEE8AA" },
								{ "PaleGreen", "#98FB98" },
								{ "PaleTurquoise", "#AFEEEE" },
								{ "PaleVioletRed", "#D87093" },
								{ "PapayaWhip", "#FFEFD5" },
								{ "PeachPuff", "#FFDAB9" },
								{ "Peru", "#CD853F" },
								{ "Pink", "#FFC0CB" },
								{ "Plum", "#DDA0DD" },
								{ "PowderBlue", "#B0E0E6" },
								{ "Purple", "#800080" },
								{ "Red", "#FF0000" },
								{ "RosyBrown", "#BC8F8F" },
								{ "RoyalBlue", "#4169E1" },
								{ "SaddleBrown", "#8B4513" },
								{ "Salmon", "#FA8072" },
								{ "SandyBrown", "#F4A460" },
								{ "SeaGreen", "#2E8B57" },
								{ "SeaShell", "#FFF5EE" },
								{ "Sienna", "#A0522D" },
								{ "Silver", "#C0C0C0" },
								{ "SkyBlue", "#87CEEB" },
								{ "SlateBlue", "#6A5ACD" },
								{ "SlateGray", "#708090" },
								{ "SlateGrey", "#708090" },
								{ "Snow", "#FFFAFA" },
								{ "SpringGreen", "#00FF7F" },
								{ "SteelBlue", "#4682B4" },
								{ "Tan", "#D2B48C" },
								{ "Teal", "#008080" },
								{ "Thistle", "#D8BFD8" },
								{ "Tomato", "#FF6347" },
								{ "Turquoise", "#40E0D0" },
								{ "Violet", "#EE82EE" },
								{ "Wheat", "#F5DEB3" },
								{ "White", "#FFFFFF" },
								{ "WhiteSmoke", "#F5F5F5" },
								{ "Yellow", "#FFFF00" },
								{ "YellowGreen", "#9ACD32" }
							};
							htmlNameToColor = new Hashtable();
							for (int j = 0; j < array4.GetLength(0); j++)
							{
								string key = array4[j, 0].ToLowerInvariant();
								int num3 = Convert.ToInt32(array4[j, 1].Substring(1), 16);
								Color color = Color.FromArgb(255, (num3 >> 16) & 0xFF, (num3 >> 8) & 0xFF, num3 & 0xFF);
								htmlNameToColor[key] = color;
							}
						}
					}
					string key2 = colorAttribute.ToLowerInvariant();
					if (htmlNameToColor.ContainsKey(key2))
					{
						result = (Color)htmlNameToColor[key2];
					}
				}
			}
		}
		catch
		{
		}
		return result;
	}
}
