using System.Data;

namespace Microsoft.Data.ProviderBase;

internal sealed class DbConnectionClosedNeverOpened : DbConnectionClosed
{
	internal static readonly DbConnectionInternal SingletonInstance = new DbConnectionClosedNeverOpened();

	private DbConnectionClosedNeverOpened()
		: base(ConnectionState.Closed, hidePassword: false, allowSetConnectionString: true)
	{
	}
}
