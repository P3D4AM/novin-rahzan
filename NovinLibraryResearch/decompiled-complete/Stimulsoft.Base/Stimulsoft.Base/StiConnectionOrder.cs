using Stimulsoft.Base.Json;
using Stimulsoft.Base.Json.Converters;

namespace Stimulsoft.Base;

[JsonConverter(typeof(StringEnumConverter))]
public enum StiConnectionOrder
{
	MsSqlDataSource = 10,
	MySqlDataSource = 20,
	OracleDataSource = 30,
	PostgreSqlDataSource = 40,
	JsonDataSource = 50,
	GisDataSource = 55,
	XmlDataSource = 60,
	OdbcDataSource = 70,
	OleDbDataSource = 80,
	MsAccessDataSource = 90,
	FirebirdDataSource = 100,
	SqlCeDataSource = 110,
	SqLiteDataSource = 120,
	Db2DataSource = 130,
	InformixDataSource = 140,
	SybaseDataSource = 150,
	SybaseAdsDataSource = 160,
	TeradataDataSource = 170,
	VistaDbDataSource = 180,
	UniversalDevartDataSource = 190,
	ODataDataSource = 200,
	ExcelDataSource = 210,
	CsvDataSource = 220,
	DBaseDataSource = 230,
	MongoDbDataSource = 240,
	AzureTableStorage = 250,
	AzureBlobStorage = 255,
	CosmosDbDataSource = 260,
	GoogleSheetsStorage = 270,
	DynamicsNavDataSource = 280,
	DropboxCloudStorage = 290,
	GoogleDriveCloudStorage = 300,
	OneDriveCloudStorage = 310,
	SharePointCloudStorage = 320,
	DataWorldDataSource = 330,
	QuickBooksDataSource = 340,
	PdoDataSource = 350,
	BigQueryDataSource = 360,
	GoogleAnalyticsDataSource = 370,
	Unspecified = 0
}
