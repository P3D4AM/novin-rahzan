using System;
using System.Collections.Generic;
using System.Linq;
using PacketParser.Entities;

namespace PacketParser.Access;

public class UserAccessChanges
{
	private bool resetCustomers;

	public bool ResetProductsGroup { get; private set; }

	public bool ResetProducts { get; private set; }

	public bool ResetCustomers
	{
		get
		{
			return resetCustomers;
		}
		private set
		{
			resetCustomers = value;
			ResetHesabTree = true;
		}
	}

	public bool ResetCustomersGroup { get; private set; }

	public bool ResetKol { get; private set; }

	public bool ResetTafsil { get; private set; }

	public bool ResetProductAttribute { get; private set; }

	public bool ResetMoein { get; private set; }

	public bool ResetHesabTree { get; private set; }

	public bool ResetDepot { get; private set; }

	public bool HasChanged
	{
		get
		{
			if (!ResetHesabTree && !ResetCustomersGroup && !ResetCustomers && !ResetProducts && !ResetProductsGroup && !ResetDepot && !ResetKol && !ResetMoein)
			{
				return ResetTafsil;
			}
			return true;
		}
	}

	public Guid UserGroupGuid { get; }

	public List<Guid> CashierGuids { get; private set; }

	public UserAccessChanges(WebUserAcc OldUser, WebUserAcc NewUser, short depId)
	{
		using (new JaegerMonitor(".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Access\\UserAccessChanges.cs"))
		{
			try
			{
				if (OldUser == null)
				{
					OldUser = new WebUserAcc();
				}
				if (NewUser != null)
				{
					EvaluateChanges(OldUser, NewUser, depId);
					UserGroupGuid = OldUser.Guid;
					CashierGuids = OldUser?.CashierList?.Select((WebCashier p) => p.Guid)?.ToList() ?? new List<Guid>();
					CashierGuids.AddRange(NewUser?.CashierList?.Select((WebCashier p) => p.Guid)?.ToList() ?? new List<Guid>());
					CashierGuids = CashierGuids.Distinct().ToList();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Access\\UserAccessChanges.cs", 53);
			}
		}
	}

	private void EvaluateChanges(WebUserAcc OldUser, WebUserAcc NewUser, short depId)
	{
		using (new JaegerMonitor("EvaluateChanges", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Access\\UserAccessChanges.cs"))
		{
			int num = 0;
			try
			{
				if (OldUser == null || NewUser == null || OldUser.Id != NewUser.Id || OldUser.UserAccess == null || NewUser.UserAccess == null)
				{
					num = 101;
					return;
				}
				num = 102;
				if ((OldUser?.UserAccess?.Product?.Product_DastrasiBeKalahayeKhas).GetValueOrDefault() != (NewUser?.UserAccess?.Product?.Product_DastrasiBeKalahayeKhas).GetValueOrDefault())
				{
					num = 103;
					bool resetProducts = (ResetProductsGroup = true);
					ResetProducts = resetProducts;
					num = 104;
					ResetProductsGroup = true;
					num = 105;
				}
				num = 110;
				if (NewUser.UserAccess.PrdGroupListGuid.Any())
				{
					WebErrorLog.Instance.StartLog("عموجون لطفا لیست Guid رو به جای لیست Id گروه های کالا پیاده کن.", "", "EvaluateChanges", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Access\\UserAccessChanges.cs", 87);
				}
				List<short> list = OldUser?.UserAccess?.PrdGroupList;
				num = 106;
				list?.AddRange(NewUser?.UserAccess?.PrdGroupList);
				num = 6;
				list = list?.Distinct()?.ToList();
				num = 7;
				if ((list?.Count() ?? 0) != (OldUser?.UserAccess?.PrdGroupList?.Count).GetValueOrDefault() || (OldUser?.UserAccess?.PrdGroupList?.Count).GetValueOrDefault() != (NewUser?.UserAccess?.PrdGroupList?.Count).GetValueOrDefault() || (list != null && list.Any((short w) => (NewUser?.UserAccess?.PrdGroupList?.Contains(w)).GetValueOrDefault())))
				{
					num = 8;
					bool resetProducts = (ResetProductsGroup = true);
					ResetProducts = resetProducts;
					num = 9;
					ResetProductsGroup = true;
					num = 10;
				}
				if (OldUser.UserAccess.Product.Product_See != NewUser.UserAccess.Product.Product_See)
				{
					ResetProducts = true;
					ResetProductsGroup = true;
					num = 12;
				}
				if (OldUser.UserAccess.Product.Product_SeeMojoodi != NewUser.UserAccess.Product.Product_SeeMojoodi)
				{
					ResetProducts = true;
				}
				if (OldUser.UserAccess.Customers.Customer_DastrasiBeAshkhaseKhas != NewUser.UserAccess.Customers.Customer_DastrasiBeAshkhaseKhas)
				{
					bool resetProducts = (ResetCustomers = true);
					ResetCustomersGroup = resetProducts;
					num = 13;
				}
				if (OldUser.UserAccess.Customers.Customer_See != NewUser.UserAccess.Customers.Customer_See)
				{
					bool resetProducts = (ResetCustomers = true);
					ResetCustomersGroup = resetProducts;
					num = 14;
				}
				if (OldUser.VisitorId != NewUser.VisitorId)
				{
					bool resetProducts = (ResetHesabTree = true);
					ResetCustomers = resetProducts;
					num = 15;
				}
				if (OldUser.UserAccess.Bardasht.Bardasht_See != NewUser.UserAccess.Bardasht.Bardasht_See)
				{
					ResetHesabTree = true;
					num = 17;
				}
				if (OldUser.UserAccess.Hazine.Hazine_See != NewUser.UserAccess.Hazine.Hazine_See)
				{
					ResetHesabTree = true;
					num = 18;
				}
				if (OldUser.UserAccess.Sandogh.Sandogh_SeeMojodi != NewUser.UserAccess.Sandogh.Sandogh_SeeMojodi)
				{
					ResetHesabTree = true;
					num = 20;
				}
				if (OldUser.UserAccess.Sandogh.Sandogh_See != NewUser.UserAccess.Sandogh.Sandogh_See)
				{
					ResetHesabTree = true;
				}
				if (OldUser.SandoghId != NewUser.SandoghId)
				{
					ResetHesabTree = true;
				}
				if (OldUser.UserAccess.Bank.Bank_See != NewUser.UserAccess.Bank.Bank_See)
				{
					ResetHesabTree = true;
				}
				if (OldUser.BankId1 != NewUser.BankId1)
				{
					ResetHesabTree = true;
				}
				if (OldUser.BankId2 != NewUser.BankId2)
				{
					ResetHesabTree = true;
				}
				if (OldUser.BankId3 != NewUser.BankId3)
				{
					ResetHesabTree = true;
				}
				if (OldUser.BankId4 != NewUser.BankId4)
				{
					ResetHesabTree = true;
				}
				num = 210;
				List<short> list2 = new List<short>(OldUser?.UserAccess?.TafsilGroupList ?? new List<short>());
				List<Guid> list3 = new List<Guid>(OldUser?.UserAccess?.TafsilGroupJustListGuid ?? new List<Guid>());
				num = 22;
				list2.AddRange(NewUser?.UserAccess?.TafsilGroupList);
				list3.AddRange(NewUser?.UserAccess?.TafsilGroupJustListGuid);
				num = 23;
				list2 = list2?.Distinct()?.ToList();
				List<Guid> source = list3?.Distinct()?.ToList();
				num = 24;
				if (list2.Count() != (OldUser?.UserAccess?.TafsilGroupList?.Count).GetValueOrDefault() || (OldUser?.UserAccess?.TafsilGroupList?.Count).GetValueOrDefault() != (NewUser?.UserAccess?.TafsilGroupList?.Count).GetValueOrDefault())
				{
					ResetHesabTree = true;
					num = 250;
				}
				if (source.Count() != (OldUser?.UserAccess?.TafsilGroupJustListGuid?.Count).GetValueOrDefault() || (OldUser?.UserAccess?.TafsilGroupJustListGuid?.Count).GetValueOrDefault() != (NewUser?.UserAccess?.TafsilGroupJustListGuid?.Count).GetValueOrDefault())
				{
					ResetHesabTree = true;
					num = 251;
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"The Error Line IS :{num} ", "EvaluateChanges", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Access\\UserAccessChanges.cs", 292);
			}
		}
	}
}
