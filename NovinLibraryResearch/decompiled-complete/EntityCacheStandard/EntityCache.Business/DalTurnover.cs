using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DalInterfaces;
using PacketParser.Entities;
using PacketParser.Entities.Accounting;
using PacketParser.Entities.Filters;
using PacketParser.Entities.Interfaces.Accounting;

namespace EntityCache.Business;

public class DalTurnover : IDalTurnover
{
	private static ConcurrentDictionary<string, DalTurnover> _dic = new ConcurrentDictionary<string, DalTurnover>();

	public ENSource Source { get; }

	public string ContextGuid { get; set; }

	public DalTurnover(string contextGuid, ENSource source)
	{
		Source = source;
		ContextGuid = contextGuid;
	}

	public static DalTurnover Get(string contextGuid, ENSource source)
	{
		DalTurnover dalTurnover = null;
		try
		{
			_dic.TryGetValue(contextGuid, out var value);
			if (value == null)
			{
				value = new DalTurnover(contextGuid, source);
				_dic.TryAdd(contextGuid, value);
			}
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 648);
			throw;
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<IEnumerable<GardeshTafsil>>> GetAllAsync(string contextGuid, GardeshTafsiliFilter filter, EnSystem system, string tranName = "", CancellationToken token = default(CancellationToken), int timeOutMiliSeconds = 30000)
	{
		string debugInfo = "Filter:" + Json.ToStringJson(filter) + " ";
		ReturnedSaveFuncInfoWithValue<IEnumerable<GardeshTafsil>> ret = new ReturnedSaveFuncInfoWithValue<IEnumerable<GardeshTafsil>>();
		try
		{
			ReturnedSaveFuncInfoWithValue<List<GardeshTafsilBusiness>> returnedSaveFuncInfoWithValue = await GardeshTafsilBusiness.GetAllAsync(contextGuid, filter, system, tranName, token, timeOutMiliSeconds);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 661);
			if (ret.HasError)
			{
				return ret;
			}
			if (!(returnedSaveFuncInfoWithValue?.value?.Any()).GetValueOrDefault())
			{
				return ret;
			}
			ReturnedSaveFuncInfoWithValue<IEnumerable<GardeshTafsil>> returnedSaveFuncInfoWithValue2 = ret;
			returnedSaveFuncInfoWithValue2.value = await GardeshTafsilMapper.Instance.MapListAsync(contextGuid, returnedSaveFuncInfoWithValue.value, new List<GardeshTafsil>(), tranName, token);
			ret.value = ret.value?.OrderBy((GardeshTafsil p) => p.RowIndex)?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "DebugInfo:" + debugInfo, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 669);
			ret.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs", 670);
		}
		return ret;
	}

	public ITafsil GetTafsil(short depId, Guid guid, CancellationToken token = default(CancellationToken))
	{
		return TafsilBusiness.Get(ContextGuid, guid, token);
	}

	public async Task<ITafsil> GetTafsilAsync(EnSystem system, short depId, Guid guid, CancellationToken token = default(CancellationToken))
	{
		return await TafsilBusiness.GetAsync(ContextGuid, system, guid, "", token, "GetTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\GardeshTafsilBusiness.cs");
	}

	public IKol GetKol(short depId, Guid guid, CancellationToken token = default(CancellationToken))
	{
		return KolBusiness.Get(ContextGuid, guid, token);
	}

	public IMoein GetMoein(short depId, Guid guid, string tranName = "", CancellationToken token = default(CancellationToken), int timeOutMiliSeconds = 30000)
	{
		return MoeinBusiness.Get(ContextGuid, guid, token, tranName);
	}

	public async Task<IProduct> GetProductAsync(short depId, Guid guid, CancellationToken token = default(CancellationToken))
	{
		return await ProductBusiness.GetAsync(ContextGuid, guid, token);
	}

	public async Task<IUserAcc> GetUserAccAsync(short depid, Guid userAccGuid, CancellationToken token = default(CancellationToken))
	{
		return await UserAccBusiness.GetAsync(ContextGuid, userAccGuid, token);
	}

	public async Task<string> GetUserAccNameAsync(short depid, Guid userAccGuid, CancellationToken token = default(CancellationToken))
	{
		return (await UserAccBusiness.GetAsync(ContextGuid, userAccGuid, token))?.Name ?? "";
	}

	public IPrdVahed GetPrdVahed(short depId, Guid guid, CancellationToken token = default(CancellationToken))
	{
		return PrdVahedBusiness.Get(ContextGuid, guid, token);
	}

	public async Task<ISanadPrint> GetSanadPrintAsync(Guid asGuid, Document asType, CancellationToken token = default(CancellationToken))
	{
		return await SanadPrintBusiness.GetAsync(ContextGuid, asGuid, asType, token);
	}
}
