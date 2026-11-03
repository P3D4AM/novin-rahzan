using PacketParser.Entities;

namespace PacketParser.Settings.Host;

public class LastSavedNumbers : Serializable<LastSavedNumbers>
{
	[PersianNameAttribute.PersianName("اخرین شماره دریافت")]
	public int LastReceptionId2 { get; set; }

	public int a
	{
		get
		{
			return LastReceptionId2;
		}
		set
		{
			LastReceptionId2 = value;
		}
	}

	[PersianNameAttribute.PersianName("اخرین شماره پرداخت")]
	public int LastPardakhtId2 { get; set; }

	public int b
	{
		get
		{
			return LastPardakhtId2;
		}
		set
		{
			LastPardakhtId2 = value;
		}
	}

	[PersianNameAttribute.PersianName("اخرین شماره فاکتورفروش")]
	public int LastOrderSellId2 { get; set; }

	public int c
	{
		get
		{
			return LastOrderSellId2;
		}
		set
		{
			LastOrderSellId2 = value;
		}
	}

	[PersianNameAttribute.PersianName("اخرین شماره فاکتور خرید")]
	public int LastOrderBuyId2 { get; set; }

	public int d
	{
		get
		{
			return LastOrderBuyId2;
		}
		set
		{
			LastOrderBuyId2 = value;
		}
	}

	[PersianNameAttribute.PersianName("اخرین شماره برگشت از فروش")]
	public int LastOrderBackSellId2 { get; set; }

	public int e
	{
		get
		{
			return LastOrderBackSellId2;
		}
		set
		{
			LastOrderBackSellId2 = value;
		}
	}

	[PersianNameAttribute.PersianName("اخرین شماره برگشت از خرید")]
	public int LastOrderBackBuyId2 { get; set; }

	public int f
	{
		get
		{
			return LastOrderBackBuyId2;
		}
		set
		{
			LastOrderBackBuyId2 = value;
		}
	}

	[PersianNameAttribute.PersianName("اخرین شماره پیش فاکتور")]
	public int LastPishFactorId2 { get; set; }

	public int g
	{
		get
		{
			return LastPishFactorId2;
		}
		set
		{
			LastPishFactorId2 = value;
		}
	}

	[PersianNameAttribute.PersianName("اخرین شماره فاکتور ضایعات")]
	public int LastZayeatId2 { get; set; }

	public int h
	{
		get
		{
			return LastZayeatId2;
		}
		set
		{
			LastZayeatId2 = value;
		}
	}

	[PersianNameAttribute.PersianName("اخرین شماره فاکتور امانی ورودی")]
	public int LastAmaniVorodId2 { get; set; }

	public int i
	{
		get
		{
			return LastAmaniVorodId2;
		}
		set
		{
			LastAmaniVorodId2 = value;
		}
	}

	[PersianNameAttribute.PersianName("اخرین شماره  فاکتور امانی خروج")]
	public int LastAmaniKhorojId2 { get; set; }

	public int j
	{
		get
		{
			return LastAmaniKhorojId2;
		}
		set
		{
			LastAmaniKhorojId2 = value;
		}
	}

	[PersianNameAttribute.PersianName("اخرین شماره فاکتور پیش خرید")]
	public int LastPreBuyId2 { get; set; }

	public int GetByType(Document factorType)
	{
		return factorType switch
		{
			Document.Reception => LastReceptionId2, 
			Document.Pardakht => LastPardakhtId2, 
			Document.OrderSell => LastOrderSellId2, 
			Document.OrderBuy => LastOrderBuyId2, 
			Document.OrderBackSell => LastOrderBackSellId2, 
			Document.OrderBackBuy => LastOrderBackBuyId2, 
			Document.PishFactor => LastPishFactorId2, 
			Document.OrderPreBuy => LastPreBuyId2, 
			Document.Zayeat => LastZayeatId2, 
			Document.AmaniVorod => LastAmaniVorodId2, 
			Document.AmaniKhoroj => LastAmaniKhorojId2, 
			_ => 0, 
		};
	}

	public LastSavedNumbers Clone()
	{
		return (LastSavedNumbers)MemberwiseClone();
	}
}
