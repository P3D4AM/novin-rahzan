using System.Drawing;

namespace Stimulsoft.Editor;

public class SyntaxError : ISyntaxError
{
	private string name = string.Empty;

	private string description = string.Empty;

	private IRange range = new Range(0, 0, 0, 0);

	public string Name
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
		}
	}

	public string Description
	{
		get
		{
			return description;
		}
		set
		{
			description = value;
		}
	}

	public IRange Range
	{
		get
		{
			return range;
		}
		set
		{
			range = value;
		}
	}

	public Point Position
	{
		get
		{
			return range.StartPoint;
		}
		set
		{
			range.StartPoint = value;
			range.EndPoint = new Point(value.X + name.Length, value.Y);
		}
	}

	public Size Size
	{
		get
		{
			return new Size(range.EndPoint.X - range.StartPoint.X, range.EndPoint.Y - range.StartPoint.Y);
		}
		set
		{
			range.EndPoint = new Point(range.StartPoint.X + value.Width, range.StartPoint.Y + value.Height);
		}
	}

	public SyntaxError()
	{
	}

	public SyntaxError(Point position, string name, string description)
	{
		this.name = name;
		this.description = description;
		range.StartPoint = position;
		range.EndPoint = new Point(position.X + name.Length, position.Y);
	}

	public override string ToString()
	{
		if (!(name != string.Empty))
		{
			return base.ToString();
		}
		return name;
	}
}
