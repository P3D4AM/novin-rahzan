using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using DevComponents.AdvTree;

namespace DevComponents.DotNetBar.Controls;

[ToolboxBitmap(typeof(Flyout), "Controls.Flyout.ico")]
[Designer("DevComponents.DotNetBar.Design.FlyoutDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[DefaultEvent("PrepareContent")]
[ToolboxItem(true)]
public class Flyout : Component, ሎ
{
	private IContainer Պ;

	private bool __2B89;

	private bool __2B80;

	private Control __2781;

	private ePointerSide __2B8A = ePointerSide.Bottom;

	private Control __2B8B;

	private bool __2B8C;

	private bool __2B8D = true;

	private eFlyoutDisplayMode __2B8E = eFlyoutDisplayMode.MouseOver;

	private eFlyoutCloseMode __2B8F = eFlyoutCloseMode.ClickOutside | eFlyoutCloseMode.ParentFormDeactivate;

	private EventHandler _2B90;

	private FlyoutShowingEventHandler _2B91;

	private EventHandler _2B92;

	private CancelEventHandler _2B93;

	private bool __2B94;

	private Control __2B95;

	private WeakReference __2B96;

	private FlyoutForm __2B97;

	private FormClosingEventHandler _2B98;

	private FormClosedEventHandler _2B99;

	private Form __1C3F;

	private bool __2B9A;

	private Color __0CD2 = Color.Empty;

	private Color _Ꮃ = Color.Empty;

	private Control _ᗚ;

	private bool __2B9B;

	[DefaultValue(null)]
	[Description("Indicates a control, usually panel with other controls inside of it, that is displayed on the flyout popup.")]
	[Category("Appearance")]
	public Control Content
	{
		get
		{
			return __2781;
		}
		set
		{
			if (value != __2781)
			{
				Control oldValue = __2781;
				__2781 = value;
				OnContentChanged(oldValue, value);
			}
		}
	}

	[DefaultValue(ePointerSide.Bottom)]
	[Category("Appearance")]
	[Description("Indicates the side of the flyout triangle pointer is displayed on.")]
	public ePointerSide PointerSide
	{
		get
		{
			return __2B8A;
		}
		set
		{
			if (value != __2B8A)
			{
				ePointerSide oldValue = __2B8A;
				__2B8A = value;
				OnPointerSideChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates the target control for the flyout display and positioning.")]
	[Category("Behavior")]
	[DefaultValue(null)]
	public Control TargetControl
	{
		get
		{
			return __2B8B;
		}
		set
		{
			if (value != __2B8B)
			{
				Control oldValue = __2B8B;
				__2B8B = value;
				OnTargetControlChanged(oldValue, value);
			}
		}
	}

	[Description("Indicates whether Flyout integrates on item level with DotNetBar controls it recognizes like SuperTabControl, AdvTree etc.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool DeepIntegration
	{
		get
		{
			return __2B8D;
		}
		set
		{
			if (value != __2B8D)
			{
				bool oldValue = __2B8D;
				__2B8D = value;
				OnDeepControlIntegrationChanged(oldValue, value);
			}
		}
	}

	[Category("Behavior")]
	[Description("Specifies when the flyout is displayed.")]
	[DefaultValue(eFlyoutDisplayMode.MouseOver)]
	public eFlyoutDisplayMode DisplayMode
	{
		get
		{
			return __2B8E;
		}
		set
		{
			if (value != __2B8E)
			{
				eFlyoutDisplayMode oldValue = __2B8E;
				__2B8E = value;
				OnDisplayModeChanged(oldValue, value);
			}
		}
	}

	[Category("Behavior")]
	[Description("Indicates when Flyout is automatically closed.")]
	[DefaultValue(eFlyoutCloseMode.ClickOutside | eFlyoutCloseMode.ParentFormDeactivate)]
	public eFlyoutCloseMode CloseMode
	{
		get
		{
			return __2B8F;
		}
		set
		{
			if (value != __2B8F)
			{
				eFlyoutCloseMode oldValue = __2B8F;
				__2B8F = value;
				OnCloseModeChanged(oldValue, value);
			}
		}
	}

	[Browsable(false)]
	public FlyoutForm FlyoutForm => __2B97;

	[Description("Indicates whether flyout is active/focused when its shown, default value is false.")]
	[DefaultValue(false)]
	[Category("Behavior")]
	public bool ActivateOnShow
	{
		get
		{
			return __2B9A;
		}
		set
		{
			if (value != __2B9A)
			{
				bool oldValue = __2B9A;
				__2B9A = value;
				OnActivateOnShowChanged(oldValue, value);
			}
		}
	}

	[DefaultValue(false)]
	[Description("Indicates whether flyout is made top-most window when shown.")]
	[Category("Behavior")]
	public bool TopMost
	{
		get
		{
			return __2B80;
		}
		set
		{
			if (value != __2B80)
			{
				bool oldValue = __2B80;
				__2B80 = value;
				OnTopMostChanged(oldValue, value);
			}
		}
	}

	[Category("Columns")]
	[Description("Indicates background flyout color. Default value is Color.Empty which indicates that current color scheme will be used.")]
	public Color BackColor
	{
		get
		{
			return __0CD2;
		}
		set
		{
			__0CD2 = value;
		}
	}

	[Description("Indicates flyout border color. Default value of Color.Empty indicates that color scheme will be used.")]
	[Category("Columns")]
	public Color BorderColor
	{
		get
		{
			return _Ꮃ;
		}
		set
		{
			_Ꮃ = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue(true)]
	[Description("Indicates whether flyout displays drop shadow")]
	public bool DropShadow
	{
		get
		{
			return __2B89;
		}
		set
		{
			if (value != __2B89)
			{
				bool oldValue = __2B89;
				__2B89 = value;
				OnDropShadowChanged(oldValue, value);
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public Control Parent
	{
		get
		{
			return _ᗚ;
		}
		set
		{
			if (value != _ᗚ)
			{
				Control ᗚ = _ᗚ;
				_ᗚ = value;
				OnParentChanged(ᗚ, value);
			}
		}
	}

	bool ሎ.IsModal => false;

	[Description("Occurs before the flyout is shown for specific target and allows you to prepare Content for it. Sender of event will be the targeted control or item.")]
	public event EventHandler PrepareContent
	{
		add
		{
			EventHandler eventHandler = _2B90;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2B90, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2B90;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2B90, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs before the flyout form is shown and allows you to cancel the showing.")]
	public event FlyoutShowingEventHandler FlyoutShowing
	{
		add
		{
			FlyoutShowingEventHandler flyoutShowingEventHandler = _2B91;
			FlyoutShowingEventHandler flyoutShowingEventHandler2;
			do
			{
				flyoutShowingEventHandler2 = flyoutShowingEventHandler;
				FlyoutShowingEventHandler value2 = (FlyoutShowingEventHandler)Delegate.Combine(flyoutShowingEventHandler2, value);
				flyoutShowingEventHandler = Interlocked.CompareExchange(ref _2B91, value2, flyoutShowingEventHandler2);
			}
			while ((object)flyoutShowingEventHandler != flyoutShowingEventHandler2);
		}
		remove
		{
			FlyoutShowingEventHandler flyoutShowingEventHandler = _2B91;
			FlyoutShowingEventHandler flyoutShowingEventHandler2;
			do
			{
				flyoutShowingEventHandler2 = flyoutShowingEventHandler;
				FlyoutShowingEventHandler value2 = (FlyoutShowingEventHandler)Delegate.Remove(flyoutShowingEventHandler2, value);
				flyoutShowingEventHandler = Interlocked.CompareExchange(ref _2B91, value2, flyoutShowingEventHandler2);
			}
			while ((object)flyoutShowingEventHandler != flyoutShowingEventHandler2);
		}
	}

	[Description("Occurs after flyout has been shown.")]
	public event EventHandler FlyoutShown
	{
		add
		{
			EventHandler eventHandler = _2B92;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2B92, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2B92;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2B92, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Provides opportunity to cancel showing of the flyout before any objects are created and allocated. This is preferred event to cancel flyout showing.")]
	public event CancelEventHandler QueryShowFlyout
	{
		add
		{
			CancelEventHandler cancelEventHandler = _2B93;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Combine(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _2B93, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
		remove
		{
			CancelEventHandler cancelEventHandler = _2B93;
			CancelEventHandler cancelEventHandler2;
			do
			{
				cancelEventHandler2 = cancelEventHandler;
				CancelEventHandler value2 = (CancelEventHandler)Delegate.Remove(cancelEventHandler2, value);
				cancelEventHandler = Interlocked.CompareExchange(ref _2B93, value2, cancelEventHandler2);
			}
			while ((object)cancelEventHandler != cancelEventHandler2);
		}
	}

	[Description("Occurs before flyout is closed and allows you to cancel the closing.")]
	public event FormClosingEventHandler FlyoutClosing
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			FormClosingEventHandler val = _2B98;
			FormClosingEventHandler val2;
			do
			{
				val2 = val;
				FormClosingEventHandler value2 = (FormClosingEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _2B98, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			FormClosingEventHandler val = _2B98;
			FormClosingEventHandler val2;
			do
			{
				val2 = val;
				FormClosingEventHandler value2 = (FormClosingEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _2B98, value2, val2);
			}
			while (val != val2);
		}
	}

	[Description("Occurs after flyout is closed.")]
	public event FormClosedEventHandler FlyoutClosed
	{
		add
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			FormClosedEventHandler val = _2B99;
			FormClosedEventHandler val2;
			do
			{
				val2 = val;
				FormClosedEventHandler value2 = (FormClosedEventHandler)Delegate.Combine((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _2B99, value2, val2);
			}
			while (val != val2);
		}
		remove
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			FormClosedEventHandler val = _2B99;
			FormClosedEventHandler val2;
			do
			{
				val2 = val;
				FormClosedEventHandler value2 = (FormClosedEventHandler)Delegate.Remove((Delegate?)(object)val2, (Delegate?)(object)value);
				val = Interlocked.CompareExchange(ref _2B99, value2, val2);
			}
			while (val != val2);
		}
	}

	public Flyout()
	{
		InitializeComponent();
	}

	public Flyout(IContainer container)
	{
		container.Add(this);
		InitializeComponent();
	}

	protected override void Dispose(bool disposing)
	{
		Close();
		if (__2B9B)
		{
			_1BE6._1BEB(this);
			__2B9B = false;
		}
		if (disposing && Պ != null)
		{
			Պ.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		Պ = new Container();
	}

	protected virtual void OnContentChanged(Control oldValue, Control newValue)
	{
	}

	protected virtual void OnPointerSideChanged(ePointerSide oldValue, ePointerSide newValue)
	{
	}

	protected virtual void OnTargetControlChanged(Control oldValue, Control newValue)
	{
		if (!base.DesignMode)
		{
			DetachFromControl(oldValue, __2B8E, __2B8D);
			AttachToControl(newValue, __2B8E, __2B8D);
		}
	}

	private void AttachToControl(Control c, eFlyoutDisplayMode displayMode, bool deepIntegration)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		if (c == null)
		{
			return;
		}
		switch (displayMode)
		{
		case eFlyoutDisplayMode.MouseClick:
			if (deepIntegration && c is SuperTabControl)
			{
				SuperTabControl superTabControl3 = (SuperTabControl)(object)c;
				superTabControl3.TabStrip.ItemClick += TabStripMouseEvent;
			}
			else if (deepIntegration && c is DevComponents.AdvTree.AdvTree)
			{
				DevComponents.AdvTree.AdvTree advTree3 = (DevComponents.AdvTree.AdvTree)(object)c;
				advTree3.NodeClick += TreeMouseEvent;
			}
			else if (deepIntegration && c is TokenEditor)
			{
				TokenEditor tokenEditor3 = (TokenEditor)(object)c;
				tokenEditor3.TokenMouseClick += new MouseEventHandler(TokenMouseEvent);
			}
			else if (deepIntegration && c is TabStrip)
			{
				TabStrip tabStrip3 = (TabStrip)(object)c;
				tabStrip3.TabMouseClick += new MouseEventHandler(TabItemMouseEvent);
			}
			else if (deepIntegration && c is TabControl)
			{
				TabControl tabControl3 = (TabControl)(object)c;
				tabControl3.TabStrip.TabMouseClick += new MouseEventHandler(TabItemMouseEvent);
			}
			else if (deepIntegration && c is Bar)
			{
				Bar bar3 = (Bar)(object)c;
				bar3.ItemClick += BaseItemMouseEvent;
			}
			else
			{
				c.MouseClick += new MouseEventHandler(TargetMouseClick);
			}
			break;
		case eFlyoutDisplayMode.MouseHover:
			if (deepIntegration && c is SuperTabControl)
			{
				SuperTabControl superTabControl2 = (SuperTabControl)(object)c;
				superTabControl2.TabStrip.MouseHover += TabStripMouseEvent;
				superTabControl2.TabStrip.MouseLeave += TargetMouseLeave;
			}
			else if (deepIntegration && c is DevComponents.AdvTree.AdvTree)
			{
				DevComponents.AdvTree.AdvTree advTree2 = (DevComponents.AdvTree.AdvTree)(object)c;
				advTree2.NodeMouseHover += TreeMouseEvent;
				advTree2.NodeMouseLeave += TargetMouseLeave;
			}
			else if (deepIntegration && c is TokenEditor)
			{
				TokenEditor tokenEditor2 = (TokenEditor)(object)c;
				tokenEditor2.TokenMouseHover += TokenMouseEvent;
				tokenEditor2.TokenMouseLeave += TargetMouseLeave;
			}
			else if (deepIntegration && c is TabStrip)
			{
				TabStrip tabStrip2 = (TabStrip)(object)c;
				tabStrip2.TabMouseHover += TabItemMouseEvent;
				tabStrip2.TabMouseLeave += TargetMouseLeave;
			}
			else if (deepIntegration && c is TabControl)
			{
				TabControl tabControl2 = (TabControl)(object)c;
				tabControl2.TabStrip.TabMouseHover += TabItemMouseEvent;
				tabControl2.TabStrip.TabMouseLeave += TargetMouseLeave;
			}
			else if (deepIntegration && c is Bar)
			{
				Bar bar2 = (Bar)(object)c;
				((Control)bar2).MouseHover += BaseItemMouseEvent;
				((Control)bar2).MouseLeave += TargetMouseLeave;
			}
			else
			{
				c.MouseHover += TargetMouseHover;
			}
			break;
		case eFlyoutDisplayMode.MouseOver:
			if (deepIntegration && c is SuperTabControl)
			{
				SuperTabControl superTabControl = (SuperTabControl)(object)c;
				superTabControl.TabStrip.MouseEnter += TabStripMouseEvent;
				superTabControl.TabStrip.MouseLeave += TargetMouseLeave;
			}
			else if (deepIntegration && c is DevComponents.AdvTree.AdvTree)
			{
				DevComponents.AdvTree.AdvTree advTree = (DevComponents.AdvTree.AdvTree)(object)c;
				advTree.NodeMouseEnter += TreeMouseEvent;
				advTree.NodeMouseLeave += TargetMouseLeave;
			}
			else if (deepIntegration && c is TokenEditor)
			{
				TokenEditor tokenEditor = (TokenEditor)(object)c;
				tokenEditor.TokenMouseEnter += TokenMouseEvent;
				tokenEditor.TokenMouseLeave += TargetMouseLeave;
			}
			else if (deepIntegration && c is TabStrip)
			{
				TabStrip tabStrip = (TabStrip)(object)c;
				tabStrip.TabMouseEnter += TabItemMouseEvent;
				tabStrip.TabMouseLeave += TargetMouseLeave;
			}
			else if (deepIntegration && c is TabControl)
			{
				TabControl tabControl = (TabControl)(object)c;
				tabControl.TabStrip.TabMouseEnter += TabItemMouseEvent;
				tabControl.TabStrip.TabMouseLeave += TargetMouseLeave;
			}
			else if (deepIntegration && c is Bar)
			{
				Bar bar = (Bar)(object)c;
				((Control)bar).MouseMove += new MouseEventHandler(BaseItemMouseEvent);
				((Control)bar).MouseLeave += TargetMouseLeave;
			}
			else
			{
				c.MouseEnter += TargetMouseEnter;
			}
			break;
		}
		c.Leave += TargetLeaveFocus;
		c.VisibleChanged += TargetVisibleChanged;
	}

	private void DetachFromControl(Control c, eFlyoutDisplayMode displayMode, bool deepIntegration)
	{
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		if (c == null)
		{
			return;
		}
		switch (displayMode)
		{
		case eFlyoutDisplayMode.MouseClick:
			if (deepIntegration && c is SuperTabControl)
			{
				SuperTabControl superTabControl3 = (SuperTabControl)(object)c;
				superTabControl3.TabStrip.ItemClick -= TabStripMouseEvent;
			}
			else if (deepIntegration && c is DevComponents.AdvTree.AdvTree)
			{
				DevComponents.AdvTree.AdvTree advTree3 = (DevComponents.AdvTree.AdvTree)(object)c;
				advTree3.NodeClick -= TreeMouseEvent;
			}
			else if (deepIntegration && c is TokenEditor)
			{
				TokenEditor tokenEditor3 = (TokenEditor)(object)c;
				tokenEditor3.TokenMouseClick -= new MouseEventHandler(TokenMouseEvent);
			}
			else if (deepIntegration && c is TabStrip)
			{
				TabStrip tabStrip3 = (TabStrip)(object)c;
				tabStrip3.TabMouseClick -= new MouseEventHandler(TabItemMouseEvent);
			}
			else if (deepIntegration && c is TabControl)
			{
				TabControl tabControl3 = (TabControl)(object)c;
				tabControl3.TabStrip.TabMouseClick -= new MouseEventHandler(TabItemMouseEvent);
			}
			else if (deepIntegration && c is Bar)
			{
				Bar bar3 = (Bar)(object)c;
				bar3.ItemClick -= BaseItemMouseEvent;
			}
			else
			{
				c.MouseClick -= new MouseEventHandler(TargetMouseClick);
			}
			break;
		case eFlyoutDisplayMode.MouseHover:
			if (deepIntegration && c is SuperTabControl)
			{
				SuperTabControl superTabControl2 = (SuperTabControl)(object)c;
				superTabControl2.TabStrip.MouseHover -= TabStripMouseEvent;
				superTabControl2.TabStrip.MouseLeave -= TargetMouseLeave;
			}
			else if (deepIntegration && c is DevComponents.AdvTree.AdvTree)
			{
				DevComponents.AdvTree.AdvTree advTree2 = (DevComponents.AdvTree.AdvTree)(object)c;
				advTree2.NodeMouseHover -= TreeMouseEvent;
				advTree2.NodeMouseLeave -= TargetMouseLeave;
			}
			else if (deepIntegration && c is TokenEditor)
			{
				TokenEditor tokenEditor2 = (TokenEditor)(object)c;
				tokenEditor2.TokenMouseHover -= TokenMouseEvent;
				tokenEditor2.TokenMouseLeave -= TargetMouseLeave;
			}
			else if (deepIntegration && c is TabStrip)
			{
				TabStrip tabStrip2 = (TabStrip)(object)c;
				tabStrip2.TabMouseHover -= TabItemMouseEvent;
				tabStrip2.TabMouseLeave -= TargetMouseLeave;
			}
			else if (deepIntegration && c is TabControl)
			{
				TabControl tabControl2 = (TabControl)(object)c;
				tabControl2.TabStrip.TabMouseHover -= TabItemMouseEvent;
				tabControl2.TabStrip.TabMouseLeave -= TargetMouseLeave;
			}
			else if (deepIntegration && c is Bar)
			{
				Bar bar2 = (Bar)(object)c;
				((Control)bar2).MouseHover -= BaseItemMouseEvent;
				((Control)bar2).MouseLeave -= TargetMouseLeave;
			}
			else
			{
				c.MouseHover -= TargetMouseHover;
			}
			break;
		case eFlyoutDisplayMode.MouseOver:
			if (deepIntegration && c is SuperTabControl)
			{
				SuperTabControl superTabControl = (SuperTabControl)(object)c;
				superTabControl.TabStrip.MouseEnter -= TabStripMouseEvent;
				superTabControl.TabStrip.MouseLeave -= TargetMouseLeave;
			}
			else if (deepIntegration && c is DevComponents.AdvTree.AdvTree)
			{
				DevComponents.AdvTree.AdvTree advTree = (DevComponents.AdvTree.AdvTree)(object)c;
				advTree.NodeMouseEnter -= TreeMouseEvent;
				advTree.NodeMouseLeave -= TargetMouseLeave;
			}
			else if (deepIntegration && c is TokenEditor)
			{
				TokenEditor tokenEditor = (TokenEditor)(object)c;
				tokenEditor.TokenMouseEnter -= TokenMouseEvent;
				tokenEditor.TokenMouseLeave -= TargetMouseLeave;
			}
			else if (deepIntegration && c is TabStrip)
			{
				TabStrip tabStrip = (TabStrip)(object)c;
				tabStrip.TabMouseEnter -= TabItemMouseEvent;
				tabStrip.TabMouseLeave -= TargetMouseLeave;
			}
			else if (deepIntegration && c is TabControl)
			{
				TabControl tabControl = (TabControl)(object)c;
				tabControl.TabStrip.TabMouseEnter -= TabItemMouseEvent;
				tabControl.TabStrip.TabMouseLeave -= TargetMouseLeave;
			}
			else if (deepIntegration && c is Bar)
			{
				Bar bar = (Bar)(object)c;
				((Control)bar).MouseEnter -= BaseItemMouseEvent;
				((Control)bar).MouseLeave -= TargetMouseLeave;
			}
			else
			{
				c.MouseEnter -= TargetMouseEnter;
			}
			break;
		}
		c.Leave -= TargetLeaveFocus;
		c.VisibleChanged -= TargetVisibleChanged;
	}

	private void TargetMouseLeave(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		MethodInvoker val = null;
		if (!__2B94)
		{
			return;
		}
		__2B8C = true;
		if (val == null)
		{
			val = (MethodInvoker)delegate
			{
				CloseFlyoutDelayed();
			};
		}
		BarUtilities.InvokeDelayed(val, 1000);
	}

	private void CloseFlyoutDelayed()
	{
		if (__2B8C)
		{
			__2B8C = false;
			if (__2B94 && __2B97 != null && !((Control)__2B97).Bounds.Contains(Control.MousePosition) && __2B8B != null && !new Rectangle(__2B8B.PointToScreen(__2B8B.Location), __2B8B.Size).Contains(Control.MousePosition))
			{
				Close();
			}
		}
	}

	private void TokenMouseEvent(object sender, EventArgs e)
	{
		if (__2B94)
		{
			Close();
		}
		Show(sender);
	}

	private void TreeMouseEvent(object sender, TreeNodeMouseEventArgs e)
	{
		if (__2B94)
		{
			Close();
		}
		Show(e.Node);
	}

	private void TabItemMouseEvent(object sender, EventArgs e)
	{
		if (sender is TabItem)
		{
			if (__2B94)
			{
				Close();
			}
			if (!__2B94)
			{
				Show(sender);
			}
		}
		else
		{
			Close();
		}
	}

	private void BaseItemMouseEvent(object sender, EventArgs e)
	{
		if (sender is BaseItem)
		{
			if (__2B94)
			{
				Close();
			}
			if (!__2B94)
			{
				Show(sender);
			}
		}
		else if (sender is Bar)
		{
			Bar bar = (Bar)sender;
			Point point = ((Control)bar).PointToClient(Control.MousePosition);
			BaseItem baseItem = bar.ItemsContainer.ItemAtLocation(point.X, point.Y);
			if (baseItem != null)
			{
				if (__2B96 == null || !__2B96.IsAlive || __2B96.Target != baseItem)
				{
					if (__2B94)
					{
						Close();
					}
					if (!__2B94)
					{
						Show(baseItem);
					}
				}
			}
			else
			{
				Close();
			}
		}
		else
		{
			Close();
		}
	}

	private void TabStripMouseEvent(object sender, EventArgs e)
	{
		if (sender is SuperTabItem)
		{
			if (__2B94)
			{
				Close();
			}
			if (!__2B94)
			{
				Show(sender);
			}
		}
		else
		{
			Close();
		}
	}

	protected virtual void OnDeepControlIntegrationChanged(bool oldValue, bool newValue)
	{
		DetachFromControl(__2B8B, __2B8E, oldValue);
		AttachToControl(__2B8B, __2B8E, newValue);
	}

	private void TargetVisibleChanged(object sender, EventArgs e)
	{
		if ((__2B8F & eFlyoutCloseMode.TargetControlHidden) == eFlyoutCloseMode.TargetControlHidden && __2B8B != null && !__2B8B.Visible)
		{
			Close();
		}
	}

	private void TargetLeaveFocus(object sender, EventArgs e)
	{
		if ((__2B8F & eFlyoutCloseMode.TargetControlLostFocus) == eFlyoutCloseMode.TargetControlLostFocus)
		{
			Close();
		}
	}

	private void TargetMouseEnter(object sender, EventArgs e)
	{
		if (!__2B94)
		{
			Show();
		}
	}

	private void TargetMouseHover(object sender, EventArgs e)
	{
		if (!__2B94)
		{
			Show();
		}
	}

	private void TargetMouseClick(object sender, MouseEventArgs e)
	{
		if (!__2B94)
		{
			Show();
		}
	}

	protected virtual void OnDisplayModeChanged(eFlyoutDisplayMode oldValue, eFlyoutDisplayMode newValue)
	{
		DetachFromControl(__2B8B, oldValue, __2B8D);
		AttachToControl(__2B8B, newValue, __2B8D);
	}

	protected virtual void OnCloseModeChanged(eFlyoutCloseMode oldValue, eFlyoutCloseMode newValue)
	{
	}

	private Rectangle GetTargetBounds(object targetItem)
	{
		if (targetItem is BaseItem)
		{
			BaseItem baseItem = (BaseItem)targetItem;
			return new Rectangle(__2B8B.PointToScreen(baseItem.Bounds.Location), baseItem.Bounds.Size);
		}
		if (targetItem is Node)
		{
			Node node = (Node)targetItem;
			return new Rectangle(__2B8B.PointToScreen(node.Bounds.Location), node.Bounds.Size);
		}
		if (targetItem is EditToken)
		{
			EditToken editToken = (EditToken)targetItem;
			return new Rectangle(__2B8B.PointToScreen(editToken.Bounds.Location), editToken.Bounds.Size);
		}
		if (targetItem is TabItem)
		{
			TabItem tabItem = (TabItem)targetItem;
			return new Rectangle(__2B8B.PointToScreen(tabItem.DisplayRectangle.Location), tabItem.DisplayRectangle.Size);
		}
		return new Rectangle(__2B8B.PointToScreen(Point.Empty), __2B8B.Size);
	}

	protected virtual void OnPrepareContent(object sender, EventArgs e)
	{
		_2B90?.Invoke(sender, e);
	}

	public virtual void Show(object targetItem)
	{
		OnPrepareContent(targetItem, EventArgs.Empty);
		Rectangle rectangle = default(Rectangle);
		ePointerSide ePointerSide2 = __2B8A;
		int pointerOffset = 10;
		if (__2B8B != null)
		{
			Rectangle targetBounds = GetTargetBounds(targetItem ?? __2B8B);
			ঊ ঊ = BarFunctions.ॻ(__2B8B);
			switch (ePointerSide2)
			{
			case ePointerSide.Top:
			{
				rectangle.Size = GetFlyoutFormSize();
				if (targetBounds.Bottom + rectangle.Height > ঊ.ঌ.Bottom)
				{
					ePointerSide2 = ePointerSide.Bottom;
					rectangle.Location = new Point(targetBounds.X, targetBounds.Y - rectangle.Height);
				}
				else
				{
					rectangle.Location = new Point(targetBounds.X, targetBounds.Bottom);
				}
				if (targetBounds.Width > rectangle.Width)
				{
					rectangle.X += (targetBounds.Width - rectangle.Width) / 2;
				}
				else if (targetBounds.Width < rectangle.Width)
				{
					rectangle.X -= (rectangle.Width - targetBounds.Width) / 2;
				}
				if (rectangle.X < ঊ.ঌ.X)
				{
					rectangle.X = ঊ.ঌ.X;
				}
				else if (rectangle.Right > ঊ.ঌ.Right)
				{
					rectangle.X = ঊ.ঌ.Right - rectangle.Width;
				}
				Rectangle rectangle2 = Rectangle.Intersect(rectangle, new Rectangle(targetBounds.X, rectangle.Y, targetBounds.Width, rectangle.Height));
				pointerOffset = ((!rectangle2.IsEmpty) ? (Math.Abs(rectangle2.X - rectangle.X) + (rectangle2.Width - FlyoutForm._2B9C.Width) / 2) : ((Math.Min(targetBounds.Width, rectangle.Width) - FlyoutForm._2B9C.Width) / 2));
				break;
			}
			case ePointerSide.Bottom:
			{
				rectangle.Size = GetFlyoutFormSize();
				if (targetBounds.Y - rectangle.Height < ঊ.ঌ.Y)
				{
					ePointerSide2 = ePointerSide.Top;
					rectangle.Location = new Point(targetBounds.X, targetBounds.Bottom);
				}
				else
				{
					rectangle.Location = new Point(targetBounds.X, targetBounds.Y - rectangle.Height);
				}
				if (targetBounds.Width > rectangle.Width)
				{
					rectangle.X += (targetBounds.Width - rectangle.Width) / 2;
				}
				else if (targetBounds.Width < rectangle.Width)
				{
					rectangle.X -= (rectangle.Width - targetBounds.Width) / 2;
				}
				if (rectangle.X < ঊ.ঌ.X)
				{
					rectangle.X = ঊ.ঌ.X;
				}
				else if (rectangle.Right > ঊ.ঌ.Right)
				{
					rectangle.X = ঊ.ঌ.Right - rectangle.Width;
				}
				Rectangle rectangle3 = Rectangle.Intersect(rectangle, new Rectangle(targetBounds.X, rectangle.Y, targetBounds.Width, rectangle.Height));
				pointerOffset = ((!rectangle3.IsEmpty) ? (Math.Abs(rectangle3.X - rectangle.X) + (rectangle3.Width - FlyoutForm._2B9C.Width) / 2) : ((Math.Min(targetBounds.Width, rectangle.Width) - FlyoutForm._2B9C.Width) / 2));
				break;
			}
			case ePointerSide.Left:
				rectangle.Size = GetFlyoutFormSize();
				if (targetBounds.Right + rectangle.Width > ঊ.ঌ.Right)
				{
					ePointerSide2 = ePointerSide.Right;
					rectangle.Location = new Point(targetBounds.X - rectangle.Width, targetBounds.Y);
				}
				else
				{
					rectangle.Location = new Point(targetBounds.Right, targetBounds.Y);
				}
				if (targetBounds.Height > rectangle.Height)
				{
					rectangle.Y += (targetBounds.Height - rectangle.Height) / 2;
				}
				pointerOffset = (Math.Min(targetBounds.Height, rectangle.Height) - FlyoutForm._2B9C.Width) / 2;
				break;
			case ePointerSide.Right:
				rectangle.Size = GetFlyoutFormSize();
				if (targetBounds.X - rectangle.Width < ঊ.ঌ.X)
				{
					ePointerSide2 = ePointerSide.Left;
					rectangle.Location = new Point(targetBounds.Right, targetBounds.Y);
				}
				else
				{
					rectangle.Location = new Point(targetBounds.X - rectangle.Width, targetBounds.Y);
				}
				if (targetBounds.Height > rectangle.Height)
				{
					rectangle.Y += (targetBounds.Height - rectangle.Height) / 2;
				}
				pointerOffset = (Math.Min(targetBounds.Height, rectangle.Height) - FlyoutForm._2B9C.Width) / 2;
				break;
			}
		}
		Show(rectangle, ePointerSide2, pointerOffset, targetItem);
	}

	public virtual void Show()
	{
		Show(__2B8B);
	}

	public virtual Size GetFlyoutFormSize()
	{
		if (__2781 != null)
		{
			Size size = __2781.Size;
			if (__2B8A == ePointerSide.Bottom || __2B8A == ePointerSide.Top)
			{
				size.Height += FlyoutForm._2B9C.Height + 4;
				size.Width += 2;
			}
			else
			{
				size.Width += FlyoutForm._2B9C.Height + 4;
				size.Height += 2;
			}
			return size;
		}
		return new Size(100, 100);
	}

	public virtual void Show(Rectangle screenFlyoutBounds)
	{
		Show(screenFlyoutBounds, __2B8A, 10, __2B8B);
	}

	protected virtual void OnFlyoutShowing(FlyoutShowingEventArgs e)
	{
		_2B91?.Invoke(this, e);
	}

	protected virtual void OnFlyoutShown(EventArgs e)
	{
		_2B92?.Invoke(this, e);
	}

	protected virtual void OnQueryShowFlyout(object sender, CancelEventArgs e)
	{
		_2B93?.Invoke(sender, e);
	}

	public virtual void Show(Rectangle screenFlyoutBounds, ePointerSide pointerSide, int pointerOffset, object targetItem)
	{
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Expected O, but got Unknown
		__2B8C = false;
		if (__2B94)
		{
			if (__2B97 != null)
			{
				((Form)__2B97).Activate();
			}
			return;
		}
		CancelEventArgs cancelEventArgs = new CancelEventArgs();
		OnQueryShowFlyout(targetItem ?? __2B8B, cancelEventArgs);
		if (cancelEventArgs.Cancel)
		{
			return;
		}
		FlyoutForm flyoutForm = new FlyoutForm();
		if (!_Ꮃ.IsEmpty)
		{
			flyoutForm.BorderColor = _Ꮃ;
		}
		if (!__0CD2.IsEmpty)
		{
			((Control)flyoutForm).BackColor = __0CD2;
		}
		flyoutForm.PointerSide = pointerSide;
		flyoutForm.ActivateOnShow = __2B9A;
		((Form)flyoutForm).TopMost = __2B80;
		flyoutForm.DropShadow = __2B89;
		flyoutForm.PointerOffset = pointerOffset;
		if (__2781 != null)
		{
			if (screenFlyoutBounds.Size.IsEmpty)
			{
				screenFlyoutBounds.Size = GetFlyoutFormSize();
			}
			if (__2781.Parent != null)
			{
				__2B95 = __2781.Parent;
				__2B95.Controls.Remove(__2781);
			}
			((Control)flyoutForm).Controls.Add(__2781);
			__2781.Location = new Point((pointerSide != ePointerSide.Left) ? 1 : (FlyoutForm._2B9C.Height + 1), (pointerSide != 0) ? 1 : (FlyoutForm._2B9C.Height + 1));
			__2781.Visible = true;
		}
		((Form)flyoutForm).Size = Size.Empty;
		((Form)flyoutForm).Location = screenFlyoutBounds.Location;
		((Form)flyoutForm).FormClosed += new FormClosedEventHandler(FlyoutFormClosed);
		((Form)flyoutForm).FormClosing += new FormClosingEventHandler(FlyoutFormClosing);
		FlyoutShowingEventArgs flyoutShowingEventArgs = new FlyoutShowingEventArgs(flyoutForm, targetItem);
		OnFlyoutShowing(flyoutShowingEventArgs);
		if (flyoutShowingEventArgs.Cancel)
		{
			FlyoutCloseCleanup(flyoutForm);
			((Form)flyoutForm).Close();
			((Component)(object)flyoutForm).Dispose();
			return;
		}
		__2B94 = true;
		((Control)flyoutForm).Visible = true;
		((Form)flyoutForm).Size = screenFlyoutBounds.Size;
		__2B97 = flyoutForm;
		__2B96 = new WeakReference(targetItem);
		OnFlyoutShown(EventArgs.Empty);
		if (!__2B9B && (__2B8F & eFlyoutCloseMode.ClickOutside) == eFlyoutCloseMode.ClickOutside)
		{
			_1BE6._1BEA(this);
			__2B9B = true;
		}
		if (_ᗚ != null && (__2B8F & eFlyoutCloseMode.ParentFormDeactivate) == eFlyoutCloseMode.ParentFormDeactivate)
		{
			Form val = null;
			val = (Form)((!(_ᗚ is Form)) ? ((object)_ᗚ.FindForm()) : ((object)(Form)_ᗚ));
			if (val != null)
			{
				val.Deactivate += ParentFormDeactivate;
				__1C3F = val;
			}
		}
	}

	protected virtual void OnFlyoutClosing(FormClosingEventArgs e)
	{
		FormClosingEventHandler val = _2B98;
		if (val != null)
		{
			val.Invoke((object)this, e);
		}
	}

	private void FlyoutFormClosing(object sender, FormClosingEventArgs e)
	{
		OnFlyoutClosing(e);
	}

	protected virtual void OnFlyoutClosed(FormClosedEventArgs e)
	{
		FormClosedEventHandler val = _2B99;
		if (val != null)
		{
			val.Invoke((object)this, e);
		}
	}

	private void ParentFormDeactivate(object sender, EventArgs e)
	{
		if (__2B97 == null || !((Control)__2B97).Bounds.Contains(Control.MousePosition))
		{
			Close();
		}
	}

	public virtual void Close()
	{
		FlyoutForm flyoutForm = __2B97;
		if (flyoutForm != null)
		{
			((Form)flyoutForm).Close();
			((Component)(object)flyoutForm).Dispose();
		}
	}

	private void FlyoutCloseCleanup(FlyoutForm form)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		__2B96 = null;
		__2B94 = false;
		((Form)form).FormClosed -= new FormClosedEventHandler(FlyoutFormClosed);
		((Control)form).Controls.Remove(__2781);
		if (__2B95 != null)
		{
			__2781.Visible = false;
			__2B95.Controls.Add(__2781);
			__2B95 = null;
		}
		__2B97 = null;
		if (__1C3F != null)
		{
			__1C3F.Deactivate -= ParentFormDeactivate;
			__1C3F = null;
		}
	}

	private void FlyoutFormClosed(object sender, FormClosedEventArgs e)
	{
		FlyoutForm form = (FlyoutForm)sender;
		OnFlyoutClosed(e);
		FlyoutCloseCleanup(form);
	}

	protected virtual void OnActivateOnShowChanged(bool oldValue, bool newValue)
	{
	}

	protected virtual void OnTopMostChanged(bool oldValue, bool newValue)
	{
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackColor()
	{
		return !__0CD2.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackColor()
	{
		BackColor = Color.Empty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColor()
	{
		return !_Ꮃ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderColor()
	{
		BorderColor = Color.Empty;
	}

	protected virtual void OnDropShadowChanged(bool oldValue, bool newValue)
	{
	}

	protected virtual void OnParentChanged(Control oldValue, Control newValue)
	{
	}

	bool ሎ.OnSysKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		return false;
	}

	bool ሎ.OnSysKeyUp(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		return false;
	}

	bool ሎ.OnKeyDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		return false;
	}

	bool ሎ.OnMouseDown(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		if ((__2B8F & eFlyoutCloseMode.ClickOutside) == eFlyoutCloseMode.ClickOutside)
		{
			FlyoutForm flyoutForm = __2B97;
			if (flyoutForm != null && ((Control)flyoutForm).Visible && !((Control)flyoutForm).Bounds.Contains(Control.MousePosition))
			{
				string text = _089F.स(hWnd);
				text = text.ToLower();
				if (text.IndexOf("combolbox") < 0)
				{
					Close();
				}
			}
		}
		return false;
	}

	bool ሎ.OnMouseMove(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		return false;
	}

	bool ሎ.OnMouseWheel(IntPtr hWnd, IntPtr wParam, IntPtr lParam)
	{
		return false;
	}
}
