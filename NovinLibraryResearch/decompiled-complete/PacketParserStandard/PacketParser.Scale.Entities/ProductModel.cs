using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PacketParser.Entities;

namespace PacketParser.Scale.Entities;

public class ProductModel
{
	public enum ProductType
	{
		Weight,
		Number,
		Enums
	}

	private string _name;

	public string ScaleName { get; set; }

	public short MemoryNumber { get; set; }

	public string Price { get; set; }

	public string Name
	{
		get
		{
			return _name;
		}
		set
		{
			_name = ReverseName(value);
		}
	}

	public ProductType Type { get; set; }

	public string Code { get; set; }

	public string BarCode { get; set; }

	public static List<ProductModel> Parse(string data, int startIndex)
	{
		List<ProductModel> list = new List<ProductModel>();
		data = data.Replace("@", "").Replace("$", "").Replace("\r", "")
			.Replace("\n", "")
			.Replace("][", ",")
			.Replace("[", "")
			.Replace("]", "");
		data = data.Replace("(", " ").Replace(")", " ");
		checked
		{
			if (Enumerable.Contains(data, 'D'))
			{
				data = data.Substring(data.IndexOf('D') + 1, data.Length - data.IndexOf('D') - 2);
			}
			string[] array = data.Split(new char[1] { ',' });
			int num = 0;
			while (num < array.Count())
			{
				ProductModel item = new ProductModel
				{
					MemoryNumber = (short)(startIndex + Convert.ToInt32(unchecked(num / 5))),
					Price = array[num++],
					Name = array[num++],
					Type = GetProductType(array[num++]),
					Code = array[num++],
					BarCode = array[num++],
					ScaleName = "unknown"
				};
				list.Add(item);
			}
			return list;
		}
	}

	private static ProductType GetProductType(string typ)
	{
		ProductType result = ProductType.Weight;
		try
		{
			result = (ProductType)Convert.ToInt32(typ);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "could not parse product type", "GetProductType", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Scale\\Entities\\ProductModel.cs", 75);
		}
		return result;
	}

	private string ReverseName(string PName)
	{
		try
		{
			string text = "";
			text = PName;
			while (true)
			{
				Match match = new Regex("([0-9]{1,}[.*/\\\\]*[0-9]{1,})").Match(text);
				if (!match.Success)
				{
					break;
				}
				string text2 = Reverse(match.Value.ToString());
				text2 = text2.Replace(".", "/").Replace("*", "/").Replace("\\", "/");
				string oldValue = match.Value.ToString();
				PName = PName.Replace(oldValue, text2);
				text = text.Replace(match.Value.ToString(), "");
			}
			return PName;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ReverseName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Scale\\Entities\\ProductModel.cs", 110);
			return PName;
		}
	}

	public static string Reverse(string s)
	{
		char[] array = s.ToCharArray();
		Array.Reverse((Array)array);
		return new string(array);
	}
}
