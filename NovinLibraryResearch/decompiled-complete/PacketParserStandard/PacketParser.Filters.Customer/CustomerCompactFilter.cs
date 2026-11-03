using System;
using System.Collections.Generic;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Filters.Customer;

public class CustomerCompactFilter
{
	public bool Debit { get; set; }

	public bool Credit { get; set; }

	public bool Countless { get; set; }

	public bool? Status { get; set; }

	public List<ITafsilSelected> SelectedCustomers { get; set; }

	public bool ShowSumAghsat { get; set; }

	public bool ShowDebitCustomerWithoutInstallment { get; set; }

	public bool ShowCreditCustomersAfterInstallment { get; set; }

	public string SearchString { get; set; } = "";


	public Guid? GroupGuid { get; set; }

	public CustomerSearchOption SearchOption { get; set; }

	public SearchLocation SearchLocation { get; set; }

	public int? TakeCount { get; set; }

	public string UserName { get; set; } = "";


	public string Password { get; set; } = "";


	public List<Guid> AreaGuidList { get; set; }

	public List<Guid> MoeinGuidList { get; set; }

	public List<Guid> TafsilGroupGuidList { get; set; }

	public Guid? RoleGuid { get; set; }

	public EnPersonType Type { get; set; } = EnPersonType.None;

}
