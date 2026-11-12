using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

internal class CliXmlView : IDataView, IReadOnlyList<IDataRow>, IReadOnlyCollection<IDataRow>, IEnumerable<IDataRow>, IEnumerable
{
	public XDocument Document { get; }

	public int Count
	{
		get
		{
			XElement root = Document.Root;
			if (root == null)
			{
				return 0;
			}
			return ((XContainer)root).Elements().Count(IsItem);
		}
	}

	public IDataRow this[int index]
	{
		get
		{
			XElement root = Document.Root;
			return CreateDataRow((root != null) ? ((XContainer)root).Elements().ElementAt(index) : null);
		}
	}

	public CliXmlView(XDocument cliXmlDocument)
	{
		Document = cliXmlDocument;
	}

	public IEnumerator<IDataRow> GetEnumerator()
	{
		XElement root = Document.Root;
		return ((root != null) ? ((XContainer)root).Elements().Where(IsItem).Select(CreateDataRow)
			.GetEnumerator() : null) ?? Enumerable.Empty<IDataRow>().GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private IDataRow CreateDataRow(XElement xe)
	{
		return new CliXmlRow(xe);
	}

	private static bool IsItem(XElement element)
	{
		XAttribute val = element.Attribute(CliXml.StreamAttr);
		if (val != null)
		{
			return string.CompareOrdinal(val.Value, "progress") != 0;
		}
		return false;
	}
}
