using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Design.Controls;

[ToolboxItem(false)]
internal class StiExpressionButton : StiToolButton, IStiPopupParentControl
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool LockPopupInvoke { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	internal StiExpressionForm ExpressionForm { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public StiReport Report { get; set; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string ExpressionString { get; set; }

	[Category("Behavior")]
	public event EventHandler Close;

	protected override void OnMouseDown(MouseEventArgs e)
	{
		base.OnMouseDown(e);
		if (LockPopupInvoke || ExpressionForm != null)
		{
			LockPopupInvoke = false;
			base.Pushed = true;
			return;
		}
		ExpressionForm = new StiExpressionForm(Report, (Control)(object)this);
		ExpressionString = null;
		Rectangle rectangle = ((Control)this).RectangleToScreen(((Control)this).ClientRectangle);
		ExpressionForm.SetLocation(rectangle.Left, rectangle.Top + ((Control)this).Height + StiScale.I2, StiScale.XXI(300.0), StiScale.YYI(400.0));
		ExpressionForm.ClosePopup += ExpressionForm_ClosePopup;
		ExpressionForm.ShowPopupForm();
		((Control)ExpressionForm.DictControl.tvData).Invalidate();
	}

	private void ExpressionForm_ClosePopup(object sender, EventArgs e)
	{
		if (ExpressionForm != null)
		{
			((Component)(object)ExpressionForm).Dispose();
			ExpressionForm = null;
			base.Pushed = false;
			InvokeClose(e);
		}
	}

	public virtual void InvokeClose(EventArgs e)
	{
		this.Close?.Invoke(this, e);
	}

	public StiExpressionButton()
	{
		base.Style = StiToolBarButtonStyle.PushButton;
	}
}
