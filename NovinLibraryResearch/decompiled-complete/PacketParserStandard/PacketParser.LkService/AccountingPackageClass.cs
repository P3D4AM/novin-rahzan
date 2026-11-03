using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.LkService;

public class AccountingPackageClass
{
	public EnLK_SpecialFacilities? SpecialFacilities { get; set; }

	public Level_Accounting Level { get; set; }

	public long LkSerial { get; set; }

	public string SoftwareSerial { get; set; }

	public PackageContainer Tax { get; set; }

	public PackageContainer Tolid { get; set; }

	public PackageContainer AdvanceFinance { get; set; }

	public PackageContainer Scale { get; set; }

	public PackageContainer CallerId { get; set; }

	public PackageContainer Barcode { get; set; }

	public PackageContainer Cheque { get; set; }

	public PackageContainer Installment { get; set; }

	public PackageContainer Depot { get; set; }

	public PackageContainer Service { get; set; }

	public PackageContainer CustomerClub { get; set; }

	public PackageContainer Serial { get; set; }

	public PackageContainer MultiCurrency { get; set; }

	public PackageContainer VoiceCall { get; set; }

	public List<PackageContainer> GetAllPackages()
	{
		try
		{
			return new List<PackageContainer>
			{
				new PackageContainer(EnLK_SpecialFacilities.Tax, Tax?.IsActive ?? false, Tax?.ActivationCode),
				new PackageContainer(EnLK_SpecialFacilities.AdvancedFinance, AdvanceFinance?.IsActive ?? false, AdvanceFinance?.ActivationCode),
				new PackageContainer(EnLK_SpecialFacilities.Scale, Scale?.IsActive ?? false, Scale?.ActivationCode),
				new PackageContainer(EnLK_SpecialFacilities.CallerId, CallerId?.IsActive ?? false, CallerId?.ActivationCode),
				new PackageContainer(EnLK_SpecialFacilities.Barcode, Barcode?.IsActive ?? false, Barcode?.ActivationCode),
				new PackageContainer(EnLK_SpecialFacilities.Cheque, Cheque?.IsActive ?? false, Cheque?.ActivationCode),
				new PackageContainer(EnLK_SpecialFacilities.Aghsat, Installment?.IsActive ?? false, Installment?.ActivationCode),
				new PackageContainer(EnLK_SpecialFacilities.Depot, Depot?.IsActive ?? false, Depot?.ActivationCode),
				new PackageContainer(EnLK_SpecialFacilities.Service, Service?.IsActive ?? false, Service?.ActivationCode),
				new PackageContainer(EnLK_SpecialFacilities.CustomerClub, CustomerClub?.IsActive ?? false, CustomerClub?.ActivationCode),
				new PackageContainer(EnLK_SpecialFacilities.ProductSerial, Serial?.IsActive ?? false, Serial?.ActivationCode),
				new PackageContainer(EnLK_SpecialFacilities.MultiCurrency, MultiCurrency?.IsActive ?? false, MultiCurrency?.ActivationCode),
				new PackageContainer(EnLK_SpecialFacilities.VoiceCall, VoiceCall?.IsActive ?? false, VoiceCall?.ActivationCode),
				new PackageContainer(EnLK_SpecialFacilities.Tolid, Tolid?.IsActive ?? false, Tolid?.ActivationCode)
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllPackages", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\LkService\\AccountingPackageClass.cs", 55);
			throw;
		}
	}
}
