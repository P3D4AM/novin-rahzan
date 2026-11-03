using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class SideBarContainerItem : ImageItem, IDesignTimeProvider
{
	private eSideBarAppearance _᥍;

	private bool _1DF4;

	private int ᅣ = 1;

	[Browsable(false)]
	public override eOrientation Orientation
	{
		get
		{
			return eOrientation.Horizontal;
		}
		set
		{
		}
	}

	internal eSideBarAppearance _1DF3
	{
		get
		{
			return _᥍;
		}
		set
		{
			if (_᥍ == value)
			{
				return;
			}
			_᥍ = value;
			if (_᥍ == eSideBarAppearance.Flat)
			{
				ᅣ = -1;
			}
			else
			{
				ᅣ = 1;
			}
			foreach (SideBarPanelItem subItem in SubItems)
			{
				subItem.Appearance = value;
			}
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[DefaultValue(false)]
	[Browsable(false)]
	public override bool Expanded
	{
		get
		{
			return base.Expanded;
		}
		set
		{
			if (!value)
			{
				foreach (BaseItem subItem in m_SubItems)
				{
					if (!(subItem is SideBarPanelItem))
					{
						continue;
					}
					foreach (BaseItem subItem2 in subItem.SubItems)
					{
						if (subItem2 is PopupItem && subItem.Expanded)
						{
							subItem2.Expanded = false;
						}
					}
				}
			}
			base.Expanded = value;
		}
	}

	public SideBarContainerItem()
	{
		m_IsContainer = true;
		m_SystemItem = true;
		m_SupportedOrientation = eSupportedOrientation.Horizontal;
		m_AllowOnlyOneSubItemExpanded = false;
	}

	public override BaseItem Copy()
	{
		SideBarContainerItem sideBarContainerItem = new SideBarContainerItem();
		CopyToItem(sideBarContainerItem);
		return sideBarContainerItem;
	}

	protected override void CopyToItem(BaseItem copy)
	{
		SideBarContainerItem objCopy = copy as SideBarContainerItem;
		base.CopyToItem(objCopy);
	}

	public override void RecalcSize()
	{
		int num = 0;
		int num2 = 0;
		bool flag = false;
		if (m_SubItems != null)
		{
			SideBarPanelItem sideBarPanelItem = null;
			foreach (BaseItem subItem in m_SubItems)
			{
				if (!subItem.Visible)
				{
					continue;
				}
				if (num2 > HeightInternal)
				{
					subItem.Displayed = false;
					flag = true;
					continue;
				}
				subItem.WidthInternal = WidthInternal;
				subItem.HeightInternal = 0;
				subItem.RecalcSize();
				if (subItem.WidthInternal != WidthInternal)
				{
					subItem.WidthInternal = WidthInternal;
				}
				if (subItem.Expanded && subItem is SideBarPanelItem && sideBarPanelItem == null)
				{
					sideBarPanelItem = subItem as SideBarPanelItem;
				}
				subItem.LeftInternal = m_Rect.Left;
				subItem.TopInternal = m_Rect.Top + num2;
				num2 += ᅣ;
				if (!subItem.Expanded)
				{
					num += subItem.HeightInternal + ᅣ;
				}
				num2 += subItem.HeightInternal;
				subItem.Displayed = true;
			}
			if (sideBarPanelItem != null && !flag)
			{
				sideBarPanelItem.HeightInternal = m_Rect.Height - num;
				sideBarPanelItem.RecalcSize();
				int num3 = sideBarPanelItem.DisplayRectangle.Bottom + ᅣ;
				if (_᥍ == eSideBarAppearance.Flat)
				{
					num3--;
				}
				for (int i = m_SubItems.IndexOf(sideBarPanelItem) + 1; i < m_SubItems.Count; i++)
				{
					BaseItem baseItem2 = m_SubItems[i];
					if (baseItem2.Visible)
					{
						baseItem2.TopInternal = num3;
						num3 += baseItem2.HeightInternal + ᅣ;
					}
				}
			}
		}
		base.RecalcSize();
	}

	public override void Paint(ItemPaintArgs pa)
	{
		if (SuspendLayout)
		{
			return;
		}
		_ = pa.Graphics;
		if (m_NeedRecalcSize)
		{
			RecalcSize();
		}
		if (m_SubItems == null)
		{
			return;
		}
		foreach (BaseItem subItem in m_SubItems)
		{
			if (subItem.Visible && subItem.Displayed)
			{
				subItem.Paint(pa);
			}
		}
	}

	protected internal override void OnSubItemExpandChange(BaseItem item)
	{
		if (_1DF4)
		{
			return;
		}
		_1DF4 = true;
		try
		{
			if (item.Expanded)
			{
				foreach (BaseItem subItem in m_SubItems)
				{
					if (subItem.Expanded && subItem != item)
					{
						subItem.Expanded = false;
					}
				}
			}
			else
			{
				bool flag = false;
				foreach (BaseItem subItem2 in m_SubItems)
				{
					if (subItem2.Expanded)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					item.Expanded = true;
				}
			}
			RecalcSize();
			object containerControl = ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val != null)
			{
				val.Refresh();
			}
		}
		finally
		{
			_1DF4 = false;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override void InternalMouseDown(MouseEventArgs objArg)
	{
		base.InternalMouseDown(objArg);
		if (DesignMode && ItemAtLocation(objArg.X, objArg.Y) == null && GetOwner() is IOwner owner)
		{
			owner.SetFocusItem(null);
		}
	}

	public bool FocusNextItem()
	{
		bool result = true;
		BaseItem focusItem = ((IOwner)GetOwner()).GetFocusItem();
		bool flag = false;
		if (focusItem == null)
		{
			flag = true;
		}
		for (int i = 0; i < 2; i++)
		{
			foreach (BaseItem subItem in SubItems)
			{
				if (subItem == focusItem)
				{
					flag = true;
				}
				else if (subItem.Visible && flag)
				{
					((IOwner)GetOwner()).SetFocusItem(subItem);
					i = 2;
					result = false;
					break;
				}
				if (!subItem.Expanded || !subItem.Visible)
				{
					continue;
				}
				foreach (BaseItem subItem2 in subItem.SubItems)
				{
					if (subItem2 == focusItem)
					{
						flag = true;
					}
					else if (subItem.Visible && flag)
					{
						((IOwner)GetOwner()).SetFocusItem(subItem2);
						i = 2;
						result = false;
						break;
					}
				}
				if (i == 2)
				{
					break;
				}
			}
		}
		return result;
	}

	public bool FocusPreviousItem()
	{
		bool result = true;
		BaseItem focusItem = ((IOwner)GetOwner()).GetFocusItem();
		bool flag = false;
		if (focusItem == null)
		{
			flag = true;
		}
		for (int i = 0; i < 2; i++)
		{
			for (int num = SubItems.Count - 1; num >= 0; num--)
			{
				BaseItem baseItem = SubItems[num];
				if (baseItem.Expanded && baseItem.Visible)
				{
					for (int num2 = baseItem.SubItems.Count - 1; num2 >= 0; num2--)
					{
						BaseItem baseItem2 = baseItem.SubItems[num2];
						if (baseItem2 == focusItem)
						{
							flag = true;
						}
						else if (baseItem.Visible && flag)
						{
							((IOwner)GetOwner()).SetFocusItem(baseItem2);
							i = 2;
							result = false;
							break;
						}
					}
					if (i == 2)
					{
						break;
					}
				}
				if (baseItem == focusItem)
				{
					flag = true;
				}
				else if (baseItem.Visible && flag)
				{
					((IOwner)GetOwner()).SetFocusItem(baseItem);
					i = 2;
					result = false;
					break;
				}
			}
		}
		return result;
	}

	protected internal override void OnItemAdded(BaseItem item)
	{
		base.OnItemAdded(item);
		NeedRecalcSize = true;
		if (ContainerControl is SideBar && item is SideBarPanelItem)
		{
			SideBarPanelItem sideBarPanelItem = item as SideBarPanelItem;
			sideBarPanelItem.Appearance = ((SideBar)ContainerControl).Appearance;
			sideBarPanelItem.ᥒ();
		}
		if (m_SubItems.Count == 1)
		{
			item.Expanded = true;
		}
		if (DesignMode)
		{
			Refresh();
		}
	}

	protected internal override void OnAfterItemRemoved(BaseItem item, int itemIndex)
	{
		base.OnAfterItemRemoved(item, itemIndex);
		NeedRecalcSize = true;
		if (DesignMode)
		{
			Refresh();
		}
	}

	protected internal override void OnSubItemsClear()
	{
		base.OnSubItemsClear();
		NeedRecalcSize = true;
		if (DesignMode)
		{
			Refresh();
		}
	}

	private InsertPosition ᆉ(Point ॺ, BaseItem ᆊ)
	{
		foreach (BaseItem subItem in SubItems)
		{
			if (subItem.Visible && subItem is IDesignTimeProvider)
			{
				InsertPosition insertPosition = ((IDesignTimeProvider)subItem).GetInsertPosition(ॺ, ᆊ);
				if (insertPosition != null)
				{
					return insertPosition;
				}
			}
		}
		return null;
	}

	InsertPosition IDesignTimeProvider.GetInsertPosition(Point ॺ, BaseItem ᆊ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆉ
		return this.ᆉ(ॺ, ᆊ);
	}

	private void ᆋ(int ᆌ, bool ᆍ)
	{
	}

	void IDesignTimeProvider.DrawReversibleMarker(int ᆌ, bool ᆍ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆋ
		this.ᆋ(ᆌ, ᆍ);
	}

	private void ᆎ(BaseItem ਤ, int ᆌ, bool ᆍ)
	{
	}

	void IDesignTimeProvider.InsertItemAt(BaseItem ਤ, int ᆌ, bool ᆍ)
	{
		//ILSpy generated this explicit interface implementation from .override directive in ᆎ
		this.ᆎ(ਤ, ᆌ, ᆍ);
	}
}
