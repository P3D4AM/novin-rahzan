using System.ComponentModel;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class WebsitePriceDiscount
{
	private EnPriceType type = EnPriceType.sell1;

	[PersianNameAttribute.PersianName("سطح قیمت")]
	public EnPriceType Type
	{
		get
		{
			if (type == EnPriceType.none)
			{
				type = EnPriceType.sell1;
			}
			return type;
		}
		set
		{
			type = value;
		}
	}

	[PersianNameAttribute.PersianName("دارای تخفیف")]
	[DefaultValue(true)]
	public bool IsDiscounted { get; set; } = true;


	[PersianNameAttribute.PersianName("قیمت گرد شده")]
	public decimal RoundedPrice { get; set; } = 250m;


	public WebsitePriceDiscount Clone()
	{
		return (WebsitePriceDiscount)MemberwiseClone();
	}
}
