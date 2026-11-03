using System.Collections;
using System.Drawing;

namespace Stimulsoft.Report.Export;

public class StiMatrixBookmarksCollection
{
	private StiMatrixCacheManager manager;

	private StiMatrix matrix;

	public string this[int row, int column]
	{
		get
		{
			if (!matrix.useCacheMode)
			{
				return matrix.bookmarks2[row, column];
			}
			return manager.GetMatrixLineData(row).Bookmarks[column];
		}
		set
		{
			if (matrix.useCacheMode)
			{
				manager.GetMatrixLineData(row).Bookmarks[column] = value;
			}
			else
			{
				matrix.bookmarks2[row, column] = value;
			}
			if (BookmarksTable == null || value == null)
			{
				return;
			}
			if (BookmarksTable.ContainsKey(value))
			{
				Size size = (Size)BookmarksTable[value];
				if (row < size.Height || (row == size.Height && column < size.Width))
				{
					BookmarksTable[value] = new Size(column, row);
				}
			}
			else
			{
				BookmarksTable[value] = new Size(column, row);
			}
		}
	}

	public Hashtable BookmarksTable { get; }

	public StiMatrixBookmarksCollection(StiMatrixCacheManager manager, StiMatrix matrix)
	{
		this.manager = manager;
		this.matrix = matrix;
		if (matrix != null && (matrix.exportFormat == StiExportFormat.Excel || matrix.exportFormat == StiExportFormat.Excel2007))
		{
			BookmarksTable = new Hashtable();
		}
	}
}
