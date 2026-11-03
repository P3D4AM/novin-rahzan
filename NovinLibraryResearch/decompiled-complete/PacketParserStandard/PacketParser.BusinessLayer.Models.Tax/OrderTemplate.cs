using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Tax;

public class OrderTemplate
{
	public EnTaxInvoiceTemplate Template { get; set; }

	public string TemplateForDisplay => Template.GetDisplay().GetTranslate();

	public OrderTemplate(EnTaxInvoiceTemplate template)
	{
		Template = template;
	}
}
