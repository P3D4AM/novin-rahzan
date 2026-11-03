using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace AccSqlServerPersistence.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("AccSqlServerPersistence.Properties.Resources", typeof(Resources).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static string Acc_sp => ResourceManager.GetString("Acc_sp", resourceCulture);

	internal static string Advertise_sp => ResourceManager.GetString("Advertise_sp", resourceCulture);

	internal static string Attendance_sp => ResourceManager.GetString("Attendance_sp", resourceCulture);

	internal static string DropConstraintForClientsOfAccounting => ResourceManager.GetString("DropConstraintForClientsOfAccounting", resourceCulture);

	internal static string DropConstraintForClientsOfSignalR => ResourceManager.GetString("DropConstraintForClientsOfSignalR", resourceCulture);

	internal static string DropReportGenerator => ResourceManager.GetString("DropReportGenerator", resourceCulture);

	internal static string ErteghaFuctions_Baje => ResourceManager.GetString("ErteghaFuctions_Baje", resourceCulture);

	internal static string FixAccountingData => ResourceManager.GetString("FixAccountingData", resourceCulture);

	internal static string FixData => ResourceManager.GetString("FixData", resourceCulture);

	internal static string GetBargashtDate => ResourceManager.GetString("GetBargashtDate", resourceCulture);

	internal static string Global_sp => ResourceManager.GetString("Global_sp", resourceCulture);

	internal static string InsertPriceTypeGuidToBarcode => ResourceManager.GetString("InsertPriceTypeGuidToBarcode", resourceCulture);

	internal static string InsertPriceTypeGuidToOrder => ResourceManager.GetString("InsertPriceTypeGuidToOrder", resourceCulture);

	internal static string InsertPriceTypeGuidToOrderDetail => ResourceManager.GetString("InsertPriceTypeGuidToOrderDetail", resourceCulture);

	internal static string InsertProductGroups => ResourceManager.GetString("InsertProductGroups", resourceCulture);

	internal static string InsertProductPrices => ResourceManager.GetString("InsertProductPrices", resourceCulture);

	internal static string InsertProductPrices2 => ResourceManager.GetString("InsertProductPrices2", resourceCulture);

	internal static string InsertTaxOdrerLog => ResourceManager.GetString("InsertTaxOdrerLog", resourceCulture);

	internal static string SqlBackUpScripts => ResourceManager.GetString("SqlBackUpScripts", resourceCulture);

	internal static string UpdateCashierFromUser => ResourceManager.GetString("UpdateCashierFromUser", resourceCulture);

	internal static string UpdateDecimal => ResourceManager.GetString("UpdateDecimal", resourceCulture);

	internal static string UpdateResids => ResourceManager.GetString("UpdateResids", resourceCulture);

	internal Resources()
	{
	}
}
