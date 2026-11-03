using System.Collections.Generic;

namespace Stimulsoft.Report.Maps.Helpers;

public static class StiMapLoc
{
	public struct StiLocItem
	{
		public string En { get; set; }

		public string Ru { get; set; }

		public StiLocItem(string en, string ru)
		{
			En = en;
			Ru = ru;
		}
	}

	private static Dictionary<StiMapID, StiLocItem> items;

	static StiMapLoc()
	{
		items = new Dictionary<StiMapID, StiLocItem>();
		Add(StiMapID.World, "World", "Мир");
		Add(StiMapID.UK, "UK", "Соединенное Королевство");
		Add(StiMapID.UKCountries, "UK (Countries)", "Соединенное Королевство (Страны)");
		Add(StiMapID.Europe, "Europe", "Европа");
		Add(StiMapID.EuropeWithRussia, "Europe (with Russia)", "Европа с Россией");
		Add(StiMapID.Austria, "Austria", "Австрия");
		Add(StiMapID.Belgium, "Belgium", "Бельгия");
		Add(StiMapID.Benelux, "Benelux", "Бенилюкс");
		Add(StiMapID.France, "France", "Франция");
		Add(StiMapID.FranceDepartments, "France (Departments)", "Франция (Департаменты)");
		Add(StiMapID.France18Regions, "France (18 Regions)", "Франция (18 Регионов)");
		Add(StiMapID.Germany, "Germany", "Германия");
		Add(StiMapID.Ireland, "Ireland", "Ирландия");
		Add(StiMapID.Liechtenstein, "Liechtenstein", "Лихтенштейн");
		Add(StiMapID.Luxembourg, "Luxembourg", "Люксембург");
		Add(StiMapID.Monaco, "Monaco", "Монако");
		Add(StiMapID.Netherlands, "Netherlands", "Нидерланды");
		Add(StiMapID.Switzerland, "Switzerland", "Швейцария");
		Add(StiMapID.Belarus, "Belarus", "Беларусь");
		Add(StiMapID.Bulgaria, "Bulgaria", "Болгария");
		Add(StiMapID.CzechRepublic, "CzechRepublic", "Чехия");
		Add(StiMapID.Poland, "Poland", "Польша");
		Add(StiMapID.Romania, "Romania", "Румыния");
		Add(StiMapID.Russia, "Russia", "Россия");
		Add(StiMapID.Slovakia, "Slovakia", "Словакия");
		Add(StiMapID.Ukraine, "Ukraine", "Украина");
		Add(StiMapID.Denmark, "Denmark", "Дания");
		Add(StiMapID.Estonia, "Estonia", "Эстония");
		Add(StiMapID.Finland, "Finland", "Финляндия");
		Add(StiMapID.Hungary, "Hungary", "Венгрия");
		Add(StiMapID.Iceland, "Iceland", "Исландия");
		Add(StiMapID.Latvia, "Latvia", "Латвия");
		Add(StiMapID.Lithuania, "Lithuania", "Литва");
		Add(StiMapID.Norway, "Norway", "Норвегия");
		Add(StiMapID.Scandinavia, "Scandinavia", "Скандинавия");
		Add(StiMapID.Sweden, "Sweden", "Швеция");
		Add(StiMapID.Albania, "Albania", "Албания");
		Add(StiMapID.Andorra, "Andorra", "Андорра");
		Add(StiMapID.BosniaAndHerzegovina, "Bosnia and Herzegovina", "Босния и Герцеговина");
		Add(StiMapID.Croatia, "Croatia", "Хорватия");
		Add(StiMapID.Cyprus, "Cyprus", "Кипр");
		Add(StiMapID.Greece, "Greece", "Греция");
		Add(StiMapID.Italy, "Italy", "Италия");
		Add(StiMapID.Macedonia, "Macedonia", "Македония");
		Add(StiMapID.Malta, "Malta", "Мальта");
		Add(StiMapID.Montenegro, "Montenegro", "Черногория");
		Add(StiMapID.Portugal, "Portugal", "Португалия");
		Add(StiMapID.SanMarino, "San Marino", "Сан-Марино");
		Add(StiMapID.Serbia, "Serbia", "Сербия");
		Add(StiMapID.Slovenia, "Slovenia", "Словения");
		Add(StiMapID.Spain, "Spain", "Испания");
		Add(StiMapID.Vatican, "Vatican", "Ватикан");
		Add(StiMapID.Armenia, "Armenia", "Армения");
		Add(StiMapID.Azerbaijan, "Azerbaijan", "Азербайджан");
		Add(StiMapID.EU, "Europe Union", "Евросоюз");
		Add(StiMapID.Georgia, "Georgia", "Грузия");
		Add(StiMapID.Kazakhstan, "Kazakhstan", "Казахстан");
		Add(StiMapID.Moldova, "Moldova", "Молдова");
		Add(StiMapID.Turkey, "Turkey", "Турция");
		Add(StiMapID.NorthAmerica, "North America", "Северная Америка");
		Add(StiMapID.USA, "USA", "США");
		Add(StiMapID.Canada, "Canada", "Канада");
		Add(StiMapID.USAAndCanada, "USA+Canada", "США+Канада");
		Add(StiMapID.Mexico, "Mexico", "Мексика");
		Add(StiMapID.SouthAmerica, "South America", "Южная Америка");
		Add(StiMapID.Argentina, "Argentina", "Аргентина");
		Add(StiMapID.ArgentinaFD, "Argentina (FD)", "Аргентина (ФО)");
		Add(StiMapID.Bolivia, "Bolivia", "Боливия");
		Add(StiMapID.Brazil, "Brazil", "Бразилия");
		Add(StiMapID.Chile, "Chile", "Чили");
		Add(StiMapID.Colombia, "Colombia", "Колумбия");
		Add(StiMapID.Ecuador, "Ecuador", "Эквадор");
		Add(StiMapID.FalklandIslands, "Falkland Islands", "Фолклендские острова");
		Add(StiMapID.Guyana, "Guyana", "Гайана");
		Add(StiMapID.Paraguay, "Paraguay", "Парагвай");
		Add(StiMapID.Peru, "Peru", "Перу");
		Add(StiMapID.Suriname, "Suriname", "Суринам");
		Add(StiMapID.Uruguay, "Uruguay", "Уругвай");
		Add(StiMapID.Venezuela, "Venezuela", "Венесуэла");
		Add(StiMapID.Afghanistan, "Afghanistan", "Афганистан");
		Add(StiMapID.Asia, "Asia", "Азия");
		Add(StiMapID.SoutheastAsia, "Southeast Asia", "Юго-Восточная Азия");
		Add(StiMapID.China, "China", "Китай");
		Add(StiMapID.Taiwan, "Taiwan", "Тайвань");
		Add(StiMapID.India, "India", "Индия");
		Add(StiMapID.Israel, "Israel", "Израиль");
		Add(StiMapID.Japan, "Japan", "Япония");
		Add(StiMapID.Malaysia, "Malaysia", "Малайзия");
		Add(StiMapID.MiddleEast, "Middle East", "Средняя Азия");
		Add(StiMapID.Oman, "Oman", "Оман");
		Add(StiMapID.Philippines, "Philippines", "Филиппины");
		Add(StiMapID.Qatar, "Qatar", "Катар");
		Add(StiMapID.SaudiArabia, "Saudi Arabia", "Саудовская Аравия");
		Add(StiMapID.SouthKorea, "South Korea", "Южная Корея");
		Add(StiMapID.Thailand, "Thailand", "Таиланд");
		Add(StiMapID.Vietnam, "Vietnam", "Вьетнам");
		Add(StiMapID.Australia, "Australia", "Австралия");
		Add(StiMapID.Indonesia, "Indonesia", "Индонезия");
		Add(StiMapID.NewZealand, "New Zealand", "Новая Зеландия");
		Add(StiMapID.Oceania, "Oceania", "Океания");
		Add(StiMapID.SouthAfrica, "South Africa", "Южная Африка");
		Add(StiMapID.CentralAfricanRepublic, "Central African Republic", "Центральноафриканская Республика");
	}

	private static void Add(StiMapID id, string en, string ru)
	{
		items.Add(id, new StiLocItem(en, ru));
	}

	public static string GetEn(StiMapID id)
	{
		if (!items.TryGetValue(id, out var value))
		{
			return string.Empty;
		}
		return value.En;
	}

	public static string GetRu(StiMapID id)
	{
		if (!items.TryGetValue(id, out var value))
		{
			return string.Empty;
		}
		return value.Ru;
	}

	public static void GetEnRu(StiMapID id, out string resEn, out string resRu)
	{
		resEn = string.Empty;
		resRu = string.Empty;
		if (items.TryGetValue(id, out var value))
		{
			resEn = value.En;
			resRu = value.Ru;
		}
	}
}
