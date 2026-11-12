using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class DesignTimeProviderContainer
{
	public static InsertPosition GetInsertPosition(BaseItem containerItem, Point pScreen, BaseItem DragItem)
	{
		InsertPosition insertPosition = null;
		Control val = null;
		val = (Control)((!(containerItem is PopupItem) || !containerItem.Expanded) ? ((object)/*isinst with value type is only supported in some contexts*/) : ((object)((PopupItem)containerItem).PopupControl));
		if (val == null)
		{
			return null;
		}
		Point pt = val.PointToClient(pScreen);
		Rectangle displayRectangle = containerItem.DisplayRectangle;
		if (containerItem is PopupItem && containerItem.Expanded)
		{
			displayRectangle = val.DisplayRectangle;
		}
		if (displayRectangle.Contains(pt) || (containerItem.SubItems.Count == 0 && val.ClientRectangle.Contains(pt)) || (containerItem is ItemContainer && ((ItemContainer)containerItem).SystemContainer && val.ClientRectangle.Contains(pt)))
		{
			BaseItem baseItem = containerItem.ExpandedItem();
			if (baseItem != null && baseItem is IDesignTimeProvider designTimeProvider)
			{
				insertPosition = designTimeProvider.GetInsertPosition(pScreen, DragItem);
				if (insertPosition != null)
				{
					return insertPosition;
				}
			}
			for (int i = 0; i < containerItem.SubItems.Count; i++)
			{
				baseItem = containerItem.SubItems[i];
				Rectangle displayRectangle2 = baseItem.DisplayRectangle;
				displayRectangle2.Inflate(2, 2);
				if (!baseItem.Visible || !displayRectangle2.Contains(pt))
				{
					continue;
				}
				if (baseItem.SystemItem && containerItem.SubItems.Count != 1)
				{
					return null;
				}
				if (baseItem == DragItem)
				{
					return new InsertPosition();
				}
				if (baseItem.IsContainer && baseItem is IDesignTimeProvider)
				{
					Rectangle rectangle = displayRectangle2;
					rectangle.Inflate(-8, -8);
					if (rectangle.Contains(pt))
					{
						return ((IDesignTimeProvider)baseItem).GetInsertPosition(pScreen, DragItem);
					}
				}
				if (!containerItem.AllowDrop)
				{
					continue;
				}
				insertPosition = new InsertPosition();
				insertPosition.TargetProvider = (IDesignTimeProvider)containerItem;
				insertPosition.Position = i;
				if (baseItem.Orientation == eOrientation.Horizontal && !baseItem.IsOnMenu)
				{
					if (pt.X <= baseItem.LeftInternal + baseItem.WidthInternal / 2 || baseItem.SystemItem)
					{
						insertPosition.Before = true;
					}
				}
				else if (pt.Y <= baseItem.TopInternal + baseItem.HeightInternal / 2 || baseItem.SystemItem)
				{
					insertPosition.Before = true;
				}
				if (containerItem.GetOwner() is IOwner owner)
				{
					BaseItem baseItem2 = owner.GetExpandedItem();
					if (baseItem2 != null)
					{
						while (baseItem2.Parent != null)
						{
							baseItem2 = baseItem2.Parent;
						}
						BaseItem baseItem3 = baseItem;
						while (baseItem3.Parent != null)
						{
							baseItem3 = baseItem3.Parent;
						}
						if (baseItem2 != baseItem3)
						{
							owner.SetExpandedItem(null);
						}
					}
				}
				if (baseItem is PopupItem && (baseItem.SubItems.Count > 0 || baseItem.IsOnMenuBar))
				{
					if (!baseItem.Expanded && baseItem.CanCustomize)
					{
						baseItem.Expanded = true;
					}
				}
				else
				{
					BaseItem.CollapseSubItems(containerItem);
				}
				break;
			}
			if (insertPosition == null && containerItem.AllowDrop)
			{
				insertPosition = ((containerItem.SubItems.Count <= 1 || !containerItem.SubItems[containerItem.SubItems.Count - 1].SystemItem) ? new InsertPosition(containerItem.SubItems.Count - 1, bBefore: false, (IDesignTimeProvider)containerItem) : new InsertPosition(containerItem.SubItems.Count - 2, bBefore: true, (IDesignTimeProvider)containerItem));
			}
		}
		else
		{
			foreach (BaseItem subItem in containerItem.SubItems)
			{
				if (subItem != DragItem && subItem is IDesignTimeProvider designTimeProvider2)
				{
					insertPosition = designTimeProvider2.GetInsertPosition(pScreen, DragItem);
					if (insertPosition != null)
					{
						break;
					}
				}
			}
		}
		return insertPosition;
	}

	public static void DrawReversibleMarker(BaseItem containerItem, int iPos, bool Before)
	{
		object containerControl = containerItem.ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val == null)
		{
			return;
		}
		BaseItem baseItem = null;
		if (iPos >= 0)
		{
			baseItem = containerItem.SubItems[iPos];
		}
		if (baseItem != null)
		{
			if (baseItem.DesignInsertMarker != 0)
			{
				baseItem.DesignInsertMarker = eDesignInsertPosition.None;
			}
			else if (Before)
			{
				baseItem.DesignInsertMarker = eDesignInsertPosition.Before;
			}
			else
			{
				baseItem.DesignInsertMarker = eDesignInsertPosition.After;
			}
		}
		else
		{
			Rectangle displayRectangle = containerItem.DisplayRectangle;
			displayRectangle.Inflate(-1, -1);
			Rectangle rectangle = new Rectangle(displayRectangle.Left + 2, displayRectangle.Top + 2, 1, displayRectangle.Height - 4);
			Rectangle rectangle2 = new Rectangle(displayRectangle.Left, displayRectangle.Top + 1, 5, 1);
			Rectangle rectangle3 = new Rectangle(displayRectangle.Left, displayRectangle.Bottom - 2, 5, 1);
		}
	}

	public static void InsertItemAt(BaseItem containerItem, BaseItem objItem, int iPos, bool Before)
	{
		if (containerItem.ExpandedItem() != null)
		{
			containerItem.ExpandedItem().Expanded = false;
		}
		if (!Before)
		{
			if (iPos + 1 >= containerItem.SubItems.Count)
			{
				containerItem.SubItems.Add(objItem, GetAppendPosition(containerItem));
			}
			else
			{
				containerItem.SubItems.Add(objItem, iPos + 1);
			}
		}
		else if (iPos >= containerItem.SubItems.Count)
		{
			containerItem.SubItems.Add(objItem, GetAppendPosition(containerItem));
		}
		else
		{
			containerItem.SubItems.Add(objItem, iPos);
		}
		if (containerItem.ContainerControl is Bar)
		{
			((Bar)containerItem.ContainerControl).RecalcLayout();
			return;
		}
		if (containerItem.ContainerControl is MenuPanel)
		{
			((MenuPanel)containerItem.ContainerControl).RecalcSize();
			return;
		}
		if (containerItem.ContainerControl is BarBaseControl)
		{
			((BarBaseControl)containerItem.ContainerControl).RecalcLayout();
			return;
		}
		if (containerItem.ContainerControl is ItemControl)
		{
			((ItemControl)containerItem.ContainerControl).RecalcLayout();
			return;
		}
		containerItem.RecalcSize();
		containerItem.Refresh();
	}

	public static int GetAppendPosition(BaseItem objParent)
	{
		int result = -1;
		int num = objParent.SubItems.Count - 1;
		while (num >= 0 && objParent.SubItems[num].SystemItem)
		{
			result = num;
			num--;
		}
		return result;
	}
}
