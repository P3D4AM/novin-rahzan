using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ADODB;

[ComImport]
[ClassInterface(0)]
[TypeLibType(6)]
[ComSourceInterfaces("ADODB.RecordsetEvents\0\0")]
[DefaultMember("Fields")]
[Guid("00000535-0000-0010-8000-00AA006D2EA4")]
public class RecordsetClass : _Recordset, Recordset, RecordsetEvents_Event
{
	[DispId(500)]
	public virtual extern Properties Properties
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(500)]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "ADODB.PropertiesToInternalPropertiesMarshaler")]
		get;
	}

	[DispId(1000)]
	public virtual extern PositionEnum AbsolutePosition
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1000)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1000)]
		[param: In]
		set;
	}

	[DispId(1001)]
	public virtual extern object ActiveConnection
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1001)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1001)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}

	[DispId(1002)]
	public virtual extern bool BOF
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1002)]
		get;
	}

	[DispId(1003)]
	public virtual extern object Bookmark
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1003)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1003)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Struct)]
		set;
	}

	[DispId(1004)]
	public virtual extern int CacheSize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1004)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1004)]
		[param: In]
		set;
	}

	[DispId(1005)]
	public virtual extern CursorTypeEnum CursorType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1005)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1005)]
		[param: In]
		set;
	}

	[DispId(1006)]
	public virtual extern bool EOF
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1006)]
		get;
	}

	[DispId(0)]
	public virtual extern Fields Fields
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(0)]
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "ADODB.FieldsToInternalFieldsMarshaler")]
		get;
	}

	[DispId(1008)]
	public virtual extern LockTypeEnum LockType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1008)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1008)]
		[param: In]
		set;
	}

	[DispId(1009)]
	[ComAliasName("ADODB.ADO_LONGPTR")]
	public virtual extern int MaxRecords
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1009)]
		[return: ComAliasName("ADODB.ADO_LONGPTR")]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1009)]
		[param: In]
		[param: ComAliasName("ADODB.ADO_LONGPTR")]
		set;
	}

	[ComAliasName("ADODB.ADO_LONGPTR")]
	[DispId(1010)]
	public virtual extern int RecordCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1010)]
		[return: ComAliasName("ADODB.ADO_LONGPTR")]
		get;
	}

	[DispId(1011)]
	public virtual extern object Source
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1011)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1011)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IDispatch)]
		set;
	}

	[DispId(1047)]
	public virtual extern PositionEnum AbsolutePage
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1047)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1047)]
		[param: In]
		set;
	}

	[DispId(1026)]
	public virtual extern EditModeEnum EditMode
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1026)]
		get;
	}

	[DispId(1030)]
	public virtual extern object Filter
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1030)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1030)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Struct)]
		set;
	}

	[ComAliasName("ADODB.ADO_LONGPTR")]
	[DispId(1050)]
	public virtual extern int PageCount
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1050)]
		[return: ComAliasName("ADODB.ADO_LONGPTR")]
		get;
	}

	[DispId(1048)]
	public virtual extern int PageSize
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1048)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1048)]
		[param: In]
		set;
	}

	[DispId(1031)]
	public virtual extern string Sort
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1031)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1031)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(1029)]
	public virtual extern int Status
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1029)]
		get;
	}

	[DispId(1054)]
	public virtual extern int State
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1054)]
		get;
	}

	[DispId(1051)]
	public virtual extern CursorLocationEnum CursorLocation
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1051)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1051)]
		[param: In]
		set;
	}

	[DispId(-8)]
	public virtual extern object Collect
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[TypeLibFunc(64)]
		[DispId(-8)]
		[return: MarshalAs(UnmanagedType.Struct)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(-8)]
		[TypeLibFunc(64)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Struct)]
		set;
	}

	[DispId(1053)]
	public virtual extern MarshalOptionsEnum MarshalOptions
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1053)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1053)]
		[param: In]
		set;
	}

	[DispId(1056)]
	public virtual extern object DataSource
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1056)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1056)]
		[param: In]
		[param: MarshalAs(UnmanagedType.IUnknown)]
		set;
	}

	[DispId(1061)]
	public virtual extern object ActiveCommand
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1061)]
		[return: MarshalAs(UnmanagedType.IDispatch)]
		get;
	}

	[DispId(1063)]
	public virtual extern bool StayInSync
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1063)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1063)]
		[param: In]
		set;
	}

	[DispId(1064)]
	public virtual extern string DataMember
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1064)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1064)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	[DispId(1067)]
	public virtual extern string Index
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1067)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1067)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	public virtual extern event RecordsetEvents_WillChangeFieldEventHandler WillChangeField;

	public virtual extern event RecordsetEvents_FieldChangeCompleteEventHandler FieldChangeComplete;

	public virtual extern event RecordsetEvents_WillChangeRecordEventHandler WillChangeRecord;

	public virtual extern event RecordsetEvents_RecordChangeCompleteEventHandler RecordChangeComplete;

	public virtual extern event RecordsetEvents_WillChangeRecordsetEventHandler WillChangeRecordset;

	public virtual extern event RecordsetEvents_RecordsetChangeCompleteEventHandler RecordsetChangeComplete;

	public virtual extern event RecordsetEvents_WillMoveEventHandler WillMove;

	public virtual extern event RecordsetEvents_MoveCompleteEventHandler MoveComplete;

	public virtual extern event RecordsetEvents_EndOfRecordsetEventHandler EndOfRecordset;

	public virtual extern event RecordsetEvents_FetchProgressEventHandler FetchProgress;

	public virtual extern event RecordsetEvents_FetchCompleteEventHandler FetchComplete;

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[SpecialName]
	[DispId(1001)]
	public virtual extern void let_ActiveConnection([In][MarshalAs(UnmanagedType.Struct)] object pvar);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[SpecialName]
	[DispId(1011)]
	public virtual extern void let_Source([In][MarshalAs(UnmanagedType.BStr)] string pvSource);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1012)]
	public virtual extern void AddNew([Optional][In][MarshalAs(UnmanagedType.Struct)] object FieldList, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Values);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1013)]
	public virtual extern void CancelUpdate();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1014)]
	public virtual extern void Close();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1015)]
	public virtual extern void Delete([In] AffectEnum AffectRecords = AffectEnum.adAffectCurrent);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1016)]
	[return: MarshalAs(UnmanagedType.Struct)]
	public virtual extern object GetRows([In] int Rows = -1, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Start, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Fields);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1017)]
	public virtual extern void Move([In][ComAliasName("ADODB.ADO_LONGPTR")] int NumRecords, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Start);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1018)]
	public virtual extern void MoveNext();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1019)]
	public virtual extern void MovePrevious();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1020)]
	public virtual extern void MoveFirst();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1021)]
	public virtual extern void MoveLast();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1022)]
	public virtual extern void Open([Optional][In][MarshalAs(UnmanagedType.Struct)] object Source, [Optional][In][MarshalAs(UnmanagedType.Struct)] object ActiveConnection, [In] CursorTypeEnum CursorType = CursorTypeEnum.adOpenUnspecified, [In] LockTypeEnum LockType = LockTypeEnum.adLockUnspecified, [In] int Options = -1);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1023)]
	public virtual extern void Requery([In] int Options = -1);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1610809378)]
	[TypeLibFunc(64)]
	public virtual extern void _xResync([In] AffectEnum AffectRecords = AffectEnum.adAffectAll);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1025)]
	public virtual extern void Update([Optional][In][MarshalAs(UnmanagedType.Struct)] object Fields, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Values);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[TypeLibFunc(64)]
	[DispId(1610809392)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Recordset _xClone();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1035)]
	public virtual extern void UpdateBatch([In] AffectEnum AffectRecords = AffectEnum.adAffectAll);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1049)]
	public virtual extern void CancelBatch([In] AffectEnum AffectRecords = AffectEnum.adAffectAll);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1052)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Recordset NextRecordset([Optional][MarshalAs(UnmanagedType.Struct)] out object RecordsAffected);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1036)]
	public virtual extern bool Supports([In] CursorOptionEnum CursorOptions);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1058)]
	public virtual extern void Find([In][MarshalAs(UnmanagedType.BStr)] string Criteria, [In][ComAliasName("ADODB.ADO_LONGPTR")] int SkipRecords = 0, [In] SearchDirectionEnum SearchDirection = SearchDirectionEnum.adSearchForward, [Optional][In][MarshalAs(UnmanagedType.Struct)] object Start);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1055)]
	public virtual extern void Cancel();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[TypeLibFunc(64)]
	[DispId(1610874883)]
	public virtual extern void _xSave([In][MarshalAs(UnmanagedType.BStr)] string FileName = "", [In] PersistFormatEnum PersistFormat = PersistFormatEnum.adPersistADTG);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1062)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string GetString([In] StringFormatEnum StringFormat = StringFormatEnum.adClipString, [In] int NumRows = -1, [In][MarshalAs(UnmanagedType.BStr)] string ColumnDelimeter = "", [In][MarshalAs(UnmanagedType.BStr)] string RowDelimeter = "", [In][MarshalAs(UnmanagedType.BStr)] string NullExpr = "");

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1065)]
	public virtual extern CompareEnum CompareBookmarks([In][MarshalAs(UnmanagedType.Struct)] object Bookmark1, [In][MarshalAs(UnmanagedType.Struct)] object Bookmark2);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1034)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Recordset Clone([In] LockTypeEnum LockType = LockTypeEnum.adLockUnspecified);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1024)]
	public virtual extern void Resync([In] AffectEnum AffectRecords = AffectEnum.adAffectAll, [In] ResyncEnum ResyncValues = ResyncEnum.adResyncAllValues);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1066)]
	public virtual extern void Seek([In][MarshalAs(UnmanagedType.Struct)] object KeyValues, [In] SeekEnum SeekOption = SeekEnum.adSeekFirstEQ);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1057)]
	public virtual extern void Save([Optional][In][MarshalAs(UnmanagedType.Struct)] object Destination, [In] PersistFormatEnum PersistFormat = PersistFormatEnum.adPersistADTG);
}
