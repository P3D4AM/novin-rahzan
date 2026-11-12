using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using Stimulsoft.Base.Json;

namespace Stimulsoft.Base;

public static class StiExceptionProviderHelper
{
	private sealed class ReportErrorCommand
	{
		public string Ident { get; } = "SystemReportError";


		public string UserName { get; set; }

		public string ErrorSubject { get; set; }

		public string ErrorBody { get; set; }

		public string ErrorNumber { get; set; }

		public bool ResultSuccess { get; set; }

		public byte[] SaveToString()
		{
			return StiGZipHelper.ConvertStringToByteArray(JsonConvert.SerializeObject(this, Formatting.Indented, StiJsonHelper.DefaultSerializerSettings));
		}

		public void Load(string str)
		{
			JsonConvert.PopulateObject(str, this);
		}
	}

	public static Task<bool> SendAsync(string userName, string errorSubject, string errorBody, string errorNumber)
	{
		return Task.Factory.StartNew(() => Send(userName, errorSubject, errorBody, errorNumber));
	}

	private static bool Send(string userName, string errorSubject, string errorBody, string errorNumber)
	{
		try
		{
			string requestUriString = (string.IsNullOrEmpty(StiExceptionProvider.ServerUrl) ? "https://reports.stimulsoft.com/1/runcommand" : StiExceptionProvider.ServerUrl);
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			WebRequest webRequest = WebRequest.Create(requestUriString);
			webRequest.Proxy = StiProxy.GetProxy();
			webRequest.Method = "POST";
			ReportErrorCommand reportErrorCommand = new ReportErrorCommand
			{
				UserName = userName,
				ErrorSubject = errorSubject,
				ErrorBody = errorBody.Replace("\r\n", "<br>"),
				ErrorNumber = errorNumber
			};
			byte[] array = reportErrorCommand.SaveToString();
			webRequest.ContentLength = array.Length;
			webRequest.ContentType = "application/json";
			using (Stream stream = webRequest.GetRequestStream())
			{
				stream.Write(array, 0, array.Length);
			}
			webRequest.Timeout = 30000;
			webRequest.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
			using WebResponse webResponse = webRequest.GetResponse();
			using StreamReader streamReader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
			string str = streamReader.ReadToEnd();
			ReportErrorCommand reportErrorCommand2 = new ReportErrorCommand();
			reportErrorCommand2.Load(str);
			return reportErrorCommand2.ResultSuccess;
		}
		catch
		{
			return false;
		}
	}
}
