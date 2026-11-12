using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

internal class Office2007FormCaptionPainter : FormCaptionPainter, IOffice2007Painter
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
		}
	}

	public override void PaintCaptionBackground(FormCaptionRendererEventArgs e)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		Office2007FormStateColorTable office2007FormStateColorTable = m_ColorTable.Form.Active;
		Form form = e.Form;
		bool flag = false;
		if (form != null && form is RibbonForm)
		{
			if (!((RibbonForm)(object)form).NonClientActive)
			{
				office2007FormStateColorTable = m_ColorTable.Form.Inactive;
			}
		}
		else if (form != null && ((form != Form.ActiveForm && form.MdiParent == null) || (form.MdiParent != null && form.MdiParent.ActiveMdiChild != form)))
		{
			office2007FormStateColorTable = m_ColorTable.Form.Inactive;
		}
		if (form is OfficeForm)
		{
			flag = true;
		}
		Rectangle bounds = e.Bounds;
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)0;
		Rectangle r = new Rectangle(bounds.X, bounds.Y, bounds.Width, (int)((double)bounds.Height * 0.3));
		DisplayHelp.FillRectangle(graphics, r, office2007FormStateColorTable.CaptionTopBackground);
		Rectangle r2 = new Rectangle(bounds.X, r.Bottom, bounds.Width, bounds.Height - r.Height);
		DisplayHelp.FillRectangle(graphics, r2, office2007FormStateColorTable.CaptionBottomBackground);
		if (flag && office2007FormStateColorTable.CaptionBottomBorder != null && office2007FormStateColorTable.CaptionBottomBorder.Length != 0)
		{
			int num = office2007FormStateColorTable.CaptionBottomBorder.Length;
			for (int i = 0; i < num; i++)
			{
				DisplayHelp.DrawLine(graphics, bounds.X, bounds.Bottom - num + i, bounds.Right, bounds.Bottom - num + i, office2007FormStateColorTable.CaptionBottomBorder[i], 1);
			}
		}
		graphics.SmoothingMode = smoothingMode;
	}
}
