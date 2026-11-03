using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;

namespace FormCustom.ExtendedControls;

public class OvalPictureBox : PictureBox
{
	private Random random = new Random();

	private string colorBaseText;

	public string ColorBaseText
	{
		get
		{
			return colorBaseText;
		}
		set
		{
			CreateImageFromString(value);
		}
	}

	public Color BorderColor { get; set; }

	private void CreateImageFromString(string value)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		colorBaseText = value;
		if (string.IsNullOrEmpty(value))
		{
			return;
		}
		Bitmap val = new Bitmap(((Control)this).Width, ((Control)this).Height);
		Color color = StringToHslColor(value);
		Graphics val2 = Graphics.FromImage((Image)(object)val);
		try
		{
			SolidBrush val3 = new SolidBrush(color);
			try
			{
				val2.FillRectangle((Brush)(object)val3, 0, 0, ((Control)this).Width, ((Control)this).Height);
				Font val4 = new Font("Tahoma", 20f);
				try
				{
					string text = CreateText(ColorBaseText).TrimEnd(Array.Empty<char>());
					Size size = TextRenderer.MeasureText(text, val4);
					val3.Color = Color.Black;
					val2.SmoothingMode = (SmoothingMode)4;
					val2.TextRenderingHint = (TextRenderingHint)4;
					val2.DrawString(text, val4, (Brush)(object)val3, new PointF((float)((Control)this).Width / 2f - (float)size.Width / 2f, (float)(((Control)this).Height - 3) / 2f - (float)size.Height / 2f));
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
		((PictureBox)this).Image = (Image)(object)val;
	}

	private Color StringToHslColor(string str)
	{
		Color color = Color.FromArgb(str.ToUpper().GetHashCode() >> 8);
		if (color.A == 0)
		{
			color = Color.FromArgb(random.Next(150, 255), color);
		}
		return color;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		Brush val2;
		try
		{
			Bitmap val = null;
			if (((PictureBox)this).Image != null)
			{
				val = new Bitmap(((PictureBox)this).Image);
				val = new Bitmap((Image)(object)val, new Size(((Control)this).Width - 3, ((Control)this).Height - 3));
			}
			else
			{
				val = new Bitmap(((Control)this).Width - 3, ((Control)this).Height - 3, (PixelFormat)137224);
			}
			val2 = (Brush)new TextureBrush((Image)(object)val);
			Graphics val3 = Graphics.FromImage((Image)(object)val);
			try
			{
				val3.FillRectangle(Brushes.White, 0, 0, ((Control)this).Width - 3, ((Control)this).Height - 3);
				val = new Bitmap(((Control)this).Width - 3, ((Control)this).Height - 3, val3);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
		}
		catch
		{
			throw;
		}
		e.Graphics.SmoothingMode = (SmoothingMode)4;
		GraphicsPath val4 = new GraphicsPath();
		val4.AddEllipse(0, 0, ((Control)this).Width - 3, ((Control)this).Height - 3);
		e.Graphics.FillPath(val2, val4);
	}

	private string CreateText(string colorBaseText)
	{
		if (string.IsNullOrEmpty(colorBaseText))
		{
			return "";
		}
		string[] array = colorBaseText.Split(new char[1] { ' ' });
		string text = "";
		string[] array2 = array;
		foreach (string text2 in array2)
		{
			text = text + text2.Substring(0, 1) + " ";
		}
		return text.ToUpper();
	}
}
