using System;
using System.Drawing;
using System.Reflection;
using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Web;

internal class StiScalingImagesHelper
{
	private const string ScaleX1_25 = "_x1_25";

	private const string ScaleX1_5 = "_x1_5";

	private const string ScaleX1_75 = "_x1_75";

	private const string ScaleX2 = "_x2";

	private const string ScaleX2_25 = "_x2_25";

	private const string ScaleX2_5 = "_x2_5";

	private const string ScaleX3 = "_x3";

	private const string ScaleX3_5 = "_x3_5";

	private const string ScaleX4 = "_x4";

	private const string ScaleX4_5 = "_x4_5";

	private const string ScaleX5 = "_x5";

	private const string ScaleX6 = "_x6";

	private const string ScaleX7 = "_x7";

	private const string ScaleX8 = "_x8";

	public static Image GetIfExistOrResizeImage(Assembly assembly, string imagesPath, string imageName, string imageExt, double sourceScale, double needScale)
	{
		string imageName2 = imagesPath + imageName + GetScaleSuffix(sourceScale) + "." + imageExt;
		string imageName3 = imagesPath + imageName + "." + imageExt;
		if (StiImageUtils.ExistsImage(assembly, imageName2))
		{
			Bitmap image = StiImageUtils.GetImage(assembly, imageName2, makeTransparent: false);
			int num = 1;
			if (needScale / sourceScale >= 2.0)
			{
				num = (int)(needScale / sourceScale);
				sourceScale *= (double)num;
			}
			int num2 = ((Image)image).Width * num;
			int num3 = ((Image)image).Height * num;
			double num4 = (needScale - sourceScale) * ((double)num2 / sourceScale) + (double)num2;
			double num5 = (needScale - sourceScale) * ((double)num3 / sourceScale) + (double)num3;
			if (num4 / (double)(int)num4 != 1.0 || num5 / (double)(int)num5 != 1.0)
			{
				return (Image)(object)StiImageUtils.GetImage(assembly, imageName3, makeTransparent: false);
			}
			return (Image)(object)StiImageUtils.ResizeImage((Image)(object)image, (int)Math.Ceiling(num4), (int)Math.Ceiling(num5), num2, num3);
		}
		Bitmap image2 = StiImageUtils.GetImage(assembly, imageName3, makeTransparent: false);
		return (Image)(object)StiImageUtils.ResizeImage((Image)(object)image2, (int)Math.Ceiling((double)((Image)image2).Width * needScale), (int)Math.Ceiling((double)((Image)image2).Height * needScale));
	}

	public static Image GetScalingImage(Assembly assembly, string imagesPath, string imageName, string imageExt, double scaleFactor)
	{
		double sourceScale = 1.0;
		if (scaleFactor <= 2.0)
		{
			if (scaleFactor <= 1.5)
			{
				if (scaleFactor != 1.25)
				{
					if (scaleFactor == 1.5)
					{
						sourceScale = 1.5;
					}
				}
				else
				{
					sourceScale = 1.0;
				}
			}
			else if (scaleFactor != 1.75)
			{
				if (scaleFactor == 2.0)
				{
					sourceScale = 2.0;
				}
			}
			else
			{
				sourceScale = 1.5;
			}
		}
		else if (scaleFactor <= 2.5)
		{
			if (scaleFactor != 2.25)
			{
				if (scaleFactor == 2.5)
				{
					sourceScale = 2.0;
				}
			}
			else
			{
				sourceScale = 2.0;
			}
		}
		else if (scaleFactor != 3.0)
		{
			if (scaleFactor != 3.5)
			{
				if (scaleFactor == 4.0)
				{
					sourceScale = 2.0;
				}
			}
			else
			{
				sourceScale = 1.5;
			}
		}
		else
		{
			sourceScale = 1.5;
		}
		return GetIfExistOrResizeImage(assembly, imagesPath, imageName, imageExt, sourceScale, scaleFactor);
	}

	public static string GetScaleSuffix(double scaleFactor)
	{
		if (scaleFactor == 1.25)
		{
			return "_x1_25";
		}
		if (scaleFactor == 1.5)
		{
			return "_x1_5";
		}
		if (scaleFactor == 1.75)
		{
			return "_x1_75";
		}
		if (scaleFactor == 2.0)
		{
			return "_x2";
		}
		if (scaleFactor == 2.25)
		{
			return "_x2_25";
		}
		if (scaleFactor == 2.5)
		{
			return "_x2_5";
		}
		if (scaleFactor == 3.0)
		{
			return "_x3";
		}
		if (scaleFactor == 3.5)
		{
			return "_x3_5";
		}
		if (scaleFactor == 4.0)
		{
			return "_x4";
		}
		if (scaleFactor == 4.5)
		{
			return "_x4_5";
		}
		if (scaleFactor == 5.0)
		{
			return "_x5";
		}
		if (scaleFactor == 6.0)
		{
			return "_x6";
		}
		if (scaleFactor == 7.0)
		{
			return "_x7";
		}
		if (scaleFactor == 8.0)
		{
			return "_x8";
		}
		return string.Empty;
	}

	public static double GetScaleFactorFromName(string name)
	{
		if (name.Contains("_x1_25."))
		{
			return 1.25;
		}
		if (name.Contains("_x1_5."))
		{
			return 1.5;
		}
		if (name.Contains("_x1_75."))
		{
			return 1.75;
		}
		if (name.Contains("_x2."))
		{
			return 2.0;
		}
		if (name.Contains("_x2_25."))
		{
			return 2.25;
		}
		if (name.Contains("_x2_5."))
		{
			return 2.5;
		}
		if (name.Contains("_x3."))
		{
			return 3.0;
		}
		if (name.Contains("_x3_5."))
		{
			return 3.5;
		}
		if (name.Contains("_x4."))
		{
			return 4.0;
		}
		if (name.Contains("_x4_5."))
		{
			return 4.5;
		}
		if (name.Contains("_x5."))
		{
			return 5.0;
		}
		if (name.Contains("_x6."))
		{
			return 6.0;
		}
		if (name.Contains("_x7."))
		{
			return 7.0;
		}
		if (name.Contains("_x8."))
		{
			return 8.0;
		}
		return 1.0;
	}

	public static string GetScaleSuffixFromName(string name)
	{
		return GetScaleSuffix(GetScaleFactorFromName(name));
	}
}
