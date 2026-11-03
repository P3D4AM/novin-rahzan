using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.BusinessLayer;
using PacketParser.Connection.LowLevel;
using PacketParser.Connection.LowLevel.Accounting.Lock;
using PacketParser.DepartmentApiModel;
using PacketParser.Entities;
using PacketParser.Entities.Department;
using PacketParser.Entities.Interfaces;
using PacketParser.LkService;
using Serilog;

namespace ClientHub.NPLockHub.LockHub;

public class LkEnvironmentHub : RetriableHubConnection, ILkEnvironmentSignalR, IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	public string ContextGuid { get; set; }

	public LkEnvironmentHub(string contextGuid, string endPointUrl, ILogger logger)
		: base(endPointUrl, logger)
	{
		ContextGuid = contextGuid;
	}

	public override Task InitializeConnectionAsync()
	{
		return Task.CompletedTask;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<LKInfo>> GetLkInfoAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<LKInfo> res = new ReturnedSaveFuncInfoWithValue<LKInfo>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<LKInfo> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetLkInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 29);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<LKInfo> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<LKInfo>>(base.Connection, "GetLkInfoAsync", (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetLkInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 32);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetLkInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 36);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetLkInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 39);
			res.AddReturnedValue(ex2, "", "GetLkInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 40);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<long>> GetLkSerialAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<long> res = new ReturnedSaveFuncInfoWithValue<long>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<long> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetLkSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 50);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<long> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<long>>(base.Connection, "GetLkSerialAsync", (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetLkSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 53);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetLkSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 57);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetLkSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 60);
			res.AddReturnedValue(ex2, "", "GetLkSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 61);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SetLkInfoAsync(LKInfo lk, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "SetLkInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 71);
			if (res.HasError)
			{
				return res;
			}
			await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "SetLkInfoAsync", (object)lk, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(res, ReturnedState.None, "SetLkInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 74);
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("", "", "SetLkInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 76);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SetLkInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 79);
			res.AddReturnedValue(ex2, "", "SetLkInfoAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 80);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<LockInformation>> GetLockInformationAsync(Package package, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<LockInformation> res = new ReturnedSaveFuncInfoWithValue<LockInformation>();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfoWithValue<LockInformation> returnedSaveFuncInfoWithValue = res;
			returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "GetLockInformationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 90);
			if (res.HasError)
			{
				return res;
			}
			WebResponseModel<LockInformation> webResponseModel = await HubConnectionExtensions.InvokeAsync<WebResponseModel<LockInformation>>(base.Connection, "GetLockInformationAsync", (object)package, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(webResponseModel, "", "GetLockInformationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 93);
			if (res.HasError)
			{
				return res;
			}
			res.value = webResponseModel.Value;
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetLockInformationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 97);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetLockInformationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 100);
			res.AddReturnedValue(ex2, "", "GetLockInformationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 101);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> CleanActivationCodesAsync(Package package, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "CleanActivationCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 111);
			if (res.HasError)
			{
				return res;
			}
			await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "CleanActivationCodesAsync", (object)package, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(res, ReturnedState.None, "CleanActivationCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 114);
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("", "", "CleanActivationCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 116);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "CleanActivationCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 119);
			res.AddReturnedValue(ex2, "", "CleanActivationCodesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 120);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SetLockInformationAsync(LockInformation lk, Package package, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "SetLockInformationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 130);
			if (res.HasError)
			{
				return res;
			}
			await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "SetLockInformationAsync", (object)lk, (object)package, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(res, ReturnedState.None, "SetLockInformationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 133);
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("", "", "SetLockInformationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 135);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SetLockInformationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 138);
			res.AddReturnedValue(ex2, "", "SetLockInformationAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 139);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SetLkSerialAsync(long value, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "SetLkSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 149);
			if (res.HasError)
			{
				return res;
			}
			await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "SetLkInfoAsync", (object)value, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(res, ReturnedState.None, "SetLkSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 152);
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("", "", "SetLkSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 154);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SetLkSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 157);
			res.AddReturnedValue(ex2, "", "SetLkSerialAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 158);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> SetSerialNumberAsync(long value, Package package, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "SetSerialNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 168);
			if (res.HasError)
			{
				return res;
			}
			await HubConnectionExtensions.InvokeAsync<ReturnedSaveFuncInfo>(base.Connection, "SetSerialNumberAsync", (object)value, (object)package, (object)token, CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token);
			res.AddReturnedValue(res, ReturnedState.None, "SetSerialNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 171);
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("", "", "SetSerialNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 173);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SetSerialNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 176);
			res.AddReturnedValue(ex2, "", "SetSerialNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 177);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfoWithValue<bool>> ShakingHandAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<bool> res = new ReturnedSaveFuncInfoWithValue<bool>();
		ReturnedSaveFuncInfoWithValue<bool> result;
		try
		{
			_ = 1;
			try
			{
				CancellationToken timeOutToken = new CancellationTokenSource(TimeSpan.FromSeconds(30.0)).Token;
				ReturnedSaveFuncInfoWithValue<bool> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await CheckConnectionAsync(), ReturnedState.None, "ShakingHandAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 187);
				if (res.HasError)
				{
					result = res;
					goto IL_0357;
				}
				if (token.IsCancellationRequested || base.IsDisposed)
				{
					res.value = false;
					result = res;
					goto IL_0357;
				}
				if (await HubConnectionExtensions.InvokeAsync<bool>(base.Connection, "ShakingHandAsync", CancellationTokenSource.CreateLinkedTokenSource(token, timeOutToken).Token))
				{
					res.value = true;
					result = res;
					goto IL_0357;
				}
			}
			catch (OperationCanceledException)
			{
				res.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "ShakingHandAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 200);
			}
			catch (SocketException)
			{
				if (token.IsCancellationRequested || base.IsDisposed)
				{
					res.value = false;
					result = res;
					goto IL_0357;
				}
			}
			catch (Exception ex3)
			{
				if (token.IsCancellationRequested || base.IsDisposed)
				{
					res.value = false;
					result = res;
					goto IL_0357;
				}
				if (!ex3.Message.ToLower().Contains("method cannot be called if the connection is not active".ToLower()))
				{
					WebErrorLog.Instance.StartLog(ex3, "", "ShakingHandAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\NPLockHub\\LockHub\\LkEnvironmentHub.cs", 217);
				}
			}
		}
		finally
		{
			await Task.Delay(5000);
		}
		return res;
		IL_0357:
		return result;
	}
}
