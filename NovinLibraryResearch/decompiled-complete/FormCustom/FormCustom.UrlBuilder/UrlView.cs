using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace FormCustom.UrlBuilder;

public class UrlView
{
	public string UrlCheckError { get; private set; }

	public string Url { get; private set; }

	public int Ordinal { get; set; }

	public bool IsOnline { get; private set; }

	public string StatusConects
	{
		get
		{
			if (!IsOnline)
			{
				return "آفلاین";
			}
			return "آنلاین";
		}
	}

	public async Task SetUrlAsync(string value)
	{
		try
		{
			Url = value;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = await CheckUrlValidationAsync(Url, default(CancellationToken));
			UrlCheckError = returnedSaveFuncInfo.GetErrorMessage();
			if (returnedSaveFuncInfo.HasError)
			{
				IsOnline = false;
			}
			else
			{
				IsOnline = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetUrlAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\UrlView.cs", 30);
			IsOnline = false;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> CheckUrlValidationAsync(string url, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (token.IsCancellationRequested)
			{
				return ret;
			}
			await url.CheckWebAddressExsistAsync(IsSSl: false, throwExceptin: true);
		}
		catch (WebException ex)
		{
			if (ex.Status == WebExceptionStatus.ProtocolError)
			{
				if (((HttpWebResponse)ex.Response).StatusCode != HttpStatusCode.BadRequest)
				{
					ret.AddError(EnSystem.None, "Url Not Valid=>" + ex.Message, "", "CheckUrlValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\UrlView.cs", 47);
				}
			}
			else
			{
				ret.AddError(EnSystem.None, "Url Not Valid=>" + ex.Message, "", "CheckUrlValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\UrlView.cs", 49);
			}
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "Url:" + url, "CheckUrlValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\UrlView.cs", 53);
			ret.AddReturnedValue(ex2, "", "CheckUrlValidationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\UrlBuilder\\UrlView.cs", 54);
		}
		return ret;
	}
}
