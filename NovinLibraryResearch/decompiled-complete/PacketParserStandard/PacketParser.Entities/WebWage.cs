using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;

namespace PacketParser.Entities;

public class WebWage : IWage, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	private static string apiUrl = HttpHelpers.ApiUrl + "webwage/";

	public string Name { get; set; }

	public EnWageType Type { get; set; }

	public string TypeForDispaly => Type.GetDisplay();

	public EnWageCalculationMethod CalculationMethod { get; set; }

	public string CalcutationMethodForDisplay => CalculationMethod.GetDisplay();

	public decimal PriceOrPercent { get; set; }

	public string PriceOrPercentForDisplay
	{
		get
		{
			if (CalculationMethod == EnWageCalculationMethod.Price)
			{
				return PriceOrPercent.NpToString("N0", 0);
			}
			return PriceOrPercent.ToString().Replace(".", "/");
		}
	}

	public decimal MinTransaction { get; set; }

	public decimal MaxTransaction { get; set; }

	public EnKhazaneMaster MasterType { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public Guid Guid { get; set; }

	public Guid? ServerGuid { get; set; }

	public static async Task<List<WebWage>> GetAllWagesAsync(CancellationToken token = default(CancellationToken), int timeOutMiliseconds = 15000)
	{
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebWage>>> returnedSaveFuncInfoWithValue = await (apiUrl + "getallwage").GetFromApi<List<WebWage>>(token, 3, null, timeOutMiliseconds);
			if (returnedSaveFuncInfoWithValue.HasError)
			{
				return null;
			}
			return returnedSaveFuncInfoWithValue.value.Data;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllWagesAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebWage.cs", 44);
		}
		return null;
	}
}
