using System;
using System.Collections.Concurrent;
using System.Threading;
using Newtonsoft.Json;
using PacketParser.Entities;

namespace PacketParser.Scale.EventArguments;

public class WeightEventArgs
{
	private static ConcurrentDictionary<MicroScaleInfo, WeightEventArgs> _lastWeight = new ConcurrentDictionary<MicroScaleInfo, WeightEventArgs>();

	public decimal? NetWeight { get; set; }

	public decimal? GrossWeight { get; set; }

	public decimal? TareWeight { get; set; }

	public decimal? Price { get; set; }

	public decimal? TotalPrice { get; set; }

	public MicroScaleInfo Sender { get; set; }

	public DateTime Date { get; set; }

	[JsonIgnore]
	public CancellationToken Token { get; set; }

	public WeightEventArgs()
	{
	}

	public WeightEventArgs(decimal weightZarib, decimal? weight, decimal? price, decimal? totalPrice, MicroScaleInfo sender, DateTime date)
	{
		if (weightZarib <= 0m)
		{
			weightZarib = 1m;
		}
		GrossWeight = weight * (decimal?)weightZarib;
		TareWeight = null;
		NetWeight = GrossWeight;
		Price = price;
		TotalPrice = totalPrice;
		Sender = sender;
		Date = date;
		if (_lastWeight.TryGetValue(sender, out var value) && (value?.Sender.Type ?? ConnectionType.None) != ConnectionType.Takin_Serial)
		{
			if (value == null)
			{
				_lastWeight.TryRemove(sender, out var _);
				_lastWeight.TryAdd(sender, this);
			}
			else if (value.TareWeight.GetValueOrDefault() != 0m)
			{
				TareWeight = value.TareWeight;
				NetWeight = GrossWeight - TareWeight;
			}
			value.Sender = Sender;
			value.Date = Date;
			value.Price = Price;
			value.TotalPrice = TotalPrice;
			value.NetWeight = NetWeight;
			value.GrossWeight = GrossWeight;
		}
		else
		{
			_lastWeight.TryAdd(sender, this);
		}
	}

	public static void Tare(MicroScaleInfo scaleInfo)
	{
		try
		{
			_lastWeight.TryGetValue(scaleInfo, out var value);
			if (value == null)
			{
				throw new NotImplementedException("تا الان از این ترازو وزن ای دریافت نشده است.");
			}
			if (DateTime.Now.AddSeconds(-60.0) > value.Date)
			{
				throw new NotImplementedException("در 60 ثانیه گذشته از ترازو وزن دریافت نشده است.");
			}
			value.TareWeight = value.GrossWeight;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Tare", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Scale\\EventArguments\\WeightEventArgs.cs", 86);
		}
	}
}
