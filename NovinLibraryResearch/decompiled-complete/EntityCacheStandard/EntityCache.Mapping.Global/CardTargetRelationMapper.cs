using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Global;

public class CardTargetRelationMapper : IItemAsyncMapper<CardTargetRelationBusiness, CardTargetRelationBusiness>
{
	public static CardTargetRelationMapper Instance { get; private set; } = new CardTargetRelationMapper();


	public Task<CardTargetRelationBusiness> MapAsync(string contextGuid, CardTargetRelationBusiness source, CardTargetRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			if (destination == null)
			{
				destination = new CardTargetRelationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.CardGuid = destination.CardGuid;
			destination.TargetGuid = destination.TargetGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\CardTargetRelationMapper.cs", 32);
			throw;
		}
	}

	public Task<List<CardTargetRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<CardTargetRelationBusiness> source, List<CardTargetRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
