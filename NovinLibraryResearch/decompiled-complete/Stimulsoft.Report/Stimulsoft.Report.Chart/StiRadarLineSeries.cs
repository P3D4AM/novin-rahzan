using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiRadarLineSeries : StiRadarSeries, IStiRadarLineSeries, IStiRadarSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiShowNullsSeries
{
	private Color lineColor = Color.Black;

	private StiPenStyle lineStyle;

	private bool lighting = true;

	private float lineWidth = 2f;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiRadarLineSeries;

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets line color of series.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color LineColor
	{
		get
		{
			return lineColor;
		}
		set
		{
			lineColor = value;
		}
	}

	[Editor("Stimulsoft.Base.Drawing.Design.StiPenStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a line style of series.")]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiPenStyle LineStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return lineStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			lineStyle = value;
		}
	}

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that light effect will be shown.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool Lighting
	{
		get
		{
			return lighting;
		}
		set
		{
			lighting = value;
		}
	}

	[DefaultValue(2f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets line width of series.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float LineWidth
	{
		get
		{
			return lineWidth;
		}
		set
		{
			if (value > 0f)
			{
				lineWidth = value;
			}
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyColor(val, "LineColor", LineColor, Color.Black);
		JsonExtensions.AddPropertyEnum(val, "LineStyle", (Enum)(object)LineStyle, (Enum)(object)(StiPenStyle)0);
		JsonExtensions.AddPropertyBool(val, "Lighting", Lighting, true, false);
		JsonExtensions.AddPropertyFloat(val, "LineWidth", LineWidth, 2f);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "LineColor":
				LineColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "LineStyle":
				LineStyle = StiJsonReportObjectHelper.DeserializeEnum<StiPenStyle>(item);
				break;
			case "Lighting":
				Lighting = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "LineWidth":
				LineWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[3]
		{
			propertiesHelper.ValueDataColumn(),
			propertiesHelper.Value(),
			propertiesHelper.ListOfValues()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Value, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.ArgumentDataColumn(),
			propertiesHelper.Argument(),
			propertiesHelper.ListOfArguments()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Argument, value);
		value = new StiPropertyObject[6]
		{
			propertiesHelper.Format(),
			propertiesHelper.SortBy(),
			propertiesHelper.SortDirection(),
			propertiesHelper.AutoSeriesKeyDataColumn(),
			propertiesHelper.AutoSeriesColorDataColumn(),
			propertiesHelper.AutoSeriesTitleDataColumn()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		value = new StiPropertyObject[5]
		{
			propertiesHelper.Lighting(),
			propertiesHelper.LineColor(),
			propertiesHelper.LineStyle(),
			propertiesHelper.LineWidth(),
			propertiesHelper.ShowShadow()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[6]
		{
			propertiesHelper.AllowApplyStyle(),
			propertiesHelper.ShowInLegend(),
			propertiesHelper.ShowNulls(),
			propertiesHelper.ShowSeriesLabels(),
			propertiesHelper.Title(),
			propertiesHelper.YAxis()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		return stiPropertyCollection;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiRadarLineArea);
	}

	public override StiSeries CreateNew()
	{
		return new StiRadarLineSeries();
	}

	public StiRadarLineSeries()
	{
		base.Core = new StiRadarLineSeriesCoreXF(this);
	}
}
