namespace Stimulsoft.Controls.Win.DotNetBar.Presentation;

internal class Location
{
	public int X;

	public int Y;

	public eRelativeLocation RelativeX;

	public eRelativeLocation RelativeY;

	public Location()
	{
	}

	public Location(int x, int y)
	{
		X = x;
		Y = y;
	}

	public Location(int x, int y, eRelativeLocation relativeX, eRelativeLocation relativeY)
	{
		X = x;
		Y = y;
		RelativeX = relativeX;
		RelativeY = relativeY;
	}
}
