using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Viewer;

[ToolboxItem(false)]
internal class StiMultiPagesButton : StiToolButton, IStiPopupParentControl
{
	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool LockPopupInvoke { get; set; }

	internal StiMultiPagesForm MultiPagesForm { get; set; }

	internal int PageWidth { get; set; }

	internal int PageHeight { get; set; }

	public event EventHandler ChangeMultiPage;

	[Category("Behavior")]
	public event EventHandler Close;

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((StiToolButton)this).OnMouseDown(e);
		if (!LockPopupInvoke && MultiPagesForm == null)
		{
			MultiPagesForm = new StiMultiPagesForm((Control)(object)this);
			Rectangle rectangle = ((Control)this).RectangleToScreen(((Control)this).ClientRectangle);
			((StiPopupBaseForm)MultiPagesForm).SetLocation(rectangle.Left, rectangle.Top + ((Control)this).Height + 1, 161, 108);
			((StiPopupBaseForm)MultiPagesForm).ClosePopup += MultiPagesForm_ClosePopup;
			((StiPopupBaseForm)MultiPagesForm).ShowPopupForm();
		}
		else
		{
			LockPopupInvoke = false;
			((StiToolButton)this).Pushed = true;
		}
	}

	private void MultiPagesForm_ClosePopup(object sender, EventArgs e)
	{
		if (MultiPagesForm != null)
		{
			((Component)(object)MultiPagesForm).Dispose();
			MultiPagesForm = null;
			((StiToolButton)this).Pushed = false;
			InvokeClose(e);
		}
	}

	internal void InvokeChangeMultiPage()
	{
		this.ChangeMultiPage?.Invoke(this, EventArgs.Empty);
	}

	public virtual void InvokeClose(EventArgs e)
	{
		this.Close?.Invoke(this, e);
	}

	public StiMultiPagesButton()
	{
		((StiToolButton)this).Style = (StiToolBarButtonStyle)1;
	}
}
