using System;
using EntityCache.Business.Accounting;
using PacketParser.Entities;

namespace EntityCache.Business.Global;

public class CustomerMetaDataBusiness : MetaDataBusiness
{
	public CustomerMetaKey MetaKey
	{
		get
		{
			try
			{
				if (short.TryParse(base.Key, out var result))
				{
					return (CustomerMetaKey)result;
				}
				string key = base.Key;
				if (key != null)
				{
					switch (key.Length)
					{
					case 12:
						switch (key[6])
						{
						case 'ح':
							if (!(key == "شماره حساب 1"))
							{
								if (!(key == "شماره حساب 2"))
								{
									break;
								}
								return CustomerMetaKey.HesabNumber2;
							}
							return CustomerMetaKey.HesabNumber1;
						case 'ک':
							if (!(key == "شماره کارت 1"))
							{
								if (!(key == "شماره کارت 2"))
								{
									break;
								}
								return CustomerMetaKey.CardNumber2;
							}
							return CustomerMetaKey.CardNumber1;
						case 'ا':
							if (!(key == "تاریخ اعتبار"))
							{
								break;
							}
							return CustomerMetaKey.CreditDate;
						}
						break;
					case 6:
						switch (key[5])
						{
						case '1':
							if (!(key == "آدرس 1"))
							{
								break;
							}
							return CustomerMetaKey.Address1;
						case '2':
							if (!(key == "آدرس 2"))
							{
								break;
							}
							return CustomerMetaKey.Address2;
						}
						break;
					case 10:
						switch (key[0])
						{
						case 'ت':
							if (!(key == "تاریخ تولد"))
							{
								break;
							}
							return CustomerMetaKey.BirthDate;
						case 'د':
							if (!(key == "دوره ویزیت"))
							{
								break;
							}
							return CustomerMetaKey.VisitPeriod;
						}
						break;
					case 7:
						switch (key[6])
						{
						case '3':
							if (!(key == "تاریخ 3"))
							{
								break;
							}
							return CustomerMetaKey.Date3;
						case '4':
							if (!(key == "تاریخ 4"))
							{
								break;
							}
							return CustomerMetaKey.Date4;
						case '5':
							if (!(key == "تاریخ 5"))
							{
								break;
							}
							return CustomerMetaKey.Date5;
						}
						break;
					case 11:
						if (!(key == "نحوه آشنایی"))
						{
							break;
						}
						return CustomerMetaKey.MethodIfIntroduction;
					case 24:
						if (!(key == "آدرس ذخیره سازی آهنگ زنگ"))
						{
							break;
						}
						return CustomerMetaKey.RingToneAddress;
					case 9:
						if (!(key == "آدرس سایت"))
						{
							break;
						}
						return CustomerMetaKey.SiteUrl;
					case 13:
						if (!(key == "اطلاعات بانکی"))
						{
							break;
						}
						return CustomerMetaKey.BankInfo;
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MetaKey", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\CustomerMetaDataBusiness.cs", 57);
				throw;
			}
			return CustomerMetaKey.None;
		}
		set
		{
			short num = (short)value;
			base.Key = num.ToString();
		}
	}

	public CustomerMetaDataBusiness(string contextGuid, MetaDataBusiness master)
		: base(contextGuid)
	{
		try
		{
			base.EntityGuid = master.EntityGuid;
			base.EntityType = master.EntityType;
			base.Key = master.Key;
			base.Value = master.Value;
			base.DataType = master.DataType;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\CustomerMetaDataBusiness.cs", 24);
		}
	}

	public CustomerMetaDataBusiness(string contextGuid)
		: base(contextGuid)
	{
	}
}
