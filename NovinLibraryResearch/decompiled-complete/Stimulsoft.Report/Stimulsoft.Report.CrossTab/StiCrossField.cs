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
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.TextFormats;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.PropertyGrid;

namespace Stimulsoft.Report.CrossTab;

[StiToolbox(false)]
[StiServiceBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.StiCrossTab.png")]
public abstract class StiCrossField : StiText, IStiCrossTabField
{
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue("")]
	[Editor("Stimulsoft.Report.Components.TextFormats.Design.StiTextFormatEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(170)]
	[Description("Gets or sets the format of the text.")]
	public override StiFormatService TextFormat
	{
		get
		{
			return base.TextFormat;
		}
		set
		{
			base.TextFormat = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(300)]
	[StiSerializable]
	[Description("The brush of the component, which is used to display text.")]
	[TypeConverter(typeof(StiExpressionBrushConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionBrushEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public override StiBrush TextBrush
	{
		get
		{
			return base.TextBrush;
		}
		set
		{
			base.TextBrush = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(180)]
	[StiSerializable]
	[Description("Gets or sets font of component.")]
	public override Font Font
	{
		get
		{
			return base.Font;
		}
		set
		{
			base.Font = value;
		}
	}

	protected override StiTextHorAlignment DefaultHorAlignment => (StiTextHorAlignment)1;

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets the text horizontal alignment.")]
	[StiOrder(130)]
	public override StiTextHorAlignment HorAlignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.HorAlignment;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.HorAlignment = value;
		}
	}

	[StiSerializable]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets the vertical alignment of an object.")]
	[StiOrder(140)]
	public override StiVertAlignment VertAlignment
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.VertAlignment;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.VertAlignment = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiExcelValueExpression ExcelValue
	{
		get
		{
			return base.ExcelValue;
		}
		set
		{
			base.ExcelValue = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override StiPrintOnType PrintOn
	{
		get
		{
			return base.PrintOn;
		}
		set
		{
			base.PrintOn = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public sealed override bool ExportAsImage
	{
		get
		{
			return base.ExportAsImage;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override bool ShrinkFontToFit
	{
		get
		{
			return base.ShrinkFontToFit;
		}
		set
		{
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override float ShrinkFontToFitMinimumSize
	{
		get
		{
			return base.ShrinkFontToFitMinimumSize;
		}
		set
		{
		}
	}

	[Browsable(false)]
	public sealed override string RenderTo
	{
		get
		{
			return base.RenderTo;
		}
		set
		{
			base.RenderTo = value;
		}
	}

	[Browsable(false)]
	public sealed override string Alias
	{
		get
		{
			return base.Alias;
		}
		set
		{
			base.Alias = value;
		}
	}

	[Browsable(false)]
	public sealed override string Name
	{
		get
		{
			return base.Name;
		}
		set
		{
			if (base.Name != value && !string.IsNullOrEmpty(base.Name) && Parent != null)
			{
				foreach (object item in Parent?.Components)
				{
					if (this is StiCrossRow && item is StiCrossTitle stiCrossTitle && stiCrossTitle.TypeOfComponent == "Row:" + base.Name)
					{
						stiCrossTitle.TypeOfComponent = "Row:" + value;
					}
					else if (this is StiCrossColumn && item is StiCrossTitle stiCrossTitle2 && stiCrossTitle2.TypeOfComponent == "Col:" + base.Name)
					{
						stiCrossTitle2.TypeOfComponent = "Row:" + value;
					}
				}
			}
			base.Name = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool Locked
	{
		get
		{
			if (base.IsDesigning)
			{
				return !Report.IsPageDesigner;
			}
			return false;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool Linked
	{
		get
		{
			if (base.IsDesigning)
			{
				return !Report.IsPageDesigner;
			}
			return false;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool CanBreak
	{
		get
		{
			return base.CanBreak;
		}
		set
		{
			base.CanBreak = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool WYSIWYG
	{
		get
		{
			return base.WYSIWYG;
		}
		set
		{
			base.WYSIWYG = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool Editable
	{
		get
		{
			return base.Editable;
		}
		set
		{
			base.Editable = value;
		}
	}

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

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiProcessingDuplicatesType ProcessingDuplicates
	{
		get
		{
			return StiProcessingDuplicatesType.None;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool OnlyText
	{
		get
		{
			return base.OnlyText;
		}
		set
		{
			base.OnlyText = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool ProcessAtEnd
	{
		get
		{
			return base.ProcessAtEnd;
		}
		set
		{
			base.ProcessAtEnd = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiProcessAt ProcessAt
	{
		get
		{
			return base.ProcessAt;
		}
		set
		{
			base.ProcessAt = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override int MaxNumberOfLines
	{
		get
		{
			return base.MaxNumberOfLines;
		}
		set
		{
			base.MaxNumberOfLines = value;
		}
	}

	[Browsable(false)]
	public override bool Enabled
	{
		get
		{
			return base.Enabled;
		}
		set
		{
			base.Enabled = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiDockStyle DockStyle
	{
		get
		{
			return base.DockStyle;
		}
		set
		{
			base.DockStyle = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool GrowToHeight
	{
		get
		{
			return base.GrowToHeight;
		}
		set
		{
			base.GrowToHeight = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override StiAnchorMode Anchor
	{
		get
		{
			return base.Anchor;
		}
		set
		{
			base.Anchor = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiShiftMode ShiftMode
	{
		get
		{
			return base.ShiftMode;
		}
		set
		{
			base.ShiftMode = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override double Left
	{
		get
		{
			return base.Left;
		}
		set
		{
			base.Left = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override double Top
	{
		get
		{
			return base.Top;
		}
		set
		{
			base.Top = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override double Width
	{
		get
		{
			return base.Width;
		}
		set
		{
			base.Width = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override double Height
	{
		get
		{
			return base.Height;
		}
		set
		{
			base.Height = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool CanShrink
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool CanGrow
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool AutoWidth
	{
		get
		{
			return base.AutoWidth;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool Printable
	{
		get
		{
			return base.Printable;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiPointerExpression Pointer
	{
		get
		{
			return base.Pointer;
		}
		set
		{
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiBookmarkExpression Bookmark
	{
		get
		{
			return base.Bookmark;
		}
		set
		{
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiGetPointerEvent GetPointerEvent
	{
		get
		{
			return base.GetPointerEvent;
		}
		set
		{
			base.GetPointerEvent = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiGetBookmarkEvent GetBookmarkEvent
	{
		get
		{
			return base.GetBookmarkEvent;
		}
		set
		{
			base.GetBookmarkEvent = value;
		}
	}

	[StiSerializable]
	[Browsable(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a script of the event ProcessCellEvent.")]
	public StiProcessCellEvent ProcessCellEvent { get; set; } = new StiProcessCellEvent();


	public override string HelpUrl => null;

	public override string LocalizedCategory => StiLocalization.Get("Report", "CrossBands");

	[Browsable(false)]
	public override StiRestrictions Restrictions
	{
		get
		{
			if (Report == null || !Report.IsPageDesigner)
			{
				return base.Restrictions;
			}
			return base.Restrictions ^ StiRestrictions.AllowDelete;
		}
		set
		{
			if (StiRestrictionsHelper.IsAllowDelete(value))
			{
				value ^= StiRestrictions.AllowDelete;
			}
			base.Restrictions = value;
		}
	}

	[Browsable(false)]
	public virtual string CellText => Alias;

	[DefaultValue(true)]
	[StiSerializable]
	[Browsable(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(290)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that all equal values of header will be merged into one.")]
	public bool MergeHeaders { get; set; } = true;


	[Browsable(false)]
	public object OriginalValue { get; set; }

	[Browsable(false)]
	public bool DisabledByCondition { get; set; }

	[Browsable(false)]
	public StiBrush ConditionBrush { get; set; }

	[Browsable(false)]
	public StiBrush ConditionTextBrush { get; set; }

	[Browsable(false)]
	public StiConditionPermissions ConditionPermissions { get; set; }

	public event StiProcessCellEventHandler ProcessCell;

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.AddPropertyEnum(val, "HorAlignment", (Enum)(object)HorAlignment, (Enum)(object)(StiTextHorAlignment)1);
		JsonExtensions.AddPropertyEnum(val, "VertAlignment", (Enum)(object)VertAlignment, (Enum)(object)(StiVertAlignment)1);
		JsonExtensions.RemoveProperty(val, "ExcelValue");
		JsonExtensions.RemoveProperty(val, "PrintOn");
		JsonExtensions.RemoveProperty(val, "ExportAsImage");
		JsonExtensions.RemoveProperty(val, "ShrinkFontToFit");
		JsonExtensions.RemoveProperty(val, "ShrinkFontToFitMinimumSize");
		JsonExtensions.RemoveProperty(val, "Locked");
		JsonExtensions.RemoveProperty(val, "Linked");
		JsonExtensions.RemoveProperty(val, "CanBreak");
		JsonExtensions.RemoveProperty(val, "WYSIWYG");
		JsonExtensions.RemoveProperty(val, "Editable");
		JsonExtensions.RemoveProperty(val, "GlobalizedName");
		JsonExtensions.RemoveProperty(val, "LinesOfUnderline");
		JsonExtensions.RemoveProperty(val, "ProcessingDuplicates");
		JsonExtensions.RemoveProperty(val, "OnlyText");
		JsonExtensions.RemoveProperty(val, "ProcessAtEnd");
		JsonExtensions.RemoveProperty(val, "ProcessAt");
		JsonExtensions.RemoveProperty(val, "MaxNumberOfLines");
		JsonExtensions.RemoveProperty(val, "DockStyle");
		JsonExtensions.RemoveProperty(val, "GrowToHeight");
		JsonExtensions.RemoveProperty(val, "ShiftMode");
		JsonExtensions.RemoveProperty(val, "Left");
		JsonExtensions.RemoveProperty(val, "Top");
		JsonExtensions.RemoveProperty(val, "Width");
		JsonExtensions.RemoveProperty(val, "Height");
		JsonExtensions.RemoveProperty(val, "AutoWidth");
		JsonExtensions.RemoveProperty(val, "Printable");
		JsonExtensions.RemoveProperty(val, "Pointer");
		JsonExtensions.RemoveProperty(val, "GetPointerEvent");
		JsonExtensions.RemoveProperty(val, "Bookmark");
		JsonExtensions.RemoveProperty(val, "GetBookmarkEvent");
		JsonExtensions.AddPropertyJObject(val, "ProcessCellEvent", ProcessCellEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyBool(val, "MergeHeaders", MergeHeaders, true, false);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			string text = item.Name;
			if (!(text == "ProcessCellEvent"))
			{
				if (text == "MergeHeaders")
				{
					MergeHeaders = StiJsonReportObjectHelper.DeserializeBool(item);
				}
			}
			else
			{
				StiProcessCellEvent stiProcessCellEvent = new StiProcessCellEvent();
				stiProcessCellEvent.LoadFromJsonObject((JObject)item.Value);
				ProcessCellEvent = stiProcessCellEvent;
			}
		}
	}

	public override StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		StiEventCollection stiEventCollection = new StiEventCollection();
		stiEventCollection.Add(StiPropertyCategories.DataEvents, new StiPropertyEventId[1] { StiPropertyEventId.ProcessCellEvent });
		stiEventCollection.Add(StiPropertyCategories.ValueEvents, new StiPropertyEventId[4]
		{
			StiPropertyEventId.GetExcelValueEvent,
			StiPropertyEventId.GetValueEvent,
			StiPropertyEventId.GetToolTipEvent,
			StiPropertyEventId.GetTagEvent
		});
		stiEventCollection.Add(StiPropertyCategories.NavigationEvents, new StiPropertyEventId[2]
		{
			StiPropertyEventId.GetHyperlinkEvent,
			StiPropertyEventId.GetBookmarkEvent
		});
		stiEventCollection.Add(StiPropertyCategories.PrintEvents, new StiPropertyEventId[2]
		{
			StiPropertyEventId.BeforePrintEvent,
			StiPropertyEventId.AfterPrintEvent
		});
		stiEventCollection.Add(StiPropertyCategories.MouseEvents, new StiPropertyEventId[5]
		{
			StiPropertyEventId.GetDrillDownReportEvent,
			StiPropertyEventId.ClickEvent,
			StiPropertyEventId.DoubleClickEvent,
			StiPropertyEventId.MouseEnterEvent,
			StiPropertyEventId.MouseLeaveEvent
		});
		return stiEventCollection;
	}

	protected virtual void OnProcessCell(StiProcessCellEventArgs e)
	{
	}

	public void InvokeProcessCell(StiProcessCellEventArgs e)
	{
		OnProcessCell(e);
		this.ProcessCell?.Invoke(this, e);
	}

	public override string ToString()
	{
		return Name;
	}

	public StiText GetTextBoxFromField()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		return new StiText
		{
			Border = (StiBorder)base.Border.Clone(),
			Brush = (StiBrush)base.Brush.Clone(),
			Font = (Font)Font.Clone(),
			TextFormat = (StiFormatService)((StiService)TextFormat).Clone(),
			TextBrush = (StiBrush)TextBrush.Clone(),
			TextOptions = (StiTextOptions)TextOptions.Clone(),
			HorAlignment = HorAlignment,
			VertAlignment = VertAlignment
		};
	}

	public StiCrossField()
		: base(new RectangleD(0.0, 0.0, 1.0, 1.0))
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		NewGuid();
		base.Border.Side = (StiBorderSides)15;
		VertAlignment = (StiVertAlignment)1;
		HorAlignment = (StiTextHorAlignment)1;
		Restrictions = StiRestrictions.AllowMove | StiRestrictions.AllowResize | StiRestrictions.AllowSelect | StiRestrictions.AllowChange;
	}
}
