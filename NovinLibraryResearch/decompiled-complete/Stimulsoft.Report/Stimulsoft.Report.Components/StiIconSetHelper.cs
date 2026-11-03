using System;
using System.Drawing;
using System.IO;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Base.Helpers;

namespace Stimulsoft.Report.Components;

public static class StiIconSetHelper
{
	public static StiIcon[] GetIconSet(StiIconSet iconSet)
	{
		return iconSet switch
		{
			StiIconSet.ArrowsColored3 => new StiIcon[3]
			{
				StiIcon.ArrowUpGreen,
				StiIcon.ArrowRightYellow,
				StiIcon.ArrowDownRed
			}, 
			StiIconSet.ArrowsColored4 => new StiIcon[4]
			{
				StiIcon.ArrowUpGreen,
				StiIcon.ArrowRightUpYellow,
				StiIcon.ArrowRightDownYellow,
				StiIcon.ArrowDownRed
			}, 
			StiIconSet.ArrowsColored5 => new StiIcon[5]
			{
				StiIcon.ArrowUpGreen,
				StiIcon.ArrowRightUpYellow,
				StiIcon.ArrowRightYellow,
				StiIcon.ArrowRightDownYellow,
				StiIcon.ArrowDownRed
			}, 
			StiIconSet.ArrowsGray3 => new StiIcon[3]
			{
				StiIcon.ArrowUpGray,
				StiIcon.ArrowRightGray,
				StiIcon.ArrowDownGray
			}, 
			StiIconSet.ArrowsGray4 => new StiIcon[4]
			{
				StiIcon.ArrowUpGray,
				StiIcon.ArrowRightUpGray,
				StiIcon.ArrowRightDownGray,
				StiIcon.ArrowDownGray
			}, 
			StiIconSet.ArrowsGray5 => new StiIcon[5]
			{
				StiIcon.ArrowUpGray,
				StiIcon.ArrowRightUpGray,
				StiIcon.ArrowRightGray,
				StiIcon.ArrowRightDownGray,
				StiIcon.ArrowDownGray
			}, 
			StiIconSet.Flags3 => new StiIcon[3]
			{
				StiIcon.FlagGreen,
				StiIcon.FlagYellow,
				StiIcon.FlagRed
			}, 
			StiIconSet.Latin4 => new StiIcon[4]
			{
				StiIcon.Latin1,
				StiIcon.Latin2,
				StiIcon.Latin3,
				StiIcon.Latin4
			}, 
			StiIconSet.Quarters5 => new StiIcon[5]
			{
				StiIcon.QuarterFull,
				StiIcon.QuarterThreeFourth,
				StiIcon.QuarterHalf,
				StiIcon.QuarterQuarter,
				StiIcon.QuarterNone
			}, 
			StiIconSet.QuartersGreen5 => new StiIcon[5]
			{
				StiIcon.QuarterFullGreen,
				StiIcon.QuarterThreeFourthGreen,
				StiIcon.QuarterHalfGreen,
				StiIcon.QuarterQuarterGreen,
				StiIcon.QuarterNoneGreen
			}, 
			StiIconSet.QuartersRed5 => new StiIcon[5]
			{
				StiIcon.QuarterFullRed,
				StiIcon.QuarterThreeFourthRed,
				StiIcon.QuarterHalfRed,
				StiIcon.QuarterQuarterRed,
				StiIcon.QuarterNoneRed
			}, 
			StiIconSet.Ratings3 => new StiIcon[3]
			{
				StiIcon.Rating4,
				StiIcon.Rating2,
				StiIcon.Rating0
			}, 
			StiIconSet.Ratings4 => new StiIcon[4]
			{
				StiIcon.Rating4,
				StiIcon.Rating3,
				StiIcon.Rating2,
				StiIcon.Rating1
			}, 
			StiIconSet.Ratings5 => new StiIcon[5]
			{
				StiIcon.Rating4,
				StiIcon.Rating3,
				StiIcon.Rating2,
				StiIcon.Rating1,
				StiIcon.Rating0
			}, 
			StiIconSet.RedToBlack4 => new StiIcon[4]
			{
				StiIcon.FromRedToBlackRed,
				StiIcon.FromRedToBlackPink,
				StiIcon.FromRedToBlackGray,
				StiIcon.QuarterFull
			}, 
			StiIconSet.Signs3 => new StiIcon[3]
			{
				StiIcon.CircleGreen,
				StiIcon.TriangleYellow,
				StiIcon.RhombRed
			}, 
			StiIconSet.Squares5 => new StiIcon[5]
			{
				StiIcon.Square0,
				StiIcon.Square1,
				StiIcon.Square2,
				StiIcon.Square3,
				StiIcon.Square4
			}, 
			StiIconSet.Stars3 => new StiIcon[3]
			{
				StiIcon.StarFull,
				StiIcon.StarHalf,
				StiIcon.StarNone
			}, 
			StiIconSet.Stars5 => new StiIcon[5]
			{
				StiIcon.StarFull,
				StiIcon.StarThreeFourth,
				StiIcon.StarHalf,
				StiIcon.StarQuarter,
				StiIcon.StarNone
			}, 
			StiIconSet.SymbolsCircled3 => new StiIcon[3]
			{
				StiIcon.CircleCheckGreen,
				StiIcon.CircleExclamationYellow,
				StiIcon.CircleCrossRed
			}, 
			StiIconSet.SymbolsUncircled3 => new StiIcon[3]
			{
				StiIcon.CheckGreen,
				StiIcon.ExclamationYellow,
				StiIcon.CrossRed
			}, 
			StiIconSet.TrafficLights4 => new StiIcon[4]
			{
				StiIcon.CircleGreen,
				StiIcon.CircleYellow,
				StiIcon.CircleRed,
				StiIcon.CircleBlack
			}, 
			StiIconSet.TrafficLightsRimmed3 => new StiIcon[3]
			{
				StiIcon.LightsGreen,
				StiIcon.LightsYellow,
				StiIcon.LightsRed
			}, 
			StiIconSet.TrafficLightsUnrimmed3 => new StiIcon[3]
			{
				StiIcon.CircleGreen,
				StiIcon.CircleYellow,
				StiIcon.CircleRed
			}, 
			StiIconSet.Triangles3 => new StiIcon[3]
			{
				StiIcon.TriangleGreen,
				StiIcon.MinusYellow,
				StiIcon.TriangleRed
			}, 
			StiIconSet.Full => new StiIcon[68]
			{
				StiIcon.ArrowUpGreen,
				StiIcon.ArrowRightYellow,
				StiIcon.ArrowDownRed,
				StiIcon.ArrowUpGray,
				StiIcon.ArrowRightGray,
				StiIcon.ArrowDownGray,
				StiIcon.ArrowRightUpYellow,
				StiIcon.ArrowRightDownYellow,
				StiIcon.ArrowRightUpGray,
				StiIcon.ArrowRightDownGray,
				StiIcon.TriangleGreen,
				StiIcon.MinusYellow,
				StiIcon.TriangleRed,
				StiIcon.FlagGreen,
				StiIcon.FlagYellow,
				StiIcon.FlagRed,
				StiIcon.Latin1,
				StiIcon.Latin2,
				StiIcon.Latin3,
				StiIcon.Latin4,
				StiIcon.CheckGreen,
				StiIcon.ExclamationYellow,
				StiIcon.CrossRed,
				StiIcon.CircleCheckGreen,
				StiIcon.CircleExclamationYellow,
				StiIcon.CircleCrossRed,
				StiIcon.CircleGreen,
				StiIcon.CircleYellow,
				StiIcon.CircleRed,
				StiIcon.CircleBlack,
				StiIcon.TriangleYellow,
				StiIcon.RhombRed,
				StiIcon.FromRedToBlackRed,
				StiIcon.FromRedToBlackPink,
				StiIcon.FromRedToBlackGray,
				StiIcon.LightsGreen,
				StiIcon.LightsYellow,
				StiIcon.LightsRed,
				StiIcon.QuarterFull,
				StiIcon.QuarterThreeFourth,
				StiIcon.QuarterHalf,
				StiIcon.QuarterQuarter,
				StiIcon.QuarterNone,
				StiIcon.QuarterFullGreen,
				StiIcon.QuarterThreeFourthGreen,
				StiIcon.QuarterHalfGreen,
				StiIcon.QuarterQuarterGreen,
				StiIcon.QuarterNoneGreen,
				StiIcon.QuarterFullRed,
				StiIcon.QuarterThreeFourthRed,
				StiIcon.QuarterHalfRed,
				StiIcon.QuarterQuarterRed,
				StiIcon.QuarterNoneRed,
				StiIcon.Rating0,
				StiIcon.Rating1,
				StiIcon.Rating2,
				StiIcon.Rating3,
				StiIcon.Rating4,
				StiIcon.Square0,
				StiIcon.Square1,
				StiIcon.Square2,
				StiIcon.Square3,
				StiIcon.Square4,
				StiIcon.StarFull,
				StiIcon.StarThreeFourth,
				StiIcon.StarHalf,
				StiIcon.StarQuarter,
				StiIcon.StarNone
			}, 
			_ => null, 
		};
	}

	public static Image[] GetIcons(StiIconSet iconSet)
	{
		StiIcon[] iconSet2 = GetIconSet(iconSet);
		Image[] array = (Image[])(object)new Image[iconSet2.Length];
		int num = 0;
		StiIcon[] array2 = iconSet2;
		foreach (StiIcon icon in array2)
		{
			array[num] = GetIcon(icon);
			num++;
		}
		return array;
	}

	public static Image GetIcon(StiIcon icon)
	{
		if (icon != 0)
		{
			return GetIcon(icon.ToString());
		}
		return null;
	}

	public static Image GetIcon(StiIconSetIndicator indicator)
	{
		if (indicator.CustomIcon != null)
		{
			try
			{
				if (StiSvgHelper.IsSvg(indicator.CustomIcon))
				{
					object obj = StiSvgHelper.OpenSvg(indicator.CustomIcon);
					SizeF svgSize = StiSvgHelper.GetSvgSize(obj);
					return StiSvgHelper.ConvertSvgToImage(indicator.CustomIcon, (int)svgSize.Width, (int)svgSize.Height, false, true, false);
				}
				MemoryStream memoryStream = new MemoryStream(indicator.CustomIcon);
				return Image.FromStream((Stream)memoryStream);
			}
			catch (Exception)
			{
			}
		}
		if (indicator.Icon != 0)
		{
			return GetIcon(indicator.Icon.ToString());
		}
		return null;
	}

	private static Image GetIcon(string name)
	{
		return (Image)(object)StiImageUtils.GetImage("Stimulsoft.Report", "Stimulsoft.Report.Components.Indicators.Images." + name + ".png");
	}
}
