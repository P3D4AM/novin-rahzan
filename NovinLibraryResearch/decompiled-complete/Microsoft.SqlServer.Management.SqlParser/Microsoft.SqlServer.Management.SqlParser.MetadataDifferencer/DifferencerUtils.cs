using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataServices;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

internal static class DifferencerUtils
{
	public static bool IsSystemObject(IMetadataObject obj)
	{
		IDatabaseObject databaseObject = obj as IDatabaseObject;
		bool num = databaseObject != null;
		bool flag = obj is ISchema && obj.Name.Equals("dbo", StringComparison.OrdinalIgnoreCase);
		if (num && databaseObject.IsSystemObject)
		{
			return !flag;
		}
		return false;
	}

	public static bool IsPropertySkipped(IMetadataObject obj, MetadataTypeInfo typeInfo, MetadataPropertyInfo propertyInfo)
	{
		bool num = propertyInfo.Id == 4 && MetadataUtils.IsUnnamedObject(obj);
		bool flag = propertyInfo.Id == 37 && obj is IColumn && ((IColumn)obj).ComputedColumnInfo != null;
		return num || flag;
	}

	public static bool IsPropertyTypeComparable(MetadataPropertyInfo propertyInfo)
	{
		if (propertyInfo.PropertyType != 0)
		{
			return propertyInfo.PropertyType == PropertyType.Child;
		}
		return true;
	}
}
