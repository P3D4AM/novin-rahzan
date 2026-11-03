using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.EventArgs;

namespace PacketParser.Entities.Interfaces;

public interface ISyncFromClient2Server<T> : IDeliverableEntity, IEditableDelivery where T : class, IHasJustGuid
{
	Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName);

	Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxparalelTask);
}
