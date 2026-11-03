using System;

namespace PacketParser.Entities.Interfaces.Department;

public interface ILkInfo : IHasModified, IHasStatus
{
	Guid Guid { get; set; }

	AccountingPackage AccountingPackage { get; set; }

	ENCode TypeAccounting { get; set; }

	HozoorPackage HozoorType { get; set; }

	long LKSerial { get; set; }

	Guid TafsilGuid { get; set; }

	DateTime ExpireDate { get; set; }

	DateTime StartDate { get; set; }

	string Description { get; set; }

	bool Tolid { get; set; }

	bool BarCode { get; set; }

	bool Scale { get; set; }

	bool SpecialAccountingPackage { get; set; }

	bool IsOffline { get; set; }

	TypeHozor Hozor_Hoghogh { get; set; }

	short ScaleCount { get; set; }

	short MaxActivationDays { get; set; }

	short Net_Accounting { get; set; }

	short Net_Hozor { get; set; }

	short Net_Scales { get; set; }

	bool CallerID { get; set; }

	DateTime? FailedDate { get; set; }

	string FailedDescription { get; set; }

	bool IsFailed { get; }

	string ExpireDateSh { get; }

	DateTime? DisableActivationCodeDate { get; set; }

	EnLK_SpecialFacilities SpecialFacilities { get; set; }
}
