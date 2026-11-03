using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ClientHub.PayamakHubs;
using Microsoft.AspNetCore.SignalR.Client;
using PacketParser.Connection.LowLevel.Payamak;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Payamak.Interfaces;

namespace ClientHub.AccountingHubs;

public class PayamakClientHubInstances : INpAsyncDisposable, IAsyncDisposable
{
	private static Dictionary<string, PayamakClientHubInstances> _dic = new Dictionary<string, PayamakClientHubInstances>();

	private bool _isDisposed;

	public bool IsDisposed => _isDisposed;

	public ISmsNetAccountSignalRClient SmsNetAccount { get; set; }

	public ISmsOutBoxSignalRClient SmsOutBox { get; set; }

	public ISmsSentSignalRClient SmsSent { get; set; }

	public ISmsTemplateSignalRClient SmsTemplate { get; set; }

	public ISmsNotificationSignalRClient SmsNotification { get; set; }

	public static event Func<object, ConnectionEventArgs<HubConnectionState>, Task> OnHubChanged;

	public static PayamakClientHubInstances GetInstance(string contextGuid)
	{
		_dic.TryGetValue(contextGuid, out var value);
		return value;
	}

	public static void SetInstance(string contextGuid, PayamakClientHubInstances instance)
	{
		try
		{
			_dic.TryGetValue(contextGuid, out var value);
			if (value != null && !value.IsDisposed)
			{
				throw new DuplicateNameException("Context with Guid:" + contextGuid + " is already Exist");
			}
			if (value != null && value.IsDisposed)
			{
				_dic.Remove(contextGuid);
			}
			_dic.Add(contextGuid, instance);
			((SmsNetAccountClientHub)instance.SmsNetAccount).Changed += PayamakClientHubInstances_Changed;
			((SmsOutBoxClientHub)instance.SmsOutBox).Changed += PayamakClientHubInstances_Changed;
			((SmsSentClientHub)instance.SmsSent).Changed += PayamakClientHubInstances_Changed;
			((SmsTemplateClientHub)instance.SmsTemplate).Changed += PayamakClientHubInstances_Changed;
			((SmsNotificationClientHub)instance.SmsNotification).Changed += PayamakClientHubInstances_Changed;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\PayamakClientHubInstances.cs", 50);
		}
	}

	private static Task PayamakClientHubInstances_Changed(object sender, ConnectionEventArgs<HubConnectionState> e)
	{
		return PayamakClientHubInstances.OnHubChanged?.Invoke(sender, e);
	}

	public async ValueTask DisposeAsync()
	{
		_ = 4;
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (SmsNetAccount != null)
				{
					((SmsNetAccountClientHub)SmsNetAccount).Changed -= PayamakClientHubInstances_Changed;
					await SmsNetAccount.DisposeAsync();
					SmsNetAccount = null;
				}
				if (SmsOutBox != null)
				{
					((SmsOutBoxClientHub)SmsOutBox).Changed -= PayamakClientHubInstances_Changed;
					await SmsOutBox.DisposeAsync();
					SmsOutBox = null;
				}
				if (SmsSent != null)
				{
					((SmsSentClientHub)SmsSent).Changed -= PayamakClientHubInstances_Changed;
					await SmsSent.DisposeAsync();
					SmsSent = null;
				}
				if (SmsTemplate != null)
				{
					((SmsTemplateClientHub)SmsTemplate).Changed -= PayamakClientHubInstances_Changed;
					await SmsTemplate.DisposeAsync();
					SmsTemplate = null;
				}
				if (SmsNotification != null)
				{
					((SmsNotificationClientHub)SmsNotification).Changed -= PayamakClientHubInstances_Changed;
					await SmsNotification.DisposeAsync();
					SmsNotification = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\PayamakHubs\\PayamakClientHubInstances.cs", 91);
		}
	}
}
