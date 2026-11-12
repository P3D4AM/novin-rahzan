using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal sealed class XmlDataTypeMethodCollection : DualTypeSortedListCollection<IXmlDataTypeMethod, IScalarFunction>
{
	private static readonly XmlDataTypeMethodCollection instance;

	public static XmlDataTypeMethodCollection Instance => instance;

	static XmlDataTypeMethodCollection()
	{
		SystemDataTypeLookup systemDataTypeLookup = SystemDataTypeLookup.Instance;
		DataTypeFactory dataTypeFactory = ObjectFactoryBase<DataTypeFactory>.Instance;
		XmlDataTypeMethod[] collection = new XmlDataTypeMethod[4]
		{
			new XmlDataTypeMethod("query", systemDataTypeLookup.Xml(), true, new ScalarParameter("XQuery", systemDataTypeLookup.NVarCharMax)),
			new XmlDataTypeMethod("value", dataTypeFactory.UnknownScalar, true, new ScalarParameter("XQuery", systemDataTypeLookup.NVarCharMax), new ScalarParameter("SQLType", systemDataTypeLookup.NVarCharMax)),
			new XmlDataTypeMethod("exist", systemDataTypeLookup.Bit, true, new ScalarParameter("XQuery", systemDataTypeLookup.NVarCharMax)),
			new XmlDataTypeMethod("modify", dataTypeFactory.Void, true, new ScalarParameter("XML_DML", systemDataTypeLookup.NVarCharMax))
		};
		instance = new XmlDataTypeMethodCollection();
		instance.AddRange(collection);
	}

	private XmlDataTypeMethodCollection()
		: base(5, CollationInfo.Ordinal)
	{
	}
}
