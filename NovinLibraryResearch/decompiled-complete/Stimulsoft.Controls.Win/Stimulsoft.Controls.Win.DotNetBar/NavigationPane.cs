using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public class NavigationPane : UserControl
{
	private PanelExTitle panelTitle;

	private NavigationBar navBar;

	private Container components;

	private int m_NavigationBarHeight;

	private bool m_CanCollapse;

	private bool m_Expanded = true;

	private bool m_ExpandedDelayed = true;

	private Image m_DefaultCollapseImage;

	private Image m_DefaultExpandImage;

	private Image m_DefaultCollapseImageMouseOver;

	private Image m_DefaultExpandImageMouseOver;

	private Image m_TitleImageCollapse;

	private Image m_TitleImageExpand;

	private Image m_TitleImageCollapseMouseOver;

	private Image m_TitleImageExpandMouseOver;

	private int m_ExpandedSize;

	private int m_CollapsedSize;

	private PanelEx m_PanePopupPanel;

	private int m_AnimationTime = 100;

	private bool m_PopupPaneVisible;

	private PanelEx m_PopupPanelContainer;

	private bool m_LayoutSuspended;

	private bool m_Hiding;

	[Browsable(false)]
	public int ExpandedSize => m_ExpandedSize;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Browsable(false)]
	public SubItemsCollection Items => navBar.Items;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[DefaultValue(32)]
	public int NavigationBarHeight
	{
		get
		{
			if (!((Component)this).DesignMode && navBar != null && navBar.Items.Count > 0)
			{
				return ((Control)navBar).Height;
			}
			return m_NavigationBarHeight;
		}
		set
		{
			m_NavigationBarHeight = value;
			((Control)navBar).Height = m_NavigationBarHeight;
		}
	}

	[Browsable(false)]
	[DevCoBrowsable(false)]
	public NavigationBar NavigationBar => navBar;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates size of the image that will be use to resize images to when button button is on the bottom summary line of navigation bar and AutoSizeButtonImage=true.")]
	public Size ImageSizeSummaryLine
	{
		get
		{
			return navBar.ImageSizeSummaryLine;
		}
		set
		{
			navBar.ImageSizeSummaryLine = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates Configure Buttons button is visible.")]
	[DefaultValue(true)]
	public bool ConfigureItemVisible
	{
		get
		{
			return navBar.ConfigureItemVisible;
		}
		set
		{
			navBar.ConfigureItemVisible = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether Show More Buttons and Show Fewer Buttons menu items are visible on Configure buttons menu.")]
	[DefaultValue(true)]
	public bool ConfigureShowHideVisible
	{
		get
		{
			return navBar.ConfigureShowHideVisible;
		}
		set
		{
			navBar.ConfigureShowHideVisible = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Gets or sets whether Navigation Pane Options menu item is visible on Configure buttons menu.")]
	[DefaultValue(true)]
	public bool ConfigureNavOptionsVisible
	{
		get
		{
			return navBar.ConfigureNavOptionsVisible;
		}
		set
		{
			navBar.ConfigureNavOptionsVisible = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Behavior")]
	[Description("Indicates whether Navigation Pane Add/Remove Buttons menu item is visible on Configure buttons menu.")]
	[DefaultValue(true)]
	public bool ConfigureAddRemoveVisible
	{
		get
		{
			return navBar.ConfigureAddRemoveVisible;
		}
		set
		{
			navBar.ConfigureAddRemoveVisible = value;
		}
	}

	public ButtonItem CheckedButton => navBar.CheckedButton;

	public NavigationPanePanel SelectedPanel => GetPanel(CheckedButton);

	[Browsable(true)]
	[Category("Data")]
	[DefaultValue(null)]
	[Description("ImageList for images used on Items.")]
	public ImageList Images
	{
		get
		{
			return navBar.Images;
		}
		set
		{
			navBar.Images = value;
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Category("Appearance")]
	[Description("PanelEx control that is used to display title.")]
	public PanelEx TitlePanel => panelTitle;

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates the padding in pixels at the top portion of the item.")]
	[DefaultValue(4)]
	public int ItemPaddingTop
	{
		get
		{
			return navBar.ItemPaddingTop;
		}
		set
		{
			navBar.ItemPaddingTop = value;
			if (((Component)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Layout")]
	[Description("Indicates the padding in pixels at the bottom of the item.")]
	[DefaultValue(4)]
	public int ItemPaddingBottom
	{
		get
		{
			return navBar.ItemPaddingBottom;
		}
		set
		{
			navBar.ItemPaddingBottom = value;
			if (((Component)this).DesignMode)
			{
				RecalcLayout();
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Indicates whether images are automatically resized to size specified in ImageSizeSummaryLine when button is on the bottom summary line of navigation bar.")]
	[DefaultValue(true)]
	public bool AutoSizeButtonImage
	{
		get
		{
			return navBar.AutoSizeButtonImage;
		}
		set
		{
			navBar.AutoSizeButtonImage = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string LayoutDefinition
	{
		get
		{
			return navBar.LayoutDefinition;
		}
		set
		{
			navBar.LayoutDefinition = value;
		}
	}

	[DefaultValue(false)]
	[Browsable(true)]
	[Category("Appearance")]
	[Description("Gets or sets whether anti-aliasing is used while painting.")]
	public bool AntiAlias
	{
		get
		{
			return navBar.AntiAlias;
		}
		set
		{
			navBar.AntiAlias = value;
			if (panelTitle != null)
			{
				panelTitle.AntiAlias = value;
			}
			if (m_PanePopupPanel != null)
			{
				m_PanePopupPanel.AntiAlias = value;
			}
			if (m_PopupPanelContainer != null)
			{
				m_PopupPanelContainer.AntiAlias = value;
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(100)]
	[Category("Expand")]
	[Description("Indicates animation time in milliseconds when control is expanded or collapsed.")]
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

	[Browsable(true)]
	[Category("Title")]
	[DefaultValue(false)]
	[Description("Indicates whether navigation pane can be collapsed.")]
	public bool CanCollapse
	{
		get
		{
			return m_CanCollapse;
		}
		set
		{
			m_CanCollapse = value;
			OnCanCollapseChanged();
			panelTitle.ExpandButtonVisible = m_CanCollapse;
		}
	}

	[Browsable(true)]
	[DefaultValue(eTitleButtonAlignment.Right)]
	[Category("Title")]
	[Description("Indicates the alignment of expand button inside title bar.")]
	public eTitleButtonAlignment TitleButtonAlignment
	{
		get
		{
			return panelTitle.ButtonAlignment;
		}
		set
		{
			panelTitle.ButtonAlignment = value;
			UpdateExpandButton();
		}
	}

	[Browsable(true)]
	[Category("Title")]
	[DefaultValue(true)]
	[Description("Indicates whether navigation pane can be collapsed.")]
	public bool Expanded
	{
		get
		{
			return m_Expanded;
		}
		set
		{
			if (m_LayoutSuspended)
			{
				m_ExpandedDelayed = value;
			}
			else if (m_Expanded != value)
			{
				SetExpanded(value, eEventSource.Code);
			}
		}
	}

	[Browsable(false)]
	public PanelEx CollapsedInnerPanel => m_PanePopupPanel;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool PopupSelectedPaneVisible
	{
		get
		{
			return m_PopupPaneVisible;
		}
		set
		{
			if (m_PopupPaneVisible != value)
			{
				if (value)
				{
					PopupSelectedPane();
				}
				else
				{
					HideSelectedPopupPane();
				}
			}
		}
	}

	[Browsable(true)]
	[DevCoBrowsable(true)]
	[Category("Appearance")]
	[Description("Specifies the visual style of the control.")]
	[DefaultValue(eDotNetBarStyle.Office2003)]
	public eDotNetBarStyle Style
	{
		get
		{
			return navBar.Style;
		}
		set
		{
			navBar.Style = value;
			ColorSchemeStyleChanged();
		}
	}

	[Description("Occurs when Item is clicked.")]
	[Category("Navigation Pane Events")]
	public event EventHandler ItemClick;

	[Description("Occurs when Item is clicked.")]
	[Category("Navigation Pane Events")]
	public event PanelChangingEventHandler PanelChanging;

	public event DotNetBarManager.LocalizeStringEventHandler LocalizeString;

	public event PanelPopupEventHandler BeforePanelPopup;

	public event EventHandler AfterPanelPopup;

	public event EventHandler PanelPopupClosed;

	public event ExpandChangeEventHandler ExpandedChanging;

	public event ExpandChangeEventHandler ExpandedChanged;

	public event EventHandler OptionsDialogClosed;

	public NavigationPane()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		navBar.ItemAdded += NavBarItemAdded;
		navBar.ItemRemoved += NavBarItemRemoved;
		navBar.ApplicationMouseDown += ApplicationMouseDown;
		navBar.LocalizeString += navBar_LocalizeString;
		navBar.OptionsDialogClosed += NavBarOptionsDialogClosed;
		navBar.SplitterVisible = true;
		m_NavigationBarHeight = ((Control)navBar).Height;
		panelTitle.ExpandedClick += panelEx1_ExpandedClick;
		panelTitle.ExpandChangeButton.ImagePaddingHorizontal = 2;
		panelTitle.ExpandChangeButton.ImagePaddingVertical = 2;
		StyleManager.Register((Control)(object)this);
	}

	private void NavBarOptionsDialogClosed(object sender, EventArgs e)
	{
		OnOptionsDialogClosed(e);
	}

	private void navBar_LocalizeString(object sender, LocalizeEventArgs e)
	{
		if (this.LocalizeString != null)
		{
			this.LocalizeString(this, e);
		}
	}

	protected virtual void OnOptionsDialogClosed(EventArgs e)
	{
		this.OptionsDialogClosed?.Invoke(this, e);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (m_DefaultCollapseImage != null)
			{
				m_DefaultCollapseImage.Dispose();
			}
			if (m_DefaultExpandImage != null)
			{
				m_DefaultExpandImage.Dispose();
			}
			if (components != null)
			{
				components.Dispose();
			}
		}
		StyleManager.Unregister((Control)(object)this);
		((ContainerControl)this).Dispose(disposing);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		if (navBar.ItemsContainer.EffectiveStyle == eDotNetBarStyle.Office2010)
		{
			navBar.GetRenderer()?.DrawNavPaneButtonBackground(new NavPaneRenderEventArgs(e.Graphics, ((Control)this).ClientRectangle));
			return;
		}
		SolidBrush val = new SolidBrush(NavigationBar.GetColorScheme().BarBackground);
		try
		{
			e.Graphics.FillRectangle((Brush)(object)val, ((Control)this).ClientRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		Pen val2 = new Pen(NavigationBar.GetColorScheme().PanelBorder, 1f);
		try
		{
			e.Graphics.DrawRectangle(val2, new Rectangle(0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1));
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void SetDesignMode()
	{
		navBar.SetDesignMode(((Component)this).DesignMode);
	}

	public virtual void RecalcLayout()
	{
		navBar.RecalcLayout();
	}

	private void NavBarItemAdded(object sender, EventArgs e)
	{
		if (sender is ButtonItem { IsOnCustomizeMenu: false } buttonItem)
		{
			buttonItem.CheckedChanged += ItemCheckedChanged;
			buttonItem.TextChanged += ItemTextChanged;
			buttonItem.VisibleChanged += ItemVisibleChanged;
			if (buttonItem.Checked)
			{
				buttonItem.SetChecked(b: false);
				buttonItem.Checked = true;
			}
		}
	}

	private void ItemVisibleChanged(object sender, EventArgs e)
	{
		if (!(sender is ButtonItem { IsOnCustomizeMenu: false, Checked: not false, Visible: false }))
		{
			return;
		}
		foreach (BaseItem item in Items)
		{
			if (item is ButtonItem { Visible: not false } buttonItem2)
			{
				buttonItem2.Checked = true;
				break;
			}
		}
	}

	private void NavBarItemRemoved(object sender, ItemRemovedEventArgs e)
	{
		if (sender is ButtonItem buttonItem)
		{
			try
			{
				buttonItem.CheckedChanged -= ItemCheckedChanged;
				buttonItem.TextChanged -= ItemTextChanged;
				buttonItem.VisibleChanged -= ItemVisibleChanged;
			}
			catch
			{
			}
		}
	}

	private void ItemCheckedChanged(object sender, EventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		if (!(sender is ButtonItem { IsOnCustomizeMenu: false } buttonItem))
		{
			return;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (!(val is NavigationPanePanel))
			{
				continue;
			}
			NavigationPanePanel navigationPanePanel = val as NavigationPanePanel;
			if (navigationPanePanel.ParentItem != buttonItem)
			{
				continue;
			}
			if (buttonItem.Checked)
			{
				if (m_Expanded)
				{
					if (!((Component)this).DesignMode)
					{
						((Control)navigationPanePanel).Visible = true;
					}
					((Control)navigationPanePanel).BringToFront();
				}
				((Control)panelTitle).Text = buttonItem.Text;
				if (m_PanePopupPanel != null)
				{
					((Control)m_PanePopupPanel).Text = ((Control)panelTitle).Text;
				}
			}
			else if (m_Expanded)
			{
				if (!((Component)this).DesignMode)
				{
					((Control)navigationPanePanel).Visible = false;
				}
				else
				{
					((Control)navigationPanePanel).SendToBack();
				}
			}
			break;
		}
	}

	private void ItemTextChanged(object sender, EventArgs e)
	{
		if (sender is ButtonItem { Checked: not false } buttonItem)
		{
			((Control)panelTitle).Text = buttonItem.Text;
			if (m_PanePopupPanel != null)
			{
				((Control)m_PanePopupPanel).Text = buttonItem.Text;
			}
		}
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		((Control)this).OnControlAdded(e);
		if (((Component)this).DesignMode || !(e.Control is NavigationPanePanel navigationPanePanel))
		{
			return;
		}
		if (navigationPanePanel.ParentItem != null && m_Expanded)
		{
			if (Items.Contains(navigationPanePanel.ParentItem) && navigationPanePanel.ParentItem is ButtonItem)
			{
				((Control)navigationPanePanel).Visible = ((ButtonItem)navigationPanePanel.ParentItem).Checked;
				if (((Control)navigationPanePanel).Visible)
				{
					((Control)navigationPanePanel).BringToFront();
				}
			}
			else
			{
				((Control)navigationPanePanel).Visible = false;
			}
		}
		else
		{
			((Control)navigationPanePanel).Visible = false;
		}
	}

	protected override void OnSystemColorsChanged(EventArgs e)
	{
		((Control)this).OnSystemColorsChanged(e);
		Application.DoEvents();
		if (m_CanCollapse)
		{
			UpdateExpandCollapseImages();
			UpdateExpandButton();
		}
	}

	public NavigationPanePanel GetPanel(ButtonItem item)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		if (item == null)
		{
			return null;
		}
		foreach (Control item2 in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item2;
			if (val is NavigationPanePanel navigationPanePanel && navigationPanePanel.ParentItem == item)
			{
				return navigationPanePanel;
			}
		}
		return null;
	}

	private void OnNavBarResize(object sender, EventArgs e)
	{
		if (((Component)this).DesignMode)
		{
			m_NavigationBarHeight = ((Control)navBar).Height;
		}
	}

	protected override void OnLayout(LayoutEventArgs e)
	{
		((ContainerControl)this).OnLayout(e);
		UpdatePanePopupPanelSize();
	}

	public void SuspendLayout()
	{
		m_LayoutSuspended = true;
		((Control)this).SuspendLayout();
	}

	public void ResumeLayout(bool performLayout)
	{
		m_LayoutSuspended = false;
		((Control)navBar).Height = m_NavigationBarHeight;
		((Control)navBar).SendToBack();
		((Control)this).ResumeLayout(true);
		Expanded = m_ExpandedDelayed;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeImageSizeSummaryLine()
	{
		return navBar.ShouldSerializeImageSizeSummaryLine();
	}

	public void ShowMoreButtons()
	{
		navBar.ShowMoreButtons();
	}

	public void ShowFewerButtons()
	{
		navBar.ShowFewerButtons();
	}

	public void SaveLayout(string fileName)
	{
		navBar.SaveLayout(fileName);
	}

	public void SaveLayout(XmlElement xmlParent)
	{
		navBar.SaveLayout(xmlParent);
	}

	private void SetExpanded(bool expanded, eEventSource source)
	{
		if (this.ExpandedChanging != null)
		{
			ExpandedChangeEventArgs expandedChangeEventArgs = new ExpandedChangeEventArgs(source, expanded);
			this.ExpandedChanging(this, expandedChangeEventArgs);
			if (expandedChangeEventArgs.Cancel)
			{
				return;
			}
		}
		m_Expanded = expanded;
		OnExpandedChanged();
		if (this.ExpandedChanged != null)
		{
			ExpandedChangeEventArgs e = new ExpandedChangeEventArgs(source, expanded);
			this.ExpandedChanged(this, e);
		}
	}

	private void OnExpandedChanged()
	{
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Invalid comparison between Unknown and I4
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Invalid comparison between Unknown and I4
		if (m_Expanded)
		{
			foreach (BaseItem item in navBar.Items)
			{
				item.RenderText = true;
			}
			panelTitle.DrawText = true;
			PopupSelectedPaneVisible = false;
			DestroyPanePopup();
			if (AnimationTime == 0 || ((Component)this).DesignMode)
			{
				TypeDescriptor.GetProperties(this)["Width"].SetValue(this, m_ExpandedSize);
			}
			else
			{
				Rectangle rectEnd = new Rectangle(((Control)this).Location, new Size(m_ExpandedSize, ((Control)this).Height));
				if ((int)((Control)this).Dock == 4)
				{
					rectEnd.Offset(-m_ExpandedSize, 0);
				}
				BarFunctions.AnimateControl((Control)(object)this, show: true, m_AnimationTime, ((Control)this).Bounds, rectEnd);
			}
		}
		else
		{
			panelTitle.DrawText = false;
			m_ExpandedSize = ((Control)this).Width;
			if (AnimationTime == 0 || ((Component)this).DesignMode)
			{
				TypeDescriptor.GetProperties(this)["Width"].SetValue(this, GetCollapsedSize());
			}
			else
			{
				Rectangle rectEnd2 = new Rectangle(((Control)this).Location, new Size(GetCollapsedSize(), ((Control)this).Height));
				if ((int)((Control)this).Dock == 4)
				{
					rectEnd2.Offset(((Control)this).Width - rectEnd2.Width, 0);
				}
				BarFunctions.AnimateControl((Control)(object)this, show: true, m_AnimationTime, ((Control)this).Bounds, rectEnd2);
			}
			SetupPanePopup();
			foreach (BaseItem item2 in navBar.Items)
			{
				item2.RenderText = false;
			}
			((Control)navBar).Invalidate();
		}
		UpdateExpandButton();
	}

	private int GetCollapsedSize()
	{
		int num = Math.Max(((Control)panelTitle).Font.Height + 10, 34);
		if (m_CollapsedSize > 0)
		{
			return m_CollapsedSize;
		}
		if (!(navBar.ItemsContainer.GetFirstVisibleItem() is ButtonItem { ImageSize: var imageSize } buttonItem))
		{
			return num;
		}
		if (imageSize.IsEmpty)
		{
			return num;
		}
		CompositeImage image = buttonItem.GetImage(ImageState.Default, Color.Black);
		if (image != null)
		{
			return Math.Max(num, image.RealWidth + 10);
		}
		return num;
	}

	private void panelEx1_ExpandedClick(object sender, EventArgs e)
	{
		SetExpanded(!Expanded, eEventSource.Mouse);
	}

	private void ApplicationMouseDown(object sender, HandlerMessageEventArgs e)
	{
		if (!PopupSelectedPaneVisible)
		{
			return;
		}
		string className = NativeFunctions.GetClassName(e.hWnd);
		className = className.ToLower();
		if (className.IndexOf("combolbox") >= 0)
		{
			return;
		}
		for (Control val = Control.FromChildHandle(e.hWnd); val != null; val = val.Parent)
		{
			if (val == m_PopupPanelContainer || val == m_PanePopupPanel)
			{
				return;
			}
		}
		PopupSelectedPaneVisible = false;
	}

	private void SetupPanePopup()
	{
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		m_PanePopupPanel = new PanelEx();
		m_PanePopupPanel.ColorSchemeStyle = navBar.Style;
		m_PanePopupPanel.ColorScheme = navBar.GetColorScheme();
		m_PanePopupPanel.ApplyButtonStyle();
		m_PanePopupPanel.Style.VerticalText = true;
		m_PanePopupPanel.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		m_PanePopupPanel.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		m_PanePopupPanel.Style.ResetBackColor2();
		m_PanePopupPanel.StyleMouseOver.VerticalText = true;
		m_PanePopupPanel.StyleMouseDown.VerticalText = true;
		((Control)m_PanePopupPanel).Font = ((Control)panelTitle).Font;
		m_PanePopupPanel.AntiAlias = AntiAlias;
		UpdatePanePopupPanelSize();
		if (CheckedButton != null)
		{
			((Control)m_PanePopupPanel).Text = CheckedButton.Text;
		}
		else
		{
			((Control)m_PanePopupPanel).Text = "Navigation Pane";
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (val is NavigationPanePanel)
			{
				val.Visible = false;
			}
		}
		((Control)this).Controls.Add((Control)(object)m_PanePopupPanel);
		((Control)m_PanePopupPanel).Click += PanePopupPanelClick;
	}

	private void PanePopupPanelClick(object sender, EventArgs e)
	{
		PopupSelectedPaneVisible = !PopupSelectedPaneVisible;
	}

	private void PopupSelectedPane()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Invalid comparison between Unknown and I4
		if (m_PopupPaneVisible)
		{
			return;
		}
		Control val = null;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val2 = item;
			if (val2 is NavigationPanePanel && ((NavigationPanePanel)(object)val2).ParentItem == CheckedButton)
			{
				val = val2;
				break;
			}
		}
		if (val == null)
		{
			return;
		}
		int height = ((Control)navBar).Top - ((Control)panelTitle).Bottom;
		Rectangle rectangle = new Rectangle(((Control)this).Width, ((Control)panelTitle).Bottom, m_ExpandedSize, height);
		Rectangle rectStart = new Rectangle(((Control)this).Width, ((Control)panelTitle).Bottom, 16, height);
		if (this.BeforePanelPopup != null)
		{
			PanelPopupEventArgs panelPopupEventArgs = new PanelPopupEventArgs(rectangle);
			this.BeforePanelPopup(this, panelPopupEventArgs);
			if (panelPopupEventArgs.Cancel)
			{
				return;
			}
			rectangle = panelPopupEventArgs.PopupBounds;
		}
		Control val3 = (Control)(object)((Control)this).FindForm();
		if (val3 == null)
		{
			val3 = ((Control)this).TopLevelControl;
		}
		if (val3 == null)
		{
			val3 = ((Control)this).Parent;
		}
		m_PopupPanelContainer = new PanelEx();
		((Control)m_PopupPanelContainer).Visible = false;
		m_PopupPanelContainer.ApplyLabelStyle();
		m_PopupPanelContainer.AntiAlias = AntiAlias;
		val3.Controls.Add((Control)(object)m_PopupPanelContainer);
		((Control)m_PopupPanelContainer).BringToFront();
		((Control)this).Controls.Remove(val);
		if (val is NavigationPanePanel)
		{
			NavigationPanePanel navigationPanePanel = (NavigationPanePanel)(object)val;
			((ScrollableControl)navigationPanePanel).DockPadding.Bottom = 1;
			if (navigationPanePanel.Style.Border == eBorderType.None)
			{
				m_PopupPanelContainer.Style.BorderSide = eBorderSide.All;
				m_PopupPanelContainer.Style.Border = eBorderType.SingleLine;
				m_PopupPanelContainer.Style.BorderWidth = 1;
				m_PopupPanelContainer.Style.BorderColor.Color = panelTitle.Style.BorderColor.Color;
				((ScrollableControl)m_PopupPanelContainer).DockPadding.All = 1;
			}
		}
		((Control)m_PopupPanelContainer).Controls.Add(val);
		val.Visible = true;
		if ((int)((Control)this).Dock == 4)
		{
			rectangle.Offset(-(((Control)this).Width + rectangle.Width), 0);
			rectStart.Offset(-(((Control)this).Width + rectStart.Width), 0);
		}
		Point point = ((Control)this).PointToScreen(rectangle.Location);
		rectangle.Location = val3.PointToClient(point);
		point = ((Control)this).PointToScreen(rectStart.Location);
		rectStart.Location = val3.PointToClient(point);
		if (AnimationTime == 0)
		{
			((Control)m_PopupPanelContainer).Bounds = rectangle;
			((Control)m_PopupPanelContainer).Visible = true;
		}
		else
		{
			BarFunctions.AnimateControl((Control)(object)m_PopupPanelContainer, show: true, AnimationTime, rectStart, rectangle);
		}
		((Control)m_PopupPanelContainer).Focus();
		((Control)m_PopupPanelContainer).Leave += m_PopupPanelContainer_Leave;
		m_PopupPaneVisible = true;
		if (this.AfterPanelPopup != null)
		{
			this.AfterPanelPopup(this, new EventArgs());
		}
	}

	private void m_PopupPanelContainer_Leave(object sender, EventArgs e)
	{
		if (PopupSelectedPaneVisible)
		{
			PopupSelectedPaneVisible = false;
		}
	}

	private void HideSelectedPopupPane()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Invalid comparison between Unknown and I4
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		if (!m_PopupPaneVisible || m_Hiding)
		{
			return;
		}
		m_Hiding = true;
		try
		{
			if (AnimationTime == 0)
			{
				((Control)m_PopupPanelContainer).Hide();
			}
			else
			{
				Rectangle rectEnd = new Rectangle(((Control)this).Width, ((Control)this).Top, 16, ((Control)this).Height);
				if ((int)((Control)this).Dock == 4)
				{
					rectEnd.Offset(-(((Control)this).Width + rectEnd.Width), 0);
				}
				Point point = ((Control)this).PointToScreen(rectEnd.Location);
				rectEnd.Location = ((Control)m_PopupPanelContainer).Parent.PointToClient(point);
				BarFunctions.AnimateControl((Control)(object)m_PopupPanelContainer, show: false, AnimationTime, ((Control)m_PopupPanelContainer).Bounds, rectEnd);
			}
			Control val = null;
			foreach (Control item in (ArrangedElementCollection)((Control)m_PopupPanelContainer).Controls)
			{
				Control val2 = item;
				if (val2 is NavigationPanePanel)
				{
					val2.Visible = false;
					((Control)m_PopupPanelContainer).Controls.Remove(val2);
					val = val2;
					break;
				}
			}
			if (val is NavigationPanePanel)
			{
				((ScrollableControl)(NavigationPanePanel)(object)val).DockPadding.Bottom = 0;
			}
			((Control)this).Controls.Add(val);
			((Control)m_PopupPanelContainer).Leave -= m_PopupPanelContainer_Leave;
			((Component)(object)m_PopupPanelContainer).Dispose();
			m_PopupPanelContainer = null;
			m_PopupPaneVisible = false;
			OnPanelPopupClosed(EventArgs.Empty);
		}
		finally
		{
			m_Hiding = false;
		}
	}

	protected virtual void OnPanelPopupClosed(EventArgs e)
	{
		this.PanelPopupClosed?.Invoke(this, e);
	}

	private void UpdatePanePopupPanelSize()
	{
		if (m_PanePopupPanel != null)
		{
			int num = 3;
			((Control)m_PanePopupPanel).Bounds = new Rectangle(new Point(((Control)this).ClientRectangle.X + num, ((Control)panelTitle).Bottom + num), new Size(((Control)this).ClientRectangle.Width - num * 2, ((Control)navBar).Top - ((Control)panelTitle).Bottom - num * 2));
		}
	}

	private void DestroyPanePopup()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		if (m_PanePopupPanel != null)
		{
			((Control)this).Controls.Remove((Control)(object)m_PanePopupPanel);
			((Component)(object)m_PanePopupPanel).Dispose();
			m_PanePopupPanel = null;
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (val is NavigationPanePanel && ((NavigationPanePanel)(object)val).ParentItem == CheckedButton)
			{
				val.Visible = true;
				if (((Component)this).DesignMode)
				{
					TypeDescriptor.GetProperties(val)["Visible"].SetValue(val, true);
				}
				val.BringToFront();
				break;
			}
		}
	}

	private void OnCanCollapseChanged()
	{
		if (m_CanCollapse)
		{
			UpdateExpandCollapseImages();
		}
		UpdateExpandButton();
	}

	private Image GetCollapseImage(bool mouseOverImage)
	{
		if (mouseOverImage)
		{
			if (m_TitleImageCollapseMouseOver != null)
			{
				return m_TitleImageCollapseMouseOver;
			}
			return m_DefaultCollapseImageMouseOver;
		}
		if (m_TitleImageCollapse != null)
		{
			return m_TitleImageCollapse;
		}
		return m_DefaultCollapseImage;
	}

	private Image GetExpandImage(bool mouseOverImage)
	{
		if (mouseOverImage)
		{
			if (m_TitleImageExpandMouseOver != null)
			{
				return m_TitleImageExpandMouseOver;
			}
			return m_DefaultExpandImageMouseOver;
		}
		if (m_TitleImageExpand != null)
		{
			return m_TitleImageExpand;
		}
		return m_DefaultExpandImage;
	}

	internal void Office2007ColorTableChanged()
	{
		ColorSchemeStyleChanged();
	}

	private void UpdateExpandButton()
	{
		LocalizationManager localizationManager = new LocalizationManager(navBar);
		if (m_Expanded)
		{
			if (panelTitle.ButtonAlignment == eTitleButtonAlignment.Right || m_TitleImageCollapse != null)
			{
				panelTitle.ExpandChangeButton.Image = GetCollapseImage(mouseOverImage: false);
				panelTitle.ExpandChangeButton.HoverImage = GetCollapseImage(mouseOverImage: true);
			}
			else
			{
				panelTitle.ExpandChangeButton.Image = GetExpandImage(mouseOverImage: false);
				panelTitle.ExpandChangeButton.HoverImage = GetExpandImage(mouseOverImage: true);
			}
			panelTitle.ExpandChangeButton.Tooltip = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavPaneCollapseButtonTooltip);
		}
		else
		{
			if (panelTitle.ButtonAlignment == eTitleButtonAlignment.Right || m_TitleImageExpand != null)
			{
				panelTitle.ExpandChangeButton.Image = GetExpandImage(mouseOverImage: false);
				panelTitle.ExpandChangeButton.HoverImage = GetExpandImage(mouseOverImage: true);
			}
			else
			{
				panelTitle.ExpandChangeButton.Image = GetCollapseImage(mouseOverImage: false);
				panelTitle.ExpandChangeButton.HoverImage = GetCollapseImage(mouseOverImage: true);
			}
			panelTitle.ExpandChangeButton.Tooltip = localizationManager.GetDefaultLocalizedString(LocalizationKeys.NavPaneExpandButtonTooltip);
		}
		panelTitle.UpdateButtonPosition();
	}

	private void UpdateExpandCollapseImages()
	{
		if (m_DefaultCollapseImage != null)
		{
			m_DefaultCollapseImage.Dispose();
		}
		if (m_DefaultExpandImage != null)
		{
			m_DefaultExpandImage.Dispose();
		}
		if (m_DefaultCollapseImageMouseOver != null)
		{
			m_DefaultCollapseImageMouseOver.Dispose();
		}
		if (m_DefaultExpandImageMouseOver != null)
		{
			m_DefaultExpandImageMouseOver.Dispose();
		}
		m_DefaultCollapseImage = UIGraphics.CreateExpandButtonImage(collapse: true, panelTitle.ColorScheme.PanelText, verticalCollapse: false);
		m_DefaultCollapseImageMouseOver = UIGraphics.CreateExpandButtonImage(collapse: true, panelTitle.ColorScheme.ItemHotText, verticalCollapse: false);
		m_DefaultExpandImage = UIGraphics.CreateExpandButtonImage(collapse: false, panelTitle.ColorScheme.PanelText, verticalCollapse: false);
		m_DefaultExpandImageMouseOver = UIGraphics.CreateExpandButtonImage(collapse: false, panelTitle.ColorScheme.ItemHotText, verticalCollapse: false);
	}

	public void LoadLayout(string fileName)
	{
		navBar.LoadLayout(fileName);
	}

	public void LoadLayout(XmlElement xmlParent)
	{
		navBar.LoadLayout(xmlParent);
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void StyleManagerStyleChanged(eDotNetBarStyle newStyle)
	{
		ColorSchemeStyleChanged();
		if (((Control)this).IsHandleCreated && ((Control)this).Visible)
		{
			((Control)this).Invalidate(true);
		}
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		ColorSchemeStyleChanged();
		((Control)this).OnHandleCreated(e);
	}

	public void ColorSchemeStyleChanged()
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		ColorScheme colorScheme = null;
		eDotNetBarStyle effectiveStyle = navBar.ItemsContainer.EffectiveStyle;
		if (BarFunctions.IsOffice2007Style(effectiveStyle) || (navBar.ColorScheme != null && navBar.ColorScheme.PredefinedColorScheme == ePredefinedColorScheme.SystemColors))
		{
			colorScheme = navBar.GetColorScheme();
		}
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val = item;
			if (val is PanelEx)
			{
				PanelEx component = val as PanelEx;
				TypeDescriptor.GetProperties(component)["ColorSchemeStyle"].SetValue(component, effectiveStyle);
				if (colorScheme != null)
				{
					((PanelEx)(object)val).ColorScheme = colorScheme;
				}
			}
		}
		if (effectiveStyle == eDotNetBarStyle.Office2010)
		{
			panelTitle.Style.Border = eBorderType.None;
		}
		else if (BarFunctions.IsOffice2007Style(effectiveStyle))
		{
			panelTitle.Style.Border = eBorderType.RaisedInner;
		}
		else
		{
			panelTitle.Style.Border = eBorderType.SingleLine;
		}
		UpdateExpandCollapseImages();
		UpdateExpandButton();
		if (effectiveStyle == eDotNetBarStyle.Office2010)
		{
			((ScrollableControl)this).DockPadding.All = 0;
			navBar.BackgroundStyle.BorderWidth = 0;
			navBar.BackgroundStyle.BorderColor.Color = Color.Transparent;
		}
		else
		{
			((ScrollableControl)this).DockPadding.All = 1;
			navBar.BackgroundStyle.BorderWidth = 1;
			navBar.BackgroundStyle.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		}
	}

	private void OnNavBarItemClick(object sender, EventArgs e)
	{
		if (this.ItemClick != null)
		{
			this.ItemClick(sender, e);
		}
	}

	private void OnNavBarOptionGroupChanging(object sender, OptionGroupChangingEventArgs e)
	{
		NavigationPanePanel oldpanel = null;
		NavigationPanePanel navigationPanePanel = null;
		if (e.OldChecked != null)
		{
			oldpanel = GetPanel((ButtonItem)e.OldChecked);
		}
		if (e.NewChecked != null)
		{
			navigationPanePanel = GetPanel((ButtonItem)e.NewChecked);
		}
		if (navigationPanePanel != null)
		{
			PanelChangingEventArgs panelChangingEventArgs = new PanelChangingEventArgs(oldpanel, navigationPanePanel);
			InvokePanelChanging(panelChangingEventArgs);
			e.Cancel = panelChangingEventArgs.Cancel;
		}
	}

	protected virtual void InvokePanelChanging(PanelChangingEventArgs e)
	{
		if (this.PanelChanging != null)
		{
			this.PanelChanging(this, e);
		}
	}

	private void InitializeComponent()
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		panelTitle = new PanelExTitle();
		navBar = new NavigationBar();
		((ISupportInitialize)navBar).BeginInit();
		((Control)panelTitle).Dock = (DockStyle)1;
		((Control)panelTitle).Font = new Font("Tahoma", 12f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)panelTitle).Name = "panelTitle";
		((Control)panelTitle).Size = new Size(150, 24);
		panelTitle.Style.Alignment = (StringAlignment)0;
		panelTitle.Style.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		panelTitle.Style.BackColor2.ColorSchemePart = eColorSchemePart.PanelBackground2;
		panelTitle.Style.Border = eBorderType.SingleLine;
		panelTitle.Style.BorderSide = eBorderSide.Bottom;
		panelTitle.Style.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		panelTitle.Style.ForeColor.ColorSchemePart = eColorSchemePart.PanelText;
		panelTitle.Style.GradientAngle = 90;
		panelTitle.Style.MarginLeft = 4;
		panelTitle.StyleMouseDown.Alignment = (StringAlignment)0;
		panelTitle.StyleMouseOver.Alignment = (StringAlignment)0;
		((Control)panelTitle).TabIndex = 0;
		panelTitle.ExpandButtonVisible = false;
		((Control)panelTitle).Text = "Title";
		navBar.BackgroundStyle.BackColor1.ColorSchemePart = eColorSchemePart.PanelBackground;
		navBar.BackgroundStyle.Border = eBorderType.SingleLine;
		navBar.BackgroundStyle.BorderSide = eBorderSide.Top;
		navBar.BackgroundStyle.BorderColor.ColorSchemePart = eColorSchemePart.PanelBorder;
		((Control)navBar).Dock = (DockStyle)2;
		((Control)navBar).Location = new Point(0, 128);
		((Control)navBar).Name = "navigationBar1";
		((Control)navBar).Size = new Size(150, 32);
		((Control)navBar).TabIndex = 1;
		((Control)navBar).Text = "navBar";
		((Control)navBar).Resize += OnNavBarResize;
		navBar.ItemClick += OnNavBarItemClick;
		navBar.OptionGroupChanging += OnNavBarOptionGroupChanging;
		((Control)navBar).Font = new Font("Tahoma", 8.25f, (FontStyle)1, (GraphicsUnit)3, (byte)0);
		((Control)this).Controls.AddRange((Control[])(object)new Control[2]
		{
			navBar,
			(Control)panelTitle
		});
		((Control)this).Name = "NavigationPane";
		((Control)this).Size = new Size(150, 192);
		((ScrollableControl)this).DockPadding.All = 1;
		((ISupportInitialize)navBar).EndInit();
	}
}
