using System;
using System.ComponentModel;
using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report;

public sealed class StiPagesRange
{
	public static readonly StiPagesRange All = new StiPagesRange();

	[DefaultValue(StiRangeType.All)]
	[JsonConverter(typeof(StringEnumConverter))]
	public StiRangeType RangeType { get; set; }

	[DefaultValue("")]
	public string PageRanges { get; set; }

	[DefaultValue(0)]
	public int CurrentPage { get; set; }

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		StiPagesRange stiPagesRange = (StiPagesRange)obj;
		if (RangeType == stiPagesRange.RangeType && PageRanges == stiPagesRange.PageRanges)
		{
			return CurrentPage == stiPagesRange.CurrentPage;
		}
		return false;
	}

	public StiPagesCollection GetSelectedPages(StiPagesCollection originalPages)
	{
		StiPagesCollection stiPagesCollection = new StiPagesCollection(originalPages.Report, originalPages)
		{
			CacheMode = originalPages.CacheMode
		};
		StiPagesCollection.CopyEventsOfPagesCollection(originalPages, stiPagesCollection);
		if (PageRanges == "All")
		{
			RangeType = StiRangeType.All;
		}
		if (RangeType == StiRangeType.Pages)
		{
			if (!string.IsNullOrEmpty(PageRanges))
			{
				string[] array = PageRanges.Split(new char[1] { ',' });
				string[] array2 = array;
				foreach (string text in array2)
				{
					if (text.IndexOf("-", StringComparison.InvariantCulture) == -1)
					{
						try
						{
							if (int.TryParse(text.Trim(), out var result) && result >= 1 && result <= originalPages.Count)
							{
								stiPagesCollection.AddV2Internal(originalPages[result - 1]);
							}
						}
						catch
						{
						}
						continue;
					}
					string[] array3 = text.Split(new char[1] { '-' });
					try
					{
						int num = int.Parse(array3[0].Trim());
						int val = int.Parse(array3[1].Trim());
						val = Math.Min(val, originalPages.Count);
						if (num < 1 || num > originalPages.Count || val < 1 || val > originalPages.Count)
						{
							continue;
						}
						if (num <= val)
						{
							for (int j = num; j <= val; j++)
							{
								stiPagesCollection.AddV2Internal(originalPages[j - 1]);
							}
							continue;
						}
						for (int num2 = num; num2 >= val; num2--)
						{
							stiPagesCollection.AddV2Internal(originalPages[num2 - 1]);
						}
					}
					catch
					{
					}
				}
			}
			if (stiPagesCollection.Count != 0)
			{
				return stiPagesCollection;
			}
		}
		else if (RangeType == StiRangeType.CurrentPage)
		{
			if (CurrentPage >= 0 && CurrentPage < originalPages.Count)
			{
				stiPagesCollection.AddV2Internal(originalPages[CurrentPage]);
			}
			if (stiPagesCollection.Count != 0)
			{
				return stiPagesCollection;
			}
		}
		foreach (StiPage originalPage in originalPages)
		{
			stiPagesCollection.AddV2Internal(originalPage);
		}
		return stiPagesCollection;
	}

	public StiPagesRange()
		: this(StiRangeType.All, "", 0)
	{
	}

	public StiPagesRange(int currentPage)
		: this(StiRangeType.CurrentPage, string.Empty, currentPage)
	{
	}

	public StiPagesRange(string pageRanges)
		: this(StiRangeType.Pages, pageRanges, 0)
	{
	}

	public StiPagesRange(StiRangeType rangeType, string pageRanges, int currentPage)
	{
		RangeType = rangeType;
		PageRanges = pageRanges;
		CurrentPage = currentPage;
	}
}
