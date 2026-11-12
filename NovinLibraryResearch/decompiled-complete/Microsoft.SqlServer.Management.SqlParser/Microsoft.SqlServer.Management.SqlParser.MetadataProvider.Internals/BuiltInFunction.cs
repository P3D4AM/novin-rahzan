using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider.Internals;

internal class BuiltInFunction : MetadataObjectBase, IBuiltInFunction, IScalarFunction, IFunction, IMetadataObject, IFunctionModuleBase, IScalar
{
	private static class ListHelper
	{
		public static readonly BuiltInFunction[] BuiltIns;

		public static readonly BuiltInFunction[] AggregateFunctions;

		public static readonly BuiltInFunction[] ParameterlessFunctions;

		public static readonly BuiltInFunction[] RankingFunctions;

		public static readonly BuiltInFunction[] MissingFunctions;

		public static BuiltInFunction[] CreateBuiltInList(SystemDataTypeLookup dataTypes)
		{
			return new BuiltInFunction[296]
			{
				Create("IS_CALLERSIGNED", dataTypes.Int, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("Param2", dataTypes.IgnoreArgs.NVarChar)),
				Create("NORMALIZE", dataTypes.IgnoreArgs.VarBinary, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.Bit)),
				Create("GEN_NORM_TABLES", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.Int)),
				Create("NORMALIZE_DENORMALIZE", dataTypes.Bit, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.Bit)),
				Create("DECOMPRESSNUMERIC", dataTypes.Variant, 3, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.VarBinary), new ScalarParameter("Param2", dataTypes.TinyInt), new ScalarParameter("Param3", dataTypes.TinyInt)),
				Create("COMPAREVARDECIMAL", dataTypes.Int, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.VarBinary), new ScalarParameter("Param2", dataTypes.IgnoreArgs.VarBinary)),
				Create("GETBINARYSPARSEVECTOR", dataTypes.IgnoreArgs.VarBinary, 0, -1, BuiltInFunctionAttributes.Nullable),
				Create("CONVERTRESVTOSTRING", dataTypes.IgnoreArgs.VarChar, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.VarBinary)),
				Create("DECOMPRESSSCALAR", dataTypes.IgnoreArgs.VarBinary, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.VarBinary), new ScalarParameter("Param2", dataTypes.TinyInt)),
				Create("COMPRESSSCALAR", dataTypes.IgnoreArgs.VarBinary, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.Int)),
				Create("COMPARECOMPRESSEDSCALARS", dataTypes.Int, 3, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.VarBinary), new ScalarParameter("Param2", dataTypes.IgnoreArgs.VarBinary), new ScalarParameter("Param3", dataTypes.TinyInt)),
				Create("CHECKSUM", dataTypes.Int, 1, -1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType)),
				Create("DAY", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.DateTime)),
				Create("ISNULL", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType), new ScalarParameter("replacement_value", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType)),
				Create("CONVERT", ObjectFactoryBase<DataTypeFactory>.Instance.DataTypeName, 3, 3, BuiltInFunctionAttributes.MultiReturnType | BuiltInFunctionAttributes.Nullable, new ScalarParameter("target_type", ObjectFactoryBase<DataTypeFactory>.Instance.DataTypeName), new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType), new ScalarParameter("date_style", dataTypes.SmallInt)),
				Create("PARSE", ObjectFactoryBase<DataTypeFactory>.Instance.DataTypeName, 2, 3, BuiltInFunctionAttributes.MultiReturnType | BuiltInFunctionAttributes.Nullable, new ScalarParameter("string_value", dataTypes.NVarChar(4000)), new ScalarParameter("data_type", ObjectFactoryBase<DataTypeFactory>.Instance.DataTypeName), new ScalarParameter("culture", dataTypes.SysName)),
				Create("TRY_PARSE", ObjectFactoryBase<DataTypeFactory>.Instance.DataTypeName, 2, 3, BuiltInFunctionAttributes.MultiReturnType | BuiltInFunctionAttributes.Nullable, new ScalarParameter("string_value", dataTypes.NVarChar(4000)), new ScalarParameter("data_type", ObjectFactoryBase<DataTypeFactory>.Instance.DataTypeName), new ScalarParameter("culture", dataTypes.SysName)),
				Create("TRY_CAST", ObjectFactoryBase<DataTypeFactory>.Instance.DataTypeName, 2, 3, BuiltInFunctionAttributes.MultiReturnType | BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.NVarChar(4000)), new ScalarParameter("data_type", ObjectFactoryBase<DataTypeFactory>.Instance.DataTypeName), new ScalarParameter("length", dataTypes.Int)),
				Create("YEAR", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.DateTime)),
				Create("CAST", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.MultiReturnType | BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType)),
				Create("FORMATMESSAGE", dataTypes.IgnoreArgs.NVarChar, 1, -1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("message_number|message_text", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType)),
				Create("TRANSLATE", dataTypes.IgnoreArgs.NVarChar, 3, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("input_string", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("characters", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("translations", dataTypes.IgnoreArgs.NVarChar)),
				Create("FORMAT", dataTypes.IgnoreArgs.NVarChar, 2, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("value_to_format", dataTypes.TinyInt), new ScalarParameter("format", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("culture", dataTypes.IgnoreArgs.NVarChar)),
				Create("BINARY_CHECKSUM", dataTypes.Int, 1, -1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType)),
				Create("PARTITION_FRAGMENT_ID", dataTypes.Int, 4, -1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.Int), new ScalarParameter("Param2", dataTypes.Int), new ScalarParameter("Param3", dataTypes.Int), new ScalarParameter("Param4", dataTypes.Int)),
				Create("MONTH", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.DateTime)),
				Create("INDEXKEY_PROPERTY", dataTypes.Int, 4, 4, BuiltInFunctionAttributes.Nullable, new ScalarParameter("table_id", dataTypes.Int), new ScalarParameter("index_id", dataTypes.Int), new ScalarParameter("key_id", dataTypes.SmallInt), new ScalarParameter("property_name", dataTypes.IgnoreArgs.VarChar)),
				Create("ISNUMERIC", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType)),
				Create("DATABASEPROPERTYEX", dataTypes.Variant, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("database_name", dataTypes.NVarChar(128)), new ScalarParameter("property_name", dataTypes.IgnoreArgs.VarChar)),
				Create("@@TOTAL_READ", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("ORIGINAL_DB_NAME", dataTypes.SysName, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("TODATETIMEOFFSET", dataTypes.IgnoreArgs.DateTimeOffset, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("datetime_expression", dataTypes.IgnoreArgs.DateTime2), new ScalarParameter("timezoneoffset_expression_integer|timezoneoffset_expression_string", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType)),
				Create("ERROR_SEVERITY", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("CONTEXT_INFO", dataTypes.IgnoreArgs.VarBinary, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("SESSION_CONTEXT", dataTypes.Variant, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("key", dataTypes.SysName)),
				Create("CURRENT_TRANSACTION_ID", dataTypes.BigInt, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("XML_SCHEMA_NAMESPACE", dataTypes.Xml(), 2, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("relational_schema", dataTypes.SysName), new ScalarParameter("xml_schema_collection_name", dataTypes.SysName), new ScalarParameter("namespace", dataTypes.NVarChar(4000))),
				Create("ASIN", dataTypes.Float, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.Float)),
				Create("STUFF", dataTypes.IgnoreArgs.VarChar, 4, 4, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression_to_be_searched", dataTypes.IgnoreArgs.VarChar), new ScalarParameter("starting_position", dataTypes.Int), new ScalarParameter("number_of_chars", dataTypes.Int), new ScalarParameter("replacement_expression", dataTypes.IgnoreArgs.VarChar)),
				Create("RTRIM", dataTypes.IgnoreArgs.NVarChar, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.IgnoreArgs.NVarChar)),
				Create("SCHEMA_NAME", dataTypes.SysName, 0, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("schema_id", dataTypes.Int)),
				Create("@@MICROSOFTVERSION", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("@@TOTAL_WRITE", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("ATN2", dataTypes.Float, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.Float), new ScalarParameter("expression", dataTypes.Float)),
				Create("STATS_DATE", dataTypes.DateTime, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("table_id", dataTypes.Int), new ScalarParameter("index_id", dataTypes.Int)),
				Create("LTRIM", dataTypes.IgnoreArgs.NVarChar, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.IgnoreArgs.NVarChar)),
				Create("PWDENCRYPT", dataTypes.IgnoreArgs.VarBinary, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.NVarChar)),
				Create("DATABASE_PRINCIPAL_ID", dataTypes.Int, 0, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("principal_name", dataTypes.SysName)),
				Create("RADIANS", ObjectFactoryBase<DataTypeFactory>.Instance.ExpressionType, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.NumericType)),
				Create("@@IO_BUSY", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("ATAN", dataTypes.Float, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.Float)),
				Create("DATEADD", dataTypes.SmallDateTime, 3, 3, BuiltInFunctionAttributes.DatePart | BuiltInFunctionAttributes.Nullable, new ScalarParameter("interval", ObjectFactoryBase<DataTypeFactory>.Instance.DatePart), new ScalarParameter("increment", dataTypes.Int), new ScalarParameter("expression", dataTypes.SmallDateTime)),
				Create("CHANGE_TRACKING_CURRENT_VERSION", dataTypes.BigInt, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("PUBLISHINGSERVERNAME", dataTypes.IgnoreArgs.NVarChar, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("@@PACKET_ERRORS", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("USER_NAME", dataTypes.NVarChar(128), 0, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("user_id", dataTypes.Int)),
				Create("PERMISSIONS", dataTypes.Int, 0, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("object_id", dataTypes.Int), new ScalarParameter("column_name", dataTypes.NVarChar(128))),
				Create("BCPCOLLATIONNAME", dataTypes.IgnoreArgs.NVarChar, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.Binary), new ScalarParameter("Param2", dataTypes.Int)),
				Create("LOG", dataTypes.Float, 1, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.Float), new ScalarParameter("Param2", dataTypes.Float)),
				Create("@@LOCK_TIMEOUT", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("INDEX_COL", dataTypes.NVarChar(256), 3, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("table_name", dataTypes.NVarChar(128)), new ScalarParameter("index_id", dataTypes.Int), new ScalarParameter("key_id", dataTypes.Int)),
				Create("DATEDIFF", dataTypes.Int, 3, 3, BuiltInFunctionAttributes.DatePart | BuiltInFunctionAttributes.Nullable, new ScalarParameter("interval", ObjectFactoryBase<DataTypeFactory>.Instance.DatePart), new ScalarParameter("starting_date", dataTypes.DateTime), new ScalarParameter("ending_date", dataTypes.DateTime)),
				Create("DATEDIFF_BIG", dataTypes.BigInt, 3, 3, BuiltInFunctionAttributes.DatePart | BuiltInFunctionAttributes.Nullable, new ScalarParameter("interval", ObjectFactoryBase<DataTypeFactory>.Instance.DatePart), new ScalarParameter("starting_date", dataTypes.DateTime), new ScalarParameter("ending_date", dataTypes.DateTime)),
				Create("@@FETCH_STATUS", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("NT_CLIENT", dataTypes.IgnoreArgs.NVarChar, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("OBJECTPROPERTY", dataTypes.Int, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("object_id", dataTypes.Int), new ScalarParameter("property_name", dataTypes.IgnoreArgs.VarChar)),
				Create("QUOTENAME", dataTypes.NVarChar(128), 1, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.NVarChar(128)), new ScalarParameter("quote_character", dataTypes.IgnoreArgs.NVarChar)),
				Create("DECRYPTBYASYMKEY", dataTypes.IgnoreArgs.VarBinary, 2, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("key_id", dataTypes.Int), new ScalarParameter("cipher_text", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("async_key_password", dataTypes.NVarChar(128))),
				Create("@@MAX_PRECISION", dataTypes.TinyInt, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("@@REMSERVER", dataTypes.NVarChar(128), 0, 0, BuiltInFunctionAttributes.GlobalVariable | BuiltInFunctionAttributes.Nullable),
				Create("@@SERVERNAME", dataTypes.IgnoreArgs.NVarChar, 0, 0, BuiltInFunctionAttributes.GlobalVariable | BuiltInFunctionAttributes.Nullable),
				Create("GETCHECKSUM", dataTypes.Int, 0, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.Int), new ScalarParameter("Param2", dataTypes.Int)),
				Create("@@ROWCOUNT", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("APPLOCK_TEST", dataTypes.Int, 3, 4, BuiltInFunctionAttributes.Nullable, new ScalarParameter("database_principal", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("resource_name", dataTypes.NVarChar(255)), new ScalarParameter("lock_mode", dataTypes.NVarChar(32)), new ScalarParameter("lock_owner", dataTypes.NVarChar(32))),
				Create("ENCRYPTBYKEY", dataTypes.IgnoreArgs.VarBinary, 2, 4, BuiltInFunctionAttributes.Nullable, new ScalarParameter("key_guid", dataTypes.UniqueIdentifier), new ScalarParameter("clear_text", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("add_authenticator", dataTypes.Int), new ScalarParameter("authenticator", dataTypes.SysName)),
				Create("KEY_GUID", dataTypes.UniqueIdentifier, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("key_name", dataTypes.SysName)),
				Create("CURSOR_STATUS", dataTypes.SmallInt, 2, 2, BuiltInFunctionAttributes.None, new ScalarParameter("cursor_source", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("cursor", dataTypes.IgnoreArgs.NVarChar)),
				Create("CEILING", ObjectFactoryBase<DataTypeFactory>.Instance.ExpressionType, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.NumericType)),
				Create("UPPER", dataTypes.IgnoreArgs.NVarChar, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.IgnoreArgs.NVarChar)),
				Create("@@TIMETICKS", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("@@NESTLEVEL", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("PWDCOMPARE", dataTypes.Int, 2, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("Param2", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("Param3", dataTypes.Int)),
				Create("TERTIARY_WEIGHTS", dataTypes.IgnoreArgs.VarBinary, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.IgnoreArgs.VarChar)),
				Create("@@OPTIONS", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("VERIFYSIGNEDBYCERT", dataTypes.Int, 3, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("certificate_id", dataTypes.Int), new ScalarParameter("signed_data", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("signature", dataTypes.IgnoreArgs.VarBinary)),
				Create("DATENAME", dataTypes.IgnoreArgs.NVarChar, 2, 2, BuiltInFunctionAttributes.DatePart | BuiltInFunctionAttributes.Nullable, new ScalarParameter("interval", ObjectFactoryBase<DataTypeFactory>.Instance.DatePart), new ScalarParameter("date", dataTypes.IgnoreArgs.DateTimeOffset)),
				Create("KEY_NAME", dataTypes.IgnoreArgs.NVarChar, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.VarBinary)),
				Create("@@LANGUAGE", dataTypes.IgnoreArgs.NVarChar, 0, 0, BuiltInFunctionAttributes.GlobalVariable | BuiltInFunctionAttributes.Nullable),
				Create("NEWSEQUENTIALID", dataTypes.UniqueIdentifier, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("ROUND", ObjectFactoryBase<DataTypeFactory>.Instance.ExpressionType, 2, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.NumericType), new ScalarParameter("precision", dataTypes.Int), new ScalarParameter("function", dataTypes.Int)),
				Create("@@PACK_RECEIVED", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("SERVERPROPERTY", dataTypes.Variant, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("property_name", dataTypes.IgnoreArgs.VarChar)),
				Create("HASHBYTES", dataTypes.IgnoreArgs.VarBinary, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("algorithm", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("input", dataTypes.IgnoreArgs.NVarChar)),
				Create("@@IDENTITY", dataTypes.IgnoreArgs.Numeric, 0, 0, BuiltInFunctionAttributes.GlobalVariable | BuiltInFunctionAttributes.Nullable),
				Create("@@ERROR", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("ASCII", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.IgnoreArgs.VarChar)),
				Create("COMPRESSNUMERIC", dataTypes.IgnoreArgs.VarBinary, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.Numeric)),
				Create("OBJECT_ID", dataTypes.Int, 1, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("object_name", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("object_type", dataTypes.IgnoreArgs.NVarChar)),
				Create("XACT_STATE", dataTypes.SmallInt, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("DATEPART", dataTypes.Int, 2, 2, BuiltInFunctionAttributes.DatePart | BuiltInFunctionAttributes.Nullable, new ScalarParameter("interval", ObjectFactoryBase<DataTypeFactory>.Instance.DatePart), new ScalarParameter("date", dataTypes.IgnoreArgs.DateTimeOffset)),
				Create("DB_NAME", dataTypes.NVarChar(128), 0, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("database_id", dataTypes.Int)),
				Create("TAN", dataTypes.Float, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.Float)),
				Create("TRIGGER_NESTLEVEL", dataTypes.Int, 0, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("object_id", dataTypes.Int), new ScalarParameter("trigger_type", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("trigger_event_category", dataTypes.IgnoreArgs.NVarChar)),
				Create("ENCRYPTBYCERT", dataTypes.IgnoreArgs.VarBinary, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("certificate_id", dataTypes.Int), new ScalarParameter("clear_text", dataTypes.IgnoreArgs.NVarChar)),
				Create("CERTENCODED", dataTypes.IgnoreArgs.VarBinary, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.Int)),
				Create("CERTPRIVATEKEY", dataTypes.IgnoreArgs.VarBinary, 1, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.Int), new ScalarParameter("Param2", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("Param3", dataTypes.IgnoreArgs.NVarChar)),
				Create("@@PROCID", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("UNCOMPRESS", dataTypes.IgnoreArgs.NVarChar, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.VarBinary)),
				Create("SIGN", ObjectFactoryBase<DataTypeFactory>.Instance.NumericType, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.NumericType)),
				Create("LEN", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.IgnoreArgs.NVarChar)),
				Create("SYSUTCDATETIME", dataTypes.IgnoreArgs.DateTime2, 0, 0, BuiltInFunctionAttributes.None),
				Create("PARSENAME", dataTypes.IgnoreArgs.NVarChar, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("object_name", dataTypes.NVarChar(128)), new ScalarParameter("object_part", dataTypes.Int)),
				Create("CHARINDEX", dataTypes.Int, 2, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("search_expression", dataTypes.IgnoreArgs.VarChar), new ScalarParameter("expression_to_be_searched", dataTypes.IgnoreArgs.VarChar), new ScalarParameter("start_location", dataTypes.Int)),
				Create("ODBCSCALE", dataTypes.Int, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.TinyInt), new ScalarParameter("Param2", dataTypes.TinyInt)),
				Create("RIGHT", dataTypes.IgnoreArgs.NVarChar, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("expression", dataTypes.Int)),
				Create("CERT_ID", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("cert_name", dataTypes.SysName)),
				Create("@@DEFAULT_LANGID", dataTypes.SmallInt, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("SPACE", dataTypes.IgnoreArgs.VarChar, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.Int)),
				Create("IS_ROLEMEMBER", dataTypes.Int, 1, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("Param2", dataTypes.IgnoreArgs.NVarChar)),
				Create("RAND", dataTypes.Float, 0, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("seed_value", dataTypes.Int)),
				Create("ABS", ObjectFactoryBase<DataTypeFactory>.Instance.ExpressionType, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.NumericType)),
				Create("SQL_VARIANT_PROPERTY", dataTypes.Variant, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.Variant), new ScalarParameter("property_name", dataTypes.VarChar(128))),
				Create("PROGRAM_NAME", dataTypes.IgnoreArgs.NVarChar, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("ISDATE", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.IgnoreArgs.NVarChar)),
				Create("FILEPROPERTY", dataTypes.Int, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("file_name", dataTypes.NVarChar(128)), new ScalarParameter("property_name", dataTypes.VarChar(128))),
				Create("@@TOTAL_ERRORS", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("SQUARE", dataTypes.Float, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.Float)),
				Create("CONNECTIONPROPERTY", dataTypes.Variant, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("property_name", dataTypes.IgnoreArgs.VarChar)),
				Create("@@CONNECTIONS", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("GETDATE", dataTypes.DateTime, 0, 0, BuiltInFunctionAttributes.None),
				Create("IDENTITYPROPERTY", dataTypes.Variant, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.Int), new ScalarParameter("Param2", dataTypes.IgnoreArgs.VarChar)),
				Create("TEXTVALID", dataTypes.Int, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("'table.column'", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("text_pointer", dataTypes.IgnoreArgs.VarBinary)),
				Create("IS_MEMBER", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("group_or_role", dataTypes.IgnoreArgs.NVarChar)),
				Create("HAS_PERMS_BY_NAME", dataTypes.Int, 3, 5, BuiltInFunctionAttributes.Nullable, new ScalarParameter("securable", dataTypes.SysName), new ScalarParameter("securable_class", dataTypes.NVarChar(60)), new ScalarParameter("permission", dataTypes.SysName), new ScalarParameter("sub-securable", dataTypes.SysName), new ScalarParameter("sub-securable_class", dataTypes.NVarChar(60))),
				Create("COLLATIONPROPERTYFROMID", dataTypes.Variant, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.Int), new ScalarParameter("Param2", dataTypes.IgnoreArgs.VarChar)),
				Create("ODBCPREC", dataTypes.Int, 3, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.TinyInt), new ScalarParameter("Param2", dataTypes.SmallInt), new ScalarParameter("Param3", dataTypes.TinyInt)),
				Create("COLLATIONNAME", dataTypes.IgnoreArgs.NVarChar, 1, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.Binary), new ScalarParameter("Param2", dataTypes.Int)),
				Create("CHANGE_TRACKING_IS_COLUMN_IN_MASK", dataTypes.Int, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("column_id", dataTypes.Int), new ScalarParameter("columns_changed_data", dataTypes.IgnoreArgs.VarBinary)),
				Create("SIGNBYASYMKEY", dataTypes.IgnoreArgs.VarBinary, 2, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("key_id", dataTypes.Int), new ScalarParameter("clear_text", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("password", dataTypes.NVarChar(128))),
				Create("REVERSE", dataTypes.IgnoreArgs.NVarChar, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.IgnoreArgs.NVarChar)),
				Create("DEGREES", ObjectFactoryBase<DataTypeFactory>.Instance.ExpressionType, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.NumericType)),
				Create("DEFAULT_DOMAIN", dataTypes.IgnoreArgs.NVarChar, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("COLUMNS_UPDATED", dataTypes.IgnoreArgs.VarBinary, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("@@CURSOR_ROWS", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("SIGNBYCERT", dataTypes.IgnoreArgs.VarBinary, 2, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("certificate_id", dataTypes.Int), new ScalarParameter("clear_text", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("cert_password", dataTypes.NVarChar(128))),
				Create("ENCRYPTBYASYMKEY", dataTypes.IgnoreArgs.VarBinary, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("key_id", dataTypes.Int), new ScalarParameter("plain_text", ObjectFactoryBase<DataTypeFactory>.Instance.String)),
				Create("@@TRANCOUNT", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("APP_NAME", dataTypes.NVarChar(128), 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("COL_NAME", dataTypes.NVarChar(128), 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("table_id", dataTypes.Int), new ScalarParameter("column_id", dataTypes.Int)),
				Create("@@CPU_BUSY", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("FILE_ID", dataTypes.SmallInt, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("file_name", dataTypes.NVarChar(128))),
				Create("TYPE_ID", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("type_name", dataTypes.NVarChar(128))),
				Create("INDEXPROPERTY", dataTypes.Int, 3, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("table_id", dataTypes.Int), new ScalarParameter("index_name", dataTypes.NVarChar(128)), new ScalarParameter("property_name", dataTypes.VarChar(128))),
				Create("IS_OBJECTSIGNED", dataTypes.Int, 4, 4, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("Param2", dataTypes.Int), new ScalarParameter("Param3", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("Param4", dataTypes.IgnoreArgs.VarBinary)),
				Create("ERROR_NUMBER", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("UNICODE", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.IgnoreArgs.NVarChar)),
				Create("NEWID", dataTypes.UniqueIdentifier, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("DECRYPTBYKEYAUTOCERT", dataTypes.IgnoreArgs.VarBinary, 3, 5, BuiltInFunctionAttributes.Nullable, new ScalarParameter("cert_id", dataTypes.Int), new ScalarParameter("cert_password", dataTypes.IgnoreArgs.VarChar), new ScalarParameter("cipher_text", dataTypes.IgnoreArgs.VarBinary), new ScalarParameter("add_authenticator", dataTypes.Int), new ScalarParameter("authenticator", dataTypes.SysName)),
				Create("DB_ID", dataTypes.SmallInt, 0, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("database_name", dataTypes.NVarChar(128))),
				Create("VERIFYSIGNEDBYASYMKEY", dataTypes.Int, 3, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("key_id", dataTypes.Int), new ScalarParameter("clear_text", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("signature", dataTypes.IgnoreArgs.VarBinary)),
				Create("GETDEFAULT", dataTypes.Int, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("Param2", dataTypes.Int)),
				Create("@@PACK_SENT", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("FLOOR", ObjectFactoryBase<DataTypeFactory>.Instance.ExpressionType, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.NumericType)),
				Create("@@SERVICENAME", dataTypes.IgnoreArgs.NVarChar, 0, 0, BuiltInFunctionAttributes.GlobalVariable | BuiltInFunctionAttributes.Nullable),
				Create("CHAR", dataTypes.Char(1), 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.Int)),
				Create("LOG10", dataTypes.Float, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.Float)),
				Create("DECRYPTBYKEYAUTOASYMKEY", dataTypes.IgnoreArgs.VarBinary, 3, 5, BuiltInFunctionAttributes.Nullable, new ScalarParameter("key_id", dataTypes.Int), new ScalarParameter("key_password", dataTypes.IgnoreArgs.VarChar), new ScalarParameter("cipher_text", dataTypes.IgnoreArgs.VarBinary), new ScalarParameter("add_authenticator", dataTypes.Int), new ScalarParameter("authenticator", dataTypes.SysName)),
				Create("SUSER_ID", dataTypes.Int, 0, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("login", dataTypes.NVarChar(256))),
				Create("TYPE_NAME", dataTypes.NVarChar(128), 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("type_id", dataTypes.Int)),
				Create("GETUTCDATE", dataTypes.DateTime, 0, 0, BuiltInFunctionAttributes.None),
				Create("@@IDLE", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("ASYMKEYPROPERTY", dataTypes.Variant, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("key_id", dataTypes.Int), new ScalarParameter("output_type", dataTypes.IgnoreArgs.NVarChar)),
				Create("EVENTDATA", dataTypes.Xml(), 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("@@VERSION", dataTypes.IgnoreArgs.NVarChar, 0, 0, BuiltInFunctionAttributes.GlobalVariable | BuiltInFunctionAttributes.Nullable),
				Create("ERROR_MESSAGE", dataTypes.IgnoreArgs.NVarChar, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("LOWER", dataTypes.IgnoreArgs.NVarChar, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.IgnoreArgs.NVarChar)),
				Create("@@MAX_CONNECTIONS", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("SCHEMA_ID", dataTypes.Int, 0, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("schema_name", dataTypes.SysName)),
				Create("COT", dataTypes.Float, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.Float)),
				Create("@@SPID", dataTypes.SmallInt, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("UPDATE", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.None, new ScalarParameter("column_name", dataTypes.Int)),
				Create("SOUNDEX", dataTypes.IgnoreArgs.VarChar, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.IgnoreArgs.VarChar)),
				Create("USER_ID", dataTypes.SmallInt, 0, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("user_name", dataTypes.NVarChar(128))),
				Create("SUSER_SNAME", dataTypes.NVarChar(256), 0, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("login_sid", dataTypes.VarBinary(85))),
				Create("GET_TRANSMISSION_STATUS", dataTypes.IgnoreArgs.NChar, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("conversation_handle", dataTypes.UniqueIdentifier)),
				Create("ORIGINAL_LOGIN", dataTypes.SysName, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("SIN", dataTypes.Float, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.Float)),
				Create("DECRYPTBYKEY", dataTypes.IgnoreArgs.VarBinary, 1, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("cipher_text", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("add_authenticator", dataTypes.Int), new ScalarParameter("authenticator", dataTypes.SysName)),
				Create("OBJIDUPDATE", dataTypes.BigInt, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.Int)),
				Create("SCOPE_IDENTITY", dataTypes.IgnoreArgs.Numeric, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("SWITCHOFFSET", dataTypes.IgnoreArgs.DateTimeOffset, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("datetimeoffset_expression", dataTypes.IgnoreArgs.DateTimeOffset), new ScalarParameter("timezoneoffset_expression_integer|timezoneoffset_expression_string", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType)),
				Create("MIN_ACTIVE_ROWVERSION", dataTypes.IgnoreArgs.VarBinary, 0, 0, BuiltInFunctionAttributes.None),
				Create("GET_NEW_ROWVERSION", dataTypes.IgnoreArgs.VarBinary, 0, 0, BuiltInFunctionAttributes.None),
				Create("SUSER_SID", dataTypes.VarBinary(85), 0, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("login", dataTypes.NVarChar(256)), new ScalarParameter("Param2", dataTypes.Int)),
				Create("GENDBNAMEFROMPATH", dataTypes.IgnoreArgs.NVarChar, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.NVarChar)),
				Create("DECRYPTBYPASSPHRASE", dataTypes.IgnoreArgs.VarBinary, 2, 4, BuiltInFunctionAttributes.Nullable, new ScalarParameter("pass_phrase", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("cipher_text", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("add_authenticator", dataTypes.Int), new ScalarParameter("authenticator", dataTypes.SysName)),
				Create("COLUMNPROPERTY", dataTypes.Int, 3, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("table_or_procedure_id", dataTypes.Int), new ScalarParameter("column_name", dataTypes.NVarChar(128)), new ScalarParameter("property_name", dataTypes.IgnoreArgs.VarChar)),
				Create("@@LANGID", dataTypes.SmallInt, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("OBJECT_SCHEMA_NAME", dataTypes.NVarChar(128), 1, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("object_id", dataTypes.Int), new ScalarParameter("database_id", dataTypes.Int)),
				Create("SESSIONPROPERTY", dataTypes.Variant, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("option", dataTypes.IgnoreArgs.VarChar)),
				Create("XTYPETOTDS", dataTypes.TinyInt, 2, 2, BuiltInFunctionAttributes.None, new ScalarParameter("Param1", dataTypes.TinyInt), new ScalarParameter("Param2", dataTypes.TinyInt)),
				Create("ASSEMBLYPROPERTY", dataTypes.Variant, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("assembly_name", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("property_name", dataTypes.IgnoreArgs.VarChar)),
				Create("SYSDATETIME", dataTypes.IgnoreArgs.DateTime2, 0, 0, BuiltInFunctionAttributes.None),
				Create("REPLACE", dataTypes.IgnoreArgs.NVarChar, 3, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression_to_be_searched", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("search_expression", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("replacement_expression", dataTypes.IgnoreArgs.NVarChar)),
				Create("DIFFERENCE", dataTypes.Int, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.IgnoreArgs.VarChar), new ScalarParameter("expression", dataTypes.IgnoreArgs.VarChar)),
				Create("REPLICATE", dataTypes.IgnoreArgs.NVarChar, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("expression", dataTypes.Int)),
				Create("SYMKEYPROPERTY", dataTypes.Variant, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("key_id", dataTypes.Int), new ScalarParameter("output_type", dataTypes.IgnoreArgs.NVarChar)),
				Create("CERTPROPERTY", dataTypes.Variant, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("cert_id", dataTypes.Int), new ScalarParameter("property_name", dataTypes.IgnoreArgs.NVarChar)),
				Create("@@TEXTSIZE", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("FILEGROUP_ID", dataTypes.SmallInt, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("filegroup_name", dataTypes.NVarChar(128))),
				Create("COLLATIONPROPERTY", dataTypes.Variant, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("collation_name", dataTypes.NVarChar(128)), new ScalarParameter("property_name", dataTypes.VarChar(128))),
				Create("DECRYPTBYCERT", dataTypes.IgnoreArgs.VarBinary, 2, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("certificate_id", dataTypes.Int), new ScalarParameter("cipher_text", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("cert_password", dataTypes.NVarChar(128))),
				Create("NCHAR", dataTypes.IgnoreArgs.NVarChar, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.Int)),
				Create("LEFT", dataTypes.IgnoreArgs.NVarChar, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("number_of_chars", dataTypes.Int)),
				Create("PLATFORM", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.None),
				Create("HAS_DBACCESS", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("database_name", dataTypes.NVarChar(128))),
				Create("IDENT_CURRENT", dataTypes.IgnoreArgs.Numeric, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("table_name", dataTypes.NVarChar(128))),
				Create("COS", dataTypes.Float, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.Float)),
				Create("CURRENT_REQUEST_ID", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.None),
				Create("DATALENGTH", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType)),
				Create("FULLTEXTSERVICEPROPERTY", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("property_name", dataTypes.IgnoreArgs.VarChar)),
				Create("POWER", ObjectFactoryBase<DataTypeFactory>.Instance.ExpressionType, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.NumericType), new ScalarParameter("power", dataTypes.Float)),
				Create("USER_SID", dataTypes.IgnoreArgs.VarBinary, 0, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.SmallInt)),
				Create("HOST_NAME", dataTypes.NVarChar(128), 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("PATINDEX", dataTypes.Int, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("search_pattern_literal", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("expression", dataTypes.IgnoreArgs.NVarChar)),
				Create("SYSDATETIMEOFFSET", dataTypes.IgnoreArgs.DateTimeOffset, 0, 0, BuiltInFunctionAttributes.None),
				Create("ASYMKEY_ID", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("key_name", dataTypes.SysName)),
				Create("ERROR_PROCEDURE", dataTypes.IgnoreArgs.NVarChar, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("@@DEF_SORTORDER_ID", dataTypes.TinyInt, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("FILE_IDEX", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("file_name", dataTypes.NVarChar(128))),
				Create("STR", dataTypes.IgnoreArgs.VarChar, 1, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.Float), new ScalarParameter("length", dataTypes.Int), new ScalarParameter("scale", dataTypes.Int)),
				Create("@@DATEFIRST", dataTypes.TinyInt, 0, 0, BuiltInFunctionAttributes.GlobalVariable | BuiltInFunctionAttributes.Nullable),
				Create("IS_SRVROLEMEMBER", dataTypes.Int, 1, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("role_name", dataTypes.NVarChar(128)), new ScalarParameter("login", dataTypes.NVarChar(128))),
				Create("APPLOCK_MODE", dataTypes.IgnoreArgs.NVarChar, 2, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("database_principal", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("resource_name", dataTypes.NVarChar(255)), new ScalarParameter("lock_owner", dataTypes.NVarChar(32))),
				Create("OBJECTPROPERTYEX", dataTypes.Variant, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("object_id", dataTypes.Int), new ScalarParameter("property_name", dataTypes.IgnoreArgs.VarChar)),
				Create("EXP", dataTypes.Float, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.Float)),
				Create("FILEGROUPPROPERTY", dataTypes.Int, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("filegroup_name", dataTypes.NVarChar(128)), new ScalarParameter("property_name", dataTypes.VarChar(128))),
				Create("SQRT", dataTypes.Float, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.Float)),
				Create("HOST_ID", dataTypes.Char(8), 0, 0, BuiltInFunctionAttributes.None),
				Create("ROWCOUNT_BIG", dataTypes.BigInt, 0, 0, BuiltInFunctionAttributes.None),
				Create("SUBSTRING", ObjectFactoryBase<DataTypeFactory>.Instance.String, 3, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.String), new ScalarParameter("starting_position", dataTypes.Int), new ScalarParameter("length", dataTypes.Int)),
				Create("FULLTEXTCATALOGPROPERTY", dataTypes.Int, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("catalog_name", dataTypes.NVarChar(128)), new ScalarParameter("property_name", dataTypes.IgnoreArgs.VarChar)),
				Create("GETANSINULL", dataTypes.SmallInt, 0, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("database_name", dataTypes.NVarChar(128))),
				Create("KEY_ID", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("key_name", dataTypes.SysName)),
				Create("TYPEPROPERTY", dataTypes.Int, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("type_name", dataTypes.NVarChar(128)), new ScalarParameter("property_name", dataTypes.VarChar(128))),
				Create("ENCRYPTBYPASSPHRASE", dataTypes.IgnoreArgs.VarBinary, 2, 4, BuiltInFunctionAttributes.Nullable, new ScalarParameter("pass_phrase", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("clear_text", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("add_authenticator", dataTypes.Int), new ScalarParameter("authenticator", dataTypes.SysName)),
				Create("ACOS", dataTypes.Float, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", dataTypes.Float)),
				Create("COL_LENGTH", dataTypes.SmallInt, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("table_name", dataTypes.NVarChar(128)), new ScalarParameter("column_name", dataTypes.NVarChar(128))),
				Create("PI", dataTypes.Float, 0, 0, BuiltInFunctionAttributes.None),
				Create("OBJECT_NAME", dataTypes.NVarChar(128), 1, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("object_id", dataTypes.Int), new ScalarParameter("Param2", dataTypes.Int)),
				Create("FILEGROUP_NAME", dataTypes.NVarChar(128), 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("filegroup_id", dataTypes.SmallInt)),
				Create("ERROR_STATE", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("OBJECT_DEFINITION", dataTypes.NVarCharMax, 1, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("object_id", dataTypes.Int), new ScalarParameter("Param2", dataTypes.Int), new ScalarParameter("Param3", dataTypes.Int)),
				Create("FILE_NAME", dataTypes.NVarChar(128), 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("file_id", dataTypes.Int)),
				Create("IDENT_INCR", dataTypes.IgnoreArgs.Numeric, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("table_or_view_name", dataTypes.NVarChar(128))),
				Create("ERROR_LINE", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("GET_FILESTREAM_TRANSACTION_CONTEXT", dataTypes.VarBinaryMax, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("CHANGE_TRACKING_MIN_VALID_VERSION", dataTypes.BigInt, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("table_object_id", dataTypes.Int)),
				Create("@@DBTS", dataTypes.IgnoreArgs.VarBinary, 0, 0, BuiltInFunctionAttributes.GlobalVariable),
				Create("IDENT_SEED", dataTypes.IgnoreArgs.Numeric, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("table_or_view_name", dataTypes.NVarChar(128))),
				Create("SUSER_NAME", dataTypes.NVarChar(256), 0, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("user_id", dataTypes.Int)),
				Create("CRYPT_GEN_RANDOM", dataTypes.IgnoreArgs.VarBinary, 1, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.Int), new ScalarParameter("Param2", dataTypes.IgnoreArgs.VarBinary)),
				Create("LOGINPROPERTY", dataTypes.Variant, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("login_name", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("property_name", dataTypes.IgnoreArgs.NVarChar)),
				Create("SID_BINARY", dataTypes.IgnoreArgs.VarBinary, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.NVarChar)),
				Create("NEWFILESTREAMVALUE", dataTypes.IgnoreArgs.VarBinary, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.VarBinary)),
				Create("TEXTPTR", dataTypes.IgnoreArgs.VarBinary, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("column_name", dataTypes.NText)),
				Create("GETPATHLOCATOR", dataTypes.IgnoreArgs.NVarChar, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.NVarChar)),
				Create("FILETABLEROOTPATH", dataTypes.IgnoreArgs.NVarChar, 0, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("Param2", dataTypes.Int)),
				Create("BRICK_ID", dataTypes.Int, 0, 0, BuiltInFunctionAttributes.None),
				Create("COLUMNPROPERTYEX", dataTypes.Variant, 3, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.Int), new ScalarParameter("Param2", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("Param3", dataTypes.IgnoreArgs.VarChar)),
				Create("SQL_CONNECTION_MODE", dataTypes.SmallInt, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("GET_CLOUD_PARTITION_MAX_SIZE", dataTypes.Int, 4, 4, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("Param2", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("Param3", dataTypes.IgnoreArgs.VarBinary), new ScalarParameter("Param4", dataTypes.IgnoreArgs.VarBinary)),
				Create("CLOUD_DATABASEPROPERTYEX", dataTypes.Variant, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.SmallInt), new ScalarParameter("Param2", dataTypes.IgnoreArgs.VarChar)),
				Create("CONCAT", dataTypes.IgnoreArgs.VarBinary, 2, -1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType), new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType)),
				Create("EOMONTH", dataTypes.DateTime, 1, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.DateTime), new ScalarParameter("Param2", dataTypes.Int)),
				Create("IIF", dataTypes.Variant, 3, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType), new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType), new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType)),
				Create("CHOOSE", dataTypes.Variant, 2, -1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("index", dataTypes.Int), new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType)),
				Create("DATEFROMPARTS", dataTypes.DateTime, 3, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.Int), new ScalarParameter("Param2", dataTypes.Int), new ScalarParameter("Param3", dataTypes.Int)),
				Create("DATETIME2FROMPARTS", dataTypes.IgnoreArgs.DateTime2, 8, 8, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.Int), new ScalarParameter("Param2", dataTypes.Int), new ScalarParameter("Param3", dataTypes.Int), new ScalarParameter("Param4", dataTypes.Int), new ScalarParameter("Param5", dataTypes.Int), new ScalarParameter("Param6", dataTypes.Int), new ScalarParameter("Param7", dataTypes.Int), new ScalarParameter("Param8", dataTypes.Int)),
				Create("DATETIMEFROMPARTS", dataTypes.DateTime, 7, 7, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.Int), new ScalarParameter("Param2", dataTypes.Int), new ScalarParameter("Param3", dataTypes.Int), new ScalarParameter("Param4", dataTypes.Int), new ScalarParameter("Param5", dataTypes.Int), new ScalarParameter("Param6", dataTypes.Int), new ScalarParameter("Param7", dataTypes.Int)),
				Create("DATETIMEOFFSETFROMPARTS", dataTypes.IgnoreArgs.DateTimeOffset, 10, 10, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.Int), new ScalarParameter("Param2", dataTypes.Int), new ScalarParameter("Param3", dataTypes.Int), new ScalarParameter("Param4", dataTypes.Int), new ScalarParameter("Param5", dataTypes.Int), new ScalarParameter("Param6", dataTypes.Int), new ScalarParameter("Param7", dataTypes.Int), new ScalarParameter("Param8", dataTypes.Int), new ScalarParameter("Param9", dataTypes.Int), new ScalarParameter("Param10", dataTypes.Int)),
				Create("TIMEFROMPARTS", dataTypes.IgnoreArgs.Time, 5, 5, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.Int), new ScalarParameter("Param2", dataTypes.Int), new ScalarParameter("Param3", dataTypes.Int), new ScalarParameter("Param4", dataTypes.Int), new ScalarParameter("Param5", dataTypes.Int)),
				Create("SMALLDATETIMEFROMPARTS", dataTypes.SmallDateTime, 5, 5, BuiltInFunctionAttributes.Nullable, new ScalarParameter("Param1", dataTypes.Int), new ScalarParameter("Param2", dataTypes.Int), new ScalarParameter("Param3", dataTypes.Int), new ScalarParameter("Param4", dataTypes.Int), new ScalarParameter("Param5", dataTypes.Int)),
				Create("ISJSON", dataTypes.Int, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("json", dataTypes.IgnoreArgs.NVarChar)),
				Create("JSON_VALUE", dataTypes.IgnoreArgs.NVarChar, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("json", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("path", dataTypes.IgnoreArgs.NVarChar)),
				Create("JSON_QUERY", dataTypes.IgnoreArgs.NVarChar, 1, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("json", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("path", dataTypes.IgnoreArgs.NVarChar)),
				Create("JSON_MODIFY", dataTypes.IgnoreArgs.NVarChar, 3, 3, BuiltInFunctionAttributes.Nullable, new ScalarParameter("json", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("path", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("new_value", dataTypes.IgnoreArgs.NVarChar)),
				Create("STRING_ESCAPE", dataTypes.IgnoreArgs.NVarChar, 2, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("value", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("escape_type", dataTypes.IgnoreArgs.NVarChar)),
				Create("COMPRESS", dataTypes.VarBinaryMax, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType)),
				Create("DECOMPRESS", dataTypes.VarBinaryMax, 1, 1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("compressed_binary_data", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType)),
				Create("TRIM", dataTypes.IgnoreArgs.NVarChar, 1, 2, BuiltInFunctionAttributes.Nullable, new ScalarParameter("characters", dataTypes.IgnoreArgs.NVarChar), new ScalarParameter("expression", dataTypes.IgnoreArgs.NVarChar)),
				Create("CONCAT_WS", dataTypes.IgnoreArgs.VarChar, 3, -1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("separator", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType), new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType), new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType)),
				Create("DATE_BUCKET", dataTypes.IgnoreArgs.DateTime2, 3, 3, BuiltInFunctionAttributes.DatePart | BuiltInFunctionAttributes.Nullable, new ScalarParameter("bucket_width_unit", ObjectFactoryBase<DataTypeFactory>.Instance.DatePart), new ScalarParameter("bucket_width", dataTypes.Int), new ScalarParameter("date", dataTypes.IgnoreArgs.DateTime2)),
				Create("GREATEST", dataTypes.Variant, 1, -1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType)),
				Create("LEAST", dataTypes.Variant, 1, -1, BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType)),
				Create("TRY_CONVERT", ObjectFactoryBase<DataTypeFactory>.Instance.DataTypeName, 3, 3, BuiltInFunctionAttributes.MultiReturnType | BuiltInFunctionAttributes.Nullable, new ScalarParameter("target_type", ObjectFactoryBase<DataTypeFactory>.Instance.DataTypeName), new ScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.AnyType), new ScalarParameter("date_style", dataTypes.SmallInt))
			};
		}

		static ListHelper()
		{
			SystemDataTypeLookup instance = SystemDataTypeLookup.Instance;
			BuiltIns = CreateBuiltInList(instance);
			IScalarParameter[] arguments = new IScalarParameter[1] { ObjectFactoryBase<ParameterFactory>.Instance.CreateScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.UnknownScalar) };
			IScalarParameter[] arguments2 = new IScalarParameter[3]
			{
				ObjectFactoryBase<ParameterFactory>.Instance.CreateScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.UnknownScalar),
				ObjectFactoryBase<ParameterFactory>.Instance.CreateScalarParameter("offset", ObjectFactoryBase<DataTypeFactory>.Instance.IntegerType),
				ObjectFactoryBase<ParameterFactory>.Instance.CreateScalarParameter("default", ObjectFactoryBase<DataTypeFactory>.Instance.UnknownScalar)
			};
			IScalarParameter[] arguments3 = new IScalarParameter[2]
			{
				ObjectFactoryBase<ParameterFactory>.Instance.CreateScalarParameter("expression", ObjectFactoryBase<DataTypeFactory>.Instance.UnknownScalar),
				ObjectFactoryBase<ParameterFactory>.Instance.CreateScalarParameter("separator", ObjectFactoryBase<DataTypeFactory>.Instance.String)
			};
			AggregateFunctions = new BuiltInFunction[21]
			{
				Create("APPROX_COUNT_DISTINCT", instance.BigInt, 1, 1, BuiltInFunctionAttributes.Aggregate | BuiltInFunctionAttributes.Nullable, arguments),
				Create("AVG", instance.Int, 1, 1, BuiltInFunctionAttributes.Aggregate | BuiltInFunctionAttributes.Nullable, arguments),
				Create("CHECKSUM_AGG", instance.Int, 1, 1, BuiltInFunctionAttributes.Aggregate | BuiltInFunctionAttributes.Nullable, arguments),
				Create("COUNT", instance.Int, 1, 1, BuiltInFunctionAttributes.Aggregate | BuiltInFunctionAttributes.Nullable, arguments),
				Create("COUNT_BIG", instance.BigInt, 1, 1, BuiltInFunctionAttributes.Aggregate | BuiltInFunctionAttributes.Nullable, arguments),
				Create("FIRST_VALUE", instance.Int, 1, 1, BuiltInFunctionAttributes.Aggregate, arguments),
				Create("GROUPING", instance.TinyInt, 1, 1, BuiltInFunctionAttributes.Aggregate, arguments),
				Create("GROUPING_ID", instance.Int, 1, -1, BuiltInFunctionAttributes.Aggregate, arguments),
				Create("LAG", instance.Int, 1, 3, BuiltInFunctionAttributes.Aggregate, arguments2),
				Create("LAST_VALUE", instance.Int, 1, 1, BuiltInFunctionAttributes.Aggregate, arguments),
				Create("LEAD", instance.Int, 1, 3, BuiltInFunctionAttributes.Aggregate, arguments2),
				Create("MAX", instance.Int, 1, 1, BuiltInFunctionAttributes.Aggregate | BuiltInFunctionAttributes.Nullable, arguments),
				Create("MIN", instance.Int, 1, 1, BuiltInFunctionAttributes.Aggregate, arguments),
				Create("PERCENTILE_CONT", instance.Float, 1, 1, BuiltInFunctionAttributes.Aggregate, arguments),
				Create("PERCENTILE_DISC", instance.Float, 1, 1, BuiltInFunctionAttributes.Aggregate, arguments),
				Create("STDEV", instance.Float, 1, 1, BuiltInFunctionAttributes.Aggregate | BuiltInFunctionAttributes.Nullable, arguments),
				Create("STDEVP", instance.Float, 1, 1, BuiltInFunctionAttributes.Aggregate | BuiltInFunctionAttributes.Nullable, arguments),
				Create("SUM", instance.Int, 1, 1, BuiltInFunctionAttributes.Aggregate | BuiltInFunctionAttributes.Nullable, arguments),
				Create("VAR", instance.Float, 1, 1, BuiltInFunctionAttributes.Aggregate | BuiltInFunctionAttributes.Nullable, arguments),
				Create("VARP", instance.Float, 1, 1, BuiltInFunctionAttributes.Aggregate | BuiltInFunctionAttributes.Nullable, arguments),
				Create("STRING_AGG", instance.NVarCharMax, 2, 2, BuiltInFunctionAttributes.Aggregate, arguments3)
			};
			ParameterlessFunctions = new BuiltInFunction[5]
			{
				Create("CURRENT_TIMESTAMP", instance.DateTime, 0, 0, BuiltInFunctionAttributes.None),
				Create("CURRENT_USER", instance.SysName, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("SESSION_USER", instance.SysName, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("SYSTEM_USER", instance.SysName, 0, 0, BuiltInFunctionAttributes.Nullable),
				Create("USER", instance.SysName, 0, 0, BuiltInFunctionAttributes.Nullable)
			};
			RankingFunctions = new BuiltInFunction[6]
			{
				Create("RANK", instance.BigInt, 0, 0, BuiltInFunctionAttributes.Ranking | BuiltInFunctionAttributes.Nullable),
				Create("DENSE_RANK", instance.BigInt, 0, 0, BuiltInFunctionAttributes.Ranking | BuiltInFunctionAttributes.Nullable),
				Create("ROW_NUMBER", instance.BigInt, 0, 0, BuiltInFunctionAttributes.Ranking | BuiltInFunctionAttributes.Nullable),
				Create("NTILE", instance.BigInt, 1, 1, BuiltInFunctionAttributes.Ranking | BuiltInFunctionAttributes.Nullable, new ScalarParameter("expression", instance.Int)),
				Create("PERCENT_RANK", instance.Float, 0, 0, BuiltInFunctionAttributes.Ranking | BuiltInFunctionAttributes.Nullable),
				Create("CUME_DIST", instance.Float, 0, 0, BuiltInFunctionAttributes.Ranking | BuiltInFunctionAttributes.Nullable)
			};
			MissingFunctions = new BuiltInFunction[1] { Create("IDENTITY", ObjectFactoryBase<DataTypeFactory>.Instance.DataTypeName, 1, 3, BuiltInFunctionAttributes.MultiReturnType, new ScalarParameter("data_type", ObjectFactoryBase<DataTypeFactory>.Instance.DataTypeName), new ScalarParameter("seed", ObjectFactoryBase<DataTypeFactory>.Instance.IntegerType), new ScalarParameter("increment", ObjectFactoryBase<DataTypeFactory>.Instance.IntegerType)) };
		}
	}

	[Flags]
	private enum BuiltInFunctionAttributes
	{
		None = 0,
		AcceptsStar = 1,
		Aggregate = 2,
		DatePart = 4,
		GlobalVariable = 8,
		MultiReturnType = 0x10,
		Ranking = 0x20,
		Nullable = 0x40
	}

	private sealed class MultiTypeBuiltInFunction : BuiltInFunction
	{
		private readonly Dictionary<ISystemDataType, BuiltInFunction> m_functionCache;

		public override BuiltInFunction this[ISystemDataType dataType]
		{
			get
			{
				if (!m_functionCache.TryGetValue(dataType, out var value))
				{
					lock (m_functionCache)
					{
						if (!m_functionCache.TryGetValue(dataType, out value))
						{
							value = CreateBuiltInFunctionForType(dataType);
							m_functionCache.Add(dataType, value);
						}
					}
				}
				return value;
			}
		}

		public MultiTypeBuiltInFunction(string name, IScalarDataType returnType, short minArgCount, short maxArgCount, BuiltInFunctionAttributes attributes, IMetadataOrderedCollection<IParameter> parameters)
			: base(name, returnType, minArgCount, maxArgCount, attributes, parameters)
		{
			m_functionCache = new Dictionary<ISystemDataType, BuiltInFunction>(8);
		}

		private BuiltInFunction CreateBuiltInFunctionForType(ISystemDataType dataType)
		{
			BuiltInFunctionAttributes attributes = m_attributes & ~BuiltInFunctionAttributes.MultiReturnType;
			return new BuiltInFunction(m_name, dataType, m_minArgCount, m_maxArgCount, attributes, m_parameters);
		}
	}

	private readonly IScalarDataType m_returnType;

	private readonly IMetadataOrderedCollection<IParameter> m_parameters;

	private readonly short m_minArgCount;

	private readonly short m_maxArgCount;

	private readonly BuiltInFunctionAttributes m_attributes;

	public virtual BuiltInFunction this[ISystemDataType dataType] => null;

	public int MinNumberOfArgs => m_minArgCount;

	public int MaxNumberOfArgs => m_maxArgCount;

	public bool IsVarArg => m_maxArgCount == -1;

	public bool IsDatePartFunction => (m_attributes & BuiltInFunctionAttributes.DatePart) == BuiltInFunctionAttributes.DatePart;

	public bool IsGlobalVariable => (m_attributes & BuiltInFunctionAttributes.GlobalVariable) == BuiltInFunctionAttributes.GlobalVariable;

	public bool IsAggregateFunction => (m_attributes & BuiltInFunctionAttributes.Aggregate) == BuiltInFunctionAttributes.Aggregate;

	public ScalarType ScalarType => ScalarType.ScalarFunction;

	public IScalarDataType DataType => m_returnType;

	public bool Nullable => (m_attributes & BuiltInFunctionAttributes.Nullable) != 0;

	public IMetadataOrderedCollection<IParameter> Parameters => m_parameters;

	public bool IsUserDefinedFunction => false;

	public IUserDefinedFunction AsUserDefinedFunction => null;

	private static BuiltInFunction Create(string name, IScalarDataType returnType, short minNumOfArgs, short maxNumOfArgs, BuiltInFunctionAttributes attributes, params IScalarParameter[] arguments)
	{
		IMetadataOrderedCollection<IParameter> parameters = Collection<IParameter>.ConvertArray(arguments, CollationInfo.OrdinalIgnoreCase);
		if ((attributes & BuiltInFunctionAttributes.MultiReturnType) != 0)
		{
			return new MultiTypeBuiltInFunction(name, returnType, minNumOfArgs, minNumOfArgs, attributes, parameters);
		}
		return new BuiltInFunction(name, returnType, minNumOfArgs, maxNumOfArgs, attributes, parameters);
	}

	private BuiltInFunction(string name, IScalarDataType returnType, short minArgCount, short maxArgCount, BuiltInFunctionAttributes attributes, IMetadataOrderedCollection<IParameter> parameters)
		: base(name)
	{
		m_returnType = returnType;
		m_parameters = parameters;
		m_minArgCount = minArgCount;
		m_maxArgCount = maxArgCount;
		m_attributes = attributes;
	}

	public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public static void PopulateCollection(BuiltInFunctionCollection collection)
	{
		collection.AddRange(ListHelper.BuiltIns);
		collection.AddRange(ListHelper.AggregateFunctions);
		collection.AddRange(ListHelper.ParameterlessFunctions);
		collection.AddRange(ListHelper.RankingFunctions);
		collection.AddRange(ListHelper.MissingFunctions);
	}
}
