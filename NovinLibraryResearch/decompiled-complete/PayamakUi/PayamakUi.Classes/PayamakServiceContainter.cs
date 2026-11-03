using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientHub;
using ClientHub.AccountingHubs;
using ClientHub.PayamakHubs;
using EntityCache.Business;
using FormCustom.Cache;
using PacketParser.Entities;
using PacketParser.Payamak.Entities;
using PacketParser.Settings.Accounting;
using PayamakUi.Forms.FRMOthers;

namespace PayamakUi.Classes;

public class PayamakServiceContainter
{
	private static CancellationTokenSource _checkPanelValidityToken;

	private static CacheBaseForm _owner;

	public static async Task InitializeServiceAsync(string contextGuid, CacheBaseForm owner)
	{
		if (string.IsNullOrEmpty(contextGuid))
		{
			throw new ArgumentException("contextGuid");
		}
		try
		{
			_owner = owner;
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(contextGuid);
			PayamakClientHubInstances instance = PayamakClientHubInstances.GetInstance(contextGuid);
			if (instance != null)
			{
				await instance.DisposeAsync();
			}
			instance = new PayamakClientHubInstances
			{
				SmsNetAccount = new SmsNetAccountClientHub(contextGuid, $"http://{setting.Payamak.BaseHubIp}:{setting.Payamak.BaseHubPort}/SmsNetAccountHub", null),
				SmsOutBox = new SmsOutBoxClientHub(contextGuid, $"http://{setting.Payamak.BaseHubIp}:{setting.Payamak.BaseHubPort}/SmsOutBoxHub", null),
				SmsSent = new SmsSentClientHub(contextGuid, $"http://{setting.Payamak.BaseHubIp}:{setting.Payamak.BaseHubPort}/SmsSentHub", null),
				SmsTemplate = new SmsTemplateClientHub(contextGuid, $"http://{setting.Payamak.BaseHubIp}:{setting.Payamak.BaseHubPort}/SmsTemplateHub", null),
				SmsNotification = new SmsNotificationClientHub(contextGuid, $"http://{setting.Payamak.BaseHubIp}:{setting.Payamak.BaseHubPort}/SmsNotificationHub", null)
			};
			PayamakClientHubInstances.SetInstance(contextGuid, instance);
			((Control)frmPayamakNotify.GetInstance(contextGuid)).Hide();
			instance.SmsNotification.DataRecieved += SmsNotification_DataRecieved;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Service\\PayamakServiceContainter.cs", 49);
		}
	}

	private static async Task SmsNotification_DataRecieved(BaseSmsNotification e)
	{
		try
		{
			if (e != null && _owner != null)
			{
				_checkPanelValidityToken = new CancellationTokenSource();
				_ = _checkPanelValidityToken.Token;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SmsNotification_DataRecieved", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\Payamak\\PayamkUi\\Classes\\Service\\PayamakServiceContainter.cs", 74);
		}
	}
}
