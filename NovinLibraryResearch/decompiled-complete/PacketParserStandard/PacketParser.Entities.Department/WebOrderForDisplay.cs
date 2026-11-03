using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;

namespace PacketParser.Entities.Department;

public class WebOrderForDisplay : WebOrder
{
	public List<WebOrderDetailForDisplay> lstOrderDetails = new List<WebOrderDetailForDisplay>();

	public WebCus OrderCus { get; set; }

	public new string SourceFa => base.Source.GetDisplay();

	public string DeliveryDateSh => base.DeliveryDate.MiladiToCalture();

	public string DateInsertSh => base.DateSabt.MiladiToCalture();

	public string CustomerName => OrderCus?.FullName ?? "";

	public short DepID { get; set; }

	private string DepUserNmae { get; set; } = "";


	private string DepPass { get; set; } = "";


	public async Task FillDisplayProperty(CancellationToken token)
	{
		new ReturnedSaveFuncInfoWithValue<WebOrderForDisplay>();
		try
		{
			if (!string.IsNullOrEmpty(DepUserNmae) && !string.IsNullOrEmpty(DepPass) && !(base.TafsilGuid == Guid.Empty) && !token.IsCancellationRequested)
			{
				await GetDetailOrderAsync(token);
				if (!token.IsCancellationRequested)
				{
					await GetCustomerAsync();
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillDisplayProperty", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebOrderForDisplay.cs", 37);
		}
	}

	public WebOrderForDisplay(Guid _OrderGuid, string depUsername, string depPassword)
	{
		try
		{
			base.Guid = _OrderGuid;
			DepUserNmae = depUsername;
			DepPass = depPassword;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebOrderForDisplay.cs", 47);
		}
	}

	public async Task<ReturnedSaveFuncInfoWithValue<WebOrderForDisplay>> WeborderToDisplaYAdpter(CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<WebOrderForDisplay> ret = new ReturnedSaveFuncInfoWithValue<WebOrderForDisplay>();
		try
		{
			if (ret.AddReturnedValue(await HesabDariClient.GetOrderAsync<WebOrderForDisplay>(DepUserNmae, DepPass, base.Source, base.Guid, token), ReturnedState.None, "WeborderToDisplaYAdpter", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebOrderForDisplay.cs", 55).HasError)
			{
				return ret;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "WeborderToDisplaYAdpter", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebOrderForDisplay.cs", 63);
			ret.AddReturnedValue(ex, "", "WeborderToDisplaYAdpter", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebOrderForDisplay.cs", 64);
		}
		return ret;
	}

	private async Task GetCustomerAsync()
	{
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<WebCus>> returnedSaveFuncInfoWithValue = await CustomerClient.GetCustomerAsync(base.TafsilGuid, DepUserNmae, base.Source, DepPass);
			if (!returnedSaveFuncInfoWithValue.HasError)
			{
				OrderCus = returnedSaveFuncInfoWithValue?.value?.Data;
			}
			else
			{
				OrderCus = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetCustomerAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebOrderForDisplay.cs", 77);
		}
	}

	private async Task GetDetailOrderAsync(CancellationToken token)
	{
		try
		{
			foreach (WebOrderDetail item in base.ListKala)
			{
				List<WebOrderDetailForDisplay> list = lstOrderDetails;
				list.Add(await WebOrderDetailForDisplay.MappAsync(item, DepUserNmae, DepPass, base.Source, token));
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetDetailOrderAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebOrderForDisplay.cs", 87);
		}
	}
}
