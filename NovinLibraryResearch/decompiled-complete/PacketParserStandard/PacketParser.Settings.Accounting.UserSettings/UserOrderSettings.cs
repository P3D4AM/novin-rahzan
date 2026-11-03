using System.ComponentModel;

namespace PacketParser.Settings.Accounting.UserSettings;

public class UserOrderSettings
{
	private string _orderSellColor;

	private string _orderBackSellColor;

	private string _orderBuyColor;

	private string _orderBackBuyColor;

	private string _orderZayeatColor;

	private string _orderAmaniVorodColor;

	private string _orderAmaniKhorojColor;

	private string _pishFactorColor;

	private string _relatedTolidColor;

	private string _TasvieNaghdiColor;

	private string _NegativeProductColor;

	private string _OrderPointColor;

	public short First { get; set; }

	public short Sencond { get; set; }

	public short Third { get; set; }

	public string OrderSellColor
	{
		get
		{
			if (string.IsNullOrEmpty(_orderSellColor))
			{
				_orderSellColor = "#deb887";
			}
			return _orderSellColor;
		}
		set
		{
			_orderSellColor = value;
		}
	}

	public string OrderBackSellColor
	{
		get
		{
			if (string.IsNullOrEmpty(_orderBackSellColor))
			{
				_orderBackSellColor = "#808080";
			}
			return _orderBackSellColor;
		}
		set
		{
			_orderBackSellColor = value;
		}
	}

	public string OrderBuyColor
	{
		get
		{
			if (string.IsNullOrEmpty(_orderBuyColor))
			{
				_orderBuyColor = "#2e8b57";
			}
			return _orderBuyColor;
		}
		set
		{
			_orderBuyColor = value;
		}
	}

	public string OrderBackBuyColor
	{
		get
		{
			if (string.IsNullOrEmpty(_orderBackBuyColor))
			{
				_orderBackBuyColor = "#4682b4";
			}
			return _orderBackBuyColor;
		}
		set
		{
			_orderBackBuyColor = value;
		}
	}

	public string OrderZayeatColor
	{
		get
		{
			if (string.IsNullOrEmpty(_orderZayeatColor))
			{
				_orderZayeatColor = "#4682b4";
			}
			return _orderZayeatColor;
		}
		set
		{
			_orderZayeatColor = value;
		}
	}

	public string OrderAmaniVorodColor
	{
		get
		{
			if (string.IsNullOrEmpty(_orderAmaniVorodColor))
			{
				_orderAmaniVorodColor = "#ffffff";
			}
			return _orderAmaniVorodColor;
		}
		set
		{
			_orderAmaniVorodColor = value;
		}
	}

	public string OrderAmaniKhorojColor
	{
		get
		{
			if (string.IsNullOrEmpty(_orderAmaniKhorojColor))
			{
				_orderAmaniKhorojColor = "#ffffff";
			}
			return _orderAmaniKhorojColor;
		}
		set
		{
			_orderAmaniKhorojColor = value;
		}
	}

	public string PishFactorColor
	{
		get
		{
			if (string.IsNullOrEmpty(_pishFactorColor))
			{
				_pishFactorColor = "#ffffff";
			}
			return _pishFactorColor;
		}
		set
		{
			_pishFactorColor = value;
		}
	}

	public string RelatedTolidColor
	{
		get
		{
			if (string.IsNullOrEmpty(_relatedTolidColor))
			{
				_relatedTolidColor = "#dda0dd";
			}
			return _relatedTolidColor;
		}
		set
		{
			_relatedTolidColor = value;
		}
	}

	public string TasvieNaghdiColor
	{
		get
		{
			if (string.IsNullOrEmpty(_TasvieNaghdiColor))
			{
				_TasvieNaghdiColor = "#ff0000";
			}
			return _TasvieNaghdiColor;
		}
		set
		{
			_TasvieNaghdiColor = value;
		}
	}

	public string NegativeProductColor
	{
		get
		{
			if (string.IsNullOrEmpty(_NegativeProductColor))
			{
				_NegativeProductColor = "#FE6587";
			}
			return _NegativeProductColor;
		}
		set
		{
			_NegativeProductColor = value;
		}
	}

	public string OrderPointColor
	{
		get
		{
			if (string.IsNullOrEmpty(_OrderPointColor))
			{
				_OrderPointColor = "#B867FF";
			}
			return _OrderPointColor;
		}
		set
		{
			_OrderPointColor = value;
		}
	}

	public FilterFactorForReport FilterFactor { get; set; } = new FilterFactorForReport();


	[DefaultValue(true)]
	public bool SanadAffective { get; set; } = true;


	[DefaultValue(true)]
	public bool kardexAffective { get; set; } = true;

}
