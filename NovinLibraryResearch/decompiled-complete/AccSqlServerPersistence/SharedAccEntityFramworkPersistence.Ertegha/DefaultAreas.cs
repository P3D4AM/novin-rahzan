using System;
using System.Collections.Generic;
using EntityCache.Business;
using PacketParser.Entities;

namespace SharedAccEntityFramworkPersistence.Ertegha;

public static class DefaultAreas
{
	private static List<VisitorAreaBusiness> list = new List<VisitorAreaBusiness>();

	private static VisitorAreaBusiness SetDef(string contextGuid, string name, string cityGuid)
	{
		try
		{
			return new VisitorAreaBusiness(contextGuid)
			{
				Name = name,
				Guid = Guid.NewGuid(),
				ParentGuid = Guid.Parse(cityGuid),
				Modified = DateTime.Now,
				Status = true,
				Level = 3,
				Code = 0L,
				ChangeStatus = EntityChangeSt.New,
				Description = ""
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetDef", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefaultAreas.cs", 32);
			return null;
		}
	}

	public static List<VisitorAreaBusiness> AddDefaultList(string contextGuid)
	{
		try
		{
			list.Add(SetDef(contextGuid, "آتشگاه", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "ابن سینا", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "بزرگمهر", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "بهارستان", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "بیدآباد", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "پروین", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "پل وحید", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "طیران", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "جلفا", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "چهرباغ خواجو", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "حسین آباد", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "حکیم نظامی", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "خاقانی", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "خانه اصفهان", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "خمینی شهر", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "خیام", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "دولت آباد", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "رباط", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "زرین شهر", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "شاهین شهر", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "شهرضا", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "شیخ بهایی", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "عباس آباد", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "فولادشهر", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "گز", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "مبارکه", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "مولوی", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "میر", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "نشاط", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "هاتف", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "پل شیری (صائب)", "6276c7a9-f6a9-4c0d-9e3c-53d59538bf94"));
			list.Add(SetDef(contextGuid, "آبشار", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "آجودانیه", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "آرارات", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "آرژانتین", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "اتابک", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "اتحاد", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "اختیارآباد", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "ارامنه", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "ارم", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "ازگل", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "استادمعین", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "اسکندری", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "افسریه", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "اقدسیه", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "اکباتان", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "المهدی", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "الهیه", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "امامت", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "امیریه", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "اندیشه", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "اوقاف", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "اوین", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "ایرانشهر", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "باغ رضوان", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "بازار", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "باغ فردوس", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "بومهن", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "بهاران", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "پاسداران", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "پاکدشت", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "پامنار", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "پرستار", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "پرند", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "پونک", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "پیروزی", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "تجریش", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "توحید", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "تهرانسر", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "تهران نو", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "تهران پارس", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "جردن", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "جلفا", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "جماران", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "جمهوری", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "جوادیه", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "جیحون", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "چهارصددستگاه", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "چیتگر", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "چیذر", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "حصارک", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "حکیمیه", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "خاوران", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "خزانه", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "خلیج فارس", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "دارآباد", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "دانشگاه تهران", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "دانشگاه شریف", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "دربند", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "درکه", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "دروازه شمیران", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "دروس", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "دولاب", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "دولت آباد", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "دهکده المپیک", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "رباط کریم", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "رودهن", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "زرکش", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "زعفرانیه", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "زمزم", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "سبلان", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "ستارخان", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "سرخه حصار", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "سعادت آباد", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "سلسبیل", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "سوهانک", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "سیدخندان", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "شاندیز", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "شاهد", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "شریف", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "شوش", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "شهرقدس", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "شهریار", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "شهید رجایی", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "عبدل آباد", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "فتح", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "فدک", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "فرحزاد", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "فرمانیه", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "فرودگاه مهرآباد", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "فشم", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "فلاح", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "قرچک", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "قلهک", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "گاندی", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "گمرک", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "لواسان", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "لویزان", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "مجیدیه", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "محمودیه", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "منیریه", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "مولوی", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "میرداماد", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "نارمک", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "نازی آباد", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "نواب", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "نیاوران", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "ورامین", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "وردآورد", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "ولنجک", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "ونک", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "هفت حوض", "2d89428b-4496-429c-8afe-f1b478e8c86b"));
			list.Add(SetDef(contextGuid, "آریاشهر", "bc6fced2-ba59-4abb-8f2b-c0d53b82eb27"));
			list.Add(SetDef(contextGuid, "امانیه", "bc6fced2-ba59-4abb-8f2b-c0d53b82eb27"));
			list.Add(SetDef(contextGuid, "باهنر", "bc6fced2-ba59-4abb-8f2b-c0d53b82eb27"));
			list.Add(SetDef(contextGuid, "بهارستان", "bc6fced2-ba59-4abb-8f2b-c0d53b82eb27"));
			list.Add(SetDef(contextGuid, "پردیس", "bc6fced2-ba59-4abb-8f2b-c0d53b82eb27"));
			list.Add(SetDef(contextGuid, "پیروزی", "bc6fced2-ba59-4abb-8f2b-c0d53b82eb27"));
			list.Add(SetDef(contextGuid, "رسالت", "bc6fced2-ba59-4abb-8f2b-c0d53b82eb27"));
			list.Add(SetDef(contextGuid, "زرگان", "bc6fced2-ba59-4abb-8f2b-c0d53b82eb27"));
			list.Add(SetDef(contextGuid, "زیباشهر", "bc6fced2-ba59-4abb-8f2b-c0d53b82eb27"));
			list.Add(SetDef(contextGuid, "سپیدار", "bc6fced2-ba59-4abb-8f2b-c0d53b82eb27"));
			list.Add(SetDef(contextGuid, "طاقانی", "bc6fced2-ba59-4abb-8f2b-c0d53b82eb27"));
			list.Add(SetDef(contextGuid, "فولادشهر", "bc6fced2-ba59-4abb-8f2b-c0d53b82eb27"));
			list.Add(SetDef(contextGuid, "کیاشهر", "bc6fced2-ba59-4abb-8f2b-c0d53b82eb27"));
			list.Add(SetDef(contextGuid, "گلستان", "bc6fced2-ba59-4abb-8f2b-c0d53b82eb27"));
			list.Add(SetDef(contextGuid, "نادری", "bc6fced2-ba59-4abb-8f2b-c0d53b82eb27"));
			list.Add(SetDef(contextGuid, "نیوسایت", "bc6fced2-ba59-4abb-8f2b-c0d53b82eb27"));
			list.Add(SetDef(contextGuid, "یوسفی", "bc6fced2-ba59-4abb-8f2b-c0d53b82eb27"));
			list.Add(SetDef(contextGuid, "17شهریور", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "آزادشهر", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "ابوطالب", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "احمدآباد", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "امامت", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "اندیشه", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "بلوار الهیه", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "بلوار توس", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "بلوار فردوسی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "پیروزی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "جانباز", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "چهراراه لشکر", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "حرم مطهر", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "خیام", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "دانش آموز", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "دانشجو", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "راهنمایی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "رسالت", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "رضاشهر", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "سجادشهر", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "سرافرازان", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "سعدی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "سناباد", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "سیدرضی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "شاندیز", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "شهرک شهید رجایی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "شهید بهشتی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "صارمی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "صیادشیرازی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "طبرسی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "طرق", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "طرقبه", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "طلاب", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "عبادی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "عبدالمطلب", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "فرامرزعباسی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "فلسطین", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "فلکه برق", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "فلکه ضد", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "قائم", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "قاسم آباد", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "کلاهدوز", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "کوثر", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "کوهسنگی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "کوی مهدی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "گاز شرقی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "گنبدسبز", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "گوهرشاد", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "لادن", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "مصلی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "مطهری جنوبی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "مطهری شمالی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "معلم", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "موسوی قوچانی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "میدان شهدا", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "میدان عدل خمینی", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "نوفل لو", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "وکیل آباد", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "هاشمیه", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "هدایت", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "هفت تیر", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "هنرستان", "3d2f0a4c-a542-4863-9f1e-6c150ec1475f"));
			list.Add(SetDef(contextGuid, "آزادگان", "6a4b585e-bc15-4200-a53b-c7f314cd233e"));
			list.Add(SetDef(contextGuid, "باغستان", "6a4b585e-bc15-4200-a53b-c7f314cd233e"));
			list.Add(SetDef(contextGuid, "جهانشهر", "6a4b585e-bc15-4200-a53b-c7f314cd233e"));
			list.Add(SetDef(contextGuid, "حصارک", "6a4b585e-bc15-4200-a53b-c7f314cd233e"));
			list.Add(SetDef(contextGuid, "خرمدشت", "6a4b585e-bc15-4200-a53b-c7f314cd233e"));
			list.Add(SetDef(contextGuid, "درختی", "6a4b585e-bc15-4200-a53b-c7f314cd233e"));
			list.Add(SetDef(contextGuid, "ساسانی", "6a4b585e-bc15-4200-a53b-c7f314cd233e"));
			list.Add(SetDef(contextGuid, "شهریار", "6a4b585e-bc15-4200-a53b-c7f314cd233e"));
			list.Add(SetDef(contextGuid, "طالقانی", "6a4b585e-bc15-4200-a53b-c7f314cd233e"));
			list.Add(SetDef(contextGuid, "عظیمیه", "6a4b585e-bc15-4200-a53b-c7f314cd233e"));
			list.Add(SetDef(contextGuid, "فردیس", "6a4b585e-bc15-4200-a53b-c7f314cd233e"));
			list.Add(SetDef(contextGuid, "گلشهر", "6a4b585e-bc15-4200-a53b-c7f314cd233e"));
			list.Add(SetDef(contextGuid, "مارلیک", "6a4b585e-bc15-4200-a53b-c7f314cd233e"));
			list.Add(SetDef(contextGuid, "ملارد", "6a4b585e-bc15-4200-a53b-c7f314cd233e"));
			list.Add(SetDef(contextGuid, "هشتگرد", "6a4b585e-bc15-4200-a53b-c7f314cd233e"));
			list.Add(SetDef(contextGuid, "هفت تیر", "6a4b585e-bc15-4200-a53b-c7f314cd233e"));
			list.Add(SetDef(contextGuid, "ادبیات", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "ارتش", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "ارم", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "امیرکبیر", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "بعثت", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "پاسارگاد", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "پاسداران", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "پل غدیر", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "جمهوری", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "چمران", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "زند", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "ستاد", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "ستارخان", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "فسا", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "کازرون", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "کلبه", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "مرودشت", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "مطهری", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "نصر", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "والفجر", "f152b99c-acb7-459e-841e-e83f85cf4e32"));
			list.Add(SetDef(contextGuid, "آذر", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "آزادگان", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "امام", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "انسجام", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "بنیاد", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "پردیسان", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "پلیس", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "پیام نور", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "توحید", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "جمهوری", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "حرم", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "دانیال", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "دورشهر", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "سالاریه", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "سمیه", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "صفائیه", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "عطاران", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "کلهری", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "گلزار", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "مدرس", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			list.Add(SetDef(contextGuid, "هنرستان", "03a5f19f-a1c8-468c-9729-a46e260fce0b"));
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "AddDefaultList", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefaultAreas.cs", 322);
			return null;
		}
	}
}
