using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiCheckBox), "Stimulsoft.Report.Images.Components.StiCheckBox.png")]
[StiGdiPainter(typeof(StiCheckBoxGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiCheckBoxWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiToolbox(true)]
[StiContextTool(typeof(IStiShift))]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiGrowToHeight))]
[StiContextTool(typeof(IStiCanShrink))]
[StiContextTool(typeof(IStiEditable))]
public class StiCheckBox : StiComponent, IStiBorder, IStiTextBrush, IStiEditable, IStiBrush, IStiBreakable, IStiExportImageExtended, IStiExportImage
{
	private bool canBreak;

	private static readonly object EventGetChecked = new object();

	private static readonly object EventGetExcelValue = new object();

	private StiCheckStyle checkStyle = StiCheckStyle.Check;

	public override StiComponentId ComponentId => StiComponentId.StiCheckBox;

	public override string HelpUrl => "User-Manual/report_internals_creating_lists_check_box.htm";

	[Browsable(false)]
	[StiNonSerialized]
	public override bool CanShrink
	{
		get
		{
			return base.CanShrink;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override bool CanGrow
	{
		get
		{
			return base.CanGrow;
		}
		set
		{
		}
	}

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
	public StiBrush TextBrush { get; set; } = (StiBrush)new StiSolidBrush(Color.Black);


	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[Description("Gets or sets value indicates that a component can be edited in the window of viewer.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool Editable { get; set; }

	[Browsable(false)]
	[StiNonSerialized]
	public virtual bool CanBreak
	{
		get
		{
			if (!canBreak)
			{
				if (GrowToHeight && Page != null)
				{
					return Height / Page.PageHeight > 0.5;
				}
				return false;
			}
			return true;
		}
		set
		{
			canBreak = value;
		}
	}

	public override int ToolboxPosition => 113;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Components;

	public override string LocalizedCategory => StiLocalization.Get("Report", "Components");

	public override string LocalizedName => StiLocalization.Get("Components", "StiCheckBox");

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when state is being checked.")]
	public StiGetCheckedEvent GetCheckedEvent
	{
		get
		{
			return new StiGetCheckedEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when the ExcelValue is calculated.")]
	public StiGetExcelValueEvent GetExcelValueEvent
	{
		get
		{
			return new StiGetExcelValueEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[Browsable(false)]
	[Description("Gets or sets checked value.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public object CheckedValue { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[StiSerializable]
	[Description("Gets or sets an expression which used to calculate check state.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiCheckedExpression Checked
	{
		get
		{
			return new StiCheckedExpression(this, "Checked");
		}
		set
		{
			value?.Set(this, "Checked", value.Value);
		}
	}

	[Browsable(false)]
	[Description("Gets or sets excel data value.")]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public string ExcelDataValue { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[StiSerializable]
	[Description("Gets or sets an expression used for export data to Excel.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiExcelValueExpression ExcelValue
	{
		get
		{
			return new StiExcelValueExpression(this, "ExcelValue");
		}
		set
		{
			value?.Set(this, "ExcelValue", value.Value);
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(155)]
	[StiSerializable]
	[TypeConverter(typeof(StiExpressionColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets a contour color.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiExpressionAllowed]
	public Color ContourColor { get; set; } = Color.Black;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[StiSerializable]
	[DefaultValue(1.0)]
	[Description("Gets or sets a contour size.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public double Size { get; set; } = 1.0;


	[StiNonSerialized]
	[Browsable(false)]
	[Editor("Stimulsoft.Report.Components.Design.StiCheckStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Obsolete("This property is obsoleted. Please use properties CheckStyleForTrue and CheckStyleForFalse.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiCheckStyle CheckStyle
	{
		get
		{
			return CheckStyleForTrue;
		}
		set
		{
			if (checkStyle == value)
			{
				CheckStyleForTrue = value;
				CheckStyleForFalse = StiCheckStyle.None;
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[StiSerializable]
	[DefaultValue("true/false")]
	[Description("Gets or sets string which describes true and false values.")]
	[Editor("Stimulsoft.Report.Components.Design.StiCheckValuesEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string Values { get; set; } = "true/false";


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[StiSerializable]
	[DefaultValue(StiCheckStyle.Check)]
	[Editor("Stimulsoft.Report.Components.Design.StiCheckStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets check style for true value.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiCheckStyle CheckStyleForTrue { get; set; } = StiCheckStyle.Check;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable]
	[DefaultValue(StiCheckStyle.None)]
	[Editor("Stimulsoft.Report.Components.Design.StiCheckStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets check style for false value.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiCheckStyle CheckStyleForFalse { get; set; } = StiCheckStyle.None;


	public event StiValueEventHandler GetChecked
	{
		add
		{
			base.Events.AddHandler(EventGetChecked, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventGetChecked, value);
		}
	}

	public event StiGetExcelValueEventHandler GetExcelValue
	{
		add
		{
			base.Events.AddHandler(EventGetExcelValue, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventGetExcelValue, value);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Invalid comparison between Unknown and I4
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "CanShrink");
		JsonExtensions.RemoveProperty(val, "CanGrow");
		JsonExtensions.AddPropertyBrush(val, "Brush", Brush);
		JsonExtensions.AddPropertyBorder(val, "Border", Border);
		JsonExtensions.AddPropertyBrush(val, "TextBrush", TextBrush);
		JsonExtensions.AddPropertyBool(val, "Editable", Editable, false, false);
		JsonExtensions.AddPropertyJObject(val, "GetCheckedEvent", GetCheckedEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetExcelValueEvent", GetExcelValueEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Checked", Checked.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ExcelDataValue", ExcelDataValue);
		JsonExtensions.AddPropertyJObject(val, "ExcelValue", ExcelValue.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyColor(val, "ContourColor", ContourColor, Color.Black);
		JsonExtensions.AddPropertyDouble(val, "Size", Size, 1.0);
		JsonExtensions.AddPropertyString(val, "Values", Values, "true/false");
		JsonExtensions.AddPropertyEnum(val, "CheckStyleForTrue", (Enum)CheckStyleForTrue, (Enum)StiCheckStyle.Check);
		JsonExtensions.AddPropertyEnum(val, "CheckStyleForFalse", (Enum)CheckStyleForFalse, (Enum)StiCheckStyle.None);
		if ((int)mode == 1 && CheckedValue != null)
		{
			JsonExtensions.AddPropertyString(val, "CheckedValue", CheckedValue.ToString());
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Expected O, but got Unknown
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Expected O, but got Unknown
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Brush":
				Brush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Border":
				Border = StiJsonReportObjectHelper.DeserializeBorder(item);
				break;
			case "TextBrush":
				TextBrush = StiJsonReportObjectHelper.DeserializeBrush(item);
				break;
			case "Editable":
				Editable = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "GetCheckedEvent":
			{
				StiGetCheckedEvent stiGetCheckedEvent = new StiGetCheckedEvent();
				stiGetCheckedEvent.LoadFromJsonObject((JObject)item.Value);
				GetCheckedEvent = stiGetCheckedEvent;
				break;
			}
			case "GetExcelValueEvent":
			{
				StiGetExcelValueEvent stiGetExcelValueEvent = new StiGetExcelValueEvent();
				stiGetExcelValueEvent.LoadFromJsonObject((JObject)item.Value);
				GetExcelValueEvent = stiGetExcelValueEvent;
				break;
			}
			case "Checked":
			{
				StiCheckedExpression stiCheckedExpression = new StiCheckedExpression();
				stiCheckedExpression.LoadFromJsonObject((JObject)item.Value);
				Checked = stiCheckedExpression;
				break;
			}
			case "ExcelDataValue":
				ExcelDataValue = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ExcelValue":
			{
				StiExcelValueExpression stiExcelValueExpression = new StiExcelValueExpression();
				stiExcelValueExpression.LoadFromJsonObject((JObject)item.Value);
				ExcelValue = stiExcelValueExpression;
				break;
			}
			case "ContourColor":
				ContourColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Size":
				Size = StiJsonReportObjectHelper.DeserializeDouble(item);
				break;
			case "Values":
				Values = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "CheckStyleForTrue":
				CheckStyleForTrue = StiJsonReportObjectHelper.DeserializeEnum<StiCheckStyle>(item);
				break;
			case "CheckStyleForFalse":
				CheckStyleForFalse = StiJsonReportObjectHelper.DeserializeEnum<StiCheckStyle>(item);
				break;
			case "CheckedValue":
				CheckedValue = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Invalid comparison between Unknown and I4
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Invalid comparison between Unknown and I4
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Check, new StiPropertyObject[1] { propertiesHelper.Checked() });
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Check, new StiPropertyObject[6]
			{
				propertiesHelper.Checked(),
				propertiesHelper.CheckStyleForTrue(),
				propertiesHelper.CheckStyleForFalse(),
				propertiesHelper.Editable(),
				propertiesHelper.Size(),
				propertiesHelper.CheckBoxValues()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[4]
			{
				propertiesHelper.Left(),
				propertiesHelper.Top(),
				propertiesHelper.Width(),
				propertiesHelper.Height()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Position, new StiPropertyObject[6]
			{
				propertiesHelper.Left(),
				propertiesHelper.Top(),
				propertiesHelper.Width(),
				propertiesHelper.Height(),
				propertiesHelper.MinSize(),
				propertiesHelper.MaxSize()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[5]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.TextBrush()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[7]
			{
				propertiesHelper.Brush(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.ContourColor(),
				propertiesHelper.TextBrush(),
				propertiesHelper.UseParentStyles()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[2]
			{
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[7]
			{
				propertiesHelper.AnchorMode(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.InteractionEditor(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[8]
			{
				propertiesHelper.AnchorMode(),
				propertiesHelper.DockStyle(),
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight(),
				propertiesHelper.InteractionEditor(),
				propertiesHelper.Printable(),
				propertiesHelper.PrintOn(),
				propertiesHelper.ShiftMode()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[1] { propertiesHelper.Name() });
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[2]
			{
				propertiesHelper.Name(),
				propertiesHelper.Alias()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[5]
			{
				propertiesHelper.Name(),
				propertiesHelper.Alias(),
				propertiesHelper.Restrictions(),
				propertiesHelper.Locked(),
				propertiesHelper.Linked()
			});
		}
		if ((int)level == 2)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Export, new StiPropertyObject[1] { propertiesHelper.ExcelValue() });
		}
		return stiPropertyCollection;
	}

	public override StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		StiEventCollection stiEventCollection = new StiEventCollection();
		stiEventCollection.Add(StiPropertyCategories.MouseEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.ClickEvent,
			StiPropertyEventId.DoubleClickEvent,
			StiPropertyEventId.MouseEnterEvent,
			StiPropertyEventId.MouseLeaveEvent
		});
		stiEventCollection.Add(StiPropertyCategories.NavigationEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.GetBookmarkEvent,
			StiPropertyEventId.GetDrillDownReportEvent,
			StiPropertyEventId.GetHyperlinkEvent,
			StiPropertyEventId.GetPointerEvent
		});
		stiEventCollection.Add(StiPropertyCategories.PrintEvents, new StiPropertyEventId[2]
		{
			StiPropertyEventId.AfterPrintEvent,
			StiPropertyEventId.BeforePrintEvent
		});
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.GetCheckedEvent,
			StiPropertyEventId.GetExcelValueEvent,
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent
		});
		return stiEventCollection;
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
		switch (format)
		{
		case StiExportFormat.Text:
			return false;
		case StiExportFormat.Excel:
		case StiExportFormat.ExcelXml:
		case StiExportFormat.Excel2007:
		case StiExportFormat.Ods:
			if (!string.IsNullOrEmpty(ExcelDataValue))
			{
				return false;
			}
			break;
		}
		return true;
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

	string IStiEditable.SaveState()
	{
		if (!(CheckedValue is bool) || !(bool)CheckedValue)
		{
			return "false";
		}
		return "true";
	}

	void IStiEditable.RestoreState(string value)
	{
		CheckedValue = value == "true";
	}

	public override object Clone(bool cloneProperties)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		StiCheckBox stiCheckBox = (StiCheckBox)base.Clone(cloneProperties);
		if (TextBrush != null)
		{
			stiCheckBox.TextBrush = (StiBrush)TextBrush.Clone();
		}
		else
		{
			stiCheckBox.TextBrush = null;
		}
		return stiCheckBox;
	}

	public bool Break(StiComponent dividedComponent, double devideFactor, ref double divideLine)
	{
		divideLine = 0.0;
		bool result = true;
		if (devideFactor > 0.5 || (Page != null && Height / Page.PageHeight > 0.5))
		{
			((StiCheckBox)dividedComponent).CheckedValue = null;
		}
		else
		{
			CheckedValue = null;
		}
		return result;
	}

	public override StiEventsCollection GetEvents()
	{
		StiEventsCollection stiEventsCollection = base.GetEvents();
		if (GetCheckedEvent != null)
		{
			stiEventsCollection.Add(GetCheckedEvent);
		}
		return stiEventsCollection;
	}

	public override void InvokeEvents()
	{
		base.InvokeEvents();
		try
		{
			if (Report.CalculationMode == StiCalculationMode.Compilation)
			{
				if ((object)base.Events[EventGetChecked] != null && CheckedValue == null)
				{
					StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
					InvokeGetChecked(this, stiValueEventArgs);
					CheckedValue = stiValueEventArgs.Value;
				}
			}
			else if (((object)base.Events[EventGetChecked] != null || !string.IsNullOrEmpty(Checked.Value)) && CheckedValue == null)
			{
				StiValueEventArgs stiValueEventArgs2 = new StiValueEventArgs();
				InvokeGetChecked(this, stiValueEventArgs2);
				CheckedValue = stiValueEventArgs2.Value;
			}
			if ((object)base.Events[EventGetExcelValue] != null && ExcelDataValue == null)
			{
				StiGetExcelValueEventArgs stiGetExcelValueEventArgs = new StiGetExcelValueEventArgs();
				InvokeGetExcelValue(this, stiGetExcelValueEventArgs);
				if (stiGetExcelValueEventArgs.Value != null)
				{
					ExcelDataValue = stiGetExcelValueEventArgs.Value;
				}
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "DoEvents...ERROR");
			StiLogService.Write(((object)this).GetType(), ex);
			if (Report != null)
			{
				Report.WriteToReportRenderingMessages(Name + " " + ex.Message);
			}
		}
	}

	protected virtual void OnGetChecked(StiValueEventArgs e)
	{
	}

	public void InvokeGetChecked(StiComponent sender, StiValueEventArgs e)
	{
		try
		{
			if (Report.CalculationMode == StiCalculationMode.Compilation)
			{
				OnGetChecked(e);
				if (base.Events[EventGetChecked] is StiValueEventHandler stiValueEventHandler)
				{
					stiValueEventHandler(sender, e);
				}
				return;
			}
			OnGetChecked(e);
			if (!string.IsNullOrEmpty(Checked.Value))
			{
				object obj = StiParser.ParseTextValue(Checked.Value, this, sender);
				if (obj != null)
				{
					e.Value = obj;
				}
			}
			if (base.Events[EventGetChecked] is StiValueEventHandler stiValueEventHandler2)
			{
				stiValueEventHandler2(sender, e);
			}
			StiBlocklyHelper.InvokeBlockly(Report, this, GetCheckedEvent, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in Checked property of '" + Name + "' can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report.WriteToReportRenderingMessages(text);
		}
	}

	protected virtual void OnGetExcelValue(StiGetExcelValueEventArgs e)
	{
	}

	public virtual void InvokeGetExcelValue(StiComponent sender, StiGetExcelValueEventArgs e)
	{
		try
		{
			OnGetExcelValue(e);
			if (base.Events[EventGetExcelValue] is StiGetExcelValueEventHandler stiGetExcelValueEventHandler)
			{
				stiGetExcelValueEventHandler(sender, e);
			}
			StiBlocklyHelper.InvokeBlockly(Report, sender, GetExcelValueEvent, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in ExcelValue property of '" + Name + "' can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report.WriteToReportRenderingMessages(text);
		}
	}

	public override StiComponent CreateNew()
	{
		return new StiCheckBox();
	}

	public bool IsChecked()
	{
		string[] array = null;
		if (Values.IndexOf('/') != -1)
		{
			array = Values.Split(new char[1] { '/' });
		}
		else if (Values.IndexOf(';') != -1)
		{
			array = Values.Split(new char[1] { ';' });
		}
		else if (Values.IndexOf(',') != -1)
		{
			array = Values.Split(new char[1] { ',' });
		}
		if (array.Length == 0)
		{
			return false;
		}
		string text = array[0].ToLowerInvariant().Trim();
		if (text.Length == 0)
		{
			return false;
		}
		if (CheckedValue == null)
		{
			return false;
		}
		string text2 = CheckedValue.ToString().ToLowerInvariant().Trim();
		return text2 == text;
	}

	public bool IsUnchecked()
	{
		string[] array = null;
		if (Values.IndexOf('/') != -1)
		{
			array = Values.Split(new char[1] { '/' });
		}
		else if (Values.IndexOf(';') != -1)
		{
			array = Values.Split(new char[1] { ';' });
		}
		else if (Values.IndexOf(',') != -1)
		{
			array = Values.Split(new char[1] { ',' });
		}
		if (array.Length < 2)
		{
			return false;
		}
		string text = array[1].ToLowerInvariant().Trim();
		if (text.Length == 0)
		{
			return false;
		}
		if (CheckedValue == null)
		{
			return false;
		}
		string text2 = CheckedValue.ToString().ToLowerInvariant().Trim();
		return text2 == text;
	}

	public Metafile GetMetafile()
	{
		Metafile val = null;
		try
		{
			return GetMetafile1();
		}
		catch
		{
			return GetMetafile1();
		}
	}

	private Metafile GetMetafile1()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				IntPtr hdc = val2.GetHdc();
				Metafile val3 = new Metafile(hdc, (EmfType)3);
				val2.ReleaseHdc(hdc);
				Graphics val4 = Graphics.FromImage((Image)(object)val3);
				try
				{
					RectangleD paintRectangle = GetPaintRectangle(convertToHInches: true, convertByZoom: false);
					((RectangleD)(ref paintRectangle)).X = 0.0;
					((RectangleD)(ref paintRectangle)).Y = 0.0;
					StiDrawing.FillRectangle(val4, Brush, paintRectangle);
					StiCheckBoxGdiPainter.PaintCheck(this, val4, paintRectangle, 1f);
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				return val3;
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private bool ShouldSerializeContourColor()
	{
		return ContourColor != Color.Black;
	}

	public StiCheckBox()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiCheckBox(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
	}
}
