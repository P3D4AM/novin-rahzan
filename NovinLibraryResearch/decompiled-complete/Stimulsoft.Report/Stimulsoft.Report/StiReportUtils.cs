using System;
using System.Drawing;
using System.Drawing.Imaging;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report;

public class StiReportUtils
{
	public static void MakeBlackAndWhite(StiReport report)
	{
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		foreach (StiPage renderedPage in report.RenderedPages)
		{
			StiComponentsCollection components = renderedPage.GetComponents();
			foreach (StiComponent item in components)
			{
				if (item is IStiBrush stiBrush)
				{
					stiBrush.Brush = (StiBrush)new StiSolidBrush(ConvertColor(StiBrush.ToColor(stiBrush.Brush), 200));
				}
				if (item is IStiTextBrush stiTextBrush)
				{
					stiTextBrush.TextBrush = (StiBrush)new StiSolidBrush(ConvertColor(StiBrush.ToColor(stiTextBrush.TextBrush), 100));
				}
				if (item is IStiBorder stiBorder)
				{
					stiBorder.Border.Color = ConvertColor(stiBorder.Border.Color, 100);
					stiBorder.Border.ShadowBrush = (StiBrush)new StiSolidBrush(ConvertColor(StiBrush.ToColor(stiBorder.Border.ShadowBrush), 200));
				}
				if (item is IStiBackColor stiBackColor)
				{
					stiBackColor.BackColor = ConvertColor(stiBackColor.BackColor, 200);
				}
				if (item is IStiForeColor stiForeColor)
				{
					stiForeColor.ForeColor = ConvertColor(stiForeColor.ForeColor, 100);
				}
				if (item is IStiBorderColor stiBorderColor)
				{
					stiBorderColor.BorderColor = ConvertColor(stiBorderColor.BorderColor, 100);
				}
				if (item is StiLinePrimitive stiLinePrimitive)
				{
					stiLinePrimitive.Color = ConvertColor(stiLinePrimitive.Color, 100);
				}
				if (item is StiImage stiImage)
				{
					Image val = stiImage.TakeGdiImageToDraw();
					try
					{
						stiImage.PutImageToDraw(ConvertImage(val));
					}
					finally
					{
						((IDisposable)val)?.Dispose();
					}
				}
			}
		}
	}

	private static Image ConvertImage(Image image)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		if (image is Metafile)
		{
			return image;
		}
		if (image == null)
		{
			return null;
		}
		Bitmap val = (Bitmap)(object)((image is Bitmap) ? image : null);
		Bitmap val2 = new Bitmap(((Image)val).Width, ((Image)val).Height);
		Graphics val3 = Graphics.FromImage((Image)(object)val2);
		ColorMatrix colorMatrix = new ColorMatrix(new float[6][]
		{
			new float[5] { 0.3f, 0.3f, 0.3f, 0f, 0f },
			new float[5] { 0.55f, 0.55f, 0.55f, 0f, 0f },
			new float[5] { 0.15f, 0.15f, 0.15f, 0f, 0f },
			new float[6] { 0f, 0f, 0f, 1f, 0f, 0f },
			new float[6] { 0f, 0f, 0f, 0f, 1f, 0f },
			new float[6] { 0f, 0f, 0f, 0f, 0f, 1f }
		});
		ImageAttributes val4 = new ImageAttributes();
		val4.SetColorMatrix(colorMatrix);
		val3.DrawImage((Image)(object)val, new Rectangle(0, 0, ((Image)val).Width, ((Image)val).Height), 0, 0, ((Image)val).Width, ((Image)val).Height, (GraphicsUnit)2, val4);
		val3.Dispose();
		((Image)val).Dispose();
		return (Image)(object)val2;
	}

	private static Color ConvertColor(Color color, int range)
	{
		int num = (color.R + color.G + color.B) / 3;
		return Color.FromArgb(color.A, (num >= range) ? Color.White : Color.Black);
	}
}
