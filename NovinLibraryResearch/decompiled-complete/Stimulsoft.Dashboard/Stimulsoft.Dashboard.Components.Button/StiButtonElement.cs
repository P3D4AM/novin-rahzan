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
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Dashboard.Components.Button.Design;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Dashboard.Export;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Components.Button;

[StiServiceBitmap(typeof(StiElement), "Stimulsoft.Dashboard.Images.Toolbox.StiButton.png")]
[StiExportTool("Stimulsoft.Dashboard.Export.Tools.StiButtonElementExportTool, Stimulsoft.Dashboard.Export, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter("Stimulsoft.Dashboard.Drawing.Painters.StiButtonElementGdiPainter, Stimulsoft.Dashboard.Drawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Dashboard.Drawing.Wpf.Painters.StiButtonElementWpfPainter, Stimulsoft.Dashboard.Drawing.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDesigner("Stimulsoft.Dashboard.Design.Designers.StiButtonElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Dashboard.WpfDesign.Designers.StiButtonElementWpfDesigner, Stimulsoft.Dashboard.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiVisualizer("Stimulsoft.Dashboard.Viewer.Visualizers.StiButtonElementVisualizer, Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfVisualizer("Stimulsoft.Dashboard.Viewer.Wpf.Visualizers.StiButtonElementWpfVisualizer, Stimulsoft.Dashboard.Viewer.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiContextTool(typeof(IStiComponentDesigner))]
public class StiButtonElement : StiElement, IStiButtonElement, IStiControlElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiDashboardElementStyle, IStiUserFilters, IStiFilterElement, IStiConvertibleElement, IStiFont, IStiForeColor, IStiBackColor, IStiTextFormat, IStiCornerRadius, IStiSimpleShadow, IStiSimpleBorder, IStiCopyStyleExt, IStiTextHorAlignment, IStiVertAlignment, IStiWordWrap, IStiFixedHeightElement, IStiBrush, IStiTextBrush
{
	private bool checkedValue;

	private StiButtonStretch stretch;

	private StiSimpleShadow shadow = new StiSimpleShadow();

	private StiElementStyleIdent style = StiElementStyleIdent.Auto;

	private string customStyleName = "";

	private List<StiDataFilterRule> userFilters = new List<StiDataFilterRule>();

	private Font font = new Font("Arial", 10f);

	private StiCornerRadius cornerRadius = new StiCornerRadius();

	private StiTextHorAlignment horAlignment = StiTextHorAlignment.Center;

	private StiVertAlignment vertAlignment = StiVertAlignment.Center;

	private bool wordWrap = true;

	private StiBrush brush = new StiStyleBrush();

	private StiBrush textBrush = new StiStyleBrush();

	private StiButtonVisualStates visualStates;

	private string text = "Button";

	private StiIconAlignment iconAlignment = StiIconAlignment.Left;

	private StiButtonElementIconSet iconSet;

	private StiBrush iconBrush = new StiStyleBrush();

	private static readonly object EventCheckedChanged = new object();

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiButtonElement;

	[Browsable(false)]
	public bool IsFixedHeight => Stretch == StiButtonStretch.StretchX;

	[StiSerializable]
	[DefaultValue(false)]
	[StiCategory("Button", StiCategorySource.PropertyCategory)]
	[StiOrder(110)]
	[TypeConverter(typeof(StiExpressionBoolConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionBoolEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets a value which indicates that this button is checked or not.")]
	[StiPropertyLevel(StiLevel.Basic)]
	[StiExpressionAllowed]
	public virtual bool Checked
	{
		get
		{
			return checkedValue;
		}
		set
		{
			if (checkedValue != value)
			{
				checkedValue = value;
				InvokeRefreshControl("Checked");
			}
		}
	}

	[StiSerializable]
	[DefaultValue(StiButtonShapeType.Rectangle)]
	[StiCategory("Button", StiCategorySource.PropertyCategory)]
	[StiOrder(277)]
	[StiPropertyLevel(StiLevel.Standard)]
	[TypeConverter(typeof(StiButtonShapeTypeConverter))]
	[Description("Indicates which shape is used to draw the form of the component.")]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiButtonShapeTypeEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public StiButtonShapeType ShapeType { get; set; }

	[StiSerializable]
	[DefaultValue(StiButtonStretch.StretchXY)]
	[StiCategory("Button", StiCategorySource.PropertyCategory)]
	[StiOrder(278)]
	[StiPropertyLevel(StiLevel.Standard)]
	[TypeConverter(typeof(StiButtonStretchConverter))]
	[Description("Describes how the component is resized to fill its allocated space.")]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiButtonStretchEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public StiButtonStretch Stretch
	{
		get
		{
			return stretch;
		}
		set
		{
			if (stretch == value)
			{
				return;
			}
			stretch = value;
			if (value == StiButtonStretch.StretchX && base.IsDesigning)
			{
				double? fixedHeight = GetFixedHeight();
				if (fixedHeight.HasValue)
				{
					Height = fixedHeight.GetValueOrDefault();
				}
			}
		}
	}

	[StiSerializable]
	[DefaultValue(StiButtonType.Button)]
	[StiCategory("Button", StiCategorySource.PropertyCategory)]
	[StiOrder(190)]
	[StiPropertyLevel(StiLevel.Basic)]
	[TypeConverter(typeof(StiButtonTypeConverter))]
	[RefreshProperties(RefreshProperties.All)]
	[Description("Describes the type of that component behaviour.")]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiButtonTypeEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public StiButtonType Type { get; set; }

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

	[StiSerializable]
	[DefaultValue(StiElementStyleIdent.Auto)]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(280)]
	[StiPropertyLevel(StiLevel.Basic)]
	[TypeConverter(typeof(StiElementStyleConverter))]
	[Description("Specifies the visual style used to draw this component.")]
	[Editor("Stimulsoft.Dashboard.Design.Components.StiElementStyleEditor, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public StiElementStyleIdent Style
	{
		get
		{
			return style;
		}
		set
		{
			if (style != value)
			{
				style = value;
				if (value != StiElementStyleIdent.Custom)
				{
					CustomStyleName = string.Empty;
				}
				InvokeRefreshControl("Style");
			}
		}
	}

	[StiSerializable]
	[DefaultValue("")]
	[Browsable(false)]
	public string CustomStyleName
	{
		get
		{
			return customStyleName;
		}
		set
		{
			if (customStyleName != value)
			{
				customStyleName = value;
				if (!string.IsNullOrWhiteSpace(value))
				{
					style = StiElementStyleIdent.Custom;
				}
				InvokeRefreshControl("CustomStyleName");
			}
		}
	}

	[Browsable(false)]
	public virtual List<StiDataFilterRule> UserFilters
	{
		get
		{
			return userFilters;
		}
		set
		{
			if (userFilters != value)
			{
				userFilters = value;
				InvokeRefreshControl("UserFilters");
			}
		}
	}

	[Browsable(false)]
	public StiFormatService TextFormat { get; set; } = new StiGeneralFormatService();


	[StiSerializable]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[Description("The font used to display text in the component.")]
	[StiOrder(180)]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(StiLevel.Basic)]
	public Font Font
	{
		get
		{
			return font;
		}
		set
		{
			if (font != value)
			{
				font = value;
				InvokeRefreshControl("Font");
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

	[DefaultValue(StiTextHorAlignment.Center)]
	[StiSerializable]
	[StiCategory("Button", StiCategorySource.PropertyCategory)]
	[StiOrder(160)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(StiLevel.Basic)]
	[Description("The horizontal alignment of the text that will be displayed on the component.")]
	public StiTextHorAlignment HorAlignment
	{
		get
		{
			return horAlignment;
		}
		set
		{
			if (horAlignment != value)
			{
				horAlignment = value;
				InvokeRefreshControl("HorAlignment");
			}
		}
	}

	[DefaultValue(StiVertAlignment.Center)]
	[StiSerializable]
	[StiCategory("Button", StiCategorySource.PropertyCategory)]
	[StiOrder(170)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiPropertyLevel(StiLevel.Basic)]
	[Description("The vertical alignment of the text that will be displayed on the component.")]
	public StiVertAlignment VertAlignment
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

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory("Button", StiCategorySource.PropertyCategory)]
	[StiOrder(200)]
	[Description("Indicates that the multiline text wraps words or not.")]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiPropertyLevel(StiLevel.Standard)]
	public bool WordWrap
	{
		get
		{
			return wordWrap;
		}
		set
		{
			if (wordWrap != value)
			{
				wordWrap = value;
				InvokeRefreshControl("WordWrap");
			}
		}
	}

	[StiSerializable]
	[Description("The brush, which is used to draw background.")]
	[TypeConverter(typeof(StiExpressionBrushConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionBrushEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiVisualStatePermission(StiVisualStatePermissionKind.AllowStyle)]
	[StiOrder(130)]
	[StiDefaultStringValue("StyleBrush")]
	[StiPropertyLevel(StiLevel.Basic)]
	public virtual StiBrush Brush
	{
		get
		{
			return brush;
		}
		set
		{
			if (brush != value)
			{
				brush = value;
				InvokeRefreshControl("Brush");
			}
		}
	}

	[StiSerializable]
	[Description("The brush of the component, which is used to display text.")]
	[TypeConverter(typeof(StiExpressionBrushConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionBrushEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiVisualStatePermission(StiVisualStatePermissionKind.AllowStyle)]
	[StiOrder(300)]
	[StiDefaultStringValue("StyleBrush")]
	[StiPropertyLevel(StiLevel.Basic)]
	public virtual StiBrush TextBrush
	{
		get
		{
			return textBrush;
		}
		set
		{
			if (textBrush != value)
			{
				textBrush = value;
				InvokeRefreshControl("TextBrush");
			}
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override Color BackColor
	{
		get
		{
			return StiBrush.ToColor(Brush);
		}
		set
		{
			Brush = new StiSolidBrush(value);
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public Color ForeColor
	{
		get
		{
			return StiBrush.ToColor(TextBrush);
		}
		set
		{
			TextBrush = new StiSolidBrush(value);
		}
	}

	public override int ToolboxPosition => 315;

	public override string LocalizedName => StiLocalization.Get("Components", "StiButton");

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 140.0, 40.0);

	public override string HelpUrl => "user-manual/dashboards_data_filtering_button.htm";

	[Description("Manages the different visual states of the component.")]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(350)]
	[StiPropertyLevel(StiLevel.Professional)]
	[StiSerializable(StiSerializationVisibility.Class)]
	[TypeConverter(typeof(StiButtonVisualStatesConverter))]
	public StiButtonVisualStates VisualStates
	{
		get
		{
			return visualStates;
		}
		set
		{
			if (value != visualStates)
			{
				visualStates = value;
				if (value != null)
				{
					value.Parent = this;
				}
				InvokeRefreshControl("VisualStates");
			}
		}
	}

	[Browsable(false)]
	public bool IsChecked
	{
		get
		{
			if (StiAppExpressionHelper.IsExpressionSpecified(this, "Checked"))
			{
				return StiAppExpressionParser.ParseBoolExpression(this, "Checked", allowDataLoading: true);
			}
			return Checked;
		}
	}

	[Browsable(false)]
	public override SizeD MinSize
	{
		get
		{
			if (!IsFixedHeight)
			{
				return base.MinSize;
			}
			double? fixedHeight = GetFixedHeight();
			if (!fixedHeight.HasValue)
			{
				return SizeD.Empty;
			}
			return new SizeD(0.0, fixedHeight.GetValueOrDefault(0.0));
		}
		set
		{
			if (!IsFixedHeight)
			{
				base.MinSize = value;
			}
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override SizeD MaxSize
	{
		get
		{
			if (!IsFixedHeight)
			{
				return base.MaxSize;
			}
			double? fixedHeight = GetFixedHeight();
			if (!fixedHeight.HasValue)
			{
				return SizeD.Empty;
			}
			return new SizeD(0.0, fixedHeight.GetValueOrDefault(0.0));
		}
		set
		{
			if (!IsFixedHeight)
			{
				base.MaxSize = value;
			}
		}
	}

	[Description("A text of the component.")]
	[StiCategory("Button", StiCategorySource.PropertyCategory)]
	[StiOrder(100)]
	[StiPropertyLevel(StiLevel.Basic)]
	[StiSerializable]
	public virtual string Text
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
				InvokeRefreshControl("Text");
			}
		}
	}

	[StiSerializable]
	[StiCategory("Button", StiCategorySource.PropertyCategory)]
	[DefaultValue("")]
	[StiOrder(120)]
	[StiPropertyLevel(StiLevel.Standard)]
	[Description("A value that is used to join radio buttons into one group.")]
	public string Group { get; set; } = "";


	[DefaultValue(StiIconAlignment.Left)]
	[StiSerializable]
	[StiCategory("Button", StiCategorySource.PropertyCategory)]
	[Description("The alignment of the icon that will be displayed on the component.")]
	[StiOrder(140)]
	[StiPropertyLevel(StiLevel.Standard)]
	public StiIconAlignment IconAlignment
	{
		get
		{
			return iconAlignment;
		}
		set
		{
			if (iconAlignment != value)
			{
				iconAlignment = value;
				InvokeRefreshControl("IconAlignment");
			}
		}
	}

	[StiCategory("Button", StiCategorySource.PropertyCategory)]
	[StiOrder(150)]
	[StiSerializable(StiSerializationVisibility.Class)]
	[StiPropertyLevel(StiLevel.Standard)]
	[Description("The set of icons used in the component.")]
	public StiButtonElementIconSet IconSet
	{
		get
		{
			return iconSet;
		}
		set
		{
			if (iconSet != value)
			{
				iconSet = value;
				InvokeRefreshControl("IconSet");
			}
		}
	}

	[StiSerializable]
	[Description("The icon brush of the component.")]
	[TypeConverter(typeof(StiExpressionBrushConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionBrushEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[StiOrder(255)]
	[StiVisualStatePermission(StiVisualStatePermissionKind.AllowStyle)]
	[StiDefaultStringValue("StyleBrush")]
	[StiPropertyLevel(StiLevel.Standard)]
	public virtual StiBrush IconBrush
	{
		get
		{
			return iconBrush;
		}
		set
		{
			if (iconBrush != value)
			{
				iconBrush = value;
				InvokeRefreshControl("IconBrush");
			}
		}
	}

	[StiCategory("Action", StiCategorySource.PropertyCategory)]
	[Description("Gets or sets a script of the event Click.")]
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

	[StiSerializable]
	[StiCategory("Button", StiCategorySource.PropertyCategory)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event CheckedChanged.")]
	public virtual StiCheckedChangedEvent CheckedChangedEvent
	{
		get
		{
			return new StiCheckedChangedEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	public event EventHandler CheckedChanged
	{
		add
		{
			base.Events.AddHandler(EventCheckedChanged, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventCheckedChanged, value);
		}
	}

	public override object Clone(bool cloneProperties)
	{
		StiButtonElement stiButtonElement = ((StiElement)base.Clone(cloneProperties)) as StiButtonElement;
		stiButtonElement.Brush = Brush?.Clone() as StiBrush;
		stiButtonElement.TextBrush = TextBrush?.Clone() as StiBrush;
		stiButtonElement.IconBrush = IconBrush?.Clone() as StiBrush;
		stiButtonElement.IconSet = IconSet?.Clone() as StiButtonElementIconSet;
		stiButtonElement.Shadow = Shadow?.Clone() as StiSimpleShadow;
		Font obj = Font;
		object obj2 = ((obj != null) ? obj.Clone() : null);
		stiButtonElement.Font = (Font)((obj2 is Font) ? obj2 : null);
		stiButtonElement.CornerRadius = CornerRadius?.Clone() as StiCornerRadius;
		stiButtonElement.VisualStates = VisualStates.Clone() as StiButtonVisualStates;
		return stiButtonElement;
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyStringNullOrEmpty("Text", Text);
		jObject.AddPropertyEnum("Style", Style, StiElementStyleIdent.Auto);
		jObject.AddPropertyEnum("Type", Type, StiButtonType.Button);
		jObject.AddPropertyEnum("ShapeType", ShapeType, StiButtonShapeType.Rectangle);
		jObject.AddPropertyEnum("Stretch", Stretch, StiButtonStretch.StretchXY);
		jObject.AddPropertyStringNullOrEmpty("CustomStyleName", CustomStyleName);
		jObject.AddPropertyStringNullOrEmpty("Group", Group);
		jObject.AddPropertyEnum("HorAlignment", HorAlignment, StiHorAlignment.Center);
		jObject.AddPropertyEnum("VertAlignment", VertAlignment, StiVertAlignment.Center);
		jObject.AddPropertyEnum("IconAlignment", IconAlignment, StiIconAlignment.Left);
		jObject.AddPropertyBool("WordWrap", WordWrap, defaultValue: true);
		jObject.AddPropertyCornerRadius("CornerRadius", CornerRadius);
		jObject.AddPropertyShadow("Shadow", Shadow);
		jObject.AddPropertyBrush("Brush", Brush);
		jObject.AddPropertyBrush("TextBrush", TextBrush);
		jObject.AddPropertyBrush("IconBrush", IconBrush);
		jObject.AddPropertyFontArial10("Font", Font);
		if (IconSet != null && !IconSet.IsDefault)
		{
			jObject.AddPropertyJObject("IconSet", IconSet.SaveToJsonObject(mode));
		}
		if (VisualStates != null)
		{
			jObject.AddPropertyJObject("VisualStates", VisualStates.SaveToJsonObject(mode));
		}
		jObject.AddPropertyJObject("ClickEvent", ClickEvent.SaveToJsonObject(mode));
		jObject.AddPropertyJObject("CheckedChangedEvent", CheckedChangedEvent.SaveToJsonObject(mode));
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
			case "Style":
				Style = item.DeserializeEnum<StiElementStyleIdent>();
				break;
			case "Type":
				Type = item.DeserializeEnum<StiButtonType>();
				break;
			case "ShapeType":
				ShapeType = item.DeserializeEnum<StiButtonShapeType>();
				break;
			case "Stretch":
				Stretch = item.DeserializeEnum<StiButtonStretch>();
				break;
			case "Brush":
				Brush = item.DeserializeBrush();
				break;
			case "TextBrush":
				TextBrush = item.DeserializeBrush();
				break;
			case "IconBrush":
				IconBrush = item.DeserializeBrush();
				break;
			case "IconSet":
				IconSet.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Font":
				Font = item.DeserializeFont(Font);
				break;
			case "CornerRadius":
				CornerRadius = item.DeserializeCornerRadius();
				break;
			case "Shadow":
				Shadow = item.DeserializeSimpleShadow();
				break;
			case "VisualStates":
				VisualStates.LoadFromJsonObject((JObject)item.Value);
				break;
			case "CustomStyleName":
				CustomStyleName = item.DeserializeString();
				break;
			case "Group":
				Group = item.DeserializeString();
				break;
			case "HorAlignment":
				HorAlignment = item.DeserializeEnum<StiTextHorAlignment>();
				break;
			case "VertAlignment":
				VertAlignment = item.DeserializeEnum<StiVertAlignment>();
				break;
			case "IconAlignment":
				IconAlignment = item.DeserializeEnum<StiIconAlignment>();
				break;
			case "WordWrap":
				WordWrap = item.DeserializeBool();
				break;
			case "ClickEvent":
				ClickEvent = new StiClickEvent();
				ClickEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			case "CheckedChangedEvent":
				CheckedChangedEvent = new StiCheckedChangedEvent();
				CheckedChangedEvent.LoadFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public IStiButtonElementIconSet GetIconSet()
	{
		return IconSet;
	}

	public IStiButtonVisualStates GetVisualStates()
	{
		return VisualStates;
	}

	private bool ShouldSerializeShadow()
	{
		if (Shadow != null)
		{
			return !Shadow.IsDefault;
		}
		return true;
	}

	public void ConvertFrom(IStiElement element)
	{
	}

	private bool ShouldSerializeFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (Font != null && Font.Name == "Arial" && Font.SizeInPoints == 10f)
		{
			return (int)Font.Style > 0;
		}
		return true;
	}

	string IStiFilterElement.GetParentKey()
	{
		return "";
	}

	void IStiFilterElement.SetParentKey(string key)
	{
	}

	void IStiFilterElement.ApplyDefaultFilters()
	{
	}

	private bool ShouldSerializeCornerRadius()
	{
		if (CornerRadius != null)
		{
			return !CornerRadius.IsDefault;
		}
		return true;
	}

	public override List<StiFont> GetFonts()
	{
		List<StiFont> fonts = base.GetFonts();
		fonts.Add(new StiFont(Font));
		fonts.AddRange(VisualStates.GetFonts());
		return fonts.Distinct().ToList();
	}

	private bool ShouldSerializeBrush()
	{
		return !(Brush is StiStyleBrush);
	}

	private bool ShouldSerializeTextBrush()
	{
		return !(TextBrush is StiStyleBrush);
	}

	public virtual void InvokeSetCopyStyle()
	{
		if (StiCopyStyleExtHub.VisualStateDefault != null)
		{
			LoadFromJsonObject(StiCopyStyleExtHub.VisualStateDefault);
		}
		VisualStates.InvokeSetCopyStyle();
	}

	public virtual void InvokeGetCopyStyle()
	{
		StiCopyStyleExtHub.VisualStateDefault = SaveToJsonObject(StiJsonSaveMode.Report);
		VisualStates.InvokeGetCopyStyle();
	}

	public override StiComponent CreateNew()
	{
		return new StiButtonElement();
	}

	private double? GetFixedHeight()
	{
		StiPage stiPage = Report?.GetCurrentPage();
		if (stiPage == null)
		{
			return null;
		}
		return StiAlignValue.AlignToMaxGrid(DefaultClientRectangle.Height, stiPage.GridSize, aligningToGrid: true);
	}

	public override StiEventsCollection GetEvents()
	{
		StiEventsCollection stiEventsCollection = base.GetEvents();
		if (ClickEvent != null)
		{
			stiEventsCollection.Add(ClickEvent);
		}
		if (CheckedChangedEvent != null)
		{
			stiEventsCollection.Add(CheckedChangedEvent);
		}
		return stiEventsCollection;
	}

	private bool ShouldSerializeVisualStates()
	{
		if (VisualStates != null)
		{
			return !VisualStates.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeIconSet()
	{
		if (IconSet != null)
		{
			return !IconSet.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeIconBrush()
	{
		return !(IconBrush is StiStyleBrush);
	}

	public void InvokeCheckedChanged(object sender, EventArgs e)
	{
		if (base.Events[EventCheckedChanged] is EventHandler eventHandler)
		{
			eventHandler(sender, e);
		}
		StiBlocklyHelper.InvokeBlockly(Report, sender, CheckedChangedEvent);
	}

	public StiButtonElement()
		: this(RectangleD.Empty)
	{
	}

	public StiButtonElement(RectangleD rect)
		: base(rect)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		IconSet = new StiButtonElementIconSet();
		VisualStates = new StiButtonVisualStates();
		Border.Side = StiBorderSides.All;
		Border.Size = 2.0;
	}
}
