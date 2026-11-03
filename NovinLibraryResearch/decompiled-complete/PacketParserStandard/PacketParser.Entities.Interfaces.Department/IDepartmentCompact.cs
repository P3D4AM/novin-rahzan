using System;

namespace PacketParser.Entities.Interfaces.Department;

public interface IDepartmentCompact
{
	Guid Guid { get; set; }

	short Id { get; set; }

	string Name { get; set; }

	Guid TafsilGuid { get; set; }

	string User { get; set; }

	string Password { get; set; }

	DateTime DateExpire { get; set; }

	string URL { get; set; }

	string StrType { get; set; }

	short AndroidCount { get; set; }

	short BajeCount { get; set; }

	short PrintCount { get; set; }

	bool HaveBKWebService { get; set; }

	string SefareshgirLevel { get; set; }

	string Description { get; set; }

	bool GPS { get; set; }

	DateTime Date { get; set; }

	string Mail { get; set; }

	string Tel { get; set; }

	bool RestApi { get; set; }

	bool WebSiteWordPress { get; set; }

	bool CloudeAttendance { get; set; }
}
