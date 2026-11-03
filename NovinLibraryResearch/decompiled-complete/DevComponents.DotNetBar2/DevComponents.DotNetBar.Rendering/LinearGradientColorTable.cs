using System.ComponentModel;
using System.Drawing;

namespace DevComponents.DotNetBar.Rendering;

public class LinearGradientColorTable
{
	public static readonly LinearGradientColorTable Empty = new LinearGradientColorTable();

	private Color _ቲ = Color.Empty;

	private Color _ታ = Color.Empty;

	private int _ቴ = 90;

	public Color Start
	{
		get
		{
			return _ቲ;
		}
		set
		{
			_ቲ = value;
		}
	}

	public Color End
	{
		get
		{
			return _ታ;
		}
		set
		{
			_ታ = value;
		}
	}

	public int GradientAngle
	{
		get
		{
			return _ቴ;
		}
		set
		{
			_ቴ = value;
		}
	}

	[Browsable(false)]
	public virtual bool IsEmpty
	{
		get
		{
			if (Start.IsEmpty && End.IsEmpty)
			{
				return GradientAngle == 90;
			}
			return false;
		}
	}

	public LinearGradientColorTable()
	{
	}

	public LinearGradientColorTable(Color start)
	{
		Start = start;
	}

	public LinearGradientColorTable(Color start, Color end)
	{
		Start = start;
		End = end;
	}

	public LinearGradientColorTable(string start, string end)
	{
		Start = ColorScheme.GetColor(start);
		End = ColorScheme.GetColor(end);
	}

	public LinearGradientColorTable(int start)
	{
		Start = ColorScheme.GetColor(start);
	}

	public LinearGradientColorTable(int start, int end)
	{
		Start = ColorScheme.GetColor(start);
		End = ColorScheme.GetColor(end);
	}

	public LinearGradientColorTable(int start, int end, int gradientAngle)
	{
		Start = ColorScheme.GetColor(start);
		End = ColorScheme.GetColor(end);
		GradientAngle = gradientAngle;
	}

	public LinearGradientColorTable(Color start, Color end, int gradientAngle)
	{
		Start = start;
		End = end;
		GradientAngle = gradientAngle;
	}
}
