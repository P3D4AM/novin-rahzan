using System;

namespace AxRealSvrOcxTcpLib;

public class _DRealSvrOcxTcpEvents_OnReceiveGLogDataEvent
{
	public string astrDeviceIP;

	public int anDevicePort;

	public int anDeviceID;

	public int anSEnrollNumber;

	public int anVerifyMode;

	public int anInOutMode;

	public DateTime anLogDate;

	public string astrSerialNo;

	public _DRealSvrOcxTcpEvents_OnReceiveGLogDataEvent(string astrDeviceIP, int anDevicePort, int anDeviceID, int anSEnrollNumber, int anVerifyMode, int anInOutMode, DateTime anLogDate, string astrSerialNo)
	{
		this.astrDeviceIP = astrDeviceIP;
		this.anDevicePort = anDevicePort;
		this.anDeviceID = anDeviceID;
		this.anSEnrollNumber = anSEnrollNumber;
		this.anVerifyMode = anVerifyMode;
		this.anInOutMode = anInOutMode;
		this.anLogDate = anLogDate;
		this.astrSerialNo = astrSerialNo;
	}
}
