using System.Collections.Generic;
using System.Windows.Forms;
using zkemkeeper;

namespace ZK;

public class ZK
{
	public CZKEMClass axCZKEM1 = new CZKEMClass();

	private bool _bIsConnected;

	private int _iMachineNumber = 1;

	private int _Port = 5005;

	private string _IP = "";

	private string _Pass = "";

	private int idwErrorCode;

	public bool Conected => _bIsConnected;

	public ZK(int MachineNum, string IP, int port, string Pass)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		_iMachineNumber = MachineNum;
		_IP = IP;
		_Port = port;
		_Pass = Pass;
		_bIsConnected = axCZKEM1.Connect_Net(IP, port);
		GetLog();
	}

	public List<Record> GetLog()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		if (!_bIsConnected)
		{
			MessageBox.Show("Please connect the device first", "Error");
			return new List<Record>();
		}
		List<Record> list = new List<Record>();
		int num = 0;
		string sdwEnrollNumber = "";
		int idwVerifyMode = 0;
		int idwInOutMode = 0;
		int idwYear = 0;
		int idwMonth = 0;
		int idwDay = 0;
		int idwHour = 0;
		int idwMinute = 0;
		int idwSecond = 0;
		int idwWorkcode = 0;
		int num2 = 0;
		int num3 = 0;
		axCZKEM1.EnableDevice(_iMachineNumber, false);
		if (axCZKEM1.ReadGeneralLogData(_iMachineNumber))
		{
			while (axCZKEM1.SSR_GetGeneralLogData(_iMachineNumber, ref sdwEnrollNumber, ref idwVerifyMode, ref idwInOutMode, ref idwYear, ref idwMonth, ref idwDay, ref idwHour, ref idwMinute, ref idwSecond, ref idwWorkcode))
			{
				num2++;
				list.Add(new Record(_iMachineNumber, sdwEnrollNumber, idwVerifyMode, idwInOutMode, idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond, idwWorkcode));
				num3++;
			}
		}
		else
		{
			axCZKEM1.GetLastError(ref num);
			if (num != 0)
			{
				MessageBox.Show("Reading data from terminal failed,ErrorCode: " + num, "Error");
			}
			else
			{
				MessageBox.Show("No data from terminal returns!", "Error");
			}
		}
		axCZKEM1.EnableDevice(_iMachineNumber, true);
		return list;
	}

	public bool Disconnect()
	{
		axCZKEM1.GetLastError(ref idwErrorCode);
		_bIsConnected = false;
		return _bIsConnected;
	}
}
