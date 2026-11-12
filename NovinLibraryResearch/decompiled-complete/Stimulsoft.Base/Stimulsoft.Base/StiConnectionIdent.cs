using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Base;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiConnectionIdent
{
	Db2DataSource = 1,
	InformixDataSource,
	MsAccessDataSource,
	MsSqlDataSource,
	MySqlDataSource,
	OdbcDataSource,
	OleDbDataSource,
	FirebirdDataSource,
	PostgreSqlDataSource,
	OracleDataSource,
	SqlCeDataSource,
	SqLiteDataSource,
	SybaseDataSource,
	SybaseAdsDataSource,
	TeradataDataSource,
	VistaDbDataSource,
	UniversalDevartDataSource,
	ODataDataSource,
	CsvDataSource,
	DBaseDataSource,
	DynamicsNavDataSource,
	ExcelDataSource,
	JsonDataSource,
	GisDataSource,
	XmlDataSource,
	MongoDbDataSource,
	DropboxCloudStorage,
	GoogleDriveCloudStorage,
	OneDriveCloudStorage,
	SharePointCloudStorage,
	AzureTableStorage,
	AzureBlobStorage,
	CosmosDbDataSource,
	GoogleSheetsStorage,
	DataWorldDataSource,
	QuickBooksDataSource,
	FirebaseDataSource,
	BigQueryDataSource,
	PdoDataSource,
	GoogleAnalyticsDataSource,
	Unspecified
}
