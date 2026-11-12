using System.Drawing;

namespace Stimulsoft.Editor;

public class Range : IRange
{
	private Point startPoint;

	private Point endPoint;

	public Point StartPoint
	{
		get
		{
			return startPoint;
		}
		set
		{
			startPoint = value;
		}
	}

	public Point EndPoint
	{
		get
		{
			return endPoint;
		}
		set
		{
			endPoint = value;
		}
	}

	public Size Size
	{
		get
		{
			return new Size(endPoint.X - startPoint.X, endPoint.Y - startPoint.Y);
		}
		set
		{
			endPoint.X = startPoint.X + value.Width;
			endPoint.Y = startPoint.Y + value.Height;
		}
	}

	public bool IsEmpty
	{
		get
		{
			if (startPoint.Y <= endPoint.Y)
			{
				if (startPoint.Y == endPoint.Y)
				{
					return startPoint.X >= endPoint.X;
				}
				return false;
			}
			return true;
		}
	}

	public Range()
	{
	}

	public Range(Point startPoint, Point endPoint)
	{
		this.startPoint = startPoint;
		this.endPoint = endPoint;
	}

	public Range(int x1, int y1, int x2, int y2)
	{
		startPoint = new Point(x1, y1);
		endPoint = new Point(x2, y2);
	}
}
