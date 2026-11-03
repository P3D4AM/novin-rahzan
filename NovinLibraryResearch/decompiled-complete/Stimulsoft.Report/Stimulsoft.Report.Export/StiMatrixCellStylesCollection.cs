namespace Stimulsoft.Report.Export;

public class StiMatrixCellStylesCollection
{
	private StiMatrixCacheManager manager;

	private StiMatrix matrix;

	public StiCellStyle this[int row, int column]
	{
		get
		{
			if (matrix.useCacheMode)
			{
				int num = manager.GetMatrixLineData(row).CellStyles[column];
				if (num == 0)
				{
					return null;
				}
				return matrix.Styles[num - 1] as StiCellStyle;
			}
			return matrix.cellStyles2[row, column];
		}
		set
		{
			if (matrix.useCacheMode)
			{
				manager.GetMatrixLineData(row).CellStyles[column] = matrix.Styles.IndexOf(value) + 1;
			}
			else
			{
				matrix.cellStyles2[row, column] = value;
			}
		}
	}

	public StiMatrixCellStylesCollection(StiMatrixCacheManager manager, StiMatrix matrix)
	{
		this.manager = manager;
		this.matrix = matrix;
	}
}
