using System;
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
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Components.Design;
using Stimulsoft.Report.Design;
using Stimulsoft.Report.Engine;
using Stimulsoft.Report.Events;
using Stimulsoft.Report.Helpers;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Dialogs;

[StiToolbox(false)]
[StiServiceBitmap(typeof(StiReportControl), "Stimulsoft.Report.Dialogs.Bmp.StiReportControl.gif")]
[StiServiceCategoryBitmap(typeof(StiComponent), "Stimulsoft.Report.Dialogs.Bmp.StiReportControl.gif")]
[StiEngine(/*Could not decode attribute arguments.*/)]
[StiDesigner("Stimulsoft.Report.Dialogs.Design.StiReportControlDesigner, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
[StiWpfDesigner("Stimulsoft.Report.WpfDesign.StiWpfReportControlDesigner, Stimulsoft.Report.WpfDesign, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a")]
public class StiReportControl : StiContainer, IStiReportControl
{
	private static readonly object EventEnter = new object();

	private static readonly object EventLeave = new object();

	private static readonly object EventMouseDown = new object();

	private static readonly object EventMouseUp = new object();

	private static readonly object EventMouseMove = new object();

	private object controlWpf;

	private StiDataBindingsCollection dataBindings;

	private string tagValueBinding = "";

	private Color foreColor = Color.Black;

	private Point location = new Point(0, 0);

	private Font font = new Font("Microsoft Sans Serif", 8f);

	private Color backColor = SystemColors.Control;

	private bool visible = true;

	private RightToLeft rightToLeft;

	[Browsable(false)]
	public override string ServiceCategory => StiLocalization.Get("Report", "Dialogs");

	[StiNonSerialized]
	[Browsable(false)]
	public override bool UseParentStyles
	{
		get
		{
			return base.UseParentStyles;
		}
		set
		{
			if (base.UseParentStyles != value)
			{
				base.UseParentStyles = value;
			}
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override SizeD MinSize
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.MinSize;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.MinSize = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
	public override SizeD MaxSize
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.MaxSize;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.MaxSize = value;
		}
	}

	[StiNonSerialized]
	[Browsable(false)]
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

	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(270)]
	public override StiDockStyle DockStyle
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

	[Browsable(false)]
	[StiNonSerialized]
	public override StiBorder Border
	{
		get
		{
			return base.Border;
		}
		set
		{
			base.Border = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public override StiBrush Brush
	{
		get
		{
			return base.Brush;
		}
		set
		{
			base.Brush = value;
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
			UpdateReportControl("Location");
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
			UpdateReportControl("Location");
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
			UpdateReportControl("Size");
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
			UpdateReportControl("Size");
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool CanShrink
	{
		get
		{
			return base.CanShrink;
		}
		set
		{
			base.CanShrink = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override bool CanGrow
	{
		get
		{
			return base.CanGrow;
		}
		set
		{
			base.CanGrow = value;
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
			base.Printable = value;
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
			base.Pointer = value;
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
			base.Bookmark = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiHyperlinkExpression Hyperlink
	{
		get
		{
			return base.Hyperlink;
		}
		set
		{
			base.Hyperlink = value;
		}
	}

	[Browsable(false)]
	[StiNonSerialized]
	public sealed override StiConditionsCollection Conditions
	{
		get
		{
			return base.Conditions;
		}
		set
		{
			base.Conditions = value;
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

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiGetHyperlinkEvent GetHyperlinkEvent
	{
		get
		{
			return base.GetHyperlinkEvent;
		}
		set
		{
			base.GetHyperlinkEvent = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiBeforePrintEvent BeforePrintEvent
	{
		get
		{
			return base.BeforePrintEvent;
		}
		set
		{
			base.BeforePrintEvent = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiAfterPrintEvent AfterPrintEvent
	{
		get
		{
			return base.AfterPrintEvent;
		}
		set
		{
			base.AfterPrintEvent = value;
		}
	}

	[StiEventHide]
	[StiNonSerialized]
	public sealed override StiGetDrillDownReportEvent GetDrillDownReportEvent
	{
		get
		{
			return base.GetDrillDownReportEvent;
		}
		set
		{
			base.GetDrillDownReportEvent = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event MouseLeave.")]
	public override StiMouseLeaveEvent MouseLeaveEvent
	{
		get
		{
			return base.MouseLeaveEvent;
		}
		set
		{
			base.MouseLeaveEvent = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event MouseEnter.")]
	public override StiMouseEnterEvent MouseEnterEvent
	{
		get
		{
			return base.MouseEnterEvent;
		}
		set
		{
			base.MouseEnterEvent = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event GetTag.")]
	public override StiGetTagEvent GetTagEvent
	{
		get
		{
			return base.GetTagEvent;
		}
		set
		{
			base.GetTagEvent = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event GetToolTip.")]
	public override StiGetToolTipEvent GetToolTipEvent
	{
		get
		{
			return base.GetToolTipEvent;
		}
		set
		{
			base.GetToolTipEvent = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
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
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event DoubleClick.")]
	public override StiDoubleClickEvent DoubleClickEvent
	{
		get
		{
			return base.DoubleClickEvent;
		}
		set
		{
			base.DoubleClickEvent = value;
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event Enter.")]
	public virtual StiEnterEvent EnterEvent
	{
		get
		{
			return new StiEnterEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event Leave.")]
	public virtual StiLeaveEvent LeaveEvent
	{
		get
		{
			return new StiLeaveEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event MouseDown.")]
	public virtual StiMouseDownEvent MouseDownEvent
	{
		get
		{
			return new StiMouseDownEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event MouseUp.")]
	public virtual StiMouseUpEvent MouseUpEvent
	{
		get
		{
			return new StiMouseUpEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
	[Description("Gets or sets a script of the event MouseMove.")]
	public virtual StiMouseMoveEvent MouseMoveEvent
	{
		get
		{
			return new StiMouseMoveEvent(this);
		}
		set
		{
			value?.Set(this, value.Script);
		}
	}

	public override string HelpUrl => null;

	[StiSerializable]
	[DefaultValue("")]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets a style of a component.")]
	[Editor("Stimulsoft.Report.Design.StiDialogStyleEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	[StiOrder(220)]
	public override string ComponentStyle
	{
		get
		{
			return base.ComponentStyle;
		}
		set
		{
			base.ComponentStyle = value;
		}
	}

	public override int ToolboxPosition => 0;

	public override StiToolboxCategory ToolboxCategory => StiToolboxCategory.Controls;

	public override string LocalizedCategory => ((StiService)this).ServiceCategory;

	public override string LocalizedName => StiLocalization.Get("Dialogs", "StiReportControl");

	[Browsable(false)]
	public object ControlWpf
	{
		get
		{
			return controlWpf;
		}
		set
		{
			controlWpf = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(760)]
	[Browsable(true)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override StiToolTipExpression ToolTip
	{
		get
		{
			return base.ToolTip;
		}
		set
		{
			base.ToolTip = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[StiOrder(720)]
	[Browsable(true)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override StiTagExpression Tag
	{
		get
		{
			return base.Tag;
		}
		set
		{
			base.Tag = value;
		}
	}

	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue("")]
	[Description("Gets the data bindings for the control.")]
	[StiOrder(110)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public StiDataBindingsCollection DataBindings
	{
		get
		{
			return dataBindings;
		}
		set
		{
			dataBindings = value;
		}
	}

	[Editor("Stimulsoft.Report.Components.Design.StiDataColumnEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiSerializable]
	[Browsable(false)]
	[Bindable(BindableSupport.Yes)]
	[DefaultValue("")]
	[Description("Gets the data bindings for the tag value.")]
	public string TagValueBinding
	{
		get
		{
			return tagValueBinding;
		}
		set
		{
			tagValueBinding = value;
		}
	}

	[StiSerializable]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("Gets or sets the foreground color of the control.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(340)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color ForeColor
	{
		get
		{
			return foreColor;
		}
		set
		{
			foreColor = value;
			UpdateReportControl("ForeColor");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[Description("Gets or sets the height and width of the control.")]
	[StiOrder(670)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Size Size
	{
		get
		{
			return new Size((int)base.Width, (int)base.Height);
		}
		set
		{
			base.Width = value.Width;
			base.Height = value.Height;
			UpdateReportControl("Size");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Browsable(true)]
	[Description("Gets or sets the coordinates of the upper-left corner of the control relative to the upper-left corner of its container.")]
	[StiOrder(480)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Point Location
	{
		get
		{
			return new Point((int)base.Left, (int)base.Top);
		}
		set
		{
			base.Left = value.X;
			base.Top = value.Y;
			UpdateReportControl("Location");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[Description("The font used to display text in the component.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(330)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Font Font
	{
		get
		{
			return font;
		}
		set
		{
			font = value;
			UpdateReportControl("Font");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiColorConverter))]
	[Editor("Stimulsoft.Base.Drawing.Design.StiColorEditor, Stimulsoft.Report.Design, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Description("Gets or sets the background color for the control.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(140)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual Color BackColor
	{
		get
		{
			return backColor;
		}
		set
		{
			backColor = value;
			UpdateReportControl("BackColor");
		}
	}

	[StiSerializable]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value indicating whether the control can respond to user interaction.")]
	[StiOrder(310)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public override bool Enabled
	{
		get
		{
			return base.Enabled;
		}
		set
		{
			base.Enabled = value;
			UpdateReportControl("Enabled");
		}
	}

	[StiSerializable]
	[DefaultValue(true)]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiBoolConverter))]
	[Description("Gets or sets a value indicating whether the control is displayed.")]
	[StiOrder(790)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			visible = value;
			UpdateReportControl("Visible");
		}
	}

	[StiSerializable]
	[StiCategory(/*Could not decode attribute arguments.*/)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[TypeConverter(typeof(StiEnumConverter))]
	[Description("Gets or sets a value indicating whether control's elements are aligned to support locales using right-to-left fonts.")]
	[StiGuiMode(/*Could not decode attribute arguments.*/)]
	[StiOrder(600)]
	[StiPropertyLevel(/*Could not decode attribute arguments.*/)]
	public virtual RightToLeft RightToLeft
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return rightToLeft;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			rightToLeft = value;
			UpdateReportControl("RightToLeft");
		}
	}

	[Browsable(false)]
	public override RectangleD DefaultClientRectangle => new RectangleD(0.0, 0.0, 96.0, 24.0);

	[Browsable(false)]
	public virtual bool IsReportContainer => false;

	public event EventHandler Enter
	{
		add
		{
			base.Events.AddHandler(EventEnter, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventEnter, value);
		}
	}

	public event EventHandler Leave
	{
		add
		{
			base.Events.AddHandler(EventLeave, value);
		}
		remove
		{
			base.Events.RemoveHandler(EventLeave, value);
		}
	}

	public event MouseEventHandler MouseDown
	{
		add
		{
			base.Events.AddHandler(EventMouseDown, (Delegate?)(object)value);
		}
		remove
		{
			base.Events.RemoveHandler(EventMouseDown, (Delegate?)(object)value);
		}
	}

	public event MouseEventHandler MouseUp
	{
		add
		{
			base.Events.AddHandler(EventMouseUp, (Delegate?)(object)value);
		}
		remove
		{
			base.Events.RemoveHandler(EventMouseUp, (Delegate?)(object)value);
		}
	}

	public event MouseEventHandler MouseMove
	{
		add
		{
			base.Events.AddHandler(EventMouseMove, (Delegate?)(object)value);
		}
		remove
		{
			base.Events.RemoveHandler(EventMouseMove, (Delegate?)(object)value);
		}
	}

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		JObject val = base.SaveToJsonObject(mode);
		JsonExtensions.RemoveProperty(val, "UseParentStyles");
		JsonExtensions.RemoveProperty(val, "MinSize");
		JsonExtensions.RemoveProperty(val, "MaxSize");
		JsonExtensions.RemoveProperty(val, "Interaction");
		JsonExtensions.RemoveProperty(val, "PrintOn");
		JsonExtensions.RemoveProperty(val, "Border");
		JsonExtensions.RemoveProperty(val, "Brush");
		JsonExtensions.RemoveProperty(val, "GrowToHeight");
		JsonExtensions.RemoveProperty(val, "ShiftMode");
		JsonExtensions.RemoveProperty(val, "Left");
		JsonExtensions.RemoveProperty(val, "Top");
		JsonExtensions.RemoveProperty(val, "Width");
		JsonExtensions.RemoveProperty(val, "Height");
		JsonExtensions.RemoveProperty(val, "CanShrink");
		JsonExtensions.RemoveProperty(val, "CanGrow");
		JsonExtensions.RemoveProperty(val, "Printable");
		JsonExtensions.RemoveProperty(val, "Pointer");
		JsonExtensions.RemoveProperty(val, "Bookmark");
		JsonExtensions.RemoveProperty(val, "Hyperlink");
		JsonExtensions.RemoveProperty(val, "Conditions");
		JsonExtensions.RemoveProperty(val, "GetPointerEvent");
		JsonExtensions.RemoveProperty(val, "GetBookmarkEvent");
		JsonExtensions.RemoveProperty(val, "GetHyperlinkEvent");
		JsonExtensions.RemoveProperty(val, "BeforePrintEvent");
		JsonExtensions.RemoveProperty(val, "AfterPrintEvent");
		JsonExtensions.RemoveProperty(val, "GetDrillDownReportEvent");
		JsonExtensions.AddPropertyJObject(val, "EnterEvent", EnterEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "LeaveEvent", LeaveEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "MouseDownEvent", MouseDownEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "MouseUpEvent", MouseUpEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyJObject(val, "MouseMoveEvent", MouseMoveEvent.SaveToJsonObject(mode));
		JsonExtensions.AddPropertyStringNullOrEmpty(val, "TagValueBinding", TagValueBinding);
		JsonExtensions.AddPropertySize(val, "Size", Size);
		JsonExtensions.AddPropertyPoint(val, "Location", Location);
		JsonExtensions.AddPropertyFontMicrosoftSansSerif8(val, "Font", Font);
		JsonExtensions.AddPropertyColor(val, "BackColor", BackColor, SystemColors.Control);
		JsonExtensions.AddPropertyBool(val, "Visible", Visible, true, false);
		JsonExtensions.AddPropertyEnum(val, "RightToLeft", (Enum)(object)RightToLeft, (Enum)(object)(RightToLeft)0);
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Expected O, but got Unknown
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Expected O, but got Unknown
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Expected O, but got Unknown
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Expected O, but got Unknown
		base.LoadFromJsonObject(jObject);
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "EnterEvent":
			{
				StiEnterEvent stiEnterEvent = new StiEnterEvent();
				stiEnterEvent.LoadFromJsonObject((JObject)item.Value);
				EnterEvent = stiEnterEvent;
				break;
			}
			case "LeaveEvent":
			{
				StiLeaveEvent stiLeaveEvent = new StiLeaveEvent();
				stiLeaveEvent.LoadFromJsonObject((JObject)item.Value);
				LeaveEvent = stiLeaveEvent;
				break;
			}
			case "MouseDownEvent":
			{
				StiMouseDownEvent stiMouseDownEvent = new StiMouseDownEvent();
				stiMouseDownEvent.LoadFromJsonObject((JObject)item.Value);
				MouseDownEvent = stiMouseDownEvent;
				break;
			}
			case "MouseUpEvent":
			{
				StiMouseUpEvent stiMouseUpEvent = new StiMouseUpEvent();
				stiMouseUpEvent.LoadFromJsonObject((JObject)item.Value);
				MouseUpEvent = stiMouseUpEvent;
				break;
			}
			case "MouseMoveEvent":
			{
				StiMouseMoveEvent stiMouseMoveEvent = new StiMouseMoveEvent();
				stiMouseMoveEvent.LoadFromJsonObject((JObject)item.Value);
				MouseMoveEvent = stiMouseMoveEvent;
				break;
			}
			case "TagValueBinding":
				tagValueBinding = StiJsonReportObjectHelper.DeserializeString(item);
				break;
			case "Size":
				Size = StiJsonReportObjectHelper.DeserializeSize(item);
				break;
			case "Location":
				Location = StiJsonReportObjectHelper.DeserializePoint(item);
				break;
			case "Font":
				font = StiJsonReportObjectHelper.DeserializeFont(item, font);
				break;
			case "BackColor":
				backColor = StiJsonReportObjectHelper.DeserializeColor(item);
				break;
			case "Visible":
				visible = StiJsonReportObjectHelper.DeserializeBool(item);
				break;
			case "RightToLeft":
				rightToLeft = StiJsonReportObjectHelper.DeserializeEnum<RightToLeft>(item);
				break;
			}
		}
	}

	public override List<StiFont> GetFonts()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		List<StiFont> fonts = base.GetFonts();
		fonts.Add(new StiFont(Font));
		return fonts.Distinct().ToList();
	}

	public override object Clone()
	{
		return base.Clone();
	}

	public override void Convert(StiUnit oldUnit, StiUnit newUnit, bool isReportSnapshot = false)
	{
	}

	public override void PackService()
	{
		base.PackService();
		dataBindings = null;
	}

	public void InvokeEnter(EventArgs e)
	{
		if (base.Events[EventEnter] is EventHandler eventHandler)
		{
			eventHandler(this, e);
		}
		StiBlocklyHelper.InvokeBlockly(Report, this, EnterEvent);
	}

	public void InvokeLeave(EventArgs e)
	{
		if (base.Events[EventLeave] is EventHandler eventHandler)
		{
			eventHandler(this, e);
		}
	}

	public void InvokeMouseDown(MouseEventArgs e)
	{
		Delegate? @delegate = base.Events[EventMouseDown];
		MouseEventHandler val = (MouseEventHandler)(object)((@delegate is MouseEventHandler) ? @delegate : null);
		if (val != null)
		{
			val.Invoke((object)this, e);
		}
		StiBlocklyHelper.InvokeBlockly(Report, this, MouseDownEvent, (EventArgs)(object)e);
	}

	public void InvokeMouseUp(MouseEventArgs e)
	{
		Delegate? @delegate = base.Events[EventMouseUp];
		MouseEventHandler val = (MouseEventHandler)(object)((@delegate is MouseEventHandler) ? @delegate : null);
		if (val != null)
		{
			val.Invoke((object)this, e);
		}
		StiBlocklyHelper.InvokeBlockly(Report, this, MouseUpEvent);
	}

	public void InvokeMouseMove(MouseEventArgs e)
	{
		Delegate? @delegate = base.Events[EventMouseMove];
		MouseEventHandler val = (MouseEventHandler)(object)((@delegate is MouseEventHandler) ? @delegate : null);
		if (val != null)
		{
			val.Invoke((object)this, e);
		}
		StiBlocklyHelper.InvokeBlockly(Report, this, MouseMoveEvent);
	}

	public override StiEventsCollection GetEvents()
	{
		StiEventsCollection stiEventsCollection = new StiEventsCollection();
		if (ClickEvent != null)
		{
			stiEventsCollection.Add(ClickEvent);
		}
		if (GetTagEvent != null)
		{
			stiEventsCollection.Add(GetTagEvent);
		}
		if (EnterEvent != null)
		{
			stiEventsCollection.Add(EnterEvent);
		}
		if (LeaveEvent != null)
		{
			stiEventsCollection.Add(LeaveEvent);
		}
		if (MouseLeaveEvent != null)
		{
			stiEventsCollection.Add(MouseLeaveEvent);
		}
		if (MouseEnterEvent != null)
		{
			stiEventsCollection.Add(MouseEnterEvent);
		}
		if (MouseDownEvent != null)
		{
			stiEventsCollection.Add(MouseDownEvent);
		}
		if (MouseUpEvent != null)
		{
			stiEventsCollection.Add(MouseUpEvent);
		}
		if (MouseMoveEvent != null)
		{
			stiEventsCollection.Add(MouseMoveEvent);
		}
		return stiEventsCollection;
	}

	public override bool CanContainIn(StiComponent component)
	{
		if (component is StiReportControl && ((StiReportControl)component).IsReportContainer)
		{
			return true;
		}
		if (component is StiForm)
		{
			return true;
		}
		return false;
	}

	private bool ShouldSerializeBackColor()
	{
		return backColor != SystemColors.Control;
	}

	private bool ShouldSerializeForeColor()
	{
		return foreColor != SystemColors.ControlText;
	}

	protected void UpdateReportControl(string propertyName)
	{
		if (Page != null)
		{
			((StiForm)Page).InvokeReportControlUpdate(this, propertyName);
		}
	}

	public virtual StiEvent GetDefaultEvent()
	{
		return ClickEvent;
	}

	public StiReportControl()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiReportControl(RectangleD rect)
		: base(rect)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
		dataBindings = new StiDataBindingsCollection(this);
	}
}
