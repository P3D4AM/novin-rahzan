using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.XEvent;

public interface ISessionObject
{
	SfcObjectState State { get; }

	string GetCreateScript();

	string GetDropScript();

	bool IsDirty();
}
