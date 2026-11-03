using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

[TypeConverter(typeof(StiUniversalConverter))]
public class StiYRadarAxis : StiRadarAxis, IStiYRadarAxis, IStiRadarAxis, ICloneable, IStiJsonReportObject, IStiPropertyGridObject
{
	[Browsable(false)]
	public StiComponentId ComponentId => StiComponentId.StiYRadarAxis;

	[Browsable(false)]
	public string PropName => string.Empty;

	[Browsable(false)]
	public StiYRadarAxisCoreXF YCore => base.Core as StiYRadarAxisCoreXF;

	[Browsable(true)]
	public override bool AllowApplyStyle
	{
		get
		{
			return base.AllowApplyStyle;
		}
		set
		{
			base.AllowApplyStyle = value;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public IStiAxisLabels Labels { get; set; } = new StiAxisLabels();


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public IStiAxisTicks Ticks { get; set; } = new StiAxisTicks();


	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public StiPenStyle LineStyle { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public Color LineColor { get; set; } = Color.Gray;


	[DefaultValue(1f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public float LineWidth { get; set; } = 1f;


	[Browsable(false)]
	public StiAxisInfoXF Info { get; set; } = new StiAxisInfoXF();


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyJObject(val, "Labels", ((IStiJsonReportObject)Labels).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Ticks", ((IStiJsonReportObject)Ticks).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyEnum(val, "LineStyle", (Enum)(object)LineStyle, (Enum)(object)(StiPenStyle)0);
		JsonExtensions.AddPropertyColor(val, "LineColor", LineColor, Color.Gray);
		JsonExtensions.AddPropertyFloat(val, "LineWidth", LineWidth, 1f);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Labels":
				((IStiJsonReportObject)Labels).LoadFromJsonObject((JObject)item.Value);
				break;
			case "Ticks":
				((IStiJsonReportObject)Ticks).LoadFromJsonObject((JObject)item.Value);
				break;
			case "LineStyle":
				LineStyle = StiJsonReportObjectHelper.DeserializeEnum<StiPenStyle>(item);
				break;
			case "LineColor":
				LineColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "LineWidth":
				LineWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			}
		}
	}

	public StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.YRadarAxis() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public override object Clone()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		IStiYRadarAxis stiYRadarAxis = base.Clone() as IStiYRadarAxis;
		stiYRadarAxis.LineStyle = LineStyle;
		stiYRadarAxis.Labels = Labels.Clone() as IStiAxisLabels;
		stiYRadarAxis.Ticks = Ticks.Clone() as IStiAxisTicks;
		return stiYRadarAxis;
	}

	public StiYRadarAxis()
	{
		base.Core = new StiYRadarAxisCoreXF(this);
	}

	public StiYRadarAxis(IStiAxisLabels labels, IStiAxisTicks ticks, StiPenStyle lineStyle, Color lineColor, float lineWidth, bool visible, bool allowApplyStyle)
		: base(visible, allowApplyStyle)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		base.Core = new StiYRadarAxisCoreXF(this);
		Labels = labels;
		Ticks = ticks;
		LineStyle = lineStyle;
		LineColor = lineColor;
		LineWidth = lineWidth;
	}

	[StiUniversalConstructor("Axis")]
	public StiYRadarAxis(IStiAxisLabels labels, IStiAxisTicks ticks, StiPenStyle lineStyle, IStiAxisRange range, Color lineColor, float lineWidth, bool visible, bool allowApplyStyle)
		: base(range, visible, allowApplyStyle)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		base.Core = new StiYRadarAxisCoreXF(this);
		Range = range;
		Labels = labels;
		Ticks = ticks;
		LineStyle = lineStyle;
		LineColor = lineColor;
		LineWidth = lineWidth;
	}
}
