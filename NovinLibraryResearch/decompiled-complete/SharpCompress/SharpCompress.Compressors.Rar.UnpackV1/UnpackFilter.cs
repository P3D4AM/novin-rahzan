using SharpCompress.Compressors.Rar.VM;

namespace SharpCompress.Compressors.Rar.UnpackV1;

internal class UnpackFilter
{
	public byte Type;

	public byte Channels;

	internal uint uBlockStart
	{
		get
		{
			return (uint)BlockStart;
		}
		set
		{
			BlockStart = (int)value;
		}
	}

	internal uint uBlockLength
	{
		get
		{
			return (uint)BlockLength;
		}
		set
		{
			BlockLength = (int)value;
		}
	}

	internal int BlockStart { get; set; }

	internal int BlockLength { get; set; }

	internal int ExecCount { get; set; }

	internal bool NextWindow { get; set; }

	internal int ParentFilter { get; set; }

	internal VMPreparedProgram Program { get; set; }

	internal UnpackFilter()
	{
		Program = new VMPreparedProgram();
	}
}
