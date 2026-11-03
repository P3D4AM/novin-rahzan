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
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiPieSeries : StiSeries, IStiPieSeries, IStiSeries, ICloneable, IStiJsonReportObject, IStiFontIconsSeries, IStiSeriesBorderThickness, IStiAllowApplyBorderColor, IStiAllowApplyBrush, IStiShowZerosSeries
{
	private bool showZeros;

	private bool allowApplyBrush = true;

	private bool allowApplyBorderColor = true;

	private float startAngle;

	private Color borderColor = Color.Gray;

	private StiBrush brush = (StiBrush)new StiSolidBrush(Color.Gainsboro);

	private bool lighting = true;

	private float diameter;

	private float distance;

	private double[] cutPieListValues = new double[0];

	private StiCutPieListExpression cutPieList = new StiCutPieListExpression();

	private StiGetCutPieListEvent getCutPieListEvent = new StiGetCutPieListEvent();

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiPieSeries;

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

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	[Description("Gets or sets value which indicates whether it is necessary to show the series element, if the series value of this column is 0.")]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool ShowZeros
	{
		get
		{
			return showZeros;
		}
		set
		{
			showZeros = value;
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
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which allow to use border color from series settings.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool AllowApplyBorderColor
	{
		get
		{
			return allowApplyBorderColor;
		}
		set
		{
			if (allowApplyBorderColor != value)
			{
				allowApplyBorderColor = value;
			}
		}
	}

	public override object[] Arguments
	{
		get
		{
			if (base.Arguments.Length != 0 || base.Chart == null || ((StiChart)base.Chart).Report == null || !((StiChart)base.Chart).Report.IsDesigning || base.IsDashboard)
			{
				return base.Arguments;
			}
			return new object[3] { "Arg1", "Arg2", "Arg3" };
		}
		set
		{
			base.Arguments = value;
		}
	}

	[StiSerializable]
	[DefaultValue(0f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets start rotation angle of series.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float StartAngle
	{
		get
		{
			return startAngle;
		}
		set
		{
			startAngle = value;
		}
	}

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
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
	[Description("Gets or sets border thickness of series pie.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual int BorderThickness { get; set; } = 1;


	[RefreshProperties(RefreshProperties.All)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
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

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
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

	[StiSerializable]
	[DefaultValue(0f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets fixed size of diameter of pie series.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float Diameter
	{
		get
		{
			return diameter;
		}
		set
		{
			diameter = value;
			if (value < 5f)
			{
				diameter = 0f;
			}
		}
	}

	[StiSerializable]
	[DefaultValue(0f)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets distance between the center of series and the center of each segment.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual float Distance
	{
		get
		{
			return distance;
		}
		set
		{
			distance = Math.Max(0f, value);
		}
	}

	[Browsable(false)]
	public double[] CutPieListValues
	{
		get
		{
			if (base.Chart == null || ((StiChart)base.Chart).Report == null || !base.Chart.IsDesigning || base.IsDashboard)
			{
				return cutPieListValues;
			}
			if (!string.IsNullOrEmpty(CutPieList.Value))
			{
				return StiSeries.GetValuesFromString(CutPieList.Value);
			}
			return new double[0];
		}
		set
		{
			cutPieListValues = value;
		}
	}

	[DefaultValue(null)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Design.StiFontIconEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiFontIconConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiFontIcons? Icon { get; set; }

	[Browsable(false)]
	[StiNonSerialized]
	[Obsolete("This property is obsolete, use the CutPieList property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiCutPieListExpression CuttedPieList
	{
		get
		{
			return CutPieList;
		}
		set
		{
			CutPieList = value;
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a list of cut pie segments. Example: 1;4;6")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiCutPieListExpression CutPieList
	{
		get
		{
			return cutPieList;
		}
		set
		{
			cutPieList = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the cut pie list.")]
	public StiGetCutPieListEvent GetCutPieListEvent
	{
		get
		{
			return getCutPieListEvent;
		}
		set
		{
			getCutPieListEvent = value;
		}
	}

	public event StiGetValueEventHandler GetCutPieList;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "TrendLine");
		JsonExtensions.AddPropertyBool(val, "ShowZeros", ShowZeros, false, false);
		JsonExtensions.AddPropertyBool(val, "AllowApplyBrush", allowApplyBrush, true, false);
		JsonExtensions.AddPropertyBool(val, "AllowApplyBorderColor", allowApplyBorderColor, true, false);
		JsonExtensions.AddPropertyFloat(val, "StartAngle", StartAngle, 0f);
		JsonExtensions.AddPropertyColor(val, "BorderColor", borderColor, Color.Gray);
		JsonExtensions.AddPropertyInt(val, "BorderThickness", BorderThickness, 1);
		JsonExtensions.AddPropertyBrush(val, "Brush", brush);
		JsonExtensions.AddPropertyBool(val, "Lighting", lighting, true, false);
		JsonExtensions.AddPropertyFloat(val, "Diameter", Diameter, 0f);
		JsonExtensions.AddPropertyFloat(val, "Distance", Distance, 0f);
		JsonExtensions.AddPropertyJObject(val, "CutPieList", cutPieList.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetCutPieListEvent", getCutPieListEvent.SaveToJsonObject(mode));
		if (Icon.HasValue)
		{
			JsonExtensions.AddPropertyEnum(val, "Icon", (Enum)(object)Icon);
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "ShowZeros":
				ShowZeros = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowApplyBrush":
				allowApplyBrush = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowApplyBorderColor":
				allowApplyBorderColor = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "StartAngle":
				StartAngle = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "BorderColor":
				borderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BorderThickness":
				BorderThickness = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "Brush":
				brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Lighting":
				lighting = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Diameter":
				Diameter = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Distance":
				Distance = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "CutPieList":
				cutPieList.LoadFromJsonObject((JObject)item.Value);
				break;
			case "GetCutPieListEvent":
				getCutPieListEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Icon":
				Icon = StiJsonReportObjectHelper.DeserializeEnum<StiFontIcons>(item);
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
		value = new StiPropertyObject[7]
		{
			propertiesHelper.AllowApplyBorderColor(),
			propertiesHelper.AllowApplyBrush(),
			propertiesHelper.Diameter(),
			propertiesHelper.BorderColor(),
			propertiesHelper.Brush(),
			propertiesHelper.Lighting(),
			propertiesHelper.ShowShadow()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, value);
		value = new StiPropertyObject[9]
		{
			propertiesHelper.AllowApplyStyle(),
			propertiesHelper.CutPieList(),
			propertiesHelper.Distance(),
			propertiesHelper.ShowInLegend(),
			propertiesHelper.ShowSeriesLabels(),
			propertiesHelper.ShowZeros(),
			propertiesHelper.StartAngle(),
			propertiesHelper.Title(),
			propertiesHelper.YAxis()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		return stiPropertyCollection;
	}

	public override object Clone()
	{
		return base.Clone() as IStiPieSeries;
	}

	public override Type GetDefaultAreaType()
	{
		return typeof(StiPieArea);
	}

	protected virtual void OnGetCutPieList(StiGetValueEventArgs e)
	{
	}

	public void InvokeGetCutPieList(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			OnGetCutPieList(e);
			if (sender.Report.CalculationMode == StiCalculationMode.Interpretation)
			{
				StiText stiText = new StiText();
				stiText.Name = "**ChartPieSeriesCutPieList**";
				stiText.Page = sender.Report.Pages[0];
				object obj = StiParser.ParseTextValue(CutPieList.Value, stiText);
				e.Value = sender.Report.ToString(obj);
			}
			if (this.GetCutPieList != null)
			{
				this.GetCutPieList(sender, e);
			}
		}
		catch (Exception ex)
		{
			string text = $"Expression in GetCutPieList property of '{((StiService)this).ServiceName}' series from '{((StiChart)base.Chart).Name}' chart can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			((StiChart)base.Chart).Report.WriteToReportRenderingMessages(text);
		}
	}

	public override StiSeries CreateNew()
	{
		return new StiPieSeries();
	}

	public StiPieSeries()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		base.Core = new StiPieSeriesCoreXF(this);
	}
}
