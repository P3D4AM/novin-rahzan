using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Smo.Notebook;

internal class NotebookModel
{
	public object metadata { get; set; }

	public int nbformat { get; set; }

	public int nbformat_minor { get; set; }

	public IList<object> cells { get; set; }

	public NotebookModel(int majorVersion, int minorVersion)
	{
		nbformat = majorVersion;
		nbformat_minor = minorVersion;
		cells = new List<object>();
	}
}
