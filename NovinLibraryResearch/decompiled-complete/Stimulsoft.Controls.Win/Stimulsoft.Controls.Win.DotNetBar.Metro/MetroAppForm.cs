using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Controls;
using Stimulsoft.Controls.Win.DotNetBar.Metro.ColorTables;
using Stimulsoft.Controls.Win.DotNetBar.Metro.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

public class MetroAppForm : Form
{
	private const string SysMenuRestore = "restore";

	private const string SysMenuMove = "move";

	private const string SysMenuSize = "size";

	private const string SysMenuMinimize = "minimize";

	private const string SysMenuMaximize = "maximize";

	private const string SysMenuClose = "close";

	private const string SysMenu = "sysMenu";

	private Padding GlassDefaultPadding = new Padding(0, 1, 1, 1);

	private Padding PlainDefaultPadding = new Padding(1, 1, 1, 1);

	private bool _PreRenderShell = true;

	private bool _ModalPanelBoundsExcludeStatusBar;

	private List<Control> _ModalPanels = new List<Control>();

	private List<long> _DisabledItemIds = new List<long>();

	private bool _IsGlassEnabled;

	private MetroShell _MetroTab;

	private bool _Sizable = true;

	private Thickness _BorderThickness;

	private BorderColors _BorderColor;

	private bool _IsActive;

	private bool _TrackSetBoundsCore;

	private Thickness _FormResizeBorder = new Thickness(5.0);

	private bool _CloseBoxVisible = true;

	private ContextMenuBar _ContextMenuBar;

	private string _SystemMenuRestore = "Restore";

	private string _SystemMenuMove = "Move";

	private string _SystemMenuSize = "Size";

	private string _SystemMenuMinimize = "Minimize";

	private string _SystemMenuMaximize = "Maximize";

	private string _SystemMenuClose = "Close";

	[DefaultValue(true)]
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool PreRenderShell
	{
		get
		{
			return _PreRenderShell;
		}
		set
		{
			_PreRenderShell = value;
		}
	}

	[Browsable(false)]
	public bool IsModalPanelDisplayed => _ModalPanels.Count > 0;

	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether modal panel when displayed shows MetroStatusBar.")]
	public bool ModalPanelBoundsExcludeStatusBar
	{
		get
		{
			return _ModalPanelBoundsExcludeStatusBar;
		}
		set
		{
			_ModalPanelBoundsExcludeStatusBar = value;
		}
	}

	[Browsable(false)]
	public bool IsGlassEnabled => _IsGlassEnabled;

	[Browsable(false)]
	public virtual MetroShell MetroShell
	{
		get
		{
			return _MetroTab;
		}
		internal set
		{
			_MetroTab = value;
		}
	}

	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether form can be resized.")]
	public bool Sizable
	{
		get
		{
			return _Sizable;
		}
		set
		{
			_Sizable = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates form border thickness.")]
	public Thickness BorderThickness
	{
		get
		{
			return _BorderThickness;
		}
		set
		{
			if (value != _BorderThickness)
			{
				Thickness borderThickness = _BorderThickness;
				_BorderThickness = value;
				OnBorderThicknessChanged(borderThickness, value);
			}
		}
	}

	public BorderColors BorderColor
	{
		get
		{
			return _BorderColor;
		}
		set
		{
			if (value != _BorderColor)
			{
				BorderColors borderColor = _BorderColor;
				_BorderColor = value;
				OnBorderColorChanged(borderColor, value);
			}
		}
	}

	[Browsable(false)]
	public bool IsActive
	{
		get
		{
			return _IsActive;
		}
		internal set
		{
			if (_IsActive != value)
			{
				_IsActive = value;
				OnIsActiveChanged();
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public FormBorderStyle FormBorderStyle
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Form)this).FormBorderStyle;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Form)this).FormBorderStyle = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Thickness FormResizeBorder
	{
		get
		{
			return _FormResizeBorder;
		}
		set
		{
			_FormResizeBorder = value;
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	[Description("Indicates whether Close button in top-right corner of the form is visible.")]
	public bool CloseBoxVisible
	{
		get
		{
			return _CloseBoxVisible;
		}
		set
		{
			_CloseBoxVisible = value;
			UpdateMetroSystemCaptionItem();
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool AutoScroll
	{
		get
		{
			return ((Form)this).AutoScroll;
		}
		set
		{
			if (value)
			{
				value = false;
			}
			((Form)this).AutoScroll = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override Point AutoScrollOffset
	{
		get
		{
			return ((Control)this).AutoScrollOffset;
		}
		set
		{
			((Control)this).AutoScrollOffset = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Size AutoScrollMargin
	{
		get
		{
			return ((ScrollableControl)this).AutoScrollMargin;
		}
		set
		{
			((ScrollableControl)this).AutoScrollMargin = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Size AutoScrollMinSize
	{
		get
		{
			return ((ScrollableControl)this).AutoScrollMinSize;
		}
		set
		{
			((ScrollableControl)this).AutoScrollMinSize = value;
		}
	}

	[DefaultValue("Restore")]
	[Description("Indicates text for form system menu Restore item.")]
	[Localizable(true)]
	[Category("System Menu")]
	public string SystemMenuRestore
	{
		get
		{
			return _SystemMenuRestore;
		}
		set
		{
			_SystemMenuRestore = value;
		}
	}

	[DefaultValue("Move")]
	[Description("Indicates text for form system menu Move item.")]
	[Localizable(true)]
	[Category("System Menu")]
	public string SystemMenuMove
	{
		get
		{
			return _SystemMenuMove;
		}
		set
		{
			_SystemMenuMove = value;
		}
	}

	[DefaultValue("Size")]
	[Description("Indicates text for form system menu Size item.")]
	[Localizable(true)]
	[Category("System Menu")]
	public string SystemMenuSize
	{
		get
		{
			return _SystemMenuSize;
		}
		set
		{
			_SystemMenuSize = value;
		}
	}

	[DefaultValue("Minimize")]
	[Description("Indicates text for form system menu Minimize item.")]
	[Localizable(true)]
	[Category("System Menu")]
	public string SystemMenuMinimize
	{
		get
		{
			return _SystemMenuMinimize;
		}
		set
		{
			_SystemMenuMinimize = value;
		}
	}

	[DefaultValue("Maximize")]
	[Description("Indicates text for form system menu Maximize item.")]
	[Localizable(true)]
	[Category("System Menu")]
	public string SystemMenuMaximize
	{
		get
		{
			return _SystemMenuMaximize;
		}
		set
		{
			_SystemMenuMaximize = value;
		}
	}

	[DefaultValue("Close")]
	[Description("Indicates text for form system menu Close item.")]
	[Localizable(true)]
	[Category("System Menu")]
	public string SystemMenuClose
	{
		get
		{
			return _SystemMenuClose;
		}
		set
		{
			_SystemMenuClose = value;
		}
	}

	[Description("Occurs before modal panel is shown and allows change of modal panel bounds.")]
	public event ModalPanelBoundsEventHandler PrepareModalPanelBounds;

	protected virtual void OnPrepareModalPanelBounds(ModalPanelBoundsEventArgs e)
	{
		this.PrepareModalPanelBounds?.Invoke(this, e);
	}

	public MetroAppForm()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		if (StyleManager.Style != eStyle.Metro)
		{
			StyleManager.Style = eStyle.Metro;
		}
		StyleManager.Register((Control)(object)this);
		_IsGlassEnabled = WinApi.IsGlassEnabled;
		((Form)this).ControlBox = false;
		if (_IsGlassEnabled)
		{
			((Control)this).Padding = GlassDefaultPadding;
		}
		else
		{
			((Control)this).Padding = PlainDefaultPadding;
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			StyleManager.Unregister((Control)(object)this);
		}
		((Form)this).Dispose(disposing);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		UpdateColorScheme();
	}

	private void UpdateColorScheme()
	{
		StyleManager.UpdateMetroAmbientColors((Control)(object)this);
	}

	protected override void OnShown(EventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		if (((Component)this).DesignMode)
		{
			UpdateColorScheme();
		}
		else if (_PreRenderShell && _MetroTab != null)
		{
			Bitmap val = new Bitmap(16, 16);
			try
			{
				((Control)_MetroTab).DrawToBitmap(val, new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height));
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		((Form)this).OnShown(e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		UpdateColorScheme();
		((Form)this).OnHandleCreated(e);
	}

	public void ShowModalPanel(Control panel)
	{
		ShowModalPanel(panel, slideIn: false, eSlideSide.Left, 800);
	}

	public void ShowModalPanel(Control panel, eSlideSide slideFromSide)
	{
		ShowModalPanel(panel, slideIn: true, slideFromSide, 800);
	}

	public void ShowModalPanel(Control panel, eSlideSide slideFromSide, int animationTimeMilliseconds)
	{
		ShowModalPanel(panel, slideIn: true, slideFromSide, 800);
	}

	private void ShowModalPanel(Control panel, bool slideIn, eSlideSide slideFromSide, int animationTimeMilliseconds)
	{
		StyleManager.UpdateAmbientColors(panel);
		SlidePanel slidePanel = new SlidePanel();
		slidePanel.CenterContent = true;
		slidePanel.AnimationTime = animationTimeMilliseconds;
		slidePanel.SlideSide = slideFromSide;
		slidePanel.SlideOutButtonVisible = false;
		if (slideIn)
		{
			((Control)slidePanel).Bounds = GetModalPanelBounds();
			slidePanel.SlideOutOfViewSilent((Control)(object)this);
		}
		else
		{
			((Control)slidePanel).Bounds = GetModalPanelBounds();
		}
		((Control)slidePanel).Controls.Add(panel);
		((Control)this).Controls.Add((Control)(object)slidePanel);
		((Control)this).Controls.SetChildIndex((Control)(object)slidePanel, 0);
		_ModalPanels.Add((Control)(object)slidePanel);
		if (_ModalPanels.Count == 1 && _MetroTab != null)
		{
			_MetroTab.MetroTabStrip.StripContainerItem.Enabled = false;
			for (int i = 0; i < _MetroTab.MetroTabStrip.CaptionContainerItem.SubItems.Count; i++)
			{
				BaseItem baseItem = _MetroTab.MetroTabStrip.CaptionContainerItem.SubItems[i];
				if (!baseItem.SystemItem || baseItem is QatCustomizeItem)
				{
					baseItem.Enabled = false;
					_DisabledItemIds.Add(baseItem.Id);
				}
			}
			((Control)_MetroTab.MetroTabStrip).Invalidate();
		}
		((Control)this).Update();
		if (slideIn)
		{
			slidePanel.IsOpen = true;
		}
	}

	public void CloseModalPanel(Control panel)
	{
		CloseModalPanel(panel, slideOut: false, eSlideSide.Left);
	}

	public void CloseModalPanel(Control panel, eSlideSide slideOutToSide)
	{
		CloseModalPanel(panel, slideOut: true, slideOutToSide);
	}

	private void CloseModalPanel(Control panel, bool slideOut, eSlideSide slideOutToSide)
	{
		SlidePanel slidePanel = null;
		foreach (Control modalPanel in _ModalPanels)
		{
			if (modalPanel.Contains(panel))
			{
				slidePanel = (SlidePanel)(object)modalPanel;
				break;
			}
		}
		if (slidePanel == null)
		{
			throw new ArgumentException("panel was not shown previously using ShowModalPanel method");
		}
		if (slideOut)
		{
			slidePanel.SlideSide = slideOutToSide;
			slidePanel.IsOpen = false;
			while (slidePanel.IsAnimating)
			{
				Application.DoEvents();
			}
		}
		else
		{
			((Control)slidePanel).Visible = false;
		}
		((Control)slidePanel).Controls.Remove(panel);
		((Control)slidePanel).Parent.Controls.Remove((Control)(object)slidePanel);
		_ModalPanels.Remove((Control)(object)slidePanel);
		((Component)(object)slidePanel).Dispose();
		if (_ModalPanels.Count != 0 || _MetroTab == null)
		{
			return;
		}
		_MetroTab.MetroTabStrip.StripContainerItem.Enabled = true;
		for (int i = 0; i < _MetroTab.MetroTabStrip.CaptionContainerItem.SubItems.Count; i++)
		{
			BaseItem baseItem = _MetroTab.MetroTabStrip.CaptionContainerItem.SubItems[i];
			if (!baseItem.SystemItem && _DisabledItemIds.Contains(baseItem.Id))
			{
				baseItem.Enabled = true;
			}
		}
		((Control)_MetroTab.MetroTabStrip).Refresh();
	}

	internal NonClientInfo GetNonClientInfo()
	{
		WinApi.RECT lpRect = new WinApi.RECT(0, 0, 200, 200);
		CreateParams createParams = ((Control)this).CreateParams;
		WinApi.AdjustWindowRectEx(ref lpRect, createParams.Style, bMenu: false, createParams.ExStyle);
		NonClientInfo nonClientInfo = new NonClientInfo();
		nonClientInfo.CaptionTotalHeight = Math.Abs(lpRect.Top);
		nonClientInfo.BottomBorder = lpRect.Height - 200 - nonClientInfo.CaptionTotalHeight;
		nonClientInfo.LeftBorder = Math.Abs(lpRect.Left);
		nonClientInfo.RightBorder = lpRect.Width - 200 - nonClientInfo.LeftBorder;
		return nonClientInfo;
	}

	private void UpdateModalPanelsSize()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		foreach (Control modalPanel in _ModalPanels)
		{
			modalPanel.Bounds = GetModalPanelBounds();
			if ((int)((Form)this).WindowState == 2)
			{
				NonClientInfo nonClientInfo = GetNonClientInfo();
				modalPanel.Padding = new Padding(nonClientInfo.LeftBorder + 1);
			}
			else
			{
				modalPanel.Padding = default(Padding);
			}
		}
	}

	private Rectangle GetModalPanelBounds()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		Thickness borderThickness = GetBorderThickness();
		int num = 0;
		if (_ModalPanelBoundsExcludeStatusBar)
		{
			foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
			{
				Control val = item;
				if (val is MetroStatusBar && (int)val.Dock == 2)
				{
					num = val.Height;
					break;
				}
			}
		}
		Rectangle empty = Rectangle.Empty;
		if (_MetroTab != null && _MetroTab.CaptionVisible)
		{
			int num2 = _MetroTab.MetroTabStrip.GetCaptionHeight() + 2;
			empty = new Rectangle((int)borderThickness.Left, num2, ((Control)this).Width - (int)borderThickness.Horizontal, ((Control)this).Height - num2 - (int)borderThickness.Bottom - num);
		}
		else
		{
			empty = new Rectangle((int)borderThickness.Left, (int)borderThickness.Top, ((Control)this).Width - (int)borderThickness.Horizontal, ((Control)this).Height - (int)borderThickness.Vertical - num);
		}
		ModalPanelBoundsEventArgs modalPanelBoundsEventArgs = new ModalPanelBoundsEventArgs(empty);
		OnPrepareModalPanelBounds(modalPanelBoundsEventArgs);
		return modalPanelBoundsEventArgs.ModalPanelBounds;
	}

	private void UpdateIsGlassEnabled()
	{
		bool isGlassEnabled = WinApi.IsGlassEnabled;
		if (isGlassEnabled != _IsGlassEnabled)
		{
			_IsGlassEnabled = isGlassEnabled;
			OnGlassEnabledChanged();
		}
	}

	private void OnGlassEnabledChanged()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (_IsGlassEnabled)
		{
			((Control)this).Padding = GlassDefaultPadding;
		}
		else
		{
			((Control)this).Padding = PlainDefaultPadding;
		}
		if (_IsGlassEnabled && ((Control)this).Region != null)
		{
			((Control)this).Region = null;
		}
		else
		{
			((Control)this).Region = GetPlainFormRegion();
		}
		((Control)this).Refresh();
	}

	private Region GetPlainFormRegion()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		return new Region(new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height));
	}

	protected override void OnStyleChanged(EventArgs e)
	{
		UpdateMetroSystemCaptionItem();
		((Form)this).OnStyleChanged(e);
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		if (e.Control is MetroShell)
		{
			_MetroTab = e.Control as MetroShell;
			UpdateMetroSystemCaptionItem();
		}
		((Control)this).OnControlAdded(e);
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		if (e.Control == _MetroTab)
		{
			_MetroTab = null;
		}
		((Control)this).OnControlRemoved(e);
	}

	public Thickness GetBorderThickness()
	{
		if (!_BorderThickness.IsZero)
		{
			return _BorderThickness;
		}
		MetroAppFormColorTable formColorTable = GetFormColorTable();
		if (!IsGlassEnabled)
		{
			return formColorTable.BorderPlainThickness;
		}
		return formColorTable.BorderThickness;
	}

	private MetroAppFormColorTable GetFormColorTable()
	{
		MetroColorTable colorTable = MetroRender.GetColorTable();
		return colorTable.MetroAppForm;
	}

	protected virtual void OnBorderThicknessChanged(Thickness oldValue, Thickness newValue)
	{
		((Control)this).Invalidate();
	}

	protected virtual void OnBorderColorChanged(BorderColors oldValue, BorderColors newValue)
	{
		((Control)this).Invalidate();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (!_IsGlassEnabled && ((Control)this).Region == null)
		{
			((Control)this).Region = GetPlainFormRegion();
		}
		MetroRender.Paint(this, e);
		((Form)this).OnPaint(e);
	}

	protected override void OnActivated(EventArgs e)
	{
		ExtendGlass();
		((Control)this).Invalidate();
		((Form)this).OnActivated(e);
	}

	protected override void OnDeactivate(EventArgs e)
	{
		((Control)this).Invalidate();
		((Form)this).OnDeactivate(e);
	}

	private void ExtendGlass()
	{
		if (WinApi.IsGlassEnabled)
		{
			WinApi.MARGINS pMargins = default(WinApi.MARGINS);
			pMargins.cyTopHeight = 0;
			pMargins.cxLeftWidth = 0;
			pMargins.cxRightWidth = 0;
			pMargins.cyBottomHeight = 1;
			WinApi.DwmExtendFrameIntoClientArea(((Control)this).Handle, ref pMargins);
		}
	}

	protected virtual bool WindowsMessageSetText(ref Message m)
	{
		if (WinApi.IsGlassEnabled)
		{
			return true;
		}
		bool flag = WinApi.ModifyHwndStyle(((Message)(ref m)).HWnd, 268435456, 0);
		((Form)this).DefWndProc(ref m);
		if (_MetroTab != null)
		{
			((Control)_MetroTab).Refresh();
		}
		if (flag)
		{
			WinApi.ModifyHwndStyle(((Message)(ref m)).HWnd, 0, 268435456);
		}
		return false;
	}

	protected virtual bool WindowsMessageWindowsPosChanged(ref Message m)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Invalid comparison between Unknown and I4
		if ((int)((Form)this).WindowState == 1 || (int)((Form)this).WindowState == 2)
		{
			_TrackSetBoundsCore = true;
			((Form)this).WndProc(ref m);
			_TrackSetBoundsCore = true;
			return false;
		}
		return true;
	}

	protected override void WndProc(ref Message m)
	{
		bool flag = true;
		if (!PreProcessWndProc(ref m))
		{
			return;
		}
		if (((Message)(ref m)).Msg == 131)
		{
			flag = WindowsMessageNCCalcSize(ref m);
		}
		else if (((Message)(ref m)).Msg == 134)
		{
			flag = WindowsMessageNCActivate(ref m);
		}
		else if (((Message)(ref m)).Msg == 798)
		{
			flag = WindowsMessageDwmCompositionChanged(ref m);
		}
		else if (((Message)(ref m)).Msg == 132)
		{
			flag = WindowsMessageNCHitTest(ref m);
		}
		else if (((Message)(ref m)).Msg == 12)
		{
			flag = WindowsMessageSetText(ref m);
		}
		else if (((Message)(ref m)).Msg == 133)
		{
			flag = WindowsMessageNCPaint(ref m);
		}
		else if (((Message)(ref m)).Msg == 20)
		{
			if (!WinApi.IsGlassEnabled)
			{
				flag = false;
				((Message)(ref m)).Result = new IntPtr(1);
			}
		}
		else if (((Message)(ref m)).Msg == 128)
		{
			flag = WindowsMessageSetIcon(ref m);
		}
		else if (((Message)(ref m)).Msg == 12)
		{
			flag = WindowsMessageSetText(ref m);
		}
		else if (((Message)(ref m)).Msg == 32)
		{
			flag = WindowsMessageSetCursor(ref m);
		}
		else if (((Message)(ref m)).Msg == 71)
		{
			flag = WindowsMessageWindowsPosChanged(ref m);
		}
		if (flag)
		{
			((Form)this).WndProc(ref m);
		}
	}

	protected virtual bool WindowsMessageSetCursor(ref Message m)
	{
		if ((!_IsActive && (WinApi.HIWORD(((Message)(ref m)).LParam) == 514 || WinApi.HIWORD(((Message)(ref m)).LParam) == 513) && WinApi.LOWORD(((Message)(ref m)).LParam) == -2) || !((Control)this).Enabled || BarUtilities.IsModalFormOpen)
		{
			return true;
		}
		bool flag = WinApi.ModifyHwndStyle(((Message)(ref m)).HWnd, 268435456, 0);
		((Form)this).DefWndProc(ref m);
		if (_MetroTab != null && ((Component)this).DesignMode)
		{
			((Control)_MetroTab).Refresh();
		}
		if (flag)
		{
			WinApi.ModifyHwndStyle(((Message)(ref m)).HWnd, 0, 268435456);
		}
		return false;
	}

	protected virtual bool WindowsMessageSetIcon(ref Message m)
	{
		bool flag = WinApi.ModifyHwndStyle(((Message)(ref m)).HWnd, 268435456, 0);
		((Form)this).DefWndProc(ref m);
		if (_MetroTab != null)
		{
			((Control)_MetroTab).Refresh();
		}
		if (flag)
		{
			WinApi.ModifyHwndStyle(((Message)(ref m)).HWnd, 0, 268435456);
		}
		return false;
	}

	protected virtual bool WindowsMessageNCPaint(ref Message m)
	{
		if (WinApi.IsGlassEnabled)
		{
			return true;
		}
		((Message)(ref m)).Result = IntPtr.Zero;
		return false;
	}

	protected virtual bool WindowsMessageNCHitTest(ref Message m)
	{
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Invalid comparison between Unknown and I4
		int x = WinApi.LOWORD(((Message)(ref m)).LParam);
		int y = WinApi.HIWORD(((Message)(ref m)).LParam);
		Point pt = ((Control)this).PointToClient(new Point(x, y));
		Thickness formResizeBorder = FormResizeBorder;
		if (Sizable && !formResizeBorder.IsZero && (int)((Form)this).WindowState == 0)
		{
			Rectangle rectangle = new Rectangle(0, 0, (int)formResizeBorder.Left, (int)formResizeBorder.Top);
			rectangle.Inflate(1, 1);
			if (rectangle.Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(13);
				return false;
			}
			rectangle = new Rectangle(((Control)this).Width - (int)formResizeBorder.Right, 0, (int)formResizeBorder.Right, (int)formResizeBorder.Top);
			rectangle.Inflate(1, 1);
			if (rectangle.Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(14);
				return false;
			}
			rectangle = new Rectangle(((Control)this).Width - (int)formResizeBorder.Right, ((Control)this).Height - (int)formResizeBorder.Bottom, (int)formResizeBorder.Right, (int)formResizeBorder.Bottom);
			rectangle.Inflate(2, 2);
			if (rectangle.Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(17);
				return false;
			}
			rectangle = new Rectangle(0, ((Control)this).Height - (int)formResizeBorder.Bottom, (int)formResizeBorder.Left, (int)formResizeBorder.Bottom);
			rectangle.Inflate(1, 1);
			if (rectangle.Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(16);
				return false;
			}
			if (new Rectangle(0, 0, (int)formResizeBorder.Left, ((Control)this).Height).Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(10);
				return false;
			}
			if (new Rectangle(0, 0, ((Control)this).Width, (int)formResizeBorder.Top).Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(12);
				return false;
			}
			if (new Rectangle(((Control)this).Width - (int)formResizeBorder.Right, 0, (int)formResizeBorder.Right, ((Control)this).Height).Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(11);
				return false;
			}
			if (new Rectangle(0, ((Control)this).Height - (int)formResizeBorder.Bottom, ((Control)this).Width, (int)formResizeBorder.Bottom).Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(15);
				return false;
			}
		}
		MetroShell metroTab = _MetroTab;
		if (metroTab != null)
		{
			Rectangle rectangle2 = new Rectangle(((int)((Control)this).RightToLeft == 0) ? ((int)formResizeBorder.Left) : (((Control)this).Width - (int)formResizeBorder.Right - 24), (int)formResizeBorder.Left, 24, 24);
			if (rectangle2.Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(5);
				return false;
			}
		}
		if (BarFunctions.IsWindows7 && (int)((Form)this).WindowState == 2 && metroTab != null && metroTab.CaptionVisible)
		{
			pt = ((Control)metroTab.MetroTabStrip).PointToClient(new Point(x, y));
			if (metroTab.MetroTabStrip.CaptionBounds.Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(2);
				return false;
			}
		}
		return true;
	}

	protected virtual bool WindowsMessageDwmCompositionChanged(ref Message m)
	{
		UpdateIsGlassEnabled();
		ExtendGlass();
		return true;
	}

	private void OnIsActiveChanged()
	{
		if (_MetroTab != null)
		{
			((Control)_MetroTab.MetroTabStrip).Invalidate(_MetroTab.MetroTabStrip.CaptionBounds);
		}
	}

	protected virtual bool WindowsMessageNCActivate(ref Message m)
	{
		if (((Message)(ref m)).WParam != IntPtr.Zero)
		{
			IsActive = true;
		}
		else
		{
			IsActive = false;
		}
		((Message)(ref m)).Result = WinApi.DefWindowProc(((Message)(ref m)).HWnd, WinApi.WindowsMessages.WM_NCACTIVATE, ((Message)(ref m)).WParam, new IntPtr(-1));
		if (((Component)this).DesignMode)
		{
			RefreshAll();
		}
		return false;
	}

	private void RefreshAll()
	{
		if (((Control)this).IsHandleCreated)
		{
			((Control)this).Invalidate(true);
			((Control)this).Refresh();
		}
	}

	protected override void OnTextChanged(EventArgs e)
	{
		if (((Component)this).DesignMode && _MetroTab != null)
		{
			((Control)_MetroTab).Refresh();
		}
		((Form)this).OnTextChanged(e);
	}

	protected override void OnResize(EventArgs e)
	{
		if (!_IsGlassEnabled)
		{
			((Control)this).Region = GetPlainFormRegion();
			((Control)this).Invalidate();
		}
		UpdateModalPanelsSize();
		((Form)this).OnResize(e);
	}

	protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Invalid comparison between Unknown and I4
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		if (_TrackSetBoundsCore || (((Component)this).DesignMode && ((specified & 8) == 8 || (specified & 4) == 4)))
		{
			if (width != ((Control)this).Width || height != ((Control)this).Height)
			{
				AdjustBounds(ref width, ref height, specified);
			}
			_TrackSetBoundsCore = false;
		}
		((Form)this).SetBoundsCore(x, y, width, height, specified);
	}

	protected virtual void AdjustBounds(ref int width, ref int height)
	{
		AdjustBounds(ref width, ref height, (BoundsSpecified)12);
	}

	protected virtual void AdjustBounds(ref int width, ref int height, BoundsSpecified specified)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Invalid comparison between Unknown and I4
		WinApi.RECT lpRect = new WinApi.RECT(0, 0, width, height);
		CreateParams createParams = ((Control)this).CreateParams;
		WinApi.AdjustWindowRectEx(ref lpRect, createParams.Style, bMenu: false, createParams.ExStyle);
		if ((specified & 8) == 8)
		{
			height -= lpRect.Height - height;
		}
		if ((specified & 4) == 4)
		{
			width -= lpRect.Width - width;
		}
	}

	protected override void SetClientSizeCore(int x, int y)
	{
		if (!((Component)this).DesignMode)
		{
			((Form)this).Size = new Size(x, y);
			Type typeFromHandle = typeof(Control);
			try
			{
				typeFromHandle.InvokeMember("clientWidth", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField, null, this, new object[1] { x });
				typeFromHandle.InvokeMember("clientHeight", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetField, null, this, new object[1] { y });
			}
			catch
			{
			}
			((Control)this).OnClientSizeChanged(EventArgs.Empty);
		}
		else
		{
			((Form)this).SetClientSizeCore(x, y);
		}
	}

	private FormWindowState GetWindowState()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		WinApi.WINDOWPLACEMENT lpwndpl = default(WinApi.WINDOWPLACEMENT);
		if (WinApi.GetWindowPlacement(((Control)this).Handle, out lpwndpl))
		{
			if (lpwndpl.showCmd == 3)
			{
				return (FormWindowState)2;
			}
			if (lpwndpl.showCmd == 6)
			{
				return (FormWindowState)1;
			}
			if (lpwndpl.showCmd == 1)
			{
				return (FormWindowState)0;
			}
		}
		return ((Form)this).WindowState;
	}

	protected virtual bool WindowsMessageNCCalcSize(ref Message m)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Invalid comparison between Unknown and I4
		if (((Message)(ref m)).WParam != IntPtr.Zero && (int)GetWindowState() == 2)
		{
			WinApi.NCCALCSIZE_PARAMS structure = (WinApi.NCCALCSIZE_PARAMS)Marshal.PtrToStructure(((Message)(ref m)).LParam, typeof(WinApi.NCCALCSIZE_PARAMS));
			NonClientInfo nonClientInfo = GetNonClientInfo();
			Size size = new Size(nonClientInfo.LeftBorder, nonClientInfo.BottomBorder);
			Size size2 = size;
			if (size.Width > 2)
			{
				size.Width -= size.Width - 2;
			}
			if (size.Height > 2)
			{
				size.Height -= size.Height - 2;
			}
			WinApi.WINDOWPOS wINDOWPOS = (WinApi.WINDOWPOS)Marshal.PtrToStructure(structure.lppos, typeof(WinApi.WINDOWPOS));
			structure.rgrc1 = (structure.rgrc0 = WinApi.RECT.FromRectangle(new Rectangle(wINDOWPOS.x + size2.Width - 1, wINDOWPOS.y + size.Height, wINDOWPOS.cx - size2.Width * 2 + 3, wINDOWPOS.cy - size2.Height - 1)));
			Marshal.StructureToPtr(structure, ((Message)(ref m)).LParam, fDeleteOld: false);
			((Message)(ref m)).Result = new IntPtr(1024);
			((Control)this).Invalidate();
			return false;
		}
		((Message)(ref m)).Result = IntPtr.Zero;
		((Control)this).Invalidate();
		return false;
	}

	private bool PreProcessWndProc(ref Message m)
	{
		return true;
	}

	protected override bool ProcessDialogKey(Keys keyData)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Invalid comparison between Unknown and I4
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if ((int)keyData == 27)
		{
			MetroShell metroShell = MetroShell;
			if (metroShell != null && metroShell.GetApplicationButton() is MetroAppButton metroAppButton && metroAppButton.ProcessEscapeKey(keyData))
			{
				return true;
			}
		}
		return ((Form)this).ProcessDialogKey(keyData);
	}

	private void UpdateMetroSystemCaptionItem()
	{
		SystemCaptionItem systemCaptionItem = GetSystemCaptionItem();
		if (systemCaptionItem != null)
		{
			systemCaptionItem.MinimizeVisible = ((Form)this).MinimizeBox;
			systemCaptionItem.RestoreMaximizeVisible = ((Form)this).MaximizeBox;
			systemCaptionItem.HelpVisible = ((Form)this).HelpButton;
			if (!((Form)this).MaximizeBox && !((Form)this).MinimizeBox && !CloseBoxVisible)
			{
				systemCaptionItem.Visible = false;
			}
			else
			{
				systemCaptionItem.Visible = true;
			}
			if (_MetroTab != null)
			{
				_MetroTab.RecalcLayout();
			}
		}
	}

	private SystemCaptionItem GetSystemCaptionItem()
	{
		if (_MetroTab != null)
		{
			return _MetroTab.MetroTabStrip.SystemCaptionItem;
		}
		return null;
	}

	internal void ShowSystemWindowMenu(Point p)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Invalid comparison between Unknown and I4
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Invalid comparison between Unknown and I4
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Invalid comparison between Unknown and I4
		if (!((Component)this).DesignMode)
		{
			CreateContextMenuBar();
			ButtonItem buttonItem = (ButtonItem)_ContextMenuBar.Items["sysMenu"];
			buttonItem.SubItems["restore"].Enabled = (int)((Form)this).WindowState > 0;
			buttonItem.SubItems["move"].Enabled = (int)((Form)this).WindowState == 0;
			buttonItem.SubItems["size"].Enabled = (int)((Form)this).WindowState == 0 && Sizable;
			buttonItem.SubItems["minimize"].Enabled = (int)((Form)this).WindowState != 1 && ((Form)this).MinimizeBox;
			buttonItem.SubItems["maximize"].Enabled = (int)((Form)this).WindowState != 2 && Sizable && ((Form)this).MaximizeBox;
			buttonItem.SubItems["close"].Enabled = true;
			buttonItem.Popup(p);
		}
	}

	internal void ShowSystemWindowMenu()
	{
		ShowSystemWindowMenu(Control.MousePosition);
	}

	private string GetLocalizedString(string key)
	{
		string defaultValue = "";
		if (key == LocalizationKeys.FormSystemMenuRestore)
		{
			defaultValue = _SystemMenuRestore;
		}
		else if (key == LocalizationKeys.FormSystemMenuMove)
		{
			defaultValue = _SystemMenuMove;
		}
		else if (key == LocalizationKeys.FormSystemMenuSize)
		{
			defaultValue = _SystemMenuSize;
		}
		else if (key == LocalizationKeys.FormSystemMenuMinimize)
		{
			defaultValue = _SystemMenuMinimize;
		}
		else if (key == LocalizationKeys.FormSystemMenuMaximize)
		{
			defaultValue = _SystemMenuMaximize;
		}
		else if (key == LocalizationKeys.FormSystemMenuClose)
		{
			defaultValue = _SystemMenuClose;
		}
		return LocalizationManager.GetLocalizedString(key, defaultValue);
	}

	private void CreateContextMenuBar()
	{
		if (_ContextMenuBar == null && !((Component)this).DesignMode)
		{
			_ContextMenuBar = new ContextMenuBar();
			_ContextMenuBar.Style = eDotNetBarStyle.StyleManagerControlled;
			((Control)this).Controls.Add((Control)(object)_ContextMenuBar);
			ButtonItem buttonItem = new ButtonItem("sysMenu");
			_ContextMenuBar.Items.Add(buttonItem);
			ButtonItem buttonItem2 = new ButtonItem("restore", GetLocalizedString(LocalizationKeys.FormSystemMenuRestore));
			buttonItem2.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.WinRestore.png");
			buttonItem2.Click += SysItemClick;
			buttonItem.SubItems.Add(buttonItem2);
			buttonItem2 = new ButtonItem("move", GetLocalizedString(LocalizationKeys.FormSystemMenuMove));
			buttonItem2.Click += SysItemClick;
			buttonItem.SubItems.Add(buttonItem2);
			buttonItem2 = new ButtonItem("size", GetLocalizedString(LocalizationKeys.FormSystemMenuSize));
			buttonItem2.Click += SysItemClick;
			buttonItem.SubItems.Add(buttonItem2);
			buttonItem2 = new ButtonItem("minimize", GetLocalizedString(LocalizationKeys.FormSystemMenuMinimize));
			buttonItem2.Click += SysItemClick;
			buttonItem2.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.WinMin.png");
			buttonItem.SubItems.Add(buttonItem2);
			buttonItem2 = new ButtonItem("maximize", GetLocalizedString(LocalizationKeys.FormSystemMenuMaximize));
			buttonItem2.Click += SysItemClick;
			buttonItem2.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.WinMax.png");
			buttonItem.SubItems.Add(buttonItem2);
			buttonItem2 = new ButtonItem("close", GetLocalizedString(LocalizationKeys.FormSystemMenuClose));
			buttonItem2.Click += SysItemClick;
			buttonItem2.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.WinClose.png");
			buttonItem2.BeginGroup = true;
			buttonItem2.FontBold = true;
			buttonItem2.HotFontBold = true;
			buttonItem2.Shortcuts.Add(eShortcut.CtrlF4);
			buttonItem.SubItems.Add(buttonItem2);
		}
	}

	private void SysItemClick(object sender, EventArgs e)
	{
		if (sender is BaseItem baseItem)
		{
			if (baseItem.Name == "restore")
			{
				NativeFunctions.PostMessage(((Control)this).Handle, 274, 61728, 0);
			}
			else if (baseItem.Name == "move")
			{
				NativeFunctions.PostMessage(((Control)this).Handle, 274, 61456, 0);
			}
			else if (baseItem.Name == "size")
			{
				NativeFunctions.PostMessage(((Control)this).Handle, 274, 61440, 0);
			}
			else if (baseItem.Name == "minimize")
			{
				NativeFunctions.PostMessage(((Control)this).Handle, 274, 61472, 0);
			}
			else if (baseItem.Name == "maximize")
			{
				NativeFunctions.PostMessage(((Control)this).Handle, 274, 61488, 0);
			}
			else if (baseItem.Name == "close")
			{
				NativeFunctions.PostMessage(((Control)this).Handle, 274, 61536, 0);
			}
		}
	}
}
