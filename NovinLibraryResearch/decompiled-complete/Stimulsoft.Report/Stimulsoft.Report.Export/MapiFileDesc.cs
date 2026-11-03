using System;
using System.Runtime.InteropServices;

namespace Stimulsoft.Report.Export;

[StructLayout(LayoutKind.Sequential)]
public class MapiFileDesc
{
	public int reserved;

	public int flags;

	public int position;

	public string pathName;

	public string fileName;

	public IntPtr fileType;
}
