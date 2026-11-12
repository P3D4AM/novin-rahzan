namespace Stimulsoft.Base.Server;

public interface IStiChangePasswordWindow
{
	bool? ShowDialog();

	bool? ShowDialog(object owner);
}
