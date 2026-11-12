using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
public class StiTimeSpanPicker : StiControlBase
{
	private MaskedTextBox maskedText;

	private ToolTip toolTip;

	private string toolTipText;

	public TimeSpan Value
	{
		get
		{
			CheckText();
			TimeSpan result = default(TimeSpan);
			string text = ((Control)maskedText).Text.Replace(",", ".").Replace(" ", "0");
			while (text.Length < 16)
			{
				text += "0";
			}
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", useUserOverride: false);
			if (TimeSpan.TryParseExact(text, "ddd\\.hh\\:mm\\:ss\\.fff", null, out result))
			{
				return result;
			}
			Thread.CurrentThread.CurrentCulture = currentCulture;
			return result;
		}
		set
		{
			((Control)maskedText).Text = value.ToString("ddd\\.hh\\:mm\\:ss\\.fff");
		}
	}

	public bool ReadOnly
	{
		get
		{
			return maskedText.ReadOnly;
		}
		set
		{
			maskedText.ReadOnly = value;
			((Control)this).Invalidate();
		}
	}

	protected override void OnMouseMove(MouseEventArgs e)
	{
		((Control)this).OnMouseMove(e);
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		if (new Rectangle(rectangle.Right - rectangle.Height, 0, rectangle.Height, rectangle.Height).Contains(e.Location))
		{
			toolTip.Show(toolTipText, (IWin32Window)(object)this, new Point(rectangle.Right + StiScale.I4, StiScale.I4));
		}
		else
		{
			toolTip.Hide((IWin32Window)(object)this);
		}
	}

	protected override void OnMouseLeave(EventArgs e)
	{
		base.OnMouseLeave(e);
		toolTip.Hide((IWin32Window)(object)this);
	}

	protected override void OnPaint(PaintEventArgs p)
	{
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		Graphics graphics = p.Graphics;
		Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
		if (!((Control)this).Enabled)
		{
			graphics.FillRectangle(Brushes.WhiteSmoke, rectangle);
		}
		else
		{
			StiDrawing.FillRectangle(graphics, Brushes.White, rectangle);
		}
		StiControlPaint.DrawBorder(graphics, rectangle, (IsMouseOver | ((Control)this).Focused) & base.HotFocus, ((Control)this).Enabled, flat: true);
		Rectangle rectangle2 = new Rectangle(rectangle.Right - rectangle.Height, 0, rectangle.Height, rectangle.Height);
		rectangle2.Inflate(-StiScale.I4, -StiScale.I4);
		Pen val = new Pen(Color.Gray, (float)StiScale.I1);
		try
		{
			SmoothingMode smoothingMode = graphics.SmoothingMode;
			graphics.SmoothingMode = (SmoothingMode)4;
			graphics.DrawEllipse(val, rectangle2);
			rectangle2.Inflate(-StiScale.I2, -StiScale.I2);
			graphics.DrawLine(val, rectangle2.Left + rectangle2.Width / 2, rectangle2.Top + rectangle2.Height / 2, rectangle2.Right, rectangle2.Top + rectangle2.Height / 2);
			graphics.DrawLine(val, rectangle2.Left + rectangle2.Width / 2, rectangle2.Top + rectangle2.Height / 2, rectangle2.Left + rectangle2.Width / 2, rectangle2.Bottom);
			graphics.SmoothingMode = smoothingMode;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	protected override void OnSizeChanged(EventArgs e)
	{
		SetPosTextBox();
		((Control)this).OnSizeChanged(e);
	}

	private void MaskedText_LostFocus(object sender, EventArgs e)
	{
		CheckText();
	}

	private void CheckText()
	{
		bool flag = ((Control)maskedText).Text.Contains(".");
		string[] array = ((Control)maskedText).Text.Split(':', ',', '.');
		string text = array[0];
		string text2 = array[1];
		if (int.TryParse(text2, out var result) && result > 23)
		{
			text2 = "23";
		}
		string text3 = array[2];
		if (int.TryParse(text3, out var result2) && result2 > 59)
		{
			text3 = "59";
		}
		string text4 = array[3];
		if (int.TryParse(text4, out var result3) && result3 > 59)
		{
			text4 = "59";
		}
		string text5 = array[4];
		string text6 = (flag ? "." : ",");
		((Control)maskedText).Text = text + text6 + text2 + ":" + text3 + ":" + text4 + text6 + text5;
	}

	protected virtual void SetPosTextBox()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			MaskedTextBox obj = maskedText;
			Padding padding = ((Control)this).Padding;
			((Control)obj).Left = ((Padding)(ref padding)).Left;
			MaskedTextBox obj2 = maskedText;
			padding = ((Control)this).Padding;
			((Control)obj2).Top = ((Padding)(ref padding)).Top;
		}
		catch
		{
		}
	}

	public StiTimeSpanPicker()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		toolTip = new ToolTip();
		toolTipText = Loc.Get("FormFormatEditor", "FormatMask") + " ddd.hh:mm:ss.fff";
		((Control)this).Padding = new Padding(StiScale.I2);
		maskedText = new MaskedTextBox();
		((TextBoxBase)maskedText).BorderStyle = (BorderStyle)0;
		maskedText.Mask = "000.00:00:00.000";
		maskedText.PromptChar = '0';
		((Control)maskedText).LostFocus += MaskedText_LostFocus;
		((Control)this).Controls.Add((Control)(object)maskedText);
		SetPosTextBox();
	}
}
