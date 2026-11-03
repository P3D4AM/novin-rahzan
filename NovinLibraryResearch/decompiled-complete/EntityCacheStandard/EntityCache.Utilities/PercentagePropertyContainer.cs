using System;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Entities;

namespace EntityCache.Utilities;

public class PercentagePropertyContainer
{
	private double? _manualPercent;

	private Tuple<decimal, decimal> _manualPrice;

	private decimal _price;

	public PercentageMode Mode
	{
		get
		{
			try
			{
				if (!_manualPercent.HasValue && _manualPrice != null)
				{
					return PercentageMode.Price;
				}
				if (_manualPercent.HasValue && _manualPrice == null)
				{
					return PercentageMode.Percent;
				}
				return PercentageMode.None;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Mode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Utilities\\PercentagePropertyContainer.cs", 31);
				throw;
			}
		}
	}

	public decimal OldQunatity => _manualPrice?.Item2 ?? 0m;

	[JsonIgnore]
	public short DigitForRound { get; }

	[JsonIgnore]
	public short DigitAfterPoint { get; }

	public PercentagePropertyContainer(short digitForRound, short digitAfterPoint)
	{
		DigitForRound = digitForRound;
		DigitAfterPoint = digitAfterPoint;
	}

	internal void SetPercentageMode(PercentageMode mode, decimal quantity, decimal orderDetSum, EnRoundDecimal? roundType = null)
	{
		try
		{
			switch (mode)
			{
			case PercentageMode.Percent:
				SetPercentString(GetPercent(orderDetSum).ToString(), orderDetSum, roundType);
				break;
			case PercentageMode.None:
			case PercentageMode.Price:
				SetPriceString(GetPrice().ToString(), quantity);
				break;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetPercentageMode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Utilities\\PercentagePropertyContainer.cs", 61);
		}
	}

	public decimal GetPercent(decimal orderDetSum)
	{
		try
		{
			if (_manualPercent.HasValue)
			{
				return Math.Round((decimal)_manualPercent.Value, DigitForRound);
			}
			if (_price <= 0m || orderDetSum <= 0m)
			{
				return 0m;
			}
			return Math.Round(_price * 100m / orderDetSum, DigitForRound);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPercent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Utilities\\PercentagePropertyContainer.cs", 73);
			return 0m;
		}
	}

	public decimal GetPrice()
	{
		return _price;
	}

	public bool SetPrice(decimal price)
	{
		if (_price == price)
		{
			return false;
		}
		_price = price;
		return true;
	}

	public void ResetPercentString()
	{
		_manualPercent = null;
	}

	public bool SetPercentString(string percent, decimal orderDetSum, EnRoundDecimal? roundType = null)
	{
		try
		{
			if (_manualPercent.HasValue && Math.Round((decimal)_manualPercent.GetValueOrDefault(), DigitForRound) == percent.ParseToDecimal(0m, 3))
			{
				return false;
			}
			_manualPercent = percent.ParseToDouble(0.0, 3);
			_manualPrice = null;
			if (roundType.GetValueOrDefault() == EnRoundDecimal.None)
			{
				_price = Math.Round(orderDetSum * (decimal)_manualPercent.Value / 100m, DigitAfterPoint);
			}
			else
			{
				switch (roundType.Value)
				{
				case EnRoundDecimal.Ceiling:
					_price = Math.Ceiling(orderDetSum * (decimal)_manualPercent.Value / 100m);
					break;
				case EnRoundDecimal.Floor:
					_price = Math.Floor(orderDetSum * (decimal)_manualPercent.Value / 100m);
					break;
				default:
					_price = Math.Round(orderDetSum * (decimal)_manualPercent.Value / 100m, DigitAfterPoint);
					break;
				}
			}
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetPercentString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Utilities\\PercentagePropertyContainer.cs", 112);
			return false;
		}
	}

	public void ResetPriceString()
	{
		_manualPrice = null;
	}

	public void Reset()
	{
		ResetPercentString();
		ResetPriceString();
	}

	public void SetPriceString(string price, decimal quantity)
	{
		_manualPercent = null;
		if (quantity <= 0m)
		{
			quantity = 1m;
		}
		_manualPrice = new Tuple<decimal, decimal>(price.ParseToDecimal(0m, 3), quantity);
	}

	public bool UpdatePrice(decimal newQuantity, decimal sum, decimal defaultPercent, int decimals = 0, EnRoundDecimal? roundType = null, decimal rowPrice = 0m)
	{
		try
		{
			decimal price = GetPrice();
			price = ((_manualPercent.GetValueOrDefault() > 0.0) ? ((roundType.GetValueOrDefault() == EnRoundDecimal.None) ? Math.Round(sum * (decimal)_manualPercent.Value / 100m, decimals) : ((!(newQuantity != 0m) || !(rowPrice != 0m)) ? Math.Floor(sum * (decimal)_manualPercent.Value / 100m) : (Math.Floor(rowPrice * (decimal)_manualPercent.Value / 100m) * newQuantity))) : ((!((_manualPrice?.Item2 ?? 0m) > 0m)) ? Math.Round(sum * defaultPercent / 100m, decimals) : Math.Round(newQuantity * _manualPrice.Item1 / _manualPrice.Item2, DigitAfterPoint)));
			return SetPrice(price);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdatePrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Utilities\\PercentagePropertyContainer.cs", 151);
		}
		return false;
	}
}
