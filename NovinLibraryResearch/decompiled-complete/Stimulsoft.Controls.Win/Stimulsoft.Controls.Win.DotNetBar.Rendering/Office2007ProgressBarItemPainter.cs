using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

internal class Office2007ProgressBarItemPainter : ProgressBarItemPainter, IOffice2007Painter
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

	public override void Paint(ProgressBarItemRenderEventArgs e)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Expected O, but got Unknown
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		Rectangle displayRectangle = e.ProgressBarItem.DisplayRectangle;
		if (displayRectangle.Width <= 0 || displayRectangle.Height <= 0)
		{
			return;
		}
		ProgressBarItem progressBarItem = e.ProgressBarItem;
		Office2007ProgressBarColorTable office2007ProgressBarColorTable = m_ColorTable.ProgressBarItem;
		if (progressBarItem.ColorTable == eProgressBarItemColor.Paused)
		{
			office2007ProgressBarColorTable = m_ColorTable.ProgressBarItemPaused;
		}
		else if (progressBarItem.ColorTable == eProgressBarItemColor.Error)
		{
			office2007ProgressBarColorTable = m_ColorTable.ProgressBarItemError;
		}
		Graphics graphics = e.Graphics;
		SmoothingMode smoothingMode = graphics.SmoothingMode;
		graphics.SmoothingMode = (SmoothingMode)0;
		DisplayHelp.DrawRoundedRectangle(graphics, office2007ProgressBarColorTable.OuterBorder, displayRectangle, 2);
		displayRectangle.Inflate(-1, -1);
		Brush val = DisplayHelp.CreateBrush(displayRectangle, office2007ProgressBarColorTable.BackgroundColors);
		if (val != null)
		{
			graphics.FillRectangle(val, displayRectangle);
			val.Dispose();
		}
		graphics.SmoothingMode = smoothingMode;
		if (!office2007ProgressBarColorTable.InnerBorder.IsEmpty)
		{
			DisplayHelp.DrawRoundedRectangle(graphics, office2007ProgressBarColorTable.InnerBorder, displayRectangle, 2);
		}
		displayRectangle.Inflate(-1, -1);
		Region clip = graphics.Clip;
		try
		{
			graphics.SetClip(displayRectangle, (CombineMode)1);
			Rectangle rectangle = displayRectangle;
			if (progressBarItem.ProgressType == eProgressItemType.Marquee)
			{
				if (progressBarItem.Orientation == eOrientation.Horizontal)
				{
					rectangle.Width = (int)((double)rectangle.Width * 0.3);
					rectangle.X += displayRectangle.Width * progressBarItem.MarqueeValue / 100 - rectangle.Width / 2;
				}
				else if (progressBarItem.Orientation == eOrientation.Vertical)
				{
					rectangle.Height = (int)((double)rectangle.Height * 0.3);
					rectangle.Y += displayRectangle.Height * progressBarItem.MarqueeValue / 100 - rectangle.Height / 2;
				}
			}
			else if (progressBarItem.Orientation == eOrientation.Horizontal)
			{
				rectangle.Width = (int)((float)rectangle.Width * ((float)(progressBarItem.Value - progressBarItem.Minimum) / (float)(progressBarItem.Maximum - progressBarItem.Minimum)));
			}
			else if (progressBarItem.Orientation == eOrientation.Vertical)
			{
				int num = (int)((float)rectangle.Height * ((float)(progressBarItem.Value - progressBarItem.Minimum) / (float)(progressBarItem.Maximum - progressBarItem.Minimum)));
				rectangle.Y = rectangle.Bottom - num;
				rectangle.Height = num;
			}
			if (rectangle.Width <= 0 || rectangle.Height <= 0)
			{
				return;
			}
			val = DisplayHelp.CreateBrush(rectangle, office2007ProgressBarColorTable.Chunk);
			if (val != null)
			{
				graphics.SmoothingMode = (SmoothingMode)3;
				if (e.ProgressBarItem.IsRenderMode)
				{
					SolidBrush val2 = new SolidBrush(Color.FromArgb(160, 255, 255, 255));
					try
					{
						rectangle.Inflate(0, -StiScale2.I2);
						graphics.FillRectangle((Brush)(object)val2, rectangle);
					}
					finally
					{
						((IDisposable)val2)?.Dispose();
					}
				}
				else
				{
					graphics.FillRectangle(val, rectangle);
				}
				val.Dispose();
				graphics.SmoothingMode = smoothingMode;
			}
			GradientColorTable chunkOverlay = office2007ProgressBarColorTable.ChunkOverlay;
			if (progressBarItem.Orientation == eOrientation.Horizontal)
			{
				chunkOverlay.LinearGradientAngle = 90;
			}
			else
			{
				chunkOverlay.LinearGradientAngle = 0;
			}
			val = DisplayHelp.CreateBrush(rectangle, chunkOverlay);
			if (val != null)
			{
				graphics.FillRectangle(val, rectangle);
				if (progressBarItem.ProgressType == eProgressItemType.Marquee)
				{
					if (progressBarItem.Orientation == eOrientation.Horizontal && rectangle.Right > displayRectangle.Right + 4)
					{
						rectangle = new Rectangle(displayRectangle.X, displayRectangle.Y, rectangle.Right - displayRectangle.Right - 4, displayRectangle.Height);
					}
					else if (progressBarItem.Orientation == eOrientation.Vertical && rectangle.Bottom > displayRectangle.Bottom + 4)
					{
						rectangle = new Rectangle(displayRectangle.X, displayRectangle.Y, displayRectangle.Height, rectangle.Bottom - displayRectangle.Bottom - 4);
					}
					graphics.FillRectangle(val, rectangle);
				}
				val.Dispose();
			}
			if (progressBarItem.Orientation == eOrientation.Horizontal && rectangle.Right + 4 <= displayRectangle.Right)
			{
				rectangle.X = rectangle.Right;
				rectangle.Width = 4;
				val = DisplayHelp.CreateBrush(rectangle, office2007ProgressBarColorTable.ChunkShadow);
				if (val != null)
				{
					graphics.FillRectangle(val, rectangle);
					val.Dispose();
				}
			}
			else if (progressBarItem.Orientation == eOrientation.Vertical && rectangle.Y - 3 >= displayRectangle.Y)
			{
				rectangle.Y -= 3;
				rectangle.Height = 3;
				val = DisplayHelp.CreateBrush(rectangle, office2007ProgressBarColorTable.ChunkShadow.Colors, office2007ProgressBarColorTable.ChunkShadow.LinearGradientAngle - 90, office2007ProgressBarColorTable.ChunkShadow.GradientType);
				if (val != null)
				{
					graphics.FillRectangle(val, rectangle);
					val.Dispose();
				}
			}
		}
		finally
		{
			graphics.Clip = clip;
		}
	}
}
