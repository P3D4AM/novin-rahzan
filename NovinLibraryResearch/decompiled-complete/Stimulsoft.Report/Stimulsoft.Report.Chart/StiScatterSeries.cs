using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiScatterSeries : StiBaseLineSeries, IStiScatterSeries, IStiBaseLineSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiAllowApplyColorNegative, IStiShowNullsSeries, IStiShowZerosSeries
{
	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiScatterSeries;

	[StiNonSerialized]
	[Browsable(false)]
	public override Color LineColorNegative
	{
		get
		{
			return base.LineColorNegative;
		}
		set
		{
			base.LineColorNegative = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool AllowApplyColorNegative
	{
		get
		{
			return base.AllowApplyColorNegative;
		}
		set
		{
			base.AllowApplyColorNegative = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override Color LineColor
	{
		get
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			if (((object)this).GetType() == typeof(StiScatterSeries))
			{
				if (Marker.Brush is StiSolidBrush)
				{
					return ((StiSolidBrush)Marker.Brush).Color;
				}
				if (Marker.Brush is StiGradientBrush)
				{
					return ((StiGradientBrush)Marker.Brush).StartColor;
				}
			}
			return base.LineColor;
		}
		set
		{
			base.LineColor = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiPenStyle LineStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.LineStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.LineStyle = value;
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
	public override float LineWidth
	{
		get
		{
			return base.LineWidth;
		}
		set
		{
			base.LineWidth = value;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "LineColorNegative");
		JsonExtensions.RemoveProperty(val, "AllowApplyColorNegative");
		JsonExtensions.RemoveProperty(val, "LineColor");
		JsonExtensions.RemoveProperty(val, "LineStyle");
		JsonExtensions.RemoveProperty(val, "Lighting");
		JsonExtensions.RemoveProperty(val, "LineWidth");
		return val;
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
		value = new StiPropertyObject[2]
		{
			propertiesHelper.LabelsOffset(),
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

	public override object Clone()
	{
		return base.Clone() as IStiScatterSeries;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiScatterArea);
	}

	public override StiSeries CreateNew()
	{
		return new StiScatterSeries();
	}

	public StiScatterSeries()
	{
		base.Core = new StiScatterSeriesCoreXF(this);
	}
}
