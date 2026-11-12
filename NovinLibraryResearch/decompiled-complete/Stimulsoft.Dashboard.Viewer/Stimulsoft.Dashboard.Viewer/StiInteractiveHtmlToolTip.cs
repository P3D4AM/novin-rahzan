using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Controls.Win.DotNetBar;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Report;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Dashboard.Viewer;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class StiInteractiveHtmlToolTip : Control
{
	private class StiToolTipObject : IStiHorAlignment, IStiVertAlignment, IStiFont
	{
		public Font Font { get; set; } = new Font("Arial", 12f);


		public StiHorAlignment HorAlignment { get; set; } = StiHorAlignment.Left;


		public StiVertAlignment VertAlignment { get; set; } = StiVertAlignment.Center;

	}

	private Timer timerHiding;

	private Timer timerShowing;

	private StiToolTipObject toolTipObject = new StiToolTipObject();

	public Rectangle referenceRectangle = Rectangle.Empty;

	private PopupShadow popupShadow;

	private const long WS_POPUP = 2147483648L;

	private const long WS_CLIPSIBLINGS = 67108864L;

	private const long WS_CLIPCHILDREN = 33554432L;

	private const long WS_EX_TOOLWINDOW = 128L;

	private const long WS_EX_TOPMOST = 8L;

	private bool IsMouseOver { get; set; }

	private int MarginDistance => StiScale.I10;

	public string HtmlText
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = value;
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Control)this).CreateParams;
			createParams.Style = -2046820352;
			createParams.ExStyle = 136;
			createParams.Caption = "";
			return createParams;
		}
	}

	public bool Active => ((Control)this).Visible;

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		string hyperlinkUnderMouse = GetHyperlinkUnderMouse(e.Location);
		((Control)this).Cursor = ((hyperlinkUnderMouse != null) ? Cursors.Hand : Cursors.Default);
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		((Control)this).Cursor = Cursors.Default;
		IsMouseOver = false;
		Hide((Control)(object)this);
	}

	protected override void OnClick(EventArgs e)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		((Control)this).OnClick(e);
		Point mousePosition = ((Control)this).PointToClient(Cursor.Position);
		string hyperlinkUnderMouse = GetHyperlinkUnderMouse(mousePosition);
		if (hyperlinkUnderMouse != null)
		{
			try
			{
				if (!StiOptions.Engine.GlobalEvents.InvokeHyperlinkClick(this, hyperlinkUnderMouse))
				{
					StiProcess.Start(hyperlinkUnderMouse);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		HideElements();
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Expected O, but got Unknown
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		SolidBrush val = new SolidBrush(Color.White);
		try
		{
			graphics.FillRectangle((Brush)(object)val, clientRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		clientRectangle.Inflate(-MarginDistance, -MarginDistance);
		StiAutoSizeHtmlTextHelper.Draw(graphics, RectangleD.CreateFromRectangle(clientRectangle), ((Control)this).Text, toolTipObject, 1.0, Color.Black, wordWrap: false, (StringTrimming)0);
		SolidBrush val2 = new SolidBrush(Color.FromArgb(200, 200, 200));
		try
		{
			Pen val3 = new Pen((Brush)(object)val2, 1f);
			try
			{
				graphics.DrawRectangle(val3, new Rectangle(((Control)this).ClientRectangle.X, ((Control)this).ClientRectangle.Y, ((Control)this).ClientRectangle.Width - 1, ((Control)this).ClientRectangle.Height - 1));
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

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		IsMouseOver = true;
	}

	protected override void OnLocationChanged(EventArgs e)
	{
		((Control)this).OnLocationChanged(e);
		if (popupShadow != null)
		{
			NativeFunctions.SetWindowPos(((Control)popupShadow).Handle, new IntPtr(0), ((Control)this).Left + 5, ((Control)this).Top + 5, 0, 0, 81);
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		if (!((Control)this).Visible && popupShadow != null)
		{
			((Control)popupShadow).Hide();
			((Component)(object)popupShadow).Dispose();
			popupShadow = null;
		}
		((Control)this).OnVisibleChanged(e);
	}

	private static Graphics GetTempGraphics()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		if (StiOptions.Engine.FullTrust && StiOptions.Export.Pdf.AllowImportSystemLibraries)
		{
			return Graphics.FromHwnd(IntPtr.Zero);
		}
		Bitmap val = new Bitmap(1, 1);
		return Graphics.FromImage((Image)(object)val);
	}

	public string GetHyperlinkUnderMouse(Point mousePosition)
	{
		Graphics tempGraphics = GetTempGraphics();
		RectangleD rect = RectangleD.CreateFromRectangle(((Control)this).ClientRectangle);
		rect.Inflate(-MarginDistance, -MarginDistance);
		SizeD sizeD = StiAutoSizeHtmlTextHelper.Measure(tempGraphics, rect, ((Control)this).Text, null);
		double val = ((sizeD.Width > rect.Width) ? (rect.Width / sizeD.Width) : 1.0);
		double val2 = ((sizeD.Height > rect.Height) ? (rect.Height / sizeD.Height) : 1.0);
		double num = Math.Min(val, val2);
		if (num < 1.0)
		{
			rect = new RectangleD(rect.X + (rect.Width - sizeD.Width * num) / 2.0, rect.Y + (rect.Height - sizeD.Height * num) / 2.0, sizeD.Width * num, sizeD.Height * num);
		}
		return StiAutoSizeHtmlTextHelper.GetHyperlinkUnderMouse(tempGraphics, rect, ((Control)this).Text, null, mousePosition, num);
	}

	private Size MeasureSize(string text)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		if (string.IsNullOrEmpty(text))
		{
			return Size.Empty;
		}
		Bitmap val = new Bitmap(1, 1);
		try
		{
			Graphics val2 = Graphics.FromImage((Image)(object)val);
			try
			{
				Rectangle rect = new Rectangle(0, 0, StiScale.XXI(200.0), 10000);
				Size result = StiAutoSizeHtmlTextHelper.Measure(val2, RectangleD.CreateFromRectangle(rect), text, toolTipObject, 1.0).ToSize();
				result.Width += MarginDistance * 2;
				result.Height += MarginDistance * 2;
				return result;
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

	public void Hide(Control control)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		if (IsMouseOver)
		{
			return;
		}
		if (timerHiding == null)
		{
			timerHiding = new Timer();
			timerHiding.Interval = 500;
			timerHiding.Tick += delegate
			{
				timerHiding.Stop();
				if (!IsMouseOver)
				{
					HideElements();
					((Control)this).Text = "";
				}
			};
		}
		timerHiding.Start();
	}

	private void HideElements()
	{
		((Control)this).Hide();
		PopupShadow obj = popupShadow;
		if (obj != null)
		{
			((Control)obj).Hide();
		}
	}

	public void SetHtmlToolTip(Control control, string text)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		((Control)this).Text = text;
		if (timerShowing == null)
		{
			timerShowing = new Timer();
			timerShowing.Interval = 300;
			timerShowing.Tick += delegate
			{
				timerShowing.Stop();
				if (IsMoouseOverControl(control))
				{
					ShowToolTip();
				}
			};
		}
		timerShowing.Start();
	}

	private bool IsMoouseOverControl(Control control)
	{
		return control.ClientRectangle.Contains(control.PointToClient(Control.MousePosition));
	}

	public void ShowToolTip()
	{
		if (!((Control)this).IsHandleCreated)
		{
			((Control)this).CreateControl();
		}
		Size size = MeasureSize(((Control)this).Text);
		Rectangle rectangle = new Rectangle(Control.MousePosition.X, Control.MousePosition.Y, size.Width, size.Height);
		rectangle.Inflate(StiScale.I2, StiScale.I2);
		rectangle.Offset(StiScale.I12, StiScale.I(24));
		ScreenInformation screenInformation = BarFunctions.ScreenFromPoint(Control.MousePosition);
		if (screenInformation != null)
		{
			Size size2 = screenInformation.WorkingArea.Size;
			size2.Width -= (int)((float)size2.Width * 0.2f);
			if (rectangle.Right > screenInformation.WorkingArea.Right)
			{
				rectangle.X = rectangle.Left - (rectangle.Right - screenInformation.WorkingArea.Right);
			}
			if (rectangle.Bottom > screenInformation.Bounds.Bottom)
			{
				if (referenceRectangle.IsEmpty)
				{
					rectangle.Y = Control.MousePosition.Y - rectangle.Height;
				}
				else
				{
					rectangle.Y = referenceRectangle.Y - rectangle.Height - 1;
				}
			}
			if (rectangle.Contains(Control.MousePosition.X, Control.MousePosition.Y))
			{
				if (rectangle.Height + Control.MousePosition.Y + 1 <= screenInformation.WorkingArea.Height && (referenceRectangle.IsEmpty || !referenceRectangle.IntersectsWith(new Rectangle(rectangle.X, Control.MousePosition.Y + 1, rectangle.Width, rectangle.Height))))
				{
					rectangle.Y = Control.MousePosition.Y + 1;
				}
				else
				{
					rectangle.Y = Control.MousePosition.Y - rectangle.Height - 1;
				}
			}
		}
		((Control)this).Location = rectangle.Location;
		((Control)this).ClientSize = rectangle.Size;
		if (NativeFunctions.ShowDropShadow)
		{
			if (popupShadow == null)
			{
				popupShadow = new PopupShadow(NativeFunctions.AlphaBlendingSupported);
				((Control)popupShadow).CreateControl();
			}
			((Control)popupShadow).Hide();
		}
		NativeFunctions.SetWindowPos(((Control)this).Handle, new IntPtr(0), 0, 0, 0, 0, 83);
		if (popupShadow != null)
		{
			NativeFunctions.SetWindowPos(((Control)popupShadow).Handle, ((Control)this).Handle, rectangle.Left + 5, rectangle.Top + 5, rectangle.Width - 2, rectangle.Height - 2, 80);
			popupShadow.UpdateShadow();
		}
	}

	public StiInteractiveHtmlToolTip()
	{
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).Font = SystemFonts.DefaultFont;
		((Control)this).BackColor = SystemColors.Control;
		((Control)this).ForeColor = SystemColors.ControlText;
	}
}
