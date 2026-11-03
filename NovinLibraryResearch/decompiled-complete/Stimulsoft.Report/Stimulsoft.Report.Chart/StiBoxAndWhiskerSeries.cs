using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiBoxAndWhiskerSeries : StiSeries, IStiBoxAndWhiskerSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiSeriesBorderThickness, IStiAllowApplyBrush
{
	private StiBrush brush = (StiBrush)new StiSolidBrush(Color.Gainsboro);

	private bool allowApplyBrush = true;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiBoxAndWhiskerSeries;

	[Browsable(false)]
	[StiNonSerialized]
	public override StiArgumentExpression Argument { get; set; } = new StiArgumentExpression();


	[Browsable(false)]
	[StiNonSerialized]
	public override string ArgumentDataColumn { get; set; } = string.Empty;


	[Browsable(false)]
	[StiNonSerialized]
	public override StiListOfArgumentsExpression ListOfArguments { get; set; } = new StiListOfArgumentsExpression();


	[StiNonSerialized]
	[Browsable(false)]
	public override StiChartConditionsCollection Conditions
	{
		get
		{
			return base.Conditions;
		}
		set
		{
			base.Conditions = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override IStiSeriesTopN TopN
	{
		get
		{
			return base.TopN;
		}
		set
		{
			base.TopN = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool TopNAllowed => false;

	[Browsable(false)]
	[StiNonSerialized]
	public override StiTrendLinesCollection TrendLines
	{
		get
		{
			return base.TrendLines;
		}
		set
		{
			base.TrendLines = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool TrendLineAllowed => false;

	[RefreshProperties(RefreshProperties.All)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets brush which will used to fill bar area.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiBrush Brush
	{
		get
		{
			return brush;
		}
		set
		{
			brush = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which allow to use brush from series settings.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool AllowApplyBrush
	{
		get
		{
			return allowApplyBrush;
		}
		set
		{
			if (allowApplyBrush != value)
			{
				allowApplyBrush = value;
			}
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Displays the data points that lie between the lower whisker line and the upper whisker line.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool ShowInnerPoints { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Displays the mean marker of the selected series.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool ShowMeanMarkers { get; set; } = true;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets border thickness of series element.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public int BorderThickness { get; set; } = 1;


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "TopN");
		JsonExtensions.RemoveProperty(val, "TrendLine");
		JsonExtensions.AddPropertyBool(val, "ShowInnerPoints", ShowInnerPoints, false, false);
		JsonExtensions.AddPropertyBool(val, "ShowMeanMarkers", ShowMeanMarkers, true, false);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyInt(val, "BorderThickness", BorderThickness, 1);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ShowInnerPoints":
				ShowInnerPoints = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShowMeanMarkers":
				ShowMeanMarkers = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "BorderThickness":
				BorderThickness = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		return new StiPropertyCollection();
	}

	public override object Clone()
	{
		return base.Clone() as IStiBoxAndWhiskerSeries;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiBoxAndWhiskerArea);
	}

	public override StiSeries CreateNew()
	{
		return new StiBoxAndWhiskerSeries();
	}

	public StiBoxAndWhiskerSeries()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		base.SeriesLabels = new StiNoneLabels();
		base.Core = new StiBoxAndWhiskerSeriesCoreXF(this);
	}
}
