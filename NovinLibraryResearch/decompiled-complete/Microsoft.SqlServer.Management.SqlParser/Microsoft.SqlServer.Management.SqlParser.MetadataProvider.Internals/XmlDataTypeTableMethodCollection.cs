using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal sealed class XmlDataTypeTableMethodCollection : DualTypeSortedListCollection<IXmlDataTypeTableMethod, IFunction>
{
	private static readonly XmlDataTypeTableMethodCollection instance;

	public static XmlDataTypeTableMethodCollection Instance => instance;

	static XmlDataTypeTableMethodCollection()
	{
		SystemDataTypeLookup systemDataTypeLookup = SystemDataTypeLookup.Instance;
		_ = ObjectFactoryBase<DataTypeFactory>.Instance;
		XmlDataTypeTableMethod[] collection = new XmlDataTypeTableMethod[1]
		{
			new XmlDataTypeTableMethod("nodes", new ScalarParameter("XQuery", systemDataTypeLookup.NVarCharMax))
		};
		instance = new XmlDataTypeTableMethodCollection();
		instance.AddRange(collection);
	}

	private XmlDataTypeTableMethodCollection()
		: base(5, CollationInfo.Ordinal)
	{
	}
}
