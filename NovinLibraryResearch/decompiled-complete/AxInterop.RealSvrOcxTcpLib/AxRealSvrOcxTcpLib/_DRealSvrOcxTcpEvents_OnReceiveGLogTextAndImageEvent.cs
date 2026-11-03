namespace AxRealSvrOcxTcpLib;

public class _DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEvent
{
	public string astrClientIP;

	public int anClientPort;

	public string astrLogText;

	public string astrLogImage;

	public _DRealSvrOcxTcpEvents_OnReceiveGLogTextAndImageEvent(string astrClientIP, int anClientPort, string astrLogText, string astrLogImage)
	{
		this.astrClientIP = astrClientIP;
		this.anClientPort = anClientPort;
		this.astrLogText = astrLogText;
		this.astrLogImage = astrLogImage;
	}
}
