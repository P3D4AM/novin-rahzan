using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.DepartmentApiModel;
using PacketParser.DepartmentApiModel.RequestModel;

namespace PacketParser.Entities.Department;

public class WebReceptionCompactForDisplay : WebReceptionCompact
{
	public bool IsChecked { get; set; }

	public string DeliveryDateSh => base.DeliveryDate.MiladiToCalture();

	public string DateSabtSh => base.DateSabt.MiladiToCalture();

	public string DateSh => base.Date.MiladiToCalture();

	public string SourceFa => base.Source.GetDisplay();

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
			if (base.DeliveryDate.HasValue)
			{
				return base.DeliveryDate.Value.ToShortTimeString();
			}
			return "نامعلوم";
		}
	}

	public string TimeInsert => base.DateSabt.ToShortTimeString();

	public string TimeM => base.Date.ToShortTimeString();

	public static Task<ReturnedSaveFuncInfoWithValue<List<WebReceptionCompactForDisplay>>> GetAll(string depUserName, string depPassword, PaymentRequestModelcs model)
	{
		return HesabDariClient.GetAllPaymentCompactAsync<WebReceptionCompactForDisplay>(depUserName, depPassword, model);
	}

	public static Task<IEnumerable<WebReceptionCompactForDisplay>> SearchAsync(string Text, List<WebReceptionCompactForDisplay> LstData)
	{
		IEnumerable<WebReceptionCompactForDisplay> enumerable = LstData;
		try
		{
			if (enumerable == null)
			{
				return null;
			}
			enumerable = enumerable.Where((WebReceptionCompactForDisplay p) => p.DateSabtSh.ToLower().Contains(Text.ToLower()) || p.CustomerFullName.Contains(Text) || p.Sum == Text.ParseToDecimal() || p.Description.ToLower().Contains(Text.ToLower()));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Department\\WebReceptionCompactForDisplay.cs", 42);
		}
		return Task.FromResult(enumerable);
	}
}
