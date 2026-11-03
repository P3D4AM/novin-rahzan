using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Chart.Design;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Chart;

[StiServiceBitmap(typeof(StiTrendLine), "Stimulsoft.Report.Images.Components.StiChart.png")]
[StiServiceCategoryBitmap(typeof(StiTrendLine), "Stimulsoft.Report.Images.Components.StiChart.png")]
[TypeConverter(typeof(StiTrendLineConverter))]
public abstract class StiTrendLine : StiService, IStiTrendLine, ICloneable, IStiDefault, IStiJsonReportObject, IStiPropertyGridObject
{
	public enum StiTextPosition
	{
		LeftTop,
		LeftBottom,
		RightTop,
		RightBottom
	}

	[Browsable(false)]
	public virtual StiComponentId ComponentId => StiComponentId.StiTrendLine;

	[Browsable(false)]
	public string PropName => string.Empty;

	public override string ServiceName => Core.LocalizedName;

	[Browsable(false)]
	public sealed override string ServiceCategory => "Chart";

	[Browsable(false)]
	public sealed override Type ServiceType => typeof(StiTrendLine);

	[Browsable(false)]
	public bool IsDefault
	{
		get
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			if (AllowApplyStyle && LineWidth == 1f && (int)LineStyle == 0 && Position == StiTextPosition.LeftBottom && Text != null && Text.Length == 0)
			{
				return TitleVisible;
			}
			return false;
		}
	}

	[Browsable(false)]
	public StiTrendLineCoreXF Core { get; set; }

	[StiSerializable]
	[StiOrder(90)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets color which will be used for drawing trend line.")]
	public virtual Color LineColor { get; set; } = Color.Black;


	[DefaultValue(1f)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[Description("Gets or sets trend line width.")]
	public float LineWidth { get; set; } = 1f;


	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets trend line style.")]
	public StiPenStyle LineStyle { get; set; }

	[StiOrder(120)]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates draw shadow or no.")]
	public bool ShowShadow { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that chart style will be used.")]
	[DefaultValue(true)]
	public bool AllowApplyStyle { get; set; } = true;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets font which used for drawing constant line text.")]
	public virtual Font Font { get; set; } = new Font("Arial", 7f);


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(StiTextPosition.LeftBottom)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets text position at trend line.")]
	public StiTextPosition Position { get; set; } = StiTextPosition.LeftBottom;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue("")]
	[Description("Gets or sets trend line text.")]
	public string Text { get; set; } = "";


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets visibility of trend line title.")]
	public bool TitleVisible { get; set; } = true;


	public JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", ((object)this).GetType().Name);
		JsonExtensions.AddPropertyColor(val, "LineColor", LineColor, Color.Black);
		JsonExtensions.AddPropertyFloat(val, "LineWidth", LineWidth, 1f);
		JsonExtensions.AddPropertyEnum(val, "LineStyle", (Enum)(object)LineStyle, (Enum)(object)(StiPenStyle)0);
		JsonExtensions.AddPropertyBool(val, "ShowShadow", ShowShadow, false, false);
		JsonExtensions.AddPropertyEnum(val, "Position", (Enum)Position, (Enum)StiTextPosition.LeftBottom);
		JsonExtensions.AddPropertyFontArial7(val, "Font", Font);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Text", Text);
		JsonExtensions.AddPropertyBool(val, "TitleVisible", TitleVisible, true, false);
		JsonExtensions.AddPropertyBool(val, "AllowApplyStyle", AllowApplyStyle, true, false);
		return val;
	}

	public void LoadFromJsonObject(JObject jObject)
	{
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "LineColor":
				LineColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "LineWidth":
				LineWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "LineStyle":
				LineStyle = StiJsonReportObjectHelper.DeserializeEnum<StiPenStyle>(item);
				break;
			case "ShowShadow":
				ShowShadow = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Font":
				Font = StiJsonReportObjectHelper.DeserializeFont(item, Font);
				break;
			case "Position":
				Position = StiJsonReportObjectHelper.DeserializeEnum<StiTextPosition>(item);
				break;
			case "Text":
				Text = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "TitleVisible":
				TitleVisible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "AllowApplyStyle":
				AllowApplyStyle = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	internal static IStiTrendLine CreateFromJsonObject(JObject jObject)
	{
		string ident = jObject.Properties().FirstOrDefault((Func<JProperty, bool>)((JProperty x) => x.Name == "Ident")).Value.ToObject<string>();
		StiTrendLine stiTrendLine = StiOptions.Services.ChartTrendLines.FirstOrDefault((StiTrendLine x) => ((object)x).GetType().Name == ident);
		if (stiTrendLine == null)
		{
			throw new Exception("Type " + ident + " is not found!");
		}
		StiTrendLine stiTrendLine2 = stiTrendLine.CreateNew();
		stiTrendLine2.LoadFromJsonObject(jObject);
		return stiTrendLine2;
	}

	public StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyObject[] value = new StiPropertyObject[1] { propertiesHelper.ChartTrendLine() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		return stiPropertyCollection;
	}

	public StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public override object Clone()
	{
		IStiTrendLine stiTrendLine = ((StiService)this).Clone() as IStiTrendLine;
		if (Core != null)
		{
			stiTrendLine.Core = Core.Clone() as StiTrendLineCoreXF;
			stiTrendLine.Core.TrendLine = stiTrendLine;
		}
		return stiTrendLine;
	}

	private bool ShouldSerializeFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (Font != null && Font.Name == "Arial" && Font.SizeInPoints == 7f)
		{
			return (int)Font.Style > 0;
		}
		return true;
	}

	public abstract StiTrendLine CreateNew();

	public override string ToString()
	{
		return ((StiService)this).ServiceName;
	}

	public StiTrendLine()
	{
	}//IL_0028: Unknown result type (might be due to invalid IL or missing references)
	//IL_0032: Expected O, but got Unknown

}
