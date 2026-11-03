using System;
using System.Threading;

namespace DevComponents.DotNetBar.Rendering;

public abstract class BaseRenderer
{
	private KeyTipsRendererEventHandler _0D3B;

	private RibbonTabGroupRendererEventHandler _0D3C;

	private ItemContainerRendererEventHandler ഽ;

	private ItemContainerSeparatorRendererEventHandler _0D3E;

	private ButtonItemRendererEventHandler _0D3F;

	private RibbonTabItemRendererEventHandler _0D40;

	private ToolbarRendererEventHandler _0D41;

	private ToolbarRendererEventHandler _0D42;

	private RibbonBarRendererEventHandler _0D43;

	private RibbonControlRendererEventHandler _0D44;

	private RibbonControlRendererEventHandler _0D45;

	private RibbonControlRendererEventHandler _0D46;

	private ColorItemRendererEventHandler _0D47;

	private SystemCaptionItemRendererEventHandler _0D48;

	private MdiSystemItemRendererEventHandler _0D49;

	private FormCaptionRendererEventHandler _0D4A;

	private QatOverflowItemRendererEventHandler _0D4B;

	private QatCustomizeItemRendererEventHandler _0D4C;

	private CheckBoxItemRendererEventHandler _0D4D;

	private ProgressBarItemRendererEventHandler ൎ;

	private NavPaneRendererEventHandler _0D4F;

	private SliderItemRendererEventHandler _0D50;

	private RangeSliderItemRendererEventHandler _0D51;

	private SideBarRendererEventHandler _0D52;

	private SideBarPanelItemRendererEventHandler _0D53;

	private ButtonItemRendererEventHandler ൔ;

	private ButtonItemRendererEventHandler ൕ;

	private SwitchButtonRendererEventHandler ൖ;

	private StepItemRendererEventHandler _0D57;

	private ListBoxItemRendererEventHandler _0D58;

	private SideNavItemRendererEventHandler _0D59;

	private TabFormStripRendererEventHandler _0D5A;

	private ButtonItemRendererEventHandler _0D5B;

	private ButtonItemRendererEventHandler _0D5C;

	private TabFormRendererEventHandler _0D5D;

	public event KeyTipsRendererEventHandler RenderKeyTips
	{
		add
		{
			KeyTipsRendererEventHandler keyTipsRendererEventHandler = _0D3B;
			KeyTipsRendererEventHandler keyTipsRendererEventHandler2;
			do
			{
				keyTipsRendererEventHandler2 = keyTipsRendererEventHandler;
				KeyTipsRendererEventHandler value2 = (KeyTipsRendererEventHandler)Delegate.Combine(keyTipsRendererEventHandler2, value);
				keyTipsRendererEventHandler = Interlocked.CompareExchange(ref _0D3B, value2, keyTipsRendererEventHandler2);
			}
			while ((object)keyTipsRendererEventHandler != keyTipsRendererEventHandler2);
		}
		remove
		{
			KeyTipsRendererEventHandler keyTipsRendererEventHandler = _0D3B;
			KeyTipsRendererEventHandler keyTipsRendererEventHandler2;
			do
			{
				keyTipsRendererEventHandler2 = keyTipsRendererEventHandler;
				KeyTipsRendererEventHandler value2 = (KeyTipsRendererEventHandler)Delegate.Remove(keyTipsRendererEventHandler2, value);
				keyTipsRendererEventHandler = Interlocked.CompareExchange(ref _0D3B, value2, keyTipsRendererEventHandler2);
			}
			while ((object)keyTipsRendererEventHandler != keyTipsRendererEventHandler2);
		}
	}

	public event RibbonTabGroupRendererEventHandler RenderRibbonTabGroup
	{
		add
		{
			RibbonTabGroupRendererEventHandler ribbonTabGroupRendererEventHandler = _0D3C;
			RibbonTabGroupRendererEventHandler ribbonTabGroupRendererEventHandler2;
			do
			{
				ribbonTabGroupRendererEventHandler2 = ribbonTabGroupRendererEventHandler;
				RibbonTabGroupRendererEventHandler value2 = (RibbonTabGroupRendererEventHandler)Delegate.Combine(ribbonTabGroupRendererEventHandler2, value);
				ribbonTabGroupRendererEventHandler = Interlocked.CompareExchange(ref _0D3C, value2, ribbonTabGroupRendererEventHandler2);
			}
			while ((object)ribbonTabGroupRendererEventHandler != ribbonTabGroupRendererEventHandler2);
		}
		remove
		{
			RibbonTabGroupRendererEventHandler ribbonTabGroupRendererEventHandler = _0D3C;
			RibbonTabGroupRendererEventHandler ribbonTabGroupRendererEventHandler2;
			do
			{
				ribbonTabGroupRendererEventHandler2 = ribbonTabGroupRendererEventHandler;
				RibbonTabGroupRendererEventHandler value2 = (RibbonTabGroupRendererEventHandler)Delegate.Remove(ribbonTabGroupRendererEventHandler2, value);
				ribbonTabGroupRendererEventHandler = Interlocked.CompareExchange(ref _0D3C, value2, ribbonTabGroupRendererEventHandler2);
			}
			while ((object)ribbonTabGroupRendererEventHandler != ribbonTabGroupRendererEventHandler2);
		}
	}

	public event ItemContainerRendererEventHandler RenderItemContainer
	{
		add
		{
			ItemContainerRendererEventHandler itemContainerRendererEventHandler = ഽ;
			ItemContainerRendererEventHandler itemContainerRendererEventHandler2;
			do
			{
				itemContainerRendererEventHandler2 = itemContainerRendererEventHandler;
				ItemContainerRendererEventHandler value2 = (ItemContainerRendererEventHandler)Delegate.Combine(itemContainerRendererEventHandler2, value);
				itemContainerRendererEventHandler = Interlocked.CompareExchange(ref ഽ, value2, itemContainerRendererEventHandler2);
			}
			while ((object)itemContainerRendererEventHandler != itemContainerRendererEventHandler2);
		}
		remove
		{
			ItemContainerRendererEventHandler itemContainerRendererEventHandler = ഽ;
			ItemContainerRendererEventHandler itemContainerRendererEventHandler2;
			do
			{
				itemContainerRendererEventHandler2 = itemContainerRendererEventHandler;
				ItemContainerRendererEventHandler value2 = (ItemContainerRendererEventHandler)Delegate.Remove(itemContainerRendererEventHandler2, value);
				itemContainerRendererEventHandler = Interlocked.CompareExchange(ref ഽ, value2, itemContainerRendererEventHandler2);
			}
			while ((object)itemContainerRendererEventHandler != itemContainerRendererEventHandler2);
		}
	}

	public event ItemContainerSeparatorRendererEventHandler RenderItemContainerSeparator
	{
		add
		{
			ItemContainerSeparatorRendererEventHandler itemContainerSeparatorRendererEventHandler = _0D3E;
			ItemContainerSeparatorRendererEventHandler itemContainerSeparatorRendererEventHandler2;
			do
			{
				itemContainerSeparatorRendererEventHandler2 = itemContainerSeparatorRendererEventHandler;
				ItemContainerSeparatorRendererEventHandler value2 = (ItemContainerSeparatorRendererEventHandler)Delegate.Combine(itemContainerSeparatorRendererEventHandler2, value);
				itemContainerSeparatorRendererEventHandler = Interlocked.CompareExchange(ref _0D3E, value2, itemContainerSeparatorRendererEventHandler2);
			}
			while ((object)itemContainerSeparatorRendererEventHandler != itemContainerSeparatorRendererEventHandler2);
		}
		remove
		{
			ItemContainerSeparatorRendererEventHandler itemContainerSeparatorRendererEventHandler = _0D3E;
			ItemContainerSeparatorRendererEventHandler itemContainerSeparatorRendererEventHandler2;
			do
			{
				itemContainerSeparatorRendererEventHandler2 = itemContainerSeparatorRendererEventHandler;
				ItemContainerSeparatorRendererEventHandler value2 = (ItemContainerSeparatorRendererEventHandler)Delegate.Remove(itemContainerSeparatorRendererEventHandler2, value);
				itemContainerSeparatorRendererEventHandler = Interlocked.CompareExchange(ref _0D3E, value2, itemContainerSeparatorRendererEventHandler2);
			}
			while ((object)itemContainerSeparatorRendererEventHandler != itemContainerSeparatorRendererEventHandler2);
		}
	}

	public event ButtonItemRendererEventHandler RenderButtonItem
	{
		add
		{
			ButtonItemRendererEventHandler buttonItemRendererEventHandler = _0D3F;
			ButtonItemRendererEventHandler buttonItemRendererEventHandler2;
			do
			{
				buttonItemRendererEventHandler2 = buttonItemRendererEventHandler;
				ButtonItemRendererEventHandler value2 = (ButtonItemRendererEventHandler)Delegate.Combine(buttonItemRendererEventHandler2, value);
				buttonItemRendererEventHandler = Interlocked.CompareExchange(ref _0D3F, value2, buttonItemRendererEventHandler2);
			}
			while ((object)buttonItemRendererEventHandler != buttonItemRendererEventHandler2);
		}
		remove
		{
			ButtonItemRendererEventHandler buttonItemRendererEventHandler = _0D3F;
			ButtonItemRendererEventHandler buttonItemRendererEventHandler2;
			do
			{
				buttonItemRendererEventHandler2 = buttonItemRendererEventHandler;
				ButtonItemRendererEventHandler value2 = (ButtonItemRendererEventHandler)Delegate.Remove(buttonItemRendererEventHandler2, value);
				buttonItemRendererEventHandler = Interlocked.CompareExchange(ref _0D3F, value2, buttonItemRendererEventHandler2);
			}
			while ((object)buttonItemRendererEventHandler != buttonItemRendererEventHandler2);
		}
	}

	public event RibbonTabItemRendererEventHandler RenderRibbonTabItem
	{
		add
		{
			RibbonTabItemRendererEventHandler ribbonTabItemRendererEventHandler = _0D40;
			RibbonTabItemRendererEventHandler ribbonTabItemRendererEventHandler2;
			do
			{
				ribbonTabItemRendererEventHandler2 = ribbonTabItemRendererEventHandler;
				RibbonTabItemRendererEventHandler value2 = (RibbonTabItemRendererEventHandler)Delegate.Combine(ribbonTabItemRendererEventHandler2, value);
				ribbonTabItemRendererEventHandler = Interlocked.CompareExchange(ref _0D40, value2, ribbonTabItemRendererEventHandler2);
			}
			while ((object)ribbonTabItemRendererEventHandler != ribbonTabItemRendererEventHandler2);
		}
		remove
		{
			RibbonTabItemRendererEventHandler ribbonTabItemRendererEventHandler = _0D40;
			RibbonTabItemRendererEventHandler ribbonTabItemRendererEventHandler2;
			do
			{
				ribbonTabItemRendererEventHandler2 = ribbonTabItemRendererEventHandler;
				RibbonTabItemRendererEventHandler value2 = (RibbonTabItemRendererEventHandler)Delegate.Remove(ribbonTabItemRendererEventHandler2, value);
				ribbonTabItemRendererEventHandler = Interlocked.CompareExchange(ref _0D40, value2, ribbonTabItemRendererEventHandler2);
			}
			while ((object)ribbonTabItemRendererEventHandler != ribbonTabItemRendererEventHandler2);
		}
	}

	public event ToolbarRendererEventHandler RenderToolbarBackground
	{
		add
		{
			ToolbarRendererEventHandler toolbarRendererEventHandler = _0D41;
			ToolbarRendererEventHandler toolbarRendererEventHandler2;
			do
			{
				toolbarRendererEventHandler2 = toolbarRendererEventHandler;
				ToolbarRendererEventHandler value2 = (ToolbarRendererEventHandler)Delegate.Combine(toolbarRendererEventHandler2, value);
				toolbarRendererEventHandler = Interlocked.CompareExchange(ref _0D41, value2, toolbarRendererEventHandler2);
			}
			while ((object)toolbarRendererEventHandler != toolbarRendererEventHandler2);
		}
		remove
		{
			ToolbarRendererEventHandler toolbarRendererEventHandler = _0D41;
			ToolbarRendererEventHandler toolbarRendererEventHandler2;
			do
			{
				toolbarRendererEventHandler2 = toolbarRendererEventHandler;
				ToolbarRendererEventHandler value2 = (ToolbarRendererEventHandler)Delegate.Remove(toolbarRendererEventHandler2, value);
				toolbarRendererEventHandler = Interlocked.CompareExchange(ref _0D41, value2, toolbarRendererEventHandler2);
			}
			while ((object)toolbarRendererEventHandler != toolbarRendererEventHandler2);
		}
	}

	public event ToolbarRendererEventHandler RenderPopupToolbarBackground
	{
		add
		{
			ToolbarRendererEventHandler toolbarRendererEventHandler = _0D42;
			ToolbarRendererEventHandler toolbarRendererEventHandler2;
			do
			{
				toolbarRendererEventHandler2 = toolbarRendererEventHandler;
				ToolbarRendererEventHandler value2 = (ToolbarRendererEventHandler)Delegate.Combine(toolbarRendererEventHandler2, value);
				toolbarRendererEventHandler = Interlocked.CompareExchange(ref _0D42, value2, toolbarRendererEventHandler2);
			}
			while ((object)toolbarRendererEventHandler != toolbarRendererEventHandler2);
		}
		remove
		{
			ToolbarRendererEventHandler toolbarRendererEventHandler = _0D42;
			ToolbarRendererEventHandler toolbarRendererEventHandler2;
			do
			{
				toolbarRendererEventHandler2 = toolbarRendererEventHandler;
				ToolbarRendererEventHandler value2 = (ToolbarRendererEventHandler)Delegate.Remove(toolbarRendererEventHandler2, value);
				toolbarRendererEventHandler = Interlocked.CompareExchange(ref _0D42, value2, toolbarRendererEventHandler2);
			}
			while ((object)toolbarRendererEventHandler != toolbarRendererEventHandler2);
		}
	}

	public event RibbonBarRendererEventHandler RenderRibbonDialogLauncher
	{
		add
		{
			RibbonBarRendererEventHandler ribbonBarRendererEventHandler = _0D43;
			RibbonBarRendererEventHandler ribbonBarRendererEventHandler2;
			do
			{
				ribbonBarRendererEventHandler2 = ribbonBarRendererEventHandler;
				RibbonBarRendererEventHandler value2 = (RibbonBarRendererEventHandler)Delegate.Combine(ribbonBarRendererEventHandler2, value);
				ribbonBarRendererEventHandler = Interlocked.CompareExchange(ref _0D43, value2, ribbonBarRendererEventHandler2);
			}
			while ((object)ribbonBarRendererEventHandler != ribbonBarRendererEventHandler2);
		}
		remove
		{
			RibbonBarRendererEventHandler ribbonBarRendererEventHandler = _0D43;
			RibbonBarRendererEventHandler ribbonBarRendererEventHandler2;
			do
			{
				ribbonBarRendererEventHandler2 = ribbonBarRendererEventHandler;
				RibbonBarRendererEventHandler value2 = (RibbonBarRendererEventHandler)Delegate.Remove(ribbonBarRendererEventHandler2, value);
				ribbonBarRendererEventHandler = Interlocked.CompareExchange(ref _0D43, value2, ribbonBarRendererEventHandler2);
			}
			while ((object)ribbonBarRendererEventHandler != ribbonBarRendererEventHandler2);
		}
	}

	public event RibbonControlRendererEventHandler RenderRibbonControlBackground
	{
		add
		{
			RibbonControlRendererEventHandler ribbonControlRendererEventHandler = _0D44;
			RibbonControlRendererEventHandler ribbonControlRendererEventHandler2;
			do
			{
				ribbonControlRendererEventHandler2 = ribbonControlRendererEventHandler;
				RibbonControlRendererEventHandler value2 = (RibbonControlRendererEventHandler)Delegate.Combine(ribbonControlRendererEventHandler2, value);
				ribbonControlRendererEventHandler = Interlocked.CompareExchange(ref _0D44, value2, ribbonControlRendererEventHandler2);
			}
			while ((object)ribbonControlRendererEventHandler != ribbonControlRendererEventHandler2);
		}
		remove
		{
			RibbonControlRendererEventHandler ribbonControlRendererEventHandler = _0D44;
			RibbonControlRendererEventHandler ribbonControlRendererEventHandler2;
			do
			{
				ribbonControlRendererEventHandler2 = ribbonControlRendererEventHandler;
				RibbonControlRendererEventHandler value2 = (RibbonControlRendererEventHandler)Delegate.Remove(ribbonControlRendererEventHandler2, value);
				ribbonControlRendererEventHandler = Interlocked.CompareExchange(ref _0D44, value2, ribbonControlRendererEventHandler2);
			}
			while ((object)ribbonControlRendererEventHandler != ribbonControlRendererEventHandler2);
		}
	}

	public event RibbonControlRendererEventHandler RenderRibbonFormCaptionText
	{
		add
		{
			RibbonControlRendererEventHandler ribbonControlRendererEventHandler = _0D45;
			RibbonControlRendererEventHandler ribbonControlRendererEventHandler2;
			do
			{
				ribbonControlRendererEventHandler2 = ribbonControlRendererEventHandler;
				RibbonControlRendererEventHandler value2 = (RibbonControlRendererEventHandler)Delegate.Combine(ribbonControlRendererEventHandler2, value);
				ribbonControlRendererEventHandler = Interlocked.CompareExchange(ref _0D45, value2, ribbonControlRendererEventHandler2);
			}
			while ((object)ribbonControlRendererEventHandler != ribbonControlRendererEventHandler2);
		}
		remove
		{
			RibbonControlRendererEventHandler ribbonControlRendererEventHandler = _0D45;
			RibbonControlRendererEventHandler ribbonControlRendererEventHandler2;
			do
			{
				ribbonControlRendererEventHandler2 = ribbonControlRendererEventHandler;
				RibbonControlRendererEventHandler value2 = (RibbonControlRendererEventHandler)Delegate.Remove(ribbonControlRendererEventHandler2, value);
				ribbonControlRendererEventHandler = Interlocked.CompareExchange(ref _0D45, value2, ribbonControlRendererEventHandler2);
			}
			while ((object)ribbonControlRendererEventHandler != ribbonControlRendererEventHandler2);
		}
	}

	public event RibbonControlRendererEventHandler RenderQuickAccessToolbarBackground
	{
		add
		{
			RibbonControlRendererEventHandler ribbonControlRendererEventHandler = _0D46;
			RibbonControlRendererEventHandler ribbonControlRendererEventHandler2;
			do
			{
				ribbonControlRendererEventHandler2 = ribbonControlRendererEventHandler;
				RibbonControlRendererEventHandler value2 = (RibbonControlRendererEventHandler)Delegate.Combine(ribbonControlRendererEventHandler2, value);
				ribbonControlRendererEventHandler = Interlocked.CompareExchange(ref _0D46, value2, ribbonControlRendererEventHandler2);
			}
			while ((object)ribbonControlRendererEventHandler != ribbonControlRendererEventHandler2);
		}
		remove
		{
			RibbonControlRendererEventHandler ribbonControlRendererEventHandler = _0D46;
			RibbonControlRendererEventHandler ribbonControlRendererEventHandler2;
			do
			{
				ribbonControlRendererEventHandler2 = ribbonControlRendererEventHandler;
				RibbonControlRendererEventHandler value2 = (RibbonControlRendererEventHandler)Delegate.Remove(ribbonControlRendererEventHandler2, value);
				ribbonControlRendererEventHandler = Interlocked.CompareExchange(ref _0D46, value2, ribbonControlRendererEventHandler2);
			}
			while ((object)ribbonControlRendererEventHandler != ribbonControlRendererEventHandler2);
		}
	}

	public event ColorItemRendererEventHandler RenderColorItem
	{
		add
		{
			ColorItemRendererEventHandler colorItemRendererEventHandler = _0D47;
			ColorItemRendererEventHandler colorItemRendererEventHandler2;
			do
			{
				colorItemRendererEventHandler2 = colorItemRendererEventHandler;
				ColorItemRendererEventHandler value2 = (ColorItemRendererEventHandler)Delegate.Combine(colorItemRendererEventHandler2, value);
				colorItemRendererEventHandler = Interlocked.CompareExchange(ref _0D47, value2, colorItemRendererEventHandler2);
			}
			while ((object)colorItemRendererEventHandler != colorItemRendererEventHandler2);
		}
		remove
		{
			ColorItemRendererEventHandler colorItemRendererEventHandler = _0D47;
			ColorItemRendererEventHandler colorItemRendererEventHandler2;
			do
			{
				colorItemRendererEventHandler2 = colorItemRendererEventHandler;
				ColorItemRendererEventHandler value2 = (ColorItemRendererEventHandler)Delegate.Remove(colorItemRendererEventHandler2, value);
				colorItemRendererEventHandler = Interlocked.CompareExchange(ref _0D47, value2, colorItemRendererEventHandler2);
			}
			while ((object)colorItemRendererEventHandler != colorItemRendererEventHandler2);
		}
	}

	public event SystemCaptionItemRendererEventHandler RenderSystemCaptionItem
	{
		add
		{
			SystemCaptionItemRendererEventHandler systemCaptionItemRendererEventHandler = _0D48;
			SystemCaptionItemRendererEventHandler systemCaptionItemRendererEventHandler2;
			do
			{
				systemCaptionItemRendererEventHandler2 = systemCaptionItemRendererEventHandler;
				SystemCaptionItemRendererEventHandler value2 = (SystemCaptionItemRendererEventHandler)Delegate.Combine(systemCaptionItemRendererEventHandler2, value);
				systemCaptionItemRendererEventHandler = Interlocked.CompareExchange(ref _0D48, value2, systemCaptionItemRendererEventHandler2);
			}
			while ((object)systemCaptionItemRendererEventHandler != systemCaptionItemRendererEventHandler2);
		}
		remove
		{
			SystemCaptionItemRendererEventHandler systemCaptionItemRendererEventHandler = _0D48;
			SystemCaptionItemRendererEventHandler systemCaptionItemRendererEventHandler2;
			do
			{
				systemCaptionItemRendererEventHandler2 = systemCaptionItemRendererEventHandler;
				SystemCaptionItemRendererEventHandler value2 = (SystemCaptionItemRendererEventHandler)Delegate.Remove(systemCaptionItemRendererEventHandler2, value);
				systemCaptionItemRendererEventHandler = Interlocked.CompareExchange(ref _0D48, value2, systemCaptionItemRendererEventHandler2);
			}
			while ((object)systemCaptionItemRendererEventHandler != systemCaptionItemRendererEventHandler2);
		}
	}

	public event MdiSystemItemRendererEventHandler RenderMdiSystemItem
	{
		add
		{
			MdiSystemItemRendererEventHandler mdiSystemItemRendererEventHandler = _0D49;
			MdiSystemItemRendererEventHandler mdiSystemItemRendererEventHandler2;
			do
			{
				mdiSystemItemRendererEventHandler2 = mdiSystemItemRendererEventHandler;
				MdiSystemItemRendererEventHandler value2 = (MdiSystemItemRendererEventHandler)Delegate.Combine(mdiSystemItemRendererEventHandler2, value);
				mdiSystemItemRendererEventHandler = Interlocked.CompareExchange(ref _0D49, value2, mdiSystemItemRendererEventHandler2);
			}
			while ((object)mdiSystemItemRendererEventHandler != mdiSystemItemRendererEventHandler2);
		}
		remove
		{
			MdiSystemItemRendererEventHandler mdiSystemItemRendererEventHandler = _0D49;
			MdiSystemItemRendererEventHandler mdiSystemItemRendererEventHandler2;
			do
			{
				mdiSystemItemRendererEventHandler2 = mdiSystemItemRendererEventHandler;
				MdiSystemItemRendererEventHandler value2 = (MdiSystemItemRendererEventHandler)Delegate.Remove(mdiSystemItemRendererEventHandler2, value);
				mdiSystemItemRendererEventHandler = Interlocked.CompareExchange(ref _0D49, value2, mdiSystemItemRendererEventHandler2);
			}
			while ((object)mdiSystemItemRendererEventHandler != mdiSystemItemRendererEventHandler2);
		}
	}

	public event FormCaptionRendererEventHandler RenderFormCaptionBackground
	{
		add
		{
			FormCaptionRendererEventHandler formCaptionRendererEventHandler = _0D4A;
			FormCaptionRendererEventHandler formCaptionRendererEventHandler2;
			do
			{
				formCaptionRendererEventHandler2 = formCaptionRendererEventHandler;
				FormCaptionRendererEventHandler value2 = (FormCaptionRendererEventHandler)Delegate.Combine(formCaptionRendererEventHandler2, value);
				formCaptionRendererEventHandler = Interlocked.CompareExchange(ref _0D4A, value2, formCaptionRendererEventHandler2);
			}
			while ((object)formCaptionRendererEventHandler != formCaptionRendererEventHandler2);
		}
		remove
		{
			FormCaptionRendererEventHandler formCaptionRendererEventHandler = _0D4A;
			FormCaptionRendererEventHandler formCaptionRendererEventHandler2;
			do
			{
				formCaptionRendererEventHandler2 = formCaptionRendererEventHandler;
				FormCaptionRendererEventHandler value2 = (FormCaptionRendererEventHandler)Delegate.Remove(formCaptionRendererEventHandler2, value);
				formCaptionRendererEventHandler = Interlocked.CompareExchange(ref _0D4A, value2, formCaptionRendererEventHandler2);
			}
			while ((object)formCaptionRendererEventHandler != formCaptionRendererEventHandler2);
		}
	}

	public event QatOverflowItemRendererEventHandler RenderQatOverflowItem
	{
		add
		{
			QatOverflowItemRendererEventHandler qatOverflowItemRendererEventHandler = _0D4B;
			QatOverflowItemRendererEventHandler qatOverflowItemRendererEventHandler2;
			do
			{
				qatOverflowItemRendererEventHandler2 = qatOverflowItemRendererEventHandler;
				QatOverflowItemRendererEventHandler value2 = (QatOverflowItemRendererEventHandler)Delegate.Combine(qatOverflowItemRendererEventHandler2, value);
				qatOverflowItemRendererEventHandler = Interlocked.CompareExchange(ref _0D4B, value2, qatOverflowItemRendererEventHandler2);
			}
			while ((object)qatOverflowItemRendererEventHandler != qatOverflowItemRendererEventHandler2);
		}
		remove
		{
			QatOverflowItemRendererEventHandler qatOverflowItemRendererEventHandler = _0D4B;
			QatOverflowItemRendererEventHandler qatOverflowItemRendererEventHandler2;
			do
			{
				qatOverflowItemRendererEventHandler2 = qatOverflowItemRendererEventHandler;
				QatOverflowItemRendererEventHandler value2 = (QatOverflowItemRendererEventHandler)Delegate.Remove(qatOverflowItemRendererEventHandler2, value);
				qatOverflowItemRendererEventHandler = Interlocked.CompareExchange(ref _0D4B, value2, qatOverflowItemRendererEventHandler2);
			}
			while ((object)qatOverflowItemRendererEventHandler != qatOverflowItemRendererEventHandler2);
		}
	}

	public event QatCustomizeItemRendererEventHandler RenderQatCustomizeItem
	{
		add
		{
			QatCustomizeItemRendererEventHandler qatCustomizeItemRendererEventHandler = _0D4C;
			QatCustomizeItemRendererEventHandler qatCustomizeItemRendererEventHandler2;
			do
			{
				qatCustomizeItemRendererEventHandler2 = qatCustomizeItemRendererEventHandler;
				QatCustomizeItemRendererEventHandler value2 = (QatCustomizeItemRendererEventHandler)Delegate.Combine(qatCustomizeItemRendererEventHandler2, value);
				qatCustomizeItemRendererEventHandler = Interlocked.CompareExchange(ref _0D4C, value2, qatCustomizeItemRendererEventHandler2);
			}
			while ((object)qatCustomizeItemRendererEventHandler != qatCustomizeItemRendererEventHandler2);
		}
		remove
		{
			QatCustomizeItemRendererEventHandler qatCustomizeItemRendererEventHandler = _0D4C;
			QatCustomizeItemRendererEventHandler qatCustomizeItemRendererEventHandler2;
			do
			{
				qatCustomizeItemRendererEventHandler2 = qatCustomizeItemRendererEventHandler;
				QatCustomizeItemRendererEventHandler value2 = (QatCustomizeItemRendererEventHandler)Delegate.Remove(qatCustomizeItemRendererEventHandler2, value);
				qatCustomizeItemRendererEventHandler = Interlocked.CompareExchange(ref _0D4C, value2, qatCustomizeItemRendererEventHandler2);
			}
			while ((object)qatCustomizeItemRendererEventHandler != qatCustomizeItemRendererEventHandler2);
		}
	}

	public event CheckBoxItemRendererEventHandler RenderCheckBoxItem
	{
		add
		{
			CheckBoxItemRendererEventHandler checkBoxItemRendererEventHandler = _0D4D;
			CheckBoxItemRendererEventHandler checkBoxItemRendererEventHandler2;
			do
			{
				checkBoxItemRendererEventHandler2 = checkBoxItemRendererEventHandler;
				CheckBoxItemRendererEventHandler value2 = (CheckBoxItemRendererEventHandler)Delegate.Combine(checkBoxItemRendererEventHandler2, value);
				checkBoxItemRendererEventHandler = Interlocked.CompareExchange(ref _0D4D, value2, checkBoxItemRendererEventHandler2);
			}
			while ((object)checkBoxItemRendererEventHandler != checkBoxItemRendererEventHandler2);
		}
		remove
		{
			CheckBoxItemRendererEventHandler checkBoxItemRendererEventHandler = _0D4D;
			CheckBoxItemRendererEventHandler checkBoxItemRendererEventHandler2;
			do
			{
				checkBoxItemRendererEventHandler2 = checkBoxItemRendererEventHandler;
				CheckBoxItemRendererEventHandler value2 = (CheckBoxItemRendererEventHandler)Delegate.Remove(checkBoxItemRendererEventHandler2, value);
				checkBoxItemRendererEventHandler = Interlocked.CompareExchange(ref _0D4D, value2, checkBoxItemRendererEventHandler2);
			}
			while ((object)checkBoxItemRendererEventHandler != checkBoxItemRendererEventHandler2);
		}
	}

	public event ProgressBarItemRendererEventHandler RenderProgressBarItem
	{
		add
		{
			ProgressBarItemRendererEventHandler progressBarItemRendererEventHandler = ൎ;
			ProgressBarItemRendererEventHandler progressBarItemRendererEventHandler2;
			do
			{
				progressBarItemRendererEventHandler2 = progressBarItemRendererEventHandler;
				ProgressBarItemRendererEventHandler value2 = (ProgressBarItemRendererEventHandler)Delegate.Combine(progressBarItemRendererEventHandler2, value);
				progressBarItemRendererEventHandler = Interlocked.CompareExchange(ref ൎ, value2, progressBarItemRendererEventHandler2);
			}
			while ((object)progressBarItemRendererEventHandler != progressBarItemRendererEventHandler2);
		}
		remove
		{
			ProgressBarItemRendererEventHandler progressBarItemRendererEventHandler = ൎ;
			ProgressBarItemRendererEventHandler progressBarItemRendererEventHandler2;
			do
			{
				progressBarItemRendererEventHandler2 = progressBarItemRendererEventHandler;
				ProgressBarItemRendererEventHandler value2 = (ProgressBarItemRendererEventHandler)Delegate.Remove(progressBarItemRendererEventHandler2, value);
				progressBarItemRendererEventHandler = Interlocked.CompareExchange(ref ൎ, value2, progressBarItemRendererEventHandler2);
			}
			while ((object)progressBarItemRendererEventHandler != progressBarItemRendererEventHandler2);
		}
	}

	public event NavPaneRendererEventHandler RenderNavPaneButtonBackground
	{
		add
		{
			NavPaneRendererEventHandler navPaneRendererEventHandler = _0D4F;
			NavPaneRendererEventHandler navPaneRendererEventHandler2;
			do
			{
				navPaneRendererEventHandler2 = navPaneRendererEventHandler;
				NavPaneRendererEventHandler value2 = (NavPaneRendererEventHandler)Delegate.Combine(navPaneRendererEventHandler2, value);
				navPaneRendererEventHandler = Interlocked.CompareExchange(ref _0D4F, value2, navPaneRendererEventHandler2);
			}
			while ((object)navPaneRendererEventHandler != navPaneRendererEventHandler2);
		}
		remove
		{
			NavPaneRendererEventHandler navPaneRendererEventHandler = _0D4F;
			NavPaneRendererEventHandler navPaneRendererEventHandler2;
			do
			{
				navPaneRendererEventHandler2 = navPaneRendererEventHandler;
				NavPaneRendererEventHandler value2 = (NavPaneRendererEventHandler)Delegate.Remove(navPaneRendererEventHandler2, value);
				navPaneRendererEventHandler = Interlocked.CompareExchange(ref _0D4F, value2, navPaneRendererEventHandler2);
			}
			while ((object)navPaneRendererEventHandler != navPaneRendererEventHandler2);
		}
	}

	public event SliderItemRendererEventHandler RenderSliderItem
	{
		add
		{
			SliderItemRendererEventHandler sliderItemRendererEventHandler = _0D50;
			SliderItemRendererEventHandler sliderItemRendererEventHandler2;
			do
			{
				sliderItemRendererEventHandler2 = sliderItemRendererEventHandler;
				SliderItemRendererEventHandler value2 = (SliderItemRendererEventHandler)Delegate.Combine(sliderItemRendererEventHandler2, value);
				sliderItemRendererEventHandler = Interlocked.CompareExchange(ref _0D50, value2, sliderItemRendererEventHandler2);
			}
			while ((object)sliderItemRendererEventHandler != sliderItemRendererEventHandler2);
		}
		remove
		{
			SliderItemRendererEventHandler sliderItemRendererEventHandler = _0D50;
			SliderItemRendererEventHandler sliderItemRendererEventHandler2;
			do
			{
				sliderItemRendererEventHandler2 = sliderItemRendererEventHandler;
				SliderItemRendererEventHandler value2 = (SliderItemRendererEventHandler)Delegate.Remove(sliderItemRendererEventHandler2, value);
				sliderItemRendererEventHandler = Interlocked.CompareExchange(ref _0D50, value2, sliderItemRendererEventHandler2);
			}
			while ((object)sliderItemRendererEventHandler != sliderItemRendererEventHandler2);
		}
	}

	public event RangeSliderItemRendererEventHandler RenderRangeSliderItem
	{
		add
		{
			RangeSliderItemRendererEventHandler rangeSliderItemRendererEventHandler = _0D51;
			RangeSliderItemRendererEventHandler rangeSliderItemRendererEventHandler2;
			do
			{
				rangeSliderItemRendererEventHandler2 = rangeSliderItemRendererEventHandler;
				RangeSliderItemRendererEventHandler value2 = (RangeSliderItemRendererEventHandler)Delegate.Combine(rangeSliderItemRendererEventHandler2, value);
				rangeSliderItemRendererEventHandler = Interlocked.CompareExchange(ref _0D51, value2, rangeSliderItemRendererEventHandler2);
			}
			while ((object)rangeSliderItemRendererEventHandler != rangeSliderItemRendererEventHandler2);
		}
		remove
		{
			RangeSliderItemRendererEventHandler rangeSliderItemRendererEventHandler = _0D51;
			RangeSliderItemRendererEventHandler rangeSliderItemRendererEventHandler2;
			do
			{
				rangeSliderItemRendererEventHandler2 = rangeSliderItemRendererEventHandler;
				RangeSliderItemRendererEventHandler value2 = (RangeSliderItemRendererEventHandler)Delegate.Remove(rangeSliderItemRendererEventHandler2, value);
				rangeSliderItemRendererEventHandler = Interlocked.CompareExchange(ref _0D51, value2, rangeSliderItemRendererEventHandler2);
			}
			while ((object)rangeSliderItemRendererEventHandler != rangeSliderItemRendererEventHandler2);
		}
	}

	public event SideBarRendererEventHandler RenderSideBar
	{
		add
		{
			SideBarRendererEventHandler sideBarRendererEventHandler = _0D52;
			SideBarRendererEventHandler sideBarRendererEventHandler2;
			do
			{
				sideBarRendererEventHandler2 = sideBarRendererEventHandler;
				SideBarRendererEventHandler value2 = (SideBarRendererEventHandler)Delegate.Combine(sideBarRendererEventHandler2, value);
				sideBarRendererEventHandler = Interlocked.CompareExchange(ref _0D52, value2, sideBarRendererEventHandler2);
			}
			while ((object)sideBarRendererEventHandler != sideBarRendererEventHandler2);
		}
		remove
		{
			SideBarRendererEventHandler sideBarRendererEventHandler = _0D52;
			SideBarRendererEventHandler sideBarRendererEventHandler2;
			do
			{
				sideBarRendererEventHandler2 = sideBarRendererEventHandler;
				SideBarRendererEventHandler value2 = (SideBarRendererEventHandler)Delegate.Remove(sideBarRendererEventHandler2, value);
				sideBarRendererEventHandler = Interlocked.CompareExchange(ref _0D52, value2, sideBarRendererEventHandler2);
			}
			while ((object)sideBarRendererEventHandler != sideBarRendererEventHandler2);
		}
	}

	public event SideBarPanelItemRendererEventHandler RenderSideBarPanelItem
	{
		add
		{
			SideBarPanelItemRendererEventHandler sideBarPanelItemRendererEventHandler = _0D53;
			SideBarPanelItemRendererEventHandler sideBarPanelItemRendererEventHandler2;
			do
			{
				sideBarPanelItemRendererEventHandler2 = sideBarPanelItemRendererEventHandler;
				SideBarPanelItemRendererEventHandler value2 = (SideBarPanelItemRendererEventHandler)Delegate.Combine(sideBarPanelItemRendererEventHandler2, value);
				sideBarPanelItemRendererEventHandler = Interlocked.CompareExchange(ref _0D53, value2, sideBarPanelItemRendererEventHandler2);
			}
			while ((object)sideBarPanelItemRendererEventHandler != sideBarPanelItemRendererEventHandler2);
		}
		remove
		{
			SideBarPanelItemRendererEventHandler sideBarPanelItemRendererEventHandler = _0D53;
			SideBarPanelItemRendererEventHandler sideBarPanelItemRendererEventHandler2;
			do
			{
				sideBarPanelItemRendererEventHandler2 = sideBarPanelItemRendererEventHandler;
				SideBarPanelItemRendererEventHandler value2 = (SideBarPanelItemRendererEventHandler)Delegate.Remove(sideBarPanelItemRendererEventHandler2, value);
				sideBarPanelItemRendererEventHandler = Interlocked.CompareExchange(ref _0D53, value2, sideBarPanelItemRendererEventHandler2);
			}
			while ((object)sideBarPanelItemRendererEventHandler != sideBarPanelItemRendererEventHandler2);
		}
	}

	public event ButtonItemRendererEventHandler RenderCrumbBarItemView
	{
		add
		{
			ButtonItemRendererEventHandler buttonItemRendererEventHandler = ൔ;
			ButtonItemRendererEventHandler buttonItemRendererEventHandler2;
			do
			{
				buttonItemRendererEventHandler2 = buttonItemRendererEventHandler;
				ButtonItemRendererEventHandler value2 = (ButtonItemRendererEventHandler)Delegate.Combine(buttonItemRendererEventHandler2, value);
				buttonItemRendererEventHandler = Interlocked.CompareExchange(ref ൔ, value2, buttonItemRendererEventHandler2);
			}
			while ((object)buttonItemRendererEventHandler != buttonItemRendererEventHandler2);
		}
		remove
		{
			ButtonItemRendererEventHandler buttonItemRendererEventHandler = ൔ;
			ButtonItemRendererEventHandler buttonItemRendererEventHandler2;
			do
			{
				buttonItemRendererEventHandler2 = buttonItemRendererEventHandler;
				ButtonItemRendererEventHandler value2 = (ButtonItemRendererEventHandler)Delegate.Remove(buttonItemRendererEventHandler2, value);
				buttonItemRendererEventHandler = Interlocked.CompareExchange(ref ൔ, value2, buttonItemRendererEventHandler2);
			}
			while ((object)buttonItemRendererEventHandler != buttonItemRendererEventHandler2);
		}
	}

	public event ButtonItemRendererEventHandler RenderCrumbBarOverflowItem
	{
		add
		{
			ButtonItemRendererEventHandler buttonItemRendererEventHandler = ൕ;
			ButtonItemRendererEventHandler buttonItemRendererEventHandler2;
			do
			{
				buttonItemRendererEventHandler2 = buttonItemRendererEventHandler;
				ButtonItemRendererEventHandler value2 = (ButtonItemRendererEventHandler)Delegate.Combine(buttonItemRendererEventHandler2, value);
				buttonItemRendererEventHandler = Interlocked.CompareExchange(ref ൕ, value2, buttonItemRendererEventHandler2);
			}
			while ((object)buttonItemRendererEventHandler != buttonItemRendererEventHandler2);
		}
		remove
		{
			ButtonItemRendererEventHandler buttonItemRendererEventHandler = ൕ;
			ButtonItemRendererEventHandler buttonItemRendererEventHandler2;
			do
			{
				buttonItemRendererEventHandler2 = buttonItemRendererEventHandler;
				ButtonItemRendererEventHandler value2 = (ButtonItemRendererEventHandler)Delegate.Remove(buttonItemRendererEventHandler2, value);
				buttonItemRendererEventHandler = Interlocked.CompareExchange(ref ൕ, value2, buttonItemRendererEventHandler2);
			}
			while ((object)buttonItemRendererEventHandler != buttonItemRendererEventHandler2);
		}
	}

	public event SwitchButtonRendererEventHandler RenderSwitchButton
	{
		add
		{
			SwitchButtonRendererEventHandler switchButtonRendererEventHandler = ൖ;
			SwitchButtonRendererEventHandler switchButtonRendererEventHandler2;
			do
			{
				switchButtonRendererEventHandler2 = switchButtonRendererEventHandler;
				SwitchButtonRendererEventHandler value2 = (SwitchButtonRendererEventHandler)Delegate.Combine(switchButtonRendererEventHandler2, value);
				switchButtonRendererEventHandler = Interlocked.CompareExchange(ref ൖ, value2, switchButtonRendererEventHandler2);
			}
			while ((object)switchButtonRendererEventHandler != switchButtonRendererEventHandler2);
		}
		remove
		{
			SwitchButtonRendererEventHandler switchButtonRendererEventHandler = ൖ;
			SwitchButtonRendererEventHandler switchButtonRendererEventHandler2;
			do
			{
				switchButtonRendererEventHandler2 = switchButtonRendererEventHandler;
				SwitchButtonRendererEventHandler value2 = (SwitchButtonRendererEventHandler)Delegate.Remove(switchButtonRendererEventHandler2, value);
				switchButtonRendererEventHandler = Interlocked.CompareExchange(ref ൖ, value2, switchButtonRendererEventHandler2);
			}
			while ((object)switchButtonRendererEventHandler != switchButtonRendererEventHandler2);
		}
	}

	public event StepItemRendererEventHandler RenderStepItem
	{
		add
		{
			StepItemRendererEventHandler stepItemRendererEventHandler = _0D57;
			StepItemRendererEventHandler stepItemRendererEventHandler2;
			do
			{
				stepItemRendererEventHandler2 = stepItemRendererEventHandler;
				StepItemRendererEventHandler value2 = (StepItemRendererEventHandler)Delegate.Combine(stepItemRendererEventHandler2, value);
				stepItemRendererEventHandler = Interlocked.CompareExchange(ref _0D57, value2, stepItemRendererEventHandler2);
			}
			while ((object)stepItemRendererEventHandler != stepItemRendererEventHandler2);
		}
		remove
		{
			StepItemRendererEventHandler stepItemRendererEventHandler = _0D57;
			StepItemRendererEventHandler stepItemRendererEventHandler2;
			do
			{
				stepItemRendererEventHandler2 = stepItemRendererEventHandler;
				StepItemRendererEventHandler value2 = (StepItemRendererEventHandler)Delegate.Remove(stepItemRendererEventHandler2, value);
				stepItemRendererEventHandler = Interlocked.CompareExchange(ref _0D57, value2, stepItemRendererEventHandler2);
			}
			while ((object)stepItemRendererEventHandler != stepItemRendererEventHandler2);
		}
	}

	public event ListBoxItemRendererEventHandler RenderListBoxItem
	{
		add
		{
			ListBoxItemRendererEventHandler listBoxItemRendererEventHandler = _0D58;
			ListBoxItemRendererEventHandler listBoxItemRendererEventHandler2;
			do
			{
				listBoxItemRendererEventHandler2 = listBoxItemRendererEventHandler;
				ListBoxItemRendererEventHandler value2 = (ListBoxItemRendererEventHandler)Delegate.Combine(listBoxItemRendererEventHandler2, value);
				listBoxItemRendererEventHandler = Interlocked.CompareExchange(ref _0D58, value2, listBoxItemRendererEventHandler2);
			}
			while ((object)listBoxItemRendererEventHandler != listBoxItemRendererEventHandler2);
		}
		remove
		{
			ListBoxItemRendererEventHandler listBoxItemRendererEventHandler = _0D58;
			ListBoxItemRendererEventHandler listBoxItemRendererEventHandler2;
			do
			{
				listBoxItemRendererEventHandler2 = listBoxItemRendererEventHandler;
				ListBoxItemRendererEventHandler value2 = (ListBoxItemRendererEventHandler)Delegate.Remove(listBoxItemRendererEventHandler2, value);
				listBoxItemRendererEventHandler = Interlocked.CompareExchange(ref _0D58, value2, listBoxItemRendererEventHandler2);
			}
			while ((object)listBoxItemRendererEventHandler != listBoxItemRendererEventHandler2);
		}
	}

	public event SideNavItemRendererEventHandler RenderSideNavItem
	{
		add
		{
			SideNavItemRendererEventHandler sideNavItemRendererEventHandler = _0D59;
			SideNavItemRendererEventHandler sideNavItemRendererEventHandler2;
			do
			{
				sideNavItemRendererEventHandler2 = sideNavItemRendererEventHandler;
				SideNavItemRendererEventHandler value2 = (SideNavItemRendererEventHandler)Delegate.Combine(sideNavItemRendererEventHandler2, value);
				sideNavItemRendererEventHandler = Interlocked.CompareExchange(ref _0D59, value2, sideNavItemRendererEventHandler2);
			}
			while ((object)sideNavItemRendererEventHandler != sideNavItemRendererEventHandler2);
		}
		remove
		{
			SideNavItemRendererEventHandler sideNavItemRendererEventHandler = _0D59;
			SideNavItemRendererEventHandler sideNavItemRendererEventHandler2;
			do
			{
				sideNavItemRendererEventHandler2 = sideNavItemRendererEventHandler;
				SideNavItemRendererEventHandler value2 = (SideNavItemRendererEventHandler)Delegate.Remove(sideNavItemRendererEventHandler2, value);
				sideNavItemRendererEventHandler = Interlocked.CompareExchange(ref _0D59, value2, sideNavItemRendererEventHandler2);
			}
			while ((object)sideNavItemRendererEventHandler != sideNavItemRendererEventHandler2);
		}
	}

	public event TabFormStripRendererEventHandler RenderTabFormStrip
	{
		add
		{
			TabFormStripRendererEventHandler tabFormStripRendererEventHandler = _0D5A;
			TabFormStripRendererEventHandler tabFormStripRendererEventHandler2;
			do
			{
				tabFormStripRendererEventHandler2 = tabFormStripRendererEventHandler;
				TabFormStripRendererEventHandler value2 = (TabFormStripRendererEventHandler)Delegate.Combine(tabFormStripRendererEventHandler2, value);
				tabFormStripRendererEventHandler = Interlocked.CompareExchange(ref _0D5A, value2, tabFormStripRendererEventHandler2);
			}
			while ((object)tabFormStripRendererEventHandler != tabFormStripRendererEventHandler2);
		}
		remove
		{
			TabFormStripRendererEventHandler tabFormStripRendererEventHandler = _0D5A;
			TabFormStripRendererEventHandler tabFormStripRendererEventHandler2;
			do
			{
				tabFormStripRendererEventHandler2 = tabFormStripRendererEventHandler;
				TabFormStripRendererEventHandler value2 = (TabFormStripRendererEventHandler)Delegate.Remove(tabFormStripRendererEventHandler2, value);
				tabFormStripRendererEventHandler = Interlocked.CompareExchange(ref _0D5A, value2, tabFormStripRendererEventHandler2);
			}
			while ((object)tabFormStripRendererEventHandler != tabFormStripRendererEventHandler2);
		}
	}

	public event ButtonItemRendererEventHandler RenderTabFormItem
	{
		add
		{
			ButtonItemRendererEventHandler buttonItemRendererEventHandler = _0D5B;
			ButtonItemRendererEventHandler buttonItemRendererEventHandler2;
			do
			{
				buttonItemRendererEventHandler2 = buttonItemRendererEventHandler;
				ButtonItemRendererEventHandler value2 = (ButtonItemRendererEventHandler)Delegate.Combine(buttonItemRendererEventHandler2, value);
				buttonItemRendererEventHandler = Interlocked.CompareExchange(ref _0D5B, value2, buttonItemRendererEventHandler2);
			}
			while ((object)buttonItemRendererEventHandler != buttonItemRendererEventHandler2);
		}
		remove
		{
			ButtonItemRendererEventHandler buttonItemRendererEventHandler = _0D5B;
			ButtonItemRendererEventHandler buttonItemRendererEventHandler2;
			do
			{
				buttonItemRendererEventHandler2 = buttonItemRendererEventHandler;
				ButtonItemRendererEventHandler value2 = (ButtonItemRendererEventHandler)Delegate.Remove(buttonItemRendererEventHandler2, value);
				buttonItemRendererEventHandler = Interlocked.CompareExchange(ref _0D5B, value2, buttonItemRendererEventHandler2);
			}
			while ((object)buttonItemRendererEventHandler != buttonItemRendererEventHandler2);
		}
	}

	public event ButtonItemRendererEventHandler RenderNewTabFormItem
	{
		add
		{
			ButtonItemRendererEventHandler buttonItemRendererEventHandler = _0D5C;
			ButtonItemRendererEventHandler buttonItemRendererEventHandler2;
			do
			{
				buttonItemRendererEventHandler2 = buttonItemRendererEventHandler;
				ButtonItemRendererEventHandler value2 = (ButtonItemRendererEventHandler)Delegate.Combine(buttonItemRendererEventHandler2, value);
				buttonItemRendererEventHandler = Interlocked.CompareExchange(ref _0D5C, value2, buttonItemRendererEventHandler2);
			}
			while ((object)buttonItemRendererEventHandler != buttonItemRendererEventHandler2);
		}
		remove
		{
			ButtonItemRendererEventHandler buttonItemRendererEventHandler = _0D5C;
			ButtonItemRendererEventHandler buttonItemRendererEventHandler2;
			do
			{
				buttonItemRendererEventHandler2 = buttonItemRendererEventHandler;
				ButtonItemRendererEventHandler value2 = (ButtonItemRendererEventHandler)Delegate.Remove(buttonItemRendererEventHandler2, value);
				buttonItemRendererEventHandler = Interlocked.CompareExchange(ref _0D5C, value2, buttonItemRendererEventHandler2);
			}
			while ((object)buttonItemRendererEventHandler != buttonItemRendererEventHandler2);
		}
	}

	public event TabFormRendererEventHandler RenderTabParentForm
	{
		add
		{
			TabFormRendererEventHandler tabFormRendererEventHandler = _0D5D;
			TabFormRendererEventHandler tabFormRendererEventHandler2;
			do
			{
				tabFormRendererEventHandler2 = tabFormRendererEventHandler;
				TabFormRendererEventHandler value2 = (TabFormRendererEventHandler)Delegate.Combine(tabFormRendererEventHandler2, value);
				tabFormRendererEventHandler = Interlocked.CompareExchange(ref _0D5D, value2, tabFormRendererEventHandler2);
			}
			while ((object)tabFormRendererEventHandler != tabFormRendererEventHandler2);
		}
		remove
		{
			TabFormRendererEventHandler tabFormRendererEventHandler = _0D5D;
			TabFormRendererEventHandler tabFormRendererEventHandler2;
			do
			{
				tabFormRendererEventHandler2 = tabFormRendererEventHandler;
				TabFormRendererEventHandler value2 = (TabFormRendererEventHandler)Delegate.Remove(tabFormRendererEventHandler2, value);
				tabFormRendererEventHandler = Interlocked.CompareExchange(ref _0D5D, value2, tabFormRendererEventHandler2);
			}
			while ((object)tabFormRendererEventHandler != tabFormRendererEventHandler2);
		}
	}

	protected virtual void OnRenderKeyTips(KeyTipsRendererEventArgs e)
	{
		if (_0D3B != null)
		{
			_0D3B(this, e);
		}
	}

	public virtual void DrawKeyTips(KeyTipsRendererEventArgs e)
	{
		OnRenderKeyTips(e);
	}

	protected virtual void OnRenderRibbonTabGroup(RibbonTabGroupRendererEventArgs e)
	{
		if (_0D3C != null)
		{
			_0D3C(this, e);
		}
	}

	public virtual void DrawRibbonTabGroup(RibbonTabGroupRendererEventArgs e)
	{
		OnRenderRibbonTabGroup(e);
	}

	protected virtual void OnRenderItemContainer(ItemContainerRendererEventArgs e)
	{
		if (ഽ != null)
		{
			ഽ(this, e);
		}
	}

	public virtual void DrawItemContainerSeparator(ItemContainerSeparatorRendererEventArgs e)
	{
		OnRenderItemContainerSeparator(e);
	}

	protected virtual void OnRenderItemContainerSeparator(ItemContainerSeparatorRendererEventArgs e)
	{
		if (_0D3E != null)
		{
			_0D3E(this, e);
		}
	}

	public virtual void DrawItemContainer(ItemContainerRendererEventArgs e)
	{
		OnRenderItemContainer(e);
	}

	protected virtual void OnRenderButtonItem(ButtonItemRendererEventArgs e)
	{
		if (_0D3F != null)
		{
			_0D3F(this, e);
		}
	}

	public virtual void DrawButtonItem(ButtonItemRendererEventArgs e)
	{
		OnRenderButtonItem(e);
	}

	protected virtual void OnRenderRibbonTabItem(RibbonTabItemRendererEventArgs e)
	{
		if (_0D40 != null)
		{
			_0D40(this, e);
		}
	}

	public virtual void DrawRibbonTabItem(RibbonTabItemRendererEventArgs e)
	{
		OnRenderRibbonTabItem(e);
	}

	protected virtual void OnRenderToolbarBackground(ToolbarRendererEventArgs e)
	{
		if (_0D41 != null)
		{
			_0D41(this, e);
		}
	}

	public virtual void DrawToolbarBackground(ToolbarRendererEventArgs e)
	{
		OnRenderToolbarBackground(e);
	}

	protected virtual void OnRenderPopupToolbarBackground(ToolbarRendererEventArgs e)
	{
		if (_0D42 != null)
		{
			_0D42(this, e);
		}
	}

	public virtual void DrawPopupToolbarBackground(ToolbarRendererEventArgs e)
	{
		OnRenderPopupToolbarBackground(e);
	}

	protected virtual void OnRenderRibbonDialogLauncher(RibbonBarRendererEventArgs e)
	{
		if (_0D43 != null)
		{
			_0D43(this, e);
		}
	}

	public virtual void DrawRibbonDialogLauncher(RibbonBarRendererEventArgs e)
	{
		OnRenderRibbonDialogLauncher(e);
	}

	protected virtual void OnRenderColorItem(ColorItemRendererEventArgs e)
	{
		if (_0D47 != null)
		{
			_0D47(this, e);
		}
	}

	public virtual void DrawColorItem(ColorItemRendererEventArgs e)
	{
		OnRenderColorItem(e);
	}

	protected virtual void OnRenderRibbonControlBackground(RibbonControlRendererEventArgs e)
	{
		if (_0D44 != null)
		{
			_0D44(this, e);
		}
	}

	public virtual void DrawRibbonControlBackground(RibbonControlRendererEventArgs e)
	{
		OnRenderRibbonControlBackground(e);
	}

	protected virtual void OnRenderSystemCaptionItem(SystemCaptionItemRendererEventArgs e)
	{
		if (_0D48 != null)
		{
			_0D48(this, e);
		}
	}

	public virtual void DrawSystemCaptionItem(SystemCaptionItemRendererEventArgs e)
	{
		OnRenderSystemCaptionItem(e);
	}

	protected virtual void OnRenderRibbonFormCaptionText(RibbonControlRendererEventArgs e)
	{
		if (_0D45 != null)
		{
			_0D45(this, e);
		}
	}

	public virtual void DrawRibbonFormCaptionText(RibbonControlRendererEventArgs e)
	{
		OnRenderRibbonFormCaptionText(e);
	}

	protected virtual void OnRenderQuickAccessToolbarBackground(RibbonControlRendererEventArgs e)
	{
		if (_0D46 != null)
		{
			_0D46(this, e);
		}
	}

	public virtual void DrawQuickAccessToolbarBackground(RibbonControlRendererEventArgs e)
	{
		OnRenderQuickAccessToolbarBackground(e);
	}

	protected virtual void OnRenderMdiSystemItem(MdiSystemItemRendererEventArgs e)
	{
		if (_0D49 != null)
		{
			_0D49(this, e);
		}
	}

	public virtual void DrawMdiSystemItem(MdiSystemItemRendererEventArgs e)
	{
		OnRenderMdiSystemItem(e);
	}

	protected virtual void OnRenderFormCaptionBackground(FormCaptionRendererEventArgs e)
	{
		if (_0D4A != null)
		{
			_0D4A(this, e);
		}
	}

	public virtual void DrawFormCaptionBackground(FormCaptionRendererEventArgs e)
	{
		OnRenderFormCaptionBackground(e);
	}

	protected virtual void OnRenderQatOverflowItem(QatOverflowItemRendererEventArgs e)
	{
		if (_0D4B != null)
		{
			_0D4B(this, e);
		}
	}

	public virtual void DrawQatOverflowItem(QatOverflowItemRendererEventArgs e)
	{
		OnRenderQatOverflowItem(e);
	}

	protected virtual void OnRenderQatCustomizeItem(QatCustomizeItemRendererEventArgs e)
	{
		if (_0D4C != null)
		{
			_0D4C(this, e);
		}
	}

	public virtual void DrawQatCustomizeItem(QatCustomizeItemRendererEventArgs e)
	{
		OnRenderQatCustomizeItem(e);
	}

	protected virtual void OnRenderCheckBoxItem(CheckBoxItemRenderEventArgs e)
	{
		if (_0D4D != null)
		{
			_0D4D(this, e);
		}
	}

	public virtual void DrawCheckBoxItem(CheckBoxItemRenderEventArgs e)
	{
		OnRenderCheckBoxItem(e);
	}

	protected virtual void OnRenderProgressBarItem(ProgressBarItemRenderEventArgs e)
	{
		if (ൎ != null)
		{
			ൎ(this, e);
		}
	}

	public virtual void DrawProgressBarItem(ProgressBarItemRenderEventArgs e)
	{
		OnRenderProgressBarItem(e);
	}

	protected virtual void OnRenderNavPaneButtonBackground(NavPaneRenderEventArgs e)
	{
		if (_0D4F != null)
		{
			_0D4F(this, e);
		}
	}

	public virtual void DrawNavPaneButtonBackground(NavPaneRenderEventArgs e)
	{
		OnRenderNavPaneButtonBackground(e);
	}

	protected virtual void OnRenderSliderItem(SliderItemRendererEventArgs e)
	{
		if (_0D50 != null)
		{
			_0D50(this, e);
		}
	}

	public virtual void DrawSliderItem(SliderItemRendererEventArgs e)
	{
		OnRenderSliderItem(e);
	}

	protected virtual void OnRenderSideBar(SideBarRendererEventArgs e)
	{
		if (_0D52 != null)
		{
			_0D52(this, e);
		}
	}

	public virtual void DrawSideBar(SideBarRendererEventArgs e)
	{
		OnRenderSideBar(e);
	}

	protected virtual void OnRenderSideBarPanelItem(SideBarPanelItemRendererEventArgs e)
	{
		if (_0D53 != null)
		{
			_0D53(this, e);
		}
	}

	public virtual void DrawSideBarPanelItem(SideBarPanelItemRendererEventArgs e)
	{
		OnRenderSideBarPanelItem(e);
	}

	protected virtual void OnRenderCrumbBarItemView(ButtonItemRendererEventArgs e)
	{
		if (ൔ != null)
		{
			ൔ(this, e);
		}
	}

	public virtual void DrawCrumbBarItemView(ButtonItemRendererEventArgs e)
	{
		OnRenderCrumbBarItemView(e);
	}

	protected virtual void OnRenderCrumbBarOverflowItem(ButtonItemRendererEventArgs e)
	{
		if (ൕ != null)
		{
			ൕ(this, e);
		}
	}

	public virtual void DrawCrumbBarOverflowItem(ButtonItemRendererEventArgs e)
	{
		OnRenderCrumbBarOverflowItem(e);
	}

	protected virtual void OnRenderSwitchButton(SwitchButtonRenderEventArgs e)
	{
		if (ൖ != null)
		{
			ൖ(this, e);
		}
	}

	public virtual void DrawSwitchButton(SwitchButtonRenderEventArgs e)
	{
		OnRenderSwitchButton(e);
	}

	protected virtual void OnRenderRangeSliderItem(RangeSliderItemRendererEventArgs e)
	{
		if (_0D51 != null)
		{
			_0D51(this, e);
		}
	}

	public virtual void DrawRangeSliderItem(RangeSliderItemRendererEventArgs e)
	{
		OnRenderRangeSliderItem(e);
	}

	protected virtual void OnRenderStepItem(StepItemRendererEventArgs e)
	{
		if (_0D57 != null)
		{
			_0D57(this, e);
		}
	}

	public virtual void DrawStepItem(StepItemRendererEventArgs e)
	{
		OnRenderStepItem(e);
	}

	protected virtual void OnRenderListBoxItem(ListBoxItemRendererEventArgs e)
	{
		if (_0D58 != null)
		{
			_0D58(this, e);
		}
	}

	public virtual void DrawListBoxItem(ListBoxItemRendererEventArgs e)
	{
		OnRenderListBoxItem(e);
	}

	protected virtual void OnRenderSideNavItem(SideNavItemRendererEventArgs e)
	{
		if (_0D59 != null)
		{
			_0D59(this, e);
		}
	}

	public virtual void DrawSideNavItem(SideNavItemRendererEventArgs e)
	{
		OnRenderSideNavItem(e);
	}

	protected virtual void OnRenderTabFormStrip(TabFormStripPainterArgs e)
	{
		if (_0D5A != null)
		{
			_0D5A(this, e);
		}
	}

	public virtual void DrawTabFormStrip(TabFormStripPainterArgs e)
	{
		OnRenderTabFormStrip(e);
	}

	protected virtual void OnRenderTabFormItem(ButtonItemRendererEventArgs e)
	{
		if (_0D5B != null)
		{
			_0D5B(this, e);
		}
	}

	public virtual void DrawTabFormItem(ButtonItemRendererEventArgs e)
	{
		OnRenderTabFormItem(e);
	}

	protected virtual void OnRenderNewTabFormItem(ButtonItemRendererEventArgs e)
	{
		if (_0D5C != null)
		{
			_0D5C(this, e);
		}
	}

	public virtual void DrawNewTabFormItem(ButtonItemRendererEventArgs e)
	{
		OnRenderNewTabFormItem(e);
	}

	protected virtual void OnRenderTabParentForm(TabFormPainterArgs e)
	{
		if (_0D5D != null)
		{
			_0D5D(this, e);
		}
	}

	public virtual void DrawTabParentForm(TabFormPainterArgs e)
	{
		OnRenderTabParentForm(e);
	}
}
