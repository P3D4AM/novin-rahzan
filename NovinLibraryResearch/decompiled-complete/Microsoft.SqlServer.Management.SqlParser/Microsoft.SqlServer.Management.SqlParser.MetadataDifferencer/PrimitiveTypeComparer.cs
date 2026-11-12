using System.Collections;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;
using Microsoft.SqlServer.Management.SqlParser.MetadataServices;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataDifferencer;

internal class PrimitiveTypeComparer : PropertyComparer
{
	public PrimitiveTypeComparer(PropertyComparerFactory creator, ChangeResultBuilder changeResultBuilder, IDifferencerFilter differenceFilter)
		: base(creator, changeResultBuilder, differenceFilter)
	{
	}

	public override bool Process(IMetadataObject sourceParent, IMetadataObject targetParent, MetadataPropertyInfo propertyInfo, bool addToChangeResult)
	{
		MetadataTypeInfo typeInfo = sourceParent.TypeInfo();
		object propertyValue = MetadataUtils.GetPropertyValue(sourceParent, typeInfo, propertyInfo);
		object propertyValue2 = MetadataUtils.GetPropertyValue(targetParent, typeInfo, propertyInfo);
		IEqualityComparer equalityComparer;
		bool flag = (((equalityComparer = base.DifferenceFilter.GetEqualityComparer(typeInfo, propertyInfo)) == null) ? Compare(propertyValue, propertyValue2, propertyInfo) : equalityComparer.Equals(propertyValue, propertyValue2));
		if (!flag && addToChangeResult)
		{
			base.ChangeResultBuilder.AddModifiedEntry(sourceParent, targetParent, new ScalarDifference(propertyInfo.Name, propertyValue, propertyValue2));
		}
		return flag;
	}

	public bool Compare(object sourceValue, object targetValue, MetadataPropertyInfo propertyInfo)
	{
		return propertyInfo.ReturnType.Id switch
		{
			3 => EqualityComparer.ByteArray.Equals((byte[])sourceValue, (byte[])targetValue), 
			5 => EqualityComparer.CollationInfo.Equals((CollationInfo)sourceValue, (CollationInfo)targetValue), 
			6 => EqualityComparer.ComputedColumnInfo.Equals((ComputedColumnInfo)sourceValue, (ComputedColumnInfo)targetValue), 
			8 => EqualityComparer.DataTypeSpec.Equals((DataTypeSpec)sourceValue, (DataTypeSpec)targetValue), 
			137 => EqualityComparer.IdentityColumnInfo.Equals((IdentityColumnInfo)sourceValue, (IdentityColumnInfo)targetValue), 
			139 => EqualityComparer.Int.Equals((int)sourceValue, (int)targetValue), 
			89 => EqualityComparer.Password.Equals((IPassword)sourceValue, (IPassword)targetValue), 
			146 => EqualityComparer.String.Equals((string)sourceValue, (string)targetValue), 
			_ => AreDefaultEqual(sourceValue, targetValue), 
		};
	}

	private static bool AreDefaultEqual(object source, object target)
	{
		if (source == null)
		{
			return target == null;
		}
		if (target == null)
		{
			return false;
		}
		return source.Equals(target);
	}
}
