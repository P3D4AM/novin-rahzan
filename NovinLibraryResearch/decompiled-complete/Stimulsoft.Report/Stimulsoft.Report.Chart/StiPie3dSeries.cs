using System;
using System.ComponentModel;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Chart.Design;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiPie3dSeries : StiPieSeries, IStiPie3dSeries, IStiPieSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiFontIconsSeries, IStiSeriesBorderThickness, IStiAllowApplyBorderColor, IStiAllowApplyBrush, IStiShowZerosSeries
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiPie3dSeries;

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(Sti3dOptionsConverter))]
	[StiOrder(380)]
	public StiPie3dOptions Options3D { get; set; } = new StiPie3dOptions();


	[Browsable(false)]
	[StiNonSerialized]
	public override int BorderThickness
	{
		get
		{
			return base.BorderThickness;
		}
		set
		{
			base.BorderThickness = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiCutPieListExpression CutPieList
	{
		get
		{
			return base.CutPieList;
		}
		set
		{
			base.CutPieList = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override float Diameter
	{
		get
		{
			return base.Diameter;
		}
		set
		{
			base.Diameter = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool Lighting
	{
		get
		{
			return base.Lighting;
		}
		set
		{
			base.Lighting = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override float Distance
	{
		get
		{
			return base.Distance;
		}
		set
		{
			base.Distance = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiFontIcons? Icon
	{
		get
		{
			return base.Icon;
		}
		set
		{
			base.Icon = value;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyJObject(val, "Options3D", Options3D.SaveToJsonObject(mode));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string name = item.Name;
			if (name == "Options3D")
			{
				Options3D.LoadFromJsonObject((JObject)item.Value);
			}
		}
	}

	public override object Clone()
	{
		return base.Clone() as IStiPie3dSeries;
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
		value = new StiPropertyObject[7]
		{
			propertiesHelper.FilterMode(),
			propertiesHelper.Filters(),
			propertiesHelper.Format(),
			propertiesHelper.ShowZeros(),
			propertiesHelper.SortBy(),
			propertiesHelper.SortDirection(),
			propertiesHelper.ChartTopN()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Data, value);
		value = new StiPropertyObject[4]
		{
			propertiesHelper.AllowApplyBorderColor(),
			propertiesHelper.AllowApplyBrush(),
			propertiesHelper.AllowApplyStyle(),
			propertiesHelper.ChartOptions3D()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[5]
		{
			propertiesHelper.ShowInLegend(),
			propertiesHelper.ShowSeriesLabels(),
			propertiesHelper.StartAngle(),
			propertiesHelper.Title(),
			propertiesHelper.YAxis()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Common, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.AutoSeriesKeyDataColumn(),
			propertiesHelper.AutoSeriesColorDataColumn(),
			propertiesHelper.AutoSeriesTitleDataColumn()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Series, value);
		return stiPropertyCollection;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiPie3dArea);
	}

	public override StiSeries CreateNew()
	{
		return new StiPie3dSeries();
	}

	private bool ShouldSerializeOptions3d()
	{
		if (Options3D != null)
		{
			return !Options3D.IsDefault;
		}
		return true;
	}

	public StiPie3dSeries()
	{
		base.Core = new StiPie3dSeriesCoreXF(this);
	}
}
