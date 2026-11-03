using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Cheque;

public class ChequeTypeModel
{
	public EnChequeType ChequeType { get; set; }

	public string ChequeTypeStr => ChequeType.GetDisplay().GetTranslate();

	public short ChequeTypeShort => (short)ChequeType;

	public ChequeTypeModel(EnChequeType model)
	{
		ChequeType = model;
	}
}
