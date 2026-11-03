using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using FormCustom.Cache;
using PacketParser.Entities;

namespace FormCustom.ExtendedControls;

public class CustomeToggleButton : CheckBox
{
	private Color onBackColor = Color.FromArgb(65, 81, 220);

	private Color offBackColor = Color.Gray;

	private Color onToggleColor = Color.FromArgb(228, 236, 255);

	private Color offToggleColor = Color.DimGray;

	private bool solidStyle = true;

	public Color OnBackColor
	{
		get
		{
			return onBackColor;
		}
		set
		{
			onBackColor = value;
			((Control)this).Invalidate();
		}
	}

	public Color OffBackColor
	{
		get
		{
			return offBackColor;
		}
		set
		{
			offBackColor = value;
			((Control)this).Invalidate();
		}
	}

	public Color OnToggleColor
	{
		get
		{
			return onToggleColor;
		}
		set
		{
			onToggleColor = value;
			((Control)this).Invalidate();
		}
	}

	public Color OfToggleColor
	{
		get
		{
			return offToggleColor;
		}
		set
		{
			offToggleColor = value;
			((Control)this).Invalidate();
		}
	}

	[DefaultValue(true)]
	public bool SolidStyle
	{
		get
		{
			return solidStyle;
		}
		set
		{
			solidStyle = value;
			((Control)this).Invalidate();
		}
	}

	public CustomeToggleButton()
	{
		((Control)this).BackColor = Color.Transparent;
		((Control)this).AutoSize = false;
		((Control)this).Size = new Size(51, 24);
	}

	private GraphicsPath GetFigurePath()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		try
		{
			int num = ((Control)this).Height - 1;
			Rectangle rectangle = new Rectangle(0, 0, num, num);
			Rectangle rectangle2 = new Rectangle(((Control)this).Width - num - 2, 0, num, num);
			GraphicsPath val = new GraphicsPath();
			val.StartFigure();
			val.AddArc(rectangle, 90f, 180f);
			val.AddArc(rectangle2, 270f, 180f);
			val.CloseFigure();
			return val;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetFigurePath", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ExtendedControls\\CustomeToggleButton.cs", 90);
			throw;
		}
	}

	protected override void OnPaint(PaintEventArgs pevent)
	{
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Expected O, but got Unknown
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		try
		{
			int num = ((Control)this).Height - 5;
			pevent.Graphics.SmoothingMode = (SmoothingMode)4;
			Control parent = ((Control)this).Parent;
			if (parent is CacheBaseForm)
			{
				pevent.Graphics.Clear(parent.BackColor);
			}
			else
			{
				pevent.Graphics.Clear(Color.White);
			}
			if (((CheckBox)this).Checked)
			{
				if (solidStyle)
				{
					pevent.Graphics.FillPath((Brush)new SolidBrush(onBackColor), GetFigurePath());
				}
				else
				{
					pevent.Graphics.DrawPath(new Pen(onBackColor, 2f), GetFigurePath());
				}
				pevent.Graphics.FillEllipse((Brush)new SolidBrush(onToggleColor), new Rectangle(((Control)this).Width - ((Control)this).Height + 1, 2, num, num));
			}
			else
			{
				if (solidStyle)
				{
					pevent.Graphics.FillPath((Brush)new SolidBrush(offBackColor), GetFigurePath());
				}
				else
				{
					pevent.Graphics.DrawPath(new Pen(offBackColor, 2f), GetFigurePath());
				}
				pevent.Graphics.FillEllipse((Brush)new SolidBrush(offToggleColor), new Rectangle(2, 2, num, num));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "OnPaint", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\ExtendedControls\\CustomeToggleButton.cs", 122);
		}
	}
}
