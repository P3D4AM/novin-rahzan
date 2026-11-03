using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting.Tolid;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Accounting.Tolid.Base;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Tolid;

public class TolidFormulaMapper : IItemAsyncMapper<TolidFormulaBusiness, TolidFormulaBusiness>
{
	public static TolidFormulaMapper Instance { get; private set; } = new TolidFormulaMapper();


	public async Task<TolidFormulaBusiness> MapAsync(string contextGuid, TolidFormulaBusiness source, TolidFormulaBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 4;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new TolidFormulaBusiness(contextGuid);
			}
			destination.DateM = source.DateM;
			destination.Title = source.Title;
			destination.Description = source.Description;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ContextGuid = contextGuid;
			OrderBusiness sourceMavad = await source.GetMavadListAsync(EnSystem.Mapper, tranName, token);
			OrderBusiness sourceExport = await source.GetExportListAsync(EnSystem.Mapper, tranName, token);
			IReadOnlyList<IBaseHazineBusiness> sourceHazine = source.HazineList;
			if ((sourceMavad?.ListKala?.Any()).GetValueOrDefault())
			{
				await destination.AddMavadListAsync(EnSystem.Mapper, sourceMavad.ListKala, clearDets: true, tranName, token);
			}
			if (sourceHazine?.Any() ?? false)
			{
				await destination.AddHazineListAsync(sourceHazine, EnSystem.Mapper);
			}
			if ((sourceExport?.ListKala?.Any()).GetValueOrDefault())
			{
				await destination.AddExportListAsync(EnSystem.Mapper, sourceExport.ListKala, clearDets: true, tranName, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidFormula\\TolidFormulaMapper.cs", 46);
			throw;
		}
		return destination;
	}

	public Task<List<TolidFormulaBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidFormulaBusiness> source, List<TolidFormulaBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
