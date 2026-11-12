using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public interface ISessionProvider
{
	ISfcScript GetCreateScript();

	ISfcScript GetAlterScript();

	ISfcScript GetDropScript();

	void ValidateAlter();

	void Start();

	void Stop();
}
