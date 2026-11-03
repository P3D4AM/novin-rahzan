using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Text.RegularExpressions;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components;

[StiToolbox(true)]
[StiServiceBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.StiMathFormula.png")]
[StiDesigner("Stimulsoft.Report.Components.Design.StiMathFormulaDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiMathFormulaGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiMathFormulaWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiEngine(/*Could not decode attribute arguments.*/)]
[StiV2Builder(typeof(StiMathFormulаV2Builder))]
public class StiMathFormula : StiComponent, IStiExportImageExtended, IStiExportImage, IStiFont, IStiBorder, IStiBrush, IStiTextBrush, IStiHorAlignment, IStiVertAlignment
{
	private Font font = new Font("Arial", 12f);

	private string laTexExpression;

	private string value;

	public override StiComponentId ComponentId => StiComponentId.StiMathFormula;

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the horizontal alignment of an object.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionEnumConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionEnumEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public StiHorAlignment HorAlignment { get; set; } = (StiHorAlignment)1;


	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the vertical alignment of an object.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionEnumConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionEnumEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public StiVertAlignment VertAlignment { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[StiSerializable]
	[Description("The brush, which is used to draw background.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionBrushConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionBrushEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public StiBrush Brush { get; set; } = (StiBrush)new StiSolidBrush();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable]
	[Description("Gets or sets border of the component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBorder Border { get; set; } = new StiBorder();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(300)]
	[StiSerializable]
	[Description("The brush of the component, which is used to display text.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionBrushConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionBrushEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public virtual StiBrush TextBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.Black);


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiOrder(180)]
	[StiSerializable]
	[Description("Gets or sets font of component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Font Font
	{
		get
		{
			return font;
		}
		set
		{
			if (value != null || !base.IsDesigning)
			{
				font = value;
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(200)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue("")]
	[Description("Gets or sets mathematical expression.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string LaTexExpression
	{
		get
		{
			return laTexExpression;
		}
		set
		{
			if (value != laTexExpression)
			{
				laTexExpression = value.Trim();
			}
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public string Value
	{
		get
		{
			return value;
		}
		set
		{
			this.value = value;
		}
	}

	public override int Priority => 0;

	public override string LocalizedCategory => StiLocalization.Get("Report", "Components");

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 100.0, 30.0);

	public override StiComponentType ComponentType => StiComponentType.Simple;

	public override int ToolboxPosition => 230;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Components;

	public override string LocalizedName => StiLocalization.Get("Components", "StiMathFormula");

	public override string HelpUrl => "user-manual/index.html?report_internals_math_formula.htm";

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Invalid comparison between Unknown and I4
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "LaTexExpression", LaTexExpression);
		JsonExtensions.AddPropertyEnum(val, "HorAlignment", (Enum)(object)HorAlignment, (Enum)(object)(StiHorAlignment)1);
		JsonExtensions.AddPropertyEnum(val, "VertAlignment", (Enum)(object)VertAlignment, (Enum)(object)(StiVertAlignment)0);
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyBorder(val, "Border", Border);
		JsonExtensions.AddPropertyFontArial8(val, "Font", Font);
		JsonExtensions.AddPropertyBrush(val, "TextBrush", TextBrush);
		if ((int)mode == 1)
		{
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "Value", Value);
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "HorAlignment":
				HorAlignment = StiJsonReportObjectHelper.DeserializeEnum<StiHorAlignment>(item);
				break;
			case "VertAlignment":
				VertAlignment = StiJsonReportObjectHelper.DeserializeEnum<StiVertAlignment>(item);
				break;
			case "LaTexExpression":
				LaTexExpression = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Border":
				Border = StiJsonReportObjectHelper.DeserializeBorder(item);
				break;
			case "Font":
				font = StiJsonReportObjectHelper.DeserializeFont(item, font);
				break;
			case "TextBrush":
				TextBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Value":
				Value = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Invalid comparison between Unknown and I4
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Invalid comparison between Unknown and I4
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyObject[] array = new StiPropertyObject[8]
		{
			propertiesHelper.HorAlignment(),
			propertiesHelper.VertAlignment(),
			propertiesHelper.Brush(),
			propertiesHelper.Border(),
			propertiesHelper.Conditions(),
			propertiesHelper.ComponentStyle(),
			propertiesHelper.Font(),
			propertiesHelper.TextBrush()
		};
		stiPropertyCollection.Add(StiPropertyCategories.Appearance, array);
		array = (((int)level != 0) ? new StiPropertyObject[6]
		{
			propertiesHelper.Left(),
			propertiesHelper.Top(),
			propertiesHelper.Width(),
			propertiesHelper.Height(),
			propertiesHelper.MinSize(),
			propertiesHelper.MaxSize()
		} : new StiPropertyObject[4]
		{
			propertiesHelper.Left(),
			propertiesHelper.Top(),
			propertiesHelper.Width(),
			propertiesHelper.Height()
		});
		stiPropertyCollection.Add(StiPropertyCategories.Position, array);
		array = (((int)level == 0) ? new StiPropertyObject[4]
		{
			propertiesHelper.CanGrow(),
			propertiesHelper.CanShrink(),
			propertiesHelper.Enabled(),
			propertiesHelper.GrowToHeight()
		} : (((int)level != 1) ? new StiPropertyObject[10]
		{
			propertiesHelper.AnchorMode(),
			propertiesHelper.CanGrow(),
			propertiesHelper.CanShrink(),
			propertiesHelper.DockStyle(),
			propertiesHelper.Enabled(),
			propertiesHelper.GrowToHeight(),
			propertiesHelper.InteractionEditor(),
			propertiesHelper.Printable(),
			propertiesHelper.PrintOn(),
			propertiesHelper.ShiftMode()
		} : new StiPropertyObject[9]
		{
			propertiesHelper.AnchorMode(),
			propertiesHelper.CanGrow(),
			propertiesHelper.CanShrink(),
			propertiesHelper.DockStyle(),
			propertiesHelper.Enabled(),
			propertiesHelper.GrowToHeight(),
			propertiesHelper.InteractionEditor(),
			propertiesHelper.PrintOn(),
			propertiesHelper.ShiftMode()
		}));
		stiPropertyCollection.Add(StiPropertyCategories.Behavior, array);
		array = (((int)level == 0) ? new StiPropertyObject[1] { propertiesHelper.Name() } : (((int)level != 1) ? new StiPropertyObject[5]
		{
			propertiesHelper.Name(),
			propertiesHelper.Alias(),
			propertiesHelper.Restrictions(),
			propertiesHelper.Locked(),
			propertiesHelper.Linked()
		} : new StiPropertyObject[2]
		{
			propertiesHelper.Name(),
			propertiesHelper.Alias()
		}));
		stiPropertyCollection.Add(StiPropertyCategories.Design, array);
		array = new StiPropertyObject[1] { propertiesHelper.LaTexExpression() };
		stiPropertyCollection.Add(StiPropertyCategories.Main, array);
		return stiPropertyCollection;
	}

	public virtual Image GetImage(ref float zoom)
	{
		return GetImage(ref zoom, StiExportFormat.None);
	}

	public virtual Image GetImage(ref float zoom, StiExportFormat format)
	{
		StiPainter painter = StiPainter.GetPainter(((object)this).GetType(), (StiGuiMode)0);
		return painter.GetImage(this, ref zoom, format);
	}

	[Browsable(false)]
	public override bool IsExportAsImage(StiExportFormat format)
	{
		return true;
	}

	public override object Clone(bool cloneProperties)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		StiMathFormula stiMathFormula = (StiMathFormula)base.Clone(cloneProperties);
		StiBorder border = Border;
		object obj = ((border != null) ? border.Clone() : null);
		stiMathFormula.Border = (StiBorder)((obj is StiBorder) ? obj : null);
		StiBrush brush = Brush;
		object obj2 = ((brush != null) ? brush.Clone() : null);
		stiMathFormula.Brush = (StiBrush)((obj2 is StiBrush) ? obj2 : null);
		StiBrush textBrush = TextBrush;
		object obj3 = ((textBrush != null) ? textBrush.Clone() : null);
		stiMathFormula.TextBrush = (StiBrush)((obj3 is StiBrush) ? obj3 : null);
		stiMathFormula.HorAlignment = HorAlignment;
		stiMathFormula.VertAlignment = VertAlignment;
		return stiMathFormula;
	}

	private bool ShouldSerializeBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (Brush is StiSolidBrush)
		{
			return !(((StiSolidBrush)Brush).Color == Color.Transparent);
		}
		return true;
	}

	private bool ShouldSerializeBorder()
	{
		if (Border != null)
		{
			return !Border.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeTextBrush()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (TextBrush is StiSolidBrush)
		{
			return !(((StiSolidBrush)TextBrush).Color == Color.Black);
		}
		return true;
	}

	private bool ShouldSerializeFont()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (Font != null && Font.Name == "Arial" && (int)font.Style == 0)
		{
			return font.Size != 8f;
		}
		return true;
	}

	public override StiComponent CreateNew()
	{
		return new StiMathFormula();
	}

	public string GetFormulaString()
	{
		try
		{
			if (Report == null || Report.IsDesigning)
			{
				return LaTexExpression;
			}
			string text = ReplaceStimulsoftLatexBracket(LaTexExpression);
			text = text.Replace("{", "__LP__");
			text = text.Replace("}", "__RP__");
			text = text.Replace("__LPE__", "{");
			text = text.Replace("__RPE__", "}");
			string text2 = System.Convert.ToString(StiParser.ParseTextValue(text, this));
			if (!string.IsNullOrWhiteSpace(text2))
			{
				text = text2.Replace("__LP__", "{");
				return text.Replace("__RP__", "}");
			}
		}
		catch
		{
		}
		return LaTexExpression;
	}

	private static string ReplaceStimulsoftLatexBracket(string formula)
	{
		Regex regex = new Regex("(?<beginExpression>\\\\sti{)(?<expression>.[^{}]*)(?<endExpression>})");
		formula = regex.Replace(formula, (Match m) => ProcessReplaceStimulsoftLatexBracket(m.Groups["beginExpression"].Value, m.Groups["expression"].Value, m.Groups["endExpression"].Value));
		return formula;
	}

	private static string ProcessReplaceStimulsoftLatexBracket(string beginExpression, string expression, string endExpression)
	{
		return "__LPE__" + expression + "__RPE__";
	}

	public StiMathFormula()
		: base(RectangleD.Empty)
	{
	}//IL_0002: Unknown result type (might be due to invalid IL or missing references)
	//IL_0008: Unknown result type (might be due to invalid IL or missing references)
	//IL_0012: Expected O, but got Unknown
	//IL_0013: Unknown result type (might be due to invalid IL or missing references)
	//IL_001d: Expected O, but got Unknown
	//IL_0023: Unknown result type (might be due to invalid IL or missing references)
	//IL_002d: Expected O, but got Unknown
	//IL_0038: Unknown result type (might be due to invalid IL or missing references)
	//IL_0042: Expected O, but got Unknown
	//IL_0043: Unknown result type (might be due to invalid IL or missing references)


	public StiMathFormula(RectangleD rect)
		: base(rect)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
	}
}
