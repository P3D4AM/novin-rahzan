using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClientHub;
using EntityCache.Business.Accounting;
using EntityCache.CallCenter;
using PacketParser.BusinessLayer;
using PacketParser.Connection;
using PacketParser.Entities;
using PacketParser.Issabel;

namespace FormCustom.Issabel;

public class HandleIssableService
{
	public static async Task<ReturnedSaveFuncInfo> HandleIssableFullServiceAsync(string contextGuid, IsabelleClientSettings setting, bool isShowForm, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			IssabelInstanceHolder.GetInstance(contextGuid);
			if (!string.IsNullOrEmpty(setting?.Ip) && setting.IsabelleSettingEnable)
			{
				await CashierBusiness.GetCashierNumberAsync(contextGuid);
				LoginCallCenterToIssabel extention = new LoginCallCenterToIssabel
				{
					ExtentionNumberList = new List<int>()
				};
				IssabelInstanceHolder issabelInstance = new IssabelInstanceHolder();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await issabelInstance.HandleReportHubs(setting, extention), ReturnedState.None, "HandleIssableFullServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Issabel\\HandleIssableService.cs", 38);
				issabelInstance.HandleSignalRHubs(setting);
				IssabelInstanceHolder.SetInstance(contextGuid, issabelInstance, handleClient: true, handleReport: true);
				if (isShowForm)
				{
					frmNotify instance = frmNotify.GetInstance(contextGuid, (IssabelClient)issabelInstance.IssabelSignalRClient);
					if (setting.IsabelleSettingEnable)
					{
						((Control)instance).Show();
					}
					else
					{
						((Component)(object)instance).Dispose();
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HandleIssableFullServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Issabel\\HandleIssableService.cs", 49);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<IssabelInstanceHolder>> HandleIssableReportServiceAsync(string contextGuid, IsabelleClientSettings setting, bool isShowForm, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<IssabelInstanceHolder> res = new ReturnedSaveFuncInfoWithValue<IssabelInstanceHolder>();
		try
		{
			if (setting == null)
			{
				throw new ArgumentNullException("Seting is null");
			}
			IssabelInstanceHolder issabelInstance2 = (res.value = IssabelInstanceHolder.GetInstance(contextGuid));
			if (issabelInstance2 != null)
			{
				await issabelInstance2.DisposeAsync();
			}
			if (!string.IsNullOrEmpty(setting?.Ip))
			{
				CashierBusiness cashierBusiness = await CashierBusiness.GetCurrentCashierAsync(contextGuid);
				LoginCallCenterToIssabel loginCallCenterToIssabel = new LoginCallCenterToIssabel();
				if (cashierBusiness.Settings.NumberList?.Any() ?? false)
				{
					foreach (IsabelleNumber number in cashierBusiness.Settings.NumberList)
					{
						LoginCallCenterToIssabel loginCallCenterToIssabel2 = loginCallCenterToIssabel;
						if (loginCallCenterToIssabel2.ExtentionNumberList == null)
						{
							loginCallCenterToIssabel2.ExtentionNumberList = new List<int>();
						}
						loginCallCenterToIssabel.ExtentionNumberList.Add(number.Number);
					}
				}
				issabelInstance2 = (res.value = new IssabelInstanceHolder());
				ReturnedSaveFuncInfoWithValue<IssabelInstanceHolder> returnedSaveFuncInfoWithValue = res;
				returnedSaveFuncInfoWithValue.AddReturnedValue(await issabelInstance2.HandleReportHubs(setting, loginCallCenterToIssabel), ReturnedState.None, "HandleIssableReportServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Issabel\\HandleIssableService.cs", 74);
				IssabelInstanceHolder.SetInstance(contextGuid, issabelInstance2, handleClient: false, handleReport: true);
				if (isShowForm)
				{
					frmNotify instance2 = frmNotify.GetInstance(contextGuid, (IssabelClient)issabelInstance2.IssabelSignalRClient);
					if (setting.IsabelleSettingEnable)
					{
						((Control)instance2).Show();
					}
					else
					{
						((Component)(object)instance2).Dispose();
					}
				}
			}
			return res;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "HandleIssableReportServiceAsync", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Issabel\\HandleIssableService.cs", 87);
			throw;
		}
	}
}
