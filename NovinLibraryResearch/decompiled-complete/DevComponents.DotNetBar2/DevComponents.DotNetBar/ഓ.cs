using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

internal class ഓ : Control
{
	private bool ഔ = true;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Control)this).CreateParams;
			createParams.ExStyle = createParams.ExStyle | 8 | (ഔ ? 524288 : 0) | 0x80;
			createParams.Style = -2046820352;
			createParams.Caption = "";
			return createParams;
		}
	}

	public ഓ(bool bAlphaShadow)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		ഔ = bAlphaShadow;
		((Control)this).Visible = false;
		((Control)this).SuspendLayout();
		if (!ഔ)
		{
			((Control)this).BackColor = SystemColors.ControlDark;
		}
		((Control)this).Size = new Size(12, 12);
		((Control)this).SetStyle((ControlStyles)1, false);
		((Control)this).SetStyle((ControlStyles)512, false);
		if (ഔ)
		{
			((Control)this).SetStyle((ControlStyles)8192, true);
			((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, false);
		}
		((Control)this).ResumeLayout();
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		((Control)this).OnHandleCreated(e);
	}

	private void ക()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Expected O, but got Unknown
		if (((Control)this).Width <= 0 || ((Control)this).Height <= 0)
		{
			return;
		}
		Bitmap val = new Bitmap(((Control)this).Width, ((Control)this).Height, (PixelFormat)2498570);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
				Region val3 = new Region();
				GraphicsPath val4 = new GraphicsPath();
				Rectangle clientRectangle = ((Control)this).ClientRectangle;
				val4.StartFigure();
				val4.AddLine(clientRectangle.Right - 5, clientRectangle.Top, clientRectangle.Right, clientRectangle.Top);
				val4.AddLine(clientRectangle.Right, clientRectangle.Top, clientRectangle.Right, clientRectangle.Top + 5);
				val4.AddLine(clientRectangle.Right, clientRectangle.Top + 5, clientRectangle.Right - 5, clientRectangle.Top);
				val4.CloseFigure();
				val4.StartFigure();
				val4.AddLine(clientRectangle.Left, clientRectangle.Bottom - 5 - 1, clientRectangle.Left, clientRectangle.Bottom);
				val4.AddLine(clientRectangle.Left, clientRectangle.Bottom, clientRectangle.Left + 5, clientRectangle.Bottom);
				val4.AddLine(clientRectangle.Left + 5, clientRectangle.Bottom, clientRectangle.Left, clientRectangle.Bottom - 5);
				val4.CloseFigure();
				val4.StartFigure();
				val4.AddRectangle(new Rectangle(0, 0, clientRectangle.Width - 5, clientRectangle.Height - 5));
				val4.CloseFigure();
				val3.Xor(val4);
				val2.SetClip(val3, (CombineMode)0);
				val4.Dispose();
				val4 = null;
				val3.Dispose();
				val3 = null;
				val2.Clear(Color.Transparent);
				val2.CompositingMode = (CompositingMode)1;
				val2.SmoothingMode = (SmoothingMode)4;
				val2.Clear(Color.Black);
				clientRectangle.Width--;
				clientRectangle.Height--;
				Color[] array = new Color[5]
				{
					Color.FromArgb(14, Color.Black),
					Color.FromArgb(43, Color.Black),
					Color.FromArgb(84, Color.Black),
					Color.FromArgb(113, Color.Black),
					Color.FromArgb(128, Color.Black)
				};
				for (int i = 0; i < 4; i++)
				{
					Pen val5 = new Pen(array[i], 1f);
					try
					{
						GraphicsPath val6 = ഗ(clientRectangle, 4 - i);
						try
						{
							val2.DrawPath(val5, val6);
						}
						finally
						{
							((IDisposable)val6)?.Dispose();
						}
						clientRectangle.Inflate(-1, -1);
					}
					finally
					{
						((IDisposable)val5)?.Dispose();
					}
				}
				IntPtr dC = _089F.GetDC(IntPtr.Zero);
				IntPtr intPtr = _0EFE.CreateCompatibleDC(dC);
				IntPtr hbitmap = val.GetHbitmap(Color.FromArgb(0));
				IntPtr hObject = _0EFE.SelectObject(intPtr, hbitmap);
				_089F.SIZE psize = default(_089F.SIZE);
				psize.cx = ((Control)this).Width;
				psize.cy = ((Control)this).Height;
				_089F.POINT pptDst = default(_089F.POINT);
				pptDst.x = ((Control)this).Left;
				pptDst.y = ((Control)this).Top;
				_089F.POINT pprSrc = default(_089F.POINT);
				pprSrc.x = 0;
				pprSrc.y = 0;
				_089F.BLENDFUNCTION pblend = default(_089F.BLENDFUNCTION);
				pblend.BlendOp = 0;
				pblend.BlendFlags = 0;
				pblend.SourceConstantAlpha = byte.MaxValue;
				pblend.AlphaFormat = 1;
				_089F.UpdateLayeredWindow(((Control)this).Handle, dC, ref pptDst, ref psize, intPtr, ref pprSrc, 0, ref pblend, 2);
				_0EFE.SelectObject(intPtr, hObject);
				_0EFE.ReleaseDC(IntPtr.Zero, dC);
				_0EFE.DeleteObject(hbitmap);
				_0EFE.DeleteDC(intPtr);
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

	public void ഖ()
	{
		if (ഔ)
		{
			ക();
		}
	}

	private GraphicsPath ഗ(Rectangle ल, int ഘ)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		val.AddArc(ल.Right - ഘ, ल.Y, ഘ, ഘ, 270f, 90f);
		val.AddLine(ल.Right, ल.Y + ഘ, ल.Right, ल.Bottom - ഘ);
		val.AddArc(ल.Right - ഘ, ल.Bottom - ഘ, ഘ, ഘ, 0f, 90f);
		val.AddLine(ल.Right - ഘ, ल.Bottom, ल.X, ल.Bottom);
		val.AddLine(ल.X, ल.Bottom, ल.X, ल.Top);
		return val;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		if (!ഔ)
		{
			e.Graphics.Clear(((Control)this).BackColor);
		}
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 33)
		{
			((Message)(ref m)).Result = new IntPtr(3);
		}
		else
		{
			((Control)this).WndProc(ref m);
		}
	}
}
