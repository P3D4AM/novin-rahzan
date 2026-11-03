using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiPictorialSeries : StiSeries, IStiPictorialSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiFontIconsSeries
{
	private StiBrush brush = (StiBrush)new StiSolidBrush(Color.Gainsboro);

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

	[RefreshProperties(RefreshProperties.All)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets brush which will used to fill pictorial icon.")]
	[Browsable(false)]
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

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Design.StiFontIconEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiFontIconConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiFontIcons? Icon { get; set; } = (StiFontIcons)5;


	public override StiComponentId ComponentId => StiComponentId.StiPictorialSeries;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "Icon", (Enum)(object)Icon, (Enum)(object)(StiFontIcons)5);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "Icon")
			{
				Icon = StiJsonReportObjectHelper.DeserializeEnum<StiFontIcons>(item);
			}
		}
	}

	public override object Clone()
	{
		IStiPictorialSeries stiPictorialSeries = base.Clone() as IStiPictorialSeries;
		object obj = Brush.Clone();
		stiPictorialSeries.Brush = (StiBrush)((obj is StiBrush) ? obj : null);
		return stiPictorialSeries;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiPictorialArea);
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
		value = new StiPropertyObject[3]
		{
			propertiesHelper.Format(),
			propertiesHelper.SortBy(),
			propertiesHelper.SortDirection()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		value = new StiPropertyObject[4]
		{
			propertiesHelper.AllowApplyStyle(),
			propertiesHelper.Icon(),
			propertiesHelper.ShowInLegend(),
			propertiesHelper.Title()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		return stiPropertyCollection;
	}

	public override StiSeries CreateNew()
	{
		return new StiPictorialSeries();
	}

	public StiPictorialSeries()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		base.Core = new StiPictorialSeriesCoreXF(this);
	}
}
