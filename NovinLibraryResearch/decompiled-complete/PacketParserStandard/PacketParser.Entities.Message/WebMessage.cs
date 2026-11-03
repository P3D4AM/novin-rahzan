using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.Compression;
using PacketParser.Entities.Message.Validators;
using PacketParser.Exceptions;

namespace PacketParser.Entities.Message;

public class WebMessage
{
	private static string apiUrl = HttpHelpers.ApiUrl;

	public long ID { get; set; }

	[JsonIgnore]
	public short DepID { get; set; }

	public long Sender { get; set; }

	public DateTime? InsertTime { get; set; }

	public DateTime? SendTime { get; set; }

	public DateTime? ScadualedTime { get; set; }

	public enMessageSender Status { get; set; }

	public string Caption { get; set; }

	public short TryCount { get; set; }

	public ENPlatFormType PlatformType { get; set; }

	public DateTime Modified { get; set; }

	public List<WebMetaData> MetaData { get; set; }

	public List<WebFileInfo> FilesForUpload { get; set; }

	public List<WebMessageDetail> MessageDetails { get; set; }

	public List<WebMessageReciver> MessageRecivers { get; set; }

	public static WebMessage Factory(DateTime? scadualedTime, string caption, short tryCount, ENPlatFormType platformType)
	{
		WebMessage webMessage = new WebMessage();
		try
		{
			DateTime valueOrDefault = scadualedTime.GetValueOrDefault();
			DateTime value;
			if (!scadualedTime.HasValue)
			{
				valueOrDefault = DateTime.Now;
				scadualedTime = valueOrDefault;
				value = valueOrDefault;
			}
			else
			{
				value = valueOrDefault;
			}
			webMessage.ScadualedTime = value;
			webMessage.Caption = caption;
			webMessage.TryCount = tryCount;
			webMessage.PlatformType = platformType;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new WebMessageValidator().ValidateModel(webMessage, isServer: false);
			if (returnedSaveFuncInfo.HasError)
			{
				NotValidExeption notValidExeption = new NotValidExeption("خطا های زیر پیش آمده است: \r\n");
				notValidExeption.ValidationErrors.AddRange(returnedSaveFuncInfo.ErrorList.ToList());
				throw notValidExeption;
			}
			return webMessage;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Factory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessage.cs", 57);
			throw;
		}
	}

	public static WebMessage Factory(DateTime? scadualedTime, string caption, short tryCount, short depID, ENPlatFormType platformType, WebMessageReciver reciver)
	{
		WebMessage webMessage = new WebMessage();
		try
		{
			webMessage.ScadualedTime = scadualedTime;
			webMessage.Caption = caption;
			webMessage.TryCount = tryCount;
			webMessage.DepID = depID;
			webMessage.PlatformType = platformType;
			webMessage.MessageRecivers = new List<WebMessageReciver>(1) { reciver };
			webMessage.InsertTime = DateTime.Now;
			webMessage.ScadualedTime = DateTime.Now;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new WebMessageValidator().ValidateModel(webMessage, isServer: true);
			if (returnedSaveFuncInfo.HasError)
			{
				NotValidExeption notValidExeption = new NotValidExeption("خطا های زیر پیش آمده است: \r\n");
				notValidExeption.ValidationErrors.AddRange(returnedSaveFuncInfo.ErrorList.ToList());
				throw notValidExeption;
			}
			return webMessage;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Factory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessage.cs", 86);
			throw;
		}
	}

	public static WebMessage Factory(DateTime? scadualedTime, string caption, short tryCount, short depID, ENPlatFormType platformType, string contact, List<string> messages)
	{
		WebMessage webMessage = new WebMessage();
		try
		{
			webMessage.ScadualedTime = scadualedTime;
			webMessage.Caption = caption;
			webMessage.TryCount = tryCount;
			webMessage.DepID = depID;
			webMessage.PlatformType = platformType;
			webMessage.InsertTime = DateTime.Now;
			webMessage.ScadualedTime = DateTime.Now;
			WebMessageReciver item = WebMessageReciver.Factory(contact, EnMessageSendType.TelegramDirect);
			webMessage.MessageDetails = new List<WebMessageDetail>();
			messages.ForEach(delegate(string message)
			{
				WebMessageDetail item2 = WebMessageDetail.Factory(message, enWebMesssageDetailType.Message);
				webMessage.MessageDetails.Add(item2);
			});
			webMessage.MessageRecivers = new List<WebMessageReciver>();
			webMessage.MessageRecivers.Add(item);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new WebMessageValidator().ValidateModel(webMessage, isServer: true);
			if (returnedSaveFuncInfo.HasError)
			{
				NotValidExeption notValidExeption = new NotValidExeption("خطا های زیر پیش آمده است: \r\n");
				notValidExeption.ValidationErrors.AddRange(returnedSaveFuncInfo.ErrorList.ToList());
				throw notValidExeption;
			}
			return webMessage;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Factory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessage.cs", 122);
			throw;
		}
	}

	public static WebMessage Factory(DateTime? scadualedTime, string caption, short tryCount, short depID, ENPlatFormType platformType, List<WebMessageDetail> details, List<WebMessageReciver> recivers)
	{
		WebMessage webMessage = new WebMessage();
		try
		{
			webMessage.ScadualedTime = scadualedTime;
			webMessage.Caption = caption;
			webMessage.TryCount = tryCount;
			webMessage.DepID = depID;
			webMessage.PlatformType = platformType;
			webMessage.MessageRecivers = recivers;
			webMessage.MessageDetails = details;
			webMessage.InsertTime = DateTime.Now;
			webMessage.ScadualedTime = DateTime.Now;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new WebMessageValidator().ValidateModel(webMessage, isServer: true);
			if (returnedSaveFuncInfo.HasError)
			{
				NotValidExeption notValidExeption = new NotValidExeption("خطا های زیر پیش آمده است: \r\n");
				notValidExeption.ValidationErrors.AddRange(returnedSaveFuncInfo.ErrorList.ToList());
				throw notValidExeption;
			}
			return webMessage;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Factory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessage.cs", 151);
			throw;
		}
	}

	public static WebMessage Factory(DateTime? scadualedTime, string caption, short tryCount, short depID, ENPlatFormType platformType, WebMessageDetail detail, WebMessageReciver reciver)
	{
		WebMessage webMessage = new WebMessage();
		try
		{
			webMessage.ScadualedTime = scadualedTime;
			webMessage.Caption = caption;
			webMessage.TryCount = tryCount;
			webMessage.DepID = depID;
			webMessage.PlatformType = platformType;
			webMessage.MessageDetails.Add(detail);
			webMessage.MessageRecivers.Add(reciver);
			webMessage.InsertTime = DateTime.Now;
			webMessage.ScadualedTime = DateTime.Now;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new WebMessageValidator().ValidateModel(webMessage, isServer: true);
			if (returnedSaveFuncInfo.HasError)
			{
				NotValidExeption notValidExeption = new NotValidExeption("خطا های زیر پیش آمده است: \r\n");
				notValidExeption.ValidationErrors.AddRange(returnedSaveFuncInfo.ErrorList.ToList());
				throw notValidExeption;
			}
			return webMessage;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Factory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessage.cs", 180);
			throw;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> PostToApiAsync(ENSource source, WebMessage message, string userName, string password, CancellationToken token = default(CancellationToken), string apiAddress = null)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (message == null)
			{
				return res.AddError("message", "", "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessage.cs", 187);
			}
			res.AddReturnedValue(await HttpHelpers.PostToApiBytesAsync<ReturnedSaveFuncInfo>(await GZip.Compress(Json.ToBytesJson(message)), loginInfo: LoginInfo.CreateLoginInfo(userName, password, source), url: (apiAddress ?? apiUrl) + "Message/SendAsync", headers: null, timeOutMilliSeconds: 15000, tryCount: 5, token: token), ReturnedState.None, "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessage.cs", 195);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessage.cs", 199);
			res.AddReturnedValue(ex, "", "PostToApiAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessage.cs", 200);
		}
		return res;
	}
}
