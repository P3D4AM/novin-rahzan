using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar;

[TypeConverter(typeof(RoundRectangleShapeDescriptorConverter))]
[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class RoundRectangleShapeDescriptor : ShapeDescriptor
{
	private int __1B44;

	private int _ᭅ;

	private int _ᭆ;

	private int _ᭇ;

	private static RoundRectangleShapeDescriptor _ᭈ = new RoundRectangleShapeDescriptor();

	private static RoundRectangleShapeDescriptor _ᭉ = new RoundRectangleShapeDescriptor(2);

	private static RoundRectangleShapeDescriptor _ᭊ = new RoundRectangleShapeDescriptor(3);

	[DefaultValue(0)]
	[Description("Gets or sets the top-left round corner size.")]
	public int TopLeft
	{
		get
		{
			return __1B44;
		}
		set
		{
			if (__1B44 != value)
			{
				__1B44 = Math.Max(0, value);
			}
		}
	}

	[Description("Gets or sets the top-right round corner size.")]
	[DefaultValue(0)]
	public int TopRight
	{
		get
		{
			return _ᭅ;
		}
		set
		{
			if (_ᭅ != value)
			{
				_ᭅ = Math.Max(0, value);
			}
		}
	}

	[Description("Gets or sets the bottom-left round corner size.")]
	[DefaultValue(0)]
	public int BottomLeft
	{
		get
		{
			return _ᭆ;
		}
		set
		{
			if (_ᭆ != value)
			{
				_ᭆ = Math.Max(0, value);
			}
		}
	}

	[DefaultValue(0)]
	[Description("Gets or sets the bottom-right round corner size.")]
	public int BottomRight
	{
		get
		{
			return _ᭇ;
		}
		set
		{
			if (_ᭇ != value)
			{
				_ᭇ = Math.Max(0, value);
			}
		}
	}

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (__1B44 == 0 && _ᭅ == 0 && _ᭆ == 0)
			{
				return _ᭇ == 0;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool IsUniform
	{
		get
		{
			if (__1B44 == _ᭅ && __1B44 == _ᭆ)
			{
				return __1B44 == _ᭇ;
			}
			return false;
		}
	}

	public static RoundRectangleShapeDescriptor RectangleShape => _ᭈ;

	public static RoundRectangleShapeDescriptor RoundCorner2 => _ᭉ;

	public static RoundRectangleShapeDescriptor RoundCorner3 => _ᭊ;

	public RoundRectangleShapeDescriptor()
	{
	}

	public RoundRectangleShapeDescriptor(int cornerSize)
		: this(cornerSize, cornerSize, cornerSize, cornerSize)
	{
	}

	public RoundRectangleShapeDescriptor(int topLeft, int topRight, int bottomLeft, int bottomRight)
	{
		__1B44 = topLeft;
		_ᭅ = topRight;
		_ᭆ = bottomLeft;
		_ᭇ = bottomRight;
	}

	public override GraphicsPath GetShape(Rectangle bounds)
	{
		if (!CanDrawShape(bounds))
		{
			return null;
		}
		return ᭋ(bounds, __1B44, _ᭅ, _ᭆ, _ᭇ);
	}

	public override GraphicsPath GetInnerShape(Rectangle bounds, int borderSize)
	{
		return ᭋ(bounds, Math.Max(0, __1B44 - borderSize), Math.Max(0, _ᭅ - borderSize), Math.Max(0, _ᭆ - borderSize), Math.Max(0, _ᭇ - borderSize));
	}

	private GraphicsPath ᭋ(Rectangle ശ, int _1B4C, int _1B4D, int _1B4E, int _1B4F)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		if (_1B4C == 0 && _1B4D == 0 && _1B4E == 0 && _1B4F == 0)
		{
			GraphicsPath val = new GraphicsPath();
			val.AddRectangle(ശ);
			return val;
		}
		return DisplayHelp.GetRoundedRectanglePath(ശ, _1B4C, _1B4D, _1B4E, _1B4F, eStyleBackgroundPathPart.Complete, eCornerType.Rounded, eCornerType.Rounded, eCornerType.Rounded, eCornerType.Rounded, 0f);
	}

	public override bool CanDrawShape(Rectangle bounds)
	{
		if (bounds.Width < Math.Max(__1B44 + _ᭅ, _ᭆ + _ᭇ) + 1 || bounds.Height < Math.Max(__1B44 + _ᭅ, _ᭆ + _ᭇ) + 1 || bounds.Height <= 1 || bounds.Width <= 1)
		{
			return false;
		}
		return true;
	}
}
