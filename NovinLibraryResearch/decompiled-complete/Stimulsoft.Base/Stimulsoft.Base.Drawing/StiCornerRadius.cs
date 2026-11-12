using System;
using System.ComponentModel;
using Stimulsoft.Base.Design;
using Stimulsoft.Base.Drawing.Design;
using Stimulsoft.Base.Localization;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Base.Drawing;

[TypeConverter(typeof(StiCornerRadiusConverter))]
public class StiCornerRadius : ICloneable, IStiDefault
{
	private float topLeft;

	private float topRight;

	private float bottomRight;

	private float bottomLeft;

	[Browsable(false)]
	public virtual bool IsDefault
	{
		get
		{
			if (TopLeft == 0f && TopRight == 0f && BottomRight == 0f)
			{
				return BottomLeft == 0f;
			}
			return false;
		}
	}

	[DefaultValue(0f)]
	[StiSerializable]
	[StiOrder(1)]
	[RefreshProperties(RefreshProperties.All)]
	public float TopLeft
	{
		get
		{
			return topLeft;
		}
		set
		{
			if (topLeft != value)
			{
				if (value < 0f)
				{
					value = 0f;
				}
				if (value > 30f)
				{
					value = 30f;
				}
				topLeft = value;
			}
		}
	}

	[DefaultValue(0f)]
	[StiSerializable]
	[StiOrder(2)]
	[RefreshProperties(RefreshProperties.All)]
	public float TopRight
	{
		get
		{
			return topRight;
		}
		set
		{
			if (topRight != value)
			{
				if (value < 0f)
				{
					value = 0f;
				}
				if (value > 30f)
				{
					value = 30f;
				}
				topRight = value;
			}
		}
	}

	[DefaultValue(0f)]
	[StiSerializable]
	[StiOrder(3)]
	[RefreshProperties(RefreshProperties.All)]
	public float BottomRight
	{
		get
		{
			return bottomRight;
		}
		set
		{
			if (bottomRight != value)
			{
				if (value < 0f)
				{
					value = 0f;
				}
				if (value > 30f)
				{
					value = 30f;
				}
				bottomRight = value;
			}
		}
	}

	[DefaultValue(0f)]
	[StiSerializable]
	[StiOrder(4)]
	[RefreshProperties(RefreshProperties.All)]
	public float BottomLeft
	{
		get
		{
			return bottomLeft;
		}
		set
		{
			if (bottomLeft != value)
			{
				if (value < 0f)
				{
					value = 0f;
				}
				if (value > 30f)
				{
					value = 30f;
				}
				bottomLeft = value;
			}
		}
	}

	[Browsable(false)]
	public bool IsEmpty
	{
		get
		{
			if (TopLeft == 0f && TopRight == 0f && BottomLeft == 0f)
			{
				return BottomRight == 0f;
			}
			return false;
		}
	}

	[Browsable(false)]
	public bool IsAllSame
	{
		get
		{
			if (TopLeft == TopRight && BottomLeft == BottomRight)
			{
				return TopLeft == BottomRight;
			}
			return false;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}

	public override bool Equals(object obj)
	{
		StiCornerRadius stiCornerRadius = (StiCornerRadius)obj;
		if (stiCornerRadius != null && stiCornerRadius.TopLeft == TopLeft && stiCornerRadius.TopRight == TopRight && stiCornerRadius.BottomLeft == BottomLeft)
		{
			return stiCornerRadius.BottomRight == BottomRight;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string ToString()
	{
		StiCornerRadiusConverter stiCornerRadiusConverter = new StiCornerRadiusConverter();
		return stiCornerRadiusConverter.ConvertToString(this);
	}

	public static StiCornerRadius TryParse(string str)
	{
		try
		{
			StiCornerRadiusConverter stiCornerRadiusConverter = new StiCornerRadiusConverter();
			return stiCornerRadiusConverter.ConvertFromString(str) as StiCornerRadius;
		}
		catch
		{
			return new StiCornerRadius();
		}
	}

	public int GetUniqueCode()
	{
		int num = 0;
		num = (num * 397) ^ TopLeft.GetHashCode();
		num = (num * 397) ^ TopRight.GetHashCode();
		num = (num * 397) ^ BottomLeft.GetHashCode();
		return (num * 397) ^ BottomRight.GetHashCode();
	}

	public StiCornerRadius()
	{
	}

	public StiCornerRadius(float all)
		: this(all, all, all, all)
	{
	}

	public StiCornerRadius(float topLeft, float topRight, float bottomRight, float bottomLeft)
	{
		TopLeft = topLeft;
		TopRight = topRight;
		BottomRight = bottomRight;
		BottomLeft = bottomLeft;
	}
}
