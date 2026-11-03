using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Tax;

public class TaxTasvieMethod
{
	public EnTaxTasvieMethod Method { get; set; }

	public string MethodForDisplay => Method.GetDisplay().GetTranslate();

	public TaxTasvieMethod(EnTaxTasvieMethod method)
	{
		Method = method;
	}
}
