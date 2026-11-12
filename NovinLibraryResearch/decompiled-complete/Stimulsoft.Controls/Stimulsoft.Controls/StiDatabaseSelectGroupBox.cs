using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StiGroupBox), "Toolbox.StiGroupBox.bmp")]
public class StiDatabaseSelectGroupBox : StiGroupBox
{
	public override int HeaderSize => (int)Math.Ceiling(24.0 * StiScaleUI.Y);

	public StiDatabaseSelectGroupBox()
	{
		base.AllowCollapse = true;
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Invalid comparison between Unknown and I4
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Invalid comparison between Unknown and I4
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = p.Graphics;
		SolidBrush val = new SolidBrush(Color.FromArgb(255, 238, 238, 238));
		try
		{
			graphics.FillRectangle((Brush)(object)val, 0, 0, ((Control)this).Width, (int)Math.Ceiling(24.0 * StiScaleUI.Y));
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		if (((Control)this).Text.Length > 0)
		{
			int num = (int)Math.Ceiling(24.0 * StiScaleUI.X);
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			Rectangle rectangle = clientRectangle;
			rectangle.Width = ((Control)this).Width - (int)Math.Ceiling(30.0 * StiScaleUI.X);
			rectangle.Height = (int)Math.Ceiling(24.0 * StiScaleUI.Y);
			if ((int)((Control)this).RightToLeft == 1)
			{
				rectangle.X = clientRectangle.Right - rectangle.Width - num;
			}
			else
			{
				rectangle.X += num;
			}
			Font val2 = new Font("Microsoft Sans Serif", 10f);
			try
			{
				SolidBrush val3 = new SolidBrush(Color.FromArgb(255, 136, 136, 136));
				try
				{
					StringFormat val4 = new StringFormat();
					try
					{
						val4.FormatFlags = (StringFormatFlags)4096;
						val4.Trimming = (StringTrimming)3;
						val4.Alignment = (StringAlignment)0;
						val4.LineAlignment = (StringAlignment)1;
						val4.HotkeyPrefix = (HotkeyPrefix)1;
						if ((int)((Control)this).RightToLeft == 1)
						{
							val4.FormatFlags = (StringFormatFlags)(val4.FormatFlags | 1);
						}
						graphics.DrawString(((Control)this).Text, val2, (Brush)(object)val3, (RectangleF)rectangle, val4);
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
		DrawCollapse(graphics);
	}

	protected override Rectangle GetCollapseRectangle()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Invalid comparison between Unknown and I4
		if ((int)((Control)this).RightToLeft != 1)
		{
			return new Rectangle((int)Math.Ceiling(7.0 * StiScaleUI.X), (int)Math.Ceiling(7.0 * StiScaleUI.Y), (int)Math.Ceiling(10.0 * StiScaleUI.X), (int)Math.Ceiling(10.0 * StiScaleUI.Y));
		}
		return new Rectangle(((Control)this).Width - (int)Math.Ceiling(17.0 * StiScaleUI.X), (int)Math.Ceiling(7.0 * StiScaleUI.Y), (int)Math.Ceiling(10.0 * StiScaleUI.X), (int)Math.Ceiling(10.0 * StiScaleUI.Y));
	}
}
