using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer;

public class ReturnedSaveFuncInfoWithValue<T> : ReturnedSaveFuncInfo
{
	public T value { get; set; }

	public new ReturnedSaveFuncInfoWithValue<T> AddErrorTokenIsCancelled(string message = "token is Cancelled!", string key = "CancellationToken", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		if (string.IsNullOrEmpty(message))
		{
			throw new ArgumentNullException("message");
		}
		try
		{
			AddError(EnSystem.UserInterface, $"{message}//{memberName}//{sourceFilePath}//{sourceLineNumber}", key, memberName, sourceFilePath, sourceLineNumber);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddErrorTokenIsCancelled", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 47);
		}
		return this;
	}

	public new ReturnedSaveFuncInfoWithValue<T> AddReturnedValue(Exception exception, string descriptions = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		base.AddReturnedValue(exception, descriptions, memberName, sourceFilePath, sourceLineNumber);
		return this;
	}

	public ReturnedSaveFuncInfoWithValue<T> AddReturnedValue(WebResponseModel<T> model, string descriptions = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		if ((model?.Result?.Any()).GetValueOrDefault())
		{
			foreach (WebResponseModelDetail item in model.Result)
			{
				if (item != null)
				{
					switch (item.State)
					{
					case ReturnedState.Information:
						AddInformation(item.Message, "", "AddReturnedValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 73);
						break;
					case ReturnedState.Error:
						AddError(item.Message, "AddReturnedValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 74);
						break;
					case ReturnedState.Warning:
						AddWarning(item.Message, "", "AddReturnedValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 75);
						break;
					default:
						throw new NotImplementedException();
					}
				}
			}
		}
		return this;
	}

	public new ReturnedSaveFuncInfoWithValue<T> AddInformation(string message, string key = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		try
		{
			AddReturnedValue(ReturnedState.Information, message, key, memberName, sourceFilePath, sourceLineNumber);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddInformation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 94);
		}
		return this;
	}

	public ReturnedSaveFuncInfoWithValue<T> AddError(EnSystem system, string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		if (string.IsNullOrEmpty(message))
		{
			throw new ArgumentNullException("message");
		}
		try
		{
			AddError(system, message, "message", memberName, sourceFilePath, sourceLineNumber);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddError", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 124);
		}
		return this;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<T>> AddErrorAsync(EnSystem system, string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		if (string.IsNullOrEmpty(message))
		{
			WebErrorLog.Instance.StartLog("Message is Empty", "", "AddErrorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 149);
			return this;
		}
		try
		{
			AddError(system, message.GetTranslate(), "message", memberName, sourceFilePath, sourceLineNumber);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddErrorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 158);
		}
		return this;
	}

	public ReturnedSaveFuncInfoWithValue<T> AddError(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		return AddError(EnSystem.UserInterface, message, memberName, sourceFilePath, sourceLineNumber);
	}

	public new ReturnedSaveFuncInfoWithValue<T> AddReturnedValue(ReturnedSaveFuncInfo returnedSaveFuncInfo, ReturnedState ignoreStates = ReturnedState.None, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		AddReturnedValue(returnedSaveFuncInfo, changeValue: true, ignoreStates, memberName, sourceFilePath, sourceLineNumber);
		return this;
	}

	public ReturnedSaveFuncInfoWithValue<T> AddReturnedValue(ReturnedSaveFuncInfo returnedSaveFuncInfo, bool changeValue, ReturnedState ignoreStates = ReturnedState.None, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		try
		{
			if (returnedSaveFuncInfo == null)
			{
				throw new ArgumentNullException("returnedSaveFuncInfo");
			}
			if (returnedSaveFuncInfo is ReturnedSaveFuncInfoWithValue<T> && changeValue)
			{
				value = ((ReturnedSaveFuncInfoWithValue<T>)returnedSaveFuncInfo).value;
			}
			base.AddReturnedValue(returnedSaveFuncInfo, ignoreStates, memberName, sourceFilePath, sourceLineNumber);
			return this;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddReturnedValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 198);
			return this;
		}
	}

	public WebResponseModel<T> GetWebResponseModel()
	{
		WebResponseModel<T> webResponseModel = new WebResponseModel<T>
		{
			Result = new List<WebResponseModelDetail>(),
			Value = value
		};
		IReadOnlyList<string> errorList = base.ErrorList;
		if (errorList != null && errorList.Any())
		{
			foreach (string error in base.ErrorList)
			{
				webResponseModel.Result.Add(new WebResponseModelDetail
				{
					Message = error,
					State = ReturnedState.Error
				});
			}
		}
		IReadOnlyList<string> warningList = base.WarningList;
		if (warningList != null && warningList.Any())
		{
			foreach (string warning in base.WarningList)
			{
				webResponseModel.Result.Add(new WebResponseModelDetail
				{
					Message = warning,
					State = ReturnedState.Warning
				});
			}
		}
		IReadOnlyList<string> informationList = base.InformationList;
		if (informationList != null && informationList.Any())
		{
			foreach (string information in base.InformationList)
			{
				webResponseModel.Result.Add(new WebResponseModelDetail
				{
					Message = information,
					State = ReturnedState.Information
				});
			}
		}
		return webResponseModel;
	}
}
