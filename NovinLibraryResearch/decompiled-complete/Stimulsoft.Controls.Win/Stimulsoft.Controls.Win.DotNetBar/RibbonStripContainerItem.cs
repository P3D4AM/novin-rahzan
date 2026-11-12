using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class RibbonStripContainerItem : ImageItem, IDesignTimeProvider
{
	private RibbonTabItemContainer m_ItemContainer;

	private CaptionItemContainer m_CaptionContainer;

	private SystemCaptionItem m_SystemCaptionItem;

	private RibbonStrip m_RibbonStrip;

	private SystemCaptionItem _WindowIcon;

	private Separator _IconSeparator;

	private List<BaseItem> _HiddenCaptionItems = new List<BaseItem>();

	private bool _MetroBackstageOpen;

	public bool MetroBackstageOpen
	{
		get
		{
			return _MetroBackstageOpen;
		}
		internal set
		{
			if (_MetroBackstageOpen == value)
			{
				return;
			}
			_MetroBackstageOpen = value;
			if (value)
			{
				foreach (BaseItem subItem in m_CaptionContainer.SubItems)
				{
					if (subItem.Visible && subItem != m_SystemCaptionItem)
					{
						subItem.Displayed = false;
						subItem.LockDisplayedChange = true;
						_HiddenCaptionItems.Add(subItem);
					}
				}
				return;
			}
			foreach (BaseItem hiddenCaptionItem in _HiddenCaptionItems)
			{
				hiddenCaptionItem.Displayed = true;
				hiddenCaptionItem.LockDisplayedChange = false;
			}
			_HiddenCaptionItems.Clear();
			NeedRecalcSize = true;
		}
	}

	private bool IsGlassEnabled
	{
		get
		{
			if (DesignMode || !m_RibbonStrip.CanSupportGlass)
			{
				return false;
			}
			if (((Control)m_RibbonStrip).FindForm() is RibbonForm ribbonForm)
			{
				return ribbonForm.IsGlassEnabled;
			}
			return WinApi.IsGlassEnabled;
		}
	}

	public GenericItemContainer RibbonStripContainer => m_ItemContainer;

	public GenericItemContainer CaptionContainer => m_CaptionContainer;

	public SystemCaptionItem SystemCaptionItem => m_SystemCaptionItem;

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

	public RibbonStripContainerItem(RibbonStrip parent)
	{
		m_RibbonStrip = parent;
		m_IsContainer = true;
		AccessibleRole = (AccessibleRole)20;
		m_ItemContainer = new RibbonTabItemContainer();
		m_ItemContainer.ContainerControl = parent;
		m_ItemContainer.GlobalItem = false;
		m_ItemContainer.WrapItems = false;
		m_ItemContainer.EventHeight = false;
		m_ItemContainer.UseMoreItemsButton = false;
		m_ItemContainer.Stretch = true;
		m_ItemContainer.Displayed = true;
		m_ItemContainer.SystemContainer = true;
		m_ItemContainer.PaddingTop = 0;
		m_ItemContainer.PaddingBottom = 0;
		m_ItemContainer.ItemSpacing = 1;
		m_CaptionContainer = new CaptionItemContainer();
		m_CaptionContainer.ContainerControl = parent;
		m_CaptionContainer.GlobalItem = false;
		m_CaptionContainer.WrapItems = false;
		m_CaptionContainer.EventHeight = false;
		m_CaptionContainer.EqualButtonSize = false;
		m_CaptionContainer.ToolbarItemsAlign = eContainerVerticalAlignment.Top;
		m_CaptionContainer.UseMoreItemsButton = false;
		m_CaptionContainer.Stretch = true;
		m_CaptionContainer.Displayed = true;
		m_CaptionContainer.SystemContainer = true;
		m_CaptionContainer.PaddingBottom = 0;
		m_CaptionContainer.PaddingTop = 0;
		m_CaptionContainer.ItemSpacing = 1;
		m_CaptionContainer.TrackSubItemsImageSize = false;
		CaptionItemContainer captionContainer = m_CaptionContainer;
		captionContainer.ItemAdded = (EventHandler)Delegate.Combine(captionContainer.ItemAdded, new EventHandler(CaptionContainerNewItemAdded));
		SubItems.Add(m_CaptionContainer);
		SubItems.Add(m_ItemContainer);
		SystemCaptionItem systemCaptionItem = new SystemCaptionItem
		{
			RestoreEnabled = false,
			IsSystemIcon = false,
			ItemAlignment = eItemAlignment.Far
		};
		m_CaptionContainer.SubItems.Add(systemCaptionItem);
		m_SystemCaptionItem = systemCaptionItem;
	}

	private void CaptionContainerNewItemAdded(object sender, EventArgs e)
	{
		if (sender is BaseItem)
		{
			BaseItem baseItem = sender as BaseItem;
			if (!(baseItem is SystemCaptionItem) && m_CaptionContainer.SubItems.Contains(m_SystemCaptionItem))
			{
				m_CaptionContainer.SubItems.RemoveItem(m_SystemCaptionItem);
				m_CaptionContainer.SubItems._Add(m_SystemCaptionItem);
			}
		}
	}

	internal void ReleaseSystemFocus()
	{
		m_ItemContainer.ReleaseSystemFocus();
		if (m_RibbonStrip.CaptionVisible)
		{
			m_CaptionContainer.ReleaseSystemFocus();
		}
	}

	public override void ContainerLostFocus(bool appLostFocus)
	{
		base.ContainerLostFocus(appLostFocus);
		m_ItemContainer.ContainerLostFocus(appLostFocus);
		if (m_RibbonStrip.CaptionVisible)
		{
			m_CaptionContainer.ContainerLostFocus(appLostFocus);
		}
	}

	internal void SetSystemFocus()
	{
		if (m_RibbonStrip.CaptionVisible && m_ItemContainer.ExpandedItem() == null)
		{
			m_CaptionContainer.SetSystemFocus();
		}
		else
		{
			m_ItemContainer.SetSystemFocus();
		}
	}

	public override void Paint(ItemPaintArgs pa)
	{
		if (!SuspendLayout)
		{
			m_ItemContainer.Paint(pa);
			if (m_RibbonStrip.CaptionVisible)
			{
				m_CaptionContainer.Paint(pa);
			}
		}
	}

	public override void RecalcSize()
	{
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Invalid comparison between Unknown and I4
		if (SuspendLayout)
		{
			return;
		}
		m_ItemContainer.Bounds = GetItemContainerBounds();
		m_ItemContainer.RecalcSize();
		if (m_ItemContainer.HeightInternal < 0)
		{
			m_ItemContainer.HeightInternal = 0;
		}
		bool flag = false;
		bool isGlassEnabled = IsGlassEnabled;
		if (m_RibbonStrip.CaptionVisible)
		{
			Size frameBorderSize = SystemInformation.FrameBorderSize;
			m_CaptionContainer.Bounds = GetCaptionContainerBounds();
			m_CaptionContainer.RecalcSize();
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			Form val2 = null;
			if (val != null)
			{
				val2 = val.FindForm();
			}
			if (val2 != null && (int)val2.WindowState == 2)
			{
				flag = true;
			}
			if (val2 is RibbonForm)
			{
				NonClientInfo nonClientInfo = ((RibbonForm)(object)val2).GetNonClientInfo();
				frameBorderSize.Width = nonClientInfo.LeftBorder;
				frameBorderSize.Height = nonClientInfo.BottomBorder;
			}
			if (m_RibbonStrip.CaptionHeight == 0 && m_SystemCaptionItem.TopInternal < frameBorderSize.Height - 1)
			{
				if (Environment.OSVersion.Version.Major >= 6 && !isGlassEnabled)
				{
					if (flag)
					{
						m_SystemCaptionItem.TopInternal = 1;
						if (_WindowIcon != null)
						{
							_WindowIcon.TopInternal = 1;
						}
					}
					else
					{
						m_SystemCaptionItem.TopInternal = frameBorderSize.Height - 4;
						if (_WindowIcon != null)
						{
							_WindowIcon.TopInternal = Math.Max(0, frameBorderSize.Height - 5);
						}
					}
				}
				else
				{
					if (EffectiveStyle == eDotNetBarStyle.Office2010 || EffectiveStyle == eDotNetBarStyle.Metro)
					{
						m_SystemCaptionItem.TopInternal = 1;
					}
					else
					{
						m_SystemCaptionItem.TopInternal = 0;
					}
					if (isGlassEnabled)
					{
						if (_WindowIcon != null)
						{
							_WindowIcon.TopInternal = frameBorderSize.Height - 2;
						}
						if (_IconSeparator != null)
						{
							_IconSeparator.TopInternal = frameBorderSize.Height - 4;
						}
					}
					else
					{
						if (_WindowIcon != null)
						{
							_WindowIcon.TopInternal = frameBorderSize.Height + 2;
						}
						if (_IconSeparator != null)
						{
							_IconSeparator.TopInternal = frameBorderSize.Height;
						}
					}
				}
			}
			if (Environment.OSVersion.Version.Major >= 6)
			{
				int num = 2;
				if (!isGlassEnabled)
				{
					num++;
				}
				else if (flag)
				{
					num++;
				}
				int num2 = 0;
				foreach (BaseItem subItem in m_CaptionContainer.SubItems)
				{
					if (!(subItem is ApplicationButton) && subItem != m_SystemCaptionItem)
					{
						subItem.TopInternal += num;
					}
					num2 = Math.Max(subItem.Bounds.Bottom, num2);
				}
				if (m_CaptionContainer.MoreItems != null)
				{
					m_CaptionContainer.MoreItems.TopInternal += num;
				}
				if (num2 > m_CaptionContainer.HeightInternal)
				{
					m_CaptionContainer.SetDisplayRectangle(new Rectangle(m_CaptionContainer.Bounds.X, m_CaptionContainer.Bounds.Y, m_CaptionContainer.Bounds.Width, num2));
				}
			}
			else
			{
				int num3 = 0;
				foreach (BaseItem subItem2 in m_CaptionContainer.SubItems)
				{
					if (subItem2.HeightInternal < m_SystemCaptionItem.HeightInternal && subItem2 != _IconSeparator && subItem2 != _WindowIcon)
					{
						subItem2.TopInternal = m_SystemCaptionItem.Bounds.Bottom - (subItem2.HeightInternal + ((subItem2 is LabelItem) ? 2 : 0));
						num3 = Math.Max(subItem2.Bounds.Bottom, num3);
					}
				}
				if (m_CaptionContainer.MoreItems != null)
				{
					m_CaptionContainer.MoreItems.TopInternal += m_SystemCaptionItem.HeightInternal - m_CaptionContainer.MoreItems.HeightInternal;
				}
				if (num3 > m_CaptionContainer.HeightInternal)
				{
					m_CaptionContainer.SetDisplayRectangle(new Rectangle(m_CaptionContainer.Bounds.X, m_CaptionContainer.Bounds.Y, m_CaptionContainer.Bounds.Width, num3));
				}
			}
			if (m_ItemContainer.HeightInternal == 0)
			{
				HeightInternal = m_CaptionContainer.HeightInternal;
			}
			else
			{
				HeightInternal = m_ItemContainer.Bounds.Bottom;
			}
		}
		else
		{
			int num4 = m_ItemContainer.HeightInternal;
			if (m_RibbonStrip.TabGroupsVisible)
			{
				num4 += m_RibbonStrip.TabGroupHeight + 1;
			}
			HeightInternal = num4;
		}
		base.RecalcSize();
	}

	private Rectangle GetItemContainerBounds()
	{
		return m_RibbonStrip.GetItemContainerBounds();
	}

	private Rectangle GetCaptionContainerBounds()
	{
		return m_RibbonStrip.GetCaptionContainerBounds();
	}

	public override BaseItem Copy()
	{
		RibbonStripContainerItem ribbonStripContainerItem = new RibbonStripContainerItem(m_RibbonStrip);
		CopyToItem(ribbonStripContainerItem);
		return ribbonStripContainerItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		RibbonStripContainerItem objCopy = copy as RibbonStripContainerItem;
		base.CopyToItem(objCopy);
	}

	public override void InternalClick(MouseButtons mb, Point mpos)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		m_ItemContainer.InternalClick(mb, mpos);
		if (m_RibbonStrip.CaptionVisible)
		{
			m_CaptionContainer.InternalClick(mb, mpos);
		}
	}

	public override void InternalDoubleClick(MouseButtons mb, Point mpos)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		m_ItemContainer.InternalDoubleClick(mb, mpos);
		if (m_RibbonStrip.CaptionVisible)
		{
			m_CaptionContainer.InternalDoubleClick(mb, mpos);
		}
	}

	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		m_ItemContainer.InternalMouseDown(objArg);
		if (m_RibbonStrip.CaptionVisible && ((DesignMode && m_CaptionContainer.ItemAtLocation(objArg.X, objArg.Y) != null) || !DesignMode))
		{
			m_CaptionContainer.InternalMouseDown(objArg);
		}
	}

	public override void InternalMouseHover()
	{
		m_ItemContainer.InternalMouseHover();
		if (m_RibbonStrip.CaptionVisible)
		{
			m_CaptionContainer.InternalMouseHover();
		}
	}

	public override void InternalMouseLeave()
	{
		m_ItemContainer.InternalMouseLeave();
		if (m_RibbonStrip.CaptionVisible)
		{
			m_CaptionContainer.InternalMouseLeave();
		}
	}

	public override void InternalMouseMove(MouseEventArgs objArg)
	{
		m_ItemContainer.InternalMouseMove(objArg);
		if (m_RibbonStrip.CaptionVisible)
		{
			m_CaptionContainer.InternalMouseMove(objArg);
		}
	}

	public override void InternalMouseUp(MouseEventArgs objArg)
	{
		m_ItemContainer.InternalMouseUp(objArg);
		if (m_RibbonStrip.CaptionVisible)
		{
			m_CaptionContainer.InternalMouseUp(objArg);
		}
	}

	public override void InternalKeyDown(KeyEventArgs objArg)
	{
		BaseItem baseItem = ExpandedItem();
		if (baseItem == null)
		{
			baseItem = m_CaptionContainer.ExpandedItem();
		}
		if (baseItem == null)
		{
			baseItem = m_ItemContainer.ExpandedItem();
		}
		if (baseItem == null || !m_RibbonStrip.CaptionVisible)
		{
			m_ItemContainer.InternalKeyDown(objArg);
			if (!objArg.Handled && m_RibbonStrip.CaptionVisible)
			{
				m_CaptionContainer.InternalKeyDown(objArg);
			}
		}
		else if (baseItem.Parent == m_ItemContainer)
		{
			m_ItemContainer.InternalKeyDown(objArg);
		}
		else
		{
			m_CaptionContainer.InternalKeyDown(objArg);
		}
	}

	public override BaseItem ItemAtLocation(int x, int y)
	{
		if (m_ItemContainer.DisplayRectangle.Contains(x, y))
		{
			return m_ItemContainer.ItemAtLocation(x, y);
		}
		if (m_CaptionContainer.DisplayRectangle.Contains(x, y))
		{
			return m_CaptionContainer.ItemAtLocation(x, y);
		}
		return null;
	}

	protected override void OnStyleChanged()
	{
		if (EffectiveStyle == eDotNetBarStyle.Office2010 || EffectiveStyle == eDotNetBarStyle.Metro)
		{
			if (_WindowIcon == null)
			{
				_IconSeparator = new Separator("sys_caption_separator");
				_IconSeparator.SetSystemItem(b: true);
				m_CaptionContainer.SubItems._Add(_IconSeparator, 0);
				_WindowIcon = new SystemCaptionItem();
				_WindowIcon.Name = "sys_caption_icon";
				_WindowIcon.Enabled = false;
				_WindowIcon.Style = Style;
				_WindowIcon.IsSystemIcon = true;
				_WindowIcon.QueryIconOnPaint = true;
				m_CaptionContainer.SubItems._Add(_WindowIcon, 0);
			}
		}
		else if (EffectiveStyle == eDotNetBarStyle.Windows7)
		{
			if (_WindowIcon == null)
			{
				_IconSeparator = new Separator("sys_caption_separator");
				_IconSeparator.FixedSize = new Size(3, 12);
				_IconSeparator.SetSystemItem(b: true);
				m_CaptionContainer.SubItems._Add(_IconSeparator, 0);
				_WindowIcon = new SystemCaptionItem();
				_WindowIcon.Name = "sys_caption_icon";
				_WindowIcon.Enabled = false;
				_WindowIcon.Style = Style;
				_WindowIcon.IsSystemIcon = true;
				_WindowIcon.QueryIconOnPaint = true;
				m_CaptionContainer.SubItems._Add(_WindowIcon, 0);
			}
		}
		else if (_WindowIcon != null)
		{
			if (m_CaptionContainer.SubItems.Contains(_WindowIcon))
			{
				m_CaptionContainer.SubItems.RemoveItem(_WindowIcon);
			}
			_WindowIcon.Dispose();
			_WindowIcon = null;
			if (m_CaptionContainer.SubItems.Contains(_IconSeparator))
			{
				m_CaptionContainer.SubItems.RemoveItem(_IconSeparator);
			}
			_IconSeparator.Dispose();
			_IconSeparator = null;
		}
		base.OnStyleChanged();
	}

	public InsertPosition GetInsertPosition(Point pScreen, BaseItem DragItem)
	{
		InsertPosition insertPosition = m_ItemContainer.GetInsertPosition(pScreen, DragItem);
		if (insertPosition == null && m_RibbonStrip.CaptionVisible)
		{
			insertPosition = m_CaptionContainer.GetInsertPosition(pScreen, DragItem);
		}
		return insertPosition;
	}

	public void DrawReversibleMarker(int iPos, bool Before)
	{
	}

	public void InsertItemAt(BaseItem objItem, int iPos, bool Before)
	{
	}
}
