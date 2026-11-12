using System;

namespace Microsoft.SqlServer.Management.SqlParser.Binder;

[Flags]
public enum CompletionContext
{
	None = 0,
	LocalTableExpressionsOnly = 1,
	ScalarVariables = 2,
	Columns = 4,
	ScalarValuedFunctions = 8,
	ColumnAliases = 0x10,
	TableValuedFunctions = 0x20,
	Tables = 0x40,
	Views = 0x80,
	TableVariables = 0x100,
	StoredProcedures = 0x400,
	Databases = 0x800,
	Schemas = 0x1000,
	GlobalVariables = 0x2000,
	BuiltinFunctions = 0x4000,
	Datepart = 0x8000,
	Credentials = 0x10000,
	ServerAsymmetricKeys = 0x20000,
	ConnectedDatabaseAsymmetricKeys = 0x40000,
	ServerCertificates = 0x80000,
	ConnectedDatabaseCertificates = 0x100000,
	Logins = 0x200000,
	Users = 0x400000,
	SystemDataTypes = 0x800000,
	AllBuiltins = 0x6000,
	Scalar = 0x780A,
	SelectScalar = 0x78EE,
	SelectTabular = 0x19E0,
	ScalarPlusAliases = 0x78FE,
	TablesOrViews = 0xC0,
	TablesExpressions = 0x5E0,
	SynyonymBaseObjects = 0xE8,
	DatabaseQualifiedTables = 0x1840,
	DatabaseQualifiedViews = 0x1880,
	DatabaseQualifiedStoredProcedures = 0x1C00,
	DatabaseQualifiedScalarValuedFunctions = 0x1808,
	DatabaseQualifiedTableValuedFunctions = 0x1820
}
