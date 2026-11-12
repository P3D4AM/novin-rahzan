using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Data.SqlClient;

internal static class SNINativeManagedWrapperX64
{
	private const string SNI = "Microsoft.Data.SqlClient.SNI.x64.dll";

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SNIAddProviderWrapper")]
	internal static extern uint SNIAddProvider(SNIHandle pConn, SNINativeMethodWrapper.ProviderEnum ProvNum, [In] ref uint pInfo);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint SNIAddProviderWrapper(SNIHandle pConn, SNINativeMethodWrapper.ProviderEnum ProvNum, [In] ref SNINativeMethodWrapper.SNICTAIPProviderInfo pInfo);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint SNIAddProviderWrapper(SNIHandle pConn, SNINativeMethodWrapper.ProviderEnum ProvNum, [In] ref SNINativeMethodWrapper.AuthProviderInfo pInfo);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SNICheckConnectionWrapper")]
	internal static extern uint SNICheckConnection([In] SNIHandle pConn);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SNICloseWrapper")]
	internal static extern uint SNIClose(IntPtr pConn);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern void SNIGetLastError(out SNINativeMethodWrapper.SNI_Error pErrorStruct);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern void SNIPacketRelease(IntPtr pPacket);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SNIPacketResetWrapper")]
	internal static extern void SNIPacketReset([In] SNIHandle pConn, SNINativeMethodWrapper.IOType IOType, SNIPacket pPacket, SNINativeMethodWrapper.ConsumerNumber ConsNum);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint SNIQueryInfo(SNINativeMethodWrapper.QTypes QType, ref uint pbQInfo);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint SNIQueryInfo(SNINativeMethodWrapper.QTypes QType, ref IntPtr pbQInfo);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SNIReadAsyncWrapper")]
	internal static extern uint SNIReadAsync(SNIHandle pConn, ref IntPtr ppNewPacket);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint SNIReadSyncOverAsync(SNIHandle pConn, ref IntPtr ppNewPacket, int timeout);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SNIRemoveProviderWrapper")]
	internal static extern uint SNIRemoveProvider(SNIHandle pConn, SNINativeMethodWrapper.ProviderEnum ProvNum);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint SNISecInitPackage(ref uint pcbMaxToken);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SNISetInfoWrapper")]
	internal static extern uint SNISetInfo(SNIHandle pConn, SNINativeMethodWrapper.QTypes QType, [In] ref uint pbQInfo);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint SNITerminate();

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "SNIWaitForSSLHandshakeToCompleteWrapper")]
	internal static extern uint SNIWaitForSSLHandshakeToComplete([In] SNIHandle pConn, int dwMilliseconds, out uint pProtocolVersion);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint UnmanagedIsTokenRestricted([In] IntPtr token, [MarshalAs(UnmanagedType.Bool)] out bool isRestricted);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint GetSniMaxComposedSpnLength();

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint SNIGetInfoWrapper([In] SNIHandle pConn, SNINativeMethodWrapper.QTypes QType, out Guid pbQInfo);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint SNIGetInfoWrapper([In] SNIHandle pConn, SNINativeMethodWrapper.QTypes QType, [MarshalAs(UnmanagedType.Bool)] out bool pbQInfo);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint SNIGetInfoWrapper([In] SNIHandle pConn, SNINativeMethodWrapper.QTypes QType, ref IntPtr pbQInfo);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint SNIGetInfoWrapper([In] SNIHandle pConn, SNINativeMethodWrapper.QTypes QType, out ushort portNum);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
	internal static extern uint SNIGetPeerAddrStrWrapper([In] SNIHandle pConn, int bufferSize, StringBuilder addrBuffer, out uint addrLen);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint SNIGetInfoWrapper([In] SNIHandle pConn, SNINativeMethodWrapper.QTypes QType, out SNINativeMethodWrapper.ProviderEnum provNum);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint SNIInitialize([In] IntPtr pmo);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint SNIOpenSyncExWrapper(ref SNINativeMethodWrapper.SNI_CLIENT_CONSUMER_INFO pClientConsumerInfo, out IntPtr ppConn);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint SNIOpenWrapper([In] ref SNINativeMethodWrapper.Sni_Consumer_Info pConsumerInfo, [MarshalAs(UnmanagedType.LPWStr)] string szConnect, [In] SNIHandle pConn, out IntPtr ppConn, [MarshalAs(UnmanagedType.Bool)] bool fSync, SqlConnectionIPAddressPreference ipPreference, [In] ref SNINativeMethodWrapper.SNI_DNSCache_Info pDNSCachedInfo);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern IntPtr SNIPacketAllocateWrapper([In] SafeHandle pConn, SNINativeMethodWrapper.IOType IOType);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint SNIPacketGetDataWrapper([In] IntPtr packet, [In][Out] byte[] readBuffer, uint readBufferLength, out uint dataSize);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal unsafe static extern void SNIPacketSetData(SNIPacket pPacket, [In] byte* pbBuf, uint cbBuf);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal unsafe static extern uint SNISecGenClientContextWrapper([In] SNIHandle pConn, [In][Out] byte[] pIn, uint cbIn, [In][Out] byte[] pOut, [In] ref uint pcbOut, [MarshalAs(UnmanagedType.Bool)] out bool pfDone, byte* szServerInfo, uint cbServerInfo, [MarshalAs(UnmanagedType.LPWStr)] string pwszUserName, [MarshalAs(UnmanagedType.LPWStr)] string pwszPassword);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint SNIWriteAsyncWrapper(SNIHandle pConn, [In] SNIPacket pPacket);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern uint SNIWriteSyncOverAsync(SNIHandle pConn, [In] SNIPacket pPacket);

	[DllImport("Microsoft.Data.SqlClient.SNI.x64.dll", CallingConvention = CallingConvention.Cdecl)]
	internal static extern IntPtr SNIClientCertificateFallbackWrapper(IntPtr pCallbackContext);
}
