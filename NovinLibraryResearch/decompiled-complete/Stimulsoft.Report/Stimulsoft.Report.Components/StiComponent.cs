using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;
using Stimulsoft.Base.Services;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.CrossTab;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.PropertyGrid;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Components;

[StiServiceBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.StiComponent.png")]
[StiServiceCategoryBitmap(typeof(StiComponent), "Stimulsoft.Report.Images.Components.catComponents.png")]
[StiV1Builder(typeof(StiComponentV1Builder))]
[StiV2Builder(typeof(StiComponentV2Builder))]
[StiGdiPainter(typeof(StiComponentGdiPainter))]
[StiWpfPainter("Stimulsoft.Report.Painters.StiComponentWpfPainter, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiToolbox(false)]
[StiDesigner(typeof(StiComponentDesigner))]
[StiWpfDesigner("Stimulsoft.Report.Wpf.Designer.StiWpfComponentDesigner, Stimulsoft.Report.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public abstract class StiComponent : StiBase, IStiReportComponent, IStiAppComponent, IStiAppCell, IStiComponentGuid, IStiCanGrow, IStiCanShrink, IStiUnitConvert, IStiShift, IStiGrowToHeight, IStiAnchor, IStiConditions, IStiPrintOn, IStiInherited, IStiReportProperty, IStiInteraction, IStiStateSaveRestore, IStiSelect, IStiPropertyGridObject, IStiJsonReportObject, IStiGetFonts, IStiAppExpressionCollection
{
	private class bitsComponent : ICloneable
	{
		public object bookmarkValue;

		public object toolTipValue;

		public object hyperlinkValue;

		public object tagValue;

		public bool enabled;

		public StiHighlightState highlightState;

		public bool ignoreNamingRule;

		public StiDockStyle dockStyle;

		public bool printable;

		public object Clone()
		{
			return MemberwiseClone();
		}

		public bitsComponent(object bookmarkValue, object toolTipValue, object hyperlinkValue, object tagValue, bool enabled, StiHighlightState highlightState, bool ignoreNamingRule, StiDockStyle dockStyle, bool printable)
		{
			this.bookmarkValue = bookmarkValue;
			this.toolTipValue = toolTipValue;
			this.hyperlinkValue = hyperlinkValue;
			this.tagValue = tagValue;
			this.enabled = enabled;
			this.highlightState = highlightState;
			this.ignoreNamingRule = ignoreNamingRule;
			this.dockStyle = dockStyle;
			this.printable = printable;
		}
	}

	protected static object PropertySelectionTick = new object();

	private bool isSelected;

	private StiStatesManager states;

	private StiShiftMode shiftMode = StiShiftMode.IncreasingSize;

	private bool canGrow;

	private bool growToHeight;

	private StiAnchorMode anchor = StiAnchorMode.Top | StiAnchorMode.Left;

	private StiConditionsCollection conditions;

	protected static object PropertyInherited = new object();

	private StiInteraction interaction;

	protected static object PropertyRenderedCount = new object();

	private bool disableCheckWidthHeight;

	protected static object PropertyMinSize = new object();

	protected static object PropertyMaxSize = new object();

	private double left;

	private double top;

	private double width;

	private double height;

	private static readonly object EventGetToolTip = new object();

	private static readonly object EventGetHyperlink = new object();

	private static readonly object EventGetTag = new object();

	private static readonly object EventGetPointer = new object();

	private static readonly object EventGetBookmark = new object();

	private static readonly object EventBeforePrint = new object();

	private static readonly object EventAfterPrint = new object();

	private static readonly object EventGetDrillDownReport = new object();

	private static readonly object EventClick = new object();

	private static readonly object EventDoubleClick = new object();

	private static readonly object EventMouseEnter = new object();

	private static readonly object EventMouseLeave = new object();

	private static readonly object EventPainting = new object();

	private static readonly object EventPainted = new object();

	private string alias = "";

	private EventHandlerList events;

	protected static object PropertyRestrictions = new object();

	protected static object PropertyShowQuickButtons = new object();

	protected static object PropertyPlaceOnToolbox = new object();

	protected static object PropertyDelimiterComponent = new object();

	protected static object PropertyDockable = new object();

	protected static object PropertyComponentStyle = new object();

	protected static object PropertyLocked = new object();

	protected static object PropertyLinked = new object();

	protected static object PropertyUseParentStyles = new object();

	private bool lockOnResize = true;

	private bitsComponent bits;

	[Browsable(false)]
	public virtual StiComponentId ComponentId => StiComponentId.StiComponent;

	[Browsable(false)]
	public string PropName => base.Name;

	[Browsable(false)]
	public virtual int SelectionTick
	{
		get
		{
			return ((StiService)this).Properties.GetInt(PropertySelectionTick, 0);
		}
		set
		{
			((StiService)this).Properties.SetInt(PropertySelectionTick, value, 0);
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue(false)]
	public virtual bool IsSelected
	{
		get
		{
			return isSelected;
		}
		set
		{
			if (!IsSelected)
			{
				SelectionTick = Environment.TickCount;
			}
			isSelected = value;
		}
	}

	protected StiStatesManager States => states ?? (states = new StiStatesManager());

	[DefaultValue(true)]
	[Browsable(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates that this component can be shifted.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool Shift
	{
		get
		{
			return ShiftMode == StiShiftMode.IncreasingSize;
		}
		set
		{
			ShiftMode = StiShiftMode.IncreasingSize;
		}
	}

	[DefaultValue(StiShiftMode.IncreasingSize)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(440)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets value which indicates the shift mode of the component.")]
	[Editor("Stimulsoft.Report.Components.Design.StiShiftModeEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiShiftMode ShiftMode
	{
		get
		{
			return shiftMode;
		}
		set
		{
			shiftMode = value;
			if (value != 0)
			{
				Anchor = StiAnchorMode.Top | StiAnchorMode.Left;
			}
		}
	}

	[StiSerializable]
	[Browsable(false)]
	[DefaultValue(null)]
	public string Guid { get; set; }

	[StiSerializable]
	[DefaultValue(StiPrintOnType.AllPages)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(360)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets value which indicates on which pages component will be printed.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiPrintOnType PrintOn { get; set; }

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that this object can shrink.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool CanShrink { get; set; }

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that this object can grow.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool CanGrow
	{
		get
		{
			return canGrow;
		}
		set
		{
			canGrow = value;
		}
	}

	[DefaultValue(false)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(190)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value which indicates that the height of this component increases/decreases to the bottom of a container.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool GrowToHeight
	{
		get
		{
			return growToHeight;
		}
		set
		{
			growToHeight = value;
			if (value)
			{
				Anchor = StiAnchorMode.Top | StiAnchorMode.Left;
			}
		}
	}

	[DefaultValue(StiAnchorMode.Top | StiAnchorMode.Left)]
	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a value which indicates the mode of linking component location to the parent component size.")]
	[Editor("Stimulsoft.Report.Components.Design.StiAnchorModeEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiAnchorMode Anchor
	{
		get
		{
			return anchor;
		}
		set
		{
			anchor = value;
			if (anchor != (StiAnchorMode.Top | StiAnchorMode.Left))
			{
				GrowToHeight = false;
				ShiftMode = StiShiftMode.None;
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[TypeConverter(typeof(StiConditionsCollectionConverter))]
	[Editor("Stimulsoft.Report.Components.Design.StiConditionsCollectionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiConditionsCollection Conditions
	{
		get
		{
			return conditions ?? (conditions = new StiConditionsCollection());
		}
		set
		{
			conditions = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	[StiSerializable]
	public bool Inherited
	{
		get
		{
			return ((StiService)this).Properties.GetBool(PropertyInherited, false);
		}
		set
		{
			((StiService)this).Properties.SetBool(PropertyInherited, value, false);
		}
	}

	[Browsable(false)]
	[Description("Gets or sets the report in which the component is located.")]
	public virtual StiReport Report
	{
		get
		{
			return Page?.Report;
		}
		set
		{
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(200)]
	[Editor("Stimulsoft.Report.Components.Design.StiInteractionEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets interaction options of this component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiInteraction Interaction
	{
		get
		{
			return interaction;
		}
		set
		{
			if (interaction != value)
			{
				interaction = value;
				if (value != null)
				{
					interaction.ParentComponent = this;
				}
			}
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	public virtual StiAppExpressionCollection Expressions { get; set; } = new StiAppExpressionCollection();


	[Browsable(false)]
	public virtual string HelpUrl => null;

	[Browsable(false)]
	public override string ServiceCategory => StiLocalization.Get("Report", "Components");

	[Browsable(false)]
	public override Type ServiceType => typeof(StiComponent);

	[StiSerializable]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(310)]
	[TypeConverter(typeof(StiExpressionBoolConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionBoolEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets value which indicates whether the component is printable or not.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiExpressionAllowed]
	public virtual bool Printable
	{
		get
		{
			if (bits == null)
			{
				return true;
			}
			return bits.printable;
		}
		set
		{
			if (!value || bits != null)
			{
				if (bits != null)
				{
					bits.printable = value;
				}
				else
				{
					bits = new bitsComponent(BookmarkValue, ToolTipValue, HyperlinkValue, TagValue, Enabled, HighlightState, IgnoreNamingRule, DockStyle, value);
				}
			}
		}
	}

	[Browsable(false)]
	public virtual bool IsRendered { get; set; }

	[Browsable(false)]
	public int RenderedCount
	{
		get
		{
			return ((StiService)this).Properties.GetInt(PropertyRenderedCount, 0);
		}
		set
		{
			((StiService)this).Properties.SetInt(PropertyRenderedCount, value, 0);
		}
	}

	[Browsable(false)]
	public virtual bool IsEnabled
	{
		get
		{
			if (Enabled)
			{
				return AllowPrintOn();
			}
			return false;
		}
	}

	[Browsable(false)]
	public virtual bool InvalidateOnMouseOver => false;

	[StiNonSerialized]
	[Browsable(false)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a type of the component docking.")]
	[Obsolete("Please use DockStyle property instead Dock property.")]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public virtual DockStyle Dock
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return ConvertDockStyle(DockStyle);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			DockStyle = ConvertDock(value);
		}
	}

	[StiSerializable]
	[DefaultValue(StiDockStyle.None)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(160)]
	[Description("Gets or sets a type of the component docking.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiExpressionEnumConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionEnumEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiExpressionAllowed]
	public virtual StiDockStyle DockStyle
	{
		get
		{
			if (bits != null)
			{
				return bits.dockStyle;
			}
			return StiDockStyle.None;
		}
		set
		{
			if (value != StiDockStyle.None || bits != null)
			{
				if (bits != null)
				{
					bits.dockStyle = value;
				}
				else
				{
					bits = new bitsComponent(BookmarkValue, ToolTipValue, HyperlinkValue, TagValue, Enabled, HighlightState, IgnoreNamingRule, value, Printable);
				}
			}
		}
	}

	[Browsable(false)]
	public virtual bool IsAutomaticDock => false;

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(500)]
	[Description("Gets or sets minimal size.")]
	[StiSerializable]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual SizeD MinSize
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			return (SizeD)((StiService)this).Properties.Get(PropertyMinSize, (object)SizeD.Empty);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			SizeD minSize = MinSize;
			if (((SizeD)(ref minSize)).Width != ((SizeD)(ref value)).Width || ((SizeD)(ref minSize)).Height != ((SizeD)(ref value)).Height)
			{
				((StiService)this).Properties.Set(PropertyMinSize, (object)value, (object)SizeD.Empty);
				width = CheckWidth(width);
				height = CheckHeight(height);
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(510)]
	[Description("Gets or sets maximal size.")]
	[StiSerializable]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual SizeD MaxSize
	{
		get
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			return (SizeD)((StiService)this).Properties.Get(PropertyMaxSize, (object)SizeD.Empty);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			SizeD maxSize = MaxSize;
			if (((SizeD)(ref maxSize)).Width != ((SizeD)(ref value)).Width || ((SizeD)(ref maxSize)).Height != ((SizeD)(ref value)).Height)
			{
				((StiService)this).Properties.Set(PropertyMaxSize, (object)value, (object)SizeD.Empty);
				width = CheckWidth(width);
				height = CheckHeight(height);
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(100)]
	[Description("Gets or sets left position of the component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual double Left
	{
		get
		{
			return left;
		}
		set
		{
			left = Math.Round(value, 2);
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[Description("Gets or sets top position of the component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual double Top
	{
		get
		{
			return top;
		}
		set
		{
			top = Math.Round(value, 2);
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[Description("Gets or sets width of the component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual double Width
	{
		get
		{
			return width;
		}
		set
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			double num = width;
			width = CheckWidth(Math.Round(value, 2));
			if (width != num)
			{
				InvokeOnResizeComponent(new SizeD(num, height), new SizeD(width, height));
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[Description("Gets or sets height of the component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual double Height
	{
		get
		{
			return height;
		}
		set
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			double num = height;
			height = CheckHeight(Math.Round(value, 2));
			if (height != num)
			{
				InvokeOnResizeComponent(new SizeD(width, num), new SizeD(width, height));
			}
		}
	}

	[Browsable(false)]
	[Description("Gets right position of the component.")]
	public virtual double Right => Left + Width;

	[Browsable(false)]
	[Description("Gets bottom position of the component.")]
	public virtual double Bottom => Top + Height;

	[Browsable(false)]
	[StiSerializable]
	public virtual RectangleD ClientRectangle
	{
		get
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			return new RectangleD(Left, Top, Width, Height);
		}
		set
		{
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			double num = width;
			double num2 = height;
			left = Math.Round(((RectangleD)(ref value)).Left, 2);
			top = Math.Round(((RectangleD)(ref value)).Top, 2);
			width = CheckWidth(Math.Round(((RectangleD)(ref value)).Width, 2));
			height = CheckHeight(Math.Round(((RectangleD)(ref value)).Height, 2));
			if (width != num || height != num2)
			{
				InvokeOnResizeComponent(new SizeD(num, num2), new SizeD(width, height));
			}
		}
	}

	[Browsable(false)]
	public virtual RectangleD DisplayRectangle
	{
		get
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			return new RectangleD(Left, Top, Width, Height);
		}
		set
		{
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			double num = width;
			double num2 = height;
			left = Math.Round(((RectangleD)(ref value)).Left, 2);
			top = Math.Round(((RectangleD)(ref value)).Top, 2);
			width = CheckWidth(Math.Round(((RectangleD)(ref value)).Width, 2));
			height = CheckHeight(Math.Round(((RectangleD)(ref value)).Height, 2));
			if (width != num || height != num2)
			{
				InvokeOnResizeComponent(new SizeD(num, num2), new SizeD(width, height));
			}
		}
	}

	[Browsable(false)]
	public virtual RectangleD SelectRectangle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return DisplayRectangle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			DisplayRectangle = value;
		}
	}

	[Browsable(false)]
	public virtual RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 60.0, 20.0);

	[Browsable(false)]
	public StiBookmark ParentBookmark { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiBookmark CurrentBookmark { get; set; }

	[Browsable(false)]
	public StiBookmark ParentPointer { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiBookmark CurrentPointer { get; set; }

	internal bool IsGetToolTipHandlerEmpty => (object)Events[EventGetToolTip] == null;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting the ToolTip for the component.")]
	public virtual StiGetToolTipEvent GetToolTipEvent
	{
		get
		{
			return new StiGetToolTipEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	internal bool IsGetHyperlinkHandlerEmpty => (object)Events[EventGetHyperlink] == null;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting Hyperlink for the component.")]
	public virtual StiGetHyperlinkEvent GetHyperlinkEvent
	{
		get
		{
			return new StiGetHyperlinkEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	internal bool IsGetTagHandlerEmpty => (object)Events[EventGetTag] == null;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting a Tag for a component.")]
	public virtual StiGetTagEvent GetTagEvent
	{
		get
		{
			return new StiGetTagEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	internal bool IsGetPointerHandlerEmpty => (object)Events[EventGetPointer] == null;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting of the Pointer for the component.")]
	public virtual StiGetPointerEvent GetPointerEvent
	{
		get
		{
			return new StiGetPointerEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	internal bool IsGetBookmarkHandlerEmpty => (object)Events[EventGetBookmark] == null;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when getting of the Bookmark for the component.")]
	public virtual StiGetBookmarkEvent GetBookmarkEvent
	{
		get
		{
			return new StiGetBookmarkEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs before printing of the component.")]
	public virtual StiBeforePrintEvent BeforePrintEvent
	{
		get
		{
			return new StiBeforePrintEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs after the component printing.")]
	public virtual StiAfterPrintEvent AfterPrintEvent
	{
		get
		{
			return new StiAfterPrintEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when it is required to get a report for the Drill-Down operation.")]
	public virtual StiGetDrillDownReportEvent GetDrillDownReportEvent
	{
		get
		{
			return new StiGetDrillDownReportEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	internal bool IsClickHandlerEmpty => (object)Events[EventClick] == null;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when user clicks on the component in the window of viewer.")]
	public virtual StiClickEvent ClickEvent
	{
		get
		{
			return new StiClickEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	internal bool IsDoubleClickHandlerEmpty => (object)Events[EventDoubleClick] == null;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when user double clicks on the component in the window of viewer.")]
	public virtual StiDoubleClickEvent DoubleClickEvent
	{
		get
		{
			return new StiDoubleClickEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	internal bool IsMouseEnterHandlerEmpty => (object)Events[EventMouseEnter] == null;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when user enters the mouse into the area of the component in the window of viewer.")]
	public virtual StiMouseEnterEvent MouseEnterEvent
	{
		get
		{
			return new StiMouseEnterEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	internal bool IsMouseLeaveHandlerEmpty => (object)Events[EventMouseLeave] == null;

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Occurs when user leaves the mouse out of the area of the component in the window of viewer.")]
	public virtual StiMouseLeaveEvent MouseLeaveEvent
	{
		get
		{
			return new StiMouseLeaveEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the component pointer in the table of contents.")]
	public object PointerValue { get; set; }

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a component pointer in the table of contents.")]
	public virtual StiPointerExpression Pointer
	{
		get
		{
			return new StiPointerExpression(this, "Pointer");
		}
		set
		{
			value?.Set(this, "Pointer", value.Value);
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the component bookmark.")]
	public object BookmarkValue
	{
		get
		{
			return bits?.bookmarkValue;
		}
		set
		{
			if (value != null || bits != null)
			{
				if (bits != null)
				{
					bits.bookmarkValue = value;
				}
				else
				{
					bits = new bitsComponent(value, ToolTipValue, HyperlinkValue, TagValue, Enabled, HighlightState, IgnoreNamingRule, DockStyle, Printable);
				}
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[StiOrder(110)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a component bookmark.")]
	public virtual StiBookmarkExpression Bookmark
	{
		get
		{
			return new StiBookmarkExpression(this, "Bookmark");
		}
		set
		{
			value?.Set(this, "Bookmark", value.Value);
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a component tips.")]
	public object ToolTipValue
	{
		get
		{
			return bits?.toolTipValue;
		}
		set
		{
			if (value != null || bits != null)
			{
				if (bits != null)
				{
					bits.toolTipValue = value;
				}
				else
				{
					bits = new bitsComponent(BookmarkValue, value, HyperlinkValue, TagValue, Enabled, HighlightState, IgnoreNamingRule, DockStyle, Printable);
				}
			}
		}
	}

	[Browsable(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(120)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a component tooltip.")]
	public virtual StiToolTipExpression ToolTip
	{
		get
		{
			return new StiToolTipExpression(this, "ToolTip");
		}
		set
		{
			value?.Set(this, "ToolTip", value.Value);
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets hyperlink of the component.")]
	public object HyperlinkValue
	{
		get
		{
			return bits?.hyperlinkValue;
		}
		set
		{
			if (value != null || bits != null)
			{
				if (bits != null)
				{
					bits.hyperlinkValue = value;
				}
				else
				{
					bits = new bitsComponent(BookmarkValue, ToolTipValue, value, TagValue, Enabled, HighlightState, IgnoreNamingRule, DockStyle, Printable);
				}
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[StiOrder(100)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets an expression to fill the component hyperlink.")]
	public virtual StiHyperlinkExpression Hyperlink
	{
		get
		{
			return new StiHyperlinkExpression(this, "Hyperlink");
		}
		set
		{
			value?.Set(this, "Hyperlink", value.Value);
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets tag of a component.")]
	public object TagValue
	{
		get
		{
			return bits?.tagValue;
		}
		set
		{
			if (value != null || bits != null)
			{
				if (bits != null)
				{
					bits.tagValue = value;
				}
				else
				{
					bits = new bitsComponent(BookmarkValue, ToolTipValue, HyperlinkValue, value, Enabled, HighlightState, IgnoreNamingRule, DockStyle, Printable);
				}
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[StiOrder(130)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the expression to fill a component tag.")]
	public virtual StiTagExpression Tag
	{
		get
		{
			return new StiTagExpression(this, "Tag");
		}
		set
		{
			value?.Set(this, "Tag", value.Value);
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(110)]
	[StiSerializable]
	[ParenthesizePropertyName(true)]
	[DefaultValue("")]
	[Description("Indicates the text that will be shown instead of the component name. If the text is not indicated, then the component name is shown.")]
	[Editor("Stimulsoft.Report.Components.Design.StiSimpleTextEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual string Alias
	{
		get
		{
			return alias;
		}
		set
		{
			if (alias != value)
			{
				alias = string.Intern(value);
			}
		}
	}

	protected EventHandlerList Events => events ?? (events = new EventHandlerList());

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(130)]
	[StiSerializable]
	[DefaultValue(StiRestrictions.All)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Editor("Stimulsoft.Report.Components.Design.StiRestrictionsModeEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets value which indicates the restrictions of a component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual StiRestrictions Restrictions
	{
		get
		{
			return (StiRestrictions)((StiService)this).Properties.Get(PropertyRestrictions, (object)StiRestrictions.All);
		}
		set
		{
			((StiService)this).Properties.Set(PropertyRestrictions, (object)value, (object)StiRestrictions.All);
		}
	}

	[Browsable(false)]
	public bool IgnoreNamingRule
	{
		get
		{
			if (bits != null)
			{
				return bits.ignoreNamingRule;
			}
			return false;
		}
		set
		{
			if (value || bits != null)
			{
				if (bits != null)
				{
					bits.ignoreNamingRule = value;
				}
				else
				{
					bits = new bitsComponent(BookmarkValue, ToolTipValue, HyperlinkValue, TagValue, Enabled, HighlightState, value, DockStyle, Printable);
				}
			}
		}
	}

	public override string Name
	{
		get
		{
			return base.Name;
		}
		set
		{
			if (IgnoreNamingRule || Report == null || !Report.IsDesigning)
			{
				base.Name = value;
				return;
			}
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentException();
			}
			if (!StiOptions.Designer.AutoCorrectComponentName)
			{
				base.Name = value;
			}
			else
			{
				base.Name = StiNameValidator.CorrectName(value, Report);
			}
		}
	}

	[Browsable(false)]
	public bool IsEventEmpty
	{
		get
		{
			StiEventsCollection stiEventsCollection = GetEvents();
			foreach (StiEvent item in stiEventsCollection)
			{
				if (item.Script.Length != 0)
				{
					return false;
				}
			}
			return true;
		}
	}

	[Browsable(false)]
	public bool ShowQuickButtons
	{
		get
		{
			return ((StiService)this).Properties.GetBool(PropertyShowQuickButtons, false);
		}
		set
		{
			((StiService)this).Properties.SetBool(PropertyShowQuickButtons, value, false);
		}
	}

	[Browsable(false)]
	[StiServiceParam]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets value which shows it is necessary to place component in toolbox or no.")]
	public virtual bool PlaceOnToolbox
	{
		get
		{
			return ((StiService)this).Properties.GetBool(PropertyPlaceOnToolbox, false);
		}
		set
		{
			((StiService)this).Properties.SetBool(PropertyPlaceOnToolbox, value, false);
		}
	}

	[Browsable(false)]
	public abstract int ToolboxPosition { get; }

	[Browsable(false)]
	public virtual StiToolboxCategory ToolboxCategory => StiToolboxCategory.Components;

	[Browsable(false)]
	public virtual bool ForceRedrawAll => false;

	[Browsable(false)]
	public bool IsPrinting
	{
		get
		{
			if (Report != null)
			{
				if (!Report.IsPrinting)
				{
					if (Report.CompiledReport != null)
					{
						return Report.CompiledReport.IsPrinting;
					}
					return false;
				}
				return true;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool IsExporting
	{
		get
		{
			if (Report != null)
			{
				return Report.IsExporting;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool IsDesigning
	{
		get
		{
			if (Report != null)
			{
				return Report.IsDesigning;
			}
			return false;
		}
	}

	[Browsable(false)]
	public virtual bool IsCross => false;

	[Browsable(false)]
	public virtual bool AllowDelete
	{
		get
		{
			if (!Inherited)
			{
				return StiRestrictionsHelper.IsAllowDelete(this);
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool DelimiterComponent
	{
		get
		{
			return ((StiService)this).Properties.GetBool(PropertyDelimiterComponent, false);
		}
		set
		{
			((StiService)this).Properties.SetBool(PropertyDelimiterComponent, value, false);
		}
	}

	[Browsable(false)]
	public virtual int Priority => 0;

	[Browsable(false)]
	public virtual StiComponentType ComponentType => StiComponentType.Simple;

	[Browsable(false)]
	public virtual bool Dockable
	{
		get
		{
			return ((StiService)this).Properties.GetBool(PropertyDockable, true);
		}
		set
		{
			((StiService)this).Properties.SetBool(PropertyDockable, value, true);
		}
	}

	[Browsable(false)]
	public StiHighlightState HighlightState
	{
		get
		{
			if (bits != null)
			{
				return bits.highlightState;
			}
			return StiHighlightState.Hide;
		}
		set
		{
			if (value != 0 || bits != null)
			{
				if (bits != null)
				{
					bits.highlightState = value;
				}
				else
				{
					bits = new bitsComponent(BookmarkValue, ToolTipValue, HyperlinkValue, TagValue, Enabled, value, IgnoreNamingRule, DockStyle, Printable);
				}
			}
		}
	}

	[StiSerializable]
	[DefaultValue("")]
	[Browsable(false)]
	public string ComponentPlacement { get; set; } = string.Empty;


	[Browsable(false)]
	public Dictionary<string, object> DrillDownParameters { get; set; }

	[StiSerializable]
	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[Description("Gets or sets a style of a component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[Editor("Stimulsoft.Report.Design.StiStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[TypeConverter(typeof(StiExpressionStyleConverter))]
	[StiExpressionAllowed]
	public virtual string ComponentStyle
	{
		get
		{
			return (string)((StiService)this).Properties.Get(PropertyComponentStyle, (object)string.Empty);
		}
		set
		{
			((StiService)this).Properties.Set(PropertyComponentStyle, (object)value, (object)string.Empty);
			if (Report != null && value != null)
			{
				Report.Styles[value]?.SetStyleToComponent(this);
			}
			if (this is StiContainer)
			{
				((StiContainer)this).SetParentStylesToChilds();
			}
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates that moving is locked.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool Locked
	{
		get
		{
			return ((StiService)this).Properties.GetBool(PropertyLocked, false);
		}
		set
		{
			((StiService)this).Properties.SetBool(PropertyLocked, value, false);
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(150)]
	[StiSerializable]
	[DefaultValue(false)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets value, indicates that the object snap to the container is turned on.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool Linked
	{
		get
		{
			return ((StiService)this).Properties.GetBool(PropertyLinked, false);
		}
		set
		{
			((StiService)this).Properties.SetBool(PropertyLinked, value, false);
		}
	}

	[StiSerializable]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(170)]
	[TypeConverter(typeof(StiExpressionBoolConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiExpressionBoolEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Indicates that this component will be available or not.")]
	[StiShowInContextMenu]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiExpressionAllowed]
	public virtual bool Enabled
	{
		get
		{
			if (bits != null)
			{
				return bits.enabled;
			}
			return true;
		}
		set
		{
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Invalid comparison between Unknown and I4
			if (!value || bits != null)
			{
				if (bits != null)
				{
					bits.enabled = value;
				}
				else
				{
					bits = new bitsComponent(BookmarkValue, ToolTipValue, HyperlinkValue, TagValue, value, HighlightState, IgnoreNamingRule, DockStyle, Printable);
				}
				if (!IsDesigning && !value && DockStyle != StiDockStyle.None && Parent != null && (!(this is StiBand) || Report == null || (int)Report.EngineVersion != 2 || StiOptions.Engine.CheckDockToContainerIfComponentDisabled))
				{
					Parent.DockToContainer();
				}
			}
		}
	}

	[StiSerializable]
	[DefaultValue(false)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(310)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value which indicates that this component must use styles from parent component.")]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool UseParentStyles
	{
		get
		{
			return ((StiService)this).Properties.GetBool(PropertyUseParentStyles, false);
		}
		set
		{
			if (UseParentStyles != value)
			{
				((StiService)this).Properties.SetBool(PropertyUseParentStyles, value, false);
				if (value && Parent != null)
				{
					Parent.SetParentStylesToChilds();
				}
			}
		}
	}

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue(null)]
	[Description("Gets or sets the page on which an object is located.")]
	public virtual StiPage Page { get; set; }

	[Browsable(false)]
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	[DefaultValue(null)]
	[Description("Gets or sets the container in which an object is located.")]
	public virtual StiContainer Parent { get; set; }

	public event StiValueEventHandler GetToolTip
	{
		add
		{
			Events.AddHandler(EventGetToolTip, value);
		}
		remove
		{
			Events.RemoveHandler(EventGetToolTip, value);
		}
	}

	public event StiValueEventHandler GetHyperlink
	{
		add
		{
			Events.AddHandler(EventGetHyperlink, value);
		}
		remove
		{
			Events.RemoveHandler(EventGetHyperlink, value);
		}
	}

	public event StiValueEventHandler GetTag
	{
		add
		{
			Events.AddHandler(EventGetTag, value);
		}
		remove
		{
			Events.RemoveHandler(EventGetTag, value);
		}
	}

	public event StiValueEventHandler GetPointer
	{
		add
		{
			Events.AddHandler(EventGetPointer, value);
		}
		remove
		{
			Events.RemoveHandler(EventGetPointer, value);
		}
	}

	public event StiValueEventHandler GetBookmark
	{
		add
		{
			Events.AddHandler(EventGetBookmark, value);
		}
		remove
		{
			Events.RemoveHandler(EventGetBookmark, value);
		}
	}

	public event EventHandler BeforePrint
	{
		add
		{
			Events.AddHandler(EventBeforePrint, value);
		}
		remove
		{
			Events.RemoveHandler(EventBeforePrint, value);
		}
	}

	public event EventHandler AfterPrint
	{
		add
		{
			Events.AddHandler(EventAfterPrint, value);
		}
		remove
		{
			Events.RemoveHandler(EventAfterPrint, value);
		}
	}

	public event StiGetDrillDownReportEventHandler GetDrillDownReport
	{
		add
		{
			Events.AddHandler(EventGetDrillDownReport, value);
		}
		remove
		{
			Events.RemoveHandler(EventGetDrillDownReport, value);
		}
	}

	public event EventHandler Click
	{
		add
		{
			Events.AddHandler(EventClick, value);
		}
		remove
		{
			Events.RemoveHandler(EventClick, value);
		}
	}

	public event EventHandler DoubleClick
	{
		add
		{
			Events.AddHandler(EventDoubleClick, value);
		}
		remove
		{
			Events.RemoveHandler(EventDoubleClick, value);
		}
	}

	public event EventHandler MouseEnter
	{
		add
		{
			Events.AddHandler(EventMouseEnter, value);
		}
		remove
		{
			Events.RemoveHandler(EventMouseEnter, value);
		}
	}

	public event EventHandler MouseLeave
	{
		add
		{
			Events.AddHandler(EventMouseLeave, value);
		}
		remove
		{
			Events.RemoveHandler(EventMouseLeave, value);
		}
	}

	public event StiPaintEventHandler Painting
	{
		add
		{
			Events.AddHandler(EventPainting, value);
		}
		remove
		{
			Events.RemoveHandler(EventPainting, value);
		}
	}

	public event StiPaintEventHandler Painted
	{
		add
		{
			Events.AddHandler(EventPainted, value);
		}
		remove
		{
			Events.RemoveHandler(EventPainted, value);
		}
	}

	public virtual JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		JObject val = new JObject();
		JsonExtensions.AddPropertyIdent(val, "Ident", ((object)this).GetType().Name);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Name", Name);
		JsonExtensions.AddPropertyEnum(val, "ShiftMode", (Enum)ShiftMode, (Enum)StiShiftMode.IncreasingSize);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Guid", Guid);
		JsonExtensions.AddPropertyEnum(val, "PrintOn", (Enum)PrintOn, (Enum)StiPrintOnType.AllPages);
		JsonExtensions.AddPropertyBool(val, "CanShrink", CanShrink, false, false);
		JsonExtensions.AddPropertyBool(val, "CanGrow", CanGrow, false, false);
		JsonExtensions.AddPropertyBool(val, "GrowToHeight", GrowToHeight, false, false);
		JsonExtensions.AddPropertyEnum(val, "Anchor", (Enum)Anchor, (Enum)(StiAnchorMode.Top | StiAnchorMode.Left));
		JsonExtensions.AddPropertyBool(val, "Inherited", Inherited, false, false);
		JsonExtensions.AddPropertyBool(val, "Printable", Printable, true, false);
		JsonExtensions.AddPropertyEnum(val, "DockStyle", (Enum)DockStyle, (Enum)StiDockStyle.None);
		JsonExtensions.AddPropertySizeD(val, "MinSize", MinSize, "0,0");
		JsonExtensions.AddPropertySizeD(val, "MaxSize", MaxSize, "0,0");
		JsonExtensions.AddPropertyRectangleD(val, "ClientRectangle", ClientRectangle);
		JsonExtensions.AddPropertyJObject(val, "GetToolTipEvent", GetToolTipEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetHyperlinkEvent", GetHyperlinkEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetTagEvent", GetTagEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetBookmarkEvent", GetBookmarkEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "BeforePrintEvent", BeforePrintEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "AfterPrintEvent", AfterPrintEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "GetDrillDownReportEvent", GetDrillDownReportEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "ClickEvent", ClickEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "DoubleClickEvent", DoubleClickEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "MouseEnterEvent", MouseEnterEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "MouseLeaveEvent", MouseLeaveEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "Alias", Alias);
		JsonExtensions.AddPropertyEnum(val, "Restrictions", (Enum)Restrictions, (Enum)StiRestrictions.All);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ComponentPlacement", ComponentPlacement);
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "ComponentStyle", ComponentStyle);
		JsonExtensions.AddPropertyBool(val, "Locked", Locked, false, false);
		JsonExtensions.AddPropertyBool(val, "Linked", Linked, false, false);
		JsonExtensions.AddPropertyBool(val, "Enabled", Enabled, true, false);
		JsonExtensions.AddPropertyBool(val, "UseParentStyles", UseParentStyles, false, false);
		if ((int)mode == 0)
		{
			JsonExtensions.AddPropertyJObject(val, "Conditions", Conditions.SaveToJsonObject(mode));
			if (Expressions != null)
			{
				JsonExtensions.AddPropertyJObject(val, "Expressions", Expressions.SaveToJsonObject(mode));
			}
			if (Interaction != null)
			{
				JsonExtensions.AddPropertyJObject(val, "Interaction", Interaction.SaveToJsonObject(mode));
			}
			JsonExtensions.AddPropertyJObject(val, "Bookmark", Bookmark.SaveToJsonObject(mode));
			JsonExtensions.AddPropertyJObject(val, "ToolTip", ToolTip.SaveToJsonObject(mode));
			JsonExtensions.AddPropertyJObject(val, "Hyperlink", Hyperlink.SaveToJsonObject(mode));
			JsonExtensions.AddPropertyJObject(val, "Tag", Tag.SaveToJsonObject(mode));
			JsonExtensions.AddPropertyJObject(val, "Pointer", Pointer.SaveToJsonObject(mode));
		}
		return val;
	}

	public virtual void LoadFromJsonObject(JObject jObject)
	{
		//IL_08db: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e5: Expected O, but got Unknown
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Expected O, but got Unknown
		//IL_0756: Unknown result type (might be due to invalid IL or missing references)
		//IL_0927: Unknown result type (might be due to invalid IL or missing references)
		//IL_0931: Expected O, but got Unknown
		//IL_0785: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Expected O, but got Unknown
		//IL_09e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ef: Expected O, but got Unknown
		//IL_09bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c9: Expected O, but got Unknown
		//IL_0869: Unknown result type (might be due to invalid IL or missing references)
		//IL_0873: Expected O, but got Unknown
		//IL_088f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0899: Expected O, but got Unknown
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		//IL_097d: Expected O, but got Unknown
		//IL_07ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Expected O, but got Unknown
		//IL_0901: Unknown result type (might be due to invalid IL or missing references)
		//IL_090b: Expected O, but got Unknown
		//IL_07f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0801: Expected O, but got Unknown
		//IL_0843: Unknown result type (might be due to invalid IL or missing references)
		//IL_084d: Expected O, but got Unknown
		//IL_070d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0717: Expected O, but got Unknown
		//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bf: Expected O, but got Unknown
		//IL_0745: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07db: Expected O, but got Unknown
		//IL_094d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0957: Expected O, but got Unknown
		//IL_0999: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a3: Expected O, but got Unknown
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		//IL_06e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Expected O, but got Unknown
		//IL_0767: Unknown result type (might be due to invalid IL or missing references)
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "Name":
				base.Name = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ShiftMode":
				shiftMode = StiJsonReportObjectHelper.DeserializeEnum<StiShiftMode>(item);
				break;
			case "Guid":
				Guid = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "PrintOn":
				PrintOn = StiJsonReportObjectHelper.DeserializeEnum<StiPrintOnType>(item);
				break;
			case "CanShrink":
				CanShrink = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "CanGrow":
				canGrow = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "GrowToHeight":
				growToHeight = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Anchor":
				anchor = StiJsonReportObjectHelper.DeserializeEnum<StiAnchorMode>(item);
				break;
			case "Conditions":
				Conditions.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Expressions":
				Expressions.LoadFromJsonObject((JObject)item.Value);
				break;
			case "Inherited":
				Inherited = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Interaction":
				Interaction = StiInteraction.LoadInteractionFromJsonObject((JObject)item.Value);
				break;
			case "Printable":
				Printable = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "DockStyle":
				DockStyle = StiJsonReportObjectHelper.DeserializeEnum<StiDockStyle>(item);
				break;
			case "MinSize":
				MinSize = StiJsonReportObjectHelper.DeserializeSizeD(item);
				break;
			case "MaxSize":
				MaxSize = StiJsonReportObjectHelper.DeserializeSizeD(item);
				break;
			case "ClientRectangle":
				ClientRectangle = StiJsonReportObjectHelper.DeserializeRectangleD(item);
				break;
			case "GetToolTipEvent":
			{
				StiGetToolTipEvent stiGetToolTipEvent = new StiGetToolTipEvent();
				stiGetToolTipEvent.LoadFromJsonObject((JObject)item.Value);
				GetToolTipEvent = stiGetToolTipEvent;
				break;
			}
			case "GetHyperlinkEvent":
			{
				StiGetHyperlinkEvent stiGetHyperlinkEvent = new StiGetHyperlinkEvent();
				stiGetHyperlinkEvent.LoadFromJsonObject((JObject)item.Value);
				GetHyperlinkEvent = stiGetHyperlinkEvent;
				break;
			}
			case "GetTagEvent":
			{
				StiGetTagEvent stiGetTagEvent = new StiGetTagEvent();
				stiGetTagEvent.LoadFromJsonObject((JObject)item.Value);
				GetTagEvent = stiGetTagEvent;
				break;
			}
			case "GetPointerEvent":
			{
				StiGetPointerEvent stiGetPointerEvent = new StiGetPointerEvent();
				stiGetPointerEvent.LoadFromJsonObject((JObject)item.Value);
				GetPointerEvent = stiGetPointerEvent;
				break;
			}
			case "GetBookmarkEvent":
			{
				StiGetBookmarkEvent stiGetBookmarkEvent = new StiGetBookmarkEvent();
				stiGetBookmarkEvent.LoadFromJsonObject((JObject)item.Value);
				GetBookmarkEvent = stiGetBookmarkEvent;
				break;
			}
			case "BeforePrintEvent":
			{
				StiBeforePrintEvent stiBeforePrintEvent = new StiBeforePrintEvent();
				stiBeforePrintEvent.LoadFromJsonObject((JObject)item.Value);
				BeforePrintEvent = stiBeforePrintEvent;
				break;
			}
			case "AfterPrintEvent":
			{
				StiAfterPrintEvent stiAfterPrintEvent = new StiAfterPrintEvent();
				stiAfterPrintEvent.LoadFromJsonObject((JObject)item.Value);
				AfterPrintEvent = stiAfterPrintEvent;
				break;
			}
			case "GetDrillDownReportEvent":
			{
				StiGetDrillDownReportEvent stiGetDrillDownReportEvent = new StiGetDrillDownReportEvent();
				stiGetDrillDownReportEvent.LoadFromJsonObject((JObject)item.Value);
				GetDrillDownReportEvent = stiGetDrillDownReportEvent;
				break;
			}
			case "ClickEvent":
			{
				StiClickEvent stiClickEvent = new StiClickEvent();
				stiClickEvent.LoadFromJsonObject((JObject)item.Value);
				ClickEvent = stiClickEvent;
				break;
			}
			case "DoubleClickEvent":
			{
				StiDoubleClickEvent stiDoubleClickEvent = new StiDoubleClickEvent();
				stiDoubleClickEvent.LoadFromJsonObject((JObject)item.Value);
				DoubleClickEvent = stiDoubleClickEvent;
				break;
			}
			case "MouseEnterEvent":
			{
				StiMouseEnterEvent stiMouseEnterEvent = new StiMouseEnterEvent();
				stiMouseEnterEvent.LoadFromJsonObject((JObject)item.Value);
				MouseEnterEvent = stiMouseEnterEvent;
				break;
			}
			case "MouseLeaveEvent":
			{
				StiMouseLeaveEvent stiMouseLeaveEvent = new StiMouseLeaveEvent();
				stiMouseLeaveEvent.LoadFromJsonObject((JObject)item.Value);
				MouseLeaveEvent = stiMouseLeaveEvent;
				break;
			}
			case "Bookmark":
			{
				StiBookmarkExpression stiBookmarkExpression = new StiBookmarkExpression();
				stiBookmarkExpression.LoadFromJsonObject((JObject)item.Value);
				Bookmark = stiBookmarkExpression;
				break;
			}
			case "ToolTip":
			{
				StiToolTipExpression stiToolTipExpression = new StiToolTipExpression();
				stiToolTipExpression.LoadFromJsonObject((JObject)item.Value);
				ToolTip = stiToolTipExpression;
				break;
			}
			case "Hyperlink":
			{
				StiHyperlinkExpression stiHyperlinkExpression = new StiHyperlinkExpression();
				stiHyperlinkExpression.LoadFromJsonObject((JObject)item.Value);
				Hyperlink = stiHyperlinkExpression;
				break;
			}
			case "Tag":
			{
				StiTagExpression stiTagExpression = new StiTagExpression();
				stiTagExpression.LoadFromJsonObject((JObject)item.Value);
				Tag = stiTagExpression;
				break;
			}
			case "Pointer":
			{
				StiPointerExpression stiPointerExpression = new StiPointerExpression();
				stiPointerExpression.LoadFromJsonObject((JObject)item.Value);
				Pointer = stiPointerExpression;
				break;
			}
			case "Alias":
				alias = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Restrictions":
				Restrictions = StiJsonReportObjectHelper.DeserializeEnum<StiRestrictions>(item);
				break;
			case "ComponentPlacement":
				ComponentPlacement = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "ComponentStyle":
				ComponentStyle = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Locked":
				Locked = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Linked":
				Linked = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "Enabled":
				Enabled = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "UseParentStyles":
				UseParentStyles = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			}
		}
	}

	public virtual StiPropertyCollection GetProperties(IStiPropertyGrid propertyGrid, StiLevel level)
	{
		return null;
	}

	public virtual StiEventCollection GetEvents(IStiPropertyGrid propertyGrid)
	{
		return null;
	}

	public void Select()
	{
		if (!IsSelected)
		{
			SelectionTick = Environment.TickCount;
		}
		if (StiRestrictionsHelper.IsAllowSelect(this))
		{
			IsSelected = true;
		}
	}

	public void Invert()
	{
		if (!IsSelected)
		{
			SelectionTick = Environment.TickCount;
		}
		if (StiRestrictionsHelper.IsAllowSelect(this))
		{
			IsSelected = !IsSelected;
		}
	}

	public void Reset()
	{
		IsSelected = false;
	}

	IStiApp IStiAppComponent.GetApp()
	{
		return (IStiApp)(object)Report;
	}

	string IStiAppComponent.GetName()
	{
		return Name;
	}

	string IStiAppCell.GetKey()
	{
		if (string.IsNullOrWhiteSpace(Guid))
		{
			NewGuid();
		}
		return Guid;
	}

	void IStiAppCell.SetKey(string key)
	{
		Guid = key;
	}

	IStiReport IStiReportComponent.GetReport()
	{
		return (IStiReport)(object)Report;
	}

	public virtual void SaveState(string stateName)
	{
		States.PushBool(stateName, this, "isRendered", IsRendered);
		States.PushInt(stateName, this, "renderedCount", RenderedCount);
	}

	public virtual void RestoreState(string stateName)
	{
		if (States.IsExist(stateName, this))
		{
			RenderedCount = States.PopInt(stateName, this, "renderedCount");
			IsRendered = States.PopBool(stateName, this, "isRendered");
		}
	}

	public virtual void ClearAllStates()
	{
		states = null;
	}

	public void NewGuid()
	{
		Guid = System.Guid.NewGuid().ToString("N");
	}

	public override object Clone()
	{
		return Clone(cloneProperties: true);
	}

	public virtual object Clone(bool cloneProperties)
	{
		StiComponent stiComponent = (StiComponent)((StiService)this).Clone();
		StiAppExpressionCollection expressions = Expressions;
		object obj = ((expressions != null) ? expressions.Clone() : null);
		stiComponent.Expressions = (StiAppExpressionCollection)((obj is StiAppExpressionCollection) ? obj : null);
		if (conditions != null)
		{
			stiComponent.conditions = (StiConditionsCollection)conditions.Clone();
		}
		else
		{
			stiComponent.conditions = null;
		}
		if (interaction != null)
		{
			stiComponent.interaction = (StiInteraction)interaction.Clone();
		}
		else
		{
			stiComponent.interaction = null;
		}
		if (stiComponent.interaction != null)
		{
			stiComponent.interaction.ParentComponent = stiComponent;
		}
		if (cloneProperties)
		{
			_003F val = stiComponent;
			object obj2 = ((StiService)this).Properties.Clone();
			((StiService)val).Properties = (StiRepositoryItems)((obj2 is StiRepositoryItems) ? obj2 : null);
		}
		if (bits != null)
		{
			stiComponent.bits = bits.Clone() as bitsComponent;
		}
		return stiComponent;
	}

	public virtual void Convert(StiUnit oldUnit, StiUnit newUnit, bool isReportSnapshot = false)
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		disableCheckWidthHeight = true;
		lockOnResize = true;
		double right = Right;
		double bottom = Bottom;
		Left = newUnit.ConvertFromHInches(oldUnit.ConvertToHInches(Left));
		Top = newUnit.ConvertFromHInches(oldUnit.ConvertToHInches(Top));
		Width = Math.Round(newUnit.ConvertFromHInches(oldUnit.ConvertToHInches(right)), 2) - Left;
		Height = Math.Round(newUnit.ConvertFromHInches(oldUnit.ConvertToHInches(bottom)), 2) - Top;
		disableCheckWidthHeight = false;
		lockOnResize = false;
		if (!isReportSnapshot)
		{
			MinSize = newUnit.ConvertFromHInches(oldUnit.ConvertToHInches(MinSize));
			MaxSize = newUnit.ConvertFromHInches(oldUnit.ConvertToHInches(MaxSize));
		}
	}

	private bool ShouldSerializeConditions()
	{
		if (conditions != null)
		{
			return conditions.Count > 0;
		}
		return true;
	}

	public virtual SizeD GetActualSize()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return new SizeD(Width, Height);
	}

	public object GetReport()
	{
		return Report;
	}

	private bool ShouldSerializeInteraction()
	{
		if (interaction != null)
		{
			return !interaction.IsDefault;
		}
		return true;
	}

	public virtual List<StiFont> GetFonts()
	{
		List<StiFont> list = new List<StiFont>();
		foreach (object condition in Conditions)
		{
			if (condition is IStiGetFonts)
			{
				list.AddRange(((IStiGetFonts)((condition is IStiGetFonts) ? condition : null)).GetFonts());
			}
		}
		return list.Distinct().ToList();
	}

	private bool ShouldSerializeExpressions()
	{
		if (Expressions != null)
		{
			return ((CollectionBase)(object)Expressions).Count != 0;
		}
		return true;
	}

	public override void PackService()
	{
		((StiService)this).PackService();
		if (conditions != null && conditions.Count == 0)
		{
			conditions = null;
		}
	}

	public virtual StiComponent CreateNew()
	{
		throw new NotImplementedException();
	}

	public virtual void DoPointer(bool createNewGuid = true)
	{
		DoGetPointer(createNewGuid);
	}

	internal void DoGetPointer(bool createNewGuid)
	{
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Invalid comparison between Unknown and I4
		bool flag = Report == null || Report.CalculationMode == StiCalculationMode.Compilation;
		if ((flag && (object)Events[EventGetPointer] != null) || (!flag && Pointer.Value.Length > 0))
		{
			StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
			if (flag)
			{
				InvokeGetPointer(this, stiValueEventArgs);
			}
			else
			{
				object obj = StiParser.ParseTextValue(Pointer.Value, this);
				stiValueEventArgs.Value = Report.ToString(obj);
			}
			if (stiValueEventArgs.Value is string && ((string)stiValueEventArgs.Value).Length > 0)
			{
				if (Report != null && (int)Report.EngineVersion == 2)
				{
					if (createNewGuid)
					{
						NewGuid();
					}
					string text = stiValueEventArgs.Value as string;
					CurrentPointer = StiBookmarksV2Helper.CreateBookmark(text, Guid);
					if (ParentPointer != null)
					{
						ParentPointer.Bookmarks.Add(CurrentPointer);
					}
				}
			}
			else
			{
				CurrentPointer = ParentPointer;
			}
		}
		else
		{
			CurrentPointer = ParentPointer;
		}
	}

	public virtual bool DoBookmark()
	{
		return DoGetBookmark();
	}

	internal bool DoGetBookmark()
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Invalid comparison between Unknown and I4
		bool result = false;
		bool flag = Report == null || Report.CalculationMode == StiCalculationMode.Compilation;
		if ((flag && (object)Events[EventGetBookmark] != null) || (!flag && Bookmark.Value.Length > 0))
		{
			StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
			if (flag)
			{
				InvokeGetBookmark(this, stiValueEventArgs);
			}
			else
			{
				object obj = StiParser.ParseTextValue(Bookmark.Value, this);
				stiValueEventArgs.Value = Report.ToString(obj);
			}
			if (stiValueEventArgs.Value is string && ((string)stiValueEventArgs.Value).Length > 0)
			{
				if (Report != null && (int)Report.EngineVersion == 2)
				{
					NewGuid();
					result = true;
					string text = stiValueEventArgs.Value as string;
					if (text.Length > 2 && text[0] == '%')
					{
						char c = text[1];
						text = text.Substring(2);
						string[] array = text.Split(new char[1] { c });
						StiBookmark bookmark = Report.Bookmark;
						for (int i = 0; i < array.Length; i++)
						{
							string text2 = array[i];
							bookmark = StiBookmarksV2Helper.GetBookmark(bookmark, text2);
							if (i == array.Length - 1)
							{
								bookmark.ComponentGuid = Guid;
							}
						}
						CurrentBookmark = bookmark;
					}
					else
					{
						CurrentBookmark = StiBookmarksV2Helper.CreateBookmark(text, Guid);
						if (ParentBookmark != null)
						{
							ParentBookmark.Bookmarks.Add(CurrentBookmark);
						}
					}
				}
				else
				{
					CurrentBookmark = StiBookmarksV1Helper.CreateBookmark(stiValueEventArgs.Value as string, this);
					ParentBookmark.Bookmarks.Add(CurrentBookmark);
					string text3 = stiValueEventArgs.Value as string;
					if (text3.Length > 2 && text3[0] == '%')
					{
						string text4 = text3.Substring(0, 1);
						char c2 = text3[1];
						text3 = text3.Substring(2);
						string[] array2 = text3.Split(new char[1] { c2 });
						StiBookmark stiBookmark = Report.ManualBookmark;
						string[] array3 = array2;
						foreach (string text5 in array3)
						{
							text4 = text4 + c2 + text5;
							stiBookmark = StiBookmarksV1Helper.GetBookmark(stiBookmark, text5);
							stiBookmark.BookmarkText = text4;
						}
						stiBookmark.BookmarkText = stiValueEventArgs.Value as string;
					}
				}
			}
			else
			{
				CurrentBookmark = ParentBookmark;
			}
		}
		else
		{
			CurrentBookmark = ParentBookmark;
		}
		return result;
	}

	protected bool AllowPrintOn()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		if (PrintOn == StiPrintOnType.AllPages)
		{
			return true;
		}
		if (Report != null && (int)Report.EngineVersion == 2)
		{
			int num = Report.PageNumber;
			int totalPageCount = Report.TotalPageCount;
			if (!StiOptions.Engine.UseAdvancedPrintOnEngineV2)
			{
				if ((PrintOn & StiPrintOnType.ExceptFirstPage) > StiPrintOnType.AllPages && num == 1)
				{
					return false;
				}
				if (PrintOn == StiPrintOnType.OnlyFirstPage && num > 1)
				{
					return false;
				}
				return true;
			}
			if (Page.SegmentPerWidth > 1 || Page.SegmentPerHeight > 1)
			{
				num += Page.SegmentPerWidth * Page.SegmentPerHeight - 1;
			}
			if (Report.ReportPass == StiReportPass.First)
			{
				if ((PrintOn & StiPrintOnType.OnlyLastPage) > StiPrintOnType.AllPages)
				{
					return false;
				}
				if ((PrintOn & StiPrintOnType.ExceptLastPage) > StiPrintOnType.AllPages)
				{
					return true;
				}
			}
			if ((PrintOn & StiPrintOnType.ExceptFirstPage) > StiPrintOnType.AllPages && num == 1)
			{
				return false;
			}
			if (PrintOn == StiPrintOnType.OnlyFirstAndLastPage)
			{
				if (num > 1 && num < totalPageCount)
				{
					return false;
				}
				return true;
			}
			if ((PrintOn & StiPrintOnType.OnlyFirstPage) > StiPrintOnType.AllPages && num > 1)
			{
				return false;
			}
			if ((PrintOn & StiPrintOnType.OnlyLastPage) > StiPrintOnType.AllPages && num < totalPageCount)
			{
				return false;
			}
			if ((PrintOn & StiPrintOnType.ExceptLastPage) > StiPrintOnType.AllPages && num == totalPageCount)
			{
				return false;
			}
			return true;
		}
		if (!StiOptions.Engine.UseAdvancedPrintOnEngineV1)
		{
			return true;
		}
		if (Report == null)
		{
			return true;
		}
		int num2 = Report.PageNumber;
		int totalPageCount2 = Report.TotalPageCount;
		if (Page.SegmentPerWidth > 1 || Page.SegmentPerHeight > 1)
		{
			num2 += Page.SegmentPerWidth * Page.SegmentPerHeight - 1;
		}
		if (Report.ReportPass == StiReportPass.First)
		{
			if ((PrintOn & StiPrintOnType.OnlyLastPage) > StiPrintOnType.AllPages)
			{
				return false;
			}
			if ((PrintOn & StiPrintOnType.ExceptLastPage) > StiPrintOnType.AllPages)
			{
				return true;
			}
		}
		if ((PrintOn & StiPrintOnType.ExceptFirstPage) > StiPrintOnType.AllPages && num2 == 1)
		{
			return false;
		}
		if (PrintOn == StiPrintOnType.OnlyFirstAndLastPage)
		{
			if (num2 > 1 && num2 < totalPageCount2)
			{
				return false;
			}
			return true;
		}
		if ((PrintOn & StiPrintOnType.OnlyFirstPage) > StiPrintOnType.AllPages && num2 > 1)
		{
			return false;
		}
		if ((PrintOn & StiPrintOnType.OnlyLastPage) > StiPrintOnType.AllPages && num2 < totalPageCount2)
		{
			return false;
		}
		if ((PrintOn & StiPrintOnType.ExceptLastPage) > StiPrintOnType.AllPages && num2 == totalPageCount2)
		{
			return false;
		}
		return true;
	}

	public virtual void Prepare()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		if (Report != null && (int)Report.EngineVersion == 1)
		{
			StiV1Builder.GetBuilder(((object)this).GetType()).Prepare(this);
		}
		else
		{
			StiV2Builder.GetBuilder(((object)this).GetType()).Prepare(this);
		}
	}

	public virtual void UnPrepare()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Invalid comparison between Unknown and I4
		if (Report != null && (int)Report.EngineVersion == 1)
		{
			StiV1Builder.GetBuilder(((object)this).GetType()).UnPrepare(this);
		}
		else
		{
			StiV2Builder.GetBuilder(((object)this).GetType()).UnPrepare(this);
		}
	}

	protected virtual bool RenderComponent(ref StiComponent renderedComponent, StiContainer outContainer)
	{
		return StiV1Builder.GetBuilder(((object)this).GetType()).InternalRender(this, ref renderedComponent, outContainer);
	}

	public virtual bool InternalRender(ref StiComponent renderedComponent, StiContainer outContainer)
	{
		return RenderComponent(ref renderedComponent, outContainer);
	}

	public virtual bool Render(ref StiComponent renderedComponent, StiContainer outContainer)
	{
		return StiV1Builder.GetBuilder(((object)this).GetType()).Render(this, ref renderedComponent, outContainer);
	}

	public virtual bool Render(StiContainer outContainer)
	{
		return StiV1Builder.GetBuilder(((object)this).GetType()).Render(this, outContainer);
	}

	public virtual void SetReportVariables()
	{
		StiV2Builder builder = StiV2Builder.GetBuilder(((object)this).GetType());
		builder.SetReportVariables(this);
	}

	public virtual StiComponent InternalRender()
	{
		StiV2Builder builder = StiV2Builder.GetBuilder(((object)this).GetType());
		return builder.InternalRender(this);
	}

	public virtual StiComponent Render()
	{
		StiV2Builder builder = StiV2Builder.GetBuilder(((object)this).GetType());
		return builder.Render(this);
	}

	public virtual byte[] GetThumbnailAsBytes(int maxSize)
	{
		Bitmap thumbnail = GetThumbnail(maxSize);
		try
		{
			return StiImageConverter.ImageToBytes((Image)(object)thumbnail, false);
		}
		finally
		{
			((IDisposable)thumbnail)?.Dispose();
		}
	}

	public virtual Bitmap GetThumbnail(int maxSize, bool isDesignTime = false)
	{
		if (Width > Height)
		{
			return GetThumbnail(maxSize, (int)((double)maxSize * Height / Width), isDesignTime);
		}
		return GetThumbnail((int)((double)maxSize * Height / Width), maxSize, isDesignTime);
	}

	public virtual Bitmap GetThumbnail(int width, int height)
	{
		return GetThumbnail(width, height, isDesignTime: false);
	}

	public virtual Bitmap GetThumbnail(int width, int height, bool isDesignTime)
	{
		StiPainter painter = StiPainter.GetPainter(((object)this).GetType(), (StiGuiMode)0);
		return painter.GetThumbnail(this, width, height, isDesignTime);
	}

	public virtual string GetQuickInfo()
	{
		switch (Report.Info.QuickInfoType)
		{
		case StiQuickInfoType.ShowAliases:
			if (!string.IsNullOrEmpty(Alias))
			{
				return Alias;
			}
			return string.Empty;
		case StiQuickInfoType.ShowEvents:
		{
			StiEventsCollection stiEventsCollection = GetEvents();
			string text = string.Empty;
			{
				foreach (StiEvent item in stiEventsCollection)
				{
					if (item.Script.Length != 0)
					{
						if (text.Length > 0)
						{
							text += ",";
						}
						text += item.ToString();
					}
				}
				return text;
			}
		}
		case StiQuickInfoType.ShowComponentsNames:
			if (Alias.Length != 0)
			{
				return Alias;
			}
			return Name;
		case StiQuickInfoType.ShowFields:
		{
			if (!(this is IStiText stiText))
			{
				return string.Empty;
			}
			int num = stiText.GetTextInternal().LastIndexOf('.');
			if (num >= 0)
			{
				return stiText.GetTextInternal().Substring(num);
			}
			return string.Empty;
		}
		case StiQuickInfoType.ShowFieldsOnly:
			return string.Empty;
		default:
			return string.Empty;
		}
	}

	public virtual void Paint(StiPaintEventArgs e)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		StiGuiMode guiMode = (StiGuiMode)(!(e.Context is Graphics));
		StiPainter painter = StiPainter.GetPainter(((object)this).GetType(), guiMode);
		painter.AnimationEngine = e.AnimationEngine;
		painter.Paint(this, e);
	}

	public virtual void PaintSelection(StiPaintEventArgs e)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		StiGuiMode guiMode = (StiGuiMode)(!(e.Context is Graphics));
		StiPainter painter = StiPainter.GetPainter(((object)this).GetType(), guiMode);
		painter.PaintSelection(this, e);
	}

	public virtual void PaintHighlight(StiPaintEventArgs e)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		StiGuiMode guiMode = (StiGuiMode)(!(e.Context is Graphics));
		StiPainter painter = StiPainter.GetPainter(((object)this).GetType(), guiMode);
		painter.PaintHighlight(this, e);
	}

	public virtual void PaintMarkers(Graphics g, RectangleD rect)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		StiComponentGdiPainter stiComponentGdiPainter = StiPainter.GetPainter(((object)this).GetType(), (StiGuiMode)0) as StiComponentGdiPainter;
		stiComponentGdiPainter.PaintMarkers(this, g, rect);
	}

	public virtual void PaintEvents(Graphics g, RectangleD rect)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		StiComponentGdiPainter stiComponentGdiPainter = StiPainter.GetPainter(((object)this).GetType(), (StiGuiMode)0) as StiComponentGdiPainter;
		stiComponentGdiPainter.PaintEvents(this, g, rect);
	}

	public static DockStyle ConvertDockStyle(StiDockStyle dockStyle)
	{
		return (DockStyle)(dockStyle switch
		{
			StiDockStyle.Fill => 5, 
			StiDockStyle.Left => 3, 
			StiDockStyle.Right => 4, 
			StiDockStyle.Top => 1, 
			StiDockStyle.Bottom => 2, 
			_ => 0, 
		});
	}

	public static StiDockStyle ConvertDock(DockStyle dock)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected I4, but got Unknown
		return (dock - 1) switch
		{
			4 => StiDockStyle.Fill, 
			2 => StiDockStyle.Left, 
			3 => StiDockStyle.Right, 
			0 => StiDockStyle.Top, 
			1 => StiDockStyle.Bottom, 
			_ => StiDockStyle.None, 
		};
	}

	public virtual RectangleD GetDockRegion(StiComponent parent)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return GetDockRegion(parent, useColumns: true);
	}

	public virtual RectangleD GetDockRegion(StiComponent parent, bool useColumns)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Invalid comparison between Unknown and I4
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		if (parent == null)
		{
			return ClientRectangle;
		}
		RectangleD val = parent.ClientRectangle;
		StiPage stiPage = parent as StiPage;
		if (Report != null && (int)Report.EngineVersion == 1)
		{
			if ((this is StiDynamicBand || this is StiReportSummaryBand) && stiPage != null && stiPage.Columns > 1)
			{
				((RectangleD)(ref val)).Width = stiPage.GetColumnWidth();
			}
		}
		else
		{
			if (ComponentType != StiComponentType.Static && stiPage != null && stiPage.Columns > 1 && useColumns)
			{
				((RectangleD)(ref val)).Width = stiPage.GetColumnWidth();
			}
			StiPanel stiPanel = parent as StiPanel;
			if (ComponentType != StiComponentType.Static && stiPanel != null && stiPanel.Columns > 1 && useColumns)
			{
				((RectangleD)(ref val)).Width = stiPanel.GetColumnWidth();
			}
		}
		if (parent.IsSelected)
		{
			val = DoOffsetRect(parent, val, parent.Page.OffsetRectangle);
		}
		((RectangleD)(ref val)).X = 0.0;
		((RectangleD)(ref val)).Y = 0.0;
		StiContainer stiContainer = (StiContainer)parent;
		foreach (StiComponent component in stiContainer.Components)
		{
			if (component.Dockable && (IsDesigning || component.Enabled))
			{
				if (component == this)
				{
					break;
				}
				RectangleD rect = component.DisplayRectangle;
				if (component.IsSelected)
				{
					rect = DoOffsetRect(component, rect, component.Page.OffsetRectangle);
				}
				switch (component.DockStyle)
				{
				case StiDockStyle.Left:
					((RectangleD)(ref val)).X = ((RectangleD)(ref val)).X + ((RectangleD)(ref rect)).Width;
					((RectangleD)(ref val)).Width = ((RectangleD)(ref val)).Width - ((RectangleD)(ref rect)).Width;
					break;
				case StiDockStyle.Right:
					((RectangleD)(ref val)).Width = ((RectangleD)(ref val)).Width - ((RectangleD)(ref rect)).Width;
					break;
				case StiDockStyle.Top:
					((RectangleD)(ref val)).Y = ((RectangleD)(ref val)).Y + ((RectangleD)(ref rect)).Height;
					((RectangleD)(ref val)).Height = ((RectangleD)(ref val)).Height - ((RectangleD)(ref rect)).Height;
					break;
				case StiDockStyle.Bottom:
					((RectangleD)(ref val)).Height = ((RectangleD)(ref val)).Height - ((RectangleD)(ref rect)).Height;
					break;
				}
			}
		}
		return val;
	}

	public virtual void DockToContainer()
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		if (DockStyle != StiDockStyle.None && Dockable && (IsDesigning || Enabled))
		{
			if (IsCross)
			{
				ClientRectangle = DockToContainer(ClientRectangle);
			}
			else
			{
				DisplayRectangle = DockToContainer(DisplayRectangle);
			}
		}
		if (!(this is StiContainer stiContainer))
		{
			return;
		}
		foreach (StiComponent component in stiContainer.Components)
		{
			if (component != stiContainer)
			{
				component.DockToContainer();
			}
		}
	}

	public virtual RectangleD DockToContainer(RectangleD rect)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		if (Parent == null || DockStyle == StiDockStyle.None || !Dockable)
		{
			return rect;
		}
		RectangleD dockRegion = GetDockRegion(Parent);
		if (Parent.Height == 100000000000.0 && DockStyle != StiDockStyle.Top)
		{
			((RectangleD)(ref dockRegion)).Y = 0.0;
			((RectangleD)(ref dockRegion)).Height = ((RectangleD)(ref rect)).Height;
		}
		switch (DockStyle)
		{
		case StiDockStyle.Left:
			((RectangleD)(ref rect)).X = ((RectangleD)(ref dockRegion)).X;
			((RectangleD)(ref rect)).Y = ((RectangleD)(ref dockRegion)).Y;
			((RectangleD)(ref rect)).Height = ((RectangleD)(ref dockRegion)).Height;
			break;
		case StiDockStyle.Right:
			((RectangleD)(ref rect)).X = ((RectangleD)(ref dockRegion)).Right - ((RectangleD)(ref rect)).Width;
			((RectangleD)(ref rect)).Y = ((RectangleD)(ref dockRegion)).Y;
			((RectangleD)(ref rect)).Height = ((RectangleD)(ref dockRegion)).Height;
			break;
		case StiDockStyle.Top:
			((RectangleD)(ref rect)).X = ((RectangleD)(ref dockRegion)).X;
			((RectangleD)(ref rect)).Y = ((RectangleD)(ref dockRegion)).Y;
			((RectangleD)(ref rect)).Width = ((RectangleD)(ref dockRegion)).Width;
			break;
		case StiDockStyle.Bottom:
			((RectangleD)(ref rect)).X = ((RectangleD)(ref dockRegion)).X;
			((RectangleD)(ref rect)).Y = ((RectangleD)(ref dockRegion)).Bottom - ((RectangleD)(ref rect)).Height;
			((RectangleD)(ref rect)).Width = ((RectangleD)(ref dockRegion)).Width;
			break;
		case StiDockStyle.Fill:
			((RectangleD)(ref rect)).X = ((RectangleD)(ref dockRegion)).X;
			((RectangleD)(ref rect)).Y = ((RectangleD)(ref dockRegion)).Y;
			((RectangleD)(ref rect)).Width = ((RectangleD)(ref dockRegion)).Width;
			((RectangleD)(ref rect)).Height = ((RectangleD)(ref dockRegion)).Height;
			break;
		}
		return rect;
	}

	private double CheckWidth(double width)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		if (disableCheckWidthHeight)
		{
			return width;
		}
		SizeD val = MinSize;
		if (((SizeD)(ref val)).Width != 0.0)
		{
			val = MinSize;
			width = Math.Max(((SizeD)(ref val)).Width, width);
		}
		val = MaxSize;
		if (((SizeD)(ref val)).Width != 0.0)
		{
			val = MaxSize;
			width = Math.Min(((SizeD)(ref val)).Width, width);
		}
		return width;
	}

	private double CheckHeight(double height)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		if (disableCheckWidthHeight)
		{
			return height;
		}
		SizeD val = MinSize;
		if (((SizeD)(ref val)).Height != 0.0)
		{
			val = MinSize;
			height = Math.Max(((SizeD)(ref val)).Height, height);
		}
		val = MaxSize;
		if (((SizeD)(ref val)).Height != 0.0)
		{
			val = MaxSize;
			height = Math.Min(((SizeD)(ref val)).Height, height);
		}
		return height;
	}

	private bool ShouldSerializeMinSize()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		SizeD minSize = MinSize;
		return !((SizeD)(ref minSize)).IsDefault;
	}

	private bool ShouldSerializeMaxSize()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		SizeD maxSize = MaxSize;
		return !((SizeD)(ref maxSize)).IsDefault;
	}

	protected internal virtual void SetDirectDisplayRectangle(RectangleD rect)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		double num = width;
		double num2 = height;
		left = ((RectangleD)(ref rect)).Left;
		top = ((RectangleD)(ref rect)).Top;
		width = ((RectangleD)(ref rect)).Width;
		height = ((RectangleD)(ref rect)).Height;
		if (width != num || height != num2)
		{
			InvokeOnResizeComponent(new SizeD(num, num2), new SizeD(width, height));
		}
	}

	public virtual void InvokeEvents()
	{
		try
		{
			bool flag = true;
			if (Report != null)
			{
				flag = Report.CalculationMode == StiCalculationMode.Compilation;
			}
			StiParserParameters stiParserParameters = new StiParserParameters();
			if (flag)
			{
				if ((object)Events[EventGetPointer] != null && PointerValue == null)
				{
					StiValueEventArgs stiValueEventArgs = new StiValueEventArgs();
					InvokeGetPointer(this, stiValueEventArgs);
					PointerValue = stiValueEventArgs.Value;
				}
			}
			else
			{
				if (PointerValue == null && Pointer.Value.Length > 0)
				{
					object obj = StiParser.ParseTextValue(Pointer.Value, this);
					PointerValue = Report.ToString(obj);
				}
				if ((object)Events[EventGetPointer] != null && PointerValue == null)
				{
					StiValueEventArgs stiValueEventArgs2 = new StiValueEventArgs();
					InvokeGetPointer(this, stiValueEventArgs2);
					PointerValue = stiValueEventArgs2.Value;
				}
			}
			if (flag)
			{
				if ((object)Events[EventGetBookmark] != null && BookmarkValue == null)
				{
					StiValueEventArgs stiValueEventArgs3 = new StiValueEventArgs();
					InvokeGetBookmark(this, stiValueEventArgs3);
					BookmarkValue = stiValueEventArgs3.Value;
				}
			}
			else
			{
				if (BookmarkValue == null && Bookmark.Value.Length > 0)
				{
					object obj2 = StiParser.ParseTextValue(Bookmark.Value, this);
					BookmarkValue = Report.ToString(obj2);
				}
				if ((object)Events[EventGetBookmark] != null && BookmarkValue == null)
				{
					StiValueEventArgs stiValueEventArgs4 = new StiValueEventArgs();
					InvokeGetBookmark(this, stiValueEventArgs4);
					BookmarkValue = stiValueEventArgs4.Value;
				}
			}
			if (flag)
			{
				if ((object)Events[EventGetTag] != null && TagValue == null)
				{
					StiValueEventArgs stiValueEventArgs5 = new StiValueEventArgs();
					InvokeGetTag(this, stiValueEventArgs5);
					TagValue = stiValueEventArgs5.Value;
				}
			}
			else
			{
				if (TagValue == null && Tag.Value.Length > 0)
				{
					stiParserParameters.GlobalizedNameExt = ".Tag";
					object obj3 = StiParser.ParseTextValue(Tag.Value, this, stiParserParameters);
					TagValue = Report.ToString(obj3);
				}
				if ((object)Events[EventGetTag] != null && TagValue == null)
				{
					StiValueEventArgs stiValueEventArgs6 = new StiValueEventArgs();
					InvokeGetTag(this, stiValueEventArgs6);
					TagValue = stiValueEventArgs6.Value;
				}
			}
			if (flag)
			{
				if ((object)Events[EventGetToolTip] != null && ToolTipValue == null)
				{
					StiValueEventArgs stiValueEventArgs7 = new StiValueEventArgs();
					InvokeGetToolTip(this, stiValueEventArgs7);
					ToolTipValue = stiValueEventArgs7.Value;
				}
			}
			else
			{
				if (ToolTipValue == null && ToolTip.Value.Length > 0)
				{
					stiParserParameters.GlobalizedNameExt = ".ToolTip";
					object obj4 = StiParser.ParseTextValue(ToolTip.Value, this, stiParserParameters);
					ToolTipValue = Report.ToString(obj4);
				}
				if ((object)Events[EventGetToolTip] != null && ToolTipValue == null)
				{
					StiValueEventArgs stiValueEventArgs8 = new StiValueEventArgs();
					InvokeGetToolTip(this, stiValueEventArgs8);
					ToolTipValue = stiValueEventArgs8.Value;
				}
			}
			if (flag)
			{
				if ((object)Events[EventGetHyperlink] != null && HyperlinkValue == null)
				{
					StiValueEventArgs stiValueEventArgs9 = new StiValueEventArgs();
					InvokeGetHyperlink(this, stiValueEventArgs9);
					HyperlinkValue = stiValueEventArgs9.Value;
				}
				return;
			}
			if (HyperlinkValue == null && Hyperlink.Value.Length > 0)
			{
				stiParserParameters.GlobalizedNameExt = ".Hyperlink";
				object obj5 = StiParser.ParseTextValue(Hyperlink.Value, this, stiParserParameters);
				HyperlinkValue = Report.ToString(obj5);
			}
			if ((object)Events[EventGetHyperlink] != null && HyperlinkValue == null)
			{
				StiValueEventArgs stiValueEventArgs10 = new StiValueEventArgs();
				InvokeGetHyperlink(this, stiValueEventArgs10);
				HyperlinkValue = stiValueEventArgs10.Value;
			}
		}
		catch (Exception ex)
		{
			StiLogService.Write(((object)this).GetType(), "DoEvents...ERROR");
			StiLogService.Write(((object)this).GetType(), ex);
			if (Report != null)
			{
				Report.WriteToReportRenderingMessages(Name + ".Events error: " + ex.Message);
			}
		}
	}

	protected virtual void OnGetToolTip(StiValueEventArgs e)
	{
	}

	public void InvokeGetToolTip(object sender, StiValueEventArgs e)
	{
		try
		{
			OnGetToolTip(e);
			if (Events[EventGetToolTip] is StiValueEventHandler stiValueEventHandler)
			{
				stiValueEventHandler(sender, e);
			}
			StiBlocklyHelper.InvokeBlockly(Report, sender, GetToolTipEvent, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in ToolTip property of '" + Name + "' can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report.WriteToReportRenderingMessages(text);
		}
	}

	protected virtual void OnGetHyperlink(StiValueEventArgs e)
	{
	}

	public void InvokeGetHyperlink(object sender, StiValueEventArgs e)
	{
		try
		{
			OnGetHyperlink(e);
			if (Events[EventGetHyperlink] is StiValueEventHandler stiValueEventHandler)
			{
				stiValueEventHandler(sender, e);
			}
			StiBlocklyHelper.InvokeBlockly(Report, sender, GetHyperlinkEvent, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in Hyperlink property of '" + Name + "' can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report.WriteToReportRenderingMessages(text);
		}
	}

	protected virtual void OnGetTag(StiValueEventArgs e)
	{
	}

	public void InvokeGetTag(object sender, StiValueEventArgs e)
	{
		try
		{
			OnGetTag(e);
			if (Events[EventGetTag] is StiValueEventHandler stiValueEventHandler)
			{
				stiValueEventHandler(sender, e);
			}
			StiBlocklyHelper.InvokeBlockly(Report, sender, GetTagEvent, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in Tag property of '" + Name + "' can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report.WriteToReportRenderingMessages(text);
		}
	}

	protected virtual void OnGetPointer(StiValueEventArgs e)
	{
	}

	public void InvokeGetPointer(object sender, StiValueEventArgs e)
	{
		try
		{
			OnGetPointer(e);
			if (Events[EventGetPointer] is StiValueEventHandler stiValueEventHandler)
			{
				stiValueEventHandler(sender, e);
			}
			StiBlocklyHelper.InvokeBlockly(Report, sender, GetPointerEvent, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in Pointer property of '" + Name + "' can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report?.WriteToReportRenderingMessages(text);
		}
	}

	protected virtual void OnGetBookmark(StiValueEventArgs e)
	{
	}

	public void InvokeGetBookmark(object sender, StiValueEventArgs e)
	{
		try
		{
			OnGetBookmark(e);
			if (Events[EventGetBookmark] is StiValueEventHandler stiValueEventHandler)
			{
				stiValueEventHandler(sender, e);
			}
			StiBlocklyHelper.InvokeBlockly(Report, sender, GetBookmarkEvent, e);
		}
		catch (Exception ex)
		{
			string text = "Expression in Bookmark property of '" + Name + "' can't be evaluated!";
			StiLogService.Write(((object)this).GetType(), text);
			StiLogService.Write(((object)this).GetType(), ex.Message);
			Report.WriteToReportRenderingMessages(text);
		}
	}

	protected virtual void OnBeforePrint(EventArgs e)
	{
	}

	public virtual void InvokeBeforePrint(object sender, EventArgs e)
	{
		try
		{
			StiAppExpressionParser.ProcessExpressions(this);
			OnBeforePrint(e);
			bool flag = true;
			if (Report != null)
			{
				flag = Report.CalculationMode == StiCalculationMode.Compilation;
			}
			if (flag)
			{
				if (Events[EventBeforePrint] is EventHandler eventHandler)
				{
					eventHandler(sender, e);
				}
			}
			else
			{
				if (Events[EventBeforePrint] is EventHandler eventHandler2)
				{
					eventHandler2(sender, e);
				}
				if (Report != null && Report.Engine != null)
				{
					object obj = Report.Engine.ParserConversionStore["*StiConditionExpression*" + Name];
					if (obj != null && !(this is StiCrossCell))
					{
						ApplyConditions(sender, obj as ArrayList, e);
					}
				}
				if (Interaction != null)
				{
					StiComponent stiComponent = sender as StiComponent;
					stiComponent.DrillDownParameters = new Dictionary<string, object>();
					try
					{
						if (!Interaction.DrillDownParameter1.IsDefault)
						{
							stiComponent.DrillDownParameters.Add(Interaction.DrillDownParameter1.Name, StiParser.ParseTextValue("{" + Interaction.DrillDownParameter1.Expression.Value + "}", this, sender));
						}
						if (!Interaction.DrillDownParameter2.IsDefault)
						{
							stiComponent.DrillDownParameters.Add(Interaction.DrillDownParameter2.Name, StiParser.ParseTextValue("{" + Interaction.DrillDownParameter2.Expression.Value + "}", this, sender));
						}
						if (!Interaction.DrillDownParameter3.IsDefault)
						{
							stiComponent.DrillDownParameters.Add(Interaction.DrillDownParameter3.Name, StiParser.ParseTextValue("{" + Interaction.DrillDownParameter3.Expression.Value + "}", this, sender));
						}
						if (!Interaction.DrillDownParameter4.IsDefault)
						{
							stiComponent.DrillDownParameters.Add(Interaction.DrillDownParameter4.Name, StiParser.ParseTextValue("{" + Interaction.DrillDownParameter4.Expression.Value + "}", this, sender));
						}
						if (!Interaction.DrillDownParameter5.IsDefault)
						{
							stiComponent.DrillDownParameters.Add(Interaction.DrillDownParameter5.Name, StiParser.ParseTextValue("{" + Interaction.DrillDownParameter5.Expression.Value + "}", this, sender));
						}
						if (!Interaction.DrillDownParameter6.IsDefault)
						{
							stiComponent.DrillDownParameters.Add(Interaction.DrillDownParameter6.Name, StiParser.ParseTextValue("{" + Interaction.DrillDownParameter6.Expression.Value + "}", this, sender));
						}
						if (!Interaction.DrillDownParameter7.IsDefault)
						{
							stiComponent.DrillDownParameters.Add(Interaction.DrillDownParameter7.Name, StiParser.ParseTextValue("{" + Interaction.DrillDownParameter7.Expression.Value + "}", this, sender));
						}
						if (!Interaction.DrillDownParameter8.IsDefault)
						{
							stiComponent.DrillDownParameters.Add(Interaction.DrillDownParameter8.Name, StiParser.ParseTextValue("{" + Interaction.DrillDownParameter8.Expression.Value + "}", this, sender));
						}
						if (!Interaction.DrillDownParameter9.IsDefault)
						{
							stiComponent.DrillDownParameters.Add(Interaction.DrillDownParameter9.Name, StiParser.ParseTextValue("{" + Interaction.DrillDownParameter9.Expression.Value + "}", this, sender));
						}
						if (!Interaction.DrillDownParameter10.IsDefault)
						{
							stiComponent.DrillDownParameters.Add(Interaction.DrillDownParameter10.Name, StiParser.ParseTextValue("{" + Interaction.DrillDownParameter10.Expression.Value + "}", this, sender));
						}
					}
					catch (Exception ex)
					{
						string text = "Expression in DrillDown property of '" + Name + "' can't be evaluated!";
						StiLogService.Write(((object)this).GetType(), text);
						StiLogService.Write(((object)this).GetType(), ex.Message);
						Report.WriteToReportRenderingMessages(text);
					}
				}
			}
			StiBlocklyHelper.InvokeBlockly(Report, sender, BeforePrintEvent);
			StiOptions.Engine.GlobalEvents.InvokeBeforePrint(sender, e);
		}
		catch (Exception ex2)
		{
			StiLogService.Write(((object)this).GetType(), Name + " InvokeBeforePrint...ERROR");
			StiLogService.Write(((object)this).GetType(), Name + " " + ex2.Message);
			if (Report != null)
			{
				Report.WriteToReportRenderingMessages(Name + ".BeforePrint event error: " + ex2.Message);
			}
		}
	}

	internal void ApplyConditions(object sender, ArrayList conditions, EventArgs e)
	{
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		if (conditions == null)
		{
			return;
		}
		StiParserParameters stiParserParameters = new StiParserParameters
		{
			IgnoreGlobalizedName = true
		};
		if (e is StiValueEventArgs)
		{
			stiParserParameters.Constants = new Hashtable();
			stiParserParameters.Constants["e.Value"] = (e as StiValueEventArgs).Value;
		}
		foreach (DictionaryEntry condition in conditions)
		{
			object obj = StiParser.ParseTextValue((string)condition.Value, this, sender, stiParserParameters);
			if (!(obj is bool) || !(bool)obj)
			{
				continue;
			}
			StiCondition stiCondition = condition.Key as StiCondition;
			if (!string.IsNullOrEmpty(stiCondition.Style))
			{
				StiConditionHelper.Apply(sender, stiCondition.Style);
			}
			else
			{
				if (sender is IStiBrush stiBrush && (stiCondition.Permissions & StiConditionPermissions.BackColor) > StiConditionPermissions.None)
				{
					stiBrush.Brush = (StiBrush)new StiSolidBrush(stiCondition.BackColor);
					if (sender is StiCrossField)
					{
						(sender as StiCrossField).ConditionBrush = (StiBrush)new StiSolidBrush(stiCondition.BackColor);
					}
				}
				if (sender is IStiBorder stiBorder && (stiCondition.Permissions & StiConditionPermissions.Borders) > StiConditionPermissions.None && (stiCondition.BorderSides & StiConditionBorderSides.NotAssigned) == 0)
				{
					stiBorder.Border = (StiBorder)stiBorder.Border.Clone();
					stiBorder.Border.Side = (StiBorderSides)stiCondition.BorderSides;
				}
				if (sender is IStiTextBrush stiTextBrush && (stiCondition.Permissions & StiConditionPermissions.TextColor) > StiConditionPermissions.None)
				{
					stiTextBrush.TextBrush = (StiBrush)new StiSolidBrush(stiCondition.TextColor);
					if (sender is StiCrossField)
					{
						(sender as StiCrossField).ConditionTextBrush = (StiBrush)new StiSolidBrush(stiCondition.TextColor);
					}
				}
				if (sender is IStiFont)
				{
					StiConditionHelper.ApplyFont(sender, stiCondition.Font, stiCondition.Permissions);
				}
				if (stiCondition.Icon != null && sender is StiText)
				{
					(sender as StiText).Indicator = new StiIconSetIndicator
					{
						CustomIcon = stiCondition.Icon,
						Alignment = stiCondition.IconAlignment,
						CustomIconSize = stiCondition.IconSize
					};
				}
				if (sender is StiCrossField)
				{
					(sender as StiCrossField).ConditionPermissions |= stiCondition.Permissions;
				}
			}
			((StiComponent)sender).Enabled = stiCondition.Enabled;
			if (stiCondition.BreakIfTrue)
			{
				break;
			}
		}
	}

	protected virtual void OnAfterPrint(EventArgs e)
	{
	}

	public virtual void InvokeAfterPrint(object sender, EventArgs e)
	{
		OnAfterPrint(e);
		if (Events[EventAfterPrint] is EventHandler eventHandler)
		{
			eventHandler(sender, e);
		}
		StiBlocklyHelper.InvokeBlockly(Report, sender, AfterPrintEvent);
		StiOptions.Engine.GlobalEvents.InvokeAfterPrint(sender, e);
	}

	protected virtual void OnGetDrillDownReport(StiGetDrillDownReportEventArgs e)
	{
	}

	public virtual void InvokeGetDrillDownReport(object sender, StiGetDrillDownReportEventArgs e)
	{
		OnGetDrillDownReport(e);
		if (Events[EventGetDrillDownReport] is StiGetDrillDownReportEventHandler stiGetDrillDownReportEventHandler)
		{
			stiGetDrillDownReportEventHandler(sender, e);
		}
		StiBlocklyHelper.InvokeBlockly(Report, sender, GetDrillDownReportEvent);
	}

	protected virtual void OnClick(EventArgs e)
	{
	}

	public virtual void InvokeClick(object sender, EventArgs e)
	{
		OnClick(e);
		if (Events[EventClick] is EventHandler eventHandler)
		{
			eventHandler(sender, e);
		}
		StiBlocklyHelper.InvokeBlockly(Report, sender, ClickEvent);
	}

	protected virtual void OnDoubleClick(EventArgs e)
	{
	}

	public virtual void InvokeDoubleClick(object sender, EventArgs e)
	{
		OnDoubleClick(e);
		if (Events[EventDoubleClick] is EventHandler eventHandler)
		{
			eventHandler(sender, e);
		}
		StiBlocklyHelper.InvokeBlockly(Report, sender, DoubleClickEvent);
	}

	protected virtual void OnMouseEnter(EventArgs e)
	{
	}

	public virtual void InvokeMouseEnter(object sender, EventArgs e)
	{
		OnMouseEnter(e);
		if (Events[EventMouseEnter] is EventHandler eventHandler)
		{
			eventHandler(sender, e);
		}
		StiBlocklyHelper.InvokeBlockly(Report, sender, MouseEnterEvent);
	}

	protected virtual void OnMouseLeave(EventArgs e)
	{
	}

	public virtual void InvokeMouseLeave(object sender, EventArgs e)
	{
		OnMouseLeave(e);
		if (Events[EventMouseLeave] is EventHandler eventHandler)
		{
			eventHandler(sender, e);
		}
		StiBlocklyHelper.InvokeBlockly(Report, sender, MouseLeaveEvent);
	}

	protected virtual void OnPainting(StiPaintEventArgs e)
	{
	}

	public void InvokePainting(StiComponent sender, StiPaintEventArgs e)
	{
		OnPainting(e);
		if (Events[EventPainting] is StiPaintEventHandler stiPaintEventHandler)
		{
			stiPaintEventHandler(sender, e);
		}
	}

	protected virtual void OnPainted(StiPaintEventArgs e)
	{
	}

	public void InvokePainted(StiComponent sender, StiPaintEventArgs e)
	{
		OnPainted(e);
		if (Events[EventPainted] is StiPaintEventHandler stiPaintEventHandler)
		{
			stiPaintEventHandler(sender, e);
		}
	}

	internal bool IsPropertiesInitialized()
	{
		return ((StiService)this).IsPropertiesInitializedProtected();
	}

	[Browsable(false)]
	public virtual bool IsExportAsImage(StiExportFormat format)
	{
		IStiBrush stiBrush = this as IStiBrush;
		IStiTextBrush stiTextBrush = this as IStiTextBrush;
		if (stiBrush != null)
		{
			StiBrush brush = stiBrush.Brush;
			if (format == StiExportFormat.Pdf && (brush is StiGradientBrush || brush is StiGlareBrush || brush is StiHatchBrush || brush is StiGlassBrush))
			{
				return false;
			}
			if (format == StiExportFormat.Xps && brush is StiGradientBrush && (stiTextBrush == null || !(stiTextBrush.TextBrush is StiGradientBrush)))
			{
				return false;
			}
			if (format == StiExportFormat.Html || format == StiExportFormat.HtmlDiv || format == StiExportFormat.HtmlSpan || format == StiExportFormat.HtmlTable)
			{
				if (brush is StiHatchBrush)
				{
					return true;
				}
			}
			else if (brush is StiGlareBrush || brush is StiGradientBrush || brush is StiGlassBrush || brush is StiHatchBrush)
			{
				return true;
			}
		}
		if (stiTextBrush != null)
		{
			StiBrush textBrush = stiTextBrush.TextBrush;
			if (format == StiExportFormat.Pdf && (textBrush is StiGradientBrush || textBrush is StiGlareBrush || textBrush is StiHatchBrush))
			{
				return false;
			}
			if (textBrush is StiGradientBrush || textBrush is StiGlareBrush || textBrush is StiGlassBrush || textBrush is StiHatchBrush)
			{
				return true;
			}
		}
		return false;
	}

	public virtual void OnRemoveComponent()
	{
	}

	private void InvokeOnResizeComponent(SizeD oldSize, SizeD newSize)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (!lockOnResize)
		{
			OnResizeComponent(oldSize, newSize);
		}
	}

	public virtual void OnResizeComponent(SizeD oldSize, SizeD newSize)
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		if (!(this is StiContainer stiContainer))
		{
			return;
		}
		decimal num = (decimal)((SizeD)(ref newSize)).Width - (decimal)((SizeD)(ref oldSize)).Width;
		decimal num2 = (decimal)((SizeD)(ref newSize)).Height - (decimal)((SizeD)(ref oldSize)).Height;
		foreach (StiComponent component in stiContainer.Components)
		{
			if (num != 0m)
			{
				if ((component.Anchor & StiAnchorMode.Left) > (StiAnchorMode)0 && (component.Anchor & StiAnchorMode.Right) > (StiAnchorMode)0)
				{
					double num3 = component.width;
					component.width = (double)((decimal)num3 + num);
					component.InvokeOnResizeComponent(new SizeD(num3, component.height), new SizeD(component.width, component.height));
				}
				if ((component.Anchor & StiAnchorMode.Left) == 0 && (component.Anchor & StiAnchorMode.Right) > (StiAnchorMode)0)
				{
					component.left = (double)((decimal)component.left + num);
				}
			}
			if (num2 != 0m)
			{
				if ((component.Anchor & StiAnchorMode.Top) > (StiAnchorMode)0 && (component.Anchor & StiAnchorMode.Bottom) > (StiAnchorMode)0)
				{
					double num4 = component.height;
					component.height = (double)((decimal)num4 + num2);
					component.InvokeOnResizeComponent(new SizeD(component.width, num4), new SizeD(component.width, component.height));
				}
				if ((component.Anchor & StiAnchorMode.Top) == 0 && (component.Anchor & StiAnchorMode.Bottom) > (StiAnchorMode)0)
				{
					component.top = (double)((decimal)component.top + num2);
				}
			}
		}
	}

	public Graphics GetMeasureGraphics()
	{
		if (Report == null)
		{
			return StiReport.GlobalMeasureGraphics;
		}
		return Report.ReportMeasureGraphics;
	}

	public virtual StiEventsCollection GetEvents()
	{
		StiEventsCollection stiEventsCollection = new StiEventsCollection();
		if (GetToolTipEvent != null)
		{
			stiEventsCollection.Add(GetToolTipEvent);
		}
		if (GetHyperlinkEvent != null)
		{
			stiEventsCollection.Add(GetHyperlinkEvent);
		}
		if (GetTagEvent != null)
		{
			stiEventsCollection.Add(GetTagEvent);
		}
		if (GetPointerEvent != null)
		{
			stiEventsCollection.Add(GetPointerEvent);
		}
		if (GetBookmarkEvent != null)
		{
			stiEventsCollection.Add(GetBookmarkEvent);
		}
		if (BeforePrintEvent != null)
		{
			stiEventsCollection.Add(BeforePrintEvent);
		}
		if (AfterPrintEvent != null)
		{
			stiEventsCollection.Add(AfterPrintEvent);
		}
		if (ClickEvent != null)
		{
			stiEventsCollection.Add(ClickEvent);
		}
		if (DoubleClickEvent != null)
		{
			stiEventsCollection.Add(DoubleClickEvent);
		}
		if (MouseEnterEvent != null)
		{
			stiEventsCollection.Add(MouseEnterEvent);
		}
		if (MouseLeaveEvent != null)
		{
			stiEventsCollection.Add(MouseLeaveEvent);
		}
		if (GetDrillDownReportEvent != null)
		{
			stiEventsCollection.Add(GetDrillDownReportEvent);
		}
		return stiEventsCollection;
	}

	protected void CopyEventHandlersToComponent(StiComponent component, bool onlyBeforePrint, StiComponent from = null)
	{
		if (from == null)
		{
			from = this;
		}
		if (onlyBeforePrint)
		{
			if (from.Events[EventBeforePrint] is EventHandler value)
			{
				component.Events.AddHandler(EventBeforePrint, value);
			}
		}
		else
		{
			component.Events.AddHandlers(from.Events);
		}
	}

	public void ClearContents()
	{
		if (IsSelected)
		{
			if (this is IStiText stiText)
			{
				stiText.SetTextInternal(string.Empty);
			}
			if (this is IStiTextElement stiTextElement)
			{
				stiTextElement.Text = "";
			}
		}
		if (this is StiContainer stiContainer)
		{
			stiContainer.Components.ToList().ForEach(delegate(StiComponent c)
			{
				c.ClearContents();
			});
		}
	}

	public void AddSize(bool leftToRightDirection)
	{
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		StiContainer stiContainer = ((this is StiPage) ? (this as StiContainer) : Parent);
		if (leftToRightDirection)
		{
			if (Page.UnlimitedWidth)
			{
				Page.SegmentPerWidth++;
			}
			StiContainer stiContainer2 = stiContainer;
			while (!(stiContainer2 is StiPage))
			{
				RectangleD dockRegion = stiContainer2.GetDockRegion(stiContainer2.Parent);
				stiContainer2.Width += ((RectangleD)(ref dockRegion)).Width;
				if (stiContainer2.Right > ((RectangleD)(ref dockRegion)).Width)
				{
					stiContainer2.Width = ((RectangleD)(ref dockRegion)).Width - stiContainer2.Left;
				}
				stiContainer2 = stiContainer2.Parent;
			}
		}
		else
		{
			Page.SegmentPerHeight++;
			double num = Page.Height - height;
			if (!(this is StiPage) && !(this is StiBand))
			{
				height += num;
			}
			StiContainer stiContainer3 = stiContainer;
			while (!(stiContainer3 is StiPage))
			{
				RectangleD dockRegion2 = stiContainer3.GetDockRegion(stiContainer3.Parent);
				stiContainer3.Height += ((RectangleD)(ref dockRegion2)).Height;
				if (stiContainer3.Bottom > ((RectangleD)(ref dockRegion2)).Height)
				{
					stiContainer3.Height = ((RectangleD)(ref dockRegion2)).Height - stiContainer3.Top;
				}
				stiContainer3 = stiContainer3.Parent;
			}
			string text = "####UNLIMITED";
			StiContainer stiContainer4 = new StiContainer
			{
				Name = text,
				DockStyle = StiDockStyle.Top,
				Height = Page.PageHeight - Page.Margins.Top - Page.Margins.Bottom
			};
			double num2 = 0.0;
			int num3 = 0;
			while (num3 < Page.Components.Count)
			{
				StiComponent stiComponent = Page.Components[num3];
				if (stiComponent.Name != text && stiComponent.DockStyle == StiDockStyle.Top)
				{
					if (stiComponent.Height + num2 > stiContainer4.Height)
					{
						break;
					}
					Page.Components.Remove(stiComponent);
					stiContainer4.Components.Add(stiComponent);
					num2 += stiComponent.Height;
				}
				else
				{
					num3++;
				}
			}
			if (Page.UnlimitedBreakable)
			{
				stiContainer4.Height = Math.Max(num2, stiContainer4.Height);
			}
			else
			{
				stiContainer4.Height = num2;
			}
			Page.Components.Insert(num3, stiContainer4);
		}
		Page.DockToContainer();
	}

	public override string ToString()
	{
		return ToString(onlyAlias: false);
	}

	public string ToString(bool onlyAlias)
	{
		if (onlyAlias && !string.IsNullOrWhiteSpace(Alias))
		{
			return Alias;
		}
		if (Name == Alias || string.IsNullOrWhiteSpace(Alias))
		{
			return Name;
		}
		return Name + " [" + Alias + "]";
	}

	public virtual bool CanContainIn(StiComponent component)
	{
		if (component is IStiCrossTab)
		{
			return this is IStiCrossTabField;
		}
		if (component is IStiReportControl)
		{
			return false;
		}
		if (!(component is StiContainer))
		{
			return false;
		}
		if (component is StiClone)
		{
			return false;
		}
		if (component is StiSubReport)
		{
			return false;
		}
		if (component is IStiElement && !(this is IStiElement))
		{
			return false;
		}
		if (!(component is IStiElement) && this is IStiElement)
		{
			return false;
		}
		return true;
	}

	public int GetLevel()
	{
		StiComponent parent = Parent;
		int num = 1;
		while (parent != null)
		{
			parent = parent.Parent;
			num++;
		}
		return num;
	}

	public RectangleD ComponentToPage(RectangleD rect)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (Parent != null)
		{
			return Parent.ContainerToPage(rect);
		}
		return rect;
	}

	public RectangleD PageToComponent(RectangleD rect)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (Parent != null)
		{
			return Parent.PageToContainer(rect);
		}
		return rect;
	}

	public PointD ComponentToPage(PointD point)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (Parent != null)
		{
			return Parent.ContainerToPage(point);
		}
		return point;
	}

	public double ComponentToPage(double value)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		PointD val = this.ComponentToPage(new PointD(0.0, value));
		return ((PointD)(ref val)).Y;
	}

	public PointD PageToComponent(PointD point)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		StiContainer parent = Parent;
		if (parent != null)
		{
			return Parent.PageToContainer(point);
		}
		return point;
	}

	public static bool IsParentSelect(StiComponent component)
	{
		if (component is StiCrossLinePrimitive)
		{
			StiCrossLinePrimitive stiCrossLinePrimitive = component as StiCrossLinePrimitive;
			StiStartPointPrimitive startPoint = stiCrossLinePrimitive.GetStartPoint();
			StiEndPointPrimitive endPoint = stiCrossLinePrimitive.GetEndPoint();
			if (startPoint == null || !IsParentSelect(startPoint))
			{
				if (endPoint != null)
				{
					return IsParentSelect(endPoint);
				}
				return false;
			}
			return true;
		}
		StiContainer parent = component.Parent;
		while (parent != null && !(parent is StiPage))
		{
			if (parent.IsSelected)
			{
				return true;
			}
			parent = parent.Parent;
		}
		return false;
	}

	public static RectangleD DoOffsetRect(StiComponent component, RectangleD rect, RectangleD offsetRect)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		if (component.Locked)
		{
			return rect;
		}
		if (component is StiPage)
		{
			return rect;
		}
		StiDesignerInfo stiDesignerInfo = component?.Report?.Info;
		if (stiDesignerInfo != null && stiDesignerInfo.IsDragDropComponent && stiDesignerInfo.DraggingComponent != component && stiDesignerInfo.DraggingLabelComponent != component)
		{
			return rect;
		}
		if (!IsParentSelect(component) && component.IsSelected)
		{
			return ((RectangleD)(ref rect)).OffsetRect(offsetRect);
		}
		return rect;
	}

	public RectangleD GetPaintRectangle(bool convertToHInches, bool convertByZoom)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return GetPaintRectangle(convertToHInches, convertByZoom, docking: true);
	}

	public RectangleD GetPaintRectangle(bool convertToHInches, bool convertByZoom, bool docking)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		if (Page == null)
		{
			return RectangleD.Empty;
		}
		if (docking)
		{
			if (IsCross)
			{
				ClientRectangle = DockToContainer(ClientRectangle);
			}
			else if (DockStyle != StiDockStyle.None && Dockable)
			{
				DisplayRectangle = DockToContainer(DisplayRectangle);
			}
		}
		RectangleD val = DoOffsetRect(this, ClientRectangle, Page.OffsetRectangle);
		val = ComponentToPage(val);
		val = ((RectangleD)(ref val)).Normalize();
		if (!convertToHInches)
		{
			if (!convertByZoom)
			{
				return val;
			}
			return ((RectangleD)(ref val)).Multiply(Page.Zoom * StiScale.Factor);
		}
		if (!convertByZoom)
		{
			return Page.Unit.ConvertToHInches(val);
		}
		RectangleD val2 = Page.Unit.ConvertToHInches(val);
		return ((RectangleD)(ref val2)).Multiply(Page.Zoom * StiScale.Factor);
	}

	public void SetPaintRectangle(RectangleD rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		rect = PageToComponent(rect);
		rect = ((RectangleD)(ref rect)).Normalize();
		ClientRectangle = rect;
	}

	public virtual RectangleD GetPaintRectangle()
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return GetPaintRectangle(convertToHInches: true, convertByZoom: true);
	}

	public RectangleD GetDisplayRectangle()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		RectangleD rect = DisplayRectangle;
		rect = DoOffsetRect(this, rect, (Page == null) ? RectangleD.Empty : Page.OffsetRectangle);
		rect = ComponentToPage(rect);
		return ((RectangleD)(ref rect)).Normalize();
	}

	[Description("Returns DataBand on which the component is located. Returns null, if nothing is located.")]
	public virtual StiDataBand GetDataBand()
	{
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Invalid comparison between Unknown and I4
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Invalid comparison between Unknown and I4
		StiComponent parent = Parent;
		if (parent == null)
		{
			return null;
		}
		while (parent != null && !(parent is StiDataBand) && !(parent is StiPage))
		{
			if (parent is StiReportTitleBand stiReportTitleBand)
			{
				StiComponent master = stiReportTitleBand.GetMaster();
				if (master != null)
				{
					return master as StiDataBand;
				}
			}
			if (parent is StiPageHeaderBand stiPageHeaderBand)
			{
				StiComponent master2 = stiPageHeaderBand.GetMaster();
				if (master2 != null)
				{
					return master2 as StiDataBand;
				}
			}
			if (parent is StiReportSummaryBand stiReportSummaryBand)
			{
				StiComponent master3 = stiReportSummaryBand.GetMaster();
				if (master3 != null)
				{
					return master3 as StiDataBand;
				}
			}
			if (parent is StiPageFooterBand stiPageFooterBand)
			{
				StiComponent master4 = stiPageFooterBand.GetMaster();
				if (master4 != null)
				{
					return master4 as StiDataBand;
				}
			}
			if (parent is StiHeaderBand masterHeaderBand)
			{
				StiComponent stiComponent = ((Report != null && (int)Report.EngineVersion == 1) ? StiHeaderBandV1Builder.GetMaster(masterHeaderBand) : StiHeaderBandV2Builder.GetMaster(masterHeaderBand));
				if (stiComponent != null)
				{
					return stiComponent as StiDataBand;
				}
			}
			if (parent is StiFooterBand masterFooterBand)
			{
				StiComponent stiComponent2 = ((Report != null && (int)Report.EngineVersion == 1) ? StiFooterBandV1Builder.GetMaster(masterFooterBand) : StiFooterBandV2Builder.GetMaster(masterFooterBand));
				if (stiComponent2 != null)
				{
					return stiComponent2 as StiDataBand;
				}
			}
			parent = parent.Parent;
		}
		if (parent is StiDataBand)
		{
			return (StiDataBand)parent;
		}
		return null;
	}

	[Description("Returns GroupHeaderBand in which the component is located. Returns null, if nothing is located.")]
	public StiGroupHeaderBand GetGroupHeaderBand()
	{
		StiComponent stiComponent = ((this is StiGroupFooterBand) ? this : Parent);
		if (stiComponent == null)
		{
			return null;
		}
		if (stiComponent is StiChildBand && stiComponent.Parent != null)
		{
			int num = stiComponent.Parent.Components.IndexOf(stiComponent) - 1;
			while (num > 0 && stiComponent.Parent.Components[num] is StiChildBand)
			{
				num--;
			}
			if (num >= 0)
			{
				stiComponent = stiComponent.Parent.Components[num];
			}
		}
		while (stiComponent != null && !(stiComponent is StiDataBand) && !(stiComponent is StiGroupHeaderBand) && !(stiComponent is StiGroupFooterBand) && !(stiComponent is StiPage))
		{
			stiComponent = stiComponent.Parent;
		}
		if (stiComponent is StiGroupHeaderBand)
		{
			return (StiGroupHeaderBand)stiComponent;
		}
		if (stiComponent is StiGroupFooterBand || stiComponent is StiDataBand)
		{
			StiDataBand stiDataBand = null;
			if (!(stiComponent is StiDataBand))
			{
				int num2 = stiComponent.Parent.Components.IndexOf(stiComponent) - 1;
				for (int num3 = num2; num3 >= 0; num3--)
				{
					if (stiComponent.Parent.Components[num3] is StiDataBand stiDataBand2)
					{
						stiDataBand = stiDataBand2;
						break;
					}
				}
			}
			else
			{
				stiDataBand = stiComponent as StiDataBand;
			}
			if (stiDataBand != null)
			{
				StiDataBandV1Builder stiDataBandV1Builder = StiV1Builder.GetBuilder(typeof(StiDataBand)) as StiDataBandV1Builder;
				stiDataBand.DataBandInfoV1.GroupHeaderComponents = stiDataBandV1Builder.GetGroupHeaders(stiDataBand);
				stiDataBand.DataBandInfoV1.GroupFooterComponents = stiDataBandV1Builder.GetGroupFooters(stiDataBand);
				stiDataBandV1Builder.GroupsComparison(stiDataBand);
				if (stiComponent is StiGroupFooterBand)
				{
					return ((StiGroupFooterBand)stiComponent).GroupFooterBandInfoV1.GroupHeader;
				}
				if (stiDataBand.DataBandInfoV1.GroupHeaderComponents.Count > 0)
				{
					return stiDataBand.DataBandInfoV1.GroupHeaderComponents[stiDataBand.DataBandInfoV1.GroupHeaderComponents.Count - 1] as StiGroupHeaderBand;
				}
			}
		}
		return null;
	}

	public bool PlacedOnColumnBand()
	{
		StiComponent stiComponent = this;
		while (stiComponent != null || !(stiComponent is StiPage))
		{
			if (stiComponent is StiColumnFooterBand || stiComponent is StiColumnHeaderBand)
			{
				return true;
			}
			if (stiComponent is StiBand)
			{
				return false;
			}
			stiComponent = stiComponent.Parent;
		}
		return false;
	}

	public StiContainer GetContainer()
	{
		StiComponent parent = Parent;
		if (parent == null)
		{
			return null;
		}
		while (parent != null && !(parent is StiContainer) && !(parent is StiPage))
		{
			parent = parent.Parent;
		}
		if (parent is StiContainer)
		{
			return (StiContainer)parent;
		}
		if (parent is StiPage)
		{
			return (StiContainer)parent;
		}
		return parent.Page;
	}

	internal bool CheckForParentComponent(StiComponent comp)
	{
		if (Parent == null)
		{
			return false;
		}
		if (Parent == comp)
		{
			return true;
		}
		return Parent.CheckForParentComponent(comp);
	}

	internal StiBaseStyle GetComponentStyle()
	{
		if (string.IsNullOrWhiteSpace(ComponentStyle) || Report == null)
		{
			return null;
		}
		return Report.Styles[ComponentStyle];
	}

	internal virtual List<object> GetFormatObjects()
	{
		return new List<object> { this };
	}

	internal virtual object GetFirstFormatObject()
	{
		return GetFormatObjects()?.FirstOrDefault();
	}

	public StiComponent()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiComponent(RectangleD rect)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		ClientRectangle = rect;
		lockOnResize = false;
		if (this is StiDataBand || this is StiGroupHeaderBand)
		{
			Interaction = new StiBandInteraction();
		}
		else if (this is StiCrossHeader)
		{
			Interaction = new StiCrossHeaderInteraction();
		}
		else
		{
			Interaction = new StiInteraction();
		}
	}
}
