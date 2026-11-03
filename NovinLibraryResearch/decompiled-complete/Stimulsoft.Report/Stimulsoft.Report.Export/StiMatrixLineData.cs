namespace Stimulsoft.Report.Export;

public class StiMatrixLineData
{
	public StiCell[] Cells;

	public StiCell[] CellsMap;

	public int[] BordersX;

	public int[] BordersY;

	public int[] CellStyles;

	public string[] Bookmarks;

	public StiMatrixLineData(int size)
	{
		Cells = new StiCell[size];
		CellsMap = new StiCell[size];
		BordersX = new int[size];
		BordersY = new int[size];
		CellStyles = new int[size];
		Bookmarks = new string[size];
	}
}
