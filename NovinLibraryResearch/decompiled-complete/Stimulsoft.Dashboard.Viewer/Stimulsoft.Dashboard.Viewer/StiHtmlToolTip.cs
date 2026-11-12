using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Helpers;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Dashboard.Viewer;

[ToolboxItem(false)]
public class StiHtmlToolTip : ToolTip
{
	private class StiToolTipObject : IStiHorAlignment, IStiVertAlignment, IStiFont
	{
		public Font Font { get; set; } = new Font("Arial", 12f);


		public StiHorAlignment HorAlignment { get; set; } = StiHorAlignment.Left;


		public StiVertAlignment VertAlignment { get; set; } = StiVertAlignment.Center;

	}

	private StiToolTipObject toolTipObject = new StiToolTipObject();

	public string HtmlText { get; set; }

	private void ToolTip_Popup(object sender, PopupEventArgs e)
	{
		e.ToolTipSize = MeasureSize(HtmlText);
	}

	private void ToolTip_Draw(object sender, DrawToolTipEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Rectangle bounds = e.Bounds;
		SolidBrush val = new SolidBrush(Color.White);
		try
		{
			graphics.FillRectangle((Brush)(object)val, bounds);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		bounds.Inflate(-StiScale.I5, -StiScale.I5);
		StiAutoSizeHtmlTextHelper.Draw(graphics, RectangleD.CreateFromRectangle(bounds), HtmlText, toolTipObject, 1.0, Color.Black, wordWrap: false, (StringTrimming)0);
		SolidBrush val2 = new SolidBrush(Color.FromArgb(200, 200, 200));
		try
		{
			Pen val3 = new Pen((Brush)(object)val2, 1f);
			try
			{
				graphics.DrawRectangle(val3, new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, e.Bounds.Height - 1));
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

	private string GetPlainTextFromHtml(string text)
	{
		if (!string.IsNullOrWhiteSpace(text))
		{
			return Regex.Replace(text, "<.*?>", string.Empty);
		}
		return text;
	}

	public void ShowHtml(string text, IWin32Window window, Point point)
	{
		HtmlText = text;
		((ToolTip)this).Show(GetPlainTextFromHtml(text), window, point);
	}

	public void SetHtmlToolTip(Control control, string caption)
	{
		HtmlText = caption;
		((ToolTip)this).SetToolTip(control, GetPlainTextFromHtml(caption));
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
				result.Width += StiScale.I10;
				result.Height += StiScale.I10;
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

	protected override void Dispose(bool disposing)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		((ToolTip)this).Dispose(disposing);
		if (disposing)
		{
			((ToolTip)this).Popup -= new PopupEventHandler(ToolTip_Popup);
			((ToolTip)this).Draw -= new DrawToolTipEventHandler(ToolTip_Draw);
		}
	}

	public StiHtmlToolTip()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		((ToolTip)this).OwnerDraw = true;
		((ToolTip)this).Popup += new PopupEventHandler(ToolTip_Popup);
		((ToolTip)this).Draw += new DrawToolTipEventHandler(ToolTip_Draw);
	}
}
