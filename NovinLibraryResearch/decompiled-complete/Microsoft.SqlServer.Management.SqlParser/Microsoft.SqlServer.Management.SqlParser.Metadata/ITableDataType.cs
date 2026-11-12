namespace Microsoft.SqlServer.Management.SqlParser.Metadata;

[Metadata(MetadataType.Value)]
[ReadOnly("Name")]
[Value(InheritanceType.NotInherited, "Name")]
[Constant(InheritanceType.NotInherited, "IsUnknown", true)]
[Constant("TabularType", TabularType.TableDataType)]
[Constant(InheritanceType.NotInherited, "CollationInfo", SpecialValue.DefaultCollationInfo)]
public interface ITableDataType : IDataType, IMetadataObject, IDatabaseTable, ITabular
{
}
