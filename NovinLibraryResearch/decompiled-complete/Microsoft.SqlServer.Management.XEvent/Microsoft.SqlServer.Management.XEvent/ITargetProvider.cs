namespace Microsoft.SqlServer.Management.XEvent;

public interface ITargetProvider
{
	string GetCreateScript();

	string GetDropScript();

	string GetTargetData();
}
