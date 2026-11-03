using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace PacketParser.TaxService;

public interface ITaxConnectionManager
{
	Task<ITaxApiConnection> GetConnectionAsync(string clientId, bool awaitOnSemaphore);

	Task UnHandleConnectionAsync(ITaxApiConnection sender);

	Task<ReturnedSaveFuncInfoWithValue<ITaxApiConnection>> GetOrHandleConnectionAsync(WebUser user, string connectionUniqeId, bool handleLoginResponse);

	Task<ReturnedSaveFuncInfoWithValue<ITaxApiConnection>> GetOrHandleConnectionAsync(ConnectionEventArgs<WebTaxLogin> connection, bool handleLoginResponse);
}
