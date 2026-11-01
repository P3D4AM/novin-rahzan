using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Message;
using PacketParser.Payamak;
using Serilog;
using WebSmsStandard.RestEntities;

namespace WebSmsStandard;

public static class SmsSender
{
	public static async Task<long[]> SendSmsAsync(string Username, string Pass, string Domain, string message, string numbers, string SenderNumber, CancellationToken inputToken)
	{
		try
		{
			CancellationToken token = CancellationTokenSource.CreateLinkedTokenSource(inputToken, new CancellationTokenSource(60000).Token).Token;
			List<WebMessage> messages = new List<WebMessage>
			{
				new WebMessage
				{
					Caption = message,
					MessageRecivers = new List<WebMessageReciver>
					{
						new WebMessageReciver
						{
							Reciver = numbers
						}
					}
				}
			};
			return await SendSmsAsync(Username, Pass, Domain, messages, SenderNumber, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "SendSmsAsync// string messages,string number// messages:" + message + "//number:" + numbers, "SendSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\WebSmsStandard\\SMSSender.cs", 36);
			if (ex.Message.Contains("The request failed with HTTP status 417"))
			{
				return new long[1] { -417L };
			}
			return new long[1] { -80L };
		}
	}

	public static async Task<long[]> SendSmsAsync(string Username, string Pass, string Domain, List<string> messages, List<string> phoneNumbers, string SenderNumber, CancellationToken inputToken, int tryCount = 10)
	{
		int line = 0;
		messages = messages ?? throw new ArgumentNullException("messages");
		if ((messages?.Count ?? 0) == 0)
		{
			throw new ArgumentException("The list is empty", "messages");
		}
		phoneNumbers = phoneNumbers ?? throw new ArgumentNullException("phoneNumbers");
		if ((phoneNumbers?.Count ?? 0) == 0)
		{
			throw new ArgumentException("The list is empty", "phoneNumbers");
		}
		if ((messages?.Count ?? 0) != (phoneNumbers?.Count ?? 0))
		{
			throw new ArgumentException("Phonenumbers count and messages count must be equal");
		}
		try
		{
			CancellationToken token = CancellationTokenSource.CreateLinkedTokenSource(inputToken, new CancellationTokenSource(60000).Token).Token;
			line = 10000;
			if (string.IsNullOrEmpty(Username))
			{
				throw new ArgumentException("Username");
			}
			Username = Username;
			line = 10001;
			if (string.IsNullOrEmpty(Pass))
			{
				throw new ArgumentException("Pass");
			}
			Pass = Pass;
			line = 10002;
			if (string.IsNullOrEmpty(Domain))
			{
				throw new ArgumentException("Domain");
			}
			Domain = Domain;
			line = 10003;
			if (string.IsNullOrEmpty(SenderNumber))
			{
				throw new ArgumentException("SenderNumber");
			}
			SenderNumber = SenderNumber;
			line = 100;
			foreach (string message in messages)
			{
				line = 101;
				if (string.IsNullOrEmpty(message))
				{
					line = 102;
					throw new NullReferenceException($"message is null/Messages Count=>{messages?.Count ?? 0}");
				}
			}
			line = 2;
			foreach (string phoneNumber in phoneNumbers)
			{
				if (string.IsNullOrEmpty(phoneNumber))
				{
					throw new NullReferenceException();
				}
			}
			line = 3;
			Domain = WebSmsSender.FixDomain(Domain);
			line = 5;
			return await SendSmsAsync_(Username, Pass, Domain, messages, phoneNumbers, SenderNumber, token, tryCount);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "method1\r\n" + $"Line:{line}\r\n" + "UserName:" + Username + "\r\nPass:" + Pass + "\r\nSenderNumber:" + SenderNumber + "\r\nDomain:" + Domain + "\r\nMobiles:" + string.Join(",", phoneNumbers) + "\r\rMessages:" + string.Join("<->", messages), "SendSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\WebSmsStandard\\SMSSender.cs", 94);
			return new long[1] { -120L };
		}
	}

	public static async Task<long[]> SendSmsAsync(string Username, string Pass, string Domain, List<WebMessage> messages, string SenderNumber, CancellationToken inputToken)
	{
		CancellationToken token = CancellationTokenSource.CreateLinkedTokenSource(inputToken, new CancellationTokenSource(60000).Token).Token;
		messages = messages ?? throw new ArgumentNullException("messages");
		if (string.IsNullOrEmpty(Username))
		{
			throw new ArgumentException("Username");
		}
		Username = Username;
		if (string.IsNullOrEmpty(Pass))
		{
			throw new ArgumentException("Pass");
		}
		Pass = Pass;
		if (string.IsNullOrEmpty(Domain))
		{
			throw new ArgumentException("Domain");
		}
		Domain = Domain;
		if (string.IsNullOrEmpty(SenderNumber))
		{
			throw new ArgumentException("SenderNumber");
		}
		SenderNumber = SenderNumber;
		int line = 0;
		try
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			if (messages != null)
			{
				foreach (WebMessage message in messages)
				{
					if (message == null)
					{
						continue;
					}
					foreach (WebMessageReciver messageReciver in message.MessageRecivers)
					{
						line = 1;
						list.Add(message.Caption);
						line = 2;
						list2.Add(messageReciver.Reciver.ToString());
						line = 3;
					}
				}
			}
			line = 4;
			Domain = WebSmsSender.FixDomain(Domain);
			line = 5;
			return await SendSmsAsync(Username, Pass, Domain, list, list2, SenderNumber, token);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line{line}// SendSmsAsync//List<WebMessage> messages//{Json.ToStringJson((object)messages, false)}", "SendSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\WebSmsStandard\\SMSSender.cs", 155);
			return new long[1] { -90L };
		}
	}

	private static async Task<long[]> SendSmsAsync_(string Username, string Pass, string Domain, List<string> listMSG, List<string> listNum, string SenderNumber, CancellationToken inputToken, int tryCount = 10)
	{
		CancellationToken token = CancellationTokenSource.CreateLinkedTokenSource(inputToken, new CancellationTokenSource(60000).Token).Token;
		JaegerMonitor monitor = new JaegerMonitor("SendSmsAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\WebSmsStandard\\SMSSender.cs", 60000);
		try
		{
			SmsModelV2 smsModelV = new SmsModelV2
			{
				UserName = Username,
				Password = Pass,
				Domain = Domain,
				SenderNumber = SenderNumber,
				Messages = listMSG,
				Numbers = listNum
			};
			int line2 = 8;
			HttpResponseMessage response;
			try
			{
				response = await HttpHelpers.PostStreamAsync((object)smsModelV, "http://webservice.novinp.ir/sms/Npsms/SendSmsV2/", token);
			}
			catch (OperationCanceledException)
			{
				if (tryCount > 0 && !inputToken.IsCancellationRequested)
				{
					await Task.Delay(new Random().Next(30000));
					if (!inputToken.IsCancellationRequested)
					{
						CancellationToken inputToken2 = inputToken;
						int num = tryCount - 1;
						tryCount = num;
						return await SendSmsAsync_(Username, Pass, Domain, listMSG, listNum, SenderNumber, inputToken2, num);
					}
				}
				return new long[1] { -119L };
			}
			catch (InvalidOperationException ex2)
			{
				if (tryCount > 0 && !inputToken.IsCancellationRequested)
				{
					await Task.Delay(new Random().Next(30000));
					if (!inputToken.IsCancellationRequested)
					{
						CancellationToken inputToken3 = inputToken;
						int num = tryCount - 1;
						tryCount = num;
						return await SendSmsAsync_(Username, Pass, Domain, listMSG, listNum, SenderNumber, inputToken3, num);
					}
				}
				WebErrorLog.Instance.StartLog((Exception)ex2, "method2//EndPoint:http://webservice.novinp.ir/sms/Npsms/SendSmsV2/", "SendSmsAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\WebSmsStandard\\SMSSender.cs", 223);
				return new long[1] { -112L };
			}
			catch (HttpRequestException ex3)
			{
				if (tryCount > 0 && !inputToken.IsCancellationRequested)
				{
					await Task.Delay(new Random().Next(30000));
					if (!inputToken.IsCancellationRequested)
					{
						CancellationToken inputToken4 = inputToken;
						int num = tryCount - 1;
						tryCount = num;
						return await SendSmsAsync_(Username, Pass, Domain, listMSG, listNum, SenderNumber, inputToken4, num);
					}
				}
				WebErrorLog.Instance.StartLog((Exception)ex3, "method2//EndPoint:http://webservice.novinp.ir/sms/Npsms/SendSmsV2/", "SendSmsAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\WebSmsStandard\\SMSSender.cs", 241);
				return new long[1] { -110L };
			}
			catch (Exception ex)
			{
				if (tryCount > 0 && !inputToken.IsCancellationRequested)
				{
					await Task.Delay(new Random().Next(30000));
					if (!inputToken.IsCancellationRequested)
					{
						CancellationToken inputToken5 = inputToken;
						int num = tryCount - 1;
						tryCount = num;
						return await SendSmsAsync_(Username, Pass, Domain, listMSG, listNum, SenderNumber, inputToken5, num);
					}
				}
				WebErrorLog.Instance.StartLog(ex, $"method2//Error while sending sms to API. sendSms_2 line:{line2}", "SendSmsAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\WebSmsStandard\\SMSSender.cs", 259);
				return new long[1] { -113L };
			}
			if (response != null && response.IsSuccessStatusCode)
			{
				line2 = 110;
				try
				{
					ActiveSendJsonResponse activeSendJsonResponse = await HttpHelpers.ReadJsonResponseAsync<ActiveSendJsonResponse>(response);
					line2 = 111;
					if (activeSendJsonResponse != null && activeSendJsonResponse.Result != null)
					{
						line2 = 112;
						return activeSendJsonResponse.Result.ToArray();
					}
					line2 = 113;
					return new long[1] { -123L };
				}
				catch (OperationCanceledException)
				{
					return new long[1] { -122L };
				}
				catch (Exception ex6)
				{
					string text = "method2//Error while parsing sms response payload.\r\nThe response model has probably changed.\r\n➡\ufe0fsendSms_2\r\n" + $"line:{line2}";
					if (ex6.Message.Contains("Stream was not readable"))
					{
						Log.Warning(ex6, text);
					}
					else
					{
						WebErrorLog.Instance.StartLog(ex6, text, "SendSmsAsync_", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\WebSmsStandard\\SMSSender.cs", 288);
					}
					return new long[1] { -121L };
				}
			}
			return new long[1] { -120L };
		}
		finally
		{
			((IDisposable)monitor)?.Dispose();
		}
	}

	public static Task<ReturnedSaveFuncInfoWithValue<decimal>> GetCreditAsync(string Username, string Pass, string Domain, CancellationToken inputToken)
	{
		return WebSmsSender.GetCreditAsync(Username, Pass, Domain, inputToken);
	}

	public static async Task<int[]> GetDeliveryAsync(string Username, string Pass, string Domain, IEnumerable<long> smsId, string lineNumber, CancellationToken inputToken)
	{
		JaegerMonitor monitor = new JaegerMonitor("GetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\WebSmsStandard\\SMSSender.cs", 60000);
		try
		{
			int line = 0;
			try
			{
				CancellationToken token = CancellationTokenSource.CreateLinkedTokenSource(inputToken, new CancellationTokenSource(60000).Token).Token;
				line = 1;
				if (smsId != null)
				{
					if (smsId == null || smsId.Count() != 0)
					{
						line = 2;
						Domain = WebSmsSender.FixDomain(Domain);
						line = 3;
						using HttpClient client = new HttpClient();
						line = 4;
						FormUrlEncodedContent content = new FormUrlEncodedContent(new KeyValuePair<string, string>[5]
						{
							new KeyValuePair<string, string>("UserName", Username),
							new KeyValuePair<string, string>("Password", Pass),
							new KeyValuePair<string, string>("Domain", Domain),
							new KeyValuePair<string, string>("SmsId", string.Join(",", smsId)),
							new KeyValuePair<string, string>("LineNumber", lineNumber)
						});
						line = 10;
						HttpResponseMessage res = await client.PostAsync("http://webservice.novinp.ir/sms/Npsms/GetDelivery/", content, token);
						line = 11;
						if (res.IsSuccessStatusCode)
						{
							line = 12;
							string obj = await res.Content.ReadAsStringAsync();
							line = 13;
							NpsmsGetDeliveryResponse npsmsGetDeliveryResponse = Json.FromJson<NpsmsGetDeliveryResponse>(obj, false, "GetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\WebSmsStandard\\SMSSender.cs", 330);
							if (npsmsGetDeliveryResponse == null)
							{
								return null;
							}
							line = 14;
							if ((int)npsmsGetDeliveryResponse.Message == 1)
							{
								return npsmsGetDeliveryResponse.Result.ToArray();
							}
							line = 15;
						}
						line = 200;
						throw new Exception($"res.IsSuccessStatusCode is false!//Wrong response status code//UserName:{Username}/Pass:{Pass}/domain:{Domain}/smsId:{smsId}/ResCode:{res?.StatusCode}");
					}
				}
				return null;
			}
			catch (OperationCanceledException)
			{
				return null;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "GetDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\WebSmsStandard\\SMSSender.cs", 348);
				return null;
			}
		}
		finally
		{
			((IDisposable)monitor)?.Dispose();
		}
	}
}
