using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;
using Stimulsoft.Controls.Win.DotNetBar;

namespace Stimulsoft.Report.Viewer;

[ToolboxItem(false)]
public class StiMultiPagesUserControl : UserControl
{
	private Container components;

	internal StiViewerControl viewerControl;

	private Bitmap PageBmp { get; set; }

	internal int PageWidth { get; set; }

	internal int PageHeight { get; set; }

	public StiMultiPagesUserControl()
	{
		InitializeComponent();
		PageBmp = StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Bmp.Office2013.Page" + StiScale.StepName + ".png");
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (PageBmp != null)
			{
				((Image)PageBmp).Dispose();
				PageBmp = null;
			}
			if (components != null)
			{
				components.Dispose();
			}
		}
		((ContainerControl)this).Dispose(disposing);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Expected O, but got Unknown
		Point point = ((Control)this).PointToClient(Cursor.Position);
		Graphics graphics = e.Graphics;
		graphics.FillRectangle(Brushes.White, 0, 0, ((Control)this).Width, ((Control)this).Height);
		PageWidth = 0;
		PageHeight = 0;
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				Rectangle rectangle = StiScale.I(new Rectangle(i * 26 + 4, j * 26 + 4, 22, 22));
				Color color = Color.White;
				if (point.X >= rectangle.X && point.Y >= rectangle.Y && point.X < ((Control)this).Width && point.Y < ((Control)this).Height)
				{
					PageWidth = Math.Max(PageWidth, i + 1);
					PageHeight = Math.Max(PageHeight, j + 1);
					color = Color.FromArgb(194, 213, 242);
				}
				Brush val = (Brush)new SolidBrush(color);
				try
				{
					graphics.FillRectangle(val, rectangle);
					graphics.DrawRectangle(SystemPens.ControlDark, rectangle);
					Rectangle rectangle2 = new Rectangle(rectangle.X + (rectangle.Width - ((Image)PageBmp).Width) / 2 + 1, rectangle.Y + (rectangle.Height - ((Image)PageBmp).Height) / 2 + 1, ((Image)PageBmp).Width, ((Image)PageBmp).Height);
					graphics.DrawImage((Image)(object)PageBmp, rectangle2);
				}
				finally
				{
					((IDisposable)val)?.Dispose();
				}
			}
		}
		Rectangle rectangle3 = StiScale.I(new Rectangle(4, 82, 152, 21));
		graphics.FillRectangle(Brushes.White, rectangle3);
		graphics.DrawRectangle(SystemPens.ControlDark, rectangle3);
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
			graphics.DrawString(text, ((Control)this).Font, Brushes.Black, (RectangleF)rectangle3, val2);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
	}

	protected override void WndProc(ref Message msg)
	{
		if (((Message)(ref msg)).Msg == 675)
		{
			((Control)this).Invalidate();
		}
		((UserControl)this).WndProc(ref msg);
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).Invalidate();
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).Invalidate();
	}

	protected override void OnMouseHover(EventArgs e)
	{
		((Control)this).Invalidate();
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		viewerControl.SetZoomMultiplePages(PageWidth, PageHeight);
		((PopupItem)viewerControl.tbZoomMultiplePages).ClosePopup();
		viewerControl.UpdateControlsState();
		viewerControl.SaveConfig();
	}

	private void InitializeComponent()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).SuspendLayout();
		((ContainerControl)this).AutoScaleDimensions = new SizeF(12f, 25f);
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
		((Control)this).BackColor = Color.White;
		((Control)this).Margin = new Padding(6, 6, 6, 6);
		((Control)this).Name = "StiMultiPagesUserControl";
		((Control)this).Size = new Size(322, 208);
		((Control)this).ResumeLayout(false);
	}
}
