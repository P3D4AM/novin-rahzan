using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using Microsoft.Data.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Data.SqlTypes;

public sealed class SqlFileStream : Stream
{
	private static int _objectTypeCount;

	internal readonly int ObjectID = Interlocked.Increment(ref _objectTypeCount);

	internal const int DefaultBufferSize = 1;

	private const ushort IoControlCodeFunctionCode = 2392;

	private FileStream m_fs;

	private string m_path;

	private byte[] m_txn;

	private bool m_disposed;

	private static readonly char[] InvalidPathChars = Path.GetInvalidPathChars();

	private const int MaxWin32PathLength = 32766;

	public string Name
	{
		get
		{
			return m_path;
		}
		private set
		{
			m_path = GetFullPathInternal(value);
		}
	}

	public byte[] TransactionContext
	{
		get
		{
			if (m_txn == null)
			{
				return null;
			}
			return (byte[])m_txn.Clone();
		}
		private set
		{
			m_txn = (byte[])value.Clone();
		}
	}

	public override bool CanRead
	{
		get
		{
			if (m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			return m_fs.CanRead;
		}
	}

	public override bool CanSeek
	{
		get
		{
			if (m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			return m_fs.CanSeek;
		}
	}

	[ComVisible(false)]
	public override bool CanTimeout
	{
		get
		{
			if (m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			return m_fs.CanTimeout;
		}
	}

	public override bool CanWrite
	{
		get
		{
			if (m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			return m_fs.CanWrite;
		}
	}

	public override long Length
	{
		get
		{
			if (m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			return m_fs.Length;
		}
	}

	public override long Position
	{
		get
		{
			if (m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			return m_fs.Position;
		}
		set
		{
			if (m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			m_fs.Position = value;
		}
	}

	[ComVisible(false)]
	public override int ReadTimeout
	{
		get
		{
			if (m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			return m_fs.ReadTimeout;
		}
		set
		{
			if (m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			m_fs.ReadTimeout = value;
		}
	}

	[ComVisible(false)]
	public override int WriteTimeout
	{
		get
		{
			if (m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			return m_fs.WriteTimeout;
		}
		set
		{
			if (m_disposed)
			{
				throw ADP.ObjectDisposed(this);
			}
			m_fs.WriteTimeout = value;
		}
	}

	public SqlFileStream(string path, byte[] transactionContext, FileAccess access)
		: this(path, transactionContext, access, FileOptions.None, 0L)
	{
	}

	public SqlFileStream(string path, byte[] transactionContext, FileAccess access, FileOptions options, long allocationSize)
	{
		long scopeId = SqlClientEventSource.Log.TryScopeEnterEvent("<sc.SqlFileStream.ctor|API> {0} access={1} options={2} path='{3}'", ObjectID, (int)access, (int)options, path);
		try
		{
			if (transactionContext == null)
			{
				throw ADP.ArgumentNull("transactionContext");
			}
			if (path == null)
			{
				throw ADP.ArgumentNull("path");
			}
			m_disposed = false;
			m_fs = null;
			OpenSqlFileStream(path, transactionContext, access, options, allocationSize);
			Name = path;
			TransactionContext = transactionContext;
		}
		finally
		{
			SqlClientEventSource.Log.TryScopeLeaveEvent(scopeId);
		}
	}

	~SqlFileStream()
	{
		Dispose(disposing: false);
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (m_disposed)
			{
				return;
			}
			try
			{
				if (disposing && m_fs != null)
				{
					m_fs.Close();
					m_fs = null;
				}
			}
			finally
			{
				m_disposed = true;
			}
		}
		finally
		{
			base.Dispose(disposing);
		}
	}

	public override void Flush()
	{
		if (m_disposed)
		{
			throw ADP.ObjectDisposed(this);
		}
		m_fs.Flush();
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		if (m_disposed)
		{
			throw ADP.ObjectDisposed(this);
		}
		return m_fs.BeginRead(buffer, offset, count, callback, state);
	}

	public override int EndRead(IAsyncResult asyncResult)
	{
		if (m_disposed)
		{
			throw ADP.ObjectDisposed(this);
		}
		return m_fs.EndRead(asyncResult);
	}

	[HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
	{
		if (m_disposed)
		{
			throw ADP.ObjectDisposed(this);
		}
		IAsyncResult result = m_fs.BeginWrite(buffer, offset, count, callback, state);
		if (count == 1)
		{
			m_fs.Flush();
		}
		return result;
	}

	public override void EndWrite(IAsyncResult asyncResult)
	{
		if (m_disposed)
		{
			throw ADP.ObjectDisposed(this);
		}
		m_fs.EndWrite(asyncResult);
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		if (m_disposed)
		{
			throw ADP.ObjectDisposed(this);
		}
		return m_fs.Seek(offset, origin);
	}

	public override void SetLength(long value)
	{
		if (m_disposed)
		{
			throw ADP.ObjectDisposed(this);
		}
		m_fs.SetLength(value);
	}

	public override int Read([In][Out] byte[] buffer, int offset, int count)
	{
		if (m_disposed)
		{
			throw ADP.ObjectDisposed(this);
		}
		return m_fs.Read(buffer, offset, count);
	}

	public override int ReadByte()
	{
		if (m_disposed)
		{
			throw ADP.ObjectDisposed(this);
		}
		return m_fs.ReadByte();
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		if (m_disposed)
		{
			throw ADP.ObjectDisposed(this);
		}
		m_fs.Write(buffer, offset, count);
		if (count == 1)
		{
			m_fs.Flush();
		}
	}

	public override void WriteByte(byte value)
	{
		if (m_disposed)
		{
			throw ADP.ObjectDisposed(this);
		}
		m_fs.WriteByte(value);
		m_fs.Flush();
	}

	[Conditional("DEBUG")]
	private static void AssertPathFormat(string path)
	{
	}

	private static string GetFullPathInternal(string path)
	{
		path = path.Trim();
		if (path.Length == 0)
		{
			throw ADP.Argument(StringsHelper.GetString(Strings.SqlFileStream_InvalidPath), "path");
		}
		if (path.Length > 32766)
		{
			throw ADP.Argument(StringsHelper.GetString(Strings.SqlFileStream_InvalidPath), "path");
		}
		if (path.IndexOfAny(InvalidPathChars) >= 0)
		{
			throw ADP.Argument(StringsHelper.GetString(Strings.SqlFileStream_InvalidPath), "path");
		}
		if (!path.StartsWith("\\\\", StringComparison.OrdinalIgnoreCase))
		{
			throw ADP.Argument(StringsHelper.GetString(Strings.SqlFileStream_InvalidPath), "path");
		}
		path = UnsafeNativeMethods.SafeGetFullPathName(path);
		if (path.StartsWith("\\\\.\\", StringComparison.Ordinal))
		{
			throw ADP.Argument(StringsHelper.GetString(Strings.SqlFileStream_PathNotValidDiskResource), "path");
		}
		return path;
	}

	private static void DemandAccessPermission(string path, FileAccess access)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		FileIOPermissionAccess val = (FileIOPermissionAccess)(access switch
		{
			FileAccess.Read => 1, 
			FileAccess.Write => 2, 
			_ => 3, 
		});
		bool flag = false;
		try
		{
			FileIOPermission val2 = new FileIOPermission(val, path);
			((CodeAccessPermission)val2).Demand();
		}
		catch (PathTooLongException e)
		{
			flag = true;
			ADP.TraceExceptionWithoutRethrow(e);
		}
		if (flag)
		{
			FileIOPermission val2 = new FileIOPermission(PermissionState.Unrestricted);
			val2.AllFiles = val;
			((CodeAccessPermission)val2).Demand();
		}
	}

	private void OpenSqlFileStream(string path, byte[] transactionContext, FileAccess access, FileOptions options, long allocationSize)
	{
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Expected O, but got Unknown
		if (access != FileAccess.Read && access != FileAccess.Write && access != FileAccess.ReadWrite)
		{
			throw ADP.ArgumentOutOfRange("access");
		}
		if ((options & (FileOptions)671088639) != 0)
		{
			throw ADP.ArgumentOutOfRange("options");
		}
		path = GetFullPathInternal(path);
		DemandAccessPermission(path, access);
		FileFullEaInformation fileFullEaInformation = null;
		SecurityQualityOfService securityQualityOfService = null;
		UnicodeString unicodeString = null;
		SafeFileHandle fileHandle = null;
		int num = 1048704;
		uint num2 = 0u;
		uint num3 = 0u;
		FileShare fileShare = FileShare.None;
		switch (access)
		{
		case FileAccess.Read:
			num |= 1;
			fileShare = FileShare.ReadWrite | FileShare.Delete;
			num3 = 1u;
			break;
		case FileAccess.Write:
			num |= 2;
			fileShare = FileShare.Read | FileShare.Delete;
			num3 = 4u;
			break;
		default:
			num |= 3;
			fileShare = FileShare.Read | FileShare.Delete;
			num3 = 4u;
			break;
		}
		if (((uint)options & 0x80000000u) != 0)
		{
			num2 |= 2u;
		}
		if ((options & FileOptions.Asynchronous) == 0)
		{
			num2 |= 0x20u;
		}
		if ((options & FileOptions.SequentialScan) != 0)
		{
			num2 |= 4u;
		}
		if ((options & FileOptions.RandomAccess) != 0)
		{
			num2 |= 0x800u;
		}
		try
		{
			fileFullEaInformation = new FileFullEaInformation(transactionContext);
			securityQualityOfService = new SecurityQualityOfService(UnsafeNativeMethods.SecurityImpersonationLevel.SecurityAnonymous, effectiveOnly: false, dynamicTrackingMode: false);
			string path2 = InitializeNtPath(path);
			unicodeString = new UnicodeString(path2);
			UnsafeNativeMethods.OBJECT_ATTRIBUTES objectAttributes = default(UnsafeNativeMethods.OBJECT_ATTRIBUTES);
			objectAttributes.length = Marshal.SizeOf(typeof(UnsafeNativeMethods.OBJECT_ATTRIBUTES));
			objectAttributes.rootDirectory = IntPtr.Zero;
			objectAttributes.attributes = 64;
			objectAttributes.securityDescriptor = IntPtr.Zero;
			objectAttributes.securityQualityOfService = securityQualityOfService;
			objectAttributes.objectName = unicodeString;
			uint num4 = 0u;
			UnsafeNativeMethods.SetErrorModeWrapper(1u, out var oldMode);
			try
			{
				SqlClientEventSource.Log.TryAdvancedTraceEvent("<sc.SqlFileStream.OpenSqlFileStream|ADV> {0}, desiredAccess=0x{1}, allocationSize={2}, fileAttributes=0x{3}, shareAccess=0x{4}, dwCreateDisposition=0x{5}, createOptions=0x{ dwCreateOptions}", ObjectID, num, allocationSize, 0, (int)fileShare, num3);
				num4 = UnsafeNativeMethods.NtCreateFile(out fileHandle, num, ref objectAttributes, out var _, ref allocationSize, 0u, fileShare, num3, num2, fileFullEaInformation, (uint)fileFullEaInformation.Length);
			}
			finally
			{
				UnsafeNativeMethods.SetErrorModeWrapper(oldMode, out oldMode);
			}
			switch (num4)
			{
			case 3221225539u:
				throw ADP.InvalidOperation(StringsHelper.GetString(Strings.SqlFileStream_FileAlreadyInTransaction));
			case 3221225485u:
				throw ADP.Argument(StringsHelper.GetString(Strings.SqlFileStream_InvalidParameter));
			case 3221225524u:
			{
				DirectoryNotFoundException ex3 = new DirectoryNotFoundException();
				ADP.TraceExceptionAsReturnValue(ex3);
				throw ex3;
			}
			default:
			{
				uint num5 = UnsafeNativeMethods.RtlNtStatusToDosError(num4);
				if (num5 == 317)
				{
					num5 = num4;
				}
				Win32Exception ex4 = new Win32Exception((int)num5);
				ADP.TraceExceptionAsReturnValue(ex4);
				throw ex4;
			}
			case 0u:
			{
				if (fileHandle.IsInvalid)
				{
					Win32Exception ex = new Win32Exception(6);
					ADP.TraceExceptionAsReturnValue(ex);
					throw ex;
				}
				UnsafeNativeMethods.FileType fileType = UnsafeNativeMethods.GetFileType(fileHandle);
				if (fileType != UnsafeNativeMethods.FileType.Disk)
				{
					fileHandle.Dispose();
					throw ADP.Argument(StringsHelper.GetString(Strings.SqlFileStream_PathNotValidDiskResource));
				}
				if (access == FileAccess.ReadWrite)
				{
					uint ioControlCode = UnsafeNativeMethods.CTL_CODE(9, 2392, 0, 0);
					uint cbBytesReturned = 0u;
					if (!UnsafeNativeMethods.DeviceIoControl(fileHandle, ioControlCode, IntPtr.Zero, 0u, IntPtr.Zero, 0u, out cbBytesReturned, IntPtr.Zero))
					{
						Win32Exception ex2 = new Win32Exception(Marshal.GetLastWin32Error());
						ADP.TraceExceptionAsReturnValue(ex2);
						throw ex2;
					}
				}
				bool flag = false;
				try
				{
					SecurityPermission val = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
					((CodeAccessPermission)val).Assert();
					flag = true;
					m_fs = new FileStream(fileHandle, access, 1, (options & FileOptions.Asynchronous) != 0);
					break;
				}
				finally
				{
					if (flag)
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
			}
		}
		catch
		{
			if (fileHandle != null && !fileHandle.IsInvalid)
			{
				fileHandle.Dispose();
			}
			throw;
		}
		finally
		{
			if (fileFullEaInformation != null)
			{
				fileFullEaInformation.Dispose();
				fileFullEaInformation = null;
			}
			if (securityQualityOfService != null)
			{
				securityQualityOfService.Dispose();
				securityQualityOfService = null;
			}
			if (unicodeString != null)
			{
				unicodeString.Dispose();
				unicodeString = null;
			}
		}
	}

	private static string InitializeNtPath(string path)
	{
		string format = "\\??\\UNC\\{0}\\{1}";
		string arg = Guid.NewGuid().ToString("N");
		return string.Format(CultureInfo.InvariantCulture, format, path.Trim(new char[1] { '\\' }), arg);
	}
}
