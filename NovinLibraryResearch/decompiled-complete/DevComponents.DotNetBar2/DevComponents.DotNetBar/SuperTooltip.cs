using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.AdvTree;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar;

[Designer("DevComponents.DotNetBar.Design.SuperTooltipDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxItem(true)]
[ProvideProperty("SuperTooltip", typeof(IComponent))]
[ComVisible(false)]
[ToolboxBitmap(typeof(SuperTooltip), "Ribbon.SuperTooltip.ico")]
public class SuperTooltip : Component, IExtenderProvider
{
	private const string _2E00 = "DevComponents.Tree.Node";

	private SuperTooltipEventHandler _2E01;

	private EventHandler _2E02;

	private MarkupLinkClickEventHandler Ӟ;

	private bool _2E03 = true;

	private Hashtable _2E04 = new Hashtable();

	private SuperTooltipControl ᕵ;

	private Timer _0A04;

	private Timer _2E05;

	private bool _2E06 = true;

	private IntPtr _2E07 = IntPtr.Zero;

	private int _2E08 = 20;

	private long _2E09;

	private SuperTooltipInfo _2E0A = new SuperTooltipInfo();

	private static SuperTooltipInfo _2E0B;

	private Font _2E0C;

	private int _2E0D;

	private int _2E0E = 2;

	private Size _2E0F = new Size(150, 24);

	private bool ݕ = true;

	private bool ե = true;

	private int _2E10;

	private bool _2E11;

	private bool _2E12;

	private int ߐ;

	private bool _2E13 = true;

	private bool _2E14 = true;

	private bool _2E15 = true;

	private bool _2E16;

	private bool _2E17;

	[DefaultValue(0)]
	[Description("Indicates maximum width of the super tooltip.")]
	[Category("Appearance")]
	[Browsable(true)]
	public int MaximumWidth
	{
		get
		{
			return ߐ;
		}
		set
		{
			ߐ = value;
		}
	}

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether form active state is ignored when control is deciding whether to show tooltip.")]
	[Browsable(true)]
	public bool IgnoreFormActiveState
	{
		get
		{
			return _2E12;
		}
		set
		{
			_2E12 = value;
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Browsable(true)]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	public bool AntiAlias
	{
		get
		{
			return ե;
		}
		set
		{
			ե = value;
		}
	}

	[Browsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether SuperTooltip will be shown for the controls assigned to it.")]
	[DefaultValue(true)]
	public bool Enabled
	{
		get
		{
			return ݕ;
		}
		set
		{
			ݕ = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Description("Indicates whether tooltip is shown immediately after the mouse enters the control.")]
	[Category("Behavior")]
	public bool ShowTooltipImmediately
	{
		get
		{
			return _2E11;
		}
		set
		{
			_2E11 = value;
		}
	}

	[Description("Indicates minimum tooltip size.")]
	[Browsable(true)]
	[Category("Appearance")]
	public Size MinimumTooltipSize
	{
		get
		{
			return _2E0F;
		}
		set
		{
			_2E0F = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(20)]
	[Category("Behavior")]
	[Description("Indicates duration in seconds that tooltip is kept on screen after it is displayed.")]
	public int TooltipDuration
	{
		get
		{
			return _2E08;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			_2E08 = value;
		}
	}

	[DefaultValue(0)]
	[Browsable(true)]
	[Description("Indicates delay time for hiding the tooltip in milliseconds after mouse has left the control.")]
	[Category("Behavior")]
	public int DelayTooltipHideDuration
	{
		get
		{
			return _2E10;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			_2E10 = value;
		}
	}

	[DefaultValue(true)]
	[Browsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether tooltip position is checked before tooltip is displayed and adjusted to tooltip always falls into screen bounds.")]
	public bool CheckOnScreenPosition
	{
		get
		{
			return _2E06;
		}
		set
		{
			_2E06 = value;
		}
	}

	[Description("Indicates whether tooltip position is checked before tooltip is displayed and adjusted so tooltip does not overlap control its displayed for")]
	[Category("Behavior")]
	[Browsable(true)]
	[DefaultValue(true)]
	public bool CheckTooltipPosition
	{
		get
		{
			return _2E13;
		}
		set
		{
			_2E13 = value;
		}
	}

	[Category("Behavior")]
	[Description("")]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool PositionBelowControl
	{
		get
		{
			return _2E03;
		}
		set
		{
			_2E03 = value;
		}
	}

	[DefaultValue(null)]
	[Description("Indicates default setting for new Tooltips you create in design time.")]
	[Editor("DevComponents.DotNetBar.Design.SuperTooltipInfoEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Localizable(true)]
	public SuperTooltipInfo DefaultTooltipSettings
	{
		get
		{
			return _2E0A;
		}
		set
		{
			_2E0A = value;
			_2E0B = value;
		}
	}

	[Browsable(false)]
	public static SuperTooltipInfo DefaultSuperTooltipInfo => _2E0B;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Hashtable SuperTooltipInfoTable => _2E04;

	[Description("Indicates hover delay multiplier which controls how fast tooltip appears. The value set here indicates how many hover events are needed to occur before the tooltip is displayed.")]
	[Category("Behavior")]
	[DefaultValue(2)]
	public int HoverDelayMultiplier
	{
		get
		{
			return _2E0E;
		}
		set
		{
			_2E0E = value;
		}
	}

	private bool IsMouseOverSuperTooltip
	{
		get
		{
			if (_2E16)
			{
				return true;
			}
			if (ᕵ != null && ((Control)ᕵ).Visible && ((Control)ᕵ).Bounds.Contains(Control.MousePosition))
			{
				return true;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool IsTooltipVisible
	{
		get
		{
			if (ᕵ != null && ((Control)ᕵ).Visible)
			{
				return true;
			}
			return false;
		}
	}

	[Browsable(false)]
	public SuperTooltipControl SuperTooltipControl => ᕵ;

	[Browsable(true)]
	[Description("Indicates default tooltip font.")]
	[DefaultValue(null)]
	[Category("Appearance")]
	public Font DefaultFont
	{
		get
		{
			return _2E0C;
		}
		set
		{
			_2E0C = value;
		}
	}

	[Description("Indicates whether complete tooltip is shown including header, body and footer. When set to false only tooltip header is shown.")]
	[Browsable(true)]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool ShowTooltipDescription
	{
		get
		{
			return _2E14;
		}
		set
		{
			_2E14 = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether tooltip is shown when control that tooltip is assigned to is focused.")]
	[Browsable(true)]
	public bool ShowTooltipForFocusedControl
	{
		get
		{
			return _2E15;
		}
		set
		{
			_2E15 = value;
		}
	}

	public event SuperTooltipEventHandler BeforeTooltipDisplay
	{
		add
		{
			SuperTooltipEventHandler superTooltipEventHandler = _2E01;
			SuperTooltipEventHandler superTooltipEventHandler2;
			do
			{
				superTooltipEventHandler2 = superTooltipEventHandler;
				SuperTooltipEventHandler value2 = (SuperTooltipEventHandler)Delegate.Combine(superTooltipEventHandler2, value);
				superTooltipEventHandler = Interlocked.CompareExchange(ref _2E01, value2, superTooltipEventHandler2);
			}
			while ((object)superTooltipEventHandler != superTooltipEventHandler2);
		}
		remove
		{
			SuperTooltipEventHandler superTooltipEventHandler = _2E01;
			SuperTooltipEventHandler superTooltipEventHandler2;
			do
			{
				superTooltipEventHandler2 = superTooltipEventHandler;
				SuperTooltipEventHandler value2 = (SuperTooltipEventHandler)Delegate.Remove(superTooltipEventHandler2, value);
				superTooltipEventHandler = Interlocked.CompareExchange(ref _2E01, value2, superTooltipEventHandler2);
			}
			while ((object)superTooltipEventHandler != superTooltipEventHandler2);
		}
	}

	public event EventHandler TooltipClosed
	{
		add
		{
			EventHandler eventHandler = _2E02;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2E02, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2E02;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2E02, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public event MarkupLinkClickEventHandler MarkupLinkClick
	{
		add
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Combine(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
		remove
		{
			MarkupLinkClickEventHandler markupLinkClickEventHandler = Ӟ;
			MarkupLinkClickEventHandler markupLinkClickEventHandler2;
			do
			{
				markupLinkClickEventHandler2 = markupLinkClickEventHandler;
				MarkupLinkClickEventHandler value2 = (MarkupLinkClickEventHandler)Delegate.Remove(markupLinkClickEventHandler2, value);
				markupLinkClickEventHandler = Interlocked.CompareExchange(ref Ӟ, value2, markupLinkClickEventHandler2);
			}
			while ((object)markupLinkClickEventHandler != markupLinkClickEventHandler2);
		}
	}

	protected override void Dispose(bool disposing)
	{
		HideTooltip();
		DestroyActiveWindowTimer();
		DisposeHideDelayedTimer();
		base.Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeMinimumTooltipSize()
	{
		if (_2E0F.Width == 150)
		{
			return _2E0F.Height != 24;
		}
		return true;
	}

	[Editor("DevComponents.DotNetBar.Design.SuperTooltipInfoEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[DefaultValue(null)]
	[Localizable(true)]
	public SuperTooltipInfo GetSuperTooltip(IComponent c)
	{
		if (_2E04.Contains(c) && _2E04[c] is SuperTooltipInfo result)
		{
			return result;
		}
		return null;
	}

	public void SetSuperTooltip(IComponent c, SuperTooltipInfo info)
	{
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		if (info != null)
		{
			if (info.BodyText == null)
			{
				info.BodyText = "";
			}
			if (info.FooterText == null)
			{
				info.FooterText = "";
			}
		}
		if (_2E04.Contains(c))
		{
			if (info == null)
			{
				RemoveSuperTooltipInfo(c);
			}
			else
			{
				_2E04[c] = info;
			}
		}
		else if (info != null)
		{
			AddSuperTooltipInfo(c, info);
		}
		if (c is MaskedTextBoxAdv && !base.DesignMode)
		{
			SetSuperTooltip((IComponent)((MaskedTextBoxAdv)c).MaskedTextBox, info);
		}
		else if (c is TextBoxDropDown)
		{
			SetSuperTooltip((IComponent)((TextBoxDropDown)c).TextBox, info);
		}
		else if (c is ExpandablePanel)
		{
			SetSuperTooltip((IComponent)((ExpandablePanel)c).TitlePanel, info);
		}
		else
		{
			if (!(c is NumericUpDown) || base.DesignMode)
			{
				return;
			}
			NumericUpDown val = (NumericUpDown)c;
			foreach (Control item in (ArrangedElementCollection)((Control)val).Controls)
			{
				Control c2 = item;
				SetSuperTooltip((IComponent)c2, info);
			}
		}
	}

	private void RemoveSuperTooltipInfo(IComponent c)
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Expected O, but got Unknown
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Expected O, but got Unknown
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Expected O, but got Unknown
		_2E04.Remove(c);
		if (base.DesignMode)
		{
			return;
		}
		if (c is MaskedTextBoxAdv)
		{
			RemoveSuperTooltipInfo((IComponent)((MaskedTextBoxAdv)c).MaskedTextBox);
		}
		else if (c is TextBoxDropDown)
		{
			RemoveSuperTooltipInfo((IComponent)((TextBoxDropDown)c).TextBox);
		}
		else if (c is ExpandablePanel)
		{
			RemoveSuperTooltipInfo((IComponent)((ExpandablePanel)c).TitlePanel);
		}
		if (c is ComboBoxItem)
		{
			ComboBoxItem comboBoxItem = c as ComboBoxItem;
			comboBoxItem.MouseHover -= ComponentMouseHover;
			comboBoxItem.MouseLeave -= ComponentMouseLeave;
			comboBoxItem.MouseEnter -= ComponentMouseEnter;
			comboBoxItem.MouseDown -= new MouseEventHandler(ComponentMouseDown);
			((Control)comboBoxItem.ComboBoxEx).MouseHover -= ComponentMouseHover;
			((Control)comboBoxItem.ComboBoxEx).MouseLeave -= ComponentMouseLeave;
			((Control)comboBoxItem.ComboBoxEx).MouseEnter -= ComponentMouseEnter;
			((Control)comboBoxItem.ComboBoxEx).MouseDown -= new MouseEventHandler(ComponentMouseDown);
		}
		else if (c is TextBoxItem)
		{
			TextBoxItem textBoxItem = c as TextBoxItem;
			textBoxItem.MouseHover -= ComponentMouseHover;
			textBoxItem.MouseLeave -= ComponentMouseLeave;
			textBoxItem.MouseEnter -= ComponentMouseEnter;
			textBoxItem.MouseDown -= new MouseEventHandler(ComponentMouseDown);
			((Control)textBoxItem.TextBox).MouseHover -= ComponentMouseHover;
			((Control)textBoxItem.TextBox).MouseLeave -= ComponentMouseLeave;
			((Control)textBoxItem.TextBox).MouseEnter -= ComponentMouseEnter;
			((Control)textBoxItem.TextBox).MouseDown -= new MouseEventHandler(ComponentMouseDown);
		}
		else if (c is BaseItem)
		{
			BaseItem baseItem = c as BaseItem;
			baseItem.MouseHover -= ComponentMouseHover;
			baseItem.MouseLeave -= ComponentMouseLeave;
			baseItem.MouseEnter -= ComponentMouseEnter;
			baseItem.MouseDown -= new MouseEventHandler(ComponentMouseDown);
		}
		else if (c is RibbonBar)
		{
			RibbonBar ribbonBar = c as RibbonBar;
			ribbonBar.DialogLauncherMouseHover -= ComponentMouseHover;
			ribbonBar.DialogLauncherMouseLeave -= ComponentMouseLeave;
			ribbonBar.DialogLauncherMouseDown -= new MouseEventHandler(ComponentMouseDown);
			ribbonBar.DialogLauncherMouseEnter -= ComponentMouseEnter;
		}
		else if (c is Control)
		{
			Control val = (Control)((c is Control) ? c : null);
			val.MouseHover -= ComponentMouseHover;
			val.MouseLeave -= ComponentMouseLeave;
			val.MouseDown -= new MouseEventHandler(ComponentMouseDown);
			val.MouseEnter -= ComponentMouseEnter;
		}
		else if (c is ISuperTooltipInfoProvider)
		{
			ISuperTooltipInfoProvider superTooltipInfoProvider = c as ISuperTooltipInfoProvider;
			superTooltipInfoProvider.DisplayTooltip -= ComponentMouseHover;
			superTooltipInfoProvider.HideTooltip -= ComponentHideTooltip;
		}
		else if (c is ToolStripButton)
		{
			ToolStripButton val2 = (ToolStripButton)((c is ToolStripButton) ? c : null);
			((ToolStripItem)val2).MouseHover -= ComponentMouseHover;
			((ToolStripItem)val2).MouseLeave -= ComponentMouseLeave;
			((ToolStripItem)val2).MouseEnter -= ComponentMouseEnter;
			((ToolStripItem)val2).MouseDown -= new MouseEventHandler(ComponentMouseDown);
		}
		else if (c is Node || IsTreeGXType(c.GetType()))
		{
			Type type = c.GetType();
			EventInfo @event = type.GetEvent("NodeMouseHover");
			if ((object)@event != null)
			{
				Delegate handler = Delegate.CreateDelegate(typeof(EventHandler), this, "ComponentMouseHover");
				@event.RemoveEventHandler(c, handler);
			}
			@event = type.GetEvent("NodeMouseLeave");
			if ((object)@event != null)
			{
				Delegate handler2 = Delegate.CreateDelegate(typeof(EventHandler), this, "ComponentMouseLeave");
				@event.RemoveEventHandler(c, handler2);
			}
			@event = type.GetEvent("NodeMouseEnter");
			if ((object)@event != null)
			{
				Delegate handler3 = Delegate.CreateDelegate(typeof(EventHandler), this, "ComponentMouseEnter");
				@event.RemoveEventHandler(c, handler3);
			}
			@event = type.GetEvent("NodeMouseDown");
			if ((object)@event != null)
			{
				Delegate handler4 = Delegate.CreateDelegate(typeof(MouseEventHandler), this, "ComponentMouseDown");
				@event.RemoveEventHandler(c, handler4);
			}
		}
		else if (c is TabItem)
		{
			TabItem tabItem = c as TabItem;
			tabItem.MouseHover -= ComponentMouseHover;
			tabItem.MouseLeave -= ComponentMouseLeave;
			tabItem.MouseEnter -= ComponentMouseEnter;
			tabItem.MouseDown -= new MouseEventHandler(ComponentMouseDown);
		}
	}

	private void AddSuperTooltipInfo(IComponent c, SuperTooltipInfo info)
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		_2E04[c] = info;
		if (c is ISuperTooltipInfoProvider)
		{
			ISuperTooltipInfoProvider superTooltipInfoProvider = c as ISuperTooltipInfoProvider;
			superTooltipInfoProvider.DisplayTooltip += ComponentMouseHover;
			superTooltipInfoProvider.HideTooltip += ComponentHideTooltip;
		}
		else if (c is ComboBoxItem)
		{
			ComboBoxItem comboBoxItem = c as ComboBoxItem;
			comboBoxItem.MouseHover += ComponentMouseHover;
			comboBoxItem.MouseLeave += ComponentMouseLeave;
			comboBoxItem.MouseEnter += ComponentMouseEnter;
			comboBoxItem.MouseDown += new MouseEventHandler(ComponentMouseDown);
			((Control)comboBoxItem.ComboBoxEx).MouseHover += ComponentMouseHover;
			((Control)comboBoxItem.ComboBoxEx).MouseLeave += ComponentMouseLeave;
			((Control)comboBoxItem.ComboBoxEx).MouseEnter += ComponentMouseEnter;
			((Control)comboBoxItem.ComboBoxEx).MouseDown += new MouseEventHandler(ComponentMouseDown);
		}
		else if (c is TextBoxItem)
		{
			TextBoxItem textBoxItem = c as TextBoxItem;
			textBoxItem.MouseHover += ComponentMouseHover;
			textBoxItem.MouseLeave += ComponentMouseLeave;
			textBoxItem.MouseEnter += ComponentMouseEnter;
			textBoxItem.MouseDown += new MouseEventHandler(ComponentMouseDown);
			((Control)textBoxItem.TextBox).MouseHover += ComponentMouseHover;
			((Control)textBoxItem.TextBox).MouseLeave += ComponentMouseLeave;
			((Control)textBoxItem.TextBox).MouseEnter += ComponentMouseEnter;
			((Control)textBoxItem.TextBox).MouseDown += new MouseEventHandler(ComponentMouseDown);
		}
		else if (c is BaseItem)
		{
			BaseItem baseItem = c as BaseItem;
			baseItem.MouseHover += ComponentMouseHover;
			baseItem.MouseLeave += ComponentMouseLeave;
			baseItem.MouseEnter += ComponentMouseEnter;
			baseItem.MouseDown += new MouseEventHandler(ComponentMouseDown);
		}
		else if (c is RibbonBar)
		{
			RibbonBar ribbonBar = c as RibbonBar;
			ribbonBar.DialogLauncherMouseHover += ComponentMouseHover;
			ribbonBar.DialogLauncherMouseEnter += ComponentMouseEnter;
			ribbonBar.DialogLauncherMouseLeave += ComponentMouseLeave;
			ribbonBar.DialogLauncherMouseDown += new MouseEventHandler(ComponentMouseDown);
		}
		else if (c is Control)
		{
			Control val = (Control)((c is Control) ? c : null);
			val.MouseHover += ComponentMouseHover;
			val.MouseLeave += ComponentMouseLeave;
			val.MouseDown += new MouseEventHandler(ComponentMouseDown);
			val.MouseEnter += ComponentMouseEnter;
		}
		else if (c is ToolStripButton)
		{
			ToolStripButton val2 = (ToolStripButton)((c is ToolStripButton) ? c : null);
			((ToolStripItem)val2).MouseHover += ComponentMouseHover;
			((ToolStripItem)val2).MouseLeave += ComponentMouseLeave;
			((ToolStripItem)val2).MouseEnter += ComponentMouseEnter;
			((ToolStripItem)val2).MouseDown += new MouseEventHandler(ComponentMouseDown);
			val2.AutoToolTip = false;
			((ToolStripItem)val2).ToolTipText = "";
		}
		else if (c is Node || IsTreeGXType(c.GetType()))
		{
			Type type = c.GetType();
			EventInfo @event = type.GetEvent("NodeMouseHover");
			if ((object)@event != null)
			{
				Delegate handler = Delegate.CreateDelegate(typeof(EventHandler), this, "ComponentMouseHover");
				@event.AddEventHandler(c, handler);
			}
			@event = type.GetEvent("NodeMouseLeave");
			if ((object)@event != null)
			{
				Delegate handler2 = Delegate.CreateDelegate(typeof(EventHandler), this, "ComponentMouseLeave");
				@event.AddEventHandler(c, handler2);
			}
			@event = type.GetEvent("NodeMouseEnter");
			if ((object)@event != null)
			{
				Delegate handler3 = Delegate.CreateDelegate(typeof(EventHandler), this, "ComponentMouseEnter");
				@event.AddEventHandler(c, handler3);
			}
			@event = type.GetEvent("NodeMouseDown");
			if ((object)@event != null)
			{
				Delegate handler4 = Delegate.CreateDelegate(typeof(MouseEventHandler), this, "ComponentMouseDown");
				@event.AddEventHandler(c, handler4);
			}
		}
		else if (c is TabItem)
		{
			TabItem tabItem = c as TabItem;
			tabItem.MouseHover += ComponentMouseHover;
			tabItem.MouseLeave += ComponentMouseLeave;
			tabItem.MouseEnter += ComponentMouseEnter;
			tabItem.MouseDown += new MouseEventHandler(ComponentMouseDown);
		}
	}

	private void ComponentHideTooltip(object sender, EventArgs e)
	{
		HideTooltip();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ComponentMouseDown(object sender, MouseEventArgs e)
	{
		HideTooltip();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ComponentMouseEnter(object sender, EventArgs e)
	{
		if (sender is Control)
		{
			ResetHover((Control)((sender is Control) ? sender : null));
		}
		if (_2E11 && ݕ)
		{
			ShowTooltip(sender);
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ComponentMouseLeave(object sender, EventArgs e)
	{
		if (!IsMouseOverSuperTooltip)
		{
			HideDelayed();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ComponentMouseHover(object sender, EventArgs e)
	{
		if (_2E11)
		{
			return;
		}
		_2E0D++;
		if (sender is Control && _2E0D < _2E0E)
		{
			ResetHover((Control)((sender is Control) ? sender : null));
		}
		else if (sender is BaseItem && _2E0D < _2E0E)
		{
			Control containerControl = GetContainerControl((BaseItem)sender);
			if (containerControl != null)
			{
				ResetHover(containerControl);
			}
		}
		else if (sender is TabItem && _2E0D < _2E0E)
		{
			ResetHover((Control)(object)((TabItem)sender).Parent);
		}
		else if (sender is Node && _2E0D < _2E0E)
		{
			ResetHover((Control)(object)((Node)sender).TreeControl);
		}
		else if (ݕ)
		{
			ShowTooltip(sender);
		}
	}

	private Control GetContainerControl(BaseItem item)
	{
		object containerControl = item.ContainerControl;
		if (containerControl is BaseItem)
		{
			containerControl = ((BaseItem)containerControl).ContainerControl;
		}
		return (Control)((containerControl is Control) ? containerControl : null);
	}

	private ComboBoxItem GetComboBoxItemFromComboBoxEx(ComboBoxEx c)
	{
		foreach (object key in _2E04.Keys)
		{
			if (key is ComboBoxItem && ((ComboBoxItem)key).ComboBoxEx == c)
			{
				return key as ComboBoxItem;
			}
		}
		return null;
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (Ӟ != null)
		{
			Ӟ(this, e);
		}
	}

	public void ShowTooltip(object sender, Point screenPosition)
	{
		ShowTooltip(sender, screenPosition, useScreenPosition: true);
	}

	public void ShowTooltip(object sender)
	{
		ShowTooltip(sender, Point.Empty, useScreenPosition: false);
	}

	private void ShowTooltip(object sender, Point screenPosition, bool useScreenPosition)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Invalid comparison between Unknown and I4
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Invalid comparison between Unknown and I4
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Invalid comparison between Unknown and I4
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Invalid comparison between Unknown and I4
		if (sender is Control && ((Control)sender).Focused && !_2E15)
		{
			return;
		}
		Rectangle rectangle = Rectangle.Empty;
		Rectangle rect = Rectangle.Empty;
		bool flag = false;
		if (!IsFormActive(sender))
		{
			return;
		}
		if (sender is ComboBoxEx)
		{
			BaseItem comboBoxItemFromComboBoxEx = GetComboBoxItemFromComboBoxEx(sender as ComboBoxEx);
			if (comboBoxItemFromComboBoxEx != null)
			{
				sender = comboBoxItemFromComboBoxEx;
			}
		}
		if (sender is BaseItem)
		{
			BaseItem baseItem = sender as BaseItem;
			Control containerControl = GetContainerControl(baseItem);
			if (baseItem is PopupItem && ((PopupItem)baseItem).Expanded && (containerControl is RibbonBar || containerControl is RibbonStrip))
			{
				return;
			}
			Control val = containerControl;
			if (val != null)
			{
				rectangle = ((!_2E03 || (val is RibbonStrip && ((RibbonStrip)(object)val).QuickToolbarItems.Contains(baseItem)) || !(val is RibbonBar)) ? new Rectangle(val.PointToScreen(baseItem.DisplayRectangle.Location), baseItem.DisplayRectangle.Size) : new Rectangle(val.PointToScreen(Point.Empty), val.Size));
				rect = new Rectangle(val.PointToScreen(baseItem.DisplayRectangle.Location), baseItem.DisplayRectangle.Size);
				flag = (int)containerControl.RightToLeft == 1;
			}
		}
		else if (sender is Control)
		{
			Control val2 = (Control)((sender is Control) ? sender : null);
			rectangle = new Rectangle(val2.PointToScreen(Point.Empty), val2.Size);
			rect = rectangle;
			flag = (int)val2.RightToLeft == 1;
		}
		else if (sender is TabItem)
		{
			TabItem tabItem = sender as TabItem;
			TabStrip parent = tabItem.Parent;
			rectangle = new Rectangle(((Control)parent).PointToScreen(Point.Empty), ((Control)parent).Size);
			rect = rectangle;
			flag = (int)((Control)parent).RightToLeft == 1;
		}
		else if (sender is ISuperTooltipInfoProvider)
		{
			ISuperTooltipInfoProvider superTooltipInfoProvider = sender as ISuperTooltipInfoProvider;
			rectangle = superTooltipInfoProvider.ComponentRectangle;
			rect = rectangle;
		}
		else if (sender is Node || IsTreeGXType(sender.GetType()))
		{
			rectangle = (Rectangle)TypeDescriptor.GetProperties(sender)["CellsBounds"].GetValue(sender);
			object? value = TypeDescriptor.GetProperties(sender)["TreeControl"].GetValue(sender);
			Control val3 = (Control)((value is Control) ? value : null);
			if (val3 != null)
			{
				rectangle = ((!_2E03) ? new Rectangle(val3.PointToScreen(rectangle.Location), rectangle.Size) : new Rectangle(val3.PointToScreen(Point.Empty), val3.Size));
				flag = (int)val3.RightToLeft == 1;
			}
			rect = rectangle;
		}
		else if (sender is ToolStripButton)
		{
			ToolStripButton val4 = (ToolStripButton)((sender is ToolStripButton) ? sender : null);
			rectangle = ((ToolStripItem)val4).Bounds;
			rect = ((ToolStripItem)val4).Bounds;
		}
		if (rectangle.IsEmpty)
		{
			return;
		}
		SuperTooltipInfo superTooltipInfo = GetSuperTooltipInfo(sender);
		if (superTooltipInfo == null && sender is BaseItem)
		{
			foreach (object key in _2E04.Keys)
			{
				if (key is BaseItem && ((BaseItem)key).Name == ((BaseItem)sender).Name)
				{
					superTooltipInfo = _2E04[key] as SuperTooltipInfo;
					break;
				}
			}
		}
		if (superTooltipInfo == null)
		{
			return;
		}
		HideTooltip();
		Point point = new Point(Control.MousePosition.X, Control.MousePosition.Y + (int)((float)SystemInformation.CursorSize.Height * 0.6f));
		if (_2E03)
		{
			point.Y = Math.Max(rectangle.Bottom + 1, point.Y);
		}
		if (useScreenPosition)
		{
			point = screenPosition;
		}
		ᕵ = new SuperTooltipControl();
		ᕵ.ShowTooltipDescription = _2E14;
		ᕵ.MaximumWidth = ߐ;
		ᕵ.MouseActivateEnabled = false;
		((Control)ᕵ).MouseEnter += SuperTooltip_MouseEnter;
		((Control)ᕵ).MouseLeave += SuperTooltip_MouseLeave;
		ᕵ.MarkupLinkClick += Tooltip_MarkupLinkClick;
		if (_2E0C != null)
		{
			((Control)ᕵ).Font = _2E0C;
		}
		if (flag)
		{
			((Control)ᕵ).RightToLeft = (RightToLeft)1;
		}
		ᕵ.MinimumTooltipSize = _2E0F;
		ᕵ.AntiAlias = ե;
		Size size = Size.Empty;
		if (_2E13 || _2E06)
		{
			if (!superTooltipInfo.CustomSize.IsEmpty && superTooltipInfo.CustomSize.Width > 0 && superTooltipInfo.CustomSize.Height > 0)
			{
				size = superTooltipInfo.CustomSize;
			}
			else
			{
				ᕵ.UpdateWithSuperTooltipInfo(superTooltipInfo);
				ᕵ._2E26(superTooltipInfo);
				size = ((Control)ᕵ).Size;
			}
		}
		if (_2E06)
		{
			ঊ ঊ2 = BarFunctions.ॹ(point);
			if (ঊ2 != null)
			{
				Rectangle rectangle2 = new Rectangle(point, size);
				_ = ঊ2.ঌ.Size;
				if (rectangle2.Right > ঊ2.ঌ.Right)
				{
					rectangle2.X -= rectangle2.Right - ঊ2.ঌ.Right;
				}
				if (rectangle2.Bottom > ঊ2.ঋ.Bottom)
				{
					rectangle2.Y = ঊ2.ঋ.Bottom - rectangle2.Height;
				}
				point = rectangle2.Location;
			}
		}
		if (_2E13 && !rect.IsEmpty)
		{
			Rectangle rectangle3 = new Rectangle(point, size);
			if (rectangle3.IntersectsWith(rect))
			{
				if (rect.Y - rectangle3.Height >= 0)
				{
					rectangle3.Y = rect.Y - rectangle3.Height;
				}
				else if (rect.X - rectangle3.Width >= 0)
				{
					rectangle3.Width = rect.X - rectangle3.Width;
				}
				point = rectangle3.Location;
			}
		}
		if (_2E01 != null)
		{
			SuperTooltipEventArgs superTooltipEventArgs = new SuperTooltipEventArgs(sender, superTooltipInfo, point);
			superTooltipEventArgs.TooltipSize = size;
			_2E01(this, superTooltipEventArgs);
			if (superTooltipEventArgs.Cancel)
			{
				((Component)(object)ᕵ).Dispose();
				ᕵ = null;
				return;
			}
			point = superTooltipEventArgs.Location;
		}
		ᕵ.ShowTooltip(superTooltipInfo, point.X, point.Y, enforceScreenPosition: false);
		_2E09 = 0L;
		CreateActiveWindowTimer();
	}

	protected virtual void OnTooltipClosed(EventArgs e)
	{
		if (_2E02 != null)
		{
			_2E02(this, e);
		}
	}

	private SuperTooltipInfo GetSuperTooltipInfo(object sender)
	{
		if (sender is TextBoxX && !_2E04.Contains(sender))
		{
			foreach (object key in _2E04.Keys)
			{
				if (key is TextBoxItem && ((TextBoxItem)key).TextBox == sender)
				{
					return _2E04[key] as SuperTooltipInfo;
				}
			}
		}
		else if (sender is ComboBoxEx && !_2E04.Contains(sender))
		{
			foreach (object key2 in _2E04.Keys)
			{
				if (key2 is ComboBoxItem && ((ComboBoxItem)key2).ComboBoxEx == sender)
				{
					return _2E04[key2] as SuperTooltipInfo;
				}
			}
		}
		return _2E04[sender] as SuperTooltipInfo;
	}

	private bool IsFormActive(object sender)
	{
		if (_2E12)
		{
			return true;
		}
		Control val = null;
		if (sender is Control)
		{
			val = (Control)((sender is Control) ? sender : null);
		}
		else if (sender is BaseItem)
		{
			val = GetContainerControl((BaseItem)sender);
		}
		if (val != null)
		{
			Form val2 = val.FindForm();
			if (val2 != null && val2.IsMdiChild && val2.MdiParent.ActiveMdiChild == val2)
			{
				return true;
			}
			if (val2 != null && Form.ActiveForm != val2)
			{
				return false;
			}
		}
		return true;
	}

	private void Tooltip_MarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
	{
		OnMarkupLinkClick(e);
	}

	private void SuperTooltip_MouseLeave(object sender, EventArgs e)
	{
		_2E16 = false;
		HideTooltip();
	}

	private void SuperTooltip_MouseEnter(object sender, EventArgs e)
	{
		_2E16 = true;
	}

	public void HideTooltip()
	{
		_2E0D = 0;
		DestroyActiveWindowTimer();
		DisposeHideDelayedTimer();
		if (ᕵ != null)
		{
			((Control)ᕵ).Hide();
			((Component)(object)ᕵ).Dispose();
			ᕵ = null;
			OnTooltipClosed(new EventArgs());
		}
		_2E16 = false;
	}

	private void HideDelayed()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		if (_2E10 <= 100)
		{
			HideTooltip();
		}
		else if (_2E05 == null)
		{
			_2E05 = new Timer();
			_2E05.Tick += HideDelayedTimer_Tick;
			_2E05.Interval = _2E10;
			_2E05.Enabled = true;
		}
	}

	private void HideDelayedTimer_Tick(object sender, EventArgs e)
	{
		if (!IsMouseOverSuperTooltip)
		{
			HideTooltip();
		}
	}

	private void DisposeHideDelayedTimer()
	{
		if (_2E05 != null)
		{
			_2E05.Stop();
			_2E05.Enabled = false;
			((Component)(object)_2E05).Dispose();
			_2E05 = null;
		}
	}

	private void CreateActiveWindowTimer()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		if (_0A04 != null || _2E17)
		{
			return;
		}
		_2E17 = true;
		try
		{
			_2E07 = _089F.GetActiveWindow();
			_0A04 = new Timer();
			_0A04.Interval = 300;
			_0A04.Tick += ActiveWindowTimerTick;
			_0A04.Start();
		}
		finally
		{
			_2E17 = false;
		}
	}

	private void ActiveWindowTimerTick(object sender, EventArgs e)
	{
		_2E09 += _0A04.Interval;
		if (_2E07 != IntPtr.Zero)
		{
			IntPtr activeWindow = _089F.GetActiveWindow();
			if (activeWindow != _2E07)
			{
				_0A04.Stop();
				HideTooltip();
				return;
			}
		}
		if (_2E08 > 0 && _2E09 > _2E08 * 1000 && !_2E16)
		{
			_0A04.Stop();
			HideTooltip();
		}
	}

	private void DestroyActiveWindowTimer()
	{
		if (_2E17)
		{
			return;
		}
		_2E17 = true;
		try
		{
			if (_0A04 != null)
			{
				_0A04.Enabled = false;
				_0A04.Stop();
				_0A04.Tick -= ActiveWindowTimerTick;
				((Component)(object)_0A04).Dispose();
				_0A04 = null;
			}
		}
		finally
		{
			_2E17 = false;
		}
	}

	private void ResetHover(Control c)
	{
		if (c != null && BarFunctions.IsHandleValid(c))
		{
			_089F.TRACKMOUSEEVENT tme = default(_089F.TRACKMOUSEEVENT);
			tme.dwFlags = 1073741824u;
			tme.hwndTrack = c.Handle;
			tme.cbSize = Marshal.SizeOf((object)tme);
			_089F.TrackMouseEvent(ref tme);
			tme.dwFlags |= 1u;
			_089F.TrackMouseEvent(ref tme);
		}
	}

	private bool IsTreeGXType(Type type)
	{
		if (!(type.BaseType.FullName == "DevComponents.Tree.Node"))
		{
			return type.FullName == "DevComponents.Tree.Node";
		}
		return true;
	}

	public bool CanExtend(object extendee)
	{
		if (extendee is Control || extendee is BaseItem || extendee is ISuperTooltipInfoProvider || IsTreeGXType(extendee.GetType()) || extendee is TabItem)
		{
			return true;
		}
		if (extendee is ToolStripButton)
		{
			return true;
		}
		if (extendee is Node)
		{
			return true;
		}
		return false;
	}
}
