using PacketParser;
using PacketParser.Entities;

namespace EntityCache.Business.Accounting.Cheque;

public class ChequeShOperationReportBusiness : ChequeOperationReportBase
{
	public ENCheckSHStatus? OldState { get; set; }

	public override string OldStateStr
	{
		get
		{
			if (!OldState.HasValue)
			{
				return "";
			}
			return OldState.Value.GetDisplay().GetTranslate();
		}
	}

	public ENCheckSHStatus NewState { get; set; }

	public override string NewStateStr => NewState.GetDisplay().GetTranslate();
}
