using System.Collections.Generic;

namespace PacketParser.LkService;

public class AccountingActivationCode
{
	public string ActivationCode { get; set; }

	public string ValidationMessage { get; set; }

	public List<PackageContainer> Packages { get; set; }
}
