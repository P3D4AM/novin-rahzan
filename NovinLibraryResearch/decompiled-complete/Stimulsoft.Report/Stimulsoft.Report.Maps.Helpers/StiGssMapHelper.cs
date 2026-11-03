using System.Collections.Generic;

namespace Stimulsoft.Report.Maps.Helpers;

public static class StiGssMapHelper
{
	private static Dictionary<string, Dictionary<string, string>> hash;

	public static bool AllowGss(string id)
	{
		return id == StiMapID.UKCountries.ToString();
	}

	public static Dictionary<string, string> Get(string id)
	{
		if (!AllowGss(id))
		{
			return null;
		}
		if (hash == null)
		{
			hash = new Dictionary<string, Dictionary<string, string>>();
		}
		if (!hash.ContainsKey(id))
		{
			Init(id);
		}
		return hash[id];
	}

	private static void Init(string id)
	{
		if (id == StiMapID.UKCountries.ToString())
		{
			InitUKCountries();
		}
	}

	public static bool IsGssValue(string value)
	{
		if (value == null)
		{
			return false;
		}
		string text = value.ToLowerInvariant();
		if (!text.StartsWith("s12") && !text.StartsWith("e06") && !text.StartsWith("e10") && !text.StartsWith("e07") && !text.StartsWith("e09") && !text.StartsWith("n09") && !text.StartsWith("w06") && !text.StartsWith("s12") && !text.StartsWith("e08"))
		{
			return text.StartsWith("e11");
		}
		return true;
	}

	private static void Add(Dictionary<string, string> data, string state, string gss)
	{
		if (gss != null)
		{
			data.Add(gss.ToLowerInvariant(), state);
		}
	}

	private static void InitUKCountries()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		Add(dictionary, "EastAyrshire", "S12000008");
		Add(dictionary, "EastDunbartonshire", "S12000045");
		Add(dictionary, "EastRenfrewshire", "S12000011");
		Add(dictionary, "Glasgow", "S12000046");
		Add(dictionary, "Inverclyde", "S12000018");
		Add(dictionary, "NorthAyshire", "S12000021");
		Add(dictionary, "NorthLanarkshire", "S12000044");
		Add(dictionary, "Renfrewshire", "S12000038");
		Add(dictionary, "SouthAyrshire", "S12000028");
		Add(dictionary, "SouthLanarkshire", "S12000029");
		Add(dictionary, "WestDunbartonshire", "S12000039");
		Add(dictionary, "Aberdeen", "S12000033");
		Add(dictionary, "Aberdeenshire", "S12000034");
		Add(dictionary, "Moray", "S12000020");
		Add(dictionary, "Falkirk", "S12000014");
		Add(dictionary, "Stirling", "S12000030");
		Add(dictionary, "Clackmannanshire", "S12000005");
		Add(dictionary, "PerthshireAndKinross", "S12000024");
		Add(dictionary, "Angus", "S12000041");
		Add(dictionary, "Dundee", "S12000042");
		Add(dictionary, "Fife", "S12000015");
		Add(dictionary, "EastLothian", "S12000010");
		Add(dictionary, "Edinburgh", "S12000036");
		Add(dictionary, "Midlothian", "S12000019");
		Add(dictionary, "WestLothian", "S12000040");
		Add(dictionary, "ScottishBorders", "S12000026");
		Add(dictionary, "Darlington", "E06000005");
		Add(dictionary, "Durham", "E06000047");
		Add(dictionary, "Hartlepool", "E06000001");
		Add(dictionary, "Middlesbrough", "E06000002");
		Add(dictionary, "RedcarAndCleveland", "E06000003");
		Add(dictionary, "StocktonOnTees", "E06000004");
		Add(dictionary, "Northumberland", "E06000057");
		Add(dictionary, "Hampshire", "E10000014");
		Add(dictionary, "Southampton", "E06000045");
		Add(dictionary, "MiltonKeynes", "E06000042");
		Add(dictionary, "Gloucestershire", "E07000081");
		Add(dictionary, "Buckinghamshire", "E10000002");
		Add(dictionary, "Hertfordshire", "E10000015");
		Add(dictionary, "BathAndNorthEastSomerset", "E06000022");
		Add(dictionary, "Bristol", "E06000023");
		Add(dictionary, "NorthSomerset", "E06000024");
		Add(dictionary, "SouthGloucestershire", "E06000025");
		Add(dictionary, "Somerset", "E10000027");
		Add(dictionary, "Devon", "E10000008");
		Add(dictionary, "Bournemouth", "E06000028");
		Add(dictionary, "Dorset", "E10000009");
		Add(dictionary, "Poole", "E06000029");
		Add(dictionary, "Cambridgeshire", "E07000008");
		Add(dictionary, "Leicestershire", "E07000134");
		Add(dictionary, "KingstonUponHull", "E06000010");
		Add(dictionary, "NorthEastLincolnshire", "E06000012");
		Add(dictionary, "NorthLincolnshire", "E06000013");
		Add(dictionary, "Derby", "E06000015");
		Add(dictionary, "Derbyshire", "E07000035");
		Add(dictionary, "BarkingAndDagenham", "E09000002");
		Add(dictionary, "Bexley", "E09000004");
		Add(dictionary, "Brent", "E09000005");
		Add(dictionary, "Bromley", "E09000006");
		Add(dictionary, "Camden", "E09000007");
		Add(dictionary, "Croydon", "E09000008");
		Add(dictionary, "Ealing", "E09000009");
		Add(dictionary, "Enfield", "E09000010");
		Add(dictionary, "Greenwich", "E09000011");
		Add(dictionary, "HammersmithAndFulham", "E09000013");
		Add(dictionary, "Hounslow", "E09000018");
		Add(dictionary, "Islington", "E09000019");
		Add(dictionary, "KensingtonAndChelsea", "E09000020");
		Add(dictionary, "Merton", "E09000024");
		Add(dictionary, "Redbridge", "E09000026");
		Add(dictionary, "RichmondUponThames", "E09000027");
		Add(dictionary, "Sutton", "E09000029");
		Add(dictionary, "TowerHamlets", "E09000030");
		Add(dictionary, "WalthamForest", "E09000031");
		Add(dictionary, "Wandsworth", "E09000032");
		Add(dictionary, "Westminster", "E09000033");
		Add(dictionary, "Lincolnshire", "E07000138");
		Add(dictionary, "Belfast", "N09000003");
		Add(dictionary, "Derry", null);
		Add(dictionary, "Omagh", null);
		Add(dictionary, "Armagh", null);
		Add(dictionary, "NewryAndMourne", null);
		Add(dictionary, "Banbridge", null);
		Add(dictionary, "Craigavon", null);
		Add(dictionary, "Dungannon", null);
		Add(dictionary, "Lisburn", null);
		Add(dictionary, "Cookstown", null);
		Add(dictionary, "Antrim", null);
		Add(dictionary, "Magherafelt", null);
		Add(dictionary, "Ballymena", null);
		Add(dictionary, "Larne", null);
		Add(dictionary, "Carrickfergus", null);
		Add(dictionary, "Newtownabbey", null);
		Add(dictionary, "NorthDown", null);
		Add(dictionary, "Down", null);
		Add(dictionary, "Coleraine", null);
		Add(dictionary, "Ballymoney", null);
		Add(dictionary, "Limavady", null);
		Add(dictionary, "Castlereagh", null);
		Add(dictionary, "Carmarthenshire", "W06000010");
		Add(dictionary, "Ceredigion", "W06000008");
		Add(dictionary, "Pembrokeshire", "W06000009");
		Add(dictionary, "Cornwall", "E06000052");
		Add(dictionary, "Powys", "W06000023");
		Add(dictionary, "Bridgend", "W06000013");
		Add(dictionary, "Caerphilly", "W06000018");
		Add(dictionary, "MerthyrTydfil", "W06000024");
		Add(dictionary, "RhonddaCynonTaf", "W06000016");
		Add(dictionary, "Cardiff", "W06000015");
		Add(dictionary, "ValeOfGlamorgan", "W06000014");
		Add(dictionary, "NeathPortTalbot", "W06000012");
		Add(dictionary, "Swansea", "W06000011");
		Add(dictionary, "York", "E06000014");
		Add(dictionary, "TelfordAndWrekin", "E06000020");
		Add(dictionary, "BlackburnWithDarwen", "E06000008");
		Add(dictionary, "Lancashire", "E10000017");
		Add(dictionary, "EastRidingOfYorkshire", "E06000011");
		Add(dictionary, "Denbighshire", "W06000004");
		Add(dictionary, "Flintshire", "W06000005");
		Add(dictionary, "Wrexham", "W06000006");
		Add(dictionary, "Anglesey", "W06000001");
		Add(dictionary, "Conwy", "W06000003");
		Add(dictionary, "Gwynedd", "W06000002");
		Add(dictionary, "BlaenauGwent", "W06000019");
		Add(dictionary, "Monmouthshire", "W06000021");
		Add(dictionary, "Newport", "W06000022");
		Add(dictionary, "Torfaen", "W06000020");
		Add(dictionary, "Strabane", null);
		Add(dictionary, "Fermanagh", null);
		Add(dictionary, "Ards", null);
		Add(dictionary, "DumfriesAndGalloway", "S12000006");
		Add(dictionary, "Cumbria", "E10000006");
		Add(dictionary, "NorthYorkshire", "E10000023");
		Add(dictionary, "Plymouth", "E06000026");
		Add(dictionary, "Torbay", "E06000027");
		Add(dictionary, "Essex", "E10000012");
		Add(dictionary, "Suffolk", "E10000029");
		Add(dictionary, "Norfolk", "E10000020");
		Add(dictionary, "BrightonAndHove", "E06000043");
		Add(dictionary, "Havering", "E09000016");
		Add(dictionary, "Thurrock", "E06000034");
		Add(dictionary, "EastSussex", "E10000011");
		Add(dictionary, "Medway", "E06000035");
		Add(dictionary, "SouthendOnSea", "E06000033");
		Add(dictionary, "Orkney", "S12000023");
		Add(dictionary, "Highland", "S12000017");
		Add(dictionary, "ArgyllAndBute", "S12000035");
		Add(dictionary, "ShetlandIslands", "S12000027");
		Add(dictionary, "WestSussex", "E10000032");
		Add(dictionary, "Northamptonshire", "E07000152");
		Add(dictionary, "Warwickshire", "E07000218");
		Add(dictionary, "Oxfordshire", "E07000178");
		Add(dictionary, "Luton", "E06000032");
		Add(dictionary, "Hillingdon", "E09000017");
		Add(dictionary, "KingstonUponThames", "E09000021");
		Add(dictionary, "Surrey", "E10000030");
		Add(dictionary, "Swindon", "E06000030");
		Add(dictionary, "Wiltshire", "E06000054");
		Add(dictionary, "IsleOfWight", "E06000046");
		Add(dictionary, "Portsmouth", "E06000044");
		Add(dictionary, "Peterborough", "E06000031");
		Add(dictionary, "Leicester", "E06000016");
		Add(dictionary, "Rutland", "E06000017");
		Add(dictionary, "Nottingham", "E06000018");
		Add(dictionary, "Nottinghamshire", "E10000024");
		Add(dictionary, "Hackney", "E09000012");
		Add(dictionary, "Haringey", "E09000014");
		Add(dictionary, "Harrow", "E09000015");
		Add(dictionary, "Lambeth", "E09000022");
		Add(dictionary, "Lewisham", "E09000023");
		Add(dictionary, "Newham", "E09000025");
		Add(dictionary, "Southwark", "E09000028");
		Add(dictionary, "EileanSiar", "S12000013");
		Add(dictionary, "Moyle", null);
		Add(dictionary, "Warrington", "E06000007");
		Add(dictionary, "Herefordshire", "E06000019");
		Add(dictionary, "Worcestershire", "E07000237");
		Add(dictionary, "Staffordshire", "E07000193");
		Add(dictionary, "StokeOnTrent", "E06000021");
		Add(dictionary, "Shropshire", "E06000051");
		Add(dictionary, "Kent", "E10000016");
		Add(dictionary, "City", "E09000001");
		Add(dictionary, "NewcastleUponTyne", "E08000021");
		Add(dictionary, "NorthTyneside", "E08000022");
		Add(dictionary, "SouthTyneside", "E08000023");
		Add(dictionary, "Sunderland", "E08000024");
		Add(dictionary, "Gateshead", "E08000037");
		Add(dictionary, "Knowsley", "E08000011");
		Add(dictionary, "Sefton", "E08000014");
		Add(dictionary, "Liverpool", "E08000012");
		Add(dictionary, "Merseyside", "E11000002");
		Add(dictionary, "Blackpool", "E06000009");
		Add(dictionary, "Kirklees", "E08000034");
		Add(dictionary, "Calderdale", "E08000033");
		Add(dictionary, "Bradford", "E08000032");
		Add(dictionary, "Leeds", "E08000035");
		Add(dictionary, "Wakefield", "E08000036");
		Add(dictionary, "Salford", "E08000006");
		Add(dictionary, "Wigan", "E08000010");
		Add(dictionary, "Bolton", "E08000001");
		Add(dictionary, "Bury", "E08000002");
		Add(dictionary, "Rochdale", "E08000005");
		Add(dictionary, "Oldham", "E08000004");
		Add(dictionary, "Tameside", "E08000008");
		Add(dictionary, "Stockport", "E08000007");
		Add(dictionary, "Manchester", "E08000003");
		Add(dictionary, "Trafford", "E08000009");
		Add(dictionary, "Rotherham", "E08000018");
		Add(dictionary, "Sheffield", "E08000019");
		Add(dictionary, "Barnsley", "E08000016");
		Add(dictionary, "Doncaster", "E08000017");
		Add(dictionary, "Birmingham", "E08000025");
		Add(dictionary, "Sandwell", "E08000028");
		Add(dictionary, "Dudley", "E08000027");
		Add(dictionary, "Wolverhampton", "E08000031");
		Add(dictionary, "Walsall", "E08000030");
		Add(dictionary, "Solihull", "E08000029");
		Add(dictionary, "Coventry", "E08000026");
		Add(dictionary, "CentralBedfordshire", "E06000056");
		Add(dictionary, "Bedford", "E06000055");
		Add(dictionary, "Reading", "E06000038");
		Add(dictionary, "WestBerkshire", "E06000037");
		Add(dictionary, "Wokingham", "E06000041");
		Add(dictionary, "BracknellForest", "E06000036");
		Add(dictionary, "RoyalBoroughOfWindsorAndMaidenhead", "E06000040");
		Add(dictionary, "Slough", "E06000039");
		Add(dictionary, "Barnet", "E09000003");
		Add(dictionary, "CheshireEast", "E06000049");
		Add(dictionary, "CheshireWestAndChester", "E06000050");
		Add(dictionary, "Halton", "E06000006");
		Add(dictionary, "IslesOfScilly", "E06000053");
		hash.Add(StiMapID.UKCountries.ToString(), dictionary);
	}
}
