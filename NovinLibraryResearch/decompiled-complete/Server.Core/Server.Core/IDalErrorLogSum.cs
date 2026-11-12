using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.CRM;

namespace Server.Core;

public interface IDalErrorLogSum
{
	Task<IEnumerable<WebErrorLogSum>> GetAllAsync(short DepId, DateTime Min, DateTime Max, List<ENSource> Sources, short State, bool HasList, short Top, string HashList, CancellationToken cancellationToken);

	Task<WebErrorLogSum> LoadDataAsync(short Depid, int id);

	Task<ReturnedSaveFuncInfoWithValue<List<WebLastErrorReportCrm>>> GetDataAsync(Guid tafsilGuid, int counts, DateTime PassedDate);
}
