using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IUser : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	long AndroidIme { get; set; }

	DateTime DateRegister { get; set; }

	DateTime LastLogin { get; set; }

	string DateRegisterSh { get; }

	Guid DefDepotGuid { get; set; }

	short DepId { get; set; }

	string Description { get; set; }

	DateTime ExpireDate { get; set; }

	string ExpireDateSh { get; }

	string Mail { get; set; }

	string Mob { get; set; }

	string Name { get; set; }

	string PanelDomain { get; set; }

	string PanelPass { get; set; }

	string PanelUName { get; set; }

	string Pass { get; set; }

	string PC_Serial { get; set; }

	string Server_Parameters { get; set; }

	string SMSLine { get; set; }

	SMSSendType SmsType { get; set; }

	ENSource Type { get; set; }

	Guid UserAccGuid { get; set; }

	string UserFullName { get; set; }

	bool IsTest { get; set; }
}
