namespace Microsoft.SqlServer.Management.XEvent;

public interface IEventProvider
{
	string GetCreateScript();

	string GetDropScript();
}
