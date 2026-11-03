using System.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Viewer;

public class StiPageParamView
{
	public StiPage Page { get; set; }

	public int X { get; set; }

	public int Y { get; set; }

	public int Width { get; set; }

	public int Height { get; set; }

	public int Line { get; set; }

	public Rectangle DisplayRectangle => new Rectangle(X, Y, Width, Height);

	public StiPageParamView()
	{
	}

	public StiPageParamView(int x, int y, int width, int height, int line)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
		Line = line;
	}
}
