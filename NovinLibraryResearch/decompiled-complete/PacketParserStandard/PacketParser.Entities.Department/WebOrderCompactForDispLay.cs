using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;
using PacketParser.DepartmentApiModel.RequestModel;

namespace PacketParser.Entities.Department;

public class WebOrderCompactForDispLay : WebOrderCompact
{
	public string FactorTypeFa => base.FactorType.GetDisplay();

	public string DeliveryDateSh => base.DeliveryDate.MiladiToCalture();

	public string DateInsertSh => base.DateInsert.MiladiToCalture();

	public string DateMSh => base.DateM.MiladiToCalture();

	public string SourceFa => base.Source.GetDisplay();

	public string TypeFactor => base.FactorType.GetDisplay();

	public string DeliveryStatusFa
	{
		get
		{
			if (base.DeliveryStatus.HasValue)
			{
				if (!base.DeliveryStatus.GetValueOrDefault())
				{
					return "خطا در تحویل";
				}
				return "تحویل شده";
			}
			return "ارسال شده";
		}
	}

	public string DeliveryTime
	{
		get
		{
			if (!base.DeliveryDate.HasValue)
			{
				return "نامعلوم";
			}
			return base.DeliveryDate.Value.ToShortTimeString();
		}
	}

	public string TimeInsert => base.DateInsert.ToShortTimeString();

	public string TimeM => base.DateM.ToShortTimeString();

	public bool IsChecked { get; set; }

	public static Task<ReturnedSaveFuncInfoWithValue<List<WebOrderCompactForDispLay>>> GetAllAsync(string depUserName, string depPassword, OrderRequestModelcs model)
	{
		return HesabDariClient.GetAllOrderCompactAsync<WebOrderCompactForDispLay>(depUserName, depPassword, model);
	}

	public static IEnumerable<WebOrderCompactForDispLay> Search(string Text, List<WebOrderCompactForDispLay> LstData)
	{
		IEnumerable<WebOrderCompactForDispLay> enumerable = LstData;
		try
		{
			if (enumerable == null)
			{
				return null;
			}
			enumerable = enumerable.Where((WebOrderCompactForDispLay p) => p.DateInsertSh.ToLower().Contains(Text.ToLower()) || p.TafsilName.Contains(Text) || p.TotalSum == Text.ParseToDecimal() || p.Description.ToLower().Contains(Text.ToLower()) || p.SumKala == Text.ParseToDecimal());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Search", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebOrderCompactForDispLay.cs", 52);
		}
		return enumerable;
	}
}
