namespace PacketParser.EventArgs;

public class CustomerPasswordRecoveryRequestEventArgs : RecoverPasswordModel
{
	public string Message { get; set; }

	public static CustomerPasswordRecoveryRequestEventArgs FromString(Packet pack, string data)
	{
		CustomerPasswordRecoveryRequestEventArgs customerPasswordRecoveryRequestEventArgs = data.FromJson<CustomerPasswordRecoveryRequestEventArgs>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\Customer\\CustomerPasswordRecoveryRequestEventArgs.cs", 8);
		customerPasswordRecoveryRequestEventArgs.Part = pack.Part;
		customerPasswordRecoveryRequestEventArgs.TotalParts = pack.TotalParts;
		customerPasswordRecoveryRequestEventArgs.PacketReferenceId = pack.PacketReferenceId;
		return customerPasswordRecoveryRequestEventArgs;
	}
}
