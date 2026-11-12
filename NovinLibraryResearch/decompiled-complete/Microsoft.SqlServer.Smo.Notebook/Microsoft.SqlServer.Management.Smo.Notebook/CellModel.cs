using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Smo.Notebook;

internal class CellModel
{
	public string cell_type { get; set; }

	public IList<string> source { get; set; }

	public object metadata { get; set; }

	public CellModel(string cellType)
	{
		cell_type = cellType;
		source = new List<string>();
	}
}
