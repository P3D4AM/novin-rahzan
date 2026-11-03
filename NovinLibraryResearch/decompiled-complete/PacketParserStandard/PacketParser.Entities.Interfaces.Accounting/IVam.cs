using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IVam : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	int Id2 { get; set; }

	decimal Zarib { get; set; }

	string Description { get; set; }

	DateTime DateM { get; set; }

	Guid? PardakhtGuid { get; set; }

	Guid VamMoeinGuid { get; set; }

	Guid VamTafsilGuid { get; set; }

	Guid SepordeTafsilGuid { get; set; }

	Guid SepordeMoeinGuid { get; set; }

	Guid? AghsatGuid { get; set; }

	decimal Sum { get; set; }

	Guid? ZamenGuid1 { get; set; }

	Guid? ZamenGuid2 { get; set; }

	Guid? ZamenGuid3 { get; set; }

	Guid? ZamenGuid4 { get; set; }

	DateTime Date1 { get; set; }

	DateTime Date2 { get; set; }

	short Average { get; set; }

	ENMoadel AverageType { get; set; }

	decimal AverageSum { get; set; }

	decimal ProfitPercent { get; set; }
}
