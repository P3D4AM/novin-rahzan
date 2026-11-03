using System;
using System.Threading.Tasks;
using ClientHub;
using EntityCache.Business.Crm.Calling;
using PacketParser.Entities;
using PacketParser.Entities.CRM.CallCenter.Asterisk;

namespace EntityCache.WebService.Repository;

public class CallingLogDetailWebServiceRepository : IDisposable
{
	protected ClsCache Cache;

	private IssabelClient _client;

	public CallingLogDetailWebServiceRepository(IssabelClient client)
	{
		if (client == null)
		{
			throw new ArgumentNullException("client");
		}
		try
		{
			_client = client;
			_client.OnAsteriskHangup += _client_OnAsteriskHangup;
			_client.OnAsteriskNewExten += _client_OnAsteriskNewExten;
			_client.OnAsteriskQueueCallerJoin += _client_OnAsteriskQueueCallerJoin;
			_client.OnAsteriskQueueCallerLeave += _client_OnAsteriskQueueCallerLeave;
			_client.OnAsteriskQueueCallerAbandon += _client_OnAsteriskQueueCallerAbandon;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogDetailWebServiceRepository.cs", 27);
		}
	}

	private async Task _client_OnAsteriskQueueCallerAbandon(object arg1, AsteriskQueueCallerAbandonEventArgs arg2)
	{
		try
		{
			await new CallingLogDetailBusiness(_client.ContextGuid).SaveAsync(EnSystem.SignalR);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "_client_OnAsteriskQueueCallerAbandon", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogDetailWebServiceRepository.cs", 37);
		}
	}

	private async Task _client_OnAsteriskQueueCallerLeave(object arg1, AsteriskQueueCallerLeaveEventArgs arg2)
	{
		try
		{
			await new CallingLogDetailBusiness(_client.ContextGuid).SaveAsync(EnSystem.SignalR);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "_client_OnAsteriskQueueCallerLeave", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogDetailWebServiceRepository.cs", 47);
		}
	}

	private async Task _client_OnAsteriskQueueCallerJoin(object arg1, AsteriskQueueCallerJoinEventArgs arg2)
	{
		try
		{
			await new CallingLogDetailBusiness(_client.ContextGuid).SaveAsync(EnSystem.SignalR);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "_client_OnAsteriskQueueCallerJoin", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogDetailWebServiceRepository.cs", 57);
		}
	}

	private async Task _client_OnAsteriskNewExten(object arg1, AsteriskNewExtenEventArgs arg2)
	{
		try
		{
			await new CallingLogDetailBusiness(_client.ContextGuid).SaveAsync(EnSystem.SignalR);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "_client_OnAsteriskNewExten", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogDetailWebServiceRepository.cs", 67);
		}
	}

	private async Task _client_OnAsteriskHangup(object arg1, AsteriskHangupEventArgs arg2)
	{
		try
		{
			await new CallingLogDetailBusiness(_client.ContextGuid).SaveAsync(EnSystem.SignalR);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "_client_OnAsteriskHangup", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogDetailWebServiceRepository.cs", 77);
		}
	}

	public void Dispose()
	{
		try
		{
			_client.OnAsteriskHangup -= _client_OnAsteriskHangup;
			_client.OnAsteriskNewExten -= _client_OnAsteriskNewExten;
			_client.OnAsteriskQueueCallerJoin -= _client_OnAsteriskQueueCallerJoin;
			_client.OnAsteriskQueueCallerLeave -= _client_OnAsteriskQueueCallerLeave;
			_client.OnAsteriskQueueCallerAbandon -= _client_OnAsteriskQueueCallerAbandon;
			_client = null;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\WebService\\Repository\\CallingLogDetailWebServiceRepository.cs", 91);
		}
	}
}
