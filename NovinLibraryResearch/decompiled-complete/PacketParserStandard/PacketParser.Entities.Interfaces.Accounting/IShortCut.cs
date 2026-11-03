using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IShortCut : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string FormName { get; set; }

	string Name { get; set; }

	int ShortCutKey { get; set; }

	string Func { get; set; }

	Guid CashierGuid { get; set; }

	string Description { get; set; }

	string BackColor { get; set; }

	string ForeColor { get; set; }

	string ImageSrc { get; set; }

	bool Enable { get; set; }

	bool Visable { get; set; }

	short SubItems { get; set; }
}
