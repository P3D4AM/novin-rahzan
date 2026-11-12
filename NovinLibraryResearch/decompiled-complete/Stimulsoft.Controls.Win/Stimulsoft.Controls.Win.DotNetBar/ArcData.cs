namespace Stimulsoft.Controls.Win.DotNetBar;

internal struct ArcData
{
	public int X;

	public int Y;

	public int Width;

	public int Height;

	public float StartAngle;

	public float SweepAngle;

	public ArcData(int x, int y, int width, int height, float startAngle, float sweepAngle)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
		StartAngle = startAngle;
		SweepAngle = sweepAngle;
	}
}
