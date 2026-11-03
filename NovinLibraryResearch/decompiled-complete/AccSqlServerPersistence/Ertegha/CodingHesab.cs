using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;

namespace Ertegha;

public class CodingHesab
{
	private ConcurrentDictionary<Guid, long> _kolDic = new ConcurrentDictionary<Guid, long>();

	public string ContextGuid { get; private set; }

	public CodingHesab(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task<IEnumerable<TafsilBusiness>> GetListTafsilAsync()
	{
		return (await ListTafsilAsync(ContextGuid))?.Where((TafsilBusiness p) => p != null);
	}

	public async Task<IEnumerable<MoeinBusiness>> GetListMoeinAsync()
	{
		return (await GetListMoeinAsync(ContextGuid))?.Where((MoeinBusiness p) => p != null);
	}

	public IEnumerable<KolBusiness> GetListKol(EnSystem system)
	{
		return ListKol(ContextGuid, system)?.Where((KolBusiness p) => p != null);
	}

	public IEnumerable<TafsilBusiness> GetListHazine()
	{
		return ListHazine(ContextGuid)?.Where((TafsilBusiness p) => p != null);
	}

	public List<KolBusiness> ListKol(string contextGuid, EnSystem system)
	{
		return new List<KolBusiness>
		{
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol36, "[موجودی نقد و بانک]".GetTranslate(), ParentDefaults.HesabGroupGuids.DarayiJari, 1100, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29983, "[سپرده های کوتاه مدت]".GetTranslate(), ParentDefaults.HesabGroupGuids.DarayiJari, 1101, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol8, "[اسناد دریافتنی]".GetTranslate(), ParentDefaults.HesabGroupGuids.DarayiJari, 1102, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29974, "[حساب های دریافتنی تجاری]".GetTranslate(), ParentDefaults.HesabGroupGuids.DarayiJari, 1103, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29970, "[سایر حساب های دریافتنی]".GetTranslate(), ParentDefaults.HesabGroupGuids.DarayiJari, 1104, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29961, "[سرمایه گذاری های کوتاه مدت]".GetTranslate(), ParentDefaults.HesabGroupGuids.DarayiJari, 1105, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29958, "[موجودی مواد و کالا]".GetTranslate(), ParentDefaults.HesabGroupGuids.DarayiJari, 1106, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29954, "[پیش پرداختها]".GetTranslate(), ParentDefaults.HesabGroupGuids.DarayiJari, 1107, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol6, "[اموال ، ماشین آلات وتجهیزات]".GetTranslate(), ParentDefaults.HesabGroupGuids.DarayiGheyreJari, 1201, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29909, "[استهلاک انباشته اموال ، ماشین آلات وتجهیزات]".GetTranslate(), ParentDefaults.HesabGroupGuids.DarayiGheyreJari, 1202, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29901, "[زمین]", ParentDefaults.HesabGroupGuids.DarayiGheyreJari, 1203, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29885, "[سرمایه کذاری های بلند مدت]".GetTranslate(), ParentDefaults.HesabGroupGuids.DarayiGheyreJari, 1204, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29878, "[هزینه سنوات آتی (مخارج انتقالی به دوره های آتی)]".GetTranslate(), ParentDefaults.HesabGroupGuids.DarayiGheyreJari, 1205, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29873, "[سایر دارائیها]".GetTranslate(), ParentDefaults.HesabGroupGuids.DarayiGheyreJari, 1206, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29863, "[حساب های پرداختنی تجاری]".GetTranslate(), ParentDefaults.HesabGroupGuids.BedehiJari, 2101, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29857, "[اسناد پرداختنی کوتاه مدت]".GetTranslate(), ParentDefaults.HesabGroupGuids.BedehiJari, 2102, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29856, "[سایر حساب های پرداختنی]".GetTranslate(), ParentDefaults.HesabGroupGuids.BedehiJari, 2103, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29840, "[سپرده های دریافتی]".GetTranslate(), ParentDefaults.HesabGroupGuids.BedehiJari, 2104, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29836, "[پیش دریافتها]".GetTranslate(), ParentDefaults.HesabGroupGuids.BedehiJari, 2105, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29833, "[ذخیره مالیات]".GetTranslate(), ParentDefaults.HesabGroupGuids.BedehiJari, 2106, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29831, "[سود سهام پیشنهادی وپرداختنی]".GetTranslate(), ParentDefaults.HesabGroupGuids.BedehiJari, 2107, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29828, "[تسهیلات و اعتبارات مالی دریافتی]".GetTranslate(), ParentDefaults.HesabGroupGuids.BedehiJari, 2108, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29825, "[سایر ذخایر]".GetTranslate(), ParentDefaults.HesabGroupGuids.BedehiJari, 2109, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29821, "[اسناد پرداختنی بلند مدت]".GetTranslate(), ParentDefaults.HesabGroupGuids.BedehiGheyreJari, 2201, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29819, "[حساب های پرداختنی بلند مدت]".GetTranslate(), ParentDefaults.HesabGroupGuids.BedehiGheyreJari, 2202, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29817, "[تسهیلات مالی دریافتی بلند مدت]".GetTranslate(), ParentDefaults.HesabGroupGuids.BedehiGheyreJari, 2203, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29814, "[ذخیره مزایای پایان خدمت کارکنان]".GetTranslate(), ParentDefaults.HesabGroupGuids.BedehiGheyreJari, 2204, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29812, "[در آمدهای انتفالی به دوره های آتی]".GetTranslate(), ParentDefaults.HesabGroupGuids.BedehiGheyreJari, 2205, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol14, "[سرمایه]".GetTranslate(), ParentDefaults.HesabGroupGuids.HoghooghSahebanSaham, 3101, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29808, "[اندوخته قانونی]".GetTranslate(), ParentDefaults.HesabGroupGuids.HoghooghSahebanSaham, 3102, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29806, "[سایر اندوخته ها وصرف سهام]".GetTranslate(), ParentDefaults.HesabGroupGuids.HoghooghSahebanSaham, 3103, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29800, "[سود (زیان ) انباشته]".GetTranslate(), ParentDefaults.HesabGroupGuids.HoghooghSahebanSaham, 3104, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29797, "[سود (زیان ) جاری]".GetTranslate(), ParentDefaults.HesabGroupGuids.HoghooghSahebanSaham, 3105, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29795, "[تقسیم سود]".GetTranslate(), ParentDefaults.HesabGroupGuids.HoghooghSahebanSaham, 3106, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29779, "[برداشت]".GetTranslate(), ParentDefaults.HesabGroupGuids.HoghooghSahebanSaham, 3107, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29778, "[هزینه های عملیاتی]".GetTranslate(), ParentDefaults.HesabGroupGuids.Hazine, 6101, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29991, "[هزینه های غیر عملیاتی]".GetTranslate(), ParentDefaults.HesabGroupGuids.Hazine, 6102, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29793, "[فروش و مرجوعی]".GetTranslate(), ParentDefaults.HesabGroupGuids.Daramad, 7101, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol19, "[سایر درآمد ها و هزینه های عملیاتی]".GetTranslate(), ParentDefaults.HesabGroupGuids.Daramad, 7102, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29874, "[سایر درآمد ها و هزینه های غیر عملیاتی]".GetTranslate(), ParentDefaults.HesabGroupGuids.Daramad, 7103, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29781, "[قیمت تمام شده کالای فروش رفته]".GetTranslate(), ParentDefaults.HesabGroupGuids.Daramad, 7104, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol18, "[خرید و مرجوعی]".GetTranslate(), ParentDefaults.HesabGroupGuids.Daramad, 7105, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29999, "[حساب های انتظامی]".GetTranslate(), ParentDefaults.HesabGroupGuids.HesabEntezami, 9102, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol29998, "[طرف حساب های انتظامی]".GetTranslate(), ParentDefaults.HesabGroupGuids.TarafHesabEntezami, 9201, st: true, "", ENMahiat.Debit_Credit, system: true, system),
			CreateKolInstance(contextGuid, ParentDefaults.KolCoding.ClsKol39, "[تولید]".GetTranslate(), ParentDefaults.HesabGroupGuids.HesabSanati, 8101, st: true, "", ENMahiat.Debit_Credit, system: true, system)
		};
	}

	private async Task<long> GetKolCode(Guid kolGuid)
	{
		long res = 0L;
		try
		{
			_kolDic.TryGetValue(kolGuid, out res);
			if (res == 0L)
			{
				res = (await KolBusiness.GetAsync(ContextGuid, kolGuid)).Code;
				_kolDic.TryAdd(kolGuid, res);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetKolCode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\CodingHesab.cs", 89);
		}
		return res;
	}

	public async Task<List<MoeinBusiness>> GetListMoeinAsync(string contextGuid)
	{
		List<MoeinBusiness> list = new List<MoeinBusiness>();
		List<MoeinBusiness> list2 = list;
		Guid cLSMoein = ParentDefaults.MoeinCoding.CLSMoein37;
		string translate = "[تنخواه گردانها]".GetTranslate();
		Guid clsKol = ParentDefaults.KolCoding.ClsKol36;
		list2.Add(CreateMoeinInstance(contextGuid, cLSMoein, translate, clsKol, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol36) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list3 = list;
		Guid cLSMoein2 = ParentDefaults.MoeinCoding.CLSMoein4;
		string translate2 = "[موجودی بانکها]".GetTranslate();
		Guid clsKol2 = ParentDefaults.KolCoding.ClsKol36;
		list3.Add(CreateMoeinInstance(contextGuid, cLSMoein2, translate2, clsKol2, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol36) + "105"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list4 = list;
		Guid cLSMoein3 = ParentDefaults.MoeinCoding.CLSMoein7;
		string translate3 = "[موجودی صندوقها]".GetTranslate();
		Guid clsKol3 = ParentDefaults.KolCoding.ClsKol36;
		list4.Add(CreateMoeinInstance(contextGuid, cLSMoein3, translate3, clsKol3, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol36) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list5 = list;
		Guid cLSMoein4 = ParentDefaults.MoeinCoding.CLSMoein29982;
		string translate4 = "[سپرده شرکت در مناقصه ومزایده]".GetTranslate();
		Guid clsKol4 = ParentDefaults.KolCoding.ClsKol29983;
		list5.Add(CreateMoeinInstance(contextGuid, cLSMoein4, translate4, clsKol4, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29983) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list6 = list;
		Guid cLSMoein5 = ParentDefaults.MoeinCoding.CLSMoein29981;
		string translate5 = "[سپرده های گمرکی]".GetTranslate();
		Guid clsKol5 = ParentDefaults.KolCoding.ClsKol29983;
		list6.Add(CreateMoeinInstance(contextGuid, cLSMoein5, translate5, clsKol5, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29983) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list7 = list;
		Guid cLSMoein6 = ParentDefaults.MoeinCoding.CLSMoein29980;
		string translate6 = "[سپرده ضمانتنامه های بانکی]".GetTranslate();
		Guid clsKol6 = ParentDefaults.KolCoding.ClsKol29983;
		list7.Add(CreateMoeinInstance(contextGuid, cLSMoein6, translate6, clsKol6, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29983) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list8 = list;
		Guid cLSMoein7 = ParentDefaults.MoeinCoding.CLSMoein29979;
		string translate7 = "[سایر سپرده ها]".GetTranslate();
		Guid clsKol7 = ParentDefaults.KolCoding.ClsKol29983;
		list8.Add(CreateMoeinInstance(contextGuid, cLSMoein7, translate7, clsKol7, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29983) + "104"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list9 = list;
		Guid cLSMoein8 = ParentDefaults.MoeinCoding.CLSMoein29978;
		string translate8 = "[چکهای در جریان وصول]".GetTranslate();
		Guid clsKol8 = ParentDefaults.KolCoding.ClsKol8;
		list9.Add(CreateMoeinInstance(contextGuid, cLSMoein8, translate8, clsKol8, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol8) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list10 = list;
		Guid cLSMoein9 = ParentDefaults.MoeinCoding.CLSMoein29976;
		string translate9 = "[اسناد دریافتنی نزد صندوق]".GetTranslate();
		Guid clsKol9 = ParentDefaults.KolCoding.ClsKol8;
		list10.Add(CreateMoeinInstance(contextGuid, cLSMoein9, translate9, clsKol9, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol8) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list11 = list;
		Guid cLSMoein10 = ParentDefaults.MoeinCoding.CLSMoein9;
		string translate10 = "[اشخاص (حساب های دریافتنی)]".GetTranslate();
		Guid clsKol10 = ParentDefaults.KolCoding.ClsKol29974;
		list11.Add(CreateMoeinInstance(contextGuid, cLSMoein10, translate10, clsKol10, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29974) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list12 = list;
		Guid cLSMoein11 = ParentDefaults.MoeinCoding.CLSMoein29973;
		string translate11 = "[شرکتها (حساب های دریافتنی)]".GetTranslate();
		Guid clsKol11 = ParentDefaults.KolCoding.ClsKol29974;
		list12.Add(CreateMoeinInstance(contextGuid, cLSMoein11, translate11, clsKol11, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29974) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list13 = list;
		Guid cLSMoein12 = ParentDefaults.MoeinCoding.CLSMoein29972;
		string translate12 = "[ذخیره مطالبات مشکوک الوصول]".GetTranslate();
		Guid clsKol12 = ParentDefaults.KolCoding.ClsKol29974;
		list13.Add(CreateMoeinInstance(contextGuid, cLSMoein12, translate12, clsKol12, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29974) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list14 = list;
		Guid cLSMoein13 = ParentDefaults.MoeinCoding.CLSMoein29971;
		string translate13 = "[اسناد واخواست شده]".GetTranslate();
		Guid clsKol13 = ParentDefaults.KolCoding.ClsKol29974;
		list14.Add(CreateMoeinInstance(contextGuid, cLSMoein13, translate13, clsKol13, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29974) + "104"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list15 = list;
		Guid cLSMoein14 = ParentDefaults.MoeinCoding.CLSMoein29969;
		string translate14 = "[مساعده حقوق]".GetTranslate();
		Guid clsKol14 = ParentDefaults.KolCoding.ClsKol29970;
		list15.Add(CreateMoeinInstance(contextGuid, cLSMoein14, translate14, clsKol14, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29970) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list16 = list;
		Guid cLSMoein15 = ParentDefaults.MoeinCoding.CLSMoein29968;
		string translate15 = "[وام ضروری کارکنان]".GetTranslate();
		Guid clsKol15 = ParentDefaults.KolCoding.ClsKol29970;
		list16.Add(CreateMoeinInstance(contextGuid, cLSMoein15, translate15, clsKol15, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29970) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list17 = list;
		Guid cLSMoein16 = ParentDefaults.MoeinCoding.CLSMoein29967;
		string translate16 = "[وام مسکن کارکنان]".GetTranslate();
		Guid clsKol16 = ParentDefaults.KolCoding.ClsKol29970;
		list17.Add(CreateMoeinInstance(contextGuid, cLSMoein16, translate16, clsKol16, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29970) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list18 = list;
		Guid cLSMoein17 = ParentDefaults.MoeinCoding.CLSMoein29966;
		string translate17 = "[جاری کارکنان]".GetTranslate();
		Guid clsKol17 = ParentDefaults.KolCoding.ClsKol29970;
		list18.Add(CreateMoeinInstance(contextGuid, cLSMoein17, translate17, clsKol17, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29970) + "104"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list19 = list;
		Guid cLSMoein18 = ParentDefaults.MoeinCoding.CLSMoein29962;
		string translate18 = "[سایر حسابها واسناد دریافتنی]".GetTranslate();
		Guid clsKol18 = ParentDefaults.KolCoding.ClsKol29970;
		list19.Add(CreateMoeinInstance(contextGuid, cLSMoein18, translate18, clsKol18, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29970) + "108"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list20 = list;
		Guid cLSMoein19 = ParentDefaults.MoeinCoding.CLSMoein29960;
		string translate19 = "[سپرده های کوتاه مدت نزد بانکها]".GetTranslate();
		Guid clsKol19 = ParentDefaults.KolCoding.ClsKol29961;
		list20.Add(CreateMoeinInstance(contextGuid, cLSMoein19, translate19, clsKol19, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29961) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list21 = list;
		Guid cLSMoein20 = ParentDefaults.MoeinCoding.CLSMoein29959;
		string translate20 = "[سهام سایر شرکتها]".GetTranslate();
		Guid clsKol20 = ParentDefaults.KolCoding.ClsKol29961;
		list21.Add(CreateMoeinInstance(contextGuid, cLSMoein20, translate20, clsKol20, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29961) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list22 = list;
		Guid cLSMoein21 = ParentDefaults.MoeinCoding.CLSMoein5;
		string translate21 = "[موجودی ابتدای دوره کالاها]".GetTranslate();
		Guid clsKol21 = ParentDefaults.KolCoding.ClsKol29958;
		list22.Add(CreateMoeinInstance(contextGuid, cLSMoein21, translate21, clsKol21, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29958) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		list.Add(CreateMoeinInstance(contextGuid, ParentDefaults.MoeinCoding.CLSMoeinProductEnd, "[موجودی پایان دوره کالاها]".GetTranslate(), ParentDefaults.KolCoding.ClsKol29958, 0L, st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list23 = list;
		Guid cLSMoein22 = ParentDefaults.MoeinCoding.CLSMoein50;
		string translate22 = "[موجودی کالا]".GetTranslate();
		Guid clsKol22 = ParentDefaults.KolCoding.ClsKol29958;
		list23.Add(CreateMoeinInstance(contextGuid, cLSMoein22, translate22, clsKol22, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29958) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list24 = list;
		Guid cLSMoein23 = ParentDefaults.MoeinCoding.CLSMoein29957;
		string translate23 = "[کالای امانی ما نزد دیگران]".GetTranslate();
		Guid clsKol23 = ParentDefaults.KolCoding.ClsKol29958;
		list24.Add(CreateMoeinInstance(contextGuid, cLSMoein23, translate23, clsKol23, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29958) + "105"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list25 = list;
		Guid cLSMoein24 = ParentDefaults.MoeinCoding.CLSMoein29953;
		string translate24 = "[پیش پرداخت خرید مواد اولیه]".GetTranslate();
		Guid clsKol24 = ParentDefaults.KolCoding.ClsKol29954;
		list25.Add(CreateMoeinInstance(contextGuid, cLSMoein24, translate24, clsKol24, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29954) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list26 = list;
		Guid cLSMoein25 = ParentDefaults.MoeinCoding.CLSMoein29952;
		string translate25 = "[پیش پرداخت خرید اموال ماشین آلات و تجهیزات]".GetTranslate();
		Guid clsKol25 = ParentDefaults.KolCoding.ClsKol29954;
		list26.Add(CreateMoeinInstance(contextGuid, cLSMoein25, translate25, clsKol25, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29954) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list27 = list;
		Guid cLSMoein26 = ParentDefaults.MoeinCoding.CLSMoein29951;
		string translate26 = "[پیش پرداخت خرید قطعات و لوازم یدکی]".GetTranslate();
		Guid clsKol26 = ParentDefaults.KolCoding.ClsKol29954;
		list27.Add(CreateMoeinInstance(contextGuid, cLSMoein26, translate26, clsKol26, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29954) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list28 = list;
		Guid cLSMoein27 = ParentDefaults.MoeinCoding.CLSMoein29950;
		string translate27 = "[پیش پرداخت هزینه های جاری]".GetTranslate();
		Guid clsKol27 = ParentDefaults.KolCoding.ClsKol29954;
		list28.Add(CreateMoeinInstance(contextGuid, cLSMoein27, translate27, clsKol27, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29954) + "104"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list29 = list;
		Guid cLSMoein28 = ParentDefaults.MoeinCoding.CLSMoein29949;
		string translate28 = "[پیش پرداخت خرید خدمات]".GetTranslate();
		Guid clsKol28 = ParentDefaults.KolCoding.ClsKol29954;
		list29.Add(CreateMoeinInstance(contextGuid, cLSMoein28, translate28, clsKol28, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29954) + "105"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list30 = list;
		Guid cLSMoein29 = ParentDefaults.MoeinCoding.CLSMoein29948;
		string translate29 = "[پیش پرداخت بیمه دارائیها]".GetTranslate();
		Guid clsKol29 = ParentDefaults.KolCoding.ClsKol29954;
		list30.Add(CreateMoeinInstance(contextGuid, cLSMoein29, translate29, clsKol29, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29954) + "106"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list31 = list;
		Guid cLSMoein30 = ParentDefaults.MoeinCoding.CLSMoein29947;
		string translate30 = "[پیش پرداخت حسابرسی]".GetTranslate();
		Guid clsKol30 = ParentDefaults.KolCoding.ClsKol29954;
		list31.Add(CreateMoeinInstance(contextGuid, cLSMoein30, translate30, clsKol30, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29954) + "107"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list32 = list;
		Guid cLSMoein31 = ParentDefaults.MoeinCoding.CLSMoein29946;
		string translate31 = "[پیش پرداخت مالیات]".GetTranslate();
		Guid clsKol31 = ParentDefaults.KolCoding.ClsKol29954;
		list32.Add(CreateMoeinInstance(contextGuid, cLSMoein31, translate31, clsKol31, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29954) + "108"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list33 = list;
		Guid cLSMoein32 = ParentDefaults.MoeinCoding.CLSMoein29945;
		string translate32 = "[پیش پرداخت هزینه های تحقیق وتوسعه]".GetTranslate();
		Guid clsKol32 = ParentDefaults.KolCoding.ClsKol29954;
		list33.Add(CreateMoeinInstance(contextGuid, cLSMoein32, translate32, clsKol32, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29954) + "109"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list34 = list;
		Guid cLSMoein33 = ParentDefaults.MoeinCoding.CLSMoein29944;
		string translate33 = "[پیش پرداخت اجاره]".GetTranslate();
		Guid clsKol33 = ParentDefaults.KolCoding.ClsKol29954;
		list34.Add(CreateMoeinInstance(contextGuid, cLSMoein33, translate33, clsKol33, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29954) + "110"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list35 = list;
		Guid cLSMoein34 = ParentDefaults.MoeinCoding.CLSMoein29943;
		string translate34 = "[پیش پرداخت هزینه سود و کارمزد وامها]".GetTranslate();
		Guid clsKol34 = ParentDefaults.KolCoding.ClsKol29954;
		list35.Add(CreateMoeinInstance(contextGuid, cLSMoein34, translate34, clsKol34, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29954) + "111"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list36 = list;
		Guid cLSMoein35 = ParentDefaults.MoeinCoding.CLSMoein29942;
		string translate35 = "[پیش پرداخت به پیمانکاران]".GetTranslate();
		Guid clsKol35 = ParentDefaults.KolCoding.ClsKol29954;
		list36.Add(CreateMoeinInstance(contextGuid, cLSMoein35, translate35, clsKol35, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29954) + "112"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list37 = list;
		Guid cLSMoein36 = ParentDefaults.MoeinCoding.CLSMoein27;
		string translate36 = "[ساختمانها]".GetTranslate();
		Guid clsKol36 = ParentDefaults.KolCoding.ClsKol6;
		list37.Add(CreateMoeinInstance(contextGuid, cLSMoein36, translate36, clsKol36, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol6) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list38 = list;
		Guid cLSMoein37 = ParentDefaults.MoeinCoding.CLSMoein29915;
		string translate37 = "[تاسیسات]".GetTranslate();
		Guid clsKol37 = ParentDefaults.KolCoding.ClsKol6;
		list38.Add(CreateMoeinInstance(contextGuid, cLSMoein37, translate37, clsKol37, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol6) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list39 = list;
		Guid cLSMoein38 = ParentDefaults.MoeinCoding.CLSMoein25;
		string translate38 = "[ماشین آلات و تجهیزات]".GetTranslate();
		Guid clsKol38 = ParentDefaults.KolCoding.ClsKol6;
		list39.Add(CreateMoeinInstance(contextGuid, cLSMoein38, translate38, clsKol38, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol6) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list40 = list;
		Guid cLSMoein39 = ParentDefaults.MoeinCoding.CLSMoein29914;
		string translate39 = "[ابزار آلات و قالبها]".GetTranslate();
		Guid clsKol39 = ParentDefaults.KolCoding.ClsKol6;
		list40.Add(CreateMoeinInstance(contextGuid, cLSMoein39, translate39, clsKol39, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol6) + "104"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list41 = list;
		Guid cLSMoein40 = ParentDefaults.MoeinCoding.CLSMoein29913;
		string translate40 = "[وسائط نقلیه]".GetTranslate();
		Guid clsKol40 = ParentDefaults.KolCoding.ClsKol6;
		list41.Add(CreateMoeinInstance(contextGuid, cLSMoein40, translate40, clsKol40, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol6) + "105"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list42 = list;
		Guid cLSMoein41 = ParentDefaults.MoeinCoding.CLSMoein24;
		string translate41 = "[اثاثیه و منصوبات]".GetTranslate();
		Guid clsKol41 = ParentDefaults.KolCoding.ClsKol6;
		list42.Add(CreateMoeinInstance(contextGuid, cLSMoein41, translate41, clsKol41, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol6) + "106"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list43 = list;
		Guid cLSMoein42 = ParentDefaults.MoeinCoding.CLSMoein29912;
		string translate42 = "[لوازم ازمایشگاهی]".GetTranslate();
		Guid clsKol42 = ParentDefaults.KolCoding.ClsKol6;
		list43.Add(CreateMoeinInstance(contextGuid, cLSMoein42, translate42, clsKol42, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol6) + "107"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list44 = list;
		Guid cLSMoein43 = ParentDefaults.MoeinCoding.CLSMoein29911;
		string translate43 = "[کنترل دارائیها - تنخواه]".GetTranslate();
		Guid clsKol43 = ParentDefaults.KolCoding.ClsKol6;
		list44.Add(CreateMoeinInstance(contextGuid, cLSMoein43, translate43, clsKol43, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol6) + "108"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list45 = list;
		Guid cLSMoein44 = ParentDefaults.MoeinCoding.CLSMoein29910;
		string translate44 = "[کنترل دارائیها - انبار]".GetTranslate();
		Guid clsKol44 = ParentDefaults.KolCoding.ClsKol6;
		list45.Add(CreateMoeinInstance(contextGuid, cLSMoein44, translate44, clsKol44, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol6) + "109"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list46 = list;
		Guid cLSMoein45 = ParentDefaults.MoeinCoding.CLSMoein29908;
		string translate45 = "[استهلاک انباشته ساختمانها]".GetTranslate();
		Guid clsKol45 = ParentDefaults.KolCoding.ClsKol29909;
		list46.Add(CreateMoeinInstance(contextGuid, cLSMoein45, translate45, clsKol45, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29909) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list47 = list;
		Guid cLSMoein46 = ParentDefaults.MoeinCoding.CLSMoein29907;
		string translate46 = "[استهلاک انباشته تاسیسات]".GetTranslate();
		Guid clsKol46 = ParentDefaults.KolCoding.ClsKol29909;
		list47.Add(CreateMoeinInstance(contextGuid, cLSMoein46, translate46, clsKol46, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29909) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list48 = list;
		Guid cLSMoein47 = ParentDefaults.MoeinCoding.CLSMoein29906;
		string translate47 = "[استهلاک انباشته ماشین آلات و تجهیزات]".GetTranslate();
		Guid clsKol47 = ParentDefaults.KolCoding.ClsKol29909;
		list48.Add(CreateMoeinInstance(contextGuid, cLSMoein47, translate47, clsKol47, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29909) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list49 = list;
		Guid cLSMoein48 = ParentDefaults.MoeinCoding.CLSMoein29905;
		string translate48 = "[استهلاک انباشته ابزار آلات و قالبها]".GetTranslate();
		Guid clsKol48 = ParentDefaults.KolCoding.ClsKol29909;
		list49.Add(CreateMoeinInstance(contextGuid, cLSMoein48, translate48, clsKol48, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29909) + "104"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list50 = list;
		Guid cLSMoein49 = ParentDefaults.MoeinCoding.CLSMoein29904;
		string translate49 = "[استهلاک انباشته وسائط نقلیه]".GetTranslate();
		Guid clsKol49 = ParentDefaults.KolCoding.ClsKol29909;
		list50.Add(CreateMoeinInstance(contextGuid, cLSMoein49, translate49, clsKol49, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29909) + "105"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list51 = list;
		Guid cLSMoein50 = ParentDefaults.MoeinCoding.CLSMoein29903;
		string translate50 = "[استهلاک انباشته اثاثیه و منصوبات]".GetTranslate();
		Guid clsKol50 = ParentDefaults.KolCoding.ClsKol29909;
		list51.Add(CreateMoeinInstance(contextGuid, cLSMoein50, translate50, clsKol50, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29909) + "106"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list52 = list;
		Guid cLSMoein51 = ParentDefaults.MoeinCoding.CLSMoein29902;
		string translate51 = "[استهلاک انباشته لوازم آزمایشگاهی]".GetTranslate();
		Guid clsKol51 = ParentDefaults.KolCoding.ClsKol29909;
		list52.Add(CreateMoeinInstance(contextGuid, cLSMoein51, translate51, clsKol51, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29909) + "107"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list53 = list;
		Guid cLSMoein52 = ParentDefaults.MoeinCoding.CLSMoein26;
		string translate52 = "[زمین]".GetTranslate();
		Guid clsKol52 = ParentDefaults.KolCoding.ClsKol29901;
		list53.Add(CreateMoeinInstance(contextGuid, cLSMoein52, translate52, clsKol52, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29901) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list54 = list;
		Guid cLSMoein53 = ParentDefaults.MoeinCoding.CLSMoein29890;
		string translate53 = "[دارائیهای ثابت در جریان تکمیل]".GetTranslate();
		Guid clsKol53 = ParentDefaults.KolCoding.ClsKol29901;
		list54.Add(CreateMoeinInstance(contextGuid, cLSMoein53, translate53, clsKol53, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29901) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list55 = list;
		Guid cLSMoein54 = ParentDefaults.MoeinCoding.CLSMoein29889;
		string translate54 = "[ساختمانهای در دست تکمیل]".GetTranslate();
		Guid clsKol54 = ParentDefaults.KolCoding.ClsKol29901;
		list55.Add(CreateMoeinInstance(contextGuid, cLSMoein54, translate54, clsKol54, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29901) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list56 = list;
		Guid cLSMoein55 = ParentDefaults.MoeinCoding.CLSMoein29888;
		string translate55 = "[تاسیسات در دست تکمیل]".GetTranslate();
		Guid clsKol55 = ParentDefaults.KolCoding.ClsKol29901;
		list56.Add(CreateMoeinInstance(contextGuid, cLSMoein55, translate55, clsKol55, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29901) + "104"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list57 = list;
		Guid cLSMoein56 = ParentDefaults.MoeinCoding.CLSMoein29887;
		string translate56 = "[ماشین آلات در دست تکمیل]".GetTranslate();
		Guid clsKol56 = ParentDefaults.KolCoding.ClsKol29901;
		list57.Add(CreateMoeinInstance(contextGuid, cLSMoein56, translate56, clsKol56, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29901) + "105"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list58 = list;
		Guid cLSMoein57 = ParentDefaults.MoeinCoding.CLSMoein29886;
		string translate57 = "[ابزار آلات و قالبهای در دست تکمیل]".GetTranslate();
		Guid clsKol57 = ParentDefaults.KolCoding.ClsKol29901;
		list58.Add(CreateMoeinInstance(contextGuid, cLSMoein57, translate57, clsKol57, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29901) + "106"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list59 = list;
		Guid cLSMoein58 = ParentDefaults.MoeinCoding.CLSMoein29884;
		string translate58 = "[سرمایه گذاری در شرکتهای تابعه]".GetTranslate();
		Guid clsKol58 = ParentDefaults.KolCoding.ClsKol29885;
		list59.Add(CreateMoeinInstance(contextGuid, cLSMoein58, translate58, clsKol58, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29885) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list60 = list;
		Guid cLSMoein59 = ParentDefaults.MoeinCoding.CLSMoein29883;
		string translate59 = "[سرمایه گذاری در شرکتهای وابسته]".GetTranslate();
		Guid clsKol59 = ParentDefaults.KolCoding.ClsKol29885;
		list60.Add(CreateMoeinInstance(contextGuid, cLSMoein59, translate59, clsKol59, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29885) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list61 = list;
		Guid cLSMoein60 = ParentDefaults.MoeinCoding.CLSMoein29882;
		string translate60 = "[سهام سایر شرکتها]".GetTranslate();
		Guid clsKol60 = ParentDefaults.KolCoding.ClsKol29885;
		list61.Add(CreateMoeinInstance(contextGuid, cLSMoein60, translate60, clsKol60, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29885) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list62 = list;
		Guid cLSMoein61 = ParentDefaults.MoeinCoding.CLSMoein29881;
		string translate61 = "[سپرده هاو مطالبات بلند مدت]".GetTranslate();
		Guid clsKol61 = ParentDefaults.KolCoding.ClsKol29885;
		list62.Add(CreateMoeinInstance(contextGuid, cLSMoein61, translate61, clsKol61, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29885) + "104"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list63 = list;
		Guid cLSMoein62 = ParentDefaults.MoeinCoding.CLSMoein29880;
		string translate62 = "[وامهای اعطایی بلند مدت]".GetTranslate();
		Guid clsKol62 = ParentDefaults.KolCoding.ClsKol29885;
		list63.Add(CreateMoeinInstance(contextGuid, cLSMoein62, translate62, clsKol62, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29885) + "105"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list64 = list;
		Guid cLSMoein63 = ParentDefaults.MoeinCoding.CLSMoein29879;
		string translate63 = "[سپرده های بلند مدت نزد بانکها]".GetTranslate();
		Guid clsKol63 = ParentDefaults.KolCoding.ClsKol29885;
		list64.Add(CreateMoeinInstance(contextGuid, cLSMoein63, translate63, clsKol63, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29885) + "106"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list65 = list;
		Guid cLSMoein64 = ParentDefaults.MoeinCoding.CLSMoein29877;
		string translate64 = "[هزینه های تاسیس و قبل از بهره برداری]".GetTranslate();
		Guid clsKol64 = ParentDefaults.KolCoding.ClsKol29878;
		list65.Add(CreateMoeinInstance(contextGuid, cLSMoein64, translate64, clsKol64, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29878) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list66 = list;
		Guid cLSMoein65 = ParentDefaults.MoeinCoding.CLSMoein29876;
		string translate65 = "[هزینه سود و کارمزد وامهای بلند مدت]".GetTranslate();
		Guid clsKol65 = ParentDefaults.KolCoding.ClsKol29878;
		list66.Add(CreateMoeinInstance(contextGuid, cLSMoein65, translate65, clsKol65, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29878) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list67 = list;
		Guid cLSMoein66 = ParentDefaults.MoeinCoding.CLSMoein29875;
		string translate66 = "[هزینه های تحقیق و توسعه]".GetTranslate();
		Guid clsKol66 = ParentDefaults.KolCoding.ClsKol29878;
		list67.Add(CreateMoeinInstance(contextGuid, cLSMoein66, translate66, clsKol66, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29878) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list68 = list;
		Guid cLSMoein67 = ParentDefaults.MoeinCoding.CLSMoein29872;
		string translate67 = "[حق الامتیازها]".GetTranslate();
		Guid clsKol67 = ParentDefaults.KolCoding.ClsKol29873;
		list68.Add(CreateMoeinInstance(contextGuid, cLSMoein67, translate67, clsKol67, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29873) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list69 = list;
		Guid cLSMoein68 = ParentDefaults.MoeinCoding.CLSMoein29871;
		string translate68 = "[سرقفلی]".GetTranslate();
		Guid clsKol68 = ParentDefaults.KolCoding.ClsKol29873;
		list69.Add(CreateMoeinInstance(contextGuid, cLSMoein68, translate68, clsKol68, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29873) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list70 = list;
		Guid cLSMoein69 = ParentDefaults.MoeinCoding.CLSMoein29870;
		string translate69 = "[ودیعه آب]".GetTranslate();
		Guid clsKol69 = ParentDefaults.KolCoding.ClsKol29873;
		list70.Add(CreateMoeinInstance(contextGuid, cLSMoein69, translate69, clsKol69, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29873) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list71 = list;
		Guid cLSMoein70 = ParentDefaults.MoeinCoding.CLSMoein29869;
		string translate70 = "[ودیعه برق]".GetTranslate();
		Guid clsKol70 = ParentDefaults.KolCoding.ClsKol29873;
		list71.Add(CreateMoeinInstance(contextGuid, cLSMoein70, translate70, clsKol70, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29873) + "104"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list72 = list;
		Guid cLSMoein71 = ParentDefaults.MoeinCoding.CLSMoein29868;
		string translate71 = "[ودیعه تلفن]".GetTranslate();
		Guid clsKol71 = ParentDefaults.KolCoding.ClsKol29873;
		list72.Add(CreateMoeinInstance(contextGuid, cLSMoein71, translate71, clsKol71, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29873) + "105"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list73 = list;
		Guid cLSMoein72 = ParentDefaults.MoeinCoding.CLSMoein29867;
		string translate72 = "[ودیعه گاز]".GetTranslate();
		Guid clsKol72 = ParentDefaults.KolCoding.ClsKol29873;
		list73.Add(CreateMoeinInstance(contextGuid, cLSMoein72, translate72, clsKol72, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29873) + "106"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list74 = list;
		Guid cLSMoein73 = ParentDefaults.MoeinCoding.CLSMoein29866;
		string translate73 = "[سایر ودایع]".GetTranslate();
		Guid clsKol73 = ParentDefaults.KolCoding.ClsKol29873;
		list74.Add(CreateMoeinInstance(contextGuid, cLSMoein73, translate73, clsKol73, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29873) + "107"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list75 = list;
		Guid cLSMoein74 = ParentDefaults.MoeinCoding.CLSMoein29865;
		string translate74 = "[نرم افزارها]".GetTranslate();
		Guid clsKol74 = ParentDefaults.KolCoding.ClsKol29873;
		list75.Add(CreateMoeinInstance(contextGuid, cLSMoein74, translate74, clsKol74, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29873) + "108"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list76 = list;
		Guid cLSMoein75 = ParentDefaults.MoeinCoding.CLSMoein28;
		string translate75 = "[اشخاص (حساب های پرداختنی)]".GetTranslate();
		Guid clsKol75 = ParentDefaults.KolCoding.ClsKol29863;
		list76.Add(CreateMoeinInstance(contextGuid, cLSMoein75, translate75, clsKol75, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29863) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list77 = list;
		Guid cLSMoein76 = ParentDefaults.MoeinCoding.CLSMoein29861;
		string translate76 = "[شرکتها (حساب های پرداختنی)]".GetTranslate();
		Guid clsKol76 = ParentDefaults.KolCoding.ClsKol29863;
		list77.Add(CreateMoeinInstance(contextGuid, cLSMoein76, translate76, clsKol76, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29863) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list78 = list;
		Guid cLSMoein77 = ParentDefaults.MoeinCoding.CLSMoein29860;
		string translate77 = "[کالای امانی دیگران نزد ما]".GetTranslate();
		Guid clsKol77 = ParentDefaults.KolCoding.ClsKol29863;
		list78.Add(CreateMoeinInstance(contextGuid, cLSMoein77, translate77, clsKol77, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29863) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list79 = list;
		Guid cLSMoein78 = ParentDefaults.MoeinCoding.CLSMoein29859;
		string translate78 = "[معلق خرید]".GetTranslate();
		Guid clsKol78 = ParentDefaults.KolCoding.ClsKol29863;
		list79.Add(CreateMoeinInstance(contextGuid, cLSMoein78, translate78, clsKol78, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29863) + "104"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list80 = list;
		Guid cLSMoein79 = ParentDefaults.MoeinCoding.CLSMoein29858;
		string translate79 = "[سایر حساب های پرداختنی تجاری]".GetTranslate();
		Guid clsKol79 = ParentDefaults.KolCoding.ClsKol29863;
		list80.Add(CreateMoeinInstance(contextGuid, cLSMoein79, translate79, clsKol79, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29863) + "105"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list81 = list;
		Guid cLSMoein80 = ParentDefaults.MoeinCoding.CLSMoein10;
		string translate80 = "[اسناد پرداختنی کوتاه مدت]".GetTranslate();
		Guid clsKol80 = ParentDefaults.KolCoding.ClsKol29857;
		list81.Add(CreateMoeinInstance(contextGuid, cLSMoein80, translate80, clsKol80, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29857) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list82 = list;
		Guid cLSMoein81 = ParentDefaults.MoeinCoding.CLSMoein29855;
		string translate81 = "[مالیات بر در آمد شرکت]".GetTranslate();
		Guid clsKol81 = ParentDefaults.KolCoding.ClsKol29856;
		list82.Add(CreateMoeinInstance(contextGuid, cLSMoein81, translate81, clsKol81, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29856) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list83 = list;
		Guid cLSMoein82 = ParentDefaults.MoeinCoding.CLSMoein29854;
		string translate82 = "[مالیات حقوق و دستمزد کارکنان]".GetTranslate();
		Guid clsKol82 = ParentDefaults.KolCoding.ClsKol29856;
		list83.Add(CreateMoeinInstance(contextGuid, cLSMoein82, translate82, clsKol82, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29856) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list84 = list;
		Guid cLSMoein83 = ParentDefaults.MoeinCoding.CLSMoein29853;
		string translate83 = "[مالیات تکلیفی اشخاص ثالث]".GetTranslate();
		Guid clsKol83 = ParentDefaults.KolCoding.ClsKol29856;
		list84.Add(CreateMoeinInstance(contextGuid, cLSMoein83, translate83, clsKol83, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29856) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list85 = list;
		Guid cLSMoein84 = ParentDefaults.MoeinCoding.CLSMoein29852;
		string translate84 = "[سازمان تامین اجتماعی – حق بیمه کارکنان]".GetTranslate();
		Guid clsKol84 = ParentDefaults.KolCoding.ClsKol29856;
		list85.Add(CreateMoeinInstance(contextGuid, cLSMoein84, translate84, clsKol84, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29856) + "104"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list86 = list;
		Guid cLSMoein85 = ParentDefaults.MoeinCoding.CLSMoein29851;
		string translate85 = "[سازمان تامین اجتماعی – حق بیمه اشخاص ثالث]".GetTranslate();
		Guid clsKol85 = ParentDefaults.KolCoding.ClsKol29856;
		list86.Add(CreateMoeinInstance(contextGuid, cLSMoein85, translate85, clsKol85, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29856) + "105"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list87 = list;
		Guid cLSMoein86 = ParentDefaults.MoeinCoding.CLSMoein29850;
		string translate86 = "[بیمه عمر و حوادث کارکنان]".GetTranslate();
		Guid clsKol86 = ParentDefaults.KolCoding.ClsKol29856;
		list87.Add(CreateMoeinInstance(contextGuid, cLSMoein86, translate86, clsKol86, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29856) + "106"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list88 = list;
		Guid cLSMoein87 = ParentDefaults.MoeinCoding.CLSMoein29849;
		string translate87 = "[صندوق کارآموزی]".GetTranslate();
		Guid clsKol87 = ParentDefaults.KolCoding.ClsKol29856;
		list88.Add(CreateMoeinInstance(contextGuid, cLSMoein87, translate87, clsKol87, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29856) + "107"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list89 = list;
		Guid cLSMoein88 = ParentDefaults.MoeinCoding.CLSMoein29848;
		string translate88 = "[حقوق ودستمزد پرداختنی]".GetTranslate();
		Guid clsKol88 = ParentDefaults.KolCoding.ClsKol29856;
		list89.Add(CreateMoeinInstance(contextGuid, cLSMoein88, translate88, clsKol88, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29856) + "108"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list90 = list;
		Guid cLSMoein89 = ParentDefaults.MoeinCoding.CLSMoein29847;
		string translate89 = "[پیمانکاران]".GetTranslate();
		Guid clsKol89 = ParentDefaults.KolCoding.ClsKol29856;
		list90.Add(CreateMoeinInstance(contextGuid, cLSMoein89, translate89, clsKol89, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29856) + "109"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list91 = list;
		Guid cLSMoein90 = ParentDefaults.MoeinCoding.CLSMoein29846;
		string translate90 = "[عیدی و پاداش پرداختنی]".GetTranslate();
		Guid clsKol90 = ParentDefaults.KolCoding.ClsKol29856;
		list91.Add(CreateMoeinInstance(contextGuid, cLSMoein90, translate90, clsKol90, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29856) + "110"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list92 = list;
		Guid cLSMoein91 = ParentDefaults.MoeinCoding.CLSMoein29845;
		string translate91 = "[بهره وری تولید پرداختنی]".GetTranslate();
		Guid clsKol91 = ParentDefaults.KolCoding.ClsKol29856;
		list92.Add(CreateMoeinInstance(contextGuid, cLSMoein91, translate91, clsKol91, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29856) + "111"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list93 = list;
		Guid cLSMoein92 = ParentDefaults.MoeinCoding.CLSMoein29844;
		string translate92 = "[هزینه های حسابرسی پرداختنی]".GetTranslate();
		Guid clsKol92 = ParentDefaults.KolCoding.ClsKol29856;
		list93.Add(CreateMoeinInstance(contextGuid, cLSMoein92, translate92, clsKol92, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29856) + "112"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list94 = list;
		Guid cLSMoein93 = ParentDefaults.MoeinCoding.CLSMoein29843;
		string translate93 = "[عوارض شهرداری]".GetTranslate();
		Guid clsKol93 = ParentDefaults.KolCoding.ClsKol29856;
		list94.Add(CreateMoeinInstance(contextGuid, cLSMoein93, translate93, clsKol93, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29856) + "113"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list95 = list;
		Guid cLSMoein94 = ParentDefaults.MoeinCoding.CLSMoein29842;
		string translate94 = "[هزینه های پرداختنی]".GetTranslate();
		Guid clsKol94 = ParentDefaults.KolCoding.ClsKol29856;
		list95.Add(CreateMoeinInstance(contextGuid, cLSMoein94, translate94, clsKol94, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29856) + "114"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list96 = list;
		Guid cLSMoein95 = ParentDefaults.MoeinCoding.CLSMoein29841;
		string translate95 = "[روند حقوق]".GetTranslate();
		Guid clsKol95 = ParentDefaults.KolCoding.ClsKol29856;
		list96.Add(CreateMoeinInstance(contextGuid, cLSMoein95, translate95, clsKol95, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29856) + "115"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list97 = list;
		Guid cLSMoein96 = ParentDefaults.MoeinCoding.CLSMoein49;
		string translate96 = "[مالیات بر ارزش افزوده]".GetTranslate();
		Guid clsKol96 = ParentDefaults.KolCoding.ClsKol29856;
		list97.Add(CreateMoeinInstance(contextGuid, cLSMoein96, translate96, clsKol96, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29856) + "116"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list98 = list;
		Guid cLSMoein97 = ParentDefaults.MoeinCoding.CLSMoein29839;
		string translate97 = "[سپرده های دریافتی مزایده و مناقصه]".GetTranslate();
		Guid clsKol97 = ParentDefaults.KolCoding.ClsKol29840;
		list98.Add(CreateMoeinInstance(contextGuid, cLSMoein97, translate97, clsKol97, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29840) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list99 = list;
		Guid cLSMoein98 = ParentDefaults.MoeinCoding.CLSMoein29838;
		string translate98 = "[سپرده های دریافتی حسن انجام کار]".GetTranslate();
		Guid clsKol98 = ParentDefaults.KolCoding.ClsKol29840;
		list99.Add(CreateMoeinInstance(contextGuid, cLSMoein98, translate98, clsKol98, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29840) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list100 = list;
		Guid cLSMoein99 = ParentDefaults.MoeinCoding.CLSMoein29837;
		string translate99 = "[سایر سپرده های دریافتی]".GetTranslate();
		Guid clsKol99 = ParentDefaults.KolCoding.ClsKol29840;
		list100.Add(CreateMoeinInstance(contextGuid, cLSMoein99, translate99, clsKol99, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29840) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list101 = list;
		Guid cLSMoein100 = ParentDefaults.MoeinCoding.CLSMoein29835;
		string translate100 = "[پیش دریافت فروش محصولات]".GetTranslate();
		Guid clsKol100 = ParentDefaults.KolCoding.ClsKol29836;
		list101.Add(CreateMoeinInstance(contextGuid, cLSMoein100, translate100, clsKol100, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29836) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list102 = list;
		Guid cLSMoein101 = ParentDefaults.MoeinCoding.CLSMoein29834;
		string translate101 = "[سایر پیش دریافتها]".GetTranslate();
		Guid clsKol101 = ParentDefaults.KolCoding.ClsKol29836;
		list102.Add(CreateMoeinInstance(contextGuid, cLSMoein101, translate101, clsKol101, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29836) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list103 = list;
		Guid cLSMoein102 = ParentDefaults.MoeinCoding.CLSMoein29832;
		string translate102 = "[ذخیره مالیات]".GetTranslate();
		Guid clsKol102 = ParentDefaults.KolCoding.ClsKol29833;
		list103.Add(CreateMoeinInstance(contextGuid, cLSMoein102, translate102, clsKol102, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29833) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list104 = list;
		Guid cLSMoein103 = ParentDefaults.MoeinCoding.CLSMoein29830;
		string translate103 = "[سود سهام پیشنهادی]".GetTranslate();
		Guid clsKol103 = ParentDefaults.KolCoding.ClsKol29831;
		list104.Add(CreateMoeinInstance(contextGuid, cLSMoein103, translate103, clsKol103, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29831) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list105 = list;
		Guid cLSMoein104 = ParentDefaults.MoeinCoding.CLSMoein29829;
		string translate104 = "[سود سهام پرداختنی]".GetTranslate();
		Guid clsKol104 = ParentDefaults.KolCoding.ClsKol29831;
		list105.Add(CreateMoeinInstance(contextGuid, cLSMoein104, translate104, clsKol104, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29831) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list106 = list;
		Guid cLSMoein105 = ParentDefaults.MoeinCoding.CLSMoein29827;
		string translate105 = "[وامهای دریافتی از بانکها - فروش اقساطی]".GetTranslate();
		Guid clsKol105 = ParentDefaults.KolCoding.ClsKol29828;
		list106.Add(CreateMoeinInstance(contextGuid, cLSMoein105, translate105, clsKol105, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29828) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list107 = list;
		Guid cLSMoein106 = ParentDefaults.MoeinCoding.CLSMoein29826;
		string translate106 = "[وامهای دریافتی از بانکها - سلف]".GetTranslate();
		Guid clsKol106 = ParentDefaults.KolCoding.ClsKol29828;
		list107.Add(CreateMoeinInstance(contextGuid, cLSMoein106, translate106, clsKol106, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29828) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list108 = list;
		Guid cLSMoein107 = ParentDefaults.MoeinCoding.CLSMoein29824;
		string translate107 = "[ذخیره مرخصی استفاده نشده]".GetTranslate();
		Guid clsKol107 = ParentDefaults.KolCoding.ClsKol29825;
		list108.Add(CreateMoeinInstance(contextGuid, cLSMoein107, translate107, clsKol107, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29825) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list109 = list;
		Guid cLSMoein108 = ParentDefaults.MoeinCoding.CLSMoein29823;
		string translate108 = "[ذخیره سایر هزینه های]".GetTranslate();
		Guid clsKol108 = ParentDefaults.KolCoding.ClsKol29825;
		list109.Add(CreateMoeinInstance(contextGuid, cLSMoein108, translate108, clsKol108, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29825) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list110 = list;
		Guid cLSMoein109 = ParentDefaults.MoeinCoding.CLSMoein29822;
		string translate109 = "[ذخیره بن کارگری]".GetTranslate();
		Guid clsKol109 = ParentDefaults.KolCoding.ClsKol29825;
		list110.Add(CreateMoeinInstance(contextGuid, cLSMoein109, translate109, clsKol109, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29825) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list111 = list;
		Guid cLSMoein110 = ParentDefaults.MoeinCoding.CLSMoein29820;
		string translate110 = "[اسنادپرداختنی بلند مدت]".GetTranslate();
		Guid clsKol110 = ParentDefaults.KolCoding.ClsKol29821;
		list111.Add(CreateMoeinInstance(contextGuid, cLSMoein110, translate110, clsKol110, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29821) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list112 = list;
		Guid cLSMoein111 = ParentDefaults.MoeinCoding.CLSMoein29818;
		string translate111 = "[حساب های پرداختنی بلند مدت]".GetTranslate();
		Guid clsKol111 = ParentDefaults.KolCoding.ClsKol29819;
		list112.Add(CreateMoeinInstance(contextGuid, cLSMoein111, translate111, clsKol111, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29819) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list113 = list;
		Guid cLSMoein112 = ParentDefaults.MoeinCoding.CLSMoein29816;
		string translate112 = "[وامهای پرداختنی بلند مدت - فروش اقساطی]".GetTranslate();
		Guid clsKol112 = ParentDefaults.KolCoding.ClsKol29817;
		list113.Add(CreateMoeinInstance(contextGuid, cLSMoein112, translate112, clsKol112, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29817) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list114 = list;
		Guid cLSMoein113 = ParentDefaults.MoeinCoding.CLSMoein29815;
		string translate113 = "[وامهای پرداختنی بلند مدت - سلف]".GetTranslate();
		Guid clsKol113 = ParentDefaults.KolCoding.ClsKol29817;
		list114.Add(CreateMoeinInstance(contextGuid, cLSMoein113, translate113, clsKol113, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29817) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list115 = list;
		Guid cLSMoein114 = ParentDefaults.MoeinCoding.CLSMoein29813;
		string translate114 = "[ذخیره مزایای پایان خدمت کارکنان]".GetTranslate();
		Guid clsKol114 = ParentDefaults.KolCoding.ClsKol29814;
		list115.Add(CreateMoeinInstance(contextGuid, cLSMoein114, translate114, clsKol114, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29814) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list116 = list;
		Guid cLSMoein115 = ParentDefaults.MoeinCoding.CLSMoein29811;
		string translate115 = "[سود ناشی از تسعیر بدهیهای بلند مدت ارزی]".GetTranslate();
		Guid clsKol115 = ParentDefaults.KolCoding.ClsKol29812;
		list116.Add(CreateMoeinInstance(contextGuid, cLSMoein115, translate115, clsKol115, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29812) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list117 = list;
		Guid cLSMoein116 = ParentDefaults.MoeinCoding.CLSMoein15;
		string translate116 = "[سرمایه ابتدای دوره]".GetTranslate();
		Guid clsKol116 = ParentDefaults.KolCoding.ClsKol14;
		list117.Add(CreateMoeinInstance(contextGuid, cLSMoein116, translate116, clsKol116, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol14) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list118 = list;
		Guid cLSMoein117 = ParentDefaults.MoeinCoding.CLSMoein29810;
		string translate117 = "[سرمایه ثبت شده – سهام عادی با نام]".GetTranslate();
		Guid clsKol117 = ParentDefaults.KolCoding.ClsKol14;
		list118.Add(CreateMoeinInstance(contextGuid, cLSMoein117, translate117, clsKol117, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol14) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list119 = list;
		Guid cLSMoein118 = ParentDefaults.MoeinCoding.CLSMoein29809;
		string translate118 = "[سرمایه تعهد شده – سهام عادی با نام]".GetTranslate();
		Guid clsKol118 = ParentDefaults.KolCoding.ClsKol14;
		list119.Add(CreateMoeinInstance(contextGuid, cLSMoein118, translate118, clsKol118, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol14) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list120 = list;
		Guid cLSMoein119 = ParentDefaults.MoeinCoding.CLSMoein29807;
		string translate119 = "[اندوخته قانونی]".GetTranslate();
		Guid clsKol119 = ParentDefaults.KolCoding.ClsKol29808;
		list120.Add(CreateMoeinInstance(contextGuid, cLSMoein119, translate119, clsKol119, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29808) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list121 = list;
		Guid cLSMoein120 = ParentDefaults.MoeinCoding.CLSMoein29805;
		string translate120 = "[اندوخته سرمایه ای]".GetTranslate();
		Guid clsKol120 = ParentDefaults.KolCoding.ClsKol29806;
		list121.Add(CreateMoeinInstance(contextGuid, cLSMoein120, translate120, clsKol120, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29806) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list122 = list;
		Guid cLSMoein121 = ParentDefaults.MoeinCoding.CLSMoein29804;
		string translate121 = "[اندوخته توسعه و تکمیل]".GetTranslate();
		Guid clsKol121 = ParentDefaults.KolCoding.ClsKol29806;
		list122.Add(CreateMoeinInstance(contextGuid, cLSMoein121, translate121, clsKol121, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29806) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list123 = list;
		Guid cLSMoein122 = ParentDefaults.MoeinCoding.CLSMoein29803;
		string translate122 = "[اندوخته احتیاطی]".GetTranslate();
		Guid clsKol122 = ParentDefaults.KolCoding.ClsKol29806;
		list123.Add(CreateMoeinInstance(contextGuid, cLSMoein122, translate122, clsKol122, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29806) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list124 = list;
		Guid cLSMoein123 = ParentDefaults.MoeinCoding.CLSMoein29802;
		string translate123 = "[اندوخته جایگزین دارائیها]".GetTranslate();
		Guid clsKol123 = ParentDefaults.KolCoding.ClsKol29806;
		list124.Add(CreateMoeinInstance(contextGuid, cLSMoein123, translate123, clsKol123, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29806) + "104"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list125 = list;
		Guid cLSMoein124 = ParentDefaults.MoeinCoding.CLSMoein29801;
		string translate124 = "[صرف سهام]".GetTranslate();
		Guid clsKol124 = ParentDefaults.KolCoding.ClsKol29806;
		list125.Add(CreateMoeinInstance(contextGuid, cLSMoein124, translate124, clsKol124, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29806) + "105"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list126 = list;
		Guid cLSMoein125 = ParentDefaults.MoeinCoding.CLSMoein29799;
		string translate125 = "[سود (زیان ) انباشته]".GetTranslate();
		Guid clsKol125 = ParentDefaults.KolCoding.ClsKol29800;
		list126.Add(CreateMoeinInstance(contextGuid, cLSMoein125, translate125, clsKol125, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29800) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list127 = list;
		Guid cLSMoein126 = ParentDefaults.MoeinCoding.CLSMoein29798;
		string translate126 = "[تعدیلات سنواتی]".GetTranslate();
		Guid clsKol126 = ParentDefaults.KolCoding.ClsKol29800;
		list127.Add(CreateMoeinInstance(contextGuid, cLSMoein126, translate126, clsKol126, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29800) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list128 = list;
		Guid cLSMoein127 = ParentDefaults.MoeinCoding.CLSMoein29796;
		string translate127 = "[سود (زیان ) جاری]".GetTranslate();
		Guid clsKol127 = ParentDefaults.KolCoding.ClsKol29797;
		list128.Add(CreateMoeinInstance(contextGuid, cLSMoein127, translate127, clsKol127, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29797) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list129 = list;
		Guid cLSMoein128 = ParentDefaults.MoeinCoding.CLSMoein29794;
		string translate128 = "[تقسیم سود]".GetTranslate();
		Guid clsKol128 = ParentDefaults.KolCoding.ClsKol29795;
		list129.Add(CreateMoeinInstance(contextGuid, cLSMoein128, translate128, clsKol128, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29795) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list130 = list;
		Guid cLSMoein129 = ParentDefaults.MoeinCoding.CLSMoein16;
		string translate129 = "[فروش ناخالص]".GetTranslate();
		Guid clsKol129 = ParentDefaults.KolCoding.ClsKol29793;
		list130.Add(CreateMoeinInstance(contextGuid, cLSMoein129, translate129, clsKol129, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29793) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list131 = list;
		Guid cLSMoein130 = ParentDefaults.MoeinCoding.CLSMoein38;
		string translate130 = "[فروش خدمات]".GetTranslate();
		Guid clsKol130 = ParentDefaults.KolCoding.ClsKol29793;
		list131.Add(CreateMoeinInstance(contextGuid, cLSMoein130, translate130, clsKol130, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29793) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list132 = list;
		Guid cLSMoein131 = ParentDefaults.MoeinCoding.CLSMoein32;
		string translate131 = "[برگشت از فروش]".GetTranslate();
		Guid clsKol131 = ParentDefaults.KolCoding.ClsKol29793;
		list132.Add(CreateMoeinInstance(contextGuid, cLSMoein131, translate131, clsKol131, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29793) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list133 = list;
		Guid cLSMoein132 = ParentDefaults.MoeinCoding.CLSMoein20;
		string translate132 = "[تخفیفات نقدی فروش]".GetTranslate();
		Guid clsKol132 = ParentDefaults.KolCoding.ClsKol29793;
		list133.Add(CreateMoeinInstance(contextGuid, cLSMoein132, translate132, clsKol132, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29793) + "104"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list134 = list;
		Guid clsMoeinBackSellService = ParentDefaults.MoeinCoding.clsMoeinBackSellService;
		string translate133 = "[مرجوعی خدمات]".GetTranslate();
		Guid clsKol133 = ParentDefaults.KolCoding.ClsKol29793;
		list134.Add(CreateMoeinInstance(contextGuid, clsMoeinBackSellService, translate133, clsKol133, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29793) + "106"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list135 = list;
		Guid cLSMoein133 = ParentDefaults.MoeinCoding.CLSMoein29783;
		string translate134 = "[سود و زیان فروش ضایعات]".GetTranslate();
		Guid clsKol134 = ParentDefaults.KolCoding.ClsKol19;
		list135.Add(CreateMoeinInstance(contextGuid, cLSMoein133, translate134, clsKol134, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol19) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list136 = list;
		Guid cLSMoein134 = ParentDefaults.MoeinCoding.CLSMoein29782;
		string translate135 = "[سایر درآمدها و هزینه های عملیاتی]".GetTranslate();
		Guid clsKol135 = ParentDefaults.KolCoding.ClsKol19;
		list136.Add(CreateMoeinInstance(contextGuid, cLSMoein134, translate135, clsKol135, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol19) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list137 = list;
		Guid cLSMoein135 = ParentDefaults.MoeinCoding.CLSMoein23;
		string translate136 = "[درآمد حمل]".GetTranslate();
		Guid clsKol136 = ParentDefaults.KolCoding.ClsKol19;
		list137.Add(CreateMoeinInstance(contextGuid, cLSMoein135, translate136, clsKol136, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol19) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list138 = list;
		Guid cLSMoein136 = ParentDefaults.MoeinCoding.CLSMoein29;
		string translate137 = "[درآمد بهره]".GetTranslate();
		Guid clsKol137 = ParentDefaults.KolCoding.ClsKol29874;
		list138.Add(CreateMoeinInstance(contextGuid, cLSMoein136, translate137, clsKol137, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29874) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list139 = list;
		Guid cLSMoein137 = ParentDefaults.MoeinCoding.CLSMoein29776;
		string translate138 = "[اضافه انبار]".GetTranslate();
		Guid clsKol138 = ParentDefaults.KolCoding.ClsKol29874;
		list139.Add(CreateMoeinInstance(contextGuid, cLSMoein137, translate138, clsKol138, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29874) + "104"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list140 = list;
		Guid cLSMoein138 = ParentDefaults.MoeinCoding.CLSMoein30;
		string translate139 = "[سود حاصل از فروش زمین ، اثاثیه ، ماشین آلات و تجهیزات]".GetTranslate();
		Guid clsKol139 = ParentDefaults.KolCoding.ClsKol29874;
		list140.Add(CreateMoeinInstance(contextGuid, cLSMoein138, translate139, clsKol139, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29874) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list141 = list;
		Guid cLSMoein139 = ParentDefaults.MoeinCoding.CLSMoein35;
		string translate140 = "[درآمد حساب سپرده گذاری]".GetTranslate();
		Guid clsKol140 = ParentDefaults.KolCoding.ClsKol29874;
		list141.Add(CreateMoeinInstance(contextGuid, cLSMoein139, translate140, clsKol140, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29874) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		list.Add(CreateMoeinInstance(contextGuid, ParentDefaults.MoeinCoding.CLSMoein29780, "[قیمت تمام شده کالای فروش رفته]".GetTranslate(), ParentDefaults.KolCoding.ClsKol29781, long.Parse(KolBusiness.Get(contextGuid, ParentDefaults.KolCoding.ClsKol29781).Code + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		list.Add(CreateMoeinInstance(contextGuid, ParentDefaults.MoeinCoding.CLSMoein17, "[خرید ناخالص]".GetTranslate(), ParentDefaults.KolCoding.ClsKol18, long.Parse(KolBusiness.Get(contextGuid, ParentDefaults.KolCoding.ClsKol18).Code + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		list.Add(CreateMoeinInstance(contextGuid, ParentDefaults.MoeinCoding.CLSMoein31, "[هزینه حمل کالای خریداری شده]".GetTranslate(), ParentDefaults.KolCoding.ClsKol18, long.Parse(KolBusiness.Get(contextGuid, ParentDefaults.KolCoding.ClsKol18).Code + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list142 = list;
		Guid cLSMoein140 = ParentDefaults.MoeinCoding.CLSMoein33;
		string translate141 = "[برگشت از خرید]".GetTranslate();
		Guid clsKol141 = ParentDefaults.KolCoding.ClsKol18;
		list142.Add(CreateMoeinInstance(contextGuid, cLSMoein140, translate141, clsKol141, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol18) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list143 = list;
		Guid cLSMoein141 = ParentDefaults.MoeinCoding.CLSMoein21;
		string translate142 = "[تخفیفات نقدی خرید]".GetTranslate();
		Guid clsKol142 = ParentDefaults.KolCoding.ClsKol18;
		list143.Add(CreateMoeinInstance(contextGuid, cLSMoein141, translate142, clsKol142, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol18) + "104"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list144 = list;
		Guid cLSMoein142 = ParentDefaults.MoeinCoding.CLSMoein51;
		string translate143 = "[هزینه بازاریابی و پورسانت]".GetTranslate();
		Guid clsKol143 = ParentDefaults.KolCoding.ClsKol29991;
		list144.Add(CreateMoeinInstance(contextGuid, cLSMoein142, translate143, clsKol143, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29991) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list145 = list;
		Guid cLSMoein143 = ParentDefaults.MoeinCoding.CLSMoein48;
		string translate144 = "[هزینه ضایعات غیر عملیاتی]".GetTranslate();
		Guid clsKol144 = ParentDefaults.KolCoding.ClsKol29991;
		list145.Add(CreateMoeinInstance(contextGuid, cLSMoein143, translate144, clsKol144, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29991) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list146 = list;
		Guid cLSMoein144 = ParentDefaults.MoeinCoding.CLSMoein34;
		string translate145 = "[هزینه بهره]".GetTranslate();
		Guid clsKol145 = ParentDefaults.KolCoding.ClsKol29991;
		list146.Add(CreateMoeinInstance(contextGuid, cLSMoein144, translate145, clsKol145, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29991) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list147 = list;
		Guid cLSMoein145 = ParentDefaults.MoeinCoding.CLSMoein29777;
		string translate146 = "[کسر انبار]".GetTranslate();
		Guid clsKol146 = ParentDefaults.KolCoding.ClsKol29991;
		list147.Add(CreateMoeinInstance(contextGuid, cLSMoein145, translate146, clsKol146, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29991) + "104"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list148 = list;
		Guid cLSMoein146 = ParentDefaults.MoeinCoding.CLSMoein29997;
		string translate147 = "[اسناد تضمینی شرکت نزد دیگران]".GetTranslate();
		Guid clsKol147 = ParentDefaults.KolCoding.ClsKol29999;
		list148.Add(CreateMoeinInstance(contextGuid, cLSMoein146, translate147, clsKol147, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29999) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list149 = list;
		Guid cLSMoein147 = ParentDefaults.MoeinCoding.CLSMoein29996;
		string translate148 = "[اسناد تضمینی دیگران نزد شرکت]".GetTranslate();
		Guid clsKol148 = ParentDefaults.KolCoding.ClsKol29999;
		list149.Add(CreateMoeinInstance(contextGuid, cLSMoein147, translate148, clsKol148, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29999) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list150 = list;
		Guid cLSMoein148 = ParentDefaults.MoeinCoding.CLSMoein29994;
		string translate149 = "[طرف حساب انتظامی دیگران نزد شرکت]".GetTranslate();
		Guid clsKol149 = ParentDefaults.KolCoding.ClsKol29998;
		list150.Add(CreateMoeinInstance(contextGuid, cLSMoein148, translate149, clsKol149, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29998) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list151 = list;
		Guid cLSMoein149 = ParentDefaults.MoeinCoding.CLSMoein29995;
		string translate150 = "[طرف حساب انتظامی شرکت نزد دیگران]".GetTranslate();
		Guid clsKol150 = ParentDefaults.KolCoding.ClsKol29998;
		list151.Add(CreateMoeinInstance(contextGuid, cLSMoein149, translate150, clsKol150, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29998) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		list.Add(CreateMoeinInstance(contextGuid, ParentDefaults.MoeinCoding.CLSMoein_HesabEntezami_Digaran, "[حسابهای انتظامی دیگران نزد شرکت]".GetTranslate(), ParentDefaults.KolCoding.ClsKol29999, 0L, st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		list.Add(CreateMoeinInstance(contextGuid, ParentDefaults.MoeinCoding.CLSMoein_HesabEntezami_Ma, "[حسابهای انتظامی شرکت نزد دیگران]".GetTranslate(), ParentDefaults.KolCoding.ClsKol29999, 0L, st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list152 = list;
		Guid cLSMoein150 = ParentDefaults.MoeinCoding.CLSMoein3;
		string translate151 = "[برداشت]".GetTranslate();
		Guid clsKol151 = ParentDefaults.KolCoding.ClsKol29779;
		list152.Add(CreateMoeinInstance(contextGuid, cLSMoein150, translate151, clsKol151, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29779) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list153 = list;
		Guid cLSMoein151 = ParentDefaults.MoeinCoding.CLSMoein11;
		string translate152 = "[هزینه های عملیاتی]".GetTranslate();
		Guid clsKol152 = ParentDefaults.KolCoding.ClsKol29778;
		list153.Add(CreateMoeinInstance(contextGuid, cLSMoein151, translate152, clsKol152, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29778) + "100"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list154 = list;
		Guid cLSMoein152 = ParentDefaults.MoeinCoding.CLSMoein40;
		string translate153 = "[مواد مستقیم]".GetTranslate();
		Guid clsKol153 = ParentDefaults.KolCoding.ClsKol39;
		list154.Add(CreateMoeinInstance(contextGuid, cLSMoein152, translate153, clsKol153, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol39) + "101"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list155 = list;
		Guid cLSMoein153 = ParentDefaults.MoeinCoding.CLSMoein42;
		string translate154 = "[کالای ساخته شده]".GetTranslate();
		Guid clsKol154 = ParentDefaults.KolCoding.ClsKol39;
		list155.Add(CreateMoeinInstance(contextGuid, cLSMoein153, translate154, clsKol154, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol39) + "102"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list156 = list;
		Guid cLSMoein154 = ParentDefaults.MoeinCoding.CLSMoein43;
		string translate155 = "[هزینه های تولید]".GetTranslate();
		Guid clsKol155 = ParentDefaults.KolCoding.ClsKol39;
		list156.Add(CreateMoeinInstance(contextGuid, cLSMoein154, translate155, clsKol155, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol39) + "103"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		list.Add(CreateMoeinInstance(contextGuid, ParentDefaults.MoeinCoding.clsMoeinPenalty, "[درآمد حاصل از جریمه دیرکرد اقساط]".GetTranslate(), ParentDefaults.KolCoding.ClsKol19, 0L, st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		List<MoeinBusiness> list157 = list;
		Guid clsMoeinUnemploymentInsurance = ParentDefaults.MoeinCoding.clsMoeinUnemploymentInsurance;
		string translate156 = "[سازمان تامین اجتماعی – حق بیمه بیکاری]".GetTranslate();
		Guid clsKol156 = ParentDefaults.KolCoding.ClsKol29856;
		list157.Add(CreateMoeinInstance(contextGuid, clsMoeinUnemploymentInsurance, translate156, clsKol156, long.Parse(await GetKolCode(ParentDefaults.KolCoding.ClsKol29856) + "117"), st: true, "", ENMahiat.Debit_Credit, system: true, peygiri: false, tedadi: false));
		return list;
	}

	public async Task<List<TafsilBusiness>> ListTafsilAsync(string contextGuid)
	{
		List<TafsilBusiness> list = new List<TafsilBusiness>();
		List<TafsilBusiness> list2 = list;
		list2.Add(await CreatTafsilInstanceAsync(contextGuid, ParentDefaults.TafsilCoding.CLSTafsil29775, "[برگشت از خرید]".GetTranslate(), 0L, st: true, "", system: true));
		List<TafsilBusiness> list3 = list;
		list3.Add(await CreatTafsilInstanceAsync(contextGuid, ParentDefaults.TafsilCoding.CLSTafsil29774, "[تخفیفات نقدی خرید]".GetTranslate(), 0L, st: true, "", system: true));
		List<TafsilBusiness> list4 = list;
		list4.Add(await CreatTafsilInstanceAsync(contextGuid, ParentDefaults.TafsilCoding.CLSTafsil29773, "[مالیات بر ارزش افزوده]".GetTranslate(), 0L, st: true, "", system: true));
		List<TafsilBusiness> list5 = list;
		list5.Add(await CreatTafsilInstanceAsync(contextGuid, ParentDefaults.TafsilCoding.CLSTafsil29772, "[عوارض شهرداری]".GetTranslate(), 0L, st: true, "", system: true));
		List<TafsilBusiness> list6 = list;
		list6.Add(await CreatTafsilInstanceAsync(contextGuid, ParentDefaults.TafsilCoding.CLSTafsil29771, "[هزینه حمل کالای خریداری شده]".GetTranslate(), 0L, st: true, "", system: true));
		List<TafsilBusiness> list7 = list;
		list7.Add(await CreatTafsilInstanceAsync(contextGuid, ParentDefaults.TafsilCoding.CLSTafsil29770, "[برگشت از فروش]".GetTranslate(), 0L, st: true, "", system: true));
		List<TafsilBusiness> list8 = list;
		list8.Add(await CreatTafsilInstanceAsync(contextGuid, ParentDefaults.TafsilCoding.CLSTafsil29769, "[تخفیفات نقدی فروش]".GetTranslate(), 0L, st: true, "", system: true));
		List<TafsilBusiness> list9 = list;
		list9.Add(await CreatTafsilInstanceAsync(contextGuid, ParentDefaults.TafsilCoding.CLSTafsil29768, "[خرید]".GetTranslate(), 0L, st: true, "", system: true));
		List<TafsilBusiness> list10 = list;
		list10.Add(await CreatTafsilInstanceAsync(contextGuid, ParentDefaults.TafsilCoding.CLSTafsil29767, "[فروش]".GetTranslate(), 0L, st: true, "", system: true));
		List<TafsilBusiness> list11 = list;
		list11.Add(await CreatTafsilInstanceAsync(contextGuid, ParentDefaults.TafsilCoding.CLSTafsil29766, "[اسناد دریافتنی نزد صندوق]".GetTranslate(), 0L, st: true, "", system: true));
		List<TafsilBusiness> list12 = list;
		list12.Add(await CreatTafsilInstanceAsync(contextGuid, ParentDefaults.TafsilCoding.CLSTafsil29765, "[اسناد پرداختنی کوتاه مدت]".GetTranslate(), 0L, st: true, "", system: true));
		List<TafsilBusiness> list13 = list;
		list13.Add(await CreatTafsilInstanceAsync(contextGuid, ParentDefaults.TafsilCoding.CLSTafsil44, "[هزینه های تولید]".GetTranslate(), 0L, st: true, "", system: true));
		List<TafsilBusiness> list14 = list;
		list14.Add(await CreatTafsilInstanceAsync(contextGuid, ParentDefaults.TafsilCoding.CLSTafsil4400, "[هزینه کارمزد]".GetTranslate(), 0L, st: true, "", system: true));
		List<TafsilBusiness> list15 = list;
		list15.Add(await CreatTafsilInstanceAsync(contextGuid, ParentDefaults.TafsilCoding.CLSTafsil440, "[درآمد بهره]".GetTranslate(), 0L, st: true, "", system: true));
		List<TafsilBusiness> list16 = list;
		list16.Add(await CreatTafsilInstanceAsync(contextGuid, ParentDefaults.TafsilCoding.CLSTafsilTolidMahsolGuid, "[کالای ساخته شده]".GetTranslate(), 0L, st: true, "", system: true));
		List<TafsilBusiness> list17 = list;
		list17.Add(await CreatTafsilInstanceAsync(contextGuid, ParentDefaults.TafsilCoding.CLSTafsilTolidMavadGuid, "[مواد مستقیم]".GetTranslate(), 0L, st: true, "", system: true));
		List<TafsilBusiness> list18 = list;
		list18.Add(await CreatTafsilInstanceAsync(contextGuid, ParentDefaults.TafsilCoding.CLSTafsilZayeat, "[هزینه ضایعات عملیاتی]".GetTranslate(), 0L, st: true, "", system: true));
		return list?.Where((TafsilBusiness p) => p != null)?.ToList();
	}

	public List<TafsilBusiness> ListHazine(string contextGuid)
	{
		return new List<TafsilBusiness>
		{
			CreateHazineInstance(contextGuid, "[1 هزینه های متفرقه]".GetTranslate()),
			CreateHazineInstance(contextGuid, "[هزینه آب]".GetTranslate()),
			CreateHazineInstance(contextGuid, "[هزینه برق]".GetTranslate()),
			CreateHazineInstance(contextGuid, "[هزینه گاز]".GetTranslate()),
			CreateHazineInstance(contextGuid, "[هزینه تلفن]".GetTranslate()),
			CreateHazineInstance(contextGuid, "[هزینه اجاره]".GetTranslate()),
			CreateHazineInstance(contextGuid, "[هزینه حمل]".GetTranslate()),
			CreateHazineInstance(contextGuid, "[هزینه حقوق پرسنل]".GetTranslate()),
			CreateHazineInstance(contextGuid, "[هزینه بیمه]".GetTranslate()),
			CreateHazineInstance(contextGuid, "[کمک هزینه اقلام مصرفی خوار و بار (بن)]".GetTranslate(), ParentDefaults.TafsilCoding.clsTafsilBon),
			CreateHazineInstance(contextGuid, "[حق مسکن]".GetTranslate(), ParentDefaults.TafsilCoding.clsTafsilMaskan),
			CreateHazineInstance(contextGuid, "[کمک هزینه عائله مندی]".GetTranslate(), ParentDefaults.TafsilCoding.clsTafsilAeleMandi),
			CreateHazineInstance(contextGuid, "[حق اولاد]".GetTranslate(), ParentDefaults.TafsilCoding.clsTafsilOlad),
			CreateHazineInstance(contextGuid, "[پایه سنوات تجمیعی]".GetTranslate(), ParentDefaults.TafsilCoding.clsTafsilSanavat),
			CreateHazineInstance(contextGuid, "[هزینه بیمه بیکاری]".GetTranslate(), ParentDefaults.TafsilCoding.clsTafsilBimeBikari)
		}?.Where((TafsilBusiness p) => p != null)?.ToList();
	}

	private KolBusiness CreateKolInstance(string contextGuid, Guid guid, string name, Guid groupGuid, int code, bool st, string description, ENMahiat mahiat, bool system, EnSystem enSystem)
	{
		try
		{
			KolBusiness kolBusiness = new KolBusiness(contextGuid);
			kolBusiness.Name = name;
			kolBusiness.Description = description;
			kolBusiness.Guid = guid;
			kolBusiness.SetGroupGuid(enSystem, groupGuid);
			kolBusiness.Code = code;
			kolBusiness.Status = st;
			kolBusiness.DateM = DateTime.Now;
			kolBusiness.System = system;
			kolBusiness.Modified = DateTime.Now;
			kolBusiness.ServerStatus = ServerStatusForClient.None;
			return kolBusiness;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateKolInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\CodingHesab.cs", 331);
			return null;
		}
	}

	private static async Task<TafsilBusiness> CreatTafsilInstanceAsync(string contextGuid, Guid guid, string name, long code, bool st, string description, bool system)
	{
		try
		{
			TafsilBusiness tafsilBusiness = await TafsilBusiness.GetAsync(contextGuid, EnSystem.None, guid, "", default(CancellationToken), "CreatTafsilInstanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\CodingHesab.cs");
			if (tafsilBusiness != null && tafsilBusiness.Guid == guid)
			{
				return null;
			}
			return new TafsilBusiness(contextGuid)
			{
				Name = name,
				Status = st,
				Description = description,
				Guid = guid,
				DateM = DateTime.Now,
				TafsilGroupGuid = ParentDefaults.TafsilLevelGuids.Others,
				System = system,
				HType = HesabType.Hesab,
				Modified = DateTime.Now,
				ServerStatus = ServerStatusForClient.None,
				Code = code
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreatTafsilInstanceAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\CodingHesab.cs", 358);
			return null;
		}
	}

	private static TafsilBusiness CreateHazineInstance(string contextGuid, string name, Guid? guid = null)
	{
		try
		{
			TafsilBusiness tafsilBusiness = new TafsilBusiness(contextGuid)
			{
				Name = name,
				Status = true,
				Description = "",
				DateM = DateTime.Now,
				TafsilGroupGuid = ParentDefaults.TafsilLevelGuids.Hazineha,
				System = true,
				HType = HesabType.Hazine,
				Modified = DateTime.Now,
				ServerStatus = ServerStatusForClient.None,
				Code = 0L
			};
			if ((guid ?? Guid.Empty) != Guid.Empty)
			{
				tafsilBusiness.Guid = guid.Value;
			}
			else
			{
				tafsilBusiness.Guid = Guid.NewGuid();
			}
			return tafsilBusiness;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateHazineInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\CodingHesab.cs", 386);
			return null;
		}
	}

	private static MoeinBusiness CreateMoeinInstance(string contextGuid, Guid guid, string name, Guid kolGuid, long code, bool st, string description, ENMahiat mahiat, bool system, bool peygiri, bool tedadi)
	{
		try
		{
			return new MoeinBusiness(contextGuid)
			{
				Guid = guid,
				Name = name,
				KolGuid = kolGuid,
				Status = st,
				Description = description,
				Mahiat = mahiat,
				System = system,
				Peygiri = peygiri,
				Tedadi = tedadi,
				DateM = DateTime.Now,
				Modified = DateTime.Now,
				ServerStatus = ServerStatusForClient.None,
				Code = code
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateMoeinInstance", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\CodingHesab.cs", 415);
			return null;
		}
	}
}
