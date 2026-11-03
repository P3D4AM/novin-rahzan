using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Dashboard.Styles;

public abstract class StiPivotElementStyle : StiElementStyle
{
	public abstract string LocalizedName { get; }

	public virtual Color CellBackColor { get; set; }

	public virtual Color CellForeColor { get; set; } = StiColor.Get("222");


	public virtual Color SelectedCellBackColor { get; set; }

	public virtual Color SelectedCellForeColor { get; set; } = StiColor.Get("222");


	public virtual Color AlternatingCellBackColor { get; set; }

	public virtual Color AlternatingCellForeColor { get; set; } = StiColor.Get("222");


	public virtual Color ColumnHeaderBackColor { get; set; }

	public virtual Color ColumnHeaderForeColor { get; set; } = Color.White;


	public virtual Color RowHeaderBackColor { get; set; }

	public virtual Color RowHeaderForeColor { get; set; } = Color.White;


	public virtual Color HotColumnHeaderBackColor { get; set; }

	public virtual Color HotRowHeaderBackColor { get; set; }

	public virtual Color LineColor { get; set; } = Color.Gainsboro;


	[Browsable(false)]
	public virtual Color BackColor { get; set; } = Color.White;


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
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Expected O, but got Unknown
		Rectangle rectangle = new Rectangle(rect.X + StiScale.XXI(15.0), rect.Y + StiScale.YYI(5.0), rect.Width - StiScale.XXI(30.0), rect.Height - StiScale.YYI(10.0));
		int num = rectangle.Width / 3;
		int num2 = rectangle.Height / 5;
		SolidBrush val = new SolidBrush(ColumnHeaderBackColor);
		try
		{
			SolidBrush val2 = new SolidBrush(RowHeaderBackColor);
			try
			{
				SolidBrush val3 = new SolidBrush(CellBackColor);
				try
				{
					SolidBrush val4 = new SolidBrush(AlternatingCellBackColor);
					try
					{
						SolidBrush val5 = new SolidBrush(LineColor);
						try
						{
							int num3 = rectangle.X;
							int num4 = rectangle.Y;
							for (int i = 0; i < 5; i++)
							{
								for (int j = 0; j < 3; j++)
								{
									Rectangle rectangle2 = new Rectangle(num3, num4, num, num2);
									if (i == 0 || j == 0)
									{
										g.FillRectangle((Brush)(object)((j == 0) ? val2 : val), rectangle2);
										g.FillRectangle((Brush)(object)val5, new Rectangle(rectangle2.Right - StiScale.XXI(1.0), rectangle2.Top, StiScale.XXI(1.0), rectangle2.Height));
										if (j == 0 && i > 0)
										{
											g.FillRectangle((Brush)(object)val5, new Rectangle(rectangle2.Left, rectangle2.Top, rectangle2.Width, StiScale.YYI(1.0)));
										}
									}
									else
									{
										SolidBrush val6 = ((i % 2 == 0) ? val4 : val3);
										g.FillRectangle((Brush)(object)val6, rectangle2);
										g.FillRectangle((Brush)(object)val5, new Rectangle(rectangle2.Right - StiScale.XXI(1.0), rectangle2.Top, StiScale.XXI(1.0), rectangle2.Height));
									}
									num3 += num;
								}
								num3 = rectangle.X;
								num4 += num2;
							}
							Pen val7 = new Pen((Brush)(object)val5, (float)(1.0 * StiScale.Factor));
							try
							{
								g.DrawRectangle(val7, new Rectangle(rectangle.X, rectangle.Y, num * 3 - StiScale.XXI(1.0), num2 * 5));
							}
							finally
							{
								((IDisposable)val7)?.Dispose();
							}
						}
						finally
						{
							((IDisposable)val5)?.Dispose();
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
