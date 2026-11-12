using System;

namespace Stimulsoft.Base.Server;

public interface IStiAccountLoginMenuControl
{
	event EventHandler LogoutClick;

	event EventHandler AccountClick;

	event EventHandler GoToCloudClick;

	event EventHandler CreateTeamClick;

	void Localize();

	int GetHeightControl();
}
