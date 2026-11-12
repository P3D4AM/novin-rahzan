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
using Stimulsoft.Dashboard.Components.RegionMap;
using Stimulsoft.Dashboard.Drawing.Helpers;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Maps;
using Stimulsoft.Report.Maps.Helpers;
using Stimulsoft.Report.Painters.Context.Animation;

namespace Stimulsoft.Report.Painters;

public class StiGdiDbsMapContextPainter : IStiAnimationContextPainter<StiGeom>
{
	private class StiMapGroup
	{
		public double? MinValue;

		public double? MaxValue;

		public Brush Fill;

		public StiMapGroup(double? value)
		{
			MinValue = value;
			MaxValue = value;
		}
	}

	private class StiStyleColorsContainer
	{
		private List<Color> stackColors = new List<Color>();

		private StiGdiDbsMapContextPainter painter;

		public Brush GetColor(int index, int count)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			if (stackColors.Count == 0)
			{
				return (Brush)(object)painter.defaultBrush;
			}
			Color color = GetColors(count)[index];
			return (Brush)new SolidBrush(color);
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

		public void Init(StiRegionMapElement map, StiGdiDbsMapContextPainter painter)
		{
			this.painter = painter;
			stackColors.Clear();
			stackColors.AddRange(GetMapStyle(map).Colors);
		}
	}

	private class HeatmapInfo
	{
		private StiGdiDbsMapContextPainter painter;

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
				return (Brush)(object)painter.defaultBrush;
			}
			if (result == 0.0)
			{
				if (zeroColor == Color.Transparent)
				{
					return (Brush)(object)painter.defaultBrush;
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

		public HeatmapInfo(StiGdiDbsMapContextPainter painter, StiRegionMapElement map, List<StiMapData> mapData)
		{
			this.painter = painter;
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
						min = value.GetValueOrDefault();
						max = value.GetValueOrDefault();
					}
					else if (value < min)
					{
						min = value.GetValueOrDefault();
					}
					else if (value > max)
					{
						max = value.GetValueOrDefault();
					}
				}
			}
			StiMapStyle mapStyle = GetMapStyle(map);
			color = mapStyle.Heatmap.Color;
			zeroColor = mapStyle.Heatmap.ZeroColor;
			mode = mapStyle.Heatmap.Mode;
		}
	}

	private class HeatmapWithGroupInfo
	{
		private StiGdiDbsMapContextPainter painter;

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
				return (Brush)(object)painter.defaultBrush;
			}
			if (data.Value == null || !double.TryParse(data.Value, out var result))
			{
				return (Brush)(object)painter.defaultBrush;
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

		public HeatmapWithGroupInfo(StiGdiDbsMapContextPainter painter, StiRegionMapElement map, List<StiMapData> mapData)
		{
			this.painter = painter;
			StiMapStyle mapStyle = GetMapStyle(map);
			zeroColor = mapStyle.HeatmapWithGroup.ZeroColor;
			mode = mapStyle.HeatmapWithGroup.Mode;
			int num = 0;
			Color[] colors = mapStyle.HeatmapWithGroup.Colors;
			foreach (StiMapData mapDatum in mapData)
			{
				string group = mapDatum.Group;
				if (group == null || mapDatum.Value == null)
				{
					continue;
				}
				double? value = mapDatum.GetValue();
				if (!value.HasValue)
				{
					continue;
				}
				if (!hash.ContainsKey(group))
				{
					double[] value2 = new double[2]
					{
						value.GetValueOrDefault(),
						value.GetValueOrDefault()
					};
					hash.Add(group, value2);
				}
				else
				{
					double[] array = hash[group];
					if (value < array[0])
					{
						array[0] = value.GetValueOrDefault();
					}
					else if (value > array[1])
					{
						array[1] = value.GetValueOrDefault();
					}
				}
				if (!hashColors.ContainsKey(group))
				{
					Color value3 = ((colors.Length == 0 || colors.Length < num) ? painter.defaultBrush.Color : colors[num]);
					num++;
					if (num >= colors.Length)
					{
						num = 0;
					}
					hashColors.Add(group, value3);
				}
			}
		}
	}

	private class BubbleInfo
	{
		private StiGdiDbsMapContextPainter painter;

		private double minValue;

		private double maxValue;

		private double restValue;

		private const float diameterMin = 14f;

		private const float diameterMax = 130f;

		private const float bubbleSize = 116f;

		public float GetBubbleSize(string key)
		{
			StiMapData stiMapData = painter.MapData.FirstOrDefault((StiMapData x) => x.Key == key);
			if (stiMapData != null && stiMapData.Value != null)
			{
				double? value = stiMapData.GetValue();
				if (value.HasValue)
				{
					double num = (value.GetValueOrDefault() - minValue) / restValue;
					return (float)(14.0 + 116.0 * num);
				}
			}
			return 0f;
		}

		public BubbleInfo(StiGdiDbsMapContextPainter painter, List<StiMapData> mapData)
		{
			this.painter = painter;
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
						minValue = value.GetValueOrDefault();
						maxValue = value.GetValueOrDefault();
					}
					else if (value < minValue)
					{
						minValue = value.GetValueOrDefault();
					}
					else if (value > maxValue)
					{
						maxValue = value.GetValueOrDefault();
					}
				}
			}
			restValue = maxValue - minValue;
		}
	}

	private class NoneInfo
	{
		private Color[] colors;

		private int index;

		public StiSolidBrush GetBrush()
		{
			Color color = colors[index];
			index++;
			if (index >= colors.Length)
			{
				index = 0;
			}
			return new StiSolidBrush(color);
		}

		public NoneInfo()
		{
			colors = StiMapHelper.GetColors();
		}
	}

	internal float individualStep;

	private StiRegionMapElement map;

	private SolidBrush defaultBrush;

	private BubbleInfo bubbleInfo;

	private HeatmapInfo heatmapInfo;

	private HeatmapWithGroupInfo heatmapWithGroupInfo;

	private NoneInfo noneInfo;

	private Dictionary<string, StiMapGroup> hashGroup = new Dictionary<string, StiMapGroup>();

	private StiStyleColorsContainer colorsContainer = new StiStyleColorsContainer();

	internal List<StiMapData> MapData { get; private set; }

	internal bool IsBorderEmpty { get; set; }

	internal StiMapStyle MapStyle { get; set; }

	public List<string> CheckedMapKeys { get; set; }

	public string MouseOverKey { get; set; }

	public List<object> KeyValues { get; set; }

	public List<object> NameValues { get; set; }

	public List<object> ValueValues { get; set; }

	public List<object> GroupValues { get; set; }

	public List<object> ColorValues { get; set; }

	public StiAnimationEngine AnimationEngine { get; set; }

	public List<StiGeom> Geoms { get; set; } = new List<StiGeom>();


	public static StiMapStyle GetMapStyle(StiRegionMapElement map)
	{
		StiMapStyle stiMapStyle = null;
		if (map.Style == StiElementStyleIdent.Custom)
		{
			if (!string.IsNullOrEmpty(map.CustomStyleName))
			{
				stiMapStyle = map.Report.Styles[map.CustomStyleName] as StiMapStyle;
			}
		}
		else
		{
			stiMapStyle = StiMap.GetMapStyle(StiDashboardStyleHelper.GetMapStyleIdent(map));
		}
		if (stiMapStyle == null)
		{
			stiMapStyle = new StiMap21StyleFX();
		}
		return stiMapStyle;
	}

	public void Render(Graphics graphics, bool useZoom)
	{
		RectangleD paintRectangle = map.GetPaintRectangle(convertToHInches: true, convertByZoom: true);
		Render(graphics, useZoom, paintRectangle);
	}

	public void Render(Graphics g, bool useZoom, RectangleD rect, bool center = true)
	{
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Expected O, but got Unknown
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c66: Expected O, but got Unknown
		//IL_0c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c87: Expected O, but got Unknown
		//IL_0809: Unknown result type (might be due to invalid IL or missing references)
		//IL_080e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0811: Unknown result type (might be due to invalid IL or missing references)
		//IL_0816: Unknown result type (might be due to invalid IL or missing references)
		//IL_0819: Unknown result type (might be due to invalid IL or missing references)
		//IL_081e: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0843: Expected O, but got Unknown
		//IL_084e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0855: Expected O, but got Unknown
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0871: Expected O, but got Unknown
		//IL_0694: Unknown result type (might be due to invalid IL or missing references)
		//IL_069b: Expected O, but got Unknown
		//IL_066e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Expected O, but got Unknown
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Expected O, but got Unknown
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Expected O, but got Unknown
		//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0704: Expected O, but got Unknown
		//IL_0c0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac3: Expected O, but got Unknown
		//IL_0ac3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aca: Expected O, but got Unknown
		//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b82: Expected I4, but got Unknown
		//IL_0b8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9e: Expected O, but got Unknown
		//IL_0b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba0: Expected O, but got Unknown
		//IL_0b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b45: Expected I4, but got Unknown
		//IL_0bcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd7: Expected O, but got Unknown
		MapStyle = GetMapStyle(map);
		PrepareDataColumns();
		UpdateGroupedData();
		UpdateHeatmapWithGroup();
		float num = 1f;
		float num2 = 1f;
		PointF pointF = default(PointF);
		StiRegionMapZoomHelper.StiMapState stiMapState = StiRegionMapZoomHelper.Get(map);
		StiMapGeomsContainer geomsObject = StiMapLoader.GetGeomsObject(map.Report, map.MapIdent, map.Language);
		bool flag = false;
		if (useZoom)
		{
			num = Math.Min((float)rect.Width / (float)geomsObject.Width, (float)rect.Height / (float)geomsObject.Height);
			if (num == 0f || num > (float)StiScale.X)
			{
				num = (float)StiScale.X;
			}
			num2 = num;
			num *= stiMapState.Zoom;
			if (num2 < (float)(0.2 * StiScale.X))
			{
				flag = true;
			}
			if (center)
			{
				double num3 = rect.Width / (double)num;
				double num4 = rect.Height / (double)num;
				pointF = new PointF((float)((num3 - geomsObject.Width) / 2.0) + (float)stiMapState.MoveOffset.X, (float)((num4 - geomsObject.Height) / 2.0) + (float)stiMapState.MoveOffset.Y);
			}
			else
			{
				pointF = new PointF(stiMapState.MoveOffset.X, stiMapState.MoveOffset.Y);
			}
		}
		else
		{
			num = stiMapState.Zoom;
			pointF = new PointF(stiMapState.MoveOffset.X, stiMapState.MoveOffset.Y);
		}
		g.ScaleTransform(num, num);
		g.TranslateTransform(pointF.X, pointF.Y);
		stiMapState.LastDrawingZoom = num;
		defaultBrush = new SolidBrush(MapStyle.DefaultColor);
		List<string> list = new List<string>();
		int num5 = 0;
		int num6 = 0;
		TimeSpan timeSpan = TimeSpan.Zero;
		float num7 = (individualStep = 0.5f / (float)geomsObject.Geoms.Count);
		StiMapGeomsObject[] array = geomsObject.Geoms.OrderBy((StiMapGeomsObject x) => x.Key).ToArray();
		StiMapGeomsObject[] array2 = array;
		foreach (StiMapGeomsObject geoms in array2)
		{
			GraphicsPath val = new GraphicsPath();
			PointF pointF2 = PointF.Empty;
			StiMapData data = MapData.FirstOrDefault((StiMapData x) => x.Key == geoms.Key);
			Brush geomBrush = GetGeomBrush(data);
			geoms.Animation.BeginTimeCorrect = TimeSpan.Zero;
			geomBrush = (Brush)StiAnimationEngine.GetAnimationOpacity(this, geomBrush, geoms.Animation);
			Color color = (Color)StiAnimationEngine.GetAnimationOpacity(this, MapStyle.BorderColor, geoms.Animation);
			if (timeSpan < geoms.Animation.BeginTime + geoms.Animation.BeginTime)
			{
				timeSpan = geoms.Animation.BeginTime + geoms.Animation.BeginTime;
			}
			individualStep += num7;
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
					pointF2 = new PointF((float)stiMoveToMapGeom.X, (float)stiMoveToMapGeom.Y);
					num6 += 2;
					num5++;
					break;
				}
				case StiMapGeomType.Line:
				{
					StiLineMapGeom stiLineMapGeom = (StiLineMapGeom)geom;
					PointF pointF4 = new PointF((float)stiLineMapGeom.X, (float)stiLineMapGeom.Y);
					val.AddLine(pointF2, pointF4);
					pointF2 = pointF4;
					num6 += 2;
					num5++;
					break;
				}
				case StiMapGeomType.EllipticalArc:
				{
					StiEllipticalArcMapGeom stiEllipticalArcMapGeom = (StiEllipticalArcMapGeom)geom;
					stiEllipticalArcMapGeom.AddToPath(val);
					pointF2 = new PointF((float)stiEllipticalArcMapGeom.EndX, (float)stiEllipticalArcMapGeom.EndY);
					num6 += 2;
					num5++;
					break;
				}
				case StiMapGeomType.QuadraticCurve:
				{
					StiQuadraticCurveMapGeom stiQuadraticCurveMapGeom = (StiQuadraticCurveMapGeom)geom;
					stiQuadraticCurveMapGeom.AddToPath(val);
					pointF2 = new PointF((float)stiQuadraticCurveMapGeom.EndX, (float)stiQuadraticCurveMapGeom.EndY);
					num6 += 2;
					num5++;
					break;
				}
				case StiMapGeomType.Bezier:
				{
					StiBezierMapGeom stiBezierMapGeom = (StiBezierMapGeom)geom;
					PointF pointF5 = new PointF((float)stiBezierMapGeom.X1, (float)stiBezierMapGeom.Y1);
					PointF pointF6 = new PointF((float)stiBezierMapGeom.X2, (float)stiBezierMapGeom.Y2);
					PointF pointF7 = new PointF((float)stiBezierMapGeom.X3, (float)stiBezierMapGeom.Y3);
					val.AddBezier(pointF2, pointF5, pointF6, pointF7);
					pointF2 = pointF7;
					num6 += 6;
					num5++;
					break;
				}
				case StiMapGeomType.Beziers:
				{
					StiBeziersMapGeom stiBeziersMapGeom = (StiBeziersMapGeom)geom;
					List<PointF> list2 = new List<PointF> { pointF2 };
					PointF pointF3 = PointF.Empty;
					for (int j = 0; j < stiBeziersMapGeom.Array.Length; j += 2)
					{
						pointF3 = new PointF((float)stiBeziersMapGeom.Array[j], (float)stiBeziersMapGeom.Array[j + 1]);
						list2.Add(pointF3);
					}
					list2.Add(pointF3);
					pointF2 = pointF3;
					if ((list2.Count - 1) % 3 == 2)
					{
						list2.RemoveAt(list2.Count - 1);
					}
					if ((list2.Count - 1) % 3 == 1)
					{
						list2.RemoveAt(list2.Count - 1);
					}
					val.AddBeziers(list2.ToArray());
					num6 += list2.Count;
					num5++;
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
			if (map.ShowValue || map.ShowName != StiDisplayNameType.None)
			{
				list.Add(geoms.Key);
			}
		}
		StiMapSvgContainer stiMapSvgContainer = StiMapLoader.LoadResource(map.Report, map.MapIdent, map.Language);
		if (list.Count > 0)
		{
			float num8 = 1f;
			float num9 = ((StiScale.Factor == 1.0) ? 18f : ((float)(10.0 * StiScale.X)));
			num9 /= stiMapState.Zoom;
			if (stiMapSvgContainer.TextScale.HasValue)
			{
				num8 = (float)stiMapSvgContainer.TextScale.GetValueOrDefault();
				num9 *= num8;
				flag = false;
			}
			if (!flag)
			{
				SmoothingMode smoothingMode = g.SmoothingMode;
				TextRenderingHint textRenderingHint = g.TextRenderingHint;
				InterpolationMode interpolationMode = g.InterpolationMode;
				g.SmoothingMode = (SmoothingMode)4;
				g.TextRenderingHint = (TextRenderingHint)4;
				g.InterpolationMode = (InterpolationMode)7;
				Font val5 = new Font("Calibri", num9);
				SolidBrush val6 = new SolidBrush(MapStyle.LabelForeground);
				SolidBrush val7 = new SolidBrush(Color.FromArgb(200, MapStyle.LabelShadowForeground));
				foreach (string key in list)
				{
					StiOpacityAnimation animation = stiMapSvgContainer.Geoms.Find((StiMapGeomsObject geom) => geom.Key == key).Animation;
					animation.BeginTimeCorrect = timeSpan;
					StiMapSvg stiMapSvg = stiMapSvgContainer.HashPaths[key];
					if ((map.ShowName == StiDisplayNameType.Short) ? ((!stiMapSvg.SkipTextIso.HasValue) ? stiMapSvg.SkipText : stiMapSvg.SkipTextIso.GetValueOrDefault()) : (stiMapSvg.SkipText && (!stiMapSvg.ShowHiddenTextIfZoom.HasValue || !((double)stiMapState.Zoom >= stiMapSvg.ShowHiddenTextIfZoom.GetValueOrDefault())) && stiMapSvg.SkipText))
					{
						continue;
					}
					string pathText = GetPathText(stiMapSvg, key);
					if (string.IsNullOrEmpty(pathText))
					{
						continue;
					}
					Rectangle pathRect = GetPathRect(stiMapSvg);
					SizeF sizeF = ((map.ShowName == StiDisplayNameType.Full && stiMapSvg.SetMaxWidth) ? g.MeasureString(pathText, val5, (int)((double)pathRect.Width * StiScale.X)) : g.MeasureString(pathText, val5));
					sizeF.Width /= (float)StiScale.Step;
					sizeF.Height /= (float)StiScale.Step;
					float x2 = 0f;
					float y = 0f;
					switch (GetPathHorAlignment(stiMapSvg))
					{
					case StiTextHorAlignment.Left:
					case StiTextHorAlignment.Width:
						x2 = pathRect.X;
						break;
					case StiTextHorAlignment.Right:
						x2 = (float)pathRect.Right - sizeF.Width;
						break;
					case StiTextHorAlignment.Center:
						x2 = (float)pathRect.X + ((float)pathRect.Width - sizeF.Width) / 2f;
						break;
					}
					switch (GetPathVertAlignment(stiMapSvg))
					{
					case StiVertAlignment.Top:
						y = pathRect.Y;
						break;
					case StiVertAlignment.Bottom:
						y = (float)pathRect.Bottom - sizeF.Height;
						break;
					case StiVertAlignment.Center:
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
							if (map.ShowName == StiDisplayNameType.Full)
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
							Pen val10 = new Pen((Brush)(SolidBrush)StiAnimationEngine.GetAnimationOpacity(this, val7, animation), 4f * num8);
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
		}
		if (!map.ShowBubble)
		{
			return;
		}
		SolidBrush val11 = new SolidBrush(Color.FromArgb(150, MapStyle.BubbleBackColor));
		try
		{
			Pen val12 = new Pen(Color.FromArgb(170, MapStyle.BubbleBorderColor), 2f);
			try
			{
				StiMapGeomsObject[] array3 = array;
				foreach (StiMapGeomsObject stiMapGeomsObject in array3)
				{
					float bubbleSize = bubbleInfo.GetBubbleSize(stiMapGeomsObject.Key);
					if (bubbleSize > 0f)
					{
						StiMapSvg stiMapSvg2 = stiMapSvgContainer.HashPaths[stiMapGeomsObject.Key];
						Point point = new Point(stiMapSvg2.RectIso.X + stiMapSvg2.RectIso.Width / 2, stiMapSvg2.RectIso.Y + stiMapSvg2.RectIso.Height / 2);
						g.DrawEllipse(val12, new RectangleF((float)point.X - bubbleSize / 2f - 1f, (float)point.Y - bubbleSize / 2f - 1f, bubbleSize + 2f, bubbleSize + 2f));
						g.FillEllipse((Brush)(object)val11, new RectangleF((float)point.X - bubbleSize / 2f, (float)point.Y - bubbleSize / 2f, bubbleSize, bubbleSize));
					}
				}
			}
			finally
			{
				((IDisposable)val12)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val11)?.Dispose();
		}
	}

	private string GetPathText(StiMapSvg path, string key)
	{
		string text = null;
		StiMapData stiMapData = MapData.FirstOrDefault((StiMapData x) => x.Key == key);
		switch (map.ShowName)
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
			if (map.ShortValue)
			{
				double? value = stiMapData.GetValue();
				if (value.HasValue)
				{
					text2 = StiAbbreviationNumberFormatHelper.Format(value.GetValueOrDefault());
				}
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
		if (map.ShowName == StiDisplayNameType.Short)
		{
			return path.RectIso.IsEmpty ? path.Rect : path.RectIso;
		}
		return path.Rect;
	}

	private StiTextHorAlignment GetPathHorAlignment(StiMapSvg path)
	{
		if (map.ShowName == StiDisplayNameType.Short)
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
		if (map.ShowName == StiDisplayNameType.Short)
		{
			if (!path.VertAlignmentIso.HasValue)
			{
				return path.VertAlignment;
			}
			return path.VertAlignmentIso.GetValueOrDefault();
		}
		return path.VertAlignment;
	}

	public Brush GetGeomBrush(StiMapData data)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
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
			return (Brush)new SolidBrush(StiColorUtils.ChangeLightness(MapStyle.IndividualColor, individualStep));
		}
		switch (map.MapType)
		{
		case StiMapType.Individual:
			return (Brush)(object)defaultBrush;
		case StiMapType.Heatmap:
			if (data == null || data.Value == null)
			{
				return (Brush)(object)defaultBrush;
			}
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
			return (Brush)(object)defaultBrush;
		}
	}

	internal void PrepareDataColumns()
	{
		if (map.DataFrom == StiMapSource.Manual)
		{
			MapData = map.GetMapData();
			return;
		}
		MapData = StiMap.GetDefaultMapData(map.Report, map.MapIdent, map.Language);
		bool flag = StiGssMapHelper.AllowGss(map.MapIdent);
		try
		{
			int num = 0;
			if (KeyValues != null)
			{
				if (num == 0)
				{
					num = KeyValues.Count;
				}
				else if (KeyValues.Count < num)
				{
					num = KeyValues.Count;
				}
			}
			if (NameValues != null)
			{
				if (num == 0)
				{
					num = NameValues.Count;
				}
				else if (NameValues.Count < num)
				{
					num = NameValues.Count;
				}
			}
			if (ValueValues != null)
			{
				if (num == 0)
				{
					num = ValueValues.Count;
				}
				else if (ValueValues.Count < num)
				{
					num = ValueValues.Count;
				}
			}
			if (GroupValues != null)
			{
				if (num == 0)
				{
					num = GroupValues.Count;
				}
				else if (GroupValues.Count < num)
				{
					num = GroupValues.Count;
				}
			}
			if (ColorValues != null)
			{
				if (num == 0)
				{
					num = ColorValues.Count;
				}
				else if (ColorValues.Count < num)
				{
					num = ColorValues.Count;
				}
			}
			if (KeyValues == null)
			{
				return;
			}
			foreach (StiMapData mapDatum in MapData)
			{
				mapDatum.Group = null;
				mapDatum.Value = null;
				if (ColorValues != null)
				{
					mapDatum.Color = null;
				}
			}
			Dictionary<string, string> dictionary = (flag ? StiGssMapHelper.Get(map.MapIdent) : null);
			for (int i = 0; i < num; i++)
			{
				if (KeyValues.Count <= i || KeyValues[i] == null)
				{
					continue;
				}
				string key = KeyValues[i].ToString();
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
					if (NameValues != null && NameValues[i] != null)
					{
						string name = NameValues[i].ToString();
						stiMapData.Name = name;
					}
					if (ValueValues != null && ValueValues[i] != null)
					{
						string value2 = ValueValues[i].ToString();
						stiMapData.Value = value2;
					}
					if (GroupValues != null && GroupValues[i] != null)
					{
						string group = GroupValues[i].ToString();
						stiMapData.Group = group;
					}
					if (ColorValues != null && ColorValues[i] != null)
					{
						string color = ColorValues[i].ToString();
						stiMapData.Color = color;
					}
				}
			}
		}
		catch
		{
		}
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

	private void FillGroupColors()
	{
		colorsContainer.Init(map, this);
		int num = 0;
		foreach (string item in hashGroup.Keys.OrderBy((string x) => x))
		{
			StiMapGroup stiMapGroup = hashGroup[item];
			stiMapGroup.Fill = colorsContainer.GetColor(num, hashGroup.Count);
			num++;
		}
	}

	public void UpdateHeatmapWithGroup()
	{
		if (map.ShowBubble)
		{
			bubbleInfo = new BubbleInfo(this, MapData);
		}
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
			if (!string.IsNullOrEmpty(mapDatum.Group) && !hashGroup.ContainsKey(mapDatum.Group))
			{
				hashGroup.Add(mapDatum.Group, new StiMapGroup(null));
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
				if (color.StartsWithInvariant("#"))
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

	public StiGdiDbsMapContextPainter(StiRegionMapElement map)
	{
		this.map = map;
	}
}
