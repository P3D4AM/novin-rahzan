using System;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Base.Services;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
public sealed class StiServiceBitmapAttribute : Attribute
{
	public string BitmapName { get; set; }

	public Type Type { get; set; }

	public static string GetImagePath(Type type)
	{
		StiServiceBitmapAttribute[] source = (StiServiceBitmapAttribute[])type.GetCustomAttributes(typeof(StiServiceBitmapAttribute), inherit: true);
		return source.FirstOrDefault()?.BitmapName;
	}

	public static Bitmap GetImage(Type type)
	{
		StiServiceBitmapAttribute[] source = (StiServiceBitmapAttribute[])type.GetCustomAttributes(typeof(StiServiceBitmapAttribute), inherit: true);
		return source.FirstOrDefault()?.GetImage();
	}

	public Bitmap GetImage()
	{
		return StiImageUtils.GetImage(Type, BitmapName);
	}

	public StiServiceBitmapAttribute(Type type, string bitmapName)
	{
		Type = type;
		BitmapName = bitmapName;
	}
}
