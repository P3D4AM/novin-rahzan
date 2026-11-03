namespace AxRealSvrOcxTcpLib;

public class _DRealSvrOcxTcpEvents_OnReceiveGLogTextOnDoorOpenEvent
{
	public string astrClientIP;

	public int anClientPort;

	public string astrLogText;

	public string astrLogImage;

	public _DRealSvrOcxTcpEvents_OnReceiveGLogTextOnDoorOpenEvent(string astrClientIP, int anClientPort, string astrLogText, string astrLogImage)
	{
		this.astrClientIP = astrClientIP;
		this.anClientPort = anClientPort;
		this.astrLogText = astrLogText;
		this.astrLogImage = astrLogImage;
	}
}
