using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Dashboard.Styles;

public abstract class StiTableElementStyle : StiElementStyle, IStiCellIndicatorStyle
{
	public abstract string LocalizedName { get; }

	public virtual Color CellBackColor { get; set; }

	public virtual Color CellForeColor { get; set; } = StiColor.Get("222");


	public virtual Color SelectedCellBackColor { get; set; } = StiColor.Get("3498db");


	public virtual Color SelectedCellForeColor { get; set; } = Color.White;


	public virtual Color AlternatingCellBackColor { get; set; }

	public virtual Color AlternatingCellForeColor { get; set; } = StiColor.Get("222");


	public virtual Color HeaderBackColor { get; set; }

	public virtual Color HeaderForeColor { get; set; } = Color.White;


	public virtual Color HotHeaderBackColor { get; set; }

	public virtual Color LineColor { get; set; } = Color.Gainsboro;


	[Browsable(false)]
	public virtual Color FooterBackColor { get; set; } = Color.White;


	[Browsable(false)]
	public virtual Color FooterForeColor { get; set; } = Color.Black;


	[Browsable(false)]
	public virtual Color BackColor { get; set; } = Color.White;


	[Browsable(false)]
	public virtual Color CellDataBarsOverlapped { get; set; } = Color.FromArgb(255, 51, 94, 150);


	[Browsable(false)]
	public virtual Color CellDataBarsPositive { get; set; } = Color.FromArgb(255, 99, 142, 198);


	[Browsable(false)]
	public virtual Color CellDataBarsNegative { get; set; } = Color.FromArgb(255, 255, 0, 0);


	[Browsable(false)]
	public virtual Color CellWinLossPositive { get; set; } = Color.FromArgb(255, 99, 142, 198);


	[Browsable(false)]
	public virtual Color CellWinLossNegative { get; set; } = Color.FromArgb(255, 255, 0, 0);


	[Browsable(false)]
	public virtual Color CellSparkline { get; set; } = Color.FromArgb(255, 83, 126, 182);


	[Browsable(false)]
	public virtual Color CellIndicatorPositive { get; set; } = Color.Green;


	[Browsable(false)]
	public virtual Color CellIndicatorNegative { get; set; } = Color.Red;


	[Browsable(false)]
	public virtual Color CellIndicatorNeutral { get; set; } = Color.LightGray;


	public void DrawStyleForGallery(Graphics g, Rectangle rect)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Expected O, but got Unknown
		Rectangle rectangle = new Rectangle(rect.X + StiScale.XXI(15.0), rect.Y + StiScale.YYI(5.0), rect.Width - StiScale.XXI(30.0), rect.Height - StiScale.YYI(10.0));
		int num = rectangle.Width / 3;
		int num2 = rectangle.Height / 5;
		SolidBrush val = new SolidBrush(HeaderBackColor);
		try
		{
			SolidBrush val2 = new SolidBrush(CellBackColor);
			try
			{
				SolidBrush val3 = new SolidBrush(AlternatingCellBackColor);
				try
				{
					SolidBrush val4 = new SolidBrush(LineColor);
					try
					{
						int num3 = rectangle.X;
						int num4 = rectangle.Y;
						for (int i = 0; i < 5; i++)
						{
							for (int j = 0; j < 3; j++)
							{
								Rectangle rectangle2 = new Rectangle(num3, num4, num, num2);
								if (i == 0)
								{
									g.FillRectangle((Brush)(object)val, rectangle2);
									g.FillRectangle((Brush)(object)val4, new Rectangle(rectangle2.Right - StiScale.XXI(1.0), rectangle2.Top, StiScale.XXI(1.0), rectangle2.Height));
								}
								else
								{
									SolidBrush val5 = ((i % 2 == 0) ? val3 : val2);
									g.FillRectangle((Brush)(object)val5, rectangle2);
									g.FillRectangle((Brush)(object)val4, new Rectangle(rectangle2.Right - StiScale.XXI(1.0), rectangle2.Top, StiScale.XXI(1.0), rectangle2.Height));
								}
								num3 += num;
							}
							num3 = rectangle.X;
							num4 += num2;
						}
						Pen val6 = new Pen((Brush)(object)val4, (float)(1.0 * StiScale.Factor));
						try
						{
							g.DrawRectangle(val6, new Rectangle(rectangle.X, rectangle.Y, num * 3 - StiScale.XXI(1.0), num2 * 5));
						}
						finally
						{
							((IDisposable)val6)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}
}
