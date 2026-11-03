using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

public class StiStrips : StiService, IStiStrips, ICloneable, IStiJsonReportObject, IStiPropertyGridObject
{
	public enum StiOrientation
	{
		Horizontal,
		Vertical,
		HorizontalRight
	}

	private bool allowApplyStyle = true;

	[Browsable(false)]
	public StiComponentId ComponentId => StiComponentId.StiStrips;

	[Browsable(false)]
	public string PropName => string.Empty;

	[Browsable(false)]
	public sealed override string ServiceCategory => "Chart";

	[Browsable(false)]
	public sealed override Type ServiceType => typeof(StiStrips);

	[Browsable(false)]
	public StiStripsCoreXF Core { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
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

	[DefaultValue(true)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets value which indicates that strips will be shown behind chart series or in front of chart series.")]
	public virtual bool ShowBehind { get; set; } = true;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets brush which will be used for drawing strips.")]
	public StiBrush StripBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.Transparent);


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which control antialiasing drawing mode.")]
	public bool Antialiasing { get; set; } = true;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets font of title text.")]
	public virtual Font Font { get; set; } = new Font("Arial", 7f);


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue("")]
	[Description("Gets or sets title text.")]
	public string Text { get; set; } = "";


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of title.")]
	public bool TitleVisible { get; set; } = true;


	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets foreground color of title.")]
	public virtual Color TitleColor { get; set; } = Color.Green;


	[DefaultValue(StiOrientation.Horizontal)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets horizontal or vertical orientation of strips.")]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiOrientation Orientation { get; set; }

	[Browsable(false)]
	[DefaultValue(true)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets value which indicates that strips will be shown in legend of chart.")]
	public virtual bool ShowInLegend { get; set; } = true;


	[DefaultValue("1")]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets maximal value of strips.")]
	public virtual string MaxValue { get; set; } = "1";


	[DefaultValue("0")]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets minimal range of strips.")]
	public virtual string MinValue { get; set; } = "0";


	[DefaultValue(true)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets visibility of strips.")]
	public virtual bool Visible { get; set; } = true;


	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public IStiChart Chart { get; set; }

	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		JObject val = new JObject();
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", AllowApplyStyle, true, false);
		JsonExtensions.AddPropertyBool(val, "ShowBehind", ShowBehind, true, false);
		JsonExtensions.AddPropertyBrush(val, "StripBrush", StripBrush);
		JsonExtensions.AddPropertyBool(val, "Antialiasing", Antialiasing, true, false);
		JsonExtensions.AddPropertyFontArial7(val, "Font", Font);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Text", Text);
		JsonExtensions.AddPropertyBool(val, "TitleVisible", TitleVisible, true, false);
		JsonExtensions.AddPropertyColor(val, "TitleColor", TitleColor, Color.Green);
		JsonExtensions.AddPropertyEnum(val, "Orientation", (Enum)Orientation, (Enum)StiOrientation.Horizontal);
		JsonExtensions.AddPropertyBool(val, "ShowInLegend", ShowInLegend, true, false);
		JsonExtensions.AddPropertyString(val, "MaxValue", MaxValue, "1");
		JsonExtensions.AddPropertyString(val, "MinValue", MinValue, "0");
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, true, false);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "AllowApplyStyle":
				AllowApplyStyle = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShowBehind":
				ShowBehind = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "StripBrush":
				StripBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Antialiasing":
				Antialiasing = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Font":
				Font = StiJsonReportObjectHelper.DeserializeFont(item, Font);
				break;
			case "Text":
				Text = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "TitleVisible":
				TitleVisible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "TitleColor":
				TitleColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Orientation":
				Orientation = StiJsonReportObjectHelper.DeserializeEnum<StiOrientation>(item);
				break;
			case "ShowInLegend":
				ShowInLegend = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "MaxValue":
				MaxValue = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "MinValue":
				MinValue = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Visible":
				Visible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.AllowApplyStyle() };
		stiPropertyCollection.Add(StiPropertyCategories.Chart, value);
		value = new StiPropertyObject[6]
		{
			propertiesHelper.MaxValue(),
			propertiesHelper.MinValue(),
			propertiesHelper.Orientation(),
			propertiesHelper.ShowBehind(),
			propertiesHelper.StripBrush(),
			propertiesHelper.Visible()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, value);
		value = new StiPropertyObject[5]
		{
			propertiesHelper.Antialiasing(),
			propertiesHelper.Font(),
			propertiesHelper.TextNotEdit(),
			propertiesHelper.TitleColor(),
			propertiesHelper.TitleVisible()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Title, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public override object Clone()
	{
		IStiStrips stiStrips = MemberwiseClone() as IStiStrips;
		if (Core != null)
		{
			stiStrips.Core = Core.Clone() as StiStripsCoreXF;
			stiStrips.Core.Strips = stiStrips;
		}
		return stiStrips;
	}

	public StiStrips()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Expected O, but got Unknown
		Core = new StiStripsCoreXF(this);
	}
}
