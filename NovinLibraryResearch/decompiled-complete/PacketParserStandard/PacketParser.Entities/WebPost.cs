using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;

namespace PacketParser.Entities;

public class WebPost : IPost, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	private static string apiUrl = HttpHelpers.ApiUrl + "webpost/";

	public byte[] Image { get; set; }

	public string Link { get; set; }

	public DateTime Startdate { get; set; } = DateTime.Now;


	public string StartDateSh => Startdate.MiladiToCalture();

	public DateTime EndDate { get; set; } = DateTime.Now;


	public string EndDateSh => EndDate.MiladiToCalture();

	public string Title { get; set; }

	public EnTypePost Type { get; set; }

	public string TypeDisplay => Type.GetDisplay();

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Guid Guid { get; set; }

	public Dictionary<string, string> MetaData { get; set; }

	public ENSource Source { get; set; }

	public short Ordinal { get; set; }

	public string SourceDisplay => Source.GetDisplay();

	public static async Task<List<WebPost>> GetListAllAsync(CancellationToken token = default(CancellationToken), int timeOutMilliSeconds = 15000)
	{
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebPost>>> returnedSaveFuncInfoWithValue = await (apiUrl + "getall").GetFromApi<List<WebPost>>(token, 3, null, timeOutMilliSeconds);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				return null;
			}
			return returnedSaveFuncInfoWithValue.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetListAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPost.cs", 40);
		}
		return null;
	}

	public static async Task<List<WebPost>> GetAllBySourceAsync(ENSource source, CancellationToken token = default(CancellationToken), int timeOutMilliSeconds = 15000)
	{
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebPost>>> returnedSaveFuncInfoWithValue = await $"{apiUrl}getbysource?source={source}".GetFromApi<List<WebPost>>(token, 3, null, timeOutMilliSeconds);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				return null;
			}
			return returnedSaveFuncInfoWithValue.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllBySourceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPost.cs", 51);
		}
		return null;
	}

	public async Task<ReturnedSaveFuncInfo> DeleteAsync(Guid guid, CancellationToken token = default(CancellationToken), int timeOutMilliSeconds = 15000)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Dictionary<string, string> headers = new Dictionary<string, string> { 
			{
				"guid",
				guid.ToString()
			} };
			if ((await (apiUrl + "delete").GetFromApi<ReturnedSaveFuncInfo>(token, 3, headers, timeOutMilliSeconds)).value.HttpClietnStatusCode == HttpStatusCode.BadRequest)
			{
				res.AddError("عملیات حذف با موفقیت انجام نشد", "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPost.cs", 62);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPost.cs", 66);
			res.AddReturnedValue(res, ReturnedState.None, "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPost.cs", 67);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(CancellationToken token = default(CancellationToken), int timeOutMilliSeconds = 15000)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if ((await HttpHelpers.PostToApi(this, apiUrl + "save", null, strongDeserialize: false, timeOutMilliSeconds, 3)).HasError)
			{
				res.AddError("عملیات ذخیره سازی با موفقیت انجام نشد", "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPost.cs", 78);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPost.cs", 82);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPost.cs", 83);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> ChangeOrdinalAsync(string contextGuid, short value, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			short oldOrinal = Ordinal;
			Ordinal = value;
			List<WebPost> list = await GetListAllAsync(token);
			if (list == null || !list.Any())
			{
				return res;
			}
			WebPost webPost = list.FirstOrDefault((WebPost q) => q.Ordinal == Ordinal && q.Guid != Guid);
			if (webPost != null)
			{
				webPost.Ordinal = oldOrinal;
			}
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(token), ReturnedState.None, "ChangeOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPost.cs", 100);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPost.cs", 104);
			res.AddReturnedValue(ex, "", "ChangeOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPost.cs", 105);
		}
		return res;
	}
}
