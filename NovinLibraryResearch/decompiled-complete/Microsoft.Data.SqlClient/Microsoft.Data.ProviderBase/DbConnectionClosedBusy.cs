using System.Data;

namespace Microsoft.Data.ProviderBase;

internal sealed class DbConnectionClosedBusy : DbConnectionBusy
{
	internal static readonly DbConnectionInternal SingletonInstance = new DbConnectionClosedBusy();

	private DbConnectionClosedBusy()
		: base(ConnectionState.Closed)
	{
	}
}
