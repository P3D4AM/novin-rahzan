using System;
using System.Runtime.InteropServices;

namespace Stimulsoft.Report.Export;

[StructLayout(LayoutKind.Sequential)]
public class MapiRecipDesc
{
	public int reserved;

	public int recipClass;

	public string name;

	public string address;

	public int eIDSize;

	public IntPtr entryID;
}
