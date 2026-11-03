using System;
using System.Collections.Generic;
using System.Linq;
using EntityCache.Business.Global;
using PacketParser;
using PacketParser.Entities;

namespace SharedAccEntityFramworkPersistence.Ertegha;

public class DefualtIntroductionMethod
{
	public static List<IntroductionMethodBusiness> ListAllDefault(string contextGuid)
	{
		try
		{
			return new List<IntroductionMethodBusiness>
			{
				CreateObject(contextGuid, "[دارای سابقه خرید]".GetTranslate()),
				CreateObject(contextGuid, "[دارای معرف]".GetTranslate()),
				CreateObject(contextGuid, "[دیوار]".GetTranslate()),
				CreateObject(contextGuid, "[شیپور]".GetTranslate()),
				CreateObject(contextGuid, "[اینستاگرام]".GetTranslate()),
				CreateObject(contextGuid, "[تراکت و بروشور]".GetTranslate()),
				CreateObject(contextGuid, "[پیامک صوتی]".GetTranslate()),
				CreateObject(contextGuid, "[پیامک متنی]".GetTranslate()),
				CreateObject(contextGuid, "[واتساپ]".GetTranslate()),
				CreateObject(contextGuid, "[دیجی کالا]".GetTranslate()),
				CreateObject(contextGuid, "[نماینده]".GetTranslate()),
				CreateObject(contextGuid, "[نمایشگاه]".GetTranslate()),
				CreateObject(contextGuid, "[بنر و بیلبورد]".GetTranslate()),
				CreateObject(contextGuid, "[موتور جستجوگر]".GetTranslate()),
				CreateObject(contextGuid, "[ویزیت حضوری]".GetTranslate()),
				CreateObject(contextGuid, "[ویزیت تلفن]".GetTranslate())
			}.Where((IntroductionMethodBusiness p) => p != null).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ListAllDefault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefualtIntroductionMethod.cs", 40);
			return new List<IntroductionMethodBusiness>();
		}
	}

	private static IntroductionMethodBusiness CreateObject(string contextGuid, string name)
	{
		try
		{
			return new IntroductionMethodBusiness(contextGuid)
			{
				Guid = Guid.NewGuid(),
				Modified = DateTime.Now,
				Status = true,
				Name = name
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateObject", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefualtIntroductionMethod.cs", 60);
			return null;
		}
	}
}
