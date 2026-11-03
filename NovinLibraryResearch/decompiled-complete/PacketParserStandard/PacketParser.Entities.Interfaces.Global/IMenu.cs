using System;

namespace PacketParser.Entities.Interfaces.Global;

public interface IMenu : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid? MasterGuid { get; set; }

	int RowCount { get; set; }

	int ColumnCount { get; set; }

	string Title { get; set; }

	string Description { get; set; }

	EnMenuType Type { get; set; }
}
