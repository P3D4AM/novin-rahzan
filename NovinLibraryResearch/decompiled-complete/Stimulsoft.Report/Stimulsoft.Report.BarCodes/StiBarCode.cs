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
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.BarCodes;

[StiServiceBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.StiBarCode.png")]
[StiGdiPainter(typeof(StiBarCodeGdiPainter))]
[StiDesigner("Stimulsoft.Report.Components.Design.StiBarCodeDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfPainter("Stimulsoft.Report.Painters.StiBarCodeWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfBarCodeDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiToolbox(true)]
[StiContextTool(typeof(IStiShift))]
[StiContextTool(typeof(IStiGrowToHeight))]
public class StiBarCode : StiComponent, IStiBarCode, IStiBackColor, IStiForeColor, IStiExportImageExtended, IStiExportImage, IStiVertAlignment, IStiHorAlignment, IStiEnumAngle, IStiBorder
{
	private StiBarCodeTypeService barCodeType = new StiEAN13BarCodeType();

	private static readonly object EventGetBarCode = new object();

	private Font font;

	[Browsable(false)]
	public override StiComponentId ComponentId => StiComponentId.StiBarCode;

	public override string HelpUrl => "user-manual/report_internals_barcodes.htm";

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

	public override int ToolboxPosition => 107;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Components;

	public override string LocalizedCategory => StiLocalization.Get("Components", "StiBarCode");

	public override string LocalizedName => StiLocalization.Get("Components", "StiBarCode");

	[StiSerializable]
	[DefaultValue(StiAngle.Angle0)]
	[TypeConverter(typeof(StiEnumConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[Description("Gets or sets angle of a bar code rotation.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiAngle Angle { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable]
	[Description("The appearance and behavior of the component border.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBorder Border { get; set; } = new StiBorder();


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(210)]
	[StiSerializable]
	[TypeConverter(typeof(StiExpressionColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets bar code color.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiExpressionAllowed]
	public Color ForeColor { get; set; } = Color.Black;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[StiSerializable]
	[TypeConverter(typeof(StiExpressionColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("The background color of the component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiExpressionAllowed]
	public Color BackColor { get; set; } = Color.White;


	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates how bar code will scale its size.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool AutoScale { get; set; }

	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates will this bar code show label text or no. Only for linear barcodes.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool ShowLabelText { get; set; } = true;


	[DefaultValue(true)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates will this bar code show quiet zones or no.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool ShowQuietZones { get; set; } = true;


	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.BarCodes.Design.StiBarCodeTypeServiceEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets type of the bar code.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBarCodeTypeService BarCodeType
	{
		get
		{
			return barCodeType;
		}
		set
		{
			if (value != null && Code != null && (barCodeType == null || Code.Value == barCodeType.DefaultCodeValue))
			{
				Code.Value = value.DefaultCodeValue;
			}
			barCodeType = value;
		}
	}

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[Description("Gets or sets the horizontal alignment of an object.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionEnumConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionEnumEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public StiHorAlignment HorAlignment { get; set; } = (StiHorAlignment)1;


	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[Description("Gets or sets the vertical alignment of an object.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionEnumConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionEnumEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public StiVertAlignment VertAlignment { get; set; }

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the component bar code.")]
	public string CodeValue { get; set; }

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a code of bar code.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiBarCodeExpression Code
	{
		get
		{
			if (BarCodeType != null)
			{
				string combinedCode = BarCodeType.GetCombinedCode();
				if (combinedCode != null)
				{
					return new StiBarCodeExpression(combinedCode);
				}
			}
			return new StiBarCodeExpression(this, "Code");
		}
		set
		{
			value?.Set(this, "Code", value.Value);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the code of bar code.")]
	public StiGetBarCodeEvent GetBarCodeEvent
	{
		get
		{
			return new StiGetBarCodeEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(180)]
	[Description("Gets or sets font of bar code.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Design.Components.StiFontEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public virtual Font Font
	{
		get
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			//IL_0023: Expected O, but got Unknown
			Font obj = font;
			if (obj == null)
			{
				Font val = new Font("Arial", 8f, (FontStyle)1, (GraphicsUnit)2);
				Font val2 = val;
				font = val;
				obj = val2;
			}
			return obj;
		}
		set
		{
			font = value;
		}
	}

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 240.0, 110.0);

	public event StiValueEventHandler GetBarCode
	{
		add
		{
			base.Events.AddHandler(EventGetBarCode, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventGetBarCode, value);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Invalid comparison between Unknown and I4
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "CanShrink");
		JsonExtensions.RemoveProperty(val, "CanGrow");
		JsonExtensions.AddPropertyEnum(val, "Angle", (Enum)Angle, (Enum)StiAngle.Angle0);
		JsonExtensions.AddPropertyBorder(val, "Border", Border);
		JsonExtensions.AddPropertyColor(val, "ForeColor", ForeColor, Color.Black);
		JsonExtensions.AddPropertyColor(val, "BackColor", BackColor, Color.White);
		JsonExtensions.AddPropertyBool(val, "AutoScale", AutoScale, false, false);
		JsonExtensions.AddPropertyBool(val, "ShowLabelText", ShowLabelText, true, false);
		JsonExtensions.AddPropertyBool(val, "ShowQuietZones", ShowQuietZones, true, false);
		JsonExtensions.AddPropertyFontArial8BoldPixel(val, "Font", Font);
		JsonExtensions.AddPropertyEnum(val, "HorAlignment", (Enum)(object)HorAlignment, (Enum)(object)(StiHorAlignment)1);
		JsonExtensions.AddPropertyEnum(val, "VertAlignment", (Enum)(object)VertAlignment, (Enum)(object)(StiVertAlignment)0);
		JsonExtensions.AddPropertyJObject(val, "GetBarCodeEvent", GetBarCodeEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "BarCodeType", BarCodeType.SaveToJsonObject(mode));
		if ((int)mode == 1)
		{
			JsonExtensions.AddPropertyStringNullOrEmpty(val, "CodeValue", CodeValue);
		}
		else
		{
			JsonExtensions.AddPropertyJObject(val, "Code", Code.SaveToJsonObject(mode));
		}
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Angle":
				Angle = StiJsonReportObjectHelper.DeserializeEnum<StiAngle>(item);
				break;
			case "Border":
				Border = StiJsonReportObjectHelper.DeserializeBorder(item);
				break;
			case "ForeColor":
				ForeColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "BackColor":
				BackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "AutoScale":
				AutoScale = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShowLabelText":
				ShowLabelText = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "ShowQuietZones":
				ShowQuietZones = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Font":
				font = StiJsonReportObjectHelper.DeserializeFont(item, Font);
				break;
			case "HorAlignment":
				HorAlignment = StiJsonReportObjectHelper.DeserializeEnum<StiHorAlignment>(item);
				break;
			case "VertAlignment":
				VertAlignment = StiJsonReportObjectHelper.DeserializeEnum<StiVertAlignment>(item);
				break;
			case "CodeValue":
				CodeValue = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Code":
			{
				StiBarCodeExpression stiBarCodeExpression = new StiBarCodeExpression();
				stiBarCodeExpression.LoadFromJsonObject((JObject)item.Value);
				Code = stiBarCodeExpression;
				break;
			}
			case "GetBarCodeEvent":
			{
				StiGetBarCodeEvent stiGetBarCodeEvent = new StiGetBarCodeEvent();
				stiGetBarCodeEvent.LoadFromJsonObject((JObject)item.Value);
				GetBarCodeEvent = stiGetBarCodeEvent;
				break;
			}
			case "BarCodeType":
				BarCodeType = StiBarCodeTypeService.CreateFromJsonObject((JObject)item.Value);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected I4, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Expected I4, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected I4, but got Unknown
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		stiPropertyCollection.Add(StiPropertyCategories.ComponentEditor, new StiPropertyObject[1] { propertiesHelper.BarCodeEditor() });
		switch ((int)level)
		{
		case 0:
			stiPropertyCollection.Add(StiPropertyCategories.BarCodeAdditional, new StiPropertyObject[1] { propertiesHelper.BarCodeAngle() });
			break;
		case 1:
			stiPropertyCollection.Add(StiPropertyCategories.BarCodeAdditional, new StiPropertyObject[5]
			{
				propertiesHelper.BarCodeAngle(),
				propertiesHelper.AutoScale(),
				propertiesHelper.HorAlignment(),
				propertiesHelper.VertAlignment(),
				propertiesHelper.ShowLabelText()
			});
			break;
		case 2:
			stiPropertyCollection.Add(StiPropertyCategories.BarCodeAdditional, new StiPropertyObject[6]
			{
				propertiesHelper.BarCodeAngle(),
				propertiesHelper.AutoScale(),
				propertiesHelper.HorAlignment(),
				propertiesHelper.VertAlignment(),
				propertiesHelper.ShowLabelText(),
				propertiesHelper.ShowQuickButtons()
			});
			break;
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
				propertiesHelper.BackColor(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.ForeColor()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[7]
			{
				propertiesHelper.BackColor(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.Font(),
				propertiesHelper.ForeColor(),
				propertiesHelper.UseParentStyles()
			});
		}
		switch ((int)level)
		{
		case 0:
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[2]
			{
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight()
			});
			break;
		case 1:
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
			break;
		case 2:
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
			break;
		}
		switch ((int)level)
		{
		case 0:
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[1] { propertiesHelper.Name() });
			break;
		case 1:
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[2]
			{
				propertiesHelper.Name(),
				propertiesHelper.Alias()
			});
			break;
		case 2:
			stiPropertyCollection.Add(StiPropertyCategories.Design, new StiPropertyObject[5]
			{
				propertiesHelper.Name(),
				propertiesHelper.Alias(),
				propertiesHelper.Restrictions(),
				propertiesHelper.Locked(),
				propertiesHelper.Linked()
			});
			break;
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
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[3]
		{
			StiPropertyEventId.GetBarCodeEvent,
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent
		});
		return stiEventCollection;
	}

	[Browsable(false)]
	public override bool IsExportAsImage(StiExportFormat format)
	{
		switch (format)
		{
		case StiExportFormat.ImageSvg:
			return false;
		case StiExportFormat.Pdf:
			if (!StiOptions.Export.Pdf.RenderBarCodeAsImage)
			{
				return false;
			}
			break;
		}
		return true;
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

	private bool ShouldSerializeForeColor()
	{
		return ForeColor != Color.Black;
	}

	private bool ShouldSerializeBackColor()
	{
		return BackColor != Color.White;
	}

	public string GetBarCodeString()
	{
		if (CodeValue != null)
		{
			return CodeValue;
		}
		if (StiOptions.Designer.CalculateBarcodeValueInDesignMode)
		{
			try
			{
				string text = System.Convert.ToString(StiParser.ParseTextValue(Code.Value, this));
				if (!string.IsNullOrWhiteSpace(text))
				{
					return text;
				}
			}
			catch
			{
			}
		}
		return Code.Value;
	}

	public override List<StiFont> GetFonts()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		List<StiFont> fonts = base.GetFonts();
		fonts.Add(new StiFont(Font));
		return fonts.Distinct().ToList();
	}

	public override void InvokeEvents()
	{
		base.InvokeEvents();
		try
		{
			if (Report.CalculationMode == StiCalculationMode.Compilation)
			{
				if ((object)base.Events[EventGetBarCode] != null && CodeValue == null)
				{
					StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
					InvokeGetBarCode(this, stiValueEventArgs);
					if (stiValueEventArgs.Value != null)
					{
						CodeValue = stiValueEventArgs.Value.ToString();
					}
				}
			}
			else if (CodeValue == null)
			{
				StiValueEventArgs stiValueEventArgs2 = new StiValueEventArgs();
				InvokeGetBarCode(this, stiValueEventArgs2);
				if (stiValueEventArgs2.Value != null)
				{
					CodeValue = stiValueEventArgs2.Value.ToString();
				}
			}
		}
		catch (Exception e)
		{
			StiLogService.Write(((object)this).GetType(), "DoEvents...ERROR");
			StiLogService.Write(((object)this).GetType(), e);
		}
	}

	protected virtual void OnGetBarCode(StiValueEventArgs e)
	{
	}

	public void InvokeGetBarCode(StiComponent sender, StiValueEventArgs e)
	{
		try
		{
			OnGetBarCode(e);
			if (Report.CalculationMode == StiCalculationMode.Compilation)
			{
				(base.Events[EventGetBarCode] as StiValueEventHandler)?.Invoke(sender, e);
				return;
			}
			object obj = StiParser.ParseTextValue(Code.Value, this, sender);
			if (obj != null)
			{
				e.Value = obj;
			}
			(base.Events[EventGetBarCode] as StiValueEventHandler)?.Invoke(sender, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in BarCode property of '" + Name + "' can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report.WriteToReportRenderingMessages(text);
		}
	}

	public override StiComponent CreateNew()
	{
		return new StiBarCode();
	}

	public StiBarCode()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiBarCode(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = false;
		Code.Value = BarCodeType.DefaultCodeValue;
	}
}
