using PacketParser;
using PacketParser.Entities;

namespace EntityCache.Business.Accounting.Cheque;

public class ChequeMOperationReportBusiness : ChequeOperationReportBase
{
	public ENUMCheck? OldState { get; set; }

	public override string OldStateStr
	{
		get
		{
			if (!OldState.HasValue)
			{
				return "";
			}
			return OldState.Value.GetDisplay();
		}
	}

	public ENUMCheck NewState { get; set; }

	public override string NewStateStr => NewState.GetDisplay();
}
