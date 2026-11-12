using System;
using System.Collections;
using System.Collections.Generic;
using Stimulsoft.Base;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;

namespace Stimulsoft.Dashboard.Helpers;

internal static class StiElementLocationHelper
{
	private static Hashtable hashLocations = new Hashtable();

	internal static RectangleD GetSumRectangle(List<RectangleD> listLocationRectangles)
	{
		RectangleD result = new RectangleD(0.0, 0.0, 0.0, 0.0);
		foreach (RectangleD listLocationRectangle in listLocationRectangles)
		{
			if (listLocationRectangle.Right > result.Right)
			{
				result = new RectangleD(result.X, result.Y, listLocationRectangle.Right - result.X, result.Height);
			}
			if (listLocationRectangle.Bottom > result.Bottom)
			{
				result = new RectangleD(result.X, result.Y, result.Width, listLocationRectangle.Bottom - result.Y);
			}
		}
		return result;
	}

	internal static List<RectangleD> GetLocationRectangles(RectangleD rect, int countRect, IStiElement element)
	{
		if (countRect == 1)
		{
			return new List<RectangleD> { CheckMinRect(rect, element) };
		}
		StiElementLocationKey stiElementLocationKey = new StiElementLocationKey(rect, countRect);
		if (hashLocations.Contains(stiElementLocationKey))
		{
			return hashLocations[stiElementLocationKey] as List<RectangleD>;
		}
		RectangleD rectangleD = GetSimpleRect(rect, countRect, element);
		RectangleD simpleRect = GetSimpleRect(new RectangleD(rect.X, rect.Y, rect.Height, rect.Width), countRect, element);
		double num = rectangleD.Width * rectangleD.Height;
		double num2 = simpleRect.Width * simpleRect.Height;
		if (num2 > num)
		{
			rectangleD = new RectangleD(simpleRect.X, simpleRect.Y, simpleRect.Height, simpleRect.Width);
		}
		List<RectangleD> list = new List<RectangleD>();
		double x = rectangleD.X;
		double y = rectangleD.Y;
		for (int i = 0; i < countRect; i++)
		{
			RectangleD item = new RectangleD(x, y, rectangleD.Width, rectangleD.Height);
			list.Add(item);
			if (Math.Floor(item.Right + item.Width) <= rect.Right)
			{
				x = item.Right;
				continue;
			}
			y = item.Bottom;
			x = rect.X;
		}
		hashLocations.Add(stiElementLocationKey, list);
		RectangleD sumRectangle = GetSumRectangle(list);
		StiElementLocationKey stiElementLocationKey2 = new StiElementLocationKey(sumRectangle, countRect);
		if (!hashLocations.Contains(stiElementLocationKey2))
		{
			hashLocations.Add(stiElementLocationKey2, list);
		}
		return list;
	}

	private static RectangleD GetMinRectangle(RectangleD rect, int count)
	{
		double num = rect.Width * rect.Height / (double)count;
		RectangleD result = rect;
		while (result.Width * result.Height > num || result.Equals(RectangleD.Empty))
		{
			result = ((!(result.Width > result.Height)) ? new RectangleD(result.X, result.Y, result.Width, result.Height / 2.0) : new RectangleD(result.X, result.Y, result.Width / 2.0, result.Height));
		}
		return result;
	}

	private static void PrepareColumnRow(out int column, out int row, RectangleD rect, RectangleD singleRect, int countRect)
	{
		column = 1;
		row = 1;
		double x = singleRect.X;
		double y = singleRect.Y;
		for (int i = 0; i < countRect - 1; i++)
		{
			RectangleD rectangleD = new RectangleD(x, y, singleRect.Width, singleRect.Height);
			if (rectangleD.Right + rectangleD.Width <= rect.Right)
			{
				if (row == 1)
				{
					column++;
				}
				x = rectangleD.Right;
			}
			else
			{
				row++;
				y = rectangleD.Bottom;
				x = rect.X;
			}
		}
	}

	private static RectangleD GetUsingRect(RectangleD rect, RectangleD singleRect, int countRect)
	{
		RectangleD result = singleRect;
		double x = singleRect.X;
		double y = singleRect.Y;
		double num = singleRect.Width;
		double num2 = singleRect.Height;
		for (int i = 0; i < countRect; i++)
		{
			RectangleD rectangleD = new RectangleD(x, y, singleRect.Width, singleRect.Height);
			if (num < rectangleD.Right - rect.X)
			{
				num = rectangleD.Right - rect.X;
			}
			if (num2 < rectangleD.Bottom - rect.Y)
			{
				num2 = rectangleD.Bottom - rect.Y;
			}
			result = new RectangleD(rect.X, rect.Y, num, num2);
			if (rectangleD.Right + rectangleD.Width <= rect.Right)
			{
				x = rectangleD.Right;
				continue;
			}
			y = rectangleD.Bottom;
			x = rect.X;
		}
		return result;
	}

	private static RectangleD CheckMinRect(RectangleD rect, IStiElement element)
	{
		double num = StiElementScale.Factor(element) * StiScale.Factor;
		if (rect.Width < 80.0 * num)
		{
			rect.Width = 80.0 * num;
		}
		if (rect.Height < 50.0 * num)
		{
			rect.Height = 50.0 * num;
		}
		return rect;
	}

	private static RectangleD GetSimpleRect(RectangleD rect, int countRect, IStiElement element)
	{
		RectangleD minRectangle = GetMinRectangle(rect, countRect);
		minRectangle = CheckMinRect(minRectangle, element);
		PrepareColumnRow(out var column, out var row, rect, minRectangle, countRect);
		RectangleD usingRect = GetUsingRect(rect, minRectangle, countRect);
		if (usingRect.Width < rect.Width)
		{
			minRectangle = new RectangleD(rect.X, rect.Y, minRectangle.Width + (rect.Width - usingRect.Width) / (double)column, minRectangle.Height);
		}
		if (usingRect.Height < rect.Height)
		{
			minRectangle = new RectangleD(rect.X, rect.Y, minRectangle.Width, minRectangle.Height + (rect.Height - usingRect.Height) / (double)row);
		}
		return minRectangle;
	}
}
