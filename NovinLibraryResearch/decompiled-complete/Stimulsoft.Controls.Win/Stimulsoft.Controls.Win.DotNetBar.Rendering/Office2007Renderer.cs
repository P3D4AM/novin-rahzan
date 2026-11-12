using System;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

public class Office2007Renderer : BaseRenderer
{
	private Office2007ColorTable m_ColorTable;

	public Office2007ColorTable ColorTable
	{
		get
		{
			return m_ColorTable;
		}
		set
		{
			m_ColorTable = value;
			if (this.ColorTableChanged != null)
			{
				this.ColorTableChanged(this, new EventArgs());
			}
		}
	}

	public event EventHandler ColorTableChanged;

	public Office2007Renderer()
	{
		m_ColorTable = new Office2007ColorTable();
	}

	public override void DrawKeyTips(KeyTipsRendererEventArgs e)
	{
		KeyTipsPainter keyTipsPainter = PainterFactory.CreateKeyTipsPainter();
		if (keyTipsPainter is IOffice2007Painter)
		{
			((IOffice2007Painter)keyTipsPainter).ColorTable = m_ColorTable;
		}
		keyTipsPainter.PaintKeyTips(e);
		base.DrawKeyTips(e);
	}

	public override void DrawRibbonTabGroup(RibbonTabGroupRendererEventArgs e)
	{
		RibbonTabGroupPainter ribbonTabGroupPainter = PainterFactory.CreateRibbonTabGroupPainter(e.EffectiveStyle);
		if (ribbonTabGroupPainter is IOffice2007Painter)
		{
			((IOffice2007Painter)ribbonTabGroupPainter).ColorTable = m_ColorTable;
		}
		ribbonTabGroupPainter.PaintTabGroup(e);
		base.DrawRibbonTabGroup(e);
	}

	public override void DrawItemContainer(ItemContainerRendererEventArgs e)
	{
		ItemContainerPainter itemContainerPainter = PainterFactory.CreateItemContainerPainter(e.ItemContainer);
		if (itemContainerPainter != null)
		{
			if (itemContainerPainter is IOffice2007Painter)
			{
				((IOffice2007Painter)itemContainerPainter).ColorTable = m_ColorTable;
			}
			itemContainerPainter.PaintBackground(e);
		}
		base.DrawItemContainer(e);
	}

	public override void DrawItemContainerSeparator(ItemContainerSeparatorRendererEventArgs e)
	{
		base.DrawItemContainerSeparator(e);
		ItemContainerPainter itemContainerPainter = PainterFactory.CreateItemContainerPainter(e.ItemContainer);
		if (itemContainerPainter != null)
		{
			if (itemContainerPainter is IOffice2007Painter)
			{
				((IOffice2007Painter)itemContainerPainter).ColorTable = m_ColorTable;
			}
			itemContainerPainter.PaintItemSeparator(e);
		}
	}

	public override void DrawButtonItem(ButtonItemRendererEventArgs e)
	{
		ButtonItemPainter buttonItemPainter = PainterFactory.CreateButtonPainter(e.ButtonItem);
		if (buttonItemPainter != null)
		{
			if (buttonItemPainter is IOffice2007Painter)
			{
				((IOffice2007Painter)buttonItemPainter).ColorTable = m_ColorTable;
			}
			buttonItemPainter.PaintButton(e.ButtonItem, e.ItemPaintArgs);
		}
		base.DrawButtonItem(e);
	}

	public override void DrawRibbonTabItem(RibbonTabItemRendererEventArgs e)
	{
		ButtonItemPainter buttonItemPainter = PainterFactory.CreateRibbonTabItemPainter(e.RibbonTabItem);
		if (buttonItemPainter != null)
		{
			if (buttonItemPainter is IOffice2007Painter)
			{
				((IOffice2007Painter)buttonItemPainter).ColorTable = m_ColorTable;
			}
			buttonItemPainter.PaintButton(e.RibbonTabItem, e.ItemPaintArgs);
		}
		base.DrawRibbonTabItem(e);
	}

	public override void DrawPopupToolbarBackground(ToolbarRendererEventArgs e)
	{
		BarBackgroundPainter barBackgroundPainter = PainterFactory.CreateBarBackgroundPainter(e.Bar);
		if (barBackgroundPainter is IOffice2007Painter)
		{
			((IOffice2007Painter)barBackgroundPainter).ColorTable = m_ColorTable;
		}
		barBackgroundPainter.PaintPopupBackground(e);
		base.DrawPopupToolbarBackground(e);
	}

	public override void DrawToolbarBackground(ToolbarRendererEventArgs e)
	{
		BarBackgroundPainter barBackgroundPainter = PainterFactory.CreateBarBackgroundPainter(e.Bar);
		if (barBackgroundPainter is IOffice2007Painter)
		{
			((IOffice2007Painter)barBackgroundPainter).ColorTable = m_ColorTable;
		}
		if (e.Bar.BarState == eBarState.Docked)
		{
			barBackgroundPainter.PaintDockedBackground(e);
		}
		else if (e.Bar.BarState == eBarState.Floating)
		{
			barBackgroundPainter.PaintFloatingBackground(e);
		}
		base.DrawToolbarBackground(e);
	}

	protected virtual void DrawFloatingToolbarBackground(ToolbarRendererEventArgs e)
	{
	}

	public override void DrawRibbonDialogLauncher(RibbonBarRendererEventArgs e)
	{
		DialogLauncherPainter dialogLauncherPainter = PainterFactory.CreateRibbonBarPainter(e.RibbonBar);
		if (dialogLauncherPainter is IOffice2007Painter)
		{
			((IOffice2007Painter)dialogLauncherPainter).ColorTable = m_ColorTable;
		}
		dialogLauncherPainter.PaintDialogLauncher(e);
		base.DrawRibbonDialogLauncher(e);
	}

	public override void DrawColorItem(ColorItemRendererEventArgs e)
	{
		ColorItemPainter colorItemPainter = PainterFactory.CreateColorItemPainter(e.ColorItem);
		if (colorItemPainter is IOffice2007Painter)
		{
			((IOffice2007Painter)colorItemPainter).ColorTable = m_ColorTable;
		}
		colorItemPainter.PaintColorItem(e);
		base.DrawColorItem(e);
	}

	public override void DrawRibbonControlBackground(RibbonControlRendererEventArgs e)
	{
		RibbonControlPainter ribbonControlPainter = PainterFactory.CreateRibbonControlPainter(e.RibbonControl);
		if (ribbonControlPainter != null)
		{
			if (ribbonControlPainter is IOffice2007Painter)
			{
				((IOffice2007Painter)ribbonControlPainter).ColorTable = m_ColorTable;
			}
			ribbonControlPainter.PaintBackground(e);
			base.DrawRibbonControlBackground(e);
		}
	}

	public override void DrawRibbonFormCaptionText(RibbonControlRendererEventArgs e)
	{
		base.DrawRibbonFormCaptionText(e);
		RibbonControlPainter ribbonControlPainter = PainterFactory.CreateRibbonControlPainter(e.RibbonControl);
		if (ribbonControlPainter != null)
		{
			if (ribbonControlPainter is IOffice2007Painter)
			{
				((IOffice2007Painter)ribbonControlPainter).ColorTable = m_ColorTable;
			}
			ribbonControlPainter.PaintCaptionText(e);
		}
	}

	public override void DrawQuickAccessToolbarBackground(RibbonControlRendererEventArgs e)
	{
		base.DrawQuickAccessToolbarBackground(e);
		RibbonControlPainter ribbonControlPainter = PainterFactory.CreateRibbonControlPainter(e.RibbonControl);
		if (ribbonControlPainter != null)
		{
			if (ribbonControlPainter is IOffice2007Painter)
			{
				((IOffice2007Painter)ribbonControlPainter).ColorTable = m_ColorTable;
			}
			ribbonControlPainter.PaintQuickAccessToolbarBackground(e);
		}
	}

	public override void DrawSystemCaptionItem(SystemCaptionItemRendererEventArgs e)
	{
		SystemCaptionItemPainter systemCaptionItemPainter = PainterFactory.CreateSystemCaptionItemPainter(e.SystemCaptionItem);
		if (systemCaptionItemPainter != null)
		{
			if (systemCaptionItemPainter is IOffice2007Painter)
			{
				((IOffice2007Painter)systemCaptionItemPainter).ColorTable = m_ColorTable;
			}
			systemCaptionItemPainter.Paint(e);
			base.DrawSystemCaptionItem(e);
		}
	}

	public override void DrawMdiSystemItem(MdiSystemItemRendererEventArgs e)
	{
		MdiSystemItemPainter mdiSystemItemPainter = PainterFactory.CreateMdiSystemItemPainter(e.MdiSystemItem);
		if (mdiSystemItemPainter != null)
		{
			if (mdiSystemItemPainter is IOffice2007Painter)
			{
				((IOffice2007Painter)mdiSystemItemPainter).ColorTable = m_ColorTable;
			}
			mdiSystemItemPainter.Paint(e);
			base.DrawMdiSystemItem(e);
		}
	}

	public override void DrawFormCaptionBackground(FormCaptionRendererEventArgs e)
	{
		FormCaptionPainter formCaptionPainter = PainterFactory.CreateFormCaptionPainter(e.Form);
		if (formCaptionPainter != null)
		{
			if (formCaptionPainter is IOffice2007Painter)
			{
				((IOffice2007Painter)formCaptionPainter).ColorTable = m_ColorTable;
			}
			formCaptionPainter.PaintCaptionBackground(e);
			base.DrawFormCaptionBackground(e);
		}
	}

	public override void DrawQatOverflowItem(QatOverflowItemRendererEventArgs e)
	{
		QatOverflowPainter qatOverflowPainter = PainterFactory.CreateQatOverflowItemPainter(e.OverflowItem);
		if (qatOverflowPainter != null)
		{
			if (qatOverflowPainter is IOffice2007Painter)
			{
				((IOffice2007Painter)qatOverflowPainter).ColorTable = m_ColorTable;
			}
			qatOverflowPainter.Paint(e);
			base.DrawQatOverflowItem(e);
		}
	}

	public override void DrawQatCustomizeItem(QatCustomizeItemRendererEventArgs e)
	{
		QatCustomizeItemPainter qatCustomizeItemPainter = PainterFactory.CreateQatCustomizeItemPainter(e.CustomizeItem);
		if (qatCustomizeItemPainter != null)
		{
			if (qatCustomizeItemPainter is IOffice2007Painter)
			{
				((IOffice2007Painter)qatCustomizeItemPainter).ColorTable = m_ColorTable;
			}
			qatCustomizeItemPainter.Paint(e);
			base.DrawQatCustomizeItem(e);
		}
	}

	public override void DrawCheckBoxItem(CheckBoxItemRenderEventArgs e)
	{
		CheckBoxItemPainter checkBoxItemPainter = PainterFactory.CreateCheckBoxItemPainter(e.CheckBoxItem);
		if (checkBoxItemPainter != null)
		{
			if (checkBoxItemPainter is IOffice2007Painter)
			{
				((IOffice2007Painter)checkBoxItemPainter).ColorTable = m_ColorTable;
			}
			checkBoxItemPainter.Paint(e);
			base.DrawCheckBoxItem(e);
		}
	}

	public override void DrawProgressBarItem(ProgressBarItemRenderEventArgs e)
	{
		ProgressBarItemPainter progressBarItemPainter = PainterFactory.CreateProgressBarItemPainter(e.ProgressBarItem);
		if (progressBarItemPainter != null)
		{
			if (progressBarItemPainter is IOffice2007Painter)
			{
				((IOffice2007Painter)progressBarItemPainter).ColorTable = m_ColorTable;
			}
			progressBarItemPainter.Paint(e);
			base.DrawProgressBarItem(e);
		}
	}

	public override void DrawNavPaneButtonBackground(NavPaneRenderEventArgs e)
	{
		NavigationPanePainter navigationPanePainter = PainterFactory.CreateNavigationPanePainter();
		if (navigationPanePainter != null)
		{
			if (navigationPanePainter is IOffice2007Painter)
			{
				((IOffice2007Painter)navigationPanePainter).ColorTable = m_ColorTable;
			}
			navigationPanePainter.PaintButtonBackground(e);
			base.DrawNavPaneButtonBackground(e);
		}
	}

	public override void DrawSliderItem(SliderItemRendererEventArgs e)
	{
		SliderPainter sliderPainter = PainterFactory.CreateSliderPainter(e.SliderItem);
		if (sliderPainter != null)
		{
			if (sliderPainter is IOffice2007Painter)
			{
				((IOffice2007Painter)sliderPainter).ColorTable = m_ColorTable;
			}
			sliderPainter.Paint(e);
			base.DrawSliderItem(e);
		}
	}

	public override void DrawSideBar(SideBarRendererEventArgs e)
	{
		SideBarPainter sideBarPainter = PainterFactory.CreateSideBarPainter();
		if (sideBarPainter != null)
		{
			if (sideBarPainter is IOffice2007Painter)
			{
				((IOffice2007Painter)sideBarPainter).ColorTable = m_ColorTable;
			}
			sideBarPainter.PaintSideBar(e);
			base.DrawSideBar(e);
		}
	}

	public override void DrawSideBarPanelItem(SideBarPanelItemRendererEventArgs e)
	{
		SideBarPainter sideBarPainter = PainterFactory.CreateSideBarPainter();
		if (sideBarPainter != null)
		{
			if (sideBarPainter is IOffice2007Painter)
			{
				((IOffice2007Painter)sideBarPainter).ColorTable = m_ColorTable;
			}
			sideBarPainter.PaintSideBarPanelItem(e);
			base.DrawSideBarPanelItem(e);
		}
	}

	public override void DrawSwitchButton(SwitchButtonRenderEventArgs e)
	{
		SwitchButtonPainter switchButtonPainter = PainterFactory.CreateSwitchButtonPainter(e.SwitchButtonItem);
		if (switchButtonPainter != null)
		{
			if (switchButtonPainter is IOffice2007Painter)
			{
				((IOffice2007Painter)switchButtonPainter).ColorTable = m_ColorTable;
			}
			switchButtonPainter.Paint(e);
		}
		base.DrawSwitchButton(e);
	}
}
