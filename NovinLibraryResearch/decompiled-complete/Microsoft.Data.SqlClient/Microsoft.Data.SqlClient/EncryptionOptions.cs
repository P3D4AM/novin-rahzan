namespace Microsoft.Data.SqlClient;

internal enum EncryptionOptions
{
	OFF = 0,
	ON = 1,
	NOT_SUP = 2,
	REQ = 3,
	LOGIN = 4,
	OPTIONS_MASK = 63,
	CTAIP = 64,
	CLIENT_CERT = 128
}
