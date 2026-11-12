#define TRACE
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

internal static class SNINativeMethodWrapper
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	internal delegate void SqlAsyncCallbackDelegate(IntPtr m_ConsKey, IntPtr pPacket, uint dwError);

	internal delegate IntPtr SqlClientCertificateDelegate(IntPtr pCallbackContext);

	internal class SqlDependencyProcessDispatcherStorage
	{
		private unsafe static void* data;

		private static int size;

		private static volatile int thelock;

		public unsafe static void* NativeGetData(out int passedSize)
		{
			passedSize = size;
			return data;
		}

		internal unsafe static bool NativeSetData(void* passedData, int passedSize)
		{
			bool result = false;
			while (Interlocked.CompareExchange(ref thelock, 1, 0) != 0)
			{
				Thread.Sleep(50);
			}
			Trace.Assert(1 == thelock);
			if (null == data)
			{
				data = Marshal.AllocHGlobal(passedSize).ToPointer();
				Trace.Assert(null != data);
				Buffer.MemoryCopy(passedData, data, passedSize, passedSize);
				Trace.Assert(size == 0);
				size = passedSize;
				result = true;
			}
			int num = Interlocked.CompareExchange(ref thelock, 0, 1);
			Trace.Assert(1 == num);
			return result;
		}
	}

	internal enum SniSpecialErrors : uint
	{
		LocalDBErrorCode = 50u,
		MultiSubnetFailoverWithMoreThan64IPs = 47u,
		MultiSubnetFailoverWithInstanceSpecified = 48u,
		MultiSubnetFailoverWithNonTcpProtocol = 49u,
		MaxErrorValue = 50157u
	}

	internal struct ConsumerInfo
	{
		internal int defaultBufferSize;

		internal SqlAsyncCallbackDelegate readDelegate;

		internal SqlAsyncCallbackDelegate writeDelegate;

		internal IntPtr key;
	}

	internal struct AuthProviderInfo
	{
		internal uint flags;

		internal string certId;

		internal bool certHash;

		internal object clientCertificateCallbackContext;

		internal SqlClientCertificateDelegate clientCertificateCallback;
	}

	internal struct CTAIPProviderInfo
	{
		internal byte[] originalNetworkAddress;

		internal bool fromDataSecurityProxy;
	}

	private struct SNIAuthProviderInfoWrapper
	{
		internal object pDelegateContext;

		internal SqlClientCertificateDelegate pSqlClientCertificateDelegate;
	}

	internal struct SNICTAIPProviderInfo
	{
		internal SNIHandle pConn;

		internal byte prgbAddress;

		internal ulong cbAddress;

		internal bool fFromDataSecurityProxy;
	}

	internal struct CredHandle
	{
		internal IntPtr dwLower;

		internal IntPtr dwUpper;
	}

	internal enum ContextAttribute
	{
		SECPKG_ATTR_SIZES = 0,
		SECPKG_ATTR_NAMES = 1,
		SECPKG_ATTR_LIFESPAN = 2,
		SECPKG_ATTR_DCE_INFO = 3,
		SECPKG_ATTR_STREAM_SIZES = 4,
		SECPKG_ATTR_AUTHORITY = 6,
		SECPKG_ATTR_PACKAGE_INFO = 10,
		SECPKG_ATTR_NEGOTIATION_INFO = 12,
		SECPKG_ATTR_UNIQUE_BINDINGS = 25,
		SECPKG_ATTR_ENDPOINT_BINDINGS = 26,
		SECPKG_ATTR_CLIENT_SPECIFIED_TARGET = 27,
		SECPKG_ATTR_APPLICATION_PROTOCOL = 35,
		SECPKG_ATTR_REMOTE_CERT_CONTEXT = 83,
		SECPKG_ATTR_LOCAL_CERT_CONTEXT = 84,
		SECPKG_ATTR_ROOT_STORE = 85,
		SECPKG_ATTR_ISSUER_LIST_EX = 89,
		SECPKG_ATTR_CONNECTION_INFO = 90,
		SECPKG_ATTR_UI_INFO = 104
	}

	internal enum ConsumerNumber
	{
		SNI_Consumer_SNI,
		SNI_Consumer_SSB,
		SNI_Consumer_PacketIsReleased,
		SNI_Consumer_Invalid
	}

	internal enum IOType
	{
		READ,
		WRITE
	}

	internal enum PrefixEnum
	{
		UNKNOWN_PREFIX,
		SM_PREFIX,
		TCP_PREFIX,
		NP_PREFIX,
		VIA_PREFIX,
		INVALID_PREFIX
	}

	internal enum ProviderEnum
	{
		HTTP_PROV,
		NP_PROV,
		SESSION_PROV,
		SIGN_PROV,
		SM_PROV,
		SMUX_PROV,
		SSL_PROV,
		TCP_PROV,
		VIA_PROV,
		CTAIP_PROV,
		MAX_PROVS,
		INVALID_PROV
	}

	internal enum QTypes
	{
		SNI_QUERY_CONN_INFO,
		SNI_QUERY_CONN_BUFSIZE,
		SNI_QUERY_CONN_KEY,
		SNI_QUERY_CLIENT_ENCRYPT_POSSIBLE,
		SNI_QUERY_SERVER_ENCRYPT_POSSIBLE,
		SNI_QUERY_CERTIFICATE,
		SNI_QUERY_LOCALDB_HMODULE,
		SNI_QUERY_CONN_ENCRYPT,
		SNI_QUERY_CONN_PROVIDERNUM,
		SNI_QUERY_CONN_CONNID,
		SNI_QUERY_CONN_PARENTCONNID,
		SNI_QUERY_CONN_SECPKG,
		SNI_QUERY_CONN_NETPACKETSIZE,
		SNI_QUERY_CONN_NODENUM,
		SNI_QUERY_CONN_PACKETSRECD,
		SNI_QUERY_CONN_PACKETSSENT,
		SNI_QUERY_CONN_PEERADDR,
		SNI_QUERY_CONN_PEERPORT,
		SNI_QUERY_CONN_LASTREADTIME,
		SNI_QUERY_CONN_LASTWRITETIME,
		SNI_QUERY_CONN_CONSUMER_ID,
		SNI_QUERY_CONN_CONNECTTIME,
		SNI_QUERY_CONN_HTTPENDPOINT,
		SNI_QUERY_CONN_LOCALADDR,
		SNI_QUERY_CONN_LOCALPORT,
		SNI_QUERY_CONN_SSLHANDSHAKESTATE,
		SNI_QUERY_CONN_SOBUFAUTOTUNING,
		SNI_QUERY_CONN_SECPKGNAME,
		SNI_QUERY_CONN_SECPKGMUTUALAUTH,
		SNI_QUERY_CONN_CONSUMERCONNID,
		SNI_QUERY_CONN_SNIUCI,
		SNI_QUERY_CONN_SUPPORTS_EXTENDED_PROTECTION,
		SNI_QUERY_CONN_CHANNEL_PROVIDES_AUTHENTICATION_CONTEXT,
		SNI_QUERY_CONN_PEERID,
		SNI_QUERY_CONN_SUPPORTS_SYNC_OVER_ASYNC,
		SNI_QUERY_CONN_SSL_SECCTXTHANDLE
	}

	internal enum TransparentNetworkResolutionMode : byte
	{
		DisabledMode,
		SequentialMode,
		ParallelMode
	}

	internal struct Sni_Consumer_Info
	{
		public int DefaultUserDataLength;

		public IntPtr ConsumerKey;

		public IntPtr fnReadComp;

		public IntPtr fnWriteComp;

		public IntPtr fnTrace;

		public IntPtr fnAcceptComp;

		public uint dwNumProts;

		public IntPtr rgListenInfo;

		public IntPtr NodeAffinity;
	}

	internal struct SNI_CLIENT_CONSUMER_INFO
	{
		public Sni_Consumer_Info ConsumerInfo;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string wszConnectionString;

		public PrefixEnum networkLibrary;

		public unsafe byte* szSPN;

		public uint cchSPN;

		public unsafe byte* szInstanceName;

		public uint cchInstanceName;

		[MarshalAs(UnmanagedType.Bool)]
		public bool fOverrideLastConnectCache;

		[MarshalAs(UnmanagedType.Bool)]
		public bool fSynchronousConnection;

		public int timeout;

		[MarshalAs(UnmanagedType.Bool)]
		public bool fParallel;

		public TransparentNetworkResolutionMode transparentNetworkResolution;

		public int totalTimeout;

		public bool isAzureSqlServerEndpoint;

		public SqlConnectionIPAddressPreference ipAddressPreference;

		public SNI_DNSCache_Info DNSCacheInfo;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct SNI_DNSCache_Info
	{
		[MarshalAs(UnmanagedType.LPWStr)]
		public string wszCachedFQDN;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string wszCachedTcpIPv4;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string wszCachedTcpIPv6;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string wszCachedTcpPort;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct SNI_Error
	{
		internal ProviderEnum provider;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 261)]
		internal string errorMessage;

		internal uint nativeError;

		internal uint sniError;

		[MarshalAs(UnmanagedType.LPWStr)]
		internal string fileName;

		[MarshalAs(UnmanagedType.LPWStr)]
		internal string function;

		internal uint lineNumber;
	}

	private static int s_sniMaxComposedSpnLength = -1;

	private static readonly bool s_is64bitProcess = Environment.Is64BitProcess;

	private const int SniOpenTimeOut = -1;

	internal const int ConnTerminatedError = 2;

	internal const int InvalidParameterError = 5;

	internal const int ProtocolNotSupportedError = 8;

	internal const int ConnTimeoutError = 11;

	internal const int ConnNotUsableError = 19;

	internal const int InvalidConnStringError = 25;

	internal const int HandshakeFailureError = 31;

	internal const int InternalExceptionError = 35;

	internal const int ConnOpenFailedError = 40;

	internal const int ErrorSpnLookup = 44;

	internal const int LocalDBErrorCode = 50;

	internal const int MultiSubnetFailoverWithMoreThan64IPs = 47;

	internal const int MultiSubnetFailoverWithInstanceSpecified = 48;

	internal const int MultiSubnetFailoverWithNonTcpProtocol = 49;

	internal const int MaxErrorValue = 50157;

	internal const int LocalDBNoInstanceName = 51;

	internal const int LocalDBNoInstallation = 52;

	internal const int LocalDBInvalidConfig = 53;

	internal const int LocalDBNoSqlUserInstanceDllPath = 54;

	internal const int LocalDBInvalidSqlUserInstanceDllPath = 55;

	internal const int LocalDBFailedToLoadDll = 56;

	internal const int LocalDBBadRuntime = 57;

	internal const int SniIP6AddrStringBufferLength = 48;

	internal static int SniMaxComposedSpnLength
	{
		get
		{
			if (s_sniMaxComposedSpnLength == -1)
			{
				s_sniMaxComposedSpnLength = checked((int)GetSniMaxComposedSpnLength());
			}
			return s_sniMaxComposedSpnLength;
		}
	}

	private static AppDomain GetDefaultAppDomainInternal()
	{
		return AppDomain.CurrentDomain;
	}

	internal static _AppDomain GetDefaultAppDomain()
	{
		return (_AppDomain)(object)GetDefaultAppDomainInternal();
	}

	internal unsafe static byte[] GetData()
	{
		int passedSize;
		IntPtr intPtr = (IntPtr)SqlDependencyProcessDispatcherStorage.NativeGetData(out passedSize);
		byte[] array = null;
		if (intPtr != IntPtr.Zero)
		{
			array = new byte[passedSize];
			Marshal.Copy(intPtr, array, 0, passedSize);
		}
		return array;
	}

	internal unsafe static void SetData(byte[] data)
	{
		fixed (byte* passedData = &data[0])
		{
			SqlDependencyProcessDispatcherStorage.NativeSetData(passedData, data.Length);
		}
	}

	[DllImport("kernel32.dll", SetLastError = true)]
	public static extern IntPtr LoadLibrary(string dllToLoad);

	[DllImport("secur32.dll", ExactSpelling = true, SetLastError = true)]
	internal static extern uint QueryContextAttributes(ref CredHandle contextHandle, [In] ContextAttribute attribute, [In] IntPtr buffer);

	internal static uint SNIAddProvider(SNIHandle pConn, ProviderEnum ProvNum, [In] ref uint pInfo)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIAddProvider(pConn, ProvNum, ref pInfo);
		}
		return SNINativeManagedWrapperX64.SNIAddProvider(pConn, ProvNum, ref pInfo);
	}

	internal static uint SNIAddProviderWrapper(SNIHandle pConn, ProviderEnum ProvNum, [In] ref SNICTAIPProviderInfo pInfo)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIAddProviderWrapper(pConn, ProvNum, ref pInfo);
		}
		return SNINativeManagedWrapperX64.SNIAddProviderWrapper(pConn, ProvNum, ref pInfo);
	}

	internal static uint SNIAddProviderWrapper(SNIHandle pConn, ProviderEnum ProvNum, [In] ref AuthProviderInfo pInfo)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIAddProviderWrapper(pConn, ProvNum, ref pInfo);
		}
		return SNINativeManagedWrapperX64.SNIAddProviderWrapper(pConn, ProvNum, ref pInfo);
	}

	internal static uint SNICheckConnection([In] SNIHandle pConn)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNICheckConnection(pConn);
		}
		return SNINativeManagedWrapperX64.SNICheckConnection(pConn);
	}

	internal static uint SNIClose(IntPtr pConn)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIClose(pConn);
		}
		return SNINativeManagedWrapperX64.SNIClose(pConn);
	}

	internal static void SNIGetLastError(out SNI_Error pErrorStruct)
	{
		if (s_is64bitProcess)
		{
			SNINativeManagedWrapperX64.SNIGetLastError(out pErrorStruct);
		}
		else
		{
			SNINativeManagedWrapperX86.SNIGetLastError(out pErrorStruct);
		}
	}

	internal static void SNIPacketRelease(IntPtr pPacket)
	{
		if (s_is64bitProcess)
		{
			SNINativeManagedWrapperX64.SNIPacketRelease(pPacket);
		}
		else
		{
			SNINativeManagedWrapperX86.SNIPacketRelease(pPacket);
		}
	}

	internal static void SNIPacketReset([In] SNIHandle pConn, IOType IOType, SNIPacket pPacket, ConsumerNumber ConsNum)
	{
		if (s_is64bitProcess)
		{
			SNINativeManagedWrapperX64.SNIPacketReset(pConn, IOType, pPacket, ConsNum);
		}
		else
		{
			SNINativeManagedWrapperX86.SNIPacketReset(pConn, IOType, pPacket, ConsNum);
		}
	}

	internal static uint SNIQueryInfo(QTypes QType, ref uint pbQInfo)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIQueryInfo(QType, ref pbQInfo);
		}
		return SNINativeManagedWrapperX64.SNIQueryInfo(QType, ref pbQInfo);
	}

	internal static uint SNIQueryInfo(QTypes QType, ref IntPtr pbQInfo)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIQueryInfo(QType, ref pbQInfo);
		}
		return SNINativeManagedWrapperX64.SNIQueryInfo(QType, ref pbQInfo);
	}

	internal static uint SNIReadAsync(SNIHandle pConn, ref IntPtr ppNewPacket)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIReadAsync(pConn, ref ppNewPacket);
		}
		return SNINativeManagedWrapperX64.SNIReadAsync(pConn, ref ppNewPacket);
	}

	internal static uint SNIReadSyncOverAsync(SNIHandle pConn, ref IntPtr ppNewPacket, int timeout)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIReadSyncOverAsync(pConn, ref ppNewPacket, timeout);
		}
		return SNINativeManagedWrapperX64.SNIReadSyncOverAsync(pConn, ref ppNewPacket, timeout);
	}

	internal static uint SNIRemoveProvider(SNIHandle pConn, ProviderEnum ProvNum)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIRemoveProvider(pConn, ProvNum);
		}
		return SNINativeManagedWrapperX64.SNIRemoveProvider(pConn, ProvNum);
	}

	internal static uint SNISecInitPackage(ref uint pcbMaxToken)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNISecInitPackage(ref pcbMaxToken);
		}
		return SNINativeManagedWrapperX64.SNISecInitPackage(ref pcbMaxToken);
	}

	internal static uint SNISetInfo(SNIHandle pConn, QTypes QType, [In] ref uint pbQInfo)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNISetInfo(pConn, QType, ref pbQInfo);
		}
		return SNINativeManagedWrapperX64.SNISetInfo(pConn, QType, ref pbQInfo);
	}

	internal static uint SNITerminate()
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNITerminate();
		}
		return SNINativeManagedWrapperX64.SNITerminate();
	}

	internal static uint SNIWaitForSSLHandshakeToComplete([In] SNIHandle pConn, int dwMilliseconds, out uint pProtocolVersion)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIWaitForSSLHandshakeToComplete(pConn, dwMilliseconds, out pProtocolVersion);
		}
		return SNINativeManagedWrapperX64.SNIWaitForSSLHandshakeToComplete(pConn, dwMilliseconds, out pProtocolVersion);
	}

	internal static uint UnmanagedIsTokenRestricted([In] IntPtr token, [MarshalAs(UnmanagedType.Bool)] out bool isRestricted)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.UnmanagedIsTokenRestricted(token, out isRestricted);
		}
		return SNINativeManagedWrapperX64.UnmanagedIsTokenRestricted(token, out isRestricted);
	}

	private static uint GetSniMaxComposedSpnLength()
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.GetSniMaxComposedSpnLength();
		}
		return SNINativeManagedWrapperX64.GetSniMaxComposedSpnLength();
	}

	private static uint SNIGetInfoWrapper([In] SNIHandle pConn, QTypes QType, out Guid pbQInfo)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIGetInfoWrapper(pConn, QType, out pbQInfo);
		}
		return SNINativeManagedWrapperX64.SNIGetInfoWrapper(pConn, QType, out pbQInfo);
	}

	private static uint SNIGetInfoWrapper([In] SNIHandle pConn, QTypes QType, [MarshalAs(UnmanagedType.Bool)] out bool pbQInfo)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIGetInfoWrapper(pConn, QType, out pbQInfo);
		}
		return SNINativeManagedWrapperX64.SNIGetInfoWrapper(pConn, QType, out pbQInfo);
	}

	private static uint SNIGetInfoWrapper([In] SNIHandle pConn, QTypes QType, ref IntPtr pbQInfo)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIGetInfoWrapper(pConn, QType, ref pbQInfo);
		}
		return SNINativeManagedWrapperX64.SNIGetInfoWrapper(pConn, QType, ref pbQInfo);
	}

	private static uint SNIGetInfoWrapper([In] SNIHandle pConn, QTypes QType, out ushort portNum)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIGetInfoWrapper(pConn, QType, out portNum);
		}
		return SNINativeManagedWrapperX64.SNIGetInfoWrapper(pConn, QType, out portNum);
	}

	private static uint SNIGetPeerAddrStrWrapper([In] SNIHandle pConn, int bufferSize, StringBuilder addrBuffer, out uint addrLen)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIGetPeerAddrStrWrapper(pConn, bufferSize, addrBuffer, out addrLen);
		}
		return SNINativeManagedWrapperX64.SNIGetPeerAddrStrWrapper(pConn, bufferSize, addrBuffer, out addrLen);
	}

	private static uint SNIGetInfoWrapper([In] SNIHandle pConn, QTypes QType, out ProviderEnum provNum)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIGetInfoWrapper(pConn, QType, out provNum);
		}
		return SNINativeManagedWrapperX64.SNIGetInfoWrapper(pConn, QType, out provNum);
	}

	private static uint SNIInitialize([In] IntPtr pmo)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIInitialize(pmo);
		}
		return SNINativeManagedWrapperX64.SNIInitialize(pmo);
	}

	private static uint SNIOpenSyncExWrapper(ref SNI_CLIENT_CONSUMER_INFO pClientConsumerInfo, out IntPtr ppConn)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIOpenSyncExWrapper(ref pClientConsumerInfo, out ppConn);
		}
		return SNINativeManagedWrapperX64.SNIOpenSyncExWrapper(ref pClientConsumerInfo, out ppConn);
	}

	private static uint SNIOpenWrapper([In] ref Sni_Consumer_Info pConsumerInfo, [MarshalAs(UnmanagedType.LPWStr)] string szConnect, [In] SNIHandle pConn, out IntPtr ppConn, [MarshalAs(UnmanagedType.Bool)] bool fSync, SqlConnectionIPAddressPreference ipPreference, [In] ref SNI_DNSCache_Info pDNSCachedInfo)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIOpenWrapper(ref pConsumerInfo, szConnect, pConn, out ppConn, fSync, ipPreference, ref pDNSCachedInfo);
		}
		return SNINativeManagedWrapperX64.SNIOpenWrapper(ref pConsumerInfo, szConnect, pConn, out ppConn, fSync, ipPreference, ref pDNSCachedInfo);
	}

	private static IntPtr SNIPacketAllocateWrapper([In] SafeHandle pConn, IOType IOType)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIPacketAllocateWrapper(pConn, IOType);
		}
		return SNINativeManagedWrapperX64.SNIPacketAllocateWrapper(pConn, IOType);
	}

	private static uint SNIPacketGetDataWrapper([In] IntPtr packet, [In][Out] byte[] readBuffer, uint readBufferLength, out uint dataSize)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIPacketGetDataWrapper(packet, readBuffer, readBufferLength, out dataSize);
		}
		return SNINativeManagedWrapperX64.SNIPacketGetDataWrapper(packet, readBuffer, readBufferLength, out dataSize);
	}

	private unsafe static void SNIPacketSetData(SNIPacket pPacket, [In] byte* pbBuf, uint cbBuf)
	{
		if (s_is64bitProcess)
		{
			SNINativeManagedWrapperX64.SNIPacketSetData(pPacket, pbBuf, cbBuf);
		}
		else
		{
			SNINativeManagedWrapperX86.SNIPacketSetData(pPacket, pbBuf, cbBuf);
		}
	}

	private unsafe static uint SNISecGenClientContextWrapper([In] SNIHandle pConn, [In][Out] byte[] pIn, uint cbIn, [In][Out] byte[] pOut, [In] ref uint pcbOut, [MarshalAs(UnmanagedType.Bool)] out bool pfDone, byte* szServerInfo, uint cbServerInfo, [MarshalAs(UnmanagedType.LPWStr)] string pwszUserName, [MarshalAs(UnmanagedType.LPWStr)] string pwszPassword)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNISecGenClientContextWrapper(pConn, pIn, cbIn, pOut, ref pcbOut, out pfDone, szServerInfo, cbServerInfo, pwszUserName, pwszPassword);
		}
		return SNINativeManagedWrapperX64.SNISecGenClientContextWrapper(pConn, pIn, cbIn, pOut, ref pcbOut, out pfDone, szServerInfo, cbServerInfo, pwszUserName, pwszPassword);
	}

	private static uint SNIWriteAsyncWrapper(SNIHandle pConn, [In] SNIPacket pPacket)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIWriteAsyncWrapper(pConn, pPacket);
		}
		return SNINativeManagedWrapperX64.SNIWriteAsyncWrapper(pConn, pPacket);
	}

	private static uint SNIWriteSyncOverAsync(SNIHandle pConn, [In] SNIPacket pPacket)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIWriteSyncOverAsync(pConn, pPacket);
		}
		return SNINativeManagedWrapperX64.SNIWriteSyncOverAsync(pConn, pPacket);
	}

	private static IntPtr SNIClientCertificateFallbackWrapper(IntPtr pCallbackContext)
	{
		if (!s_is64bitProcess)
		{
			return SNINativeManagedWrapperX86.SNIClientCertificateFallbackWrapper(pCallbackContext);
		}
		return SNINativeManagedWrapperX64.SNIClientCertificateFallbackWrapper(pCallbackContext);
	}

	internal static uint SNISecGetServerCertificate(SNIHandle pConnectionObject, ref X509Certificate2 certificate)
	{
		X509Certificate x509Certificate = null;
		bool success = false;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			pConnectionObject.DangerousAddRef(ref success);
			IntPtr pbQInfo = Marshal.AllocHGlobal(Marshal.SizeOf<CredHandle>());
			uint num = SNIGetInfoWrapper(pConnectionObject, QTypes.SNI_QUERY_CONN_SSL_SECCTXTHANDLE, ref pbQInfo);
			if (num == 0)
			{
				CredHandle contextHandle = Marshal.PtrToStructure<CredHandle>(pbQInfo);
				if ((num = QueryContextAttributes(ref contextHandle, ContextAttribute.SECPKG_ATTR_REMOTE_CERT_CONTEXT, x509Certificate.Handle)) == 0)
				{
					certificate = new X509Certificate2(x509Certificate.Handle);
				}
			}
			Marshal.FreeHGlobal(pbQInfo);
			return num;
		}
		finally
		{
			x509Certificate?.Dispose();
			if (success)
			{
				pConnectionObject.DangerousRelease();
			}
		}
	}

	internal static uint SniGetConnectionId(SNIHandle pConn, ref Guid connId)
	{
		return SNIGetInfoWrapper(pConn, QTypes.SNI_QUERY_CONN_CONNID, out connId);
	}

	internal static uint SniGetProviderNumber(SNIHandle pConn, ref ProviderEnum provNum)
	{
		return SNIGetInfoWrapper(pConn, QTypes.SNI_QUERY_CONN_PROVIDERNUM, out provNum);
	}

	internal static uint SniGetConnectionPort(SNIHandle pConn, ref ushort portNum)
	{
		return SNIGetInfoWrapper(pConn, QTypes.SNI_QUERY_CONN_PEERPORT, out portNum);
	}

	internal static uint SniGetConnectionIPString(SNIHandle pConn, ref string connIPStr)
	{
		uint num = 0u;
		uint addrLen = 0u;
		int num2 = 48;
		StringBuilder stringBuilder = new StringBuilder(num2);
		uint result = SNIGetPeerAddrStrWrapper(pConn, num2, stringBuilder, out addrLen);
		connIPStr = stringBuilder.ToString(0, Convert.ToInt32(addrLen));
		return result;
	}

	internal static uint SNIInitialize()
	{
		return SNIInitialize(IntPtr.Zero);
	}

	internal static uint SNIOpenMarsSession(ConsumerInfo consumerInfo, SNIHandle parent, ref IntPtr pConn, bool fSync, SqlConnectionIPAddressPreference ipPreference, SQLDNSInfo cachedDNSInfo)
	{
		Sni_Consumer_Info native_consumerInfo = default(Sni_Consumer_Info);
		MarshalConsumerInfo(consumerInfo, ref native_consumerInfo);
		SNI_DNSCache_Info pDNSCachedInfo = default(SNI_DNSCache_Info);
		pDNSCachedInfo.wszCachedFQDN = cachedDNSInfo?.FQDN;
		pDNSCachedInfo.wszCachedTcpIPv4 = cachedDNSInfo?.AddrIPv4;
		pDNSCachedInfo.wszCachedTcpIPv6 = cachedDNSInfo?.AddrIPv6;
		pDNSCachedInfo.wszCachedTcpPort = cachedDNSInfo?.Port;
		return SNIOpenWrapper(ref native_consumerInfo, "session:", parent, out pConn, fSync, ipPreference, ref pDNSCachedInfo);
	}

	internal unsafe static uint SNIOpenSyncEx(ConsumerInfo consumerInfo, string constring, ref IntPtr pConn, byte[] spnBuffer, byte[] instanceName, bool fOverrideCache, bool fSync, int timeout, bool fParallel, int transparentNetworkResolutionStateNo, int totalTimeout, bool isAzureSqlServerEndpoint, SqlConnectionIPAddressPreference ipPreference, SQLDNSInfo cachedDNSInfo)
	{
		fixed (byte* szInstanceName = &instanceName[0])
		{
			SNI_CLIENT_CONSUMER_INFO pClientConsumerInfo = default(SNI_CLIENT_CONSUMER_INFO);
			MarshalConsumerInfo(consumerInfo, ref pClientConsumerInfo.ConsumerInfo);
			pClientConsumerInfo.wszConnectionString = constring;
			pClientConsumerInfo.networkLibrary = PrefixEnum.UNKNOWN_PREFIX;
			pClientConsumerInfo.szInstanceName = szInstanceName;
			pClientConsumerInfo.cchInstanceName = (uint)instanceName.Length;
			pClientConsumerInfo.fOverrideLastConnectCache = fOverrideCache;
			pClientConsumerInfo.fSynchronousConnection = fSync;
			pClientConsumerInfo.timeout = timeout;
			pClientConsumerInfo.fParallel = fParallel;
			pClientConsumerInfo.isAzureSqlServerEndpoint = ADP.IsAzureSqlServerEndpoint(constring);
			switch (transparentNetworkResolutionStateNo)
			{
			case 0:
				pClientConsumerInfo.transparentNetworkResolution = TransparentNetworkResolutionMode.DisabledMode;
				break;
			case 1:
				pClientConsumerInfo.transparentNetworkResolution = TransparentNetworkResolutionMode.SequentialMode;
				break;
			case 2:
				pClientConsumerInfo.transparentNetworkResolution = TransparentNetworkResolutionMode.ParallelMode;
				break;
			}
			pClientConsumerInfo.totalTimeout = totalTimeout;
			pClientConsumerInfo.ipAddressPreference = ipPreference;
			pClientConsumerInfo.DNSCacheInfo.wszCachedFQDN = cachedDNSInfo?.FQDN;
			pClientConsumerInfo.DNSCacheInfo.wszCachedTcpIPv4 = cachedDNSInfo?.AddrIPv4;
			pClientConsumerInfo.DNSCacheInfo.wszCachedTcpIPv6 = cachedDNSInfo?.AddrIPv6;
			pClientConsumerInfo.DNSCacheInfo.wszCachedTcpPort = cachedDNSInfo?.Port;
			if (spnBuffer != null)
			{
				fixed (byte* szSPN = &spnBuffer[0])
				{
					pClientConsumerInfo.szSPN = szSPN;
					pClientConsumerInfo.cchSPN = (uint)spnBuffer.Length;
					return SNIOpenSyncExWrapper(ref pClientConsumerInfo, out pConn);
				}
			}
			return SNIOpenSyncExWrapper(ref pClientConsumerInfo, out pConn);
		}
	}

	internal static uint SNIAddProvider(SNIHandle pConn, ProviderEnum providerEnum, AuthProviderInfo authInfo)
	{
		uint num = 0u;
		if (authInfo.clientCertificateCallback != null)
		{
			SNIAuthProviderInfoWrapper sNIAuthProviderInfoWrapper = default(SNIAuthProviderInfoWrapper);
			sNIAuthProviderInfoWrapper.pDelegateContext = authInfo.clientCertificateCallbackContext;
			sNIAuthProviderInfoWrapper.pSqlClientCertificateDelegate = authInfo.clientCertificateCallback;
			authInfo.clientCertificateCallbackContext = sNIAuthProviderInfoWrapper;
			authInfo.clientCertificateCallback = SNIClientCertificateFallbackWrapper;
		}
		uint num2 = SNIAddProviderWrapper(pConn, providerEnum, ref authInfo);
		if (num2 == num)
		{
			num2 = SNIGetInfoWrapper(pConn, QTypes.SNI_QUERY_CONN_SUPPORTS_SYNC_OVER_ASYNC, out bool _);
		}
		return num2;
	}

	internal static uint SNIAddProvider(SNIHandle pConn, ProviderEnum providerEnum, CTAIPProviderInfo authInfo)
	{
		uint num = 0u;
		SNICTAIPProviderInfo pInfo = default(SNICTAIPProviderInfo);
		pInfo.prgbAddress = authInfo.originalNetworkAddress[0];
		pInfo.cbAddress = (byte)authInfo.originalNetworkAddress.Length;
		pInfo.fFromDataSecurityProxy = authInfo.fromDataSecurityProxy;
		uint num2 = SNIAddProviderWrapper(pConn, providerEnum, ref pInfo);
		if (num2 == num)
		{
			num2 = SNIGetInfoWrapper(pConn, QTypes.SNI_QUERY_CONN_SUPPORTS_SYNC_OVER_ASYNC, out bool _);
		}
		return num2;
	}

	internal static void SNIPacketAllocate(SafeHandle pConn, IOType IOType, ref IntPtr pPacket)
	{
		pPacket = SNIPacketAllocateWrapper(pConn, IOType);
	}

	internal static uint SNIPacketGetData(IntPtr packet, byte[] readBuffer, ref uint dataSize)
	{
		return SNIPacketGetDataWrapper(packet, readBuffer, (uint)readBuffer.Length, out dataSize);
	}

	internal unsafe static void SNIPacketSetData(SNIPacket packet, byte[] data, int length)
	{
		fixed (byte* pbBuf = &data[0])
		{
			SNIPacketSetData(packet, pbBuf, (uint)length);
		}
	}

	internal unsafe static void SNIPacketSetData(SNIPacket packet, byte[] data, int length, SecureString[] passwords, int[] passwordOffsets)
	{
		bool success = false;
		bool flag = false;
		IntPtr intPtr = IntPtr.Zero;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			fixed (byte* ptr = &data[0])
			{
			}
			if (passwords != null)
			{
				for (int i = 0; i < passwords.Length; i++)
				{
					if (passwords[i] == null)
					{
						continue;
					}
					RuntimeHelpers.PrepareConstrainedRegions();
					try
					{
						intPtr = Marshal.SecureStringToCoTaskMemUnicode(passwords[i]);
						char* ptr2 = (char*)intPtr.ToPointer();
						byte* ptr3 = (byte*)intPtr.ToPointer();
						int length2 = passwords[i].Length;
						for (int j = 0; j < length2; j++)
						{
							int num = *ptr2;
							byte b = (byte)((uint)num & 0xFFu);
							byte b2 = (byte)((uint)(num >> 8) & 0xFFu);
							*(ptr3++) = (byte)((uint)(((b & 0xF) << 4) | (b >> 4)) ^ 0xA5u);
							*(ptr3++) = (byte)((uint)(((b2 & 0xF) << 4) | (b2 >> 4)) ^ 0xA5u);
							ptr2++;
						}
						flag = true;
						Marshal.Copy(intPtr, data, passwordOffsets[i], length2 * 2);
					}
					finally
					{
						if (intPtr != IntPtr.Zero)
						{
							Marshal.ZeroFreeCoTaskMemUnicode(intPtr);
						}
					}
				}
			}
			packet.DangerousAddRef(ref success);
			fixed (byte* pbBuf = &data[0])
			{
				SNIPacketSetData(packet, pbBuf, (uint)length);
			}
		}
		finally
		{
			if (success)
			{
				packet.DangerousRelease();
			}
			if (flag)
			{
				for (int k = 0; k < data.Length; k++)
				{
					data[k] = 0;
				}
			}
		}
	}

	internal unsafe static uint SNISecGenClientContext(SNIHandle pConnectionObject, byte[] inBuff, uint receivedLength, byte[] OutBuff, ref uint sendLength, byte[] serverUserName)
	{
		fixed (byte* szServerInfo = &serverUserName[0])
		{
			bool pfDone;
			return SNISecGenClientContextWrapper(pConnectionObject, inBuff, receivedLength, OutBuff, ref sendLength, out pfDone, szServerInfo, (uint)serverUserName.Length, null, null);
		}
	}

	internal static uint SNIWritePacket(SNIHandle pConn, SNIPacket packet, bool sync)
	{
		if (sync)
		{
			return SNIWriteSyncOverAsync(pConn, packet);
		}
		return SNIWriteAsyncWrapper(pConn, packet);
	}

	private static void MarshalConsumerInfo(ConsumerInfo consumerInfo, ref Sni_Consumer_Info native_consumerInfo)
	{
		native_consumerInfo.DefaultUserDataLength = consumerInfo.defaultBufferSize;
		native_consumerInfo.fnReadComp = ((consumerInfo.readDelegate != null) ? Marshal.GetFunctionPointerForDelegate(consumerInfo.readDelegate) : IntPtr.Zero);
		native_consumerInfo.fnWriteComp = ((consumerInfo.writeDelegate != null) ? Marshal.GetFunctionPointerForDelegate(consumerInfo.writeDelegate) : IntPtr.Zero);
		native_consumerInfo.ConsumerKey = consumerInfo.key;
	}
}
