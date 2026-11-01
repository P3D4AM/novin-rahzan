using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;
using Serilog;
using TimeAttendanceDAL.Dal.FLRGharardad;

namespace TimeAttendanceDAL.Mapper;

public class ContractDetailMapper : IItemAsyncMapper<GharardadDetNew, WebAttContractDetail>
{
	public static ContractDetailMapper Instance { get; } = new ContractDetailMapper();


	public async Task<WebAttContractDetail> MapAsync(string contextGuid, GharardadDetNew source, WebAttContractDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			if (source == null)
			{
				destination = null;
				return await Task.FromResult<WebAttContractDetail>(destination);
			}
			if (destination == null)
			{
				destination = new WebAttContractDetail();
			}
			destination.HesabId = source.HesabId;
			destination.Name = source.Name;
			destination.nValue = source.nValue;
			destination.Order = source.Order;
			destination.Desc = source.Desc;
			destination.TypeAccount = source.TypeAccount;
			destination.IsDecrease = source.IsDecrease;
			destination.Account = source.Account;
			destination.Formula = source.Formula;
			destination.Privilege = source.Privilege;
			WebAttContractDetail val = destination;
			val.SetTaxable(await source.GetTaxableAsync(token));
			val = destination;
			val.SetInsurance(await source.GetInsuranceAsync(token));
			return await Task.FromResult<WebAttContractDetail>(destination);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public Task<List<WebAttContractDetail>> MapListAsync(string contextGuid, IEnumerable<GharardadDetNew> source, List<WebAttContractDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return MapperUtilities.MapListAsync<GharardadDetNew, WebAttContractDetail>(source, contextGuid, (IItemAsyncMapper<GharardadDetNew, WebAttContractDetail>)this, destination, tranName, (short)20);
	}
}
