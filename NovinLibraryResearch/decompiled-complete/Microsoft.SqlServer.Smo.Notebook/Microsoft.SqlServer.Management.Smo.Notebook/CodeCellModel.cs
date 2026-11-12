using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Smo.Notebook;

internal class CodeCellModel : CellModel
{
	public IList<object> outputs { get; set; }

	public int execution_count { get; set; }

	public CodeCellModel()
		: base("code")
	{
		outputs = new List<object>();
	}
}
