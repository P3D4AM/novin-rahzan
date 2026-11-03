using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dialogs;
using Stimulsoft.Report.Events;

namespace Stimulsoft.Report.Painters;

public class StiReportControlGdiPainter : StiContainerGdiPainter
{
	public void DrawControl(StiReportControl component, Graphics g, Control control, RectangleD rect)
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		if (component.Page == null || component.Report == null || control == null)
		{
			return;
		}
		Rectangle rectangle = ((RectangleD)(ref rect)).ToRectangle();
		if (rectangle.Width <= 0 || rectangle.Height <= 0)
		{
			return;
		}
		control.Left = 0;
		control.Top = 0;
		control.Width = rectangle.Width;
		control.Height = rectangle.Height;
		control.ForeColor = component.ForeColor;
		control.BackColor = component.BackColor;
		control.RightToLeft = component.RightToLeft;
		control.Enabled = component.Enabled;
		control.Font = component.Font;
		Bitmap val = new Bitmap(rectangle.Width, rectangle.Height);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			val2.Clear(SystemColors.Control);
			if (control is Button)
			{
				control.DrawToBitmap(val, new Rectangle(0, 0, rectangle.Width, rectangle.Height));
			}
			else
			{
				Type type = ((object)control).GetType();
				MethodInfo method = type.GetMethod("GetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
				object[] parameters = new object[1] { (object)(ControlStyles)65536 };
				bool flag = (bool)method.Invoke(control, parameters);
				MethodInfo method2 = type.GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic);
				parameters = new object[2]
				{
					(object)(ControlStyles)65536,
					false
				};
				method2.Invoke(control, parameters);
				IntPtr hdc = val2.GetHdc();
				Message val3 = Message.Create(control.Handle, 15, hdc, IntPtr.Zero);
				MethodInfo method3 = type.GetMethod("WndProc", BindingFlags.Instance | BindingFlags.NonPublic);
				parameters = new object[1] { val3 };
				method3.Invoke(control, parameters);
				val2.ReleaseHdc(hdc);
				parameters = new object[2]
				{
					(object)(ControlStyles)65536,
					flag
				};
				method2.Invoke(control, parameters);
			}
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		if (val != null)
		{
			GraphicsState val4 = g.Save();
			g.SetClip(rectangle);
			g.DrawImage((Image)(object)val, rectangle);
			g.Restore(val4);
			((Image)val).Dispose();
			val = null;
		}
	}

	public override void Paint(StiComponent component, StiPaintEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Expected O, but got Unknown
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Expected O, but got Unknown
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		if (!(e.Context is Graphics))
		{
			throw new Exception("StiGdiPainter can work only with System.Drawing.Graphics context!");
		}
		StiReportControl stiReportControl = component as StiReportControl;
		Graphics graphics = e.Graphics;
		RectangleD paintRectangle = stiReportControl.GetPaintRectangle();
		if (((RectangleD)(ref paintRectangle)).Width > 0.0 && ((RectangleD)(ref paintRectangle)).Height > 0.0)
		{
			RectangleD clipRectangle = e.ClipRectangle;
			if ((((RectangleD)(ref clipRectangle)).IsEmpty || ((RectangleD)(ref paintRectangle)).IntersectsWith(e.ClipRectangle)) && stiReportControl.IsDesigning)
			{
				if (stiReportControl.Report.Info.FillContainer)
				{
					Color baseColor = StiColorUtils.Dark(Color.White, (byte)40);
					baseColor = Color.FromArgb(100, baseColor);
					StiDrawing.FillRectangle(graphics, baseColor, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
				}
				StiDrawing.DrawRectangle(graphics, Pens.Black, ((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height);
				StringFormat val = new StringFormat();
				try
				{
					Font val2 = new Font("Arial", (float)(15.0 * stiReportControl.Page.Zoom));
					try
					{
						Brush val3 = (Brush)new SolidBrush(Color.Gray);
						try
						{
							val.LineAlignment = (StringAlignment)1;
							val.Alignment = (StringAlignment)1;
							StiTextDrawing.DrawString(graphics, stiReportControl.Name, val2, val3, new RectangleD(((RectangleD)(ref paintRectangle)).Left, ((RectangleD)(ref paintRectangle)).Top, ((RectangleD)(ref paintRectangle)).Width, ((RectangleD)(ref paintRectangle)).Height), val);
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
				PaintMarkers(component, graphics, paintRectangle);
			}
		}
		PaintComponents(stiReportControl, e);
	}
}
