using System;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities.Message.Validators;

internal class WebMessageValidator
{
	internal ReturnedSaveFuncInfo ValidateModel(WebMessage message, bool isServer)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(message.Caption))
			{
				returnedSaveFuncInfo.AddError("مقدار کپشن نباید خالی باشد", "", "ValidateModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\Validators\\WebMessageValidator.cs", 14);
			}
			if (isServer)
			{
				if (message.DepID <= 0)
				{
					returnedSaveFuncInfo.AddError("شناسه دپارتمان باید پر بشه", "", "ValidateModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\Validators\\WebMessageValidator.cs", 17);
				}
				if (message.TryCount <= 0)
				{
					returnedSaveFuncInfo.AddError("مقدار trycount نباید از یک کمتر باشد", "", "ValidateModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\Validators\\WebMessageValidator.cs", 18);
				}
				if (message.InsertTime.Value.Subtract(DateTime.Now).Days < 0)
				{
					returnedSaveFuncInfo.AddError("تاریخ ذخیره سازی نمی تواند کوچکتر از تاریخ امروز باشد", "", "ValidateModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\Validators\\WebMessageValidator.cs", 19);
				}
			}
			if (message.ScadualedTime.Value.Subtract(DateTime.Now).Days < 0)
			{
				returnedSaveFuncInfo.AddError("تاریخ زمان بندی شده ارسال پیام نمی تواند کوچکتر از تاریخ امروز باشد", "", "ValidateModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\Validators\\WebMessageValidator.cs", 21);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ValidateModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\Validators\\WebMessageValidator.cs", 23);
			returnedSaveFuncInfo.AddError(ex.Message, "", "ValidateModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Message\\Validators\\WebMessageValidator.cs", 23);
		}
		return returnedSaveFuncInfo;
	}
}
