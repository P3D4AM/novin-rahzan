using System.Runtime.InteropServices;

namespace ADODB;

[ComVisible(false)]
[ComEventInterface(typeof(ConnectionEvents_0000), typeof(ConnectionEvents_EventProvider_0000))]
public interface ConnectionEvents_Event
{
	event ConnectionEvents_InfoMessageEventHandler InfoMessage;

	event ConnectionEvents_BeginTransCompleteEventHandler BeginTransComplete;

	event ConnectionEvents_CommitTransCompleteEventHandler CommitTransComplete;

	event ConnectionEvents_RollbackTransCompleteEventHandler RollbackTransComplete;

	event ConnectionEvents_WillExecuteEventHandler WillExecute;

	event ConnectionEvents_ExecuteCompleteEventHandler ExecuteComplete;

	event ConnectionEvents_WillConnectEventHandler WillConnect;

	event ConnectionEvents_ConnectCompleteEventHandler ConnectComplete;

	event ConnectionEvents_DisconnectEventHandler Disconnect;
}
