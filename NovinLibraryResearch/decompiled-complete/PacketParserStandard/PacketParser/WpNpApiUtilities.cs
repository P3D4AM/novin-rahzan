using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities;

namespace PacketParser;

public class WpNpApiUtilities
{
	public static string CompatibleVersion { get; } = "0.0.3";


	public static async Task<ReturnedSaveFuncInfoWithValue<UrlDataResponseModel>> InqueryWpExtentitionVerionAsync(WebUser UserInfo, CancellationToken token, bool IsTest = false)
	{
		ReturnedSaveFuncInfoWithValue<UrlDataResponseModel> ret = new ReturnedSaveFuncInfoWithValue<UrlDataResponseModel>
		{
			value = new UrlDataResponseModel()
		};
		string body = "";
		HttpResponseMessage response = new HttpResponseMessage();
		try
		{
			if (UserInfo.WordPressBasicSettings.HostUrl.EndsWith("/"))
			{
				ret.value.Url = UserInfo.WordPressBasicSettings.HostUrl + "wp-json/wc/v3/novin/version";
			}
			else
			{
				ret.value.Url = UserInfo.WordPressBasicSettings.HostUrl + "/wp-json/wc/v3/novin/version";
			}
			HttpClient httpClient = new HttpClient();
			HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, ret.value.Url);
			response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, token);
			UrlDataResponseModel value = ret.value;
			value.Data = await response.Content.ReadAsStringAsync();
			if (string.IsNullOrEmpty(ret.value.Data))
			{
				ret.AddError("پاسخی از سرور دریافت نشد.", "InqueryWpExtentitionVerionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\WpNpApiUtilities.cs", 32);
			}
			if (!ret.value.Data.StartsWith("\"") || !ret.value.Data.EndsWith("\""))
			{
				throw new Exception("api response for version is not valid\r\n" + ret.value.Data);
			}
			response.EnsureSuccessStatusCode();
			ret.value.Data = ret.value.Data.Replace("\"", "");
			Version version = new Version(ret.value.Data);
			if (version.Build <= 0 && version.Major <= 0 && version.Minor <= 0)
			{
				ret.AddError(ret.value.Data, "InqueryWpExtentitionVerionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\WpNpApiUtilities.cs", 43);
				throw new NotImplementedException("url:" + ret.value.Url + "\r\nResul:" + ret.value.Data);
			}
			ret.value.Data = ret.value.Data;
		}
		catch (HttpRequestException exception)
		{
			ReturnedSaveFuncInfoWithValue<UrlDataResponseModel> returnedSaveFuncInfoWithValue = ret;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await HttpHelpers.ParseExceptionAsync(exception, response), ReturnedState.None, "InqueryWpExtentitionVerionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\WpNpApiUtilities.cs", 50);
		}
		catch (OperationCanceledException ex)
		{
			ret.AddReturnedValue(ex, "", "InqueryWpExtentitionVerionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\WpNpApiUtilities.cs", 54);
			ret.AddError($"{ex}", "InqueryWpExtentitionVerionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\WpNpApiUtilities.cs", 55);
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("404"))
			{
				WebErrorLog.Instance.StartLog(ex2, "#Url=>\r\n" + ret.value.Url + "\r\nbody=>" + body, "InqueryWpExtentitionVerionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\WpNpApiUtilities.cs", 60);
			}
			ret.AddReturnedValue(ex2, "body=>" + body, "InqueryWpExtentitionVerionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\WpNpApiUtilities.cs", 63);
			ret.AddError($"{ex2}", "InqueryWpExtentitionVerionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\WpNpApiUtilities.cs", 64);
		}
		return ret;
	}
}
