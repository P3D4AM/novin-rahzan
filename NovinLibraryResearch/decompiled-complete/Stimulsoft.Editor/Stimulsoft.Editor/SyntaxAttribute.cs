using System.Drawing;

namespace Stimulsoft.Editor;

public class SyntaxAttribute : ISyntaxAttribute
{
	private string name = string.Empty;

	private object obj;

	private Point position = Point.Empty;

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

	public object Value
	{
		get
		{
			return obj;
		}
		set
		{
			obj = value;
		}
	}

	public Point Position
	{
		get
		{
			return position;
		}
		set
		{
			position = value;
		}
	}

	public SyntaxAttribute()
	{
	}

	public SyntaxAttribute(Point position, string name, object value)
	{
		this.position = position;
		this.name = name;
		obj = value;
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
