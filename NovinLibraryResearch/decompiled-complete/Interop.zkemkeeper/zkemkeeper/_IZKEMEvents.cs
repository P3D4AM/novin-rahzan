using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace zkemkeeper;

[ComImport]
[Guid("CF83B580-5D32-4C65-B44E-BEDC750CDFA8")]
[InterfaceType(2)]
[TypeLibType(4096)]
public interface _IZKEMEvents
{
	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1)]
	void OnAttTransaction([In] int EnrollNumber, [In] int IsInValid, [In] int AttState, [In] int VerifyMethod, [In] int Year, [In] int Month, [In] int Day, [In] int Hour, [In] int Minute, [In] int Second);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(2)]
	void OnKeyPress([In] int Key);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(3)]
	void OnEnrollFinger([In] int EnrollNumber, [In] int FingerIndex, [In] int ActionResult, [In] int TemplateLength);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(4)]
	void OnNewUser([In] int EnrollNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(5)]
	void OnEMData([In] int DataType, [In] int DataLen, [In] ref sbyte DataBuffer);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(6)]
	void OnConnected();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(7)]
	void OnDisConnected();

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(8)]
	void OnFinger();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(9)]
	void OnVerify([In] int UserID);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10)]
	void OnFingerFeature([In] int Score);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(11)]
	void OnHIDNum([In] int CardNumber);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(12)]
	void OnDoor([In] int EventType);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(13)]
	void OnAlarm([In] int AlarmType, [In] int EnrollNumber, [In] int Verified);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(14)]
	void OnWriteCard([In] int EnrollNumber, [In] int ActionResult, [In] int Length);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(15)]
	void OnEmptyCard([In] int ActionResult);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(16)]
	void OnDeleteTemplate([In] int EnrollNumber, [In] int FingerIndex);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(17)]
	void OnAttTransactionEx([In][MarshalAs(UnmanagedType.BStr)] string EnrollNumber, [In] int IsInValid, [In] int AttState, [In] int VerifyMethod, [In] int Year, [In] int Month, [In] int Day, [In] int Hour, [In] int Minute, [In] int Second, [In] int WorkCode);

	[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(18)]
	void OnEnrollFingerEx([In][MarshalAs(UnmanagedType.BStr)] string EnrollNumber, [In] int FingerIndex, [In] int ActionResult, [In] int TemplateLength);
}
