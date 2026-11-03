using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.Connection.LowLevel;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace ClientHub.AccountingHubs;

public class GoldAndCurrencyHubInstances : INpAsyncDisposable, IAsyncDisposable
{
	private static Dictionary<string, GoldAndCurrencyHubInstances> _dic = new Dictionary<string, GoldAndCurrencyHubInstances>();

	private bool _isDisposed;

	public IGoldAndCurrencySignalRClient GoldAndCurrency { get; set; }

	public bool IsDisposed => _isDisposed;

	public static GoldAndCurrencyHubInstances GetInstance(string contextGuid)
	{
		_dic.TryGetValue(contextGuid, out var value);
		return value;
	}

	public static void SetInstance(string contextGuid, GoldAndCurrencyHubInstances instance)
	{
		try
		{
			_dic.TryGetValue(contextGuid, out var value);
			if (value == null)
			{
				_dic.Add(contextGuid, instance);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\GoldAndCurrency\\GoldAndCurrencyHubInstances.cs", 33);
		}
	}

	public async ValueTask DisposeAsync()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (GoldAndCurrency != null)
				{
					await GoldAndCurrency.DisposeAsync();
					GoldAndCurrency = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "DisposeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedClientHub\\GoldAndCurrency\\GoldAndCurrencyHubInstances.cs", 48);
		}
	}
}
