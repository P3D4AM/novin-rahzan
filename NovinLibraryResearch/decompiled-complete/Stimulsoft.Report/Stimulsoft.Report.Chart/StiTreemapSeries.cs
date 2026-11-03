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
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiTreemapSeries : StiSeries, IStiTreemapSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiSeriesBorderThickness, IStiFontIconsSeries
{
	private StiSeriesSortType sortBy;

	private StiSeriesSortDirection sortDirection = StiSeriesSortDirection.Descending;

	private Color borderColor = Color.Gray;

	private StiBrush brush = (StiBrush)new StiSolidBrush(Color.Gainsboro);

	public override StiComponentId ComponentId => StiComponentId.StiTreemapSeries;

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

	[StiNonSerialized]
	[Browsable(false)]
	public override StiSeriesYAxis YAxis
	{
		get
		{
			return base.YAxis;
		}
		set
		{
			base.YAxis = value;
		}
	}

	[DefaultValue(StiSeriesSortType.Value)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets mode of series values sorting.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override StiSeriesSortType SortBy
	{
		get
		{
			return sortBy;
		}
		set
		{
			sortBy = value;
		}
	}

	[DefaultValue(StiSeriesSortDirection.Descending)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets sort direction.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override StiSeriesSortDirection SortDirection
	{
		get
		{
			return sortDirection;
		}
		set
		{
			sortDirection = value;
		}
	}

	[StiSerializable]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets border color of series box.")]
	[TypeConverter(typeof(StiColorConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public Color BorderColor
	{
		get
		{
			return borderColor;
		}
		set
		{
			borderColor = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets border thickness of series element.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public int BorderThickness { get; set; } = 1;


	[RefreshProperties(RefreshProperties.All)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets brush which will used to fill treemap box.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBrush Brush
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

	[DefaultValue(null)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Design.StiFontIconEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiFontIconConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiFontIcons? Icon { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "SortBy");
		JsonExtensions.AddPropertyEnum(val, "SortBy", (Enum)SortBy, (Enum)StiSeriesSortType.Value);
		JsonExtensions.AddPropertyColor(val, "BorderColor", borderColor, Color.Gray);
		JsonExtensions.AddPropertyInt(val, "BorderThickness", BorderThickness, 1);
		JsonExtensions.AddPropertyBrush(val, "Brush", brush);
		if (Icon.HasValue)
		{
			JsonExtensions.AddPropertyEnum(val, "Icon", (Enum)(object)Icon);
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "BorderColor":
				borderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BorderThickness":
				BorderThickness = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Brush":
				brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Icon":
				Icon = StiJsonReportObjectHelper.DeserializeEnum<StiFontIcons>(item);
				break;
			}
		}
	}

	public override object Clone()
	{
		IStiTreemapSeries stiTreemapSeries = base.Clone() as IStiTreemapSeries;
		object obj = Brush.Clone();
		stiTreemapSeries.Brush = (StiBrush)((obj is StiBrush) ? obj : null);
		return stiTreemapSeries;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiTreemapArea);
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
		value = new StiPropertyObject[3]
		{
			propertiesHelper.BorderColor(),
			propertiesHelper.Brush(),
			propertiesHelper.ShowShadow()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[5]
		{
			propertiesHelper.AllowApplyStyle(),
			propertiesHelper.ShowInLegend(),
			propertiesHelper.ShowSeriesLabels(),
			propertiesHelper.Title(),
			propertiesHelper.fWidth()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		return stiPropertyCollection;
	}

	public override StiSeries CreateNew()
	{
		return new StiTreemapSeries();
	}

	public StiTreemapSeries()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		base.Core = new StiTreemapSeriesCoreXF(this);
	}
}
