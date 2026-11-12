using System;
using System.ComponentModel;
using System.Drawing;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Drawing;

[TypeConverter(typeof(RectangleMConverter))]
public struct RectangleM
{
	public static RectangleM Empty = new RectangleM(0m, 0m, 0m, 0m);

	public decimal X { get; set; }

	public decimal Y { get; set; }

	public decimal Width { get; set; }

	public decimal Height { get; set; }

	public decimal Left => X;

	public decimal Top => Y;

	public decimal Right => X + Width;

	public decimal Bottom => Y + Height;

	public bool IsEmpty
	{
		get
		{
			if (Width == 0m && Height == 0m && X == 0m)
			{
				return Y == 0m;
			}
			return false;
		}
	}

	public PointM Location => new PointM(X, Y);

	public SizeM Size => new SizeM(Width, Height);

	public void Inflate(decimal width, decimal height)
	{
		X -= width;
		Y -= height;
		Width += 2m * width;
		Height += 2m * height;
	}

	public Rectangle ToRectangle()
	{
		return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
	}

	public RectangleF ToRectangleF()
	{
		return new RectangleF((float)X, (float)Y, (float)Width, (float)Height);
	}

	public RectangleD ToRectangleD()
	{
		return new RectangleD((double)X, (double)Y, (double)Width, (double)Height);
	}

	public RectangleM Normalize()
	{
		RectangleM result = this;
		if (result.Width < 0m)
		{
			result.X += result.Width;
			result.Width = -result.Width;
		}
		if (result.Height < 0m)
		{
			result.Y += result.Height;
			result.Height = -result.Height;
		}
		return result;
	}

	public RectangleM Multiply(decimal multipleFactor)
	{
		return new RectangleM(X * multipleFactor, Y * multipleFactor, Width * multipleFactor, Height * multipleFactor);
	}

	public RectangleM Divide(decimal divideFactor)
	{
		return new RectangleM(X / divideFactor, Y / divideFactor, Width / divideFactor, Height / divideFactor);
	}

	public static RectangleM CreateFromRectangle(Rectangle rect)
	{
		return new RectangleM(rect.X, rect.Y, rect.Width, rect.Height);
	}

	public static RectangleM CreateFromRectangle(RectangleF rect)
	{
		return new RectangleM((decimal)rect.X, (decimal)rect.Y, (decimal)rect.Width, (decimal)rect.Height);
	}

	public RectangleM OffsetSize(RectangleM offsettingRectangle)
	{
		return new RectangleM(X + offsettingRectangle.Width, Y + offsettingRectangle.Height, Width, Height);
	}

	public RectangleM OffsetRect(RectangleM offsettingRectangle)
	{
		return new RectangleM(X - offsettingRectangle.X, Y - offsettingRectangle.Y, Width + offsettingRectangle.Width, Height + offsettingRectangle.Height);
	}

	public bool IntersectsWith(RectangleM rect)
	{
		decimal num = Math.Round(rect.X, 2);
		decimal num2 = Math.Round(rect.Y, 2);
		decimal num3 = Math.Round(rect.Right, 2);
		decimal num4 = Math.Round(rect.Bottom, 2);
		decimal num5 = Math.Round(X, 2);
		decimal num6 = Math.Round(Y, 2);
		decimal num7 = Math.Round(Right, 2);
		decimal num8 = Math.Round(Bottom, 2);
		if (num < num7 && num2 < num8 && num3 > num5)
		{
			return num4 > num6;
		}
		return false;
	}

	public RectangleM AlignToGrid(decimal gridSize, bool aligningToGrid)
	{
		if (aligningToGrid)
		{
			return new RectangleM(Math.Round(X / gridSize) * gridSize, Math.Round(Y / gridSize) * gridSize, Math.Round(Width / gridSize) * gridSize, Math.Round(Height / gridSize) * gridSize);
		}
		return new RectangleM(Left, Top, Width, Height);
	}

	public RectangleM FitToRectangle(RectangleM rectangle)
	{
		if (IsEmpty)
		{
			return rectangle;
		}
		if (rectangle.IsEmpty)
		{
			return this;
		}
		RectangleM result = this;
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

	public bool Contains(PointM pt)
	{
		return Contains(pt.X, pt.Y);
	}

	public bool Contains(RectangleM rect)
	{
		if (X < rect.X && rect.X + rect.Width < X + Width && Y < rect.Y)
		{
			return !(rect.Y + rect.Height > Y + Height);
		}
		return false;
	}

	public bool Contains(decimal x, decimal y)
	{
		if (X <= x && x < X + Width && Y <= y)
		{
			return y < Y + Height;
		}
		return false;
	}

	public static RectangleM Intersect(RectangleM a, RectangleM b)
	{
		decimal num = Math.Max(a.X, b.X);
		decimal num2 = Math.Min(a.X + a.Width, b.X + b.Width);
		decimal num3 = Math.Max(a.Y, b.Y);
		decimal num4 = Math.Min(a.Y + a.Height, b.Y + b.Height);
		if (!(num2 >= num) || !(num4 >= num3))
		{
			return Empty;
		}
		return new RectangleM(num, num3, num2 - num, num4 - num3);
	}

	public void Intersect(RectangleM rect)
	{
		rect = Intersect(rect, this);
		X = rect.X;
		Y = rect.Y;
		Width = rect.Width;
		Height = rect.Height;
	}

	public override bool Equals(object obj)
	{
		RectangleM rectangleM = (RectangleM)obj;
		if (rectangleM.X == X && rectangleM.Y == Y && rectangleM.Width == Width)
		{
			return rectangleM.Height == Height;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (int)X ^ (((int)Y << 13) | ((int)Y >> 19)) ^ (((int)Width << 26) | ((int)Width >> 6)) ^ (((int)Height << 7) | ((int)Height >> 25));
	}

	public override string ToString()
	{
		return $"X={X} Y={Y} W={Width} H={Height}";
	}

	public void LoadFromJson(JObject jObject)
	{
		foreach (JProperty item in jObject.Properties())
		{
			switch (item.Name)
			{
			case "X":
				X = item.DeserializeDecimal();
				break;
			case "Y":
				Y = item.DeserializeDecimal();
				break;
			case "Width":
				Width = item.DeserializeDecimal();
				break;
			case "Height":
				Height = item.DeserializeDecimal();
				break;
			}
		}
	}

	public RectangleM(decimal x, decimal y, decimal width, decimal height)
	{
		this = default(RectangleM);
		X = x;
		Y = y;
		Width = width;
		Height = height;
	}

	public RectangleM(PointM location, SizeM size)
	{
		this = default(RectangleM);
		X = location.X;
		Y = location.Y;
		Width = size.Width;
		Height = size.Height;
	}
}
