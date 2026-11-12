using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ADODB;

[ComImport]
[DefaultMember("ConnectionString")]
[ClassInterface(0)]
[Guid("00000514-0000-0010-8000-00AA006D2EA4")]
[TypeLibType(6)]
[ComSourceInterfaces("ADODB.ConnectionEvents\0\0")]
public class ConnectionClass : _Connection, Connection, ConnectionEvents_Event
{
	[DispId(500)]
	public virtual extern Properties Properties
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(500)]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "ADODB.PropertiesToInternalPropertiesMarshaler")]
		get;
	}

	[DispId(0)]
	public virtual extern string ConnectionString
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(2)]
	public virtual extern int CommandTimeout
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[param: In]
		set;
	}

	[DispId(3)]
	public virtual extern int ConnectionTimeout
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3)]
		[param: In]
		set;
	}

	[DispId(4)]
	public virtual extern string Version
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
	}

	[DispId(11)]
	public virtual extern Errors Errors
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(11)]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "ADODB.ErrorsToInternalErrorsMarshaler")]
		get;
	}

	[DispId(12)]
	public virtual extern string DefaultDatabase
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(12)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(12)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(13)]
	public virtual extern IsolationLevelEnum IsolationLevel
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(13)]
		[param: In]
		set;
	}

	[DispId(14)]
	public virtual extern int Attributes
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(14)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(14)]
		[param: In]
		set;
	}

	[DispId(15)]
	public virtual extern CursorLocationEnum CursorLocation
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(15)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(15)]
		[param: In]
		set;
	}

	[DispId(16)]
	public virtual extern ConnectModeEnum Mode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(16)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(16)]
		[param: In]
		set;
	}

	[DispId(17)]
	public virtual extern string Provider
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(17)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(17)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(18)]
	public virtual extern int State
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(18)]
		get;
	}

	public virtual extern event ConnectionEvents_InfoMessageEventHandler InfoMessage;

	public virtual extern event ConnectionEvents_BeginTransCompleteEventHandler BeginTransComplete;

	public virtual extern event ConnectionEvents_CommitTransCompleteEventHandler CommitTransComplete;

	public virtual extern event ConnectionEvents_RollbackTransCompleteEventHandler RollbackTransComplete;

	public virtual extern event ConnectionEvents_WillExecuteEventHandler WillExecute;

	public virtual extern event ConnectionEvents_ExecuteCompleteEventHandler ExecuteComplete;

	public virtual extern event ConnectionEvents_WillConnectEventHandler WillConnect;

	public virtual extern event ConnectionEvents_ConnectCompleteEventHandler ConnectComplete;

	public virtual extern event ConnectionEvents_DisconnectEventHandler Disconnect;

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5)]
	public virtual extern void Close();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Recordset Execute([In][MarshalAs(UnmanagedType.BStr)] string CommandText, [Optional][MarshalAs(UnmanagedType.Struct)] out object RecordsAffected, [In] int Options = -1);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(7)]
	public virtual extern int BeginTrans();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8)]
	public virtual extern void CommitTrans();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(9)]
	public virtual extern void RollbackTrans();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10)]
	public virtual extern void Open([In][MarshalAs(UnmanagedType.BStr)] string ConnectionString = "", [In][MarshalAs(UnmanagedType.BStr)] string UserID = "", [In][MarshalAs(UnmanagedType.BStr)] string Password = "", [In] int Options = -1);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(19)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Recordset OpenSchema([In] SchemaEnum Schema, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Restrictions, [Optional][In][MarshalAs(UnmanagedType.Struct)] object SchemaID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(21)]
	public virtual extern void Cancel();
}
