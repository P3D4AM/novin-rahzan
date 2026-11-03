using System;

namespace Stimulsoft.Report.Export;

public class StiMatrixCacheSegment
{
	private int segmentHeight;

	private string cacheGuid;

	private StiMatrixLineData[] lines;

	public string CacheGuid
	{
		get
		{
			if (cacheGuid == null)
			{
				NewCacheGuid();
			}
			return cacheGuid;
		}
		set
		{
			cacheGuid = value;
		}
	}

	public StiMatrixLineData[] Lines
	{
		get
		{
			if (lines == null)
			{
				lines = new StiMatrixLineData[segmentHeight];
			}
			return lines;
		}
		set
		{
			lines = value;
		}
	}

	public bool IsSaved { get; set; }

	public void NewCacheGuid()
	{
		cacheGuid = Guid.NewGuid().ToString().Replace("-", "");
	}

	public void Clear()
	{
		if (lines == null)
		{
			return;
		}
		StiMatrixLineData[] array = lines;
		foreach (StiMatrixLineData stiMatrixLineData in array)
		{
			if (stiMatrixLineData != null)
			{
				stiMatrixLineData.Bookmarks = null;
				stiMatrixLineData.BordersX = null;
				stiMatrixLineData.BordersY = null;
				stiMatrixLineData.Cells = null;
				stiMatrixLineData.CellsMap = null;
				stiMatrixLineData.CellStyles = null;
			}
		}
		lines = null;
	}

	public StiMatrixCacheSegment(int height)
	{
		segmentHeight = height;
	}
}
