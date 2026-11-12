using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
public sealed class StiDesignerIconAttribute : Attribute
{
	public string BitmapName { get; set; }

	public Type Type { get; set; }

	public static string GetImagePath(Type type)
	{
		StiDesignerIconAttribute[] array = (StiDesignerIconAttribute[])type.GetCustomAttributes(typeof(StiDesignerIconAttribute), inherit: true);
		if (array.Length == 0)
		{
			return null;
		}
		return array[0].BitmapName;
	}

	public static Bitmap GetImage(Type type)
	{
		StiDesignerIconAttribute[] array = (StiDesignerIconAttribute[])type.GetCustomAttributes(typeof(StiDesignerIconAttribute), inherit: true);
		if (array.Length == 0)
		{
			return null;
		}
		return array[0].GetImage();
	}

	public Bitmap GetImage()
	{
		return StiImageUtils.GetImage(Type, BitmapName);
	}

	public StiDesignerIconAttribute(Type type, string bitmapName)
	{
		Type = type;
		BitmapName = bitmapName;
	}
}
