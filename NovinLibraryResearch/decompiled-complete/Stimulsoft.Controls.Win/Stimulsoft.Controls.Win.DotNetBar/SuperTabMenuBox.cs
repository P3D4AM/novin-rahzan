using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SuperTabMenuBox : PopupItem
{
	private SuperTabControlBox _ControlBox;

	private Size _ItemSize = new Size(16, 16);

	private bool _IsMouseOver;

	private bool _IsMouseDown;

	private bool _AutoHide;

	private bool _ShowTabsOnly = true;

	private bool _ShowImages = true;

	private bool _RaiseClickOnSelection;

	[Browsable(false)]
	public override bool BeginGroup
	{
		get
		{
			return base.BeginGroup;
		}
		set
		{
			base.BeginGroup = value;
		}
	}

	[Browsable(false)]
	public override bool AutoCollapseOnClick
	{
		get
		{
			return base.AutoCollapseOnClick;
		}
		set
		{
			base.AutoCollapseOnClick = value;
		}
	}

	[Browsable(false)]
	public override bool CanCustomize
	{
		get
		{
			return base.CanCustomize;
		}
		set
		{
			base.CanCustomize = value;
		}
	}

	[Browsable(false)]
	public override string Category
	{
		get
		{
			return base.Category;
		}
		set
		{
			base.Category = value;
		}
	}

	[Browsable(false)]
	public override bool ClickAutoRepeat
	{
		get
		{
			return base.ClickAutoRepeat;
		}
		set
		{
			base.ClickAutoRepeat = value;
		}
	}

	[Browsable(false)]
	public override int ClickRepeatInterval
	{
		get
		{
			return base.ClickRepeatInterval;
		}
		set
		{
			base.ClickRepeatInterval = value;
		}
	}

	[Browsable(false)]
	public override Cursor Cursor
	{
		get
		{
			return base.Cursor;
		}
		set
		{
			base.Cursor = value;
		}
	}

	[Browsable(false)]
	public override bool Enabled
	{
		get
		{
			return base.Enabled;
		}
		set
		{
			base.Enabled = value;
		}
	}

	[Browsable(false)]
	public override bool Stretch
	{
		get
		{
			return base.Stretch;
		}
		set
		{
			base.Stretch = value;
		}
	}

	[Browsable(false)]
	public override string Text
	{
		get
		{
			return base.Text;
		}
		set
		{
			base.Text = value;
		}
	}

	[Browsable(false)]
	public override bool ThemeAware
	{
		get
		{
			return base.ThemeAware;
		}
		set
		{
			base.ThemeAware = value;
		}
	}

	[Browsable(false)]
	public override string Tooltip
	{
		get
		{
			return base.Tooltip;
		}
		set
		{
			base.Tooltip = value;
		}
	}

	[Browsable(false)]
	public override eItemAlignment ItemAlignment
	{
		get
		{
			return base.ItemAlignment;
		}
		set
		{
			base.ItemAlignment = value;
		}
	}

	[Browsable(false)]
	public override string KeyTips
	{
		get
		{
			return base.KeyTips;
		}
		set
		{
			base.KeyTips = value;
		}
	}

	[Browsable(false)]
	public override ShortcutsCollection Shortcuts
	{
		get
		{
			return base.Shortcuts;
		}
		set
		{
			base.Shortcuts = value;
		}
	}

	[Browsable(false)]
	public override string Description
	{
		get
		{
			return base.Description;
		}
		set
		{
			base.Description = value;
		}
	}

	[Browsable(false)]
	public override bool GlobalItem
	{
		get
		{
			return base.GlobalItem;
		}
		set
		{
			base.GlobalItem = value;
		}
	}

	[Browsable(false)]
	public override bool ShowSubItems
	{
		get
		{
			return base.ShowSubItems;
		}
		set
		{
			base.ShowSubItems = value;
		}
	}

	[Browsable(false)]
	public override string GlobalName
	{
		get
		{
			return base.GlobalName;
		}
		set
		{
			base.GlobalName = value;
		}
	}

	[Browsable(false)]
	public override Command Command
	{
		get
		{
			return base.Command;
		}
		set
		{
			base.Command = value;
		}
	}

	[Browsable(false)]
	public override object CommandParameter
	{
		get
		{
			return base.CommandParameter;
		}
		set
		{
			base.CommandParameter = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether the MenuBox is automatically hidden when the tab items size does not exceed the size of the control.")]
	public bool AutoHide
	{
		get
		{
			return _AutoHide;
		}
		set
		{
			if (_AutoHide != value)
			{
				_AutoHide = value;
				Visible = !value || !AllItemsVisible();
				MyRefresh();
			}
		}
	}

	[Browsable(true)]
	[DefaultValue(false)]
	[Category("Behavior")]
	[Description("Indicates whether the MenuBox raises a ClickEvent when selected.")]
	public bool RaiseClickOnSelection
	{
		get
		{
			return _RaiseClickOnSelection;
		}
		set
		{
			_RaiseClickOnSelection = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether MenuBox shows only Tabs entries.")]
	public bool ShowTabsOnly
	{
		get
		{
			return _ShowTabsOnly;
		}
		set
		{
			_ShowTabsOnly = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates whether the MenuBox displays each menu entry with its associated Image/Icon.")]
	public bool ShowImages
	{
		get
		{
			return _ShowImages;
		}
		set
		{
			_ShowImages = value;
		}
	}

	[Browsable(true)]
	[DefaultValue(true)]
	[Category("Behavior")]
	[Description("Indicates MenuBox Visible state.")]
	public override bool Visible
	{
		get
		{
			return base.Visible;
		}
		set
		{
			if (base.Visible != value)
			{
				base.Visible = value;
				MyRefresh();
			}
		}
	}

	internal bool IsMouseDown => _IsMouseDown;

	internal bool IsMouseOver => _IsMouseOver;

	public SuperTabMenuBox(SuperTabControlBox controlBox)
	{
		_ControlBox = controlBox;
		Style = eDotNetBarStyle.Office2007;
	}

	public override void RecalcSize()
	{
		base.RecalcSize();
		WidthInternal = _ItemSize.Width;
		HeightInternal = _ItemSize.Height;
	}

	private void MyRefresh()
	{
		if (_ControlBox.TabDisplay != null)
		{
			SuperTabStripItem tabStripItem = _ControlBox.TabDisplay.TabStripItem;
			if (tabStripItem != null)
			{
				_ControlBox.NeedRecalcSize = true;
				_ControlBox.RecalcSize();
				tabStripItem.NeedRecalcSize = true;
				tabStripItem.Refresh();
			}
		}
	}

	public override void Paint(ItemPaintArgs p)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		Graphics graphics = p.Graphics;
		using SuperTabColorTable superTabColorTable = _ControlBox.TabDisplay.GetColorTable();
		Color image = superTabColorTable.ControlBoxDefault.Image;
		if (IsMouseOver)
		{
			Rectangle bounds = Bounds;
			bounds.Width--;
			bounds.Height--;
			image = superTabColorTable.ControlBoxMouseOver.Image;
			Brush val = (Brush)new SolidBrush(superTabColorTable.ControlBoxMouseOver.Background);
			try
			{
				graphics.FillRectangle(val, bounds);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			Pen val2 = new Pen(superTabColorTable.ControlBoxMouseOver.Border);
			try
			{
				graphics.DrawRectangle(val2, bounds);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		if (AllItemsVisible())
		{
			graphics.DrawImageUnscaled((Image)(object)GetMenuButton1(graphics, image), Bounds);
		}
		else
		{
			graphics.DrawImageUnscaled((Image)(object)GetMenuButton2(graphics, image), Bounds);
		}
	}

	private bool AllItemsVisible()
	{
		SubItemsCollection tabs = _ControlBox.TabDisplay.Tabs;
		for (int i = 0; i < tabs.Count; i++)
		{
			if (tabs[i].Visible && !tabs[i].Displayed)
			{
				return false;
			}
		}
		return true;
	}

	private Bitmap GetMenuButton1(Graphics g, Color color)
	{
		return _ControlBox.TabDisplay.GetMenuButton1(g, color);
	}

	private Bitmap GetMenuButton2(Graphics g, Color color)
	{
		return _ControlBox.TabDisplay.GetMenuButton2(g, color);
	}

	public override void InternalMouseEnter()
	{
		base.InternalMouseEnter();
		_IsMouseOver = true;
		Refresh();
	}

	public override void InternalMouseLeave()
	{
		base.InternalMouseLeave();
		_IsMouseOver = false;
		Refresh();
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Invalid comparison between Unknown and I4
		_IsMouseDown = true;
		if (!DesignMode && (int)objArg.Button == 1048576)
		{
			LoadTabMenu();
			Point popupPosition = GetPopupPosition();
			Popup(popupPosition);
		}
	}

	private Point GetPopupPosition()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		Point point = new Point(Bounds.X, Bounds.Bottom);
		Control val = (Control)_ControlBox.GetContainerControl(checkIsContainer: true);
		if (val != null)
		{
			return val.PointToScreen(point);
		}
		return point;
	}

	private void LoadTabMenu()
	{
		SubItems.Clear();
		SubItemsCollection tabs = _ControlBox.TabDisplay.Tabs;
		if (_ShowTabsOnly)
		{
			LoadTabsMenu(tabs);
		}
		else
		{
			LoadItemsMenu(tabs);
		}
	}

	private void LoadTabsMenu(SubItemsCollection tabs)
	{
		for (int i = 0; i < tabs.Count; i++)
		{
			if (tabs[i] is SuperTabItem { Visible: not false } superTabItem)
			{
				ButtonItem buttonItem = new ButtonItem();
				buttonItem.Tag = superTabItem;
				buttonItem.Text = superTabItem.Text;
				if (superTabItem.Enabled)
				{
					buttonItem.Click += SuperTabMenuBoxClick;
				}
				else
				{
					buttonItem.Enabled = false;
				}
				SubItems.Add(buttonItem);
				if (_ShowImages)
				{
					SetMenuImage(buttonItem, superTabItem.GetTabImage());
				}
				if (superTabItem.IsSelected)
				{
					buttonItem.Checked = true;
				}
			}
		}
	}

	private void LoadItemsMenu(SubItemsCollection tabs)
	{
		ButtonItem[] array = new ButtonItem[tabs.Count];
		for (int i = 0; i < tabs.Count; i++)
		{
			if (!tabs[i].Visible)
			{
				continue;
			}
			array[i] = new ButtonItem();
			array[i].Tag = tabs[i];
			array[i].Text = tabs[i].Text;
			if (tabs[i].Enabled)
			{
				array[i].Click += SuperTabMenuBoxClick;
			}
			else
			{
				array[i].Enabled = false;
			}
			if (_ShowImages)
			{
				if (tabs[i] is SuperTabItem)
				{
					SetMenuImage(array[i], ((SuperTabItem)tabs[i]).GetTabImage());
				}
				else if (tabs[i] is ButtonItem)
				{
					SetMenuImage(array[i], ((ButtonItem)tabs[i]).GetImage());
				}
			}
		}
		int selectedTabIndex = _ControlBox.TabDisplay.TabStripItem.SelectedTabIndex;
		if (selectedTabIndex >= 0 && selectedTabIndex < tabs.Count)
		{
			array[selectedTabIndex].Checked = true;
		}
		SubItemsCollection subItems = SubItems;
		BaseItem[] items = array;
		subItems.AddRange(items);
	}

	private void SetMenuImage(ButtonItem bi, CompositeImage image)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		if (image == null)
		{
			return;
		}
		if (image.Icon != null)
		{
			bi.Icon = image.Icon;
		}
		else if (image.Image != null)
		{
			if (ImageAnimator.CanAnimate(image.Image))
			{
				FrameDimension val = new FrameDimension(image.Image.FrameDimensionsList[0]);
				image.Image.SelectActiveFrame(val, 0);
				bi.Image = (Image)new Bitmap(image.Image);
				bi.NeedRecalcSize = true;
			}
			else
			{
				bi.Image = image.Image;
			}
		}
	}

	private void SuperTabMenuBoxClick(object sender, EventArgs e)
	{
		if (sender is ButtonItem { Tag: BaseItem tag } buttonItem)
		{
			if (buttonItem.Tag is SuperTabItem selectedTab)
			{
				_ControlBox.TabDisplay.TabStripItem.SelectedTab = selectedTab;
			}
			else
			{
				_ControlBox.TabDisplay.TabStripItem.VisibleTab = tag;
			}
			if (_RaiseClickOnSelection)
			{
				tag.RaiseClick(eEventSource.Code);
			}
		}
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		base.InternalMouseUp(objArg);
		_IsMouseDown = false;
		Refresh();
	}

	public override BaseItem Copy()
	{
		SuperTabMenuBox superTabMenuBox = new SuperTabMenuBox(_ControlBox);
		CopyToItem(superTabMenuBox);
		return superTabMenuBox;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		SuperTabMenuBox copy2 = copy as SuperTabMenuBox;
		base.CopyToItem((BaseItem)copy2);
	}
}
