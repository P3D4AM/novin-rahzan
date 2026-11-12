namespace mCore;

public interface ISMS
{
	string About { get; }

	string Port { get; set; }

	BaudRate BaudRate { get; set; }

	Parity Parity { get; set; }

	DataBits DataBits { get; set; }

	StopBits StopBits { get; set; }

	FlowControl FlowControl { get; set; }

	long Timeout { get; set; }

	bool IsConnected { get; }

	MessageMemory MessageMemory { get; set; }

	string Manufacturer { get; }

	string Model { get; }

	string IMEI { get; }

	string IMSI { get; }

	string Revision { get; }

	int SignalStrength { get; }

	int SignalStrengthDB { get; }

	string SMSC { get; set; }

	string Network { get; }

	string Validity { get; set; }

	LongMessage LongMessage { get; set; }

	bool DisableCheckPIN { get; set; }

	string PIN { set; }

	int DelayAfterPIN { get; set; }

	int SendDelay { get; set; }

	int SendRetry { get; set; }

	Encoding Encoding { get; set; }

	int BatteryLevel { get; }

	string OwnNumber { get; }

	bool DebugMode { get; set; }

	LogType LogType { get; set; }

	string LogFolderPath { get; set; }

	long LogSize { get; }

	long ErrorCode { get; }

	string ErrorDescription { get; }

	string ModemInitString { get; set; }

	bool DeliveryReport { get; set; }

	bool NewMessageIndication { get; set; }

	bool NewUSSDIndication { get; set; }

	bool NewMessageConcatenate { get; set; }

	bool AutoDeleteNewMessage { get; set; }

	bool IncomingCallIndication { get; set; }

	bool ReconnectOnSendError { get; set; }

	event SMS.NewMessageReceivedEventHandler NewMessageReceived;

	event SMS.NewDeliveryReportEventHandler NewDeliveryReport;

	event SMS.NewIncomingCallEventHandler NewIncomingCall;

	event SMS.NewUSSDReceivedEventhandler NewUSSDReceived;

	event SMS.QueueSMSSentEventHandler QueueSMSSent;

	event SMS.QueueWapPushSentEventHandler QueueWapPushSent;

	event SMS.QueueVCardSentEventHandler QueueVCardSent;

	event SMS.QueueVCalendarSentEventHandler QueueVCalendarSent;

	event SMS.QueueSmartMessageSentEventHandler QueueSmartMessageSent;

	event SMS.QueueSMSSendingEventHandler QueueSMSSending;

	event SMS.QueueWapPushSendingEventHandler QueueWapPushSending;

	event SMS.QueueVCardSendingEventHandler QueueVCardSending;

	event SMS.QueueVCalendarSendingEventHandler QueueVCalendarSending;

	event SMS.QueueSmartMessageSendingEventHandler QueueSmartMessageSending;

	WapPush WapPush();

	vCard vCard();

	vCalendar vCalendar();

	License License();

	Queue Queue();

	Inbox inbox();

	Message inbox(int Index);

	bool Connect();

	bool Disconnect();

	bool CancelUSSDSession();

	bool SendUSSD(string USSD_Command);

	string SendSMS(string DestinationNumber, string TextMessage);

	string SendSMS(string DestinationNumber, string TextMessage, bool AlertMessage);

	string SendSmartMessage(string DestinationNumber, string TextMessage, int DestinationPort);

	string SendSmartMessage(string DestinationNumber, string TextMessage, int DestinationPort, int SourcePort);

	string SendSmartMessageToQueue(string DestinationNumber, string TextMessage, int DestinationPort);

	string SendSmartMessageToQueue(string DestinationNumber, string TextMessage, int DestinationPort, int SourcePort);

	string SendSmartMessageToQueue(string DestinationNumber, string TextMessage, int DestinationPort, QueuePriority Priority);

	string SendSmartMessageToQueue(string DestinationNumber, string TextMessage, int DestinationPort, int SourcePort, QueuePriority Priority);

	string SendSMSToQueue(string DestinationNumber, string TextMessage);

	string SendSMSToQueue(string DestinationNumber, string TextMessage, QueuePriority Priority);

	string SendSMSToQueue(string DestinationNumber, string TextMessage, QueuePriority Priority, bool AlertMessage);

	bool ClearLog();

	bool ClearLog(int KeepLinesInLog);

	bool IsError();

	bool IsError(bool ShowMsgBox);

	bool IsError(bool ShowMsgBox, string MsgBoxTitle);

	string Command(string ATCommand);
}
