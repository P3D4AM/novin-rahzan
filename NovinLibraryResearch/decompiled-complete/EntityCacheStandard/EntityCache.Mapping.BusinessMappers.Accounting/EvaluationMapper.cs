using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Evaluation;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class EvaluationMapper : IItemAsyncMapper<EvaluationBusiness, EvaluationBusiness>
{
	public static EvaluationMapper Instance { get; private set; } = new EvaluationMapper();


	public async Task<EvaluationBusiness> MapAsync(string contextGuid, EvaluationBusiness source, EvaluationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = await EvaluationBusiness.GenerateDefaultAsync(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Status = source.Status;
			destination.Modified = source.Modified;
			destination.Id2 = source.Id2;
			destination.DateM = source.DateM;
			destination.SanadNo = source.SanadNo;
			destination.SanadSt = source.SanadSt;
			destination.CashierGuid = source.CashierGuid;
			destination.Description = source.Description;
			destination.AnbarGardaniEzafeGuid = source.AnbarGardaniEzafeGuid;
			destination.AnbarGardaniKasrGuid = source.AnbarGardaniKasrGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\EvaluationMapper.cs", 48);
			throw;
		}
	}

	public Task<List<EvaluationBusiness>> MapListAsync(string contextGuid, IEnumerable<EvaluationBusiness> source, List<EvaluationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
