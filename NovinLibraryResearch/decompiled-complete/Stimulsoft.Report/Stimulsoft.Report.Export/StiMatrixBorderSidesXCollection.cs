using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Export;

public class StiMatrixBorderSidesXCollection
{
	private StiMatrixCacheManager manager;

	private StiMatrix matrix;

	public StiBorderSide this[int row, int column]
	{
		get
		{
			if (matrix.useCacheMode)
			{
				int num = manager.GetMatrixLineData(row).BordersX[column];
				if (num != 0)
				{
					return matrix.BorderSides[num - 1];
				}
				return null;
			}
			return matrix.bordersX2[row, column];
		}
		set
		{
			if (matrix.useCacheMode)
			{
				int borderSideIndex = matrix.GetBorderSideIndex(value);
				manager.GetMatrixLineData(row).BordersX[column] = borderSideIndex;
			}
			else
			{
				matrix.bordersX2[row, column] = value;
			}
		}
	}

	public StiMatrixBorderSidesXCollection(StiMatrixCacheManager manager, StiMatrix matrix)
	{
		this.manager = manager;
		this.matrix = matrix;
	}
}
