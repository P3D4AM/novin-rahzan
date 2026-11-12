using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Stimulsoft.Base.Drawing;

public static class StiImageTransparenceHelper
{
	public static Bitmap GetTransparentedImage(Image source, float transparency)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		if (source == null)
		{
			return null;
		}
		float[][] array = new float[5][]
		{
			new float[5] { 1f, 0f, 0f, 0f, 0f },
			new float[5] { 0f, 1f, 0f, 0f, 0f },
			new float[5] { 0f, 0f, 1f, 0f, 0f },
			new float[5] { 0f, 0f, 0f, transparency, 0f },
			new float[5] { 0f, 0f, 0f, 0f, 1f }
		};
		ImageAttributes val = new ImageAttributes();
		val.SetColorMatrix(new ColorMatrix(array), (ColorMatrixFlag)0, (ColorAdjustType)1);
		Bitmap val2 = new Bitmap(source.Width, source.Height);
		Graphics val3 = Graphics.FromImage((Image)(object)val2);
		try
		{
			val3.Clear(Color.Transparent);
			val3.DrawImage(source, new Rectangle(Point.Empty, source.Size), 0, 0, source.Width, source.Height, (GraphicsUnit)2, val);
			return val2;
		}
		finally
		{
			((IDisposable)val3)?.Dispose();
		}
	}
}
