using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Mapping.BusinessMappers;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Filters;
using PacketParser.EventArgs;

namespace EntityCache.Business;

public class ExecuteGardeshTafsiliBusiness : ExecuteQueryBusiness
{
	public ExecuteGardeshTafsiliBusiness(string contextGuid)
		: base(contextGuid)
	{
	}

	internal static async Task<ReturnedSaveFuncInfoWithValue<IEnumerable<GardeshTafsilBusiness>>> GetGardehHesabAsync(string contextGuid, GardeshTafsiliFilter filter, EnSystem system, CancellationToken token, int timeOutMiliSeconds = 30000)
	{
		ReturnedSaveFuncInfoWithValue<IEnumerable<GardeshTafsilBusiness>> ret = new ReturnedSaveFuncInfoWithValue<IEnumerable<GardeshTafsilBusiness>>();
		try
		{
			if ((ClsCache.GetCache(contextGuid).BufferLoginState?.Status ?? EnLoginResult.NotExists) != EnLoginResult.Success)
			{
				ret.AddError(system, "به علت قطع بودن ارتباط اینترنت امکان تهیه گزارش وجود ندارد.".GetTranslate(), "GetGardehHesabAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ExecuteQuery\\ExecuteGardeshTafsiliBusiness.cs", 28);
				return ret;
			}
			ExecuteQueryRequestEventArgs queryRequest = new ExecuteQueryRequestEventArgs
			{
				JsonRequest = Json.ToStringJson(filter),
				PacketReferenceId = Guid.NewGuid(),
				Type = PacketType.GardeshTafsili
			};
			Task<ExecuteQueryResponseEventArgs> response = ExecuteQueryBusiness.GetQueryResponseAsync(contextGuid, queryRequest, token, timeOutMiliSeconds);
			ReturnedSaveFuncInfoWithValue<IEnumerable<GardeshTafsilBusiness>> returnedSaveFuncInfoWithValue;
			if (!(await response).Result)
			{
				returnedSaveFuncInfoWithValue = ret;
				returnedSaveFuncInfoWithValue.AddError((await response).ResultMessage, "GetGardehHesabAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ExecuteQuery\\ExecuteGardeshTafsiliBusiness.cs", 39);
			}
			if (ret.HasError)
			{
				return ret;
			}
			List<WebGardeshTafsiliCompact> source = (await response).JsonResult.FromJson<List<WebGardeshTafsiliCompact>>("GetGardehHesabAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ExecuteQuery\\ExecuteGardeshTafsiliBusiness.cs", 41);
			returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.value = await GardeshTafsilMapper.Instance.MapListAsync(contextGuid, source, new List<GardeshTafsilBusiness>(), "", token);
			if ((ret?.value?.Any()).GetValueOrDefault())
			{
				await Task.WhenAll(ret.value.Select((GardeshTafsilBusiness p) => p.SetPrdAsync(system)));
			}
			ret.value = ret.value?.OrderBy((GardeshTafsilBusiness p) => p.RowIndex)?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetGardehHesabAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ExecuteQuery\\ExecuteGardeshTafsiliBusiness.cs", 49);
			ret.AddReturnedValue(ex, "", "GetGardehHesabAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\ExecuteQuery\\ExecuteGardeshTafsiliBusiness.cs", 50);
		}
		return ret;
	}
}
