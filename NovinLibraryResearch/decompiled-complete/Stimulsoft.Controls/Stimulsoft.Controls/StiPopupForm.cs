using System;
using System.ComponentModel;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Controls;

public class StiPopupForm : StiPopupBaseForm
{
	private StiShadowPopupForm shadow;

	protected override bool Layered => false;

	protected bool IsDrawShadow => StiOSFeature.IsLayeredWindows;

	public bool IsDrawShadowInPopupWindow { get; set; } = true;


	protected override void Dispose(bool disposing)
	{
		((Form)this).Dispose(disposing);
		if (IsDrawShadow && !StiPopupBaseForm.is64Bit && shadow != null)
		{
			shadow.ClosePopupForm();
			((Component)(object)shadow).Dispose();
			shadow = null;
		}
	}

	public override void SetLocation(int x, int y, int width, int height)
	{
		base.SetLocation(x, y, width, height);
		((ScrollableControl)this).DockPadding.Left = StiScaleUI.I2;
		((ScrollableControl)this).DockPadding.Top = StiScaleUI.I2;
		((ScrollableControl)this).DockPadding.Right = StiScaleUI.I2;
		((ScrollableControl)this).DockPadding.Bottom = StiScaleUI.I2;
		if (IsDrawShadow && !StiPopupBaseForm.is64Bit && shadow != null)
		{
			((Control)shadow).CreateControl();
			((Control)shadow).SetBounds(((Control)this).Left + 4 + 1, ((Control)this).Top + 4 + 1, ((Control)this).Width, ((Control)this).Height, (BoundsSpecified)15);
		}
	}

	public override void ShowPopupForm()
	{
		base.ShowPopupForm();
		ShowShadow();
	}

	protected void ShowShadow()
	{
		if (IsDrawShadow && IsDrawShadowInPopupWindow && shadow != null && !((Control)shadow).IsDisposed && !StiPopupBaseForm.is64Bit && shadow != null)
		{
			shadow.UpdateZOrder(((Control)this).Handle);
			shadow.ShowPopupForm();
		}
	}

	protected override void OnClosePopup(EventArgs e)
	{
		base.OnClosePopup(e);
		if (IsDrawShadow && !StiPopupBaseForm.is64Bit && shadow != null)
		{
			((Control)shadow).Visible = false;
		}
	}

	public StiPopupForm()
		: this(null)
	{
	}

	public StiPopupForm(Control parentControl)
		: base(parentControl)
	{
		if (IsDrawShadow)
		{
			shadow = new StiShadowPopupForm(parentControl);
		}
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).SetStyle((ControlStyles)65536, false);
		((Control)this).SetStyle((ControlStyles)2, true);
	}
}
