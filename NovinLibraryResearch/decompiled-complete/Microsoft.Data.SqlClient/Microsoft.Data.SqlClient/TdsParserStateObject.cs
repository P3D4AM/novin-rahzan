using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Common;

namespace Microsoft.Data.SqlClient;

internal sealed class TdsParserStateObject
{
	private sealed class TimeoutState
	{
		public const int Stopped = 0;

		public const int Running = 1;

		public const int ExpiredAsync = 2;

		public const int ExpiredSync = 3;

		private readonly int _value;

		public int IdentityValue => _value;

		public TimeoutState(int value)
		{
			_value = value;
		}
	}

	private struct NullBitmap
	{
		private byte[] _nullBitmap;

		private int _columnsCount;

		internal bool TryInitialize(TdsParserStateObject stateObj, int columnsCount)
		{
			_columnsCount = columnsCount;
			int num = (columnsCount + 7) / 8;
			if (_nullBitmap == null || _nullBitmap.Length != num)
			{
				_nullBitmap = new byte[num];
			}
			if (!stateObj.TryReadByteArray(_nullBitmap, 0, _nullBitmap.Length))
			{
				return false;
			}
			SqlClientEventSource.Log.TryAdvancedTraceEvent("TdsParserStateObject.NullBitmap.Initialize | INFO | ADV | State Object Id {0}, NBCROW bitmap received, column count = {1}", stateObj.ObjectID, columnsCount);
			SqlClientEventSource.Log.TryAdvancedTraceBinEvent("TdsParserStateObject.NullBitmap.Initialize | INFO | ADV | State Object Id {0}, NBCROW bitmap data. Null Bitmap {1}, Null bitmap length: {2}", stateObj.ObjectID, _nullBitmap, (ushort)_nullBitmap.Length);
			return true;
		}

		internal bool ReferenceEquals(NullBitmap obj)
		{
			return _nullBitmap == obj._nullBitmap;
		}

		internal NullBitmap Clone()
		{
			NullBitmap result = default(NullBitmap);
			result._nullBitmap = ((_nullBitmap == null) ? null : ((byte[])_nullBitmap.Clone()));
			result._columnsCount = _columnsCount;
			return result;
		}

		internal void Clean()
		{
			_columnsCount = 0;
		}

		internal bool IsGuaranteedNull(int columnOrdinal)
		{
			if (_columnsCount == 0)
			{
				return false;
			}
			byte b = (byte)(1 << (columnOrdinal & 7));
			byte b2 = _nullBitmap[columnOrdinal >> 3];
			return (b & b2) != 0;
		}
	}

	private class PacketData
	{
		public byte[] Buffer;

		public int Read;
	}

	private class StateSnapshot
	{
		private List<PacketData> _snapshotInBuffs;

		private int _snapshotInBuffCurrent;

		private int _snapshotInBytesUsed;

		private int _snapshotInBytesPacket;

		private bool _snapshotPendingData;

		private bool _snapshotErrorTokenReceived;

		private bool _snapshotHasOpenResult;

		private bool _snapshotReceivedColumnMetadata;

		private bool _snapshotAttentionReceived;

		private byte _snapshotMessageStatus;

		private NullBitmap _snapshotNullBitmapInfo;

		private ulong _snapshotLongLen;

		private ulong _snapshotLongLenLeft;

		private _SqlMetaDataSet _snapshotCleanupMetaData;

		private _SqlMetaDataSetCollection _snapshotCleanupAltMetaDataSetArray;

		private readonly TdsParserStateObject _stateObj;

		public StateSnapshot(TdsParserStateObject state)
		{
			_snapshotInBuffs = new List<PacketData>();
			_stateObj = state;
		}

		internal void CloneNullBitmapInfo()
		{
			if (_stateObj._nullBitmapInfo.ReferenceEquals(_snapshotNullBitmapInfo))
			{
				_stateObj._nullBitmapInfo = _stateObj._nullBitmapInfo.Clone();
			}
		}

		internal void CloneCleanupAltMetaDataSetArray()
		{
			if (_stateObj._cleanupAltMetaDataSetArray != null && _snapshotCleanupAltMetaDataSetArray == _stateObj._cleanupAltMetaDataSetArray)
			{
				_stateObj._cleanupAltMetaDataSetArray = (_SqlMetaDataSetCollection)_stateObj._cleanupAltMetaDataSetArray.Clone();
			}
		}

		internal void PushBuffer(byte[] buffer, int read)
		{
			PacketData packetData = new PacketData();
			packetData.Buffer = buffer;
			packetData.Read = read;
			_snapshotInBuffs.Add(packetData);
		}

		internal bool Replay()
		{
			if (_snapshotInBuffCurrent < _snapshotInBuffs.Count)
			{
				PacketData packetData = _snapshotInBuffs[_snapshotInBuffCurrent];
				_stateObj._inBuff = packetData.Buffer;
				_stateObj._inBytesUsed = 0;
				_stateObj._inBytesRead = packetData.Read;
				_snapshotInBuffCurrent++;
				return true;
			}
			return false;
		}

		internal void Snap()
		{
			_snapshotInBuffs.Clear();
			_snapshotInBuffCurrent = 0;
			_snapshotInBytesUsed = _stateObj._inBytesUsed;
			_snapshotInBytesPacket = _stateObj._inBytesPacket;
			_snapshotPendingData = _stateObj._pendingData;
			_snapshotErrorTokenReceived = _stateObj._errorTokenReceived;
			_snapshotMessageStatus = _stateObj._messageStatus;
			_snapshotNullBitmapInfo = _stateObj._nullBitmapInfo;
			_snapshotLongLen = _stateObj._longlen;
			_snapshotLongLenLeft = _stateObj._longlenleft;
			_snapshotCleanupMetaData = _stateObj._cleanupMetaData;
			_snapshotCleanupAltMetaDataSetArray = _stateObj._cleanupAltMetaDataSetArray;
			_snapshotHasOpenResult = _stateObj._hasOpenResult;
			_snapshotReceivedColumnMetadata = _stateObj._receivedColMetaData;
			_snapshotAttentionReceived = _stateObj._attentionReceived;
			PushBuffer(_stateObj._inBuff, _stateObj._inBytesRead);
		}

		internal void ResetSnapshotState()
		{
			_snapshotInBuffCurrent = 0;
			Replay();
			_stateObj._inBytesUsed = _snapshotInBytesUsed;
			_stateObj._inBytesPacket = _snapshotInBytesPacket;
			_stateObj._pendingData = _snapshotPendingData;
			_stateObj._errorTokenReceived = _snapshotErrorTokenReceived;
			_stateObj._messageStatus = _snapshotMessageStatus;
			_stateObj._nullBitmapInfo = _snapshotNullBitmapInfo;
			_stateObj._cleanupMetaData = _snapshotCleanupMetaData;
			_stateObj._cleanupAltMetaDataSetArray = _snapshotCleanupAltMetaDataSetArray;
			if (!_stateObj._hasOpenResult && _snapshotHasOpenResult)
			{
				_stateObj.IncrementAndObtainOpenResultCount(_stateObj._executedUnderTransaction);
			}
			else if (_stateObj._hasOpenResult && !_snapshotHasOpenResult)
			{
				_stateObj.DecrementOpenResultCount();
			}
			_stateObj._receivedColMetaData = _snapshotReceivedColumnMetadata;
			_stateObj._attentionReceived = _snapshotAttentionReceived;
			_stateObj._bTmpRead = 0;
			_stateObj._partialHeaderBytesRead = 0;
			_stateObj._longlen = _snapshotLongLen;
			_stateObj._longlenleft = _snapshotLongLenLeft;
			_stateObj._snapshotReplay = true;
		}

		internal void PrepareReplay()
		{
			ResetSnapshotState();
		}
	}

	private const int AttentionTimeoutSeconds = 5;

	private const long CheckConnectionWindow = 50000L;

	private static int _objectTypeCount;

	internal readonly int _objectID = Interlocked.Increment(ref _objectTypeCount);

	private readonly TdsParser _parser;

	private SNIHandle _sessionHandle;

	private readonly WeakReference _owner = new WeakReference(null);

	internal SqlDataReader.SharedState _readerState;

	private int _activateCount;

	internal readonly int _inputHeaderLen = 8;

	internal readonly int _outputHeaderLen = 8;

	internal byte[] _outBuff;

	internal int _outBytesUsed = 8;

	private byte[] _inBuff;

	internal int _inBytesUsed;

	internal int _inBytesRead;

	internal int _inBytesPacket;

	internal int _spid;

	internal byte _outputMessageType;

	internal byte _messageStatus;

	internal byte _outputPacketNumber = 1;

	internal uint _outputPacketCount;

	internal bool _pendingData;

	internal volatile bool _fResetEventOwned;

	internal volatile bool _fResetConnectionSent;

	internal bool _errorTokenReceived;

	internal bool _bulkCopyOpperationInProgress;

	internal bool _bulkCopyWriteTimeout;

	private SNIPacket _sniPacket;

	internal SNIPacket _sniAsyncAttnPacket;

	private WritePacketCache _writePacketCache = new WritePacketCache();

	private Dictionary<IntPtr, SNIPacket> _pendingWritePackets = new Dictionary<IntPtr, SNIPacket>();

	private object _writePacketLockObject = new object();

	private GCHandle _gcHandle;

	private int _pendingCallbacks;

	private long _timeoutMilliseconds;

	private long _timeoutTime;

	private int _timeoutState;

	private int _timeoutIdentitySource;

	private volatile int _timeoutIdentityValue;

	internal volatile bool _attentionSent;

	internal bool _attentionReceived;

	internal volatile bool _attentionSending;

	internal bool _enforceTimeoutDelay;

	internal int _enforcedTimeoutDelayInMilliSeconds = 5000;

	private readonly LastIOTimer _lastSuccessfulIOTimer;

	private SecureString[] _securePasswords = new SecureString[2];

	private int[] _securePasswordOffsetsInBuffer = new int[2];

	private bool _cancelled;

	private const int _waitForCancellationLockPollTimeout = 100;

	private volatile int _allowObjectID;

	internal bool _hasOpenResult;

	internal SqlInternalTransaction _executedUnderTransaction;

	internal ulong _longlen;

	internal ulong _longlenleft;

	internal int[] _decimalBits;

	internal byte[] _bTmp = new byte[12];

	internal int _bTmpRead;

	internal Decoder _plpdecoder;

	internal bool _accumulateInfoEvents;

	internal List<SqlError> _pendingInfoEvents;

	internal byte[] _bLongBytes;

	internal byte[] _bIntBytes;

	internal byte[] _bShortBytes;

	internal byte[] _bDecimalBytes;

	private byte[] _partialHeaderBuffer = new byte[8];

	internal int _partialHeaderBytesRead;

	internal _SqlMetaDataSet _cleanupMetaData;

	internal _SqlMetaDataSetCollection _cleanupAltMetaDataSetArray;

	internal int _tracePasswordOffset;

	internal int _tracePasswordLength;

	internal int _traceChangePasswordOffset;

	internal int _traceChangePasswordLength;

	internal bool _receivedColMetaData;

	private SniContext _sniContext;

	private bool _bcpLock;

	private NullBitmap _nullBitmapInfo;

	internal TaskCompletionSource<object> _networkPacketTaskSource;

	private Timer _networkPacketTimeout;

	internal bool _syncOverAsync = true;

	private bool _snapshotReplay;

	private StateSnapshot _snapshot;

	internal ExecutionContext _executionContext;

	internal bool _asyncReadWithoutSnapshot;

	internal SqlErrorCollection _errors;

	internal SqlErrorCollection _warnings;

	internal object _errorAndWarningsLock = new object();

	private bool _hasErrorOrWarning;

	internal SqlErrorCollection _preAttentionErrors;

	internal SqlErrorCollection _preAttentionWarnings;

	private volatile TaskCompletionSource<object> _writeCompletionSource;

	private volatile int _asyncWriteCount;

	private volatile Exception _delayedWriteAsyncCallbackException;

	private int _readingCount;

	internal int ObjectID => _objectID;

	internal bool BcpLock
	{
		get
		{
			return _bcpLock;
		}
		set
		{
			_bcpLock = value;
		}
	}

	internal SNIHandle Handle => _sessionHandle;

	internal bool HasOpenResult => _hasOpenResult;

	internal bool IsOrphaned
	{
		get
		{
			if (_activateCount != 0)
			{
				return !_owner.IsAlive;
			}
			return false;
		}
	}

	internal object Owner
	{
		set
		{
			if (!(value is SqlDataReader sqlDataReader))
			{
				_readerState = null;
			}
			else
			{
				_readerState = sqlDataReader._sharedState;
			}
			_owner.Target = value;
		}
	}

	internal bool HasOwner => _owner.IsAlive;

	internal TdsParser Parser => _parser;

	internal SniContext SniContext
	{
		get
		{
			return _sniContext;
		}
		set
		{
			_sniContext = value;
		}
	}

	internal uint Status
	{
		get
		{
			if (_sessionHandle != null)
			{
				return _sessionHandle.Status;
			}
			return uint.MaxValue;
		}
	}

	internal bool TimeoutHasExpired => TdsParserStaticMethods.TimeoutHasExpired(_timeoutTime);

	internal long TimeoutTime
	{
		get
		{
			if (_timeoutMilliseconds != 0L)
			{
				_timeoutTime = TdsParserStaticMethods.GetTimeout(_timeoutMilliseconds);
				_timeoutMilliseconds = 0L;
			}
			return _timeoutTime;
		}
		set
		{
			_timeoutMilliseconds = 0L;
			_timeoutTime = value;
		}
	}

	public bool IsTimeoutStateExpired
	{
		get
		{
			int timeoutState = _timeoutState;
			if (timeoutState != 2)
			{
				return timeoutState == 3;
			}
			return true;
		}
	}

	internal bool HasErrorOrWarning => _hasErrorOrWarning;

	internal int ErrorCount
	{
		get
		{
			int result = 0;
			lock (_errorAndWarningsLock)
			{
				if (_errors != null)
				{
					result = _errors.Count;
				}
			}
			return result;
		}
	}

	internal int WarningCount
	{
		get
		{
			int result = 0;
			lock (_errorAndWarningsLock)
			{
				if (_warnings != null)
				{
					result = _warnings.Count;
				}
			}
			return result;
		}
	}

	internal int PreAttentionErrorCount
	{
		get
		{
			int result = 0;
			lock (_errorAndWarningsLock)
			{
				if (_preAttentionErrors != null)
				{
					result = _preAttentionErrors.Count;
				}
			}
			return result;
		}
	}

	internal int PreAttentionWarningCount
	{
		get
		{
			int result = 0;
			lock (_errorAndWarningsLock)
			{
				if (_preAttentionWarnings != null)
				{
					result = _preAttentionWarnings.Count;
				}
			}
			return result;
		}
	}

	internal TdsParserStateObject(TdsParser parser)
	{
		_parser = parser;
		SetPacketSize(4096);
		IncrementPendingCallbacks();
		_lastSuccessfulIOTimer = new LastIOTimer();
	}

	internal TdsParserStateObject(TdsParser parser, SNIHandle physicalConnection, bool async)
	{
		_parser = parser;
		SniContext = SniContext.Snix_GetMarsSession;
		SetPacketSize(_parser._physicalStateObj._outBuff.Length);
		SNINativeMethodWrapper.ConsumerInfo myInfo = CreateConsumerInfo(async);
		SQLDNSInfo result;
		bool dNSInfo = SQLFallbackDNSCache.Instance.GetDNSInfo(_parser.FQDNforDNSCahce, out result);
		_sessionHandle = new SNIHandle(myInfo, physicalConnection, _parser.Connection.ConnectionOptions.IPAddressPreference, result);
		if (_sessionHandle.Status != 0)
		{
			AddError(parser.ProcessSNIError(this));
			ThrowExceptionAndWarning();
		}
		IncrementPendingCallbacks();
		_lastSuccessfulIOTimer = parser._physicalStateObj._lastSuccessfulIOTimer;
	}

	internal int GetTimeoutRemaining()
	{
		int result;
		if (_timeoutMilliseconds != 0L)
		{
			result = (int)Math.Min(2147483647L, _timeoutMilliseconds);
			_timeoutTime = TdsParserStaticMethods.GetTimeout(_timeoutMilliseconds);
			_timeoutMilliseconds = 0L;
		}
		else
		{
			result = TdsParserStaticMethods.GetTimeoutMilliseconds(_timeoutTime);
		}
		return result;
	}

	internal bool TryStartNewRow(bool isNullCompressed, int nullBitmapColumnsCount = 0)
	{
		if (_snapshot != null)
		{
			_snapshot.CloneNullBitmapInfo();
		}
		if (isNullCompressed)
		{
			if (!_nullBitmapInfo.TryInitialize(this, nullBitmapColumnsCount))
			{
				return false;
			}
		}
		else
		{
			_nullBitmapInfo.Clean();
		}
		return true;
	}

	internal bool IsRowTokenReady()
	{
		int num = Math.Min(_inBytesPacket, _inBytesRead - _inBytesUsed) - 1;
		if (num > 0)
		{
			if (_inBuff[_inBytesUsed] == 209)
			{
				return true;
			}
			if (_inBuff[_inBytesUsed] == 210)
			{
				int num2 = 1 + (_cleanupMetaData.Length + 7) / 8;
				return num2 <= num;
			}
		}
		return false;
	}

	internal bool IsNullCompressionBitSet(int columnOrdinal)
	{
		return _nullBitmapInfo.IsGuaranteedNull(columnOrdinal);
	}

	internal void Activate(object owner)
	{
		Owner = owner;
		int num = Interlocked.Increment(ref _activateCount);
	}

	internal void Cancel(int objectID)
	{
		bool lockTaken = false;
		try
		{
			while (!lockTaken && _parser.State != 0 && _parser.State != TdsParserState.Broken)
			{
				Monitor.TryEnter(this, 100, ref lockTaken);
				if (!lockTaken || _cancelled || objectID != _allowObjectID || objectID == -1)
				{
					continue;
				}
				_cancelled = true;
				if (!_pendingData || _attentionSent)
				{
					continue;
				}
				bool lockTaken2 = false;
				while (!lockTaken2 && _parser.State != 0 && _parser.State != TdsParserState.Broken)
				{
					try
					{
						_parser.Connection._parserLock.Wait(canReleaseFromAnyThread: false, 100, ref lockTaken2);
						if (lockTaken2)
						{
							_parser.Connection.ThreadHasParserLockForClose = true;
							SendAttention();
						}
					}
					finally
					{
						if (lockTaken2)
						{
							if (_parser.Connection.ThreadHasParserLockForClose)
							{
								_parser.Connection.ThreadHasParserLockForClose = false;
							}
							_parser.Connection._parserLock.Release();
						}
					}
				}
			}
		}
		finally
		{
			if (lockTaken)
			{
				Monitor.Exit(this);
			}
		}
	}

	internal void CancelRequest()
	{
		ResetBuffer();
		ResetPacketCounters();
		if (!_bulkCopyWriteTimeout)
		{
			SendAttention();
			Parser.ProcessPendingAck(this);
		}
	}

	public void CheckSetResetConnectionState(uint error, CallbackType callbackType)
	{
		if (_fResetEventOwned)
		{
			if (callbackType == CallbackType.Read && error == 0)
			{
				_parser._fResetConnection = false;
				_fResetConnectionSent = false;
				_fResetEventOwned = !_parser._resetConnectionEvent.Set();
			}
			if (error != 0)
			{
				_fResetConnectionSent = false;
				_fResetEventOwned = !_parser._resetConnectionEvent.Set();
			}
		}
	}

	internal void CloseSession()
	{
		ResetCancelAndProcessAttention();
		Parser.PutSession(this);
	}

	private void ResetCancelAndProcessAttention()
	{
		lock (this)
		{
			_cancelled = false;
			_allowObjectID = -1;
			if (_attentionSent)
			{
				Parser.ProcessPendingAck(this);
			}
			SetTimeoutStateStopped();
		}
	}

	private SNINativeMethodWrapper.ConsumerInfo CreateConsumerInfo(bool async)
	{
		SNINativeMethodWrapper.ConsumerInfo result = default(SNINativeMethodWrapper.ConsumerInfo);
		result.defaultBufferSize = _outBuff.Length;
		if (async)
		{
			result.readDelegate = SNILoadHandle.SingletonInstance.ReadAsyncCallbackDispatcher;
			result.writeDelegate = SNILoadHandle.SingletonInstance.WriteAsyncCallbackDispatcher;
			_gcHandle = GCHandle.Alloc(this, GCHandleType.Normal);
			result.key = (IntPtr)_gcHandle;
		}
		return result;
	}

	internal void CreatePhysicalSNIHandle(string serverName, bool ignoreSniOpenTimeout, long timerExpire, out byte[] instanceName, byte[] spnBuffer, bool flushCache, bool async, bool fParallel, TransparentNetworkResolutionState transparentNetworkResolutionState, int totalTimeout, SqlConnectionIPAddressPreference ipPreference, string cachedFQDN)
	{
		SNINativeMethodWrapper.ConsumerInfo myInfo = CreateConsumerInfo(async);
		long num;
		if (long.MaxValue == timerExpire)
		{
			num = 2147483647L;
		}
		else
		{
			num = ADP.TimerRemainingMilliseconds(timerExpire);
			if (num > int.MaxValue)
			{
				num = 2147483647L;
			}
			else if (0 > num)
			{
				num = 0L;
			}
		}
		SQLDNSInfo result;
		bool dNSInfo = SQLFallbackDNSCache.Instance.GetDNSInfo(cachedFQDN, out result);
		_sessionHandle = new SNIHandle(myInfo, serverName, spnBuffer, ignoreSniOpenTimeout, checked((int)num), out instanceName, flushCache, !async, fParallel, transparentNetworkResolutionState, totalTimeout, ipPreference, result);
	}

	internal bool Deactivate()
	{
		bool result = false;
		try
		{
			TdsParserState state = Parser.State;
			if (state != TdsParserState.Broken && state != 0)
			{
				if (_pendingData)
				{
					Parser.DrainData(this);
				}
				if (HasOpenResult)
				{
					DecrementOpenResultCount();
				}
				ResetCancelAndProcessAttention();
				result = true;
			}
		}
		catch (Exception e)
		{
			if (!ADP.IsCatchableExceptionType(e))
			{
				throw;
			}
			ADP.TraceExceptionWithoutRethrow(e);
		}
		return result;
	}

	internal void RemoveOwner()
	{
		if (_parser.MARSOn)
		{
			int num = Interlocked.Decrement(ref _activateCount);
		}
		Owner = null;
	}

	internal void DecrementOpenResultCount()
	{
		if (_executedUnderTransaction == null)
		{
			_parser.DecrementNonTransactedOpenResultCount();
		}
		else
		{
			_executedUnderTransaction.DecrementAndObtainOpenResultCount();
			_executedUnderTransaction = null;
		}
		_hasOpenResult = false;
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal int DecrementPendingCallbacks(bool release)
	{
		int num = Interlocked.Decrement(ref _pendingCallbacks);
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParserStateObject.DecrementPendingCallbacks|ADV> {0}, after decrementing _pendingCallbacks: {1}", ObjectID, _pendingCallbacks);
		if ((num == 0 || release) && _gcHandle.IsAllocated)
		{
			SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParserStateObject.DecrementPendingCallbacks|ADV> {0}, FREEING HANDLE!", ObjectID);
			_gcHandle.Free();
		}
		return num;
	}

	internal void Dispose()
	{
		SafeHandle sniPacket = _sniPacket;
		SafeHandle sessionHandle = _sessionHandle;
		SafeHandle sniAsyncAttnPacket = _sniAsyncAttnPacket;
		_sniPacket = null;
		_sessionHandle = null;
		_sniAsyncAttnPacket = null;
		Timer networkPacketTimeout = _networkPacketTimeout;
		if (networkPacketTimeout != null)
		{
			_networkPacketTimeout = null;
			networkPacketTimeout.Dispose();
		}
		if (Volatile.Read(ref _readingCount) > 0)
		{
			SpinWait.SpinUntil(() => Volatile.Read(ref _readingCount) == 0);
		}
		if (sessionHandle != null || sniPacket != null)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				sniPacket?.Dispose();
				sniAsyncAttnPacket?.Dispose();
				if (sessionHandle != null)
				{
					sessionHandle.Dispose();
					DecrementPendingCallbacks(release: true);
				}
			}
		}
		if (_writePacketCache == null)
		{
			return;
		}
		lock (_writePacketLockObject)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				_writePacketCache.Dispose();
			}
		}
	}

	internal int IncrementAndObtainOpenResultCount(SqlInternalTransaction transaction)
	{
		_hasOpenResult = true;
		if (transaction == null)
		{
			return _parser.IncrementNonTransactedOpenResultCount();
		}
		_executedUnderTransaction = transaction;
		return transaction.IncrementAndObtainOpenResultCount();
	}

	[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
	internal int IncrementPendingCallbacks()
	{
		int result = Interlocked.Increment(ref _pendingCallbacks);
		SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.TdsParserStateObject.IncrementPendingCallbacks|ADV> {0}, after incrementing _pendingCallbacks: {1}", ObjectID, _pendingCallbacks);
		return result;
	}

	internal void SetTimeoutSeconds(int timeout)
	{
		SetTimeoutMilliseconds((long)timeout * 1000L);
	}

	internal void SetTimeoutMilliseconds(long timeout)
	{
		if (timeout <= 0)
		{
			_timeoutMilliseconds = 0L;
			_timeoutTime = long.MaxValue;
		}
		else
		{
			_timeoutMilliseconds = timeout;
			_timeoutTime = 0L;
		}
	}

	internal void StartSession(int objectID)
	{
		_allowObjectID = objectID;
	}

	internal void ThrowExceptionAndWarning(bool callerHasConnectionLock = false, bool asyncClose = false)
	{
		_parser.ThrowExceptionAndWarning(this, callerHasConnectionLock, asyncClose);
	}

	internal Task ExecuteFlush()
	{
		lock (this)
		{
			if (_cancelled && 1 == _outputPacketNumber)
			{
				ResetBuffer();
				_cancelled = false;
				throw SQL.OperationCancelled();
			}
			Task task = WritePacket(1);
			if (task == null)
			{
				_pendingData = true;
				_messageStatus = 0;
				return null;
			}
			return AsyncHelper.CreateContinuationTask(task, delegate
			{
				_pendingData = true;
				_messageStatus = 0;
			});
		}
	}

	internal bool TryProcessHeader()
	{
		if (_partialHeaderBytesRead > 0 || _inBytesUsed + _inputHeaderLen > _inBytesRead)
		{
			do
			{
				int num = Math.Min(_inBytesRead - _inBytesUsed, _inputHeaderLen - _partialHeaderBytesRead);
				Buffer.BlockCopy(_inBuff, _inBytesUsed, _partialHeaderBuffer, _partialHeaderBytesRead, num);
				_partialHeaderBytesRead += num;
				_inBytesUsed += num;
				if (_partialHeaderBytesRead == _inputHeaderLen)
				{
					_partialHeaderBytesRead = 0;
					_inBytesPacket = ((_partialHeaderBuffer[2] << 8) | _partialHeaderBuffer[3]) - _inputHeaderLen;
					_messageStatus = _partialHeaderBuffer[1];
					_spid = (_partialHeaderBuffer[4] << 8) | _partialHeaderBuffer[5];
					continue;
				}
				if (_parser.State == TdsParserState.Broken || _parser.State == TdsParserState.Closed)
				{
					ThrowExceptionAndWarning();
					return true;
				}
				if (!TryReadNetworkPacket())
				{
					return false;
				}
				if (IsTimeoutStateExpired)
				{
					ThrowExceptionAndWarning();
					return true;
				}
			}
			while (_partialHeaderBytesRead != 0);
		}
		else
		{
			_messageStatus = _inBuff[_inBytesUsed + 1];
			_inBytesPacket = ((_inBuff[_inBytesUsed + 2] << 8) | _inBuff[_inBytesUsed + 2 + 1]) - _inputHeaderLen;
			_spid = (_inBuff[_inBytesUsed + 4] << 8) | _inBuff[_inBytesUsed + 4 + 1];
			_inBytesUsed += _inputHeaderLen;
		}
		if (_inBytesPacket < 0)
		{
			throw SQL.ParsingError(ParsingErrorState.CorruptedTdsStream);
		}
		return true;
	}

	internal bool TryPrepareBuffer()
	{
		if (_inBytesPacket == 0 && _inBytesUsed < _inBytesRead && !TryProcessHeader())
		{
			return false;
		}
		if (_inBytesUsed == _inBytesRead)
		{
			if (_inBytesPacket > 0)
			{
				if (!TryReadNetworkPacket())
				{
					return false;
				}
			}
			else if (_inBytesPacket == 0)
			{
				if (!TryReadNetworkPacket())
				{
					return false;
				}
				if (!TryProcessHeader())
				{
					return false;
				}
				if (_inBytesUsed == _inBytesRead && !TryReadNetworkPacket())
				{
					return false;
				}
			}
		}
		return true;
	}

	internal void ResetBuffer()
	{
		_outBytesUsed = _outputHeaderLen;
	}

	internal void ResetPacketCounters()
	{
		_outputPacketNumber = 1;
		_outputPacketCount = 0u;
	}

	internal bool SetPacketSize(int size)
	{
		if (size > 32768)
		{
			throw SQL.InvalidPacketSize();
		}
		if (_inBuff == null || _inBuff.Length != size)
		{
			if (_inBuff == null)
			{
				_inBuff = new byte[size];
				_inBytesRead = 0;
				_inBytesUsed = 0;
			}
			else if (size != _inBuff.Length)
			{
				if (_inBytesRead > _inBytesUsed)
				{
					byte[] inBuff = _inBuff;
					_inBuff = new byte[size];
					int num = _inBytesRead - _inBytesUsed;
					if (inBuff.Length < _inBytesUsed + num || _inBuff.Length < num)
					{
						string str = StringsHelper.GetString(Strings.SQL_InvalidInternalPacketSize) + " " + inBuff.Length + ", " + _inBytesUsed + ", " + num + ", " + _inBuff.Length;
						throw SQL.InvalidInternalPacketSize(str);
					}
					Buffer.BlockCopy(inBuff, _inBytesUsed, _inBuff, 0, num);
					_inBytesRead -= _inBytesUsed;
					_inBytesUsed = 0;
				}
				else
				{
					_inBuff = new byte[size];
					_inBytesRead = 0;
					_inBytesUsed = 0;
				}
			}
			_outBuff = new byte[size];
			_outBytesUsed = _outputHeaderLen;
			return true;
		}
		return false;
	}

	internal bool TryPeekByte(out byte value)
	{
		if (!TryReadByte(out value))
		{
			return false;
		}
		_inBytesPacket++;
		_inBytesUsed--;
		return true;
	}

	public bool TryReadByteArray(byte[] buff, int offset, int len)
	{
		int totalRead;
		return TryReadByteArray(buff, offset, len, out totalRead);
	}

	public bool TryReadByteArray(byte[] buff, int offset, int len, out int totalRead)
	{
		totalRead = 0;
		while (len > 0)
		{
			if ((_inBytesPacket == 0 || _inBytesUsed == _inBytesRead) && !TryPrepareBuffer())
			{
				return false;
			}
			int num = Math.Min(len, Math.Min(_inBytesPacket, _inBytesRead - _inBytesUsed));
			if (buff != null)
			{
				Buffer.BlockCopy(_inBuff, _inBytesUsed, buff, offset + totalRead, num);
			}
			totalRead += num;
			_inBytesUsed += num;
			_inBytesPacket -= num;
			len -= num;
		}
		return true;
	}

	internal bool TryReadByte(out byte value)
	{
		value = 0;
		if ((_inBytesPacket == 0 || _inBytesUsed == _inBytesRead) && !TryPrepareBuffer())
		{
			return false;
		}
		_inBytesPacket--;
		value = _inBuff[_inBytesUsed++];
		return true;
	}

	internal bool TryReadChar(out char value)
	{
		byte[] array;
		int num;
		if (_inBytesUsed + 2 > _inBytesRead || _inBytesPacket < 2)
		{
			if (!TryReadByteArray(_bTmp, 0, 2))
			{
				value = '\0';
				return false;
			}
			array = _bTmp;
			num = 0;
		}
		else
		{
			array = _inBuff;
			num = _inBytesUsed;
			_inBytesUsed += 2;
			_inBytesPacket -= 2;
		}
		value = (char)((array[num + 1] << 8) + array[num]);
		return true;
	}

	internal bool TryReadInt16(out short value)
	{
		byte[] array;
		int num;
		if (_inBytesUsed + 2 > _inBytesRead || _inBytesPacket < 2)
		{
			if (!TryReadByteArray(_bTmp, 0, 2))
			{
				value = 0;
				return false;
			}
			array = _bTmp;
			num = 0;
		}
		else
		{
			array = _inBuff;
			num = _inBytesUsed;
			_inBytesUsed += 2;
			_inBytesPacket -= 2;
		}
		value = (short)((array[num + 1] << 8) + array[num]);
		return true;
	}

	internal bool TryReadInt32(out int value)
	{
		if (_inBytesUsed + 4 > _inBytesRead || _inBytesPacket < 4)
		{
			if (!TryReadByteArray(_bTmp, 0, 4))
			{
				value = 0;
				return false;
			}
			value = BitConverter.ToInt32(_bTmp, 0);
			return true;
		}
		value = BitConverter.ToInt32(_inBuff, _inBytesUsed);
		_inBytesUsed += 4;
		_inBytesPacket -= 4;
		return true;
	}

	internal bool TryReadInt64(out long value)
	{
		if ((_inBytesPacket == 0 || _inBytesUsed == _inBytesRead) && !TryPrepareBuffer())
		{
			value = 0L;
			return false;
		}
		if (_bTmpRead > 0 || _inBytesUsed + 8 > _inBytesRead || _inBytesPacket < 8)
		{
			int totalRead = 0;
			if (!TryReadByteArray(_bTmp, _bTmpRead, 8 - _bTmpRead, out totalRead))
			{
				_bTmpRead += totalRead;
				value = 0L;
				return false;
			}
			_bTmpRead = 0;
			value = BitConverter.ToInt64(_bTmp, 0);
			return true;
		}
		value = BitConverter.ToInt64(_inBuff, _inBytesUsed);
		_inBytesUsed += 8;
		_inBytesPacket -= 8;
		return true;
	}

	internal bool TryReadUInt16(out ushort value)
	{
		byte[] array;
		int num;
		if (_inBytesUsed + 2 > _inBytesRead || _inBytesPacket < 2)
		{
			if (!TryReadByteArray(_bTmp, 0, 2))
			{
				value = 0;
				return false;
			}
			array = _bTmp;
			num = 0;
		}
		else
		{
			array = _inBuff;
			num = _inBytesUsed;
			_inBytesUsed += 2;
			_inBytesPacket -= 2;
		}
		value = (ushort)((array[num + 1] << 8) + array[num]);
		return true;
	}

	internal bool TryReadUInt32(out uint value)
	{
		if ((_inBytesPacket == 0 || _inBytesUsed == _inBytesRead) && !TryPrepareBuffer())
		{
			value = 0u;
			return false;
		}
		if (_bTmpRead > 0 || _inBytesUsed + 4 > _inBytesRead || _inBytesPacket < 4)
		{
			int totalRead = 0;
			if (!TryReadByteArray(_bTmp, _bTmpRead, 4 - _bTmpRead, out totalRead))
			{
				_bTmpRead += totalRead;
				value = 0u;
				return false;
			}
			_bTmpRead = 0;
			value = BitConverter.ToUInt32(_bTmp, 0);
			return true;
		}
		value = BitConverter.ToUInt32(_inBuff, _inBytesUsed);
		_inBytesUsed += 4;
		_inBytesPacket -= 4;
		return true;
	}

	internal bool TryReadSingle(out float value)
	{
		if (_inBytesUsed + 4 > _inBytesRead || _inBytesPacket < 4)
		{
			if (!TryReadByteArray(_bTmp, 0, 4))
			{
				value = 0f;
				return false;
			}
			value = BitConverter.ToSingle(_bTmp, 0);
			return true;
		}
		value = BitConverter.ToSingle(_inBuff, _inBytesUsed);
		_inBytesUsed += 4;
		_inBytesPacket -= 4;
		return true;
	}

	internal bool TryReadDouble(out double value)
	{
		if (_inBytesUsed + 8 > _inBytesRead || _inBytesPacket < 8)
		{
			if (!TryReadByteArray(_bTmp, 0, 8))
			{
				value = 0.0;
				return false;
			}
			value = BitConverter.ToDouble(_bTmp, 0);
			return true;
		}
		value = BitConverter.ToDouble(_inBuff, _inBytesUsed);
		_inBytesUsed += 8;
		_inBytesPacket -= 8;
		return true;
	}

	internal bool TryReadString(int length, out string value)
	{
		int num = length << 1;
		int index = 0;
		byte[] bytes;
		if (_inBytesUsed + num > _inBytesRead || _inBytesPacket < num)
		{
			if (_bTmp == null || _bTmp.Length < num)
			{
				_bTmp = new byte[num];
			}
			if (!TryReadByteArray(_bTmp, 0, num))
			{
				value = null;
				return false;
			}
			bytes = _bTmp;
		}
		else
		{
			bytes = _inBuff;
			index = _inBytesUsed;
			_inBytesUsed += num;
			_inBytesPacket -= num;
		}
		value = Encoding.Unicode.GetString(bytes, index, num);
		return true;
	}

	internal bool TryReadStringWithEncoding(int length, Encoding encoding, bool isPlp, out string value)
	{
		if (encoding == null)
		{
			if (isPlp)
			{
				if (!_parser.TrySkipPlpValue((ulong)length, this, out var _))
				{
					value = null;
					return false;
				}
			}
			else if (!TrySkipBytes(length))
			{
				value = null;
				return false;
			}
			_parser.ThrowUnsupportedCollationEncountered(this);
		}
		byte[] buff = null;
		int index = 0;
		if (isPlp)
		{
			if (!TryReadPlpBytes(ref buff, 0, int.MaxValue, out length))
			{
				value = null;
				return false;
			}
		}
		else if (_inBytesUsed + length > _inBytesRead || _inBytesPacket < length)
		{
			if (_bTmp == null || _bTmp.Length < length)
			{
				_bTmp = new byte[length];
			}
			if (!TryReadByteArray(_bTmp, 0, length))
			{
				value = null;
				return false;
			}
			buff = _bTmp;
		}
		else
		{
			buff = _inBuff;
			index = _inBytesUsed;
			_inBytesUsed += length;
			_inBytesPacket -= length;
		}
		value = encoding.GetString(buff, index, length);
		return true;
	}

	internal ulong ReadPlpLength(bool returnPlpNullIfNull)
	{
		if (!TryReadPlpLength(returnPlpNullIfNull, out var lengthLeft))
		{
			throw SQL.SynchronousCallMayNotPend();
		}
		return lengthLeft;
	}

	internal bool TryReadPlpLength(bool returnPlpNullIfNull, out ulong lengthLeft)
	{
		bool flag = false;
		if (_longlen == 0L)
		{
			if (!TryReadInt64(out var value))
			{
				lengthLeft = 0uL;
				return false;
			}
			_longlen = (ulong)value;
		}
		if (_longlen == ulong.MaxValue)
		{
			_longlen = 0uL;
			_longlenleft = 0uL;
			flag = true;
		}
		else
		{
			if (!TryReadUInt32(out var value2))
			{
				lengthLeft = 0uL;
				return false;
			}
			if (value2 == 0)
			{
				_longlenleft = 0uL;
				_longlen = 0uL;
			}
			else
			{
				_longlenleft = value2;
			}
		}
		if (flag && returnPlpNullIfNull)
		{
			lengthLeft = ulong.MaxValue;
			return true;
		}
		lengthLeft = _longlenleft;
		return true;
	}

	internal int ReadPlpBytesChunk(byte[] buff, int offset, int len)
	{
		int num = (int)Math.Min(_longlenleft, (ulong)len);
		int totalRead;
		bool flag = TryReadByteArray(buff, offset, num, out totalRead);
		_longlenleft -= (ulong)num;
		if (!flag)
		{
			throw SQL.SynchronousCallMayNotPend();
		}
		return totalRead;
	}

	internal bool TryReadPlpBytes(ref byte[] buff, int offst, int len, out int totalBytesRead)
	{
		int totalRead = 0;
		if (_longlen == 0L)
		{
			if (buff == null)
			{
				buff = new byte[0];
			}
			totalBytesRead = 0;
			return true;
		}
		int num = len;
		if (buff == null && _longlen != 18446744073709551614uL)
		{
			buff = new byte[Math.Min((int)_longlen, len)];
		}
		ulong lengthLeft;
		if (_longlenleft == 0L)
		{
			if (!TryReadPlpLength(returnPlpNullIfNull: false, out lengthLeft))
			{
				totalBytesRead = 0;
				return false;
			}
			if (_longlenleft == 0L)
			{
				totalBytesRead = 0;
				return true;
			}
		}
		if (buff == null)
		{
			buff = new byte[_longlenleft];
		}
		totalBytesRead = 0;
		while (num > 0)
		{
			int num2 = (int)Math.Min(_longlenleft, (ulong)num);
			if (buff.Length < offst + num2)
			{
				byte[] array = new byte[offst + num2];
				Buffer.BlockCopy(buff, 0, array, 0, offst);
				buff = array;
			}
			bool flag = TryReadByteArray(buff, offst, num2, out totalRead);
			num -= totalRead;
			offst += totalRead;
			totalBytesRead += totalRead;
			_longlenleft -= (ulong)totalRead;
			if (!flag)
			{
				return false;
			}
			if (_longlenleft == 0L && !TryReadPlpLength(returnPlpNullIfNull: false, out lengthLeft))
			{
				return false;
			}
			if (_longlenleft == 0L)
			{
				break;
			}
		}
		return true;
	}

	internal bool TrySkipLongBytes(long num)
	{
		int num2 = 0;
		while (num > 0)
		{
			num2 = (int)Math.Min(2147483647L, num);
			if (!TryReadByteArray(null, 0, num2))
			{
				return false;
			}
			num -= num2;
		}
		return true;
	}

	internal bool TrySkipBytes(int num)
	{
		return TryReadByteArray(null, 0, num);
	}

	internal void SetSnapshot()
	{
		_snapshot = new StateSnapshot(this);
		_snapshot.Snap();
		_snapshotReplay = false;
	}

	internal void ResetSnapshot()
	{
		_snapshot = null;
		_snapshotReplay = false;
	}

	internal bool TryReadNetworkPacket()
	{
		if (_snapshot != null)
		{
			if (_snapshotReplay && _snapshot.Replay())
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.ReadNetworkPacket|{0}|ADV> Async packet replay{0}", "INFO");
				return true;
			}
			_inBuff = new byte[_inBuff.Length];
		}
		if (_syncOverAsync)
		{
			ReadSniSyncOverAsync();
			return true;
		}
		ReadSni(new TaskCompletionSource<object>());
		return false;
	}

	internal void PrepareReplaySnapshot()
	{
		_networkPacketTaskSource = null;
		_snapshot.PrepareReplay();
	}

	internal void ReadSniSyncOverAsync()
	{
		if (_parser.State == TdsParserState.Broken || _parser.State == TdsParserState.Closed)
		{
			throw ADP.ClosedConnectionError();
		}
		IntPtr ppNewPacket = IntPtr.Zero;
		RuntimeHelpers.PrepareConstrainedRegions();
		bool flag = false;
		try
		{
			Interlocked.Increment(ref _readingCount);
			flag = true;
			SNIHandle handle = Handle;
			if (handle == null)
			{
				throw ADP.ClosedConnectionError();
			}
			uint num = SNINativeMethodWrapper.SNIReadSyncOverAsync(handle, ref ppNewPacket, GetTimeoutRemaining());
			Interlocked.Decrement(ref _readingCount);
			flag = false;
			if (_parser.MARSOn)
			{
				CheckSetResetConnectionState(num, CallbackType.Read);
			}
			if (num == 0)
			{
				ProcessSniPacket(ppNewPacket, 0u);
			}
			else
			{
				ReadSniError(this, num);
			}
		}
		finally
		{
			if (flag)
			{
				Interlocked.Decrement(ref _readingCount);
			}
			if (ppNewPacket != IntPtr.Zero)
			{
				SNINativeMethodWrapper.SNIPacketRelease(ppNewPacket);
			}
		}
	}

	internal void OnConnectionClosed()
	{
		Parser.State = TdsParserState.Broken;
		Parser.Connection.BreakConnection();
		Thread.MemoryBarrier();
		_networkPacketTaskSource?.TrySetException(ADP.ExceptionWithStackTrace(ADP.ClosedConnectionError()));
		_writeCompletionSource?.TrySetException(ADP.ExceptionWithStackTrace(ADP.ClosedConnectionError()));
	}

	public void SetTimeoutStateStopped()
	{
		Interlocked.Exchange(ref _timeoutState, 0);
		_timeoutIdentityValue = 0;
	}

	private void OnTimeoutAsync(object state)
	{
		if (_enforceTimeoutDelay)
		{
			Thread.Sleep(_enforcedTimeoutDelayInMilliSeconds);
		}
		int timeoutIdentityValue = _timeoutIdentityValue;
		TimeoutState timeoutState = (TimeoutState)state;
		if (timeoutState.IdentityValue == _timeoutIdentityValue)
		{
			OnTimeoutCore(1, 2);
		}
	}

	private bool OnTimeoutSync()
	{
		return OnTimeoutCore(1, 3);
	}

	private bool OnTimeoutCore(int expectedState, int targetState)
	{
		bool result = false;
		if (Interlocked.CompareExchange(ref _timeoutState, targetState, expectedState) == expectedState)
		{
			result = true;
			lock (this)
			{
				if (!_attentionSent)
				{
					AddError(new SqlError(-2, 0, 11, _parser.Server, _parser.Connection.TimeoutErrorInternal.GetErrorMessage(), "", 0, 258u));
					TaskCompletionSource<object> source = _networkPacketTaskSource;
					if (_parser.Connection.IsInPool)
					{
						_parser.State = TdsParserState.Broken;
						_parser.Connection.BreakConnection();
						if (source != null)
						{
							source.TrySetCanceled();
						}
					}
					else if (_parser.State == TdsParserState.OpenLoggedIn)
					{
						try
						{
							SendAttention(mustTakeWriteLock: true);
						}
						catch (Exception e)
						{
							if (!ADP.IsCatchableExceptionType(e))
							{
								throw;
							}
							if (source != null)
							{
								source.TrySetCanceled();
							}
						}
					}
					if (source != null)
					{
						Task.Delay(5000).ContinueWith(delegate
						{
							if (!source.Task.IsCompleted)
							{
								int num = IncrementPendingCallbacks();
								RuntimeHelpers.PrepareConstrainedRegions();
								try
								{
									if (num == 3 && !source.Task.IsCompleted)
									{
										bool flag = false;
										try
										{
											CheckThrowSNIException();
										}
										catch (Exception exception)
										{
											if (source.TrySetException(exception))
											{
												flag = true;
											}
										}
										_parser.State = TdsParserState.Broken;
										_parser.Connection.BreakConnection();
										if (!flag)
										{
											source.TrySetCanceled();
										}
									}
								}
								finally
								{
									DecrementPendingCallbacks(release: false);
								}
							}
						});
					}
				}
			}
		}
		return result;
	}

	internal void ReadSni(TaskCompletionSource<object> completion)
	{
		_networkPacketTaskSource = completion;
		Thread.MemoryBarrier();
		if (_parser.State == TdsParserState.Broken || _parser.State == TdsParserState.Closed)
		{
			throw ADP.ClosedConnectionError();
		}
		IntPtr ppNewPacket = IntPtr.Zero;
		uint num = 0u;
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			if (Interlocked.CompareExchange(ref _timeoutState, 1, 0) == 0)
			{
				_timeoutIdentityValue = Interlocked.Increment(ref _timeoutIdentitySource);
			}
			_networkPacketTimeout?.Dispose();
			_networkPacketTimeout = new Timer(OnTimeoutAsync, new TimeoutState(_timeoutIdentityValue), -1, -1);
			int timeoutRemaining = GetTimeoutRemaining();
			if (timeoutRemaining > 0)
			{
				ChangeNetworkPacketTimeout(timeoutRemaining, -1);
			}
			SNIHandle sNIHandle = null;
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				Interlocked.Increment(ref _readingCount);
				sNIHandle = Handle;
				if (sNIHandle != null)
				{
					IncrementPendingCallbacks();
					num = SNINativeMethodWrapper.SNIReadAsync(sNIHandle, ref ppNewPacket);
					if (num != 0 && 997 != num)
					{
						DecrementPendingCallbacks(release: false);
					}
				}
				Interlocked.Decrement(ref _readingCount);
			}
			if (sNIHandle == null)
			{
				throw ADP.ClosedConnectionError();
			}
			if (num == 0)
			{
				ReadAsyncCallback(ADP.PtrZero, ppNewPacket, 0u);
			}
			else if (997 != num)
			{
				ReadSniError(this, num);
				_networkPacketTaskSource.TrySetResult(null);
				SetTimeoutStateStopped();
				ChangeNetworkPacketTimeout(-1, -1);
			}
			else if (timeoutRemaining == 0)
			{
				ChangeNetworkPacketTimeout(-1, -1);
				OnTimeoutSync();
			}
		}
		finally
		{
			if (ppNewPacket != IntPtr.Zero)
			{
				SNINativeMethodWrapper.SNIPacketRelease(ppNewPacket);
			}
		}
	}

	internal bool IsConnectionAlive(bool throwOnException)
	{
		bool result = true;
		if (DateTime.UtcNow.Ticks - _lastSuccessfulIOTimer._value > 50000)
		{
			if (_parser == null || _parser.State == TdsParserState.Broken || _parser.State == TdsParserState.Closed)
			{
				result = false;
				if (throwOnException)
				{
					throw SQL.ConnectionDoomed();
				}
			}
			else if (_pendingCallbacks <= 1 && (_parser.Connection == null || _parser.Connection.IsInPool))
			{
				IntPtr zero = IntPtr.Zero;
				RuntimeHelpers.PrepareConstrainedRegions();
				try
				{
					SniContext = SniContext.Snix_Connect;
					uint num = SNINativeMethodWrapper.SNICheckConnection(Handle);
					if (num != 0 && num != 258)
					{
						SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.IsConnectionAlive|Info> received error {0} on idle connection", (int)num);
						result = false;
						if (throwOnException)
						{
							AddError(_parser.ProcessSNIError(this));
							ThrowExceptionAndWarning();
						}
					}
					else
					{
						_lastSuccessfulIOTimer._value = DateTime.UtcNow.Ticks;
					}
				}
				finally
				{
					if (zero != IntPtr.Zero)
					{
						SNINativeMethodWrapper.SNIPacketRelease(zero);
					}
				}
			}
		}
		return result;
	}

	internal bool ValidateSNIConnection()
	{
		if (_parser == null || _parser.State == TdsParserState.Broken || _parser.State == TdsParserState.Closed)
		{
			return false;
		}
		if (DateTime.UtcNow.Ticks - _lastSuccessfulIOTimer._value <= 50000)
		{
			return true;
		}
		uint num = 0u;
		SniContext = SniContext.Snix_Connect;
		try
		{
			Interlocked.Increment(ref _readingCount);
			SNIHandle handle = Handle;
			if (handle != null)
			{
				num = SNINativeMethodWrapper.SNICheckConnection(handle);
			}
		}
		finally
		{
			Interlocked.Decrement(ref _readingCount);
		}
		if (num != 0)
		{
			return num == 258;
		}
		return true;
	}

	private void ReadSniError(TdsParserStateObject stateObj, uint error)
	{
		if (258 == error)
		{
			bool flag = false;
			if (IsTimeoutStateExpired)
			{
				flag = true;
			}
			else
			{
				stateObj.SetTimeoutStateStopped();
				AddError(new SqlError(-2, 0, 11, _parser.Server, _parser.Connection.TimeoutErrorInternal.GetErrorMessage(), "", 0, 258u));
				if (!stateObj._attentionSent)
				{
					if (stateObj.Parser.State == TdsParserState.OpenLoggedIn)
					{
						stateObj.SendAttention(mustTakeWriteLock: true);
						IntPtr ppNewPacket = IntPtr.Zero;
						RuntimeHelpers.PrepareConstrainedRegions();
						bool flag2 = false;
						try
						{
							Interlocked.Increment(ref _readingCount);
							flag2 = true;
							SNIHandle handle = Handle;
							if (handle == null)
							{
								throw ADP.ClosedConnectionError();
							}
							error = SNINativeMethodWrapper.SNIReadSyncOverAsync(handle, ref ppNewPacket, stateObj.GetTimeoutRemaining());
							Interlocked.Decrement(ref _readingCount);
							flag2 = false;
							if (error == 0)
							{
								stateObj.ProcessSniPacket(ppNewPacket, 0u);
								return;
							}
							flag = true;
						}
						finally
						{
							if (flag2)
							{
								Interlocked.Decrement(ref _readingCount);
							}
							if (ppNewPacket != IntPtr.Zero)
							{
								SNINativeMethodWrapper.SNIPacketRelease(ppNewPacket);
							}
						}
					}
					else if (_parser._loginWithFailover)
					{
						_parser.Disconnect();
					}
					else if (_parser.State == TdsParserState.OpenNotLoggedIn && (_parser.Connection.ConnectionOptions.MultiSubnetFailover || _parser.Connection.ConnectionOptions.TransparentNetworkIPResolution))
					{
						_parser.Disconnect();
					}
					else
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				_parser.State = TdsParserState.Broken;
				_parser.Connection.BreakConnection();
			}
		}
		else
		{
			AddError(_parser.ProcessSNIError(stateObj));
		}
		ThrowExceptionAndWarning();
	}

	public void ProcessSniPacket(IntPtr packet, uint error)
	{
		if (error != 0)
		{
			if (_parser.State != 0 && _parser.State != TdsParserState.Broken)
			{
				AddError(_parser.ProcessSNIError(this));
			}
			return;
		}
		uint dataSize = 0u;
		if (SNINativeMethodWrapper.SNIPacketGetData(packet, _inBuff, ref dataSize) == 0)
		{
			if (_inBuff.Length < dataSize)
			{
				throw SQL.InvalidInternalPacketSize(StringsHelper.GetString(Strings.SqlMisc_InvalidArraySizeMessage));
			}
			_lastSuccessfulIOTimer._value = DateTime.UtcNow.Ticks;
			_inBytesRead = (int)dataSize;
			_inBytesUsed = 0;
			if (_snapshot != null)
			{
				_snapshot.PushBuffer(_inBuff, _inBytesRead);
				if (_snapshotReplay)
				{
					_snapshot.Replay();
				}
			}
			SniReadStatisticsAndTracing();
			SqlClientEventSource.Log.TryAdvancedTraceBinEvent("TdsParser.ReadNetworkPacketAsyncCallback | INFO | ADV | State Object Id {0}, Packet read. In Buffer {1}, In Bytes Read: {2}", ObjectID, _inBuff, (ushort)_inBytesRead);
			return;
		}
		throw SQL.ParsingError(ParsingErrorState.ProcessSniPacketFailed);
	}

	private void ChangeNetworkPacketTimeout(int dueTime, int period)
	{
		Timer networkPacketTimeout = _networkPacketTimeout;
		if (networkPacketTimeout != null)
		{
			try
			{
				networkPacketTimeout.Change(dueTime, period);
			}
			catch (ObjectDisposedException)
			{
			}
		}
	}

	public void ReadAsyncCallback(IntPtr key, IntPtr packet, uint error)
	{
		TaskCompletionSource<object> source = _networkPacketTaskSource;
		if (source == null && _parser._pMarsPhysicalConObj == this)
		{
			return;
		}
		RuntimeHelpers.PrepareConstrainedRegions();
		bool flag = true;
		try
		{
			if (_parser.MARSOn)
			{
				CheckSetResetConnectionState(error, CallbackType.Read);
			}
			ChangeNetworkPacketTimeout(-1, -1);
			if (TimeoutHasExpired)
			{
				OnTimeoutSync();
			}
			int num = Interlocked.CompareExchange(ref _timeoutState, 0, 1);
			if (_timeoutState != 1)
			{
				_timeoutIdentityValue = 0;
			}
			ProcessSniPacket(packet, error);
		}
		catch (Exception e)
		{
			flag = ADP.IsCatchableExceptionType(e);
			throw;
		}
		finally
		{
			int num2 = DecrementPendingCallbacks(release: false);
			if (flag && source != null && num2 < 2)
			{
				if (error == 0)
				{
					if (_executionContext != null)
					{
						ExecutionContext.Run(_executionContext, delegate
						{
							source.TrySetResult(null);
						}, null);
					}
					else
					{
						source.TrySetResult(null);
					}
				}
				else if (_executionContext != null)
				{
					ExecutionContext.Run(_executionContext, delegate
					{
						ReadAsyncCallbackCaptureException(source);
					}, null);
				}
				else
				{
					ReadAsyncCallbackCaptureException(source);
				}
			}
		}
	}

	private void ReadAsyncCallbackCaptureException(TaskCompletionSource<object> source)
	{
		bool flag = false;
		try
		{
			if (_hasErrorOrWarning)
			{
				ThrowExceptionAndWarning(callerHasConnectionLock: false, asyncClose: true);
			}
			else if (_parser.State == TdsParserState.Closed || _parser.State == TdsParserState.Broken)
			{
				throw ADP.ClosedConnectionError();
			}
		}
		catch (Exception exception)
		{
			if (source.TrySetException(exception))
			{
				flag = true;
			}
		}
		if (!flag)
		{
			Task.Factory.StartNew(delegate
			{
				_parser.State = TdsParserState.Broken;
				_parser.Connection.BreakConnection();
				source.TrySetCanceled();
			});
		}
	}

	public void WriteAsyncCallback(IntPtr key, IntPtr packet, uint sniError)
	{
		RemovePacketFromPendingList(packet);
		try
		{
			if (sniError != 0)
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.WriteAsyncCallback|Info> write async returned error code {0}", (int)sniError);
				try
				{
					AddError(_parser.ProcessSNIError(this));
					ThrowExceptionAndWarning(callerHasConnectionLock: false, asyncClose: true);
				}
				catch (Exception ex)
				{
					TaskCompletionSource<object> writeCompletionSource = _writeCompletionSource;
					if (writeCompletionSource != null)
					{
						writeCompletionSource.TrySetException(ex);
						return;
					}
					_delayedWriteAsyncCallbackException = ex;
					Thread.MemoryBarrier();
					writeCompletionSource = _writeCompletionSource;
					if (writeCompletionSource != null)
					{
						Exception ex2 = Interlocked.Exchange(ref _delayedWriteAsyncCallbackException, null);
						if (ex2 != null)
						{
							writeCompletionSource.TrySetException(ex2);
						}
					}
					return;
				}
			}
			else
			{
				_lastSuccessfulIOTimer._value = DateTime.UtcNow.Ticks;
			}
		}
		finally
		{
			Interlocked.Decrement(ref _asyncWriteCount);
		}
		TaskCompletionSource<object> writeCompletionSource2 = _writeCompletionSource;
		if (_asyncWriteCount == 0)
		{
			writeCompletionSource2?.TrySetResult(null);
		}
	}

	internal void WriteSecureString(SecureString secureString)
	{
		int num = ((_securePasswords[0] != null) ? 1 : 0);
		_securePasswords[num] = secureString;
		_securePasswordOffsetsInBuffer[num] = _outBytesUsed;
		int num2 = secureString.Length * 2;
		_outBytesUsed += num2;
	}

	internal void ResetSecurePasswordsInfomation()
	{
		for (int i = 0; i < _securePasswords.Length; i++)
		{
			_securePasswords[i] = null;
			_securePasswordOffsetsInBuffer[i] = 0;
		}
	}

	internal Task WaitForAccumulatedWrites()
	{
		Exception ex = Interlocked.Exchange(ref _delayedWriteAsyncCallbackException, null);
		if (ex != null)
		{
			throw ex;
		}
		if (_asyncWriteCount == 0)
		{
			return null;
		}
		_writeCompletionSource = new TaskCompletionSource<object>();
		Task task = _writeCompletionSource.Task;
		Thread.MemoryBarrier();
		if (_parser.State == TdsParserState.Closed || _parser.State == TdsParserState.Broken)
		{
			throw ADP.ClosedConnectionError();
		}
		ex = Interlocked.Exchange(ref _delayedWriteAsyncCallbackException, null);
		if (ex != null)
		{
			throw ex;
		}
		if (_asyncWriteCount == 0 && (!task.IsCompleted || task.Exception == null))
		{
			task = null;
		}
		return task;
	}

	internal void WriteByte(byte b)
	{
		if (_outBytesUsed == _outBuff.Length)
		{
			WritePacket(0, canAccumulate: true);
		}
		_outBuff[_outBytesUsed++] = b;
	}

	internal Task WriteByteArray(byte[] b, int len, int offsetBuffer, bool canAccumulate = true, TaskCompletionSource<object> completion = null)
	{
		try
		{
			bool asyncWrite = _parser._asyncWrite;
			int num = offsetBuffer;
			do
			{
				if (_outBytesUsed + len > _outBuff.Length)
				{
					int num2 = _outBuff.Length - _outBytesUsed;
					Buffer.BlockCopy(b, num, _outBuff, _outBytesUsed, num2);
					num += num2;
					_outBytesUsed += num2;
					len -= num2;
					Task task = WritePacket(0, canAccumulate);
					if (task != null)
					{
						Task result = null;
						if (completion == null)
						{
							completion = new TaskCompletionSource<object>();
							result = completion.Task;
						}
						WriteByteArraySetupContinuation(b, len, completion, num, task);
						return result;
					}
					continue;
				}
				Buffer.BlockCopy(b, num, _outBuff, _outBytesUsed, len);
				_outBytesUsed += len;
				break;
			}
			while (len > 0);
			completion?.SetResult(null);
			return null;
		}
		catch (Exception exception)
		{
			if (completion != null)
			{
				completion.SetException(exception);
				return null;
			}
			throw;
		}
	}

	private void WriteByteArraySetupContinuation(byte[] b, int len, TaskCompletionSource<object> completion, int offset, Task packetTask)
	{
		AsyncHelper.ContinueTask(packetTask, completion, delegate
		{
			WriteByteArray(b, len, offset, canAccumulate: false, completion);
		}, _parser.Connection);
	}

	internal Task WritePacket(byte flushMode, bool canAccumulate = false)
	{
		TdsParserState state = _parser.State;
		if (state == TdsParserState.Closed || state == TdsParserState.Broken)
		{
			throw ADP.ClosedConnectionError();
		}
		if ((state == TdsParserState.OpenLoggedIn && !_bulkCopyOpperationInProgress && _outBytesUsed == _outputHeaderLen + BitConverter.ToInt32(_outBuff, _outputHeaderLen) && _outputPacketCount == 0) || (_outBytesUsed == _outputHeaderLen && _outputPacketCount == 0))
		{
			return null;
		}
		byte outputPacketNumber = _outputPacketNumber;
		bool flag = _cancelled && _parser._asyncWrite;
		byte b;
		if (flag)
		{
			b = 3;
			ResetPacketCounters();
		}
		else if (1 == flushMode)
		{
			b = 1;
			ResetPacketCounters();
		}
		else if (flushMode == 0)
		{
			b = 4;
			_outputPacketNumber++;
			_outputPacketCount++;
		}
		else
		{
			b = 1;
		}
		_outBuff[0] = _outputMessageType;
		_outBuff[1] = b;
		_outBuff[2] = (byte)(_outBytesUsed >> 8);
		_outBuff[3] = (byte)((uint)_outBytesUsed & 0xFFu);
		_outBuff[4] = 0;
		_outBuff[5] = 0;
		_outBuff[6] = outputPacketNumber;
		_outBuff[7] = 0;
		Task task = null;
		_parser.CheckResetConnection(this);
		task = WriteSni(canAccumulate);
		if (flag)
		{
			task = AsyncHelper.CreateContinuationTask(task, CancelWritePacket, _parser.Connection);
		}
		return task;
	}

	private void CancelWritePacket()
	{
		_parser.Connection.ThreadHasParserLockForClose = true;
		try
		{
			SendAttention();
			ResetCancelAndProcessAttention();
			throw SQL.OperationCancelled();
		}
		finally
		{
			_parser.Connection.ThreadHasParserLockForClose = false;
		}
	}

	private Task SNIWritePacket(SNIHandle handle, SNIPacket packet, out uint sniError, bool canAccumulate, bool callerHasConnectionLock)
	{
		Exception ex = Interlocked.Exchange(ref _delayedWriteAsyncCallbackException, null);
		if (ex != null)
		{
			throw ex;
		}
		Task task = null;
		_writeCompletionSource = null;
		IntPtr pointer = IntPtr.Zero;
		bool flag = !_parser._asyncWrite;
		if (flag && _asyncWriteCount > 0)
		{
			Task task2 = WaitForAccumulatedWrites();
			if (task2 != null)
			{
				try
				{
					task2.Wait();
				}
				catch (AggregateException ex2)
				{
					throw ex2.InnerException;
				}
			}
		}
		if (!flag)
		{
			pointer = AddPacketToPendingList(packet);
		}
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
		}
		finally
		{
			sniError = SNINativeMethodWrapper.SNIWritePacket(handle, packet, flag);
		}
		if (sniError == 997)
		{
			Interlocked.Increment(ref _asyncWriteCount);
			if (!canAccumulate)
			{
				_writeCompletionSource = new TaskCompletionSource<object>();
				task = _writeCompletionSource.Task;
				Thread.MemoryBarrier();
				ex = Interlocked.Exchange(ref _delayedWriteAsyncCallbackException, null);
				if (ex != null)
				{
					throw ex;
				}
				if (_asyncWriteCount == 0 && (!task.IsCompleted || task.Exception == null))
				{
					task = null;
				}
			}
		}
		else
		{
			if (_parser.MARSOn)
			{
				CheckSetResetConnectionState(sniError, CallbackType.Write);
			}
			if (sniError == 0)
			{
				_lastSuccessfulIOTimer._value = DateTime.UtcNow.Ticks;
				if (!flag)
				{
					RemovePacketFromPendingList(pointer);
				}
			}
			else
			{
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.WritePacket|Info> write async returned error code {0}", (int)sniError);
				AddError(_parser.ProcessSNIError(this));
				ThrowExceptionAndWarning(callerHasConnectionLock);
			}
		}
		return task;
	}

	internal void SendAttention(bool mustTakeWriteLock = false)
	{
		if (_attentionSent || _parser.State == TdsParserState.Closed || _parser.State == TdsParserState.Broken)
		{
			return;
		}
		SNIPacket packet = (_sniAsyncAttnPacket = new SNIPacket(Handle));
		SNINativeMethodWrapper.SNIPacketSetData(packet, SQL.AttentionHeader, 8, null, null);
		RuntimeHelpers.PrepareConstrainedRegions();
		try
		{
			_attentionSending = true;
			bool flag = false;
			if (mustTakeWriteLock && !_parser.Connection.ThreadHasParserLockForClose)
			{
				flag = true;
				_parser.Connection._parserLock.Wait(canReleaseFromAnyThread: false);
				_parser.Connection.ThreadHasParserLockForClose = true;
			}
			try
			{
				if (_parser.State == TdsParserState.Closed || _parser.State == TdsParserState.Broken)
				{
					return;
				}
				_parser._asyncWrite = false;
				SNIWritePacket(Handle, packet, out var _, canAccumulate: false, callerHasConnectionLock: false);
				SqlClientEventSource.Log.TryTraceEvent("<sc.TdsParser.SendAttention|{0}> Send Attention ASync.", "Info");
			}
			finally
			{
				if (flag)
				{
					_parser.Connection.ThreadHasParserLockForClose = false;
					_parser.Connection._parserLock.Release();
				}
			}
			SetTimeoutSeconds(5);
			_attentionSent = true;
		}
		finally
		{
			_attentionSending = false;
		}
		SqlClientEventSource.Log.TryAdvancedTraceBinEvent("TdsParser.WritePacket | INFO | ADV | State Object Id {0}, Packet sent. Out Buffer {1}, Out Bytes Used {2}", ObjectID, _outBuff, (ushort)_outBytesUsed);
		SqlClientEventSource.Log.TryTraceEvent("TdsParser.SendAttention | INFO | Attention sent to the server.");
	}

	private Task WriteSni(bool canAccumulate)
	{
		SNIPacket resetWritePacket = GetResetWritePacket();
		SNINativeMethodWrapper.SNIPacketSetData(resetWritePacket, _outBuff, _outBytesUsed, _securePasswords, _securePasswordOffsetsInBuffer);
		uint sniError;
		Task result = SNIWritePacket(Handle, resetWritePacket, out sniError, canAccumulate, callerHasConnectionLock: true);
		if (_bulkCopyOpperationInProgress && GetTimeoutRemaining() == 0)
		{
			_parser.Connection.ThreadHasParserLockForClose = true;
			try
			{
				AddError(new SqlError(-2, 0, 11, _parser.Server, _parser.Connection.TimeoutErrorInternal.GetErrorMessage(), "", 0, 258u));
				_bulkCopyWriteTimeout = true;
				SendAttention();
				_parser.ProcessPendingAck(this);
				ThrowExceptionAndWarning();
			}
			finally
			{
				_parser.Connection.ThreadHasParserLockForClose = false;
			}
		}
		if (_parser.State == TdsParserState.OpenNotLoggedIn && (_parser.EncryptionOptions & EncryptionOptions.OPTIONS_MASK) == EncryptionOptions.LOGIN)
		{
			_parser.RemoveEncryption();
			_parser.EncryptionOptions = EncryptionOptions.OFF | (_parser.EncryptionOptions & (EncryptionOptions)(-64));
			ClearAllWritePackets();
		}
		SniWriteStatisticsAndTracing();
		ResetBuffer();
		return result;
	}

	internal SNIPacket GetResetWritePacket()
	{
		if (_sniPacket != null)
		{
			SNINativeMethodWrapper.SNIPacketReset(Handle, SNINativeMethodWrapper.IOType.WRITE, _sniPacket, SNINativeMethodWrapper.ConsumerNumber.SNI_Consumer_SNI);
		}
		else
		{
			lock (_writePacketLockObject)
			{
				_sniPacket = _writePacketCache.Take(Handle);
			}
		}
		return _sniPacket;
	}

	internal void ClearAllWritePackets()
	{
		if (_sniPacket != null)
		{
			_sniPacket.Dispose();
			_sniPacket = null;
		}
		lock (_writePacketLockObject)
		{
			_writePacketCache.Clear();
		}
	}

	private IntPtr AddPacketToPendingList(SNIPacket packet)
	{
		_sniPacket = null;
		IntPtr intPtr = packet.DangerousGetHandle();
		lock (_writePacketLockObject)
		{
			_pendingWritePackets.Add(intPtr, packet);
			return intPtr;
		}
	}

	private void RemovePacketFromPendingList(IntPtr pointer)
	{
		lock (_writePacketLockObject)
		{
			if (_pendingWritePackets.TryGetValue(pointer, out var value))
			{
				_pendingWritePackets.Remove(pointer);
				_writePacketCache.Add(value);
			}
		}
	}

	private void SniReadStatisticsAndTracing()
	{
		SqlStatistics statistics = Parser.Statistics;
		if (statistics != null)
		{
			if (statistics.WaitForReply)
			{
				statistics.SafeIncrement(ref statistics._serverRoundtrips);
				statistics.ReleaseAndUpdateNetworkServerTimer();
			}
			statistics.SafeAdd(ref statistics._bytesReceived, _inBytesRead);
			statistics.SafeIncrement(ref statistics._buffersReceived);
		}
	}

	private void SniWriteStatisticsAndTracing()
	{
		SqlStatistics statistics = _parser.Statistics;
		if (statistics != null)
		{
			statistics.SafeIncrement(ref statistics._buffersSent);
			statistics.SafeAdd(ref statistics._bytesSent, _outBytesUsed);
			statistics.RequestNetworkServerTimer();
		}
		if (SqlClientEventSource.Log.IsAdvancedTraceOn())
		{
			if (_tracePasswordOffset != 0)
			{
				for (int i = _tracePasswordOffset; i < _tracePasswordOffset + _tracePasswordLength; i++)
				{
					_outBuff[i] = 0;
				}
				_tracePasswordOffset = 0;
				_tracePasswordLength = 0;
			}
			if (_traceChangePasswordOffset != 0)
			{
				for (int j = _traceChangePasswordOffset; j < _traceChangePasswordOffset + _traceChangePasswordLength; j++)
				{
					_outBuff[j] = 0;
				}
				_traceChangePasswordOffset = 0;
				_traceChangePasswordLength = 0;
			}
		}
		SqlClientEventSource.Log.TryAdvancedTraceBinEvent("TdsParser.WritePacket | INFO | ADV | State Object Id {0}, Packet sent. Out buffer: {1}, Out Bytes Used: {2}", ObjectID, _outBuff, (ushort)_outBytesUsed);
	}

	[Conditional("DEBUG")]
	private void AssertValidState()
	{
		if (_inBytesUsed >= 0 && _inBytesRead >= 0)
		{
			_ = _inBytesUsed;
			_ = _inBytesRead;
		}
	}

	internal void AddError(SqlError error)
	{
		_syncOverAsync = true;
		lock (_errorAndWarningsLock)
		{
			_hasErrorOrWarning = true;
			if (_errors == null)
			{
				_errors = new SqlErrorCollection();
			}
			_errors.Add(error);
		}
	}

	internal void AddWarning(SqlError error)
	{
		_syncOverAsync = true;
		lock (_errorAndWarningsLock)
		{
			_hasErrorOrWarning = true;
			if (_warnings == null)
			{
				_warnings = new SqlErrorCollection();
			}
			_warnings.Add(error);
		}
	}

	internal SqlErrorCollection GetFullErrorAndWarningCollection(out bool broken)
	{
		SqlErrorCollection collectionToAddTo = new SqlErrorCollection();
		broken = false;
		lock (_errorAndWarningsLock)
		{
			_hasErrorOrWarning = false;
			AddErrorsToCollection(_errors, ref collectionToAddTo, ref broken);
			AddErrorsToCollection(_warnings, ref collectionToAddTo, ref broken);
			_errors = null;
			_warnings = null;
			AddErrorsToCollection(_preAttentionErrors, ref collectionToAddTo, ref broken);
			AddErrorsToCollection(_preAttentionWarnings, ref collectionToAddTo, ref broken);
			_preAttentionErrors = null;
			_preAttentionWarnings = null;
			return collectionToAddTo;
		}
	}

	private void AddErrorsToCollection(SqlErrorCollection inCollection, ref SqlErrorCollection collectionToAddTo, ref bool broken)
	{
		if (inCollection == null)
		{
			return;
		}
		foreach (SqlError item in inCollection)
		{
			collectionToAddTo.Add(item);
			broken |= item.Class >= 20;
		}
	}

	internal void StoreErrorAndWarningForAttention()
	{
		lock (_errorAndWarningsLock)
		{
			_hasErrorOrWarning = false;
			_preAttentionErrors = _errors;
			_preAttentionWarnings = _warnings;
			_errors = null;
			_warnings = null;
		}
	}

	internal void RestoreErrorAndWarningAfterAttention()
	{
		lock (_errorAndWarningsLock)
		{
			_hasErrorOrWarning = (_preAttentionErrors != null && _preAttentionErrors.Count > 0) || (_preAttentionWarnings != null && _preAttentionWarnings.Count > 0);
			_errors = _preAttentionErrors;
			_warnings = _preAttentionWarnings;
			_preAttentionErrors = null;
			_preAttentionWarnings = null;
		}
	}

	internal void CheckThrowSNIException()
	{
		if (HasErrorOrWarning)
		{
			ThrowExceptionAndWarning();
		}
	}

	[Conditional("DEBUG")]
	internal void AssertStateIsClean()
	{
		TdsParser parser = _parser;
		if (parser != null && parser.State != 0)
		{
			_ = parser.State;
			_ = 3;
		}
	}

	internal void CloneCleanupAltMetaDataSetArray()
	{
		if (_snapshot != null)
		{
			_snapshot.CloneCleanupAltMetaDataSetArray();
		}
	}
}
