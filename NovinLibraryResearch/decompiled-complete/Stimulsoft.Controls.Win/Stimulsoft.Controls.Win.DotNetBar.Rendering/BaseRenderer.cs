namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

public abstract class BaseRenderer
{
	public event KeyTipsRendererEventHandler RenderKeyTips;

	public event RibbonTabGroupRendererEventHandler RenderRibbonTabGroup;

	public event ItemContainerRendererEventHandler RenderItemContainer;

	public event ItemContainerSeparatorRendererEventHandler RenderItemContainerSeparator;

	public event ButtonItemRendererEventHandler RenderButtonItem;

	public event RibbonTabItemRendererEventHandler RenderRibbonTabItem;

	public event ToolbarRendererEventHandler RenderToolbarBackground;

	public event ToolbarRendererEventHandler RenderPopupToolbarBackground;

	public event RibbonBarRendererEventHandler RenderRibbonDialogLauncher;

	public event RibbonControlRendererEventHandler RenderRibbonControlBackground;

	public event RibbonControlRendererEventHandler RenderRibbonFormCaptionText;

	public event RibbonControlRendererEventHandler RenderQuickAccessToolbarBackground;

	public event ColorItemRendererEventHandler RenderColorItem;

	public event SystemCaptionItemRendererEventHandler RenderSystemCaptionItem;

	public event MdiSystemItemRendererEventHandler RenderMdiSystemItem;

	public event FormCaptionRendererEventHandler RenderFormCaptionBackground;

	public event QatOverflowItemRendererEventHandler RenderQatOverflowItem;

	public event QatCustomizeItemRendererEventHandler RenderQatCustomizeItem;

	public event CheckBoxItemRendererEventHandler RenderCheckBoxItem;

	public event ProgressBarItemRendererEventHandler RenderProgressBarItem;

	public event NavPaneRendererEventHandler RenderNavPaneButtonBackground;

	public event SliderItemRendererEventHandler RenderSliderItem;

	public event SideBarRendererEventHandler RenderSideBar;

	public event SideBarPanelItemRendererEventHandler RenderSideBarPanelItem;

	public event ButtonItemRendererEventHandler RenderCrumbBarItemView;

	public event ButtonItemRendererEventHandler RenderCrumbBarOverflowItem;

	public event SwitchButtonRendererEventHandler RenderSwitchButton;

	protected virtual void OnRenderKeyTips(KeyTipsRendererEventArgs e)
	{
		if (this.RenderKeyTips != null)
		{
			this.RenderKeyTips(this, e);
		}
	}

	public virtual void DrawKeyTips(KeyTipsRendererEventArgs e)
	{
		OnRenderKeyTips(e);
	}

	protected virtual void OnRenderRibbonTabGroup(RibbonTabGroupRendererEventArgs e)
	{
		if (this.RenderRibbonTabGroup != null)
		{
			this.RenderRibbonTabGroup(this, e);
		}
	}

	public virtual void DrawRibbonTabGroup(RibbonTabGroupRendererEventArgs e)
	{
		OnRenderRibbonTabGroup(e);
	}

	protected virtual void OnRenderItemContainer(ItemContainerRendererEventArgs e)
	{
		if (this.RenderItemContainer != null)
		{
			this.RenderItemContainer(this, e);
		}
	}

	public virtual void DrawItemContainerSeparator(ItemContainerSeparatorRendererEventArgs e)
	{
		OnRenderItemContainerSeparator(e);
	}

	protected virtual void OnRenderItemContainerSeparator(ItemContainerSeparatorRendererEventArgs e)
	{
		if (this.RenderItemContainerSeparator != null)
		{
			this.RenderItemContainerSeparator(this, e);
		}
	}

	public virtual void DrawItemContainer(ItemContainerRendererEventArgs e)
	{
		OnRenderItemContainer(e);
	}

	protected virtual void OnRenderButtonItem(ButtonItemRendererEventArgs e)
	{
		if (this.RenderButtonItem != null)
		{
			this.RenderButtonItem(this, e);
		}
	}

	public virtual void DrawButtonItem(ButtonItemRendererEventArgs e)
	{
		OnRenderButtonItem(e);
	}

	protected virtual void OnRenderRibbonTabItem(RibbonTabItemRendererEventArgs e)
	{
		if (this.RenderRibbonTabItem != null)
		{
			this.RenderRibbonTabItem(this, e);
		}
	}

	public virtual void DrawRibbonTabItem(RibbonTabItemRendererEventArgs e)
	{
		OnRenderRibbonTabItem(e);
	}

	protected virtual void OnRenderToolbarBackground(ToolbarRendererEventArgs e)
	{
		if (this.RenderToolbarBackground != null)
		{
			this.RenderToolbarBackground(this, e);
		}
	}

	public virtual void DrawToolbarBackground(ToolbarRendererEventArgs e)
	{
		OnRenderToolbarBackground(e);
	}

	protected virtual void OnRenderPopupToolbarBackground(ToolbarRendererEventArgs e)
	{
		if (this.RenderPopupToolbarBackground != null)
		{
			this.RenderPopupToolbarBackground(this, e);
		}
	}

	public virtual void DrawPopupToolbarBackground(ToolbarRendererEventArgs e)
	{
		OnRenderPopupToolbarBackground(e);
	}

	protected virtual void OnRenderRibbonDialogLauncher(RibbonBarRendererEventArgs e)
	{
		if (this.RenderRibbonDialogLauncher != null)
		{
			this.RenderRibbonDialogLauncher(this, e);
		}
	}

	public virtual void DrawRibbonDialogLauncher(RibbonBarRendererEventArgs e)
	{
		OnRenderRibbonDialogLauncher(e);
	}

	protected virtual void OnRenderColorItem(ColorItemRendererEventArgs e)
	{
		if (this.RenderColorItem != null)
		{
			this.RenderColorItem(this, e);
		}
	}

	public virtual void DrawColorItem(ColorItemRendererEventArgs e)
	{
		OnRenderColorItem(e);
	}

	protected virtual void OnRenderRibbonControlBackground(RibbonControlRendererEventArgs e)
	{
		if (this.RenderRibbonControlBackground != null)
		{
			this.RenderRibbonControlBackground(this, e);
		}
	}

	public virtual void DrawRibbonControlBackground(RibbonControlRendererEventArgs e)
	{
		OnRenderRibbonControlBackground(e);
	}

	protected virtual void OnRenderSystemCaptionItem(SystemCaptionItemRendererEventArgs e)
	{
		if (this.RenderSystemCaptionItem != null)
		{
			this.RenderSystemCaptionItem(this, e);
		}
	}

	public virtual void DrawSystemCaptionItem(SystemCaptionItemRendererEventArgs e)
	{
		OnRenderSystemCaptionItem(e);
	}

	protected virtual void OnRenderRibbonFormCaptionText(RibbonControlRendererEventArgs e)
	{
		if (this.RenderRibbonFormCaptionText != null)
		{
			this.RenderRibbonFormCaptionText(this, e);
		}
	}

	public virtual void DrawRibbonFormCaptionText(RibbonControlRendererEventArgs e)
	{
		OnRenderRibbonFormCaptionText(e);
	}

	protected virtual void OnRenderQuickAccessToolbarBackground(RibbonControlRendererEventArgs e)
	{
		if (this.RenderQuickAccessToolbarBackground != null)
		{
			this.RenderQuickAccessToolbarBackground(this, e);
		}
	}

	public virtual void DrawQuickAccessToolbarBackground(RibbonControlRendererEventArgs e)
	{
		OnRenderQuickAccessToolbarBackground(e);
	}

	protected virtual void OnRenderMdiSystemItem(MdiSystemItemRendererEventArgs e)
	{
		if (this.RenderMdiSystemItem != null)
		{
			this.RenderMdiSystemItem(this, e);
		}
	}

	public virtual void DrawMdiSystemItem(MdiSystemItemRendererEventArgs e)
	{
		OnRenderMdiSystemItem(e);
	}

	protected virtual void OnRenderFormCaptionBackground(FormCaptionRendererEventArgs e)
	{
		if (this.RenderFormCaptionBackground != null)
		{
			this.RenderFormCaptionBackground(this, e);
		}
	}

	public virtual void DrawFormCaptionBackground(FormCaptionRendererEventArgs e)
	{
		OnRenderFormCaptionBackground(e);
	}

	protected virtual void OnRenderQatOverflowItem(QatOverflowItemRendererEventArgs e)
	{
		if (this.RenderQatOverflowItem != null)
		{
			this.RenderQatOverflowItem(this, e);
		}
	}

	public virtual void DrawQatOverflowItem(QatOverflowItemRendererEventArgs e)
	{
		OnRenderQatOverflowItem(e);
	}

	protected virtual void OnRenderQatCustomizeItem(QatCustomizeItemRendererEventArgs e)
	{
		if (this.RenderQatCustomizeItem != null)
		{
			this.RenderQatCustomizeItem(this, e);
		}
	}

	public virtual void DrawQatCustomizeItem(QatCustomizeItemRendererEventArgs e)
	{
		OnRenderQatCustomizeItem(e);
	}

	protected virtual void OnRenderCheckBoxItem(CheckBoxItemRenderEventArgs e)
	{
		if (this.RenderCheckBoxItem != null)
		{
			this.RenderCheckBoxItem(this, e);
		}
	}

	public virtual void DrawCheckBoxItem(CheckBoxItemRenderEventArgs e)
	{
		OnRenderCheckBoxItem(e);
	}

	protected virtual void OnRenderProgressBarItem(ProgressBarItemRenderEventArgs e)
	{
		if (this.RenderProgressBarItem != null)
		{
			this.RenderProgressBarItem(this, e);
		}
	}

	public virtual void DrawProgressBarItem(ProgressBarItemRenderEventArgs e)
	{
		OnRenderProgressBarItem(e);
	}

	protected virtual void OnRenderNavPaneButtonBackground(NavPaneRenderEventArgs e)
	{
		if (this.RenderNavPaneButtonBackground != null)
		{
			this.RenderNavPaneButtonBackground(this, e);
		}
	}

	public virtual void DrawNavPaneButtonBackground(NavPaneRenderEventArgs e)
	{
		OnRenderNavPaneButtonBackground(e);
	}

	protected virtual void OnRenderSliderItem(SliderItemRendererEventArgs e)
	{
		if (this.RenderSliderItem != null)
		{
			this.RenderSliderItem(this, e);
		}
	}

	public virtual void DrawSliderItem(SliderItemRendererEventArgs e)
	{
		OnRenderSliderItem(e);
	}

	protected virtual void OnRenderSideBar(SideBarRendererEventArgs e)
	{
		if (this.RenderSideBar != null)
		{
			this.RenderSideBar(this, e);
		}
	}

	public virtual void DrawSideBar(SideBarRendererEventArgs e)
	{
		OnRenderSideBar(e);
	}

	protected virtual void OnRenderSideBarPanelItem(SideBarPanelItemRendererEventArgs e)
	{
		if (this.RenderSideBarPanelItem != null)
		{
			this.RenderSideBarPanelItem(this, e);
		}
	}

	public virtual void DrawSideBarPanelItem(SideBarPanelItemRendererEventArgs e)
	{
		OnRenderSideBarPanelItem(e);
	}

	protected virtual void OnRenderCrumbBarItemView(ButtonItemRendererEventArgs e)
	{
		if (this.RenderCrumbBarItemView != null)
		{
			this.RenderCrumbBarItemView(this, e);
		}
	}

	public virtual void DrawCrumbBarItemView(ButtonItemRendererEventArgs e)
	{
		OnRenderCrumbBarItemView(e);
	}

	protected virtual void OnRenderCrumbBarOverflowItem(ButtonItemRendererEventArgs e)
	{
		if (this.RenderCrumbBarOverflowItem != null)
		{
			this.RenderCrumbBarOverflowItem(this, e);
		}
	}

	public virtual void DrawCrumbBarOverflowItem(ButtonItemRendererEventArgs e)
	{
		OnRenderCrumbBarOverflowItem(e);
	}

	protected virtual void OnRenderSwitchButton(SwitchButtonRenderEventArgs e)
	{
		if (this.RenderSwitchButton != null)
		{
			this.RenderSwitchButton(this, e);
		}
	}

	public virtual void DrawSwitchButton(SwitchButtonRenderEventArgs e)
	{
		OnRenderSwitchButton(e);
	}
}
