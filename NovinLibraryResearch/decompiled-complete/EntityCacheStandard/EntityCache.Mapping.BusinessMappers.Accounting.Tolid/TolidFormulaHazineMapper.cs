using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Tolid;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Tolid;

public class TolidFormulaHazineMapper : IItemAsyncMapper<TolidFormulaHazineBusiness, TolidFormulaHazineBusiness>
{
	public static TolidFormulaHazineMapper Instance { get; private set; } = new TolidFormulaHazineMapper();


	public Task<TolidFormulaHazineBusiness> MapAsync(string contextGuid, TolidFormulaHazineBusiness source, TolidFormulaHazineBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TolidFormulaHazineBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = Guid.NewGuid();
			destination.Modified = DateTime.Now;
			destination.MoeinGuid = source.MoeinGuid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.Price = source.Price;
			destination.Description = source.Description;
			destination.Type = source.Type;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidFormula\\TolidFormulaHazineMapper.cs", 39);
			throw;
		}
	}

	public Task<List<TolidFormulaHazineBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidFormulaHazineBusiness> source, List<TolidFormulaHazineBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
