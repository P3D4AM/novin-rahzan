using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Chart.Design;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

[StiServiceBitmap(typeof(StiSeriesLabels), "Stimulsoft.Report.Images.Components.StiChart.png")]
[StiServiceCategoryBitmap(typeof(StiSeriesLabels), "Stimulsoft.Report.Images.Components.StiChart.png")]
[TypeConverter(typeof(StiSeriesLabelsConverter))]
public abstract class StiSeriesLabels : StiService, IStiSerializeToCodeAsClass, IStiSeriesLabels, ICloneable, IStiJsonReportObject, IStiPropertyGridObject
{
	private bool allowApplyStyle = true;

	[Browsable(false)]
	public virtual StiComponentId ComponentId => StiComponentId.StiSeriesLabels;

	[Browsable(false)]
	public string PropName => string.Empty;

	public override string ServiceName => Core.LocalizedName;

	[Browsable(false)]
	public sealed override string ServiceCategory => "Chart";

	[Browsable(false)]
	public sealed override Type ServiceType => typeof(StiSeriesLabels);

	[StiSerializable]
	[StiOrder(240)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that whether it is necessary to avoid intersections between border of series labels and border of series.")]
	public bool PreventIntersection { get; set; }

	[Browsable(false)]
	public StiSeriesLabelsCoreXF Core { get; set; }

	[Browsable(false)]
	public StiAxisSeriesLabelsCoreXF AxisCore => Core as StiAxisSeriesLabelsCoreXF;

	[Browsable(false)]
	public StiPieSeriesLabelsCoreXF PieCore => Core as StiPieSeriesLabelsCoreXF;

	[StiSerializable]
	[StiOrder(90)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that chart style will be used.")]
	[DefaultValue(true)]
	public bool AllowApplyStyle
	{
		get
		{
			return allowApplyStyle;
		}
		set
		{
			if (allowApplyStyle != value)
			{
				allowApplyStyle = value;
				if (value && Chart != null)
				{
					Core.ApplyStyle(Chart.Style);
				}
			}
		}
	}

	[StiOrder(125)]
	[TypeConverter(typeof(StiChartConditionsCollectionConverter))]
	[Description("Gets or sets collection of conditions which can be used to change behavior of series labels.")]
	[Editor("Stimulsoft.Report.Chart.Design.StiChartConditionsEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public StiChartConditionsCollection Conditions
	{
		get
		{
			if (Chart == null)
			{
				return null;
			}
			return Chart.SeriesLabelsConditions;
		}
		set
		{
			if (Chart != null)
			{
				Chart.SeriesLabelsConditions = value;
			}
		}
	}

	[Browsable(false)]
	[Obsolete("ShowOnZeroValues property is obsolete. Please use ShowZeros property instead it.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShowOnZeroValues
	{
		get
		{
			return ShowZeros;
		}
		set
		{
			ShowZeros = value;
		}
	}

	[DefaultValue(false)]
	[StiOrder(270)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that series labels will be shown or not if value equal to zero.")]
	public bool ShowZeros { get; set; }

	[DefaultValue(true)]
	[StiOrder(255)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that series labels will be shown or not if value equal to null.")]
	public bool ShowNulls { get; set; } = true;


	[DefaultValue(false)]
	[StiOrder(230)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets vibility of marker.")]
	public bool MarkerVisible { get; set; }

	[StiSerializable]
	[StiOrder(220)]
	[TypeConverter(typeof(StiSizeConverter))]
	[Description("Gets or sets marker size.")]
	public Size MarkerSize { get; set; } = new Size(8, 6);


	[StiSerializable]
	[StiOrder(210)]
	[DefaultValue(StiMarkerAlignment.Left)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets marker alignment related to label text.")]
	public virtual StiMarkerAlignment MarkerAlignment { get; set; }

	[DefaultValue(0)]
	[StiOrder(280)]
	[StiSerializable]
	[Description("Gets or sets value which indicates with what steps do labels be shown.")]
	public int Step { get; set; }

	[StiOrder(320)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[DefaultValue(StiSeriesLabelsValueType.Value)]
	[Description("Gets or sets which type of information will be shown in series labels.")]
	public virtual StiSeriesLabelsValueType ValueType { get; set; }

	[StiOrder(330)]
	[StiSerializable]
	[DefaultValue("")]
	[Description("Gets or sets string which contain separator for value information (if applicated).")]
	public virtual string ValueTypeSeparator { get; set; } = "-";


	[StiOrder(190)]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[DefaultValue(StiSeriesLabelsValueType.Value)]
	[Description("Gets or sets which type of information will be shown in legend.")]
	public virtual StiSeriesLabelsValueType LegendValueType { get; set; }

	[DefaultValue("")]
	[StiOrder(300)]
	[StiSerializable]
	[Description("Gets or sets text which will be shown before label text.")]
	public string TextBefore { get; set; } = "";


	[DefaultValue("")]
	[StiOrder(290)]
	[StiSerializable]
	[Description("Gets or sets text which will be shown after label text.")]
	public string TextAfter { get; set; } = "";


	[DefaultValue(0f)]
	[StiOrder(100)]
	[StiSerializable]
	[Description("Gets or sets angle of text rotation.")]
	public virtual float Angle { get; set; }

	[DefaultValue("")]
	[StiOrder(170)]
	[Description("Gets or sets format string which used for formating series values (if applicable).")]
	[StiSerializable]
	[Editor("Stimulsoft.Report.Chart.Design.StiFormatEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string Format { get; set; } = "";


	[StiSerializable]
	[StiOrder(110)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which control antialiasing drawing mode of series labels.")]
	public virtual bool Antialiasing { get; set; } = true;


	[DefaultValue(true)]
	[StiOrder(340)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visiblity of series labels.")]
	public virtual bool Visible { get; set; } = true;


	[DefaultValue(true)]
	[StiOrder(130)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which incates that border will be drawn or not.")]
	public virtual bool DrawBorder { get; set; } = true;


	[DefaultValue(false)]
	[StiOrder(310)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that series colors must be used.")]
	public virtual bool UseSeriesColor { get; set; }

	[StiSerializable]
	[StiOrder(180)]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets foreground color of series labels.")]
	public virtual Color LabelColor { get; set; } = Color.Black;


	[StiSerializable]
	[StiOrder(140)]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets border color of series labels.")]
	public virtual Color BorderColor { get; set; } = Color.Black;


	[StiOrder(150)]
	[StiSerializable]
	[Description("Gets or sets brush which will be used to fill area of series labels.")]
	public virtual StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.White);


	[StiSerializable]
	[StiOrder(160)]
	[Description("Gets or sets font which will be used to draw series labels.")]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Font Font { get; set; } = new Font("Arial", 7f);


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public IStiChart Chart { get; set; }

	[StiSerializable]
	[StiOrder(360)]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets word wrap.")]
	public bool WordWrap { get; set; }

	[DefaultValue(0f)]
	[StiOrder(350)]
	[StiSerializable]
	[Description("Gets or sets fixed width of axis labels.")]
	public int Width { get; set; }

	[Browsable(false)]
	public StiFormatService FormatService { get; set; }

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", ((object)this).GetType().Name);
		JsonExtensions.AddPropertyJObject(val, "Conditions", Conditions.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyBool(val, "PreventIntersection", PreventIntersection, false, false);
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", AllowApplyStyle, true, false);
		JsonExtensions.AddPropertyBool(val, "ShowZeros", ShowZeros, false, false);
		JsonExtensions.AddPropertyBool(val, "ShowNulls", ShowNulls, true, false);
		JsonExtensions.AddPropertyBool(val, "MarkerVisible", MarkerVisible, false, false);
		JsonExtensions.AddPropertySize(val, "MarkerSize", MarkerSize);
		JsonExtensions.AddPropertyEnum(val, "MarkerAlignment", (Enum)MarkerAlignment, (Enum)StiMarkerAlignment.Left);
		JsonExtensions.AddPropertyInt(val, "Step", Step, 0);
		JsonExtensions.AddPropertyEnum(val, "ValueType", (Enum)ValueType, (Enum)StiSeriesLabelsValueType.Value);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ValueTypeSeparator", ValueTypeSeparator);
		JsonExtensions.AddPropertyEnum(val, "LegendValueType", (Enum)LegendValueType, (Enum)StiSeriesLabelsValueType.Value);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "TextBefore", TextBefore);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "TextAfter", TextAfter);
		JsonExtensions.AddPropertyFloat(val, "Angle", Angle, 0f);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Format", Format);
		JsonExtensions.AddPropertyBool(val, "Antialiasing", Antialiasing, true, false);
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, true, false);
		JsonExtensions.AddPropertyBool(val, "DrawBorder", DrawBorder, true, false);
		JsonExtensions.AddPropertyBool(val, "UseSeriesColor", UseSeriesColor, false, false);
		JsonExtensions.AddPropertyColor(val, "LabelColor", LabelColor, Color.Black);
		JsonExtensions.AddPropertyColor(val, "BorderColor", BorderColor, Color.Black);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyFontArial7(val, "Font", Font);
		JsonExtensions.AddPropertyBool(val, "WordWrap", WordWrap, false, false);
		JsonExtensions.AddPropertyInt(val, "Width", Width, 0);
		if (FormatService != null && !(FormatService is StiGeneralFormatService))
		{
			JsonExtensions.AddPropertyJObject(val, "FormatService", FormatService.SaveToJsonObject(mode));
		}
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cd: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Conditions":
				Conditions.LoadFromJsonObject((JObject)item.Value);
				break;
			case "PreventIntersection":
				PreventIntersection = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowApplyStyle":
				AllowApplyStyle = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShowZeros":
				ShowZeros = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShowNulls":
				ShowNulls = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "MarkerVisible":
				MarkerVisible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "MarkerSize":
				MarkerSize = StiJsonReportObjectHelper.DeserializeSize(item);
				break;
			case "MarkerAlignment":
				MarkerAlignment = StiJsonReportObjectHelper.DeserializeEnum<StiMarkerAlignment>(item);
				break;
			case "Step":
				Step = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "ValueType":
				ValueType = StiJsonReportObjectHelper.DeserializeEnum<StiSeriesLabelsValueType>(item);
				break;
			case "ValueTypeSeparator":
				ValueTypeSeparator = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "LegendValueType":
				LegendValueType = StiJsonReportObjectHelper.DeserializeEnum<StiSeriesLabelsValueType>(item);
				break;
			case "TextBefore":
				TextBefore = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "TextAfter":
				TextAfter = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Angle":
				Angle = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "Format":
				Format = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Antialiasing":
				Antialiasing = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Visible":
				Visible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "DrawBorder":
				DrawBorder = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "UseSeriesColor":
				UseSeriesColor = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "LabelColor":
				LabelColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BorderColor":
				BorderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Font":
				Font = StiJsonReportObjectHelper.DeserializeFont(item, Font);
				break;
			case "WordWrap":
				WordWrap = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Width":
				Width = StiJsonReportObjectHelper.DeserializeInt(item);
				break;
			case "FormatService":
				FormatService = StiFormatService.CreateFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	internal static IStiSeriesLabels LoadFromJsonObjectInternal(JObject jObject, StiChart chart)
	{
		string ident = jObject.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>();
		StiSeriesLabels stiSeriesLabels = StiOptions.Services.ChartSerieLabels.FirstOrDefault((StiSeriesLabels x) => ((object)x).GetType().Name == ident);
		if (stiSeriesLabels == null)
		{
			throw new Exception("Type " + ident + " is not found!");
		}
		StiSeriesLabels stiSeriesLabels2 = stiSeriesLabels.CreateNew();
		stiSeriesLabels2.Chart = chart;
		stiSeriesLabels2.LoadFromJsonObject(jObject);
		return stiSeriesLabels2;
	}

	public virtual StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		return null;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public override object Clone()
	{
		IStiSeriesLabels stiSeriesLabels = ((StiService)this).Clone() as IStiSeriesLabels;
		object obj = Brush.Clone();
		stiSeriesLabels.Brush = (StiBrush)((obj is StiBrush) ? obj : null);
		object obj2 = Font.Clone();
		stiSeriesLabels.Font = (Font)((obj2 is Font) ? obj2 : null);
		if (Core != null)
		{
			stiSeriesLabels.Core = Core.Clone() as StiSeriesLabelsCoreXF;
			stiSeriesLabels.Core.SeriesLabels = stiSeriesLabels;
		}
		return stiSeriesLabels;
	}

	private bool ShouldSerializeConditions()
	{
		if (Conditions != null)
		{
			return Conditions.Count > 0;
		}
		return true;
	}

	private bool ShouldSerializeMarkerSize()
	{
		if (MarkerSize.Width == 8)
		{
			return MarkerSize.Height != 6;
		}
		return true;
	}

	public override string ToString()
	{
		return ((StiService)this).ServiceName;
	}

	public virtual StiSeriesLabels CreateNew()
	{
		throw new NotImplementedException();
	}
}
