using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;

namespace PacketParser.Entities.Department;

public class WebOrderDetailForDisplay : WebOrderDetail
{
	public WebOrderDetail ParentOrder { get; set; }

	public WebProduct Product { get; set; }

	public WebDepot Depot { get; set; }

	public string DepotName { get; set; }

	private WebOrderDetailForDisplay()
	{
	}

	public static async Task<WebOrderDetailForDisplay> MappAsync(WebOrderDetail source, string depUserName, string DepPassword, ENSource enSource, CancellationToken token)
	{
		try
		{
			WebOrderDetailForDisplay destination = new WebOrderDetailForDisplay();
			if (source == null)
			{
				return null;
			}
			destination.ParentOrder = source;
			destination.Price = source?.Price ?? 0m;
			destination.Quantity = source?.Quantity ?? 0m;
			destination.Discount = source?.Discount ?? 0m;
			destination.Description = source?.Description ?? "";
			await destination.GetKalaDepotAsync(depUserName, DepPassword, enSource, token);
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MappAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebOrderDetailForDisplay.cs", 34);
			throw;
		}
	}

	private async Task<ReturnedSaveFuncInfo> GetKalaDepotAsync(string depUserName, string DepPassword, ENSource source, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			_ = base.ProductGuid;
			if (base.ProductGuid != Guid.Empty)
			{
				ReturnedSaveFuncInfoWithValue<WebProduct> returnedSaveFuncInfoWithValue = await ProductClient.GetProductAsync(depUserName, DepPassword, source, base.ProductGuid, token);
				if (!returnedSaveFuncInfoWithValue.HasError)
				{
					Product = returnedSaveFuncInfoWithValue.value;
					base.ProductName = Product?.Name ?? "";
				}
				else
				{
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetKalaDepotAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebOrderDetailForDisplay.cs", 51);
				}
			}
			_ = base.DepotGuid;
			if (base.DepotGuid != Guid.Empty)
			{
				ReturnedSaveFuncInfoWithValue<WebDepot> returnedSaveFuncInfoWithValue2 = await DepotClient.GetDepotAsync(depUserName, DepPassword, source, base.DepotGuid, token);
				if (!returnedSaveFuncInfoWithValue2.HasError)
				{
					Depot = returnedSaveFuncInfoWithValue2.value;
					DepotName = Depot?.Name ?? "";
				}
				else
				{
					ret.AddReturnedValue(returnedSaveFuncInfoWithValue2, ReturnedState.None, "GetKalaDepotAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebOrderDetailForDisplay.cs", 61);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetKalaDepotAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebOrderDetailForDisplay.cs", 66);
			ret.AddReturnedValue(ex, "", "GetKalaDepotAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebOrderDetailForDisplay.cs", 67);
		}
		return ret;
	}
}
