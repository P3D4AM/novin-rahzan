using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer.Interfaces;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer;

public class ReturnedSaveFuncInfo
{
	private const short stringMaxLenLimitation = 500;

	private const int maxCount = 10000;

	private IReturnedSaveFuncInfo UiUpdater;

	private SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

	[JsonIgnore]
	public IReadOnlyList<string> InformationList => Informations?.ToList()?.Select((Tuple<string, string> p) => p.Item2)?.ToList();

	public Guid Guid { get; set; } = Guid.NewGuid();


	[JsonIgnore]
	public IReadOnlyList<string> ErrorList => Errors?.ToList()?.Select((Tuple<string, string> p) => p.Item2)?.ToList();

	[JsonIgnore]
	public IReadOnlyList<string> WarningList => Warnings?.ToList()?.Select((Tuple<string, string> p) => p.Item2)?.ToList();

	public ConcurrentBag<Tuple<string, string>> Informations { get; protected set; } = new ConcurrentBag<Tuple<string, string>>();


	public ConcurrentBag<Tuple<string, string>> Errors { get; protected set; } = new ConcurrentBag<Tuple<string, string>>();


	public ConcurrentBag<Tuple<string, string>> Warnings { get; protected set; } = new ConcurrentBag<Tuple<string, string>>();


	public bool HasError => ErrorList?.Any() ?? false;

	public bool ShowAllMessage
	{
		get
		{
			if (!HasError && !HasWarning)
			{
				return HasInformation;
			}
			return true;
		}
	}

	public bool HasWarning => (WarningMessage?.Length ?? 0) > 0;

	public string ErrorMessage => GetErrorMessage();

	public string InformationMessage => GetInformationMessage();

	public string WarningMessage
	{
		get
		{
			try
			{
				return (WarningList.Count > 0) ? string.Join(Environment.NewLine, WarningList) : "";
			}
			catch (ThreadAbortException)
			{
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "WarningMessage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 306);
			}
			return null;
		}
	}

	public bool HasInformation => InformationList?.Any() ?? false;

	public int Count
	{
		get
		{
			ConcurrentBag<Tuple<string, string>> informations = Informations;
			if (informations == null)
			{
				int? num = WarningList?.Count;
				int? num2 = num;
				if (!num2.HasValue)
				{
					num = ErrorList?.Count;
					int? num3 = num;
					return num3.GetValueOrDefault();
				}
				return num2.GetValueOrDefault();
			}
			return informations.Count;
		}
	}

	[JsonIgnore]
	[XmlIgnore]
	public bool HasTokenCancelle
	{
		get
		{
			try
			{
				if (ErrorList == null || !ErrorList.Any())
				{
					return false;
				}
				return ErrorList.Where((string q) => !q.Contains("token is Cancelled")).Any();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "HasTokenCancelle", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 326);
				return false;
			}
		}
	}

	public string Messages => GetErrorMessage() + "\r\n" + WarningMessage + "\r\n" + GetInformationMessage() + "\r\n";

	public ReturnedSaveFuncInfo(IReturnedSaveFuncInfo uiUpdater = null)
	{
		UiUpdater = uiUpdater;
	}

	public ReturnedSaveFuncInfo()
	{
	}

	public ReturnedSaveFuncInfo(Exception exception, string description = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		try
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(description))
			{
				stringBuilder.Append("Descriptin: " + description.GetTranslate() + " ");
			}
			if (sourceLineNumber != 0)
			{
				stringBuilder.Append($"SourceLineNumber: {sourceLineNumber} ");
			}
			if (!string.IsNullOrEmpty(memberName))
			{
				stringBuilder.Append("memberName: " + memberName + " ");
			}
			if (!string.IsNullOrEmpty(sourceFilePath))
			{
				stringBuilder.Append("sourceFilePath: " + sourceFilePath + " ");
			}
			Errors.Add(new Tuple<string, string>("", exception.Message + "  " + stringBuilder));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 271);
		}
	}

	public ReturnedSaveFuncInfo(ReturnedState information, string description, string key = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		AddReturnedValue(information, description, key, memberName, sourceFilePath, sourceLineNumber);
	}

	public string GetErrorMessage()
	{
		try
		{
			return (ErrorList.Count > 0) ? string.Join(Environment.NewLine, ErrorList) : "";
		}
		catch (ThreadAbortException)
		{
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetErrorMessage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 340);
		}
		return null;
	}

	public Task<string> GetErrorMessageAsync()
	{
		try
		{
			return Task.FromResult((ErrorList.Count > 0) ? string.Join(Environment.NewLine, ErrorList.GetTranslate()) : "");
		}
		catch (ThreadAbortException)
		{
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetErrorMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 352);
		}
		return Task.FromResult<string>(null);
	}

	public Task<string> GetWarningMessageAsync()
	{
		try
		{
			return Task.FromResult((WarningList.Count > 0) ? string.Join(Environment.NewLine, WarningList.GetTranslate()) : "");
		}
		catch (ThreadAbortException)
		{
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetWarningMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 364);
		}
		return Task.FromResult<string>(null);
	}

	public Task<string> GetInformationMessageAsync()
	{
		try
		{
			return Task.FromResult(Informations.Any() ? string.Join(Environment.NewLine, InformationList.GetTranslate()) : "");
		}
		catch (ThreadAbortException)
		{
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetInformationMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 376);
		}
		return Task.FromResult<string>(null);
	}

	public string GetInformationMessage()
	{
		try
		{
			return Informations.Any() ? string.Join(Environment.NewLine, InformationList) : "";
		}
		catch (ThreadAbortException)
		{
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex3)
		{
			WebErrorLog.Instance.StartLog(ex3, "", "GetInformationMessage", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 387);
		}
		return null;
	}

	public ReturnedSaveFuncInfo AddErrorTokenIsCancelled(string message = "", string key = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		return AddError(EnSystem.UserInterface, $"Token has been cancelled! {message}//{memberName}//{sourceFilePath}//{sourceLineNumber}", key, memberName, sourceFilePath, sourceLineNumber);
	}

	public ReturnedSaveFuncInfo AddError(EnSystem system, string message, string key = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0, StringBuilder sb = null)
	{
		try
		{
			AddReturnedValue(ReturnedState.Error, message, key, memberName, sourceFilePath, sourceLineNumber);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddError", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 447);
		}
		return this;
	}

	public Task<ReturnedSaveFuncInfo> AddErrorAsync(string message, string key = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0, StringBuilder sb = null)
	{
		return AddErrorAsync(EnSystem.None, message, key, memberName, sourceFilePath, sourceLineNumber, sb);
	}

	public async Task<ReturnedSaveFuncInfo> AddErrorAsync(EnSystem system, string message, string key = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0, StringBuilder sb = null)
	{
		try
		{
			await AddReturnedValueAsync(ReturnedState.Error, message.GetTranslate(), key, memberName, sourceFilePath, sourceLineNumber);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddErrorAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 471);
		}
		return this;
	}

	public ReturnedSaveFuncInfo AddError(string message, string key = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0, StringBuilder sb = null)
	{
		return AddError(EnSystem.UserInterface, message, key, memberName, sourceFilePath, sourceLineNumber, sb);
	}

	public async Task<ReturnedSaveFuncInfo> AddWarningAsync(string message, string key = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		try
		{
			await AddReturnedValueAsync(ReturnedState.Warning, message.GetTranslate(), key, memberName, sourceFilePath, sourceLineNumber);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddWarningAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 499);
		}
		return this;
	}

	public ReturnedSaveFuncInfo AddWarning(string message, string key = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		try
		{
			AddReturnedValue(ReturnedState.Warning, message, key, memberName, sourceFilePath, sourceLineNumber);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddWarning", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 518);
		}
		return this;
	}

	public virtual ReturnedSaveFuncInfo AddInformation(string message, string key = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		try
		{
			AddReturnedValue(ReturnedState.Information, message, key, memberName, sourceFilePath, sourceLineNumber);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddInformation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 537);
		}
		return this;
	}

	public virtual async Task<ReturnedSaveFuncInfo> AddInformationAsync(string message, string key = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		try
		{
			await AddReturnedValueAsync(ReturnedState.Information, message.GetTranslate(), key, memberName, sourceFilePath, sourceLineNumber);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddInformationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 556);
		}
		return this;
	}

	public void AddReturnedValue(CancellationToken token, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		try
		{
			if (token.IsCancellationRequested)
			{
				AddReturnedValue(ReturnedState.Error, "Operation Has Been Cancelled!!!", "CancellationToken", memberName, sourceFilePath, sourceLineNumber);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddReturnedValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 576);
		}
	}

	public void AddReturnedHttpStatusCode(HttpStatusCode Response, Exception exception = null, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		try
		{
			string text = $"کد خطا:{Response}({checked((short)Response)})\r\n";
			switch (Response)
			{
			case HttpStatusCode.RequestTimeout:
				text += "کاربر گرامی مدت زمان انتظار برای دریافت پاسخ بیش از حد طولانی شده است\r\n لطفا کمی بعد دوباره تلاش نمایید";
				break;
			case HttpStatusCode.MethodNotAllowed:
				text += "سمت سرور پاسخ دهنده ای یافت نشد";
				break;
			case HttpStatusCode.NotFound:
				text += "متاسفانه آدرسی که برای آن درخواست ارسال شده است وجود ندارد";
				break;
			case HttpStatusCode.Unauthorized:
				text += "خطای احراز هویت \r\n متاسفانه شما مجاز به استفاده از این سرویس نمی باشید";
				break;
			case HttpStatusCode.ServiceUnavailable:
				text += "سرور در حال بروزر سانی است یا برای لحظاتی در دسترس نمی باشد لطفا کمی بعد مجددا سعی نمایید ";
				break;
			case HttpStatusCode.InternalServerError:
				text += "خطایی در سمت سرور رخ داده است لطفا کمی بعد مجددا سعی کنید ";
				break;
			case HttpStatusCode.BadRequest:
				text += "اطلاعات ارسالی شما نامعتبر می باشد\r\n لطفا مقادیر ارسال شده را بررسی مجدد نمایید";
				break;
			case HttpStatusCode.Found:
				text += "متاسفانه نمی توان درخواست شما را ثبت کرد زیرا اطلاعاتی مشابه اطلاعات شما در سرور موجود می باشد";
				break;
			case HttpStatusCode.Created:
				text += "اطلاعات ارسالی شما با موفقیت ثبت گردید";
				break;
			case HttpStatusCode.Accepted:
				text += "اطلاعات ارسالی شما با موفقیت ویرایش گردید";
				break;
			case HttpStatusCode.NotModified:
				text += "عملیات ویرایش اطلاعات با شکست مواجه گشت";
				break;
			case HttpStatusCode.NoContent:
				text += "داده ای در سمت سرور یافت نشد";
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				text += "\r\n";
			}
			string text2 = exception?.Message;
			for (Exception ex = exception?.InnerException; ex != null; ex = ex.InnerException)
			{
				text2 = text2 + "\r\n" + ex.Message;
			}
			AddError(EnSystem.None, text, "Response", memberName, sourceFilePath, sourceLineNumber);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddReturnedHttpStatusCode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 622);
			AddError(EnSystem.None, ex2.Message, "Response", memberName, sourceFilePath, sourceLineNumber);
		}
	}

	public void AddReturnedValue(ReturnedState returnedState, string description, string key = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		if (Count > 10000)
		{
			WebErrorLog.Instance.StartLog($"List overflow currentCount({Count})/memberName:{memberName}/sourceFilePath:{sourceFilePath}/sourceLineNumber:{sourceLineNumber}", "", "AddReturnedValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 638);
			return;
		}
		try
		{
			description = description?.LimitLength(500);
			switch (returnedState)
			{
			case ReturnedState.Information:
				Informations.Add(new Tuple<string, string>(key, description));
				UiUpdater?.AddInformationAsync(description, key);
				break;
			case ReturnedState.Error:
				Errors.Add(new Tuple<string, string>(key, description));
				UiUpdater?.AddErrorAsync(description, key);
				break;
			case ReturnedState.Warning:
				Warnings.Add(new Tuple<string, string>(key, description));
				UiUpdater?.AddWarningAsync(description, key);
				break;
			case ReturnedState.Success:
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddReturnedValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 663);
		}
	}

	public async Task AddReturnedValueAsync(ReturnedState returnedState, string description, string key = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		bool semaphoreHasBeenTaken = false;
		if (Count > 10000)
		{
			WebErrorLog.Instance.StartLog($"List overflow currentCount({Count})/memberName:{memberName}/sourceFilePath:{sourceFilePath}/sourceLineNumber:{sourceLineNumber}", "", "AddReturnedValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 678);
			return;
		}
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			description = description?.LimitLength(500);
			switch (returnedState)
			{
			case ReturnedState.Information:
				Informations.Add(new Tuple<string, string>(key, description.GetTranslate()));
				UiUpdater?.AddInformationAsync(description.GetTranslate(), key);
				break;
			case ReturnedState.Error:
				Errors.Add(new Tuple<string, string>(key, description.GetTranslate()));
				UiUpdater?.AddErrorAsync(description.GetTranslate(), key);
				break;
			case ReturnedState.Warning:
				Warnings.Add(new Tuple<string, string>(key, description.GetTranslate()));
				UiUpdater?.AddWarningAsync(description.GetTranslate(), key);
				break;
			case ReturnedState.Success:
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddReturnedValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 705);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
	}

	public void ResetInformationList()
	{
		while (true)
		{
			ConcurrentBag<Tuple<string, string>> informations = Informations;
			if (informations != null && !informations.IsEmpty)
			{
				Informations.TryTake(out var _);
				continue;
			}
			break;
		}
	}

	public void ResetErrorList()
	{
		while (true)
		{
			ConcurrentBag<Tuple<string, string>> errors = Errors;
			if (errors != null && !errors.IsEmpty)
			{
				Errors.TryTake(out var _);
				continue;
			}
			break;
		}
	}

	public void ResetWarningList()
	{
		while (true)
		{
			ConcurrentBag<Tuple<string, string>> warnings = Warnings;
			if (warnings != null && !warnings.IsEmpty)
			{
				Warnings.TryTake(out var _);
				continue;
			}
			break;
		}
	}

	public virtual ReturnedSaveFuncInfo AddReturnedValue(Exception exception, string descriptions = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		if (Count > 10000)
		{
			WebErrorLog.Instance.StartLog($"List overflow currentCount({Count})/memberName:{memberName}/sourceFilePath:{sourceFilePath}/sourceLineNumber:{sourceLineNumber}", "", "AddReturnedValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 739);
			return this;
		}
		try
		{
			if (exception == null)
			{
				return this;
			}
			string text = exception.Message;
			Exception ex = exception;
			while (ex.InnerException != null)
			{
				ex = ex.InnerException;
				text = text + "\r\nInnerException:" + ex.Message;
			}
			text = text?.LimitLength(500);
			FileInfo fileInfo = new FileInfo(sourceFilePath);
			Errors.Add(new Tuple<string, string>("Exception", $"{fileInfo.Name} \r\n {memberName} \r\n Line: {sourceLineNumber}\r\n UserDesc: {descriptions}\r\n Message: {text}"));
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddReturnedValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 760);
		}
		return this;
	}

	public virtual async Task<ReturnedSaveFuncInfo> AddReturnedValueAsync(Exception exception, string descriptions = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		bool semaphoreHasBeenTaken = false;
		if (Count > 10000)
		{
			WebErrorLog.Instance.StartLog($"List overflow currentCount({Count})/memberName:{memberName}/sourceFilePath:{sourceFilePath}/sourceLineNumber:{sourceLineNumber}", "", "AddReturnedValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 776);
			return this;
		}
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			if (exception == null)
			{
				return this;
			}
			string text = exception.Message;
			Exception ex = exception;
			while (ex.InnerException != null)
			{
				ex = ex.InnerException;
				text = text + "\r\nInnerException:" + ex.Message;
			}
			text = text?.LimitLength(500);
			FileInfo fileInfo = new FileInfo(sourceFilePath);
			Errors.Add(new Tuple<string, string>("Exception", $"{fileInfo.Name} \r\n {memberName} \r\n Line: {sourceLineNumber}\r\n UserDesc: {descriptions}\r\n Message: {text}"));
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddReturnedValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 797);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
		return this;
	}

	public override string ToString()
	{
		return Messages;
	}

	public virtual ReturnedSaveFuncInfo AddReturnedValue(ReturnedSaveFuncInfo returnedSaveFuncInfo, ReturnedState ignoreStates = ReturnedState.None, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		if (returnedSaveFuncInfo == null)
		{
			WebErrorLog.Instance.StartLog("#ReturnedSaveFuncInfo_is_Null_returnedSaveFuncInfo", "", "AddReturnedValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 818);
			return this;
		}
		if (checked(Count + returnedSaveFuncInfo.Count) > 10000)
		{
			WebErrorLog.Instance.StartLog($"List overflow currentCount({Count})/memberName:{memberName}/sourceFilePath:{sourceFilePath}/sourceLineNumber:{sourceLineNumber}", "", "AddReturnedValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 824);
			return this;
		}
		try
		{
			if ((ignoreStates & ReturnedState.Error) != ReturnedState.Error && returnedSaveFuncInfo != null && returnedSaveFuncInfo.ErrorList?.Count > 0)
			{
				foreach (Tuple<string, string> error in returnedSaveFuncInfo.Errors)
				{
					if (error == null)
					{
						throw new NullReferenceException();
					}
					Errors.Add(error);
				}
			}
			if ((ignoreStates & ReturnedState.Warning) != ReturnedState.Warning && returnedSaveFuncInfo != null && returnedSaveFuncInfo.WarningList?.Count > 0)
			{
				foreach (Tuple<string, string> warning in returnedSaveFuncInfo.Warnings)
				{
					if (warning == null)
					{
						throw new NullReferenceException();
					}
					Warnings.Add(warning);
				}
			}
			if ((ignoreStates & ReturnedState.Information) != ReturnedState.Information && returnedSaveFuncInfo != null && returnedSaveFuncInfo.InformationList?.Count > 0)
			{
				foreach (Tuple<string, string> item in returnedSaveFuncInfo.Informations.ToList())
				{
					if (item == null)
					{
						throw new NullReferenceException();
					}
					Informations.Add(item);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddReturnedValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 882);
		}
		return returnedSaveFuncInfo;
	}

	public virtual async Task<ReturnedSaveFuncInfo> AddReturnedValueAsync(ReturnedSaveFuncInfo returnedSaveFuncInfo, ReturnedState ignoreStates = ReturnedState.None, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		bool semaphoreHasBeenTaken = false;
		if (returnedSaveFuncInfo == null)
		{
			WebErrorLog.Instance.StartLog("#ReturnedSaveFuncInfo_is_Null_returnedSaveFuncInfo", "", "AddReturnedValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 899);
			return this;
		}
		if (checked(Count + returnedSaveFuncInfo.Count) > 10000)
		{
			WebErrorLog.Instance.StartLog($"List overflow currentCount({Count})/memberName:{memberName}/sourceFilePath:{sourceFilePath}/sourceLineNumber:{sourceLineNumber}", "", "AddReturnedValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 904);
			return this;
		}
		int line = 0;
		try
		{
			await _semaphore.WaitAsync();
			semaphoreHasBeenTaken = true;
			line = 1;
			if ((ignoreStates & ReturnedState.Error) != ReturnedState.Error)
			{
				if (returnedSaveFuncInfo != null && returnedSaveFuncInfo.ErrorList?.Count > 0)
				{
					line = 2;
					foreach (Tuple<string, string> error in returnedSaveFuncInfo.Errors)
					{
						line = 4;
						if (error == null)
						{
							throw new NullReferenceException();
						}
						Errors.Add(error);
						line = 5;
						UiUpdater?.AddErrorAsync(error.Item2.GetTranslate(), error.Item1);
						line = 6;
					}
					line = 7;
				}
			}
			if ((ignoreStates & ReturnedState.Warning) != ReturnedState.Warning)
			{
				if (returnedSaveFuncInfo != null && returnedSaveFuncInfo.WarningList?.Count > 0)
				{
					line = 8;
					foreach (Tuple<string, string> warning in returnedSaveFuncInfo.Warnings)
					{
						line = 10;
						if (warning == null)
						{
							throw new NullReferenceException();
						}
						Warnings.Add(warning);
						line = 11;
						UiUpdater?.AddWarningAsync(warning.Item2.GetTranslate(), warning.Item1);
						line = 12;
					}
					line = 13;
				}
			}
			line = 14;
			if ((ignoreStates & ReturnedState.Information) != ReturnedState.Information)
			{
				if (returnedSaveFuncInfo != null && returnedSaveFuncInfo.InformationList?.Count > 0)
				{
					line = 15;
					foreach (Tuple<string, string> item in returnedSaveFuncInfo.Informations.ToList())
					{
						line = 16;
						if (item == null)
						{
							throw new NullReferenceException();
						}
						Informations.Add(item);
						line = 17;
						UiUpdater?.AddInformationAsync(item.Item2.GetTranslate(), item.Item1);
						line = 18;
					}
					line = 19;
				}
			}
			line = 20;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}\r\n" + $"#Line_{line}", "AddReturnedValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 963);
		}
		finally
		{
			if (semaphoreHasBeenTaken)
			{
				_semaphore.Release();
			}
		}
		return returnedSaveFuncInfo;
	}

	public void throwExceptionIfError()
	{
		if (HasError)
		{
			throw new Exception("Errors:" + GetErrorMessage() + "\n Warnings:" + WarningMessage + "\n Informations:" + GetInformationMessage());
		}
	}

	public void AddReturnedValue(IEnumerable<ReturnedSaveFuncInfo> returnedSaveFuncInfos, ReturnedState ignoreStates = ReturnedState.None, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		try
		{
			if (returnedSaveFuncInfos == null)
			{
				return;
			}
			foreach (ReturnedSaveFuncInfo returnedSaveFuncInfo in returnedSaveFuncInfos)
			{
				AddReturnedValue(returnedSaveFuncInfo, ignoreStates, memberName, sourceFilePath, sourceLineNumber);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddReturnedValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 1002);
		}
	}

	public async Task AddReturnedValueAsync(IEnumerable<ReturnedSaveFuncInfo> returnedSaveFuncInfos, ReturnedState ignoreStates = ReturnedState.None, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		try
		{
			if (returnedSaveFuncInfos == null)
			{
				return;
			}
			foreach (ReturnedSaveFuncInfo returnedSaveFuncInfo in returnedSaveFuncInfos)
			{
				await AddReturnedValueAsync(returnedSaveFuncInfo, ignoreStates, memberName, sourceFilePath, sourceLineNumber);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "memberName:" + memberName + "\r\nsourceFilePath:" + sourceFilePath + "\r\n" + $"sourceLineNumber:{sourceLineNumber}", "AddReturnedValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\BusinessLayer\\ReturnedSaveFuncInfo.cs", 1022);
		}
	}

	public List<ApiValidationResult> GetValidationResults()
	{
		List<ApiValidationResult> list = new List<ApiValidationResult>();
		if (Errors.Count > 0)
		{
			IEnumerable<string> enumerable = Errors.Select((Tuple<string, string> x) => x.Item1).Distinct();
			ApiValidationResult apiValidationResult = new ApiValidationResult
			{
				Type = ReturnSaveFuncInfoType.Error,
				Results = new Dictionary<string, List<string>>()
			};
			apiValidationResult.Results = new Dictionary<string, List<string>>();
			foreach (string item3 in enumerable)
			{
				List<string> list2 = new List<string>();
				foreach (string item4 in (from x in Errors
					where x.Item1 == item3
					select x.Item2).ToList())
				{
					list2.Add(item4);
				}
				apiValidationResult.Results.Add(item3, list2);
			}
			list.Add(apiValidationResult);
		}
		if (Warnings.Count > 0)
		{
			IEnumerable<string> enumerable2 = Warnings.Select((Tuple<string, string> x) => x.Item1).Distinct();
			ApiValidationResult apiValidationResult2 = new ApiValidationResult
			{
				Type = ReturnSaveFuncInfoType.Warning,
				Results = new Dictionary<string, List<string>>()
			};
			apiValidationResult2.Results = new Dictionary<string, List<string>>();
			foreach (string item2 in enumerable2)
			{
				List<string> list3 = new List<string>();
				foreach (string item5 in (from x in Warnings
					where x.Item1 == item2
					select x.Item2).ToList())
				{
					list3.Add(item5);
				}
				apiValidationResult2.Results.Add(item2, list3);
			}
			list.Add(apiValidationResult2);
		}
		if (Informations.Count > 0)
		{
			IEnumerable<string> enumerable3 = Informations.Select((Tuple<string, string> x) => x.Item1).Distinct();
			ApiValidationResult apiValidationResult3 = new ApiValidationResult
			{
				Type = ReturnSaveFuncInfoType.Information,
				Results = new Dictionary<string, List<string>>()
			};
			apiValidationResult3.Results = new Dictionary<string, List<string>>();
			foreach (string item in enumerable3)
			{
				List<string> list4 = new List<string>();
				foreach (string item6 in (from x in Informations
					where x.Item1 == item
					select x.Item2).ToList())
				{
					list4.Add(item6);
				}
				apiValidationResult3.Results.Add(item, list4);
			}
			list.Add(apiValidationResult3);
		}
		return list;
	}
}
