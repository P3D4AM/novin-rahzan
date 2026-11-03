namespace AxRealSvrOcxTcpLib;

public class _DRealSvrOcxTcpEvents_OnReceiveGLogTextEvent
{
	public string astrClientIP;

	public int anClientPort;

	public string astrLogText;

	public _DRealSvrOcxTcpEvents_OnReceiveGLogTextEvent(string astrClientIP, int anClientPort, string astrLogText)
	{
		this.astrClientIP = astrClientIP;
		this.anClientPort = anClientPort;
		this.astrLogText = astrLogText;
	}
}
