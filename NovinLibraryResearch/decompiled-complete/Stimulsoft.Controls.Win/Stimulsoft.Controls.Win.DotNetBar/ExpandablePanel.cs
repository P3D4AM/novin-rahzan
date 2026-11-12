using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(ExpandablePanel), "ExpandablePanel.ico")]
public class ExpandablePanel : PanelEx
{
	private PanelExTitle m_TitleBar = new PanelExTitle();

	private bool m_Expanded = true;

	private int m_AnimationTime = 100;

	private Rectangle m_ExpandedBounds = Rectangle.Empty;

	private Image m_ButtonImageExpand;

	private Image m_ButtonImageCollapse;

	private eCollapseDirection m_CollapseDirection;

	private Image m_Office2007ButtonImageExpand;

	private Image m_Office2007ButtonImageCollapse;

	private Image m_Office2007ButtonImageExpandMouseOver;

	private Image m_Office2007ButtonImageCollapseMouseOver;

	private PanelEx m_VerticalExpandPane;

	private bool m_ExpandOnTitleClick;

	private bool _HideControlsWhenCollapsed;

	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether panel will hide the controls it contains when its collapsed and show them when its expanded")]
	public bool HideControlsWhenCollapsed
	{
		get
		{
			return _HideControlsWhenCollapsed;
		}
		set
		{
			_HideControlsWhenCollapsed = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Expand")]
	[Description("Indicates whether panel is collapsed/expanded when title bar is clicked.")]
	public bool ExpandOnTitleClick
	{
		get
		{
			return m_ExpandOnTitleClick;
		}
		set
		{
			m_ExpandOnTitleClick = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(eCollapseDirection.BottomToTop)]
	[Category("Expand")]
	[Description("Indicates collapse/expand direction for the control.")]
	public eCollapseDirection CollapseDirection
	{
		get
		{
			return m_CollapseDirection;
		}
		set
		{
			m_CollapseDirection = value;
			OnCollapseDirectionChanged();
		}
	}

	[Browsable(true)]
	[Category("Expand")]
	[DefaultValue(true)]
	[Description("Indicates whether panel is expaned or not. Default value is true.")]
	public bool Expanded
	{
		get
		{
			return m_Expanded;
		}
		set
		{
			if (m_Expanded != value)
			{
				SetExpanded(value, eEventSource.Code);
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
			return m_AnimationTime;
		}
		set
		{
			if (m_AnimationTime >= 0)
			{
				m_AnimationTime = value;
			}
		}
	}

	[Browsable(false)]
	public Rectangle ExpandedBounds
	{
		get
		{
			return m_ExpandedBounds;
		}
		set
		{
			m_ExpandedBounds = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Expand Button")]
	[Description("Indicates whether expand button is visible or not.")]
	public bool ExpandButtonVisible
	{
		get
		{
			return m_TitleBar.ExpandButtonVisible;
		}
		set
		{
			m_TitleBar.ExpandButtonVisible = value;
		}
	}

	[Browsable(false)]
	public Rectangle ExpandButtonBounds => m_TitleBar.ExpandChangeButton.DisplayRectangle;

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Title")]
	[Description("Indicates image used on title bar to collapse panel.")]
	public Image ButtonImageCollapse
	{
		get
		{
			return m_ButtonImageCollapse;
		}
		set
		{
			m_ButtonImageCollapse = value;
			UpdateExpandButtonImage();
		}
	}

	[Browsable(true)]
	[DefaultValue(null)]
	[Category("Title")]
	[Description("Indicates image used on title bar to expand panel.")]
	public Image ButtonImageExpand
	{
		get
		{
			return m_ButtonImageExpand;
		}
		set
		{
			m_ButtonImageExpand = value;
			UpdateExpandButtonImage();
		}
	}

	[Browsable(true)]
	[DefaultValue("")]
	[Category("Title")]
	[Description("Indicates text for the title of the panel.")]
	[Localizable(true)]
	[Editor("Stimulsoft.Controls.Win.DotNetBar.Design.TextMarkupUIEditor, Stimulsoft.Controls.Win.DotNetBar.Design, Version=10.9.0.8, Culture=neutral,  PublicKeyToken=ebe6666cba19647a", typeof(UITypeEditor))]
	public string TitleText
	{
		get
		{
			return ((Control)m_TitleBar).Text;
		}
		set
		{
			((Control)m_TitleBar).Text = value;
			if (m_VerticalExpandPane != null)
			{
				((Control)m_VerticalExpandPane).Text = value;
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Title")]
	[Description("Gets or sets title style.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ItemStyle TitleStyle => m_TitleBar.Style;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Title")]
	[Description("Gets or sets the title style when mouse hovers over the title.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ItemStyle TitleStyleMouseOver => m_TitleBar.StyleMouseOver;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[NotifyParentProperty(true)]
	[Category("Title")]
	[Description("Gets or sets the Title style when mouse button is pressed on the title.")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public ItemStyle TitleStyleMouseDown => m_TitleBar.StyleMouseDown;

	[Browsable(true)]
	[DefaultValue(26)]
	[Category("Title")]
	[Description("Indicates height of the title portion of the panel.")]
	public int TitleHeight
	{
		get
		{
			return GetTitleHeight();
		}
		set
		{
			SetTitleHeight(value);
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public PanelEx TitlePanel => m_TitleBar;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ButtonItem ExpandButton => m_TitleBar.ExpandChangeButton;

	[DefaultValue(eTitleButtonAlignment.Right)]
	[Category("Expand Button")]
	[Description("Indicates the alignment of expand button.")]
	public eTitleButtonAlignment ExpandButtonAlignment
	{
		get
		{
			return m_TitleBar.ButtonAlignment;
		}
		set
		{
			m_TitleBar.ButtonAlignment = value;
		}
	}

	[Browsable(false)]
	public PanelEx VerticalExpandPanel => m_VerticalExpandPane;

	public event ExpandChangeEventHandler ExpandedChanging;

	public event ExpandChangeEventHandler ExpandedChanged;

	public ExpandablePanel()
	{
		((Control)m_TitleBar).Text = "Title Bar";
		((Control)m_TitleBar).Location = new Point(0, 0);
		((Control)m_TitleBar).Size = new Size(24, 26);
		((Control)m_TitleBar).Dock = (DockStyle)1;
		((Control)this).Controls.Add((Control)(object)m_TitleBar);
		m_TitleBar.ColorScheme = base.ColorScheme;
		m_TitleBar.ExpandedClick += m_TitleBar_ExpandedClick;
		((Control)m_TitleBar).Click += TitleBarClick;
		((Control)this).BackColor = SystemColors.Control;
		UpdateExpandButtonImage();
	}

	private void TitleBarClick(object sender, EventArgs e)
	{
		if (m_ExpandOnTitleClick && !m_TitleBar.ExpandChangeButton.DisplayRectangle.Contains(((Control)m_TitleBar).PointToClient(Control.MousePosition)))
		{
			SetExpanded(!Expanded, eEventSource.Mouse);
		}
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		base.OnControlAdded(e);
		((Control)m_TitleBar).SendToBack();
	}

	private void m_TitleBar_ExpandedClick(object sender, EventArgs e)
	{
		SetExpanded(!Expanded, eEventSource.Mouse);
	}

	private void SetExpanded(bool expanded, eEventSource action)
	{
		ExpandedChangeEventArgs expandedChangeEventArgs = new ExpandedChangeEventArgs(action, expanded);
		InvokeExpandedChanging(expandedChangeEventArgs);
		if (!expandedChangeEventArgs.Cancel)
		{
			m_Expanded = expanded;
			OnExpandedChanged();
			InvokeExpandedChanged(expandedChangeEventArgs);
		}
	}

	private DockStyle GetControlDock(Control c)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		if ((int)c.Dock == 0 || (int)c.Dock == 5)
		{
			return (DockStyle)1;
		}
		return c.Dock;
	}

	private Rectangle GetExpandedBounds()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Invalid comparison between Unknown and I4
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Invalid comparison between Unknown and I4
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Invalid comparison between Unknown and I4
		Rectangle result = m_ExpandedBounds;
		DockStyle controlDock = GetControlDock((Control)(object)m_TitleBar);
		if ((int)controlDock == 3)
		{
			result = new Rectangle(((Control)this).Bounds.Location, m_ExpandedBounds.Size);
		}
		else if ((int)controlDock == 4)
		{
			result = new Rectangle(((Control)this).Bounds.X - (m_ExpandedBounds.Width - ((Control)this).Width), ((Control)this).Bounds.Y, m_ExpandedBounds.Width, m_ExpandedBounds.Height);
		}
		else if ((int)controlDock == 1)
		{
			result = ((m_CollapseDirection == eCollapseDirection.LeftToRight) ? new Rectangle(((Control)this).Bounds.X - (m_ExpandedBounds.Width - ((Control)this).Bounds.Width), ((Control)this).Bounds.Y, m_ExpandedBounds.Width, m_ExpandedBounds.Height) : ((m_CollapseDirection != eCollapseDirection.TopToBottom) ? new Rectangle(((Control)this).Bounds.Location, m_ExpandedBounds.Size) : new Rectangle(((Control)this).Bounds.X, m_ExpandedBounds.Y, m_ExpandedBounds.Width, m_ExpandedBounds.Height)));
		}
		else if ((int)controlDock == 2)
		{
			result = new Rectangle(((Control)this).Bounds.X, ((Control)this).Bounds.Y - (m_ExpandedBounds.Height - ((Control)this).Height), m_ExpandedBounds.Width, m_ExpandedBounds.Height);
		}
		return result;
	}

	private void OnExpandedChanged()
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Expected O, but got Unknown
		if (Expanded)
		{
			m_TitleBar.RenderText = true;
			base.RenderText = true;
			if (!m_ExpandedBounds.IsEmpty)
			{
				if (m_VerticalExpandPane != null)
				{
					DisposeVerticalExpandPane();
				}
				if (_HideControlsWhenCollapsed)
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
				Rectangle expandedBounds = GetExpandedBounds();
				if (AnimationTime == 0 || ((Component)(object)this).DesignMode)
				{
					TypeDescriptor.GetProperties(this)["Bounds"].SetValue(this, expandedBounds);
				}
				else
				{
					Rectangle bounds = ((Control)this).Bounds;
					BarFunctions.AnimateControl((Control)(object)this, show: true, m_AnimationTime, bounds, expandedBounds);
				}
				TypeDescriptor.GetProperties(this)["ExpandedBounds"].SetValue(this, Rectangle.Empty);
			}
		}
		else
		{
			TypeDescriptor.GetProperties(this)["ExpandedBounds"].SetValue(this, ((Control)this).Bounds);
			Point point = ((Control)this).PointToScreen(Point.Empty);
			point = ((((Control)this).Parent == null) ? Point.Empty : ((Control)this).Parent.PointToClient(point));
			Rectangle rectangle = new Rectangle(point, new Size(((Control)m_TitleBar).Width + ((ScrollableControl)this).DockPadding.Left + ((ScrollableControl)this).DockPadding.Right, Math.Max(((Control)m_TitleBar).Height + ((ScrollableControl)this).DockPadding.Top + ((ScrollableControl)this).DockPadding.Bottom, m_TitleBar.ExpandChangeButton.HeightInternal)));
			if (m_CollapseDirection == eCollapseDirection.RightToLeft)
			{
				rectangle = new Rectangle(point, new Size(Math.Max(((Control)m_TitleBar).Height + ((ScrollableControl)this).DockPadding.Top + ((ScrollableControl)this).DockPadding.Bottom, m_TitleBar.ExpandChangeButton.WidthInternal + 6), ((Control)this).Height));
			}
			else if (m_CollapseDirection == eCollapseDirection.LeftToRight)
			{
				Size size = new Size(Math.Max(((Control)m_TitleBar).Height + ((ScrollableControl)this).DockPadding.Top + ((ScrollableControl)this).DockPadding.Bottom, m_TitleBar.ExpandChangeButton.WidthInternal + 6), ((Control)this).Height);
				if (!point.IsEmpty)
				{
					point.X += ((Control)this).Width - size.Width;
				}
				rectangle = new Rectangle(point, size);
			}
			else if (m_CollapseDirection == eCollapseDirection.TopToBottom)
			{
				Size size2 = new Size(((Control)m_TitleBar).Width + ((ScrollableControl)this).DockPadding.Left + ((ScrollableControl)this).DockPadding.Right, Math.Max(((Control)m_TitleBar).Height + ((ScrollableControl)this).DockPadding.Top + ((ScrollableControl)this).DockPadding.Bottom, m_TitleBar.ExpandChangeButton.HeightInternal));
				if (!point.IsEmpty)
				{
					point.Y += ((Control)this).Height - size2.Height;
				}
				rectangle = new Rectangle(point, size2);
			}
			if (AnimationTime == 0 || ((Component)(object)this).DesignMode)
			{
				TypeDescriptor.GetProperties(this)["Bounds"].SetValue(this, rectangle);
			}
			else
			{
				Rectangle bounds2 = ((Control)this).Bounds;
				BarFunctions.AnimateControl((Control)(object)this, show: true, m_AnimationTime, bounds2, rectangle);
			}
			if (m_CollapseDirection == eCollapseDirection.LeftToRight || m_CollapseDirection == eCollapseDirection.RightToLeft)
			{
				m_TitleBar.RenderText = false;
				base.RenderText = false;
			}
			if (_HideControlsWhenCollapsed)
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
			((Control)m_TitleBar).SendToBack();
			if (m_CollapseDirection == eCollapseDirection.LeftToRight || m_CollapseDirection == eCollapseDirection.RightToLeft)
			{
				SetupExpandVerticalPane();
			}
		}
		UpdateExpandButtonImage();
		((Control)this).Refresh();
	}

	private void DisposeVerticalExpandPane()
	{
		if (m_VerticalExpandPane != null)
		{
			((Control)this).Controls.Remove((Control)(object)m_VerticalExpandPane);
			((Component)(object)m_VerticalExpandPane).Dispose();
			m_VerticalExpandPane = null;
		}
	}

	private void UpdateExpandButtonImage()
	{
		Image image = null;
		Image hoverImage = null;
		if (Expanded)
		{
			if (m_ButtonImageCollapse != null)
			{
				image = m_ButtonImageCollapse;
			}
			else if (BarFunctions.IsOffice2007Style(base.ColorSchemeStyle))
			{
				if (m_CollapseDirection == eCollapseDirection.BottomToTop || m_CollapseDirection == eCollapseDirection.RightToLeft)
				{
					image = m_Office2007ButtonImageCollapse;
					hoverImage = m_Office2007ButtonImageCollapseMouseOver;
				}
				else
				{
					image = m_Office2007ButtonImageExpand;
					hoverImage = m_Office2007ButtonImageExpandMouseOver;
				}
			}
			else if (m_CollapseDirection == eCollapseDirection.BottomToTop)
			{
				image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.ExpandTitle.png");
			}
			else if (m_CollapseDirection == eCollapseDirection.TopToBottom)
			{
				image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.CollapseTitle.png");
			}
			else if (m_CollapseDirection == eCollapseDirection.LeftToRight)
			{
				Bitmap val = BarFunctions.LoadBitmap("SystemImages.ExpandTitle.png");
				((Image)val).RotateFlip((RotateFlipType)3);
				image = (Image)(object)val;
			}
			else if (m_CollapseDirection == eCollapseDirection.RightToLeft)
			{
				Bitmap val2 = BarFunctions.LoadBitmap("SystemImages.ExpandTitle.png");
				((Image)val2).RotateFlip((RotateFlipType)1);
				image = (Image)(object)val2;
			}
		}
		else if (m_ButtonImageExpand != null)
		{
			image = m_ButtonImageExpand;
		}
		else if (BarFunctions.IsOffice2007Style(base.ColorSchemeStyle))
		{
			if (m_CollapseDirection == eCollapseDirection.BottomToTop || m_CollapseDirection == eCollapseDirection.RightToLeft)
			{
				image = m_Office2007ButtonImageExpand;
				hoverImage = m_Office2007ButtonImageExpandMouseOver;
			}
			else
			{
				image = m_Office2007ButtonImageCollapse;
				hoverImage = m_Office2007ButtonImageCollapseMouseOver;
			}
		}
		else if (m_CollapseDirection == eCollapseDirection.BottomToTop)
		{
			image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.CollapseTitle.png");
		}
		else if (m_CollapseDirection == eCollapseDirection.TopToBottom)
		{
			image = (Image)(object)BarFunctions.LoadBitmap("SystemImages.ExpandTitle.png");
		}
		else if (m_CollapseDirection == eCollapseDirection.LeftToRight)
		{
			Bitmap val3 = BarFunctions.LoadBitmap("SystemImages.CollapseTitle.png");
			((Image)val3).RotateFlip((RotateFlipType)3);
			image = (Image)(object)val3;
		}
		else if (m_CollapseDirection == eCollapseDirection.RightToLeft)
		{
			Bitmap val4 = BarFunctions.LoadBitmap("SystemImages.CollapseTitle.png");
			((Image)val4).RotateFlip((RotateFlipType)1);
			image = (Image)(object)val4;
		}
		m_TitleBar.ExpandChangeButton.Image = image;
		m_TitleBar.ExpandChangeButton.HoverImage = hoverImage;
		m_TitleBar.UpdateButtonPosition();
		((Control)m_TitleBar).Invalidate();
	}

	private void UpdateOffice2007Images()
	{
		if (m_Office2007ButtonImageCollapse != null)
		{
			m_Office2007ButtonImageCollapse.Dispose();
		}
		if (m_Office2007ButtonImageExpand != null)
		{
			m_Office2007ButtonImageExpand.Dispose();
		}
		if (m_Office2007ButtonImageCollapseMouseOver != null)
		{
			m_Office2007ButtonImageCollapseMouseOver.Dispose();
		}
		if (m_Office2007ButtonImageExpandMouseOver != null)
		{
			m_Office2007ButtonImageExpandMouseOver.Dispose();
		}
		bool verticalCollapse = m_CollapseDirection == eCollapseDirection.TopToBottom || m_CollapseDirection == eCollapseDirection.BottomToTop;
		m_Office2007ButtonImageCollapse = UIGraphics.CreateExpandButtonImage(collapse: true, base.ColorScheme.PanelText, verticalCollapse);
		m_Office2007ButtonImageCollapseMouseOver = UIGraphics.CreateExpandButtonImage(collapse: true, base.ColorScheme.ItemHotText, verticalCollapse);
		m_Office2007ButtonImageExpand = UIGraphics.CreateExpandButtonImage(collapse: false, base.ColorScheme.PanelText, verticalCollapse);
		m_Office2007ButtonImageExpandMouseOver = UIGraphics.CreateExpandButtonImage(collapse: false, base.ColorScheme.ItemHotText, verticalCollapse);
	}

	private void InvokeExpandedChanging(ExpandedChangeEventArgs e)
	{
		if (this.ExpandedChanging != null)
		{
			this.ExpandedChanging(this, e);
		}
	}

	private void InvokeExpandedChanged(ExpandedChangeEventArgs e)
	{
		if (this.ExpandedChanged != null)
		{
			this.ExpandedChanged(this, e);
		}
	}

	protected override void OnColorSchemeChanged()
	{
		base.OnColorSchemeChanged();
		m_TitleBar.ColorScheme = GetColorScheme();
		UpdateAutoGeneratedImages();
	}

	private void UpdateAutoGeneratedImages()
	{
		DisposeAutoGeneratedImages();
		if (BarFunctions.IsOffice2007Style(base.ColorSchemeStyle))
		{
			UpdateOffice2007Images();
		}
		UpdateExpandButtonImage();
	}

	private void DisposeAutoGeneratedImages()
	{
		if (m_Office2007ButtonImageCollapse != null)
		{
			m_Office2007ButtonImageCollapse.Dispose();
			m_Office2007ButtonImageCollapse = null;
		}
		if (m_Office2007ButtonImageCollapseMouseOver != null)
		{
			m_Office2007ButtonImageCollapseMouseOver.Dispose();
			m_Office2007ButtonImageCollapseMouseOver = null;
		}
		if (m_Office2007ButtonImageExpand != null)
		{
			m_Office2007ButtonImageExpand.Dispose();
			m_Office2007ButtonImageExpand = null;
		}
		if (m_Office2007ButtonImageExpandMouseOver != null)
		{
			m_Office2007ButtonImageExpandMouseOver.Dispose();
			m_Office2007ButtonImageExpandMouseOver = null;
		}
	}

	private int GetTitleHeight()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		if ((int)((Control)m_TitleBar).Dock == 3 || (int)((Control)m_TitleBar).Dock == 4)
		{
			return ((Control)m_TitleBar).Width;
		}
		return ((Control)m_TitleBar).Height;
	}

	private void SetTitleHeight(int value)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		if ((int)((Control)m_TitleBar).Dock == 3 || (int)((Control)m_TitleBar).Dock == 4)
		{
			((Control)m_TitleBar).Width = value;
		}
		else
		{
			((Control)m_TitleBar).Height = value;
		}
	}

	private void OnCollapseDirectionChanged()
	{
		UpdateAutoGeneratedImages();
	}

	protected override void Dispose(bool disposing)
	{
		DisposeAutoGeneratedImages();
		if (BarUtilities.DisposeItemImages && !((Component)(object)this).DesignMode)
		{
			BarUtilities.DisposeImage(ref m_ButtonImageCollapse);
			BarUtilities.DisposeImage(ref m_ButtonImageExpand);
		}
		base.Dispose(disposing);
	}

	private void SetupExpandVerticalPane()
	{
		m_VerticalExpandPane = new PanelEx();
		m_VerticalExpandPane.ColorSchemeStyle = base.ColorSchemeStyle;
		m_VerticalExpandPane.ColorScheme = base.ColorScheme;
		m_VerticalExpandPane.ApplyButtonStyle();
		m_VerticalExpandPane.Style.VerticalText = true;
		m_VerticalExpandPane.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		m_VerticalExpandPane.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		m_VerticalExpandPane.Style.ResetBackColor2();
		m_VerticalExpandPane.StyleMouseOver.VerticalText = true;
		m_VerticalExpandPane.StyleMouseDown.VerticalText = true;
		((Control)m_VerticalExpandPane).Dock = (DockStyle)5;
		((Control)m_VerticalExpandPane).Text = ((Control)TitlePanel).Text;
		((Control)this).Controls.Add((Control)(object)m_VerticalExpandPane);
		if (TitleStyle.Font != null)
		{
			((Control)m_VerticalExpandPane).Font = TitleStyle.Font;
		}
		else if (((Control)TitlePanel).Font != null)
		{
			((Control)m_VerticalExpandPane).Font = ((Control)TitlePanel).Font;
		}
		((Control)m_VerticalExpandPane).Click += VerticalExpandPaneClick;
		((Control)m_VerticalExpandPane).BringToFront();
	}

	private void VerticalExpandPaneClick(object sender, EventArgs e)
	{
		Expanded = !Expanded;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeExpandedBounds()
	{
		return !m_ExpandedBounds.IsEmpty;
	}

	public void ResetTitleStyle()
	{
		m_TitleBar.ResetStyle();
	}

	public void ResetTitleStyleMouseOver()
	{
		m_TitleBar.ResetStyleMouseOver();
	}

	public void ResetTitleStyleMouseDown()
	{
		m_TitleBar.ResetStyleMouseDown();
	}

	protected override void OnAntiAliasChanged()
	{
		m_TitleBar.AntiAlias = base.AntiAlias;
	}
}
