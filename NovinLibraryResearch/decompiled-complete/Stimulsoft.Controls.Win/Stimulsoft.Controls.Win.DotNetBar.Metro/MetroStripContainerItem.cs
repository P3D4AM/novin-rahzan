using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Metro;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
internal class MetroStripContainerItem : ImageItem, IDesignTimeProvider
{
	private const string DefaultSettingsButtonText = "<font size=\"7\">SETTINGS</font>";

	private const string DefaultHelpButtonText = "<font size=\"7\">HELP</font>";

	private MetroTabItemContainer _ItemContainer;

	private CaptionItemContainer _CaptionContainer;

	private SystemCaptionItem _SystemCaptionItem;

	private MetroTabStrip _TabStrip;

	private SystemCaptionItem _WindowIcon;

	private Separator _IconSeparator;

	private ButtonItem _Settings;

	private ButtonItem _Help;

	private string _SettingsButtonText = "";

	private string _HelpButtonText;

	public GenericItemContainer RibbonStripContainer => _ItemContainer;

	public GenericItemContainer CaptionContainer => _CaptionContainer;

	public SystemCaptionItem SystemCaptionItem => _SystemCaptionItem;

	[Browsable(false)]
	[DefaultValue(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override bool Expanded
	{
		get
		{
			return base.Expanded;
		}
		set
		{
			base.Expanded = value;
			if (value)
			{
				return;
			}
			foreach (BaseItem subItem in SubItems)
			{
				subItem.Expanded = false;
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Size SubItemsImageSize
	{
		get
		{
			return base.SubItemsImageSize;
		}
		set
		{
		}
	}

	public bool SettingsButtonVisible
	{
		get
		{
			return _Settings.Visible;
		}
		set
		{
			if (value != _Settings.Visible)
			{
				_Settings.Visible = value;
				RecalcSize();
			}
		}
	}

	public bool HelpButtonVisible
	{
		get
		{
			return _Help.Visible;
		}
		set
		{
			if (value != _Help.Visible)
			{
				_Help.Visible = value;
				RecalcSize();
			}
		}
	}

	public string SettingsButtonText
	{
		get
		{
			return _SettingsButtonText;
		}
		set
		{
			if (value != _SettingsButtonText)
			{
				string settingsButtonText = _SettingsButtonText;
				_SettingsButtonText = value;
				OnSettingsButtonTextChanged(settingsButtonText, value);
			}
		}
	}

	public string HelpButtonText
	{
		get
		{
			return _HelpButtonText;
		}
		set
		{
			if (value != _HelpButtonText)
			{
				string helpButtonText = _HelpButtonText;
				_HelpButtonText = value;
				OnHelpButtonTextChanged(helpButtonText, value);
			}
		}
	}

	public MetroStripContainerItem(MetroTabStrip parent)
	{
		_TabStrip = parent;
		m_IsContainer = true;
		AccessibleRole = (AccessibleRole)20;
		_ItemContainer = new MetroTabItemContainer();
		_ItemContainer.ContainerControl = parent;
		_ItemContainer.GlobalItem = false;
		_ItemContainer.WrapItems = false;
		_ItemContainer.EventHeight = false;
		_ItemContainer.UseMoreItemsButton = false;
		_ItemContainer.Stretch = true;
		_ItemContainer.Displayed = true;
		_ItemContainer.SystemContainer = true;
		_ItemContainer.PaddingTop = 0;
		_ItemContainer.PaddingBottom = 0;
		_ItemContainer.PaddingLeft = 0;
		_ItemContainer.ItemSpacing = 1;
		_CaptionContainer = new CaptionItemContainer();
		_CaptionContainer.ContainerControl = parent;
		_CaptionContainer.GlobalItem = false;
		_CaptionContainer.WrapItems = false;
		_CaptionContainer.EventHeight = false;
		_CaptionContainer.EqualButtonSize = false;
		_CaptionContainer.ToolbarItemsAlign = eContainerVerticalAlignment.Top;
		_CaptionContainer.UseMoreItemsButton = false;
		_CaptionContainer.Stretch = true;
		_CaptionContainer.Displayed = true;
		_CaptionContainer.SystemContainer = true;
		_CaptionContainer.PaddingBottom = 0;
		_CaptionContainer.PaddingTop = 0;
		_CaptionContainer.PaddingLeft = 6;
		_CaptionContainer.ItemSpacing = 1;
		_CaptionContainer.TrackSubItemsImageSize = false;
		CaptionItemContainer captionContainer = _CaptionContainer;
		captionContainer.ItemAdded = (EventHandler)Delegate.Combine(captionContainer.ItemAdded, new EventHandler(CaptionContainerNewItemAdded));
		SubItems.Add(_CaptionContainer);
		SubItems.Add(_ItemContainer);
		_Settings = new ButtonItem("sysSettingsButton");
		_Settings.Text = "<font size=\"7\">SETTINGS</font>";
		_Settings.ItemAlignment = eItemAlignment.Far;
		_Settings.Click += SettingsButtonClick;
		_Settings.SetSystemItem(b: true);
		_Settings.CanCustomize = false;
		_CaptionContainer.SubItems.Add(_Settings);
		_Help = new ButtonItem("sysHelpButton");
		_Help.Text = "<font size=\"7\">HELP</font>";
		_Help.SetSystemItem(b: true);
		_Help.CanCustomize = false;
		_Help.ItemAlignment = eItemAlignment.Far;
		_Help.Click += HelpButtonClick;
		_CaptionContainer.SubItems.Add(_Help);
		SystemCaptionItem systemCaptionItem = new SystemCaptionItem
		{
			RestoreEnabled = false,
			IsSystemIcon = false,
			ItemAlignment = eItemAlignment.Far
		};
		_CaptionContainer.SubItems.Add(systemCaptionItem);
		_SystemCaptionItem = systemCaptionItem;
	}

	private void CaptionContainerNewItemAdded(object sender, EventArgs e)
	{
		if (!(sender is BaseItem))
		{
			return;
		}
		BaseItem baseItem = sender as BaseItem;
		if (!(baseItem is SystemCaptionItem))
		{
			if (_CaptionContainer.SubItems.Contains(_Settings))
			{
				_CaptionContainer.SubItems.RemoveItem(_Settings);
				_CaptionContainer.SubItems._Add(_Settings);
			}
			if (_CaptionContainer.SubItems.Contains(_Help))
			{
				_CaptionContainer.SubItems.RemoveItem(_Help);
				_CaptionContainer.SubItems._Add(_Help);
			}
			if (_CaptionContainer.SubItems.Contains(_SystemCaptionItem))
			{
				_CaptionContainer.SubItems.RemoveItem(_SystemCaptionItem);
				_CaptionContainer.SubItems._Add(_SystemCaptionItem);
			}
		}
	}

	private void SettingsButtonClick(object sender, EventArgs e)
	{
		if (ContainerControl is MetroTabStrip metroTabStrip && ((Control)metroTabStrip).Parent is MetroShell metroShell)
		{
			metroShell.InvokeSettingsButtonClick(e);
		}
	}

	private void HelpButtonClick(object sender, EventArgs e)
	{
		if (ContainerControl is MetroTabStrip metroTabStrip && ((Control)metroTabStrip).Parent is MetroShell metroShell)
		{
			metroShell.InvokeHelpButtonClick(e);
		}
	}

	internal void ReleaseSystemFocus()
	{
		_ItemContainer.ReleaseSystemFocus();
		if (_TabStrip.CaptionVisible)
		{
			_CaptionContainer.ReleaseSystemFocus();
		}
	}

	public override void ContainerLostFocus(bool appLostFocus)
	{
		base.ContainerLostFocus(appLostFocus);
		_ItemContainer.ContainerLostFocus(appLostFocus);
		if (_TabStrip.CaptionVisible)
		{
			_CaptionContainer.ContainerLostFocus(appLostFocus);
		}
	}

	internal void SetSystemFocus()
	{
		if (_TabStrip.CaptionVisible && _ItemContainer.ExpandedItem() == null)
		{
			_CaptionContainer.SetSystemFocus();
		}
		else
		{
			_ItemContainer.SetSystemFocus();
		}
	}

	public override void Paint(ItemPaintArgs pa)
	{
		if (!SuspendLayout)
		{
			_ItemContainer.Paint(pa);
			if (_TabStrip.CaptionVisible)
			{
				_CaptionContainer.Paint(pa);
			}
		}
	}

	public override void RecalcSize()
	{
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Invalid comparison between Unknown and I4
		if (SuspendLayout)
		{
			return;
		}
		_ItemContainer.Bounds = GetItemContainerBounds();
		_ItemContainer.RecalcSize();
		if (_ItemContainer.HeightInternal < 0)
		{
			_ItemContainer.HeightInternal = 0;
		}
		bool flag = false;
		if (_TabStrip.CaptionVisible)
		{
			_CaptionContainer.Bounds = GetCaptionContainerBounds();
			_CaptionContainer.RecalcSize();
			Size frameBorderSize = SystemInformation.FrameBorderSize;
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null && val.FindForm() is MetroAppForm metroAppForm)
			{
				NonClientInfo nonClientInfo = metroAppForm.GetNonClientInfo();
				frameBorderSize.Width = nonClientInfo.LeftBorder;
				frameBorderSize.Height = nonClientInfo.BottomBorder;
			}
			if (_TabStrip.CaptionHeight == 0 && _SystemCaptionItem.TopInternal < frameBorderSize.Height - 1)
			{
				object containerControl2 = ContainerControl;
				Control val2 = (Control)((containerControl2 is Control) ? containerControl2 : null);
				Form val3 = null;
				if (val2 != null)
				{
					val3 = val2.FindForm();
				}
				if (val3 != null && (int)val3.WindowState == 2)
				{
					flag = true;
				}
				if (flag)
				{
					_SystemCaptionItem.TopInternal = 1;
					if (_WindowIcon != null)
					{
						_WindowIcon.TopInternal = 1;
					}
				}
				else
				{
					_SystemCaptionItem.TopInternal = frameBorderSize.Height - 4;
					if (_WindowIcon != null)
					{
						_WindowIcon.TopInternal = frameBorderSize.Height - 5;
					}
				}
			}
			if (Environment.OSVersion.Version.Major >= 6)
			{
				int num = 3;
				if (flag)
				{
					num++;
				}
				int num2 = 0;
				foreach (BaseItem subItem in _CaptionContainer.SubItems)
				{
					if (!(subItem is ApplicationButton) && !(subItem is MetroAppButton) && subItem != _SystemCaptionItem && subItem != _IconSeparator)
					{
						subItem.TopInternal += num;
					}
					else if (subItem == _IconSeparator)
					{
						subItem.TopInternal += ((!flag) ? 1 : 2);
					}
					num2 = Math.Max(subItem.Bounds.Bottom, num2);
				}
				if (_CaptionContainer.MoreItems != null)
				{
					_CaptionContainer.MoreItems.TopInternal += num;
				}
				if (num2 > _CaptionContainer.HeightInternal)
				{
					_CaptionContainer.SetDisplayRectangle(new Rectangle(_CaptionContainer.Bounds.X, _CaptionContainer.Bounds.Y, _CaptionContainer.Bounds.Width, num2));
				}
			}
			else
			{
				int num3 = 0;
				foreach (BaseItem subItem2 in _CaptionContainer.SubItems)
				{
					if (subItem2.HeightInternal < _SystemCaptionItem.HeightInternal && subItem2 != _IconSeparator && subItem2 != _WindowIcon)
					{
						subItem2.TopInternal = _SystemCaptionItem.Bounds.Bottom - (subItem2.HeightInternal + ((subItem2 is LabelItem) ? 2 : 0));
						num3 = Math.Max(subItem2.Bounds.Bottom, num3);
					}
				}
				if (_CaptionContainer.MoreItems != null)
				{
					_CaptionContainer.MoreItems.TopInternal += _SystemCaptionItem.HeightInternal - _CaptionContainer.MoreItems.HeightInternal;
				}
				if (num3 > _CaptionContainer.HeightInternal)
				{
					_CaptionContainer.SetDisplayRectangle(new Rectangle(_CaptionContainer.Bounds.X, _CaptionContainer.Bounds.Y, _CaptionContainer.Bounds.Width, num3));
				}
			}
			if (_ItemContainer.HeightInternal == 0)
			{
				HeightInternal = _CaptionContainer.HeightInternal;
			}
			else
			{
				HeightInternal = _ItemContainer.Bounds.Bottom;
			}
		}
		else
		{
			int heightInternal = _ItemContainer.HeightInternal;
			HeightInternal = heightInternal;
		}
		base.RecalcSize();
	}

	private Rectangle GetItemContainerBounds()
	{
		return _TabStrip.GetItemContainerBounds();
	}

	private Rectangle GetCaptionContainerBounds()
	{
		return _TabStrip.GetCaptionContainerBounds();
	}

	public override BaseItem Copy()
	{
		MetroStripContainerItem metroStripContainerItem = new MetroStripContainerItem(_TabStrip);
		CopyToItem(metroStripContainerItem);
		return metroStripContainerItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		MetroStripContainerItem objCopy = copy as MetroStripContainerItem;
		base.CopyToItem(objCopy);
	}

	public override void InternalClick(MouseButtons mb, Point mpos)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		_ItemContainer.InternalClick(mb, mpos);
		if (_TabStrip.CaptionVisible)
		{
			_CaptionContainer.InternalClick(mb, mpos);
		}
	}

	public override void InternalDoubleClick(MouseButtons mb, Point mpos)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		_ItemContainer.InternalDoubleClick(mb, mpos);
		if (_TabStrip.CaptionVisible)
		{
			_CaptionContainer.InternalDoubleClick(mb, mpos);
		}
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		_ItemContainer.InternalMouseDown(objArg);
		if (_TabStrip.CaptionVisible && ((DesignMode && _CaptionContainer.ItemAtLocation(objArg.X, objArg.Y) != null) || !DesignMode))
		{
			_CaptionContainer.InternalMouseDown(objArg);
		}
	}

	public override void InternalMouseHover()
	{
		_ItemContainer.InternalMouseHover();
		if (_TabStrip.CaptionVisible)
		{
			_CaptionContainer.InternalMouseHover();
		}
	}

	public override void InternalMouseLeave()
	{
		_ItemContainer.InternalMouseLeave();
		if (_TabStrip.CaptionVisible)
		{
			_CaptionContainer.InternalMouseLeave();
		}
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		_ItemContainer.InternalMouseMove(objArg);
		if (_TabStrip.CaptionVisible)
		{
			_CaptionContainer.InternalMouseMove(objArg);
		}
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		_ItemContainer.InternalMouseUp(objArg);
		if (_TabStrip.CaptionVisible)
		{
			_CaptionContainer.InternalMouseUp(objArg);
		}
	}

	public override void InternalKeyDown(KeyEventArgs objArg)
	{
		BaseItem baseItem = ExpandedItem();
		if (baseItem == null)
		{
			baseItem = _CaptionContainer.ExpandedItem();
		}
		if (baseItem == null)
		{
			baseItem = _ItemContainer.ExpandedItem();
		}
		if (baseItem == null || !_TabStrip.CaptionVisible)
		{
			_ItemContainer.InternalKeyDown(objArg);
			if (!objArg.Handled && _TabStrip.CaptionVisible)
			{
				_CaptionContainer.InternalKeyDown(objArg);
			}
		}
		else if (baseItem.Parent == _ItemContainer)
		{
			_ItemContainer.InternalKeyDown(objArg);
		}
		else
		{
			_CaptionContainer.InternalKeyDown(objArg);
		}
	}

	public override BaseItem ItemAtLocation(int x, int y)
	{
		if (_ItemContainer.DisplayRectangle.Contains(x, y))
		{
			return _ItemContainer.ItemAtLocation(x, y);
		}
		if (_CaptionContainer.DisplayRectangle.Contains(x, y))
		{
			return _CaptionContainer.ItemAtLocation(x, y);
		}
		return null;
	}

	protected override void OnStyleChanged()
	{
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Expected O, but got Unknown
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Expected O, but got Unknown
		switch (EffectiveStyle)
		{
		case eDotNetBarStyle.Office2010:
			if (_WindowIcon == null)
			{
				_IconSeparator = new Separator("sys_caption_separator");
				_IconSeparator.SetSystemItem(b: true);
				_IconSeparator.DesignTimeVisible = false;
				_IconSeparator.CanCustomize = false;
				_CaptionContainer.SubItems._Add(_IconSeparator, 0);
				_WindowIcon = new SystemCaptionItem();
				_WindowIcon.Name = "sys_caption_icon";
				_WindowIcon.Enabled = false;
				_WindowIcon.Style = Style;
				_WindowIcon.IsSystemIcon = true;
				_WindowIcon.DesignTimeVisible = false;
				_WindowIcon.CanCustomize = false;
				_WindowIcon.QueryIconOnPaint = true;
				_WindowIcon.MouseDown += new MouseEventHandler(WindowIconMouseDown);
				_WindowIcon.DoubleClick += WindowIconDoubleClick;
				_CaptionContainer.SubItems._Add(_WindowIcon, 0);
			}
			break;
		case eDotNetBarStyle.Windows7:
			if (_WindowIcon == null)
			{
				_IconSeparator = new Separator("sys_caption_separator");
				_IconSeparator.FixedSize = new Size(3, 12);
				_IconSeparator.SetSystemItem(b: true);
				_IconSeparator.DesignTimeVisible = false;
				_IconSeparator.CanCustomize = false;
				_CaptionContainer.SubItems._Add(_IconSeparator, 0);
				_WindowIcon = new SystemCaptionItem();
				_WindowIcon.Name = "sys_caption_icon";
				_WindowIcon.Enabled = false;
				_WindowIcon.Style = Style;
				_WindowIcon.IsSystemIcon = true;
				_WindowIcon.QueryIconOnPaint = true;
				_WindowIcon.DesignTimeVisible = false;
				_WindowIcon.CanCustomize = false;
				_WindowIcon.MouseDown += new MouseEventHandler(WindowIconMouseDown);
				_CaptionContainer.SubItems._Add(_WindowIcon, 0);
			}
			break;
		case eDotNetBarStyle.Metro:
			if (_WindowIcon == null)
			{
				_IconSeparator = new Separator("sys_caption_separator");
				_IconSeparator.FixedSize = new Size(3, 14);
				_IconSeparator.SetSystemItem(b: true);
				_IconSeparator.DesignTimeVisible = false;
				_IconSeparator.CanCustomize = false;
				_CaptionContainer.SubItems._Add(_IconSeparator, 0);
				_WindowIcon = new SystemCaptionItem();
				_WindowIcon.Name = "sys_caption_icon";
				_WindowIcon.Enabled = false;
				_WindowIcon.Style = Style;
				_WindowIcon.IsSystemIcon = true;
				_WindowIcon.DesignTimeVisible = false;
				_WindowIcon.CanCustomize = false;
				_WindowIcon.QueryIconOnPaint = true;
				_WindowIcon.MouseDown += new MouseEventHandler(WindowIconMouseDown);
				_WindowIcon.DoubleClick += WindowIconDoubleClick;
				_CaptionContainer.SubItems._Add(_WindowIcon, 0);
			}
			break;
		default:
			if (_WindowIcon != null)
			{
				if (_CaptionContainer.SubItems.Contains(_WindowIcon))
				{
					_CaptionContainer.SubItems.RemoveItem(_WindowIcon);
				}
				_WindowIcon.MouseDown -= new MouseEventHandler(WindowIconMouseDown);
				_WindowIcon.DoubleClick -= WindowIconDoubleClick;
				_WindowIcon.Dispose();
				_WindowIcon = null;
				if (_CaptionContainer.SubItems.Contains(_IconSeparator))
				{
					_CaptionContainer.SubItems.RemoveItem(_IconSeparator);
				}
				_IconSeparator.Dispose();
				_IconSeparator = null;
			}
			break;
		}
		base.OnStyleChanged();
	}

	private void WindowIconDoubleClick(object sender, EventArgs e)
	{
		if (_TabStrip != null)
		{
			_TabStrip.CloseParentForm();
		}
	}

	private void WindowIconMouseDown(object sender, MouseEventArgs e)
	{
		if (ContainerControl is MetroTabStrip metroTabStrip)
		{
			Point point = new Point(_WindowIcon.LeftInternal, _WindowIcon.Bounds.Bottom + 1);
			point = ((Control)metroTabStrip).PointToScreen(point);
			metroTabStrip.ShowSystemMenu(point);
		}
	}

	public InsertPosition GetInsertPosition(Point pScreen, BaseItem DragItem)
	{
		InsertPosition insertPosition = _ItemContainer.GetInsertPosition(pScreen, DragItem);
		if (insertPosition == null && _TabStrip.CaptionVisible)
		{
			insertPosition = _CaptionContainer.GetInsertPosition(pScreen, DragItem);
		}
		return insertPosition;
	}

	public void DrawReversibleMarker(int iPos, bool Before)
	{
	}

	public void InsertItemAt(BaseItem objItem, int iPos, bool Before)
	{
	}

	protected virtual void OnSettingsButtonTextChanged(string oldValue, string newValue)
	{
		if (string.IsNullOrEmpty(newValue))
		{
			_Settings.Text = "<font size=\"7\">SETTINGS</font>";
		}
		else
		{
			_Settings.Text = "<font size=\"7\">" + newValue + "</font>";
		}
	}

	protected virtual void OnHelpButtonTextChanged(string oldValue, string newValue)
	{
		if (string.IsNullOrEmpty(newValue))
		{
			_Help.Text = "<font size=\"7\">HELP</font>";
		}
		else
		{
			_Help.Text = "<font size=\"7\">" + newValue + "</font>";
		}
	}
}
