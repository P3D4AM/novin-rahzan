using System;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities.Message.Validators;

internal class WebMessageReciverValidator
{
	internal ReturnedSaveFuncInfo ValidateModel(WebMessageReciver model, bool isServer)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(model.Reciver))
			{
				returnedSaveFuncInfo.AddError("گیرنده پیام مشخص نشده است", "", "ValidateModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\Validators\\WebMessageReciverValidator .cs", 14);
			}
			if (model.Type == EnMessageSendType.None)
			{
				returnedSaveFuncInfo.AddError("نوع گیرنده پیام باید مشخص باشد", "", "ValidateModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\Validators\\WebMessageReciverValidator .cs", 15);
			}
			if (isServer && model.MessageID <= 0)
			{
				returnedSaveFuncInfo.AddError("شناسه پیام باید مشخص شود", "", "ValidateModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\Validators\\WebMessageReciverValidator .cs", 17);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ValidateModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\Validators\\WebMessageReciverValidator .cs", 19);
			returnedSaveFuncInfo.AddError(ex.Message, "", "ValidateModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\Validators\\WebMessageReciverValidator .cs", 19);
		}
		return returnedSaveFuncInfo;
	}
}
