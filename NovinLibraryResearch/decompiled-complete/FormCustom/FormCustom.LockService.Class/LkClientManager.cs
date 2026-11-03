using System;
using System.Windows.Forms;
using EntityCache.Business.LkService.Interfaces;
using PacketParser.Assistance;
using PacketParser.Entities;
using PacketParser.LkService;

namespace FormCustom.LockService.Class;

public class LkClientManager : ILkClientManager
{
	public string ContextGuid { get; private set; }

	public ILkUiFactory Owner { get; private set; }

	public ILkClient LkClient { get; set; }

	public LkClientManager(string contextGuid, ILkUiFactory owner)
	{
		ContextGuid = contextGuid;
		Owner = owner;
		LkClient = LockClientSetting.GetSetting();
	}

	public bool ShowDemoForm(Package package)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Invalid comparison between Unknown and I4
		try
		{
			DialogResult val = (DialogResult)0;
			try
			{
				val = ((Form)Owner.NewFrmDemo(ContextGuid, package)).ShowDialog((IWin32Window)(Form)Owner);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ShowDemoForm", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LkClientManager.cs", 34);
			}
			return (int)val == 1;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "ShowDemoForm", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LkClientManager.cs", 39);
			throw;
		}
	}

	public bool ShowActivationForm(AccountingPackageClass cls, Package package, bool isDemo)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		try
		{
			return (int)((Form)Owner.NewFrmActivation(ContextGuid, cls, package, isDemo)).ShowDialog((IWin32Window)(Form)Owner) == 1;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ShowActivationForm", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LkClientManager.cs", 54);
			throw;
		}
	}

	public bool ShowActivationForm(AttendancePackageClass cls, Package package)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Invalid comparison between Unknown and I4
		try
		{
			return (int)((Form)Owner.NewFrmActivation(ContextGuid, cls, package)).ShowDialog((IWin32Window)(Form)Owner) == 1;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ShowActivationForm", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\LockService\\Class\\LkClientManager.cs", 69);
			throw;
		}
	}
}
