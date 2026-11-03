using System;
using System.Linq;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Message.Validators;
using PacketParser.Exceptions;

namespace PacketParser.Entities.Message;

public class WebMessageDetail
{
	public long ID { get; set; }

	public string Content { get; set; }

	public enWebMesssageDetailType ContentType { get; set; } = enWebMesssageDetailType.None;


	public long MessageID { get; set; }

	public DateTime Modified { get; set; }

	public static WebMessageDetail Factory(string content, enWebMesssageDetailType contentType)
	{
		WebMessageDetail webMessageDetail = new WebMessageDetail();
		try
		{
			webMessageDetail.Content = content;
			webMessageDetail.ContentType = contentType;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new WebMessageDetailValidator().ValidateModel(webMessageDetail, isServer: false);
			if (returnedSaveFuncInfo.HasError)
			{
				NotValidExeption exception = new NotValidExeption("خطاهای زیر در رخ داده است \r\n");
				returnedSaveFuncInfo.ErrorList.ToList().ForEach(delegate(string error)
				{
					exception.ValidationErrors.Add(error);
				});
				throw exception;
			}
			return webMessageDetail;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Factory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageDetail.cs", 38);
			throw;
		}
	}

	public static WebMessageDetail Factory(string content, enWebMesssageDetailType contentType, long messageID)
	{
		WebMessageDetail webMessageDetail = new WebMessageDetail();
		try
		{
			webMessageDetail.Content = content;
			webMessageDetail.ContentType = contentType;
			webMessageDetail.MessageID = messageID;
			webMessageDetail.Modified = DateTime.Now;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new WebMessageDetailValidator().ValidateModel(webMessageDetail, isServer: true);
			if (returnedSaveFuncInfo.HasError)
			{
				NotValidExeption exception = new NotValidExeption("خطاهای زیر در رخ داده است \r\n");
				returnedSaveFuncInfo.ErrorList.ToList().ForEach(delegate(string error)
				{
					exception.ValidationErrors.Add(error);
				});
				throw exception;
			}
			return webMessageDetail;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Factory", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\WebMessageDetail.cs", 62);
			throw;
		}
	}
}
