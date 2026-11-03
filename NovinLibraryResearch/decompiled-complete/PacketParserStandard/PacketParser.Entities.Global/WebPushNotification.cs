using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Global;

namespace PacketParser.Entities.Global;

public class WebPushNotification : Serializable<WebPushNotification>, IPushNotification, IHasJustGuid, IHasStatus
{
	private static string Url = HttpHelpers.ApiUrl + "PushNotification/";

	public Guid Guid { get; set; }

	public string Message { get; set; }

	public string Title { get; set; }

	public EnPushNotificationType Type { get; set; }

	public short DisplayCount { get; set; }

	public ENSource Source { get; set; }

	public DateTime Date { get; set; }

	public DateTime ExpireDate { get; set; }

	public DateTime ModifiedDate { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	[JsonIgnore]
	[XmlIgnore]
	public string DateSh => LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(Date);

	[JsonIgnore]
	[XmlIgnore]
	public string ExpireDateSh => LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(ExpireDate);

	public string HelperUri { get; set; }

	public async Task<ReturnedSaveFuncInfoWithValue<List<WebPushNotification>>> GetAllAsync(CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<WebPushNotification>> ret = new ReturnedSaveFuncInfoWithValue<List<WebPushNotification>>
		{
			value = new List<WebPushNotification>()
		};
		try
		{
			ReturnedSaveFuncInfoWithValue<WebResponse<List<WebPushNotification>>> returnedSaveFuncInfoWithValue = await (Url + "GetAll").GetFromApi<List<WebPushNotification>>(token, 1, null, 60000);
			ret.AddReturnedValue(returnedSaveFuncInfoWithValue, ReturnedState.None, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Global\\PushNotificationMessage.cs", 40);
			if (ret.HasError)
			{
				return ret;
			}
			if (returnedSaveFuncInfoWithValue.value.HttpClietnStatusCode == HttpStatusCode.OK)
			{
				ret.value = returnedSaveFuncInfoWithValue.value.Data;
			}
			else
			{
				ret.AddError(EnSystem.None, returnedSaveFuncInfoWithValue.value.HttpClietnStatusCode.ToString(), "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Global\\PushNotificationMessage.cs", 45);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Global\\PushNotificationMessage.cs", 49);
			ret.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Global\\PushNotificationMessage.cs", 50);
		}
		return ret;
	}

	public ReturnedSaveFuncInfo CheckValidation()
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(Message))
			{
				returnedSaveFuncInfo.AddError(EnSystem.None, "پیغام ارسالی برای کاربر نمیتواند خالی باشد", "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Global\\PushNotificationMessage.cs", 59);
			}
			if (string.IsNullOrEmpty(Title))
			{
				Title = "اطلاعیه درون برنامه ای";
			}
			if (Source == ENSource.None)
			{
				returnedSaveFuncInfo.AddError(EnSystem.None, "لطفا سورس مورد نظر را انتخاب کنید", "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Global\\PushNotificationMessage.cs", 61);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Global\\PushNotificationMessage.cs", 65);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "CheckValidation", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Global\\PushNotificationMessage.cs", 66);
		}
		return returnedSaveFuncInfo;
	}
}
