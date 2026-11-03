using System;
using FormCustom.Forms;
using FormCustom.LockService.Forms;
using PacketParser.Assistance;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.LkService;

namespace FormCustom.Froms.LkManage;

public class LkUiFactory : BaseForm, ILkUiFactory, ICacheBaseForm, INpDisposable, IDisposable
{
	public long LkSerial { get; set; }

	public LkUiFactory()
		: base("")
	{
	}

	public LkUiFactory(string contextGuid)
		: base(contextGuid)
	{
	}

	public LkUiFactory(string contextGuid, BaseFormConfig config)
		: base(contextGuid, config)
	{
	}

	public ILkUiFactory NewFrmActivation(string contextGuid, AccountingPackageClass cls, Package package, bool isDemo)
	{
		return new frmActivationNew(contextGuid, cls, package, isDemo);
	}

	public ILkUiFactory NewFrmDemo(string contextGuid, Package package)
	{
		return new FRMDemo(contextGuid, package);
	}

	public ILkUiFactory NewFrmActivation(string contextGuid, AttendancePackageClass cls, Package package)
	{
		return new FRMActivation(contextGuid, package);
	}

	public ILkUiFactory NewFrmLkClientSetting(string contextGuid, Package package, string message = "")
	{
		return new FrmLkClientSetting(contextGuid, package, message);
	}

	public ILkUiFactory NewFrmNotConnect(string contextGuid, string ip, short Port, string errorText)
	{
		throw new NotImplementedException();
	}

	bool INpDisposable.get_IsDisposed()
	{
		return base.IsDisposed;
	}
}
