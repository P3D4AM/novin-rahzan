using System;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class ContainerKeyboardNavigation
{
	internal static void ProcessKeyDown(BaseItem container, KeyEventArgs objArg)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Invalid comparison between Unknown and I4
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Invalid comparison between Unknown and I4
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Invalid comparison between Unknown and I4
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Invalid comparison between Unknown and I4
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Invalid comparison between Unknown and I4
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Invalid comparison between Unknown and I4
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Invalid comparison between Unknown and I4
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Invalid comparison between Unknown and I4
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Invalid comparison between Unknown and I4
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Invalid comparison between Unknown and I4
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Invalid comparison between Unknown and I4
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		if (container.SubItems.Count == 0 || objArg.Handled)
		{
			return;
		}
		BaseItem baseItem = container.ExpandedItem();
		if (baseItem != null)
		{
			baseItem.InternalKeyDown(objArg);
			if (objArg.Handled)
			{
				return;
			}
		}
		eOrientation eOrientation2 = container.Orientation;
		if (container is ItemContainer)
		{
			eOrientation2 = ((ItemContainer)container).LayoutOrientation;
		}
		if ((eOrientation2 == eOrientation.Horizontal && ((int)objArg.KeyCode == 37 || (int)objArg.KeyCode == 39 || (container.HotSubItem == null && ((int)objArg.KeyCode == 40 || (int)objArg.KeyCode == 38)))) || (eOrientation2 == eOrientation.Vertical && ((int)objArg.KeyCode == 38 || (int)objArg.KeyCode == 40 || (container.HotSubItem == null && ((int)objArg.KeyCode == 39 || (int)objArg.KeyCode == 37)))))
		{
			if (container.HotSubItem != null)
			{
				container.HotSubItem.InternalMouseLeave();
				if (container.AutoExpand && container.HotSubItem.Expanded)
				{
					container.HotSubItem.Expanded = false;
				}
			}
			if ((int)objArg.KeyCode == 37 || (int)objArg.KeyCode == 38)
			{
				int num = 0;
				if (container.HotSubItem != null)
				{
					num = container.SubItems.IndexOf(container.HotSubItem) - 1;
				}
				if (num < 0)
				{
					num = container.SubItems.Count - 1;
				}
				BaseItem baseItem2 = null;
				bool flag = false;
				do
				{
					for (int num2 = num; num2 >= 0; num2--)
					{
						baseItem2 = container.SubItems[num2];
						if (CanFocus(baseItem2))
						{
							num = num2;
							break;
						}
					}
					if (!CanFocus(container.SubItems[num]))
					{
						if (!flag)
						{
							num = container.SubItems.Count - 1;
							flag = true;
						}
						else
						{
							flag = false;
						}
					}
					else
					{
						flag = false;
					}
				}
				while (flag);
				container.HotSubItem = container.SubItems[num];
			}
			else
			{
				int i = 0;
				if (container.HotSubItem != null)
				{
					i = container.SubItems.IndexOf(container.HotSubItem) + 1;
				}
				for (; i < container.SubItems.Count && !CanFocus(container.SubItems[i]); i++)
				{
				}
				if (i >= container.SubItems.Count)
				{
					i = 0;
				}
				BaseItem baseItem3 = null;
				for (int j = i; j < container.SubItems.Count; j++)
				{
					baseItem3 = container.SubItems[j];
					if (CanFocus(baseItem3))
					{
						i = j;
						break;
					}
				}
				container.HotSubItem = container.SubItems[i];
			}
			if (container.HotSubItem != null)
			{
				if (container.HotSubItem is ItemContainer)
				{
					((ItemContainer)container.HotSubItem).SelectFirstItem();
				}
				else
				{
					container.HotSubItem.InternalMouseEnter();
					container.HotSubItem.InternalMouseMove(new MouseEventArgs((MouseButtons)0, 0, container.HotSubItem.LeftInternal + 1, container.HotSubItem.TopInternal + 1, 0));
					if (container.ContainerControl is IScrollableItemControl scrollableItemControl)
					{
						scrollableItemControl.KeyboardItemSelected(container.HotSubItem);
					}
				}
			}
			objArg.Handled = true;
			return;
		}
		if ((int)objArg.KeyCode == 27)
		{
			if (baseItem != null)
			{
				baseItem.Expanded = false;
				objArg.Handled = true;
				return;
			}
			object containerControl = container.ContainerControl;
			Control val = (Control)((containerControl is Control) ? containerControl : null);
			if (val is Bar)
			{
				Bar bar = val as Bar;
				if (bar.BarState == eBarState.Popup)
				{
					bar.ParentItem.Expanded = false;
				}
				else if (container.AutoExpand)
				{
					container.AutoExpand = false;
				}
				else if (((Control)bar).Focused || bar.MenuFocus)
				{
					bar.MenuFocus = false;
					bar.ReleaseFocus();
				}
				objArg.Handled = true;
			}
			else if (val is ItemControl)
			{
				ItemControl itemControl = val as ItemControl;
				if (container.AutoExpand)
				{
					container.AutoExpand = false;
				}
				else if (((Control)itemControl).Focused || itemControl.MenuFocus)
				{
					itemControl.MenuFocus = false;
					itemControl.ReleaseFocus();
				}
			}
			return;
		}
		BaseItem baseItem4 = container.ExpandedItem();
		if (baseItem4 != null)
		{
			baseItem4.InternalKeyDown(objArg);
			return;
		}
		int num3 = 0;
		if (objArg.Shift)
		{
			try
			{
				byte[] array = new byte[256];
				if (NativeFunctions.GetKeyboardState(array))
				{
					byte[] array2 = new byte[2];
					if (NativeFunctions.ToAscii((uint)objArg.KeyValue, 0u, array, array2, 0u) != 0)
					{
						num3 = array2[0];
					}
				}
			}
			catch (Exception)
			{
				num3 = 0;
			}
		}
		if (num3 == 0)
		{
			num3 = (int)NativeFunctions.MapVirtualKey((uint)objArg.KeyValue, 2u);
		}
		if (container.HotSubItem != null)
		{
			container.HotSubItem.InternalKeyDown(objArg);
		}
	}

	private static bool CanFocus(BaseItem objNew)
	{
		if (objNew == null)
		{
			return false;
		}
		if (!objNew.Visible || !objNew.GetEnabled())
		{
			return false;
		}
		if (objNew is LabelItem)
		{
			return false;
		}
		return true;
	}
}
