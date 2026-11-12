using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Design.Ribbon.Styles;

[ToolboxItem(false)]
internal sealed class StiCollectionNameStyleButton : Control
{
	internal static Color SelectedColor = Color.FromArgb(255, 43, 86, 154);

	internal static Color MouseOverColor = Color.FromArgb(255, 224, 236, 255);

	internal static Color StrokeColor = Color.FromArgb(255, 198, 198, 198);

	private string collectionName;

	private static Image bitmap;

	private bool isMouseOver;

	public bool IsSelected { get; set; }

	public StiCollectionNameStyleButton(string collectionName)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		this.collectionName = collectionName;
		if (bitmap == null)
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(StiRibbonBackStageUserControl));
			bitmap = (Image)componentResourceManager.GetObject("pageOpen1.Image");
			if (StiScale.Id != 0)
			{
				bitmap = (Image)(object)StiImageUtils.ResizeImage(bitmap, StiScale.XXI(bitmap.Width), StiScale.YYI(bitmap.Height));
			}
		}
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Expected O, but got Unknown
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Expected O, but got Unknown
		((Control)this).OnPaint(e);
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		if (isMouseOver)
		{
			SolidBrush val = new SolidBrush(MouseOverColor);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val, clientRectangle);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		if (IsSelected)
		{
			Pen val2 = new Pen(SelectedColor);
			try
			{
				e.Graphics.DrawRectangle(val2, clientRectangle.X, clientRectangle.Y, clientRectangle.Width - 1, clientRectangle.Height - 1);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
		}
		clientRectangle.X += StiScale.XXI(3.0);
		clientRectangle.Y += StiScale.YYI(3.0);
		clientRectangle.Width -= StiScale.XXI(6.0);
		clientRectangle.Height -= StiScale.YYI(6.0);
		e.Graphics.FillRectangle(Brushes.White, clientRectangle);
		Pen val3 = new Pen(StrokeColor);
		try
		{
			e.Graphics.DrawRectangle(val3, clientRectangle);
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
		e.Graphics.DrawImage(bitmap, new Rectangle(clientRectangle.X + StiScale.XXI(5.0), clientRectangle.Y + (clientRectangle.Height - bitmap.Height) / 2, bitmap.Width, bitmap.Height));
		clientRectangle.X += bitmap.Width + StiScale.XXI(10.0);
		clientRectangle.Width -= bitmap.Height + StiScale.YYI(5.0);
		Font val4 = new Font("Arial", 8f);
		try
		{
			StringFormat val5 = new StringFormat();
			try
			{
				val5.LineAlignment = (StringAlignment)1;
				e.Graphics.DrawString(collectionName, val4, Brushes.Black, (RectangleF)clientRectangle, val5);
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

	protected override void OnMouseLeave(EventArgs e)
	{
		((Control)this).OnMouseLeave(e);
		isMouseOver = false;
		((Control)this).Invalidate();
	}

	protected override void OnMouseEnter(EventArgs e)
	{
		((Control)this).OnMouseEnter(e);
		isMouseOver = true;
		((Control)this).Invalidate();
	}
}
