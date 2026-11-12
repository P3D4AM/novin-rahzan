using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Dashboard;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Dashboard.Viewer;

[ToolboxItem(false)]
public class StiElementPanel : StiShadowPanel
{
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsEmptyDashboard { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public bool IsShowWatermark { get; set; }

	protected override void OnPaint(PaintEventArgs e)
	{
		base.OnPaint(e);
		Graphics graphics = e.Graphics;
		StiDrawing.FillRectangle(graphics, ((Control)this).BackColor, 0.0, 0.0, ((Control)this).Width, ((Control)this).Height);
		PaintEmptyDashboard(graphics);
	}

	private void PaintEmptyDashboard(Graphics g)
	{
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		if (!IsEmptyDashboard || IsShowWatermark)
		{
			return;
		}
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		rectangle.Inflate(-StiScale.I10, -StiScale.I10);
		bool flag = rectangle.Width > StiScale.I(60) && rectangle.Height > StiScale.I(60);
		bool flag2 = rectangle.Width > StiScale.I(200) && rectangle.Height > StiScale.I(60);
		Bitmap notDefinedImage = GetNotDefinedImage();
		try
		{
			Font gdiFont = StiElementConsts.DragDrop.Font.GetGdiFont();
			try
			{
				float num = (float)((Image)notDefinedImage).Height + gdiFont.GetHeight() + (float)StiScaleUI.I4;
				RectangleF rectangleF = new RectangleD(rectangle.X, (float)rectangle.Y + ((float)rectangle.Height - num) / 2f, rectangle.Width, num).ToRectangleF();
				if (flag)
				{
					if (flag2)
					{
						g.DrawImage((Image)(object)notDefinedImage, rectangleF.X + (rectangleF.Width - (float)((Image)notDefinedImage).Width) / 2f, rectangleF.Y, (float)((Image)notDefinedImage).Width, (float)((Image)notDefinedImage).Height);
					}
					else
					{
						g.DrawImage((Image)(object)notDefinedImage, rectangleF.X + (rectangleF.Width - (float)((Image)notDefinedImage).Width) / 2f, rectangleF.Y + (rectangleF.Height - (float)((Image)notDefinedImage).Height) / 2f, (float)((Image)notDefinedImage).Width, (float)((Image)notDefinedImage).Height);
					}
				}
				if (flag2)
				{
					StringFormat val = new StringFormat();
					try
					{
						val.LineAlignment = (StringAlignment)2;
						val.Alignment = (StringAlignment)1;
						StiTextDrawing.DrawString(rect: new RectangleD(rectangleF.X, rectangleF.Y, rectangleF.Width, rectangleF.Height), g: g, text: Loc.Get("Dashboard", "EmptyDashboardHeader"), font: gdiFont, color: Color.Gray, stringFormat: val);
						return;
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
			finally
			{
				((IDisposable)gdiFont)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)notDefinedImage)?.Dispose();
		}
	}

	private Bitmap GetNotDefinedImage()
	{
		int num = ((!(StiScaleUI.Factor >= 3.0)) ? 1 : 2);
		string text = ((StiScaleUI.Factor >= 4.0) ? "_x2" : ((StiScaleUI.Factor >= 3.0) ? "_x1_5" : ((StiScaleUI.Factor >= 2.0) ? "_x2" : ((!(StiScaleUI.Factor >= 1.5)) ? "" : "_x1_5"))));
		Bitmap val = StiImageUtils.GetImage("Stimulsoft.Dashboard.Viewer", "Stimulsoft.Dashboard.Viewer.Images.NoResult.Dashboard" + text + ".png");
		if (num != 1)
		{
			Image image = StiImageResizer.GetImage((Image)(object)val, allowDispose: true, num);
			val = (Bitmap)(object)((image is Bitmap) ? image : null);
		}
		return val;
	}

	public StiElementPanel()
	{
		((Control)this).SetStyle((ControlStyles)131072, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
	}
}
