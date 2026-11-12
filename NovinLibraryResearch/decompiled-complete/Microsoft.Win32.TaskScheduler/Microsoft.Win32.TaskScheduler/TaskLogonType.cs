using System.ComponentModel;

namespace Microsoft.Win32.TaskScheduler;

[DefaultValue(TaskLogonType.S4U)]
public enum TaskLogonType
{
	None,
	Password,
	S4U,
	InteractiveToken,
	Group,
	ServiceAccount,
	InteractiveTokenOrPassword
}
