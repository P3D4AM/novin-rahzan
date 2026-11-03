using Riss.Devices;

internal class l : z
{
	public override int s()
	{
		if (l() == null)
		{
			switch (n().CommunicationType)
			{
			case CommunicationType.Serial:
				a(new w());
				break;
			case CommunicationType.Usb:
				a(new ab());
				break;
			case CommunicationType.Tcp:
				a(new e());
				break;
			default:
				a((ZdCommBase)null);
				break;
			}
		}
		else
		{
			q();
		}
		int num = a();
		if (num == 0)
		{
			num = a();
		}
		return num;
	}
}
