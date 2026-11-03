using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Accounting;
using PacketParser.Entities.Filters;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.DalInterfaces;

public interface IDalTurnover
{
	ENSource Source { get; }

	ITafsil GetTafsil(short depId, Guid guid, CancellationToken token = default(CancellationToken));

	Task<ITafsil> GetTafsilAsync(EnSystem system, short depId, Guid guid, CancellationToken token = default(CancellationToken));

	IKol GetKol(short depId, Guid guid, CancellationToken token = default(CancellationToken));

	IMoein GetMoein(short depId, Guid guid, string tranName = "", CancellationToken token = default(CancellationToken), int timeOutMiliSeconds = 30000);

	Task<IProduct> GetProductAsync(short depId, Guid guid, CancellationToken token = default(CancellationToken));

	IPrdVahed GetPrdVahed(short depId, Guid guid, CancellationToken token = default(CancellationToken));

	Task<ReturnedSaveFuncInfoWithValue<IEnumerable<GardeshTafsil>>> GetAllAsync(string contextGuid, GardeshTafsiliFilter filter, EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken), int timeOutMiliSeconds = 30000);

	Task<IUserAcc> GetUserAccAsync(short depId, Guid userAccGuid, CancellationToken token = default(CancellationToken));

	Task<string> GetUserAccNameAsync(short depId, Guid userAccGuid, CancellationToken token = default(CancellationToken));

	Task<ISanadPrint> GetSanadPrintAsync(Guid asGuid, Document asType, CancellationToken token = default(CancellationToken));
}
