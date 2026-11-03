using System;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities.Message.Validators;

internal class WebMessageDetailValidator
{
	internal ReturnedSaveFuncInfo ValidateModel(WebMessageDetail model, bool isServer)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(model.Content))
			{
				returnedSaveFuncInfo.AddError("مقدار کپشن نباید خالی باشد", "", "ValidateModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\Validators\\WebMessageDetailValidator.cs", 15);
			}
			if (isServer && model.MessageID <= 0)
			{
				returnedSaveFuncInfo.AddError("شناسه دپارتمان باید پر بشه", "", "ValidateModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\Validators\\WebMessageDetailValidator.cs", 17);
			}
			if (model.ContentType == enWebMesssageDetailType.None)
			{
				returnedSaveFuncInfo.AddError("نوع پیام برای ارسال را مشخص نمایید", "", "ValidateModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\Validators\\WebMessageDetailValidator.cs", 19);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ValidateModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\Validators\\WebMessageDetailValidator.cs", 21);
			returnedSaveFuncInfo.AddError(ex.Message, "", "ValidateModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\Validators\\WebMessageDetailValidator.cs", 21);
		}
		return returnedSaveFuncInfo;
	}
}
