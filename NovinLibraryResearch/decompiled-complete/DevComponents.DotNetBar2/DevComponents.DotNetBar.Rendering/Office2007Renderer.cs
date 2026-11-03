using System;
using System.Threading;

namespace DevComponents.DotNetBar.Rendering;

public class Office2007Renderer : BaseRenderer
{
	private EventHandler ቇ;

	private Office2007ColorTable ຊ;

	public Office2007ColorTable ColorTable
	{
		get
		{
			return ຊ;
		}
		set
		{
			ຊ = value;
			if (ቇ != null)
			{
				ቇ(this, new EventArgs());
			}
		}
	}

	public event EventHandler ColorTableChanged
	{
		add
		{
			EventHandler eventHandler = ቇ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ቇ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler eventHandler = ቇ;
			EventHandler eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler value2 = (EventHandler)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ቇ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public Office2007Renderer()
	{
		ຊ = new Office2007ColorTable();
	}

	public override void DrawKeyTips(KeyTipsRendererEventArgs e)
	{
		_၁ _၁ = ᣓ.ᤑ();
		if (_၁ is ᙓ)
		{
			((ᙓ)_၁).ᘽ = ຊ;
		}
		_၁.PaintKeyTips(e);
		base.DrawKeyTips(e);
	}

	public override void DrawRibbonTabGroup(RibbonTabGroupRendererEventArgs e)
	{
		_250D _250D = ᣓ.ᤔ(e.EffectiveStyle);
		if (_250D is ᙓ)
		{
			((ᙓ)_250D).ᘽ = ຊ;
		}
		_250D.PaintTabGroup(e);
		base.DrawRibbonTabGroup(e);
	}

	public override void DrawItemContainer(ItemContainerRendererEventArgs e)
	{
		_24AE _24AE = ᣓ.ᤏ(e.ItemContainer);
		if (_24AE != null)
		{
			if (_24AE is ᙓ)
			{
				((ᙓ)_24AE).ᘽ = ຊ;
			}
			_24AE.PaintBackground(e);
		}
		base.DrawItemContainer(e);
	}

	public override void DrawItemContainerSeparator(ItemContainerSeparatorRendererEventArgs e)
	{
		base.DrawItemContainerSeparator(e);
		_24AE _24AE = ᣓ.ᤏ(e.ItemContainer);
		if (_24AE != null)
		{
			if (_24AE is ᙓ)
			{
				((ᙓ)_24AE).ᘽ = ຊ;
			}
			_24AE.PaintItemSeparator(e);
		}
	}

	public override void DrawButtonItem(ButtonItemRendererEventArgs e)
	{
		_192F obj = ᣓ.ᤈ(e.ButtonItem);
		if (obj != null)
		{
			if (obj is ᙓ)
			{
				((ᙓ)obj).ᘽ = ຊ;
			}
			obj.PaintButton(e.ButtonItem, e.က);
		}
		base.DrawButtonItem(e);
	}

	public override void DrawRibbonTabItem(RibbonTabItemRendererEventArgs e)
	{
		_192F obj = ᣓ.ᤌ(e.RibbonTabItem);
		if (obj != null)
		{
			if (obj is ᙓ)
			{
				((ᙓ)obj).ᘽ = ຊ;
			}
			obj.PaintButton(e.RibbonTabItem, e.က);
		}
		base.DrawRibbonTabItem(e);
	}

	public override void DrawPopupToolbarBackground(ToolbarRendererEventArgs e)
	{
		_24B2 _24B2 = ᣓ.ᤐ(e.Bar);
		if (_24B2 is ᙓ)
		{
			((ᙓ)_24B2).ᘽ = ຊ;
		}
		_24B2.PaintPopupBackground(e);
		base.DrawPopupToolbarBackground(e);
	}

	public override void DrawToolbarBackground(ToolbarRendererEventArgs e)
	{
		_24B2 _24B2 = ᣓ.ᤐ(e.Bar);
		if (_24B2 is ᙓ)
		{
			((ᙓ)_24B2).ᘽ = ຊ;
		}
		if (e.Bar.BarState == eBarState.Docked)
		{
			_24B2.PaintDockedBackground(e);
		}
		else if (e.Bar.BarState == eBarState.Floating)
		{
			_24B2.PaintFloatingBackground(e);
		}
		base.DrawToolbarBackground(e);
	}

	protected virtual void DrawFloatingToolbarBackground(ToolbarRendererEventArgs e)
	{
	}

	public override void DrawRibbonDialogLauncher(RibbonBarRendererEventArgs e)
	{
		_24B5 _24B5 = ᣓ.ᤒ(e.RibbonBar);
		if (_24B5 is ᙓ)
		{
			((ᙓ)_24B5).ᘽ = ຊ;
		}
		_24B5.PaintDialogLauncher(e);
		base.DrawRibbonDialogLauncher(e);
	}

	public override void DrawColorItem(ColorItemRendererEventArgs e)
	{
		Ẓ ẓ2 = ᣓ.ᤕ(e.ColorItem);
		if (ẓ2 is ᙓ)
		{
			((ᙓ)ẓ2).ᘽ = ຊ;
		}
		ẓ2.PaintColorItem(e);
		base.DrawColorItem(e);
	}

	public override void DrawRibbonControlBackground(RibbonControlRendererEventArgs e)
	{
		_2205 _2205 = ᣓ.ᤖ(e.RibbonControl);
		if (_2205 != null)
		{
			if (_2205 is ᙓ)
			{
				((ᙓ)_2205).ᘽ = ຊ;
			}
			_2205.PaintBackground(e);
			base.DrawRibbonControlBackground(e);
		}
	}

	public override void DrawRibbonFormCaptionText(RibbonControlRendererEventArgs e)
	{
		base.DrawRibbonFormCaptionText(e);
		_2205 _2205 = ᣓ.ᤖ(e.RibbonControl);
		if (_2205 != null)
		{
			if (_2205 is ᙓ)
			{
				((ᙓ)_2205).ᘽ = ຊ;
			}
			_2205.PaintCaptionText(e);
		}
	}

	public override void DrawQuickAccessToolbarBackground(RibbonControlRendererEventArgs e)
	{
		base.DrawQuickAccessToolbarBackground(e);
		_2205 _2205 = ᣓ.ᤖ(e.RibbonControl);
		if (_2205 != null)
		{
			if (_2205 is ᙓ)
			{
				((ᙓ)_2205).ᘽ = ຊ;
			}
			_2205.PaintQuickAccessToolbarBackground(e);
		}
	}

	public override void DrawSystemCaptionItem(SystemCaptionItemRendererEventArgs e)
	{
		_24B7 _24D1 = ᣓ.ᤗ(e.SystemCaptionItem);
		if (_24D1 != null)
		{
			if (_24D1 is ᙓ)
			{
				((ᙓ)_24D1).ᘽ = ຊ;
			}
			_24D1.Paint(e);
			base.DrawSystemCaptionItem(e);
		}
	}

	public override void DrawMdiSystemItem(MdiSystemItemRendererEventArgs e)
	{
		_24BA _24D4 = ᣓ.ᤘ(e.MdiSystemItem);
		if (_24D4 != null)
		{
			if (_24D4 is ᙓ)
			{
				((ᙓ)_24D4).ᘽ = ຊ;
			}
			_24D4.Paint(e);
			base.DrawMdiSystemItem(e);
		}
	}

	public override void DrawFormCaptionBackground(FormCaptionRendererEventArgs e)
	{
		_24BC _24D6 = ᣓ.ᤚ(e.Form);
		if (_24D6 != null)
		{
			if (_24D6 is ᙓ)
			{
				((ᙓ)_24D6).ᘽ = ຊ;
			}
			_24D6.PaintCaptionBackground(e);
			base.DrawFormCaptionBackground(e);
		}
	}

	public override void DrawQatOverflowItem(QatOverflowItemRendererEventArgs e)
	{
		_20A8 _20A82 = ᣓ.ᤛ(e.OverflowItem);
		if (_20A82 != null)
		{
			if (_20A82 is ᙓ)
			{
				((ᙓ)_20A82).ᘽ = ຊ;
			}
			_20A82.Paint(e);
			base.DrawQatOverflowItem(e);
		}
	}

	public override void DrawQatCustomizeItem(QatCustomizeItemRendererEventArgs e)
	{
		ᯈ ᯈ2 = ᣓ.ᤝ(e.CustomizeItem);
		if (ᯈ2 != null)
		{
			if (ᯈ2 is ᙓ)
			{
				((ᙓ)ᯈ2).ᘽ = ຊ;
			}
			ᯈ2.Paint(e);
			base.DrawQatCustomizeItem(e);
		}
	}

	public override void DrawCheckBoxItem(CheckBoxItemRenderEventArgs e)
	{
		ᢘ ᢘ2 = ᣓ._191F(e.CheckBoxItem);
		if (ᢘ2 != null)
		{
			if (ᢘ2 is ᙓ)
			{
				((ᙓ)ᢘ2).ᘽ = ຊ;
			}
			ᢘ2.Paint(e);
			base.DrawCheckBoxItem(e);
		}
	}

	public override void DrawProgressBarItem(ProgressBarItemRenderEventArgs e)
	{
		_24BE _24D8 = ᣓ._1921(e.ProgressBarItem);
		if (_24D8 != null)
		{
			if (_24D8 is ᙓ)
			{
				((ᙓ)_24D8).ᘽ = ຊ;
			}
			_24D8.Paint(e);
			base.DrawProgressBarItem(e);
		}
	}

	public override void DrawNavPaneButtonBackground(NavPaneRenderEventArgs e)
	{
		_24C0 _24DA = ᣓ._1923();
		if (_24DA != null)
		{
			if (_24DA is ᙓ)
			{
				((ᙓ)_24DA).ᘽ = ຊ;
			}
			_24DA.PaintButtonBackground(e);
			base.DrawNavPaneButtonBackground(e);
		}
	}

	public override void DrawSliderItem(SliderItemRendererEventArgs e)
	{
		_24C1 _24DB = ᣓ._1924(e.SliderItem);
		if (_24DB != null)
		{
			if (_24DB is ᙓ)
			{
				((ᙓ)_24DB).ᘽ = ຊ;
			}
			_24DB.Paint(e);
			base.DrawSliderItem(e);
		}
	}

	public override void DrawSideBar(SideBarRendererEventArgs e)
	{
		_24DE _24DE2 = ᣓ._1927();
		if (_24DE2 != null)
		{
			if (_24DE2 is ᙓ)
			{
				((ᙓ)_24DE2).ᘽ = ຊ;
			}
			_24DE2.PaintSideBar(e);
			base.DrawSideBar(e);
		}
	}

	public override void DrawSideBarPanelItem(SideBarPanelItemRendererEventArgs e)
	{
		_24DE _24DE2 = ᣓ._1927();
		if (_24DE2 != null)
		{
			if (_24DE2 is ᙓ)
			{
				((ᙓ)_24DE2).ᘽ = ຊ;
			}
			_24DE2.PaintSideBarPanelItem(e);
			base.DrawSideBarPanelItem(e);
		}
	}

	public override void DrawCrumbBarItemView(ButtonItemRendererEventArgs e)
	{
		_24DF _24DF = ᣓ.ᤊ(e.ButtonItem);
		if (_24DF != null)
		{
			if (_24DF != null)
			{
				((ᙓ)_24DF).ᘽ = ຊ;
			}
			_24DF._139A(e.ButtonItem, e.က);
		}
		base.DrawCrumbBarItemView(e);
	}

	public override void DrawCrumbBarOverflowItem(ButtonItemRendererEventArgs e)
	{
		_24DF _24DF = ᣓ.ᤊ(e.ButtonItem);
		if (_24DF != null)
		{
			if (_24DF != null)
			{
				((ᙓ)_24DF).ᘽ = ຊ;
			}
			_24DF._24E6(e.ButtonItem, e.က);
		}
		base.DrawCrumbBarOverflowItem(e);
	}

	public override void DrawSwitchButton(SwitchButtonRenderEventArgs e)
	{
		ᣒ ᣒ2 = ᣓ._1928(e.SwitchButtonItem);
		if (ᣒ2 != null)
		{
			if (ᣒ2 is ᙓ)
			{
				((ᙓ)ᣒ2).ᘽ = ຊ;
			}
			ᣒ2.Paint(e);
		}
		base.DrawSwitchButton(e);
	}

	public override void DrawRangeSliderItem(RangeSliderItemRendererEventArgs e)
	{
		_24E7 _24E72 = ᣓ._1926(e.SliderItem);
		if (_24E72 != null)
		{
			if (_24E72 is ᙓ)
			{
				((ᙓ)_24E72).ᘽ = ຊ;
			}
			_24E72.Paint(e);
			base.DrawRangeSliderItem(e);
		}
	}

	public override void DrawStepItem(StepItemRendererEventArgs e)
	{
		_24A7 _24A72 = ᣓ._1929(e.Item);
		if (_24A72 != null)
		{
			if (_24A72 != null)
			{
				((ᙓ)_24A72).ᘽ = ຊ;
			}
			_24A72.Paint(e);
			base.DrawStepItem(e);
		}
	}

	public override void DrawListBoxItem(ListBoxItemRendererEventArgs e)
	{
		_245E obj = ᣓ._192A(e.Item);
		if (obj != null)
		{
			if (obj != null)
			{
				((ᙓ)obj).ᘽ = ຊ;
			}
			obj.Paint(e);
			base.DrawListBoxItem(e);
		}
	}

	public override void DrawSideNavItem(SideNavItemRendererEventArgs e)
	{
		base.DrawSideNavItem(e);
		if (e.Cancel)
		{
			return;
		}
		_2514 _25142 = ᣓ._192B(e.Item);
		if (_25142 != null)
		{
			if (_25142 != null)
			{
				((ᙓ)_25142).ᘽ = ຊ;
			}
			_25142.Paint(e);
		}
	}

	public override void DrawTabFormStrip(TabFormStripPainterArgs e)
	{
		base.DrawTabFormStrip(e);
		if (e.Cancel)
		{
			return;
		}
		_2515 _25152 = ᣓ._192C(e.TabFormStrip);
		if (_25152 != null)
		{
			if (_25152 is ᙓ)
			{
				((ᙓ)_25152).ᘽ = ຊ;
			}
			_25152.Paint(e);
		}
	}

	public override void DrawTabFormItem(ButtonItemRendererEventArgs e)
	{
		base.DrawTabFormItem(e);
		if (e.Cancel)
		{
			return;
		}
		_192F obj = ᣓ.ᤈ(e.ButtonItem);
		if (obj != null)
		{
			if (obj is ᙓ)
			{
				((ᙓ)obj).ᘽ = ຊ;
			}
			obj.PaintButton(e.ButtonItem, e.က);
		}
	}

	public override void DrawNewTabFormItem(ButtonItemRendererEventArgs e)
	{
		base.DrawNewTabFormItem(e);
		if (e.Cancel)
		{
			return;
		}
		_192F obj = ᣓ.ᤈ(e.ButtonItem);
		if (obj != null)
		{
			if (obj is ᙓ)
			{
				((ᙓ)obj).ᘽ = ຊ;
			}
			obj.PaintButton(e.ButtonItem, e.က);
		}
	}

	public override void DrawTabParentForm(TabFormPainterArgs e)
	{
		base.DrawTabParentForm(e);
		if (!e.Cancel)
		{
			_2516 _25162 = ᣓ._192D(e.TabParentForm);
			if (_25162 is ᙓ)
			{
				((ᙓ)_25162).ᘽ = ຊ;
			}
			_25162.Paint(e);
		}
	}
}
