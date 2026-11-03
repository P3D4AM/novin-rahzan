namespace Stimulsoft.Report.Export;

public class StiMatrixCellsCollection
{
	private StiMatrixCacheManager manager;

	private StiMatrix matrix;

	public StiCell this[int row, int column]
	{
		get
		{
			if (!matrix.useCacheMode)
			{
				return matrix.cells2[row, column];
			}
			return manager.GetMatrixLineData(row).Cells[column];
		}
		set
		{
			if (matrix.useCacheMode)
			{
				manager.GetMatrixLineData(row).Cells[column] = value;
			}
			else
			{
				matrix.cells2[row, column] = value;
			}
		}
	}

	public StiMatrixCellsCollection(StiMatrixCacheManager manager, StiMatrix matrix)
	{
		this.manager = manager;
		this.matrix = matrix;
	}
}
