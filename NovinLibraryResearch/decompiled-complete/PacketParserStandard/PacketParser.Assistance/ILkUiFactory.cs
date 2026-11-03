using System;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.LkService;

namespace PacketParser.Assistance;

public interface ILkUiFactory : ICacheBaseForm, INpDisposable, IDisposable
{
	ILkUiFactory NewFrmLkClientSetting(string contextGuid, Package package, string message = "");

	ILkUiFactory NewFrmNotConnect(string contextGuid, string ip, short Port, string errorText);

	ILkUiFactory NewFrmActivation(string contextGuid, AccountingPackageClass cls, Package package, bool isDemo);

	ILkUiFactory NewFrmActivation(string contextGuid, AttendancePackageClass cls, Package package);

	ILkUiFactory NewFrmDemo(string contextGuid, Package package);
}
