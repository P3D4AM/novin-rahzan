using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Painters;
using Stimulsoft.Report.Units;

namespace Stimulsoft.Report.Viewer;

[ToolboxItem(false)]
public class StiThumbsControl : Panel
{
	public StiViewerControl viewerControl;

	private int selectedIndex;

	public int SelectedIndex
	{
		get
		{
			return selectedIndex;
		}
		set
		{
			if (selectedIndex != value)
			{
				selectedIndex = value;
				ScrollToSelectedIndex();
				((Control)this).Invalidate();
			}
		}
	}

	protected override void OnMouseWheel(MouseEventArgs e)
	{
		((ScrollableControl)this).OnMouseWheel(e);
		if (e.Delta < 0)
		{
			if (viewerControl.CurrentReport != null && SelectedIndex < viewerControl.CurrentReport.RenderedPages.Count - 1)
			{
				SelectedIndex++;
			}
		}
		else if (SelectedIndex >= 1)
		{
			SelectedIndex--;
		}
	}

	protected override void OnMouseDown(MouseEventArgs e)
	{
		((Control)this).OnMouseDown(e);
		if (viewerControl.CurrentReport == null)
		{
			return;
		}
		for (int i = 0; i < viewerControl.CurrentReport.RenderedPages.Count; i++)
		{
			if (GetRectangleFromIndex(i).Contains(e.X, e.Y))
			{
				SelectedIndex = i;
				viewerControl.CurrentPrintPage = i;
				viewerControl.ScrollToCurPage(fullSize: true);
				((Control)viewerControl.View).Invalidate();
				break;
			}
		}
	}

	protected override void OnClick(EventArgs e)
	{
		((Control)this).OnClick(e);
		((Control)this).Focus();
	}

	protected override void OnPaintBackground(PaintEventArgs e)
	{
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Expected O, but got Unknown
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Expected O, but got Unknown
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Expected O, but got Unknown
		//IL_0545: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Brush val = (Brush)new SolidBrush(((Control)this).BackColor);
		try
		{
			graphics.FillRectangle(val, e.ClipRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (viewerControl == null || viewerControl.CurrentReport == null)
		{
			return;
		}
		int num = 0;
		foreach (StiPage renderedPage in viewerControl.CurrentReport.RenderedPages)
		{
			if (renderedPage.Report == null)
			{
				continue;
			}
			Rectangle rectangleFromIndex = GetRectangleFromIndex(num);
			if (rectangleFromIndex.Bottom > 0 && rectangleFromIndex.Top < ((Control)this).ClientRectangle.Bottom)
			{
				Rectangle rectangle = rectangleFromIndex;
				rectangle.Inflate(-StiScale.I4, -StiScale.I4);
				if (num == SelectedIndex)
				{
					Brush val2 = (Brush)new SolidBrush(Color.FromArgb(255, 194, 213, 242));
					try
					{
						graphics.FillRectangle(val2, rectangle);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				rectangle.Inflate(-StiScale.I3, -StiScale.I3);
				rectangle.Height -= StiScale.YYI(10.0);
				Rectangle empty = Rectangle.Empty;
				StiUnit unit = renderedPage.Unit;
				RectangleD displayRectangle = renderedPage.DisplayRectangle;
				double num2 = unit.ConvertToHInches(((RectangleD)(ref displayRectangle)).Width);
				StiUnit unit2 = renderedPage.Unit;
				displayRectangle = renderedPage.DisplayRectangle;
				double num3 = unit2.ConvertToHInches(((RectangleD)(ref displayRectangle)).Height);
				if (num2 > num3)
				{
					double num4 = num2 / num3;
					int num5 = rectangle.Width - StiScale.I4;
					int num6 = (int)((double)num5 / num4);
					empty = new Rectangle(rectangle.X + StiScale.I2, rectangle.Y + (rectangle.Height - num6) / 2, num5, num6);
				}
				else
				{
					double num7 = num3 / (double)(float)num2;
					int num8 = rectangle.Height - StiScale.I4;
					int num9 = (int)((double)num8 / num7);
					empty = new Rectangle(rectangle.X + (rectangle.Width - num9) / 2, rectangle.Y + StiScale.I2, num9, num8);
				}
				if (empty.Width < StiScale.I2)
				{
					empty.Width = StiScale.I2;
				}
				if (empty.Height < StiScale.I2)
				{
					empty.Height = StiScale.I2;
				}
				if (empty.Width > StiScale.XXI(2500.0))
				{
					empty.Width = StiScale.XXI(2500.0);
				}
				if (empty.Height > StiScale.YYI(2000.0))
				{
					empty.Height = StiScale.YYI(2000.0);
				}
				rectangle.Height += StiScale.YYI(10.0);
				if (viewerControl == null)
				{
					ShadowPaintInfo val3 = new ShadowPaintInfo();
					val3.Graphics = e.Graphics;
					val3.Rectangle = empty;
					val3.Size = StiScale.I4;
					ShadowPainter.Paint(val3);
				}
				Bitmap val4 = new Bitmap(empty.Width, empty.Height);
				try
				{
					Graphics val5 = Graphics.FromImage((Image)(object)val4);
					try
					{
						val5.Clear(Color.White);
						if (renderedPage.Report != null && renderedPage.Report.RenderedPages.CacheMode)
						{
							renderedPage.Report.RenderedPages.GetPage(renderedPage);
						}
						double zoom = renderedPage.Report.Info.Zoom;
						double num10 = 0.0;
						if (num2 > num3)
						{
							double num11 = empty.Width;
							StiUnit unit3 = renderedPage.Unit;
							displayRectangle = renderedPage.DisplayRectangle;
							double val6 = unit3.ConvertToHInches(((RectangleD)(ref displayRectangle)).Width);
							StiUnit unit4 = renderedPage.Unit;
							displayRectangle = renderedPage.DisplayRectangle;
							num10 = num11 / Math.Max(val6, unit4.ConvertToHInches(((RectangleD)(ref displayRectangle)).Height));
						}
						else
						{
							double num12 = empty.Width;
							StiUnit unit5 = renderedPage.Unit;
							displayRectangle = renderedPage.DisplayRectangle;
							double val7 = unit5.ConvertToHInches(((RectangleD)(ref displayRectangle)).Width);
							StiUnit unit6 = renderedPage.Unit;
							displayRectangle = renderedPage.DisplayRectangle;
							num10 = num12 / Math.Min(val7, unit6.ConvertToHInches(((RectangleD)(ref displayRectangle)).Height));
						}
						num10 *= 1.01;
						num10 /= StiScale.Factor;
						bool showInteractive = renderedPage.Report.Info.ShowInteractive;
						renderedPage.Report.Info.ShowInteractive = false;
						renderedPage.Report.Info.Zoom = num10;
						StiTextGdiPainter.IsThumbnailsMode = true;
						bool denyDrawSegmentMode = renderedPage.DenyDrawSegmentMode;
						renderedPage.DenyDrawSegmentMode = true;
						renderedPage.LockHighlight = true;
						renderedPage.Paint(val5);
						renderedPage.LockHighlight = false;
						renderedPage.DenyDrawSegmentMode = denyDrawSegmentMode;
						StiTextGdiPainter.IsThumbnailsMode = false;
						renderedPage.Report.Info.Zoom = zoom;
						renderedPage.Report.Info.ShowInteractive = showInteractive;
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
					graphics.DrawImage((Image)(object)val4, empty.X, empty.Y, empty.Width, empty.Height);
					if (viewerControl != null)
					{
						Pen val8 = new Pen(Color.FromArgb(255, 119, 119, 119));
						try
						{
							graphics.DrawRectangle(val8, empty);
						}
						finally
						{
							((IDisposable)val8)?.Dispose();
						}
					}
				}
				finally
				{
					((IDisposable)val4)?.Dispose();
				}
				StringFormat val9 = new StringFormat();
				try
				{
					Font val10 = new Font("Arial", 8f, (FontStyle)1);
					try
					{
						val9.Alignment = (StringAlignment)1;
						val9.LineAlignment = (StringAlignment)1;
						Rectangle rectangle2 = new Rectangle(rectangle.X, rectangle.Bottom - StiScale.YYI(12.0), rectangle.Width, StiScale.YYI(16.0));
						int startPageNumber = 0;
						int endPageNumber = 0;
						viewerControl.CurrentReport.GetPageNumber(num, out startPageNumber, out endPageNumber);
						string text = ((startPageNumber != endPageNumber) ? (startPageNumber + ".." + endPageNumber) : startPageNumber.ToString());
						graphics.DrawString(text, val10, Brushes.White, (RectangleF)rectangle2, val9);
					}
					finally
					{
						((IDisposable)val10)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val9)?.Dispose();
				}
			}
			num++;
		}
	}

	public override bool PreProcessMessage(ref Message m)
	{
		if (((Message)(ref m)).Msg == 256)
		{
			if (((Message)(ref m)).WParam == (IntPtr)38 || ((Message)(ref m)).WParam == (IntPtr)37)
			{
				if (SelectedIndex >= 1)
				{
					SelectedIndex--;
				}
				return false;
			}
			if (((Message)(ref m)).WParam == (IntPtr)40 || ((Message)(ref m)).WParam == (IntPtr)39)
			{
				if (viewerControl.CurrentReport != null && SelectedIndex < viewerControl.CurrentReport.RenderedPages.Count - 1)
				{
					SelectedIndex++;
				}
				return false;
			}
			if (((Message)(ref m)).WParam == (IntPtr)36)
			{
				SelectedIndex = 0;
				return false;
			}
			if (((Message)(ref m)).WParam == (IntPtr)35)
			{
				if (viewerControl.CurrentReport != null && SelectedIndex < viewerControl.CurrentReport.RenderedPages.Count - 1)
				{
					SelectedIndex = viewerControl.CurrentReport.RenderedPages.Count - 1;
				}
				return false;
			}
		}
		return ((Control)this).PreProcessMessage(ref m);
	}

	private Rectangle GetRectangleFromIndex(int index)
	{
		Size thumbsItemSize = viewerControl.GetThumbsItemSize();
		int width = thumbsItemSize.Width;
		int height = thumbsItemSize.Height;
		int y = -((ScrollBar)viewerControl.scrollBarThumbs).Value + index * height;
		return new Rectangle(0, y, width, height);
	}

	internal void ScrollToSelectedIndex()
	{
		try
		{
			if (!((Control)viewerControl.scrollBarThumbs).Enabled)
			{
				return;
			}
			Rectangle rectangleFromIndex = GetRectangleFromIndex(SelectedIndex);
			if (((Control)this).ClientRectangle.Bottom < rectangleFromIndex.Bottom)
			{
				int num = rectangleFromIndex.Bottom - ((Control)this).ClientRectangle.Bottom + 10;
				if (((ScrollBar)viewerControl.scrollBarThumbs).Value + num < ((ScrollBar)viewerControl.scrollBarThumbs).Maximum)
				{
					StiVScrollBar scrollBarThumbs = viewerControl.scrollBarThumbs;
					((ScrollBar)scrollBarThumbs).Value = ((ScrollBar)scrollBarThumbs).Value + num;
				}
				else
				{
					((ScrollBar)viewerControl.scrollBarThumbs).Value = ((ScrollBar)viewerControl.scrollBarThumbs).Maximum - ((ScrollBar)viewerControl.scrollBarThumbs).LargeChange;
				}
			}
			if (((Control)this).ClientRectangle.Top > rectangleFromIndex.Top)
			{
				int top = rectangleFromIndex.Top;
				if (((ScrollBar)viewerControl.scrollBarThumbs).Value + top < ((ScrollBar)viewerControl.scrollBarThumbs).Maximum)
				{
					StiVScrollBar scrollBarThumbs2 = viewerControl.scrollBarThumbs;
					((ScrollBar)scrollBarThumbs2).Value = ((ScrollBar)scrollBarThumbs2).Value + top;
				}
				else
				{
					((ScrollBar)viewerControl.scrollBarThumbs).Value = ((ScrollBar)viewerControl.scrollBarThumbs).Maximum - ((ScrollBar)viewerControl.scrollBarThumbs).LargeChange;
				}
			}
		}
		catch
		{
		}
	}

	public StiThumbsControl()
	{
		((Panel)this).BorderStyle = (BorderStyle)0;
		((Control)this).SetStyle((ControlStyles)512, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
		((Panel)this).TabStop = true;
	}
}
