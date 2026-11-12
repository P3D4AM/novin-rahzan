using System.Xml.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

internal class CliXmlRow : IDataRow
{
	private XElement Element { get; }

	public CliXmlRow(XElement element)
	{
		Element = element;
	}

	public bool TryGetData(string column, out object value)
	{
		value = null;
		string streamName = Element.GetStreamName();
		if (string.CompareOrdinal(column, 0, streamName, 0, streamName.Length) == 0)
		{
			return Element.TryGetCliValue(column, streamName.Length, out value);
		}
		return false;
	}
}
