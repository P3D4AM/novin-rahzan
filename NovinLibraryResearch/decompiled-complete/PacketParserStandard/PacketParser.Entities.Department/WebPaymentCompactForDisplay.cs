using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;
using PacketParser.DepartmentApiModel.RequestModel;

namespace PacketParser.Entities.Department;

public class WebPaymentCompactForDisplay : WebPaymentCompact
{
	public bool IsChecked { get; set; }

	public string DeliveryDateSh => base.DeliveryDate.MiladiToCalture();

	public string DateSabtSh => base.DateSabt.MiladiToCalture();

	public string DateSh => base.Date.MiladiToCalture();

	public string SourceFa => base.Source.GetDisplay();

	public string DeliveryTime
	{
		get
		{
			if (base.DeliveryDate.HasValue)
			{
				return base.DeliveryDate.Value.ToShortTimeString();
			}
			return "نامعلوم";
		}
	}

	public string TimeInsert => base.Date.ToShortTimeString();

	public string TimeM => base.DateSabt.ToShortTimeString();

	public static Task<ReturnedSaveFuncInfoWithValue<List<WebPaymentCompactForDisplay>>> GetAll(string depUserName, string depPassword, PaymentRequestModelcs model)
	{
		return HesabDariClient.GetAllPaymentCompactAsync<WebPaymentCompactForDisplay>(depUserName, depPassword, model);
	}

	public static Task<IEnumerable<WebPaymentCompactForDisplay>> SearchAsync(string Text, List<WebPaymentCompactForDisplay> LstData)
	{
		IEnumerable<WebPaymentCompactForDisplay> enumerable = LstData;
		try
		{
			if (enumerable == null)
			{
				return null;
			}
			enumerable = enumerable.Where((WebPaymentCompactForDisplay p) => p.DateSabtSh.ToLower().Contains(Text.ToLower()) || p.CustomerFullName.Contains(Text) || p.Sum == Text.ParseToDecimal() || p.Description.ToLower().Contains(Text.ToLower()));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebPaymentCompactForDisplay.cs", 42);
		}
		return Task.FromResult(enumerable);
	}
}
