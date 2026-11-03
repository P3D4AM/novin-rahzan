using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebLinkGenerator : Serializable<WebLinkGenerator>, INpDisposable, IDisposable
{
	private static string apiUrl = HttpHelpers.ApiUrl + "api/LinkGenerator/";

	public Guid Guid { get; set; }

	public string OrginalUrl { get; set; }

	public string Path { get; set; }

	public string LongUrl => OrginalUrl + Path;

	public string ShortUrl { get; set; }

	public ENSource Source { get; set; }

	public short DepId { get; set; }

	public WebLinkGenerator(Guid guid, short depId, ENSource source)
	{
		Guid = guid;
		DepId = depId;
		Source = source;
	}

	public void GenerateUrl(string longurl = "https://nphozor.ir/AdminLogin")
	{
		checked
		{
			try
			{
				if (Guid == Guid.Empty)
				{
					Guid = Guid.NewGuid();
				}
				string[] array = longurl.Split(new char[1] { '/' });
				string text = array[0];
				string text2 = array[2];
				OrginalUrl = text + "//" + text2;
				for (int i = 3; i < array.Length; i++)
				{
					Path = Path + "/" + array[i];
				}
				int num = new Random().Next(10000000, 99999999) + Guid.GetHashCode();
				ShortUrl = OrginalUrl + "/" + $"0x{num.GetHashCode():X8}";
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateUrl", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLinkGenerator.cs", 46);
				throw;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebLinkGenerator>> GetAsync(string shortUrl, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<WebLinkGenerator> result = new ReturnedSaveFuncInfoWithValue<WebLinkGenerator>();
		try
		{
			Dictionary<string, string> headers = new Dictionary<string, string> { { "ShortUrl", shortUrl } };
			ReturnedSaveFuncInfoWithValue<WebResponse<WebLinkGenerator>> res = await (apiUrl + "Get").GetFromApi<WebLinkGenerator>(token, 3, headers);
			result.AddReturnedValue(res, ReturnedState.None, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLinkGenerator.cs", 57);
			if (res.value.HttpClietnStatusCode == HttpStatusCode.NoContent)
			{
				await result.AddErrorAsync("متاسفانه لینک مورد نظر یافت نشد", "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLinkGenerator.cs", 59);
			}
			if (result.HasError)
			{
				return result;
			}
			result.value = res.value?.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLinkGenerator.cs", 65);
			result.AddReturnedValue(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebLinkGenerator.cs", 66);
		}
		return result;
	}
}
