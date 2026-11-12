using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Dashboard.Components.Text.Design;
using Stimulsoft.Dashboard.Export;
using Stimulsoft.Dashboard.Interactions;
using Stimulsoft.Dashboard.Interactions.Design;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Styles;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Components.Text;

[StiServiceBitmap(typeof(StiElement), "Stimulsoft.Dashboard.Images.Toolbox.StiText.png")]
[StiExportTool("Stimulsoft.Dashboard.Export.Tools.StiTextElementExportTool, Stimulsoft.Dashboard.Export, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter("Stimulsoft.Dashboard.Drawing.Painters.StiTextElementGdiPainter, Stimulsoft.Dashboard.Drawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Dashboard.Drawing.Wpf.Painters.StiTextElementWpfPainter, Stimulsoft.Dashboard.Drawing.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDesigner("Stimulsoft.Dashboard.Design.Designers.StiTextElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Dashboard.WpfDesign.Designers.StiTextElementWpfDesigner, Stimulsoft.Dashboard.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiVisualizer("Stimulsoft.Dashboard.Viewer.Visualizers.StiTextElementVisualizer, Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfVisualizer("Stimulsoft.Dashboard.Viewer.Wpf.Visualizers.StiTextElementWpfVisualizer, Stimulsoft.Dashboard.Viewer.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiContextTool(typeof(IStiTitleElement))]
public class StiTextElement : StiElement, IStiSimpleShadow, IStiCornerRadius, IStiTextFont, IStiForeColor, IStiTextHorAlignment, IStiVertAlignment, IStiTextElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiGroupElement, IStiCrossFiltering, IStiTitleElement, IStiElementInteraction, IStiGlobalizationProvider
{
	private string text = "";

	private Color foreColor = Color.Transparent;

	private StiTextHorAlignment? horAlignment;

	private StiVertAlignment vertAlignment = StiVertAlignment.Center;

	private StiSimpleShadow shadow = new StiSimpleShadow();

	private StiCornerRadius cornerRadius = new StiCornerRadius(0f);

	private StiTextSizeMode sizeMode = StiTextSizeMode.Fit;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiTextElement;

	[Browsable(false)]
	public override bool IsDefined => !string.IsNullOrWhiteSpace(Text);

	[Browsable(false)]
	public override bool IsQuerable
	{
		get
		{
			if (StiOptions.Engine.DashboardTextElementExpressionParser == StiParserType.DataParser && !string.IsNullOrEmpty(Text))
			{
				return Text.Contains("{");
			}
			return false;
		}
	}

	[StiSerializable(StiSerializationVisibility.Class)]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(490)]
	[TypeConverter(typeof(StiTitleConverter))]
	public IStiTitle Title { get; set; } = new StiTitle();


	[DefaultValue("")]
	[StiSerializable]
	[StiCategory("Text", StiCategorySource.PropertyCategory)]
	[TypeConverter(typeof(StiTextConverter))]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiElementEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiOrder(100)]
	public string Text
	{
		get
		{
			return text;
		}
		set
		{
			if (text != value)
			{
				text = value;
				horAlignment = null;
				InvokeRefreshControl("Text");
			}
		}
	}

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory("Text", StiCategorySource.PropertyCategory)]
	[StiOrder(250)]
	[StiPropertyLevel(StiLevel.Standard)]
	public bool RightToLeft { get; set; }

	[StiSerializable]
	[StiCategory("Text", StiCategorySource.PropertyCategory)]
	[DefaultValue("")]
	[StiOrder(102)]
	[StiPropertyLevel(StiLevel.Standard)]
	public string Group { get; set; } = "";


	[StiSerializable]
	[StiCategory("Text", StiCategorySource.PropertyCategory)]
	[DefaultValue(true)]
	[StiOrder(100)]
	[StiPropertyLevel(StiLevel.Standard)]
	[TypeConverter(typeof(StiBoolConverter))]
	public bool CrossFiltering { get; set; } = true;


	[StiSerializable]
	[TypeConverter(typeof(StiStyleColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionStyleColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[Description("The foreground color of this component, which is used to display text.")]
	[StiOrder(210)]
	[StiExpressionAllowed]
	public Color ForeColor
	{
		get
		{
			return foreColor;
		}
		set
		{
			if (foreColor != value)
			{
				foreColor = value;
				InvokeRefreshControl("ForeColor");
			}
		}
	}

	[DefaultValue(StiTextHorAlignment.Center)]
	[StiNonSerialized]
	[TypeConverter(typeof(StiEnumConverter))]
	[Browsable(false)]
	[JsonIgnore]
	public virtual StiTextHorAlignment HorAlignment
	{
		get
		{
			if (!horAlignment.HasValue)
			{
				IStiHtmlTextHelper htmlTextHelper = GetHtmlTextHelper();
				horAlignment = htmlTextHelper.GetHorAlign(Text, StiDashboardStyleHelper.GetForeColor(this));
			}
			return horAlignment.Value;
		}
		set
		{
			Text = GetHtmlTextHelper()?.SetHorAlignment(Text, value, StiDashboardStyleHelper.GetForeColor(this));
			InvokeRefreshControl("HorAlignment");
		}
	}

	[StiSerializable]
	[DefaultValue(StiVertAlignment.Center)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Browsable(false)]
	public virtual StiVertAlignment VertAlignment
	{
		get
		{
			return vertAlignment;
		}
		set
		{
			if (vertAlignment != value)
			{
				vertAlignment = value;
				InvokeRefreshControl("VertAlignment");
			}
		}
	}

	[StiSerializable]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(300)]
	[Description("Specifies the interior spacing of the component.")]
	[StiPropertyLevel(StiLevel.Basic)]
	public override StiPadding Padding
	{
		get
		{
			return base.Padding;
		}
		set
		{
			base.Padding = value;
		}
	}

	[StiSerializable(StiSerializationVisibility.Class)]
	[TypeConverter(typeof(StiTextDashboardInteractionConverter))]
	[Browsable(false)]
	public virtual IStiDashboardInteraction DashboardInteraction { get; set; }

	[Description("Shadow settings of the component.")]
	[StiSerializable]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(276)]
	public StiSimpleShadow Shadow
	{
		get
		{
			return shadow;
		}
		set
		{
			if (shadow != value)
			{
				shadow = value;
				InvokeRefreshControl("Shadow");
			}
		}
	}

	[Description("Represents the value to which the corners are rounded.")]
	[StiSerializable]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(150)]
	public StiCornerRadius CornerRadius
	{
		get
		{
			return cornerRadius;
		}
		set
		{
			if (cornerRadius != value)
			{
				cornerRadius = value;
				InvokeRefreshControl("CornerRadius");
			}
		}
	}

	[Editor("Stimulsoft.Report.Components.Design.StiElementInteractionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiOrder(205)]
	[StiSerializable(StiSerializationVisibility.Class)]
	[TypeConverter(typeof(StiDashboardInteractionConverter))]
	[Browsable(true)]
	public override StiInteraction Interaction
	{
		get
		{
			return base.Interaction;
		}
		set
		{
			base.Interaction = value;
		}
	}

	public override int ToolboxPosition => 312;

	public override string LocalizedName => StiLocalization.Get("Components", "StiText");

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 100.0, 40.0);

	public override string HelpUrl => "user-manual/dashboards_text.htm";

	[StiEventHide]
	[StiNonSerialized]
	public override StiClickEvent ClickEvent
	{
		get
		{
			return base.ClickEvent;
		}
		set
		{
			base.ClickEvent = value;
		}
	}

	[Description("Specifies how the text output in the component area depends on the text size.")]
	[StiSerializable]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(StiLevel.Standard)]
	[StiCategory("Text", StiCategorySource.PropertyCategory)]
	[StiOrder(165)]
	public StiTextSizeMode SizeMode
	{
		get
		{
			return sizeMode;
		}
		set
		{
			if (sizeMode != value)
			{
				sizeMode = value;
				InvokeRefreshControl("SizeMode");
			}
		}
	}

	public override object Clone(bool cloneProperties)
	{
		StiTextElement stiTextElement = base.Clone(cloneProperties) as StiTextElement;
		((IStiTitleElement)stiTextElement).Title = ((StiTitle)Title)?.Clone() as StiTitle;
		stiTextElement.Shadow = (StiSimpleShadow)(Shadow?.Clone());
		stiTextElement.CornerRadius = CornerRadius?.Clone() as StiCornerRadius;
		return stiTextElement;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.RemoveProperty("HorAlignment");
		jObject.RemoveProperty("Padding");
		jObject.AddPropertyJObject("Padding", Padding.SaveToJsonObject(mode, 0.0, 0.0, 0.0, 0.0));
		jObject.AddPropertyColor("ForeColor", ForeColor, Color.Transparent);
		jObject.AddPropertyStringNullOrEmpty("Text", Text);
		jObject.AddPropertyEnum("VertAlignment", VertAlignment, StiVertAlignment.Center);
		jObject.AddPropertyEnum("SizeMode", SizeMode, StiTextSizeMode.Fit);
		jObject.AddPropertyCornerRadius("CornerRadius", CornerRadius);
		jObject.AddPropertyShadow("Shadow", Shadow);
		if (Title != null)
		{
			jObject.AddPropertyJObject("Title", ((StiTitle)Title).SaveToJsonObject(mode));
		}
		jObject.AddPropertyStringNullOrEmpty("Group", Group);
		jObject.AddPropertyBool("CrossFiltering", CrossFiltering, defaultValue: true);
		if (DashboardInteraction is StiTextDashboardInteraction stiTextDashboardInteraction)
		{
			jObject.AddPropertyJObject("DashboardInteraction", stiTextDashboardInteraction.SaveToJsonObject(mode));
		}
		return jObject;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Text":
				Text = item.DeserializeString();
				break;
			case "VertAlignment":
				VertAlignment = item.DeserializeEnum<StiVertAlignment>();
				break;
			case "SizeMode":
				SizeMode = item.DeserializeEnum<StiTextSizeMode>();
				break;
			case "Title":
				Title = StiTitle.CreateFromJsonObject((JObject)item.Value);
				break;
			case "Group":
				Group = item.DeserializeString();
				break;
			case "CrossFiltering":
				CrossFiltering = item.DeserializeBool();
				break;
			case "ForeColor":
				ForeColor = item.DeserializeColor();
				break;
			case "DashboardInteraction":
			{
				StiTextDashboardInteraction stiTextDashboardInteraction = StiDashboardInteractionLoader.LoadInteractionFromJsonObject((JObject)item.Value) as StiTextDashboardInteraction;
				DashboardInteraction = stiTextDashboardInteraction ?? null;
				break;
			}
			case "Shadow":
				Shadow = item.DeserializeSimpleShadow();
				break;
			case "CornerRadius":
				CornerRadius = item.DeserializeCornerRadius();
				break;
			}
		}
	}

	public override List<IStiMeter> FetchAllMeters()
	{
		return GetMeters();
	}

	public override List<IStiMeter> GetMeters()
	{
		if (StiOptions.Engine.DashboardTextElementExpressionParser == StiParserType.ReportParser)
		{
			return new List<IStiMeter>();
		}
		if (string.IsNullOrEmpty(Text))
		{
			return null;
		}
		return StiExpressionHelper.FetchBlocksFromExpression(Text)?.Select((string e) => new StiTextMeter(e, e, e)).Cast<IStiMeter>().ToList();
	}

	public override List<string> RetrieveUsedDataNames()
	{
		if (StiOptions.Engine.DashboardTextElementExpressionParser == StiParserType.ReportParser)
		{
			return base.RetrieveUsedDataNames();
		}
		return StiUsedDataHelper.GetMany(GetMeters()).Distinct().ToList();
	}

	public string GetSimpleText()
	{
		return GetHtmlTextHelper()?.GetSimpleText(Text, StiDashboardStyleHelper.GetForeColor(this));
	}

	private bool ShouldSerializeForeColor()
	{
		return ForeColor != Color.Transparent;
	}

	public Font GetFont()
	{
		return GetHtmlTextHelper()?.GetFont(Text, StiDashboardStyleHelper.GetForeColor(this));
	}

	public void SetFontName(string fontName)
	{
		Text = GetHtmlTextHelper()?.SetFontName(Text, fontName, StiDashboardStyleHelper.GetForeColor(this));
	}

	public void SetFontSize(float fontSize)
	{
		Text = GetHtmlTextHelper()?.SetFontSize(Text, fontSize, StiDashboardStyleHelper.GetForeColor(this));
	}

	public void GrowFontSize()
	{
		Text = GetHtmlTextHelper()?.GrowFontSize(Text, StiDashboardStyleHelper.GetForeColor(this));
	}

	public void ShrinkFontSize()
	{
		Text = GetHtmlTextHelper()?.ShrinkFontSize(Text, StiDashboardStyleHelper.GetForeColor(this));
	}

	public void SetFontBoldStyle(bool isBold)
	{
		Text = GetHtmlTextHelper()?.SetFontBoldStyle(Text, isBold, StiDashboardStyleHelper.GetForeColor(this));
	}

	public void SetFontItalicStyle(bool isItalic)
	{
		Text = GetHtmlTextHelper()?.SetFontItalicStyle(Text, isItalic, StiDashboardStyleHelper.GetForeColor(this));
	}

	public void SetFontUnderlineStyle(bool isUnderline)
	{
		Text = GetHtmlTextHelper()?.SetFontUnderlineStyle(Text, isUnderline, StiDashboardStyleHelper.GetForeColor(this));
	}

	private bool ShouldSerializePadding()
	{
		return !Padding.IsEmpty;
	}

	void IStiGlobalizationProvider.SetString(string propertyName, string value)
	{
		if (!(propertyName == "Title.Text"))
		{
			if (propertyName == "Text")
			{
				Text = value;
			}
		}
		else
		{
			Title.Text = value;
		}
	}

	string IStiGlobalizationProvider.GetString(string propertyName)
	{
		if (!(propertyName == "Title.Text"))
		{
			if (propertyName == "Text")
			{
				return Text;
			}
			throw new ArgumentException("Property with name " + propertyName);
		}
		return Title.Text;
	}

	string[] IStiGlobalizationProvider.GetAllStrings()
	{
		return new string[2] { "Title.Text", "Text" };
	}

	private bool ShouldSerializeDashboardInteraction()
	{
		if (DashboardInteraction != null)
		{
			return !DashboardInteraction.IsDefault;
		}
		return true;
	}

	public override List<StiFont> GetFonts()
	{
		List<StiFont> fonts = base.GetFonts();
		Font font = GetFont();
		if (font != null)
		{
			fonts.Add(new StiFont(font));
		}
		if (font != null)
		{
			font.Dispose();
		}
		return fonts.Distinct().ToList();
	}

	private bool ShouldSerializeShadow()
	{
		if (Shadow != null)
		{
			return !Shadow.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeCornerRadius()
	{
		if (CornerRadius != null)
		{
			return !CornerRadius.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeInteraction()
	{
		if (DashboardInteraction != null)
		{
			return !DashboardInteraction.IsDefault;
		}
		return true;
	}

	public override List<StiPage> GetNestedPages()
	{
		if (DashboardInteraction == null || DashboardInteraction.OnClick == StiInteractionOnClick.None)
		{
			return null;
		}
		string pageKey = DashboardInteraction?.DrillDownPageKey;
		if (StiKeyHelper.IsEmptyKey(pageKey))
		{
			return null;
		}
		StiPage stiPage = Report.Pages.ToList().FirstOrDefault((StiPage p) => p.Guid == pageKey);
		if (stiPage == null)
		{
			return null;
		}
		return new List<StiPage> { stiPage };
	}

	public override StiComponent CreateNew()
	{
		return new StiTextElement();
	}

	private IStiHtmlTextHelper GetHtmlTextHelper()
	{
		return new StiHtmlTextHelper();
	}

	public StiTextElement()
		: this(RectangleD.Empty)
	{
	}

	public StiTextElement(RectangleD rect)
		: base(rect)
	{
		Title.Text = LocalizedName;
		Title.Visible = false;
		Padding = new StiPadding(0.0);
		DashboardInteraction = new StiTextDashboardInteraction();
	}
}
