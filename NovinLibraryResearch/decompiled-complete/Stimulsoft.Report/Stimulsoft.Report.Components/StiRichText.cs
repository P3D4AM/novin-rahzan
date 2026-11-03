using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Controls;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Export;
using Stimulsoft.Report.Export.Tools;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.QuickButtons;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiRichText), "Stimulsoft.Report.Images.Components.StiRichText.png")]
[StiToolbox(true)]
[StiContextTool(typeof(IStiCanGrow))]
[StiContextTool(typeof(IStiCanShrink))]
[StiContextTool(typeof(IStiShift))]
[StiContextTool(typeof(IStiEditable))]
[StiContextTool(typeof(IStiGrowToHeight))]
[StiContextTool(typeof(IStiComponentDesigner))]
[StiContextTool(typeof(IStiBreakable))]
[StiContextTool(typeof(IStiOnlyText))]
[StiContextTool(typeof(IStiTextOptions))]
[StiDesigner("Stimulsoft.Report.Components.Design.StiRichTextDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfRichTextDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiGdiPainter(typeof(StiRichTextGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiRichTextWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiQuickButton("Stimulsoft.Report.QuickButtons.Design.StiRichTextQuickButton, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfQuickButton("Stimulsoft.Report.WpfDesign.StiWpfRichTextQuickButton, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiRichText : StiSimpleText, IStiExportImageExtended, IStiExportImage, IStiBreakable, IStiBorder, IStiGlobalizationProvider, IStiBackColor
{
	protected static object PropertyCanBreak = new object();

	private static object lockRichTextThread = new object();

	private static readonly object EventGetDataUrl = new object();

	private string dataColumn = string.Empty;

	public override StiComponentId ComponentId => StiComponentId.StiRichText;

	public override string HelpUrl => "user-manual/report_internals_rich_text_output.htm";

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates whether the component can or cannot break its contents on several pages.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool CanBreak
	{
		get
		{
			return ((StiService)this).Properties.GetBool(PropertyCanBreak, false);
		}
		set
		{
			((StiService)this).Properties.SetBool(PropertyCanBreak, value, false);
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable]
	[Description("The appearance and behavior of the component border.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiBorder Border { get; set; } = new StiBorder();


	[StiSerializable]
	[TypeConverter(typeof(StiExpressionColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("The background color of the component.")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiExpressionAllowed]
	public Color BackColor { get; set; } = Color.White;


	[Browsable(false)]
	[StiNonSerialized]
	public sealed override string GlobalizedName
	{
		get
		{
			return base.GlobalizedName;
		}
		set
		{
			base.GlobalizedName = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override StiPenStyle LinesOfUnderline
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.LinesOfUnderline;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool HideZeros
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiProcessingDuplicatesType ProcessingDuplicates
	{
		get
		{
			return StiProcessingDuplicatesType.None;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override int MaxNumberOfLines
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	[Editor("Stimulsoft.Report.Components.Design.StiRichTextExpressionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiRichTextExpressionConverter))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override StiExpression Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
		}
	}

	public override int ToolboxPosition => 105;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Components;

	public override string LocalizedName => StiLocalization.Get("Components", "StiRichText");

	public override string LocalizedCategory => StiLocalization.Get("Report", "Components");

	[Browsable(false)]
	public Image Image { get; set; }

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when the DataUrl is calculated.")]
	public virtual StiGetDataUrlEvent GetDataUrlEvent
	{
		get
		{
			return new StiGetDataUrlEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(180)]
	[Description("Gets or sets text margins.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiMargins Margins { get; set; } = new StiMargins(0.0, 0.0, 0.0, 0.0);


	[Browsable(false)]
	public Font DefaultFont { get; set; }

	[Browsable(false)]
	public Color DefaultColor { get; set; } = Color.Empty;


	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets word wrap.")]
	[StiOrder(310)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiShowInContextMenu]
	[StiSerializable]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool WordWrap { get; set; } = true;


	[DefaultValue(true)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets detection of urls.")]
	[StiOrder(120)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiShowInContextMenu]
	[StiSerializable]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool DetectUrls { get; set; } = true;


	[StiSerializable]
	[Editor("Stimulsoft.Report.Components.Design.StiRichTextExpressionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(180)]
	[Description("Gets or sets a name of the column that contains the RTF text.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public string DataColumn
	{
		get
		{
			return dataColumn;
		}
		set
		{
			if (dataColumn != value)
			{
				dataColumn = value;
				ResetImage();
			}
		}
	}

	[StiSerializable]
	[Description("Gets or sets a URL that contains the RTF text.")]
	[DefaultValue(null)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(190)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Components.Design.StiRichTextExpressionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public StiDataUrlExpression DataUrl
	{
		get
		{
			return new StiDataUrlExpression(this, "DataUrl");
		}
		set
		{
			if (value != null)
			{
				value.Set(this, "DataUrl", value.Value);
				ResetImage();
			}
		}
	}

	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[DefaultValue(false)]
	[Description("Gets or sets value which indicates that it is necessary to fully convert the expression to Rtf format. Full convertion of expressions slows down the report rendering.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool FullConvertExpression { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(320)]
	[DefaultValue(false)]
	[Description("Gets or sets value which indicates that it is necessary to use the Wysiwyg mode of the rendering.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool Wysiwyg { get; set; }

	[StiSerializable]
	[TypeConverter(typeof(StiBoolConverter))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(250)]
	[DefaultValue(false)]
	[Description("Gets or sets horizontal output direction.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public bool RightToLeft { get; set; }

	[Browsable(false)]
	public string RtfText
	{
		get
		{
			return UnpackRtf(StiRtfHelper.XmlDecodeFast(GetTextInternal()));
		}
		set
		{
			SetTextInternal(XmlConvert.EncodeName(PackRtf(value)));
		}
	}

	private bool IsReportWpf
	{
		get
		{
			if (Report != null)
			{
				return Report.IsWpf;
			}
			return false;
		}
	}

	public event StiGetDataUrlEventHandler GetDataUrl
	{
		add
		{
			base.Events.AddHandler(EventGetDataUrl, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventGetDataUrl, value);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "GlobalizedName");
		JsonExtensions.RemoveProperty(val, "LinesOfUnderline");
		JsonExtensions.RemoveProperty(val, "HideZeros");
		JsonExtensions.RemoveProperty(val, "ProcessingDuplicates");
		JsonExtensions.RemoveProperty(val, "MaxNumberOfLines");
		JsonExtensions.AddPropertyBool(val, "CanBreak", CanBreak, false, false);
		JsonExtensions.AddPropertyBorder(val, "Border", Border);
		JsonExtensions.AddPropertyJObject(val, "GetDataUrlEvent", GetDataUrlEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "Margins", Margins.SaveToJsonObject(0.0, 0.0, 0.0, 0.0));
		JsonExtensions.AddPropertyBool(val, "WordWrap", WordWrap, true, false);
		JsonExtensions.AddPropertyBool(val, "DetectUrls", DetectUrls, true, false);
		JsonExtensions.AddPropertyColor(val, "BackColor", BackColor, Color.White);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "DataColumn", DataColumn);
		JsonExtensions.AddPropertyJObject(val, "DataUrl", DataUrl.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyBool(val, "FullConvertExpression", FullConvertExpression, false, false);
		JsonExtensions.AddPropertyBool(val, "Wysiwyg", Wysiwyg, false, false);
		JsonExtensions.AddPropertyBool(val, "RightToLeft", RightToLeft, false, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "CanBreak":
				CanBreak = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Border":
				Border = StiJsonReportObjectHelper.DeserializeBorder(item);
				break;
			case "GetDataUrlEvent":
			{
				StiGetDataUrlEvent stiGetDataUrlEvent = new StiGetDataUrlEvent();
				stiGetDataUrlEvent.LoadFromJsonObject((JObject)item.Value);
				GetDataUrlEvent = stiGetDataUrlEvent;
				break;
			}
			case "Margins":
				Margins.LoadFromJsonObject((JObject)item.Value);
				break;
			case "WordWrap":
				WordWrap = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "DetectUrls":
				DetectUrls = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "BackColor":
				BackColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "DataColumn":
				dataColumn = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "DataUrl":
			{
				StiDataUrlExpression stiDataUrlExpression = new StiDataUrlExpression();
				stiDataUrlExpression.LoadFromJsonObject((JObject)item.Value);
				DataUrl = stiDataUrlExpression;
				break;
			}
			case "FullConvertExpression":
				FullConvertExpression = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Wysiwyg":
				Wysiwyg = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "RightToLeft":
				RightToLeft = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public override StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Invalid comparison between Unknown and I4
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Invalid comparison between Unknown and I4
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Invalid comparison between Unknown and I4
		IStiPropertiesHelper propertiesHelper = propertyGrid.PropertiesHelper;
		StiPropertyCollection stiPropertyCollection = new StiPropertyCollection();
		stiPropertyCollection.Add(StiPropertyCategories.ComponentEditor, new StiPropertyObject[1] { propertiesHelper.RichTextDesign() });
		if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, new StiPropertyObject[6]
			{
				propertiesHelper.DetectUrls(),
				propertiesHelper.Editable(),
				propertiesHelper.Margins(),
				propertiesHelper.OnlyText(),
				propertiesHelper.RightToLeft(),
				propertiesHelper.WordWrap()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.TextAdditional, new StiPropertyObject[9]
			{
				propertiesHelper.DetectUrls(),
				propertiesHelper.Editable(),
				propertiesHelper.FullConvertExpression(),
				propertiesHelper.Margins(),
				propertiesHelper.OnlyText(),
				propertiesHelper.ProcessAt(),
				propertiesHelper.RightToLeft(),
				propertiesHelper.WordWrap(),
				propertiesHelper.Wysiwyg()
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
		if ((int)level != 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[3]
			{
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle()
			});
		}
		else
		{
			stiPropertyCollection.Add(StiPropertyCategories.Appearance, new StiPropertyObject[5]
			{
				propertiesHelper.BackColor(),
				propertiesHelper.Border(),
				propertiesHelper.Conditions(),
				propertiesHelper.ComponentStyle(),
				propertiesHelper.UseParentStyles()
			});
		}
		if ((int)level == 0)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[5]
			{
				propertiesHelper.CanBreak(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
				propertiesHelper.Enabled(),
				propertiesHelper.GrowToHeight()
			});
		}
		else if ((int)level == 1)
		{
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[10]
			{
				propertiesHelper.AnchorMode(),
				propertiesHelper.CanBreak(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
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
			stiPropertyCollection.Add(StiPropertyCategories.Behavior, new StiPropertyObject[11]
			{
				propertiesHelper.AnchorMode(),
				propertiesHelper.CanBreak(),
				propertiesHelper.CanGrow(),
				propertiesHelper.CanShrink(),
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
			StiPropertyEventId.GetDataUrlEvent,
			StiPropertyEventId.GetTagEvent,
			StiPropertyEventId.GetToolTipEvent,
			StiPropertyEventId.GetValueEvent
		});
		return stiEventCollection;
	}

	public bool Break(StiComponent dividedComponent, double devideFactor, ref double divideLine)
	{
		divideLine = 0.0;
		if (string.IsNullOrEmpty(GetTextInternal()))
		{
			return true;
		}
		if (IsReportWpf)
		{
			Type type = Type.GetType("Stimulsoft.Report.Wpf.StiWpfBreakTextHelper, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
			if (type == null)
			{
				throw new Exception("Assembly 'Stimulsoft.Report.Wpf' is not found");
			}
			IStiWpfBreakTextHelper stiWpfBreakTextHelper = StiActivator.CreateObject(type, new object[0]) as IStiWpfBreakTextHelper;
			AppDomain appDomain = null;
			if (StiOptions.Engine.RenderRichTextInOtherDomain)
			{
				if (dividedComponent.Report != null && dividedComponent.Report.WpfRichTextDomain != null)
				{
					appDomain = dividedComponent.Report.WpfRichTextDomain;
				}
				if (appDomain == null)
				{
					AppDomainSetup appDomainSetup = new AppDomainSetup();
					appDomainSetup.ShadowCopyFiles = "false";
					appDomainSetup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
					appDomain = AppDomain.CreateDomain(StiGuidUtils.NewGuid(), AppDomain.CurrentDomain.Evidence, appDomainSetup);
				}
				if (dividedComponent.Report != null)
				{
					dividedComponent.Report.WpfRichTextDomain = appDomain;
				}
			}
			return stiWpfBreakTextHelper.BreakRichText(this, dividedComponent, devideFactor, ref divideLine, appDomain);
		}
		if (StiOptions.Engine.ForceLockRichTextThread)
		{
			lock (lockRichTextThread)
			{
				return BreakWin(dividedComponent);
			}
		}
		return BreakWin(dividedComponent);
	}

	private bool BreakWin(StiComponent dividedComponent)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		RectangleD rect = Report.Unit.ConvertToHInches(ClientRectangle);
		if (((RectangleD)(ref rect)).Width < 0.0)
		{
			((RectangleD)(ref rect)).Width = Math.Abs(((RectangleD)(ref rect)).Width);
		}
		rect = ConvertTextMargins(rect, convert: false);
		int charEnd = 0;
		bool flag = true;
		int num = 0;
		string rtfText = RtfText;
		while (flag && num < 100)
		{
			flag = false;
			try
			{
				StiRichTextBox stiRichTextBox = new StiRichTextBox(drawTransparent: false);
				try
				{
					((TextBoxBase)stiRichTextBox).WordWrap = WordWrap;
					((RichTextBox)stiRichTextBox).DetectUrls = DetectUrls;
					if (RightToLeft)
					{
						((Control)stiRichTextBox).RightToLeft = (RightToLeft)1;
					}
					GetPreparedText((RichTextBox)(object)stiRichTextBox);
					if (((TextBoxBase)stiRichTextBox).TextLength <= 0)
					{
						continue;
					}
					int num2 = (int)((RectangleD)(ref rect)).Width;
					if (!WordWrap)
					{
						num2 = 100000;
					}
					int num3 = 0;
					if (Wysiwyg || StiDpiHelper.NeedGraphicsRichTextScale)
					{
						lock (StiReport.GlobalRichTextMeasureGraphics)
						{
							num3 = StiRichTextHelper.FormatRange(StiRtfFormatType.MeasureRtf, (RichTextBox)(object)stiRichTextBox, new Rectangle(0, 0, num2, (int)((RectangleD)(ref rect)).Height), StiReport.GlobalRichTextMeasureGraphics, 0, -1, out charEnd);
						}
					}
					else
					{
						Graphics val = Graphics.FromHwnd(IntPtr.Zero);
						try
						{
							num3 = StiRichTextHelper.FormatRange(StiRtfFormatType.MeasureRtf, (RichTextBox)(object)stiRichTextBox, new Rectangle(0, 0, num2, (int)((RectangleD)(ref rect)).Height), val, 0, -1, out charEnd);
						}
						finally
						{
							((IDisposable)val)?.Dispose();
						}
					}
					if (num3 <= (int)((RectangleD)(ref rect)).Height || charEnd <= 0 || StiOptions.Engine.ForceRtfBreak)
					{
						continue;
					}
					((TextBoxBase)stiRichTextBox).SelectionStart = 0;
					((TextBoxBase)stiRichTextBox).SelectionLength = charEnd;
					((RichTextBox)stiRichTextBox).Rtf = ((RichTextBox)stiRichTextBox).SelectedRtf;
					try
					{
						if (Wysiwyg || StiDpiHelper.NeedGraphicsRichTextScale)
						{
							lock (StiReport.GlobalRichTextMeasureGraphics)
							{
								int charEnd2 = 0;
								num3 = StiRichTextHelper.FormatRange(StiRtfFormatType.MeasureRtf, (RichTextBox)(object)stiRichTextBox, new Rectangle(0, 0, num2, (int)((RectangleD)(ref rect)).Height), StiReport.GlobalRichTextMeasureGraphics, 0, -1, out charEnd2);
							}
						}
						else
						{
							Graphics val2 = Graphics.FromHwnd(IntPtr.Zero);
							try
							{
								int charEnd3 = 0;
								num3 = StiRichTextHelper.FormatRange(StiRtfFormatType.MeasureRtf, (RichTextBox)(object)stiRichTextBox, new Rectangle(0, 0, num2, (int)((RectangleD)(ref rect)).Height), val2, 0, -1, out charEnd3);
							}
							finally
							{
								((IDisposable)val2)?.Dispose();
							}
						}
						if (num3 <= (int)((RectangleD)(ref rect)).Height)
						{
							continue;
						}
						int num4 = 0;
						int num5 = (int)((RectangleD)(ref rect)).Height;
						int num6 = (int)((double)num5 * (2.0 - (double)num3 / (double)num5));
						if (num6 < num5 / 2)
						{
							num6 = num5 / 2;
						}
						int num7 = 0;
						int num8 = 0;
						int charEnd4 = 0;
						while (num5 - num4 > 5)
						{
							if (Wysiwyg || StiDpiHelper.NeedGraphicsRichTextScale)
							{
								lock (StiReport.GlobalRichTextMeasureGraphics)
								{
									num3 = StiRichTextHelper.FormatRange(StiRtfFormatType.MeasureRtf, (RichTextBox)(object)stiRichTextBox, new Rectangle(0, 0, num2, num6), StiReport.GlobalRichTextMeasureGraphics, 0, -1, out charEnd4);
								}
							}
							else
							{
								Graphics val3 = Graphics.FromHwnd(IntPtr.Zero);
								try
								{
									num3 = StiRichTextHelper.FormatRange(StiRtfFormatType.MeasureRtf, (RichTextBox)(object)stiRichTextBox, new Rectangle(0, 0, num2, num6), val3, 0, -1, out charEnd4);
								}
								finally
								{
									((IDisposable)val3)?.Dispose();
								}
							}
							if ((double)num3 > ((RectangleD)(ref rect)).Height)
							{
								num5 = num6;
							}
							else
							{
								num4 = num6;
								num8 = charEnd4;
							}
							num6 = (num4 + num5) / 2;
							if (num7 == num3)
							{
								break;
							}
							num7 = num3;
						}
						charEnd = num8;
					}
					catch (Exception ex)
					{
						Report?.WriteToReportRenderingMessages(Name + " " + ex.Message);
					}
				}
				finally
				{
					((IDisposable)stiRichTextBox)?.Dispose();
				}
			}
			catch (Exception ex2)
			{
				flag = true;
				num++;
				Report?.WriteToReportRenderingMessages(Name + " " + ex2.Message);
			}
		}
		if (charEnd == -1)
		{
			charEnd = 0;
		}
		StiRichTextBox stiRichTextBox2 = new StiRichTextBox(drawTransparent: false);
		try
		{
			if (rtfText != null && rtfText.TrimStart(new char[0]).ToLowerInvariant().IndexOf("rtf", StringComparison.InvariantCulture) != -1)
			{
				((RichTextBox)stiRichTextBox2).Rtf = StiExportUtils.CorrectRichTextForRiched20(rtfText);
			}
			else
			{
				((Control)stiRichTextBox2).Text = GetTextInternal();
				if (StiOptions.Engine.DefaultRtfFont != null)
				{
					((TextBoxBase)stiRichTextBox2).SelectAll();
					((RichTextBox)stiRichTextBox2).SelectionFont = StiOptions.Engine.DefaultRtfFont;
				}
				else if (DefaultFont != null)
				{
					((TextBoxBase)stiRichTextBox2).SelectAll();
					((RichTextBox)stiRichTextBox2).SelectionFont = DefaultFont;
				}
				if (!DefaultColor.IsEmpty)
				{
					((TextBoxBase)stiRichTextBox2).SelectAll();
					((RichTextBox)stiRichTextBox2).SelectionColor = DefaultColor;
				}
			}
			((TextBoxBase)stiRichTextBox2).SelectionStart = 0;
			((TextBoxBase)stiRichTextBox2).SelectionLength = charEnd;
			if (StiOptions.Engine.RtfCache.Enabled)
			{
				NewGuid();
			}
			RtfText = ((RichTextBox)stiRichTextBox2).SelectedRtf;
			((TextBoxBase)stiRichTextBox2).SelectionStart = charEnd;
			((TextBoxBase)stiRichTextBox2).SelectionLength = Math.Max(((TextBoxBase)stiRichTextBox2).TextLength - charEnd, 0);
			if (StiOptions.Engine.RtfCache.Enabled)
			{
				dividedComponent.NewGuid();
			}
			string text = ((RichTextBox)stiRichTextBox2).SelectedRtf;
			Regex regex = new Regex("\\\\f\\d{1,2}\\s");
			Regex regex2 = new Regex("\\\\fs\\d{1,3}");
			Match match = regex.Match(text);
			if (match != null && match.Success)
			{
				Match match2 = regex2.Match(text);
				if (match2 != null && match2.Success && match2.Index - match.Index > 25)
				{
					text = text.Insert(match.Index, match2.Value);
				}
			}
			int num9 = text.LastIndexOf("\\par");
			int startIndex = text.LastIndexOf("}");
			if (num9 < text.Length - 15)
			{
				text = text.Insert(startIndex, "\\par");
			}
			((StiRichText)dividedComponent).RtfText = text;
		}
		finally
		{
			((IDisposable)stiRichTextBox2)?.Dispose();
		}
		return true;
	}

	public override object Clone(bool cloneProperties)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		StiRichText stiRichText = (StiRichText)base.Clone(cloneProperties);
		if (Border != null)
		{
			stiRichText.Border = (StiBorder)Border.Clone();
		}
		else
		{
			stiRichText.Border = null;
		}
		return stiRichText;
	}

	void IStiGlobalizationProvider.SetString(string propertyName, string value)
	{
		switch (propertyName)
		{
		case "Text":
			SetTextInternal(value);
			break;
		case "ToolTip":
			ToolTip.Value = value;
			break;
		case "Tag":
			Tag.Value = value;
			break;
		case "Hyperlink":
			Hyperlink.Value = value;
			break;
		default:
			throw new ArgumentException("Property with name " + propertyName);
		}
	}

	string IStiGlobalizationProvider.GetString(string propertyName)
	{
		return propertyName switch
		{
			"Text" => GetTextInternal(), 
			"Tag" => Tag.Value, 
			"ToolTip" => ToolTip.Value, 
			"Hyperlink" => Hyperlink.Value, 
			_ => throw new ArgumentException("Property with name " + propertyName), 
		};
	}

	string[] IStiGlobalizationProvider.GetAllStrings()
	{
		List<string> list = new List<string>();
		if (StiOptions.Engine.Globalization.AllowUseText)
		{
			list.Add("Text");
		}
		if (StiOptions.Engine.Globalization.AllowUseTag)
		{
			list.Add("Tag");
		}
		if (StiOptions.Engine.Globalization.AllowUseToolTip)
		{
			list.Add("ToolTip");
		}
		if (StiOptions.Engine.Globalization.AllowUseHyperlink)
		{
			list.Add("Hyperlink");
		}
		return list.ToArray();
	}

	private bool ShouldSerializeBorder()
	{
		if (Border != null)
		{
			return !Border.IsDefault;
		}
		return true;
	}

	private bool ShouldSerializeBackColor()
	{
		return BackColor != Color.White;
	}

	public virtual Image GetImage(ref float zoom)
	{
		return GetImage(ref zoom, StiExportFormat.None);
	}

	public virtual Image GetImage(ref float zoom, StiExportFormat format)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		bool flag = Image == null;
		StiPainter stiPainter = StiPainter.GetPainter(guiMode: (Report == null) ? ((StiGuiMode)(StiOptions.Configuration.IsWPF ? 1 : 0)) : ((StiGuiMode)((IsReportWpf || Report.RenderedWith == StiRenderedWith.Wpf) ? 1 : 0)), type: ((object)this).GetType());
		Image image = stiPainter.GetImage(this, ref zoom, format);
		if (flag)
		{
			Image image2 = Image;
			if (image2 != null)
			{
				image2.Dispose();
			}
			ResetImage();
		}
		return image;
	}

	[Browsable(false)]
	public override bool IsExportAsImage(StiExportFormat format)
	{
		if (format == StiExportFormat.Text || format == StiExportFormat.Rtf || format == StiExportFormat.RtfFrame || format == StiExportFormat.RtfTable || format == StiExportFormat.RtfWinWord)
		{
			return false;
		}
		return true;
	}

	public override List<StiFont> GetFonts()
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		List<StiFont> fonts = base.GetFonts();
		try
		{
			StiPdfExportService stiPdfExportService = new StiPdfExportService
			{
				report = Report
			};
			stiPdfExportService.InitPdfFonts();
			StiPdfMetafileRender stiPdfMetafileRender = new StiPdfMetafileRender(stiPdfExportService, useUnicodeMode: true);
			stiPdfMetafileRender.AssembleRtf(this);
			PdfFonts pdfFonts = stiPdfMetafileRender.GetPdfFonts();
			foreach (object font in pdfFonts.fontList)
			{
				fonts.Add(new StiFont(((PdfFonts.pfontInfo)font).Font));
			}
		}
		catch
		{
		}
		return fonts.Distinct().ToList();
	}

	public RectangleD ConvertTextMargins(RectangleD rect, bool convert)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		if (Margins.IsEmpty)
		{
			return rect;
		}
		double num = Page.Zoom;
		double num2 = Margins.Left;
		double right = Margins.Right;
		double num3 = Margins.Top;
		double bottom = Margins.Bottom;
		if (!convert)
		{
			num = 1.0;
		}
		if (num2 != 0.0)
		{
			((RectangleD)(ref rect)).X = ((RectangleD)(ref rect)).X + num2 * num;
			((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - num2 * num;
		}
		if (num3 != 0.0)
		{
			((RectangleD)(ref rect)).Y = ((RectangleD)(ref rect)).Y + num3 * num;
			((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height - num3 * num;
		}
		if (right != 0.0)
		{
			((RectangleD)(ref rect)).Width = ((RectangleD)(ref rect)).Width - right * num;
		}
		if (bottom != 0.0)
		{
			((RectangleD)(ref rect)).Height = ((RectangleD)(ref rect)).Height - bottom * num;
		}
		return rect;
	}

	public void RenderMetafile()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		if (RenderMetafile(useTransparent: true) && Image == null && Wysiwyg)
		{
			lock (StiReport.GlobalRichTextMeasureGraphics)
			{
				Bitmap val = new Bitmap(1, 1);
				val.SetResolution((float)(96.0 * StiDpiHelper.GraphicsRichTextScale), (float)(96.0 * StiDpiHelper.GraphicsRichTextScale));
				Graphics val2 = Graphics.FromImage((Image)(object)val);
				val2.PageUnit = (GraphicsUnit)1;
				val2.PageScale = 1f;
				StiReport.GlobalRichTextMeasureGraphics = val2;
			}
			RenderMetafile(useTransparent: true);
		}
	}

	public bool RenderMetafile(bool useTransparent)
	{
		if (StiOptions.Engine.ForceLockRichTextThread)
		{
			lock (lockRichTextThread)
			{
				return RenderMetafile2(useTransparent);
			}
		}
		return RenderMetafile2(useTransparent);
	}

	private bool RenderMetafile2(bool useTransparent)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		if (Page == null || Report == null)
		{
			return false;
		}
		bool flag = true;
		int num = 0;
		while (flag && num < 10)
		{
			flag = false;
			try
			{
				StiRichTextBox stiRichTextBox = new StiRichTextBox(useTransparent && (BackColor.A == 0 || Wysiwyg || StiDpiHelper.NeedGraphicsRichTextScale));
				try
				{
					string preparedText = GetPreparedText((RichTextBox)(object)stiRichTextBox);
					if (string.IsNullOrWhiteSpace(preparedText))
					{
						return false;
					}
					RectangleD paintRectangle = GetPaintRectangle(convertToHInches: true, convertByZoom: false);
					paintRectangle = ConvertTextMargins(paintRectangle, convert: false);
					if (BackColor.A > 0)
					{
						((Control)stiRichTextBox).BackColor = BackColor;
					}
					((TextBoxBase)stiRichTextBox).WordWrap = WordWrap;
					((RichTextBox)stiRichTextBox).DetectUrls = DetectUrls;
					if (RightToLeft)
					{
						IDictionary<string, object> clipboardData = StiClipboardHelper.GetClipboardData();
						((TextBoxBase)stiRichTextBox).SelectAll();
						((TextBoxBase)stiRichTextBox).Cut();
						((Control)stiRichTextBox).Text = "Temp";
						((Control)stiRichTextBox).RightToLeft = (RightToLeft)1;
						((TextBoxBase)stiRichTextBox).SelectAll();
						((TextBoxBase)stiRichTextBox).Paste();
						StiClipboardHelper.SetClipboardData(clipboardData);
					}
					if (Wysiwyg || StiDpiHelper.NeedGraphicsRichTextScale || (StiOptions.Configuration.IsWPF && StiOptions.Engine.RenderRichTextInWpfAlwaysWysiwyg))
					{
						lock (StiReport.GlobalRichTextMeasureGraphics)
						{
							CreateMetafile(StiReport.GlobalRichTextMeasureGraphics, paintRectangle, stiRichTextBox, preparedText);
						}
						continue;
					}
					Graphics val = Graphics.FromHwnd(((Control)stiRichTextBox).Handle);
					try
					{
						CreateMetafile(val, paintRectangle, stiRichTextBox, preparedText);
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)stiRichTextBox)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				flag = true;
				num++;
				Report?.WriteToReportRenderingMessages(Name + " " + ex.Message);
			}
		}
		if (flag)
		{
			return true;
		}
		return false;
	}

	private void CreateMetafile(Graphics graph, RectangleD rect, StiRichTextBox richTextBox, string text)
	{
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		if (((RectangleD)(ref rect)).Width < 0.0)
		{
			((RectangleD)(ref rect)).Width = 1.0;
		}
		if (((RectangleD)(ref rect)).Height < 0.0)
		{
			((RectangleD)(ref rect)).Height = 1.0;
		}
		int num = (int)((RectangleD)(ref rect)).Width + 1;
		int num2 = (int)((RectangleD)(ref rect)).Height + 1;
		if (!WordWrap)
		{
			num = 100000;
		}
		float dpiX = graph.DpiX;
		float dpiY = graph.DpiY;
		IntPtr hdc = graph.GetHdc();
		Metafile val = null;
		try
		{
			if (Wysiwyg || StiDpiHelper.NeedGraphicsRichTextScale || (StiOptions.Configuration.IsWPF && StiOptions.Engine.RenderRichTextInWpfAlwaysWysiwyg))
			{
				float num3 = (float)(((RectangleD)(ref rect)).Width * (double)dpiX / 95.69999694824219);
				float num4 = (float)(((RectangleD)(ref rect)).Height * (double)dpiY / 95.69999694824219);
				if (num3 == 0f)
				{
					num3 = 1f;
				}
				if (num4 == 0f)
				{
					num4 = 1f;
				}
				val = new Metafile(hdc, new RectangleF(0f, 0f, num3, num4), (MetafileFrameUnit)2, StiOptions.Engine.RichTextDrawingMetafileType);
			}
			else
			{
				val = new Metafile(hdc, StiOptions.Engine.RichTextDrawingMetafileType);
			}
		}
		finally
		{
			graph.ReleaseHdc(hdc);
		}
		if (!string.IsNullOrEmpty(text))
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				int charEnd = 0;
				val2.PageUnit = (GraphicsUnit)2;
				StiRichTextHelper.FormatRange(StiRtfFormatType.DrawRtf, (RichTextBox)(object)richTextBox, new Rectangle(0, 0, num, num2), val2, 0, text.Length, out charEnd);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		Image image = Image;
		if (image != null)
		{
			image.Dispose();
		}
		Image = (Image)(object)val;
		if (Report != null)
		{
			if (Report.RichTextImageCache == null)
			{
				Report.RichTextImageCache = new List<StiRichText>();
			}
			int num5 = Report.RichTextImageCache.IndexOf(this);
			if (num5 == -1)
			{
				Report.RichTextImageCache.Add(this);
			}
			int num6 = ((Report.ReportCacheMode == StiReportCacheMode.On) ? 50 : StiOptions.Engine.RichTextImageCacheSize);
			if (Report.RichTextImageCache.Count > num6)
			{
				StiRichText stiRichText = Report.RichTextImageCache[0];
				Report.RichTextImageCache.RemoveAt(0);
				stiRichText?.ResetImage(force: true);
			}
		}
	}

	public override SizeD GetActualSize()
	{
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		SizeD actualSizeWin = default(SizeD);
		((SizeD)(ref actualSizeWin))._002Ector(Width, Height);
		if (CanGrow || CanShrink)
		{
			Hashtable hashtable = null;
			if (StiOptions.Engine.AllowCacheForGetActualSize)
			{
				StiReport report = Report;
				if (report?.Engine != null)
				{
					if (report.Engine.HashCheckSize == null)
					{
						report.Engine.HashCheckSize = new Hashtable();
					}
					object obj = report.Engine.HashCheckSize[this];
					if (obj != null)
					{
						return (SizeD)obj;
					}
					hashtable = report.Engine.HashCheckSize;
				}
			}
			if (IsReportWpf)
			{
				StiUnit unit = Page.Unit;
				RectangleD val = unit.ConvertToHInches(ClientRectangle);
				SizeD val2 = StiWpfTextRender.MeasureRtfString(((RectangleD)(ref val)).Width, this);
				((SizeD)(ref actualSizeWin))._002Ector(Width, unit.ConvertFromHInches(((SizeD)(ref val2)).Height));
			}
			else if (StiOptions.Engine.FullTrust)
			{
				if (StiOptions.Engine.ForceLockRichTextThread)
				{
					lock (lockRichTextThread)
					{
						actualSizeWin = GetActualSizeWin();
					}
				}
				else
				{
					actualSizeWin = GetActualSizeWin();
				}
			}
			if (hashtable != null)
			{
				hashtable[this] = actualSizeWin;
			}
		}
		return actualSizeWin;
	}

	private SizeD GetActualSizeWin()
	{
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		bool flag = true;
		int num = 0;
		while (flag && num < 100)
		{
			flag = false;
			try
			{
				StiRichTextBox stiRichTextBox = new StiRichTextBox(drawTransparent: false);
				try
				{
					((TextBoxBase)stiRichTextBox).WordWrap = WordWrap;
					((RichTextBox)stiRichTextBox).DetectUrls = DetectUrls;
					if (RightToLeft)
					{
						((Control)stiRichTextBox).RightToLeft = (RightToLeft)1;
					}
					GetPreparedText((RichTextBox)(object)stiRichTextBox);
					if (((TextBoxBase)stiRichTextBox).TextLength == 0)
					{
						return new SizeD(0.0, 0.0);
					}
					RectangleD rect = GetPaintRectangle(convertToHInches: true, convertByZoom: false);
					rect = ConvertTextMargins(rect, convert: false);
					if (((TextBoxBase)stiRichTextBox).TextLength <= 0)
					{
						continue;
					}
					int num2 = (int)((RectangleD)(ref rect)).Width;
					if (!WordWrap)
					{
						num2 = 100000;
					}
					int num3 = 2;
					if (Wysiwyg || StiDpiHelper.NeedGraphicsRichTextScale)
					{
						lock (StiReport.GlobalRichTextMeasureGraphics)
						{
							int num4 = 0;
							while (num4 < ((TextBoxBase)stiRichTextBox).TextLength)
							{
								int charEnd = 0;
								int num5 = 0;
								for (int i = 0; i < 10; i++)
								{
									num5 = StiRichTextHelper.FormatRange(StiRtfFormatType.TotalRtfHeight, (RichTextBox)(object)stiRichTextBox, new Rectangle(0, 0, num2, (int)((RectangleD)(ref rect)).Height), StiReport.GlobalRichTextMeasureGraphics, num4, -1, out charEnd);
									if (num5 != 1000000)
									{
										break;
									}
									num5 = (int)((RectangleD)(ref rect)).Height;
								}
								num4 = charEnd;
								num3 += num5;
							}
						}
					}
					else
					{
						Graphics val = Graphics.FromHwnd(IntPtr.Zero);
						try
						{
							int num6 = 0;
							while (num6 < ((TextBoxBase)stiRichTextBox).TextLength)
							{
								int charEnd2 = 0;
								int num7 = 0;
								for (int j = 0; j < 10; j++)
								{
									num7 = StiRichTextHelper.FormatRange(StiRtfFormatType.TotalRtfHeight, (RichTextBox)(object)stiRichTextBox, new Rectangle(0, 0, num2, (int)((RectangleD)(ref rect)).Height), val, num6, -1, out charEnd2);
									if (num7 != 1000000)
									{
										break;
									}
									num7 = (int)((RectangleD)(ref rect)).Height;
								}
								num6 = charEnd2;
								num3 += num7;
							}
						}
						finally
						{
							((IDisposable)val)?.Dispose();
						}
					}
					num3 += (int)(Margins.Top + Margins.Bottom);
					double num8 = Report.Unit.ConvertFromHInches((double)num3);
					if (Report.Unit.ConvertToHInches(Math.Round(num8, 2)) < (double)num3)
					{
						num8 += 0.01;
					}
					if (((RichTextBox)stiRichTextBox).Rtf.Contains("{\\pict"))
					{
						num8 *= 1.015;
					}
					return new SizeD(Width, num8);
				}
				finally
				{
					((IDisposable)stiRichTextBox)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				flag = true;
				num++;
				Report?.WriteToReportRenderingMessages(Name + " " + ex.Message);
			}
		}
		return new SizeD(Width, Height);
	}

	internal string GetPreparedText(RichTextBox richTextBox)
	{
		if (base.IsDesigning)
		{
			string rtfFromSource = GetRtfFromSource((DataUrl != null) ? DataUrl.Value : null, DataColumn);
			if (rtfFromSource != null)
			{
				if (StiRtfHelper.IsRtfText(rtfFromSource))
				{
					richTextBox.Rtf = StiExportUtils.CorrectRichTextForRiched20(rtfFromSource);
				}
				else
				{
					((Control)richTextBox).Text = rtfFromSource;
				}
				return rtfFromSource;
			}
		}
		string textInternal = GetTextInternal();
		if (string.IsNullOrEmpty(textInternal))
		{
			if (base.IsDesigning)
			{
				return null;
			}
			((Control)richTextBox).Text = string.Empty;
		}
		else
		{
			int num = textInternal.TrimStart(new char[0]).ToLowerInvariant().IndexOf("rtf");
			if (num != -1 && num < 30)
			{
				string baseText = UnpackRtf(StiRtfHelper.XmlDecodeFast(textInternal));
				richTextBox.Rtf = StiExportUtils.CorrectRichTextForRiched20(baseText);
			}
			else
			{
				((Control)richTextBox).Text = textInternal;
				if (StiOptions.Engine.DefaultRtfFont != null)
				{
					((TextBoxBase)richTextBox).SelectAll();
					richTextBox.SelectionFont = StiOptions.Engine.DefaultRtfFont;
				}
				else if (DefaultFont != null)
				{
					((TextBoxBase)richTextBox).SelectAll();
					richTextBox.SelectionFont = DefaultFont;
				}
				if (!DefaultColor.IsEmpty)
				{
					((TextBoxBase)richTextBox).SelectAll();
					richTextBox.SelectionColor = DefaultColor;
				}
			}
		}
		((TextBoxBase)richTextBox).WordWrap = true;
		string rtf = richTextBox.Rtf;
		if (((Control)richTextBox).Text == string.Empty)
		{
			((Control)richTextBox).Text = " ";
		}
		return rtf;
	}

	public override void InvokeGetValue(StiComponent sender, StiGetValueEventArgs e)
	{
		try
		{
			bool flag = Report != null && Report.CalculationMode == StiCalculationMode.Interpretation;
			string text = null;
			if (flag && DataUrl.Value.Length > 0)
			{
				object obj = StiParser.ParseTextValue(DataUrl.Value, this);
				if (obj != null)
				{
					text = Report.ToString(obj);
				}
			}
			if ((object)base.Events[EventGetDataUrl] != null && text == null)
			{
				StiGetDataUrlEventArgs stiGetDataUrlEventArgs = new StiGetDataUrlEventArgs();
				InvokeGetDataUrl(this, stiGetDataUrlEventArgs);
				if (stiGetDataUrlEventArgs.Value != null)
				{
					text = stiGetDataUrlEventArgs.Value.ToString();
				}
			}
			string text2 = string.Empty;
			try
			{
				string rtfFromSource = GetRtfFromSource(text, DataColumn);
				if (!string.IsNullOrEmpty(rtfFromSource))
				{
					text2 = ((!StiRtfHelper.IsRtfText(rtfFromSource)) ? rtfFromSource : PackRtf(rtfFromSource));
				}
			}
			catch
			{
				if (!StiOptions.Engine.HideExceptions)
				{
					throw;
				}
			}
			if (text2 != null && text2.Length > 0)
			{
				e.Value = text2;
			}
			else
			{
				base.InvokeGetValue(sender, e);
			}
		}
		catch (Exception ex)
		{
			string text3 = "Expression in Text property of '" + Name + "' can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text3);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report?.WriteToReportRenderingMessages(text3);
		}
	}

	public string GetRtfFromSource(string dataUrlValue, string dataColumn)
	{
		string rtfFromUrl = GetRtfFromUrl(dataUrlValue);
		if (!string.IsNullOrEmpty(rtfFromUrl))
		{
			return rtfFromUrl;
		}
		rtfFromUrl = GetRtfFromFile(dataUrlValue);
		if (!string.IsNullOrEmpty(rtfFromUrl))
		{
			return rtfFromUrl;
		}
		rtfFromUrl = GetRtfFromDataColumn(dataColumn);
		if (!string.IsNullOrEmpty(rtfFromUrl))
		{
			return rtfFromUrl;
		}
		return null;
	}

	protected string GetRtfFromUrl(string url)
	{
		try
		{
			if (!string.IsNullOrEmpty(url))
			{
				if (StiHyperlinkProcessor.IsServerHyperlink(url))
				{
					return StiStimulsoftServerResource.GetRichText(this, StiHyperlinkProcessor.GetServerNameFromHyperlink(url));
				}
				string resourceNameFromHyperlink = StiHyperlinkProcessor.GetResourceNameFromHyperlink(url);
				if (resourceNameFromHyperlink != null)
				{
					return StiHyperlinkProcessor.GetString(Report, url);
				}
				string variableNameFromHyperlink = StiHyperlinkProcessor.GetVariableNameFromHyperlink(url);
				if (variableNameFromHyperlink != null)
				{
					return StiHyperlinkProcessor.GetString(Report, url);
				}
				CookieContainer cookieContainer = Report?.CookieContainer;
				return StiRichTextFromURL.LoadRichText(url, cookieContainer);
			}
		}
		catch (Exception ex)
		{
			string text = "The RTF file can't be loaded from URL '" + url + "' in richtext component " + Name + "!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report?.WriteToReportRenderingMessages(text);
		}
		return null;
	}

	protected string GetRtfFromFile(string hyperlink)
	{
		string fileNameFromHyperlink = StiHyperlinkProcessor.GetFileNameFromHyperlink(hyperlink);
		if (fileNameFromHyperlink == null)
		{
			return null;
		}
		if (File.Exists(fileNameFromHyperlink))
		{
			try
			{
				return File.ReadAllText(fileNameFromHyperlink, Encoding.Default);
			}
			catch (Exception ex)
			{
				string text = "The RTF can't be loaded from file '" + fileNameFromHyperlink + "' in richtext component " + Name + "!";
				StiLogService.Write(((object)this).GetType(), text);
				StiLogService.Write(((object)this).GetType(), ex.Message);
				Report.WriteToReportRenderingMessages(text);
			}
		}
		else
		{
			string text2 = "The file '" + fileNameFromHyperlink + "' does not exist in richtext component " + Name + "!";
			StiLogService.Write(((object)this).GetType(), text2);
			Report?.WriteToReportRenderingMessages(text2);
		}
		return null;
	}

	protected string GetRtfFromDataColumn(string dataColumn)
	{
		try
		{
			object dataFromDataColumn = StiDataColumn.GetDataFromDataColumn(Report.Dictionary, dataColumn);
			return (dataFromDataColumn is byte[]) ? Encoding.UTF8.GetString(dataFromDataColumn as byte[]) : (dataFromDataColumn as string);
		}
		catch (Exception ex)
		{
			string text = "The RTF can't be loaded from data column '" + dataColumn + "' in richtext component " + Name + "!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report?.WriteToReportRenderingMessages(text);
		}
		return null;
	}

	protected virtual void OnGetDataUrl(StiGetDataUrlEventArgs e)
	{
	}

	public virtual void InvokeGetDataUrl(StiComponent sender, StiGetDataUrlEventArgs e)
	{
		try
		{
			OnGetDataUrl(e);
			if (base.Events[EventGetDataUrl] is StiGetDataUrlEventHandler stiGetDataUrlEventHandler)
			{
				stiGetDataUrlEventHandler(sender, e);
			}
			StiBlocklyHelper.InvokeBlockly(Report, this, GetDataUrlEvent, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in DataUrl property of '" + Name + "' can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report?.WriteToReportRenderingMessages(text);
		}
	}

	public override StiComponent CreateNew()
	{
		return new StiRichText();
	}

	public override void OnResizeComponent(SizeD oldSize, SizeD newSize)
	{
		ResetImage();
	}

	private bool ShouldSerializeMargins()
	{
		if (Margins != null)
		{
			return !Margins.IsDefault;
		}
		return true;
	}

	public static string PackRtf(string str)
	{
		StringBuilder stringBuilder = new StringBuilder(str);
		StringBuilder stringBuilder2 = new StringBuilder();
		for (int i = 0; i < stringBuilder.Length; i++)
		{
			char c = stringBuilder[i];
			if (c == '{' && (i == 0 || stringBuilder[i - 1] != '\\'))
			{
				stringBuilder2 = stringBuilder2.Append("__LP__");
			}
			else if (c == '}' && (i == 0 || stringBuilder[i - 1] != '\\'))
			{
				stringBuilder2 = stringBuilder2.Append("__RP__");
			}
			else if (c != 0)
			{
				stringBuilder2 = stringBuilder2.Append(c);
			}
		}
		stringBuilder2 = stringBuilder2.Replace("\\{", "{");
		stringBuilder2 = stringBuilder2.Replace("\\}", "}");
		return stringBuilder2.ToString();
	}

	public static string UnpackRtf(string str)
	{
		if (string.IsNullOrEmpty(str))
		{
			return string.Empty;
		}
		StringBuilder stringBuilder = new StringBuilder();
		int i = 0;
		int num = 0;
		for (; i < str.Length; i++)
		{
			switch (str[i])
			{
			case '{':
				if (i > num)
				{
					stringBuilder.Append(str, num, i - num);
				}
				stringBuilder.Append("\\{");
				num = i + 1;
				break;
			case '}':
				if (i > num)
				{
					stringBuilder.Append(str, num, i - num);
				}
				stringBuilder.Append("\\}");
				num = i + 1;
				break;
			case '_':
				if (i + 5 < str.Length && str[i + 1] == '_' && (str[i + 2] == 'L' || str[i + 2] == 'R') && str[i + 3] == 'P' && str[i + 4] == '_' && str[i + 5] == '_')
				{
					if (i > num)
					{
						stringBuilder.Append(str, num, i - num);
					}
					stringBuilder.Append((str[i + 2] == 'L') ? "{" : "}");
					i += 5;
					num = i + 1;
				}
				break;
			}
		}
		if (i > num)
		{
			stringBuilder.Append(str, num, i - num);
		}
		return stringBuilder.ToString();
	}

	public override string GetTextInternal()
	{
		if (StiOptions.Engine.RtfCache.Enabled && Report != null && !base.IsDesigning && base.Guid != null)
		{
			string rtfCacheName = StiRtfCache.GetRtfCacheName(Report.RtfCachePath, base.Guid);
			if (File.Exists(rtfCacheName))
			{
				return RemovePageBreakTag(StiRtfCache.LoadRtf(rtfCacheName));
			}
		}
		return RemovePageBreakTag(base.GetTextInternal());
	}

	public override void SetTextInternal(string value)
	{
		if (StiOptions.Engine.RtfCache.Enabled && Report != null && !base.IsDesigning)
		{
			if (string.IsNullOrEmpty(Report.RtfCachePath))
			{
				Report.RtfCachePath = StiRtfCache.CreateNewCache();
			}
			if (base.Guid == null)
			{
				NewGuid();
			}
			string rtfCacheName = StiRtfCache.GetRtfCacheName(Report.RtfCachePath, base.Guid);
			StiRtfCache.SaveRtf(value, rtfCacheName);
		}
		else
		{
			base.SetTextInternal(value);
		}
	}

	private static string RemovePageBreakTag(string inputString)
	{
		if (string.IsNullOrEmpty(inputString))
		{
			return inputString;
		}
		inputString = RemoveToken(inputString, "_x005C_pagebb");
		inputString = RemoveToken(inputString, "_x005C_page");
		inputString = RemoveToken(inputString, "\\pagebb");
		inputString = RemoveToken(inputString, "\\page");
		return inputString;
	}

	private static string RemoveToken(string inputString, string token)
	{
		int num = 0;
		while (true)
		{
			num = inputString.IndexOf(token, num, StringComparison.InvariantCulture);
			if (num == -1)
			{
				break;
			}
			int num2 = num + token.Length;
			if (!char.IsLetterOrDigit(inputString[num2]))
			{
				inputString = inputString.Remove(num, num2 - num);
			}
			else
			{
				num = num2;
			}
		}
		return inputString;
	}

	public void SetFont(Font font)
	{
		StiRichTextBox stiRichTextBox = new StiRichTextBox(drawTransparent: false);
		try
		{
			((RichTextBox)stiRichTextBox).Rtf = RtfText;
			((TextBoxBase)stiRichTextBox).SelectAll();
			((RichTextBox)stiRichTextBox).SelectionFont = font;
			RtfText = ((RichTextBox)stiRichTextBox).Rtf;
		}
		finally
		{
			((IDisposable)stiRichTextBox)?.Dispose();
		}
	}

	public void SetColor(Color color)
	{
		StiRichTextBox stiRichTextBox = new StiRichTextBox(drawTransparent: false);
		try
		{
			((RichTextBox)stiRichTextBox).Rtf = RtfText;
			((TextBoxBase)stiRichTextBox).SelectAll();
			((RichTextBox)stiRichTextBox).SelectionColor = color;
			RtfText = ((RichTextBox)stiRichTextBox).Rtf;
		}
		finally
		{
			((IDisposable)stiRichTextBox)?.Dispose();
		}
	}

	public void ResetImage(bool force = false)
	{
		if (!base.IsDesigning && !force)
		{
			return;
		}
		try
		{
			Image image = Image;
			if (image != null)
			{
				image.Dispose();
			}
			Image = null;
		}
		catch
		{
		}
	}

	internal void ResetSourceProperties()
	{
		ResetImage();
		DataUrl.Value = string.Empty;
		Text.Value = string.Empty;
		DataColumn = string.Empty;
	}

	public StiRichText()
		: this(RectangleD.Empty, string.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiRichText(RectangleD rect)
		: this(rect, string.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiRichText(RectangleD rect, string text)
		: base(rect, text)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
	}
}
