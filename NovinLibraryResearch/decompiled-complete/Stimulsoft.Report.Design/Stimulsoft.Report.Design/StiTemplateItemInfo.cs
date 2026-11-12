using System.Drawing;

namespace Stimulsoft.Report.Design;

public struct StiTemplateItemInfo
{
	public Bitmap Bitmap;

	public object Tag;

	public int ImageWidth;

	public int ImageHeight;

	public int X;

	public int Y;

	public int Width;

	public int Height;

	public StiTemplateItemInfo(Bitmap bitmap, object tag, int x, int y, int width, int height, int imageWidth, int imageHeight)
	{
		Bitmap = bitmap;
		Tag = tag;
		X = x;
		Y = y;
		Width = width;
		Height = height;
		ImageWidth = imageWidth;
		ImageHeight = imageHeight;
	}
}
