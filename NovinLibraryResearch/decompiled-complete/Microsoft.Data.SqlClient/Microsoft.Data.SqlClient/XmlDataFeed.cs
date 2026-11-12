using System.Xml;

namespace Microsoft.Data.SqlClient;

internal class XmlDataFeed : DataFeed
{
	internal XmlReader _source;

	internal XmlDataFeed(XmlReader source)
	{
		_source = source;
	}
}
