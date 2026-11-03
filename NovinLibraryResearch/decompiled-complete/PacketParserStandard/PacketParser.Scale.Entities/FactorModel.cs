using System;
using System.Collections.Generic;
using System.Linq;
using PacketParser.Entities;

namespace PacketParser.Scale.Entities;

public class FactorModel
{
	public string MemoryNumber { get; set; }

	public string Time { get; set; }

	public string Date { get; set; }

	public string ScaleName { get; set; }

	public string Cashier { get; set; }

	public string Number { get; set; }

	public byte ScaleNo { get; set; }

	public string SumTotalOnScale { get; set; }

	public decimal SumTotalOnAccounting { get; set; }

	public string ProductCount { get; set; }

	public string TaxOnScale { get; set; }

	public decimal TaxOnAccounting { get; set; }

	public string DiscountOnScale { get; set; }

	public decimal DiscountOnAccounting { get; set; }

	public string SumAghalmOnScale { get; set; }

	public decimal SumAghalmOnAccounting { get; set; }

	public bool FactorValid { get; set; }

	public DateTime DateM { get; set; }

	public List<FactorItemModel> Items { get; set; }

	public FactorModel(string data)
	{
		CreateFactorModel(data);
	}

	public FactorModel()
	{
	}

	private void CreateFactorModel(string data)
	{
		int num = 1;
		string[] array = data.Replace("@", "").Replace("$", "").Split(new char[1] { ',' });
		checked
		{
			try
			{
				num = 2;
				MemoryNumber = array[0];
				num = 3;
				Time = array[1].Substring(0, 2) + ":" + array[1].Substring(2, 2);
				num = 4;
				Date = array[2].Substring(0, 2) + "/" + array[2].Substring(2, 2) + "/" + array[2].Substring(4, 2);
				num = 5;
				ScaleName = array[3];
				num = 6;
				Cashier = array[4];
				num = 7;
				Number = array[5];
				num = 8;
				SumTotalOnScale = array[6];
				num = 9;
				ProductCount = array[7] ?? "";
				num = 10;
				TaxOnScale = array[8];
				num = 11;
				DiscountOnScale = array[9];
				num = 12;
				SumAghalmOnScale = array[10];
				num = 13;
				Items = new List<FactorItemModel>();
				num = 14;
				int num2 = 0;
				num = 15;
				int num3 = 0;
				num = 16;
				int num4 = 11;
				while (num4 + 5 < array.Count())
				{
					num = 17;
					Items.Add(new FactorItemModel
					{
						MemoryNumber = array[++num4],
						WeightOrCount = array[++num4],
						UnitPriceOnScale = array[++num4],
						RowSumOnScale = array[++num4],
						ProductCode = array[++num4]
					});
					num = 18;
					num2++;
				}
				num = 19;
				foreach (FactorItemModel item in Items)
				{
					num3 += item.RowSumOnScale.ParseToInt();
				}
				num = 21;
				if (num2 == ProductCount.ParseToInt() && Math.Abs(num3 - SumTotalOnScale.ParseToInt()) < 100)
				{
					FactorValid = true;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{num}//Data:{data}", "CreateFactorModel", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Scale\\Entities\\FactorModel.cs", 92);
			}
		}
	}
}
