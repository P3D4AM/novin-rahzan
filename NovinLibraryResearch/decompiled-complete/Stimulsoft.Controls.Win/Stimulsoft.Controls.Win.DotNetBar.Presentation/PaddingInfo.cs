namespace Stimulsoft.Controls.Win.DotNetBar.Presentation;

internal class PaddingInfo
{
	public int Left;

	public int Right;

	public int Top;

	public int Bottom;

	public int HorizontalPadding => Left + Right;

	public int VerticalPadding => Top + Bottom;

	public PaddingInfo()
	{
	}

	public PaddingInfo(int left, int top, int right, int bottom)
	{
		Left = left;
		Top = top;
		Right = right;
		Bottom = bottom;
	}
}
