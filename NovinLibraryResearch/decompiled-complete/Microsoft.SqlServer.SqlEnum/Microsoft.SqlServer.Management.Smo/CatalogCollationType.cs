using System.ComponentModel;

namespace Microsoft.SqlServer.Management.Smo;

[TypeConverter(typeof(CatalogCollationTypeConverter))]
public enum CatalogCollationType
{
	[LocDisplayName("dbCatalogCollationDatabaseDefault")]
	[TsqlSyntaxString("DATABASE_DEFAULT")]
	DatabaseDefault,
	[LocDisplayName("dbCatalogCollationContained")]
	[TsqlSyntaxString("Latin1_General_100_CI_AS_KS_WS_SC")]
	ContainedDatabaseFixedCollation,
	[LocDisplayName("dbCatalogCollationSQL_Latin1_General_CP1_CI_AS")]
	[TsqlSyntaxString("SQL_Latin1_General_CP1_CI_AS")]
	SQLLatin1GeneralCP1CIAS
}
