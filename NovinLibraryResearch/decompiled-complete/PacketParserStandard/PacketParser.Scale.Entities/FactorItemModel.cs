using System;
using PacketParser.Entities;

namespace PacketParser.Scale.Entities;

public class FactorItemModel
{
	private string _memoryNumber;

	public string MemoryNumber
	{
		get
		{
			try
			{
				if (_memoryNumber.ParseToLong(0L) > 11000)
				{
					_memoryNumber = checked(_memoryNumber.ParseToLong(0L) - 11000).ToString();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MemoryNumber", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Scale\\Entities\\FactorModel.cs", 109);
			}
			return _memoryNumber;
		}
		set
		{
			_memoryNumber = value;
		}
	}

	public string WeightOrCount { get; set; }

	public string UnitPriceOnScale { get; set; }

	public decimal UnitPriceOnAccounting { get; set; }

	public string RowSumOnScale { get; set; }

	public decimal RowSumOnAccounting { get; set; }

	public string ProductCode { get; set; }
}
