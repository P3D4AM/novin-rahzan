using Microsoft.Data.SqlClient;

namespace Microsoft.Data.Common;

internal static class DbConnectionStringDefaults
{
	private const string _emptyString = "";

	internal const string Driver = "";

	internal const string Dsn = "";

	internal const bool AdoNetPooler = false;

	internal const string FileName = "";

	internal const int OleDbServices = -13;

	internal const string Provider = "";

	internal const bool Unicode = false;

	internal const bool OmitOracleConnectionName = false;

	internal const ApplicationIntent ApplicationIntent = ApplicationIntent.ReadWrite;

	internal const string ApplicationName = "Framework Microsoft SqlClient Data Provider";

	internal const bool AsynchronousProcessing = false;

	internal const string AttachDBFilename = "";

	internal const int CommandTimeout = 30;

	internal const int ConnectTimeout = 15;

	internal const bool ConnectionReset = true;

	internal const bool ContextConnection = false;

	internal const string CurrentLanguage = "";

	internal const string DataSource = "";

	internal const bool Encrypt = false;

	internal const bool Enlist = true;

	internal const string FailoverPartner = "";

	internal const string InitialCatalog = "";

	internal const bool IntegratedSecurity = false;

	internal const int LoadBalanceTimeout = 0;

	internal const bool MultipleActiveResultSets = false;

	internal const bool MultiSubnetFailover = false;

	internal static readonly bool TransparentNetworkIPResolution = !LocalAppContextSwitches.DisableTNIRByDefault;

	internal const int MaxPoolSize = 100;

	internal const int MinPoolSize = 0;

	internal const string NetworkLibrary = "";

	internal const int PacketSize = 8000;

	internal const string Password = "";

	internal const bool PersistSecurityInfo = false;

	internal const bool Pooling = true;

	internal const bool TrustServerCertificate = false;

	internal const string TypeSystemVersion = "Latest";

	internal const string UserID = "";

	internal const bool UserInstance = false;

	internal const bool Replication = false;

	internal const string WorkstationID = "";

	internal const string TransactionBinding = "Implicit Unbind";

	internal const int ConnectRetryCount = 1;

	internal const int ConnectRetryInterval = 10;

	internal static readonly SqlAuthenticationMethod Authentication = SqlAuthenticationMethod.NotSpecified;

	internal static readonly SqlConnectionColumnEncryptionSetting ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Disabled;

	internal const string EnclaveAttestationUrl = "";

	internal const SqlConnectionAttestationProtocol AttestationProtocol = SqlConnectionAttestationProtocol.NotSpecified;

	internal const SqlConnectionIPAddressPreference IPAddressPreference = SqlConnectionIPAddressPreference.IPv4First;

	internal const string Certificate = "";

	internal const PoolBlockingPeriod PoolBlockingPeriod = PoolBlockingPeriod.Auto;
}
