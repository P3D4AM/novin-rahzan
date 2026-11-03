using System;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Engine;

internal class StiPageNumberHelper
{
	private bool finished;

	internal StiEngine Engine { get; }

	public bool ClearPageNumbersOnFinish { get; set; } = true;


	public StiPageNumberCollection PageNumbers { get; set; } = new StiPageNumberCollection();


	internal bool IsFinished => finished;

	internal void ResetPageNumber()
	{
		ResetPageNumber(PageNumbers.Count - 1);
	}

	internal void ResetPageNumber(int pageIndex)
	{
		if (pageIndex < PageNumbers.Count && pageIndex >= 0)
		{
			PageNumbers[pageIndex].ResetPageNumber = true;
		}
	}

	internal void AddPageNumber(int pageIndex, int segmentPerWidth, int segmentPerHeight)
	{
		if (pageIndex >= PageNumbers.Count)
		{
			PageNumbers.Add(new StiPageNumber
			{
				SegmentPerWidth = segmentPerWidth,
				SegmentPerHeight = segmentPerHeight
			});
		}
	}

	internal int GetPageNumber(int pageIndex)
	{
		if (!finished)
		{
			if (Engine.Report.RenderedPages.Count == 0)
			{
				return 1;
			}
			if (PageNumbers != null && pageIndex >= 0 && pageIndex < PageNumbers.Count && PageNumbers[pageIndex].ResetPageNumber)
			{
				return 1;
			}
			return Engine.Report.RenderedPages.Count;
		}
		if (PageNumbers.Count == 0)
		{
			return pageIndex + 1;
		}
		pageIndex = Math.Max(pageIndex, 0);
		pageIndex = Math.Min(pageIndex, PageNumbers.Count - 1);
		return PageNumbers[pageIndex].PageNumber;
	}

	internal int GetPageNumber(StiPage page)
	{
		return GetPageNumber(Engine.Report.RenderedPages.IndexOf(page));
	}

	internal int GetTotalPageCount(int pageIndex)
	{
		if (!finished)
		{
			if (Engine.Report.RenderedPages.Count == 0)
			{
				return 1;
			}
			return Engine.Report.RenderedPages.Count;
		}
		if (PageNumbers.Count == 0)
		{
			return 1;
		}
		pageIndex = Math.Max(pageIndex, 0);
		pageIndex = Math.Min(pageIndex, PageNumbers.Count - 1);
		return PageNumbers[pageIndex].TotalPageCount;
	}

	internal int GetTotalPageCount(StiPage page)
	{
		return GetTotalPageCount(Engine.Report.RenderedPages.IndexOf(page));
	}

	internal int GetPageNumberThrough(int pageIndex)
	{
		if (!finished)
		{
			if (Engine.Report.RenderedPages.Count == 0)
			{
				return 1;
			}
			return Engine.Report.RenderedPages.Count;
		}
		if (PageNumbers.Count == 0)
		{
			return 1;
		}
		pageIndex = Math.Max(pageIndex, 0);
		pageIndex = Math.Min(pageIndex, PageNumbers.Count - 1);
		return PageNumbers[pageIndex].PageNumberThrough;
	}

	internal int GetPageNumberThrough(StiPage page)
	{
		return GetPageNumberThrough(Engine.Report.RenderedPages.IndexOf(page));
	}

	internal int GetTotalPageCountThrough(int pageIndex)
	{
		if (PageNumbers.Count == 0)
		{
			return 1;
		}
		pageIndex = Math.Max(pageIndex, 0);
		pageIndex = Math.Min(pageIndex, PageNumbers.Count - 1);
		return PageNumbers[pageIndex].TotalPageCountThrough;
	}

	private void SetSystemVariables(int startIndex, int endIndex)
	{
		if (startIndex != endIndex)
		{
			int num = 0;
			for (int i = startIndex; i < endIndex; i++)
			{
				num += PageNumbers[i].Step;
			}
			int num2 = 1;
			for (int j = startIndex; j < endIndex; j++)
			{
				PageNumbers[j].PageNumber = num2;
				PageNumbers[j].TotalPageCount = num;
				num2 += PageNumbers[j].Step;
			}
		}
	}

	internal void ProcessPageNumbers()
	{
		if (Engine != null && Engine.Report != null && Engine.Report.RenderedPages != null)
		{
			while (PageNumbers.Count > Engine.Report.RenderedPages.Count)
			{
				PageNumbers.RemoveAt(Engine.Report.RenderedPages.Count);
			}
		}
		int num = 0;
		int num2 = 0;
		foreach (StiPageNumber pageNumber in PageNumbers)
		{
			if (Engine != null && Engine.Report != null && Engine.Report.RenderedPages != null && num2 < Engine.Report.RenderedPages.Count)
			{
				StiPage pageWithoutCache = Engine.Report.RenderedPages.GetPageWithoutCache(num2);
				pageNumber.SegmentPerWidth = pageWithoutCache.SegmentPerWidth;
				pageNumber.SegmentPerHeight = pageWithoutCache.SegmentPerHeight;
			}
			num += pageNumber.Step;
			num2++;
		}
		int num3 = 1;
		int startIndex = 0;
		int num4 = 0;
		foreach (StiPageNumber pageNumber2 in PageNumbers)
		{
			if (pageNumber2.ResetPageNumber)
			{
				SetSystemVariables(startIndex, num4);
				startIndex = num4;
			}
			pageNumber2.PageNumberThrough = num3++;
			pageNumber2.TotalPageCountThrough = num;
			num4++;
		}
		SetSystemVariables(startIndex, num4);
		finished = true;
	}

	internal void Clear()
	{
		if (ClearPageNumbersOnFinish)
		{
			PageNumbers.Clear();
		}
	}

	internal void ClearNotFixed()
	{
		if (ClearPageNumbersOnFinish && PageNumbers.Count > 0)
		{
			int num = PageNumbers.Count - 1;
			while (num >= 0 && !PageNumbers[num].FixedPosition)
			{
				PageNumbers.RemoveAt(num);
				num--;
			}
		}
	}

	internal StiPageNumberHelper(StiEngine engine)
	{
		Engine = engine;
	}
}
