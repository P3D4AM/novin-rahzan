using System;
using System.Collections.Generic;
using System.Linq;
using EntityCache.Business;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Ertegha;

public class DefaultBankSegestObjs
{
	public static List<SuggestedBankBusiness> ListAllSugBanksDefault(string contextGuid)
	{
		try
		{
			return new List<SuggestedBankBusiness>
			{
				CreateObject(contextGuid, "تجارت شعبه : "),
				CreateObject(contextGuid, "رفاه کارگران شعبه : "),
				CreateObject(contextGuid, "سپه شعبه : "),
				CreateObject(contextGuid, "سپه طلایی"),
				CreateObject(contextGuid, "پست بانک شعبه : "),
				CreateObject(contextGuid, "صادرات شعبه : "),
				CreateObject(contextGuid, "ملت شعبه : "),
				CreateObject(contextGuid, "ملی شعبه : "),
				CreateObject(contextGuid, "مهر ایران (بانک) شعبه : "),
				CreateObject(contextGuid, "مهر بسیجیان (صندوق پس انداز قرض الحسنه) شعبه : "),
				CreateObject(contextGuid, "توسعه شعبه : "),
				CreateObject(contextGuid, "اقتصاد نوین شعبه : "),
				CreateObject(contextGuid, "پارسیان شعبه : "),
				CreateObject(contextGuid, "کارآفرین شعبه : "),
				CreateObject(contextGuid, "سامان شعبه : "),
				CreateObject(contextGuid, "پاسارگاد شعبه : "),
				CreateObject(contextGuid, "سرمایه شعبه : "),
				CreateObject(contextGuid, "سینا شعبه : "),
				CreateObject(contextGuid, "تات شعبه : "),
				CreateObject(contextGuid, "شهر شعبه : "),
				CreateObject(contextGuid, "دی شعبه : "),
				CreateObject(contextGuid, "انصار شعبه : "),
				CreateObject(contextGuid, "توسعه صادرات شعبه : "),
				CreateObject(contextGuid, "صنعت و معدن شعبه : "),
				CreateObject(contextGuid, "کشاورزی شعبه : "),
				CreateObject(contextGuid, "مسکن شعبه : "),
				CreateObject(contextGuid, "توسعه تعاون شعبه : "),
				CreateObject(contextGuid, "فرشتگان شعبه : "),
				CreateObject(contextGuid, "میزان شعبه : "),
				CreateObject(contextGuid, "عسکریه شعبه : "),
				CreateObject(contextGuid, "الزهرا شعبه : "),
				CreateObject(contextGuid, "امید شعبه : "),
				CreateObject(contextGuid, "قوامین شعبه : "),
				CreateObject(contextGuid, "کارسازان آینده شعبه : "),
				CreateObject(contextGuid, "ثامن شعبه : "),
				CreateObject(contextGuid, "قائم شعبه : "),
				CreateObject(contextGuid, "صالحین شعبه : "),
				CreateObject(contextGuid, "ثامن الائمه شعبه : "),
				CreateObject(contextGuid, "ثامن الحجج شعبه : "),
				CreateObject(contextGuid, "ایرانیان شعبه : "),
				CreateObject(contextGuid, "اعتماد ایرانیان شعبه : "),
				CreateObject(contextGuid, "دانشجویان و دانش آموختگان وحدت شعبه : "),
				CreateObject(contextGuid, "قدس شعبه : "),
				CreateObject(contextGuid, "وحدت شعبه : "),
				CreateObject(contextGuid, "بدر توس شعبه : "),
				CreateObject(contextGuid, "مولی الموحدین شعبه : "),
				CreateObject(contextGuid, "دانش شعبه : "),
				CreateObject(contextGuid, "امین طلاب شعبه : "),
				CreateObject(contextGuid, "بهمن ایثار شعبه : "),
				CreateObject(contextGuid, "افضل توس شعبه : "),
				CreateObject(contextGuid, "معین الرضا شعبه : "),
				CreateObject(contextGuid, "فردوسی شعبه : "),
				CreateObject(contextGuid, "باران شعبه : "),
				CreateObject(contextGuid, "ایران زمین شعبه : "),
				CreateObject(contextGuid, "پیشگامان : ")
			}.Where((SuggestedBankBusiness p) => p != null).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ListAllSugBanksDefault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefaultBankSegestObjs.cs", 80);
			return new List<SuggestedBankBusiness>();
		}
	}

	private static SuggestedBankBusiness CreateObject(string contextGuid, string name)
	{
		try
		{
			return new SuggestedBankBusiness(contextGuid)
			{
				Guid = Guid.NewGuid(),
				Modified = DateTime.Now,
				BankName = name,
				ShobeCode = "",
				AccountNo = ""
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateObject", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefaultBankSegestObjs.cs", 101);
			return null;
		}
	}
}
