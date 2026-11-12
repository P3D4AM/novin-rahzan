using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Data.Common;
using Microsoft.Data.Sql;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.Data.SqlClient.Server;
using Microsoft.Data.SqlTypes;

namespace Microsoft.Data.SqlClient;

internal sealed class TdsParser
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	internal struct ReliabilitySection
	{
		[Conditional("DEBUG")]
		internal void Start()
		{
		}

		[Conditional("DEBUG")]
		internal void Stop()
		{
		}

		[Conditional("DEBUG")]
		internal static void Assert(string message)
		{
		}
	}

	private class TdsOrderUnique
	{
		internal short ColumnOrdinal;

		internal byte Flags;

		internal TdsOrderUnique(short ordinal, byte flags)
		{
			ColumnOrdinal = ordinal;
			Flags = flags;
		}
	}

	private class TdsOutputStream : Stream
	{
		private TdsParser _parser;

		private TdsParserStateObject _stateObj;

		private byte[] _preambleToStrip;

		public override bool CanRead => false;

		public override bool CanSeek => false;

		public override bool CanWrite => true;

		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		public override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		public TdsOutputStream(TdsParser parser, TdsParserStateObject stateObj, byte[] preambleToStrip)
		{
			_parser = parser;
			_stateObj = stateObj;
			_preambleToStrip = preambleToStrip;
		}

		public override void Flush()
		{
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		private void StripPreamble(byte[] buffer, ref int offset, ref int count)
		{
			if (_preambleToStrip != null && count >= _preambleToStrip.Length)
			{
				for (int i = 0; i < _preambleToStrip.Length; i++)
				{
					if (_preambleToStrip[i] != buffer[i])
					{
						_preambleToStrip = null;
						return;
					}
				}
				offset += _preambleToStrip.Length;
				count -= _preambleToStrip.Length;
			}
			_preambleToStrip = null;
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			ValidateWriteParameters(buffer, offset, count);
			StripPreamble(buffer, ref offset, ref count);
			if (count > 0)
			{
				_parser.WriteInt(count, _stateObj);
				_stateObj.WriteByteArray(buffer, count, offset);
			}
		}

		public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		{
			ValidateWriteParameters(buffer, offset, count);
			StripPreamble(buffer, ref offset, ref count);
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
				Task task = null;
				if (count > 0)
				{
					_parser.WriteInt(count, _stateObj);
					task = _stateObj.WriteByteArray(buffer, count, offset, canAccumulate: false);
				}
				if (task == null)
				{
					return CompletedTask;
				}
				return task;
			}
			catch (OutOfMemoryException)
			{
				_parser._connHandler.DoomThisConnection();
				throw;
			}
			catch (StackOverflowException)
			{
				_parser._connHandler.DoomThisConnection();
				throw;
			}
			catch (ThreadAbortException)
			{
				_parser._connHandler.DoomThisConnection();
				throw;
			}
		}

		internal static void ValidateWriteParameters(byte[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw ADP.ArgumentNull("buffer");
			}
			if (offset < 0)
			{
				throw ADP.ArgumentOutOfRange("offset");
			}
			if (count < 0)
			{
				throw ADP.ArgumentOutOfRange("count");
			}
			try
			{
				if (checked(offset + count) > buffer.Length)
				{
					throw ExceptionBuilder.InvalidOffsetLength();
				}
			}
			catch (OverflowException)
			{
				throw ExceptionBuilder.InvalidOffsetLength();
			}
		}
	}

	private class ConstrainedTextWriter : TextWriter
	{
		private TextWriter _next;

		private int _size;

		private int _written;

		public bool IsComplete
		{
			get
			{
				if (_size > 0)
				{
					return _written >= _size;
				}
				return false;
			}
		}

		public override Encoding Encoding => _next.Encoding;

		public ConstrainedTextWriter(TextWriter next, int size)
		{
			_next = next;
			_size = size;
			_written = 0;
			if (_size < 1)
			{
				_size = int.MaxValue;
			}
		}

		public override void Flush()
		{
			_next.Flush();
		}

		public override Task FlushAsync()
		{
			return _next.FlushAsync();
		}

		public override void Write(char value)
		{
			if (_written < _size)
			{
				_next.Write(value);
				_written++;
			}
		}

		public override void Write(char[] buffer, int index, int count)
		{
			ValidateWriteParameters(buffer, index, count);
			count = Math.Min(_size - _written, count);
			if (count > 0)
			{
				_next.Write(buffer, index, count);
			}
			_written += count;
		}

		public override Task WriteAsync(char value)
		{
			if (_written < _size)
			{
				_written++;
				return _next.WriteAsync(value);
			}
			return CompletedTask;
		}

		public override Task WriteAsync(char[] buffer, int index, int count)
		{
			ValidateWriteParameters(buffer, index, count);
			count = Math.Min(_size - _written, count);
			if (count > 0)
			{
				_written += count;
				return _next.WriteAsync(buffer, index, count);
			}
			return CompletedTask;
		}

		public override Task WriteAsync(string value)
		{
			return WriteAsync(value.ToCharArray());
		}

		internal static void ValidateWriteParameters(char[] buffer, int offset, int count)
		{
			if (buffer == null)
			{
				throw ADP.ArgumentNull("buffer");
			}
			if (offset < 0)
			{
				throw ADP.ArgumentOutOfRange("offset");
			}
			if (count < 0)
			{
				throw ADP.ArgumentOutOfRange("count");
			}
			try
			{
				if (checked(offset + count) > buffer.Length)
				{
					throw ExceptionBuilder.InvalidOffsetLength();
				}
			}
			catch (OverflowException)
			{
				throw ExceptionBuilder.InvalidOffsetLength();
			}
		}
	}

	private static int _objectTypeCount;

	private readonly SqlClientLogger _logger = new SqlClientLogger();

	private readonly string _typeName;

	internal readonly int _objectID = Interlocked.Increment(ref _objectTypeCount);

	private static Task completedTask;

	internal TdsParserStateObject _physicalStateObj;

	internal TdsParserStateObject _pMarsPhysicalConObj;

	private const int constBinBufferSize = 4096;

	private const int constTextBufferSize = 4096;

	private const string enableTruncateSwitch = "Switch.Microsoft.Data.SqlClient.TruncateScaledDecimal";

	internal TdsParserState _state;

	private string _server = "";

	internal volatile bool _fResetConnection;

	internal volatile bool _fPreserveTransaction;

	private SqlCollation _defaultCollation;

	private int _defaultCodePage;

	private int _defaultLCID;

	internal Encoding _defaultEncoding;

	private static EncryptionOptions _sniSupportedEncryptionOption = SNILoadHandle.SingletonInstance.Options;

	private static SNINativeMethodWrapper.SqlClientCertificateDelegate _clientCertificateCallback = ClientCertificateDelegate;

	private EncryptionOptions _encryptionOption = _sniSupportedEncryptionOption;

	private SqlInternalTransaction _currentTransaction;

	private SqlInternalTransaction _pendingTransaction;

	private long _retainedTransactionId;

	private int _nonTransactedOpenResultCount;

	private SqlInternalConnectionTds _connHandler;

	private bool _fMARS;

	internal bool _loginWithFailover;

	internal AutoResetEvent _resetConnectionEvent;

	internal TdsParserSessionPool _sessionPool;

	private bool _isShiloh;

	private bool _isShilohSP1;

	private bool _isYukon;

	private bool _isKatmai;

	private bool _isDenali;

	private byte[] _sniSpnBuffer;

	private SqlStatistics _statistics;

	private bool _statisticsIsInTransaction;

	private static byte[] s_nicAddress;

	private static bool s_fSSPILoaded = false;

	private static volatile uint s_maxSSPILength = 0u;

	private static readonly byte[] s_longDataHeader = new byte[25]
	{
		16, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255, 255, 255, 255, 255, 255,
		255, 255, 255, 255, 255
	};

	private static object s_tdsParserLock = new object();

	private const int ATTENTION_TIMEOUT = 5000;

	private static readonly byte[] s_xmlMetadataSubstituteSequence = new byte[8] { 231, 255, 255, 0, 0, 0, 0, 0 };

	private const int GUID_SIZE = 16;

	internal bool _asyncWrite;

	private bool _serverSupportsColumnEncryption;

	private static readonly byte[] s_FeatureExtDataAzureSQLSupportFeatureRequest = new byte[1] { 1 };

	private static readonly IEnumerable<SqlDataRecord> __tvpEmptyValue = new List<SqlDataRecord>().AsReadOnly();

	private const ulong _indeterminateSize = ulong.MaxValue;

	private const string StateTraceFormatString = "\n\t         _physicalStateObj = {0}\n\t         _pMarsPhysicalConObj = {1}\n\t         _state = {2}\n\t         _server = {3}\n\t         _fResetConnection = {4}\n\t         _defaultCollation = {5}\n\t         _defaultCodePage = {6}\n\t         _defaultLCID = {7}\n\t         _defaultEncoding = {8}\n\t         _encryptionOption = {9}\n\t         _currentTransaction = {10}\n\t         _pendingTransaction = {11}\n\t         _retainedTransactionId = {12}\n\t         _nonTransactedOpenResultCount = {13}\n\t         _connHandler = {14}\n\t         _fMARS = {15}\n\t         _sessionPool = {16}\n\t         _isShiloh = {17}\n\t         _isShilohSP1 = {18}\n\t         _isYukon = {19}\n\t         _sniSpnBuffer = {20}\n\t         _errors = {21}\n\t         _warnings = {22}\n\t         _attentionErrors = {23}\n\t         _attentionWarnings = {24}\n\t         _statistics = {25}\n\t         _statisticsIsInTransaction = {26}\n\t         _fPreserveTransaction = {27}         _fParallel = {28}";

	private static Task CompletedTask
	{
		get
		{
			if (completedTask == null)
			{
				completedTask = Task.FromResult<object>(null);
			}
			return completedTask;
		}
	}

	internal int ObjectID => _objectID;

	internal bool IsColumnEncryptionSupported
	{
		get
		{
			return _serverSupportsColumnEncryption;
		}
		set
		{
			_serverSupportsColumnEncryption = value;
		}
	}

	internal byte TceVersionSupported { get; set; }

	internal bool AreEnclaveRetriesSupported { get; set; }

	internal string EnclaveType { get; set; }

	internal bool isTcpProtocol { get; set; }

	internal string FQDNforDNSCahce { get; set; }

	internal bool IsDataClassificationEnabled => DataClassificationVersion != 0;

	internal int DataClassificationVersion { get; set; }

	internal SqlInternalConnectionTds Connection => _connHandler;

	private static bool EnableTruncateSwitch
	{
		get
		{
			bool isEnabled;
			return AppContext.TryGetSwitch("Switch.Microsoft.Data.SqlClient.TruncateScaledDecimal", out isEnabled) && isEnabled;
		}
	}

	internal SqlInternalTransaction CurrentTransaction
	{
		get
		{
			return _currentTransaction;
		}
		set
		{
			if ((_currentTransaction == null && value != null) || (_currentTransaction != null && value == null))
			{
				_currentTransaction = value;
			}
		}
	}

	internal int DefaultLCID => _defaultLCID;

	internal EncryptionOptions EncryptionOptions
	{
		get
		{
			return _encryptionOption;
		}
		set
		{
			_encryptionOption = value;
		}
	}

	internal bool IsYukonOrNewer => _isYukon;

	internal bool IsKatmaiOrNewer => _isKatmai;

	internal bool MARSOn => _fMARS;

	internal SqlInternalTransaction PendingTransaction
	{
		get
		{
			return _pendingTransaction;
		}
		set
		{
			_pendingTransaction = value;
		}
	}

	internal string Server => _server;

	internal TdsParserState State
	{
		get
		{
			return _state;
		}
		set
		{
			_state = value;
		}
	}

	internal SqlStatistics Statistics
	{
		get
		{
			return _statistics;
		}
		set
		{
			_statistics = value;
		}
	}

	private bool IncludeTraceHeader
	{
		get
		{
			if (_isDenali)
			{
				return SqlClientEventSource.Log.IsEnabled();
			}
			return false;
		}
	}

	internal TdsParser(bool MARS, bool fAsynchronous)
	{
		_fMARS = MARS;
		_physicalStateObj = new TdsParserStateObject(this);
		DataClassificationVersion = 0;
		_typeName = GetType().Name;
	}

	internal int IncrementNonTransactedOpenResultCount()
	{
		return Interlocked.Increment(ref _nonTransactedOpenResultCount);
	}

	internal void DecrementNonTransactedOpenResultCount()
	{
		Interlocked.Decrement(ref _nonTransactedOpenResultCount);
	}

	internal void ProcessPendingAck(TdsParserStateObject stateObj)
	{
		if (stateObj._attentionSent)
		{
			ProcessAttention(stateObj);
		}
	}

	internal void Connect(ServerInfo serverInfo, SqlInternalConnectionTds connHandler, bool ignoreSniOpenTimeout, long timerExpire, bool encrypt, bool trustServerCert, bool integratedSecurity, bool withFailover, bool isFirstTransparentAttempt, SqlAuthenticationMethod authType, string certificate, ServerCertificateValidationCallback serverCallback, ClientCertificateRetrievalCallback clientCallback, bool useOriginalAddressInfo, bool disableTnir)
	{
		if (_state != 0)
		{
			return;
		}
		_connHandler = connHandler;
		_loginWithFailover = withFailover;
		_connHandler.IsSQLDNSCachingSupported = false;
		if (SNILoadHandle.SingletonInstance.SNIStatus != 0)
		{
			_physicalStateObj.AddError(ProcessSNIError(_physicalStateObj));
			_physicalStateObj.Dispose();
			ThrowExceptionAndWarning(_physicalStateObj);
		}
		if (connHandler.ConnectionOptions.LocalDBInstance != null)
		{
			LocalDBAPI.CreateLocalDBInstance(connHandler.ConnectionOptions.LocalDBInstance);
		}
		if (integratedSecurity || authType == SqlAuthenticationMethod.ActiveDirectoryIntegrated)
		{
			LoadSSPILibrary();
			_sniSpnBuffer = new byte[SNINativeMethodWrapper.SniMaxComposedSpnLength];
			SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Connect|SEC> SSPI or Active Directory Authentication Library for SQL Server based integrated authentication");
		}
		else
		{
			_sniSpnBuffer = null;
			switch (authType)
			{
			case SqlAuthenticationMethod.ActiveDirectoryPassword:
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Connect|SEC> Active Directory Password authentication");
				break;
			case SqlAuthenticationMethod.ActiveDirectoryIntegrated:
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Connect|SEC> Active Directory Integrated authentication");
				break;
			case SqlAuthenticationMethod.ActiveDirectoryInteractive:
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Connect|SEC> Active Directory Interactive authentication");
				break;
			case SqlAuthenticationMethod.ActiveDirectoryServicePrincipal:
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Connect|SEC> Active Directory Service Principal authentication");
				break;
			case SqlAuthenticationMethod.ActiveDirectoryDeviceCodeFlow:
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Connect|SEC> Active Directory Device Code Flow authentication");
				break;
			case SqlAuthenticationMethod.ActiveDirectoryManagedIdentity:
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Connect|SEC> Active Directory Managed Identity authentication");
				break;
			case SqlAuthenticationMethod.ActiveDirectoryMSI:
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Connect|SEC> Active Directory MSI authentication");
				break;
			case SqlAuthenticationMethod.ActiveDirectoryDefault:
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Connect|SEC> Active Directory Default authentication");
				break;
			case SqlAuthenticationMethod.SqlPassword:
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Connect|SEC> SQL Password authentication");
				break;
			default:
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Connect|SEC> SQL authentication");
				break;
			}
		}
		byte[] instanceName = null;
		_connHandler.TimeoutErrorInternal.EndPhase(SqlConnectionTimeoutErrorPhase.PreLoginBegin);
		_connHandler.TimeoutErrorInternal.SetAndBeginPhase(SqlConnectionTimeoutErrorPhase.InitializeConnection);
		bool multiSubnetFailover = _connHandler.ConnectionOptions.MultiSubnetFailover;
		TransparentNetworkResolutionState transparentNetworkResolutionState = ((_connHandler.ConnectionOptions.TransparentNetworkIPResolution && !disableTnir) ? (isFirstTransparentAttempt ? TransparentNetworkResolutionState.SequentialMode : TransparentNetworkResolutionState.ParallelMode) : TransparentNetworkResolutionState.DisabledMode);
		int connectTimeout = _connHandler.ConnectionOptions.ConnectTimeout;
		FQDNforDNSCahce = serverInfo.ResolvedServerName;
		int num = FQDNforDNSCahce.IndexOf(",");
		if (num != -1)
		{
			FQDNforDNSCahce = FQDNforDNSCahce.Substring(0, num);
		}
		_physicalStateObj.CreatePhysicalSNIHandle(serverInfo.ExtendedServerName, ignoreSniOpenTimeout, timerExpire, out instanceName, _sniSpnBuffer, flushCache: false, async: true, multiSubnetFailover, transparentNetworkResolutionState, connectTimeout, _connHandler.ConnectionOptions.IPAddressPreference, FQDNforDNSCahce);
		if (_physicalStateObj.Status != 0)
		{
			_physicalStateObj.AddError(ProcessSNIError(_physicalStateObj));
			_physicalStateObj.Dispose();
			SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Connect|ERR|SEC> Login failure");
			ThrowExceptionAndWarning(_physicalStateObj);
		}
		_server = serverInfo.ResolvedServerName;
		if (connHandler.PoolGroupProviderInfo != null)
		{
			connHandler.PoolGroupProviderInfo.AliasCheck((serverInfo.PreRoutingServerName == null) ? serverInfo.ResolvedServerName : serverInfo.PreRoutingServerName);
		}
		_state = TdsParserState.OpenNotLoggedIn;
		_physicalStateObj.SniContext = SniContext.Snix_PreLoginBeforeSuccessfullWrite;
		_physicalStateObj.TimeoutTime = timerExpire;
		bool marsCapable = false;
		_connHandler.TimeoutErrorInternal.EndPhase(SqlConnectionTimeoutErrorPhase.InitializeConnection);
		_connHandler.TimeoutErrorInternal.SetAndBeginPhase(SqlConnectionTimeoutErrorPhase.SendPreLoginHandshake);
		uint num2 = SNINativeMethodWrapper.SniGetConnectionId(_physicalStateObj.Handle, ref _connHandler._clientConnectionId);
		AssignPendingDNSInfo(serverInfo.UserProtocol, FQDNforDNSCahce);
		SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Connect|SEC> Sending prelogin handshake");
		SendPreLoginHandshake(instanceName, encrypt, !string.IsNullOrEmpty(certificate), useOriginalAddressInfo);
		_connHandler.TimeoutErrorInternal.EndPhase(SqlConnectionTimeoutErrorPhase.SendPreLoginHandshake);
		_connHandler.TimeoutErrorInternal.SetAndBeginPhase(SqlConnectionTimeoutErrorPhase.ConsumePreLoginHandshake);
		_physicalStateObj.SniContext = SniContext.Snix_PreLogin;
		SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Connect|SEC> Consuming prelogin handshake");
		PreLoginHandshakeStatus preLoginHandshakeStatus = ConsumePreLoginHandshake(authType, encrypt, trustServerCert, integratedSecurity, serverCallback, clientCallback, out marsCapable, out _connHandler._fedAuthRequired);
		if (preLoginHandshakeStatus == PreLoginHandshakeStatus.InstanceFailure)
		{
			SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Connect|SEC> Prelogin handshake unsuccessful. Reattempting prelogin handshake");
			_physicalStateObj.Dispose();
			_physicalStateObj.SniContext = SniContext.Snix_Connect;
			_physicalStateObj.CreatePhysicalSNIHandle(serverInfo.ExtendedServerName, ignoreSniOpenTimeout, timerExpire, out instanceName, _sniSpnBuffer, flushCache: true, async: true, multiSubnetFailover, transparentNetworkResolutionState, connectTimeout, _connHandler.ConnectionOptions.IPAddressPreference, serverInfo.ResolvedServerName);
			if (_physicalStateObj.Status != 0)
			{
				_physicalStateObj.AddError(ProcessSNIError(_physicalStateObj));
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Connect|ERR|SEC> Login failure");
				ThrowExceptionAndWarning(_physicalStateObj);
			}
			uint num3 = SNINativeMethodWrapper.SniGetConnectionId(_physicalStateObj.Handle, ref _connHandler._clientConnectionId);
			SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Connect|SEC> Sending prelogin handshake");
			AssignPendingDNSInfo(serverInfo.UserProtocol, FQDNforDNSCahce);
			SendPreLoginHandshake(instanceName, encrypt, !string.IsNullOrEmpty(certificate), useOriginalAddressInfo);
			preLoginHandshakeStatus = ConsumePreLoginHandshake(authType, encrypt, trustServerCert, integratedSecurity, serverCallback, clientCallback, out marsCapable, out _connHandler._fedAuthRequired);
			if (preLoginHandshakeStatus == PreLoginHandshakeStatus.InstanceFailure)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Connect|ERR|SEC> Prelogin handshake unsuccessful. Login failure");
				throw SQL.InstanceFailure();
			}
		}
		SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Connect|SEC> Prelogin handshake successful");
		if (_fMARS && marsCapable)
		{
			_sessionPool = new TdsParserSessionPool(this);
		}
		else
		{
			_fMARS = false;
		}
	}

	internal void AssignPendingDNSInfo(string userProtocol, string DNSCacheKey)
	{
		ushort portNum = 0;
		string connIPStr = string.Empty;
		isTcpProtocol = false;
		SNINativeMethodWrapper.ProviderEnum provNum = SNINativeMethodWrapper.ProviderEnum.INVALID_PROV;
		if (string.IsNullOrEmpty(userProtocol))
		{
			uint num = SNINativeMethodWrapper.SniGetProviderNumber(_physicalStateObj.Handle, ref provNum);
			isTcpProtocol = provNum == SNINativeMethodWrapper.ProviderEnum.TCP_PROV;
		}
		else if (userProtocol == "tcp")
		{
			isTcpProtocol = true;
		}
		if (isTcpProtocol)
		{
			uint num = SNINativeMethodWrapper.SniGetConnectionPort(_physicalStateObj.Handle, ref portNum);
			num = SNINativeMethodWrapper.SniGetConnectionIPString(_physicalStateObj.Handle, ref connIPStr);
			_connHandler.pendingSQLDNSObject = new SQLDNSInfo(DNSCacheKey, null, null, portNum.ToString());
			if (IPAddress.TryParse(connIPStr, out IPAddress address))
			{
				if (AddressFamily.InterNetwork == address.AddressFamily)
				{
					_connHandler.pendingSQLDNSObject.AddrIPv4 = connIPStr;
				}
				else if (AddressFamily.InterNetworkV6 == address.AddressFamily)
				{
					_connHandler.pendingSQLDNSObject.AddrIPv6 = connIPStr;
				}
			}
		}
		else
		{
			_connHandler.pendingSQLDNSObject = null;
		}
	}

	internal void RemoveEncryption()
	{
		uint num = 0u;
		if (SNINativeMethodWrapper.SNIRemoveProvider(_physicalStateObj.Handle, SNINativeMethodWrapper.ProviderEnum.SSL_PROV) != 0)
		{
			_physicalStateObj.AddError(ProcessSNIError(_physicalStateObj));
			ThrowExceptionAndWarning(_physicalStateObj);
		}
		try
		{
		}
		finally
		{
			_physicalStateObj.ClearAllWritePackets();
		}
	}

	internal void EnableMars()
	{
		if (!_fMARS)
		{
			return;
		}
		_pMarsPhysicalConObj = _physicalStateObj;
		uint num = 0u;
		uint pInfo = 0u;
		if (SNINativeMethodWrapper.SNIAddProvider(_pMarsPhysicalConObj.Handle, SNINativeMethodWrapper.ProviderEnum.SMUX_PROV, ref pInfo) != 0)
		{
			_physicalStateObj.AddError(ProcessSNIError(_physicalStateObj));
			ThrowExceptionAndWarning(_physicalStateObj);
		}
		IntPtr ppNewPacket = IntPtr.Zero;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
		}
		finally
		{
			_pMarsPhysicalConObj.IncrementPendingCallbacks();
			num = SNINativeMethodWrapper.SNIReadAsync(_pMarsPhysicalConObj.Handle, ref ppNewPacket);
			if (ppNewPacket != IntPtr.Zero)
			{
				SNINativeMethodWrapper.SNIPacketRelease(ppNewPacket);
			}
		}
		if (997 != num)
		{
			_physicalStateObj.AddError(ProcessSNIError(_physicalStateObj));
			ThrowExceptionAndWarning(_physicalStateObj);
		}
		_physicalStateObj = CreateSession();
	}

	internal TdsParserStateObject CreateSession()
	{
		TdsParserStateObject tdsParserStateObject = new TdsParserStateObject(this, _pMarsPhysicalConObj.Handle, async: true);
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParser.CreateSession|ADV> {0} created session {1}", ObjectID, tdsParserStateObject.ObjectID);
		return tdsParserStateObject;
	}

	internal TdsParserStateObject GetSession(object owner)
	{
		TdsParserStateObject tdsParserStateObject = null;
		if (MARSOn)
		{
			tdsParserStateObject = _sessionPool.GetSession(owner);
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParser.GetSession|ADV> {0} getting session {1} from pool", ObjectID, tdsParserStateObject.ObjectID);
		}
		else
		{
			tdsParserStateObject = _physicalStateObj;
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParser.GetSession|ADV> {0} getting physical session {1}", ObjectID, tdsParserStateObject.ObjectID);
		}
		return tdsParserStateObject;
	}

	internal void PutSession(TdsParserStateObject session)
	{
		if (MARSOn)
		{
			_sessionPool.PutSession(session);
		}
		else if (_state == TdsParserState.Closed || _state == TdsParserState.Broken)
		{
			_physicalStateObj.SniContext = SniContext.Snix_Close;
			_physicalStateObj.Dispose();
		}
		else
		{
			_physicalStateObj.Owner = null;
		}
	}

	internal void BestEffortCleanup()
	{
		_state = TdsParserState.Broken;
		_physicalStateObj?.Handle?.Dispose();
		if (_fMARS)
		{
			_sessionPool?.BestEffortCleanup();
			_pMarsPhysicalConObj?.Handle?.Dispose();
		}
	}

	private void SendPreLoginHandshake(byte[] instanceName, bool encrypt, bool clientCertificate, bool useCtaip)
	{
		_physicalStateObj._outputMessageType = 18;
		int num = 36;
		byte[] array = new byte[1059];
		int num2 = 0;
		for (int i = 0; i < 7; i++)
		{
			int num3 = 0;
			_physicalStateObj.WriteByte((byte)i);
			_physicalStateObj.WriteByte((byte)((num & 0xFF00) >> 8));
			_physicalStateObj.WriteByte((byte)((uint)num & 0xFFu));
			switch (i)
			{
			case 0:
			{
				Version assemblyVersion = ADP.GetAssemblyVersion();
				array[num2++] = (byte)((uint)assemblyVersion.Major & 0xFFu);
				array[num2++] = (byte)((uint)assemblyVersion.Minor & 0xFFu);
				array[num2++] = (byte)((assemblyVersion.Build & 0xFF00) >> 8);
				array[num2++] = (byte)((uint)assemblyVersion.Build & 0xFFu);
				array[num2++] = (byte)((uint)assemblyVersion.Revision & 0xFFu);
				array[num2++] = (byte)((assemblyVersion.Revision & 0xFF00) >> 8);
				num += 6;
				num3 = 6;
				break;
			}
			case 1:
				if (_encryptionOption == EncryptionOptions.NOT_SUP)
				{
					array[num2] = 2;
				}
				else
				{
					if (encrypt)
					{
						array[num2] = 1;
						_encryptionOption = EncryptionOptions.ON;
					}
					else
					{
						array[num2] = 0;
						_encryptionOption = EncryptionOptions.OFF;
					}
					if (clientCertificate)
					{
						array[num2] |= 128;
						_encryptionOption |= EncryptionOptions.CLIENT_CERT;
					}
				}
				if (useCtaip)
				{
					array[num2] |= 64;
					_encryptionOption |= EncryptionOptions.CTAIP;
				}
				num2++;
				num++;
				num3 = 1;
				break;
			case 2:
			{
				int j;
				for (j = 0; instanceName[j] != 0; j++)
				{
					array[num2] = instanceName[j];
					num2++;
				}
				array[num2] = 0;
				num2++;
				j++;
				num += j;
				num3 = j;
				break;
			}
			case 3:
			{
				int currentThreadIdForTdsLoginOnly = TdsParserStaticMethods.GetCurrentThreadIdForTdsLoginOnly();
				array[num2++] = (byte)((0xFF000000u & currentThreadIdForTdsLoginOnly) >> 24);
				array[num2++] = (byte)((0xFF0000 & currentThreadIdForTdsLoginOnly) >> 16);
				array[num2++] = (byte)((0xFF00 & currentThreadIdForTdsLoginOnly) >> 8);
				array[num2++] = (byte)(0xFFu & (uint)currentThreadIdForTdsLoginOnly);
				num += 4;
				num3 = 4;
				break;
			}
			case 4:
				array[num2++] = (byte)(_fMARS ? 1u : 0u);
				num++;
				num3++;
				break;
			case 5:
			{
				byte[] src = _connHandler._clientConnectionId.ToByteArray();
				Buffer.BlockCopy(src, 0, array, num2, 16);
				num2 += 16;
				num += 16;
				num3 = 16;
				ActivityCorrelator.ActivityId activityId = ActivityCorrelator.Next();
				Guid id = activityId.Id;
				src = id.ToByteArray();
				Buffer.BlockCopy(src, 0, array, num2, 16);
				num2 += 16;
				array[num2++] = (byte)(0xFFu & activityId.Sequence);
				array[num2++] = (byte)((0xFF00 & activityId.Sequence) >> 8);
				array[num2++] = (byte)((0xFF0000 & activityId.Sequence) >> 16);
				array[num2++] = (byte)((0xFF000000u & activityId.Sequence) >> 24);
				int num4 = 20;
				num += num4;
				num3 += num4;
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.SendPreLoginHandshake|INFO> ClientConnectionID {0}, ActivityID {1}", _connHandler._clientConnectionId, activityId);
				break;
			}
			case 6:
				array[num2++] = 1;
				num++;
				num3++;
				break;
			}
			_physicalStateObj.WriteByte((byte)((num3 & 0xFF00) >> 8));
			_physicalStateObj.WriteByte((byte)((uint)num3 & 0xFFu));
		}
		_physicalStateObj.WriteByte(byte.MaxValue);
		_physicalStateObj.WriteByteArray(array, num2, 0);
		_physicalStateObj.WritePacket(1);
	}

	private PreLoginHandshakeStatus ConsumePreLoginHandshake(SqlAuthenticationMethod authType, bool encrypt, bool trustServerCert, bool integratedSecurity, ServerCertificateValidationCallback serverCallback, ClientCertificateRetrievalCallback clientCallback, out bool marsCapable, out bool fedAuthRequired)
	{
		marsCapable = _fMARS;
		fedAuthRequired = false;
		bool flag = false;
		if (!_physicalStateObj.TryReadNetworkPacket())
		{
			throw SQL.SynchronousCallMayNotPend();
		}
		if (_physicalStateObj._inBytesRead == 0)
		{
			_physicalStateObj.AddError(new SqlError(0, 0, 20, _server, SQLMessage.PreloginError(), "", 0));
			_physicalStateObj.Dispose();
			ThrowExceptionAndWarning(_physicalStateObj);
		}
		if (!_physicalStateObj.TryProcessHeader())
		{
			throw SQL.SynchronousCallMayNotPend();
		}
		if (_physicalStateObj._inBytesPacket > 32768 || _physicalStateObj._inBytesPacket <= 0)
		{
			throw SQL.ParsingError(ParsingErrorState.CorruptedTdsStream);
		}
		byte[] array = new byte[_physicalStateObj._inBytesPacket];
		if (!_physicalStateObj.TryReadByteArray(array, 0, array.Length))
		{
			throw SQL.SynchronousCallMayNotPend();
		}
		if (array[0] == 170)
		{
			throw SQL.InvalidSQLServerVersionUnknown();
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = array[num++];
		while (true)
		{
			switch (num4)
			{
			case 0:
			{
				num2 = (array[num++] << 8) | array[num++];
				num3 = (array[num++] << 8) | array[num++];
				byte b = array[num2];
				byte b2 = array[num2 + 1];
				int num5 = (array[num2 + 2] << 8) | array[num2 + 3];
				flag = b >= 9;
				if (!flag)
				{
					marsCapable = false;
				}
				goto IL_0656;
			}
			case 1:
			{
				num2 = (array[num++] << 8) | array[num++];
				num3 = (array[num++] << 8) | array[num++];
				EncryptionOptions encryptionOptions = (EncryptionOptions)array[num2];
				switch (_encryptionOption & EncryptionOptions.OPTIONS_MASK)
				{
				case EncryptionOptions.ON:
					if (encryptionOptions == EncryptionOptions.NOT_SUP)
					{
						_physicalStateObj.AddError(new SqlError(20, 0, 20, _server, SQLMessage.EncryptionNotSupportedByServer(), "", 0));
						_physicalStateObj.Dispose();
						ThrowExceptionAndWarning(_physicalStateObj);
					}
					break;
				case EncryptionOptions.OFF:
					if ((encryptionOptions & EncryptionOptions.OPTIONS_MASK) == 0)
					{
						_encryptionOption = EncryptionOptions.LOGIN | (_encryptionOption & (EncryptionOptions)(-64));
					}
					else if ((encryptionOptions & EncryptionOptions.OPTIONS_MASK) == EncryptionOptions.REQ)
					{
						_encryptionOption = EncryptionOptions.ON | (_encryptionOption & (EncryptionOptions)(-64));
					}
					break;
				case EncryptionOptions.NOT_SUP:
					if ((encryptionOptions & EncryptionOptions.OPTIONS_MASK) == EncryptionOptions.REQ)
					{
						_physicalStateObj.AddError(new SqlError(20, 0, 20, _server, SQLMessage.EncryptionNotSupportedByClient(), "", 0));
						_physicalStateObj.Dispose();
						ThrowExceptionAndWarning(_physicalStateObj);
					}
					break;
				}
				if ((_encryptionOption & EncryptionOptions.CTAIP) != 0 && (encryptionOptions & EncryptionOptions.CTAIP) == 0)
				{
					_physicalStateObj.AddError(new SqlError(21, 0, 20, _server, SQLMessage.CTAIPNotSupportedByServer(), "", 0));
					_physicalStateObj.Dispose();
					ThrowExceptionAndWarning(_physicalStateObj);
				}
				if ((_encryptionOption & EncryptionOptions.OPTIONS_MASK) == EncryptionOptions.ON || (_encryptionOption & EncryptionOptions.OPTIONS_MASK) == EncryptionOptions.LOGIN)
				{
					if (serverCallback != null)
					{
						trustServerCert = true;
					}
					uint num6 = 0u;
					uint num7 = (((_encryptionOption == EncryptionOptions.ON && !trustServerCert) || ((authType != 0 || _connHandler._accessTokenInBytes != null) && !trustServerCert)) ? 1u : 0u) | ((flag && (_encryptionOption & EncryptionOptions.CLIENT_CERT) == 0) ? 2u : 0u);
					if (encrypt && !integratedSecurity)
					{
						num7 |= 0x10u;
					}
					SNINativeMethodWrapper.AuthProviderInfo authInfo = default(SNINativeMethodWrapper.AuthProviderInfo);
					authInfo.flags = num7;
					authInfo.certId = null;
					authInfo.certHash = false;
					authInfo.clientCertificateCallbackContext = IntPtr.Zero;
					authInfo.clientCertificateCallback = null;
					if ((_encryptionOption & EncryptionOptions.CLIENT_CERT) != 0)
					{
						string certificate = _connHandler.ConnectionOptions.Certificate;
						if (certificate.StartsWith("subject:", StringComparison.OrdinalIgnoreCase))
						{
							authInfo.certId = certificate.Substring(8);
						}
						else if (certificate.StartsWith("sha1:", StringComparison.OrdinalIgnoreCase))
						{
							authInfo.certId = certificate.Substring(5);
							authInfo.certHash = true;
						}
						if (clientCallback != null)
						{
							authInfo.clientCertificateCallbackContext = clientCallback;
							authInfo.clientCertificateCallback = _clientCertificateCallback;
						}
					}
					if (SNINativeMethodWrapper.SNIAddProvider(_physicalStateObj.Handle, SNINativeMethodWrapper.ProviderEnum.SSL_PROV, authInfo) != 0)
					{
						_physicalStateObj.AddError(ProcessSNIError(_physicalStateObj));
						ThrowExceptionAndWarning(_physicalStateObj);
					}
					if (SNINativeMethodWrapper.SNIWaitForSSLHandshakeToComplete(_physicalStateObj.Handle, _physicalStateObj.GetTimeoutRemaining(), out var pProtocolVersion) != 0)
					{
						_physicalStateObj.AddError(ProcessSNIError(_physicalStateObj));
						ThrowExceptionAndWarning(_physicalStateObj);
					}
					string protocolWarning = SslProtocolsHelper.GetProtocolWarning(pProtocolVersion);
					if (!string.IsNullOrEmpty(protocolWarning))
					{
						_logger.LogWarning(_typeName, MethodBase.GetCurrentMethod().Name, protocolWarning);
					}
					if (serverCallback != null)
					{
						X509Certificate2 certificate2 = null;
						if (SNINativeMethodWrapper.SNISecGetServerCertificate(_physicalStateObj.Handle, ref certificate2) != 0)
						{
							_physicalStateObj.AddError(ProcessSNIError(_physicalStateObj));
							ThrowExceptionAndWarning(_physicalStateObj);
						}
						if (!serverCallback(certificate2))
						{
							throw SQL.InvalidServerCertificate();
						}
					}
					try
					{
					}
					finally
					{
						_physicalStateObj.ClearAllWritePackets();
					}
				}
				goto IL_0656;
			}
			case 2:
			{
				num2 = (array[num++] << 8) | array[num++];
				num3 = (array[num++] << 8) | array[num++];
				byte b3 = 1;
				byte b4 = array[num2];
				if (b4 == b3)
				{
					return PreLoginHandshakeStatus.InstanceFailure;
				}
				goto IL_0656;
			}
			case 3:
				num += 4;
				goto IL_0656;
			case 4:
				num2 = (array[num++] << 8) | array[num++];
				num3 = (array[num++] << 8) | array[num++];
				marsCapable = ((array[num2] != 0) ? true : false);
				goto IL_0656;
			case 5:
				num += 4;
				goto IL_0656;
			case 6:
				num2 = (array[num++] << 8) | array[num++];
				num3 = (array[num++] << 8) | array[num++];
				if (array[num2] != 0 && array[num2] != 1)
				{
					SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.ConsumePreLoginHandshake|ERR> {0}, Server sent an unexpected value for FedAuthRequired PreLogin Option. Value was {1}.", ObjectID, (int)array[num2]);
					throw SQL.ParsingErrorValue(ParsingErrorState.FedAuthRequiredPreLoginResponseInvalidValue, array[num2]);
				}
				if ((_connHandler.ConnectionOptions != null && _connHandler.ConnectionOptions.Authentication != 0) || _connHandler._accessTokenInBytes != null)
				{
					fedAuthRequired = array[num2] == 1;
				}
				goto IL_0656;
			default:
				num += 4;
				goto IL_0656;
			case 255:
				break;
				IL_0656:
				if (num < array.Length)
				{
					goto IL_065c;
				}
				break;
			}
			break;
			IL_065c:
			num4 = array[num++];
		}
		return PreLoginHandshakeStatus.Successful;
	}

	internal void Deactivate(bool connectionIsDoomed)
	{
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParser.Deactivate|ADV> {0} deactivating", ObjectID);
		if (SqlClientEventSource.Log.IsStateDumpEnabled())
		{
			SqlClientEventSource.Log.StateDumpEvent("<sc.TdsParser.Deactivate|STATE> {0}, {1}", ObjectID, TraceString());
		}
		if (MARSOn)
		{
			_sessionPool.Deactivate();
		}
		if (!connectionIsDoomed && _physicalStateObj != null)
		{
			if (_physicalStateObj._pendingData)
			{
				DrainData(_physicalStateObj);
			}
			if (_physicalStateObj.HasOpenResult)
			{
				_physicalStateObj.DecrementOpenResultCount();
			}
		}
		SqlInternalTransaction currentTransaction = CurrentTransaction;
		if (currentTransaction != null && currentTransaction.HasParentTransaction)
		{
			currentTransaction.CloseFromConnection();
		}
		Statistics = null;
	}

	internal void Disconnect()
	{
		if (_sessionPool != null)
		{
			_sessionPool.Dispose();
		}
		if (_state == TdsParserState.Closed)
		{
			return;
		}
		_state = TdsParserState.Closed;
		try
		{
			if (!_physicalStateObj.HasOwner)
			{
				_physicalStateObj.SniContext = SniContext.Snix_Close;
				_physicalStateObj.Dispose();
			}
			else
			{
				_physicalStateObj.DecrementPendingCallbacks(release: false);
			}
			if (_pMarsPhysicalConObj != null)
			{
				_pMarsPhysicalConObj.Dispose();
			}
		}
		finally
		{
			_pMarsPhysicalConObj = null;
		}
	}

	private void FireInfoMessageEvent(SqlConnection connection, TdsParserStateObject stateObj, SqlError error)
	{
		string serverVersion = null;
		if (_state == TdsParserState.OpenLoggedIn)
		{
			serverVersion = _connHandler.ServerVersion;
		}
		SqlErrorCollection sqlErrorCollection = new SqlErrorCollection();
		sqlErrorCollection.Add(error);
		SqlException exception = SqlException.CreateException(sqlErrorCollection, serverVersion, _connHandler);
		connection.OnInfoMessage(new SqlInfoMessageEventArgs(exception), out var notified);
		if (notified)
		{
			stateObj._syncOverAsync = true;
		}
	}

	internal void DisconnectTransaction(SqlInternalTransaction internalTransaction)
	{
		if (_currentTransaction != null && _currentTransaction == internalTransaction)
		{
			_currentTransaction = null;
		}
	}

	internal void RollbackOrphanedAPITransactions()
	{
		SqlInternalTransaction currentTransaction = CurrentTransaction;
		if (currentTransaction != null && currentTransaction.HasParentTransaction && currentTransaction.IsOrphaned)
		{
			currentTransaction.CloseFromConnection();
		}
	}

	internal void ThrowExceptionAndWarning(TdsParserStateObject stateObj, bool callerHasConnectionLock = false, bool asyncClose = false)
	{
		SqlException ex = null;
		bool broken;
		SqlErrorCollection fullErrorAndWarningCollection = stateObj.GetFullErrorAndWarningCollection(out broken);
		if (fullErrorAndWarningCollection.Count == 0)
		{
			SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.ThrowExceptionAndWarning|ERR> Potential multi-threaded misuse of connection, unexpectedly empty warnings/errors under lock {0}", ObjectID);
		}
		broken &= _state != TdsParserState.Closed;
		if (broken)
		{
			if (_state == TdsParserState.OpenNotLoggedIn && (_connHandler.ConnectionOptions.TransparentNetworkIPResolution || _connHandler.ConnectionOptions.MultiSubnetFailover || _loginWithFailover) && fullErrorAndWarningCollection.Count == 1 && (fullErrorAndWarningCollection[0].Number == -2 || (long)fullErrorAndWarningCollection[0].Number == 258))
			{
				broken = false;
				Disconnect();
			}
			else
			{
				_state = TdsParserState.Broken;
			}
		}
		if (fullErrorAndWarningCollection != null && fullErrorAndWarningCollection.Count > 0)
		{
			string serverVersion = null;
			if (_state == TdsParserState.OpenLoggedIn)
			{
				serverVersion = _connHandler.ServerVersion;
			}
			ex = SqlException.CreateException(fullErrorAndWarningCollection, serverVersion, _connHandler);
			if (ex.Procedure == "InitSSPIPackage")
			{
				ex._doNotReconnect = true;
			}
		}
		if (ex == null)
		{
			return;
		}
		if (broken)
		{
			stateObj._networkPacketTaskSource?.TrySetException(ADP.ExceptionWithStackTrace(ex));
		}
		if (asyncClose)
		{
			SqlInternalConnectionTds connHandler = _connHandler;
			Action<Action> wrapCloseInAction = delegate(Action closeAction)
			{
				Task.Factory.StartNew(delegate
				{
					connHandler._parserLock.Wait(canReleaseFromAnyThread: false);
					connHandler.ThreadHasParserLockForClose = true;
					try
					{
						closeAction();
					}
					finally
					{
						connHandler.ThreadHasParserLockForClose = false;
						connHandler._parserLock.Release();
					}
				});
			};
			_connHandler.OnError(ex, broken, wrapCloseInAction);
			return;
		}
		bool threadHasParserLockForClose = _connHandler.ThreadHasParserLockForClose;
		if (callerHasConnectionLock)
		{
			_connHandler.ThreadHasParserLockForClose = true;
		}
		try
		{
			_connHandler.OnError(ex, broken);
		}
		finally
		{
			if (callerHasConnectionLock)
			{
				_connHandler.ThreadHasParserLockForClose = threadHasParserLockForClose;
			}
		}
	}

	internal SqlError ProcessSNIError(TdsParserStateObject stateObj)
	{
		SNINativeMethodWrapper.SNI_Error pErrorStruct = default(SNINativeMethodWrapper.SNI_Error);
		SNINativeMethodWrapper.SNIGetLastError(out pErrorStruct);
		if (pErrorStruct.sniError != 0)
		{
			switch (pErrorStruct.sniError)
			{
			case 47u:
				throw SQL.MultiSubnetFailoverWithMoreThan64IPs();
			case 48u:
				throw SQL.MultiSubnetFailoverWithInstanceSpecified();
			case 49u:
				throw SQL.MultiSubnetFailoverWithNonTcpProtocol();
			}
		}
		string text = (string.IsNullOrEmpty(pErrorStruct.errorMessage) ? string.Empty : pErrorStruct.errorMessage);
		string @string = StringsHelper.GetString(Enum.GetName(typeof(SniContext), stateObj.SniContext));
		string res = $"SNI_PN{(int)pErrorStruct.provider}";
		string string2 = StringsHelper.GetString(res);
		uint win32ErrorCode = pErrorStruct.nativeError;
		if (pErrorStruct.sniError == 0)
		{
			int num = text.IndexOf(':');
			if (0 <= num)
			{
				int length = text.Length;
				length -= 2;
				num += 2;
				length -= num;
				if (length > 0)
				{
					text = text.Substring(num, length);
				}
			}
		}
		else
		{
			text = SQL.GetSNIErrorMessage((int)pErrorStruct.sniError);
			if (pErrorStruct.sniError == 50)
			{
				text += LocalDBAPI.GetLocalDBMessage((int)pErrorStruct.nativeError);
				win32ErrorCode = 0u;
			}
		}
		text = $"{@string} (provider: {string2}, error: {(int)pErrorStruct.sniError} - {text})";
		return new SqlError((int)pErrorStruct.nativeError, 0, 20, _server, text, pErrorStruct.function, (int)pErrorStruct.lineNumber, win32ErrorCode);
	}

	internal void CheckResetConnection(TdsParserStateObject stateObj)
	{
		if (!_fResetConnection || stateObj._fResetConnectionSent)
		{
			return;
		}
		try
		{
			if (_fMARS && !stateObj._fResetEventOwned)
			{
				stateObj._fResetEventOwned = _resetConnectionEvent.WaitOne(stateObj.GetTimeoutRemaining(), exitContext: false);
				if (stateObj._fResetEventOwned && stateObj.TimeoutHasExpired)
				{
					stateObj._fResetEventOwned = !_resetConnectionEvent.Set();
					stateObj.TimeoutTime = 0L;
				}
				if (!stateObj._fResetEventOwned)
				{
					stateObj.ResetBuffer();
					stateObj.AddError(new SqlError(-2, 0, 11, _server, _connHandler.TimeoutErrorInternal.GetErrorMessage(), "", 0, 258u));
					ThrowExceptionAndWarning(stateObj, callerHasConnectionLock: true);
				}
			}
			if (_fResetConnection)
			{
				if (_fPreserveTransaction)
				{
					stateObj._outBuff[1] = (byte)(stateObj._outBuff[1] | 0x10u);
				}
				else
				{
					stateObj._outBuff[1] = (byte)(stateObj._outBuff[1] | 8u);
				}
				if (!_fMARS)
				{
					_fResetConnection = false;
					_fPreserveTransaction = false;
				}
				else
				{
					stateObj._fResetConnectionSent = true;
				}
			}
			else if (_fMARS && stateObj._fResetEventOwned)
			{
				stateObj._fResetEventOwned = !_resetConnectionEvent.Set();
			}
		}
		catch (Exception)
		{
			if (_fMARS && stateObj._fResetEventOwned)
			{
				stateObj._fResetConnectionSent = false;
				stateObj._fResetEventOwned = !_resetConnectionEvent.Set();
			}
			throw;
		}
	}

	internal byte[] SerializeShort(int v, TdsParserStateObject stateObj)
	{
		if (stateObj._bShortBytes == null)
		{
			stateObj._bShortBytes = new byte[2];
		}
		byte[] bShortBytes = stateObj._bShortBytes;
		int num = 0;
		bShortBytes[num++] = (byte)((uint)v & 0xFFu);
		bShortBytes[num++] = (byte)((uint)(v >> 8) & 0xFFu);
		return bShortBytes;
	}

	internal void WriteShort(int v, TdsParserStateObject stateObj)
	{
		if (stateObj._outBytesUsed + 2 > stateObj._outBuff.Length)
		{
			stateObj.WriteByte((byte)((uint)v & 0xFFu));
			stateObj.WriteByte((byte)((uint)(v >> 8) & 0xFFu));
		}
		else
		{
			stateObj._outBuff[stateObj._outBytesUsed] = (byte)((uint)v & 0xFFu);
			stateObj._outBuff[stateObj._outBytesUsed + 1] = (byte)((uint)(v >> 8) & 0xFFu);
			stateObj._outBytesUsed += 2;
		}
	}

	internal void WriteUnsignedShort(ushort us, TdsParserStateObject stateObj)
	{
		WriteShort((short)us, stateObj);
	}

	internal byte[] SerializeUnsignedInt(uint i, TdsParserStateObject stateObj)
	{
		return SerializeInt((int)i, stateObj);
	}

	internal void WriteUnsignedInt(uint i, TdsParserStateObject stateObj)
	{
		WriteInt((int)i, stateObj);
	}

	internal byte[] SerializeInt(int v, TdsParserStateObject stateObj)
	{
		if (stateObj._bIntBytes == null)
		{
			stateObj._bIntBytes = new byte[4];
		}
		int num = 0;
		byte[] bIntBytes = stateObj._bIntBytes;
		bIntBytes[num++] = (byte)((uint)v & 0xFFu);
		bIntBytes[num++] = (byte)((uint)(v >> 8) & 0xFFu);
		bIntBytes[num++] = (byte)((uint)(v >> 16) & 0xFFu);
		bIntBytes[num++] = (byte)((uint)(v >> 24) & 0xFFu);
		return bIntBytes;
	}

	internal void WriteInt(int v, TdsParserStateObject stateObj)
	{
		if (stateObj._outBytesUsed + 4 > stateObj._outBuff.Length)
		{
			for (int i = 0; i < 32; i += 8)
			{
				stateObj.WriteByte((byte)((uint)(v >> i) & 0xFFu));
			}
		}
		else
		{
			stateObj._outBuff[stateObj._outBytesUsed] = (byte)((uint)v & 0xFFu);
			stateObj._outBuff[stateObj._outBytesUsed + 1] = (byte)((uint)(v >> 8) & 0xFFu);
			stateObj._outBuff[stateObj._outBytesUsed + 2] = (byte)((uint)(v >> 16) & 0xFFu);
			stateObj._outBuff[stateObj._outBytesUsed + 3] = (byte)((uint)(v >> 24) & 0xFFu);
			stateObj._outBytesUsed += 4;
		}
	}

	internal byte[] SerializeFloat(float v)
	{
		if (float.IsInfinity(v) || float.IsNaN(v))
		{
			throw ADP.ParameterValueOutOfRange(v.ToString());
		}
		return BitConverter.GetBytes(v);
	}

	internal void WriteFloat(float v, TdsParserStateObject stateObj)
	{
		byte[] bytes = BitConverter.GetBytes(v);
		stateObj.WriteByteArray(bytes, bytes.Length, 0);
	}

	internal byte[] SerializeLong(long v, TdsParserStateObject stateObj)
	{
		int num = 0;
		if (stateObj._bLongBytes == null)
		{
			stateObj._bLongBytes = new byte[8];
		}
		byte[] bLongBytes = stateObj._bLongBytes;
		bLongBytes[num++] = (byte)(v & 0xFF);
		bLongBytes[num++] = (byte)((v >> 8) & 0xFF);
		bLongBytes[num++] = (byte)((v >> 16) & 0xFF);
		bLongBytes[num++] = (byte)((v >> 24) & 0xFF);
		bLongBytes[num++] = (byte)((v >> 32) & 0xFF);
		bLongBytes[num++] = (byte)((v >> 40) & 0xFF);
		bLongBytes[num++] = (byte)((v >> 48) & 0xFF);
		bLongBytes[num++] = (byte)((v >> 56) & 0xFF);
		return bLongBytes;
	}

	internal void WriteLong(long v, TdsParserStateObject stateObj)
	{
		if (stateObj._outBytesUsed + 8 > stateObj._outBuff.Length)
		{
			for (int i = 0; i < 64; i += 8)
			{
				stateObj.WriteByte((byte)((v >> i) & 0xFF));
			}
			return;
		}
		stateObj._outBuff[stateObj._outBytesUsed] = (byte)(v & 0xFF);
		stateObj._outBuff[stateObj._outBytesUsed + 1] = (byte)((v >> 8) & 0xFF);
		stateObj._outBuff[stateObj._outBytesUsed + 2] = (byte)((v >> 16) & 0xFF);
		stateObj._outBuff[stateObj._outBytesUsed + 3] = (byte)((v >> 24) & 0xFF);
		stateObj._outBuff[stateObj._outBytesUsed + 4] = (byte)((v >> 32) & 0xFF);
		stateObj._outBuff[stateObj._outBytesUsed + 5] = (byte)((v >> 40) & 0xFF);
		stateObj._outBuff[stateObj._outBytesUsed + 6] = (byte)((v >> 48) & 0xFF);
		stateObj._outBuff[stateObj._outBytesUsed + 7] = (byte)((v >> 56) & 0xFF);
		stateObj._outBytesUsed += 8;
	}

	internal byte[] SerializePartialLong(long v, int length)
	{
		byte[] array = new byte[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = (byte)((v >> i * 8) & 0xFF);
		}
		return array;
	}

	internal void WritePartialLong(long v, int length, TdsParserStateObject stateObj)
	{
		if (stateObj._outBytesUsed + length > stateObj._outBuff.Length)
		{
			for (int i = 0; i < length * 8; i += 8)
			{
				stateObj.WriteByte((byte)((v >> i) & 0xFF));
			}
			return;
		}
		for (int j = 0; j < length; j++)
		{
			stateObj._outBuff[stateObj._outBytesUsed + j] = (byte)((v >> j * 8) & 0xFF);
		}
		stateObj._outBytesUsed += length;
	}

	internal void WriteUnsignedLong(ulong uv, TdsParserStateObject stateObj)
	{
		WriteLong((long)uv, stateObj);
	}

	internal byte[] SerializeDouble(double v)
	{
		if (double.IsInfinity(v) || double.IsNaN(v))
		{
			throw ADP.ParameterValueOutOfRange(v.ToString());
		}
		return BitConverter.GetBytes(v);
	}

	internal void WriteDouble(double v, TdsParserStateObject stateObj)
	{
		byte[] bytes = BitConverter.GetBytes(v);
		stateObj.WriteByteArray(bytes, bytes.Length, 0);
	}

	internal void PrepareResetConnection(bool preserveTransaction)
	{
		_fResetConnection = true;
		_fPreserveTransaction = preserveTransaction;
	}

	internal bool RunReliably(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj)
	{
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			return Run(runBehavior, cmdHandler, dataStream, bulkCopyHandler, stateObj);
		}
		catch (OutOfMemoryException)
		{
			_connHandler.DoomThisConnection();
			throw;
		}
		catch (StackOverflowException)
		{
			_connHandler.DoomThisConnection();
			throw;
		}
		catch (ThreadAbortException)
		{
			_connHandler.DoomThisConnection();
			throw;
		}
	}

	internal bool Run(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj)
	{
		bool syncOverAsync = stateObj._syncOverAsync;
		try
		{
			stateObj._syncOverAsync = true;
			bool dataReady;
			bool flag = TryRun(runBehavior, cmdHandler, dataStream, bulkCopyHandler, stateObj, out dataReady);
			return dataReady;
		}
		finally
		{
			stateObj._syncOverAsync = syncOverAsync;
		}
	}

	internal static bool IsValidTdsToken(byte token)
	{
		if (token != 170 && token != 171 && token != 173 && token != 227 && token != 172 && token != 121 && token != 160 && token != 161 && token != 162 && token != 163 && token != 129 && token != 136 && token != 164 && token != 165 && token != 169 && token != 211 && token != 209 && token != 210 && token != 253 && token != 254 && token != byte.MaxValue && token != 57 && token != 237 && token != 124 && token != 120 && token != 237 && token != 174 && token != 228)
		{
			return token == 238;
		}
		return true;
	}

	internal bool TryRun(RunBehavior runBehavior, SqlCommand cmdHandler, SqlDataReader dataStream, BulkCopySimpleResultSet bulkCopyHandler, TdsParserStateObject stateObj, out bool dataReady)
	{
		if (TdsParserState.Broken == State || State == TdsParserState.Closed)
		{
			dataReady = true;
			return true;
		}
		dataReady = false;
		do
		{
			if (stateObj.IsTimeoutStateExpired)
			{
				runBehavior = RunBehavior.Attention;
			}
			if (TdsParserState.Broken == State || State == TdsParserState.Closed)
			{
				break;
			}
			if (!stateObj._accumulateInfoEvents && stateObj._pendingInfoEvents != null)
			{
				if (RunBehavior.Clean != (RunBehavior.Clean & runBehavior))
				{
					SqlConnection sqlConnection = null;
					if (_connHandler != null)
					{
						sqlConnection = _connHandler.Connection;
					}
					if (sqlConnection != null && sqlConnection.FireInfoMessageEventOnUserErrors)
					{
						foreach (SqlError pendingInfoEvent in stateObj._pendingInfoEvents)
						{
							FireInfoMessageEvent(sqlConnection, stateObj, pendingInfoEvent);
						}
					}
					else
					{
						foreach (SqlError pendingInfoEvent2 in stateObj._pendingInfoEvents)
						{
							stateObj.AddWarning(pendingInfoEvent2);
						}
					}
				}
				stateObj._pendingInfoEvents = null;
			}
			if (!stateObj.TryReadByte(out var value))
			{
				return false;
			}
			if (!IsValidTdsToken(value))
			{
				_state = TdsParserState.Broken;
				_connHandler.BreakConnection();
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.Run|ERR> Potential multi-threaded misuse of connection, unexpected TDS token found {0}", ObjectID);
				throw SQL.ParsingErrorToken(ParsingErrorState.InvalidTdsTokenReceived, value);
			}
			if (!TryGetTokenLength(value, stateObj, out var tokenLength))
			{
				return false;
			}
			switch (value)
			{
			case 170:
				stateObj._errorTokenReceived = true;
				goto case 171;
			case 171:
			{
				if (!TryProcessError(value, stateObj, out var error))
				{
					return false;
				}
				if (value == 171 && stateObj._accumulateInfoEvents)
				{
					if (stateObj._pendingInfoEvents == null)
					{
						stateObj._pendingInfoEvents = new List<SqlError>();
					}
					stateObj._pendingInfoEvents.Add(error);
					stateObj._syncOverAsync = true;
				}
				else if (RunBehavior.Clean != (RunBehavior.Clean & runBehavior))
				{
					SqlConnection sqlConnection2 = null;
					if (_connHandler != null)
					{
						sqlConnection2 = _connHandler.Connection;
					}
					if (sqlConnection2 != null && sqlConnection2.FireInfoMessageEventOnUserErrors && error.Class <= 16)
					{
						FireInfoMessageEvent(sqlConnection2, stateObj, error);
					}
					else if (error.Class < 11)
					{
						stateObj.AddWarning(error);
					}
					else if (error.Class < 20)
					{
						stateObj.AddError(error);
						if (dataStream != null && !dataStream.IsInitialized)
						{
							runBehavior = RunBehavior.UntilDone;
						}
					}
					else
					{
						stateObj.AddError(error);
						runBehavior = RunBehavior.UntilDone;
					}
				}
				else if (error.Class >= 20)
				{
					stateObj.AddError(error);
				}
				break;
			}
			case 165:
				if (dataStream != null)
				{
					if (!TryProcessColInfo(dataStream.MetaData, dataStream, stateObj, out var metaData))
					{
						return false;
					}
					if (!dataStream.TrySetMetaData(metaData, moreInfo: false))
					{
						return false;
					}
					dataStream.BrowseModeInfoConsumed = true;
				}
				else if (!stateObj.TrySkipBytes(tokenLength))
				{
					return false;
				}
				break;
			case 253:
			case 254:
			case byte.MaxValue:
				if (!TryProcessDone(cmdHandler, dataStream, ref runBehavior, stateObj))
				{
					return false;
				}
				if (value == 254 && cmdHandler != null)
				{
					if (cmdHandler.IsDescribeParameterEncryptionRPCCurrentlyInProgress)
					{
						cmdHandler.OnDoneDescribeParameterEncryptionProc(stateObj);
					}
					else
					{
						cmdHandler.OnDoneProc();
					}
				}
				break;
			case 169:
				if (!stateObj.TrySkipBytes(tokenLength))
				{
					return false;
				}
				break;
			case 136:
			{
				stateObj.CloneCleanupAltMetaDataSetArray();
				if (stateObj._cleanupAltMetaDataSetArray == null)
				{
					stateObj._cleanupAltMetaDataSetArray = new _SqlMetaDataSetCollection();
				}
				if (!TryProcessAltMetaData(tokenLength, stateObj, out var metaData2))
				{
					return false;
				}
				stateObj._cleanupAltMetaDataSetArray.SetAltMetaData(metaData2);
				if (dataStream != null)
				{
					if (!stateObj.TryPeekByte(out var value3))
					{
						return false;
					}
					if (!dataStream.TrySetAltMetaDataSet(metaData2, 136 != value3))
					{
						return false;
					}
				}
				break;
			}
			case 211:
				if (!stateObj.TryStartNewRow(isNullCompressed: false))
				{
					return false;
				}
				if (RunBehavior.ReturnImmediately != (RunBehavior.ReturnImmediately & runBehavior))
				{
					if (!stateObj.TryReadUInt16(out var value2))
					{
						return false;
					}
					if (!TrySkipRow(stateObj._cleanupAltMetaDataSetArray.GetAltMetaData(value2), stateObj))
					{
						return false;
					}
				}
				else
				{
					dataReady = true;
				}
				break;
			case 227:
			{
				stateObj._syncOverAsync = true;
				if (!TryProcessEnvChange(tokenLength, stateObj, out var sqlEnvChange))
				{
					return false;
				}
				for (int i = 0; i < sqlEnvChange.Length; i++)
				{
					if (sqlEnvChange[i] == null || Connection.IgnoreEnvChange)
					{
						continue;
					}
					switch (sqlEnvChange[i].type)
					{
					case 8:
					case 11:
						_currentTransaction = _pendingTransaction;
						_pendingTransaction = null;
						if (_currentTransaction != null)
						{
							_currentTransaction.TransactionId = sqlEnvChange[i].newLongValue;
						}
						else
						{
							TransactionType type = ((8 == sqlEnvChange[i].type) ? TransactionType.LocalFromTSQL : TransactionType.Distributed);
							_currentTransaction = new SqlInternalTransaction(_connHandler, type, null, sqlEnvChange[i].newLongValue);
						}
						if (_statistics != null && !_statisticsIsInTransaction)
						{
							_statistics.SafeIncrement(ref _statistics._transactions);
						}
						_statisticsIsInTransaction = true;
						_retainedTransactionId = 0L;
						break;
					case 9:
					case 12:
					case 17:
						_retainedTransactionId = 0L;
						goto case 10;
					case 10:
						if (_currentTransaction != null)
						{
							if (9 == sqlEnvChange[i].type)
							{
								_currentTransaction.Completed(TransactionState.Committed);
							}
							else if (10 == sqlEnvChange[i].type)
							{
								if (_currentTransaction.IsDistributed && _currentTransaction.IsActive)
								{
									_retainedTransactionId = sqlEnvChange[i].oldLongValue;
								}
								_currentTransaction.Completed(TransactionState.Aborted);
							}
							else
							{
								_currentTransaction.Completed(TransactionState.Unknown);
							}
							_currentTransaction = null;
						}
						_statisticsIsInTransaction = false;
						break;
					default:
						_connHandler.OnEnvChange(sqlEnvChange[i]);
						break;
					}
				}
				break;
			}
			case 173:
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.TryRun|SEC> Received login acknowledgement token");
				if (!TryProcessLoginAck(stateObj, out var sqlLoginAck))
				{
					return false;
				}
				_connHandler.OnLoginAck(sqlLoginAck);
				break;
			}
			case 174:
				if (!TryProcessFeatureExtAck(stateObj))
				{
					return false;
				}
				break;
			case 238:
			{
				_connHandler._federatedAuthenticationInfoReceived = true;
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.TryRun|SEC> Received federated authentication info token");
				if (!TryProcessFedAuthInfo(stateObj, tokenLength, out var sqlFedAuthInfo))
				{
					return false;
				}
				_connHandler.OnFedAuthInfo(sqlFedAuthInfo);
				break;
			}
			case 228:
				if (!TryProcessSessionState(stateObj, tokenLength, _connHandler._currentSessionData))
				{
					return false;
				}
				break;
			case 129:
			{
				if (tokenLength != 65535)
				{
					if (!TryProcessMetaData(tokenLength, stateObj, out var metaData3, cmdHandler?.ColumnEncryptionSetting ?? SqlCommandColumnEncryptionSetting.UseConnectionSetting))
					{
						return false;
					}
					stateObj._cleanupMetaData = metaData3;
				}
				else if (cmdHandler != null)
				{
					stateObj._cleanupMetaData = cmdHandler.MetaData;
				}
				if (!stateObj.TryPeekByte(out var value5))
				{
					return false;
				}
				if (163 == value5)
				{
					if (!stateObj.TryReadByte(out var _))
					{
						return false;
					}
					if (!TryProcessDataClassification(stateObj, out var sensitivityClassification))
					{
						return false;
					}
					if (dataStream != null && !dataStream.TrySetSensitivityClassification(sensitivityClassification))
					{
						return false;
					}
					if (!stateObj.TryPeekByte(out value5))
					{
						return false;
					}
				}
				if (dataStream != null)
				{
					if (!dataStream.TrySetMetaData(stateObj._cleanupMetaData, 164 == value5 || 165 == value5))
					{
						return false;
					}
				}
				else
				{
					bulkCopyHandler?.SetMetaData(stateObj._cleanupMetaData);
				}
				break;
			}
			case 210:
				if (!stateObj.TryStartNewRow(isNullCompressed: true, stateObj._cleanupMetaData.Length))
				{
					return false;
				}
				goto IL_08d4;
			case 209:
				if (!stateObj.TryStartNewRow(isNullCompressed: false))
				{
					return false;
				}
				goto IL_08d4;
			case 121:
			{
				if (!stateObj.TryReadInt32(out var value4))
				{
					return false;
				}
				cmdHandler?.OnReturnStatus(value4);
				break;
			}
			case 172:
			{
				if (!TryProcessReturnValue(tokenLength, stateObj, out var returnValue, cmdHandler?.ColumnEncryptionSetting ?? SqlCommandColumnEncryptionSetting.UseConnectionSetting))
				{
					return false;
				}
				cmdHandler?.OnReturnValue(returnValue, stateObj);
				break;
			}
			case 237:
				stateObj._syncOverAsync = true;
				ProcessSSPI(tokenLength);
				break;
			case 164:
				if (dataStream != null)
				{
					if (!TryProcessTableName(tokenLength, stateObj, out var multiPartTableNames))
					{
						return false;
					}
					dataStream.TableNames = multiPartTableNames;
				}
				else if (!stateObj.TrySkipBytes(tokenLength))
				{
					return false;
				}
				break;
			case 162:
				{
					if (!TryProcessResColSrcs(stateObj, tokenLength))
					{
						return false;
					}
					break;
				}
				IL_08d4:
				if (bulkCopyHandler != null)
				{
					if (!TryProcessRow(stateObj._cleanupMetaData, bulkCopyHandler.CreateRowBuffer(), bulkCopyHandler.CreateIndexMap(), stateObj))
					{
						return false;
					}
				}
				else if (RunBehavior.ReturnImmediately != (RunBehavior.ReturnImmediately & runBehavior))
				{
					if (!TrySkipRow(stateObj._cleanupMetaData, stateObj))
					{
						return false;
					}
				}
				else
				{
					dataReady = true;
				}
				if (_statistics != null)
				{
					_statistics.WaitForDoneAfterRow = true;
				}
				break;
			}
		}
		while ((stateObj._pendingData && RunBehavior.ReturnImmediately != (RunBehavior.ReturnImmediately & runBehavior)) || (!stateObj._pendingData && stateObj._attentionSent && !stateObj._attentionReceived));
		if (!stateObj._pendingData && CurrentTransaction != null)
		{
			CurrentTransaction.Activate();
		}
		if (stateObj._attentionReceived)
		{
			SpinWait.SpinUntil(() => !stateObj._attentionSending);
			if (stateObj._attentionSent)
			{
				stateObj._attentionSent = false;
				stateObj._attentionReceived = false;
				if (RunBehavior.Clean != (RunBehavior.Clean & runBehavior) && !stateObj.IsTimeoutStateExpired)
				{
					stateObj.AddError(new SqlError(0, 0, 11, _server, SQLMessage.OperationCancelled(), "", 0));
				}
			}
		}
		if (stateObj.HasErrorOrWarning)
		{
			ThrowExceptionAndWarning(stateObj);
		}
		return true;
	}

	private bool TryProcessEnvChange(int tokenLength, TdsParserStateObject stateObj, out SqlEnvChange[] sqlEnvChange)
	{
		int i = 0;
		int num = 0;
		SqlEnvChange[] array = new SqlEnvChange[3];
		sqlEnvChange = null;
		SqlEnvChange sqlEnvChange2;
		for (; tokenLength > i; i += sqlEnvChange2.length)
		{
			if (num >= array.Length)
			{
				SqlEnvChange[] array2 = new SqlEnvChange[array.Length + 3];
				for (int j = 0; j < array.Length; j++)
				{
					array2[j] = array[j];
				}
				array = array2;
			}
			sqlEnvChange2 = new SqlEnvChange();
			if (!stateObj.TryReadByte(out sqlEnvChange2.type))
			{
				return false;
			}
			array[num] = sqlEnvChange2;
			num++;
			byte value7;
			switch (sqlEnvChange2.type)
			{
			case 1:
			case 2:
				if (!TryReadTwoStringFields(sqlEnvChange2, stateObj))
				{
					return false;
				}
				break;
			case 3:
				if (!TryReadTwoStringFields(sqlEnvChange2, stateObj))
				{
					return false;
				}
				if (sqlEnvChange2.newValue == "iso_1")
				{
					_defaultCodePage = 1252;
					_defaultEncoding = Encoding.GetEncoding(_defaultCodePage);
				}
				else
				{
					string s = sqlEnvChange2.newValue.Substring(2);
					_defaultCodePage = int.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture);
					_defaultEncoding = Encoding.GetEncoding(_defaultCodePage);
				}
				break;
			case 4:
			{
				if (!TryReadTwoStringFields(sqlEnvChange2, stateObj))
				{
					throw SQL.SynchronousCallMayNotPend();
				}
				int num2 = int.Parse(sqlEnvChange2.newValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
				if (_physicalStateObj.SetPacketSize(num2))
				{
					_physicalStateObj.ClearAllWritePackets();
					uint pbQInfo = (uint)num2;
					uint num3 = SNINativeMethodWrapper.SNISetInfo(_physicalStateObj.Handle, SNINativeMethodWrapper.QTypes.SNI_QUERY_CONN_BUFSIZE, ref pbQInfo);
				}
				break;
			}
			case 5:
				if (!TryReadTwoStringFields(sqlEnvChange2, stateObj))
				{
					return false;
				}
				_defaultLCID = int.Parse(sqlEnvChange2.newValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
				break;
			case 6:
				if (!TryReadTwoStringFields(sqlEnvChange2, stateObj))
				{
					return false;
				}
				break;
			case 7:
				if (!stateObj.TryReadByte(out value7))
				{
					return false;
				}
				sqlEnvChange2.newLength = value7;
				if (sqlEnvChange2.newLength == 5)
				{
					if (!TryProcessCollation(stateObj, out sqlEnvChange2.newCollation))
					{
						return false;
					}
					_defaultCollation = sqlEnvChange2.newCollation;
					_defaultLCID = sqlEnvChange2.newCollation.LCID;
					int codePage = GetCodePage(sqlEnvChange2.newCollation, stateObj);
					if ((sqlEnvChange2.newCollation.info & 0x4000000) == 67108864)
					{
						_defaultEncoding = Encoding.UTF8;
						if (codePage != _defaultCodePage)
						{
							_defaultCodePage = codePage;
						}
					}
					else if (codePage != _defaultCodePage)
					{
						_defaultCodePage = codePage;
						_defaultEncoding = Encoding.GetEncoding(_defaultCodePage);
					}
				}
				if (!stateObj.TryReadByte(out value7))
				{
					return false;
				}
				sqlEnvChange2.oldLength = value7;
				if (sqlEnvChange2.oldLength == 5 && !TryProcessCollation(stateObj, out sqlEnvChange2.oldCollation))
				{
					return false;
				}
				sqlEnvChange2.length = 3 + sqlEnvChange2.newLength + sqlEnvChange2.oldLength;
				break;
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 17:
				if (!stateObj.TryReadByte(out value7))
				{
					return false;
				}
				sqlEnvChange2.newLength = value7;
				if (sqlEnvChange2.newLength > 0)
				{
					if (!stateObj.TryReadInt64(out sqlEnvChange2.newLongValue))
					{
						return false;
					}
				}
				else
				{
					sqlEnvChange2.newLongValue = 0L;
				}
				if (!stateObj.TryReadByte(out value7))
				{
					return false;
				}
				sqlEnvChange2.oldLength = value7;
				if (sqlEnvChange2.oldLength > 0)
				{
					if (!stateObj.TryReadInt64(out sqlEnvChange2.oldLongValue))
					{
						return false;
					}
				}
				else
				{
					sqlEnvChange2.oldLongValue = 0L;
				}
				sqlEnvChange2.length = 3 + sqlEnvChange2.newLength + sqlEnvChange2.oldLength;
				break;
			case 13:
				if (!TryReadTwoStringFields(sqlEnvChange2, stateObj))
				{
					return false;
				}
				break;
			case 15:
				if (!stateObj.TryReadInt32(out sqlEnvChange2.newLength))
				{
					return false;
				}
				sqlEnvChange2.newBinValue = new byte[sqlEnvChange2.newLength];
				if (!stateObj.TryReadByteArray(sqlEnvChange2.newBinValue, 0, sqlEnvChange2.newLength))
				{
					return false;
				}
				if (!stateObj.TryReadByte(out value7))
				{
					return false;
				}
				sqlEnvChange2.oldLength = value7;
				sqlEnvChange2.length = 5 + sqlEnvChange2.newLength;
				break;
			case 16:
			case 18:
				if (!TryReadTwoBinaryFields(sqlEnvChange2, stateObj))
				{
					return false;
				}
				break;
			case 19:
				if (!TryReadTwoStringFields(sqlEnvChange2, stateObj))
				{
					return false;
				}
				break;
			case 20:
			{
				if (!stateObj.TryReadUInt16(out var value))
				{
					return false;
				}
				sqlEnvChange2.newLength = value;
				if (!stateObj.TryReadByte(out var value2))
				{
					return false;
				}
				if (!stateObj.TryReadUInt16(out var value3))
				{
					return false;
				}
				if (!stateObj.TryReadUInt16(out var value4))
				{
					return false;
				}
				if (!stateObj.TryReadString(value4, out var value5))
				{
					return false;
				}
				sqlEnvChange2.newRoutingInfo = new RoutingInfo(value2, value3, value5);
				if (!stateObj.TryReadUInt16(out var value6))
				{
					return false;
				}
				if (!stateObj.TrySkipBytes(value6))
				{
					return false;
				}
				sqlEnvChange2.length = sqlEnvChange2.newLength + value6 + 5;
				break;
			}
			}
		}
		sqlEnvChange = array;
		return true;
	}

	private bool TryReadTwoBinaryFields(SqlEnvChange env, TdsParserStateObject stateObj)
	{
		if (!stateObj.TryReadByte(out var value))
		{
			return false;
		}
		env.newLength = value;
		env.newBinValue = new byte[env.newLength];
		if (!stateObj.TryReadByteArray(env.newBinValue, 0, env.newLength))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out value))
		{
			return false;
		}
		env.oldLength = value;
		env.oldBinValue = new byte[env.oldLength];
		if (!stateObj.TryReadByteArray(env.oldBinValue, 0, env.oldLength))
		{
			return false;
		}
		env.length = 3 + env.newLength + env.oldLength;
		return true;
	}

	private bool TryReadTwoStringFields(SqlEnvChange env, TdsParserStateObject stateObj)
	{
		if (!stateObj.TryReadByte(out var value))
		{
			return false;
		}
		if (!stateObj.TryReadString(value, out var value2))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value3))
		{
			return false;
		}
		if (!stateObj.TryReadString(value3, out var value4))
		{
			return false;
		}
		env.newLength = value;
		env.newValue = value2;
		env.oldLength = value3;
		env.oldValue = value4;
		env.length = 3 + env.newLength * 2 + env.oldLength * 2;
		return true;
	}

	private bool TryProcessDone(SqlCommand cmd, SqlDataReader reader, ref RunBehavior run, TdsParserStateObject stateObj)
	{
		if (LocalAppContextSwitches.MakeReadAsyncBlocking)
		{
			stateObj._syncOverAsync = true;
		}
		if (!stateObj.TryReadUInt16(out var value))
		{
			return false;
		}
		if (!stateObj.TryReadUInt16(out var value2))
		{
			return false;
		}
		int value4;
		if (_isYukon)
		{
			if (!stateObj.TryReadInt64(out var value3))
			{
				return false;
			}
			value4 = (int)value3;
		}
		else
		{
			if (!stateObj.TryReadInt32(out value4))
			{
				return false;
			}
			if (_state == TdsParserState.OpenNotLoggedIn && stateObj._inBytesRead > stateObj._inBytesUsed)
			{
				if (!stateObj.TryPeekByte(out var value5))
				{
					return false;
				}
				if (value5 == 0 && !stateObj.TryReadInt32(out value4))
				{
					return false;
				}
			}
		}
		if (32 == (value & 0x20))
		{
			stateObj._attentionReceived = true;
		}
		if (cmd != null && 16 == (value & 0x10))
		{
			if (value2 != 193)
			{
				if (cmd.IsDescribeParameterEncryptionRPCCurrentlyInProgress)
				{
					cmd.RowsAffectedByDescribeParameterEncryption = value4;
				}
				else
				{
					cmd.InternalRecordsAffected = value4;
				}
			}
			if (stateObj._receivedColMetaData || value2 != 193)
			{
				cmd.OnStatementCompleted(value4);
			}
		}
		stateObj._receivedColMetaData = false;
		if (2 == (2 & value) && stateObj.ErrorCount == 0 && !stateObj._errorTokenReceived && RunBehavior.Clean != (RunBehavior.Clean & run))
		{
			stateObj.AddError(new SqlError(0, 0, 11, _server, SQLMessage.SevereError(), "", 0));
			if (reader != null && !reader.IsInitialized)
			{
				run = RunBehavior.UntilDone;
			}
		}
		if (256 == (0x100 & value) && RunBehavior.Clean != (RunBehavior.Clean & run))
		{
			stateObj.AddError(new SqlError(0, 0, 20, _server, SQLMessage.SevereError(), "", 0));
			if (reader != null && !reader.IsInitialized)
			{
				run = RunBehavior.UntilDone;
			}
		}
		ProcessSqlStatistics(value2, value, value4);
		if (1 != (value & 1))
		{
			stateObj._errorTokenReceived = false;
			if (stateObj._inBytesUsed >= stateObj._inBytesRead)
			{
				stateObj._pendingData = false;
			}
		}
		if (!stateObj._pendingData && stateObj._hasOpenResult)
		{
			stateObj.DecrementOpenResultCount();
		}
		return true;
	}

	private void ProcessSqlStatistics(ushort curCmd, ushort status, int count)
	{
		if (_statistics != null)
		{
			if (_statistics.WaitForDoneAfterRow)
			{
				_statistics.SafeIncrement(ref _statistics._sumResultSets);
				_statistics.WaitForDoneAfterRow = false;
			}
			if (16 != (status & 0x10))
			{
				count = 0;
			}
			switch (curCmd)
			{
			case 195:
			case 196:
			case 197:
			case 279:
				_statistics.SafeIncrement(ref _statistics._iduCount);
				_statistics.SafeAdd(ref _statistics._iduRows, count);
				if (!_statisticsIsInTransaction)
				{
					_statistics.SafeIncrement(ref _statistics._transactions);
				}
				break;
			case 193:
				_statistics.SafeIncrement(ref _statistics._selectCount);
				_statistics.SafeAdd(ref _statistics._selectRows, count);
				break;
			case 212:
				if (!_statisticsIsInTransaction)
				{
					_statistics.SafeIncrement(ref _statistics._transactions);
				}
				_statisticsIsInTransaction = true;
				break;
			case 32:
				_statistics.SafeIncrement(ref _statistics._cursorOpens);
				break;
			case 210:
				_statisticsIsInTransaction = false;
				break;
			case 213:
				_statisticsIsInTransaction = false;
				break;
			}
		}
		else
		{
			switch (curCmd)
			{
			case 212:
				_statisticsIsInTransaction = true;
				break;
			case 210:
			case 213:
				_statisticsIsInTransaction = false;
				break;
			case 211:
				break;
			}
		}
	}

	private bool TryProcessFeatureExtAck(TdsParserStateObject stateObj)
	{
		byte value;
		do
		{
			if (!stateObj.TryReadByte(out value))
			{
				return false;
			}
			if (value != byte.MaxValue)
			{
				if (!stateObj.TryReadUInt32(out var value2))
				{
					return false;
				}
				byte[] array = new byte[value2];
				if (value2 != 0 && !stateObj.TryReadByteArray(array, 0, checked((int)value2)))
				{
					return false;
				}
				_connHandler.OnFeatureExtAck(value, array);
			}
		}
		while (value != byte.MaxValue);
		bool flag = false;
		if (_connHandler._cleanSQLDNSCaching)
		{
			flag = SQLFallbackDNSCache.Instance.DeleteDNSInfo(FQDNforDNSCahce);
		}
		if (_connHandler.IsSQLDNSCachingSupported && _connHandler.pendingSQLDNSObject != null && !SQLFallbackDNSCache.Instance.IsDuplicate(_connHandler.pendingSQLDNSObject))
		{
			flag = SQLFallbackDNSCache.Instance.AddDNSInfo(_connHandler.pendingSQLDNSObject);
			_connHandler.pendingSQLDNSObject = null;
		}
		if (Connection.RoutingInfo == null && _connHandler.ConnectionOptions.ColumnEncryptionSetting == SqlConnectionColumnEncryptionSetting.Enabled && !IsColumnEncryptionSupported)
		{
			throw SQL.TceNotSupported();
		}
		if (Connection.RoutingInfo == null)
		{
			SqlConnectionAttestationProtocol attestationProtocol = _connHandler.ConnectionOptions.AttestationProtocol;
			if (TceVersionSupported < 2)
			{
				if (!string.IsNullOrWhiteSpace(_connHandler.ConnectionOptions.EnclaveAttestationUrl) && attestationProtocol != 0)
				{
					throw SQL.EnclaveComputationsNotSupported();
				}
				if (!string.IsNullOrWhiteSpace(_connHandler.ConnectionOptions.EnclaveAttestationUrl))
				{
					throw SQL.AttestationURLNotSupported();
				}
				if (_connHandler.ConnectionOptions.AttestationProtocol != 0)
				{
					throw SQL.AttestationProtocolNotSupported();
				}
			}
			if (!string.IsNullOrWhiteSpace(_connHandler.ConnectionOptions.EnclaveAttestationUrl))
			{
				if (string.IsNullOrWhiteSpace(EnclaveType))
				{
					throw SQL.EnclaveTypeNotReturned();
				}
				if (attestationProtocol != 0 && !IsValidAttestationProtocol(attestationProtocol, EnclaveType))
				{
					throw SQL.AttestationProtocolNotSupportEnclaveType(ConvertAttestationProtocolToString(attestationProtocol), EnclaveType);
				}
			}
		}
		return true;
	}

	private bool IsValidAttestationProtocol(SqlConnectionAttestationProtocol attestationProtocol, string enclaveType)
	{
		string text = enclaveType.ToUpper();
		if (!(text == "VBS"))
		{
			if (!(text == "SGX"))
			{
				throw SQL.EnclaveTypeNotSupported(enclaveType);
			}
			if (attestationProtocol != SqlConnectionAttestationProtocol.AAS)
			{
				return false;
			}
		}
		else if (attestationProtocol != SqlConnectionAttestationProtocol.AAS && attestationProtocol != SqlConnectionAttestationProtocol.HGS)
		{
			return false;
		}
		return true;
	}

	private string ConvertAttestationProtocolToString(SqlConnectionAttestationProtocol attestationProtocol)
	{
		return attestationProtocol switch
		{
			SqlConnectionAttestationProtocol.AAS => "AAS", 
			SqlConnectionAttestationProtocol.HGS => "HGS", 
			_ => "NotSpecified", 
		};
	}

	private bool TryReadByteString(TdsParserStateObject stateObj, out string value)
	{
		value = string.Empty;
		if (!stateObj.TryReadByte(out var value2))
		{
			return false;
		}
		if (!stateObj.TryReadString(value2, out value))
		{
			return false;
		}
		return true;
	}

	private bool TryReadSensitivityLabel(TdsParserStateObject stateObj, out string label, out string id)
	{
		label = string.Empty;
		id = string.Empty;
		if (!TryReadByteString(stateObj, out label))
		{
			return false;
		}
		if (!TryReadByteString(stateObj, out id))
		{
			return false;
		}
		return true;
	}

	private bool TryReadSensitivityInformationType(TdsParserStateObject stateObj, out string informationType, out string id)
	{
		informationType = string.Empty;
		id = string.Empty;
		if (!TryReadByteString(stateObj, out informationType))
		{
			return false;
		}
		if (!TryReadByteString(stateObj, out id))
		{
			return false;
		}
		return true;
	}

	private bool TryProcessDataClassification(TdsParserStateObject stateObj, out SensitivityClassification sensitivityClassification)
	{
		if (DataClassificationVersion == 0)
		{
			throw SQL.ParsingError(ParsingErrorState.DataClassificationNotExpected);
		}
		sensitivityClassification = null;
		if (!stateObj.TryReadUInt16(out var value))
		{
			return false;
		}
		List<Label> list = new List<Label>(value);
		for (ushort num = 0; num < value; num++)
		{
			if (!TryReadSensitivityLabel(stateObj, out var label, out var id))
			{
				return false;
			}
			list.Add(new Label(label, id));
		}
		if (!stateObj.TryReadUInt16(out var value2))
		{
			return false;
		}
		List<InformationType> list2 = new List<InformationType>(value2);
		for (ushort num2 = 0; num2 < value2; num2++)
		{
			if (!TryReadSensitivityInformationType(stateObj, out var informationType, out var id2))
			{
				return false;
			}
			list2.Add(new InformationType(informationType, id2));
		}
		int value3 = -1;
		if (DataClassificationVersion > 1 && (!stateObj.TryReadInt32(out value3) || !Enum.IsDefined(typeof(SensitivityRank), value3)))
		{
			return false;
		}
		if (!stateObj.TryReadUInt16(out var value4))
		{
			return false;
		}
		List<ColumnSensitivity> list3 = new List<ColumnSensitivity>(value4);
		for (ushort num3 = 0; num3 < value4; num3++)
		{
			if (!stateObj.TryReadUInt16(out var value5))
			{
				return false;
			}
			List<SensitivityProperty> list4 = new List<SensitivityProperty>(value5);
			for (ushort num4 = 0; num4 < value5; num4++)
			{
				if (!stateObj.TryReadUInt16(out var value6))
				{
					return false;
				}
				Label label2 = null;
				if (value6 != ushort.MaxValue)
				{
					if (value6 >= list.Count)
					{
						throw SQL.ParsingError(ParsingErrorState.DataClassificationInvalidLabelIndex);
					}
					label2 = list[value6];
				}
				if (!stateObj.TryReadUInt16(out var value7))
				{
					return false;
				}
				InformationType informationType2 = null;
				if (value7 != ushort.MaxValue)
				{
					if (value7 >= list2.Count)
					{
						throw SQL.ParsingError(ParsingErrorState.DataClassificationInvalidInformationTypeIndex);
					}
					informationType2 = list2[value7];
				}
				int value8 = -1;
				if (DataClassificationVersion > 1 && (!stateObj.TryReadInt32(out value8) || !Enum.IsDefined(typeof(SensitivityRank), value8)))
				{
					return false;
				}
				list4.Add(new SensitivityProperty(label2, informationType2, (SensitivityRank)value8));
			}
			list3.Add(new ColumnSensitivity(list4));
		}
		sensitivityClassification = new SensitivityClassification(list, list2, list3, (SensitivityRank)value3);
		return true;
	}

	private bool TryProcessResColSrcs(TdsParserStateObject stateObj, int tokenLength)
	{
		if (!stateObj.TrySkipBytes(tokenLength))
		{
			return false;
		}
		return true;
	}

	private bool TryProcessSessionState(TdsParserStateObject stateObj, int length, SessionData sdata)
	{
		if (length < 5)
		{
			throw SQL.ParsingErrorLength(ParsingErrorState.SessionStateLengthTooShort, length);
		}
		if (!stateObj.TryReadUInt32(out var value))
		{
			return false;
		}
		if (value == uint.MaxValue)
		{
			_connHandler.DoNotPoolThisConnection();
		}
		if (!stateObj.TryReadByte(out var value2))
		{
			return false;
		}
		if (value2 > 1)
		{
			throw SQL.ParsingErrorStatus(ParsingErrorState.SessionStateInvalidStatus, value2);
		}
		bool flag = value2 != 0;
		length -= 5;
		while (length > 0)
		{
			if (!stateObj.TryReadByte(out var value3))
			{
				return false;
			}
			if (!stateObj.TryReadByte(out var value4))
			{
				return false;
			}
			int value5;
			if (value4 < byte.MaxValue)
			{
				value5 = value4;
			}
			else if (!stateObj.TryReadInt32(out value5))
			{
				return false;
			}
			byte[] array = null;
			checked
			{
				lock (sdata._delta)
				{
					if (sdata._delta[value3] == null)
					{
						array = new byte[value5];
						sdata._delta[value3] = new SessionStateRecord
						{
							_version = value,
							_dataLength = value5,
							_data = array,
							_recoverable = flag
						};
						sdata._deltaDirty = true;
						if (!flag)
						{
							sdata._unrecoverableStatesCount = (byte)(unchecked((uint)sdata._unrecoverableStatesCount) + 1u);
						}
					}
					else if (sdata._delta[value3]._version <= value)
					{
						SessionStateRecord sessionStateRecord = sdata._delta[value3];
						sessionStateRecord._version = value;
						sessionStateRecord._dataLength = value5;
						if (sessionStateRecord._recoverable != flag)
						{
							if (flag)
							{
								unchecked
								{
									sdata._unrecoverableStatesCount--;
								}
							}
							else
							{
								sdata._unrecoverableStatesCount = (byte)(unchecked((uint)sdata._unrecoverableStatesCount) + 1u);
							}
							sessionStateRecord._recoverable = flag;
						}
						array = sessionStateRecord._data;
						if (array.Length < value5)
						{
							array = (sessionStateRecord._data = new byte[value5]);
						}
					}
				}
				if (array != null)
				{
					if (!stateObj.TryReadByteArray(array, 0, value5))
					{
						return false;
					}
				}
				else if (!stateObj.TrySkipBytes(value5))
				{
					return false;
				}
			}
			length = ((value4 >= byte.MaxValue) ? (length - (6 + value5)) : (length - (2 + value5)));
		}
		return true;
	}

	private bool TryProcessLoginAck(TdsParserStateObject stateObj, out SqlLoginAck sqlLoginAck)
	{
		SqlLoginAck sqlLoginAck2 = new SqlLoginAck();
		sqlLoginAck = null;
		if (!stateObj.TrySkipBytes(1))
		{
			return false;
		}
		byte[] array = new byte[4];
		if (!stateObj.TryReadByteArray(array, 0, array.Length))
		{
			return false;
		}
		sqlLoginAck2.tdsVersion = (uint)((((((array[0] << 8) | array[1]) << 8) | array[2]) << 8) | array[3]);
		uint num = sqlLoginAck2.tdsVersion & 0xFF00FFFFu;
		uint num2 = (sqlLoginAck2.tdsVersion >> 16) & 0xFFu;
		switch (num)
		{
		case 117440512u:
			switch (num2)
			{
			case 1u:
				_isShiloh = true;
				break;
			default:
				throw SQL.InvalidTDSVersion();
			case 0u:
				break;
			}
			break;
		case 1895825409u:
			if (num2 != 0)
			{
				throw SQL.InvalidTDSVersion();
			}
			_isShilohSP1 = true;
			break;
		case 1912602626u:
			if (num2 != 9)
			{
				throw SQL.InvalidTDSVersion();
			}
			_isYukon = true;
			break;
		case 1929379843u:
			if (num2 != 11)
			{
				throw SQL.InvalidTDSVersion();
			}
			_isKatmai = true;
			break;
		case 1946157060u:
			if (num2 != 0)
			{
				throw SQL.InvalidTDSVersion();
			}
			_isDenali = true;
			break;
		default:
			throw SQL.InvalidTDSVersion();
		}
		_isKatmai |= _isDenali;
		_isYukon |= _isKatmai;
		_isShilohSP1 |= _isYukon;
		_isShiloh |= _isShilohSP1;
		sqlLoginAck2.isVersion8 = _isShiloh;
		stateObj._outBytesUsed = stateObj._outputHeaderLen;
		if (!stateObj.TryReadByte(out var value))
		{
			return false;
		}
		if (!stateObj.TryReadString(value, out sqlLoginAck2.programName))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out sqlLoginAck2.majorVersion))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out sqlLoginAck2.minorVersion))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value2))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value3))
		{
			return false;
		}
		sqlLoginAck2.buildNum = (short)((value2 << 8) + value3);
		_state = TdsParserState.OpenLoggedIn;
		if (_isYukon && _fMARS)
		{
			_resetConnectionEvent = new AutoResetEvent(initialState: true);
		}
		if (_connHandler.ConnectionOptions.UserInstance && ADP.IsEmpty(_connHandler.InstanceName))
		{
			stateObj.AddError(new SqlError(0, 0, 20, Server, SQLMessage.UserInstanceFailure(), "", 0));
			ThrowExceptionAndWarning(stateObj);
		}
		sqlLoginAck = sqlLoginAck2;
		return true;
	}

	private bool TryProcessFedAuthInfo(TdsParserStateObject stateObj, int tokenLen, out SqlFedAuthInfo sqlFedAuthInfo)
	{
		sqlFedAuthInfo = null;
		SqlFedAuthInfo sqlFedAuthInfo2 = new SqlFedAuthInfo();
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParser.TryProcessFedAuthInfo> FEDAUTHINFO token stream length = {0}", tokenLen);
		if (tokenLen < 4)
		{
			SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.TryProcessFedAuthInfo|ERR> FEDAUTHINFO token stream length too short for CountOfInfoIDs.");
			throw SQL.ParsingErrorLength(ParsingErrorState.FedAuthInfoLengthTooShortForCountOfInfoIds, tokenLen);
		}
		if (!stateObj.TryReadUInt32(out var value))
		{
			SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.TryProcessFedAuthInfo|ERR> Failed to read CountOfInfoIDs in FEDAUTHINFO token stream.");
			throw SQL.ParsingError(ParsingErrorState.FedAuthInfoFailedToReadCountOfInfoIds);
		}
		tokenLen -= 4;
		if (SqlClientEventSource.Log.IsAdvancedTraceOn())
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParser.TryProcessFedAuthInfo|ADV> CountOfInfoIDs = {0}", value.ToString(CultureInfo.InvariantCulture));
		}
		if (tokenLen > 0)
		{
			byte[] array = new byte[tokenLen];
			int totalRead = 0;
			bool flag = stateObj.TryReadByteArray(array, 0, tokenLen, out totalRead);
			if (SqlClientEventSource.Log.IsAdvancedTraceOn())
			{
				SqlClientEventSource.Log.AdvancedTraceEvent("<sc.TdsParser.TryProcessFedAuthInfo|ADV> Read rest of FEDAUTHINFO token stream: {0}", BitConverter.ToString(array, 0, totalRead));
			}
			if (!flag || totalRead != tokenLen)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.TryProcessFedAuthInfo|ERR> Failed to read FEDAUTHINFO token stream. Attempted to read {0} bytes, actually read {1}", tokenLen, totalRead);
				throw SQL.ParsingError(ParsingErrorState.FedAuthInfoFailedToReadTokenStream);
			}
			uint num = checked(value * 9);
			for (uint num2 = 0u; num2 < value; num2++)
			{
				byte b;
				string @string;
				checked
				{
					uint num3 = num2 * 9;
					b = array[num3];
					uint num4 = BitConverter.ToUInt32(array, (int)(num3 + 1));
					uint num5 = BitConverter.ToUInt32(array, (int)(num3 + 5));
					if (SqlClientEventSource.Log.IsAdvancedTraceOn())
					{
						SqlClientEventSource.Log.AdvancedTraceEvent("<sc.TdsParser.TryProcessFedAuthInfo> FedAuthInfoOpt: ID={0}, DataLen={1}, Offset={2}", b, num4.ToString(CultureInfo.InvariantCulture), num5.ToString(CultureInfo.InvariantCulture));
					}
					num5 -= 4;
					if (num5 < num || num5 >= tokenLen)
					{
						SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.TryProcessFedAuthInfo|ERR> FedAuthInfoDataOffset points to an invalid location.");
						throw SQL.ParsingErrorOffset(ParsingErrorState.FedAuthInfoInvalidOffset, unchecked((int)num5));
					}
					try
					{
						@string = Encoding.Unicode.GetString(array, (int)num5, (int)num4);
					}
					catch (ArgumentOutOfRangeException innerException)
					{
						SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.TryProcessFedAuthInfo|ERR> Failed to read FedAuthInfoData.");
						throw SQL.ParsingError(ParsingErrorState.FedAuthInfoFailedToReadData, innerException);
					}
					catch (ArgumentException innerException2)
					{
						SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.TryProcessFedAuthInfo|ERR> FedAuthInfoData is not in unicode format.");
						throw SQL.ParsingError(ParsingErrorState.FedAuthInfoDataNotUnicode, innerException2);
					}
					SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParser.TryProcessFedAuthInfo|ADV> FedAuthInfoData: {0}", @string);
				}
				switch ((TdsEnums.FedAuthInfoId)b)
				{
				case TdsEnums.FedAuthInfoId.Spn:
					sqlFedAuthInfo2.spn = @string;
					break;
				case TdsEnums.FedAuthInfoId.Stsurl:
					sqlFedAuthInfo2.stsurl = @string;
					break;
				default:
					SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParser.TryProcessFedAuthInfo|ADV> Ignoring unknown federated authentication info option: {0}", b);
					break;
				}
			}
			SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.TryProcessFedAuthInfo> Processed FEDAUTHINFO token stream: {0}", sqlFedAuthInfo2);
			if (string.IsNullOrWhiteSpace(sqlFedAuthInfo2.stsurl) || string.IsNullOrWhiteSpace(sqlFedAuthInfo2.spn))
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.TryProcessFedAuthInfo|ERR> FEDAUTHINFO token stream does not contain both STSURL and SPN.");
				throw SQL.ParsingError(ParsingErrorState.FedAuthInfoDoesNotContainStsurlAndSpn);
			}
			sqlFedAuthInfo = sqlFedAuthInfo2;
			return true;
		}
		SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.TryProcessFedAuthInfo|ERR> FEDAUTHINFO token stream is not long enough to contain the data it claims to.");
		throw SQL.ParsingErrorLength(ParsingErrorState.FedAuthInfoLengthTooShortForData, tokenLen);
	}

	internal bool TryProcessError(byte token, TdsParserStateObject stateObj, out SqlError error)
	{
		error = null;
		if (!stateObj.TryReadInt32(out var value))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value2))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value3))
		{
			return false;
		}
		if (!stateObj.TryReadUInt16(out var value4))
		{
			return false;
		}
		if (!stateObj.TryReadString(value4, out var value5))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value6))
		{
			return false;
		}
		string server;
		if (value6 == 0)
		{
			server = _server;
		}
		else if (!stateObj.TryReadString(value6, out server))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out value6))
		{
			return false;
		}
		if (!stateObj.TryReadString(value6, out var value7))
		{
			return false;
		}
		int value8;
		if (_isYukon)
		{
			if (!stateObj.TryReadInt32(out value8))
			{
				return false;
			}
		}
		else
		{
			if (!stateObj.TryReadUInt16(out var value9))
			{
				return false;
			}
			value8 = value9;
			if (_state == TdsParserState.OpenNotLoggedIn)
			{
				if (!stateObj.TryPeekByte(out var value10))
				{
					return false;
				}
				if (value10 == 0)
				{
					if (!stateObj.TryReadUInt16(out var value11))
					{
						return false;
					}
					value8 = (value8 << 16) + value11;
				}
			}
		}
		error = new SqlError(value, value2, value3, _server, value5, value7, value8);
		return true;
	}

	internal bool TryProcessReturnValue(int length, TdsParserStateObject stateObj, out SqlReturnValue returnValue, SqlCommandColumnEncryptionSetting columnEncryptionSetting)
	{
		returnValue = null;
		SqlReturnValue sqlReturnValue = new SqlReturnValue();
		sqlReturnValue.length = length;
		if (_isYukon && !stateObj.TryReadUInt16(out sqlReturnValue.parmIndex))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value))
		{
			return false;
		}
		sqlReturnValue.parameter = null;
		if (value > 0 && !stateObj.TryReadString(value, out sqlReturnValue.parameter))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var _))
		{
			return false;
		}
		uint value3;
		if (IsYukonOrNewer)
		{
			if (!stateObj.TryReadUInt32(out value3))
			{
				return false;
			}
		}
		else
		{
			if (!stateObj.TryReadUInt16(out var value4))
			{
				return false;
			}
			value3 = value4;
		}
		if (!stateObj.TryReadByte(out var value5))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out value5))
		{
			return false;
		}
		if (_serverSupportsColumnEncryption)
		{
			sqlReturnValue.isEncrypted = 8 == (value5 & 8);
		}
		if (!stateObj.TryReadByte(out var value6))
		{
			return false;
		}
		int tokenLength;
		if (value6 == 241)
		{
			tokenLength = 65535;
		}
		else if (IsVarTimeTds(value6))
		{
			tokenLength = 0;
		}
		else if (value6 == 40)
		{
			tokenLength = 3;
		}
		else if (!TryGetTokenLength(value6, stateObj, out tokenLength))
		{
			return false;
		}
		sqlReturnValue.metaType = MetaType.GetSqlDataType(value6, value3, tokenLength);
		sqlReturnValue.type = sqlReturnValue.metaType.SqlDbType;
		if (_isShiloh)
		{
			sqlReturnValue.tdsType = sqlReturnValue.metaType.NullableType;
			sqlReturnValue.IsNullable = true;
			if (tokenLength == 65535)
			{
				sqlReturnValue.metaType = MetaType.GetMaxMetaTypeFromMetaType(sqlReturnValue.metaType);
			}
		}
		else
		{
			if (sqlReturnValue.metaType.NullableType == value6)
			{
				sqlReturnValue.IsNullable = true;
			}
			sqlReturnValue.tdsType = value6;
		}
		if (sqlReturnValue.type == SqlDbType.Decimal)
		{
			if (!stateObj.TryReadByte(out sqlReturnValue.precision))
			{
				return false;
			}
			if (!stateObj.TryReadByte(out sqlReturnValue.scale))
			{
				return false;
			}
		}
		if (sqlReturnValue.metaType.IsVarTime && !stateObj.TryReadByte(out sqlReturnValue.scale))
		{
			return false;
		}
		if (value6 == 240 && !TryProcessUDTMetaData(sqlReturnValue, stateObj))
		{
			return false;
		}
		if (sqlReturnValue.type == SqlDbType.Xml)
		{
			if (!stateObj.TryReadByte(out var value7))
			{
				return false;
			}
			if (((uint)value7 & (true ? 1u : 0u)) != 0)
			{
				if (!stateObj.TryReadByte(out value))
				{
					return false;
				}
				if (sqlReturnValue.xmlSchemaCollection == null)
				{
					sqlReturnValue.xmlSchemaCollection = new SqlMetaDataXmlSchemaCollection();
				}
				if (value != 0 && !stateObj.TryReadString(value, out sqlReturnValue.xmlSchemaCollection.Database))
				{
					return false;
				}
				if (!stateObj.TryReadByte(out value))
				{
					return false;
				}
				if (value != 0 && !stateObj.TryReadString(value, out sqlReturnValue.xmlSchemaCollection.OwningSchema))
				{
					return false;
				}
				if (!stateObj.TryReadInt16(out var value8))
				{
					return false;
				}
				if (value8 != 0 && !stateObj.TryReadString(value8, out sqlReturnValue.xmlSchemaCollection.Name))
				{
					return false;
				}
			}
		}
		else if (_isShiloh && sqlReturnValue.metaType.IsCharType)
		{
			if (!TryProcessCollation(stateObj, out sqlReturnValue.collation))
			{
				return false;
			}
			if ((sqlReturnValue.collation.info & 0x4000000) == 67108864)
			{
				sqlReturnValue.encoding = Encoding.UTF8;
			}
			else
			{
				int codePage = GetCodePage(sqlReturnValue.collation, stateObj);
				if (codePage == _defaultCodePage)
				{
					sqlReturnValue.codePage = _defaultCodePage;
					sqlReturnValue.encoding = _defaultEncoding;
				}
				else
				{
					sqlReturnValue.codePage = codePage;
					sqlReturnValue.encoding = Encoding.GetEncoding(sqlReturnValue.codePage);
				}
			}
		}
		if (_serverSupportsColumnEncryption && sqlReturnValue.isEncrypted && !TryProcessTceCryptoMetadata(stateObj, sqlReturnValue, null, columnEncryptionSetting, isReturnValue: true))
		{
			return false;
		}
		bool isNull = false;
		if (!TryProcessColumnHeaderNoNBC(sqlReturnValue, stateObj, out isNull, out var length2))
		{
			return false;
		}
		int length3 = (int)((length2 > int.MaxValue) ? int.MaxValue : length2);
		if (sqlReturnValue.metaType.IsPlp)
		{
			length3 = int.MaxValue;
		}
		if (isNull)
		{
			GetNullSqlValue(sqlReturnValue.value, sqlReturnValue, SqlCommandColumnEncryptionSetting.Disabled, _connHandler);
		}
		else if (!TryReadSqlValue(sqlReturnValue.value, sqlReturnValue, length3, stateObj, SqlCommandColumnEncryptionSetting.Disabled, null))
		{
			return false;
		}
		returnValue = sqlReturnValue;
		return true;
	}

	internal bool TryProcessTceCryptoMetadata(TdsParserStateObject stateObj, SqlMetaDataPriv col, SqlTceCipherInfoTable cipherTable, SqlCommandColumnEncryptionSetting columnEncryptionSetting, bool isReturnValue)
	{
		ushort value = 0;
		if (cipherTable != null)
		{
			if (!stateObj.TryReadUInt16(out value))
			{
				return false;
			}
			if (value >= cipherTable.Size)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.TryProcessTceCryptoMetadata|TCE> Incorrect ordinal received {0}, max tab size: {1}", value, cipherTable.Size);
				throw SQL.ParsingErrorValue(ParsingErrorState.TceInvalidOrdinalIntoCipherInfoTable, value);
			}
		}
		if (!stateObj.TryReadUInt32(out var value2))
		{
			return false;
		}
		col.baseTI = new SqlMetaDataPriv();
		if (!TryProcessTypeInfo(stateObj, col.baseTI, value2))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value3))
		{
			return false;
		}
		string value4 = null;
		if (value3 == 0)
		{
			if (!stateObj.TryReadByte(out var value5))
			{
				return false;
			}
			if (!stateObj.TryReadString(value5, out value4))
			{
				return false;
			}
		}
		if (!stateObj.TryReadByte(out var value6))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value7))
		{
			return false;
		}
		if (columnEncryptionSetting == SqlCommandColumnEncryptionSetting.Enabled || (columnEncryptionSetting == SqlCommandColumnEncryptionSetting.ResultSetOnly && !isReturnValue) || (columnEncryptionSetting == SqlCommandColumnEncryptionSetting.UseConnectionSetting && _connHandler != null && _connHandler.ConnectionOptions != null && _connHandler.ConnectionOptions.ColumnEncryptionSetting == SqlConnectionColumnEncryptionSetting.Enabled))
		{
			col.cipherMD = new SqlCipherMetadata(cipherTable?[value], value, value3, value4, value6, value7);
		}
		else
		{
			col.isEncrypted = false;
		}
		return true;
	}

	internal bool TryProcessCollation(TdsParserStateObject stateObj, out SqlCollation collation)
	{
		SqlCollation sqlCollation = new SqlCollation();
		if (!stateObj.TryReadUInt32(out sqlCollation.info))
		{
			collation = null;
			return false;
		}
		if (!stateObj.TryReadByte(out sqlCollation.sortId))
		{
			collation = null;
			return false;
		}
		collation = sqlCollation;
		return true;
	}

	private void WriteCollation(SqlCollation collation, TdsParserStateObject stateObj)
	{
		if (collation == null)
		{
			_physicalStateObj.WriteByte(0);
			return;
		}
		_physicalStateObj.WriteByte(5);
		WriteUnsignedInt(collation.info, _physicalStateObj);
		_physicalStateObj.WriteByte(collation.sortId);
	}

	internal int GetCodePage(SqlCollation collation, TdsParserStateObject stateObj)
	{
		int num = 0;
		if (collation.sortId != 0)
		{
			num = TdsEnums.CODE_PAGE_FROM_SORT_ID[collation.sortId];
		}
		else
		{
			int lCID = collation.LCID;
			bool flag = false;
			try
			{
				num = CultureInfo.GetCultureInfo(lCID).TextInfo.ANSICodePage;
				flag = true;
			}
			catch (ArgumentException e)
			{
				ADP.TraceExceptionWithoutRethrow(e);
			}
			if (!flag || num == 0)
			{
				CultureInfo cultureInfo = null;
				switch (lCID)
				{
				case 66564:
				case 66577:
				case 66578:
				case 67588:
				case 68612:
				case 69636:
				case 70660:
					lCID &= 0x3FFF;
					try
					{
						cultureInfo = new CultureInfo(lCID);
						flag = true;
					}
					catch (ArgumentException e3)
					{
						ADP.TraceExceptionWithoutRethrow(e3);
					}
					break;
				case 2087:
					try
					{
						cultureInfo = new CultureInfo(1063);
						flag = true;
					}
					catch (ArgumentException e2)
					{
						ADP.TraceExceptionWithoutRethrow(e2);
					}
					break;
				case 1087:
					num = 1251;
					break;
				}
				if (!flag)
				{
					ThrowUnsupportedCollationEncountered(stateObj);
				}
				if (cultureInfo != null)
				{
					num = cultureInfo.TextInfo.ANSICodePage;
				}
			}
		}
		return num;
	}

	internal void DrainData(TdsParserStateObject stateObj)
	{
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			try
			{
				SqlDataReader.SharedState readerState = stateObj._readerState;
				if (readerState != null && readerState._dataReady)
				{
					_SqlMetaDataSet cleanupMetaData = stateObj._cleanupMetaData;
					if (stateObj._partialHeaderBytesRead > 0 && !stateObj.TryProcessHeader())
					{
						throw SQL.SynchronousCallMayNotPend();
					}
					if (readerState._nextColumnHeaderToRead == 0)
					{
						if (!stateObj.Parser.TrySkipRow(stateObj._cleanupMetaData, stateObj))
						{
							throw SQL.SynchronousCallMayNotPend();
						}
					}
					else
					{
						if (readerState._nextColumnDataToRead < readerState._nextColumnHeaderToRead)
						{
							if (readerState._nextColumnHeaderToRead > 0 && cleanupMetaData[readerState._nextColumnHeaderToRead - 1].metaType.IsPlp)
							{
								if (stateObj._longlen != 0L && !TrySkipPlpValue(ulong.MaxValue, stateObj, out var _))
								{
									throw SQL.SynchronousCallMayNotPend();
								}
							}
							else if (0 < readerState._columnDataBytesRemaining && !stateObj.TrySkipLongBytes(readerState._columnDataBytesRemaining))
							{
								throw SQL.SynchronousCallMayNotPend();
							}
						}
						if (!stateObj.Parser.TrySkipRow(cleanupMetaData, readerState._nextColumnHeaderToRead, stateObj))
						{
							throw SQL.SynchronousCallMayNotPend();
						}
					}
				}
				Run(RunBehavior.Clean, null, null, null, stateObj);
			}
			catch
			{
				_connHandler.DoomThisConnection();
				throw;
			}
		}
		catch (OutOfMemoryException)
		{
			_connHandler.DoomThisConnection();
			throw;
		}
		catch (StackOverflowException)
		{
			_connHandler.DoomThisConnection();
			throw;
		}
		catch (ThreadAbortException)
		{
			_connHandler.DoomThisConnection();
			throw;
		}
	}

	internal void ThrowUnsupportedCollationEncountered(TdsParserStateObject stateObj)
	{
		stateObj.AddError(new SqlError(0, 0, 11, _server, SQLMessage.CultureIdError(), "", 0));
		if (stateObj != null)
		{
			DrainData(stateObj);
			stateObj._pendingData = false;
		}
		ThrowExceptionAndWarning(stateObj);
	}

	internal bool TryProcessAltMetaData(int cColumns, TdsParserStateObject stateObj, out _SqlMetaDataSet metaData)
	{
		metaData = null;
		_SqlMetaDataSet sqlMetaDataSet = new _SqlMetaDataSet(cColumns, null);
		int[] array = new int[cColumns];
		if (!stateObj.TryReadUInt16(out sqlMetaDataSet.id))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value))
		{
			return false;
		}
		while (value > 0)
		{
			if (!stateObj.TrySkipBytes(2))
			{
				return false;
			}
			value--;
		}
		for (int i = 0; i < cColumns; i++)
		{
			_SqlMetaData sqlMetaData = sqlMetaDataSet[i];
			if (!stateObj.TryReadByte(out sqlMetaData.op))
			{
				return false;
			}
			if (!stateObj.TryReadUInt16(out sqlMetaData.operand))
			{
				return false;
			}
			if (!TryCommonProcessMetaData(stateObj, sqlMetaData, null, fColMD: false, SqlCommandColumnEncryptionSetting.Disabled))
			{
				return false;
			}
			if (ADP.IsEmpty(sqlMetaData.column))
			{
				switch (sqlMetaData.op)
				{
				case 79:
					sqlMetaData.column = "avg";
					break;
				case 75:
					sqlMetaData.column = "cnt";
					break;
				case 9:
					sqlMetaData.column = "cntb";
					break;
				case 82:
					sqlMetaData.column = "max";
					break;
				case 81:
					sqlMetaData.column = "min";
					break;
				case 77:
					sqlMetaData.column = "sum";
					break;
				case 83:
					sqlMetaData.column = "any";
					break;
				case 86:
					sqlMetaData.column = "noop";
					break;
				case 48:
					sqlMetaData.column = "stdev";
					break;
				case 49:
					sqlMetaData.column = "stdevp";
					break;
				case 50:
					sqlMetaData.column = "var";
					break;
				case 51:
					sqlMetaData.column = "varp";
					break;
				}
			}
			array[i] = i;
		}
		sqlMetaDataSet.indexMap = array;
		sqlMetaDataSet.visibleColumns = cColumns;
		metaData = sqlMetaDataSet;
		return true;
	}

	internal bool TryReadCipherInfoEntry(TdsParserStateObject stateObj, out SqlTceCipherInfoEntry entry)
	{
		byte value = 0;
		entry = new SqlTceCipherInfoEntry();
		if (!stateObj.TryReadInt32(out var value2))
		{
			return false;
		}
		if (!stateObj.TryReadInt32(out var value3))
		{
			return false;
		}
		if (!stateObj.TryReadInt32(out var value4))
		{
			return false;
		}
		byte[] array = new byte[8];
		if (!stateObj.TryReadByteArray(array, 0, 8))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out value))
		{
			return false;
		}
		for (int i = 0; i < value; i++)
		{
			if (!stateObj.TryReadUInt16(out var value5))
			{
				return false;
			}
			int num = value5;
			byte[] array2 = new byte[num];
			if (!stateObj.TryReadByteArray(array2, 0, num))
			{
				return false;
			}
			if (!stateObj.TryReadByte(out var value6))
			{
				return false;
			}
			num = value6;
			if (!stateObj.TryReadString(num, out var value7))
			{
				return false;
			}
			if (!stateObj.TryReadUInt16(out value5))
			{
				return false;
			}
			num = value5;
			if (!stateObj.TryReadString(num, out var value8))
			{
				return false;
			}
			if (!stateObj.TryReadByte(out var value9))
			{
				return false;
			}
			num = value9;
			if (!stateObj.TryReadString(num, out var value10))
			{
				return false;
			}
			entry.Add(array2, value2, value3, value4, array, value8, value7, value10);
		}
		return true;
	}

	internal bool TryProcessCipherInfoTable(TdsParserStateObject stateObj, out SqlTceCipherInfoTable cipherTable)
	{
		short value = 0;
		cipherTable = null;
		if (!stateObj.TryReadInt16(out value))
		{
			return false;
		}
		if (value != 0)
		{
			SqlTceCipherInfoTable sqlTceCipherInfoTable = new SqlTceCipherInfoTable(value);
			for (int i = 0; i < value; i++)
			{
				if (!TryReadCipherInfoEntry(stateObj, out var entry))
				{
					return false;
				}
				sqlTceCipherInfoTable[i] = entry;
			}
			cipherTable = sqlTceCipherInfoTable;
		}
		return true;
	}

	internal bool TryProcessMetaData(int cColumns, TdsParserStateObject stateObj, out _SqlMetaDataSet metaData, SqlCommandColumnEncryptionSetting columnEncryptionSetting)
	{
		SqlTceCipherInfoTable cipherTable = null;
		if (_serverSupportsColumnEncryption && !TryProcessCipherInfoTable(stateObj, out cipherTable))
		{
			metaData = null;
			return false;
		}
		_SqlMetaDataSet sqlMetaDataSet = new _SqlMetaDataSet(cColumns, cipherTable);
		for (int i = 0; i < cColumns; i++)
		{
			if (!TryCommonProcessMetaData(stateObj, sqlMetaDataSet[i], cipherTable, fColMD: true, columnEncryptionSetting))
			{
				metaData = null;
				return false;
			}
		}
		metaData = sqlMetaDataSet;
		return true;
	}

	private bool IsVarTimeTds(byte tdsType)
	{
		if (tdsType != 41 && tdsType != 42)
		{
			return tdsType == 43;
		}
		return true;
	}

	private bool TryProcessTypeInfo(TdsParserStateObject stateObj, SqlMetaDataPriv col, uint userType)
	{
		if (!stateObj.TryReadByte(out var value))
		{
			return false;
		}
		if (value == 241)
		{
			col.length = 65535;
		}
		else if (IsVarTimeTds(value))
		{
			col.length = 0;
		}
		else if (value == 40)
		{
			col.length = 3;
		}
		else if (!TryGetTokenLength(value, stateObj, out col.length))
		{
			return false;
		}
		col.metaType = MetaType.GetSqlDataType(value, userType, col.length);
		col.type = col.metaType.SqlDbType;
		if (_isShiloh)
		{
			col.tdsType = (col.IsNullable ? col.metaType.NullableType : col.metaType.TDSType);
		}
		else
		{
			col.tdsType = value;
		}
		if (_isYukon)
		{
			if (240 == value && !TryProcessUDTMetaData(col, stateObj))
			{
				return false;
			}
			if (col.length == 65535)
			{
				col.metaType = MetaType.GetMaxMetaTypeFromMetaType(col.metaType);
				col.length = int.MaxValue;
				if (value == 241)
				{
					if (!stateObj.TryReadByte(out var value2))
					{
						return false;
					}
					if (((uint)value2 & (true ? 1u : 0u)) != 0)
					{
						if (!stateObj.TryReadByte(out var value3))
						{
							return false;
						}
						if (col.xmlSchemaCollection == null)
						{
							col.xmlSchemaCollection = new SqlMetaDataXmlSchemaCollection();
						}
						if (value3 != 0 && !stateObj.TryReadString(value3, out col.xmlSchemaCollection.Database))
						{
							return false;
						}
						if (!stateObj.TryReadByte(out value3))
						{
							return false;
						}
						if (value3 != 0 && !stateObj.TryReadString(value3, out col.xmlSchemaCollection.OwningSchema))
						{
							return false;
						}
						if (!stateObj.TryReadInt16(out var value4))
						{
							return false;
						}
						if (value3 != 0 && !stateObj.TryReadString(value4, out col.xmlSchemaCollection.Name))
						{
							return false;
						}
					}
				}
			}
		}
		if (col.type == SqlDbType.Decimal)
		{
			if (!stateObj.TryReadByte(out col.precision))
			{
				return false;
			}
			if (!stateObj.TryReadByte(out col.scale))
			{
				return false;
			}
		}
		if (col.metaType.IsVarTime)
		{
			if (!stateObj.TryReadByte(out col.scale))
			{
				return false;
			}
			switch (col.metaType.SqlDbType)
			{
			case SqlDbType.Time:
				col.length = MetaType.GetTimeSizeFromScale(col.scale);
				break;
			case SqlDbType.DateTime2:
				col.length = 3 + MetaType.GetTimeSizeFromScale(col.scale);
				break;
			case SqlDbType.DateTimeOffset:
				col.length = 5 + MetaType.GetTimeSizeFromScale(col.scale);
				break;
			}
		}
		if (_isShiloh && col.metaType.IsCharType && value != 241)
		{
			if (!TryProcessCollation(stateObj, out col.collation))
			{
				return false;
			}
			if ((col.collation.info & 0x4000000) == 67108864)
			{
				col.encoding = Encoding.UTF8;
			}
			else
			{
				int codePage = GetCodePage(col.collation, stateObj);
				if (codePage == _defaultCodePage)
				{
					col.codePage = _defaultCodePage;
					col.encoding = _defaultEncoding;
				}
				else
				{
					col.codePage = codePage;
					col.encoding = Encoding.GetEncoding(col.codePage);
				}
			}
		}
		return true;
	}

	private bool TryCommonProcessMetaData(TdsParserStateObject stateObj, _SqlMetaData col, SqlTceCipherInfoTable cipherTable, bool fColMD, SqlCommandColumnEncryptionSetting columnEncryptionSetting)
	{
		uint value;
		if (IsYukonOrNewer)
		{
			if (!stateObj.TryReadUInt32(out value))
			{
				return false;
			}
		}
		else
		{
			if (!stateObj.TryReadUInt16(out var value2))
			{
				return false;
			}
			value = value2;
		}
		if (!stateObj.TryReadByte(out var value3))
		{
			return false;
		}
		col.Updatability = (byte)((value3 & 0xB) >> 2);
		col.IsNullable = 1 == (value3 & 1);
		col.IsIdentity = 16 == (value3 & 0x10);
		if (!stateObj.TryReadByte(out value3))
		{
			return false;
		}
		col.IsColumnSet = 4 == (value3 & 4);
		if (fColMD && _serverSupportsColumnEncryption)
		{
			col.isEncrypted = 8 == (value3 & 8);
		}
		if (!TryProcessTypeInfo(stateObj, col, value))
		{
			return false;
		}
		if (col.metaType.IsLong && !col.metaType.IsPlp)
		{
			if (_isYukon)
			{
				int length = 65535;
				if (!TryProcessOneTable(stateObj, ref length, out col.multiPartTableName))
				{
					return false;
				}
			}
			else
			{
				if (!stateObj.TryReadUInt16(out var value4))
				{
					return false;
				}
				if (!stateObj.TryReadString(value4, out var value5))
				{
					return false;
				}
				col.multiPartTableName = new MultiPartTableName(value5);
			}
		}
		if (fColMD && _serverSupportsColumnEncryption && col.isEncrypted && cipherTable != null && !TryProcessTceCryptoMetadata(stateObj, col, cipherTable, columnEncryptionSetting, isReturnValue: false))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out var value6))
		{
			return false;
		}
		if (!stateObj.TryReadString(value6, out col.column))
		{
			return false;
		}
		stateObj._receivedColMetaData = true;
		return true;
	}

	private bool TryProcessUDTMetaData(SqlMetaDataPriv metaData, TdsParserStateObject stateObj)
	{
		if (!stateObj.TryReadUInt16(out var value))
		{
			return false;
		}
		metaData.length = value;
		if (!stateObj.TryReadByte(out var value2))
		{
			return false;
		}
		if (metaData.udt == null)
		{
			metaData.udt = new SqlMetaDataUdt();
		}
		if (value2 != 0 && !stateObj.TryReadString(value2, out metaData.udt.DatabaseName))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out value2))
		{
			return false;
		}
		if (value2 != 0 && !stateObj.TryReadString(value2, out metaData.udt.SchemaName))
		{
			return false;
		}
		if (!stateObj.TryReadByte(out value2))
		{
			return false;
		}
		if (value2 != 0 && !stateObj.TryReadString(value2, out metaData.udt.TypeName))
		{
			return false;
		}
		if (!stateObj.TryReadUInt16(out value))
		{
			return false;
		}
		if (value != 0 && !stateObj.TryReadString(value, out metaData.udt.AssemblyQualifiedName))
		{
			return false;
		}
		return true;
	}

	private void WriteUDTMetaData(object value, string database, string schema, string type, TdsParserStateObject stateObj)
	{
		if (ADP.IsEmpty(database))
		{
			stateObj.WriteByte(0);
		}
		else
		{
			stateObj.WriteByte((byte)database.Length);
			WriteString(database, stateObj);
		}
		if (ADP.IsEmpty(schema))
		{
			stateObj.WriteByte(0);
		}
		else
		{
			stateObj.WriteByte((byte)schema.Length);
			WriteString(schema, stateObj);
		}
		if (ADP.IsEmpty(type))
		{
			stateObj.WriteByte(0);
			return;
		}
		stateObj.WriteByte((byte)type.Length);
		WriteString(type, stateObj);
	}

	internal bool TryProcessTableName(int length, TdsParserStateObject stateObj, out MultiPartTableName[] multiPartTableNames)
	{
		int num = 0;
		MultiPartTableName[] array = new MultiPartTableName[1];
		while (length > 0)
		{
			if (!TryProcessOneTable(stateObj, ref length, out var multiPartTableName))
			{
				multiPartTableNames = null;
				return false;
			}
			if (num == 0)
			{
				array[num] = multiPartTableName;
			}
			else
			{
				MultiPartTableName[] array2 = new MultiPartTableName[array.Length + 1];
				Array.Copy(array, 0, array2, 0, array.Length);
				array2[array.Length] = multiPartTableName;
				array = array2;
			}
			num++;
		}
		multiPartTableNames = array;
		return true;
	}

	private bool TryProcessOneTable(TdsParserStateObject stateObj, ref int length, out MultiPartTableName multiPartTableName)
	{
		multiPartTableName = default(MultiPartTableName);
		MultiPartTableName multiPartTableName2;
		ushort value2;
		string value3;
		if (_isShilohSP1)
		{
			multiPartTableName2 = default(MultiPartTableName);
			if (!stateObj.TryReadByte(out var value))
			{
				return false;
			}
			length--;
			if (value == 4)
			{
				if (!stateObj.TryReadUInt16(out value2))
				{
					return false;
				}
				length -= 2;
				if (!stateObj.TryReadString(value2, out value3))
				{
					return false;
				}
				multiPartTableName2.ServerName = value3;
				value--;
				length -= value2 * 2;
			}
			if (value == 3)
			{
				if (!stateObj.TryReadUInt16(out value2))
				{
					return false;
				}
				length -= 2;
				if (!stateObj.TryReadString(value2, out value3))
				{
					return false;
				}
				multiPartTableName2.CatalogName = value3;
				length -= value2 * 2;
				value--;
			}
			if (value == 2)
			{
				if (!stateObj.TryReadUInt16(out value2))
				{
					return false;
				}
				length -= 2;
				if (!stateObj.TryReadString(value2, out value3))
				{
					return false;
				}
				multiPartTableName2.SchemaName = value3;
				length -= value2 * 2;
				value--;
			}
			if (value == 1)
			{
				if (!stateObj.TryReadUInt16(out value2))
				{
					return false;
				}
				length -= 2;
				if (!stateObj.TryReadString(value2, out value3))
				{
					return false;
				}
				multiPartTableName2.TableName = value3;
				length -= value2 * 2;
				value--;
			}
		}
		else
		{
			if (!stateObj.TryReadUInt16(out value2))
			{
				return false;
			}
			length -= 2;
			if (!stateObj.TryReadString(value2, out value3))
			{
				return false;
			}
			string name = value3;
			length -= value2 * 2;
			multiPartTableName2 = new MultiPartTableName(MultipartIdentifier.ParseMultipartIdentifier(name, "[\"", "]\"", Strings.SQL_TDSParserTableName, ThrowOnEmptyMultipartName: false));
		}
		multiPartTableName = multiPartTableName2;
		return true;
	}

	private bool TryProcessColInfo(_SqlMetaDataSet columns, SqlDataReader reader, TdsParserStateObject stateObj, out _SqlMetaDataSet metaData)
	{
		metaData = null;
		for (int i = 0; i < columns.Length; i++)
		{
			_SqlMetaData sqlMetaData = columns[i];
			if (!stateObj.TryReadByte(out var _))
			{
				return false;
			}
			if (!stateObj.TryReadByte(out sqlMetaData.tableNum))
			{
				return false;
			}
			if (!stateObj.TryReadByte(out var value2))
			{
				return false;
			}
			sqlMetaData.IsDifferentName = 32 == (value2 & 0x20);
			sqlMetaData.IsExpression = 4 == (value2 & 4);
			sqlMetaData.IsKey = 8 == (value2 & 8);
			sqlMetaData.IsHidden = 16 == (value2 & 0x10);
			if (sqlMetaData.IsDifferentName)
			{
				if (!stateObj.TryReadByte(out var value3))
				{
					return false;
				}
				if (!stateObj.TryReadString(value3, out sqlMetaData.baseColumn))
				{
					return false;
				}
			}
			if (reader.TableNames != null && sqlMetaData.tableNum > 0)
			{
				sqlMetaData.multiPartTableName = reader.TableNames[sqlMetaData.tableNum - 1];
			}
			if (sqlMetaData.IsExpression)
			{
				sqlMetaData.Updatability = 0;
			}
		}
		metaData = columns;
		return true;
	}

	internal bool TryProcessColumnHeader(SqlMetaDataPriv col, TdsParserStateObject stateObj, int columnOrdinal, out bool isNull, out ulong length)
	{
		if (stateObj.IsNullCompressionBitSet(columnOrdinal))
		{
			isNull = true;
			length = 0uL;
			return true;
		}
		return TryProcessColumnHeaderNoNBC(col, stateObj, out isNull, out length);
	}

	private bool TryProcessColumnHeaderNoNBC(SqlMetaDataPriv col, TdsParserStateObject stateObj, out bool isNull, out ulong length)
	{
		if (col.metaType.IsLong && !col.metaType.IsPlp)
		{
			if (!stateObj.TryReadByte(out var value))
			{
				isNull = false;
				length = 0uL;
				return false;
			}
			if (value != 0)
			{
				if (!stateObj.TrySkipBytes(value))
				{
					isNull = false;
					length = 0uL;
					return false;
				}
				if (!stateObj.TrySkipBytes(8))
				{
					isNull = false;
					length = 0uL;
					return false;
				}
				isNull = false;
				return TryGetDataLength(col, stateObj, out length);
			}
			isNull = true;
			length = 0uL;
			return true;
		}
		if (!TryGetDataLength(col, stateObj, out var length2))
		{
			isNull = false;
			length = 0uL;
			return false;
		}
		isNull = IsNull(col.metaType, length2);
		length = (isNull ? 0 : length2);
		return true;
	}

	internal bool TryGetAltRowId(TdsParserStateObject stateObj, out int id)
	{
		if (!stateObj.TryReadByte(out var _))
		{
			id = 0;
			return false;
		}
		if (!stateObj.TryStartNewRow(isNullCompressed: false))
		{
			id = 0;
			return false;
		}
		if (!stateObj.TryReadUInt16(out var value2))
		{
			id = 0;
			return false;
		}
		id = value2;
		return true;
	}

	private bool TryProcessRow(_SqlMetaDataSet columns, object[] buffer, int[] map, TdsParserStateObject stateObj)
	{
		SqlBuffer sqlBuffer = new SqlBuffer();
		for (int i = 0; i < columns.Length; i++)
		{
			_SqlMetaData sqlMetaData = columns[i];
			if (!TryProcessColumnHeader(sqlMetaData, stateObj, i, out var isNull, out var length))
			{
				return false;
			}
			if (isNull)
			{
				GetNullSqlValue(sqlBuffer, sqlMetaData, SqlCommandColumnEncryptionSetting.Disabled, _connHandler);
				buffer[map[i]] = sqlBuffer.SqlValue;
			}
			else
			{
				if (!TryReadSqlValue(sqlBuffer, sqlMetaData, (int)(sqlMetaData.metaType.IsPlp ? int.MaxValue : length), stateObj, SqlCommandColumnEncryptionSetting.Disabled, sqlMetaData.column))
				{
					return false;
				}
				buffer[map[i]] = sqlBuffer.SqlValue;
				if (stateObj._longlen != 0L)
				{
					throw new SqlTruncateException(StringsHelper.GetString(Strings.SqlMisc_TruncationMaxDataMessage));
				}
			}
			sqlBuffer.Clear();
		}
		return true;
	}

	internal static bool ShouldHonorTceForRead(SqlCommandColumnEncryptionSetting columnEncryptionSetting, SqlInternalConnectionTds connection)
	{
		switch (columnEncryptionSetting)
		{
		case SqlCommandColumnEncryptionSetting.Disabled:
			return false;
		case SqlCommandColumnEncryptionSetting.Enabled:
			return true;
		case SqlCommandColumnEncryptionSetting.ResultSetOnly:
			return true;
		default:
			if (connection != null && connection.ConnectionOptions != null)
			{
				return connection.ConnectionOptions.ColumnEncryptionSetting == SqlConnectionColumnEncryptionSetting.Enabled;
			}
			return false;
		}
	}

	internal static object GetNullSqlValue(SqlBuffer nullVal, SqlMetaDataPriv md, SqlCommandColumnEncryptionSetting columnEncryptionSetting, SqlInternalConnectionTds connection)
	{
		SqlDbType type = md.type;
		if (type == SqlDbType.VarBinary && md.isEncrypted && ShouldHonorTceForRead(columnEncryptionSetting, connection))
		{
			type = md.baseTI.type;
		}
		switch (type)
		{
		case SqlDbType.Real:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Single);
			break;
		case SqlDbType.Float:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Double);
			break;
		case SqlDbType.Binary:
		case SqlDbType.Image:
		case SqlDbType.VarBinary:
		case SqlDbType.Udt:
			nullVal.SqlBinary = SqlBinary.Null;
			break;
		case SqlDbType.UniqueIdentifier:
			nullVal.SqlGuid = SqlGuid.Null;
			break;
		case SqlDbType.Bit:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Boolean);
			break;
		case SqlDbType.TinyInt:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Byte);
			break;
		case SqlDbType.SmallInt:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Int16);
			break;
		case SqlDbType.Int:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Int32);
			break;
		case SqlDbType.BigInt:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Int64);
			break;
		case SqlDbType.Char:
		case SqlDbType.NChar:
		case SqlDbType.NText:
		case SqlDbType.NVarChar:
		case SqlDbType.Text:
		case SqlDbType.VarChar:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.String);
			break;
		case SqlDbType.Decimal:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Decimal);
			break;
		case SqlDbType.DateTime:
		case SqlDbType.SmallDateTime:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.DateTime);
			break;
		case SqlDbType.Money:
		case SqlDbType.SmallMoney:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Money);
			break;
		case SqlDbType.Variant:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Empty);
			break;
		case SqlDbType.Xml:
			nullVal.SqlCachedBuffer = SqlCachedBuffer.Null;
			break;
		case SqlDbType.Date:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Date);
			break;
		case SqlDbType.Time:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.Time);
			break;
		case SqlDbType.DateTime2:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.DateTime2);
			break;
		case SqlDbType.DateTimeOffset:
			nullVal.SetToNullOfType(SqlBuffer.StorageType.DateTimeOffset);
			break;
		}
		return nullVal;
	}

	internal bool TrySkipRow(_SqlMetaDataSet columns, TdsParserStateObject stateObj)
	{
		return TrySkipRow(columns, 0, stateObj);
	}

	internal bool TrySkipRow(_SqlMetaDataSet columns, int startCol, TdsParserStateObject stateObj)
	{
		for (int i = startCol; i < columns.Length; i++)
		{
			_SqlMetaData md = columns[i];
			if (!TrySkipValue(md, i, stateObj))
			{
				return false;
			}
		}
		return true;
	}

	internal bool TrySkipValue(SqlMetaDataPriv md, int columnOrdinal, TdsParserStateObject stateObj)
	{
		if (stateObj.IsNullCompressionBitSet(columnOrdinal))
		{
			return true;
		}
		if (md.metaType.IsPlp)
		{
			if (!TrySkipPlpValue(ulong.MaxValue, stateObj, out var _))
			{
				return false;
			}
		}
		else if (md.metaType.IsLong)
		{
			if (!stateObj.TryReadByte(out var value))
			{
				return false;
			}
			if (value != 0)
			{
				if (!stateObj.TrySkipBytes(value + 8))
				{
					return false;
				}
				if (!TryGetTokenLength(md.tdsType, stateObj, out var tokenLength))
				{
					return false;
				}
				if (!stateObj.TrySkipBytes(tokenLength))
				{
					return false;
				}
			}
		}
		else
		{
			if (!TryGetTokenLength(md.tdsType, stateObj, out var tokenLength2))
			{
				return false;
			}
			if (!IsNull(md.metaType, (ulong)tokenLength2) && !stateObj.TrySkipBytes(tokenLength2))
			{
				return false;
			}
		}
		return true;
	}

	private bool IsNull(MetaType mt, ulong length)
	{
		if (mt.IsPlp)
		{
			return ulong.MaxValue == length;
		}
		if (65535 == length && !mt.IsLong)
		{
			return true;
		}
		if (length == 0L && !mt.IsCharType)
		{
			return !mt.IsBinType;
		}
		return false;
	}

	private bool TryReadSqlStringValue(SqlBuffer value, byte type, int length, Encoding encoding, bool isPlp, TdsParserStateObject stateObj)
	{
		switch (type)
		{
		case 35:
		case 39:
		case 47:
		case 167:
		case 175:
		{
			if (encoding == null)
			{
				encoding = _defaultEncoding;
			}
			if (!stateObj.TryReadStringWithEncoding(length, encoding, isPlp, out var value3))
			{
				return false;
			}
			value.SetToString(value3);
			break;
		}
		case 99:
		case 231:
		case 239:
		{
			string value2 = null;
			if (isPlp)
			{
				char[] buff = null;
				if (!TryReadPlpUnicodeChars(ref buff, 0, length >> 1, stateObj, out length))
				{
					return false;
				}
				value2 = ((length <= 0) ? ADP.StrEmpty : new string(buff, 0, length));
			}
			else if (!stateObj.TryReadString(length >> 1, out value2))
			{
				return false;
			}
			value.SetToString(value2);
			break;
		}
		}
		return true;
	}

	internal bool DeserializeUnencryptedValue(SqlBuffer value, byte[] unencryptedBytes, SqlMetaDataPriv md, TdsParserStateObject stateObj, byte normalizationVersion)
	{
		if (normalizationVersion != 1)
		{
			throw SQL.UnsupportedNormalizationVersion(normalizationVersion);
		}
		byte tdsType = md.baseTI.tdsType;
		int num = unencryptedBytes.Length;
		int length = md.baseTI.length;
		byte scale = md.baseTI.scale;
		switch (tdsType)
		{
		case 38:
		case 48:
		case 50:
		case 52:
		case 56:
		case 104:
		case 127:
		{
			if (unencryptedBytes.Length != 8)
			{
				return false;
			}
			long num2 = BitConverter.ToInt64(unencryptedBytes, 0);
			if (tdsType == 50 || tdsType == 104)
			{
				value.Boolean = num2 != 0;
			}
			else if (tdsType == 48 || length == 1)
			{
				value.Byte = (byte)num2;
			}
			else if (tdsType == 52 || length == 2)
			{
				value.Int16 = (short)num2;
			}
			else if (tdsType == 56 || length == 4)
			{
				value.Int32 = (int)num2;
			}
			else
			{
				value.Int64 = num2;
			}
			break;
		}
		case 109:
			if (num == 4)
			{
				goto case 59;
			}
			goto case 62;
		case 59:
		{
			if (unencryptedBytes.Length != 4)
			{
				return false;
			}
			float single = BitConverter.ToSingle(unencryptedBytes, 0);
			value.Single = single;
			break;
		}
		case 62:
		{
			if (unencryptedBytes.Length != 8)
			{
				return false;
			}
			double @double = BitConverter.ToDouble(unencryptedBytes, 0);
			value.Double = @double;
			break;
		}
		case 60:
		case 110:
		case 122:
		{
			if (unencryptedBytes.Length != 8)
			{
				return false;
			}
			int num5 = BitConverter.ToInt32(unencryptedBytes, 0);
			uint num6 = BitConverter.ToUInt32(unencryptedBytes, 4);
			long toMoney = ((long)num5 << 32) + num6;
			value.SetToMoney(toMoney);
			break;
		}
		case 111:
			if (num == 4)
			{
				goto case 58;
			}
			goto case 61;
		case 58:
		{
			if (unencryptedBytes.Length != 4)
			{
				return false;
			}
			ushort daypart2 = (ushort)((unencryptedBytes[1] << 8) + unencryptedBytes[0]);
			ushort num7 = (ushort)((unencryptedBytes[3] << 8) + unencryptedBytes[2]);
			value.SetToDateTime(daypart2, num7 * SqlDateTime.SQLTicksPerMinute);
			break;
		}
		case 61:
		{
			if (unencryptedBytes.Length != 8)
			{
				return false;
			}
			int daypart = BitConverter.ToInt32(unencryptedBytes, 0);
			uint timepart = BitConverter.ToUInt32(unencryptedBytes, 4);
			value.SetToDateTime(daypart, (int)timepart);
			break;
		}
		case 36:
			value.SqlGuid = SqlTypeWorkarounds.SqlGuidCtor(unencryptedBytes, ignored: true);
			break;
		case 34:
		case 37:
		case 45:
		case 165:
		case 173:
			if (tdsType == 45 || tdsType == 173)
			{
				byte[] array2 = new byte[md.baseTI.length];
				Buffer.BlockCopy(unencryptedBytes, 0, array2, 0, unencryptedBytes.Length);
				unencryptedBytes = array2;
			}
			value.SqlBinary = SqlTypeWorkarounds.SqlBinaryCtor(unencryptedBytes, ignored: true);
			break;
		case 106:
		case 108:
		{
			int num3 = 0;
			byte b = unencryptedBytes[num3++];
			bool positive = 1 == b;
			num = checked(num - 1);
			int[] array = new int[4];
			int num4 = num >> 2;
			for (int i = 0; i < num4; i++)
			{
				array[i] = BitConverter.ToInt32(unencryptedBytes, num3);
				num3 += 4;
			}
			value.SetToDecimal(md.baseTI.precision, md.baseTI.scale, positive, array);
			break;
		}
		case 35:
		case 39:
		case 47:
		case 167:
		case 175:
		{
			Encoding encoding = md.baseTI.encoding;
			if (encoding == null)
			{
				encoding = _defaultEncoding;
			}
			if (encoding == null)
			{
				ThrowUnsupportedCollationEncountered(stateObj);
			}
			string text2 = encoding.GetString(unencryptedBytes, 0, num);
			if (tdsType == 47 || tdsType == 175)
			{
				text2 = text2.PadRight(md.baseTI.length);
			}
			value.SetToString(text2);
			break;
		}
		case 99:
		case 231:
		case 239:
		{
			string text = Encoding.Unicode.GetString(unencryptedBytes, 0, num);
			if (tdsType == 239)
			{
				text = text.PadRight(md.baseTI.length / 2);
			}
			value.SetToString(text);
			break;
		}
		case 40:
			value.SetToDate(unencryptedBytes);
			break;
		case 41:
			value.SetToTime(unencryptedBytes, num, 7, scale);
			break;
		case 42:
			value.SetToDateTime2(unencryptedBytes, num, 7, scale);
			break;
		case 43:
			value.SetToDateTimeOffset(unencryptedBytes, num, 7, scale);
			break;
		default:
		{
			MetaType metaType = md.baseTI.metaType;
			if (metaType == null)
			{
				metaType = MetaType.GetSqlDataType(tdsType, 0u, num);
			}
			throw SQL.UnsupportedDatatypeEncryption(metaType.TypeName);
		}
		}
		return true;
	}

	internal bool TryReadSqlValue(SqlBuffer value, SqlMetaDataPriv md, int length, TdsParserStateObject stateObj, SqlCommandColumnEncryptionSetting columnEncryptionOverride, string columnName, SqlCommand command = null)
	{
		bool isPlp = md.metaType.IsPlp;
		byte tdsType = md.tdsType;
		if (isPlp)
		{
			length = int.MaxValue;
		}
		switch (tdsType)
		{
		case 106:
		case 108:
			if (!TryReadSqlDecimal(value, length, md.precision, md.scale, stateObj))
			{
				return false;
			}
			break;
		case 34:
		case 37:
		case 45:
		case 165:
		case 173:
		case 240:
		{
			byte[] buff = null;
			if (isPlp)
			{
				if (!stateObj.TryReadPlpBytes(ref buff, 0, length, out var _))
				{
					return false;
				}
			}
			else
			{
				buff = new byte[length];
				if (!stateObj.TryReadByteArray(buff, 0, length))
				{
					return false;
				}
			}
			if (md.isEncrypted && (columnEncryptionOverride == SqlCommandColumnEncryptionSetting.Enabled || columnEncryptionOverride == SqlCommandColumnEncryptionSetting.ResultSetOnly || (columnEncryptionOverride == SqlCommandColumnEncryptionSetting.UseConnectionSetting && _connHandler != null && _connHandler.ConnectionOptions != null && _connHandler.ConnectionOptions.ColumnEncryptionSetting == SqlConnectionColumnEncryptionSetting.Enabled)))
			{
				try
				{
					byte[] array = SqlSecurityUtility.DecryptWithKey(buff, md.cipherMD, _connHandler.Connection, command);
					if (array != null)
					{
						DeserializeUnencryptedValue(value, array, md, stateObj, md.NormalizationRuleVersion);
					}
				}
				catch (Exception e)
				{
					throw SQL.ColumnDecryptionFailed(columnName, null, e);
				}
			}
			else
			{
				value.SqlBinary = SqlTypeWorkarounds.SqlBinaryCtor(buff, ignored: true);
			}
			break;
		}
		case 35:
		case 39:
		case 47:
		case 99:
		case 167:
		case 175:
		case 231:
		case 239:
			if (!TryReadSqlStringValue(value, tdsType, length, md.encoding, isPlp, stateObj))
			{
				return false;
			}
			break;
		case 241:
		{
			if (!SqlCachedBuffer.TryCreate(md, this, stateObj, out var buffer))
			{
				return false;
			}
			value.SqlCachedBuffer = buffer;
			break;
		}
		case 40:
		case 41:
		case 42:
		case 43:
			if (!TryReadSqlDateTime(value, tdsType, length, md.scale, stateObj))
			{
				return false;
			}
			break;
		default:
			if (!TryReadSqlValueInternal(value, tdsType, length, stateObj))
			{
				return false;
			}
			break;
		}
		return true;
	}

	private bool TryReadSqlDateTime(SqlBuffer value, byte tdsType, int length, byte scale, TdsParserStateObject stateObj)
	{
		byte[] array = new byte[length];
		if (!stateObj.TryReadByteArray(array, 0, length))
		{
			return false;
		}
		switch (tdsType)
		{
		case 40:
			value.SetToDate(array);
			break;
		case 41:
			value.SetToTime(array, length, scale, scale);
			break;
		case 42:
			value.SetToDateTime2(array, length, scale, scale);
			break;
		case 43:
			value.SetToDateTimeOffset(array, length, scale, scale);
			break;
		}
		return true;
	}

	internal bool TryReadSqlValueInternal(SqlBuffer value, byte tdsType, int length, TdsParserStateObject stateObj)
	{
		byte value2;
		int value5;
		switch (tdsType)
		{
		case 50:
		case 104:
			if (!stateObj.TryReadByte(out value2))
			{
				return false;
			}
			value.Boolean = value2 != 0;
			break;
		case 38:
			if (length == 1)
			{
				goto case 48;
			}
			if (length == 2)
			{
				goto case 52;
			}
			if (length == 4)
			{
				goto case 56;
			}
			goto case 127;
		case 48:
			if (!stateObj.TryReadByte(out value2))
			{
				return false;
			}
			value.Byte = value2;
			break;
		case 52:
		{
			if (!stateObj.TryReadInt16(out var value9))
			{
				return false;
			}
			value.Int16 = value9;
			break;
		}
		case 56:
			if (!stateObj.TryReadInt32(out value5))
			{
				return false;
			}
			value.Int32 = value5;
			break;
		case 127:
		{
			if (!stateObj.TryReadInt64(out var value12))
			{
				return false;
			}
			value.Int64 = value12;
			break;
		}
		case 109:
			if (length == 4)
			{
				goto case 59;
			}
			goto case 62;
		case 59:
		{
			if (!stateObj.TryReadSingle(out var value13))
			{
				return false;
			}
			value.Single = value13;
			break;
		}
		case 62:
		{
			if (!stateObj.TryReadDouble(out var value6))
			{
				return false;
			}
			value.Double = value6;
			break;
		}
		case 110:
			if (length != 4)
			{
				goto case 60;
			}
			goto case 122;
		case 60:
		{
			if (!stateObj.TryReadInt32(out var value3))
			{
				return false;
			}
			if (!stateObj.TryReadUInt32(out var value4))
			{
				return false;
			}
			long toMoney = ((long)value3 << 32) + value4;
			value.SetToMoney(toMoney);
			break;
		}
		case 122:
			if (!stateObj.TryReadInt32(out value5))
			{
				return false;
			}
			value.SetToMoney(value5);
			break;
		case 111:
			if (length == 4)
			{
				goto case 58;
			}
			goto case 61;
		case 58:
		{
			if (!stateObj.TryReadUInt16(out var value7))
			{
				return false;
			}
			if (!stateObj.TryReadUInt16(out var value8))
			{
				return false;
			}
			value.SetToDateTime(value7, value8 * SqlDateTime.SQLTicksPerMinute);
			break;
		}
		case 61:
		{
			if (!stateObj.TryReadInt32(out var value10))
			{
				return false;
			}
			if (!stateObj.TryReadUInt32(out var value11))
			{
				return false;
			}
			value.SetToDateTime(value10, (int)value11);
			break;
		}
		case 36:
		{
			byte[] array2 = new byte[length];
			if (!stateObj.TryReadByteArray(array2, 0, length))
			{
				return false;
			}
			value.SqlGuid = SqlTypeWorkarounds.SqlGuidCtor(array2, ignored: true);
			break;
		}
		case 34:
		case 37:
		case 45:
		case 165:
		case 173:
		{
			byte[] array = new byte[length];
			if (!stateObj.TryReadByteArray(array, 0, length))
			{
				return false;
			}
			value.SqlBinary = SqlTypeWorkarounds.SqlBinaryCtor(array, ignored: true);
			break;
		}
		case 98:
			if (!TryReadSqlVariant(value, length, stateObj))
			{
				return false;
			}
			break;
		}
		return true;
	}

	internal bool TryReadSqlVariant(SqlBuffer value, int lenTotal, TdsParserStateObject stateObj)
	{
		if (!stateObj.TryReadByte(out var value2))
		{
			return false;
		}
		ushort value3 = 0;
		if (!stateObj.TryReadByte(out var value4))
		{
			return false;
		}
		MetaType sqlDataType = MetaType.GetSqlDataType(value2, 0u, 0);
		byte propBytes = sqlDataType.PropBytes;
		int num = 2 + value4;
		int length = lenTotal - num;
		switch (value2)
		{
		case 36:
		case 48:
		case 50:
		case 52:
		case 56:
		case 58:
		case 59:
		case 60:
		case 61:
		case 62:
		case 122:
		case 127:
			if (!TryReadSqlValueInternal(value, value2, length, stateObj))
			{
				return false;
			}
			break;
		case 106:
		case 108:
		{
			if (!stateObj.TryReadByte(out var value6))
			{
				return false;
			}
			if (!stateObj.TryReadByte(out var value7))
			{
				return false;
			}
			if (value4 > propBytes && !stateObj.TrySkipBytes(value4 - propBytes))
			{
				return false;
			}
			if (!TryReadSqlDecimal(value, 17, value6, value7, stateObj))
			{
				return false;
			}
			break;
		}
		case 165:
		case 173:
			if (!stateObj.TryReadUInt16(out value3))
			{
				return false;
			}
			if (value4 > propBytes && !stateObj.TrySkipBytes(value4 - propBytes))
			{
				return false;
			}
			goto case 36;
		case 167:
		case 175:
		case 231:
		case 239:
		{
			if (!TryProcessCollation(stateObj, out var collation))
			{
				return false;
			}
			if (!stateObj.TryReadUInt16(out value3))
			{
				return false;
			}
			if (value4 > propBytes && !stateObj.TrySkipBytes(value4 - propBytes))
			{
				return false;
			}
			Encoding encoding = Encoding.GetEncoding(GetCodePage(collation, stateObj));
			if (!TryReadSqlStringValue(value, value2, length, encoding, isPlp: false, stateObj))
			{
				return false;
			}
			break;
		}
		case 40:
			if (!TryReadSqlDateTime(value, value2, length, 0, stateObj))
			{
				return false;
			}
			break;
		case 41:
		case 42:
		case 43:
		{
			if (!stateObj.TryReadByte(out var value5))
			{
				return false;
			}
			if (value4 > propBytes && !stateObj.TrySkipBytes(value4 - propBytes))
			{
				return false;
			}
			if (!TryReadSqlDateTime(value, value2, length, value5, stateObj))
			{
				return false;
			}
			break;
		}
		}
		return true;
	}

	internal Task WriteSqlVariantValue(object value, int length, int offset, TdsParserStateObject stateObj, bool canAccumulate = true)
	{
		if (ADP.IsNull(value))
		{
			WriteInt(0, stateObj);
			WriteInt(0, stateObj);
			return null;
		}
		MetaType metaTypeFromValue = MetaType.GetMetaTypeFromValue(value);
		if (108 == metaTypeFromValue.TDSType && 8 == length)
		{
			metaTypeFromValue = MetaType.GetMetaTypeFromValue(new SqlMoney((decimal)value));
		}
		if (metaTypeFromValue.IsAnsiType)
		{
			length = GetEncodingCharLength((string)value, length, 0, _defaultEncoding);
		}
		WriteInt(2 + metaTypeFromValue.PropBytes + length, stateObj);
		WriteInt(2 + metaTypeFromValue.PropBytes + length, stateObj);
		stateObj.WriteByte(metaTypeFromValue.TDSType);
		stateObj.WriteByte(metaTypeFromValue.PropBytes);
		switch (metaTypeFromValue.TDSType)
		{
		case 59:
			WriteFloat((float)value, stateObj);
			break;
		case 62:
			WriteDouble((double)value, stateObj);
			break;
		case 127:
			WriteLong((long)value, stateObj);
			break;
		case 56:
			WriteInt((int)value, stateObj);
			break;
		case 52:
			WriteShort((short)value, stateObj);
			break;
		case 48:
			stateObj.WriteByte((byte)value);
			break;
		case 50:
			if ((bool)value)
			{
				stateObj.WriteByte(1);
			}
			else
			{
				stateObj.WriteByte(0);
			}
			break;
		case 165:
		{
			byte[] b2 = (byte[])value;
			WriteShort(length, stateObj);
			return stateObj.WriteByteArray(b2, length, offset, canAccumulate);
		}
		case 167:
		{
			string s2 = (string)value;
			WriteUnsignedInt(_defaultCollation.info, stateObj);
			stateObj.WriteByte(_defaultCollation.sortId);
			WriteShort(length, stateObj);
			return WriteEncodingChar(s2, _defaultEncoding, stateObj, canAccumulate);
		}
		case 36:
		{
			byte[] b = ((Guid)value).ToByteArray();
			stateObj.WriteByteArray(b, length, 0);
			break;
		}
		case 231:
		{
			string s = (string)value;
			WriteUnsignedInt(_defaultCollation.info, stateObj);
			stateObj.WriteByte(_defaultCollation.sortId);
			WriteShort(length, stateObj);
			length >>= 1;
			return WriteString(s, length, offset, stateObj, canAccumulate);
		}
		case 61:
		{
			TdsDateTime tdsDateTime = MetaType.FromDateTime((DateTime)value, 8);
			WriteInt(tdsDateTime.days, stateObj);
			WriteInt(tdsDateTime.time, stateObj);
			break;
		}
		case 60:
			WriteCurrency((decimal)value, 8, stateObj);
			break;
		case 108:
			stateObj.WriteByte(metaTypeFromValue.Precision);
			stateObj.WriteByte((byte)((decimal.GetBits((decimal)value)[3] & 0xFF0000) >> 16));
			WriteDecimal((decimal)value, stateObj);
			break;
		case 41:
			stateObj.WriteByte(metaTypeFromValue.Scale);
			WriteTime((TimeSpan)value, metaTypeFromValue.Scale, length, stateObj);
			break;
		case 43:
			stateObj.WriteByte(metaTypeFromValue.Scale);
			WriteDateTimeOffset((DateTimeOffset)value, metaTypeFromValue.Scale, length, stateObj);
			break;
		}
		return null;
	}

	internal Task WriteSqlVariantDataRowValue(object value, TdsParserStateObject stateObj, bool canAccumulate = true)
	{
		if (value == null || DBNull.Value == value)
		{
			WriteInt(0, stateObj);
			return null;
		}
		MetaType metaTypeFromValue = MetaType.GetMetaTypeFromValue(value);
		int numChars = 0;
		if (metaTypeFromValue.IsAnsiType)
		{
			numChars = GetEncodingCharLength((string)value, numChars, 0, _defaultEncoding);
		}
		switch (metaTypeFromValue.TDSType)
		{
		case 59:
			WriteSqlVariantHeader(6, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteFloat((float)value, stateObj);
			break;
		case 62:
			WriteSqlVariantHeader(10, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteDouble((double)value, stateObj);
			break;
		case 127:
			WriteSqlVariantHeader(10, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteLong((long)value, stateObj);
			break;
		case 56:
			WriteSqlVariantHeader(6, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteInt((int)value, stateObj);
			break;
		case 52:
			WriteSqlVariantHeader(4, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteShort((short)value, stateObj);
			break;
		case 48:
			WriteSqlVariantHeader(3, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			stateObj.WriteByte((byte)value);
			break;
		case 50:
			WriteSqlVariantHeader(3, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			if ((bool)value)
			{
				stateObj.WriteByte(1);
			}
			else
			{
				stateObj.WriteByte(0);
			}
			break;
		case 165:
		{
			byte[] array2 = (byte[])value;
			numChars = array2.Length;
			WriteSqlVariantHeader(4 + numChars, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteShort(numChars, stateObj);
			return stateObj.WriteByteArray(array2, numChars, 0, canAccumulate);
		}
		case 167:
		{
			string text2 = (string)value;
			numChars = text2.Length;
			WriteSqlVariantHeader(9 + numChars, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteUnsignedInt(_defaultCollation.info, stateObj);
			stateObj.WriteByte(_defaultCollation.sortId);
			WriteShort(numChars, stateObj);
			return WriteEncodingChar(text2, _defaultEncoding, stateObj, canAccumulate);
		}
		case 36:
		{
			byte[] array = ((Guid)value).ToByteArray();
			numChars = array.Length;
			WriteSqlVariantHeader(18, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			stateObj.WriteByteArray(array, numChars, 0);
			break;
		}
		case 231:
		{
			string text = (string)value;
			numChars = text.Length * 2;
			WriteSqlVariantHeader(9 + numChars, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteUnsignedInt(_defaultCollation.info, stateObj);
			stateObj.WriteByte(_defaultCollation.sortId);
			WriteShort(numChars, stateObj);
			numChars >>= 1;
			return WriteString(text, numChars, 0, stateObj, canAccumulate);
		}
		case 61:
		{
			TdsDateTime tdsDateTime = MetaType.FromDateTime((DateTime)value, 8);
			WriteSqlVariantHeader(10, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteInt(tdsDateTime.days, stateObj);
			WriteInt(tdsDateTime.time, stateObj);
			break;
		}
		case 60:
			WriteSqlVariantHeader(10, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			WriteCurrency((decimal)value, 8, stateObj);
			break;
		case 108:
			WriteSqlVariantHeader(21, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			stateObj.WriteByte(metaTypeFromValue.Precision);
			stateObj.WriteByte((byte)((decimal.GetBits((decimal)value)[3] & 0xFF0000) >> 16));
			WriteDecimal((decimal)value, stateObj);
			break;
		case 41:
			WriteSqlVariantHeader(8, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			stateObj.WriteByte(metaTypeFromValue.Scale);
			WriteTime((TimeSpan)value, metaTypeFromValue.Scale, 5, stateObj);
			break;
		case 43:
			WriteSqlVariantHeader(13, metaTypeFromValue.TDSType, metaTypeFromValue.PropBytes, stateObj);
			stateObj.WriteByte(metaTypeFromValue.Scale);
			WriteDateTimeOffset((DateTimeOffset)value, metaTypeFromValue.Scale, 10, stateObj);
			break;
		}
		return null;
	}

	internal void WriteSqlVariantHeader(int length, byte tdstype, byte propbytes, TdsParserStateObject stateObj)
	{
		WriteInt(length, stateObj);
		stateObj.WriteByte(tdstype);
		stateObj.WriteByte(propbytes);
	}

	internal void WriteSqlVariantDateTime2(DateTime value, TdsParserStateObject stateObj)
	{
		SmiMetaData defaultDateTime = SmiMetaData.DefaultDateTime2;
		WriteSqlVariantHeader((int)(defaultDateTime.MaxLength + 3), 42, 1, stateObj);
		stateObj.WriteByte(defaultDateTime.Scale);
		WriteDateTime2(value, defaultDateTime.Scale, (int)defaultDateTime.MaxLength, stateObj);
	}

	internal void WriteSqlVariantDate(DateTime value, TdsParserStateObject stateObj)
	{
		SmiMetaData defaultDate = SmiMetaData.DefaultDate;
		WriteSqlVariantHeader((int)(defaultDate.MaxLength + 2), 40, 0, stateObj);
		WriteDate(value, stateObj);
	}

	private byte[] SerializeSqlMoney(SqlMoney value, int length, TdsParserStateObject stateObj)
	{
		return SerializeCurrency(value.Value, length, stateObj);
	}

	private void WriteSqlMoney(SqlMoney value, int length, TdsParserStateObject stateObj)
	{
		int[] bits = decimal.GetBits(value.Value);
		bool flag = (bits[3] & int.MinValue) != 0;
		long num = (long)(((ulong)(uint)bits[1] << 32) | (uint)bits[0]);
		if (flag)
		{
			num = -num;
		}
		if (length == 4)
		{
			decimal value2 = value.Value;
			if (value2 < TdsEnums.SQL_SMALL_MONEY_MIN || value2 > TdsEnums.SQL_SMALL_MONEY_MAX)
			{
				throw SQL.MoneyOverflow(value2.ToString(CultureInfo.InvariantCulture));
			}
			WriteInt((int)num, stateObj);
		}
		else
		{
			WriteInt((int)(num >> 32), stateObj);
			WriteInt((int)num, stateObj);
		}
	}

	private byte[] SerializeCurrency(decimal value, int length, TdsParserStateObject stateObj)
	{
		int[] bits = decimal.GetBits(new SqlMoney(value).Value);
		bool flag = (bits[3] & int.MinValue) != 0;
		long num = (long)(((ulong)(uint)bits[1] << 32) | (uint)bits[0]);
		if (flag)
		{
			num = -num;
		}
		if (length == 4)
		{
			if (value < TdsEnums.SQL_SMALL_MONEY_MIN || value > TdsEnums.SQL_SMALL_MONEY_MAX)
			{
				throw SQL.MoneyOverflow(value.ToString(CultureInfo.InvariantCulture));
			}
			length = 8;
		}
		if (stateObj._bLongBytes == null)
		{
			stateObj._bLongBytes = new byte[8];
		}
		byte[] bLongBytes = stateObj._bLongBytes;
		int num2 = 0;
		byte[] src = SerializeInt((int)(num >> 32), stateObj);
		Buffer.BlockCopy(src, 0, bLongBytes, num2, 4);
		num2 += 4;
		src = SerializeInt((int)num, stateObj);
		Buffer.BlockCopy(src, 0, bLongBytes, num2, 4);
		return bLongBytes;
	}

	private void WriteCurrency(decimal value, int length, TdsParserStateObject stateObj)
	{
		int[] bits = decimal.GetBits(new SqlMoney(value).Value);
		bool flag = (bits[3] & int.MinValue) != 0;
		long num = (long)(((ulong)(uint)bits[1] << 32) | (uint)bits[0]);
		if (flag)
		{
			num = -num;
		}
		if (length == 4)
		{
			if (value < TdsEnums.SQL_SMALL_MONEY_MIN || value > TdsEnums.SQL_SMALL_MONEY_MAX)
			{
				throw SQL.MoneyOverflow(value.ToString(CultureInfo.InvariantCulture));
			}
			WriteInt((int)num, stateObj);
		}
		else
		{
			WriteInt((int)(num >> 32), stateObj);
			WriteInt((int)num, stateObj);
		}
	}

	private byte[] SerializeDate(DateTime value)
	{
		long v = value.Subtract(DateTime.MinValue).Days;
		return SerializePartialLong(v, 3);
	}

	private void WriteDate(DateTime value, TdsParserStateObject stateObj)
	{
		long v = value.Subtract(DateTime.MinValue).Days;
		WritePartialLong(v, 3, stateObj);
	}

	private byte[] SerializeTime(TimeSpan value, byte scale, int length)
	{
		if (0 > value.Ticks || value.Ticks >= 864000000000L)
		{
			throw SQL.TimeOverflow(value.ToString());
		}
		long num = value.Ticks / TdsEnums.TICKS_FROM_SCALE[scale];
		num *= TdsEnums.TICKS_FROM_SCALE[scale];
		length = 5;
		return SerializePartialLong(num, length);
	}

	private void WriteTime(TimeSpan value, byte scale, int length, TdsParserStateObject stateObj)
	{
		if (0 > value.Ticks || value.Ticks >= 864000000000L)
		{
			throw SQL.TimeOverflow(value.ToString());
		}
		long v = value.Ticks / TdsEnums.TICKS_FROM_SCALE[scale];
		WritePartialLong(v, length, stateObj);
	}

	private byte[] SerializeDateTime2(DateTime value, byte scale, int length)
	{
		long num = value.TimeOfDay.Ticks / TdsEnums.TICKS_FROM_SCALE[scale];
		num *= TdsEnums.TICKS_FROM_SCALE[scale];
		length = 8;
		byte[] array = new byte[length];
		int num2 = 0;
		byte[] src = SerializePartialLong(num, length - 3);
		Buffer.BlockCopy(src, 0, array, num2, length - 3);
		num2 += length - 3;
		src = SerializeDate(value);
		Buffer.BlockCopy(src, 0, array, num2, 3);
		return array;
	}

	private void WriteDateTime2(DateTime value, byte scale, int length, TdsParserStateObject stateObj)
	{
		long v = value.TimeOfDay.Ticks / TdsEnums.TICKS_FROM_SCALE[scale];
		WritePartialLong(v, length - 3, stateObj);
		WriteDate(value, stateObj);
	}

	private byte[] SerializeDateTimeOffset(DateTimeOffset value, byte scale, int length)
	{
		int num = 0;
		byte[] array = SerializeDateTime2(value.UtcDateTime, scale, length - 2);
		length = array.Length + 2;
		byte[] array2 = new byte[length];
		Buffer.BlockCopy(array, 0, array2, num, length - 2);
		num += length - 2;
		short num2 = (short)value.Offset.TotalMinutes;
		array2[num++] = (byte)((uint)num2 & 0xFFu);
		array2[num++] = (byte)((uint)(num2 >> 8) & 0xFFu);
		return array2;
	}

	private void WriteDateTimeOffset(DateTimeOffset value, byte scale, int length, TdsParserStateObject stateObj)
	{
		WriteDateTime2(value.UtcDateTime, scale, length - 2, stateObj);
		short num = (short)value.Offset.TotalMinutes;
		stateObj.WriteByte((byte)((uint)num & 0xFFu));
		stateObj.WriteByte((byte)((uint)(num >> 8) & 0xFFu));
	}

	private bool TryReadSqlDecimal(SqlBuffer value, int length, byte precision, byte scale, TdsParserStateObject stateObj)
	{
		if (!stateObj.TryReadByte(out var value2))
		{
			return false;
		}
		bool positive = 1 == value2;
		length = checked(length - 1);
		if (!TryReadDecimalBits(length, stateObj, out var bits))
		{
			return false;
		}
		value.SetToDecimal(precision, scale, positive, bits);
		return true;
	}

	private bool TryReadDecimalBits(int length, TdsParserStateObject stateObj, out int[] bits)
	{
		bits = stateObj._decimalBits;
		if (bits == null)
		{
			bits = new int[4];
		}
		else
		{
			for (int i = 0; i < bits.Length; i++)
			{
				bits[i] = 0;
			}
		}
		int num = length >> 2;
		for (int i = 0; i < num; i++)
		{
			if (!stateObj.TryReadInt32(out bits[i]))
			{
				return false;
			}
		}
		return true;
	}

	internal static SqlDecimal AdjustSqlDecimalScale(SqlDecimal d, int newScale)
	{
		if (d.Scale != newScale)
		{
			bool fRound = !EnableTruncateSwitch;
			return SqlDecimal.AdjustScale(d, newScale - d.Scale, fRound);
		}
		return d;
	}

	internal static decimal AdjustDecimalScale(decimal value, int newScale)
	{
		int num = (decimal.GetBits(value)[3] & 0xFF0000) >> 16;
		if (newScale != num)
		{
			bool fRound = !EnableTruncateSwitch;
			SqlDecimal n = new SqlDecimal(value);
			return SqlDecimal.AdjustScale(n, newScale - num, fRound).Value;
		}
		return value;
	}

	internal byte[] SerializeSqlDecimal(SqlDecimal d, TdsParserStateObject stateObj)
	{
		if (stateObj._bDecimalBytes == null)
		{
			stateObj._bDecimalBytes = new byte[17];
		}
		byte[] bDecimalBytes = stateObj._bDecimalBytes;
		int num = 0;
		if (d.IsPositive)
		{
			bDecimalBytes[num++] = 1;
		}
		else
		{
			bDecimalBytes[num++] = 0;
		}
		SqlTypeWorkarounds.SqlDecimalExtractData(d, out var data, out var data2, out var data3, out var data4);
		byte[] src = SerializeUnsignedInt(data, stateObj);
		Buffer.BlockCopy(src, 0, bDecimalBytes, num, 4);
		num += 4;
		src = SerializeUnsignedInt(data2, stateObj);
		Buffer.BlockCopy(src, 0, bDecimalBytes, num, 4);
		num += 4;
		src = SerializeUnsignedInt(data3, stateObj);
		Buffer.BlockCopy(src, 0, bDecimalBytes, num, 4);
		num += 4;
		src = SerializeUnsignedInt(data4, stateObj);
		Buffer.BlockCopy(src, 0, bDecimalBytes, num, 4);
		return bDecimalBytes;
	}

	internal void WriteSqlDecimal(SqlDecimal d, TdsParserStateObject stateObj)
	{
		if (d.IsPositive)
		{
			stateObj.WriteByte(1);
		}
		else
		{
			stateObj.WriteByte(0);
		}
		SqlTypeWorkarounds.SqlDecimalExtractData(d, out var data, out var data2, out var data3, out var data4);
		WriteUnsignedInt(data, stateObj);
		WriteUnsignedInt(data2, stateObj);
		WriteUnsignedInt(data3, stateObj);
		WriteUnsignedInt(data4, stateObj);
	}

	private byte[] SerializeDecimal(decimal value, TdsParserStateObject stateObj)
	{
		int[] bits = decimal.GetBits(value);
		if (stateObj._bDecimalBytes == null)
		{
			stateObj._bDecimalBytes = new byte[17];
		}
		byte[] bDecimalBytes = stateObj._bDecimalBytes;
		int num = 0;
		if (2147483648u == (bits[3] & 0x80000000u))
		{
			bDecimalBytes[num++] = 0;
		}
		else
		{
			bDecimalBytes[num++] = 1;
		}
		byte[] src = SerializeInt(bits[0], stateObj);
		Buffer.BlockCopy(src, 0, bDecimalBytes, num, 4);
		num += 4;
		src = SerializeInt(bits[1], stateObj);
		Buffer.BlockCopy(src, 0, bDecimalBytes, num, 4);
		num += 4;
		src = SerializeInt(bits[2], stateObj);
		Buffer.BlockCopy(src, 0, bDecimalBytes, num, 4);
		num += 4;
		src = SerializeInt(0, stateObj);
		Buffer.BlockCopy(src, 0, bDecimalBytes, num, 4);
		return bDecimalBytes;
	}

	private void WriteDecimal(decimal value, TdsParserStateObject stateObj)
	{
		stateObj._decimalBits = decimal.GetBits(value);
		if (2147483648u == (stateObj._decimalBits[3] & 0x80000000u))
		{
			stateObj.WriteByte(0);
		}
		else
		{
			stateObj.WriteByte(1);
		}
		WriteInt(stateObj._decimalBits[0], stateObj);
		WriteInt(stateObj._decimalBits[1], stateObj);
		WriteInt(stateObj._decimalBits[2], stateObj);
		WriteInt(0, stateObj);
	}

	private void WriteIdentifier(string s, TdsParserStateObject stateObj)
	{
		if (s != null)
		{
			stateObj.WriteByte(checked((byte)s.Length));
			WriteString(s, stateObj);
		}
		else
		{
			stateObj.WriteByte(0);
		}
	}

	private void WriteIdentifierWithShortLength(string s, TdsParserStateObject stateObj)
	{
		if (s != null)
		{
			WriteShort(checked((short)s.Length), stateObj);
			WriteString(s, stateObj);
		}
		else
		{
			WriteShort(0, stateObj);
		}
	}

	private Task WriteString(string s, TdsParserStateObject stateObj, bool canAccumulate = true)
	{
		return WriteString(s, s.Length, 0, stateObj, canAccumulate);
	}

	internal byte[] SerializeCharArray(char[] carr, int length, int offset)
	{
		int num = 2 * length;
		byte[] array = new byte[num];
		CopyCharsToBytes(carr, offset, array, 0, length);
		return array;
	}

	internal Task WriteCharArray(char[] carr, int length, int offset, TdsParserStateObject stateObj, bool canAccumulate = true)
	{
		int num = 2 * length;
		if (num < stateObj._outBuff.Length - stateObj._outBytesUsed)
		{
			CopyCharsToBytes(carr, offset, stateObj._outBuff, stateObj._outBytesUsed, length);
			stateObj._outBytesUsed += num;
			return null;
		}
		if (stateObj._bTmp == null || stateObj._bTmp.Length < num)
		{
			stateObj._bTmp = new byte[num];
		}
		CopyCharsToBytes(carr, offset, stateObj._bTmp, 0, length);
		return stateObj.WriteByteArray(stateObj._bTmp, num, 0, canAccumulate);
	}

	internal byte[] SerializeString(string s, int length, int offset)
	{
		int num = 2 * length;
		byte[] array = new byte[num];
		CopyStringToBytes(s, offset, array, 0, length);
		return array;
	}

	internal Task WriteString(string s, int length, int offset, TdsParserStateObject stateObj, bool canAccumulate = true)
	{
		int num = 2 * length;
		if (num < stateObj._outBuff.Length - stateObj._outBytesUsed)
		{
			CopyStringToBytes(s, offset, stateObj._outBuff, stateObj._outBytesUsed, length);
			stateObj._outBytesUsed += num;
			return null;
		}
		if (stateObj._bTmp == null || stateObj._bTmp.Length < num)
		{
			stateObj._bTmp = new byte[num];
		}
		CopyStringToBytes(s, offset, stateObj._bTmp, 0, length);
		return stateObj.WriteByteArray(stateObj._bTmp, num, 0, canAccumulate);
	}

	private static void CopyCharsToBytes(char[] source, int sourceOffset, byte[] dest, int destOffset, int charLength)
	{
		Buffer.BlockCopy(source, sourceOffset, dest, destOffset, charLength * 2);
	}

	private static void CopyStringToBytes(string source, int sourceOffset, byte[] dest, int destOffset, int charLength)
	{
		Encoding.Unicode.GetBytes(source, sourceOffset, charLength, dest, destOffset);
	}

	private Task WriteEncodingChar(string s, Encoding encoding, TdsParserStateObject stateObj, bool canAccumulate = true)
	{
		return WriteEncodingChar(s, s.Length, 0, encoding, stateObj, canAccumulate);
	}

	private byte[] SerializeEncodingChar(string s, int numChars, int offset, Encoding encoding)
	{
		byte[] array = null;
		if (encoding == null)
		{
			encoding = _defaultEncoding;
		}
		char[] array2 = s.ToCharArray(offset, numChars);
		array = new byte[encoding.GetByteCount(array2, 0, array2.Length)];
		encoding.GetBytes(array2, 0, array2.Length, array, 0);
		return array;
	}

	private Task WriteEncodingChar(string s, int numChars, int offset, Encoding encoding, TdsParserStateObject stateObj, bool canAccumulate = true)
	{
		if (encoding == null)
		{
			encoding = _defaultEncoding;
		}
		int num = stateObj._outBuff.Length - stateObj._outBytesUsed;
		if (numChars <= num && encoding.GetMaxByteCount(numChars) <= num)
		{
			int bytes = encoding.GetBytes(s, offset, numChars, stateObj._outBuff, stateObj._outBytesUsed);
			stateObj._outBytesUsed += bytes;
			return null;
		}
		char[] chars = s.ToCharArray(offset, numChars);
		byte[] bytes2 = encoding.GetBytes(chars, 0, numChars);
		return stateObj.WriteByteArray(bytes2, bytes2.Length, 0, canAccumulate);
	}

	internal int GetEncodingCharLength(string value, int numChars, int charOffset, Encoding encoding)
	{
		if (value == null || value == ADP.StrEmpty)
		{
			return 0;
		}
		if (encoding == null)
		{
			if (_defaultEncoding == null)
			{
				ThrowUnsupportedCollationEncountered(null);
			}
			encoding = _defaultEncoding;
		}
		char[] chars = value.ToCharArray(charOffset, numChars);
		return encoding.GetByteCount(chars, 0, numChars);
	}

	internal bool TryGetDataLength(SqlMetaDataPriv colmeta, TdsParserStateObject stateObj, out ulong length)
	{
		if (_isYukon && colmeta.metaType.IsPlp)
		{
			return stateObj.TryReadPlpLength(returnPlpNullIfNull: true, out length);
		}
		if (!TryGetTokenLength(colmeta.tdsType, stateObj, out var tokenLength))
		{
			length = 0uL;
			return false;
		}
		length = (ulong)tokenLength;
		return true;
	}

	internal bool TryGetTokenLength(byte token, TdsParserStateObject stateObj, out int tokenLength)
	{
		switch (token)
		{
		case 174:
			tokenLength = -1;
			return true;
		case 228:
		case 238:
			return stateObj.TryReadInt32(out tokenLength);
		default:
			if (_isYukon)
			{
				switch (token)
				{
				case 240:
					tokenLength = -1;
					return true;
				case 172:
					tokenLength = -1;
					return true;
				case 241:
				{
					if (!stateObj.TryReadUInt16(out var value))
					{
						tokenLength = 0;
						return false;
					}
					tokenLength = value;
					return true;
				}
				}
			}
			switch (token & 0x30)
			{
			case 48:
				tokenLength = (1 << ((token & 0xC) >> 2)) & 0xFF;
				return true;
			case 16:
				tokenLength = 0;
				return true;
			case 0:
			case 32:
			{
				if ((token & 0x80u) != 0)
				{
					if (!stateObj.TryReadUInt16(out var value2))
					{
						tokenLength = 0;
						return false;
					}
					tokenLength = value2;
					return true;
				}
				if ((token & 0xC) == 0)
				{
					if (!stateObj.TryReadInt32(out tokenLength))
					{
						return false;
					}
					return true;
				}
				if (!stateObj.TryReadByte(out var value3))
				{
					tokenLength = 0;
					return false;
				}
				tokenLength = value3;
				return true;
			}
			default:
				tokenLength = 0;
				return true;
			}
		}
	}

	private void ProcessAttention(TdsParserStateObject stateObj)
	{
		if (_state == TdsParserState.Closed || _state == TdsParserState.Broken)
		{
			return;
		}
		stateObj.StoreErrorAndWarningForAttention();
		try
		{
			Run(RunBehavior.Attention, null, null, null, stateObj);
		}
		catch (Exception e)
		{
			if (!ADP.IsCatchableExceptionType(e))
			{
				throw;
			}
			ADP.TraceExceptionWithoutRethrow(e);
			_state = TdsParserState.Broken;
			_connHandler.BreakConnection();
			throw;
		}
		stateObj.RestoreErrorAndWarningAfterAttention();
	}

	private static int StateValueLength(int dataLen)
	{
		if (dataLen >= 255)
		{
			return dataLen + 5;
		}
		return dataLen + 1;
	}

	internal int WriteSessionRecoveryFeatureRequest(SessionData reconnectData, bool write)
	{
		int num = 1;
		if (write)
		{
			_physicalStateObj.WriteByte(1);
		}
		if (reconnectData == null)
		{
			if (write)
			{
				WriteInt(0, _physicalStateObj);
			}
			return num + 4;
		}
		int num2 = 0;
		num2 += 1 + 2 * TdsParserStaticMethods.NullAwareStringLength(reconnectData._initialDatabase);
		num2 += 1 + 2 * TdsParserStaticMethods.NullAwareStringLength(reconnectData._initialLanguage);
		num2 += ((reconnectData._initialCollation == null) ? 1 : 6);
		for (int i = 0; i < 256; i++)
		{
			if (reconnectData._initialState[i] != null)
			{
				num2 += 1 + StateValueLength(reconnectData._initialState[i].Length);
			}
		}
		int num3 = 0;
		num3 += 1 + 2 * ((!(reconnectData._initialDatabase == reconnectData._database)) ? TdsParserStaticMethods.NullAwareStringLength(reconnectData._database) : 0);
		num3 += 1 + 2 * ((!(reconnectData._initialLanguage == reconnectData._language)) ? TdsParserStaticMethods.NullAwareStringLength(reconnectData._language) : 0);
		num3 += ((reconnectData._collation == null || SqlCollation.AreSame(reconnectData._collation, reconnectData._initialCollation)) ? 1 : 6);
		bool[] array = new bool[256];
		for (int j = 0; j < 256; j++)
		{
			if (reconnectData._delta[j] == null)
			{
				continue;
			}
			array[j] = true;
			if (reconnectData._initialState[j] != null && reconnectData._initialState[j].Length == reconnectData._delta[j]._dataLength)
			{
				array[j] = false;
				for (int k = 0; k < reconnectData._delta[j]._dataLength; k++)
				{
					if (reconnectData._initialState[j][k] != reconnectData._delta[j]._data[k])
					{
						array[j] = true;
						break;
					}
				}
			}
			if (array[j])
			{
				num3 += 1 + StateValueLength(reconnectData._delta[j]._dataLength);
			}
		}
		if (write)
		{
			WriteInt(8 + num2 + num3, _physicalStateObj);
			WriteInt(num2, _physicalStateObj);
			WriteIdentifier(reconnectData._initialDatabase, _physicalStateObj);
			WriteCollation(reconnectData._initialCollation, _physicalStateObj);
			WriteIdentifier(reconnectData._initialLanguage, _physicalStateObj);
			for (int l = 0; l < 256; l++)
			{
				if (reconnectData._initialState[l] != null)
				{
					_physicalStateObj.WriteByte((byte)l);
					if (reconnectData._initialState[l].Length < 255)
					{
						_physicalStateObj.WriteByte((byte)reconnectData._initialState[l].Length);
					}
					else
					{
						_physicalStateObj.WriteByte(byte.MaxValue);
						WriteInt(reconnectData._initialState[l].Length, _physicalStateObj);
					}
					_physicalStateObj.WriteByteArray(reconnectData._initialState[l], reconnectData._initialState[l].Length, 0);
				}
			}
			WriteInt(num3, _physicalStateObj);
			WriteIdentifier((reconnectData._database != reconnectData._initialDatabase) ? reconnectData._database : null, _physicalStateObj);
			WriteCollation(SqlCollation.AreSame(reconnectData._initialCollation, reconnectData._collation) ? null : reconnectData._collation, _physicalStateObj);
			WriteIdentifier((reconnectData._language != reconnectData._initialLanguage) ? reconnectData._language : null, _physicalStateObj);
			for (int m = 0; m < 256; m++)
			{
				if (array[m])
				{
					_physicalStateObj.WriteByte((byte)m);
					if (reconnectData._delta[m]._dataLength < 255)
					{
						_physicalStateObj.WriteByte((byte)reconnectData._delta[m]._dataLength);
					}
					else
					{
						_physicalStateObj.WriteByte(byte.MaxValue);
						WriteInt(reconnectData._delta[m]._dataLength, _physicalStateObj);
					}
					_physicalStateObj.WriteByteArray(reconnectData._delta[m]._data, reconnectData._delta[m]._dataLength, 0);
				}
			}
		}
		return num + (num2 + num3 + 12);
	}

	internal int WriteFedAuthFeatureRequest(FederatedAuthenticationFeatureExtensionData fedAuthFeatureData, bool write)
	{
		int num = 0;
		int num2 = 0;
		switch (fedAuthFeatureData.libraryType)
		{
		case TdsEnums.FedAuthLibrary.MSAL:
			num = 2;
			break;
		case TdsEnums.FedAuthLibrary.SecurityToken:
			num = 5 + fedAuthFeatureData.accessToken.Length;
			break;
		}
		num2 = num + 5;
		if (write)
		{
			_physicalStateObj.WriteByte(2);
			byte b = 0;
			switch (fedAuthFeatureData.libraryType)
			{
			case TdsEnums.FedAuthLibrary.MSAL:
				b = (byte)(b | 4u);
				break;
			case TdsEnums.FedAuthLibrary.SecurityToken:
				b = (byte)(b | 2u);
				break;
			}
			b |= (byte)(fedAuthFeatureData.fedAuthRequiredPreLoginResponse ? 1u : 0u);
			WriteInt(num, _physicalStateObj);
			_physicalStateObj.WriteByte(b);
			switch (fedAuthFeatureData.libraryType)
			{
			case TdsEnums.FedAuthLibrary.MSAL:
			{
				byte b2 = 0;
				switch (fedAuthFeatureData.authentication)
				{
				case SqlAuthenticationMethod.ActiveDirectoryPassword:
					b2 = 1;
					break;
				case SqlAuthenticationMethod.ActiveDirectoryIntegrated:
					b2 = 2;
					break;
				case SqlAuthenticationMethod.ActiveDirectoryInteractive:
					b2 = 3;
					break;
				case SqlAuthenticationMethod.ActiveDirectoryServicePrincipal:
					b2 = 1;
					break;
				case SqlAuthenticationMethod.ActiveDirectoryDeviceCodeFlow:
					b2 = 3;
					break;
				case SqlAuthenticationMethod.ActiveDirectoryManagedIdentity:
				case SqlAuthenticationMethod.ActiveDirectoryMSI:
					b2 = 3;
					break;
				case SqlAuthenticationMethod.ActiveDirectoryDefault:
					b2 = 3;
					break;
				}
				_physicalStateObj.WriteByte(b2);
				break;
			}
			case TdsEnums.FedAuthLibrary.SecurityToken:
				WriteInt(fedAuthFeatureData.accessToken.Length, _physicalStateObj);
				_physicalStateObj.WriteByteArray(fedAuthFeatureData.accessToken, fedAuthFeatureData.accessToken.Length, 0);
				break;
			}
		}
		return num2;
	}

	internal int WriteDataClassificationFeatureRequest(bool write)
	{
		int result = 6;
		if (write)
		{
			_physicalStateObj.WriteByte(9);
			WriteInt(1, _physicalStateObj);
			_physicalStateObj.WriteByte(2);
		}
		return result;
	}

	internal int WriteTceFeatureRequest(bool write)
	{
		int result = 6;
		if (write)
		{
			_physicalStateObj.WriteByte(4);
			WriteInt(1, _physicalStateObj);
			_physicalStateObj.WriteByte(3);
		}
		return result;
	}

	internal int WriteGlobalTransactionsFeatureRequest(bool write)
	{
		int result = 5;
		if (write)
		{
			_physicalStateObj.WriteByte(5);
			WriteInt(0, _physicalStateObj);
		}
		return result;
	}

	internal int WriteAzureSQLSupportFeatureRequest(bool write)
	{
		int result = 6;
		if (write)
		{
			_physicalStateObj.WriteByte(8);
			WriteInt(s_FeatureExtDataAzureSQLSupportFeatureRequest.Length, _physicalStateObj);
			_physicalStateObj.WriteByteArray(s_FeatureExtDataAzureSQLSupportFeatureRequest, s_FeatureExtDataAzureSQLSupportFeatureRequest.Length, 0);
		}
		return result;
	}

	internal int WriteUTF8SupportFeatureRequest(bool write)
	{
		int result = 5;
		if (write)
		{
			_physicalStateObj.WriteByte(10);
			WriteInt(0, _physicalStateObj);
		}
		return result;
	}

	internal int WriteSQLDNSCachingFeatureRequest(bool write)
	{
		int result = 5;
		if (write)
		{
			_physicalStateObj.WriteByte(11);
			WriteInt(0, _physicalStateObj);
		}
		return result;
	}

	internal void TdsLogin(SqlLogin rec, TdsEnums.FeatureExtension requestedFeatures, SessionData recoverySessionData, FederatedAuthenticationFeatureExtensionData fedAuthFeatureExtensionData, SqlClientOriginalNetworkAddressInfo originalNetworkAddressInfo)
	{
		_physicalStateObj.SetTimeoutSeconds(rec.timeout);
		_connHandler.TimeoutErrorInternal.EndPhase(SqlConnectionTimeoutErrorPhase.LoginBegin);
		_connHandler.TimeoutErrorInternal.SetAndBeginPhase(SqlConnectionTimeoutErrorPhase.ProcessConnectionAuth);
		if (originalNetworkAddressInfo != null)
		{
			SNINativeMethodWrapper.CTAIPProviderInfo authInfo = default(SNINativeMethodWrapper.CTAIPProviderInfo);
			authInfo.originalNetworkAddress = originalNetworkAddressInfo.Address.GetAddressBytes();
			authInfo.fromDataSecurityProxy = originalNetworkAddressInfo.IsFromDataSecurityProxy;
			if (SNINativeMethodWrapper.SNIAddProvider(_physicalStateObj.Handle, SNINativeMethodWrapper.ProviderEnum.CTAIP_PROV, authInfo) != 0)
			{
				_physicalStateObj.AddError(ProcessSNIError(_physicalStateObj));
				ThrowExceptionAndWarning(_physicalStateObj);
			}
			try
			{
			}
			finally
			{
				_physicalStateObj.ClearAllWritePackets();
			}
		}
		byte[] array = null;
		byte[] array2 = null;
		bool flag = requestedFeatures != TdsEnums.FeatureExtension.None;
		string text;
		int num;
		if (rec.credential != null)
		{
			text = rec.credential.UserId;
			num = rec.credential.Password.Length * 2;
		}
		else
		{
			text = rec.userName;
			array = TdsParserStaticMethods.ObfuscatePassword(rec.password);
			num = array.Length;
		}
		int num2;
		if (rec.newSecurePassword != null)
		{
			num2 = rec.newSecurePassword.Length * 2;
		}
		else
		{
			array2 = TdsParserStaticMethods.ObfuscatePassword(rec.newPassword);
			num2 = array2.Length;
		}
		_physicalStateObj._outputMessageType = 16;
		int num3 = 94;
		string text2 = "Framework Microsoft SqlClient Data Provider";
		byte[] array3;
		uint sendLength;
		int v;
		checked
		{
			num3 += (rec.hostName.Length + rec.applicationName.Length + rec.serverName.Length + text2.Length + rec.language.Length + rec.database.Length + rec.attachDBFilename.Length) * 2;
			if (flag)
			{
				num3 += 4;
			}
			array3 = null;
			sendLength = 0u;
			if (!rec.useSSPI && !_connHandler._federatedAuthenticationInfoRequested && !_connHandler._federatedAuthenticationRequested)
			{
				num3 += text.Length * 2 + num + num2;
			}
			else if (rec.useSSPI)
			{
				array3 = new byte[s_maxSSPILength];
				sendLength = s_maxSSPILength;
				_physicalStateObj.SniContext = SniContext.Snix_LoginSspi;
				SSPIData(null, 0u, array3, ref sendLength);
				if (sendLength > int.MaxValue)
				{
					throw SQL.InvalidSSPIPacketSize();
				}
				_physicalStateObj.SniContext = SniContext.Snix_Login;
				num3 += (int)sendLength;
			}
			v = num3;
			if (flag)
			{
				if ((requestedFeatures & TdsEnums.FeatureExtension.SessionRecovery) != 0)
				{
					num3 += WriteSessionRecoveryFeatureRequest(recoverySessionData, write: false);
				}
				if ((requestedFeatures & TdsEnums.FeatureExtension.FedAuth) != 0)
				{
					num3 += WriteFedAuthFeatureRequest(fedAuthFeatureExtensionData, write: false);
				}
				if ((requestedFeatures & TdsEnums.FeatureExtension.Tce) != 0)
				{
					num3 += WriteTceFeatureRequest(write: false);
				}
				if ((requestedFeatures & TdsEnums.FeatureExtension.GlobalTransactions) != 0)
				{
					num3 += WriteGlobalTransactionsFeatureRequest(write: false);
				}
				if ((requestedFeatures & TdsEnums.FeatureExtension.AzureSQLSupport) != 0)
				{
					num3 += WriteAzureSQLSupportFeatureRequest(write: false);
				}
				if ((requestedFeatures & TdsEnums.FeatureExtension.DataClassification) != 0)
				{
					num3 += WriteDataClassificationFeatureRequest(write: false);
				}
				if ((requestedFeatures & TdsEnums.FeatureExtension.UTF8Support) != 0)
				{
					num3 += WriteUTF8SupportFeatureRequest(write: false);
				}
				if ((requestedFeatures & TdsEnums.FeatureExtension.SQLDNSCaching) != 0)
				{
					num3 += WriteSQLDNSCachingFeatureRequest(write: false);
				}
				num3++;
			}
		}
		try
		{
			WriteInt(num3, _physicalStateObj);
			if (recoverySessionData == null)
			{
				WriteInt(1946157060, _physicalStateObj);
			}
			else
			{
				WriteUnsignedInt(recoverySessionData._tdsVersion, _physicalStateObj);
			}
			WriteInt(rec.packetSize, _physicalStateObj);
			WriteInt(100663296, _physicalStateObj);
			WriteInt(TdsParserStaticMethods.GetCurrentProcessIdForTdsLoginOnly(), _physicalStateObj);
			WriteInt(0, _physicalStateObj);
			int num4 = 0;
			num4 |= 0x20;
			num4 |= 0x40;
			num4 |= 0x80;
			num4 |= 0x100;
			num4 |= 0x200;
			if (rec.useReplication)
			{
				num4 |= 0x3000;
			}
			if (rec.useSSPI)
			{
				num4 |= 0x8000;
			}
			if (rec.readOnlyIntent)
			{
				num4 |= 0x200000;
			}
			if (!ADP.IsEmpty(rec.newPassword) || (rec.newSecurePassword != null && rec.newSecurePassword.Length != 0))
			{
				num4 |= 0x1000000;
			}
			if (rec.userInstance)
			{
				num4 |= 0x4000000;
			}
			if (flag)
			{
				num4 |= 0x10000000;
			}
			WriteInt(num4, _physicalStateObj);
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParser.TdsLogin|ADV> {0}, TDS Login7 flags = {1}:", ObjectID, num4);
			WriteInt(0, _physicalStateObj);
			WriteInt(0, _physicalStateObj);
			int num5 = 94;
			WriteShort(num5, _physicalStateObj);
			WriteShort(rec.hostName.Length, _physicalStateObj);
			num5 += rec.hostName.Length * 2;
			if (!rec.useSSPI && !_connHandler._federatedAuthenticationInfoRequested && !_connHandler._federatedAuthenticationRequested)
			{
				WriteShort(num5, _physicalStateObj);
				WriteShort(text.Length, _physicalStateObj);
				num5 += text.Length * 2;
				WriteShort(num5, _physicalStateObj);
				WriteShort(num / 2, _physicalStateObj);
				num5 += num;
			}
			else
			{
				WriteShort(0, _physicalStateObj);
				WriteShort(0, _physicalStateObj);
				WriteShort(0, _physicalStateObj);
				WriteShort(0, _physicalStateObj);
			}
			WriteShort(num5, _physicalStateObj);
			WriteShort(rec.applicationName.Length, _physicalStateObj);
			num5 += rec.applicationName.Length * 2;
			WriteShort(num5, _physicalStateObj);
			WriteShort(rec.serverName.Length, _physicalStateObj);
			num5 += rec.serverName.Length * 2;
			WriteShort(num5, _physicalStateObj);
			if (flag)
			{
				WriteShort(4, _physicalStateObj);
				num5 += 4;
			}
			else
			{
				WriteShort(0, _physicalStateObj);
			}
			WriteShort(num5, _physicalStateObj);
			WriteShort(text2.Length, _physicalStateObj);
			num5 += text2.Length * 2;
			WriteShort(num5, _physicalStateObj);
			WriteShort(rec.language.Length, _physicalStateObj);
			num5 += rec.language.Length * 2;
			WriteShort(num5, _physicalStateObj);
			WriteShort(rec.database.Length, _physicalStateObj);
			num5 += rec.database.Length * 2;
			if (s_nicAddress == null)
			{
				s_nicAddress = TdsParserStaticMethods.GetNetworkPhysicalAddressForTdsLoginOnly();
			}
			_physicalStateObj.WriteByteArray(s_nicAddress, s_nicAddress.Length, 0);
			WriteShort(num5, _physicalStateObj);
			if (rec.useSSPI)
			{
				WriteShort((int)sendLength, _physicalStateObj);
				num5 += (int)sendLength;
			}
			else
			{
				WriteShort(0, _physicalStateObj);
			}
			WriteShort(num5, _physicalStateObj);
			WriteShort(rec.attachDBFilename.Length, _physicalStateObj);
			num5 += rec.attachDBFilename.Length * 2;
			WriteShort(num5, _physicalStateObj);
			WriteShort(num2 / 2, _physicalStateObj);
			WriteInt(0, _physicalStateObj);
			WriteString(rec.hostName, _physicalStateObj);
			if (!rec.useSSPI && !_connHandler._federatedAuthenticationInfoRequested && !_connHandler._federatedAuthenticationRequested)
			{
				WriteString(text, _physicalStateObj);
				_physicalStateObj._tracePasswordOffset = _physicalStateObj._outBytesUsed;
				_physicalStateObj._tracePasswordLength = num;
				if (rec.credential != null)
				{
					_physicalStateObj.WriteSecureString(rec.credential.Password);
				}
				else
				{
					_physicalStateObj.WriteByteArray(array, num, 0);
				}
			}
			WriteString(rec.applicationName, _physicalStateObj);
			WriteString(rec.serverName, _physicalStateObj);
			if (flag)
			{
				WriteInt(v, _physicalStateObj);
			}
			WriteString(text2, _physicalStateObj);
			WriteString(rec.language, _physicalStateObj);
			WriteString(rec.database, _physicalStateObj);
			if (rec.useSSPI)
			{
				_physicalStateObj.WriteByteArray(array3, (int)sendLength, 0);
			}
			WriteString(rec.attachDBFilename, _physicalStateObj);
			if (!rec.useSSPI && !_connHandler._federatedAuthenticationInfoRequested && !_connHandler._federatedAuthenticationRequested)
			{
				_physicalStateObj._traceChangePasswordOffset = _physicalStateObj._outBytesUsed;
				_physicalStateObj._traceChangePasswordLength = num2;
				if (rec.newSecurePassword != null)
				{
					_physicalStateObj.WriteSecureString(rec.newSecurePassword);
				}
				else
				{
					_physicalStateObj.WriteByteArray(array2, num2, 0);
				}
			}
			if (flag)
			{
				if ((requestedFeatures & TdsEnums.FeatureExtension.SessionRecovery) != 0)
				{
					WriteSessionRecoveryFeatureRequest(recoverySessionData, write: true);
				}
				if ((requestedFeatures & TdsEnums.FeatureExtension.FedAuth) != 0)
				{
					SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.TdsLogin|SEC> Sending federated authentication feature request");
					WriteFedAuthFeatureRequest(fedAuthFeatureExtensionData, write: true);
				}
				if ((requestedFeatures & TdsEnums.FeatureExtension.Tce) != 0)
				{
					WriteTceFeatureRequest(write: true);
				}
				if ((requestedFeatures & TdsEnums.FeatureExtension.GlobalTransactions) != 0)
				{
					WriteGlobalTransactionsFeatureRequest(write: true);
				}
				if ((requestedFeatures & TdsEnums.FeatureExtension.AzureSQLSupport) != 0)
				{
					WriteAzureSQLSupportFeatureRequest(write: true);
				}
				if ((requestedFeatures & TdsEnums.FeatureExtension.DataClassification) != 0)
				{
					WriteDataClassificationFeatureRequest(write: true);
				}
				if ((requestedFeatures & TdsEnums.FeatureExtension.UTF8Support) != 0)
				{
					WriteUTF8SupportFeatureRequest(write: true);
				}
				if ((requestedFeatures & TdsEnums.FeatureExtension.SQLDNSCaching) != 0)
				{
					WriteSQLDNSCachingFeatureRequest(write: true);
				}
				_physicalStateObj.WriteByte(byte.MaxValue);
			}
		}
		catch (Exception e)
		{
			if (ADP.IsCatchableExceptionType(e))
			{
				_physicalStateObj.ResetPacketCounters();
				_physicalStateObj.ResetBuffer();
			}
			throw;
		}
		_physicalStateObj.WritePacket(1);
		_physicalStateObj.ResetSecurePasswordsInfomation();
		_physicalStateObj._pendingData = true;
		_physicalStateObj._messageStatus = 0;
		if (originalNetworkAddressInfo != null)
		{
			if (SNINativeMethodWrapper.SNIRemoveProvider(_physicalStateObj.Handle, SNINativeMethodWrapper.ProviderEnum.CTAIP_PROV) != 0)
			{
				_physicalStateObj.AddError(ProcessSNIError(_physicalStateObj));
				ThrowExceptionAndWarning(_physicalStateObj);
			}
			try
			{
			}
			finally
			{
				_physicalStateObj.ClearAllWritePackets();
			}
		}
	}

	internal void SendFedAuthToken(SqlFedAuthToken fedAuthToken)
	{
		SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.SendFedAuthToken|SEC> Sending federated authentication token");
		_physicalStateObj._outputMessageType = 8;
		byte[] accessToken = fedAuthToken.accessToken;
		WriteUnsignedInt((uint)(accessToken.Length + 4), _physicalStateObj);
		WriteUnsignedInt((uint)accessToken.Length, _physicalStateObj);
		_physicalStateObj.WriteByteArray(accessToken, accessToken.Length, 0);
		_physicalStateObj.WritePacket(1);
		_physicalStateObj._pendingData = true;
		_physicalStateObj._messageStatus = 0;
		_connHandler._federatedAuthenticationRequested = true;
	}

	private void SSPIData(byte[] receivedBuff, uint receivedLength, byte[] sendBuff, ref uint sendLength)
	{
		SNISSPIData(receivedBuff, receivedLength, sendBuff, ref sendLength);
	}

	private void SNISSPIData(byte[] receivedBuff, uint receivedLength, byte[] sendBuff, ref uint sendLength)
	{
		if (receivedBuff == null)
		{
			receivedLength = 0u;
		}
		if (SNINativeMethodWrapper.SNISecGenClientContext(_physicalStateObj.Handle, receivedBuff, receivedLength, sendBuff, ref sendLength, _sniSpnBuffer) != 0)
		{
			SSPIError(SQLMessage.SSPIGenerateError(), "GenClientContext");
		}
	}

	private void ProcessSSPI(int receivedLength)
	{
		SniContext sniContext = _physicalStateObj.SniContext;
		_physicalStateObj.SniContext = SniContext.Snix_ProcessSspi;
		byte[] array = new byte[receivedLength];
		if (!_physicalStateObj.TryReadByteArray(array, 0, receivedLength))
		{
			throw SQL.SynchronousCallMayNotPend();
		}
		byte[] array2 = new byte[s_maxSSPILength];
		uint sendLength = s_maxSSPILength;
		SSPIData(array, (uint)receivedLength, array2, ref sendLength);
		_physicalStateObj.WriteByteArray(array2, (int)sendLength, 0);
		_physicalStateObj._outputMessageType = 17;
		_physicalStateObj.WritePacket(1);
		_physicalStateObj.SniContext = sniContext;
	}

	private void SSPIError(string error, string procedure)
	{
		_physicalStateObj.AddError(new SqlError(0, 0, 11, _server, error, procedure, 0));
		ThrowExceptionAndWarning(_physicalStateObj);
	}

	private void LoadSSPILibrary()
	{
		if (!s_fSSPILoaded)
		{
			lock (s_tdsParserLock)
			{
				if (!s_fSSPILoaded)
				{
					uint pcbMaxToken = 0u;
					if (SNINativeMethodWrapper.SNISecInitPackage(ref pcbMaxToken) != 0)
					{
						SSPIError(SQLMessage.SSPIInitializeError(), "InitSSPIPackage");
					}
					s_maxSSPILength = pcbMaxToken;
					s_fSSPILoaded = true;
				}
			}
		}
		if (s_maxSSPILength > int.MaxValue)
		{
			throw SQL.InvalidSSPIPacketSize();
		}
	}

	internal byte[] GetDTCAddress(int timeout, TdsParserStateObject stateObj)
	{
		byte[] array = null;
		using (SqlDataReader sqlDataReader = TdsExecuteTransactionManagerRequest(null, TdsEnums.TransactionManagerRequestType.GetDTCAddress, null, TdsEnums.TransactionManagerIsolationLevel.Unspecified, timeout, null, stateObj, isDelegateControlRequest: true))
		{
			if (sqlDataReader != null && sqlDataReader.Read())
			{
				long bytes = sqlDataReader.GetBytes(0, 0L, null, 0, 0);
				if (bytes <= int.MaxValue)
				{
					int num = (int)bytes;
					array = new byte[num];
					sqlDataReader.GetBytes(0, 0L, array, 0, num);
				}
			}
		}
		return array;
	}

	internal void PropagateDistributedTransaction(byte[] buffer, int timeout, TdsParserStateObject stateObj)
	{
		TdsExecuteTransactionManagerRequest(buffer, TdsEnums.TransactionManagerRequestType.Propagate, null, TdsEnums.TransactionManagerIsolationLevel.Unspecified, timeout, null, stateObj, isDelegateControlRequest: true);
	}

	internal SqlDataReader TdsExecuteTransactionManagerRequest(byte[] buffer, TdsEnums.TransactionManagerRequestType request, string transactionName, TdsEnums.TransactionManagerIsolationLevel isoLevel, int timeout, SqlInternalTransaction transaction, TdsParserStateObject stateObj, bool isDelegateControlRequest)
	{
		if (TdsParserState.Broken == State || State == TdsParserState.Closed)
		{
			return null;
		}
		bool threadHasParserLockForClose = _connHandler.ThreadHasParserLockForClose;
		if (!threadHasParserLockForClose)
		{
			_connHandler._parserLock.Wait(canReleaseFromAnyThread: false);
			_connHandler.ThreadHasParserLockForClose = true;
		}
		bool asyncWrite = _asyncWrite;
		try
		{
			_asyncWrite = false;
			if (!isDelegateControlRequest)
			{
				_connHandler.CheckEnlistedTransactionBinding();
			}
			stateObj._outputMessageType = 14;
			stateObj.SetTimeoutSeconds(timeout);
			stateObj.SniContext = SniContext.Snix_Execute;
			if (_isYukon)
			{
				WriteInt(22, stateObj);
				WriteInt(18, stateObj);
				WriteMarsHeaderData(stateObj, _currentTransaction);
			}
			WriteShort((short)request, stateObj);
			bool flag = false;
			switch (request)
			{
			case TdsEnums.TransactionManagerRequestType.GetDTCAddress:
				WriteShort(0, stateObj);
				flag = true;
				break;
			case TdsEnums.TransactionManagerRequestType.Propagate:
				if (buffer != null)
				{
					WriteShort(buffer.Length, stateObj);
					stateObj.WriteByteArray(buffer, buffer.Length, 0);
				}
				else
				{
					WriteShort(0, stateObj);
				}
				break;
			case TdsEnums.TransactionManagerRequestType.Begin:
				if (_currentTransaction != transaction)
				{
					PendingTransaction = transaction;
				}
				stateObj.WriteByte((byte)isoLevel);
				stateObj.WriteByte((byte)(transactionName.Length * 2));
				WriteString(transactionName, stateObj);
				break;
			case TdsEnums.TransactionManagerRequestType.Commit:
				stateObj.WriteByte(0);
				stateObj.WriteByte(0);
				break;
			case TdsEnums.TransactionManagerRequestType.Rollback:
				stateObj.WriteByte((byte)(transactionName.Length * 2));
				WriteString(transactionName, stateObj);
				stateObj.WriteByte(0);
				break;
			case TdsEnums.TransactionManagerRequestType.Save:
				stateObj.WriteByte((byte)(transactionName.Length * 2));
				WriteString(transactionName, stateObj);
				break;
			}
			Task task = stateObj.WritePacket(1);
			stateObj._pendingData = true;
			stateObj._messageStatus = 0;
			SqlDataReader sqlDataReader = null;
			stateObj.SniContext = SniContext.Snix_Read;
			if (flag)
			{
				sqlDataReader = new SqlDataReader(null, CommandBehavior.Default);
				sqlDataReader.Bind(stateObj);
				_SqlMetaDataSet metaData = sqlDataReader.MetaData;
			}
			else
			{
				Run(RunBehavior.UntilDone, null, null, null, stateObj);
			}
			if ((request == TdsEnums.TransactionManagerRequestType.Begin || request == TdsEnums.TransactionManagerRequestType.Propagate) && (transaction == null || transaction.TransactionId != _retainedTransactionId))
			{
				_retainedTransactionId = 0L;
			}
			return sqlDataReader;
		}
		catch (Exception e)
		{
			if (!ADP.IsCatchableExceptionType(e))
			{
				throw;
			}
			FailureCleanup(stateObj, e);
			throw;
		}
		finally
		{
			_pendingTransaction = null;
			_asyncWrite = asyncWrite;
			if (!threadHasParserLockForClose)
			{
				_connHandler.ThreadHasParserLockForClose = false;
				_connHandler._parserLock.Release();
			}
		}
	}

	internal void FailureCleanup(TdsParserStateObject stateObj, Exception e)
	{
		int outputPacketNumber = stateObj._outputPacketNumber;
		SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.FailureCleanup|ERR> Exception caught on ExecuteXXX: '{0}'", e);
		if (stateObj.HasOpenResult)
		{
			stateObj.DecrementOpenResultCount();
		}
		stateObj.ResetBuffer();
		stateObj.ResetPacketCounters();
		if (outputPacketNumber != 1 && _state == TdsParserState.OpenLoggedIn)
		{
			bool threadHasParserLockForClose = _connHandler.ThreadHasParserLockForClose;
			try
			{
				_connHandler.ThreadHasParserLockForClose = true;
				stateObj.SendAttention();
				ProcessAttention(stateObj);
			}
			finally
			{
				_connHandler.ThreadHasParserLockForClose = threadHasParserLockForClose;
			}
		}
		SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.FailureCleanup|ERR> Exception rethrown.");
	}

	internal Task TdsExecuteSQLBatch(string text, int timeout, SqlNotificationRequest notificationRequest, TdsParserStateObject stateObj, bool sync, bool callerHasConnectionLock = false, byte[] enclavePackage = null)
	{
		if (TdsParserState.Broken == State || State == TdsParserState.Closed)
		{
			return null;
		}
		if (stateObj.BcpLock)
		{
			throw SQL.ConnectionLockedForBcpEvent();
		}
		bool flag = !callerHasConnectionLock && !_connHandler.ThreadHasParserLockForClose;
		bool flag2 = false;
		if (flag)
		{
			_connHandler._parserLock.Wait(!sync);
			flag2 = true;
		}
		_asyncWrite = !sync;
		try
		{
			if (_state == TdsParserState.Closed || _state == TdsParserState.Broken)
			{
				throw ADP.ClosedConnectionError();
			}
			_connHandler.CheckEnlistedTransactionBinding();
			stateObj.SetTimeoutSeconds(timeout);
			if (!_fMARS && _physicalStateObj.HasOpenResult)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.TdsExecuteSQLBatch|ERR> Potential multi-threaded misuse of connection, non-MARs connection with an open result {0}", ObjectID);
			}
			stateObj.SniContext = SniContext.Snix_Execute;
			if (_isYukon)
			{
				WriteRPCBatchHeaders(stateObj, notificationRequest);
			}
			stateObj._outputMessageType = 1;
			WriteEnclaveInfo(stateObj, enclavePackage);
			WriteString(text, text.Length, 0, stateObj);
			Task task = stateObj.ExecuteFlush();
			if (task == null)
			{
				stateObj.SniContext = SniContext.Snix_Read;
				return null;
			}
			bool taskReleaseConnectionLock = flag2;
			flag2 = false;
			return task.ContinueWith(delegate(Task t)
			{
				try
				{
					if (t.IsFaulted)
					{
						FailureCleanup(stateObj, t.Exception.InnerException);
						throw t.Exception.InnerException;
					}
					stateObj.SniContext = SniContext.Snix_Read;
				}
				finally
				{
					if (taskReleaseConnectionLock)
					{
						_connHandler._parserLock.Release();
					}
				}
			}, TaskScheduler.Default);
		}
		catch (Exception e)
		{
			if (!ADP.IsCatchableExceptionType(e))
			{
				throw;
			}
			FailureCleanup(stateObj, e);
			throw;
		}
		finally
		{
			if (flag2)
			{
				_connHandler._parserLock.Release();
			}
		}
	}

	internal Task TdsExecuteRPC(SqlCommand cmd, _SqlRPC[] rpcArray, int timeout, bool inSchema, SqlNotificationRequest notificationRequest, TdsParserStateObject stateObj, bool isCommandProc, bool sync = true, TaskCompletionSource<object> completion = null, int startRpc = 0, int startParam = 0)
	{
		bool flag = completion == null;
		bool flag2 = false;
		try
		{
			_SqlRPC sqlRPC = null;
			if (flag)
			{
				_connHandler._parserLock.Wait(!sync);
				flag2 = true;
			}
			try
			{
				if (TdsParserState.Broken == State || State == TdsParserState.Closed)
				{
					throw ADP.ClosedConnectionError();
				}
				if (flag)
				{
					_asyncWrite = !sync;
					_connHandler.CheckEnlistedTransactionBinding();
					stateObj.SetTimeoutSeconds(timeout);
					if (!_fMARS && _physicalStateObj.HasOpenResult)
					{
						SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.TdsExecuteRPC|ERR> Potential multi-threaded misuse of connection, non-MARs connection with an open result {0}", ObjectID);
					}
					stateObj.SniContext = SniContext.Snix_Execute;
					if (_isYukon)
					{
						WriteRPCBatchHeaders(stateObj, notificationRequest);
					}
					stateObj._outputMessageType = 3;
				}
				for (int ii = startRpc; ii < rpcArray.Length; ii++)
				{
					sqlRPC = rpcArray[ii];
					if (startParam == 0 || ii > startRpc)
					{
						if (sqlRPC.ProcID != 0 && _isShiloh)
						{
							WriteShort(65535, stateObj);
							WriteShort((short)sqlRPC.ProcID, stateObj);
						}
						else
						{
							int length = sqlRPC.rpcName.Length;
							WriteShort(length, stateObj);
							WriteString(sqlRPC.rpcName, length, 0, stateObj);
						}
						WriteShort((short)sqlRPC.options, stateObj);
					}
					byte[] enclavePackage = ((cmd.enclavePackage != null) ? cmd.enclavePackage.EnclavePackageBytes : null);
					WriteEnclaveInfo(stateObj, enclavePackage);
					SqlParameter[] parameters = sqlRPC.parameters;
					bool advancedTraceIsOn = SqlClientEventSource.Log.IsAdvancedTraceOn();
					int i;
					for (i = ((ii == startRpc) ? startParam : 0); i < parameters.Length; i++)
					{
						SqlParameter sqlParameter = parameters[i];
						if (sqlParameter == null)
						{
							break;
						}
						if (sqlParameter.ForceColumnEncryption && cmd.ColumnEncryptionSetting != SqlCommandColumnEncryptionSetting.Enabled && (cmd.ColumnEncryptionSetting != 0 || !cmd.Connection.IsColumnEncryptionSettingEnabled))
						{
							throw SQL.ParamInvalidForceColumnEncryptionSetting(sqlParameter.ParameterName, sqlRPC.GetCommandTextOrRpcName());
						}
						if (sqlParameter.ForceColumnEncryption && sqlParameter.CipherMetadata == null && (sqlParameter.Direction == ParameterDirection.Input || sqlParameter.Direction == ParameterDirection.InputOutput))
						{
							throw SQL.ParamUnExpectedEncryptionMetadata(sqlParameter.ParameterName, sqlRPC.GetCommandTextOrRpcName());
						}
						sqlParameter.Validate(i, isCommandProc);
						MetaType metaType = sqlParameter.InternalMetaType;
						if (metaType.IsNewKatmaiType)
						{
							WriteSmiParameter(sqlParameter, i, (sqlRPC.paramoptions[i] & 2) != 0, stateObj, advancedTraceIsOn);
							continue;
						}
						if ((!_isShiloh && !metaType.Is70Supported) || (!_isYukon && !metaType.Is80Supported) || (!_isKatmai && !metaType.Is90Supported))
						{
							throw ADP.VersionDoesNotSupportDataType(metaType.TypeName);
						}
						object obj = null;
						bool flag3 = true;
						bool flag4 = false;
						bool flag5 = false;
						if (sqlParameter.Direction == ParameterDirection.Output)
						{
							flag4 = sqlParameter.ParameterIsSqlType;
							sqlParameter.Value = null;
							sqlParameter.ParameterIsSqlType = flag4;
						}
						else
						{
							obj = sqlParameter.GetCoercedValue();
							flag3 = sqlParameter.IsNull;
							if (!flag3)
							{
								flag4 = sqlParameter.CoercedValueIsSqlType;
								flag5 = sqlParameter.CoercedValueIsDataFeed;
							}
						}
						WriteParameterName(sqlParameter.ParameterNameFixed, stateObj);
						stateObj.WriteByte(sqlRPC.paramoptions[i]);
						int num = (metaType.IsSizeInCharacters ? (sqlParameter.GetParameterSize() * 2) : sqlParameter.GetParameterSize());
						int num2 = ((metaType.TDSType != 240) ? sqlParameter.GetActualSize() : 0);
						byte b = 0;
						byte b2 = 0;
						if (metaType.SqlDbType == SqlDbType.Decimal)
						{
							b = sqlParameter.GetActualPrecision();
							b2 = sqlParameter.GetActualScale();
							if (b > 38)
							{
								throw SQL.PrecisionValueOutOfRange(b);
							}
							if (!flag3)
							{
								if (flag4)
								{
									obj = AdjustSqlDecimalScale((SqlDecimal)obj, b2);
									if (b != 0 && b < ((SqlDecimal)obj).Precision)
									{
										throw ADP.ParameterValueOutOfRange((SqlDecimal)obj);
									}
								}
								else
								{
									obj = AdjustDecimalScale((decimal)obj, b2);
									SqlDecimal sqlDecimal = new SqlDecimal((decimal)obj);
									if (b != 0 && b < sqlDecimal.Precision)
									{
										throw ADP.ParameterValueOutOfRange((decimal)obj);
									}
								}
							}
						}
						bool flag6 = (sqlRPC.paramoptions[i] & 8) != 0;
						SqlColumnEncryptionInputParameterInfo columnEncryptionParameterInfo = null;
						if (flag6)
						{
							byte[] array = null;
							byte[] array2 = null;
							if (!flag3)
							{
								try
								{
									array = ((!flag4) ? SerializeUnencryptedValue(obj, metaType, sqlParameter.GetActualScale(), num2, sqlParameter.Offset, flag5, sqlParameter.NormalizationRuleVersion, stateObj) : SerializeUnencryptedSqlValue(obj, metaType, num2, sqlParameter.Offset, sqlParameter.NormalizationRuleVersion, stateObj));
									array2 = SqlSecurityUtility.EncryptWithKey(array, sqlParameter.CipherMetadata, _connHandler.Connection, cmd);
								}
								catch (Exception e)
								{
									throw SQL.ParamEncryptionFailed(sqlParameter.ParameterName, null, e);
								}
							}
							else
							{
								array2 = null;
							}
							metaType = MetaType.MetaMaxVarBinary;
							num = -1;
							num2 = ((array2 != null) ? array2.Length : 0);
							columnEncryptionParameterInfo = new SqlColumnEncryptionInputParameterInfo(sqlParameter.GetMetadataForTypeInfo(), sqlParameter.CipherMetadata);
							obj = array2;
							flag4 = false;
						}
						stateObj.WriteByte(metaType.NullableType);
						if (metaType.TDSType == 98)
						{
							WriteSqlVariantValue(flag4 ? MetaType.GetComValueFromSqlVariant(obj) : obj, sqlParameter.GetActualSize(), sqlParameter.Offset, stateObj);
							continue;
						}
						int num3 = 0;
						int maxSize = 0;
						if (metaType.IsAnsiType)
						{
							if (!flag3 && !flag5)
							{
								string value = ((!flag4) ? ((string)obj) : ((!(obj is SqlString sqlString)) ? new string(((SqlChars)obj).Value) : sqlString.Value));
								num3 = GetEncodingCharLength(value, num2, sqlParameter.Offset, _defaultEncoding);
							}
							if (metaType.IsPlp)
							{
								WriteShort(65535, stateObj);
							}
							else
							{
								maxSize = ((num > num3) ? num : num3);
								if (maxSize == 0)
								{
									maxSize = ((!metaType.IsNCharType) ? 1 : 2);
								}
								WriteParameterVarLen(metaType, maxSize, isNull: false, stateObj);
							}
						}
						else if (metaType.SqlDbType == SqlDbType.Timestamp)
						{
							WriteParameterVarLen(metaType, 8, isNull: false, stateObj);
						}
						else
						{
							if (metaType.SqlDbType == SqlDbType.Udt)
							{
								byte[] array3 = null;
								Format format = Format.Native;
								int num4 = (IsKatmaiOrNewer ? int.MaxValue : 32767);
								if (!flag3)
								{
									if (obj is byte[] array4)
									{
										array3 = array4;
									}
									else if (obj is SqlBytes { Storage: var storage } sqlBytes)
									{
										switch (storage)
										{
										case StorageState.Buffer:
											array3 = sqlBytes.Buffer;
											break;
										case StorageState.Stream:
										case StorageState.UnmanagedBuffer:
											array3 = sqlBytes.Value;
											break;
										}
									}
									else
									{
										array3 = _connHandler.Connection.GetBytes(obj, out format, out maxSize);
									}
									num = array3.Length;
									if (num < 0 || (num >= num4 && maxSize != -1))
									{
										throw SQL.UDTInvalidSize(maxSize, num4);
									}
								}
								string[] array5 = SqlParameter.ParseTypeName(sqlParameter.UdtTypeName, isUdtTypeName: true);
								if (!ADP.IsEmpty(array5[0]) && 255 < array5[0].Length)
								{
									throw ADP.ArgumentOutOfRange("names");
								}
								if (!ADP.IsEmpty(array5[1]) && 255 < array5[^2].Length)
								{
									throw ADP.ArgumentOutOfRange("names");
								}
								if (255 < array5[2].Length)
								{
									throw ADP.ArgumentOutOfRange("names");
								}
								WriteUDTMetaData(obj, array5[0], array5[1], array5[2], stateObj);
								if (!flag3)
								{
									WriteUnsignedLong((ulong)array3.Length, stateObj);
									if (array3.Length != 0)
									{
										WriteInt(array3.Length, stateObj);
										stateObj.WriteByteArray(array3, array3.Length, 0);
									}
									WriteInt(0, stateObj);
								}
								else
								{
									WriteUnsignedLong(ulong.MaxValue, stateObj);
								}
								continue;
							}
							if (metaType.IsPlp)
							{
								if (metaType.SqlDbType != SqlDbType.Xml)
								{
									WriteShort(65535, stateObj);
								}
							}
							else if (!metaType.IsVarTime && metaType.SqlDbType != SqlDbType.Date)
							{
								maxSize = ((num > num2) ? num : num2);
								if (maxSize == 0 && IsYukonOrNewer)
								{
									maxSize = ((!metaType.IsNCharType) ? 1 : 2);
								}
								WriteParameterVarLen(metaType, maxSize, isNull: false, stateObj);
							}
						}
						if (metaType.SqlDbType == SqlDbType.Decimal)
						{
							if (b == 0)
							{
								if (_isShiloh)
								{
									stateObj.WriteByte(29);
								}
								else
								{
									stateObj.WriteByte(28);
								}
							}
							else
							{
								stateObj.WriteByte(b);
							}
							stateObj.WriteByte(b2);
						}
						else if (metaType.IsVarTime)
						{
							stateObj.WriteByte(sqlParameter.GetActualScale());
						}
						if (_isYukon && metaType.SqlDbType == SqlDbType.Xml)
						{
							if ((sqlParameter.XmlSchemaCollectionDatabase != null && sqlParameter.XmlSchemaCollectionDatabase != ADP.StrEmpty) || (sqlParameter.XmlSchemaCollectionOwningSchema != null && sqlParameter.XmlSchemaCollectionOwningSchema != ADP.StrEmpty) || (sqlParameter.XmlSchemaCollectionName != null && sqlParameter.XmlSchemaCollectionName != ADP.StrEmpty))
							{
								stateObj.WriteByte(1);
								if (sqlParameter.XmlSchemaCollectionDatabase != null && sqlParameter.XmlSchemaCollectionDatabase != ADP.StrEmpty)
								{
									int length = sqlParameter.XmlSchemaCollectionDatabase.Length;
									stateObj.WriteByte((byte)length);
									WriteString(sqlParameter.XmlSchemaCollectionDatabase, length, 0, stateObj);
								}
								else
								{
									stateObj.WriteByte(0);
								}
								if (sqlParameter.XmlSchemaCollectionOwningSchema != null && sqlParameter.XmlSchemaCollectionOwningSchema != ADP.StrEmpty)
								{
									int length = sqlParameter.XmlSchemaCollectionOwningSchema.Length;
									stateObj.WriteByte((byte)length);
									WriteString(sqlParameter.XmlSchemaCollectionOwningSchema, length, 0, stateObj);
								}
								else
								{
									stateObj.WriteByte(0);
								}
								if (sqlParameter.XmlSchemaCollectionName != null && sqlParameter.XmlSchemaCollectionName != ADP.StrEmpty)
								{
									int length = sqlParameter.XmlSchemaCollectionName.Length;
									WriteShort((short)length, stateObj);
									WriteString(sqlParameter.XmlSchemaCollectionName, length, 0, stateObj);
								}
								else
								{
									WriteShort(0, stateObj);
								}
							}
							else
							{
								stateObj.WriteByte(0);
							}
						}
						else if (_isShiloh && metaType.IsCharType)
						{
							SqlCollation sqlCollation = ((sqlParameter.Collation != null) ? sqlParameter.Collation : _defaultCollation);
							WriteUnsignedInt(sqlCollation.info, stateObj);
							stateObj.WriteByte(sqlCollation.sortId);
						}
						if (num3 == 0)
						{
							WriteParameterVarLen(metaType, num2, flag3, stateObj, flag5);
						}
						else
						{
							WriteParameterVarLen(metaType, num3, flag3, stateObj, flag5);
						}
						Task task = null;
						if (!flag3)
						{
							task = ((!flag4) ? WriteValue(obj, metaType, (byte)((!flag6) ? sqlParameter.GetActualScale() : 0), num2, num3, (!flag6) ? sqlParameter.Offset : 0, stateObj, (!flag6) ? sqlParameter.Size : 0, flag5) : WriteSqlValue(obj, metaType, num2, num3, sqlParameter.Offset, stateObj));
						}
						if (flag6)
						{
							task = WriteEncryptionMetadata(task, columnEncryptionParameterInfo, stateObj);
						}
						if (sync)
						{
							continue;
						}
						if (task == null)
						{
							task = stateObj.WaitForAccumulatedWrites();
						}
						if (task == null)
						{
							continue;
						}
						Task task2 = null;
						if (completion == null)
						{
							completion = new TaskCompletionSource<object>();
							task2 = completion.Task;
						}
						AsyncHelper.ContinueTask(task, completion, delegate
						{
							TdsExecuteRPC(cmd, rpcArray, timeout, inSchema, notificationRequest, stateObj, isCommandProc, sync, completion, ii, i + 1);
						}, _connHandler, delegate(Exception exc)
						{
							TdsExecuteRPC_OnFailure(exc, stateObj);
						});
						if (flag2)
						{
							task2.ContinueWith(delegate
							{
								_connHandler._parserLock.Release();
							}, TaskScheduler.Default);
							flag2 = false;
						}
						return task2;
					}
					if (ii < rpcArray.Length - 1)
					{
						if (_isYukon)
						{
							stateObj.WriteByte(byte.MaxValue);
						}
						else
						{
							stateObj.WriteByte(128);
						}
					}
				}
				Task task3 = stateObj.ExecuteFlush();
				if (task3 != null)
				{
					Task result = null;
					if (completion == null)
					{
						completion = new TaskCompletionSource<object>();
						result = completion.Task;
					}
					bool taskReleaseConnectionLock = flag2;
					task3.ContinueWith(delegate(Task tsk)
					{
						ExecuteFlushTaskCallback(tsk, stateObj, completion, taskReleaseConnectionLock);
					}, TaskScheduler.Default);
					flag2 = false;
					return result;
				}
			}
			catch (Exception e2)
			{
				FailureCleanup(stateObj, e2);
				throw;
			}
			FinalizeExecuteRPC(stateObj);
			if (completion != null)
			{
				completion.SetResult(null);
			}
			return null;
		}
		catch (Exception exception)
		{
			FinalizeExecuteRPC(stateObj);
			if (completion != null)
			{
				completion.SetException(exception);
				return null;
			}
			throw;
		}
		finally
		{
			if (flag2)
			{
				_connHandler._parserLock.Release();
			}
		}
	}

	private void WriteEnclaveInfo(TdsParserStateObject stateObj, byte[] enclavePackage)
	{
		if (TceVersionSupported >= 2)
		{
			if (enclavePackage != null)
			{
				WriteShort((short)enclavePackage.Length, stateObj);
				stateObj.WriteByteArray(enclavePackage, enclavePackage.Length, 0);
			}
			else
			{
				WriteShort(0, stateObj);
			}
		}
	}

	private void FinalizeExecuteRPC(TdsParserStateObject stateObj)
	{
		stateObj.SniContext = SniContext.Snix_Read;
		_asyncWrite = false;
	}

	private void TdsExecuteRPC_OnFailure(Exception exc, TdsParserStateObject stateObj)
	{
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			FailureCleanup(stateObj, exc);
		}
		catch (OutOfMemoryException)
		{
			_connHandler.DoomThisConnection();
			throw;
		}
		catch (StackOverflowException)
		{
			_connHandler.DoomThisConnection();
			throw;
		}
		catch (ThreadAbortException)
		{
			_connHandler.DoomThisConnection();
			throw;
		}
	}

	private void ExecuteFlushTaskCallback(Task tsk, TdsParserStateObject stateObj, TaskCompletionSource<object> completion, bool releaseConnectionLock)
	{
		try
		{
			FinalizeExecuteRPC(stateObj);
			if (tsk.Exception != null)
			{
				Exception exception = tsk.Exception.InnerException;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					FailureCleanup(stateObj, tsk.Exception);
				}
				catch (OutOfMemoryException exception2)
				{
					_connHandler.DoomThisConnection();
					completion.SetException(exception2);
					throw;
				}
				catch (StackOverflowException exception3)
				{
					_connHandler.DoomThisConnection();
					completion.SetException(exception3);
					throw;
				}
				catch (ThreadAbortException exception4)
				{
					_connHandler.DoomThisConnection();
					completion.SetException(exception4);
					throw;
				}
				catch (Exception ex)
				{
					exception = ex;
				}
				completion.SetException(exception);
			}
			else
			{
				completion.SetResult(null);
			}
		}
		finally
		{
			if (releaseConnectionLock)
			{
				_connHandler._parserLock.Release();
			}
		}
	}

	private void WriteParameterName(string parameterName, TdsParserStateObject stateObj)
	{
		if (!ADP.IsEmpty(parameterName))
		{
			int num = parameterName.Length & 0xFF;
			stateObj.WriteByte((byte)num);
			WriteString(parameterName, num, 0, stateObj);
		}
		else
		{
			stateObj.WriteByte(0);
		}
	}

	private void WriteSmiParameter(SqlParameter param, int paramIndex, bool sendDefault, TdsParserStateObject stateObj, bool advancedTraceIsOn)
	{
		ParameterPeekAheadValue peekAhead;
		SmiParameterMetaData smiParameterMetaData = param.MetaDataForSmi(out peekAhead);
		if (!_isKatmai)
		{
			MetaType metaTypeFromSqlDbType = MetaType.GetMetaTypeFromSqlDbType(smiParameterMetaData.SqlDbType, smiParameterMetaData.IsMultiValued);
			throw ADP.VersionDoesNotSupportDataType(metaTypeFromSqlDbType.TypeName);
		}
		object value;
		ExtendedClrTypeCode typeCode;
		if (sendDefault)
		{
			if (SqlDbType.Structured == smiParameterMetaData.SqlDbType && smiParameterMetaData.IsMultiValued)
			{
				value = __tvpEmptyValue;
				typeCode = ExtendedClrTypeCode.IEnumerableOfSqlDataRecord;
			}
			else
			{
				value = null;
				typeCode = ExtendedClrTypeCode.DBNull;
			}
		}
		else if (param.Direction == ParameterDirection.Output)
		{
			bool parameterIsSqlType = param.ParameterIsSqlType;
			param.Value = null;
			value = null;
			typeCode = ExtendedClrTypeCode.DBNull;
			param.ParameterIsSqlType = parameterIsSqlType;
		}
		else
		{
			value = param.GetCoercedValue();
			typeCode = MetaDataUtilsSmi.DetermineExtendedTypeCodeForUseWithSqlDbType(smiParameterMetaData.SqlDbType, smiParameterMetaData.IsMultiValued, value, null, 210uL);
		}
		int args = (sendDefault ? 1 : 0);
		if (advancedTraceIsOn)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParser.WriteSmiParameter|ADV> {0}, Sending parameter '{1}', default flag={2}, metadata:{3}", ObjectID, param.ParameterName, args, smiParameterMetaData.TraceString(3));
		}
		WriteSmiParameterMetaData(smiParameterMetaData, sendDefault, stateObj);
		TdsParameterSetter setters = new TdsParameterSetter(stateObj, smiParameterMetaData);
		ValueUtilsSmi.SetCompatibleValueV200(new SmiEventSink_Default(), setters, 0, smiParameterMetaData, value, typeCode, param.Offset, (0 < param.Size) ? param.Size : (-1), peekAhead);
	}

	private void WriteSmiParameterMetaData(SmiParameterMetaData metaData, bool sendDefault, TdsParserStateObject stateObj)
	{
		byte b = 0;
		if (ParameterDirection.Output == metaData.Direction || ParameterDirection.InputOutput == metaData.Direction)
		{
			b = (byte)(b | 1u);
		}
		if (sendDefault)
		{
			b = (byte)(b | 2u);
		}
		WriteParameterName(metaData.Name, stateObj);
		stateObj.WriteByte(b);
		WriteSmiTypeInfo(metaData, stateObj);
	}

	private void WriteSmiTypeInfo(SmiExtendedMetaData metaData, TdsParserStateObject stateObj)
	{
		checked
		{
			switch (metaData.SqlDbType)
			{
			case SqlDbType.BigInt:
				stateObj.WriteByte(38);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.Binary:
				stateObj.WriteByte(173);
				WriteUnsignedShort((ushort)metaData.MaxLength, stateObj);
				break;
			case SqlDbType.Bit:
				stateObj.WriteByte(104);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.Char:
				stateObj.WriteByte(175);
				WriteUnsignedShort((ushort)metaData.MaxLength, stateObj);
				WriteUnsignedInt(_defaultCollation.info, stateObj);
				stateObj.WriteByte(_defaultCollation.sortId);
				break;
			case SqlDbType.DateTime:
				stateObj.WriteByte(111);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.Decimal:
				stateObj.WriteByte(108);
				stateObj.WriteByte((byte)MetaType.MetaDecimal.FixedLength);
				stateObj.WriteByte(unchecked((byte)((metaData.Precision == 0) ? 1 : metaData.Precision)));
				stateObj.WriteByte(metaData.Scale);
				break;
			case SqlDbType.Float:
				stateObj.WriteByte(109);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.Image:
				stateObj.WriteByte(165);
				WriteUnsignedShort(ushort.MaxValue, stateObj);
				break;
			case SqlDbType.Int:
				stateObj.WriteByte(38);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.Money:
				stateObj.WriteByte(110);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.NChar:
				stateObj.WriteByte(239);
				WriteUnsignedShort((ushort)(metaData.MaxLength * 2), stateObj);
				WriteUnsignedInt(_defaultCollation.info, stateObj);
				stateObj.WriteByte(_defaultCollation.sortId);
				break;
			case SqlDbType.NText:
				stateObj.WriteByte(231);
				WriteUnsignedShort(ushort.MaxValue, stateObj);
				WriteUnsignedInt(_defaultCollation.info, stateObj);
				stateObj.WriteByte(_defaultCollation.sortId);
				break;
			case SqlDbType.NVarChar:
				stateObj.WriteByte(231);
				if (-1 == metaData.MaxLength)
				{
					WriteUnsignedShort(ushort.MaxValue, stateObj);
				}
				else
				{
					WriteUnsignedShort((ushort)(metaData.MaxLength * 2), stateObj);
				}
				WriteUnsignedInt(_defaultCollation.info, stateObj);
				stateObj.WriteByte(_defaultCollation.sortId);
				break;
			case SqlDbType.Real:
				stateObj.WriteByte(109);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.UniqueIdentifier:
				stateObj.WriteByte(36);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.SmallDateTime:
				stateObj.WriteByte(111);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.SmallInt:
				stateObj.WriteByte(38);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.SmallMoney:
				stateObj.WriteByte(110);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.Text:
				stateObj.WriteByte(167);
				WriteUnsignedShort(ushort.MaxValue, stateObj);
				WriteUnsignedInt(_defaultCollation.info, stateObj);
				stateObj.WriteByte(_defaultCollation.sortId);
				break;
			case SqlDbType.Timestamp:
				stateObj.WriteByte(173);
				WriteShort((int)metaData.MaxLength, stateObj);
				break;
			case SqlDbType.TinyInt:
				stateObj.WriteByte(38);
				stateObj.WriteByte((byte)metaData.MaxLength);
				break;
			case SqlDbType.VarBinary:
				stateObj.WriteByte(165);
				WriteUnsignedShort(unchecked((ushort)metaData.MaxLength), stateObj);
				break;
			case SqlDbType.VarChar:
				stateObj.WriteByte(167);
				WriteUnsignedShort(unchecked((ushort)metaData.MaxLength), stateObj);
				WriteUnsignedInt(_defaultCollation.info, stateObj);
				stateObj.WriteByte(_defaultCollation.sortId);
				break;
			case SqlDbType.Variant:
				stateObj.WriteByte(98);
				WriteInt((int)metaData.MaxLength, stateObj);
				break;
			case SqlDbType.Xml:
				stateObj.WriteByte(241);
				if (ADP.IsEmpty(metaData.TypeSpecificNamePart1) && ADP.IsEmpty(metaData.TypeSpecificNamePart2) && ADP.IsEmpty(metaData.TypeSpecificNamePart3))
				{
					stateObj.WriteByte(0);
					break;
				}
				stateObj.WriteByte(1);
				WriteIdentifier(metaData.TypeSpecificNamePart1, stateObj);
				WriteIdentifier(metaData.TypeSpecificNamePart2, stateObj);
				WriteIdentifierWithShortLength(metaData.TypeSpecificNamePart3, stateObj);
				break;
			case SqlDbType.Udt:
				stateObj.WriteByte(240);
				WriteIdentifier(metaData.TypeSpecificNamePart1, stateObj);
				WriteIdentifier(metaData.TypeSpecificNamePart2, stateObj);
				WriteIdentifier(metaData.TypeSpecificNamePart3, stateObj);
				break;
			case SqlDbType.Structured:
				if (metaData.IsMultiValued)
				{
					WriteTvpTypeInfo(metaData, stateObj);
				}
				break;
			case SqlDbType.Date:
				stateObj.WriteByte(40);
				break;
			case SqlDbType.Time:
				stateObj.WriteByte(41);
				stateObj.WriteByte(metaData.Scale);
				break;
			case SqlDbType.DateTime2:
				stateObj.WriteByte(42);
				stateObj.WriteByte(metaData.Scale);
				break;
			case SqlDbType.DateTimeOffset:
				stateObj.WriteByte(43);
				stateObj.WriteByte(metaData.Scale);
				break;
			case (SqlDbType)24:
			case (SqlDbType)26:
			case (SqlDbType)27:
			case (SqlDbType)28:
				break;
			}
		}
	}

	private void WriteTvpTypeInfo(SmiExtendedMetaData metaData, TdsParserStateObject stateObj)
	{
		stateObj.WriteByte(243);
		WriteIdentifier(metaData.TypeSpecificNamePart1, stateObj);
		WriteIdentifier(metaData.TypeSpecificNamePart2, stateObj);
		WriteIdentifier(metaData.TypeSpecificNamePart3, stateObj);
		if (metaData.FieldMetaData.Count == 0)
		{
			WriteUnsignedShort(ushort.MaxValue, stateObj);
		}
		else
		{
			WriteUnsignedShort(checked((ushort)metaData.FieldMetaData.Count), stateObj);
			SmiDefaultFieldsProperty smiDefaultFieldsProperty = (SmiDefaultFieldsProperty)metaData.ExtendedProperties[SmiPropertySelector.DefaultFields];
			for (int i = 0; i < metaData.FieldMetaData.Count; i++)
			{
				WriteTvpColumnMetaData(metaData.FieldMetaData[i], smiDefaultFieldsProperty[i], stateObj);
			}
			WriteTvpOrderUnique(metaData, stateObj);
		}
		stateObj.WriteByte(0);
	}

	private void WriteTvpColumnMetaData(SmiExtendedMetaData md, bool isDefault, TdsParserStateObject stateObj)
	{
		if (SqlDbType.Timestamp == md.SqlDbType)
		{
			WriteUnsignedInt(80u, stateObj);
		}
		else
		{
			WriteUnsignedInt(0u, stateObj);
		}
		ushort num = 1;
		if (isDefault)
		{
			num = (ushort)(num | 0x200u);
		}
		WriteUnsignedShort(num, stateObj);
		WriteSmiTypeInfo(md, stateObj);
		WriteIdentifier(null, stateObj);
	}

	private void WriteTvpOrderUnique(SmiExtendedMetaData metaData, TdsParserStateObject stateObj)
	{
		SmiOrderProperty smiOrderProperty = (SmiOrderProperty)metaData.ExtendedProperties[SmiPropertySelector.SortOrder];
		SmiUniqueKeyProperty smiUniqueKeyProperty = (SmiUniqueKeyProperty)metaData.ExtendedProperties[SmiPropertySelector.UniqueKey];
		List<TdsOrderUnique> list = new List<TdsOrderUnique>(metaData.FieldMetaData.Count);
		for (int i = 0; i < metaData.FieldMetaData.Count; i++)
		{
			byte b = 0;
			SmiOrderProperty.SmiColumnOrder smiColumnOrder = smiOrderProperty[i];
			if (smiColumnOrder.Order == SortOrder.Ascending)
			{
				b = 1;
			}
			else if (SortOrder.Descending == smiColumnOrder.Order)
			{
				b = 2;
			}
			if (smiUniqueKeyProperty[i])
			{
				b = (byte)(b | 4u);
			}
			if (b != 0)
			{
				list.Add(new TdsOrderUnique(checked((short)(i + 1)), b));
			}
		}
		if (0 >= list.Count)
		{
			return;
		}
		stateObj.WriteByte(16);
		WriteShort(list.Count, stateObj);
		foreach (TdsOrderUnique item in list)
		{
			WriteShort(item.ColumnOrdinal, stateObj);
			stateObj.WriteByte(item.Flags);
		}
	}

	internal Task WriteBulkCopyDone(TdsParserStateObject stateObj)
	{
		if (State != TdsParserState.OpenNotLoggedIn && State != TdsParserState.OpenLoggedIn)
		{
			throw ADP.ClosedConnectionError();
		}
		stateObj.WriteByte(253);
		WriteShort(0, stateObj);
		WriteShort(0, stateObj);
		WriteInt(0, stateObj);
		stateObj._pendingData = true;
		stateObj._messageStatus = 0;
		return stateObj.WritePacket(1);
	}

	internal void LoadColumnEncryptionKeys(_SqlMetaDataSet metadataCollection, SqlConnection connection, SqlCommand command = null)
	{
		if (!_serverSupportsColumnEncryption || !ShouldEncryptValuesForBulkCopy())
		{
			return;
		}
		for (int i = 0; i < metadataCollection.Length; i++)
		{
			if (metadataCollection[i] != null)
			{
				_SqlMetaData sqlMetaData = metadataCollection[i];
				if (sqlMetaData.isEncrypted)
				{
					SqlSecurityUtility.DecryptSymmetricKey(sqlMetaData.cipherMD, connection, command);
				}
			}
		}
	}

	internal void WriteEncryptionEntries(ref SqlTceCipherInfoTable cekTable, TdsParserStateObject stateObj)
	{
		for (int i = 0; i < cekTable.Size; i++)
		{
			WriteInt(cekTable[i].DatabaseId, stateObj);
			WriteInt(cekTable[i].CekId, stateObj);
			WriteInt(cekTable[i].CekVersion, stateObj);
			stateObj.WriteByteArray(cekTable[i].CekMdVersion, 8, 0);
			stateObj.WriteByte(0);
		}
	}

	internal void WriteCekTable(_SqlMetaDataSet metadataCollection, TdsParserStateObject stateObj)
	{
		if (_serverSupportsColumnEncryption)
		{
			if (metadataCollection.cekTable == null || !ShouldEncryptValuesForBulkCopy())
			{
				WriteShort(0, stateObj);
				return;
			}
			SqlTceCipherInfoTable cekTable = metadataCollection.cekTable;
			ushort v = (ushort)cekTable.Size;
			WriteShort(v, stateObj);
			WriteEncryptionEntries(ref cekTable, stateObj);
		}
	}

	internal void WriteTceUserTypeAndTypeInfo(SqlMetaDataPriv mdPriv, TdsParserStateObject stateObj)
	{
		WriteInt(0, stateObj);
		stateObj.WriteByte(mdPriv.tdsType);
		switch (mdPriv.type)
		{
		case SqlDbType.Decimal:
			WriteTokenLength(mdPriv.tdsType, mdPriv.length, stateObj);
			stateObj.WriteByte(mdPriv.precision);
			stateObj.WriteByte(mdPriv.scale);
			return;
		case SqlDbType.Time:
		case SqlDbType.DateTime2:
		case SqlDbType.DateTimeOffset:
			stateObj.WriteByte(mdPriv.scale);
			return;
		case SqlDbType.Date:
			return;
		}
		WriteTokenLength(mdPriv.tdsType, mdPriv.length, stateObj);
		if (mdPriv.metaType.IsCharType && _isShiloh)
		{
			WriteUnsignedInt(mdPriv.collation.info, stateObj);
			stateObj.WriteByte(mdPriv.collation.sortId);
		}
	}

	internal void WriteCryptoMetadata(_SqlMetaData md, TdsParserStateObject stateObj)
	{
		if (_serverSupportsColumnEncryption && md.isEncrypted && ShouldEncryptValuesForBulkCopy())
		{
			WriteShort(md.cipherMD.CekTableOrdinal, stateObj);
			WriteTceUserTypeAndTypeInfo(md.baseTI, stateObj);
			stateObj.WriteByte(md.cipherMD.CipherAlgorithmId);
			if (md.cipherMD.CipherAlgorithmId == 0)
			{
				stateObj.WriteByte((byte)md.cipherMD.CipherAlgorithmName.Length);
				WriteString(md.cipherMD.CipherAlgorithmName, stateObj);
			}
			stateObj.WriteByte(md.cipherMD.EncryptionType);
			stateObj.WriteByte(md.cipherMD.NormalizationRuleVersion);
		}
	}

	internal void WriteBulkCopyMetaData(_SqlMetaDataSet metadataCollection, int count, TdsParserStateObject stateObj)
	{
		if (State != TdsParserState.OpenNotLoggedIn && State != TdsParserState.OpenLoggedIn)
		{
			throw ADP.ClosedConnectionError();
		}
		stateObj.WriteByte(129);
		WriteShort(count, stateObj);
		WriteCekTable(metadataCollection, stateObj);
		for (int i = 0; i < metadataCollection.Length; i++)
		{
			if (metadataCollection[i] == null)
			{
				continue;
			}
			_SqlMetaData sqlMetaData = metadataCollection[i];
			if (IsYukonOrNewer)
			{
				WriteInt(0, stateObj);
			}
			else
			{
				WriteShort(0, stateObj);
			}
			ushort num = (ushort)(sqlMetaData.Updatability << 2);
			num = (ushort)(num | (sqlMetaData.IsNullable ? 1u : 0u));
			num = (ushort)(num | (sqlMetaData.IsIdentity ? 16u : 0u));
			if (_serverSupportsColumnEncryption && ShouldEncryptValuesForBulkCopy())
			{
				num = (ushort)(num | (sqlMetaData.isEncrypted ? 2048u : 0u));
			}
			WriteShort(num, stateObj);
			switch (sqlMetaData.type)
			{
			case SqlDbType.Decimal:
				stateObj.WriteByte(sqlMetaData.tdsType);
				WriteTokenLength(sqlMetaData.tdsType, sqlMetaData.length, stateObj);
				stateObj.WriteByte(sqlMetaData.precision);
				stateObj.WriteByte(sqlMetaData.scale);
				break;
			case SqlDbType.Xml:
				stateObj.WriteByteArray(s_xmlMetadataSubstituteSequence, s_xmlMetadataSubstituteSequence.Length, 0);
				break;
			case SqlDbType.Udt:
				stateObj.WriteByte(165);
				WriteTokenLength(165, sqlMetaData.length, stateObj);
				break;
			case SqlDbType.Date:
				stateObj.WriteByte(sqlMetaData.tdsType);
				break;
			case SqlDbType.Time:
			case SqlDbType.DateTime2:
			case SqlDbType.DateTimeOffset:
				stateObj.WriteByte(sqlMetaData.tdsType);
				stateObj.WriteByte(sqlMetaData.scale);
				break;
			default:
				stateObj.WriteByte(sqlMetaData.tdsType);
				WriteTokenLength(sqlMetaData.tdsType, sqlMetaData.length, stateObj);
				if (sqlMetaData.metaType.IsCharType && _isShiloh)
				{
					WriteUnsignedInt(sqlMetaData.collation.info, stateObj);
					stateObj.WriteByte(sqlMetaData.collation.sortId);
				}
				break;
			}
			if (sqlMetaData.metaType.IsLong && !sqlMetaData.metaType.IsPlp)
			{
				WriteShort(sqlMetaData.tableName.Length, stateObj);
				WriteString(sqlMetaData.tableName, stateObj);
			}
			WriteCryptoMetadata(sqlMetaData, stateObj);
			stateObj.WriteByte((byte)sqlMetaData.column.Length);
			WriteString(sqlMetaData.column, stateObj);
		}
	}

	internal bool ShouldEncryptValuesForBulkCopy()
	{
		if (_connHandler != null && _connHandler.ConnectionOptions != null && SqlConnectionColumnEncryptionSetting.Enabled == _connHandler.ConnectionOptions.ColumnEncryptionSetting)
		{
			return true;
		}
		return false;
	}

	internal object EncryptColumnValue(object value, SqlMetaDataPriv metadata, string column, TdsParserStateObject stateObj, bool isDataFeed, bool isSqlType)
	{
		if (isDataFeed)
		{
			SQL.StreamNotSupportOnEncryptedColumn(column);
		}
		int num;
		switch (metadata.baseTI.metaType.NullableType)
		{
		case 34:
		case 165:
		case 173:
			num = (isSqlType ? ((SqlBinary)value).Length : ((byte[])value).Length);
			if (metadata.baseTI.length > 0 && num > metadata.baseTI.length)
			{
				num = metadata.baseTI.length;
			}
			break;
		case 36:
			num = 16;
			break;
		case 35:
		case 167:
		case 175:
		{
			if (_defaultEncoding == null)
			{
				ThrowUnsupportedCollationEncountered(null);
			}
			string s = (isSqlType ? ((SqlString)value).Value : ((string)value));
			num = _defaultEncoding.GetByteCount(s);
			if (metadata.baseTI.length > 0 && num > metadata.baseTI.length)
			{
				num = metadata.baseTI.length;
			}
			break;
		}
		case 99:
		case 231:
		case 239:
			num = (isSqlType ? ((SqlString)value).Value.Length : ((string)value).Length) * 2;
			if (metadata.baseTI.length > 0 && num > metadata.baseTI.length)
			{
				num = metadata.baseTI.length;
			}
			break;
		default:
			num = metadata.baseTI.length;
			break;
		}
		byte[] plainText = ((!isSqlType) ? SerializeUnencryptedValue(value, metadata.baseTI.metaType, metadata.baseTI.scale, num, 0, isDataFeed, metadata.cipherMD.NormalizationRuleVersion, stateObj) : SerializeUnencryptedSqlValue(value, metadata.baseTI.metaType, num, 0, metadata.cipherMD.NormalizationRuleVersion, stateObj));
		return SqlSecurityUtility.EncryptWithKey(plainText, metadata.cipherMD, _connHandler.Connection, null);
	}

	internal Task WriteBulkCopyValue(object value, SqlMetaDataPriv metadata, TdsParserStateObject stateObj, bool isSqlType, bool isDataFeed, bool isNull)
	{
		Encoding defaultEncoding = _defaultEncoding;
		SqlCollation defaultCollation = _defaultCollation;
		int defaultCodePage = _defaultCodePage;
		int defaultLCID = _defaultLCID;
		Task result = null;
		Task task = null;
		if (State != TdsParserState.OpenNotLoggedIn && State != TdsParserState.OpenLoggedIn)
		{
			throw ADP.ClosedConnectionError();
		}
		try
		{
			if (metadata.encoding != null)
			{
				_defaultEncoding = metadata.encoding;
			}
			if (metadata.collation != null)
			{
				if ((metadata.collation.info & 0x4000000) == 67108864)
				{
					_defaultEncoding = Encoding.UTF8;
				}
				_defaultCollation = metadata.collation;
				_defaultLCID = _defaultCollation.LCID;
			}
			_defaultCodePage = metadata.codePage;
			MetaType metaType = metadata.metaType;
			int num = 0;
			int num2 = 0;
			if (isNull)
			{
				if (metaType.IsPlp && (metaType.NullableType != 240 || metaType.IsLong))
				{
					WriteLong(-1L, stateObj);
				}
				else if (!metaType.IsFixed && !metaType.IsLong && !metaType.IsVarTime)
				{
					WriteShort(65535, stateObj);
				}
				else
				{
					stateObj.WriteByte(0);
				}
				return result;
			}
			if (!isDataFeed)
			{
				switch (metaType.NullableType)
				{
				case 34:
				case 165:
				case 173:
				case 240:
					num = (isSqlType ? ((SqlBinary)value).Length : ((byte[])value).Length);
					break;
				case 36:
					num = 16;
					break;
				case 35:
				case 167:
				case 175:
				{
					if (_defaultEncoding == null)
					{
						ThrowUnsupportedCollationEncountered(null);
					}
					string text = null;
					text = ((!isSqlType) ? ((string)value) : ((SqlString)value).Value);
					num = text.Length;
					num2 = _defaultEncoding.GetByteCount(text);
					break;
				}
				case 99:
				case 231:
				case 239:
					num = (isSqlType ? ((SqlString)value).Value.Length : ((string)value).Length) * 2;
					break;
				case 241:
					if (value is XmlReader)
					{
						value = MetaType.GetStringFromXml((XmlReader)value);
					}
					num = (isSqlType ? ((SqlString)value).Value.Length : ((string)value).Length) * 2;
					break;
				default:
					num = metadata.length;
					break;
				}
			}
			if (metaType.IsLong)
			{
				switch (metaType.SqlDbType)
				{
				case SqlDbType.Image:
				case SqlDbType.NText:
				case SqlDbType.Text:
					stateObj.WriteByteArray(s_longDataHeader, s_longDataHeader.Length, 0);
					WriteTokenLength(metadata.tdsType, (num2 == 0) ? num : num2, stateObj);
					break;
				case SqlDbType.NVarChar:
				case SqlDbType.VarBinary:
				case SqlDbType.VarChar:
				case SqlDbType.Xml:
				case SqlDbType.Udt:
					WriteUnsignedLong(18446744073709551614uL, stateObj);
					break;
				}
			}
			else
			{
				WriteTokenLength(metadata.tdsType, (num2 == 0) ? num : num2, stateObj);
			}
			if (isSqlType)
			{
				task = WriteSqlValue(value, metaType, num, num2, 0, stateObj);
			}
			else if (metaType.SqlDbType != SqlDbType.Udt || metaType.IsLong)
			{
				task = WriteValue(value, metaType, metadata.scale, num, num2, 0, stateObj, metadata.length, isDataFeed);
				if (task == null && _asyncWrite)
				{
					task = stateObj.WaitForAccumulatedWrites();
				}
			}
			else
			{
				WriteShort(num, stateObj);
				task = stateObj.WriteByteArray((byte[])value, num, 0);
			}
			if (task != null)
			{
				result = WriteBulkCopyValueSetupContinuation(task, defaultEncoding, defaultCollation, defaultCodePage, defaultLCID);
			}
		}
		finally
		{
			if (task == null)
			{
				_defaultEncoding = defaultEncoding;
				_defaultCollation = defaultCollation;
				_defaultCodePage = defaultCodePage;
				_defaultLCID = defaultLCID;
			}
		}
		return result;
	}

	private Task WriteBulkCopyValueSetupContinuation(Task internalWriteTask, Encoding saveEncoding, SqlCollation saveCollation, int saveCodePage, int saveLCID)
	{
		return internalWriteTask.ContinueWith(delegate(Task t)
		{
			_defaultEncoding = saveEncoding;
			_defaultCollation = saveCollation;
			_defaultCodePage = saveCodePage;
			_defaultLCID = saveLCID;
			return t;
		}, TaskScheduler.Default).Unwrap();
	}

	private void WriteMarsHeaderData(TdsParserStateObject stateObj, SqlInternalTransaction transaction)
	{
		WriteShort(2, stateObj);
		if (transaction != null && transaction.TransactionId != 0L)
		{
			WriteLong(transaction.TransactionId, stateObj);
			WriteInt(stateObj.IncrementAndObtainOpenResultCount(transaction), stateObj);
		}
		else
		{
			WriteLong(_retainedTransactionId, stateObj);
			WriteInt(stateObj.IncrementAndObtainOpenResultCount(null), stateObj);
		}
	}

	private int GetNotificationHeaderSize(SqlNotificationRequest notificationRequest)
	{
		if (notificationRequest != null)
		{
			string userData = notificationRequest.UserData;
			string options = notificationRequest.Options;
			int timeout = notificationRequest.Timeout;
			if (userData == null)
			{
				throw ADP.ArgumentNull("CallbackId");
			}
			if (65535 < userData.Length)
			{
				throw ADP.ArgumentOutOfRange("CallbackId");
			}
			if (options == null)
			{
				throw ADP.ArgumentNull("Service");
			}
			if (65535 < options.Length)
			{
				throw ADP.ArgumentOutOfRange("Service");
			}
			if (-1 > timeout)
			{
				throw ADP.ArgumentOutOfRange("Timeout");
			}
			int num = 8 + userData.Length * 2 + 2 + options.Length * 2;
			if (timeout > 0)
			{
				num += 4;
			}
			return num;
		}
		return 0;
	}

	private void WriteQueryNotificationHeaderData(SqlNotificationRequest notificationRequest, TdsParserStateObject stateObj)
	{
		string userData = notificationRequest.UserData;
		string options = notificationRequest.Options;
		int timeout = notificationRequest.Timeout;
		SqlClientEventSource.Log.TryNotificationTraceEvent("<sc.TdsParser.WriteQueryNotificationHeader|DEP> NotificationRequest: userData: '{0}', options: '{1}', timeout: '{2}'", notificationRequest.UserData, notificationRequest.Options, notificationRequest.Timeout);
		WriteShort(1, stateObj);
		WriteShort(userData.Length * 2, stateObj);
		WriteString(userData, stateObj);
		WriteShort(options.Length * 2, stateObj);
		WriteString(options, stateObj);
		if (timeout > 0)
		{
			WriteInt(timeout, stateObj);
		}
	}

	private void WriteTraceHeaderData(TdsParserStateObject stateObj)
	{
		ActivityCorrelator.ActivityId current = ActivityCorrelator.Current;
		WriteShort(3, stateObj);
		Guid id = current.Id;
		stateObj.WriteByteArray(id.ToByteArray(), 16, 0);
		WriteUnsignedInt(current.Sequence, stateObj);
		SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.WriteTraceHeaderData|INFO> ActivityID {0}", current);
	}

	private void WriteRPCBatchHeaders(TdsParserStateObject stateObj, SqlNotificationRequest notificationRequest)
	{
		int notificationHeaderSize = GetNotificationHeaderSize(notificationRequest);
		int v = (IncludeTraceHeader ? (22 + notificationHeaderSize + 26) : (22 + notificationHeaderSize));
		WriteInt(v, stateObj);
		WriteInt(18, stateObj);
		WriteMarsHeaderData(stateObj, CurrentTransaction);
		if (notificationHeaderSize != 0)
		{
			WriteInt(notificationHeaderSize, stateObj);
			WriteQueryNotificationHeaderData(notificationRequest, stateObj);
		}
		if (IncludeTraceHeader)
		{
			WriteInt(26, stateObj);
			WriteTraceHeaderData(stateObj);
		}
	}

	private void WriteTokenLength(byte token, int length, TdsParserStateObject stateObj)
	{
		int num = 0;
		if (_isYukon)
		{
			if (240 == token)
			{
				num = 8;
			}
			else if (token == 241)
			{
				num = 8;
			}
		}
		if (num == 0)
		{
			switch (token & 0x30)
			{
			case 48:
				num = 0;
				break;
			case 16:
				num = 0;
				break;
			case 0:
			case 32:
				num = (((token & 0x80) == 0) ? (((token & 0xCu) != 0) ? 1 : 4) : 2);
				break;
			}
			switch (num)
			{
			case 1:
				stateObj.WriteByte((byte)length);
				break;
			case 2:
				WriteShort(length, stateObj);
				break;
			case 4:
				WriteInt(length, stateObj);
				break;
			case 8:
				WriteShort(65535, stateObj);
				break;
			}
		}
	}

	private bool IsBOMNeeded(MetaType type, object value)
	{
		if (type.NullableType == 241)
		{
			Type type2 = value.GetType();
			if (type2 == typeof(SqlString))
			{
				if (!((SqlString)value).IsNull && ((SqlString)value).Value.Length > 0 && (((SqlString)value).Value[0] & 0xFF) != 255)
				{
					return true;
				}
			}
			else if (type2 == typeof(string) && ((string)value).Length > 0)
			{
				if (value != null && (((string)value)[0] & 0xFF) != 255)
				{
					return true;
				}
			}
			else if (type2 == typeof(SqlXml))
			{
				if (!((SqlXml)value).IsNull)
				{
					return true;
				}
			}
			else if (type2 == typeof(XmlDataFeed))
			{
				return true;
			}
		}
		return false;
	}

	private Task GetTerminationTask(Task unterminatedWriteTask, object value, MetaType type, int actualLength, TdsParserStateObject stateObj, bool isDataFeed)
	{
		if (type.IsPlp && (actualLength > 0 || isDataFeed))
		{
			if (unterminatedWriteTask == null)
			{
				WriteInt(0, stateObj);
				return null;
			}
			return AsyncHelper.CreateContinuationTask(unterminatedWriteTask, WriteInt, 0, stateObj, _connHandler);
		}
		return unterminatedWriteTask;
	}

	private Task WriteSqlValue(object value, MetaType type, int actualLength, int codePageByteSize, int offset, TdsParserStateObject stateObj)
	{
		return GetTerminationTask(WriteUnterminatedSqlValue(value, type, actualLength, codePageByteSize, offset, stateObj), value, type, actualLength, stateObj, isDataFeed: false);
	}

	private Task WriteUnterminatedSqlValue(object value, MetaType type, int actualLength, int codePageByteSize, int offset, TdsParserStateObject stateObj)
	{
		switch (type.NullableType)
		{
		case 109:
			if (type.FixedLength == 4)
			{
				WriteFloat(((SqlSingle)value).Value, stateObj);
			}
			else
			{
				WriteDouble(((SqlDouble)value).Value, stateObj);
			}
			break;
		case 34:
		case 165:
		case 173:
			if (type.IsPlp)
			{
				WriteInt(actualLength, stateObj);
			}
			if (value is SqlBinary)
			{
				return stateObj.WriteByteArray(((SqlBinary)value).Value, actualLength, offset, canAccumulate: false);
			}
			return stateObj.WriteByteArray(((SqlBytes)value).Value, actualLength, offset, canAccumulate: false);
		case 36:
		{
			byte[] b = ((SqlGuid)value).ToByteArray();
			stateObj.WriteByteArray(b, actualLength, 0);
			break;
		}
		case 104:
			if (((SqlBoolean)value).Value)
			{
				stateObj.WriteByte(1);
			}
			else
			{
				stateObj.WriteByte(0);
			}
			break;
		case 38:
			if (type.FixedLength == 1)
			{
				stateObj.WriteByte(((SqlByte)value).Value);
			}
			else if (type.FixedLength == 2)
			{
				WriteShort(((SqlInt16)value).Value, stateObj);
			}
			else if (type.FixedLength == 4)
			{
				WriteInt(((SqlInt32)value).Value, stateObj);
			}
			else
			{
				WriteLong(((SqlInt64)value).Value, stateObj);
			}
			break;
		case 35:
		case 167:
		case 175:
			if (type.IsPlp)
			{
				WriteInt(codePageByteSize, stateObj);
			}
			if (value is SqlChars)
			{
				string s = new string(((SqlChars)value).Value);
				return WriteEncodingChar(s, actualLength, offset, _defaultEncoding, stateObj, canAccumulate: false);
			}
			return WriteEncodingChar(((SqlString)value).Value, actualLength, offset, _defaultEncoding, stateObj, canAccumulate: false);
		case 99:
		case 231:
		case 239:
		case 241:
			if (type.IsPlp)
			{
				if (IsBOMNeeded(type, value))
				{
					WriteInt(actualLength + 2, stateObj);
					WriteShort(65279, stateObj);
				}
				else
				{
					WriteInt(actualLength, stateObj);
				}
			}
			if (actualLength != 0)
			{
				actualLength >>= 1;
			}
			if (value is SqlChars)
			{
				return WriteCharArray(((SqlChars)value).Value, actualLength, offset, stateObj, canAccumulate: false);
			}
			return WriteString(((SqlString)value).Value, actualLength, offset, stateObj, canAccumulate: false);
		case 108:
			WriteSqlDecimal((SqlDecimal)value, stateObj);
			break;
		case 111:
		{
			SqlDateTime sqlDateTime = (SqlDateTime)value;
			if (type.FixedLength == 4)
			{
				if (0 > sqlDateTime.DayTicks || sqlDateTime.DayTicks > 65535)
				{
					throw SQL.SmallDateTimeOverflow(sqlDateTime.ToString());
				}
				WriteShort(sqlDateTime.DayTicks, stateObj);
				WriteShort(sqlDateTime.TimeTicks / SqlDateTime.SQLTicksPerMinute, stateObj);
			}
			else
			{
				WriteInt(sqlDateTime.DayTicks, stateObj);
				WriteInt(sqlDateTime.TimeTicks, stateObj);
			}
			break;
		}
		case 110:
			WriteSqlMoney((SqlMoney)value, type.FixedLength, stateObj);
			break;
		case 240:
			throw SQL.UDTUnexpectedResult(value.GetType().AssemblyQualifiedName);
		}
		return null;
	}

	private async Task WriteXmlFeed(XmlDataFeed feed, TdsParserStateObject stateObj, bool needBom, Encoding encoding, int size)
	{
		byte[] preambleToStrip = null;
		if (!needBom)
		{
			preambleToStrip = encoding.GetPreamble();
		}
		ConstrainedTextWriter writer = new ConstrainedTextWriter(new StreamWriter(new TdsOutputStream(this, stateObj, preambleToStrip), encoding), size);
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.CloseOutput = false;
		xmlWriterSettings.ConformanceLevel = ConformanceLevel.Fragment;
		if (_asyncWrite)
		{
			xmlWriterSettings.Async = true;
		}
		XmlWriter ww = XmlWriter.Create(writer, xmlWriterSettings);
		if (feed._source.ReadState == ReadState.Initial)
		{
			feed._source.Read();
		}
		while (!feed._source.EOF && !writer.IsComplete)
		{
			if (feed._source.NodeType == XmlNodeType.XmlDeclaration)
			{
				feed._source.Read();
			}
			else if (_asyncWrite)
			{
				await ww.WriteNodeAsync(feed._source, defattr: true).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				ww.WriteNode(feed._source, defattr: true);
			}
		}
		if (_asyncWrite)
		{
			await ww.FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		else
		{
			ww.Flush();
		}
	}

	private async Task WriteTextFeed(TextDataFeed feed, Encoding encoding, bool needBom, TdsParserStateObject stateObj, int size)
	{
		char[] inBuff = new char[4096];
		encoding = encoding ?? new UnicodeEncoding(bigEndian: false, byteOrderMark: false);
		ConstrainedTextWriter writer = new ConstrainedTextWriter(new StreamWriter(new TdsOutputStream(this, stateObj, null), encoding), size);
		if (needBom)
		{
			if (_asyncWrite)
			{
				await writer.WriteAsync('\ufeff').ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				writer.Write('\ufeff');
			}
		}
		int nWritten = 0;
		do
		{
			int nRead = ((!_asyncWrite) ? feed._source.ReadBlock(inBuff, 0, 4096) : (await feed._source.ReadBlockAsync(inBuff, 0, 4096).ConfigureAwait(continueOnCapturedContext: false)));
			if (nRead == 0)
			{
				break;
			}
			if (_asyncWrite)
			{
				await writer.WriteAsync(inBuff, 0, nRead).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				writer.Write(inBuff, 0, nRead);
			}
			nWritten += nRead;
		}
		while (!writer.IsComplete);
		if (_asyncWrite)
		{
			await writer.FlushAsync().ConfigureAwait(continueOnCapturedContext: false);
		}
		else
		{
			writer.Flush();
		}
	}

	private async Task WriteStreamFeed(StreamDataFeed feed, TdsParserStateObject stateObj, int len)
	{
		TdsOutputStream output = new TdsOutputStream(this, stateObj, null);
		byte[] buff = new byte[4096];
		int nWritten = 0;
		do
		{
			int num = 4096;
			if (len > 0 && nWritten + num > len)
			{
				num = len - nWritten;
			}
			int nRead = ((!_asyncWrite) ? feed._source.Read(buff, 0, num) : (await feed._source.ReadAsync(buff, 0, num).ConfigureAwait(continueOnCapturedContext: false)));
			if (nRead == 0)
			{
				break;
			}
			if (_asyncWrite)
			{
				await output.WriteAsync(buff, 0, nRead).ConfigureAwait(continueOnCapturedContext: false);
			}
			else
			{
				output.Write(buff, 0, nRead);
			}
			nWritten += nRead;
		}
		while (len <= 0 || nWritten < len);
	}

	private Task NullIfCompletedWriteTask(Task task)
	{
		if (task == null)
		{
			return null;
		}
		return task.Status switch
		{
			TaskStatus.RanToCompletion => null, 
			TaskStatus.Faulted => throw task.Exception.InnerException, 
			TaskStatus.Canceled => throw SQL.OperationCancelled(), 
			_ => task, 
		};
	}

	private Task WriteValue(object value, MetaType type, byte scale, int actualLength, int encodingByteSize, int offset, TdsParserStateObject stateObj, int paramSize, bool isDataFeed)
	{
		return GetTerminationTask(WriteUnterminatedValue(value, type, scale, actualLength, encodingByteSize, offset, stateObj, paramSize, isDataFeed), value, type, actualLength, stateObj, isDataFeed);
	}

	private Task WriteUnterminatedValue(object value, MetaType type, byte scale, int actualLength, int encodingByteSize, int offset, TdsParserStateObject stateObj, int paramSize, bool isDataFeed)
	{
		switch (type.NullableType)
		{
		case 109:
			if (type.FixedLength == 4)
			{
				WriteFloat((float)value, stateObj);
			}
			else
			{
				WriteDouble((double)value, stateObj);
			}
			break;
		case 34:
		case 165:
		case 173:
		case 240:
			if (isDataFeed)
			{
				return NullIfCompletedWriteTask(WriteStreamFeed((StreamDataFeed)value, stateObj, paramSize));
			}
			if (type.IsPlp)
			{
				WriteInt(actualLength, stateObj);
			}
			return stateObj.WriteByteArray((byte[])value, actualLength, offset, canAccumulate: false);
		case 36:
		{
			byte[] b = ((Guid)value).ToByteArray();
			stateObj.WriteByteArray(b, actualLength, 0);
			break;
		}
		case 104:
			if ((bool)value)
			{
				stateObj.WriteByte(1);
			}
			else
			{
				stateObj.WriteByte(0);
			}
			break;
		case 38:
			if (type.FixedLength == 1)
			{
				stateObj.WriteByte((byte)value);
			}
			else if (type.FixedLength == 2)
			{
				WriteShort((short)value, stateObj);
			}
			else if (type.FixedLength == 4)
			{
				WriteInt((int)value, stateObj);
			}
			else
			{
				WriteLong((long)value, stateObj);
			}
			break;
		case 35:
		case 167:
		case 175:
			if (isDataFeed)
			{
				if (!(value is TextDataFeed feed2))
				{
					return NullIfCompletedWriteTask(WriteXmlFeed((XmlDataFeed)value, stateObj, needBom: true, _defaultEncoding, paramSize));
				}
				return NullIfCompletedWriteTask(WriteTextFeed(feed2, _defaultEncoding, needBom: false, stateObj, paramSize));
			}
			if (type.IsPlp)
			{
				WriteInt(encodingByteSize, stateObj);
			}
			if (value is byte[])
			{
				return stateObj.WriteByteArray((byte[])value, actualLength, 0, canAccumulate: false);
			}
			return WriteEncodingChar((string)value, actualLength, offset, _defaultEncoding, stateObj, canAccumulate: false);
		case 99:
		case 231:
		case 239:
		case 241:
			if (isDataFeed)
			{
				if (!(value is TextDataFeed feed))
				{
					return NullIfCompletedWriteTask(WriteXmlFeed((XmlDataFeed)value, stateObj, IsBOMNeeded(type, value), Encoding.Unicode, paramSize));
				}
				return NullIfCompletedWriteTask(WriteTextFeed(feed, null, IsBOMNeeded(type, value), stateObj, paramSize));
			}
			if (type.IsPlp)
			{
				if (IsBOMNeeded(type, value))
				{
					WriteInt(actualLength + 2, stateObj);
					WriteShort(65279, stateObj);
				}
				else
				{
					WriteInt(actualLength, stateObj);
				}
			}
			if (value is byte[])
			{
				return stateObj.WriteByteArray((byte[])value, actualLength, 0, canAccumulate: false);
			}
			actualLength >>= 1;
			return WriteString((string)value, actualLength, offset, stateObj, canAccumulate: false);
		case 108:
			WriteDecimal((decimal)value, stateObj);
			break;
		case 111:
		{
			TdsDateTime tdsDateTime = MetaType.FromDateTime((DateTime)value, (byte)type.FixedLength);
			if (type.FixedLength == 4)
			{
				if (0 > tdsDateTime.days || tdsDateTime.days > 65535)
				{
					throw SQL.SmallDateTimeOverflow(MetaType.ToDateTime(tdsDateTime.days, tdsDateTime.time, 4).ToString(CultureInfo.InvariantCulture));
				}
				WriteShort(tdsDateTime.days, stateObj);
				WriteShort(tdsDateTime.time, stateObj);
			}
			else
			{
				WriteInt(tdsDateTime.days, stateObj);
				WriteInt(tdsDateTime.time, stateObj);
			}
			break;
		}
		case 110:
			WriteCurrency((decimal)value, type.FixedLength, stateObj);
			break;
		case 40:
			WriteDate((DateTime)value, stateObj);
			break;
		case 41:
			if (scale > 7)
			{
				throw SQL.TimeScaleValueOutOfRange(scale);
			}
			WriteTime((TimeSpan)value, scale, actualLength, stateObj);
			break;
		case 42:
			if (scale > 7)
			{
				throw SQL.TimeScaleValueOutOfRange(scale);
			}
			WriteDateTime2((DateTime)value, scale, actualLength, stateObj);
			break;
		case 43:
			WriteDateTimeOffset((DateTimeOffset)value, scale, actualLength, stateObj);
			break;
		}
		return null;
	}

	private Task WriteEncryptionMetadata(Task terminatedWriteTask, SqlColumnEncryptionInputParameterInfo columnEncryptionParameterInfo, TdsParserStateObject stateObj)
	{
		if (terminatedWriteTask == null)
		{
			WriteEncryptionMetadata(columnEncryptionParameterInfo, stateObj);
			return null;
		}
		return AsyncHelper.CreateContinuationTask(terminatedWriteTask, WriteEncryptionMetadata, columnEncryptionParameterInfo, stateObj, _connHandler);
	}

	private void WriteEncryptionMetadata(SqlColumnEncryptionInputParameterInfo columnEncryptionParameterInfo, TdsParserStateObject stateObj)
	{
		WriteSmiTypeInfo(columnEncryptionParameterInfo.ParameterMetadata, stateObj);
		stateObj.WriteByteArray(columnEncryptionParameterInfo.SerializedWireFormat, columnEncryptionParameterInfo.SerializedWireFormat.Length, 0);
	}

	private byte[] SerializeUnencryptedValue(object value, MetaType type, byte scale, int actualLength, int offset, bool isDataFeed, byte normalizationVersion, TdsParserStateObject stateObj)
	{
		if (normalizationVersion != 1)
		{
			throw SQL.UnsupportedNormalizationVersion(normalizationVersion);
		}
		switch (type.NullableType)
		{
		case 109:
			if (type.FixedLength == 4)
			{
				return SerializeFloat((float)value);
			}
			return SerializeDouble((double)value);
		case 34:
		case 165:
		case 173:
		case 240:
		{
			byte[] array3 = new byte[actualLength];
			Buffer.BlockCopy((byte[])value, offset, array3, 0, actualLength);
			return array3;
		}
		case 36:
			return ((Guid)value).ToByteArray();
		case 104:
			return SerializeLong(((bool)value) ? 1 : 0, stateObj);
		case 38:
			if (type.FixedLength == 1)
			{
				return SerializeLong((byte)value, stateObj);
			}
			if (type.FixedLength == 2)
			{
				return SerializeLong((short)value, stateObj);
			}
			if (type.FixedLength == 4)
			{
				return SerializeLong((int)value, stateObj);
			}
			return SerializeLong((long)value, stateObj);
		case 35:
		case 167:
		case 175:
			if (value is byte[])
			{
				byte[] array = new byte[actualLength];
				Buffer.BlockCopy((byte[])value, 0, array, 0, actualLength);
				return array;
			}
			return SerializeEncodingChar((string)value, actualLength, offset, _defaultEncoding);
		case 99:
		case 231:
		case 239:
		case 241:
			if (value is byte[])
			{
				byte[] array2 = new byte[actualLength];
				Buffer.BlockCopy((byte[])value, 0, array2, 0, actualLength);
				return array2;
			}
			actualLength >>= 1;
			return SerializeString((string)value, actualLength, offset);
		case 108:
			return SerializeDecimal((decimal)value, stateObj);
		case 111:
		{
			TdsDateTime tdsDateTime = MetaType.FromDateTime((DateTime)value, (byte)type.FixedLength);
			if (type.FixedLength == 4)
			{
				if (0 > tdsDateTime.days || tdsDateTime.days > 65535)
				{
					throw SQL.SmallDateTimeOverflow(MetaType.ToDateTime(tdsDateTime.days, tdsDateTime.time, 4).ToString(CultureInfo.InvariantCulture));
				}
				if (stateObj._bIntBytes == null)
				{
					stateObj._bIntBytes = new byte[4];
				}
				byte[] bIntBytes = stateObj._bIntBytes;
				int num = 0;
				byte[] src = SerializeShort(tdsDateTime.days, stateObj);
				Buffer.BlockCopy(src, 0, bIntBytes, num, 2);
				num += 2;
				src = SerializeShort(tdsDateTime.time, stateObj);
				Buffer.BlockCopy(src, 0, bIntBytes, num, 2);
				return bIntBytes;
			}
			if (stateObj._bLongBytes == null)
			{
				stateObj._bLongBytes = new byte[8];
			}
			byte[] bLongBytes = stateObj._bLongBytes;
			int num2 = 0;
			byte[] src2 = SerializeInt(tdsDateTime.days, stateObj);
			Buffer.BlockCopy(src2, 0, bLongBytes, num2, 4);
			num2 += 4;
			src2 = SerializeInt(tdsDateTime.time, stateObj);
			Buffer.BlockCopy(src2, 0, bLongBytes, num2, 4);
			return bLongBytes;
		}
		case 110:
			return SerializeCurrency((decimal)value, type.FixedLength, stateObj);
		case 40:
			return SerializeDate((DateTime)value);
		case 41:
			if (scale > 7)
			{
				throw SQL.TimeScaleValueOutOfRange(scale);
			}
			return SerializeTime((TimeSpan)value, scale, actualLength);
		case 42:
			if (scale > 7)
			{
				throw SQL.TimeScaleValueOutOfRange(scale);
			}
			return SerializeDateTime2((DateTime)value, scale, actualLength);
		case 43:
			if (scale > 7)
			{
				throw SQL.TimeScaleValueOutOfRange(scale);
			}
			return SerializeDateTimeOffset((DateTimeOffset)value, scale, actualLength);
		default:
			throw SQL.UnsupportedDatatypeEncryption(type.TypeName);
		}
	}

	private byte[] SerializeUnencryptedSqlValue(object value, MetaType type, int actualLength, int offset, byte normalizationVersion, TdsParserStateObject stateObj)
	{
		if (normalizationVersion != 1)
		{
			throw SQL.UnsupportedNormalizationVersion(normalizationVersion);
		}
		switch (type.NullableType)
		{
		case 109:
			if (type.FixedLength == 4)
			{
				return SerializeFloat(((SqlSingle)value).Value);
			}
			return SerializeDouble(((SqlDouble)value).Value);
		case 34:
		case 165:
		case 173:
		{
			byte[] array = new byte[actualLength];
			if (value is SqlBinary sqlBinary)
			{
				Buffer.BlockCopy(sqlBinary.Value, offset, array, 0, actualLength);
			}
			else
			{
				Buffer.BlockCopy(((SqlBytes)value).Value, offset, array, 0, actualLength);
			}
			return array;
		}
		case 36:
			return ((SqlGuid)value).ToByteArray();
		case 104:
			return SerializeLong(((SqlBoolean)value).Value ? 1 : 0, stateObj);
		case 38:
			if (type.FixedLength == 1)
			{
				return SerializeLong(((SqlByte)value).Value, stateObj);
			}
			if (type.FixedLength == 2)
			{
				return SerializeLong(((SqlInt16)value).Value, stateObj);
			}
			if (type.FixedLength == 4)
			{
				return SerializeLong(((SqlInt32)value).Value, stateObj);
			}
			return SerializeLong(((SqlInt64)value).Value, stateObj);
		case 35:
		case 167:
		case 175:
			if (value is SqlChars)
			{
				string s = new string(((SqlChars)value).Value);
				return SerializeEncodingChar(s, actualLength, offset, _defaultEncoding);
			}
			return SerializeEncodingChar(((SqlString)value).Value, actualLength, offset, _defaultEncoding);
		case 99:
		case 231:
		case 239:
		case 241:
			if (actualLength != 0)
			{
				actualLength >>= 1;
			}
			if (value is SqlChars)
			{
				return SerializeCharArray(((SqlChars)value).Value, actualLength, offset);
			}
			return SerializeString(((SqlString)value).Value, actualLength, offset);
		case 108:
			return SerializeSqlDecimal((SqlDecimal)value, stateObj);
		case 111:
		{
			SqlDateTime sqlDateTime = (SqlDateTime)value;
			if (type.FixedLength == 4)
			{
				if (0 > sqlDateTime.DayTicks || sqlDateTime.DayTicks > 65535)
				{
					throw SQL.SmallDateTimeOverflow(sqlDateTime.ToString());
				}
				if (stateObj._bIntBytes == null)
				{
					stateObj._bIntBytes = new byte[4];
				}
				byte[] bIntBytes = stateObj._bIntBytes;
				int num = 0;
				byte[] src = SerializeShort(sqlDateTime.DayTicks, stateObj);
				Buffer.BlockCopy(src, 0, bIntBytes, num, 2);
				num += 2;
				src = SerializeShort(sqlDateTime.TimeTicks / SqlDateTime.SQLTicksPerMinute, stateObj);
				Buffer.BlockCopy(src, 0, bIntBytes, num, 2);
				return bIntBytes;
			}
			if (stateObj._bLongBytes == null)
			{
				stateObj._bLongBytes = new byte[8];
			}
			byte[] bLongBytes = stateObj._bLongBytes;
			int num2 = 0;
			byte[] src2 = SerializeInt(sqlDateTime.DayTicks, stateObj);
			Buffer.BlockCopy(src2, 0, bLongBytes, num2, 4);
			num2 += 4;
			src2 = SerializeInt(sqlDateTime.TimeTicks, stateObj);
			Buffer.BlockCopy(src2, 0, bLongBytes, num2, 4);
			return bLongBytes;
		}
		case 110:
			return SerializeSqlMoney((SqlMoney)value, type.FixedLength, stateObj);
		default:
			throw SQL.UnsupportedDatatypeEncryption(type.TypeName);
		}
	}

	internal void WriteParameterVarLen(MetaType type, int size, bool isNull, TdsParserStateObject stateObj, bool unknownLength = false)
	{
		if (type.IsLong)
		{
			if (isNull)
			{
				if (type.IsPlp)
				{
					WriteLong(-1L, stateObj);
				}
				else
				{
					WriteInt(-1, stateObj);
				}
			}
			else if (type.NullableType == 241 || unknownLength)
			{
				WriteUnsignedLong(18446744073709551614uL, stateObj);
			}
			else if (type.IsPlp)
			{
				WriteLong(size, stateObj);
			}
			else
			{
				WriteInt(size, stateObj);
			}
		}
		else if (type.IsVarTime)
		{
			if (isNull)
			{
				stateObj.WriteByte(0);
			}
			else
			{
				stateObj.WriteByte((byte)size);
			}
		}
		else if (!type.IsFixed)
		{
			if (isNull)
			{
				WriteShort(65535, stateObj);
			}
			else
			{
				WriteShort(size, stateObj);
			}
		}
		else if (isNull)
		{
			stateObj.WriteByte(0);
		}
		else
		{
			stateObj.WriteByte((byte)((uint)type.FixedLength & 0xFFu));
		}
	}

	private bool TryReadPlpUnicodeCharsChunk(char[] buff, int offst, int len, TdsParserStateObject stateObj, out int charsRead)
	{
		if (stateObj._longlenleft == 0L)
		{
			charsRead = 0;
			return true;
		}
		charsRead = len;
		if (stateObj._longlenleft >> 1 < (ulong)len)
		{
			charsRead = (int)(stateObj._longlenleft >> 1);
		}
		for (int i = 0; i < charsRead; i++)
		{
			if (!stateObj.TryReadChar(out buff[offst + i]))
			{
				return false;
			}
		}
		stateObj._longlenleft -= (ulong)((long)charsRead << 1);
		return true;
	}

	internal int ReadPlpUnicodeChars(ref char[] buff, int offst, int len, TdsParserStateObject stateObj)
	{
		if (!TryReadPlpUnicodeChars(ref buff, offst, len, stateObj, out var totalCharsRead))
		{
			throw SQL.SynchronousCallMayNotPend();
		}
		return totalCharsRead;
	}

	internal bool TryReadPlpUnicodeChars(ref char[] buff, int offst, int len, TdsParserStateObject stateObj, out int totalCharsRead)
	{
		int num = 0;
		int num2 = 0;
		if (stateObj._longlen == 0L)
		{
			totalCharsRead = 0;
			return true;
		}
		num2 = len;
		if (buff == null && stateObj._longlen != 18446744073709551614uL)
		{
			buff = new char[Math.Min((int)stateObj._longlen, len)];
		}
		if (stateObj._longlenleft == 0L)
		{
			if (!stateObj.TryReadPlpLength(returnPlpNullIfNull: false, out var _))
			{
				totalCharsRead = 0;
				return false;
			}
			if (stateObj._longlenleft == 0L)
			{
				totalCharsRead = 0;
				return true;
			}
		}
		totalCharsRead = 0;
		while (num2 > 0)
		{
			num = (int)Math.Min(stateObj._longlenleft + 1 >> 1, (ulong)num2);
			if (buff == null || buff.Length < offst + num)
			{
				char[] array = new char[offst + num];
				if (buff != null)
				{
					Buffer.BlockCopy(buff, 0, array, 0, offst * 2);
				}
				buff = array;
			}
			if (num > 0)
			{
				if (!TryReadPlpUnicodeCharsChunk(buff, offst, num, stateObj, out num))
				{
					return false;
				}
				num2 -= num;
				offst += num;
				totalCharsRead += num;
			}
			if (stateObj._longlenleft == 1 && num2 > 0)
			{
				if (!stateObj.TryReadByte(out var value))
				{
					return false;
				}
				stateObj._longlenleft--;
				if (!stateObj.TryReadPlpLength(returnPlpNullIfNull: false, out var _))
				{
					return false;
				}
				if (!stateObj.TryReadByte(out var value2))
				{
					return false;
				}
				stateObj._longlenleft--;
				buff[offst] = (char)(((value2 & 0xFF) << 8) + (value & 0xFF));
				offst = checked(offst + 1);
				num++;
				num2--;
				totalCharsRead++;
			}
			if (stateObj._longlenleft == 0L && !stateObj.TryReadPlpLength(returnPlpNullIfNull: false, out var _))
			{
				return false;
			}
			if (stateObj._longlenleft == 0L)
			{
				break;
			}
		}
		return true;
	}

	internal int ReadPlpAnsiChars(ref char[] buff, int offst, int len, SqlMetaDataPriv metadata, TdsParserStateObject stateObj)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		if (stateObj._longlen == 0L)
		{
			return 0;
		}
		num2 = len;
		if (stateObj._longlenleft == 0L)
		{
			stateObj.ReadPlpLength(returnPlpNullIfNull: false);
			if (stateObj._longlenleft == 0L)
			{
				stateObj._plpdecoder = null;
				return 0;
			}
		}
		if (stateObj._plpdecoder == null)
		{
			Encoding encoding = metadata.encoding;
			if (encoding == null)
			{
				if (_defaultEncoding == null)
				{
					ThrowUnsupportedCollationEncountered(stateObj);
				}
				encoding = _defaultEncoding;
			}
			stateObj._plpdecoder = encoding.GetDecoder();
		}
		while (num2 > 0)
		{
			num3 = (int)Math.Min(stateObj._longlenleft, (ulong)num2);
			if (stateObj._bTmp == null || stateObj._bTmp.Length < num3)
			{
				stateObj._bTmp = new byte[num3];
			}
			num3 = stateObj.ReadPlpBytesChunk(stateObj._bTmp, 0, num3);
			num = stateObj._plpdecoder.GetChars(stateObj._bTmp, 0, num3, buff, offst);
			num2 -= num;
			offst += num;
			num4 += num;
			if (stateObj._longlenleft == 0L)
			{
				stateObj.ReadPlpLength(returnPlpNullIfNull: false);
			}
			if (stateObj._longlenleft == 0L)
			{
				stateObj._plpdecoder = null;
				break;
			}
		}
		return num4;
	}

	internal ulong SkipPlpValue(ulong cb, TdsParserStateObject stateObj)
	{
		if (!TrySkipPlpValue(cb, stateObj, out var totalBytesSkipped))
		{
			throw SQL.SynchronousCallMayNotPend();
		}
		return totalBytesSkipped;
	}

	internal bool TrySkipPlpValue(ulong cb, TdsParserStateObject stateObj, out ulong totalBytesSkipped)
	{
		totalBytesSkipped = 0uL;
		if (stateObj._longlenleft == 0L && !stateObj.TryReadPlpLength(returnPlpNullIfNull: false, out var _))
		{
			return false;
		}
		while (totalBytesSkipped < cb && stateObj._longlenleft != 0)
		{
			int num = (int)((stateObj._longlenleft <= int.MaxValue) ? stateObj._longlenleft : int.MaxValue);
			num = ((cb - totalBytesSkipped < (ulong)num) ? ((int)(cb - totalBytesSkipped)) : num);
			if (!stateObj.TrySkipBytes(num))
			{
				return false;
			}
			stateObj._longlenleft -= (ulong)num;
			totalBytesSkipped += (ulong)num;
			if (stateObj._longlenleft == 0L && !stateObj.TryReadPlpLength(returnPlpNullIfNull: false, out var _))
			{
				return false;
			}
		}
		return true;
	}

	internal ulong PlpBytesLeft(TdsParserStateObject stateObj)
	{
		if (stateObj._longlen != 0L && stateObj._longlenleft == 0L)
		{
			stateObj.ReadPlpLength(returnPlpNullIfNull: false);
		}
		return stateObj._longlenleft;
	}

	internal bool TryPlpBytesLeft(TdsParserStateObject stateObj, out ulong left)
	{
		if (stateObj._longlen != 0L && stateObj._longlenleft == 0L && !stateObj.TryReadPlpLength(returnPlpNullIfNull: false, out left))
		{
			return false;
		}
		left = stateObj._longlenleft;
		return true;
	}

	internal ulong PlpBytesTotalLength(TdsParserStateObject stateObj)
	{
		if (stateObj._longlen == 18446744073709551614uL)
		{
			return ulong.MaxValue;
		}
		if (stateObj._longlen == ulong.MaxValue)
		{
			return 0uL;
		}
		return stateObj._longlen;
	}

	internal string TraceString()
	{
		return string.Format(null, "\n\t         _physicalStateObj = {0}\n\t         _pMarsPhysicalConObj = {1}\n\t         _state = {2}\n\t         _server = {3}\n\t         _fResetConnection = {4}\n\t         _defaultCollation = {5}\n\t         _defaultCodePage = {6}\n\t         _defaultLCID = {7}\n\t         _defaultEncoding = {8}\n\t         _encryptionOption = {9}\n\t         _currentTransaction = {10}\n\t         _pendingTransaction = {11}\n\t         _retainedTransactionId = {12}\n\t         _nonTransactedOpenResultCount = {13}\n\t         _connHandler = {14}\n\t         _fMARS = {15}\n\t         _sessionPool = {16}\n\t         _isShiloh = {17}\n\t         _isShilohSP1 = {18}\n\t         _isYukon = {19}\n\t         _sniSpnBuffer = {20}\n\t         _errors = {21}\n\t         _warnings = {22}\n\t         _attentionErrors = {23}\n\t         _attentionWarnings = {24}\n\t         _statistics = {25}\n\t         _statisticsIsInTransaction = {26}\n\t         _fPreserveTransaction = {27}         _fParallel = {28}", (_physicalStateObj == null) ? "(null)" : _physicalStateObj.ObjectID.ToString((IFormatProvider?)null), (_pMarsPhysicalConObj == null) ? "(null)" : _pMarsPhysicalConObj.ObjectID.ToString((IFormatProvider?)null), _state, _server, _fResetConnection ? bool.TrueString : bool.FalseString, (_defaultCollation == null) ? "(null)" : _defaultCollation.TraceString(), _defaultCodePage, _defaultLCID, TraceObjectClass(_defaultEncoding), _encryptionOption, (_currentTransaction == null) ? "(null)" : _currentTransaction.TraceString(), (_pendingTransaction == null) ? "(null)" : _pendingTransaction.TraceString(), _retainedTransactionId, _nonTransactedOpenResultCount, (_connHandler == null) ? "(null)" : _connHandler.ObjectID.ToString((IFormatProvider?)null), _fMARS ? bool.TrueString : bool.FalseString, (_sessionPool == null) ? "(null)" : _sessionPool.TraceString(), _isShiloh ? bool.TrueString : bool.FalseString, _isShilohSP1 ? bool.TrueString : bool.FalseString, _isYukon ? bool.TrueString : bool.FalseString, (_sniSpnBuffer == null) ? "(null)" : _sniSpnBuffer.Length.ToString((IFormatProvider?)null), (_physicalStateObj != null) ? "(null)" : _physicalStateObj.ErrorCount.ToString((IFormatProvider?)null), (_physicalStateObj != null) ? "(null)" : _physicalStateObj.WarningCount.ToString((IFormatProvider?)null), (_physicalStateObj != null) ? "(null)" : _physicalStateObj.PreAttentionErrorCount.ToString((IFormatProvider?)null), (_physicalStateObj != null) ? "(null)" : _physicalStateObj.PreAttentionWarningCount.ToString((IFormatProvider?)null), (_statistics == null) ? bool.TrueString : bool.FalseString, _statisticsIsInTransaction ? bool.TrueString : bool.FalseString, _fPreserveTransaction ? bool.TrueString : bool.FalseString, (_connHandler == null) ? "(null)" : _connHandler.ConnectionOptions.MultiSubnetFailover.ToString(null), (_connHandler == null) ? "(null)" : _connHandler.ConnectionOptions.TransparentNetworkIPResolution.ToString(null));
	}

	private string TraceObjectClass(object instance)
	{
		if (instance == null)
		{
			return "(null)";
		}
		return instance.GetType().ToString();
	}

	private static IntPtr ClientCertificateDelegate(IntPtr ptrContext)
	{
		GCHandle gCHandle = GCHandle.FromIntPtr(ptrContext);
		try
		{
			ClientCertificateRetrievalCallback clientCertificateRetrievalCallback = (ClientCertificateRetrievalCallback)gCHandle.Target;
			return clientCertificateRetrievalCallback()?.Handle ?? IntPtr.Zero;
		}
		catch
		{
			return IntPtr.Zero;
		}
	}
}
