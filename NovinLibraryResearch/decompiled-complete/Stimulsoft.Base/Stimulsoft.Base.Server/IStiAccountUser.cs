using System;

namespace Stimulsoft.Base.Server;

public interface IStiAccountUser
{
	byte[] Picture { get; set; }

	string FirstName { get; set; }

	string LastName { get; set; }

	string UserName { get; set; }

	string UserKey { get; set; }

	string SessionKey { get; set; }

	DateTime? SessionDate { get; set; }

	DateTime Created { get; set; }

	StiDesignerSpecification DesignerSpecification { get; set; }

	bool Activated { get; set; }

	bool Expired { get; set; }

	int Days { get; set; }

	void SaveToFile();
}
