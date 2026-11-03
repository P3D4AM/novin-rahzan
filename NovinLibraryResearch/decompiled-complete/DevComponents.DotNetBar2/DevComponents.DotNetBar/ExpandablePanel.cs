using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace DevComponents.DotNetBar;

[ToolboxBitmap(typeof(ExpandablePanel), "ExpandablePanel.ico")]
[ToolboxItem(true)]
[Designer("DevComponents.DotNetBar.Design.ExpandablePanelDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class ExpandablePanel : PanelEx
{
	private ExpandChangeEventHandler ℏ;

	private ExpandChangeEventHandler _1C2B;

	private PanelExTitle ℐ = new PanelExTitle();

	private bool Ӫ = true;

	private int ᇰ = 100;

	private Rectangle ℑ = Rectangle.Empty;

	private Image ℒ;

	private Image ℓ;

	private eCollapseDirection _2114;

	private Image ℕ;

	private Image _2116;

	private Image _2117;

	private Image _2118;

	private PanelEx ℙ;

	private bool ℚ;

	private bool _ℛ;

	[Description("Indicates whether panel will hide the controls it contains when its collapsed and show them when its expanded")]
	[Category("Behavior")]
	[DefaultValue(false)]
	public bool HideControlsWhenCollapsed
	{
		get
		{
			return _ℛ;
		}
		set
		{
			_ℛ = value;
		}
	}

	[Description("Indicates whether panel is collapsed/expanded when title bar is clicked.")]
	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Expand")]
	public bool ExpandOnTitleClick
	{
		get
		{
			return ℚ;
		}
		set
		{
			ℚ = value;
		}
	}

	[DefaultValue(eCollapseDirection.BottomToTop)]
	[Category("Expand")]
	[Browsable(true)]
	[Description("Indicates collapse/expand direction for the control.")]
	public eCollapseDirection CollapseDirection
	{
		get
		{
			return _2114;
		}
		set
		{
			_2114 = value;
			Å();
		}
	}

	[Category("Expand")]
	[Description("Indicates whether panel is expaned or not. Default value is true.")]
	[DefaultValue(true)]
	[Browsable(true)]
	public bool Expanded
	{
		get
		{
			return Ӫ;
		}
		set
		{
			if (Ӫ != value)
			{
				_211E(value, eEventSource.Code);
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(100)]
	[Category("Expand")]
	[Description("Indicates animation time in milliseconds, set to 0 to disable animation.")]
	public int AnimationTime
	{
		get
		{
			return ᇰ;
		}
		set
		{
			if (ᇰ >= 0)
			{
				ᇰ = value;
			}
		}
	}

	[Browsable(false)]
	public Rectangle ExpandedBounds
	{
		get
		{
			return ℑ;
		}
		set
		{
			ℑ = value;
		}
	}

	[Description("Indicates whether expand button is visible or not.")]
	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Expand Button")]
	public bool ExpandButtonVisible
	{
		get
		{
			return ℐ.ExpandButtonVisible;
		}
		set
		{
			ℐ.ExpandButtonVisible = value;
		}
	}

	[Browsable(false)]
	public Rectangle ExpandButtonBounds => ℐ.ExpandChangeButton.DisplayRectangle;

	[Description("Indicates image used on title bar to collapse panel.")]
	[DefaultValue(null)]
	[Category("Title")]
	[Browsable(true)]
	public Image ButtonImageCollapse
	{
		get
		{
			return ℓ;
		}
		set
		{
			ℓ = value;
			_2123();
		}
	}

	[Category("Title")]
	[Browsable(true)]
	[Description("Indicates image used on title bar to expand panel.")]
	[DefaultValue(null)]
	public Image ButtonImageExpand
	{
		get
		{
			return ℒ;
		}
		set
		{
			ℒ = value;
			_2123();
		}
	}

	[DefaultValue("")]
	[Description("Indicates text for the title of the panel.")]
	[Category("Title")]
	[Editor("DevComponents.DotNetBar.Design.TextMarkupUIEditor, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf", typeof(UITypeEditor))]
	[Localizable(true)]
	[Browsable(true)]
	public string TitleText
	{
		get
		{
			return ((Control)ℐ).Text;
		}
		set
		{
			((Control)ℐ).Text = value;
			if (ℙ != null)
			{
				((Control)ℙ).Text = value;
			}
		}
	}

	[DevCoBrowsable(true)]
	[Browsable(true)]
	[Category("Title")]
	[NotifyParentProperty(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Description("Gets or sets title style.")]
	public ItemStyle TitleStyle => ℐ.Style;

	[Description("Gets or sets the title style when mouse hovers over the title.")]
	[DevCoBrowsable(true)]
	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Title")]
	[NotifyParentProperty(true)]
	public ItemStyle TitleStyleMouseOver => ℐ.StyleMouseOver;

	[NotifyParentProperty(true)]
	[Category("Title")]
	[Description("Gets or sets the Title style when mouse button is pressed on the title.")]
	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ItemStyle TitleStyleMouseDown => ℐ.StyleMouseDown;

	[Category("Title")]
	[DefaultValue(26)]
	[Description("Indicates height of the title portion of the panel.")]
	[Browsable(true)]
	public int TitleHeight
	{
		get
		{
			return _2129();
		}
		set
		{
			K(value);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public PanelEx TitlePanel => ℐ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public ButtonItem ExpandButton => ℐ.ExpandChangeButton;

	[Category("Expand Button")]
	[Description("Indicates the alignment of expand button.")]
	[DefaultValue(eTitleButtonAlignment.Right)]
	public eTitleButtonAlignment ExpandButtonAlignment
	{
		get
		{
			return ℐ.ButtonAlignment;
		}
		set
		{
			ℐ.ButtonAlignment = value;
		}
	}

	[Browsable(false)]
	public PanelEx VerticalExpandPanel => ℙ;

	public event ExpandChangeEventHandler ExpandedChanging
	{
		add
		{
			ExpandChangeEventHandler expandChangeEventHandler = ℏ;
			ExpandChangeEventHandler expandChangeEventHandler2;
			do
			{
				expandChangeEventHandler2 = expandChangeEventHandler;
				ExpandChangeEventHandler value2 = (ExpandChangeEventHandler)Delegate.Combine(expandChangeEventHandler2, value);
				expandChangeEventHandler = Interlocked.CompareExchange(ref ℏ, value2, expandChangeEventHandler2);
			}
			while ((object)expandChangeEventHandler != expandChangeEventHandler2);
		}
		remove
		{
			ExpandChangeEventHandler expandChangeEventHandler = ℏ;
			ExpandChangeEventHandler expandChangeEventHandler2;
			do
			{
				expandChangeEventHandler2 = expandChangeEventHandler;
				ExpandChangeEventHandler value2 = (ExpandChangeEventHandler)Delegate.Remove(expandChangeEventHandler2, value);
				expandChangeEventHandler = Interlocked.CompareExchange(ref ℏ, value2, expandChangeEventHandler2);
			}
			while ((object)expandChangeEventHandler != expandChangeEventHandler2);
		}
	}

	public event ExpandChangeEventHandler ExpandedChanged
	{
		add
		{
			ExpandChangeEventHandler expandChangeEventHandler = _1C2B;
			ExpandChangeEventHandler expandChangeEventHandler2;
			do
			{
				expandChangeEventHandler2 = expandChangeEventHandler;
				ExpandChangeEventHandler value2 = (ExpandChangeEventHandler)Delegate.Combine(expandChangeEventHandler2, value);
				expandChangeEventHandler = Interlocked.CompareExchange(ref _1C2B, value2, expandChangeEventHandler2);
			}
			while ((object)expandChangeEventHandler != expandChangeEventHandler2);
		}
		remove
		{
			ExpandChangeEventHandler expandChangeEventHandler = _1C2B;
			ExpandChangeEventHandler expandChangeEventHandler2;
			do
			{
				expandChangeEventHandler2 = expandChangeEventHandler;
				ExpandChangeEventHandler value2 = (ExpandChangeEventHandler)Delegate.Remove(expandChangeEventHandler2, value);
				expandChangeEventHandler = Interlocked.CompareExchange(ref _1C2B, value2, expandChangeEventHandler2);
			}
			while ((object)expandChangeEventHandler != expandChangeEventHandler2);
		}
	}

	public ExpandablePanel()
	{
		((Control)ℐ).Text = "Title Bar";
		((Control)ℐ).Location = new Point(0, 0);
		((Control)ℐ).Size = new Size(24, 26);
		((Control)ℐ).Dock = (DockStyle)1;
		((Control)this).Controls.Add((Control)(object)ℐ);
		ℐ.ColorScheme = base.ColorScheme;
		ℐ.ExpandedClick += ℝ;
		((Control)ℐ).Click += ℜ;
		((Control)this).BackColor = SystemColors.Control;
		_2123();
	}

	private void ℜ(object _٠, EventArgs ؿ)
	{
		Point pt = ((Control)ℐ).PointToClient(Control.MousePosition);
		if (ℚ && !ℐ.ExpandChangeButton.DisplayRectangle.Contains(pt))
		{
			_211E(!Expanded, eEventSource.Mouse);
		}
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		base.OnControlAdded(e);
		((Control)ℐ).SendToBack();
	}

	private void ℝ(object _٠, EventArgs ؿ)
	{
		_211E(!Expanded, eEventSource.Mouse);
	}

	private void _211E(bool _211F, eEventSource ڃ)
	{
		ExpandedChangeEventArgs expandedChangeEventArgs = new ExpandedChangeEventArgs(ڃ, _211F);
		_2125(expandedChangeEventArgs);
		if (!expandedChangeEventArgs.Cancel)
		{
			Ӫ = _211F;
			ᱞ();
			Ω(expandedChangeEventArgs);
		}
	}

	private DockStyle _2120(Control _0BDE)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if ((int)_0BDE.Dock == 0 || (int)_0BDE.Dock == 5)
		{
			return (DockStyle)1;
		}
		return _0BDE.Dock;
	}

	private Rectangle _2121()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Invalid comparison between Unknown and I4
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Invalid comparison between Unknown and I4
		Rectangle result = ℑ;
		DockStyle val = _2120((Control)(object)ℐ);
		if ((int)val == 3)
		{
			result = new Rectangle(((Control)this).Bounds.Location, ℑ.Size);
		}
		else if ((int)val == 4)
		{
			result = new Rectangle(((Control)this).Bounds.X - (ℑ.Width - ((Control)this).Width), ((Control)this).Bounds.Y, ℑ.Width, ℑ.Height);
		}
		else if ((int)val == 1)
		{
			result = ((_2114 == eCollapseDirection.LeftToRight) ? new Rectangle(((Control)this).Bounds.X - (ℑ.Width - ((Control)this).Bounds.Width), ((Control)this).Bounds.Y, ℑ.Width, ℑ.Height) : ((_2114 != eCollapseDirection.TopToBottom) ? new Rectangle(((Control)this).Bounds.Location, ℑ.Size) : new Rectangle(((Control)this).Bounds.X, ℑ.Y, ℑ.Width, ℑ.Height)));
		}
		else if ((int)val == 2)
		{
			result = new Rectangle(((Control)this).Bounds.X, ((Control)this).Bounds.Y - (ℑ.Height - ((Control)this).Height), ℑ.Width, ℑ.Height);
		}
		return result;
	}

	private void ᱞ()
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		if (Expanded)
		{
			ℐ._0AD4 = true;
			base._0AD4 = true;
			if (!ℑ.IsEmpty)
			{
				if (ℙ != null)
				{
					_2122();
				}
				if (_ℛ)
				{
					((Control)this).SuspendLayout();
					foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
					{
						Control val = item;
						if (!(val is ContextMenuBar) && val != TitlePanel)
						{
							val.Visible = true;
						}
					}
					((Control)this).ResumeLayout();
				}
				Rectangle rectangle = _2121();
				if (AnimationTime == 0 || ((Component)(object)this).DesignMode)
				{
					TypeDescriptor.GetProperties(this)["Bounds"].SetValue(this, rectangle);
				}
				else
				{
					Rectangle bounds = ((Control)this).Bounds;
					BarFunctions.AnimateControl((Control)(object)this, show: true, ᇰ, bounds, rectangle);
				}
				TypeDescriptor.GetProperties(this)["ExpandedBounds"].SetValue(this, Rectangle.Empty);
			}
		}
		else
		{
			TypeDescriptor.GetProperties(this)["ExpandedBounds"].SetValue(this, ((Control)this).Bounds);
			Point point = ((Control)this).PointToScreen(Point.Empty);
			point = ((((Control)this).Parent == null) ? Point.Empty : ((Control)this).Parent.PointToClient(point));
			Rectangle rectangle2 = new Rectangle(point, new Size(((Control)ℐ).Width + ((ScrollableControl)this).DockPadding.Left + ((ScrollableControl)this).DockPadding.Right, Math.Max(((Control)ℐ).Height + ((ScrollableControl)this).DockPadding.Top + ((ScrollableControl)this).DockPadding.Bottom, ℐ.ExpandChangeButton.HeightInternal)));
			if (_2114 == eCollapseDirection.RightToLeft)
			{
				rectangle2 = new Rectangle(point, new Size(Math.Max(((Control)ℐ).Height + ((ScrollableControl)this).DockPadding.Top + ((ScrollableControl)this).DockPadding.Bottom, ℐ.ExpandChangeButton.WidthInternal + 6), ((Control)this).Height));
			}
			else if (_2114 == eCollapseDirection.LeftToRight)
			{
				Size size = new Size(Math.Max(((Control)ℐ).Height + ((ScrollableControl)this).DockPadding.Top + ((ScrollableControl)this).DockPadding.Bottom, ℐ.ExpandChangeButton.WidthInternal + 6), ((Control)this).Height);
				if (!point.IsEmpty)
				{
					point.X += ((Control)this).Width - size.Width;
				}
				rectangle2 = new Rectangle(point, size);
			}
			else if (_2114 == eCollapseDirection.TopToBottom)
			{
				Size size2 = new Size(((Control)ℐ).Width + ((ScrollableControl)this).DockPadding.Left + ((ScrollableControl)this).DockPadding.Right, Math.Max(((Control)ℐ).Height + ((ScrollableControl)this).DockPadding.Top + ((ScrollableControl)this).DockPadding.Bottom, ℐ.ExpandChangeButton.HeightInternal));
				if (!point.IsEmpty)
				{
					point.Y += ((Control)this).Height - size2.Height;
				}
				rectangle2 = new Rectangle(point, size2);
			}
			if (AnimationTime == 0 || ((Component)(object)this).DesignMode)
			{
				TypeDescriptor.GetProperties(this)["Bounds"].SetValue(this, rectangle2);
			}
			else
			{
				Rectangle bounds2 = ((Control)this).Bounds;
				BarFunctions.AnimateControl((Control)(object)this, show: true, ᇰ, bounds2, rectangle2);
			}
			if (_2114 == eCollapseDirection.LeftToRight || _2114 == eCollapseDirection.RightToLeft)
			{
				ℐ._0AD4 = false;
				base._0AD4 = false;
			}
			if (_ℛ)
			{
				((Control)this).SuspendLayout();
				foreach (Control item2 in (ArrangedElementCollection)((Control)this).Controls)
				{
					Control val2 = item2;
					if (!(val2 is ContextMenuBar) && val2 != TitlePanel)
					{
						val2.Visible = false;
					}
				}
				((Control)this).ResumeLayout();
			}
			((Control)ℐ).SendToBack();
			if (_2114 == eCollapseDirection.LeftToRight || _2114 == eCollapseDirection.RightToLeft)
			{
				ℬ();
			}
		}
		_2123();
		((Control)this).Refresh();
	}

	private void _2122()
	{
		if (ℙ != null)
		{
			((Control)this).Controls.Remove((Control)(object)ℙ);
			((Component)(object)ℙ).Dispose();
			ℙ = null;
		}
	}

	private void _2123()
	{
		Image image = null;
		Image hoverImage = null;
		if (Expanded)
		{
			if (ℓ != null)
			{
				image = ℓ;
			}
			else if (BarFunctions.IsOffice2007Style(base.ColorSchemeStyle))
			{
				if (_2114 == eCollapseDirection.BottomToTop || _2114 == eCollapseDirection.RightToLeft)
				{
					image = _2116;
					hoverImage = _2118;
				}
				else
				{
					image = ℕ;
					hoverImage = _2117;
				}
			}
			else if (_2114 == eCollapseDirection.BottomToTop)
			{
				image = (Image)(object)BarFunctions.ই("SystemImages.ExpandTitle.png");
			}
			else if (_2114 == eCollapseDirection.TopToBottom)
			{
				image = (Image)(object)BarFunctions.ই("SystemImages.CollapseTitle.png");
			}
			else if (_2114 == eCollapseDirection.LeftToRight)
			{
				Bitmap val = BarFunctions.ই("SystemImages.ExpandTitle.png");
				((Image)val).RotateFlip((RotateFlipType)3);
				image = (Image)(object)val;
			}
			else if (_2114 == eCollapseDirection.RightToLeft)
			{
				Bitmap val2 = BarFunctions.ই("SystemImages.ExpandTitle.png");
				((Image)val2).RotateFlip((RotateFlipType)1);
				image = (Image)(object)val2;
			}
		}
		else if (ℒ != null)
		{
			image = ℒ;
		}
		else if (BarFunctions.IsOffice2007Style(base.ColorSchemeStyle))
		{
			if (_2114 == eCollapseDirection.BottomToTop || _2114 == eCollapseDirection.RightToLeft)
			{
				image = ℕ;
				hoverImage = _2117;
			}
			else
			{
				image = _2116;
				hoverImage = _2118;
			}
		}
		else if (_2114 == eCollapseDirection.BottomToTop)
		{
			image = (Image)(object)BarFunctions.ই("SystemImages.CollapseTitle.png");
		}
		else if (_2114 == eCollapseDirection.TopToBottom)
		{
			image = (Image)(object)BarFunctions.ই("SystemImages.ExpandTitle.png");
		}
		else if (_2114 == eCollapseDirection.LeftToRight)
		{
			Bitmap val3 = BarFunctions.ই("SystemImages.CollapseTitle.png");
			((Image)val3).RotateFlip((RotateFlipType)3);
			image = (Image)(object)val3;
		}
		else if (_2114 == eCollapseDirection.RightToLeft)
		{
			Bitmap val4 = BarFunctions.ই("SystemImages.CollapseTitle.png");
			((Image)val4).RotateFlip((RotateFlipType)1);
			image = (Image)(object)val4;
		}
		ℐ.ExpandChangeButton.Image = image;
		ℐ.ExpandChangeButton.HoverImage = hoverImage;
		if (BarFunctions.IsHandleValid((Control)(object)this))
		{
			ℐ.UpdateButtonPosition();
			((Control)ℐ).Invalidate();
		}
	}

	private void ℤ()
	{
		if (_2116 != null)
		{
			_2116.Dispose();
		}
		if (ℕ != null)
		{
			ℕ.Dispose();
		}
		if (_2118 != null)
		{
			_2118.Dispose();
		}
		if (_2117 != null)
		{
			_2117.Dispose();
		}
		bool flag = _2114 == eCollapseDirection.TopToBottom || _2114 == eCollapseDirection.BottomToTop;
		_2116 = ឱ.ឳ(_17B4: true, base.ColorScheme.PanelText, flag);
		_2118 = ឱ.ឳ(_17B4: true, base.ColorScheme.ItemHotText, flag);
		ℕ = ឱ.ឳ(_17B4: false, base.ColorScheme.PanelText, flag);
		_2117 = ឱ.ឳ(_17B4: false, base.ColorScheme.ItemHotText, flag);
	}

	private void _2125(ExpandedChangeEventArgs ؿ)
	{
		if (ℏ != null)
		{
			ℏ(this, ؿ);
		}
	}

	private void Ω(ExpandedChangeEventArgs ؿ)
	{
		if (_1C2B != null)
		{
			_1C2B(this, ؿ);
		}
	}

	protected override void OnColorSchemeChanged()
	{
		base.OnColorSchemeChanged();
		ℐ.ColorScheme = GetColorScheme();
		_2127();
	}

	private void _2127()
	{
		ℨ();
		if (BarFunctions.IsOffice2007Style(base.ColorSchemeStyle))
		{
			ℤ();
		}
		_2123();
	}

	private void ℨ()
	{
		if (_2116 != null)
		{
			_2116.Dispose();
			_2116 = null;
		}
		if (_2118 != null)
		{
			_2118.Dispose();
			_2118 = null;
		}
		if (ℕ != null)
		{
			ℕ.Dispose();
			ℕ = null;
		}
		if (_2117 != null)
		{
			_2117.Dispose();
			_2117 = null;
		}
	}

	private int _2129()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		if ((int)((Control)ℐ).Dock == 3 || (int)((Control)ℐ).Dock == 4)
		{
			return ((Control)ℐ).Width;
		}
		return ((Control)ℐ).Height;
	}

	private void K(int ԝ)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		if ((int)((Control)ℐ).Dock == 3 || (int)((Control)ℐ).Dock == 4)
		{
			((Control)ℐ).Width = ԝ;
		}
		else
		{
			((Control)ℐ).Height = ԝ;
		}
	}

	private void Å()
	{
		_2127();
	}

	protected override void Dispose(bool disposing)
	{
		ℨ();
		if (BarUtilities.DisposeItemImages && !((Component)(object)this).DesignMode)
		{
			BarUtilities.ᆟ(ref ℓ);
			BarUtilities.ᆟ(ref ℒ);
		}
		base.Dispose(disposing);
	}

	private void ℬ()
	{
		ℙ = new PanelEx();
		ℙ.ColorSchemeStyle = base.ColorSchemeStyle;
		ℙ.ColorScheme = base.ColorScheme;
		ℙ.ApplyButtonStyle();
		ℙ.Style.VerticalText = true;
		ℙ.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		ℙ.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		ℙ.Style.ResetBackColor2();
		ℙ.StyleMouseOver.VerticalText = true;
		ℙ.StyleMouseDown.VerticalText = true;
		((Control)ℙ).Dock = (DockStyle)5;
		((Control)ℙ).Text = ((Control)TitlePanel).Text;
		((Control)this).Controls.Add((Control)(object)ℙ);
		if (TitleStyle.Font != null)
		{
			((Control)ℙ).Font = TitleStyle.Font;
		}
		else if (((Control)TitlePanel).Font != null)
		{
			((Control)ℙ).Font = ((Control)TitlePanel).Font;
		}
		((Control)ℙ).Click += ℭ;
		((Control)ℙ).BringToFront();
	}

	private void ℭ(object _٠, EventArgs ؿ)
	{
		Expanded = !Expanded;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeExpandedBounds()
	{
		return !ℑ.IsEmpty;
	}

	public void ResetTitleStyle()
	{
		ℐ.ResetStyle();
	}

	public void ResetTitleStyleMouseOver()
	{
		ℐ.ResetStyleMouseOver();
	}

	public void ResetTitleStyleMouseDown()
	{
		ℐ.ResetStyleMouseDown();
	}

	protected override void OnAntiAliasChanged()
	{
		ℐ.AntiAlias = base.AntiAlias;
	}
}
