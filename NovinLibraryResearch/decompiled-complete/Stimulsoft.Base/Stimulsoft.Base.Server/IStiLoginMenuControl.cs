namespace Stimulsoft.Base.Server;

public interface IStiLoginMenuControl
{
	bool IsFirstStart { get; }

	void CheckThemeState();

	bool ShowAuthorizationWindow(bool checkClose = false);

	void Localize();
}
