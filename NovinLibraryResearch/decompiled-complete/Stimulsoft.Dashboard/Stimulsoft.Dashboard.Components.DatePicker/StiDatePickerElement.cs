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
using Stimulsoft.Base.Meters;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Dashboard.Components.Design;
using Stimulsoft.Dashboard.Components.Helpers;
using Stimulsoft.Dashboard.Export;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Dashboard.Visualizers;
using Stimulsoft.Data.Engine;
using Stimulsoft.Data.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Painters;

namespace Stimulsoft.Dashboard.Components.DatePicker;

[StiServiceBitmap(typeof(StiElement), "Stimulsoft.Dashboard.Images.Toolbox.StiDatePicker.png")]
[StiExportTool("Stimulsoft.Dashboard.Export.Tools.StiDatePickerElementExportTool, Stimulsoft.Dashboard.Export, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter("Stimulsoft.Dashboard.Drawing.Painters.StiDatePickerElementGdiPainter, Stimulsoft.Dashboard.Drawing, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Dashboard.Drawing.Wpf.Painters.StiDatePickerElementWpfPainter, Stimulsoft.Dashboard.Drawing.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiDesigner("Stimulsoft.Dashboard.Design.Designers.StiDatePickerElementDesigner, Stimulsoft.Dashboard.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Dashboard.WpfDesign.Designers.StiDatePickerElementWpfDesigner, Stimulsoft.Dashboard.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiVisualizer("Stimulsoft.Dashboard.Viewer.Visualizers.StiDatePickerElementVisualizer, Stimulsoft.Dashboard.Viewer, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfVisualizer("Stimulsoft.Dashboard.Viewer.Wpf.Visualizers.StiDatePickerElementWpfVisualizer, Stimulsoft.Dashboard.Viewer.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiContextTool(typeof(IStiComponentDesigner))]
public class StiDatePickerElement : StiElement, IStiDatePickerElement, IStiControlElement, IStiElement, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiQueryObject, IStiRetrieval, IStiDashboardElementStyle, IStiUserFilters, IStiFilterElement, IStiConvertibleElement, IStiFont, IStiForeColor, IStiBackColor, IStiTextFormat, IStiDataFilters, IStiSimpleShadow, IStiCornerRadius, IStiFixedHeightElement, IStiGroupElement
{
	private List<StiDataFilterRule> userFilters = new List<StiDataFilterRule>();

	private List<StiDataFilterRule> dataFilters = new List<StiDataFilterRule>();

	private StiElementStyleIdent style = StiElementStyleIdent.Auto;

	private string customStyleName = "";

	private Font font = new Font("Arial", 8f);

	private Color foreColor = Color.Transparent;

	private StiSimpleShadow shadow = new StiSimpleShadow();

	private StiCornerRadius cornerRadius = new StiCornerRadius(0f);

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiDatePickerElement;

	[StiSerializable]
	[StiCategory("DatePicker", StiCategorySource.PropertyCategory)]
	[DefaultValue("")]
	[StiOrder(101)]
	[StiPropertyLevel(StiLevel.Standard)]
	public string Group { get; set; } = "";


	[Browsable(false)]
	public bool IsFixedHeight => true;

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

	[StiSerializable(StiSerializationVisibility.List)]
	[Browsable(false)]
	public virtual List<StiDataFilterRule> DataFilters
	{
		get
		{
			return dataFilters;
		}
		set
		{
			if (dataFilters != value)
			{
				dataFilters = value;
				InvokeRefreshControl("DataFilters");
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

	[StiSerializable]
	[StiCategory("Appearance", StiCategorySource.PropertyCategory)]
	[Description("The font used to display text in the component.")]
	[StiOrder(180)]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
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

	[Browsable(false)]
	public override bool IsDefined => ValueMeter != null;

	[Editor("Stimulsoft.Report.Components.TextFormats.Design.StiTextFormatEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable(StiSerializationVisibility.Class)]
	[StiCategory("Behavior", StiCategorySource.PropertyCategory)]
	[StiOrder(480)]
	[StiPropertyLevel(StiLevel.Standard)]
	public virtual StiFormatService TextFormat { get; set; } = new StiDateFormatService();


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

	public override int ToolboxPosition => 317;

	public override string LocalizedName => StiLocalization.Get("Components", "StiDatePicker");

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 200.0, 40.0);

	[StiNonSerialized]
	[Browsable(false)]
	public override SizeD MinSize
	{
		get
		{
			StiPage stiPage = Report?.GetCurrentPage();
			if (stiPage == null)
			{
				return SizeD.Empty;
			}
			double num = StiAlignValue.AlignToMaxGrid(DefaultClientRectangle.Height, stiPage.GridSize, aligningToGrid: true);
			return new SizeD(0.0, num);
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override SizeD MaxSize
	{
		get
		{
			StiPage stiPage = Report?.GetCurrentPage();
			if (stiPage == null)
			{
				return SizeD.Empty;
			}
			double num = StiAlignValue.AlignToMaxGrid(DefaultClientRectangle.Height, stiPage.GridSize, aligningToGrid: true);
			return new SizeD(0.0, num);
		}
		set
		{
		}
	}

	public override string HelpUrl => "user-manual/dashboards_data_filtering_date_picker.htm";

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

	[StiSerializable]
	[DefaultValue(StiDateCondition.GreaterThanOrEqualTo)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Browsable(false)]
	public StiDateCondition Condition { get; set; } = StiDateCondition.GreaterThanOrEqualTo;


	[StiSerializable]
	[DefaultValue(StiDateSelectionMode.Single)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Browsable(false)]
	public StiDateSelectionMode SelectionMode { get; set; }

	[StiSerializable]
	[DefaultValue(StiDateSelectionMode.Single)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Browsable(false)]
	public StiInitialDateRangeSelectionSource InitialRangeSelectionSource { get; set; } = StiInitialDateRangeSelectionSource.Variable;


	[StiSerializable(StiSerializationVisibility.Class)]
	[DefaultValue(null)]
	[Browsable(false)]
	public StiValueDatePickerMeter ValueMeter { get; set; }

	[Browsable(false)]
	[StiSerializable]
	[DefaultValue(StiInitialDateRangeSelection.MonthPrevious)]
	[TypeConverter(typeof(StiEnumConverter))]
	public StiInitialDateRangeSelection InitialRangeSelection { get; set; } = StiInitialDateRangeSelection.MonthPrevious;


	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		JObject jObject = base.SaveToJsonObject(mode);
		jObject.AddPropertyJObject("DataFilters", StiJsonReportObjectHelper.Serialize.JObjectCollection(DataFilters, mode));
		jObject.AddPropertyColor("ForeColor", ForeColor, Color.Transparent);
		jObject.AddPropertyFontArial8("Font", Font);
		jObject.AddPropertyEnum("Condition", Condition, StiDateCondition.GreaterThanOrEqualTo);
		jObject.AddPropertyEnum("SelectionMode", SelectionMode, StiDateSelectionMode.Single);
		jObject.AddPropertyEnum("InitialRangeSelection", InitialRangeSelection, StiInitialDateRangeSelection.MonthPrevious);
		if (ValueMeter != null)
		{
			jObject.AddPropertyJObject("ValueMeter", ValueMeter.SaveToJsonObject(mode));
		}
		jObject.AddPropertyEnum("Style", Style, StiElementStyleIdent.Auto);
		jObject.AddPropertyStringNullOrEmpty("Group", Group);
		jObject.AddPropertyStringNullOrEmpty("CustomStyleName", CustomStyleName);
		jObject.AddPropertyCornerRadius("CornerRadius", CornerRadius);
		jObject.AddPropertyShadow("Shadow", Shadow);
		if (!(TextFormat is StiGeneralFormatService) && TextFormat != null)
		{
			jObject.AddPropertyJObject("TextFormat", TextFormat.SaveToJsonObject(mode));
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
			case "DataFilters":
				DataFilters.AddRange(from p in ((JObject)item.Value).Properties()
					select StiDataFilterRule.LoadFromJson((JObject)p.Value));
				break;
			case "ForeColor":
				ForeColor = item.DeserializeColor();
				break;
			case "Font":
				Font = item.DeserializeFont(Font);
				break;
			case "Condition":
				Condition = item.DeserializeEnum<StiDateCondition>();
				break;
			case "SelectionMode":
				SelectionMode = item.DeserializeEnum<StiDateSelectionMode>();
				break;
			case "InitialRangeSelection":
				InitialRangeSelection = item.DeserializeEnum<StiInitialDateRangeSelection>();
				break;
			case "ValueMeter":
			{
				StiValueDatePickerMeter stiValueDatePickerMeter = StiMeterLoader.LoadFromJson((JObject)item.Value) as StiValueDatePickerMeter;
				ValueMeter = stiValueDatePickerMeter ?? null;
				break;
			}
			case "Style":
				Style = item.DeserializeEnum<StiElementStyleIdent>();
				break;
			case "Group":
				Group = item.DeserializeString();
				break;
			case "CustomStyleName":
				CustomStyleName = item.DeserializeString();
				break;
			case "TextFormat":
				TextFormat = StiFormatService.CreateFromJsonObject((JObject)item.Value);
				break;
			case "Shadow":
				Shadow = item.DeserializeSimpleShadow();
				break;
			case "CornerRadius":
				CornerRadius = item.DeserializeCornerRadius();
				break;
			}
		}
	}

	public override object Clone(bool cloneProperties)
	{
		StiDatePickerElement stiDatePickerElement = ((StiElement)base.Clone(cloneProperties)) as StiDatePickerElement;
		stiDatePickerElement.ValueMeter = ValueMeter?.Clone() as StiValueDatePickerMeter;
		stiDatePickerElement.UserFilters = UserFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiDatePickerElement.DataFilters = DataFilters.Select((StiDataFilterRule c) => c.Clone() as StiDataFilterRule).ToList();
		stiDatePickerElement.Shadow = (StiSimpleShadow)(Shadow?.Clone());
		stiDatePickerElement.CornerRadius = CornerRadius?.Clone() as StiCornerRadius;
		return stiDatePickerElement;
	}

	string IStiFilterElement.GetParentKey()
	{
		return null;
	}

	void IStiFilterElement.SetParentKey(string key)
	{
	}

	void IStiFilterElement.ApplyDefaultFilters()
	{
		if (!(StiDatePickerHelper.GetVariableSpecifiedAsValue(this) is StiVariable stiVariable))
		{
			List<StiDataFilterRule> list = StiDatePickerHelper.FetchDefaultUserFilters(this);
			if (list != null)
			{
				UserFilters = list;
			}
		}
		else if (Range.IsRangeType(stiVariable.Type) && InitialRangeSelectionSource == StiInitialDateRangeSelectionSource.Selection)
		{
			StiDatePickerHelper.CalculateRangeFromInitial(InitialRangeSelection, out var start, out var end);
			DateTimeRange dateTimeRange = new DateTimeRange(start, end);
			StiVariableHelper.SetVariableValue(Report, stiVariable, dateTimeRange);
			if (stiVariable.InitBy == StiVariableInitBy.Value)
			{
				stiVariable.ValueObject = dateTimeRange;
			}
		}
	}

	private bool ShouldSerializeFont()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		if (Font != null && Font.Name == "Arial" && Font.SizeInPoints == 8f)
		{
			return (int)Font.Style > 0;
		}
		return true;
	}

	private bool ShouldSerializeForeColor()
	{
		return ForeColor != Color.Transparent;
	}

	public override List<IStiMeter> FetchAllMeters()
	{
		List<IStiMeter> list = new List<IStiMeter>();
		if (ValueMeter != null)
		{
			list.Add(ValueMeter);
		}
		return list;
	}

	public override List<IStiMeter> GetMeters()
	{
		List<IStiMeter> list = new List<IStiMeter>();
		if (StiDatePickerHelper.IsVariableSpecifiedAsValue(this))
		{
			list.Add(ValueMeter);
		}
		else if (ValueMeter != null && SelectionMode == StiDateSelectionMode.AutoRange)
		{
			list.Add(ValueMeter);
		}
		return list;
	}

	public override List<string> RetrieveUsedDataNames()
	{
		if (ValueMeter != null && SelectionMode == StiDateSelectionMode.AutoRange)
		{
			return StiUsedDataHelper.GetMany(ValueMeter);
		}
		return base.RetrieveUsedDataNames();
	}

	public void AddValueMeter(IStiAppDataCell cell)
	{
		ValueMeter = ((cell != null) ? StiMeterHelper.DatePicker.GetValue(cell) : null);
	}

	public void AddValueMeter(IStiMeter meter)
	{
		ValueMeter = ((meter != null) ? StiMeterHelper.DatePicker.GetValue(meter as StiMeter) : null);
	}

	public IStiMeter GetValueMeter()
	{
		return ValueMeter;
	}

	public void RemoveValueMeter()
	{
		ValueMeter = null;
	}

	public void CreateNewValueMeter()
	{
		ValueMeter = new StiValueDatePickerMeter();
	}

	public void ConvertFrom(IStiElement element)
	{
		List<IStiMeter> list = element.FetchAllMeters();
		StiMeter stiMeter = list?.FirstOrDefault((IStiMeter m) => m is IStiValueMeter) as StiMeter;
		stiMeter = stiMeter ?? (list?.FirstOrDefault((IStiMeter m) => m is IStiMeasureMeter) as StiMeter);
		if (stiMeter != null)
		{
			ValueMeter = ValueMeter ?? new StiValueDatePickerMeter();
			ValueMeter.LoadFromJsonObject(stiMeter.SaveToJsonObject(StiJsonSaveMode.Report));
		}
	}

	public override List<StiFont> GetFonts()
	{
		List<StiFont> fonts = base.GetFonts();
		fonts.Add(new StiFont(Font));
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

	public override StiComponent CreateNew()
	{
		return new StiDatePickerElement();
	}

	public StiDatePickerElement()
		: this(RectangleD.Empty)
	{
	}

	public StiDatePickerElement(RectangleD rect)
		: base(rect)
	{
	}//IL_003e: Unknown result type (might be due to invalid IL or missing references)
	//IL_0048: Expected O, but got Unknown

}
