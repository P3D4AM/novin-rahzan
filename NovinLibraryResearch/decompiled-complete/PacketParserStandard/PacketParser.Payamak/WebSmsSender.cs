using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs.Sms;

namespace PacketParser.Payamak;

public static class WebSmsSender
{
	public static string FixDomain(string Domain)
	{
		using (new JaegerMonitor("FixDomain", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\WebSmsSender.cs"))
		{
			try
			{
				return Domain.ToLower().Replace(".com", "").Replace(".ir", "");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FixDomain", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\WebSmsSender.cs", 24);
				throw;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<decimal>> GetCreditAsync(string Username, string Pass, string Domain, CancellationToken inputToken)
	{
		using (new JaegerMonitor("GetCreditAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\WebSmsSender.cs"))
		{
			int line = 0;
			ReturnedSaveFuncInfoWithValue<decimal> ret = new ReturnedSaveFuncInfoWithValue<decimal>();
			try
			{
				CancellationToken token = CancellationTokenSource.CreateLinkedTokenSource(inputToken, new CancellationTokenSource(60000).Token).Token;
				line = 1;
				Domain = FixDomain(Domain);
				line = 3;
				using HttpClient client = new HttpClient();
				line = 4;
				FormUrlEncodedContent content = new FormUrlEncodedContent(new _003C_003Ez__ReadOnlyArray<KeyValuePair<string, string>>(new KeyValuePair<string, string>[3]
				{
					new KeyValuePair<string, string>("UserName", Username),
					new KeyValuePair<string, string>("Password", Pass),
					new KeyValuePair<string, string>("Domain", Domain)
				}));
				line = 10;
				HttpResponseMessage res = await client.PostAsync("http://webservice.novinp.ir/sms/Npsms/GetCredit/", content, token);
				line = 11;
				if (res.IsSuccessStatusCode)
				{
					line = 12;
					string data = await res.Content.ReadAsStringAsync();
					line = 13;
					NpsmsGetCreditResponse npsmsGetCreditResponse = data.FromJson<NpsmsGetCreditResponse>(sendErrorIfException: false, "GetCreditAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\WebSmsSender.cs", 55);
					if (npsmsGetCreditResponse == null)
					{
						return ret.AddError("response == null", "GetCreditAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\WebSmsSender.cs", 56);
					}
					line = 14;
					if (npsmsGetCreditResponse.Message == ResponseResult.Succeeded)
					{
						ret.value = npsmsGetCreditResponse.Credit.ParseToDecimal();
						return ret;
					}
					line = 15;
				}
				line = 200;
				ret.AddError($"res.IsSuccessStatusCode is false!//Wrong response status code//UserName:{Username}/Pass:{Pass}/domain:{Domain}/ResCode:{res?.StatusCode}", "GetCreditAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\WebSmsSender.cs", 67);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "GetCreditAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\WebSmsSender.cs", 69);
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "GetCreditAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\WebSmsSender.cs", 72);
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "GetCreditAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Payamak\\WebSmsSender.cs", 73);
			}
			return ret;
		}
	}
}
