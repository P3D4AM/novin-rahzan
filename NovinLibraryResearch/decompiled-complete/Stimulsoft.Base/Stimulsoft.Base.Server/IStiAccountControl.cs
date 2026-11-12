using System;

namespace Stimulsoft.Base.Server;

public interface IStiAccountControl
{
	event EventHandler Closed;

	void Localize();

	void SelectedMyTeam();
}
