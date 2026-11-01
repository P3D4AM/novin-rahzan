using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities.Attendance;
using PacketParser.Mapping;
using Serilog;
using TimeAttendanceDAL.Dal.Salary;

namespace TimeAttendanceDAL.Mapper;

public class FishDetailMapper : IItemAsyncMapper<FishDetJson, WebAttFishDetail>
{
	public static FishDetailMapper Instance { get; private set; } = new FishDetailMapper();


	public async Task<WebAttFishDetail> MapAsync(string contextGuid, FishDetJson source, WebAttFishDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			if (source == null)
			{
				destination = null;
				return await Task.FromResult<WebAttFishDetail>(destination);
			}
			if (destination == null)
			{
				destination = new WebAttFishDetail();
			}
			destination.HesabId = source.HesabId;
			destination.Debit = source.Debit;
			destination.Credit = source.Credit;
			destination.TmpValue = source.TmpValue;
			destination.Description = source.Description;
			destination.TypeAccount = source.TypeAccount;
			destination.Type = source.Type;
			destination.Name = source.Name;
			destination.IsDecrease = source.IsDecrease;
			destination.Formula = source.Formula;
			destination.Privilege = source.Privilege;
			destination.TotalPayable = source.TotalPayable;
			destination.RemainingBeforeBill = source.RemainingBeforeBill;
			destination.StartMounthPayM = source.StartMounthPayM;
			WebAttFishDetail val = destination;
			val.CodeMoein = await source.GetCodeMoeinAsync(token);
			val = destination;
			val.CodeTafsil = await source.GetCodeTafsilAsync(token);
			destination.Taxable = source.Taxable;
			destination.Insurance = source.Insurance;
			destination.DisableCompute = source.DisableCompute;
			return destination;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public Task<List<WebAttFishDetail>> MapListAsync(string contextGuid, IEnumerable<FishDetJson> source, List<WebAttFishDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return MapperUtilities.MapListAsync<FishDetJson, WebAttFishDetail>(source, contextGuid, (IItemAsyncMapper<FishDetJson, WebAttFishDetail>)this, destination, tranName, (short)20);
	}
}
