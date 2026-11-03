using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Metro.ColorTables;
using DevComponents.DotNetBar.Metro.Rendering;

namespace DevComponents.DotNetBar.Metro;

public class MetroAppForm : Form
{
	private const string Ꭱ = "restore";

	private const string Ꭲ = "move";

	private const string Ꭳ = "size";

	private const string Ꭴ = "minimize";

	private const string Ꭵ = "maximize";

	private const string Ꭶ = "close";

	private const string Ꭷ = "sysMenu";

	private ModalPanelBoundsEventHandler Ꭸ;

	private Padding Ꭹ = new Padding(1, 1, 1, 1);

	private Padding Ꭺ = new Padding(1, 1, 1, 1);

	private bool _Ꭻ = true;

	private bool _Ꭼ;

	private List<Control> _Ꭽ = new List<Control>();

	private List<long> _Ꭾ = new List<long>();

	private bool _Ꭿ;

	private MetroShell _Ꮀ;

	private bool _Ꮁ = true;

	private Thickness _Ꮂ = default(Thickness);

	private BorderColors _Ꮃ;

	private bool _Ꮄ;

	private bool _Ꮅ;

	private bool _Ꮆ;

	private bool _Ꮇ;

	private bool _Ꮈ;

	private int _Ꮉ = 4;

	private Thickness _Ꮊ = new Thickness(5.0);

	private bool _Ꮋ = true;

	private ContextMenuBar _כ;

	private string _Ꮌ = "Restore";

	private string _Ꮍ = "Move";

	private string _Ꮎ = "Size";

	private string _Ꮏ = "Minimize";

	private string _Ꮐ = "Maximize";

	private string _Ꮑ = "Close";

	[Browsable(false)]
	[DefaultValue(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool PreRenderShell
	{
		get
		{
			return _Ꭻ;
		}
		set
		{
			_Ꭻ = value;
		}
	}

	[Browsable(false)]
	public bool IsModalPanelDisplayed => _Ꭽ.Count > 0;

	[Description("Indicates whether modal panel when displayed shows MetroStatusBar.")]
	[Category("Appearance")]
	[DefaultValue(false)]
	public bool ModalPanelBoundsExcludeStatusBar
	{
		get
		{
			return _Ꭼ;
		}
		set
		{
			_Ꭼ = value;
		}
	}

	[Browsable(false)]
	public bool IsGlassEnabled => _Ꭿ;

	[Browsable(false)]
	public virtual MetroShell MetroShell
	{
		get
		{
			return _Ꮀ;
		}
		internal set
		{
			_Ꮀ = value;
		}
	}

	[Description("Indicates whether form can be resized.")]
	[Category("Behavior")]
	[DefaultValue(true)]
	public bool Sizable
	{
		get
		{
			return _Ꮁ;
		}
		set
		{
			_Ꮁ = value;
		}
	}

	[Description("Indicates form border thickness.")]
	[Category("Appearance")]
	public Thickness BorderThickness
	{
		get
		{
			return _Ꮂ;
		}
		set
		{
			if (value != _Ꮂ)
			{
				Thickness ꮂ = _Ꮂ;
				_Ꮂ = value;
				OnBorderThicknessChanged(ꮂ, value);
			}
		}
	}

	public BorderColors BorderColor
	{
		get
		{
			return _Ꮃ;
		}
		set
		{
			if (value != _Ꮃ)
			{
				BorderColors ꮃ = _Ꮃ;
				_Ꮃ = value;
				OnBorderColorChanged(ꮃ, value);
			}
		}
	}

	[Browsable(false)]
	public bool IsActive
	{
		get
		{
			return _Ꮄ;
		}
		internal set
		{
			if (_Ꮄ != value)
			{
				_Ꮄ = value;
				Ꮢ();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public int AutoHideTaskBarReduction
	{
		get
		{
			return _Ꮉ;
		}
		set
		{
			_Ꮉ = value;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Thickness FormResizeBorder
	{
		get
		{
			return _Ꮊ;
		}
		set
		{
			_Ꮊ = value;
		}
	}

	[DefaultValue(true)]
	[Description("Indicates whether Close button in top-right corner of the form is visible.")]
	[Category("Appearance")]
	public bool CloseBoxVisible
	{
		get
		{
			return _Ꮋ;
		}
		set
		{
			_Ꮋ = value;
			Ꮦ();
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

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
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
			return _Ꮌ;
		}
		set
		{
			_Ꮌ = value;
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
			return _Ꮍ;
		}
		set
		{
			_Ꮍ = value;
		}
	}

	[Category("System Menu")]
	[DefaultValue("Size")]
	[Localizable(true)]
	[Description("Indicates text for form system menu Size item.")]
	public string SystemMenuSize
	{
		get
		{
			return _Ꮎ;
		}
		set
		{
			_Ꮎ = value;
		}
	}

	[DefaultValue("Minimize")]
	[Localizable(true)]
	[Category("System Menu")]
	[Description("Indicates text for form system menu Minimize item.")]
	public string SystemMenuMinimize
	{
		get
		{
			return _Ꮏ;
		}
		set
		{
			_Ꮏ = value;
		}
	}

	[Localizable(true)]
	[Category("System Menu")]
	[DefaultValue("Maximize")]
	[Description("Indicates text for form system menu Maximize item.")]
	public string SystemMenuMaximize
	{
		get
		{
			return _Ꮐ;
		}
		set
		{
			_Ꮐ = value;
		}
	}

	[Category("System Menu")]
	[Localizable(true)]
	[Description("Indicates text for form system menu Close item.")]
	[DefaultValue("Close")]
	public string SystemMenuClose
	{
		get
		{
			return _Ꮑ;
		}
		set
		{
			_Ꮑ = value;
		}
	}

	[Description("Occurs before modal panel is shown and allows change of modal panel bounds.")]
	public event ModalPanelBoundsEventHandler PrepareModalPanelBounds
	{
		add
		{
			ModalPanelBoundsEventHandler modalPanelBoundsEventHandler = Ꭸ;
			ModalPanelBoundsEventHandler modalPanelBoundsEventHandler2;
			do
			{
				modalPanelBoundsEventHandler2 = modalPanelBoundsEventHandler;
				ModalPanelBoundsEventHandler value2 = (ModalPanelBoundsEventHandler)Delegate.Combine(modalPanelBoundsEventHandler2, value);
				modalPanelBoundsEventHandler = Interlocked.CompareExchange(ref Ꭸ, value2, modalPanelBoundsEventHandler2);
			}
			while ((object)modalPanelBoundsEventHandler != modalPanelBoundsEventHandler2);
		}
		remove
		{
			ModalPanelBoundsEventHandler modalPanelBoundsEventHandler = Ꭸ;
			ModalPanelBoundsEventHandler modalPanelBoundsEventHandler2;
			do
			{
				modalPanelBoundsEventHandler2 = modalPanelBoundsEventHandler;
				ModalPanelBoundsEventHandler value2 = (ModalPanelBoundsEventHandler)Delegate.Remove(modalPanelBoundsEventHandler2, value);
				modalPanelBoundsEventHandler = Interlocked.CompareExchange(ref Ꭸ, value2, modalPanelBoundsEventHandler2);
			}
			while ((object)modalPanelBoundsEventHandler != modalPanelBoundsEventHandler2);
		}
	}

	protected virtual void OnPrepareModalPanelBounds(ModalPanelBoundsEventArgs e)
	{
		Ꭸ?.Invoke(this, e);
	}

	public MetroAppForm()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		if (!StyleManager.IsMetro(StyleManager.Style))
		{
			StyleManager.Style = eStyle.Metro;
		}
		StyleManager.Register((Control)(object)this);
		_Ꭿ = _0EFE.ষ;
		((Form)this).ControlBox = false;
		if (_Ꭿ)
		{
			((Control)this).Padding = Ꭹ;
		}
		else
		{
			((Control)this).Padding = Ꭺ;
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
		Ꮒ();
	}

	private void Ꮒ()
	{
		StyleManager.UpdateMetroAmbientColors((Control)(object)this);
	}

	protected override void OnShown(EventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		if (((Component)this).DesignMode)
		{
			Ꮒ();
		}
		else if (_Ꭻ && _Ꮀ != null)
		{
			Bitmap val = new Bitmap(16, 16);
			try
			{
				((Control)_Ꮀ).DrawToBitmap(val, new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height));
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
		Ꮒ();
		((Form)this).OnHandleCreated(e);
	}

	protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		Dpi.SetScaling(factor);
		((Form)this).ScaleControl(factor, specified);
	}

	public void ShowModalPanel(Control panel)
	{
		Ꮓ(panel, Ꮕ: false, eSlideSide.Left, 500);
	}

	public void ShowModalPanel(Control panel, eSlideSide slideFromSide)
	{
		Ꮓ(panel, Ꮕ: true, slideFromSide, 500);
	}

	public void ShowModalPanel(Control panel, eSlideSide slideFromSide, int animationTimeMilliseconds)
	{
		Ꮓ(panel, Ꮕ: true, slideFromSide, 500);
	}

	private void Ꮓ(Control Ꮔ, bool Ꮕ, eSlideSide Ꮖ, int Ꮗ)
	{
		StyleManager.UpdateAmbientColors(Ꮔ);
		SlidePanel slidePanel = new SlidePanel();
		slidePanel.Ԭ = true;
		slidePanel.AnimationTime = Ꮗ;
		slidePanel.SlideSide = Ꮖ;
		slidePanel.SlideOutButtonVisible = false;
		if (Ꮕ)
		{
			((Control)slidePanel).Bounds = Ꮝ();
			slidePanel.SlideOutOfViewSilent((Control)(object)this);
		}
		else
		{
			((Control)slidePanel).Bounds = Ꮝ();
		}
		((Control)slidePanel).Controls.Add(Ꮔ);
		((Control)this).Controls.Add((Control)(object)slidePanel);
		((Control)this).Controls.SetChildIndex((Control)(object)slidePanel, 0);
		_Ꭽ.Add((Control)(object)slidePanel);
		if (_Ꭽ.Count == 1 && _Ꮀ != null)
		{
			_Ꮀ.MetroTabStrip.StripContainerItem.Enabled = false;
			for (int i = 0; i < _Ꮀ.MetroTabStrip.CaptionContainerItem.SubItems.Count; i++)
			{
				BaseItem baseItem = _Ꮀ.MetroTabStrip.CaptionContainerItem.SubItems[i];
				if (!baseItem.SystemItem || baseItem is QatCustomizeItem)
				{
					baseItem.Enabled = false;
					_Ꭾ.Add(baseItem.Id);
				}
			}
			((Control)_Ꮀ.MetroTabStrip).Invalidate();
		}
		((Control)this).Update();
		if (Ꮕ)
		{
			slidePanel.IsOpen = true;
		}
	}

	public void CloseModalPanel(Control panel)
	{
		Ꮘ(panel, Ꮙ: false, eSlideSide.Left);
	}

	public void CloseModalPanel(Control panel, eSlideSide slideOutToSide)
	{
		Ꮘ(panel, Ꮙ: true, slideOutToSide);
	}

	private void Ꮘ(Control Ꮔ, bool Ꮙ, eSlideSide Ꮚ)
	{
		SlidePanel slidePanel = null;
		foreach (Control item in _Ꭽ)
		{
			if (item.Contains(Ꮔ))
			{
				slidePanel = (SlidePanel)(object)item;
				break;
			}
		}
		if (slidePanel == null)
		{
			throw new ArgumentException("panel was not shown previously using ShowModalPanel method");
		}
		if (Ꮙ)
		{
			slidePanel.SlideSide = Ꮚ;
			slidePanel.IsOpen = false;
			DateTime now = DateTime.Now;
			while (slidePanel._2364)
			{
				Application.DoEvents();
				if (DateTime.Now.Subtract(now).TotalMilliseconds > 950.0)
				{
					slidePanel._2373();
					((Control)slidePanel).Visible = false;
					break;
				}
			}
		}
		else
		{
			((Control)slidePanel).Visible = false;
		}
		((Control)slidePanel).Controls.Remove(Ꮔ);
		if (((Control)slidePanel).Parent != null)
		{
			((Control)slidePanel).Parent.Controls.Remove((Control)(object)slidePanel);
		}
		if (_Ꭽ.Contains((Control)(object)slidePanel))
		{
			_Ꭽ.Remove((Control)(object)slidePanel);
		}
		if (!((Control)slidePanel).IsDisposed)
		{
			((Component)(object)slidePanel).Dispose();
		}
		if (_Ꭽ.Count != 0 || _Ꮀ == null)
		{
			return;
		}
		_Ꮀ.MetroTabStrip.StripContainerItem.Enabled = true;
		for (int i = 0; i < _Ꮀ.MetroTabStrip.CaptionContainerItem.SubItems.Count; i++)
		{
			BaseItem baseItem = _Ꮀ.MetroTabStrip.CaptionContainerItem.SubItems[i];
			if (!baseItem.SystemItem && _Ꭾ.Contains(baseItem.Id))
			{
				baseItem.Enabled = true;
			}
		}
		((Control)_Ꮀ.MetroTabStrip).Refresh();
	}

	internal Ἇ Ꮛ()
	{
		_0EFE.RECT lpRect = new _0EFE.RECT(0, 0, 200, 200);
		CreateParams createParams = ((Control)this).CreateParams;
		_0EFE.AdjustWindowRectEx(ref lpRect, createParams.Style, bMenu: false, createParams.ExStyle);
		Ἇ ἇ = new Ἇ();
		ἇ.ἐ = Math.Abs(lpRect.Top);
		ἇ.ἑ = lpRect.Height - 200 - ἇ.ἐ;
		ἇ.ἒ = Math.Abs(lpRect.Left);
		ἇ.ἓ = lpRect.Width - 200 - ἇ.ἒ;
		return ἇ;
	}

	private void Ꮜ()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		foreach (Control item in _Ꭽ)
		{
			item.Bounds = Ꮝ();
			if ((int)((Form)this).WindowState == 2)
			{
				Ἇ ἇ = Ꮛ();
				item.Padding = new Padding(ἇ.ἒ + 1);
			}
			else
			{
				item.Padding = default(Padding);
			}
		}
	}

	private Rectangle Ꮝ()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Invalid comparison between Unknown and I4
		Thickness borderThickness = GetBorderThickness();
		int num = 0;
		if (_Ꭼ)
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
		if (_Ꮀ != null && _Ꮀ.CaptionVisible)
		{
			int num2 = _Ꮀ.MetroTabStrip.GetCaptionHeight() + 2;
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

	private void Ꮞ()
	{
		bool ষ = _0EFE.ষ;
		if (ষ != _Ꭿ)
		{
			_Ꭿ = ষ;
			Ꮟ();
		}
	}

	private void Ꮟ()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (_Ꭿ)
		{
			((Control)this).Padding = Ꭹ;
		}
		else
		{
			((Control)this).Padding = Ꭺ;
		}
		if (_Ꭿ && ((Control)this).Region != null)
		{
			((Control)this).Region = null;
		}
		else
		{
			((Control)this).Region = Ꮠ();
		}
		((Control)this).Refresh();
	}

	private Region Ꮠ()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		return new Region(new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height));
	}

	protected override void OnStyleChanged(EventArgs e)
	{
		Ꮦ();
		((Form)this).OnStyleChanged(e);
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		if (e.Control is MetroShell)
		{
			_Ꮀ = e.Control as MetroShell;
			Ꮦ();
		}
		((Control)this).OnControlAdded(e);
	}

	protected override void OnControlRemoved(ControlEventArgs e)
	{
		if (e.Control == _Ꮀ)
		{
			_Ꮀ = null;
		}
		((Control)this).OnControlRemoved(e);
	}

	public Thickness GetBorderThickness()
	{
		if (!_Ꮂ.IsZero)
		{
			return _Ꮂ;
		}
		MetroAppFormColorTable metroAppFormColorTable = Ꮡ();
		if (!IsGlassEnabled)
		{
			return metroAppFormColorTable.BorderPlainThickness;
		}
		return metroAppFormColorTable.BorderThickness;
	}

	private MetroAppFormColorTable Ꮡ()
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
		if (!_Ꭿ && ((Control)this).Region == null)
		{
			((Control)this).Region = Ꮠ();
		}
		MetroRender.Paint(this, e);
		((Form)this).OnPaint(e);
	}

	protected override void OnActivated(EventArgs e)
	{
		_0F13();
		((Control)this).Invalidate();
		((Form)this).OnActivated(e);
	}

	protected override void OnDeactivate(EventArgs e)
	{
		((Control)this).Invalidate();
		((Form)this).OnDeactivate(e);
	}

	private void _0F13()
	{
		if (_0EFE.ষ)
		{
			_0EFE.MARGINS pMargins = default(_0EFE.MARGINS);
			pMargins.cyTopHeight = 0;
			pMargins.cxLeftWidth = 0;
			pMargins.cxRightWidth = 0;
			pMargins.cyBottomHeight = ((!_089F.ष()) ? 1 : 0);
			_0EFE.DwmExtendFrameIntoClientArea(((Control)this).Handle, ref pMargins);
		}
	}

	protected virtual bool WindowsMessageSetText(ref Message m)
	{
		if (_0EFE.ষ)
		{
			return true;
		}
		bool flag = _0EFE._0F07(((Message)(ref m)).HWnd, 268435456, 0);
		((Form)this).DefWndProc(ref m);
		if (_Ꮀ != null)
		{
			((Control)_Ꮀ).Refresh();
		}
		if (flag)
		{
			_0EFE._0F07(((Message)(ref m)).HWnd, 0, 268435456);
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
			_Ꮈ = true;
			((Form)this).WndProc(ref m);
			_Ꮈ = true;
			return false;
		}
		return true;
	}

	protected override void WndProc(ref Message m)
	{
		bool flag = true;
		if (!Ꮥ(ref m))
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
			if (!_0EFE.ষ)
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
		if ((!_Ꮄ && (_0EFE._0F0D(((Message)(ref m)).LParam) == 514 || _0EFE._0F0D(((Message)(ref m)).LParam) == 513) && _0EFE._0F0B(((Message)(ref m)).LParam) == -2) || !((Control)this).Enabled || BarUtilities.ᆖ)
		{
			return true;
		}
		bool flag = _0EFE._0F07(((Message)(ref m)).HWnd, 268435456, 0);
		((Form)this).DefWndProc(ref m);
		if (_Ꮀ != null && ((Component)this).DesignMode)
		{
			((Control)_Ꮀ).Refresh();
		}
		if (flag)
		{
			_0EFE._0F07(((Message)(ref m)).HWnd, 0, 268435456);
		}
		return false;
	}

	protected virtual bool WindowsMessageSetIcon(ref Message m)
	{
		bool flag = _0EFE._0F07(((Message)(ref m)).HWnd, 268435456, 0);
		((Form)this).DefWndProc(ref m);
		if (_Ꮀ != null)
		{
			((Control)_Ꮀ).Refresh();
		}
		if (flag)
		{
			_0EFE._0F07(((Message)(ref m)).HWnd, 0, 268435456);
		}
		return false;
	}

	protected virtual bool WindowsMessageNCPaint(ref Message m)
	{
		if (_0EFE.ষ)
		{
			return true;
		}
		((Message)(ref m)).Result = IntPtr.Zero;
		return false;
	}

	protected virtual bool WindowsMessageNCHitTest(ref Message m)
	{
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Invalid comparison between Unknown and I4
		int x = _0EFE._0F0B(((Message)(ref m)).LParam);
		int y = _0EFE._0F0D(((Message)(ref m)).LParam);
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
			rectangle = new Rectangle(((Control)this).Width - ((int)formResizeBorder.Right + Dpi.Width10), ((Control)this).Height - ((int)formResizeBorder.Bottom + Dpi.Height10), (int)formResizeBorder.Right + Dpi.Width10, (int)formResizeBorder.Bottom + Dpi.Height10);
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
		MetroShell ꮀ = _Ꮀ;
		if (ꮀ != null)
		{
			Rectangle rectangle2 = new Rectangle(((int)((Control)this).RightToLeft == 0) ? ((int)formResizeBorder.Left) : (((Control)this).Width - (int)formResizeBorder.Right - 24), (int)formResizeBorder.Left, 24, 24);
			if (rectangle2.Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(5);
				return false;
			}
		}
		if (BarFunctions.IsWindows7 && (int)((Form)this).WindowState == 2 && ꮀ != null && ꮀ.CaptionVisible)
		{
			pt = ((Control)ꮀ.MetroTabStrip).PointToClient(new Point(x, y));
			if (ꮀ.MetroTabStrip._0D65.Contains(pt))
			{
				((Message)(ref m)).Result = new IntPtr(2);
				return false;
			}
		}
		return true;
	}

	protected virtual bool WindowsMessageDwmCompositionChanged(ref Message m)
	{
		Ꮞ();
		_0F13();
		return true;
	}

	private void Ꮢ()
	{
		if (_Ꮀ != null)
		{
			((Control)_Ꮀ.MetroTabStrip).Invalidate(_Ꮀ.MetroTabStrip._0D65);
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
		((Message)(ref m)).Result = _0EFE.DefWindowProc(((Message)(ref m)).HWnd, _0EFE.WindowsMessages.WM_NCACTIVATE, ((Message)(ref m)).WParam, new IntPtr(-1));
		if (((Component)this).DesignMode)
		{
			Ꮣ();
		}
		return false;
	}

	private void Ꮣ()
	{
		if (((Control)this).IsHandleCreated)
		{
			((Control)this).Invalidate(true);
			((Control)this).Refresh();
		}
	}

	protected override void OnTextChanged(EventArgs e)
	{
		if (((Component)this).DesignMode && _Ꮀ != null)
		{
			((Control)_Ꮀ).Refresh();
		}
		((Form)this).OnTextChanged(e);
	}

	protected override void OnResize(EventArgs e)
	{
		if (!_Ꭿ)
		{
			((Control)this).Region = Ꮠ();
			((Control)this).Invalidate();
		}
		Ꮜ();
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
		if (_Ꮈ || (((Component)this).DesignMode && ((specified & 8) == 8 || (specified & 4) == 4)))
		{
			if (width != ((Control)this).Width || height != ((Control)this).Height)
			{
				AdjustBounds(ref width, ref height, specified);
			}
			_Ꮈ = false;
		}
		((Form)this).SetBoundsCore(x, y, width, height, specified);
	}

	protected virtual void AdjustBounds(ref int width, ref int height)
	{
		AdjustBounds(ref width, ref height, (BoundsSpecified)12);
	}

	protected virtual void AdjustBounds(ref int width, ref int height, BoundsSpecified specified)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Invalid comparison between Unknown and I4
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		_0EFE.RECT lpRect = new _0EFE.RECT(0, 0, width, height);
		CreateParams createParams = ((Control)this).CreateParams;
		_0EFE.AdjustWindowRectEx(ref lpRect, createParams.Style, bMenu: false, createParams.ExStyle);
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

	private FormWindowState Ꮤ()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		_0EFE.WINDOWPLACEMENT lpwndpl = default(_0EFE.WINDOWPLACEMENT);
		if (_0EFE.GetWindowPlacement(((Control)this).Handle, out lpwndpl))
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
		if (((Message)(ref m)).WParam != IntPtr.Zero && (int)Ꮤ() == 2)
		{
			_0EFE.NCCALCSIZE_PARAMS nCCALCSIZE_PARAMS = (_0EFE.NCCALCSIZE_PARAMS)Marshal.PtrToStructure(((Message)(ref m)).LParam, typeof(_0EFE.NCCALCSIZE_PARAMS));
			Ἇ ἇ = Ꮛ();
			Size size = new Size(ἇ.ἒ, ἇ.ἑ);
			Size size2 = size;
			if (size.Width > 2)
			{
				size.Width -= size.Width - 2;
			}
			if (size.Height > 2)
			{
				size.Height -= size.Height - 2;
			}
			_0EFE.WINDOWPOS wINDOWPOS = (_0EFE.WINDOWPOS)Marshal.PtrToStructure(nCCALCSIZE_PARAMS.lppos, typeof(_0EFE.WINDOWPOS));
			_0EFE.RECT rECT = _0EFE.RECT.FromRectangle(new Rectangle(wINDOWPOS.x + size2.Width - 1, wINDOWPOS.y + size.Height, wINDOWPOS.cx - size2.Width * 2 + 3, wINDOWPOS.cy - size2.Height - 1));
			if (_Ꮉ > 0)
			{
				_23CE _23CE = new _23CE();
				if (_23CE._23D0)
				{
					if (_23CE._0AF4 == TaskbarPosition.Bottom)
					{
						rECT.Bottom -= _Ꮉ;
					}
					else if (_23CE._0AF4 == TaskbarPosition.Top)
					{
						rECT.Bottom -= _Ꮉ;
						rECT.Top += _Ꮉ;
					}
					else if (_23CE._0AF4 == TaskbarPosition.Left)
					{
						rECT.Right -= _Ꮉ;
						rECT.Left += _Ꮉ;
					}
					else if (_23CE._0AF4 == TaskbarPosition.Right)
					{
						rECT.Right -= _Ꮉ;
					}
				}
			}
			nCCALCSIZE_PARAMS.rgrc0 = rECT;
			nCCALCSIZE_PARAMS.rgrc1 = rECT;
			Marshal.StructureToPtr((object)nCCALCSIZE_PARAMS, ((Message)(ref m)).LParam, fDeleteOld: false);
			((Message)(ref m)).Result = new IntPtr(1024);
			((Control)this).Invalidate();
			return false;
		}
		((Message)(ref m)).Result = IntPtr.Zero;
		((Control)this).Invalidate();
		return false;
	}

	private bool Ꮥ(ref Message Ⴘ)
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

	private void Ꮦ()
	{
		SystemCaptionItem systemCaptionItem = Ꮧ();
		if (systemCaptionItem != null)
		{
			systemCaptionItem.MinimizeVisible = ((Form)this).MinimizeBox;
			systemCaptionItem.RestoreMaximizeVisible = ((Form)this).MaximizeBox;
			systemCaptionItem.CloseVisible = CloseBoxVisible;
			systemCaptionItem.HelpVisible = ((Form)this).HelpButton;
			if (!((Form)this).MaximizeBox && !((Form)this).MinimizeBox && !CloseBoxVisible)
			{
				systemCaptionItem.Visible = false;
			}
			else
			{
				systemCaptionItem.Visible = true;
			}
			if (_Ꮀ != null)
			{
				_Ꮀ.RecalcLayout();
			}
		}
	}

	private SystemCaptionItem Ꮧ()
	{
		if (_Ꮀ != null)
		{
			return _Ꮀ.MetroTabStrip._൧;
		}
		return null;
	}

	internal void Ꮨ(Point _065C)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Invalid comparison between Unknown and I4
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Invalid comparison between Unknown and I4
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Invalid comparison between Unknown and I4
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Invalid comparison between Unknown and I4
		if (!((Component)this).DesignMode)
		{
			Ꮪ();
			ButtonItem buttonItem = (ButtonItem)_כ.Items["sysMenu"];
			buttonItem.SubItems["restore"].Enabled = (int)((Form)this).WindowState != 0;
			buttonItem.SubItems["move"].Enabled = (int)((Form)this).WindowState == 0;
			buttonItem.SubItems["size"].Enabled = (int)((Form)this).WindowState == 0 && Sizable;
			buttonItem.SubItems["minimize"].Enabled = (int)((Form)this).WindowState != 1 && ((Form)this).MinimizeBox;
			buttonItem.SubItems["maximize"].Enabled = (int)((Form)this).WindowState != 2 && Sizable && ((Form)this).MaximizeBox;
			buttonItem.SubItems["close"].Enabled = true;
			buttonItem.Popup(_065C);
		}
	}

	internal void Ꮨ()
	{
		Ꮨ(Control.MousePosition);
	}

	private string Ꮩ(string _੬)
	{
		string defaultValue = "";
		if (_੬ == LocalizationKeys.FormSystemMenuRestore)
		{
			defaultValue = _Ꮌ;
		}
		else if (_੬ == LocalizationKeys.FormSystemMenuMove)
		{
			defaultValue = _Ꮍ;
		}
		else if (_੬ == LocalizationKeys.FormSystemMenuSize)
		{
			defaultValue = _Ꮎ;
		}
		else if (_੬ == LocalizationKeys.FormSystemMenuMinimize)
		{
			defaultValue = _Ꮏ;
		}
		else if (_੬ == LocalizationKeys.FormSystemMenuMaximize)
		{
			defaultValue = _Ꮐ;
		}
		else if (_੬ == LocalizationKeys.FormSystemMenuClose)
		{
			defaultValue = _Ꮑ;
		}
		return LocalizationManager.GetLocalizedString(_੬, defaultValue);
	}

	private void Ꮪ()
	{
		if (_כ == null && !((Component)this).DesignMode)
		{
			_כ = new ContextMenuBar();
			_כ.Style = eDotNetBarStyle.StyleManagerControlled;
			((Control)this).Controls.Add((Control)(object)_כ);
			ButtonItem buttonItem = new ButtonItem("sysMenu");
			_כ.Items.Add(buttonItem);
			ButtonItem buttonItem2 = new ButtonItem("restore", Ꮩ(LocalizationKeys.FormSystemMenuRestore));
			buttonItem2.Image = (Image)(object)BarFunctions.ই("SystemImages.WinRestore.png");
			buttonItem2.Click += Ꮫ;
			buttonItem.SubItems.Add(buttonItem2);
			buttonItem2 = new ButtonItem("move", Ꮩ(LocalizationKeys.FormSystemMenuMove));
			buttonItem2.Click += Ꮫ;
			buttonItem.SubItems.Add(buttonItem2);
			buttonItem2 = new ButtonItem("size", Ꮩ(LocalizationKeys.FormSystemMenuSize));
			buttonItem2.Click += Ꮫ;
			buttonItem.SubItems.Add(buttonItem2);
			buttonItem2 = new ButtonItem("minimize", Ꮩ(LocalizationKeys.FormSystemMenuMinimize));
			buttonItem2.Click += Ꮫ;
			buttonItem2.Image = (Image)(object)BarFunctions.ই("SystemImages.WinMin.png");
			buttonItem.SubItems.Add(buttonItem2);
			buttonItem2 = new ButtonItem("maximize", Ꮩ(LocalizationKeys.FormSystemMenuMaximize));
			buttonItem2.Click += Ꮫ;
			buttonItem2.Image = (Image)(object)BarFunctions.ই("SystemImages.WinMax.png");
			buttonItem.SubItems.Add(buttonItem2);
			buttonItem2 = new ButtonItem("close", Ꮩ(LocalizationKeys.FormSystemMenuClose));
			buttonItem2.Click += Ꮫ;
			buttonItem2.Image = (Image)(object)BarFunctions.ই("SystemImages.WinClose.png");
			buttonItem2.BeginGroup = true;
			buttonItem2.FontBold = true;
			buttonItem2.HotFontBold = true;
			buttonItem2.Shortcuts.Add(eShortcut.CtrlF4);
			buttonItem.SubItems.Add(buttonItem2);
		}
	}

	private void Ꮫ(object _٠, EventArgs ؿ)
	{
		if (_٠ is BaseItem baseItem)
		{
			if (baseItem.Name == "restore")
			{
				_089F.PostMessage(((Control)this).Handle, 274, 61728, 0);
			}
			else if (baseItem.Name == "move")
			{
				_089F.PostMessage(((Control)this).Handle, 274, 61456, 0);
			}
			else if (baseItem.Name == "size")
			{
				_089F.PostMessage(((Control)this).Handle, 274, 61440, 0);
			}
			else if (baseItem.Name == "minimize")
			{
				_089F.PostMessage(((Control)this).Handle, 274, 61472, 0);
			}
			else if (baseItem.Name == "maximize")
			{
				_089F.PostMessage(((Control)this).Handle, 274, 61488, 0);
			}
			else if (baseItem.Name == "close")
			{
				_089F.PostMessage(((Control)this).Handle, 274, 61536, 0);
			}
		}
	}
}
