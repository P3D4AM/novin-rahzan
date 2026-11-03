using System.Threading.Tasks;
using System.Windows.Forms;
using Stimulsoft.Report.Dictionary.Databases.Google;

namespace Stimulsoft.Report.Dictionary;

public interface IStiDataEditors
{
	DialogResult CsvDatabaseEdit(StiCsvDatabase database, StiDictionary dictionary, bool newDatabase);

	DialogResult DBaseDatabaseEdit(StiDBaseDatabase database, StiDictionary dictionary, bool newDatabase);

	DialogResult ExcelDatabaseEdit(StiExcelDatabase database, StiDictionary dictionary, bool newDatabase);

	DialogResult JsonDatabaseEdit(StiJsonDatabase database, StiDictionary dictionary, bool newDatabase);

	DialogResult GisDatabaseEdit(StiGisDatabase database, StiDictionary dictionary, bool newDatabase);

	DialogResult SqlDatabaseEdit(StiSqlDatabase database, StiDictionary dictionary, bool newDatabase);

	Task<DialogResult> SqlDatabaseEditAsync(StiSqlDatabase database, StiDictionary dictionary, bool newDatabase);

	DialogResult NoSqlDatabaseEdit(StiNoSqlDatabase database, StiDictionary dictionary, bool newDatabase);

	Task<DialogResult> NoSqlDatabaseEditAsync(StiNoSqlDatabase database, StiDictionary dictionary, bool newDatabase);

	DialogResult GoogleSheetsDatabaseEdit(StiGoogleSheetsDatabase database, StiDictionary dictionary, bool newDatabase);

	Task<DialogResult> GoogleSheetsDatabaseEditAsync(StiGoogleSheetsDatabase database, StiDictionary dictionary, bool newDatabase);

	DialogResult DataWorldDatabaseEdit(StiDataWorldDatabase dataWorldDatabase, StiDictionary dictionary, bool newDatabase);

	Task<DialogResult> DataWorldDatabaseEditAsync(StiDataWorldDatabase dataWorldDatabase, StiDictionary dictionary, bool newDatabase);

	DialogResult FirebaseDatabaseEdit(StiFirebaseDatabase firebaseDatabase, StiDictionary dictionary, bool newDatabase);

	Task<DialogResult> FirebaseDatabaseEditAsync(StiFirebaseDatabase firebaseDatabase, StiDictionary dictionary, bool newDatabase);

	DialogResult BigQueryDatabaseEdit(StiBigQueryDatabase database, StiDictionary dictionary, bool newDatabase);

	Task<DialogResult> BigQueryDatabaseEditAsync(StiBigQueryDatabase database, StiDictionary dictionary, bool newDatabase);

	DialogResult AzureBlobStorageDatabaseEdit(StiAzureBlobStorageDatabase database, StiDictionary dictionary, bool newDatabase);

	DialogResult GoogleAnalyticsDatabaseEdit(StiGoogleAnalyticsDatabase stiGoogleAnalyticsDatabase, StiDictionary dictionary, bool isNewDatabase);

	Task<DialogResult> GoogleAnalyticsDatabaseEditAsync(StiGoogleAnalyticsDatabase stiGoogleAnalyticsDatabase, StiDictionary dictionary, bool isNewDatabase);

	Task<DialogResult> AzureBlobStorageDatabaseEditAsync(StiAzureBlobStorageDatabase database, StiDictionary dictionary, bool newDatabase);

	DialogResult QuickBooksDatabaseEdit(StiQuickBooksDatabase quickBooksDatabase, StiDictionary dictionary, bool newDatabase);

	Task<DialogResult> QuickBooksDatabaseEditAsync(StiQuickBooksDatabase quickBooksDatabase, StiDictionary dictionary, bool newDatabase);

	DialogResult XmlDatabaseEdit(StiXmlDatabase database, StiDictionary dictionary, bool newDatabase);

	StiUserNameAndPassword PromptUserNameAndPassword();

	bool DataStoreAdapterEdit(StiDataAdapterService adapter, StiDictionary dictionary, StiDataSource dataSource);

	bool DataStoreAdapterNew(StiDataAdapterService adapter, StiDictionary dictionary, StiDataSource dataSource);

	bool VirtualAdapterEdit(StiVirtualAdapterService adapter, StiDictionary dictionary, StiDataSource dataSource);

	bool VirtualAdapterNew(StiVirtualAdapterService adapter, StiDictionary dictionary, StiDataSource dataSource);

	bool CrossTabAdapterNew(StiCrossTabAdapterService adapter, StiDictionary dictionary, StiDataSource dataSource);

	bool CrossTabAdapterEdit(StiCrossTabAdapterService adapter, StiDictionary dictionary, StiDataSource dataSource);
}
