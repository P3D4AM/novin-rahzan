using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
[TypeConverter(typeof(RoundRectangleShapeDescriptorConverter))]
public class RoundRectangleShapeDescriptor : ShapeDescriptor
{
	private int _TopLeft;

	private int _TopRight;

	private int _BottomLeft;

	private int _BottomRight;

	private static RoundRectangleShapeDescriptor _RectangleShape = new RoundRectangleShapeDescriptor();

	private static RoundRectangleShapeDescriptor _RoundCorner2 = new RoundRectangleShapeDescriptor(2);

	private static RoundRectangleShapeDescriptor _RoundCorner3 = new RoundRectangleShapeDescriptor(3);

	[DefaultValue(0)]
	[Description("Gets or sets the top-left round corner size.")]
	public int TopLeft
	{
		get
		{
			return _TopLeft;
		}
		set
		{
			if (_TopLeft != value)
			{
				_TopLeft = Math.Max(0, value);
			}
		}
	}

	[DefaultValue(0)]
	[Description("Gets or sets the top-right round corner size.")]
	public int TopRight
	{
		get
		{
			return _TopRight;
		}
		set
		{
			if (_TopRight != value)
			{
				_TopRight = Math.Max(0, value);
			}
		}
	}

	[DefaultValue(0)]
	[Description("Gets or sets the bottom-left round corner size.")]
	public int BottomLeft
	{
		get
		{
			return _BottomLeft;
		}
		set
		{
			if (_BottomLeft != value)
			{
				_BottomLeft = Math.Max(0, value);
			}
		}
	}

	[DefaultValue(0)]
	[Description("Gets or sets the bottom-right round corner size.")]
	public int BottomRight
	{
		get
		{
			return _BottomRight;
		}
		set
		{
			if (_BottomRight != value)
			{
				_BottomRight = Math.Max(0, value);
			}
		}
	}

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (_TopLeft == 0 && _TopRight == 0 && _BottomLeft == 0)
			{
				return _BottomRight == 0;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool IsUniform
	{
		get
		{
			if (_TopLeft == _TopRight && _TopLeft == _BottomLeft)
			{
				return _TopLeft == _BottomRight;
			}
			return false;
		}
	}

	public static RoundRectangleShapeDescriptor RectangleShape => _RectangleShape;

	public static RoundRectangleShapeDescriptor RoundCorner2 => _RoundCorner2;

	public static RoundRectangleShapeDescriptor RoundCorner3 => _RoundCorner3;

	public RoundRectangleShapeDescriptor()
	{
	}

	public RoundRectangleShapeDescriptor(int cornerSize)
		: this(cornerSize, cornerSize, cornerSize, cornerSize)
	{
	}

	public RoundRectangleShapeDescriptor(int topLeft, int topRight, int bottomLeft, int bottomRight)
	{
		_TopLeft = topLeft;
		_TopRight = topRight;
		_BottomLeft = bottomLeft;
		_BottomRight = bottomRight;
	}

	public override GraphicsPath GetShape(Rectangle bounds)
	{
		if (!CanDrawShape(bounds))
		{
			return null;
		}
		return GetShape(bounds, _TopLeft, _TopRight, _BottomLeft, _BottomRight);
	}

	public override GraphicsPath GetInnerShape(Rectangle bounds, int borderSize)
	{
		return GetShape(bounds, Math.Max(0, _TopLeft - borderSize), Math.Max(0, _TopRight - borderSize), Math.Max(0, _BottomLeft - borderSize), Math.Max(0, _BottomRight - borderSize));
	}

	private GraphicsPath GetShape(Rectangle bounds, int topLeft, int topRight, int bottomLeft, int bottomRight)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (topLeft == 0 && topRight == 0 && bottomLeft == 0 && bottomRight == 0)
		{
			GraphicsPath val = new GraphicsPath();
			val.AddRectangle(bounds);
			return val;
		}
		return DisplayHelp.GetRoundedRectanglePath(bounds, topLeft);
	}

	public override bool CanDrawShape(Rectangle bounds)
	{
		if (bounds.Width < Math.Max(_TopLeft + _TopRight, _BottomLeft + _BottomRight) + 1 || bounds.Height < Math.Max(_TopLeft + _TopRight, _BottomLeft + _BottomRight) + 1 || bounds.Height <= 1 || bounds.Width <= 1)
		{
			return false;
		}
		return true;
	}
}
