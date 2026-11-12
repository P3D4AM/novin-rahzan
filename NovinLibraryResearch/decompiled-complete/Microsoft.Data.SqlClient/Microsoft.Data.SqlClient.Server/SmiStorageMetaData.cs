using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;

namespace Microsoft.Data.SqlClient.Server;

internal class SmiStorageMetaData : SmiExtendedMetaData
{
	private readonly bool _allowsDBNull;

	private readonly string _serverName;

	private readonly string _catalogName;

	private readonly string _schemaName;

	private readonly string _tableName;

	private readonly string _columnName;

	private readonly SqlBoolean _isKey;

	private readonly bool _isIdentity;

	private readonly bool _isColumnSet;

	internal bool AllowsDBNull => _allowsDBNull;

	internal string ServerName => _serverName;

	internal string CatalogName => _catalogName;

	internal string SchemaName => _schemaName;

	internal string TableName => _tableName;

	internal string ColumnName => _columnName;

	internal SqlBoolean IsKey => _isKey;

	internal bool IsIdentity => _isIdentity;

	internal bool IsColumnSet => _isColumnSet;

	internal SmiStorageMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, string name, string typeSpecificNamePart1, string typeSpecificNamePart2, string typeSpecificNamePart3, bool allowsDBNull, string serverName, string catalogName, string schemaName, string tableName, string columnName, SqlBoolean isKey, bool isIdentity)
		: this(dbType, maxLength, precision, scale, localeId, compareOptions, userDefinedType, isMultiValued: false, null, null, name, typeSpecificNamePart1, typeSpecificNamePart2, typeSpecificNamePart3, allowsDBNull, serverName, catalogName, schemaName, tableName, columnName, isKey, isIdentity)
	{
	}

	internal SmiStorageMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, bool isMultiValued, IList<SmiExtendedMetaData> fieldMetaData, SmiMetaDataPropertyCollection extendedProperties, string name, string typeSpecificNamePart1, string typeSpecificNamePart2, string typeSpecificNamePart3, bool allowsDBNull, string serverName, string catalogName, string schemaName, string tableName, string columnName, SqlBoolean isKey, bool isIdentity)
		: this(dbType, maxLength, precision, scale, localeId, compareOptions, userDefinedType, null, isMultiValued, fieldMetaData, extendedProperties, name, typeSpecificNamePart1, typeSpecificNamePart2, typeSpecificNamePart3, allowsDBNull, serverName, catalogName, schemaName, tableName, columnName, isKey, isIdentity, isColumnSet: false)
	{
	}

	internal SmiStorageMetaData(SqlDbType dbType, long maxLength, byte precision, byte scale, long localeId, SqlCompareOptions compareOptions, Type userDefinedType, string udtAssemblyQualifiedName, bool isMultiValued, IList<SmiExtendedMetaData> fieldMetaData, SmiMetaDataPropertyCollection extendedProperties, string name, string typeSpecificNamePart1, string typeSpecificNamePart2, string typeSpecificNamePart3, bool allowsDBNull, string serverName, string catalogName, string schemaName, string tableName, string columnName, SqlBoolean isKey, bool isIdentity, bool isColumnSet)
		: base(dbType, maxLength, precision, scale, localeId, compareOptions, userDefinedType, udtAssemblyQualifiedName, isMultiValued, fieldMetaData, extendedProperties, name, typeSpecificNamePart1, typeSpecificNamePart2, typeSpecificNamePart3)
	{
		_allowsDBNull = allowsDBNull;
		_serverName = serverName;
		_catalogName = catalogName;
		_schemaName = schemaName;
		_tableName = tableName;
		_columnName = columnName;
		_isKey = isKey;
		_isIdentity = isIdentity;
		_isColumnSet = isColumnSet;
	}

	internal override string TraceString(int indent)
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}{1}         AllowsDBNull={2}\n\t{1}           ServerName='{3}'\n\t{1}          CatalogName='{4}'\n\t{1}           SchemaName='{5}'\n\t{1}            TableName='{6}'\n\t{1}           ColumnName='{7}'\n\t{1}                IsKey={8}\n\t{1}           IsIdentity={9}\n\t", base.TraceString(indent), new string(' ', indent), AllowsDBNull, ServerName ?? "<null>", CatalogName ?? "<null>", SchemaName ?? "<null>", TableName ?? "<null>", ColumnName ?? "<null>", IsKey, IsIdentity);
	}
}
