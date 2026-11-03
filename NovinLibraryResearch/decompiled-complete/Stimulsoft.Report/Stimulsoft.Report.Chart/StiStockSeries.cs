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

public class StiStockSeries : StiCandlestickSeries, IStiStockSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiFinancialSeries, IStiAllowApplyColorNegative
{
	private Color lineColor = Color.Black;

	private StiPenStyle lineStyle;

	private float lineWidth = 2f;

	private Color lineColorNegative = Color.Firebrick;

	private bool allowApplyColorNegative;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiStockSeries;

	[Browsable(false)]
	[StiNonSerialized]
	public override Color BorderColor
	{
		get
		{
			return base.BorderColor;
		}
		set
		{
			base.BorderColor = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override float BorderWidth
	{
		get
		{
			return base.BorderWidth;
		}
		set
		{
			if (value > 0f)
			{
				base.BorderWidth = value;
			}
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override StiBrush Brush
	{
		get
		{
			return base.Brush;
		}
		set
		{
			base.Brush = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override StiBrush BrushNegative
	{
		get
		{
			return base.BrushNegative;
		}
		set
		{
			base.BrushNegative = value;
		}
	}

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

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a line color of series for negative values.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color LineColorNegative
	{
		get
		{
			return lineColorNegative;
		}
		set
		{
			lineColorNegative = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates that the specific color for negative values will be used.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool AllowApplyColorNegative
	{
		get
		{
			return allowApplyColorNegative;
		}
		set
		{
			allowApplyColorNegative = value;
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "BorderColor");
		JsonExtensions.RemoveProperty(val, "BorderWidth");
		JsonExtensions.RemoveProperty(val, "Brush");
		JsonExtensions.RemoveProperty(val, "BrushNegative");
		JsonExtensions.AddPropertyColor(val, "LineColor", LineColor, Color.Black);
		JsonExtensions.AddPropertyEnum(val, "LineStyle", (Enum)(object)LineStyle, (Enum)(object)(StiPenStyle)0);
		JsonExtensions.AddPropertyFloat(val, "LineWidth", LineWidth, 2f);
		JsonExtensions.AddPropertyColor(val, "LineColorNegative", LineColorNegative, Color.Firebrick);
		JsonExtensions.AddPropertyBool(val, "AllowApplyColorNegative", AllowApplyColorNegative, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
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
			case "LineWidth":
				LineWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "LineColorNegative":
				LineColorNegative = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "AllowApplyColorNegative":
				AllowApplyColorNegative = StiJsonReportObjectHelper.DeserializeBool(item);
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
			propertiesHelper.ValueDataColumnOpen(),
			propertiesHelper.ValueOpen(),
			propertiesHelper.ListOfValuesOpen()
		};
		stiPropertyCollection.Add(StiPropertyCategories.ValueOpen, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.ValueDataColumnClose(),
			propertiesHelper.ValueClose(),
			propertiesHelper.ListOfValuesClose()
		};
		stiPropertyCollection.Add(StiPropertyCategories.ValueClose, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.ValueDataColumnHigh(),
			propertiesHelper.ValueHigh(),
			propertiesHelper.ListOfValuesHigh()
		};
		stiPropertyCollection.Add(StiPropertyCategories.ValueHigh, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.ValueDataColumnLow(),
			propertiesHelper.ValueLow(),
			propertiesHelper.ListOfValuesLow()
		};
		stiPropertyCollection.Add(StiPropertyCategories.ValueLow, value);
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
			propertiesHelper.LineColor(),
			propertiesHelper.LineColorNegative(),
			propertiesHelper.LineStyle(),
			propertiesHelper.LineWidth(),
			propertiesHelper.ShowShadow()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[6]
		{
			propertiesHelper.AllowApplyStyle(),
			propertiesHelper.AllowApplyColorNegative(),
			propertiesHelper.ShowInLegend(),
			propertiesHelper.ShowSeriesLabels(),
			propertiesHelper.Title(),
			propertiesHelper.YAxis()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		return stiPropertyCollection;
	}

	public override object Clone()
	{
		return base.Clone() as IStiStockSeries;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiStockArea);
	}

	public override StiSeries CreateNew()
	{
		return new StiStockSeries();
	}

	public StiStockSeries()
	{
		base.Core = new StiStockSeriesCoreXF(this);
	}
}
