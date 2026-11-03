using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;

namespace PacketParser.Entities;

public class WebPageContainer<T> where T : class
{
	public int TotalItems { get; set; }

	public int TotalPages { get; set; }

	public int CurrentPageCount { get; set; }

	public int EachPageCount { get; set; }

	public int CurrentPageNumber { get; set; }

	public List<NumberContainer<T>> Items { get; set; }

	[JsonIgnore]
	public List<T> OrginItems { get; set; }

	public static ReturnedSaveFuncInfoWithValue<WebPageContainer<T>> GeneratePageItems(List<T> items, int eachPageCount, int currentPageNumber)
	{
		int num = 1;
		ReturnedSaveFuncInfoWithValue<WebPageContainer<T>> returnedSaveFuncInfoWithValue = new ReturnedSaveFuncInfoWithValue<WebPageContainer<T>>();
		num = 2;
		if (eachPageCount <= 0)
		{
			returnedSaveFuncInfoWithValue.AddError(EnSystem.None, "Input Arg PageCount ==0 OR PageCount<0 Not Valid ", "GeneratePageItems", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPageContainer.cs", 25);
		}
		num = 3;
		if (items == null)
		{
			returnedSaveFuncInfoWithValue.AddError(EnSystem.None, "eachPageCount is null or empty", "GeneratePageItems", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPageContainer.cs", 27);
		}
		num = 4;
		if (currentPageNumber < 0)
		{
			returnedSaveFuncInfoWithValue.AddError(EnSystem.None, string.Format("value {0} for {1} is less than zero", currentPageNumber, "currentPageNumber"), "GeneratePageItems", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPageContainer.cs", 29);
		}
		checked
		{
			try
			{
				if (returnedSaveFuncInfoWithValue.HasError)
				{
					return returnedSaveFuncInfoWithValue;
				}
				num = 5;
				if (currentPageNumber <= 0)
				{
					currentPageNumber = 1;
				}
				num = 6;
				int num2 = eachPageCount * (currentPageNumber - 1);
				num = 7;
				int num3 = eachPageCount * currentPageNumber;
				num = 8;
				if (num3 > (items?.Count() ?? 0))
				{
					num = 9;
					num3 = items?.Count() ?? 0;
				}
				num = 10;
				WebPageContainer<T> value = new WebPageContainer<T>
				{
					EachPageCount = eachPageCount,
					CurrentPageCount = num3 - num2,
					Items = new List<NumberContainer<T>>(),
					TotalItems = (items?.Count ?? 0),
					TotalPages = (int)Math.Ceiling((float)(items?.Count ?? 0) / (float)eachPageCount),
					CurrentPageNumber = currentPageNumber
				};
				returnedSaveFuncInfoWithValue.value = value;
				num = 11;
				if ((items?.Count ?? 0) > 0 && returnedSaveFuncInfoWithValue.value.TotalPages <= 0)
				{
					num = 12;
					returnedSaveFuncInfoWithValue.value.TotalPages = 1;
				}
				num = 13;
				returnedSaveFuncInfoWithValue.value.OrginItems = new List<T>();
				num = 14;
				for (int i = num2; i < num3; i++)
				{
					num = 15;
					returnedSaveFuncInfoWithValue.value.OrginItems.Add(items[i]);
					num = 16;
					returnedSaveFuncInfoWithValue.value.Items.Add(new NumberContainer<T>
					{
						Item = items[i],
						RowNumber = i
					});
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Error in Line {num}", "GeneratePageItems", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPageContainer.cs", 82);
				returnedSaveFuncInfoWithValue.AddReturnedValue(ex, "", "GeneratePageItems", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebPageContainer.cs", 83);
			}
			return returnedSaveFuncInfoWithValue;
		}
	}
}
