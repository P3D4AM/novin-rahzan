namespace Stimulsoft.Report.Engine;

internal class StiPageNumber
{
	public bool ResetPageNumber { get; set; }

	public int PageNumber { get; set; } = -1;


	public int TotalPageCount { get; set; } = -1;


	public int PageNumberThrough { get; set; } = -1;


	public int TotalPageCountThrough { get; set; } = -1;


	public int SegmentPerWidth { get; set; } = 1;


	public int SegmentPerHeight { get; set; } = 1;


	public int Step => SegmentPerWidth * SegmentPerHeight;

	internal bool FixedPosition { get; set; }
}
