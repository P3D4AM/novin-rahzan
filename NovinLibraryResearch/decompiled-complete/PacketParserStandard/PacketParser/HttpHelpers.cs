using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities;
using PacketParser.HttpClients;

namespace PacketParser;

public static class HttpHelpers
{
	public static string ApiUrl { get; set; } = "https://www.novinrank.ir/";


	public static string CrmApiUrl { get; set; } = "https://crm.novinp.ir/";


	public static void SerializeJsonIntoStream(object value, Stream stream)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		value = value ?? throw new ArgumentNullException("value");
		stream = stream ?? throw new ArgumentNullException("stream");
		using StreamWriter streamWriter = new StreamWriter(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false), 1024, leaveOpen: true);
		JsonTextWriter val = new JsonTextWriter((TextWriter)streamWriter)
		{
			Formatting = (Formatting)0
		};
		try
		{
			JsonSerializer.CreateDefault().Serialize((JsonWriter)(object)val, value);
			((JsonWriter)val).Flush();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static T DeserializeJsonWithStreamAsync<T>(Stream source)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		source = source ?? throw new ArgumentNullException("source");
		using StreamReader streamReader = new StreamReader(source);
		JsonTextReader val = new JsonTextReader((TextReader)streamReader);
		try
		{
			return new JsonSerializer().Deserialize<T>((JsonReader)(object)val);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static async Task<T> ReadJsonResponseAsync<T>(HttpResponseMessage response)
	{
		return DeserializeJsonWithStreamAsync<T>(await response.Content.ReadAsStreamAsync());
	}

	public static HttpContent CreateHttpContent(object content)
	{
		HttpContent httpContent = null;
		if (content != null)
		{
			MemoryStream memoryStream = new MemoryStream();
			SerializeJsonIntoStream(content, memoryStream);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			httpContent = new StreamContent(memoryStream);
			httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
		}
		return httpContent;
	}

	public static async Task<HttpResponseMessage> PostStreamAsyncByHandleerror(object content, string url, CancellationToken cancellationToken)
	{
		HttpResponseMessage response = new HttpResponseMessage();
		try
		{
			using HttpClient client = new HttpClient();
			using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
			using HttpContent httpContent = CreateHttpContent(content);
			request.Content = httpContent;
			response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
			response.EnsureSuccessStatusCode();
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (HttpRequestException ex2)
		{
			if (ex2.Message.Contains("503 (Service Unavailable)"))
			{
				response.StatusCode = HttpStatusCode.ServiceUnavailable;
				return response;
			}
			WebErrorLog.Instance.StartLog(ex2, "Content/url/token\r\nUrl:" + url, "PostStreamAsyncByHandleerror", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 96);
			throw;
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "Content/url/token", "PostStreamAsyncByHandleerror", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 103);
			throw;
		}
		return response;
	}

	public static async Task<HttpResponseMessage> PostStreamAsyncByHandleError(object content, string url, Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken), bool useAdminLogin = true)
	{
		customHeaders = FillHeader(customHeaders, useAdminLogin);
		using HttpClient client = new HttpClient();
		HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
		HttpContent content2 = CreateHttpContent(content);
		httpRequestMessage.Content = content2;
		if (customHeaders != null)
		{
			foreach (KeyValuePair<string, string> customHeader in customHeaders)
			{
				client.DefaultRequestHeaders.Add(customHeader.Key, customHeader.Value);
			}
		}
		HttpResponseMessage obj = await client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
		obj.EnsureSuccessStatusCode();
		return obj;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<HttpResponseMessage>> PostStreamAsyncByHandleError(object content, string url, HttpClient client, Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken), bool useAdminLogin = true)
	{
		ReturnedSaveFuncInfoWithValue<HttpResponseMessage> response = new ReturnedSaveFuncInfoWithValue<HttpResponseMessage>();
		try
		{
			customHeaders = FillHeader(customHeaders, useAdminLogin);
			HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
			HttpContent content2 = CreateHttpContent(content);
			httpRequestMessage.Content = content2;
			if (customHeaders != null)
			{
				foreach (KeyValuePair<string, string> customHeader in customHeaders)
				{
					if (!string.IsNullOrEmpty(customHeader.Value))
					{
						client.DefaultRequestHeaders.Add(customHeader.Key, customHeader.Value);
					}
				}
			}
			ReturnedSaveFuncInfoWithValue<HttpResponseMessage> returnedSaveFuncInfoWithValue = response;
			returnedSaveFuncInfoWithValue.value = await client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
			response.value.EnsureSuccessStatusCode();
		}
		catch (OperationCanceledException exception)
		{
			response.AddReturnedValue(exception, "", "PostStreamAsyncByHandleError", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 144);
			throw;
		}
		catch (HttpRequestException ex)
		{
			WebErrorLog.Instance.Warning(ex, "Content/url/client/header/token", "PostStreamAsyncByHandleError", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 149);
			ReturnedSaveFuncInfoWithValue<HttpResponseMessage> returnedSaveFuncInfoWithValue = response;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await ParseExceptionAsync(ex, response.value), ReturnedState.None, "PostStreamAsyncByHandleError", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 150);
		}
		catch (Exception ex2)
		{
			response.AddReturnedValue(ex2, "", "PostStreamAsyncByHandleError", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 154);
			WebErrorLog.Instance.StartLog(ex2, "Content/url/client/header/token\r\nUrl:" + url, "PostStreamAsyncByHandleError", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 155);
		}
		return response;
	}

	public static Task<ReturnedSaveFuncInfoWithValue<HttpResponseMessage>> ParseExceptionAsync(HttpRequestException exception, HttpResponseMessage response)
	{
		ReturnedSaveFuncInfoWithValue<HttpResponseMessage> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<HttpResponseMessage>
		{
			value = response
		};
		try
		{
			if (exception.Message.Contains("The remote name could not be resolved") || exception.Message.Contains("An error occurred while sending the request."))
			{
				string text = exception.Message;
				for (Exception innerException = exception.InnerException; innerException != null; innerException = innerException.InnerException)
				{
					text = text + "\r\n" + innerException.Message;
				}
				returnedSaveFuncInfoWithValue.AddError(EnSystem.None, "دسترسی به سرور به علت قطع بودن ارتباط اینترنتی مهیا نمیباشد.\\r\\n" + text, "ParseExceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 174);
			}
			else if ((returnedSaveFuncInfoWithValue?.value?.StatusCode).GetValueOrDefault() > (HttpStatusCode)0)
			{
				returnedSaveFuncInfoWithValue.AddReturnedHttpStatusCode(returnedSaveFuncInfoWithValue.value.StatusCode, exception, "ParseExceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 180);
			}
			else
			{
				returnedSaveFuncInfoWithValue.AddReturnedValue(exception, "", "ParseExceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 182);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ParseExceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 187);
			returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "ParseExceptionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 188);
		}
		return Task.FromResult(returnedSaveFuncInfoWithValue);
	}

	public static async Task<HttpResponseMessage> SendStreamAsyncByHandleError(object content, string url, HttpClient client, HttpMethod method, Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken), bool useAdminLogin = true)
	{
		customHeaders = FillHeader(customHeaders, useAdminLogin);
		HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, url);
		HttpContent content2 = CreateHttpContent(content);
		httpRequestMessage.Content = content2;
		if (customHeaders != null)
		{
			foreach (KeyValuePair<string, string> customHeader in customHeaders)
			{
				client.DefaultRequestHeaders.Add(customHeader.Key, customHeader.Value);
			}
		}
		HttpResponseMessage obj = await client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
		obj.EnsureSuccessStatusCode();
		return obj;
	}

	[DebuggerHidden]
	public static async Task<ReturnedSaveFuncInfoWithValue<HttpResponseMessage>> GetStreamAsync(string url, HttpClient client, Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken), bool useAdminLogin = true)
	{
		int line = 0;
		ReturnedSaveFuncInfoWithValue<HttpResponseMessage> response = new ReturnedSaveFuncInfoWithValue<HttpResponseMessage>();
		try
		{
			customHeaders = FillHeader(customHeaders, useAdminLogin);
			line = 1;
			if (customHeaders != null)
			{
				line = 2;
				foreach (KeyValuePair<string, string> customHeader in customHeaders)
				{
					line = 3;
					client.DefaultRequestHeaders.Add(customHeader.Key, customHeader.Value);
				}
				line = 4;
			}
			line = 5;
			HttpResponseMessage httpResponseMessage = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
			line = 6;
			ReturnedSaveFuncInfoWithValue<HttpResponseMessage> returnedSaveFuncInfoWithValue = response;
			if (returnedSaveFuncInfoWithValue.value == null)
			{
				returnedSaveFuncInfoWithValue.value = new HttpResponseMessage();
			}
			line = 7;
			response.value = httpResponseMessage;
			line = 8;
			httpResponseMessage.EnsureSuccessStatusCode();
		}
		catch (OperationCanceledException exception)
		{
			response.AddReturnedValue(exception, "URL: " + url, "GetStreamAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 244);
			throw;
		}
		catch (HttpRequestException ex)
		{
			WebErrorLog.Instance.Warning(ex, "Content/url/client/header/token \r\n URL: " + url, "GetStreamAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 249);
			if (ex.Message.Contains("The remote name could not be resolved") || ex.Message.Contains("An error occurred while sending the request."))
			{
				string text = ex.Message;
				for (Exception innerException = ex.InnerException; innerException != null; innerException = innerException.InnerException)
				{
					text = text + "\r\n" + innerException.Message;
				}
				response.AddError(EnSystem.None, "دسترسی به سرور به علت قطع بودن ارتباط اینترنتی مهیا نمیباشد.\\r\\n" + text, "GetStreamAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 259);
			}
			else if (checked((short?)response?.value?.StatusCode).GetValueOrDefault() > 0)
			{
				response.AddReturnedHttpStatusCode(response.value.StatusCode, ex, "GetStreamAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 265);
			}
			else
			{
				response.AddReturnedValue(ex, "", "GetStreamAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 267);
			}
		}
		catch (Exception ex2)
		{
			response.AddReturnedValue(ex2, "", "GetStreamAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 272);
			WebErrorLog.Instance.StartLog(ex2, "url(" + url + ")\n" + $"client/header/token/Line:{line}", "GetStreamAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 273);
			throw;
		}
		return response;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<T>>> PostToApi<T, U>(U item, string url, Dictionary<string, string> headers = null, bool strongDeserialize = false, int timeOutMilliSeconds = 15000, short tryCount = 3, bool useAdminLogin = true) where U : new()
	{
		ReturnedSaveFuncInfoWithValue<WebResponse<T>> result = new ReturnedSaveFuncInfoWithValue<WebResponse<T>>
		{
			value = new WebResponse<T>()
		};
		try
		{
			headers = FillHeader(headers, useAdminLogin);
			using HttpClient ClientHttp = new HttpClient();
			ClientHttp.Timeout = new TimeSpan(0, 0, 0, 0, timeOutMilliSeconds);
			ReturnedSaveFuncInfoWithValue<HttpResponseMessage> ret = await PostStreamAsyncByHandleError(item, url, ClientHttp, headers, default(CancellationToken), useAdminLogin);
			if (!ret.HasError)
			{
				string text = await ret.value.Content.ReadAsStringAsync();
				bool flag = true;
				try
				{
					if (JObject.Parse(text)["data"] == null)
					{
						flag = false;
					}
				}
				catch
				{
					flag = false;
				}
				if (flag)
				{
					result.value = text.FromJson<WebResponse<T>>(sendErrorIfException: true, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 302);
				}
				else
				{
					result.value.Data = text.FromJson<T>(sendErrorIfException: true, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 304);
				}
				result.value.HttpClietnStatusCode = ret.value.StatusCode;
			}
			else
			{
				int num = (int)(ret.value?.StatusCode ?? HttpStatusCode.NotAcceptable);
				if ((num == 502 || num == 503 || num == 504 || num == 511) && tryCount > 0)
				{
					await Task.Delay(2000);
				}
				result.AddReturnedValue(ret, ReturnedState.None, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 314);
			}
			if (ret.value == null)
			{
				ret.AddError(EnSystem.None, "ret.value is null!!", "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 318);
			}
		}
		catch (OperationCanceledException exception)
		{
			result.AddReturnedValue(exception, "", "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 320);
		}
		catch (TimeoutException exception2)
		{
			result.AddReturnedValue(exception2, "URL: " + url, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 323);
			if (tryCount > 0)
			{
				await Task.Delay(2000);
				Dictionary<string, string> headers2;
				headers = (headers2 = null);
				bool strongDeserialize2;
				strongDeserialize = (strongDeserialize2 = false);
				int timeOutMilliSeconds2;
				timeOutMilliSeconds = (timeOutMilliSeconds2 = 15000);
				short num2 = checked((short)(tryCount - 1));
				tryCount = num2;
				return await PostToApi<T, U>(item, url, headers2, strongDeserialize2, timeOutMilliSeconds2, num2);
			}
		}
		catch (HttpRequestException ex)
		{
			WebErrorLog.Instance.Warning(ex, "Type HttpRequestException=>Inner Exception=>" + ex?.InnerException?.Message + " \r\n URL: " + url, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 332);
			result.AddReturnedValue(ex, "", "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 333);
			await Task.Delay(2000);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "URL: " + url, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 338);
			result.AddReturnedValue(ex2, "", "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 339);
		}
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<T>>> PostToApiByAdminToken<T, U>(U item, string url, string depUser, string depPass, ENSource source = ENSource.None, Dictionary<string, string> headers = null, bool strongDeserialize = false, int timeOutMilliSeconds = 15000, short tryCount = 3) where U : new()
	{
		ReturnedSaveFuncInfoWithValue<WebResponse<T>> result = new ReturnedSaveFuncInfoWithValue<WebResponse<T>>
		{
			value = new WebResponse<T>()
		};
		try
		{
			headers = FillHeader(headers, useAdminLogin: true);
			using HttpClient ClientHttp = new HttpClient();
			ClientHttp.Timeout = new TimeSpan(0, 0, 0, 0, timeOutMilliSeconds);
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new RestTokenManagement(source, depUser, depPass).GetAdminToken(default(CancellationToken));
			if (headers == null)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				object obj = returnedSaveFuncInfoWithValue.value ?? "";
				if (obj == null)
				{
					obj = "";
				}
				dictionary.Add("Authorization", (string)obj);
				headers = dictionary;
			}
			else
			{
				Dictionary<string, string> dictionary2 = headers;
				object obj2 = returnedSaveFuncInfoWithValue.value ?? "";
				if (obj2 == null)
				{
					obj2 = "";
				}
				dictionary2.Add("Authorization", (string)obj2);
			}
			ReturnedSaveFuncInfoWithValue<HttpResponseMessage> ret = await PostStreamAsyncByHandleError(item, url, ClientHttp, headers);
			if (!ret.HasError)
			{
				string data = await ret.value.Content.ReadAsStringAsync();
				T val = data.FromJson<T>(sendErrorIfException: false, "PostToApiByAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 363);
				if (val == null)
				{
					result.value = data.FromJson<WebResponse<T>>(sendErrorIfException: true, "PostToApiByAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 365);
				}
				else
				{
					result.value.Data = val;
				}
				result.value.HttpClietnStatusCode = ret.value.StatusCode;
			}
			else
			{
				int value = (int)(ret.value?.StatusCode).Value;
				if ((value == 502 || value == 503 || value == 504 || value == 511) && tryCount > 0)
				{
					await Task.Delay(2000);
				}
				result.AddReturnedValue(ret, ReturnedState.None, "PostToApiByAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 380);
			}
			if (ret.value == null)
			{
				ret.AddError(EnSystem.None, "ret.value is null!!", "PostToApiByAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 383);
			}
		}
		catch (OperationCanceledException exception)
		{
			result.AddReturnedValue(exception, "", "PostToApiByAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 385);
		}
		catch (TimeoutException exception2)
		{
			result.AddReturnedValue(exception2, "URL: " + url, "PostToApiByAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 388);
			if (tryCount > 0)
			{
				await Task.Delay(2000);
				Dictionary<string, string> headers2;
				headers = (headers2 = null);
				bool strongDeserialize2;
				strongDeserialize = (strongDeserialize2 = false);
				int timeOutMilliSeconds2;
				timeOutMilliSeconds = (timeOutMilliSeconds2 = 15000);
				short num = checked((short)(tryCount - 1));
				tryCount = num;
				return await PostToApi<T, U>(item, url, headers2, strongDeserialize2, timeOutMilliSeconds2, num);
			}
		}
		catch (HttpRequestException ex)
		{
			WebErrorLog.Instance.Warning(ex, "Type HttpRequestException=>Inner Exception=>" + ex?.InnerException?.Message + " \r\n URL: " + url, "PostToApiByAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 397);
			result.AddReturnedValue(ex, "", "PostToApiByAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 398);
			await Task.Delay(2000);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "URL: " + url, "PostToApiByAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 403);
			result.AddReturnedValue(ex2, "", "PostToApiByAdminToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 404);
		}
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<T>>> PostToApi<T, U>(U item, string url, string depUser, string depPass, ENSource source = ENSource.None, Dictionary<string, string> headers = null, bool strongDeserialize = false, int timeOutMilliSeconds = 15000, short tryCount = 3) where U : new()
	{
		ReturnedSaveFuncInfoWithValue<WebResponse<T>> result = new ReturnedSaveFuncInfoWithValue<WebResponse<T>>
		{
			value = new WebResponse<T>()
		};
		try
		{
			headers = FillHeader(headers, useAdminLogin: true);
			using HttpClient ClientHttp = new HttpClient();
			ClientHttp.Timeout = new TimeSpan(0, 0, 0, 0, timeOutMilliSeconds);
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new RestTokenManagement(source, depUser, depPass).GetToken(default(CancellationToken));
			if (headers == null)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				object obj = returnedSaveFuncInfoWithValue.value ?? "";
				if (obj == null)
				{
					obj = "";
				}
				dictionary.Add("Authorization", (string)obj);
				headers = dictionary;
			}
			else
			{
				Dictionary<string, string> dictionary2 = headers;
				object obj2 = returnedSaveFuncInfoWithValue.value ?? "";
				if (obj2 == null)
				{
					obj2 = "";
				}
				dictionary2.Add("Authorization", (string)obj2);
			}
			ReturnedSaveFuncInfoWithValue<HttpResponseMessage> ret = await PostStreamAsyncByHandleError(item, url, ClientHttp, headers);
			if (!ret.HasError)
			{
				string data = await ret.value.Content.ReadAsStringAsync();
				T val = data.FromJson<T>(sendErrorIfException: false, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 428);
				if (val == null)
				{
					result.value = data.FromJson<WebResponse<T>>(sendErrorIfException: true, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 430);
				}
				else
				{
					result.value.Data = val;
				}
				result.value.HttpClietnStatusCode = ret.value.StatusCode;
			}
			else
			{
				int value = (int)(ret.value?.StatusCode).Value;
				if ((value == 502 || value == 503 || value == 504 || value == 511) && tryCount > 0)
				{
					await Task.Delay(2000);
				}
				result.AddReturnedValue(ret, ReturnedState.None, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 442);
			}
			if (ret.value == null)
			{
				ret.AddError(EnSystem.None, "ret.value is null!!", "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 446);
			}
		}
		catch (OperationCanceledException exception)
		{
			result.AddReturnedValue(exception, "", "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 448);
		}
		catch (TimeoutException exception2)
		{
			result.AddReturnedValue(exception2, "URL: " + url, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 451);
			if (tryCount > 0)
			{
				await Task.Delay(2000);
				Dictionary<string, string> headers2;
				headers = (headers2 = null);
				bool strongDeserialize2;
				strongDeserialize = (strongDeserialize2 = false);
				int timeOutMilliSeconds2;
				timeOutMilliSeconds = (timeOutMilliSeconds2 = 15000);
				short num = checked((short)(tryCount - 1));
				tryCount = num;
				return await PostToApi<T, U>(item, url, headers2, strongDeserialize2, timeOutMilliSeconds2, num);
			}
		}
		catch (HttpRequestException ex)
		{
			WebErrorLog.Instance.Warning(ex, "Type HttpRequestException=>Inner Exception=>" + ex?.InnerException?.Message + " \r\n URL: " + url, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 460);
			result.AddReturnedValue(ex, "", "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 461);
			await Task.Delay(2000);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "URL: " + url, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 466);
			result.AddReturnedValue(ex2, "", "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 467);
		}
		return result;
	}

	public static async Task<ReturnedSaveFuncInfo> PostToApi<U>(U item, string url, string depUser, string depPass, ENSource source, Dictionary<string, string> headers = null, bool strongDeserialize = false, int timeOutMilliSeconds = 15000, short tryCount = 3) where U : new()
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			headers = FillHeader(headers, useAdminLogin: true);
			using HttpClient ClientHttp = new HttpClient();
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new RestTokenManagement(source, depUser, depPass).GetToken(default(CancellationToken));
			if (headers == null)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				object obj = returnedSaveFuncInfoWithValue.value ?? "";
				if (obj == null)
				{
					obj = "";
				}
				dictionary.Add("Authorization", (string)obj);
				headers = dictionary;
			}
			else
			{
				Dictionary<string, string> dictionary2 = headers;
				object obj2 = returnedSaveFuncInfoWithValue.value ?? "";
				if (obj2 == null)
				{
					obj2 = "";
				}
				dictionary2.Add("Authorization", (string)obj2);
			}
			ClientHttp.Timeout = new TimeSpan(0, 0, 0, 0, timeOutMilliSeconds);
			ret.AddReturnedValue(await PostStreamAsyncByHandleError(item, url, ClientHttp, headers), ReturnedState.None, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 485);
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 487);
		}
		catch (TimeoutException exception2)
		{
			ret.AddReturnedValue(exception2, "URL: " + url, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 490);
			if (tryCount > 0)
			{
				await Task.Delay(2000);
				Dictionary<string, string> headers2;
				headers = (headers2 = null);
				bool strongDeserialize2;
				strongDeserialize = (strongDeserialize2 = false);
				int timeOutMilliSeconds2;
				timeOutMilliSeconds = (timeOutMilliSeconds2 = 15000);
				short num = checked((short)(tryCount - 1));
				tryCount = num;
				return await PostToApi(item, url, headers2, strongDeserialize2, timeOutMilliSeconds2, num);
			}
		}
		catch (HttpRequestException ex)
		{
			WebErrorLog.Instance.Warning(ex, "Type HttpRequestException=>Inner Exception=>" + ex?.InnerException?.Message + " \r\n URL: " + url, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 499);
			ret.AddReturnedValue(ex, "URL: " + url, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 500);
			await Task.Delay(2000);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 505);
			ret.AddReturnedValue(ex2, "", "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 506);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> PostToApi<U>(U item, string url, Dictionary<string, string> headers = null, bool strongDeserialize = false, int timeOutMilliSeconds = 15000, short tryCount = 3) where U : new()
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			headers = FillHeader(headers, useAdminLogin: true);
			using HttpClient ClientHttp = new HttpClient();
			ClientHttp.Timeout = new TimeSpan(0, 0, 0, 0, timeOutMilliSeconds);
			ret.AddReturnedValue(await PostStreamAsyncByHandleError(item, url, ClientHttp, headers), ReturnedState.None, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 519);
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 521);
		}
		catch (TimeoutException exception2)
		{
			ret.AddReturnedValue(exception2, "URL: " + url, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 524);
			if (tryCount > 0)
			{
				await Task.Delay(2000);
				Dictionary<string, string> headers2;
				headers = (headers2 = null);
				bool strongDeserialize2;
				strongDeserialize = (strongDeserialize2 = false);
				int timeOutMilliSeconds2;
				timeOutMilliSeconds = (timeOutMilliSeconds2 = 15000);
				short num = checked((short)(tryCount - 1));
				tryCount = num;
				return await PostToApi(item, url, headers2, strongDeserialize2, timeOutMilliSeconds2, num);
			}
		}
		catch (HttpRequestException ex)
		{
			WebErrorLog.Instance.Warning(ex, "Type HttpRequestException=>Inner Exception=>" + ex?.InnerException?.Message + " \r\n URL: " + url, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 533);
			ret.AddReturnedValue(ex, "URL: " + url, "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 534);
			await Task.Delay(2000);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 539);
			ret.AddReturnedValue(ex2, "", "PostToApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 540);
		}
		return ret;
	}

	public static Task<ReturnedSaveFuncInfoWithValue<WebResponse<T>>> GetFromApi<T>(this string url, LoginInfo loginInfo, CancellationToken token, short tryCount = 3, Dictionary<string, string> headers = null, int timeOutMilliSeconds = 15000)
	{
		return url.GetFromApi<T>(loginInfo?.UserName, loginInfo?.Password, loginInfo?.Source ?? ENSource.None, token, tryCount, headers, timeOutMilliSeconds);
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<T>>> GetFromApi<T>(this string url, string depUserName, string depPassword, ENSource source, CancellationToken token, short tryCount = 3, Dictionary<string, string> headers = null, int timeOutMilliSeconds = 15000)
	{
		int line = 1;
		ReturnedSaveFuncInfoWithValue<WebResponse<T>> result = new ReturnedSaveFuncInfoWithValue<WebResponse<T>>
		{
			value = new WebResponse<T>()
		};
		_ = "#url =>" + url + "\r\n#depUserName =>" + depUserName + "\r\n#depPassword =>" + depPassword + "\r\n" + $"source =>{source}\r\n" + $"tryCount =>{tryCount}\r\n" + "headers =>" + Json.ToStringJson(headers);
		try
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new RestTokenManagement(source, depUserName, depPassword).GetAdminToken(token);
			line = 1;
			if (result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 561).HasError)
			{
				return result;
			}
			line = 2;
			if (headers == null)
			{
				line = 3;
				headers = new Dictionary<string, string> { 
				{
					"Authorization",
					returnedSaveFuncInfoWithValue.value ?? ""
				} };
			}
			else
			{
				line = 4;
				headers.Add("Authorization", returnedSaveFuncInfoWithValue.value ?? "");
			}
			line = 5;
			headers.Add("UserName", depUserName ?? "");
			line = 6;
			headers.Add("Password", depPassword ?? "");
			line = 7;
			using HttpClient clientHttp = new HttpClient();
			line = 8;
			clientHttp.Timeout = new TimeSpan(0, 0, 0, 0, timeOutMilliSeconds);
			line = 9;
			ReturnedSaveFuncInfoWithValue<HttpResponseMessage> ret = await GetStreamAsync(url, clientHttp, headers);
			line = 10;
			if (!ret.HasError)
			{
				line = 11;
				string data = await ret.value.Content.ReadAsStringAsync();
				line = 12;
				T val = data.FromJson<T>(sendErrorIfException: false, "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 591);
				line = 13;
				if (val == null)
				{
					line = 14;
					result.value = data.FromJson<WebResponse<T>>(sendErrorIfException: true, "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 596);
				}
				else
				{
					line = 15;
					result.value.Data = val;
				}
				line = 16;
				result.value.HttpClietnStatusCode = ret.value.StatusCode;
			}
			else
			{
				line = 17;
				int value = (int)(ret.value?.StatusCode).Value;
				line = 18;
				if (((uint)(value - 502) <= 2u || value == 511) ? true : false)
				{
					line = 19;
					if (tryCount > 0)
					{
						line = 20;
						await Task.Delay(new Random().Next(2000));
						line = 21;
						short num = checked((short)(tryCount - 1));
						tryCount = num;
						return await url.GetFromApi<T>(depUserName, depPassword, source, token, num, headers, timeOutMilliSeconds);
					}
				}
				line = 22;
				result.AddReturnedValue(ret, ReturnedState.None, "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 623);
			}
		}
		catch (OperationCanceledException ex)
		{
			result.AddError($"#OperationCanceledException\r\n#Line_{line}\r\nException:{ex.Message} \r\n URL: {url}", "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 626);
		}
		catch (HttpRequestException ex2)
		{
			result.AddError($"#HttpRequestException \r\n#Line_{line}\r\nException:{ex2.Message} \r\n URL: {url}", "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 627);
		}
		catch (TimeoutException ex3)
		{
			result.AddError($"#TimeoutException \r\n#Line_{line}\r\nException:{ex3.Message} \r\n URL: {url}", "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 630);
			if (tryCount > 0)
			{
				await Task.Delay(2000);
				short num = checked((short)(tryCount - 1));
				tryCount = num;
				return await url.GetFromApi<T>(depUserName, depPassword, source, token, num, headers, timeOutMilliSeconds);
			}
			result.AddReturnedValue(ex3, "", "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 636);
		}
		catch (Exception ex4)
		{
			result.AddError($"#GeneralException\r\n#Line_{line}\r\nException:{ex4.Message} \r\n URL: {url}", "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 638);
		}
		return result;
	}

	private static Dictionary<string, string> FillHeader(Dictionary<string, string> headers, bool useAdminLogin)
	{
		if (headers == null)
		{
			headers = new Dictionary<string, string>();
		}
		if (useAdminLogin)
		{
			if (!headers.TryGetValue("UserName", out var value))
			{
				headers.Add("UserName", DepAdmin.UserName);
			}
			if (!headers.TryGetValue("Password", out value))
			{
				headers.Add("Password", DepAdmin.Password);
			}
		}
		return headers;
	}

	private static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<T>>> FillLoginInfo<T>(LoginInfo loginInfo, Dictionary<string, string> headers, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<WebResponse<T>> result = new ReturnedSaveFuncInfoWithValue<WebResponse<T>>();
		if (loginInfo != null)
		{
			ReturnedSaveFuncInfoWithValue<string> returnedSaveFuncInfoWithValue = await new RestTokenManagement(loginInfo.Source, loginInfo.UserName, loginInfo.Password).GetToken(token);
			if (result.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "FillLoginInfo", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 659).HasError)
			{
				return result;
			}
			if (headers == null)
			{
				headers = new Dictionary<string, string> { 
				{
					"Authorization",
					returnedSaveFuncInfoWithValue.value ?? ""
				} };
			}
			else
			{
				headers.Add("Authorization", returnedSaveFuncInfoWithValue.value ?? "");
			}
		}
		return result;
	}

	[DebuggerHidden]
	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<T>>> GetFromApi<T>(this string url, CancellationToken token, short tryCount = 3, Dictionary<string, string> headers = null, int timeOutMilliSeconds = 15000, bool useAdminLogin = true)
	{
		if (string.IsNullOrEmpty(url))
		{
			throw new ArgumentException("url");
		}
		ReturnedSaveFuncInfoWithValue<WebResponse<T>> result = new ReturnedSaveFuncInfoWithValue<WebResponse<T>>
		{
			value = new WebResponse<T>()
		};
		checked
		{
			try
			{
				using HttpClient clientHttp = ((CacheableHttpClient.Default == null) ? new HttpClient() : CacheableHttpClient.Default);
				clientHttp.Timeout = new TimeSpan(0, 0, 0, 0, timeOutMilliSeconds);
				headers = FillHeader(headers, useAdminLogin);
				ReturnedSaveFuncInfoWithValue<HttpResponseMessage> ret = await GetStreamAsync(url, clientHttp, headers, token, useAdminLogin);
				if (ret.HasError)
				{
					int num = unchecked((int)(ret.value?.StatusCode ?? ((HttpStatusCode)0)));
					if ((num == 502 || num == 503 || num == 504 || num == 511) && tryCount > 0)
					{
						await Task.Delay(2000);
						short num2 = (short)(tryCount - 1);
						tryCount = num2;
						return await url.GetFromApi<T>(token, num2, headers, timeOutMilliSeconds, useAdminLogin);
					}
					result.AddReturnedValue(ret, ReturnedState.None, "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 692);
				}
				else
				{
					T val = (await ret.value.Content.ReadAsStringAsync()).FromJson<T>("GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 697);
					if (val is WebResponse<T> value)
					{
						result.value = value;
					}
					else
					{
						result.value.Data = val;
					}
					result.value.HttpClietnStatusCode = ret.value.StatusCode;
				}
			}
			catch (OperationCanceledException exception)
			{
				result.AddReturnedValue(exception, "", "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 705);
			}
			catch (HttpRequestException exception2)
			{
				result.AddReturnedValue(exception2, "", "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 706);
			}
			catch (TimeoutException exception3)
			{
				if (tryCount > 0)
				{
					await Task.Delay(2000);
					short num2 = (short)(tryCount - 1);
					tryCount = num2;
					return await url.GetFromApi<T>(token, num2, headers, timeOutMilliSeconds + 5000);
				}
				result.AddReturnedValue(exception3, "URL: " + url, "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 714);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "11/url:{@Url}", url, "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 718);
				result.AddReturnedValue(ex, "", "GetFromApi", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 719);
			}
			return result;
		}
	}

	[DebuggerHidden]
	public static async Task<HttpResponseMessage> PostStreamAsync(object content, string url, CancellationToken cancellationToken)
	{
		HttpResponseMessage response = new HttpResponseMessage();
		try
		{
			using (HttpClient client = new HttpClient())
			{
				using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
				using HttpContent httpContent = CreateHttpContent(content);
				request.Content = httpContent;
				response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
				await response.Content.ReadAsStringAsync();
				response.EnsureSuccessStatusCode();
			}
			return response;
		}
		catch
		{
			throw;
		}
	}

	public static async Task<HttpResponseMessage> PostStreamAsync(object content, string url, Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		customHeaders = FillHeader(customHeaders, useAdminLogin: true);
		using HttpClient client = new HttpClient();
		HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
		HttpContent content2 = CreateHttpContent(content);
		httpRequestMessage.Content = content2;
		if (customHeaders != null)
		{
			foreach (KeyValuePair<string, string> customHeader in customHeaders)
			{
				client.DefaultRequestHeaders.Add(customHeader.Key, customHeader.Value);
			}
		}
		HttpResponseMessage obj = await client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
		obj.EnsureSuccessStatusCode();
		return obj;
	}

	public static async Task<HttpResponseMessage> PostStreamAsync(object content, string url, HttpClient client, Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		new HttpResponseMessage();
		try
		{
			HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url);
			HttpContent content2 = CreateHttpContent(content);
			httpRequestMessage.Content = content2;
			customHeaders = FillHeader(customHeaders, useAdminLogin: true);
			if (customHeaders != null)
			{
				foreach (KeyValuePair<string, string> customHeader in customHeaders)
				{
					client.DefaultRequestHeaders.Add(customHeader.Key, customHeader.Value);
				}
			}
			HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
			httpResponseMessage.EnsureSuccessStatusCode();
			return httpResponseMessage;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static async Task<HttpResponseMessage> SendStreamAsync(object content, string url, HttpClient client, HttpMethod method, Dictionary<string, string> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		customHeaders = FillHeader(customHeaders, useAdminLogin: true);
		HttpRequestMessage httpRequestMessage = new HttpRequestMessage(method, url);
		HttpContent content2 = CreateHttpContent(content);
		httpRequestMessage.Content = content2;
		if (customHeaders != null)
		{
			foreach (KeyValuePair<string, string> customHeader in customHeaders)
			{
				client.DefaultRequestHeaders.Add(customHeader.Key, customHeader.Value);
			}
		}
		HttpResponseMessage obj = await client.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
		obj.EnsureSuccessStatusCode();
		return obj;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<T>>> PostToApiAsync<T, U>(U item, string url, LoginInfo loginInfo, Dictionary<string, string> headers, int timeOutMilliSeconds = 15000, short tryCount = 5, CancellationToken token = default(CancellationToken), bool useAdminLogin = true) where U : new()
	{
		ReturnedSaveFuncInfoWithValue<WebResponse<T>> result = new ReturnedSaveFuncInfoWithValue<WebResponse<T>>
		{
			value = new WebResponse<T>()
		};
		try
		{
			headers = FillHeader(headers, useAdminLogin);
			ReturnedSaveFuncInfoWithValue<WebResponse<T>> returnedSaveFuncInfoWithValue = result;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await FillLoginInfo<T>(loginInfo, headers, token), ReturnedState.None, "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 800);
			if (result.HasError)
			{
				return result;
			}
			using HttpClient ClientHttp = new HttpClient();
			ClientHttp.Timeout = new TimeSpan(0, 0, 0, 0, timeOutMilliSeconds);
			ReturnedSaveFuncInfoWithValue<HttpResponseMessage> ret = await PostStreamAsyncByHandleError(item, url, ClientHttp, headers, default(CancellationToken), useAdminLogin);
			if (!ret.HasError)
			{
				string data = await ret.value.Content.ReadAsStringAsync();
				T val = data.FromJson<T>(sendErrorIfException: false, "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 811);
				if (result.value == null)
				{
					result.value = new WebResponse<T>();
				}
				if (val == null)
				{
					result.value = data.FromJson<WebResponse<T>>(sendErrorIfException: true, "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 815);
				}
				else
				{
					result.value.Data = val;
				}
				result.value.HttpClietnStatusCode = ret.value.StatusCode;
			}
			else
			{
				int num = (int)(ret.value?.StatusCode ?? HttpStatusCode.NotAcceptable);
				if ((num == 502 || num == 503 || num == 504 || num == 511) && tryCount > 0)
				{
					await Task.Delay(2000);
				}
				if (ret.value.Content != null)
				{
					string text = await ret.value.Content.ReadAsStringAsync();
					if (!string.IsNullOrEmpty(text))
					{
						result.AddError(text, "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 829);
					}
				}
				result.AddReturnedValue(ret, ReturnedState.None, "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 831);
			}
			if (ret.value == null)
			{
				ret.AddError(EnSystem.None, "ret.value is null!!", "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 835);
			}
		}
		catch (OperationCanceledException exception)
		{
			result.AddReturnedValue(exception, "", "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 837);
		}
		catch (TimeoutException exception2)
		{
			result.AddReturnedValue(exception2, "URL: " + url, "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 840);
			if (tryCount > 0)
			{
				await Task.Delay(2000);
				Dictionary<string, string> headers2 = headers;
				short num2 = checked((short)(tryCount - 1));
				tryCount = num2;
				return await PostToApiAsync<T, U>(item, url, loginInfo, headers2, timeOutMilliSeconds, num2, token, useAdminLogin);
			}
		}
		catch (HttpRequestException ex)
		{
			WebErrorLog.Instance.Warning(ex, "Type HttpRequestException=>Inner Exception=>" + ex?.InnerException?.Message + " \r\n URL: " + url, "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 849);
			result.AddReturnedValue(ex, "", "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 850);
			await Task.Delay(2000);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "URL: " + url, "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 855);
			result.AddReturnedValue(ex2, "", "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 856);
		}
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<T>>> PostToApiStringAsync<T>(string item, string url, LoginInfo loginInfo, Dictionary<string, string> headers, int timeOutMilliSeconds = 15000, short tryCount = 5, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<WebResponse<T>> result = new ReturnedSaveFuncInfoWithValue<WebResponse<T>>
		{
			value = new WebResponse<T>()
		};
		try
		{
			headers = FillHeader(headers, useAdminLogin: true);
			ReturnedSaveFuncInfoWithValue<WebResponse<T>> returnedSaveFuncInfoWithValue = result;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await FillLoginInfo<T>(loginInfo, headers, token), ReturnedState.None, "PostToApiStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 866);
			if (result.HasError)
			{
				return result;
			}
			using HttpClient ClientHttp = new HttpClient();
			ClientHttp.Timeout = new TimeSpan(0, 0, 0, 0, timeOutMilliSeconds);
			ReturnedSaveFuncInfoWithValue<HttpResponseMessage> ret = await PostStreamAsyncByHandleError(item, url, ClientHttp, headers, token);
			if (!ret.HasError)
			{
				string data = await ret.value.Content.ReadAsStringAsync();
				T val = data.FromJson<T>(sendErrorIfException: false, "PostToApiStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 877);
				if (val == null)
				{
					result.value = data.FromJson<WebResponse<T>>(sendErrorIfException: true, "PostToApiStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 879);
				}
				else
				{
					result.value.Data = val;
				}
				if (result.value != null)
				{
					result.value.HttpClietnStatusCode = ret.value.StatusCode;
				}
			}
			else
			{
				int num = (int)(ret.value?.StatusCode ?? HttpStatusCode.NotAcceptable);
				if ((num == 502 || num == 503 || num == 504 || num == 511) && tryCount > 0)
				{
					await Task.Delay(2000);
				}
				if (ret.value.Content != null)
				{
					string text = await ret.value.Content.ReadAsStringAsync();
					if (!string.IsNullOrEmpty(text))
					{
						result.AddError(text, "PostToApiStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 893);
					}
				}
				result.AddReturnedValue(ret, ReturnedState.None, "PostToApiStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 895);
			}
			if (ret.value == null)
			{
				ret.AddError(EnSystem.None, "ret.value is null!!", "PostToApiStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 899);
			}
		}
		catch (OperationCanceledException exception)
		{
			result.AddReturnedValue(exception, "", "PostToApiStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 901);
		}
		catch (TimeoutException exception2)
		{
			result.AddReturnedValue(exception2, "URL: " + url, "PostToApiStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 904);
			if (tryCount > 0)
			{
				await Task.Delay(2000);
				Dictionary<string, string> headers2 = headers;
				short num2 = checked((short)(tryCount - 1));
				tryCount = num2;
				return await PostToApiStringAsync<T>(item, url, loginInfo, headers2, timeOutMilliSeconds, num2, token);
			}
		}
		catch (HttpRequestException ex)
		{
			WebErrorLog.Instance.Warning(ex, "Type HttpRequestException=>Inner Exception=>" + ex?.InnerException?.Message + " \r\n URL: " + url, "PostToApiStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 913);
			result.AddReturnedValue(ex, "", "PostToApiStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 914);
			await Task.Delay(2000);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "URL: " + url, "PostToApiStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 919);
			result.AddReturnedValue(ex2, "", "PostToApiStringAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 920);
		}
		return result;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<WebResponse<T>>> PostToApiBytesAsync<T>(byte[] item, string url, LoginInfo loginInfo, Dictionary<string, string> headers, int timeOutMilliSeconds = 15000, short tryCount = 5, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfoWithValue<WebResponse<T>> result = new ReturnedSaveFuncInfoWithValue<WebResponse<T>>
		{
			value = new WebResponse<T>()
		};
		try
		{
			headers = FillHeader(headers, useAdminLogin: true);
			ReturnedSaveFuncInfoWithValue<WebResponse<T>> returnedSaveFuncInfoWithValue = result;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await FillLoginInfo<T>(loginInfo, headers, token), ReturnedState.None, "PostToApiBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 930);
			if (result.HasError)
			{
				return result;
			}
			using HttpClient ClientHttp = new HttpClient();
			ClientHttp.Timeout = new TimeSpan(0, 0, 0, 0, timeOutMilliSeconds);
			ReturnedSaveFuncInfoWithValue<HttpResponseMessage> ret = await PostStreamAsyncByHandleError(item, url, ClientHttp, headers, token);
			if (!ret.HasError)
			{
				string data = await ret.value.Content.ReadAsStringAsync();
				T val = data.FromJson<T>(sendErrorIfException: false, "PostToApiBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 940);
				if (val == null)
				{
					result.value = data.FromJson<WebResponse<T>>(sendErrorIfException: true, "PostToApiBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 942);
				}
				else
				{
					result.value.Data = val;
				}
				if (result.value != null)
				{
					result.value.HttpClietnStatusCode = ret.value.StatusCode;
				}
			}
			else
			{
				int num = (int)(ret.value?.StatusCode ?? HttpStatusCode.NotAcceptable);
				if ((num == 502 || num == 503 || num == 504 || num == 511) && tryCount > 0)
				{
					await Task.Delay(2000);
				}
				if (ret.value.Content != null)
				{
					string text = await ret.value.Content.ReadAsStringAsync();
					if (!string.IsNullOrEmpty(text))
					{
						result.AddError(text, "PostToApiBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 956);
					}
				}
				result.AddReturnedValue(ret, ReturnedState.None, "PostToApiBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 958);
			}
			if (ret.value == null)
			{
				ret.AddError(EnSystem.None, "ret.value is null!!", "PostToApiBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 962);
			}
		}
		catch (OperationCanceledException exception)
		{
			result.AddReturnedValue(exception, "", "PostToApiBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 964);
		}
		catch (TimeoutException exception2)
		{
			result.AddReturnedValue(exception2, "URL: " + url, "PostToApiBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 967);
			if (tryCount > 0)
			{
				await Task.Delay(2000);
				Dictionary<string, string> headers2 = headers;
				short num2 = checked((short)(tryCount - 1));
				tryCount = num2;
				return await PostToApiBytesAsync<T>(item, url, loginInfo, headers2, timeOutMilliSeconds, num2, token);
			}
		}
		catch (HttpRequestException ex)
		{
			WebErrorLog.Instance.Warning(ex, "Type HttpRequestException=>Inner Exception=>" + ex?.InnerException?.Message + " \r\n URL: " + url, "PostToApiBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 976);
			result.AddReturnedValue(ex, "", "PostToApiBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 977);
			await Task.Delay(2000);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "URL: " + url, "PostToApiBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 982);
			result.AddReturnedValue(ex2, "", "PostToApiBytesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Performance\\HTTPHelpers.cs", 983);
		}
		return result;
	}
}
