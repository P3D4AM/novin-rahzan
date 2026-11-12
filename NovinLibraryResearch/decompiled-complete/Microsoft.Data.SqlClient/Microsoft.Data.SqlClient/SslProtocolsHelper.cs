using System;

namespace Microsoft.Data.SqlClient;

internal static class SslProtocolsHelper
{
	[Flags]
	private enum NativeProtocols
	{
		SP_PROT_SSL2_SERVER = 4,
		SP_PROT_SSL2_CLIENT = 8,
		SP_PROT_SSL3_SERVER = 0x10,
		SP_PROT_SSL3_CLIENT = 0x20,
		SP_PROT_TLS1_0_SERVER = 0x40,
		SP_PROT_TLS1_0_CLIENT = 0x80,
		SP_PROT_TLS1_1_SERVER = 0x100,
		SP_PROT_TLS1_1_CLIENT = 0x200,
		SP_PROT_TLS1_2_SERVER = 0x400,
		SP_PROT_TLS1_2_CLIENT = 0x800,
		SP_PROT_TLS1_3_SERVER = 0x1000,
		SP_PROT_TLS1_3_CLIENT = 0x2000,
		SP_PROT_SSL2 = 0xC,
		SP_PROT_SSL3 = 0x30,
		SP_PROT_TLS1_0 = 0xC0,
		SP_PROT_TLS1_1 = 0x300,
		SP_PROT_TLS1_2 = 0xC00,
		SP_PROT_TLS1_3 = 0x3000,
		SP_PROT_NONE = 0
	}

	private static string ToFriendlyName(this NativeProtocols protocol)
	{
		if (protocol.HasFlag(NativeProtocols.SP_PROT_TLS1_3_CLIENT) || protocol.HasFlag(NativeProtocols.SP_PROT_TLS1_3_SERVER))
		{
			return "TLS 1.3";
		}
		if (protocol.HasFlag(NativeProtocols.SP_PROT_TLS1_2_CLIENT) || protocol.HasFlag(NativeProtocols.SP_PROT_TLS1_2_SERVER))
		{
			return "TLS 1.2";
		}
		if (protocol.HasFlag(NativeProtocols.SP_PROT_TLS1_1_CLIENT) || protocol.HasFlag(NativeProtocols.SP_PROT_TLS1_1_SERVER))
		{
			return "TLS 1.1";
		}
		if (protocol.HasFlag(NativeProtocols.SP_PROT_TLS1_0_CLIENT) || protocol.HasFlag(NativeProtocols.SP_PROT_TLS1_0_SERVER))
		{
			return "TLS 1.0";
		}
		if (protocol.HasFlag(NativeProtocols.SP_PROT_SSL3_CLIENT) || protocol.HasFlag(NativeProtocols.SP_PROT_SSL3_SERVER))
		{
			return "SSL 3.0";
		}
		if (protocol.HasFlag(NativeProtocols.SP_PROT_SSL2_CLIENT) || protocol.HasFlag(NativeProtocols.SP_PROT_SSL2_SERVER))
		{
			return "SSL 2.0";
		}
		if (protocol.HasFlag(NativeProtocols.SP_PROT_NONE))
		{
			return "None";
		}
		throw new ArgumentException(StringsHelper.GetString(Strings.net_invalid_enum, "NativeProtocols"), "NativeProtocols");
	}

	public static string GetProtocolWarning(uint protocol)
	{
		string result = string.Empty;
		if ((protocol & 0x33Cu) != 0)
		{
			result = StringsHelper.GetString(Strings.SEC_ProtocolWarning, ((NativeProtocols)protocol).ToFriendlyName());
		}
		return result;
	}
}
