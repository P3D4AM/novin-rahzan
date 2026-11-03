using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report.Chart;

public abstract class StiRadarArea : StiArea, IStiRadarArea, IStiArea, ICloneable, IStiJsonReportObject
{
	private IStiInterlacingHor interlacingHor;

	private IStiInterlacingVert interlacingVert;

	private IStiRadarGridLinesHor gridLinesHor;

	private IStiRadarGridLinesVert gridLinesVert;

	private IStiXRadarAxis xAxis;

	private IStiYRadarAxis yAxis;

	[DefaultValue(true)]
	[StiNonSerialized]
	[Browsable(false)]
	public override bool ColorEach
	{
		get
		{
			return base.ColorEach;
		}
		set
		{
			base.ColorEach = value;
		}
	}

	[Description("Gets or sets interlacing settings on horizontal axis.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	[Browsable(false)]
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
	[Browsable(false)]
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
	[Browsable(false)]
	public IStiRadarGridLinesHor GridLinesHor
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

	[Description("Gets or sets grid lines on vertical axis.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	[Browsable(false)]
	public IStiRadarGridLinesVert GridLinesVert
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

	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets style of radar area.")]
	[DefaultValue(StiRadarStyle.Circle)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public StiRadarStyle RadarStyle { get; set; } = StiRadarStyle.Circle;


	[Description("Gets or sets settings of X Axis.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	[Browsable(false)]
	public IStiXRadarAxis XAxis
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

	[Description("Gets or sets settings of Y Axis.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiUniversalConverter))]
	[Browsable(false)]
	public IStiYRadarAxis YAxis
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

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyJObject(val, "InterlacingHor", ((IStiJsonReportObject)interlacingHor).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "InterlacingVert", ((IStiJsonReportObject)interlacingVert).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GridLinesHor", ((IStiJsonReportObject)gridLinesHor).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GridLinesVert", ((IStiJsonReportObject)gridLinesHor).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyEnum(val, "RadarStyle", (Enum)RadarStyle, (Enum)StiRadarStyle.Circle);
		JsonExtensions.AddPropertyJObject(val, "XAxis", ((IStiJsonReportObject)xAxis).SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "YAxis", ((IStiJsonReportObject)yAxis).SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Expected O, but got Unknown
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "InterlacingHor":
				((IStiJsonReportObject)interlacingHor).LoadFromJsonObject((JObject)item.Value);
				break;
			case "InterlacingVert":
				((IStiJsonReportObject)interlacingVert).LoadFromJsonObject((JObject)item.Value);
				break;
			case "GridLinesHor":
				((IStiJsonReportObject)gridLinesHor).LoadFromJsonObject((JObject)item.Value);
				break;
			case "GridLinesVert":
				((IStiJsonReportObject)gridLinesVert).LoadFromJsonObject((JObject)item.Value);
				break;
			case "RadarStyle":
				RadarStyle = StiJsonReportObjectHelper.DeserializeEnum<StiRadarStyle>(item);
				break;
			case "XAxis":
				((IStiJsonReportObject)xAxis).LoadFromJsonObject((JObject)item.Value);
				if (((StiRadarAxis)xAxis).jsonLoadFromJsonObjectArea)
				{
					((StiRadarAxis)xAxis).jsonLoadFromJsonObjectArea = false;
					xAxis.Area = this;
				}
				break;
			case "YAxis":
				((IStiJsonReportObject)yAxis).LoadFromJsonObject((JObject)item.Value);
				if (((StiRadarAxis)yAxis).jsonLoadFromJsonObjectArea)
				{
					((StiRadarAxis)yAxis).jsonLoadFromJsonObjectArea = false;
					yAxis.Area = this;
				}
				break;
			}
		}
	}

	public override object Clone()
	{
		IStiRadarArea stiRadarArea = base.Clone() as IStiRadarArea;
		stiRadarArea.InterlacingHor = interlacingHor.Clone() as IStiInterlacingHor;
		stiRadarArea.InterlacingVert = interlacingVert.Clone() as IStiInterlacingVert;
		stiRadarArea.GridLinesHor = gridLinesHor.Clone() as IStiRadarGridLinesHor;
		stiRadarArea.GridLinesVert = gridLinesVert.Clone() as IStiRadarGridLinesVert;
		stiRadarArea.XAxis = XAxis.Clone() as IStiXRadarAxis;
		stiRadarArea.YAxis = YAxis.Clone() as IStiYRadarAxis;
		return stiRadarArea;
	}

	public override Type GetDefaultSeriesLabelsType()
	{
		return typeof(StiNoneLabels);
	}

	public override Type[] GetSeriesLabelsTypes()
	{
		return new Type[5]
		{
			typeof(StiNoneLabels),
			typeof(StiOutsideBaseAxisLabels),
			typeof(StiOutsideEndAxisLabels),
			typeof(StiOutsideAxisLabels),
			typeof(StiValueAxisLabels)
		};
	}

	public StiRadarArea()
	{
		XAxis = new StiXRadarAxis();
		YAxis = new StiYRadarAxis();
		InterlacingHor = new StiInterlacingHor();
		InterlacingVert = new StiInterlacingVert();
		GridLinesHor = new StiRadarGridLinesHor();
		GridLinesVert = new StiRadarGridLinesVert();
	}
}
