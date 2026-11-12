using System.Xml.Serialization;

namespace Microsoft.Win32.TaskScheduler;

public enum TaskRunLevel
{
	[XmlEnum("LeastPrivilege")]
	LUA,
	[XmlEnum("HighestAvailable")]
	Highest
}
