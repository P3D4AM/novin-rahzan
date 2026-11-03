using PacketParser.Entities;
using PacketParser.LkService;

namespace EntityCache.Business.LkService.Interfaces;

public interface ILkClientManager
{
	ILkClient LkClient { get; set; }

	bool ShowDemoForm(Package package);

	bool ShowActivationForm(AccountingPackageClass cls, Package package, bool isDemo);

	bool ShowActivationForm(AttendancePackageClass cls, Package package);
}
