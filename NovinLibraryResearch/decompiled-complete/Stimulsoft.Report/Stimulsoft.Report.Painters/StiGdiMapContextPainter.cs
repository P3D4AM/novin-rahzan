using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Maps.Geoms;
using Stimulsoft.Base.Meters;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Extensions;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Maps.Helpers;
using Stimulsoft.Report.Painters.Context.Animation;

namespace Stimulsoft.Report.Painters;

public class StiGdiMapContextPainter : IStiAnimationContextPainter<StiGeom>
{
	private class StiMapGroup
	{
		public double? MinValue;

		public double? MaxValue;

		public Brush Fill;

		public StiSolidBrush Fill1;
	}

	private class StiStyleColorsContainer
	{
		private List<Color> stackColors = new List<Color>();

		private StiGdiMapContextPainter painter;

		public Brush GetColor(int index, int count)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			if (stackColors.Count == 0)
			{
				return (Brush)(object)painter.DefaultBrush;
			}
			Color color = GetColors(count)[index];
			return (Brush)new SolidBrush(color);
		}

		public StiSolidBrush GetColor1(int index, int count)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			if (stackColors.Count == 0)
			{
				return painter.DefaultBrush1;
			}
			Color color = GetColors(count)[index];
			return new StiSolidBrush(color);
		}

		private Color[] GetColors(int seriesCount)
		{
			Color[] array = new Color[seriesCount];
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < seriesCount; i++)
			{
				if (stackColors.Count == 0 || stackColors.Count < num)
				{
					array[i] = Color.FromArgb(255, 112, 173, 71);
				}
				else if (num2 != 0)
				{
					Color color = stackColors[num];
					int alpha = Math.Min(color.A + num2, 255);
					int red = Math.Min(color.R + num2, 255);
					int green = Math.Min(color.G + num2, 255);
					int blue = Math.Min(color.B + num2, 255);
					array[i] = Color.FromArgb(alpha, red, green, blue);
				}
				else
				{
					array[i] = stackColors[num];
				}
				num++;
				if (num == stackColors.Count)
				{
					num = 0;
					num2 = 50;
				}
			}
			return array;
		}

		public void Init(StiMap map, StiGdiMapContextPainter painter)
		{
			this.painter = painter;
			stackColors.Clear();
			Color[] currentStyleColors = map.GetCurrentStyleColors();
			stackColors.AddRange(currentStyleColors);
		}
	}

	private class HeatmapInfo
	{
		private StiGdiMapContextPainter painter;

		private double min;

		private double max;

		private Color color;

		private Color zeroColor;

		private StiHeatmapFillMode mode;

		public Brush GetBrush(StiMapData data)
		{
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected O, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Expected O, but got Unknown
			if (data == null || data.Value == null || !double.TryParse(data.Value, out var result))
			{
				return (Brush)(object)painter.DefaultBrush;
			}
			if (result == 0.0)
			{
				if (zeroColor == Color.Transparent)
				{
					return (Brush)(object)painter.DefaultBrush;
				}
				return (Brush)new SolidBrush(zeroColor);
			}
			if (min != max)
			{
				float num = (float)(0.85 * (max - result) / (max - min));
				if (mode == StiHeatmapFillMode.Lightness)
				{
					return (Brush)new SolidBrush(StiColorUtils.ChangeLightness(color, num));
				}
				return (Brush)new SolidBrush(StiColorUtils.ChangeDarkness(color, num));
			}
			return (Brush)new SolidBrush(color);
		}

		public StiBrush GetBrush1(StiMapData data)
		{
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Expected O, but got Unknown
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Expected O, but got Unknown
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Expected O, but got Unknown
			if (data == null || data.Value == null || !double.TryParse(data.Value, out var result))
			{
				return (StiBrush)(object)painter.DefaultBrush1;
			}
			if (result == 0.0)
			{
				if (zeroColor == Color.Transparent)
				{
					return (StiBrush)(object)painter.DefaultBrush1;
				}
				return (StiBrush)new StiSolidBrush(zeroColor);
			}
			if (min != max)
			{
				float num = (float)(0.85 * (max - result) / (max - min));
				if (mode == StiHeatmapFillMode.Lightness)
				{
					return (StiBrush)new StiSolidBrush(StiColorUtils.ChangeLightness(color, num));
				}
				return (StiBrush)new StiSolidBrush(StiColorUtils.ChangeDarkness(color, num));
			}
			return (StiBrush)new StiSolidBrush(color);
		}

		public HeatmapInfo(StiGdiMapContextPainter painter, StiMap map, List<StiMapData> mapData)
		{
			this.painter = painter;
			bool flag = true;
			foreach (StiMapData mapDatum in mapData)
			{
				if (mapDatum.Value != null && mapDatum.Value != null && double.TryParse(mapDatum.Value, out var result))
				{
					if (flag)
					{
						flag = false;
						min = result;
						max = result;
					}
					else if (result < min)
					{
						min = result;
					}
					else if (result > max)
					{
						max = result;
					}
				}
			}
			StiMapStyle mapStyle = StiMap.GetMapStyle(map);
			color = mapStyle.Heatmap.Color;
			zeroColor = mapStyle.Heatmap.ZeroColor;
			mode = mapStyle.Heatmap.Mode;
		}
	}

	private class HeatmapWithGroupInfo
	{
		private StiGdiMapContextPainter painter;

		private Dictionary<string, double[]> hash = new Dictionary<string, double[]>();

		private Dictionary<string, Color> hashColors = new Dictionary<string, Color>();

		private Color zeroColor;

		private StiHeatmapFillMode mode;

		public Brush GetBrush(StiMapData data)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Expected O, but got Unknown
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Expected O, but got Unknown
			if (data.Group == null)
			{
				return null;
			}
			if (!hash.ContainsKey(data.Group))
			{
				return (Brush)(object)painter.DefaultBrush;
			}
			if (data.Value == null || !double.TryParse(data.Value, out var result))
			{
				return (Brush)(object)painter.DefaultBrush;
			}
			if (result != 0.0)
			{
				Color color = hashColors[data.Group];
				double[] array = hash[data.Group];
				if (array[0] != array[1])
				{
					if (mode == StiHeatmapFillMode.Lightness)
					{
						return (Brush)new SolidBrush(StiColorUtils.ChangeLightness(color, (float)(0.8500000238418579 * (array[1] - result) / (array[1] - array[0]))));
					}
					return (Brush)new SolidBrush(StiColorUtils.ChangeDarkness(color, (float)(0.8500000238418579 * (array[1] - result) / (array[1] - array[0]))));
				}
				return (Brush)new SolidBrush(color);
			}
			return (Brush)new SolidBrush(zeroColor);
		}

		public StiBrush GetBrush1(StiMapData data)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Expected O, but got Unknown
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Expected O, but got Unknown
			if (data.Group == null)
			{
				return null;
			}
			if (!hash.ContainsKey(data.Group))
			{
				return (StiBrush)(object)painter.DefaultBrush1;
			}
			if (data.Value == null || !double.TryParse(data.Value, out var result))
			{
				return (StiBrush)(object)painter.DefaultBrush1;
			}
			if (result != 0.0)
			{
				Color color = hashColors[data.Group];
				double[] array = hash[data.Group];
				if (array[0] != array[1])
				{
					if (mode == StiHeatmapFillMode.Lightness)
					{
						return (StiBrush)new StiSolidBrush(StiColorUtils.ChangeLightness(color, (float)(0.8500000238418579 * (array[1] - result) / (array[1] - array[0]))));
					}
					return (StiBrush)new StiSolidBrush(StiColorUtils.ChangeDarkness(color, (float)(0.8500000238418579 * (array[1] - result) / (array[1] - array[0]))));
				}
				return (StiBrush)new StiSolidBrush(color);
			}
			return (StiBrush)new StiSolidBrush(zeroColor);
		}

		public HeatmapWithGroupInfo(StiGdiMapContextPainter painter, StiMap map, List<StiMapData> mapData)
		{
			this.painter = painter;
			StiMapStyle mapStyle = StiMap.GetMapStyle(map);
			zeroColor = mapStyle.HeatmapWithGroup.ZeroColor;
			mode = mapStyle.HeatmapWithGroup.Mode;
			int num = 0;
			Color[] colors = mapStyle.HeatmapWithGroup.Colors;
			foreach (StiMapData mapDatum in mapData)
			{
				string group = mapDatum.Group;
				if (group == null || mapDatum.Value == null || mapDatum.Value == null || !double.TryParse(mapDatum.Value, out var result))
				{
					continue;
				}
				if (!hash.ContainsKey(group))
				{
					double[] value = new double[2] { result, result };
					hash.Add(group, value);
				}
				else
				{
					double[] array = hash[group];
					if (result < array[0])
					{
						array[0] = result;
					}
					else if (result > array[1])
					{
						array[1] = result;
					}
				}
				if (!hashColors.ContainsKey(group))
				{
					Color value2 = ((colors.Length == 0 || colors.Length < num) ? painter.DefaultBrush.Color : colors[num]);
					num++;
					if (num >= colors.Length)
					{
						num = 0;
					}
					hashColors.Add(group, value2);
				}
			}
		}
	}

	private class NoneInfo
	{
		private Color[] Colors;

		private int index;

		public NoneInfo()
		{
			Colors = StiMapHelper.GetColors();
		}

		public StiSolidBrush GetBrush()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			Color color = Colors[index];
			index++;
			if (index >= Colors.Length)
			{
				index = 0;
			}
			return new StiSolidBrush(color);
		}
	}

	private StiMap map;

	private HeatmapInfo heatmapInfo;

	private HeatmapWithGroupInfo heatmapWithGroupInfo;

	private NoneInfo noneInfo;

	private Dictionary<string, StiMapGroup> hashGroup = new Dictionary<string, StiMapGroup>();

	private StiStyleColorsContainer colorsContainer = new StiStyleColorsContainer();

	internal float IndividualStep { get; set; }

	internal SolidBrush DefaultBrush { get; set; }

	internal StiSolidBrush DefaultBrush1 { get; set; }

	internal List<StiMapData> MapData { get; private set; }

	internal bool IsBorderEmpty { get; set; }

	internal StiMapStyle MapStyle { get; set; }

	public StiDataTable DataTable { get; set; }

	public List<string> CheckedMapKeys { get; set; }

	public string MouseOverKey { get; set; }

	public StiAnimationEngine AnimationEngine { get; set; }

	public List<StiGeom> Geoms { get; } = new List<StiGeom>();


	public void Render(Graphics graphics, bool useZoom)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Render(graphics, useZoom, map.GetPaintRectangle(convertToHInches: true, convertByZoom: true));
	}

	public Brush GetGeomBrush(StiMapData data)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		if (map.MapType == StiMapType.Individual)
		{
			if (map.ColorEach)
			{
				if (data == null)
				{
					return (Brush)new SolidBrush(MapStyle.DefaultColor);
				}
				Brush val = ParseHexColor(data.Color);
				if (val != null)
				{
					return val;
				}
				return (Brush)new SolidBrush(noneInfo.GetBrush().Color);
			}
			return (Brush)new SolidBrush(StiColorUtils.ChangeLightness(MapStyle.IndividualColor, IndividualStep));
		}
		switch (map.MapType)
		{
		case StiMapType.Individual:
			return (Brush)(object)DefaultBrush;
		case StiMapType.Heatmap:
			return heatmapInfo.GetBrush(data);
		case StiMapType.Group:
			if (data == null || data.Group == null || !hashGroup.ContainsKey(data.Group))
			{
				return (Brush)new SolidBrush(MapStyle.DefaultColor);
			}
			return hashGroup[data.Group].Fill;
		case StiMapType.HeatmapWithGroup:
			if (data == null || data.Group == null)
			{
				return (Brush)new SolidBrush(MapStyle.DefaultColor);
			}
			return heatmapWithGroupInfo.GetBrush(data);
		default:
			return (Brush)(object)DefaultBrush;
		}
	}

	public StiBrush GetGeomBrush1(StiMapData data)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		if (map.MapType == StiMapType.Individual)
		{
			if (map.ColorEach)
			{
				if (data == null)
				{
					return (StiBrush)new StiSolidBrush(MapStyle.DefaultColor);
				}
				StiBrush val = ParseHexColor1(data.Color);
				if (val != null)
				{
					return val;
				}
				return (StiBrush)new StiSolidBrush(noneInfo.GetBrush().Color);
			}
			return (StiBrush)new StiSolidBrush(StiColorUtils.ChangeLightness(MapStyle.IndividualColor, IndividualStep));
		}
		switch (map.MapType)
		{
		case StiMapType.Individual:
			return (StiBrush)(object)DefaultBrush1;
		case StiMapType.Heatmap:
			if (data.Value == null)
			{
				return (StiBrush)(object)DefaultBrush1;
			}
			return heatmapInfo.GetBrush1(data);
		case StiMapType.Group:
			if (data.Group == null || !hashGroup.ContainsKey(data.Group))
			{
				return (StiBrush)new StiSolidBrush(MapStyle.DefaultColor);
			}
			return (StiBrush)(object)hashGroup[data.Group].Fill1;
		case StiMapType.HeatmapWithGroup:
			if (data.Group == null)
			{
				return (StiBrush)new StiSolidBrush(MapStyle.DefaultColor);
			}
			return heatmapWithGroupInfo.GetBrush1(data);
		default:
			return (StiBrush)(object)DefaultBrush1;
		}
	}

	protected List<object> GetValues(IStiMeter meter)
	{
		if (DataTable == null || meter == null)
		{
			return null;
		}
		int num = DataTable.Meters.IndexOf(meter);
		if (num == -1)
		{
			return null;
		}
		return IEnumerableExt.GetArrayItem((IEnumerable<object[]>)DataTable.Rows, num).ToList();
	}

	internal void PrepareDataColumns()
	{
		if (map.DataFrom == StiMapSource.Manual)
		{
			MapData = map.GetMapData();
			return;
		}
		MapData = StiMap.GetDefaultMapData(map.Report, map.MapIdent, map.Language);
		List<object> list = null;
		List<object> list2 = null;
		List<object> list3 = null;
		List<object> list4 = null;
		List<object> list5 = null;
		if (DataTable == null && map.DataTable != null)
		{
			DataTable = map.DataTable;
		}
		if (DataTable != null)
		{
			StiDataTable dataTable = DataTable;
			list = GetValues((dataTable == null) ? null : dataTable.Meters?.FirstOrDefault((Func<IStiMeter, bool>)((IStiMeter f) => f is IStiKeyMapMeter)));
			StiDataTable dataTable2 = DataTable;
			list2 = GetValues((dataTable2 == null) ? null : dataTable2.Meters?.FirstOrDefault((Func<IStiMeter, bool>)((IStiMeter f) => f is IStiNameMapMeter)));
			StiDataTable dataTable3 = DataTable;
			list3 = GetValues((dataTable3 == null) ? null : dataTable3.Meters?.FirstOrDefault((Func<IStiMeter, bool>)((IStiMeter f) => f is IStiValueMapMeter)));
			StiDataTable dataTable4 = DataTable;
			list4 = GetValues((dataTable4 == null) ? null : dataTable4.Meters?.FirstOrDefault((Func<IStiMeter, bool>)((IStiMeter f) => f is IStiGroupMapMeter)));
			StiDataTable dataTable5 = DataTable;
			list5 = GetValues((dataTable5 == null) ? null : dataTable5.Meters?.FirstOrDefault((Func<IStiMeter, bool>)((IStiMeter f) => f is IStiColorMapMeter)));
		}
		else
		{
			try
			{
				map.Report.Dictionary.Connect();
				map.Report.Dictionary.ConnectDataTransformations();
				list = ((!string.IsNullOrEmpty(map.KeyDataColumn)) ? StiDataColumn.GetDataListFromDataColumn(map.Report.Dictionary, map.KeyDataColumn) : null);
				list2 = ((!string.IsNullOrEmpty(map.NameDataColumn)) ? StiDataColumn.GetDataListFromDataColumn(map.Report.Dictionary, map.NameDataColumn) : null);
				list3 = ((!string.IsNullOrEmpty(map.ValueDataColumn)) ? StiDataColumn.GetDataListFromDataColumn(map.Report.Dictionary, map.ValueDataColumn) : null);
				list4 = ((!string.IsNullOrEmpty(map.GroupDataColumn)) ? StiDataColumn.GetDataListFromDataColumn(map.Report.Dictionary, map.GroupDataColumn) : null);
				list5 = ((!string.IsNullOrEmpty(map.ColorDataColumn)) ? StiDataColumn.GetDataListFromDataColumn(map.Report.Dictionary, map.ColorDataColumn) : null);
				map.Report.Dictionary.Disconnect();
			}
			catch
			{
			}
		}
		bool flag = StiGssMapHelper.AllowGss(map.MapIdent);
		if (list != null)
		{
			list = new StiMapKeyHelper().GetMapKeysFromNames(list, map.MapIdent, map.Language, (IStiReport)(object)map.Report)?.Cast<object>().ToList();
		}
		if (list == null)
		{
			return;
		}
		try
		{
			int num = 0;
			if (list != null)
			{
				if (num == 0)
				{
					num = list.Count;
				}
				else if (list.Count < num)
				{
					num = list.Count;
				}
			}
			if (list2 != null)
			{
				if (num == 0)
				{
					num = list2.Count;
				}
				else if (list2.Count < num)
				{
					num = list2.Count;
				}
			}
			if (list3 != null)
			{
				if (num == 0)
				{
					num = list3.Count;
				}
				else if (list3.Count < num)
				{
					num = list3.Count;
				}
			}
			if (list4 != null)
			{
				if (num == 0)
				{
					num = list4.Count;
				}
				else if (list4.Count < num)
				{
					num = list4.Count;
				}
			}
			if (list5 != null)
			{
				if (num == 0)
				{
					num = list5.Count;
				}
				else if (list5.Count < num)
				{
					num = list5.Count;
				}
			}
			if (list == null)
			{
				return;
			}
			foreach (StiMapData mapDatum in MapData)
			{
				mapDatum.Group = null;
				mapDatum.Value = null;
				if (list5 != null)
				{
					mapDatum.Color = null;
				}
			}
			Dictionary<string, string> dictionary = (flag ? StiGssMapHelper.Get(map.MapIdent) : null);
			for (int i = 0; i < num; i++)
			{
				string key = null;
				if (list.Count <= i || list[i] == null)
				{
					continue;
				}
				key = list[i].ToString();
				if (string.IsNullOrEmpty(key))
				{
					continue;
				}
				key = key.ToLowerInvariant();
				if (flag && StiGssMapHelper.IsGssValue(key))
				{
					if (dictionary.TryGetValue(key, out var value))
					{
						key = value.ToLowerInvariant();
					}
					else
					{
						key = string.Empty;
					}
				}
				StiMapData stiMapData = MapData.FirstOrDefault((StiMapData x) => x.Key.ToLowerInvariant() == key);
				if (stiMapData != null)
				{
					if (list2 != null && list2[i] != null)
					{
						string name = list2[i].ToString();
						stiMapData.Name = name;
					}
					if (list3 != null && list3[i] != null)
					{
						string value2 = list3[i].ToString();
						stiMapData.Value = value2;
					}
					if (list4 != null && list4[i] != null)
					{
						string group = list4[i].ToString();
						stiMapData.Group = group;
					}
					if (list5 != null && list5[i] != null)
					{
						string color = list5[i].ToString();
						stiMapData.Color = color;
					}
				}
			}
		}
		catch
		{
		}
	}

	public static StiMapInteractionContainer RenderInteractionContainer(StiReport report, string mapID, string lang)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Expected O, but got Unknown
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		if (mapID == StiMapID.ChinaWithHongKongAndMacau.ToString() || mapID == StiMapID.ChinaWithHongKongMacauAndTaiwan.ToString())
		{
			mapID = StiMapID.China.ToString();
		}
		StiMapGeomsContainer geomsObject = StiMapLoader.GetGeomsObject(report, mapID, lang);
		StiMapInteractionContainer stiMapInteractionContainer = new StiMapInteractionContainer(geomsObject.Width, geomsObject.Height);
		foreach (StiMapGeomsObject item in geomsObject.Geoms.OrderBy((StiMapGeomsObject x) => x.Key))
		{
			List<GraphicsPath> list = new List<GraphicsPath>();
			GraphicsPath val = new GraphicsPath();
			PointF pointF = PointF.Empty;
			foreach (StiMapGeom geom in item.Geoms)
			{
				switch (geom.GeomType)
				{
				case StiMapGeomType.MoveTo:
				{
					if (val.PointCount > 0)
					{
						val.CloseFigure();
						list.Add(val);
						val = new GraphicsPath();
					}
					StiMoveToMapGeom stiMoveToMapGeom = (StiMoveToMapGeom)geom;
					pointF = new PointF((float)stiMoveToMapGeom.X, (float)stiMoveToMapGeom.Y);
					break;
				}
				case StiMapGeomType.Line:
				{
					StiLineMapGeom stiLineMapGeom = (StiLineMapGeom)geom;
					PointF pointF3 = new PointF((float)stiLineMapGeom.X, (float)stiLineMapGeom.Y);
					val.AddLine(pointF, pointF3);
					pointF = pointF3;
					break;
				}
				case StiMapGeomType.EllipticalArc:
				{
					StiEllipticalArcMapGeom stiEllipticalArcMapGeom = (StiEllipticalArcMapGeom)geom;
					stiEllipticalArcMapGeom.AddToPath(val);
					pointF = new PointF((float)stiEllipticalArcMapGeom.EndX, (float)stiEllipticalArcMapGeom.EndY);
					break;
				}
				case StiMapGeomType.QuadraticCurve:
				{
					StiQuadraticCurveMapGeom stiQuadraticCurveMapGeom = (StiQuadraticCurveMapGeom)geom;
					stiQuadraticCurveMapGeom.AddToPath(val);
					pointF = new PointF((float)stiQuadraticCurveMapGeom.EndX, (float)stiQuadraticCurveMapGeom.EndY);
					break;
				}
				case StiMapGeomType.Bezier:
				{
					StiBezierMapGeom stiBezierMapGeom = (StiBezierMapGeom)geom;
					PointF pointF4 = new PointF((float)stiBezierMapGeom.X1, (float)stiBezierMapGeom.Y1);
					PointF pointF5 = new PointF((float)stiBezierMapGeom.X2, (float)stiBezierMapGeom.Y2);
					PointF pointF6 = new PointF((float)stiBezierMapGeom.X3, (float)stiBezierMapGeom.Y3);
					val.AddBezier(pointF, pointF4, pointF5, pointF6);
					pointF = pointF6;
					break;
				}
				case StiMapGeomType.Beziers:
				{
					StiBeziersMapGeom stiBeziersMapGeom = (StiBeziersMapGeom)geom;
					List<PointF> list2 = new List<PointF> { pointF };
					PointF pointF2 = PointF.Empty;
					for (int i = 0; i < stiBeziersMapGeom.Array.Length; i += 2)
					{
						pointF2 = new PointF((float)stiBeziersMapGeom.Array[i], (float)stiBeziersMapGeom.Array[i + 1]);
						list2.Add(pointF2);
					}
					list2.Add(pointF2);
					pointF = pointF2;
					if (list2.Count == 9)
					{
						list2.RemoveAt(8);
						list2.RemoveAt(7);
					}
					val.AddBeziers(list2.ToArray());
					break;
				}
				case StiMapGeomType.Close:
					val.CloseFigure();
					list.Add(val);
					val = new GraphicsPath();
					break;
				}
			}
			if (val.PointCount > 0)
			{
				val.CloseFigure();
				list.Add(val);
			}
			stiMapInteractionContainer.Add(item.Key, list);
		}
		return stiMapInteractionContainer;
	}

	private void DrawState(Graphics g, GraphicsPath path, string key)
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		if (CheckedMapKeys != null && CheckedMapKeys.Contains(key))
		{
			HatchBrush val = new HatchBrush((HatchStyle)23, Color.FromArgb(100, Color.White), Color.Transparent);
			try
			{
				Bitmap val2 = new Bitmap(8, 8);
				try
				{
					Graphics val3 = Graphics.FromImage((Image)(object)val2);
					try
					{
						val3.FillRectangle((Brush)(object)val, 0, 0, 8, 8);
						TextureBrush val4 = new TextureBrush((Image)(object)val2);
						val4.ScaleTransform((float)StiScale.X, (float)StiScale.Y);
						g.FillPath((Brush)(object)val4, path);
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
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
		}
		if (MouseOverKey == key)
		{
			SolidBrush val5 = new SolidBrush(Color.FromArgb(100, Color.White));
			try
			{
				g.FillPath((Brush)(object)val5, path);
			}
			finally
			{
				((IDisposable)val5)?.Dispose();
			}
		}
	}

	public void Render(Graphics g, bool useZoom, RectangleD rect, bool center = true)
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ea: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		//IL_0759: Unknown result type (might be due to invalid IL or missing references)
		//IL_075c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		//IL_0764: Unknown result type (might be due to invalid IL or missing references)
		//IL_0769: Unknown result type (might be due to invalid IL or missing references)
		//IL_0787: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Expected O, but got Unknown
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a0: Expected O, but got Unknown
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bc: Expected O, but got Unknown
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Expected O, but got Unknown
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Expected O, but got Unknown
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0653: Expected O, but got Unknown
		//IL_0b1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b26: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_090b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0910: Unknown result type (might be due to invalid IL or missing references)
		//IL_0912: Unknown result type (might be due to invalid IL or missing references)
		//IL_0929: Expected I4, but got Unknown
		//IL_096f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0974: Unknown result type (might be due to invalid IL or missing references)
		//IL_0976: Unknown result type (might be due to invalid IL or missing references)
		//IL_0989: Expected I4, but got Unknown
		//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d3: Expected O, but got Unknown
		//IL_09d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09da: Expected O, but got Unknown
		//IL_0a7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a92: Expected I4, but got Unknown
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aae: Expected O, but got Unknown
		//IL_0aa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab0: Expected O, but got Unknown
		//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a55: Expected I4, but got Unknown
		//IL_0adb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae7: Expected O, but got Unknown
		if (map.MapIdent == StiMapID.ChinaWithHongKongAndMacau.ToString() || map.MapIdent == StiMapID.ChinaWithHongKongMacauAndTaiwan.ToString())
		{
			map.MapIdent = StiMapID.China.ToString();
		}
		MapStyle = StiMap.GetMapStyle(map);
		string text = ((!string.IsNullOrEmpty(MapStyle.DashboardName)) ? MapStyle.DashboardName : MapStyle.Name);
		DefaultBrush = new SolidBrush(MapStyle.DefaultColor);
		DefaultBrush1 = new StiSolidBrush(MapStyle.DefaultColor);
		PrepareDataColumns();
		UpdateGroupedData();
		UpdateHeatmapWithGroup();
		float num = 1f;
		StiMapGeomsContainer geomsObject = StiMapLoader.GetGeomsObject(map.Report, map.MapIdent, map.Language);
		bool flag = false;
		if (useZoom && map.Stretch)
		{
			num = Math.Min((float)((RectangleD)(ref rect)).Width / (float)geomsObject.Width, (float)((RectangleD)(ref rect)).Height / (float)geomsObject.Height);
			if (num == 0f)
			{
				num = (float)StiScale.X;
			}
			if (!map.Report.IsExporting && num > (float)StiScale.X)
			{
				num = (float)StiScale.X;
			}
			if (num < (float)(0.2 * StiScale.X))
			{
				flag = true;
			}
			g.ScaleTransform(num, num);
			if (center)
			{
				double num2 = ((RectangleD)(ref rect)).Width / (double)num;
				double num3 = ((RectangleD)(ref rect)).Height / (double)num;
				g.TranslateTransform((float)((num2 - geomsObject.Width) / 2.0), (float)((num3 - geomsObject.Height) / 2.0));
			}
		}
		List<string> list = new List<string>();
		TimeSpan timeSpan = TimeSpan.Zero;
		float num5 = (IndividualStep = 0.5f / (float)geomsObject.Geoms.Count);
		foreach (StiMapGeomsObject geoms in geomsObject.Geoms.OrderBy((StiMapGeomsObject x) => x.Key))
		{
			GraphicsPath val = new GraphicsPath();
			PointF pointF = PointF.Empty;
			StiMapData data = MapData.FirstOrDefault((StiMapData x) => x.Key == geoms.Key);
			Brush geomBrush = GetGeomBrush(data);
			geoms.Animation.BeginTimeCorrect = TimeSpan.Zero;
			geomBrush = (Brush)StiAnimationEngine.GetAnimationOpacity(this, geomBrush, geoms.Animation);
			Color color = (Color)StiAnimationEngine.GetAnimationOpacity(this, MapStyle.BorderColor, geoms.Animation);
			if (timeSpan < geoms.Animation.BeginTime + geoms.Animation.BeginTime)
			{
				timeSpan = geoms.Animation.BeginTime + geoms.Animation.BeginTime;
			}
			IndividualStep += num5;
			foreach (StiMapGeom geom in geoms.Geoms)
			{
				switch (geom.GeomType)
				{
				case StiMapGeomType.MoveTo:
				{
					if (val.PointCount > 0)
					{
						val.CloseFigure();
						g.FillPath(geomBrush, val);
						DrawState(g, val, geoms.Key);
						if (!IsBorderEmpty)
						{
							Pen val3 = new Pen(color, (float)MapStyle.BorderSize);
							try
							{
								g.DrawPath(val3, val);
							}
							finally
							{
								((IDisposable)val3)?.Dispose();
							}
						}
						val.Dispose();
						val = new GraphicsPath();
					}
					StiMoveToMapGeom stiMoveToMapGeom = (StiMoveToMapGeom)geom;
					pointF = new PointF((float)stiMoveToMapGeom.X, (float)stiMoveToMapGeom.Y);
					break;
				}
				case StiMapGeomType.Line:
				{
					StiLineMapGeom stiLineMapGeom = (StiLineMapGeom)geom;
					PointF pointF6 = new PointF((float)stiLineMapGeom.X, (float)stiLineMapGeom.Y);
					val.AddLine(pointF, pointF6);
					pointF = pointF6;
					break;
				}
				case StiMapGeomType.Bezier:
				{
					StiBezierMapGeom stiBezierMapGeom = (StiBezierMapGeom)geom;
					PointF pointF2 = new PointF((float)stiBezierMapGeom.X1, (float)stiBezierMapGeom.Y1);
					PointF pointF3 = new PointF((float)stiBezierMapGeom.X2, (float)stiBezierMapGeom.Y2);
					PointF pointF4 = new PointF((float)stiBezierMapGeom.X3, (float)stiBezierMapGeom.Y3);
					val.AddBezier(pointF, pointF2, pointF3, pointF4);
					pointF = pointF4;
					break;
				}
				case StiMapGeomType.Beziers:
				{
					StiBeziersMapGeom stiBeziersMapGeom = (StiBeziersMapGeom)geom;
					List<PointF> list2 = new List<PointF> { pointF };
					PointF pointF5 = PointF.Empty;
					for (int i = 0; i < stiBeziersMapGeom.Array.Length; i += 2)
					{
						pointF5 = new PointF((float)stiBeziersMapGeom.Array[i], (float)stiBeziersMapGeom.Array[i + 1]);
						list2.Add(pointF5);
					}
					list2.Add(pointF5);
					pointF = pointF5;
					if ((list2.Count - 1) % 3 == 2)
					{
						list2.RemoveAt(list2.Count - 1);
					}
					if ((list2.Count - 1) % 3 == 1)
					{
						list2.RemoveAt(list2.Count - 1);
					}
					val.AddBeziers(list2.ToArray());
					break;
				}
				case StiMapGeomType.Close:
					val.CloseFigure();
					g.FillPath(geomBrush, val);
					DrawState(g, val, geoms.Key);
					if (!IsBorderEmpty)
					{
						Pen val2 = new Pen(color, (float)MapStyle.BorderSize);
						try
						{
							g.DrawPath(val2, val);
						}
						finally
						{
							((IDisposable)val2)?.Dispose();
						}
					}
					val.Dispose();
					val = new GraphicsPath();
					break;
				}
			}
			if (val.PointCount > 0)
			{
				val.CloseFigure();
				g.FillPath(geomBrush, val);
				DrawState(g, val, geoms.Key);
				if (!IsBorderEmpty)
				{
					Pen val4 = new Pen(color, (float)MapStyle.BorderSize);
					try
					{
						g.DrawPath(val4, val);
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
			}
			val.Dispose();
			if (map.ShowValue || map.DisplayNameType != StiDisplayNameType.None)
			{
				list.Add(geoms.Key);
			}
		}
		if (list.Count <= 0)
		{
			return;
		}
		StiMapSvgContainer stiMapSvgContainer = StiMapLoader.LoadResource(map.Report, map.MapIdent, map.Language);
		float num6 = 1f;
		float num7 = ((StiScale.Factor == 1.0) ? 18f : ((float)(10.0 * StiScale.X)));
		if (stiMapSvgContainer.TextScale.HasValue)
		{
			num6 = (float)stiMapSvgContainer.TextScale.GetValueOrDefault();
			num7 *= num6;
			flag = false;
		}
		if (flag)
		{
			return;
		}
		SmoothingMode smoothingMode = g.SmoothingMode;
		TextRenderingHint textRenderingHint = g.TextRenderingHint;
		InterpolationMode interpolationMode = g.InterpolationMode;
		g.SmoothingMode = (SmoothingMode)4;
		g.TextRenderingHint = (TextRenderingHint)4;
		g.InterpolationMode = (InterpolationMode)7;
		Font val5 = new Font("Calibri", num7);
		SolidBrush val6 = new SolidBrush(MapStyle.LabelForeground);
		SolidBrush val7 = new SolidBrush(Color.FromArgb(200, MapStyle.LabelShadowForeground));
		foreach (string key in list)
		{
			StiOpacityAnimation animation = stiMapSvgContainer.Geoms.Find((StiMapGeomsObject geom) => geom.Key == key).Animation;
			animation.BeginTimeCorrect = timeSpan;
			StiMapSvg stiMapSvg = stiMapSvgContainer.HashPaths[key];
			bool flag2 = false;
			if ((map.DisplayNameType != StiDisplayNameType.Short) ? stiMapSvg.SkipText : ((!stiMapSvg.SkipTextIso.HasValue) ? stiMapSvg.SkipText : stiMapSvg.SkipTextIso.GetValueOrDefault()))
			{
				continue;
			}
			string pathText = GetPathText(stiMapSvg, key);
			if (string.IsNullOrEmpty(pathText))
			{
				continue;
			}
			Rectangle pathRect = GetPathRect(stiMapSvg);
			SizeF sizeF = ((map.DisplayNameType == StiDisplayNameType.Full && stiMapSvg.SetMaxWidth) ? g.MeasureString(pathText, val5, (int)((double)pathRect.Width * StiScale.X)) : g.MeasureString(pathText, val5));
			sizeF.Width /= (float)StiScale.Step;
			sizeF.Height /= (float)StiScale.Step;
			float x2 = 0f;
			float y = 0f;
			StiTextHorAlignment pathHorAlignment = GetPathHorAlignment(stiMapSvg);
			switch ((int)pathHorAlignment)
			{
			case 0:
			case 3:
				x2 = pathRect.X;
				break;
			case 2:
				x2 = (float)pathRect.Right - sizeF.Width;
				break;
			case 1:
				x2 = (float)pathRect.X + ((float)pathRect.Width - sizeF.Width) / 2f;
				break;
			}
			StiVertAlignment pathVertAlignment = GetPathVertAlignment(stiMapSvg);
			switch ((int)pathVertAlignment)
			{
			case 0:
				y = pathRect.Y;
				break;
			case 2:
				y = (float)pathRect.Bottom - sizeF.Height;
				break;
			case 1:
				y = (float)pathRect.Y + ((float)pathRect.Height - sizeF.Height) / 2f;
				break;
			}
			GraphicsPath val8 = new GraphicsPath();
			try
			{
				StringFormat val9 = new StringFormat();
				try
				{
					val9.Alignment = (StringAlignment)1;
					val9.LineAlignment = (StringAlignment)1;
					if (map.DisplayNameType == StiDisplayNameType.Full)
					{
						float width = (stiMapSvg.SetMaxWidth ? (sizeF.Width + 20f) : (sizeF.Width + 10f));
						RectangleF rectangleF = new RectangleF(x2, y, width, sizeF.Height);
						val8.AddString(pathText, val5.FontFamily, (int)val5.Style, val5.Size, rectangleF, val9);
					}
					else
					{
						RectangleF rectangleF2 = new RectangleF(x2, y, sizeF.Width, sizeF.Height);
						val8.AddString(pathText, val5.FontFamily, (int)val5.Style, val5.Size, rectangleF2, val9);
					}
					Pen val10 = new Pen((Brush)(SolidBrush)StiAnimationEngine.GetAnimationOpacity(this, val7, animation), 4f * num6);
					try
					{
						val10.LineJoin = (LineJoin)2;
						g.DrawPath(val10, val8);
					}
					finally
					{
						((IDisposable)val10)?.Dispose();
					}
					g.FillPath((Brush)(SolidBrush)StiAnimationEngine.GetAnimationOpacity(this, val6, animation), val8);
				}
				finally
				{
					((IDisposable)val9)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val8)?.Dispose();
			}
		}
		g.SmoothingMode = smoothingMode;
		g.TextRenderingHint = textRenderingHint;
		g.InterpolationMode = interpolationMode;
		val5.Dispose();
		((Brush)val6).Dispose();
		((Brush)val7).Dispose();
	}

	private string GetPathText(StiMapSvg path, string key)
	{
		string text = null;
		StiMapData stiMapData = MapData.FirstOrDefault((StiMapData x) => x.Key == key);
		switch (map.DisplayNameType)
		{
		case StiDisplayNameType.Full:
			text = ((stiMapData != null) ? stiMapData.Name : path.EnglishName);
			break;
		case StiDisplayNameType.Short:
			text = StiMapHelper.PrepareIsoCode(path.ISOCode);
			break;
		}
		if (map.ShowValue && stiMapData != null && stiMapData.Value != null)
		{
			string text2 = null;
			if (map.ShortValue && double.TryParse(stiMapData.Value, out var result))
			{
				text2 = StiAbbreviationNumberFormatHelper.Format(result);
			}
			if (text2 == null)
			{
				text2 = stiMapData.Value;
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

	private Rectangle GetPathRect(StiMapSvg path)
	{
		if (map.DisplayNameType == StiDisplayNameType.Short)
		{
			return path.RectIso.IsEmpty ? path.Rect : path.RectIso;
		}
		return path.Rect;
	}

	private StiTextHorAlignment GetPathHorAlignment(StiMapSvg path)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
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

	private StiVertAlignment GetPathVertAlignment(StiMapSvg path)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
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

	private void FillGroupColors()
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		colorsContainer.Init(map, this);
		int num = 0;
		foreach (string item in hashGroup.Keys.OrderBy((string x) => x))
		{
			StiMapGroup stiMapGroup = hashGroup[item];
			stiMapGroup.Fill1 = colorsContainer.GetColor1(num, hashGroup.Count);
			stiMapGroup.Fill = (Brush)new SolidBrush(stiMapGroup.Fill1.Color);
			num++;
		}
	}

	public void UpdateHeatmapWithGroup()
	{
		heatmapInfo = new HeatmapInfo(this, map, MapData);
		noneInfo = new NoneInfo();
		if (map.MapType == StiMapType.HeatmapWithGroup)
		{
			heatmapWithGroupInfo = new HeatmapWithGroupInfo(this, map, MapData);
		}
	}

	public void UpdateGroupedData()
	{
		if (MapData == null)
		{
			return;
		}
		foreach (StiMapData mapDatum in MapData)
		{
			if (string.IsNullOrEmpty(mapDatum.Group))
			{
				continue;
			}
			double? num = null;
			if (mapDatum.Value != null && double.TryParse(mapDatum.Value, out var result))
			{
				num = result;
			}
			StiMapGroup stiMapGroup = null;
			if (!hashGroup.ContainsKey(mapDatum.Group))
			{
				stiMapGroup = new StiMapGroup
				{
					MinValue = num,
					MaxValue = num
				};
				hashGroup.Add(mapDatum.Group, stiMapGroup);
				continue;
			}
			stiMapGroup = hashGroup[mapDatum.Group];
			if (num.HasValue)
			{
				if (!stiMapGroup.MinValue.HasValue || !stiMapGroup.MaxValue.HasValue)
				{
					stiMapGroup.MinValue = 0.0;
					stiMapGroup.MaxValue = 0.0;
				}
				if (stiMapGroup.MinValue > num)
				{
					stiMapGroup.MinValue = num;
				}
				else if (stiMapGroup.MaxValue < num)
				{
					stiMapGroup.MaxValue = num;
				}
			}
		}
		FillGroupColors();
	}

	private Brush ParseHexColor(string color)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		try
		{
			if (!string.IsNullOrEmpty(color))
			{
				if (StringExt.StartsWithInvariant(color, "#"))
				{
					return (Brush)new SolidBrush(ColorTranslator.FromHtml(color));
				}
				return (Brush)new SolidBrush(Color.FromName(color));
			}
		}
		catch
		{
		}
		return null;
	}

	private StiBrush ParseHexColor1(string color)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		try
		{
			if (!string.IsNullOrEmpty(color) && StringExt.StartsWithInvariant(color, "#"))
			{
				return (StiBrush)new StiSolidBrush(ColorTranslator.FromHtml(color));
			}
		}
		catch
		{
		}
		return null;
	}

	public StiGdiMapContextPainter(StiMap map)
	{
		this.map = map;
	}
}
