namespace PacketParser.PosToPc.Sadad.Classes;

internal static class MagicConsts
{
	public static int PCPOS_AMOUNT_LEN;

	public static int PCPOS_BILLLEN_LEN;

	public static int PCPOS_BILLNO_LEN;

	public static int PCPOS_CARDNO_LEN;

	public static int PCPOS_DATE_LEN;

	public static string PCPOS_DEFAULT_MAC;

	public static int PCPOS_FISHSEQ_LEN;

	public static string PCPOS_GET_BALANCE_TRCOD;

	public static int PCPOS_MACNO_LEN;

	public static int PCPOS_MERCHANT_LEN;

	public static string PCPOS_MSG_VER;

	public static int PCPOS_POSRES_LEN;

	public static string PCPOS_QUERY_FISHNO_TRCOD;

	public static string PCPOS_QUERY_LAST_TRCOD;

	public static string PCPOS_QUERY_REFNO_TRCOD;

	public static int PCPOS_REFNO_LEN;

	public static int PCPOS_RESERVE1_LEN;

	public static int PCPOS_SWITCHRES_LEN;

	public static int PCPOS_TERMINAL_LEN;

	public static int PCPOS_TIME_LEN;

	public static int PCPOS_TRACENO_LEN;

	public static string PCPOS_TRANS_TRCOD;

	public static int PCPOS_TRANSCODE_LEN;

	public static byte SERIALPOS_SNDRCV_FOOTER;

	public static byte SERIALPOS_SNDRCV_HEADER;

	public static int START_LEN;

	static MagicConsts()
	{
		PCPOS_AMOUNT_LEN = 12;
		PCPOS_BILLLEN_LEN = 2;
		PCPOS_BILLNO_LEN = 24;
		PCPOS_CARDNO_LEN = 19;
		PCPOS_DATE_LEN = 8;
		PCPOS_DEFAULT_MAC = "                ";
		PCPOS_FISHSEQ_LEN = 2;
		PCPOS_GET_BALANCE_TRCOD = "02";
		PCPOS_MACNO_LEN = 12;
		PCPOS_MERCHANT_LEN = 9;
		PCPOS_MSG_VER = "300";
		PCPOS_POSRES_LEN = 2;
		PCPOS_QUERY_FISHNO_TRCOD = "04";
		PCPOS_QUERY_LAST_TRCOD = "05";
		PCPOS_QUERY_REFNO_TRCOD = "03";
		PCPOS_REFNO_LEN = 12;
		PCPOS_RESERVE1_LEN = 2;
		PCPOS_SWITCHRES_LEN = 2;
		PCPOS_TERMINAL_LEN = 8;
		PCPOS_TIME_LEN = 6;
		PCPOS_TRACENO_LEN = 6;
		PCPOS_TRANS_TRCOD = "01";
		PCPOS_TRANSCODE_LEN = 2;
		SERIALPOS_SNDRCV_FOOTER = 3;
		SERIALPOS_SNDRCV_HEADER = 2;
		START_LEN = 4;
	}
}
