using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using EntityCache.Interfaces;
using FormCustom.Cache;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace FormCustom.Notification;

public class SimpleNotificationFactory : IUiNotificationFactory, IHasContext
{
	private static ConcurrentDictionary<string, SimpleNotificationFactory> _instances = new ConcurrentDictionary<string, SimpleNotificationFactory>();

	private FrmSimpleNotification _frm;

	private static SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

	public string ContextGuid { get; private set; }

	public SimpleNotificationFactory(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static SimpleNotificationFactory GetInstance(string contextGuid)
	{
		try
		{
			OwnerFactory.Instance.GetOwner(contextGuid);
			_instances.TryGetValue(contextGuid, out var value);
			if (value != null)
			{
				return value;
			}
			_instances.TryRemove(contextGuid, out var _);
			value = new SimpleNotificationFactory(contextGuid);
			_instances.TryAdd(contextGuid, value);
			return value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetInstance", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 41);
			return null;
		}
	}

	public async Task DisplayAsync(string title, string message, ReturnedState status, CancellationToken token, int duration = 30000)
	{
		int line = 0;
		try
		{
			CacheBaseForm _owner = OwnerFactory.Instance.GetOwner(ContextGuid);
			await _semaphore.WaitAsync();
			line = 1;
			if (_frm?.IsDisposed ?? true)
			{
				line = 2;
				((Control)_owner).Invoke((Delegate)(Action)delegate
				{
					_frm = new FrmSimpleNotification(_owner.ContextGuid, title, message, status, duration, token);
					((Control)_frm).Show();
				});
				line = 3;
			}
			else
			{
				line = 10;
				await _frm.UpdateMessageAsync(title, message, status, duration, token);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "DisplayAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Notification\\FrmSimpleNotification.cs", 98);
		}
		finally
		{
			_semaphore.Release();
		}
	}

	public Task CancelNotif(int smsId)
	{
		throw new NotImplementedException();
	}
}
