using Microsoft.SqlServer.Management.SqlParser.Common;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal interface ISerializationHandler<TId>
{
	TId RegisterValue(IMetadataObject obj, IMetadataObject parentObj, bool isChild);

	TId RegisterValue<T>(IMetadataCollection<T> obj, IMetadataObject parentObj, bool isChild) where T : IMetadataObject;

	TId RegisterValue(bool value);

	TId RegisterValue(byte value);

	TId RegisterValue(byte[] value);

	TId RegisterValue(double value);

	TId RegisterValue(int value);

	TId RegisterValue(string value);

	TId RegisterValue(ActivationOrder value);

	TId RegisterValue(DatabaseCompatibilityLevel value);

	TId RegisterValue(ExecutionContextType value);

	TId RegisterValue(ForeignKeyAction value);

	TId RegisterValue(GridDensity value);

	TId RegisterValue(IndexType value);

	TId RegisterValue(LoginType value);

	TId RegisterValue(SortOrder value);

	TId RegisterValue(StatisticsType value);

	TId RegisterValue(TabularType value);

	TId RegisterValue(UserType value);

	TId RegisterValue(DatabasePermissionType value);

	TId RegisterValue(PermissionState value);

	TId RegisterValue(SynonymBaseType value);

	TId RegisterValue(ComputedColumnInfo value);

	TId RegisterValue(IdentityColumnInfo value);

	TId RegisterValue(IPassword value);

	TId RegisterValue(ITriggerEventTypeSet value);

	TId RegisterValue(CollationInfo value);

	TId RegisterValue(DataTypeSpec value);
}
