using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Dashboard.Components;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Dashboard;
using Stimulsoft.Report.Dashboard.Helpers;

namespace Stimulsoft.Dashboard.Helpers;

public static class StiDashboardSizeHelper
{
	public class StiRangeBand
	{
		public int Top { get; set; }

		public int Bottom { get; set; }

		public int Height => Bottom - Top;

		public int OriginalTop { get; }

		public int OriginalBottom { get; }

		public int OriginalHeight => OriginalBottom - OriginalTop;

		public bool IsFixed { get; set; }

		public override string ToString()
		{
			string text = (IsFixed ? ", Fixed" : "");
			return $"Top-{Top}, Bottom-{Bottom}, Height-{Height}{text}";
		}

		public bool Intersect(Rectangle rect)
		{
			if (rect.Bottom > Top)
			{
				return rect.Top < Bottom;
			}
			return false;
		}

		public StiRangeBand(int top, int bottom)
		{
			OriginalTop = (Top = top);
			OriginalBottom = (Bottom = bottom);
		}
	}

	public static void Place(StiDashboard dashboard, double prevWidth, double prevHeight)
	{
		if (dashboard == null)
		{
			return;
		}
		IEnumerable<StiComponent> enumerable = from c in dashboard.GetComponents().ToList()
			where c is IStiElement
			select c;
		double num = dashboard.Width / prevWidth;
		Dictionary<StiComponent, Rectangle> rects = enumerable.ToDictionary((StiComponent e) => e, (StiComponent e) => Round(e.GetPaintRectangle(convertToHInches: false, convertByZoom: false)));
		List<int> allLocations = GetAllLocations(enumerable, rects, prevHeight);
		List<StiRangeBand> list = new List<StiRangeBand>();
		for (int i = 0; i < allLocations.Count - 1; i++)
		{
			list.Add(new StiRangeBand(allLocations[i], allLocations[i + 1]));
		}
		foreach (StiComponent component2 in enumerable.Where((StiComponent e) => e is IStiFixedHeightElement && (e as IStiFixedHeightElement).IsFixedHeight))
		{
			list.Where((StiRangeBand b) => b.Intersect(rects[component2])).ToList().ForEach(delegate(StiRangeBand c)
			{
				c.IsFixed = true;
			});
		}
		int num2 = list.Where((StiRangeBand b) => b.IsFixed).Sum((StiRangeBand b) => b.Height);
		double num3 = (dashboard.Height - (double)num2) / (prevHeight - (double)num2);
		int num4 = 0;
		foreach (StiRangeBand item in list)
		{
			item.Top = num4;
			int num5 = Round(item.IsFixed ? ((double)item.OriginalHeight) : ((double)item.OriginalHeight * num3));
			item.Bottom = item.Top + num5;
			num4 += num5;
		}
		enumerable = TopUpFilterElements(enumerable);
		foreach (StiComponent component in enumerable)
		{
			int num6 = Round((double)rects[component].Left * num);
			int num7 = Round((double)rects[component].Right * num);
			int top = list.FirstOrDefault((StiRangeBand b) => b.OriginalTop == rects[component].Top).Top;
			int bottom = list.FirstOrDefault((StiRangeBand b) => b.OriginalBottom == rects[component].Bottom).Bottom;
			component.ClientRectangle = new RectangleD(num6, top, num7 - num6, bottom - top);
		}
	}

	private static List<int> GetAllLocations(IEnumerable<StiComponent> components, Dictionary<StiComponent, Rectangle> rects, double height)
	{
		List<int> list = new List<int>();
		foreach (StiComponent component in components)
		{
			list.Add(Round(rects[component].Top));
			list.Add(Round(rects[component].Bottom));
		}
		list.Add(0);
		list.Add(Round(height));
		return (from y in list.Distinct()
			orderby y
			select y).ToList();
	}

	private static IEnumerable<StiComponent> TopUpFilterElements(IEnumerable<StiComponent> elements)
	{
		List<IStiFilterElement> filterElements = elements.Where((StiComponent e) => e is IStiFilterElement).Cast<IStiFilterElement>().ToList();
		filterElements = filterElements.OrderBy((IStiFilterElement f) => GetLevel(f, filterElements)).ToList();
		List<StiComponent> collection = elements.Where((StiComponent e) => !(e is IStiFilterElement)).ToList();
		List<StiComponent> list = new List<StiComponent>();
		list.AddRange(filterElements.Cast<StiComponent>().ToList());
		list.AddRange(collection);
		return list;
	}

	private static int GetLevel(IStiFilterElement element, List<IStiFilterElement> elements)
	{
		if (StiCrossLinkedFilterHelper.IsCrossLinkedFilter(element))
		{
			return 0;
		}
		int num = 0;
		while (true)
		{
			if (string.IsNullOrWhiteSpace(element.GetParentKey()))
			{
				return num;
			}
			element = elements.FirstOrDefault((IStiFilterElement e) => e.GetKey() == element.GetParentKey());
			if (element == null)
			{
				break;
			}
			num++;
		}
		return num;
	}

	private static int Round(double value)
	{
		return (int)Math.Round(value, MidpointRounding.AwayFromZero);
	}

	private static Rectangle Round(RectangleD rect)
	{
		return new Rectangle(Round(rect.Left), Round(rect.Top), Round(rect.Width), Round(rect.Height));
	}
}
