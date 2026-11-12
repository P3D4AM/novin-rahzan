using System;

namespace Stimulsoft.Base;

public sealed class StiAlignValue
{
	public static double AlignToMaxGrid(double value, double gridSize, bool aligningToGrid)
	{
		if (!aligningToGrid)
		{
			return value;
		}
		double num = Math.Round(value / gridSize) * gridSize;
		if (value > num)
		{
			num += gridSize;
		}
		return num;
	}

	public static decimal AlignToMaxGrid(decimal value, decimal gridSize, bool aligningToGrid)
	{
		if (!aligningToGrid)
		{
			return value;
		}
		decimal num = Math.Round(value / gridSize) * gridSize;
		if (value > num)
		{
			num += gridSize;
		}
		return num;
	}

	public static double AlignToMinGrid(double value, double gridSize, bool aligningToGrid)
	{
		if (!aligningToGrid)
		{
			return value;
		}
		double num = Math.Round(value / gridSize) * gridSize;
		if (value < num)
		{
			num -= gridSize;
		}
		return num;
	}

	public static double AlignToGrid(double value, double gridSize, bool aligningToGrid)
	{
		if (!aligningToGrid)
		{
			return value;
		}
		return Math.Round(value / gridSize) * gridSize;
	}
}
