using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiHyperlinkToolTip : ToolTip
{
	private void OnPopup(object sender, PopupEventArgs e)
	{
		string toolTip = ((ToolTip)this).GetToolTip(e.AssociatedControl);
		e.ToolTipSize = MeasureSize(toolTip, e.AssociatedControl);
	}

	private void OnDraw(object sender, DrawToolTipEventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = e.Graphics;
		graphics.FillRectangle(Brushes.White, e.Bounds);
		SolidBrush val = new SolidBrush(Color.FromArgb(200, 200, 200));
		try
		{
			Pen val2 = new Pen((Brush)(object)val);
			try
			{
				graphics.DrawRectangle(val2, new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1));
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
		Font val3 = new Font(e.AssociatedControl.Font.Name, e.AssociatedControl.Font.SizeInPoints, (FontStyle)4);
		try
		{
			TextFormatFlags val4 = (TextFormatFlags)5;
			TextRenderer.DrawText((IDeviceContext)(object)graphics, e.ToolTipText, val3, e.Bounds, Color.FromArgb(6, 69, 173), val4);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
	}

	private Size MeasureSize(string str, Control control)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				Font val3 = new Font(control.Font.Name, control.Font.SizeInPoints, (FontStyle)4);
				try
				{
					Size result = TextRenderer.MeasureText((IDeviceContext)(object)val2, str, val3);
					result.Width += 10;
					result.Height += 10;
					return result;
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

	public StiHyperlinkToolTip()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		((ToolTip)this).OwnerDraw = true;
		((ToolTip)this).Draw += new DrawToolTipEventHandler(OnDraw);
		((ToolTip)this).Popup += new PopupEventHandler(OnPopup);
	}
}
