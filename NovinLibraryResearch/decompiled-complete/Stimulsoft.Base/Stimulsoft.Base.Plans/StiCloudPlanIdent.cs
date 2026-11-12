using System;

namespace Stimulsoft.Base.Plans;

[Flags]
public enum StiCloudPlanIdent
{
	Trial = 0,
	RBase = 1,
	RSingle = 2,
	RTeam = 4,
	REnterprise = 8,
	DBase = 0x20,
	DSingle = 0x40,
	DTeam = 0x80,
	DEnterprise = 0x100,
	Developer = 1,
	Expiried = 0x200
}
