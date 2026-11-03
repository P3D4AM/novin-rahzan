using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Controls;

[Designer("DevComponents.DotNetBar.Design.WarningBoxDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
[ToolboxBitmap(typeof(WarningBox), "Controls.WarningBox.ico")]
[DefaultEvent("OptionsClick")]
[ComVisible(false)]
[ToolboxItem(true)]
public class WarningBox : UserControl
{
	private EventHandler _2275;

	private EventHandler _2276;

	private MarkupLinkClickEventHandler Ӟ;

	private bool __2277 = true;

	private bool __137B = true;

	private int __2278;

	private Timer __2279;

	private eWarningBoxColorScheme __21AD;

	private bool _᮰ = true;

	private IContainer Պ;

	internal PanelEx _227A;

	internal Panel _227B;

	internal ButtonX _0F35;

	private PanelEx _227C;

	private LabelX _227D;

	[Browsable(true)]
	[DefaultValue("Close")]
	[Localizable(true)]
	[Category("Warning")]
	public string CloseButtonTooltip
	{
		get
		{
			return _0F35.Tooltip;
		}
		set
		{
			_0F35.Tooltip = value;
		}
	}

	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Category("Warning")]
	[DefaultValue("")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Localizable(true)]
	[Browsable(true)]
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

	[Category("Appearance")]
	[DefaultValue(false)]
	[Description("Indicates whether text is wrapped on multiple lines if it cannot fit the space allocated to the control.")]
	public bool WordWrap
	{
		get
		{
			return _227D.WordWrap;
		}
		set
		{
			_227D.WordWrap = value;
		}
	}

	[Browsable(true)]
	[Localizable(true)]
	[Category("Warning")]
	[DefaultValue(null)]
	public Image Image
	{
		get
		{
			return _227D.Image;
		}
		set
		{
			_227D.Image = value;
		}
	}

	[DefaultValue("Options...")]
	[Localizable(true)]
	[Browsable(true)]
	[Category("Warning")]
	public string OptionsText
	{
		get
		{
			return ((Control)_227C).Text;
		}
		set
		{
			((Control)_227C).Text = value;
		}
	}

	[Category("Warning")]
	[Browsable(true)]
	[DefaultValue(true)]
	public bool OptionsButtonVisible
	{
		get
		{
			return __2277;
		}
		set
		{
			if (__2277 != value)
			{
				__2277 = value;
				((Control)_227C).Visible = __2277;
				_2280();
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
			return __137B;
		}
		set
		{
			if (__137B != value)
			{
				__137B = value;
				((Control)_227B).Visible = __137B;
			}
		}
	}

	[Description("Indicates timeout in seconds after which the control automatically closes itself.")]
	[Category("Behavior")]
	[DefaultValue(0)]
	public int AutoCloseTimeout
	{
		get
		{
			return __2278;
		}
		set
		{
			if (value < 0)
			{
				value = 0;
			}
			__2278 = value;
			_2282();
		}
	}

	[Description("Indicates control's color scheme.")]
	[Category("Appearance")]
	[DefaultValue(eWarningBoxColorScheme.Default)]
	public eWarningBoxColorScheme ColorScheme
	{
		get
		{
			return __21AD;
		}
		set
		{
			__21AD = value;
			UpdateColorScheme();
			((Control)this).Invalidate(true);
		}
	}

	[Category("Appearance")]
	[DefaultValue(true)]
	public bool AntiAlias
	{
		get
		{
			return _᮰;
		}
		set
		{
			if (value != _᮰)
			{
				bool oldValue = _᮰;
				_᮰ = value;
				OnAntiAliasChanged(oldValue, value);
			}
		}
	}

	[Description("Occurs when Close button is clicked.")]
	public event EventHandler CloseClick
	{
		add
		{
			EventHandler eventHandler = _2275;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2275, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2275;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2275, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	[Description("Occurs when Options button is clicked.")]
	public event EventHandler OptionsClick
	{
		add
		{
			EventHandler eventHandler = _2276;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2276, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = _2276;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref _2276, value2, eventHandler2);
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

	public WarningBox()
	{
		ـ();
		_0F35.Image = (Image)(object)BarFunctions.ই("SystemImages.CloseButton.png");
		StyleManager.Register((Control)(object)this);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		UpdateColorScheme();
	}

	protected virtual void OnCloseClick(EventArgs e)
	{
		_2275?.Invoke(this, e);
	}

	protected virtual void OnOptionsClick(EventArgs e)
	{
		_2276?.Invoke(this, e);
	}

	private void _227E(object _٠, EventArgs ؿ)
	{
		OnOptionsClick(ؿ);
	}

	private void _227F(object _٠, EventArgs ؿ)
	{
		OnCloseClick(ؿ);
	}

	protected override void OnTextChanged(EventArgs e)
	{
		((Control)_227D).Text = ((Control)this).Text;
		((Control)this).OnTextChanged(e);
	}

	private void _2280()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		if (__2277)
		{
			LabelX labelX = _227D;
			int width = ((Control)_227A).Width;
			int num = ((Control)_227C).Width + 3;
			Padding padding = ((Control)_227A).Padding;
			((Control)labelX).Width = width - (num + ((Padding)(ref padding)).Horizontal);
		}
		else
		{
			LabelX labelX2 = _227D;
			int width2 = ((Control)_227A).Width;
			Padding padding2 = ((Control)_227A).Padding;
			((Control)labelX2).Width = width2 - (((Padding)(ref padding2)).Right + ((Control)_227D).Left);
		}
	}

	public void UpdateColorScheme()
	{
		Office2007Renderer office2007Renderer = (Office2007Renderer)GlobalManager.Renderer;
		Office2007WarningBoxColorTable office2007WarningBoxColorTable = null;
		office2007WarningBoxColorTable = ((office2007Renderer != null) ? office2007Renderer.ColorTable.WarningBox : new Office2007WarningBoxColorTable());
		if (__21AD == eWarningBoxColorScheme.Default)
		{
			((Control)this).BackColor = office2007WarningBoxColorTable.BackColor;
			_227A.Style.BorderColor.Color = office2007WarningBoxColorTable.WarningBorderColor;
			_227A.Style.BackColor1.Color = office2007WarningBoxColorTable.WarningBackColor1;
			_227A.Style.BackColor2.Color = office2007WarningBoxColorTable.WarningBackColor2;
			_227C.Style.BackColor1.ColorSchemePart = eColorSchemePart.Custom;
			_227C.Style.BackColor1.Color = office2007WarningBoxColorTable.WarningBackColor1;
			_227C.Style.BackColor2.ColorSchemePart = eColorSchemePart.Custom;
			_227C.Style.BackColor2.Color = office2007WarningBoxColorTable.WarningBackColor2;
			_227C.Style.BorderColor.ColorSchemePart = eColorSchemePart.Custom;
			_227C.Style.BorderColor.Color = office2007WarningBoxColorTable.WarningBorderColor;
		}
		else if (__21AD == eWarningBoxColorScheme.Green)
		{
			((Control)this).BackColor = office2007WarningBoxColorTable.GreenBackColor;
			_227A.Style.BorderColor.Color = office2007WarningBoxColorTable.GreenWarningBorderColor;
			_227A.Style.BackColor1.Color = office2007WarningBoxColorTable.GreenWarningBackColor1;
			_227A.Style.BackColor2.Color = office2007WarningBoxColorTable.GreenWarningBackColor2;
			_227C.Style.BackColor1.ColorSchemePart = eColorSchemePart.Custom;
			_227C.Style.BackColor1.Color = office2007WarningBoxColorTable.GreenWarningBackColor1;
			_227C.Style.BackColor2.ColorSchemePart = eColorSchemePart.Custom;
			_227C.Style.BackColor2.Color = office2007WarningBoxColorTable.GreenWarningBackColor2;
			_227C.Style.BorderColor.ColorSchemePart = eColorSchemePart.Custom;
			_227C.Style.BorderColor.Color = office2007WarningBoxColorTable.GreenWarningBorderColor;
		}
		else if (__21AD == eWarningBoxColorScheme.Yellow)
		{
			((Control)this).BackColor = office2007WarningBoxColorTable.YellowBackColor;
			_227A.Style.BorderColor.Color = office2007WarningBoxColorTable.YellowWarningBorderColor;
			_227A.Style.BackColor1.Color = office2007WarningBoxColorTable.YellowWarningBackColor1;
			_227A.Style.BackColor2.Color = office2007WarningBoxColorTable.YellowWarningBackColor2;
			_227C.Style.BackColor1.ColorSchemePart = eColorSchemePart.Custom;
			_227C.Style.BackColor1.Color = office2007WarningBoxColorTable.YellowWarningBackColor1;
			_227C.Style.BackColor2.ColorSchemePart = eColorSchemePart.Custom;
			_227C.Style.BackColor2.Color = office2007WarningBoxColorTable.YellowWarningBackColor2;
			_227C.Style.BorderColor.ColorSchemePart = eColorSchemePart.Custom;
			_227C.Style.BorderColor.Color = office2007WarningBoxColorTable.YellowWarningBorderColor;
		}
		else if (__21AD == eWarningBoxColorScheme.Red)
		{
			((Control)this).BackColor = office2007WarningBoxColorTable.RedBackColor;
			_227A.Style.BorderColor.Color = office2007WarningBoxColorTable.RedWarningBorderColor;
			_227A.Style.BackColor1.Color = office2007WarningBoxColorTable.RedWarningBackColor1;
			_227A.Style.BackColor2.Color = office2007WarningBoxColorTable.RedWarningBackColor2;
			_227C.Style.BackColor1.ColorSchemePart = eColorSchemePart.Custom;
			_227C.Style.BackColor1.Color = office2007WarningBoxColorTable.RedWarningBackColor1;
			_227C.Style.BackColor2.ColorSchemePart = eColorSchemePart.Custom;
			_227C.Style.BackColor2.Color = office2007WarningBoxColorTable.RedWarningBackColor2;
			_227C.Style.BorderColor.ColorSchemePart = eColorSchemePart.Custom;
			_227C.Style.BorderColor.Color = office2007WarningBoxColorTable.RedWarningBorderColor;
		}
		((Control)this).Invalidate(true);
	}

	private void _2281(object _٠, MarkupLinkClickEventArgs ؿ)
	{
		OnMarkupLinkClick(ؿ);
	}

	protected virtual void OnMarkupLinkClick(MarkupLinkClickEventArgs e)
	{
		if (Ӟ != null)
		{
			Ӟ(this, e);
		}
	}

	protected override void OnHandleDestroyed(EventArgs e)
	{
		_2286();
		((Control)this).OnHandleDestroyed(e);
	}

	private void _2282()
	{
		if (__2278 == 0)
		{
			_2286();
		}
		else if (((Control)this).Visible)
		{
			_2283();
		}
	}

	private void _2283()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		if (__2278 != 0 && __2279 == null && !((Component)this).DesignMode)
		{
			__2279 = new Timer();
			__2279.Interval = __2278 * 1000;
			__2279.Tick += _2284;
			__2279.Start();
		}
	}

	private void _2284(object _٠, EventArgs ؿ)
	{
		__2279.Stop();
		_2286();
		_2285();
	}

	private void _2285()
	{
		OnCloseClick(EventArgs.Empty);
		((Control)this).Visible = false;
	}

	private void _2286()
	{
		Timer val = __2279;
		__2279 = null;
		if (val != null)
		{
			val.Stop();
			((Component)(object)val).Dispose();
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (((Control)this).Visible)
		{
			if (__2278 > 0)
			{
				_2283();
			}
		}
		else
		{
			_2286();
		}
		((ScrollableControl)this).OnVisibleChanged(e);
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		if (__21AD == eWarningBoxColorScheme.Default)
		{
			UpdateColorScheme();
		}
		((Control)this).OnHandleCreated(e);
	}

	protected virtual void OnAntiAliasChanged(bool oldValue, bool newValue)
	{
		_227A.AntiAlias = newValue;
		_0F35.AntiAlias = newValue;
		_227C.AntiAlias = newValue;
		_227D.AntiAlias = newValue;
		((Control)this).Invalidate(true);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			StyleManager.Unregister((Control)(object)this);
		}
		if (disposing && Պ != null)
		{
			Պ.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void ـ()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		_227C = new PanelEx();
		_227A = new PanelEx();
		_227D = new LabelX();
		_227B = new Panel();
		_0F35 = new ButtonX();
		((Control)_227A).SuspendLayout();
		((Control)_227B).SuspendLayout();
		((Control)this).SuspendLayout();
		((Control)_227C).Anchor = (AnchorStyles)9;
		_227C.CanvasColor = SystemColors.Control;
		_227C.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)_227C).Location = new Point(311, 5);
		((Control)_227C).Name = "OptionsButton";
		((Control)_227C).Size = new Size(88, 23);
		_227C.Style.Alignment = (StringAlignment)1;
		_227C.Style.BackColor1.Color = Color.White;
		_227C.Style.BackColor2.Color = Color.FromArgb(229, 244, 254);
		_227C.Style.Border = eBorderType.SingleLine;
		_227C.Style.BorderColor.ColorSchemePart = eColorSchemePart.Custom;
		_227C.Style.BorderColor.Color = Color.FromArgb(162, 188, 213);
		_227C.Style.ForeColor.ColorSchemePart = eColorSchemePart.ItemText;
		_227C.Style.GradientAngle = 90;
		_227C.StyleMouseDown.Alignment = (StringAlignment)1;
		_227C.StyleMouseDown.BackColor1.ColorSchemePart = eColorSchemePart.ItemPressedBackground;
		_227C.StyleMouseDown.BackColor2.ColorSchemePart = eColorSchemePart.ItemPressedBackground2;
		_227C.StyleMouseDown.BorderColor.ColorSchemePart = eColorSchemePart.ItemPressedBorder;
		_227C.StyleMouseDown.ForeColor.ColorSchemePart = eColorSchemePart.ItemPressedText;
		_227C.StyleMouseOver.Alignment = (StringAlignment)1;
		_227C.StyleMouseOver.BackColor1.ColorSchemePart = eColorSchemePart.ItemHotBackground;
		_227C.StyleMouseOver.BackColor2.ColorSchemePart = eColorSchemePart.ItemHotBackground2;
		_227C.StyleMouseOver.BorderColor.ColorSchemePart = eColorSchemePart.ItemHotBorder;
		_227C.StyleMouseOver.ForeColor.ColorSchemePart = eColorSchemePart.ItemHotText;
		((Control)_227C).TabIndex = 1;
		((Control)_227C).Text = "Options...";
		((Control)_227C).Click += _227E;
		_227A.CanvasColor = SystemColors.Control;
		_227A.ColorSchemeStyle = eDotNetBarStyle.Office2007;
		((Control)_227A).Controls.Add((Control)(object)_227D);
		((Control)_227A).Controls.Add((Control)(object)_227C);
		((Control)_227A).Dock = (DockStyle)5;
		((Control)_227A).Location = new Point(0, 0);
		((Control)_227A).Name = "PanelWarning";
		((Control)_227A).Padding = new Padding(4);
		((Control)_227A).Size = new Size(405, 33);
		_227A.Style.BackColor1.Color = Color.White;
		_227A.Style.BackColor2.Color = Color.FromArgb(229, 244, 254);
		_227A.Style.Border = eBorderType.SingleLine;
		_227A.Style.BorderColor.Color = Color.FromArgb(162, 188, 213);
		_227A.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		_227A.Style.GradientAngle = 90;
		_227A.Style.MarginLeft = 5;
		((Control)_227A).TabIndex = 11;
		((Control)_227D).Anchor = (AnchorStyles)15;
		((Control)_227D).ForeColor = Color.FromArgb(50, 50, 50);
		((Control)_227D).Location = new Point(4, 5);
		((Control)_227D).Margin = new Padding(5);
		((Control)_227D).Name = "WarningLabel";
		_227D.PaddingLeft = 2;
		((Control)_227D).Size = new Size(304, 22);
		((Control)_227D).TabIndex = 0;
		_227D.MarkupLinkClick += _2281;
		((Control)_227B).BackColor = Color.Transparent;
		((Control)_227B).Controls.Add((Control)(object)_0F35);
		((Control)_227B).Dock = (DockStyle)4;
		((Control)_227B).Location = new Point(405, 0);
		((Control)_227B).Name = "PanelClose";
		((Control)_227B).Size = new Size(22, 33);
		((Control)_227B).TabIndex = 12;
		((Control)_0F35).AccessibleRole = (AccessibleRole)43;
		_0F35.ColorTable = eButtonColor.Flat;
		((Control)_0F35).Dock = (DockStyle)1;
		_0F35.FocusCuesEnabled = false;
		((Control)_0F35).Location = new Point(0, 0);
		((Control)_0F35).Name = "CloseButton";
		((Control)_0F35).Size = new Size(22, 17);
		((Control)_0F35).TabIndex = 1;
		_0F35.Tooltip = "Close";
		_0F35.AntiAlias = true;
		((Control)_0F35).Click += _227F;
		((ContainerControl)this).AutoScaleDimensions = new SizeF(96f, 96f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)2;
		((Control)this).BackColor = Color.FromArgb(196, 219, 249);
		((Control)this).Controls.Add((Control)(object)_227A);
		((Control)this).Controls.Add((Control)(object)_227B);
		((Control)this).Name = "WarningBox";
		((Control)this).Size = new Size(427, 33);
		((Control)_227A).ResumeLayout(false);
		((Control)_227B).ResumeLayout(false);
		((Control)this).ResumeLayout(false);
	}
}
