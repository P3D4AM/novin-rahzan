namespace Stimulsoft.Base.Server;

public interface IStiProfileWindow
{
	bool? ShowDialog();

	bool? ShowDialog(object owner);
}
