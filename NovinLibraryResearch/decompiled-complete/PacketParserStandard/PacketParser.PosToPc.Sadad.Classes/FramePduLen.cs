namespace PacketParser.PosToPc.Sadad.Classes;

internal class FramePduLen
{
	public static int Footer;

	public static int Header;

	public static int Length;

	public static int Version;

	public static int Sync;

	public static int Lrc;

	public static int PacketType;

	static FramePduLen()
	{
		Footer = 1;
		Header = 1;
		Length = 3;
		Version = 3;
		Sync = 1;
		Lrc = 1;
		PacketType = 1;
	}
}
