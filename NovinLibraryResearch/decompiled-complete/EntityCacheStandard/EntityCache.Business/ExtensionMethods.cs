using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Settings.Accounting;

namespace EntityCache.Business;

public static class ExtensionMethods
{
	public static Task<ReturnedSaveFuncInfo> SaveAsync(this AccGlobalSettings setting, string contextGuid, bool awaitOnSemaphore = true, short tryCount = 10, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return LocalSettingBusiness.SetSettingAsync(contextGuid, setting, send2Server: true, token);
	}

	public static Task<ReturnedSaveFuncInfo> SaveAsync(this NPSMS_WebServiceSettings setting, string contextGuid, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		return LocalSettingBusiness.SaveAsync(contextGuid, "AccNPSMS_WebServiceSettings", setting.ToString()?.FixString(), awaitOnSemaphore: true, 10);
	}

	public static async Task<ReturnedSaveFuncInfo> SaveAsync(this ScaleSettings setting, string contextGuid, object sender, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		ReturnedSaveFuncInfo obj = await LocalSettingBusiness.SetScaleSettingsAsync(contextGuid, setting);
		if (!obj.HasError)
		{
			ScaleEventAggregator.RaiseChanged(contextGuid, sender, EventArgs.Empty);
		}
		return obj;
	}

	public static Task<ReturnedSaveFuncInfo> SaveAsync(this TransferSettings setting, string contextGuid, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		return LocalSettingBusiness.SaveAsync(contextGuid, "Acc_TranferSettings", setting.ToString(), awaitOnSemaphore: true, 10);
	}

	public static Task<ReturnedSaveFuncInfo> SaveAsync(this KioskSettings setting, string contextGuid, object sender, bool awaitOnSemaphore = true, short tryCount = 10, string tranName = "")
	{
		return LocalSettingBusiness.SetKioskSettingsAsync(contextGuid, setting);
	}
}
