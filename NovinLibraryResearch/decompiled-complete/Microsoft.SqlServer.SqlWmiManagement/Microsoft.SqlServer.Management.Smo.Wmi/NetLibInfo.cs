using System;

namespace Microsoft.SqlServer.Management.Smo.Wmi;

public sealed class NetLibInfo
{
	internal string fileName;

	internal string version;

	internal DateTime date;

	internal int size;

	public string FileName => fileName;

	public string Version => version;

	public DateTime Date => date;

	public int Size => size;

	internal NetLibInfo()
	{
	}
}
