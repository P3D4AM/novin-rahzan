using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls;

namespace Stimulsoft.Report.Viewer;

internal class StiMultiPagesForm : StiPopupForm
{
	private Container components;

	private Bitmap PageBmp { get; set; }

	internal int PageWidth { get; set; }

	internal int PageHeight { get; set; }

	public StiMultiPagesForm()
		: this(null)
	{
	}

	public StiMultiPagesForm(Control parentControl)
		: base(parentControl)
	{
		((Control)this).DoubleBuffered = true;
		InitializeComponent();
		PageBmp = StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Page.gif");
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Image)PageBmp).Dispose();
		((StiPopupForm)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		((Form)this).AutoScaleBaseSize = new Size(5, 13);
		((Control)this).BackColor = SystemColors.Window;
		((Form)this).ClientSize = new Size(320, 400);
		((Control)this).Name = "StiMultiPagesForm";
		((Control)this).Text = "StiMultiPagesForm";
		((Control)this).MouseHover += StiMultiPagesForm_MouseHover;
		((Control)this).MouseMove += new MouseEventHandler(StiMultiPagesForm_MouseMove);
		((Control)this).MouseLeave += StiMultiPagesForm_MouseLeave;
		((Control)this).Click += StiMultiPagesForm_Click;
	}

	protected override void WndProc(ref Message msg)
	{
		if (((Message)(ref msg)).Msg == 675)
		{
			((Control)this).Invalidate();
		}
		((StiPopupBaseForm)this).WndProc(ref msg);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		Point point = ((Control)this).PointToClient(Cursor.Position);
		Graphics graphics = e.Graphics;
		graphics.FillRectangle(Brushes.WhiteSmoke, 0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
		graphics.DrawRectangle(SystemPens.ControlDark, 0, 0, ((Control)this).Width - 1, ((Control)this).Height - 1);
		PageWidth = 0;
		PageHeight = 0;
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				Rectangle rectangle = new Rectangle(i * 26 + 4, j * 26 + 4, 22, 22);
				Color color = Color.White;
				if (point.X >= rectangle.X && point.Y >= rectangle.Y && point.X < ((Control)this).Width && point.Y < ((Control)this).Height)
				{
					PageWidth = Math.Max(PageWidth, i + 1);
					PageHeight = Math.Max(PageHeight, j + 1);
					color = SystemColors.Highlight;
				}
				Brush val = (Brush)new SolidBrush(color);
				try
				{
					graphics.FillRectangle(val, rectangle);
					graphics.DrawRectangle(SystemPens.ControlDark, rectangle);
					graphics.DrawImageUnscaled((Image)(object)PageBmp, rectangle.X + 4, rectangle.Y + 4);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
		}
		Rectangle rectangle2 = new Rectangle(4, 82, 152, 21);
		graphics.FillRectangle(Brushes.White, rectangle2);
		graphics.DrawRectangle(SystemPens.ControlDark, rectangle2);
		string text = StiLocalization.Get("FormViewer", "ZoomXXPagesCancel");
		if (PageWidth >= 1 && PageHeight >= 1)
		{
			text = string.Format(StiLocalization.Get("FormViewer", "ZoomXXPages"), PageWidth, PageHeight);
		}
		StringFormat val2 = new StringFormat();
		try
		{
			val2.Alignment = (StringAlignment)1;
			val2.LineAlignment = (StringAlignment)1;
			graphics.DrawString(text, ((Control)this).Font, Brushes.Black, (RectangleF)rectangle2, val2);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	private void StiMultiPagesForm_MouseMove(object sender, MouseEventArgs e)
	{
		((Control)this).Invalidate();
	}

	private void StiMultiPagesForm_MouseLeave(object sender, EventArgs e)
	{
		((Control)this).Invalidate();
	}

	private void StiMultiPagesForm_MouseHover(object sender, EventArgs e)
	{
		((Control)this).Invalidate();
	}

	private void StiMultiPagesForm_Click(object sender, EventArgs e)
	{
		((StiPopupBaseForm)this).ClosePopupForm();
		((StiMultiPagesButton)(object)((StiPopupBaseForm)this).ParentControl).PageWidth = PageWidth;
		((StiMultiPagesButton)(object)((StiPopupBaseForm)this).ParentControl).PageHeight = PageHeight;
		((StiMultiPagesButton)(object)((StiPopupBaseForm)this).ParentControl).InvokeChangeMultiPage();
	}
}
