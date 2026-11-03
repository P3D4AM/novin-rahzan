using System;
using System.Collections.Generic;
using PacketParser.DefaultsCoding;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.Settings.Accounting.UserSettings;

namespace PacketParser.Entities;

public class WebCashier : Serializable<WebCashier>, ICashier, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public short Id { get; set; }

	public Guid Guid { get; set; }

	public Guid UserGuid { get; set; }

	public DateTime Modified { get; set; }

	public string Name { get; set; }

	public string UserName { get; set; }

	public byte[] HashedPassword { get; set; }

	public string Description { get; set; }

	public DateTime DateM { get; set; }

	public bool Status { get; set; }

	public List<CashierTime> CashierTime { get; set; }

	public UserSettings Settings { get; set; }

	public Guid? SandoghGuid { get; set; }

	public Guid? DepotGuid { get; set; }

	public Guid? VisitorGuid { get; set; }

	public Guid? BankGuid1 { get; set; }

	public Guid? BankGuid2 { get; set; }

	public Guid? BankGuid3 { get; set; }

	public Guid? BankGuid4 { get; set; }

	public Guid? CusGuid { get; set; }

	public Guid? DefaultCusGroup { get; set; }

	public bool AllVisitorAccess { get; set; }

	public bool AllSandoghsAccess { get; set; }

	public bool AllDepotsAccess { get; set; }

	public bool AllBanksAccess { get; set; }

	public short LoginAccess { get; set; }

	public Guid? ReceptionSmsCustomerGuid { get; set; }

	public Guid? PardakhtSmsCustomerGuid { get; set; }

	public Guid FareMoeinGuid { get; set; } = ParentDefaults.MoeinCoding.CLSMoein31;


	public Guid? FareTafsilGuid { get; set; } = ParentDefaults.TafsilCoding.CLSTafsil29771;


	public static List<Packet> GenerateSaveRequest(IEnumerable<WebCashier> cashiers, int partitionId)
	{
		throw new NotImplementedException();
	}
}
