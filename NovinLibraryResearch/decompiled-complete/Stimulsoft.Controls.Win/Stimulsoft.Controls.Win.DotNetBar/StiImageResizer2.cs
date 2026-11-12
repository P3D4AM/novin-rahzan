using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar;

public static class StiImageResizer2
{
	public static Image GetImage(Image image)
	{
		if (image == null)
		{
			return null;
		}
		if (StiScale2.Factor == 1.0)
		{
			return image;
		}
		Bitmap result = StiImageUtils2.ResizeImage(image, StiScale2.XXI(image.Width), StiScale2.YYI(image.Height));
		image.Dispose();
		return (Image)(object)result;
	}
}
