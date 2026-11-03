using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using EntityCache.Mapping.Global;
using PacketParser.Entities;
using PacketParser.Entities.Global;
using PacketParser.Entities.Interfaces;
using PacketParser.Global.PushNotification;
using PacketParser.Mapping;

namespace FormCustom.Forms;

public class PushNotificationManager : IHasContext
{
	public string ContextGuid { get; private set; }

	public ENSource Source { get; private set; }

	public List<PushNotificationBusiness> NotificationList { get; private set; }

	public PushNotificationManager(string contextGuid, ENSource source)
	{
		ContextGuid = contextGuid;
		Source = source;
	}

	public async Task UpdatePushNotificationAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			await SaveNewNotificationToLocalAsync(token);
			NotificationList = (await PushNotificationBusiness.GetActiveNotificationAsync(ContextGuid, Source))?.ToList() ?? new List<PushNotificationBusiness>();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdatePushNotificationAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\PushNotificationManager.cs", 36);
		}
	}

	private async Task SaveNewNotificationToLocalAsync(CancellationToken token)
	{
		_ = 3;
		try
		{
			if (token.IsCancellationRequested)
			{
				return;
			}
			List<PushNotificationBusiness> list = await (await WebPushNotificationClient.GetFromApi(await PushNotificationBusiness.GetLastModifiedDateAsync(ContextGuid), Source)).value.MapListAsync<WebPushNotification, PushNotificationBusiness>(ContextGuid, PushNotificationMapper.Instance, null, "", 20);
			if (token.IsCancellationRequested)
			{
				return;
			}
			foreach (PushNotificationBusiness item in list)
			{
				await item.SaveAsync(10);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveNewNotificationToLocalAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Forms\\PushNotificationManager.cs", 50);
		}
	}
}
