using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;

namespace Microsoft.Data.SqlClient.Server;

internal class SmiExtendedMetaData : SmiMetaData
{
	private readonly string _name;

	private readonly string _typeSpecificNamePart1;

	private readonly string _typeSpecificNamePart2;

	private readonly string _typeSpecificNamePart3;

	internal string Name => _name;

	internal string TypeSpecificNamePart1 => _typeSpecificNamePart1;

	internal string TypeSpecificNamePart2 => _typeSpecificNamePart2;

	internal string TypeSpecificNamePart3 => _typeSpecificNamePart3;

	internal SmiExtendedMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, string name, string typeSpecificNamePart1, string typeSpecificNamePart2, string typeSpecificNamePart3)
		: this(dbType, maxLength, precision, scale, localeId, compareOptions, userDefinedType, isMultiValued: false, null, null, name, typeSpecificNamePart1, typeSpecificNamePart2, typeSpecificNamePart3)
	{
	}

	internal SmiExtendedMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, bool isMultiValued, IList<SmiExtendedMetaData> fieldMetaData, SmiMetaDataPropertyCollection extendedProperties, string name, string typeSpecificNamePart1, string typeSpecificNamePart2, string typeSpecificNamePart3)
		: this(dbType, maxLength, precision, scale, localeId, compareOptions, userDefinedType, null, isMultiValued, fieldMetaData, extendedProperties, name, typeSpecificNamePart1, typeSpecificNamePart2, typeSpecificNamePart3)
	{
	}

	internal SmiExtendedMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, string udtAssemblyQualifiedName, bool isMultiValued, IList<SmiExtendedMetaData> fieldMetaData, SmiMetaDataPropertyCollection extendedProperties, string name, string typeSpecificNamePart1, string typeSpecificNamePart2, string typeSpecificNamePart3)
		: base(dbType, maxLength, precision, scale, localeId, compareOptions, userDefinedType, udtAssemblyQualifiedName, isMultiValued, fieldMetaData, extendedProperties)
	{
		_name = name;
		_typeSpecificNamePart1 = typeSpecificNamePart1;
		_typeSpecificNamePart2 = typeSpecificNamePart2;
		_typeSpecificNamePart3 = typeSpecificNamePart3;
	}

	internal override string TraceString(int indent)
	{
		return string.Format(CultureInfo.InvariantCulture, "{2}                 Name={0}{1}{2}TypeSpecificNamePart1='{3}'\n\t{2}TypeSpecificNamePart2='{4}'\n\t{2}TypeSpecificNamePart3='{5}'\n\t", _name ?? "<null>", base.TraceString(indent), new string(' ', indent), TypeSpecificNamePart1 ?? "<null>", TypeSpecificNamePart2 ?? "<null>", TypeSpecificNamePart3 ?? "<null>");
	}
}
