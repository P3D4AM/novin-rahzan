using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.IO;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Gauge;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report;

public class StiGaugeStyle : StiBaseStyle
{
	private Font tickLabelMajorFont = new Font("Arial", 10f);

	private Font tickLabelMinorFont = new Font("Arial", 10f);

	public override StiComponentId ComponentId => StiComponentId.StiGaugeStyle;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush(Color.White);


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color BorderColor { get; set; } = Color.Transparent;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color TargetColor { get; set; } = Color.Black;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public Color ForeColor { get; set; } = Color.Black;


	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(0f)]
	public float BorderWidth { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush TickMarkMajorBrush { get; set; } = (StiBrush)new StiSolidBrush("#990000");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush TickMarkMajorBorder { get; set; } = (StiBrush)new StiSolidBrush("#0bac45");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public float TickMarkMajorBorderWidth { get; set; } = 1f;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush TickMarkMinorBrush { get; set; } = (StiBrush)new StiSolidBrush("#4472c4");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush TickMarkMinorBorder { get; set; } = (StiBrush)new StiSolidBrush("#4472c4");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public float TickMarkMinorBorderWidth { get; set; } = 1f;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush TickLabelMajorTextBrush { get; set; } = (StiBrush)new StiSolidBrush("#33475B");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public Font TickLabelMajorFont
	{
		get
		{
			return tickLabelMajorFont;
		}
		set
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			if (tickLabelMajorFont != value)
			{
				if (value == null)
				{
					value = new Font("Arial", 10f);
				}
				tickLabelMajorFont = value;
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush TickLabelMinorTextBrush { get; set; } = (StiBrush)new StiSolidBrush("#33475B");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public Font TickLabelMinorFont
	{
		get
		{
			return tickLabelMinorFont;
		}
		set
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Expected O, but got Unknown
			if (tickLabelMinorFont != value)
			{
				if (value == null)
				{
					value = new Font("Arial", 10f);
				}
				tickLabelMinorFont = value;
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush MarkerBrush { get; set; } = (StiBrush)new StiSolidBrush("#70ad47");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush LinearBarBrush { get; set; } = (StiBrush)new StiSolidBrush("#4472c4");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush LinearScaleBrush { get; set; } = (StiBrush)new StiSolidBrush("#70ad47");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush LinearBarBorderBrush { get; set; } = (StiBrush)new StiEmptyBrush();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush LinearBarEmptyBrush { get; set; } = (StiBrush)new StiEmptyBrush();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush LinearBarEmptyBorderBrush { get; set; } = (StiBrush)new StiEmptyBrush();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush RadialBarBrush { get; set; } = (StiBrush)new StiSolidBrush("#ffc000");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush RadialBarBorderBrush { get; set; } = (StiBrush)new StiEmptyBrush();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush RadialBarEmptyBrush { get; set; } = (StiBrush)new StiSolidBrush("#43682b");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush RadialBarEmptyBorderBrush { get; set; } = (StiBrush)new StiEmptyBrush();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush NeedleBrush { get; set; } = (StiBrush)new StiSolidBrush("#ffc000");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush NeedleBorderBrush { get; set; } = (StiBrush)new StiEmptyBrush();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public float NeedleBorderWidth { get; set; } = 1f;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush NeedleCapBrush { get; set; } = (StiBrush)new StiSolidBrush("#ffc000");


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiSerializable]
	public StiBrush NeedleCapBorderBrush { get; set; } = (StiBrush)new StiSolidBrush("#ffc000");


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyColor(val, "BorderColor", BorderColor, Color.Transparent);
		JsonExtensions.AddPropertyColor(val, "ForeColor", ForeColor, Color.Black);
		JsonExtensions.AddPropertyColor(val, "TargetColor", TargetColor, Color.Black);
		JsonExtensions.AddPropertyFloat(val, "BorderWidth", BorderWidth, 0f);
		JsonExtensions.AddPropertyBrush(val, "TickMarkMajorBrush", TickMarkMajorBrush);
		JsonExtensions.AddPropertyBrush(val, "TickMarkMajorBorder", TickMarkMajorBorder);
		JsonExtensions.AddPropertyFloat(val, "TickMarkMajorBorderWidth", TickMarkMajorBorderWidth, 1f);
		JsonExtensions.AddPropertyBrush(val, "TickMarkMinorBrush", TickMarkMinorBrush);
		JsonExtensions.AddPropertyBrush(val, "TickMarkMinorBorder", TickMarkMinorBorder);
		JsonExtensions.AddPropertyFloat(val, "TickMarkMinorBorderWidth", TickMarkMinorBorderWidth, 1f);
		JsonExtensions.AddPropertyBrush(val, "TickLabelMajorTextBrush", TickLabelMajorTextBrush);
		JsonExtensions.AddPropertyFontArial8(val, "TickLabelMajorFont", TickLabelMajorFont);
		JsonExtensions.AddPropertyBrush(val, "TickLabelMinorTextBrush", TickLabelMinorTextBrush);
		JsonExtensions.AddPropertyFontArial8(val, "TickLabelMinorFont", TickLabelMinorFont);
		JsonExtensions.AddPropertyBrush(val, "MarkerBrush", MarkerBrush);
		JsonExtensions.AddPropertyBrush(val, "LinearBarBrush", LinearBarBrush);
		JsonExtensions.AddPropertyBrush(val, "LinearBarBorderBrush", LinearBarBorderBrush);
		JsonExtensions.AddPropertyBrush(val, "LinearBarEmptyBrush", LinearBarEmptyBrush);
		JsonExtensions.AddPropertyBrush(val, "LinearBarEmptyBorderBrush", LinearBarEmptyBorderBrush);
		JsonExtensions.AddPropertyBrush(val, "RadialBarBrush", RadialBarBrush);
		JsonExtensions.AddPropertyBrush(val, "RadialBarBorderBrush", RadialBarBorderBrush);
		JsonExtensions.AddPropertyBrush(val, "RadialBarEmptyBrush", RadialBarEmptyBrush);
		JsonExtensions.AddPropertyBrush(val, "RadialBarEmptyBorderBrush", RadialBarEmptyBorderBrush);
		JsonExtensions.AddPropertyBrush(val, "NeedleBrush", NeedleBrush);
		JsonExtensions.AddPropertyBrush(val, "NeedleBorderBrush", NeedleBorderBrush);
		JsonExtensions.AddPropertyBrush(val, "LinearScaleBrush", LinearScaleBrush);
		JsonExtensions.AddPropertyFloat(val, "NeedleBorderWidth", NeedleBorderWidth, 1f);
		JsonExtensions.AddPropertyBrush(val, "NeedleCapBrush", NeedleCapBrush);
		JsonExtensions.AddPropertyBrush(val, "NeedleCapBorderBrush", NeedleCapBorderBrush);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "LinearScaleBrush":
				LinearScaleBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "BorderColor":
				BorderColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "ForeColor":
				ForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "TargetColor":
				TargetColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BorderWidth":
				BorderWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "TickMarkMajorBrush":
				TickMarkMajorBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "TickMarkMajorBorder":
				TickMarkMajorBorder = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "TickMarkMajorBorderWidth":
				TickMarkMajorBorderWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "TickMarkMinorBrush":
				TickMarkMinorBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "TickMarkMinorBorder":
				TickMarkMinorBorder = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "TickMarkMinorBorderWidth":
				TickMarkMinorBorderWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "TickLabelMajorTextBrush":
				TickLabelMajorTextBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "TickLabelMajorFont":
				TickLabelMajorFont = StiJsonReportObjectHelper.DeserializeFont(item, TickLabelMajorFont);
				break;
			case "TickLabelMinorTextBrush":
				TickLabelMinorTextBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "TickLabelMinorFont":
				TickLabelMinorFont = StiJsonReportObjectHelper.DeserializeFont(item, TickLabelMinorFont);
				break;
			case "MarkerBrush":
				MarkerBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "LinearBarBrush":
				LinearBarBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "LinearBarBorderBrush":
				LinearBarBorderBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "LinearBarEmptyBrush":
				LinearBarEmptyBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "LinearBarEmptyBorderBrush":
				LinearBarEmptyBorderBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "RadialBarBrush":
				RadialBarBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "RadialBarBorderBrush":
				RadialBarBorderBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "RadialBarEmptyBrush":
				RadialBarEmptyBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "RadialBarEmptyBorderBrush":
				RadialBarEmptyBorderBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "NeedleBrush":
				NeedleBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "NeedleBorderBrush":
				NeedleBorderBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "NeedleBorderWidth":
				NeedleBorderWidth = StiJsonReportObjectHelper.DeserializeFloat(item);
				break;
			case "NeedleCapBrush":
				NeedleCapBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "NeedleCapBorderBrush":
				NeedleCapBorderBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		StiPropertyObject[] value = new StiPropertyObject[4]
		{
			propertiesHelper.StyleName(),
			propertiesHelper.Description(),
			propertiesHelper.StyleCollectionName(),
			propertiesHelper.StyleConditions()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Main, value);
		value = new StiPropertyObject[1] { propertiesHelper.MarkerBrush() };
		stiPropertyCollection.Add(StiPropertyCategories.Market, value);
		value = new StiPropertyObject[3]
		{
			propertiesHelper.BorderWidth(),
			propertiesHelper.BorderColor(),
			propertiesHelper.Brush()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Misc, value);
		value = new StiPropertyObject[2]
		{
			propertiesHelper.TickMarkMajorBorder(),
			propertiesHelper.TickMarkMajorBrush()
		};
		stiPropertyCollection.Add(StiPropertyCategories.TickMarkMajor, value);
		value = new StiPropertyObject[2]
		{
			propertiesHelper.TickMarkMinorBorder(),
			propertiesHelper.TickMarkMinorBrush()
		};
		stiPropertyCollection.Add(StiPropertyCategories.TickMarkMinor, value);
		value = new StiPropertyObject[2]
		{
			propertiesHelper.TickLabelMajorTextBrush(),
			propertiesHelper.TickLabelMajorFont()
		};
		stiPropertyCollection.Add(StiPropertyCategories.TickLabelMajor, value);
		value = new StiPropertyObject[2]
		{
			propertiesHelper.TickLabelMinorTextBrush(),
			propertiesHelper.TickLabelMinorFont()
		};
		stiPropertyCollection.Add(StiPropertyCategories.TickLabelMinor, value);
		value = new StiPropertyObject[4]
		{
			propertiesHelper.LinearBarBorderBrush(),
			propertiesHelper.LinearBarBrush(),
			propertiesHelper.LinearBarEmptyBorderBrush(),
			propertiesHelper.LinearBarEmptyBrush()
		};
		stiPropertyCollection.Add(StiPropertyCategories.LinearScaleBar, value);
		value = new StiPropertyObject[4]
		{
			propertiesHelper.RadialBarBorderBrush(),
			propertiesHelper.RadialBarBrush(),
			propertiesHelper.RadialBarEmptyBorderBrush(),
			propertiesHelper.RadialBarEmptyBrush()
		};
		stiPropertyCollection.Add(StiPropertyCategories.RadialScaleBar, value);
		value = new StiPropertyObject[4]
		{
			propertiesHelper.NeedleBorderBrush(),
			propertiesHelper.NeedleBrush(),
			propertiesHelper.NeedleCapBorderBrush(),
			propertiesHelper.NeedleCapBrush()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Needle, value);
		return stiPropertyCollection;
	}

	public override object Clone()
	{
		StiGaugeStyle stiGaugeStyle = base.Clone() as StiGaugeStyle;
		object obj = Brush.Clone();
		stiGaugeStyle.Brush = (StiBrush)((obj is StiBrush) ? obj : null);
		object obj2 = TickMarkMajorBrush.Clone();
		stiGaugeStyle.TickMarkMajorBrush = (StiBrush)((obj2 is StiBrush) ? obj2 : null);
		object obj3 = TickMarkMajorBorder.Clone();
		stiGaugeStyle.TickMarkMajorBorder = (StiBrush)((obj3 is StiBrush) ? obj3 : null);
		object obj4 = TickMarkMinorBrush.Clone();
		stiGaugeStyle.TickMarkMinorBrush = (StiBrush)((obj4 is StiBrush) ? obj4 : null);
		object obj5 = TickMarkMinorBorder.Clone();
		stiGaugeStyle.TickMarkMinorBorder = (StiBrush)((obj5 is StiBrush) ? obj5 : null);
		object obj6 = TickLabelMajorTextBrush.Clone();
		stiGaugeStyle.TickLabelMajorTextBrush = (StiBrush)((obj6 is StiBrush) ? obj6 : null);
		object obj7 = TickLabelMajorFont.Clone();
		stiGaugeStyle.TickLabelMajorFont = (Font)((obj7 is Font) ? obj7 : null);
		object obj8 = TickLabelMinorTextBrush.Clone();
		stiGaugeStyle.TickLabelMinorTextBrush = (StiBrush)((obj8 is StiBrush) ? obj8 : null);
		object obj9 = TickLabelMinorFont.Clone();
		stiGaugeStyle.TickLabelMinorFont = (Font)((obj9 is Font) ? obj9 : null);
		object obj10 = MarkerBrush.Clone();
		stiGaugeStyle.MarkerBrush = (StiBrush)((obj10 is StiBrush) ? obj10 : null);
		object obj11 = LinearBarBrush.Clone();
		stiGaugeStyle.LinearBarBrush = (StiBrush)((obj11 is StiBrush) ? obj11 : null);
		object obj12 = LinearScaleBrush.Clone();
		stiGaugeStyle.LinearScaleBrush = (StiBrush)((obj12 is StiBrush) ? obj12 : null);
		object obj13 = LinearBarBorderBrush.Clone();
		stiGaugeStyle.LinearBarBorderBrush = (StiBrush)((obj13 is StiBrush) ? obj13 : null);
		object obj14 = LinearBarEmptyBrush.Clone();
		stiGaugeStyle.LinearBarEmptyBrush = (StiBrush)((obj14 is StiBrush) ? obj14 : null);
		object obj15 = LinearBarEmptyBorderBrush.Clone();
		stiGaugeStyle.LinearBarEmptyBorderBrush = (StiBrush)((obj15 is StiBrush) ? obj15 : null);
		object obj16 = RadialBarBrush.Clone();
		stiGaugeStyle.RadialBarBrush = (StiBrush)((obj16 is StiBrush) ? obj16 : null);
		object obj17 = RadialBarBorderBrush.Clone();
		stiGaugeStyle.RadialBarBorderBrush = (StiBrush)((obj17 is StiBrush) ? obj17 : null);
		object obj18 = RadialBarEmptyBrush.Clone();
		stiGaugeStyle.RadialBarEmptyBrush = (StiBrush)((obj18 is StiBrush) ? obj18 : null);
		object obj19 = RadialBarEmptyBorderBrush.Clone();
		stiGaugeStyle.RadialBarEmptyBorderBrush = (StiBrush)((obj19 is StiBrush) ? obj19 : null);
		object obj20 = NeedleBrush.Clone();
		stiGaugeStyle.NeedleBrush = (StiBrush)((obj20 is StiBrush) ? obj20 : null);
		object obj21 = NeedleBorderBrush.Clone();
		stiGaugeStyle.NeedleBorderBrush = (StiBrush)((obj21 is StiBrush) ? obj21 : null);
		object obj22 = NeedleCapBrush.Clone();
		stiGaugeStyle.NeedleCapBrush = (StiBrush)((obj22 is StiBrush) ? obj22 : null);
		object obj23 = NeedleCapBorderBrush.Clone();
		stiGaugeStyle.NeedleCapBorderBrush = (StiBrush)((obj23 is StiBrush) ? obj23 : null);
		return stiGaugeStyle;
	}

	private bool ShouldSerializeBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (Brush is StiSolidBrush)
		{
			return !(((StiSolidBrush)Brush).Color == Color.White);
		}
		return true;
	}

	private bool ShouldSerializeBorderColor()
	{
		return BorderColor != Color.Transparent;
	}

	private bool ShouldSerializeTargetColor()
	{
		return TargetColor != Color.Black;
	}

	private bool ShouldSerializeForeColor()
	{
		return ForeColor != Color.Transparent;
	}

	private bool ShouldSerializeBorderWidth()
	{
		return BorderWidth != 0f;
	}

	private bool ShouldSerializeTickMarkMajorBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (TickMarkMajorBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)TickMarkMajorBrush).Color == ColorTranslator.FromHtml("#990000"));
		}
		return true;
	}

	private bool ShouldSerializeTickMarkMajorBorder()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (TickMarkMajorBorder is StiSolidBrush)
		{
			return !(((StiSolidBrush)TickMarkMajorBorder).Color == ColorTranslator.FromHtml("#0bac45"));
		}
		return true;
	}

	private bool ShouldSerializeTickMarkMajorBorderWidth()
	{
		return TickMarkMajorBorderWidth != 1f;
	}

	private bool ShouldSerializeTickMarkMinorBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (TickMarkMinorBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)TickMarkMinorBrush).Color == ColorTranslator.FromHtml("#4472c4"));
		}
		return true;
	}

	private bool ShouldSerializeTickMarkMinorBorder()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (TickMarkMinorBorder is StiSolidBrush)
		{
			return !(((StiSolidBrush)TickMarkMinorBorder).Color == ColorTranslator.FromHtml("#4472c4"));
		}
		return true;
	}

	private bool ShouldSerializeTickMarkMinorBorderWidth()
	{
		return TickMarkMinorBorderWidth != 1f;
	}

	private bool ShouldSerializeTickLabelMajorTextBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (TickLabelMajorTextBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)TickLabelMajorTextBrush).Color == ColorTranslator.FromHtml("#FF33475B"));
		}
		return true;
	}

	private bool ShouldSerializeTickLabelMajorFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (TickLabelMajorFont != null && TickLabelMajorFont.Name == "Arial" && TickLabelMajorFont.Size == 10f)
		{
			return (int)TickLabelMajorFont.Style > 0;
		}
		return true;
	}

	private bool ShouldSerializeTickLabelMinorTextBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (TickLabelMinorTextBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)TickLabelMinorTextBrush).Color == ColorTranslator.FromHtml("#FF33475B"));
		}
		return true;
	}

	private bool ShouldSerializeTickLabelMinorFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (TickLabelMinorFont != null && TickLabelMinorFont.Name == "Arial" && TickLabelMinorFont.Size == 10f)
		{
			return (int)TickLabelMinorFont.Style > 0;
		}
		return true;
	}

	private bool ShouldSerializeMarkerBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (MarkerBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)MarkerBrush).Color == ColorTranslator.FromHtml("#70ad47"));
		}
		return true;
	}

	private bool ShouldSerializeLinearBarBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (LinearBarBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)LinearBarBrush).Color == ColorTranslator.FromHtml("#4472c4"));
		}
		return true;
	}

	private bool ShouldSerializeLinearScaleBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (LinearScaleBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)LinearScaleBrush).Color == ColorTranslator.FromHtml("#70ad47"));
		}
		return true;
	}

	private bool ShouldSerializeLinearBarBorderBrush()
	{
		return !(LinearBarBorderBrush is StiEmptyBrush);
	}

	private bool ShouldSerializeLinearBarEmptyBrush()
	{
		return !(LinearBarEmptyBrush is StiEmptyBrush);
	}

	private bool ShouldSerializeLinearBarEmptyBorderBrush()
	{
		return !(LinearBarEmptyBorderBrush is StiEmptyBrush);
	}

	private bool ShouldSerializeRadialBarBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (RadialBarBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)RadialBarBrush).Color == ColorTranslator.FromHtml("#ffc000"));
		}
		return true;
	}

	private bool ShouldSerializeRadialBarBorderBrush()
	{
		return !(RadialBarBorderBrush is StiEmptyBrush);
	}

	private bool ShouldSerializeRadialBarEmptyBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (RadialBarEmptyBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)RadialBarEmptyBrush).Color == ColorTranslator.FromHtml("#43682b"));
		}
		return true;
	}

	private bool ShouldSerializeRadialBarEmptyBorderBrush()
	{
		return !(RadialBarEmptyBorderBrush is StiEmptyBrush);
	}

	private bool ShouldSerializeNeedleBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (NeedleBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)NeedleBrush).Color == ColorTranslator.FromHtml("#ffc000"));
		}
		return true;
	}

	private bool ShouldSerializeNeedleBorderBrush()
	{
		return !(NeedleBorderBrush is StiEmptyBrush);
	}

	private bool ShouldSerializeNeedleBorderWidth()
	{
		return NeedleBorderWidth != 1f;
	}

	private bool ShouldSerializeNeedleCapBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (NeedleCapBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)NeedleCapBrush).Color == ColorTranslator.FromHtml("#ffc000"));
		}
		return true;
	}

	private bool ShouldSerializeNeedleCapBorderBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (NeedleCapBorderBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)NeedleCapBorderBrush).Color == ColorTranslator.FromHtml("#ffc000"));
		}
		return true;
	}

	public override void DrawStyle(Graphics g, Rectangle rect, bool paintValue, bool paintImage)
	{
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Expected O, but got Unknown
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		Rectangle rectangle = new Rectangle(rect.X + StiScaleUI.I11, rect.Y + StiScaleUI.I6, StiScaleUI.I(29), StiScaleUI.I(29));
		Rectangle rectangle2 = new Rectangle(rectangle.X + rectangle.Width / 2 - rectangle.Height / 2, rectangle.Y, rectangle.Height, rectangle.Height);
		Brush brush = StiBrush.GetBrush(Brush, rectangle);
		try
		{
			g.FillEllipse(brush, rectangle2);
		}
		finally
		{
			((IDisposable)brush)?.Dispose();
		}
		g.DrawEllipse(Pens.LightGray, rectangle2);
		Rectangle rectangle3 = new Rectangle(rectangle.X + rectangle.Width / 2 - 2, rectangle.Y + rectangle.Height / 2 - 2, StiScaleUI.I4, StiScaleUI.I4);
		Brush brush2 = StiBrush.GetBrush(NeedleCapBrush, rectangle3);
		try
		{
			Pen val = new Pen(brush2);
			try
			{
				g.DrawEllipse(val, rectangle3);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)brush2)?.Dispose();
		}
		g.DrawLine(new Pen(StiBrush.ToColor(NeedleBrush)), rectangle2.X + rectangle2.Width / 2 + 2, rectangle2.Y + rectangle2.Height / 2, rectangle2.X + rectangle2.Width + 2, rectangle2.Y + rectangle2.Height / 2);
		if (rect.Height < StiScaleUI.I(32))
		{
			rect.Width -= StiScaleUI.I(36);
			rect.X += StiScaleUI.I(36);
		}
		else
		{
			rect.Width -= StiScaleUI.I(56);
			rect.X += StiScaleUI.I(56);
		}
		SmoothingMode smoothingMode = g.SmoothingMode;
		g.SmoothingMode = (SmoothingMode)4;
		SolidBrush val2 = new SolidBrush(Color.Black);
		try
		{
			Font val3 = new Font("Arial", 8f);
			try
			{
				StringFormat val4 = new StringFormat();
				try
				{
					val4.Alignment = (StringAlignment)0;
					val4.LineAlignment = (StringAlignment)1;
					g.DrawString(Name, val3, (Brush)(object)val2, (RectangleF)rect, val4);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		g.SmoothingMode = smoothingMode;
	}

	public override void DrawBox(Graphics g, Rectangle rect, bool paintValue, bool paintImage)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		rect.X++;
		rect.Y++;
		rect.Width -= 2;
		rect.Height -= 3;
		DrawStyle(g, rect, paintValue, paintImage);
		Pen val = new Pen(Color.FromArgb(150, Color.Gray));
		try
		{
			val.DashStyle = (DashStyle)1;
			g.DrawRectangle(val, rect);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public override void GetStyleFromComponent(StiComponent component, StiStyleElements styleElements)
	{
		if (styleElements != StiStyleElements.All)
		{
			throw new Exception("StiGaugeStyle support only StiStyleElements.All.");
		}
		if (component is StiGauge && (styleElements & StiStyleElements.Brush) > (StiStyleElements)0)
		{
			IStiBrush stiBrush = component as IStiBrush;
			object obj = stiBrush.Brush.Clone();
			Brush = (StiBrush)((obj is StiBrush) ? obj : null);
		}
	}

	public override void SetStyleToComponent(StiComponent component)
	{
	}

	public override void Save(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Serialize((object)this, stream, "StiGaugeStyle");
	}

	public override void Load(Stream stream)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		StiSerializing val = new StiSerializing((StiObjectStringConverter)(object)new StiReportObjectStringConverter());
		val.Deserialize((object)this, stream, "StiGaugeStyle");
	}

	internal StiGaugeStyle(string name, string description, StiReport report)
		: base(name, description, report)
	{
	}//IL_0006: Unknown result type (might be due to invalid IL or missing references)
	//IL_0010: Expected O, but got Unknown
	//IL_0037: Unknown result type (might be due to invalid IL or missing references)
	//IL_0041: Expected O, but got Unknown
	//IL_0047: Unknown result type (might be due to invalid IL or missing references)
	//IL_0051: Expected O, but got Unknown
	//IL_0062: Unknown result type (might be due to invalid IL or missing references)
	//IL_006c: Expected O, but got Unknown
	//IL_0072: Unknown result type (might be due to invalid IL or missing references)
	//IL_007c: Expected O, but got Unknown
	//IL_008d: Unknown result type (might be due to invalid IL or missing references)
	//IL_0097: Expected O, but got Unknown
	//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
	//IL_00ac: Expected O, but got Unknown
	//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
	//IL_00bc: Expected O, but got Unknown
	//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
	//IL_00d1: Expected O, but got Unknown
	//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
	//IL_00e1: Expected O, but got Unknown
	//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
	//IL_00f1: Expected O, but got Unknown
	//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
	//IL_0101: Expected O, but got Unknown
	//IL_0102: Unknown result type (might be due to invalid IL or missing references)
	//IL_010c: Expected O, but got Unknown
	//IL_010d: Unknown result type (might be due to invalid IL or missing references)
	//IL_0117: Expected O, but got Unknown
	//IL_0118: Unknown result type (might be due to invalid IL or missing references)
	//IL_0122: Expected O, but got Unknown
	//IL_0128: Unknown result type (might be due to invalid IL or missing references)
	//IL_0132: Expected O, but got Unknown
	//IL_0133: Unknown result type (might be due to invalid IL or missing references)
	//IL_013d: Expected O, but got Unknown
	//IL_0143: Unknown result type (might be due to invalid IL or missing references)
	//IL_014d: Expected O, but got Unknown
	//IL_014e: Unknown result type (might be due to invalid IL or missing references)
	//IL_0158: Expected O, but got Unknown
	//IL_015e: Unknown result type (might be due to invalid IL or missing references)
	//IL_0168: Expected O, but got Unknown
	//IL_0169: Unknown result type (might be due to invalid IL or missing references)
	//IL_0173: Expected O, but got Unknown
	//IL_0184: Unknown result type (might be due to invalid IL or missing references)
	//IL_018e: Expected O, but got Unknown
	//IL_0194: Unknown result type (might be due to invalid IL or missing references)
	//IL_019e: Expected O, but got Unknown


	public StiGaugeStyle(string name, string description)
		: this(name, description, null)
	{
	}

	public StiGaugeStyle(string name)
		: this(name, "")
	{
	}

	public StiGaugeStyle()
		: this("")
	{
	}
}
