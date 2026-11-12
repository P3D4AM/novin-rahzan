namespace Stimulsoft.Base;

public interface IStiAppComponent : IStiAppCell
{
	string GetName();

	IStiApp GetApp();
}
