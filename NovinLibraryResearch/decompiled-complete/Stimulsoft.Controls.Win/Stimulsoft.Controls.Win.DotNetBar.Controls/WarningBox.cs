using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxBitmap(typeof(WarningBox), "Controls.WarningBox.ico")]
[ToolboxItem(false)]
[DefaultEvent("OptionsClick")]
[ComVisible(false)]
public class WarningBox : UserControl
{
	private bool _OptionsButtonVisible = true;

	private bool _CloseButtonVisible = true;

	private int _AutoCloseTimeout;

	private Timer _AutoCloseTimer;

	private eWarningBoxColorScheme _ColorScheme;

	private bool _AntiAlias = true;

	private IContainer components;

	internal PanelEx PanelWarning;

	internal Panel PanelClose;

	internal ButtonX CloseButton;

	private PanelEx OptionsButton;

	private LabelX WarningLabel;

	[Browsable(true)]
	[Category("Warning")]
	[Localizable(true)]
	[DefaultValue("Close")]
	public string CloseButtonTooltip
	{
		get
		{
			return CloseButton.Tooltip;
		}
		set
		{
			CloseButton.Tooltip = value;
		}
	}

	[Browsable(true)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	[Category("Warning")]
	[Localizable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[DefaultValue("")]
	public override string Text
	{
		get
		{
			return ((UserControl)this).Text;
		}
		set
		{
			((UserControl)this).Text = value;
		}
	}

	[DefaultValue(false)]
	[Category("Appearance")]
	[Description("Indicates whether text is wrapped on multiple lines if it cannot fit the space allocated to the control.")]
	public bool WordWrap
	{
		get
		{
			return WarningLabel.WordWrap;
		}
		set
		{
			WarningLabel.WordWrap = value;
		}
	}

	[Browsable(true)]
	[Category("Warning")]
	[DefaultValue(null)]
	[Localizable(true)]
	public Image Image
	{
		get
		{
			return WarningLabel.Image;
		}
		set
		{
			WarningLabel.Image = value;
		}
	}

	[Browsable(true)]
	[Category("Warning")]
	[DefaultValue("Options...")]
	[Localizable(true)]
	public string OptionsText
	{
		get
		{
			return ((Control)OptionsButton).Text;
		}
		set
		{
			((Control)OptionsButton).Text = value;
		}
	}

	[Browsable(true)]
	[Category("Warning")]
	[DefaultValue(true)]
	public bool OptionsButtonVisible
	{
		get
		{
			return _OptionsButtonVisible;
		}
		set
		{
			if (_OptionsButtonVisible != value)
			{
				_OptionsButtonVisible = value;
				((Control)OptionsButton).Visible = _OptionsButtonVisible;
				OnOptionsButtonVisibleChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Warning")]
	[DefaultValue(true)]
	public bool CloseButtonVisible
	{
		get
		{
			return _CloseButtonVisible;
		}
		set
		{
			if (_CloseButtonVisible != value)
			{
				_CloseButtonVisible = value;
				((Control)PanelClose).Visible = _CloseButtonVisible;
			}
		}
	}

	[DefaultValue(0)]
	[Category("Behavior")]
	[Description("Indicates timeout in seconds after which the control automatically closes itself.")]
	public int AutoCloseTimeout
	{
		get
		{
			return _AutoCloseTimeout;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			_AutoCloseTimeout = value;
			OnAutoCloseTimeoutChanged();
		}
	}

	[DefaultValue(eWarningBoxColorScheme.Default)]
	[Category("Appearance")]
	[Description("Indicates control's color scheme.")]
	public eWarningBoxColorScheme ColorScheme
	{
		get
		{
			return _ColorScheme;
		}
		set
		{
			_ColorScheme = value;
			UpdateColorScheme();
			((Control)this).Invalidate(true);
		}
	}

	[DefaultValue(true)]
	[Category("Appearance")]
	public bool AntiAlias
	{
		get
		{
			return _AntiAlias;
		}
		set
		{
			if (value != _AntiAlias)
			{
				bool antiAlias = _AntiAlias;
				_AntiAlias = value;
				OnAntiAliasChanged(antiAlias, value);
			}
		}
	}

	[Description("Occurs when Close button is clicked.")]
	public event EventHandler CloseClick;

	[Description("Occurs when Options button is clicked.")]
	public event EventHandler OptionsClick;

	public event MarkupLinkClickEventHandler MarkupLinkClick;

	public WarningBox()
	{
		InitializeComponent();
		CloseButton.Image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.CloseButton.png");
		StyleManager.Register((Control)(object)this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		UpdateColorScheme();
	}

	protected virtual void OnCloseClick(EventArgs e)
	{
		this.CloseClick?.Invoke(this, e);
	}

	protected virtual void OnOptionsClick(EventArgs e)
	{
		this.OptionsClick?.Invoke(this, e);
	}

	private void OptionsButton_Click(object sender, EventArgs e)
	{
		OnOptionsClick(e);
	}

	private void CloseButton_Click(object sender, EventArgs e)
	{
		OnCloseClick(e);
	}

	protected override void OnTextChanged(EventArgs e)
	{
		((Control)WarningLabel).Text = ((Control)this).Text;
		((Control)this).OnTextChanged(e);
	}

	private void OnOptionsButtonVisibleChanged()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		Padding padding;
		if (_OptionsButtonVisible)
		{
			LabelX warningLabel = WarningLabel;
			int width = ((Control)PanelWarning).Width;
			int num = ((Control)OptionsButton).Width + 3;
			padding = ((Control)PanelWarning).Padding;
			((Control)warningLabel).Width = width - (num + ((Padding)(ref padding)).Horizontal);
		}
		else
		{
			LabelX warningLabel2 = WarningLabel;
			int width2 = ((Control)PanelWarning).Width;
			padding = ((Control)PanelWarning).Padding;
			((Control)warningLabel2).Width = width2 - (((Padding)(ref padding)).Right + ((Control)WarningLabel).Left);
		}
	}

	public void UpdateColorScheme()
	{
		Office2007Renderer office2007Renderer = (Office2007Renderer)GlobalManager.Renderer;
		Office2007WarningBoxColorTable office2007WarningBoxColorTable = null;
		office2007WarningBoxColorTable = ((office2007Renderer != null) ? office2007Renderer.ColorTable.WarningBox : new Office2007WarningBoxColorTable());
		if (_ColorScheme == eWarningBoxColorScheme.Default)
		{
			((Control)this).BackColor = office2007WarningBoxColorTable.BackColor;
			PanelWarning.Style.BorderColor.Color = office2007WarningBoxColorTable.WarningBorderColor;
			PanelWarning.Style.BackColor1.Color = office2007WarningBoxColorTable.WarningBackColor1;
			PanelWarning.Style.BackColor2.Color = office2007WarningBoxColorTable.WarningBackColor2;
			OptionsButton.Style.BackColor1.ColorSchemePart = eColorSchemePart.Custom;
			OptionsButton.Style.BackColor1.Color = office2007WarningBoxColorTable.WarningBackColor1;
			OptionsButton.Style.BackColor2.ColorSchemePart = eColorSchemePart.Custom;
			OptionsButton.Style.BackColor2.Color = office2007WarningBoxColorTable.WarningBackColor2;
			OptionsButton.Style.BorderColor.ColorSchemePart = eColorSchemePart.Custom;
			OptionsButton.Style.BorderColor.Color = office2007WarningBoxColorTable.WarningBorderColor;
		}
		else if (_ColorScheme == eWarningBoxColorScheme.Green)
		{
			((Control)this).BackColor = office2007WarningBoxColorTable.GreenBackColor;
			PanelWarning.Style.BorderColor.Color = office2007WarningBoxColorTable.GreenWarningBorderColor;
			PanelWarning.Style.BackColor1.Color = office2007WarningBoxColorTable.GreenWarningBackColor1;
			PanelWarning.Style.BackColor2.Color = office2007WarningBoxColorTable.GreenWarningBackColor2;
			OptionsButton.Style.BackColor1.ColorSchemePart = eColorSchemePart.Custom;
			OptionsButton.Style.BackColor1.Color = office2007WarningBoxColorTable.GreenWarningBackColor1;
			OptionsButton.Style.BackColor2.ColorSchemePart = eColorSchemePart.Custom;
			OptionsButton.Style.BackColor2.Color = office2007WarningBoxColorTable.GreenWarningBackColor2;
			OptionsButton.Style.BorderColor.ColorSchemePart = eColorSchemePart.Custom;
			OptionsButton.Style.BorderColor.Color = office2007WarningBoxColorTable.GreenWarningBorderColor;
		}
		else if (_ColorScheme == eWarningBoxColorScheme.Yellow)
		{
			((Control)this).BackColor = office2007WarningBoxColorTable.YellowBackColor;
			PanelWarning.Style.BorderColor.Color = office2007WarningBoxColorTable.YellowWarningBorderColor;
			PanelWarning.Style.BackColor1.Color = office2007WarningBoxColorTable.YellowWarningBackColor1;
			PanelWarning.Style.BackColor2.Color = office2007WarningBoxColorTable.YellowWarningBackColor2;
			OptionsButton.Style.BackColor1.ColorSchemePart = eColorSchemePart.Custom;
			OptionsButton.Style.BackColor1.Color = office2007WarningBoxColorTable.YellowWarningBackColor1;
			OptionsButton.Style.BackColor2.ColorSchemePart = eColorSchemePart.Custom;
			OptionsButton.Style.BackColor2.Color = office2007WarningBoxColorTable.YellowWarningBackColor2;
			OptionsButton.Style.BorderColor.ColorSchemePart = eColorSchemePart.Custom;
			OptionsButton.Style.BorderColor.Color = office2007WarningBoxColorTable.YellowWarningBorderColor;
		}
		else if (_ColorScheme == eWarningBoxColorScheme.Red)
		{
			((Control)this).BackColor = office2007WarningBoxColorTable.RedBackColor;
			PanelWarning.Style.BorderColor.Color = office2007WarningBoxColorTable.RedWarningBorderColor;
			PanelWarning.Style.BackColor1.Color = office2007WarningBoxColorTable.RedWarningBackColor1;
			PanelWarning.Style.BackColor2.Color = office2007WarningBoxColorTable.RedWarningBackColor2;
			OptionsButton.Style.BackColor1.ColorSchemePart = eColorSchemePart.Custom;
			OptionsButton.Style.BackColor1.Color = office2007WarningBoxColorTable.RedWarningBackColor1;
			OptionsButton.Style.BackColor2.ColorSchemePart = eColorSchemePart.Custom;
			OptionsButton.Style.BackColor2.Color = office2007WarningBoxColorTable.RedWarningBackColor2;
			OptionsButton.Style.BorderColor.ColorSchemePart = eColorSchemePart.Custom;
			OptionsButton.Style.BorderColor.Color = office2007WarningBoxColorTable.RedWarningBorderColor;
		}
		((Control)this).Invalidate(true);
	}

	private void WarningLabel_MarkupLinkClick(object sender, MarkupLinkClickEventArgs e)
	{
		OnMarkupLinkClick(e);
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (this.MarkupLinkClick != null)
		{
			this.MarkupLinkClick(this, e);
		}
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		DestroyAutoCloseTimer();
		((Control)this).OnHandleDestroyed(e);
	}

	private void OnAutoCloseTimeoutChanged()
	{
		if (_AutoCloseTimeout == 0)
		{
			DestroyAutoCloseTimer();
		}
		else if (((Control)this).Visible)
		{
			SetupAutoCloseTimer();
		}
	}

	private void SetupAutoCloseTimer()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		if (_AutoCloseTimeout != 0 && _AutoCloseTimer == null && !((Component)this).DesignMode)
		{
			_AutoCloseTimer = new Timer();
			_AutoCloseTimer.Interval = _AutoCloseTimeout * 1000;
			_AutoCloseTimer.Tick += AutoCloseTimerTick;
			_AutoCloseTimer.Start();
		}
	}

	private void AutoCloseTimerTick(object sender, EventArgs e)
	{
		_AutoCloseTimer.Stop();
		DestroyAutoCloseTimer();
		AutoClose();
	}

	private void AutoClose()
	{
		OnCloseClick(EventArgs.Empty);
		((Control)this).Visible = false;
	}

	private void DestroyAutoCloseTimer()
	{
		Timer autoCloseTimer = _AutoCloseTimer;
		_AutoCloseTimer = null;
		if (autoCloseTimer != null)
		{
			autoCloseTimer.Stop();
			((Component)(object)autoCloseTimer).Dispose();
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (((Control)this).Visible)
		{
			if (_AutoCloseTimeout > 0)
			{
				SetupAutoCloseTimer();
			}
		}
		else
		{
			DestroyAutoCloseTimer();
		}
		((ScrollableControl)this).OnVisibleChanged(e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		if (_ColorScheme == eWarningBoxColorScheme.Default)
		{
			UpdateColorScheme();
		}
		((Control)this).OnHandleCreated(e);
	}

	protected virtual void OnAntiAliasChanged(bool oldValue, bool newValue)
	{
		PanelWarning.AntiAlias = newValue;
		CloseButton.AntiAlias = newValue;
		OptionsButton.AntiAlias = newValue;
		WarningLabel.AntiAlias = newValue;
		((Control)this).Invalidate(true);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			StyleManager.Unregister((Control)(object)this);
		}
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		OptionsButton = new PanelEx();
		PanelWarning = new PanelEx();
		WarningLabel = new LabelX();
		PanelClose = new Panel();
		CloseButton = new ButtonX();
		((Control)PanelWarning).SuspendLayout();
		((Control)PanelClose).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)OptionsButton).Anchor = (AnchorStyles)9;
		OptionsButton.CanvasColor = SystemColors.Control;
		OptionsButton.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)OptionsButton).Location = new Point(311, 5);
		((Control)OptionsButton).Name = "OptionsButton";
		((Control)OptionsButton).Size = new Size(88, 23);
		OptionsButton.Style.Alignment = (StringAlignment)1;
		OptionsButton.Style.BackColor1.Color = Color.White;
		OptionsButton.Style.BackColor2.Color = Color.FromArgb(229, 244, 254);
		OptionsButton.Style.Border = eBorderType.SingleLine;
		OptionsButton.Style.BorderColor.ColorSchemePart = eColorSchemePart.Custom;
		OptionsButton.Style.BorderColor.Color = Color.FromArgb(162, 188, 213);
		OptionsButton.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
		OptionsButton.Style.GradientAngle = 90;
		OptionsButton.StyleMouseDown.Alignment = (StringAlignment)1;
		OptionsButton.StyleMouseDown.BackColor1.ColorSchemePart = eColorSchemePart.ItemPressedBackground;
		OptionsButton.StyleMouseDown.BackColor2.ColorSchemePart = eColorSchemePart.ItemPressedBackground2;
		OptionsButton.StyleMouseDown.BorderColor.ColorSchemePart = eColorSchemePart.ItemPressedBorder;
		OptionsButton.StyleMouseDown.ForeColor.ColorSchemePart = eColorSchemePart.ItemPressedText;
		OptionsButton.StyleMouseOver.Alignment = (StringAlignment)1;
		OptionsButton.StyleMouseOver.BackColor1.ColorSchemePart = eColorSchemePart.ItemHotBackground;
		OptionsButton.StyleMouseOver.BackColor2.ColorSchemePart = eColorSchemePart.ItemHotBackground2;
		OptionsButton.StyleMouseOver.BorderColor.ColorSchemePart = eColorSchemePart.ItemHotBorder;
		OptionsButton.StyleMouseOver.ForeColor.ColorSchemePart = eColorSchemePart.ItemHotText;
		((Control)OptionsButton).TabIndex = 1;
		((Control)OptionsButton).Text = "Options...";
		((Control)OptionsButton).Click += OptionsButton_Click;
		PanelWarning.CanvasColor = SystemColors.Control;
		PanelWarning.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)PanelWarning).Controls.Add((Control)(object)WarningLabel);
		((Control)PanelWarning).Controls.Add((Control)(object)OptionsButton);
		((Control)PanelWarning).Dock = (DockStyle)5;
		((Control)PanelWarning).Location = new Point(0, 0);
		((Control)PanelWarning).Name = "PanelWarning";
		((Control)PanelWarning).Padding = new Padding(4);
		((Control)PanelWarning).Size = new Size(405, 33);
		PanelWarning.Style.BackColor1.Color = Color.White;
		PanelWarning.Style.BackColor2.Color = Color.FromArgb(229, 244, 254);
		PanelWarning.Style.Border = eBorderType.SingleLine;
		PanelWarning.Style.BorderColor.Color = Color.FromArgb(162, 188, 213);
		PanelWarning.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		PanelWarning.Style.GradientAngle = 90;
		PanelWarning.Style.MarginLeft = 5;
		((Control)PanelWarning).TabIndex = 11;
		((Control)WarningLabel).Anchor = (AnchorStyles)15;
		((Control)WarningLabel).ForeColor = Color.FromArgb(50, 50, 50);
		((Control)WarningLabel).Location = new Point(4, 5);
		((Control)WarningLabel).Margin = new Padding(5);
		((Control)WarningLabel).Name = "WarningLabel";
		WarningLabel.PaddingLeft = 2;
		((Control)WarningLabel).Size = new Size(304, 22);
		((Control)WarningLabel).TabIndex = 0;
		WarningLabel.MarkupLinkClick += WarningLabel_MarkupLinkClick;
		((Control)PanelClose).BackColor = Color.Transparent;
		((Control)PanelClose).Controls.Add((Control)(object)CloseButton);
		((Control)PanelClose).Dock = (DockStyle)4;
		((Control)PanelClose).Location = new Point(405, 0);
		((Control)PanelClose).Name = "PanelClose";
		((Control)PanelClose).Size = new Size(22, 33);
		((Control)PanelClose).TabIndex = 12;
		((Control)CloseButton).AccessibleRole = (AccessibleRole)43;
		CloseButton.ColorTable = eButtonColor.Flat;
		((Control)CloseButton).Dock = (DockStyle)1;
		CloseButton.FocusCuesEnabled = false;
		((Control)CloseButton).Location = new Point(0, 0);
		((Control)CloseButton).Name = "CloseButton";
		((Control)CloseButton).Size = new Size(22, 17);
		((Control)CloseButton).TabIndex = 1;
		CloseButton.Tooltip = "Close";
		CloseButton.AntiAlias = true;
		((Control)CloseButton).Click += CloseButton_Click;
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)0;
		((Control)this).BackColor = Color.FromArgb(196, 219, 249);
		((Control)this).Controls.Add((Control)(object)PanelWarning);
		((Control)this).Controls.Add((Control)(object)PanelClose);
		((Control)this).Name = "WarningBox";
		((Control)this).Size = new Size(427, 33);
		((Control)PanelWarning).ResumeLayout(false);
		((Control)PanelClose).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
