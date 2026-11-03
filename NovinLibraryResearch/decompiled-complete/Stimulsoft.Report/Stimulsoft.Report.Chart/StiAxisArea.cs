using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Chart;

public abstract class StiAxisArea : StiArea, IStiAxisArea, IStiArea, ICloneable, IStiJsonReportObject
{
	private IStiInterlacingHor interlacingHor;

	private IStiInterlacingVert interlacingVert;

	private IStiGridLinesHor gridLinesHor;

	private IStiGridLinesHor gridLinesHorRight;

	private IStiGridLinesVert gridLinesVert;

	private IStiYAxis yAxis;

	private IStiYAxis yRightAxis;

	private IStiXAxis xAxis;

	private IStiXAxis xTopAxis;

	[Browsable(false)]
	public StiAxisAreaCoreXF AxisCore => base.Core as StiAxisAreaCoreXF;

	[Description("Gets or sets interlacing settings on horizontal axis.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public IStiInterlacingHor InterlacingHor
	{
		get
		{
			return interlacingHor;
		}
		set
		{
			interlacingHor = value;
			if (interlacingHor != null)
			{
				interlacingHor.Area = this;
			}
		}
	}

	[Description("Gets or sets interlacing settings on vertical axis.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public IStiInterlacingVert InterlacingVert
	{
		get
		{
			return interlacingVert;
		}
		set
		{
			interlacingVert = value;
			if (interlacingVert != null)
			{
				interlacingVert.Area = this;
			}
		}
	}

	[Description("Gets or sets horizontal grid lines on left axis.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public IStiGridLinesHor GridLinesHor
	{
		get
		{
			return gridLinesHor;
		}
		set
		{
			gridLinesHor = value;
			if (gridLinesHor != null)
			{
				gridLinesHor.Area = this;
			}
		}
	}

	[Description("Gets or sets horizontal grid lines on right axis.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public IStiGridLinesHor GridLinesHorRight
	{
		get
		{
			return gridLinesHorRight;
		}
		set
		{
			gridLinesHorRight = value;
			if (gridLinesHorRight != null)
			{
				gridLinesHorRight.Area = this;
			}
		}
	}

	[Description("Gets or sets grid lines on vertical axis.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public IStiGridLinesVert GridLinesVert
	{
		get
		{
			return gridLinesVert;
		}
		set
		{
			gridLinesVert = value;
			if (gridLinesVert != null)
			{
				gridLinesVert.Area = this;
			}
		}
	}

	[Description("Gets or sets settings of YAxis.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public IStiYAxis YAxis
	{
		get
		{
			return yAxis;
		}
		set
		{
			yAxis = value;
			if (yAxis != null)
			{
				yAxis.Area = this;
			}
		}
	}

	[Description("Gets or sets settings of YRightAxis.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public IStiYAxis YRightAxis
	{
		get
		{
			return yRightAxis;
		}
		set
		{
			yRightAxis = value;
			if (yRightAxis != null)
			{
				yRightAxis.Area = this;
			}
		}
	}

	[Description("Gets or sets settings of XAxis.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public IStiXAxis XAxis
	{
		get
		{
			return xAxis;
		}
		set
		{
			xAxis = value;
			if (xAxis != null)
			{
				xAxis.Area = this;
			}
		}
	}

	[Description("Gets or sets settings of XTopAxis.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	public IStiXAxis XTopAxis
	{
		get
		{
			return xTopAxis;
		}
		set
		{
			xTopAxis = value;
			if (xTopAxis != null)
			{
				xTopAxis.Area = this;
			}
		}
	}

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

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyJObject(val, "InterlacingHor", ((IStiJsonReportObject)interlacingHor).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "InterlacingVert", ((IStiJsonReportObject)interlacingVert).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GridLinesHor", ((IStiJsonReportObject)gridLinesHor).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GridLinesHorRight", ((IStiJsonReportObject)gridLinesHorRight).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GridLinesVert", ((IStiJsonReportObject)gridLinesVert).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "YAxis", ((IStiJsonReportObject)yAxis).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "YRightAxis", ((IStiJsonReportObject)yRightAxis).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "XAxis", ((IStiJsonReportObject)xAxis).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "XTopAxis", ((IStiJsonReportObject)xTopAxis).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyBool(val, "ReverseHor", ReverseHor, false, false);
		JsonExtensions.AddPropertyBool(val, "ReverseVert", ReverseVert, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "InterlacingHor":
				((IStiJsonReportObject)interlacingHor).LoadFromJsonObject((JObject)item.Value);
				if (((StiInterlacing)interlacingHor).needSetAreaJsonPropertyInternal)
				{
					((StiInterlacing)interlacingHor).needSetAreaJsonPropertyInternal = false;
					interlacingHor.Area = this;
				}
				break;
			case "InterlacingVert":
				((IStiJsonReportObject)interlacingVert).LoadFromJsonObject((JObject)item.Value);
				if (((StiInterlacing)interlacingVert).needSetAreaJsonPropertyInternal)
				{
					((StiInterlacing)interlacingVert).needSetAreaJsonPropertyInternal = false;
					interlacingVert.Area = this;
				}
				break;
			case "GridLinesHor":
				((IStiJsonReportObject)gridLinesHor).LoadFromJsonObject((JObject)item.Value);
				if (((StiGridLines)gridLinesHor).needSetAreaJsonPropertyInternal)
				{
					((StiGridLines)gridLinesHor).needSetAreaJsonPropertyInternal = false;
					gridLinesHor.Area = this;
				}
				break;
			case "GridLinesHorRight":
				((IStiJsonReportObject)gridLinesHorRight).LoadFromJsonObject((JObject)item.Value);
				if (((StiGridLines)gridLinesHorRight).needSetAreaJsonPropertyInternal)
				{
					((StiGridLines)gridLinesHorRight).needSetAreaJsonPropertyInternal = false;
					gridLinesHorRight.Area = this;
				}
				break;
			case "GridLinesVert":
				((IStiJsonReportObject)gridLinesVert).LoadFromJsonObject((JObject)item.Value);
				if (((StiGridLines)gridLinesVert).needSetAreaJsonPropertyInternal)
				{
					((StiGridLines)gridLinesVert).needSetAreaJsonPropertyInternal = false;
					gridLinesVert.Area = this;
				}
				break;
			case "YAxis":
				((IStiJsonReportObject)yAxis).LoadFromJsonObject((JObject)item.Value);
				break;
			case "YRightAxis":
				((IStiJsonReportObject)yRightAxis).LoadFromJsonObject((JObject)item.Value);
				break;
			case "XAxis":
				((IStiJsonReportObject)xAxis).LoadFromJsonObject((JObject)item.Value);
				break;
			case "XTopAxis":
				((IStiJsonReportObject)xTopAxis).LoadFromJsonObject((JObject)item.Value);
				break;
			case "ReverseHor":
				ReverseHor = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ReverseVert":
				ReverseVert = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public override object Clone()
	{
		IStiAxisArea stiAxisArea = base.Clone() as IStiAxisArea;
		stiAxisArea.InterlacingHor = interlacingHor.Clone() as IStiInterlacingHor;
		stiAxisArea.InterlacingVert = interlacingVert.Clone() as IStiInterlacingVert;
		stiAxisArea.GridLinesHor = gridLinesHor.Clone() as IStiGridLinesHor;
		stiAxisArea.GridLinesHorRight = gridLinesHorRight.Clone() as IStiGridLinesHor;
		stiAxisArea.GridLinesVert = gridLinesVert.Clone() as IStiGridLinesVert;
		stiAxisArea.YAxis = yAxis.Clone() as IStiYAxis;
		stiAxisArea.YRightAxis = yRightAxis.Clone() as IStiYAxis;
		stiAxisArea.XAxis = xAxis.Clone() as IStiXAxis;
		stiAxisArea.XTopAxis = xTopAxis.Clone() as IStiXAxis;
		return stiAxisArea;
	}

	private bool ShouldSerializeInterlacingHor()
	{
		if (InterlacingHor != null)
		{
			return !((IStiDefault)InterlacingHor).IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeInterlacingVert()
	{
		if (InterlacingVert != null)
		{
			return !((IStiDefault)InterlacingVert).IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeGridLinesHor()
	{
		if (GridLinesHor != null)
		{
			return !((IStiDefault)GridLinesHor).IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeGridLinesHorRight()
	{
		if (GridLinesHorRight != null)
		{
			return !((IStiDefault)GridLinesHorRight).IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeGridLinesVert()
	{
		if (GridLinesVert != null)
		{
			return !((IStiDefault)GridLinesVert).IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeYAxis()
	{
		if (YAxis != null)
		{
			return !((IStiDefault)YAxis).IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeYRightAxis()
	{
		if (YRightAxis != null)
		{
			return !((IStiDefault)YRightAxis).IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeXAxis()
	{
		if (XAxis != null)
		{
			return !((IStiDefault)XAxis).IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeXTopAxis()
	{
		if (XTopAxis != null)
		{
			return !((IStiDefault)XTopAxis).IsDefault;
		}
		return true;
	}

	public override Type GetDefaultSeriesLabelsType()
	{
		return typeof(StiCenterAxisLabels);
	}

	public override Type[] GetSeriesLabelsTypes()
	{
		return new Type[10]
		{
			typeof(StiNoneLabels),
			typeof(StiInsideBaseAxisLabels),
			typeof(StiInsideEndAxisLabels),
			typeof(StiCenterAxisLabels),
			typeof(StiOutsideBaseAxisLabels),
			typeof(StiOutsideEndAxisLabels),
			typeof(StiOutsideAxisLabels),
			typeof(StiLeftAxisLabels),
			typeof(StiValueAxisLabels),
			typeof(StiRightAxisLabels)
		};
	}

	public StiAxisArea()
	{
		InterlacingHor = new StiInterlacingHor();
		InterlacingVert = new StiInterlacingVert();
		GridLinesHor = new StiGridLinesHor();
		GridLinesHorRight = new StiGridLinesHor();
		GridLinesVert = new StiGridLinesVert();
		GridLinesHorRight.Visible = false;
		XAxis = new StiXBottomAxis();
		YAxis = new StiYLeftAxis();
		XTopAxis = new StiXTopAxis();
		YRightAxis = new StiYRightAxis();
	}
}
