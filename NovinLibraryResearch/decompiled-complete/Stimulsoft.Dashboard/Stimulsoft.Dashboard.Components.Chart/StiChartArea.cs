using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Dashboard.Components.Chart.Design;
using Stimulsoft.Report.Dashboard;

namespace Stimulsoft.Dashboard.Components.Chart;

[TypeConverter(typeof(StiChartAreaConverter))]
public class StiChartArea : IStiJsonReportObject, IStiChartArea
{
	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool ColorEach { get; set; }

	[Description("Gets or sets value which indicate that all values on horizontal axis is reverse.")]
	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool ReverseHor { get; set; }

	[Description("Gets or sets value which indicate that all values on vertical axis is reverse.")]
	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	public virtual bool ReverseVert { get; set; }

	[Description("Gets or sets interlacing settings on horizontal axis.")]
	[StiSerializable(StiSerializationVisibility.Class)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public StiHorChartInterlacing InterlacingHor { get; set; } = new StiHorChartInterlacing();


	[Description("Gets or sets interlacing settings on vertical axis.")]
	[StiSerializable(StiSerializationVisibility.Class)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public StiVertChartInterlacing InterlacingVert { get; set; } = new StiVertChartInterlacing();


	[Description("Gets or sets horizontal grid lines on left axis.")]
	[StiSerializable(StiSerializationVisibility.Class)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public StiHorChartGridLines GridLinesHor { get; set; } = new StiHorChartGridLines();


	[Description("Gets or sets grid lines on vertical axis.")]
	[StiSerializable(StiSerializationVisibility.Class)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public StiVertChartGridLines GridLinesVert { get; set; } = new StiVertChartGridLines();


	[Description("Gets or sets settings of XAxis.")]
	[StiSerializable(StiSerializationVisibility.Class)]
	[StiPropertyLevel(StiLevel.Standard)]
	public StiXChartAxis XAxis { get; set; } = new StiXChartAxis();


	[Description("Gets or sets settings of XTopAxis.")]
	[StiSerializable(StiSerializationVisibility.Class)]
	[StiPropertyLevel(StiLevel.Standard)]
	public StiXTopChartAxis XTopAxis { get; set; } = new StiXTopChartAxis();


	[Description("Gets or sets settings of YAxis.")]
	[StiSerializable(StiSerializationVisibility.Class)]
	[StiPropertyLevel(StiLevel.Standard)]
	public StiYChartAxis YAxis { get; set; } = new StiYChartAxis();


	[Description("Gets or sets settings of YRightAxis.")]
	[StiSerializable(StiSerializationVisibility.Class)]
	[StiPropertyLevel(StiLevel.Standard)]
	public StiYRightChartAxis YRightAxis { get; set; } = new StiYRightChartAxis();


	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			if (ColorEach && ShouldSerializeInterlacingHor() && ShouldSerializeInterlacingVert() && ShouldSerializeGridLinesHor() && ShouldSerializeGridLinesVert() && ShouldSerializeXAxis() && ShouldSerializeYAxis() && ShouldSerializeXTopAxis() && ShouldSerializeYRightAxis() && !ReverseHor)
			{
				return ReverseVert;
			}
			return true;
		}
	}

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = new JObject();
		jObject.AddPropertyBool("ColorEach", ColorEach);
		jObject.AddPropertyJObject("GridLinesHor", GridLinesHor.SaveToJsonObject(mode));
		jObject.AddPropertyJObject("GridLinesVert", GridLinesVert.SaveToJsonObject(mode));
		jObject.AddPropertyJObject("InterlacingHor", InterlacingHor.SaveToJsonObject(mode));
		jObject.AddPropertyJObject("InterlacingVert", InterlacingVert.SaveToJsonObject(mode));
		jObject.AddPropertyBool("ReverseHor", ReverseHor);
		jObject.AddPropertyBool("ReverseVert", ReverseVert);
		return jObject;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ColorEach":
				ColorEach = item.DeserializeBool();
				break;
			case "GridLinesHor":
				GridLinesHor.LoadFromJsonObject((JObject)item.Value);
				break;
			case "GridLinesVert":
				GridLinesVert.LoadFromJsonObject((JObject)item.Value);
				break;
			case "InterlacingHor":
				InterlacingHor.LoadFromJsonObject((JObject)item.Value);
				break;
			case "InterlacingVert":
				InterlacingVert.LoadFromJsonObject((JObject)item.Value);
				break;
			case "ReverseHor":
				ReverseHor = item.DeserializeBool();
				break;
			case "ReverseVert":
				ReverseVert = item.DeserializeBool();
				break;
			}
		}
	}

	public object Clone()
	{
		StiChartArea stiChartArea = MemberwiseClone() as StiChartArea;
		stiChartArea.GridLinesHor = GridLinesHor?.Clone() as StiHorChartGridLines;
		stiChartArea.GridLinesVert = GridLinesVert?.Clone() as StiVertChartGridLines;
		stiChartArea.InterlacingHor = InterlacingHor?.Clone() as StiHorChartInterlacing;
		stiChartArea.InterlacingVert = InterlacingVert?.Clone() as StiVertChartInterlacing;
		return stiChartArea;
	}

	private bool ShouldSerializeInterlacingHor()
	{
		if (InterlacingHor != null)
		{
			return !InterlacingHor.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeInterlacingVert()
	{
		if (InterlacingVert != null)
		{
			return !InterlacingVert.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeGridLinesHor()
	{
		if (GridLinesHor != null)
		{
			return !GridLinesHor.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeGridLinesVert()
	{
		if (GridLinesVert != null)
		{
			return !GridLinesVert.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeXAxis()
	{
		if (XAxis != null)
		{
			return !XAxis.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeXTopAxis()
	{
		if (XTopAxis != null)
		{
			return !XTopAxis.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeYAxis()
	{
		if (YAxis != null)
		{
			return !YAxis.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeYRightAxis()
	{
		if (YRightAxis != null)
		{
			return !YRightAxis.IsDefault;
		}
		return true;
	}

	public StiChartArea()
	{
	}

	public StiChartArea(bool colorEach)
	{
		ColorEach = colorEach;
	}

	public StiChartArea(bool colorEach, bool reverseHor, bool reverseVert, StiHorChartGridLines gridLinesHor, StiVertChartGridLines gridLinesVert, StiHorChartInterlacing interlacingHor, StiVertChartInterlacing interlacingVert)
	{
		ColorEach = colorEach;
		ReverseHor = reverseHor;
		ReverseVert = reverseVert;
		GridLinesHor = gridLinesHor;
		GridLinesVert = gridLinesVert;
		InterlacingHor = interlacingHor;
		InterlacingVert = interlacingVert;
	}

	[StiUniversalConstructor("Area")]
	public StiChartArea(bool colorEach, bool reverseHor, bool reverseVert, StiHorChartGridLines gridLinesHor, StiVertChartGridLines gridLinesVert, StiHorChartInterlacing interlacingHor, StiVertChartInterlacing interlacingVert, StiXChartAxis xAxis, StiXTopChartAxis xTopAxis, StiYChartAxis yAxis, StiYRightChartAxis yRightAxis)
	{
		ColorEach = colorEach;
		ReverseHor = reverseHor;
		ReverseVert = reverseVert;
		GridLinesHor = gridLinesHor;
		GridLinesVert = gridLinesVert;
		InterlacingHor = interlacingHor;
		InterlacingVert = interlacingVert;
		XAxis = xAxis;
		YAxis = yAxis;
		XTopAxis = xTopAxis;
		YRightAxis = yRightAxis;
	}
}
