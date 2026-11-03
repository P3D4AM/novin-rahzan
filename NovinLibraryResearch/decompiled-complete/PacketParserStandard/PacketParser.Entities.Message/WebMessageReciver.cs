using System;
using System.Linq;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Message.Validators;
using PacketParser.Exceptions;

namespace PacketParser.Entities.Message;

public class WebMessageReciver
{
	public long ID { get; set; }

	public long MessageID { get; set; }

	public string Reciver { get; set; }

	public EnMessageSendType Type { get; set; }

	public DateTime? Modified { get; set; }

	public static WebMessageReciver Factory(string reciver, EnMessageSendType type)
	{
		WebMessageReciver webMessageReciver = new WebMessageReciver();
		try
		{
			webMessageReciver.Reciver = reciver;
			webMessageReciver.Type = type;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new WebMessageReciverValidator().ValidateModel(webMessageReciver, isServer: false);
			if (returnedSaveFuncInfo.HasError)
			{
				NotValidExeption exception = new NotValidExeption("خطاهای زیر در رخ داده است \r\n");
				returnedSaveFuncInfo.ErrorList.ToList().ForEach(delegate(string error)
				{
					exception.ValidationErrors.Add(error);
				});
				throw exception;
			}
			return webMessageReciver;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Factory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageReciver.cs", 36);
			throw;
		}
	}

	public static WebMessageReciver Factory(string reciver, EnMessageSendType type, long messageID)
	{
		WebMessageReciver webMessageReciver = new WebMessageReciver();
		try
		{
			webMessageReciver.Reciver = reciver;
			webMessageReciver.Type = type;
			webMessageReciver.MessageID = messageID;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new WebMessageReciverValidator().ValidateModel(webMessageReciver, isServer: true);
			if (returnedSaveFuncInfo.HasError)
			{
				NotValidExeption exception = new NotValidExeption("خطاهای زیر در رخ داده است \r\n");
				returnedSaveFuncInfo.ErrorList.ToList().ForEach(delegate(string error)
				{
					exception.ValidationErrors.Add(error);
				});
				throw exception;
			}
			return webMessageReciver;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Factory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageReciver.cs", 59);
			throw;
		}
	}
}
