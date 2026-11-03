using Stimulsoft.Base.Drawing;

namespace Stimulsoft.Report.Export;

public class StiMatrixBorderSidesYCollection
{
	private StiMatrixCacheManager manager;

	private StiMatrix matrix;

	public StiBorderSide this[int row, int column]
	{
		get
		{
			if (matrix.useCacheMode)
			{
				int num = manager.GetMatrixLineData(row).BordersY[column];
				if (num == 0)
				{
					return null;
				}
				return matrix.BorderSides[num - 1];
			}
			return matrix.bordersY2[row, column];
		}
		set
		{
			if (matrix.useCacheMode)
			{
				int borderSideIndex = matrix.GetBorderSideIndex(value);
				manager.GetMatrixLineData(row).BordersY[column] = borderSideIndex;
			}
			else
			{
				matrix.bordersY2[row, column] = value;
			}
		}
	}

	public StiMatrixBorderSidesYCollection(StiMatrixCacheManager manager, StiMatrix matrix)
	{
		this.manager = manager;
		this.matrix = matrix;
	}
}
