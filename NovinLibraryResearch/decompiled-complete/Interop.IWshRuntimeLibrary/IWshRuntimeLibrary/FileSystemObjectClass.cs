using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IWshRuntimeLibrary;

[ComImport]
[TypeLibType(2)]
[ClassInterface(0)]
[Guid("0D43FE01-F093-11CF-8940-00A0C9054228")]
public class FileSystemObjectClass : IFileSystem3, FileSystemObject
{
	[DispId(10010)]
	public virtual extern Drives Drives
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(10010)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10000)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string BuildPath([In][MarshalAs(UnmanagedType.BStr)] string Path, [In][MarshalAs(UnmanagedType.BStr)] string Name);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10004)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string GetDriveName([In][MarshalAs(UnmanagedType.BStr)] string Path);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10005)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string GetParentFolderName([In][MarshalAs(UnmanagedType.BStr)] string Path);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10006)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string GetFileName([In][MarshalAs(UnmanagedType.BStr)] string Path);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10007)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string GetBaseName([In][MarshalAs(UnmanagedType.BStr)] string Path);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10008)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string GetExtensionName([In][MarshalAs(UnmanagedType.BStr)] string Path);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10002)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string GetAbsolutePathName([In][MarshalAs(UnmanagedType.BStr)] string Path);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10003)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string GetTempName();

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10015)]
	public virtual extern bool DriveExists([In][MarshalAs(UnmanagedType.BStr)] string DriveSpec);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10016)]
	public virtual extern bool FileExists([In][MarshalAs(UnmanagedType.BStr)] string FileSpec);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10017)]
	public virtual extern bool FolderExists([In][MarshalAs(UnmanagedType.BStr)] string FolderSpec);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10011)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Drive GetDrive([In][MarshalAs(UnmanagedType.BStr)] string DriveSpec);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10012)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern File GetFile([In][MarshalAs(UnmanagedType.BStr)] string FilePath);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10013)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Folder GetFolder([In][MarshalAs(UnmanagedType.BStr)] string FolderPath);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(10014)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Folder GetSpecialFolder([In][ComAliasName("IWshRuntimeLibrary.SpecialFolderConst")] SpecialFolderConst SpecialFolder);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1200)]
	public virtual extern void DeleteFile([In][MarshalAs(UnmanagedType.BStr)] string FileSpec, [In] bool Force = false);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1201)]
	public virtual extern void DeleteFolder([In][MarshalAs(UnmanagedType.BStr)] string FolderSpec, [In] bool Force = false);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1204)]
	public virtual extern void MoveFile([In][MarshalAs(UnmanagedType.BStr)] string Source, [In][MarshalAs(UnmanagedType.BStr)] string Destination);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1205)]
	public virtual extern void MoveFolder([In][MarshalAs(UnmanagedType.BStr)] string Source, [In][MarshalAs(UnmanagedType.BStr)] string Destination);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1202)]
	public virtual extern void CopyFile([In][MarshalAs(UnmanagedType.BStr)] string Source, [In][MarshalAs(UnmanagedType.BStr)] string Destination, [In] bool OverWriteFiles = true);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1203)]
	public virtual extern void CopyFolder([In][MarshalAs(UnmanagedType.BStr)] string Source, [In][MarshalAs(UnmanagedType.BStr)] string Destination, [In] bool OverWriteFiles = true);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1120)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern Folder CreateFolder([In][MarshalAs(UnmanagedType.BStr)] string Path);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1101)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern TextStream CreateTextFile([In][MarshalAs(UnmanagedType.BStr)] string FileName, [In] bool Overwrite = true, [In] bool Unicode = false);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(1100)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern TextStream OpenTextFile([In][MarshalAs(UnmanagedType.BStr)] string FileName, [In] IOMode IOMode = IOMode.ForReading, [In] bool Create = false, [In] Tristate Format = Tristate.TristateFalse);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(20000)]
	[return: MarshalAs(UnmanagedType.Interface)]
	public virtual extern TextStream GetStandardStream([In][ComAliasName("IWshRuntimeLibrary.StandardStreamTypes")] StandardStreamTypes StandardStreamType, [In] bool Unicode = false);

	[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
	[DispId(20010)]
	[return: MarshalAs(UnmanagedType.BStr)]
	public virtual extern string GetFileVersion([In][MarshalAs(UnmanagedType.BStr)] string FileName);
}
