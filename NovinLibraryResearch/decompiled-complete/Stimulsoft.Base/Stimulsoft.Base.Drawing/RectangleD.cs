using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Drawing;

[TypeConverter(typeof(RectangleDConverter))]
public struct RectangleD
{
	public static RectangleD Empty = new RectangleD(0.0, 0.0, 0.0, 0.0);

	public double X { get; set; }

	public double Y { get; set; }

	public double Width { get; set; }

	public double Height { get; set; }

	public double Left => X;

	public double Top => Y;

	public double Right => X + Width;

	public double Bottom => Y + Height;

	public bool IsEmpty
	{
		get
		{
			if (Width == 0.0 && Height == 0.0 && X == 0.0)
			{
				return Y == 0.0;
			}
			return false;
		}
	}

	public PointD Location => new PointD(X, Y);

	public SizeD Size => new SizeD(Width, Height);

	public static bool operator !=(RectangleD left, RectangleD right)
	{
		if (left.X == right.X && left.Y == right.Y && left.Width == right.Width)
		{
			return left.Height != right.Height;
		}
		return true;
	}

	public static bool operator ==(RectangleD left, RectangleD right)
	{
		if (left.X == right.X && left.Y == right.Y && left.Width == right.Width)
		{
			return left.Height == right.Height;
		}
		return false;
	}

	public static RectangleD Ceiling(RectangleD value)
	{
		return new RectangleD(Math.Ceiling(value.X), Math.Ceiling(value.Y), Math.Ceiling(value.Width), Math.Ceiling(value.Height));
	}

	public static RectangleD Round(RectangleD value)
	{
		return new RectangleD(Math.Round(value.X), Math.Round(value.Y), Math.Round(value.Width), Math.Round(value.Height));
	}

	public static RectangleD Union(RectangleD a, RectangleD b)
	{
		double num = Math.Min(a.X, b.X);
		double num2 = Math.Max(a.X + a.Width, b.X + b.Width);
		double num3 = Math.Min(a.Y, b.Y);
		double num4 = Math.Max(a.Y + a.Height, b.Y + b.Height);
		return new RectangleD(num, num3, num2 - num, num4 - num3);
	}

	public void Inflate(double width, double height)
	{
		X -= width;
		Y -= height;
		Width += 2.0 * width;
		Height += 2.0 * height;
	}

	public Rectangle ToRectangle()
	{
		return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
	}

	public RectangleF ToRectangleF()
	{
		return new RectangleF((float)X, (float)Y, (float)Width, (float)Height);
	}

	public RectangleM ToRectangleM()
	{
		return new RectangleM((decimal)X, (decimal)Y, (decimal)Width, (decimal)Height);
	}

	public RectangleD Normalize()
	{
		RectangleD result = this;
		if (result.Width < 0.0)
		{
			result.X += result.Width;
			result.Width = 0.0 - result.Width;
		}
		if (result.Height < 0.0)
		{
			result.Y += result.Height;
			result.Height = 0.0 - result.Height;
		}
		return result;
	}

	public RectangleD Multiply(double multipleFactor)
	{
		return new RectangleD(X * multipleFactor, Y * multipleFactor, Width * multipleFactor, Height * multipleFactor);
	}

	public RectangleD Divide(double divideFactor)
	{
		return new RectangleD(X / divideFactor, Y / divideFactor, Width / divideFactor, Height / divideFactor);
	}

	public static RectangleD CreateFromRectangle(Rectangle rect)
	{
		return new RectangleD(rect.X, rect.Y, rect.Width, rect.Height);
	}

	public static RectangleD CreateFromRectangle(RectangleF rect)
	{
		return new RectangleD(rect.X, rect.Y, rect.Width, rect.Height);
	}

	public RectangleD OffsetSize(RectangleD offsettingRectangle)
	{
		return new RectangleD(X + offsettingRectangle.Width, Y + offsettingRectangle.Height, Width, Height);
	}

	public RectangleD OffsetRect(RectangleD offsettingRectangle)
	{
		return new RectangleD(X - offsettingRectangle.X, Y - offsettingRectangle.Y, Width + offsettingRectangle.Width, Height + offsettingRectangle.Height);
	}

	public bool IntersectsWith(RectangleD rect)
	{
		decimal num = Math.Round((decimal)rect.X, 2);
		decimal num2 = Math.Round((decimal)rect.Y, 2);
		decimal num3 = Math.Round((decimal)rect.Right, 2);
		decimal num4 = Math.Round((decimal)rect.Bottom, 2);
		decimal num5 = Math.Round((decimal)X, 2);
		decimal num6 = Math.Round((decimal)Y, 2);
		decimal num7 = Math.Round((decimal)Right, 2);
		decimal num8 = Math.Round((decimal)Bottom, 2);
		if (num < num7 && num2 < num8 && num3 > num5)
		{
			return num4 > num6;
		}
		return false;
	}

	public RectangleD AlignToGrid(double gridSize, bool aligningToGrid)
	{
		if (aligningToGrid)
		{
			return new RectangleD(Math.Round(X / gridSize) * gridSize, Math.Round(Y / gridSize) * gridSize, Math.Round(Width / gridSize) * gridSize, Math.Round(Height / gridSize) * gridSize);
		}
		return new RectangleD(Left, Top, Width, Height);
	}

	public RectangleD FitToRectangle(RectangleD rectangle)
	{
		if (IsEmpty)
		{
			return rectangle;
		}
		if (rectangle.IsEmpty)
		{
			return this;
		}
		RectangleD result = this;
		if (result.Left > rectangle.Left)
		{
			result.Width += result.Left - rectangle.Left;
			result.X = rectangle.Left;
		}
		if (result.Top > rectangle.Top)
		{
			result.Height += result.Top - rectangle.Top;
			result.Y = rectangle.Top;
		}
		if (result.Right < rectangle.Right)
		{
			result.Width += rectangle.Right - result.Right;
		}
		if (result.Bottom < rectangle.Bottom)
		{
			result.Height += rectangle.Bottom - result.Bottom;
		}
		return result;
	}

	public bool Contains(PointD pt)
	{
		return Contains(pt.X, pt.Y);
	}

	public bool Contains(RectangleD rect)
	{
		if (X < rect.X && rect.X + rect.Width < X + Width && Y < rect.Y)
		{
			return !(rect.Y + rect.Height > Y + Height);
		}
		return false;
	}

	public bool Contains(double x, double y)
	{
		if (X <= x && x < X + Width && Y <= y)
		{
			return y < Y + Height;
		}
		return false;
	}

	public static RectangleD Intersect(RectangleD a, RectangleD b)
	{
		double num = Math.Max(a.X, b.X);
		double num2 = Math.Min(a.X + a.Width, b.X + b.Width);
		double num3 = Math.Max(a.Y, b.Y);
		double num4 = Math.Min(a.Y + a.Height, b.Y + b.Height);
		if (num2 >= num && num4 >= num3)
		{
			return new RectangleD(num, num3, num2 - num, num4 - num3);
		}
		return Empty;
	}

	public void Intersect(RectangleD rect)
	{
		rect = Intersect(rect, this);
		X = rect.X;
		Y = rect.Y;
		Width = rect.Width;
		Height = rect.Height;
	}

	public override bool Equals(object obj)
	{
		RectangleD rectangleD = (RectangleD)obj;
		if (rectangleD.X == X && rectangleD.Y == Y && rectangleD.Width == Width)
		{
			return rectangleD.Height == Height;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (int)X ^ (((int)Y << 13) | ((int)Y >> 19)) ^ (((int)Width << 26) | ((int)Width >> 6)) ^ (((int)Height << 7) | ((int)Height >> 25));
	}

	public override string ToString()
	{
		return $"X={(decimal)X} Y={(decimal)Y} W={(decimal)Width} H={(decimal)Height}";
	}

	public void LoadFromJson(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "X":
				X = item.DeserializeDouble();
				break;
			case "Y":
				Y = item.DeserializeDouble();
				break;
			case "Width":
				Width = item.DeserializeDouble();
				break;
			case "Height":
				Height = item.DeserializeDouble();
				break;
			}
		}
	}

	public RectangleD(double x, double y, double width, double height)
	{
		X = x;
		Y = y;
		Width = width;
		Height = height;
	}

	public RectangleD(PointD location, SizeD size)
	{
		X = location.X;
		Y = location.Y;
		Width = size.Width;
		Height = size.Height;
	}
}
