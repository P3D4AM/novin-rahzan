using SharpCompress.IO;

namespace SharpCompress.Common.Rar.Headers;

internal class SignHeader : RarHeader
{
	internal int CreationTime { get; private set; }

	internal short ArcNameSize { get; private set; }

	internal short UserNameSize { get; private set; }

	protected SignHeader(RarHeader header, RarCrcBinaryReader reader)
		: base(header, reader, HeaderType.Sign)
	{
		if (base.IsRar5)
		{
			throw new InvalidFormatException("unexpected rar5 record");
		}
	}

	protected override void ReadFinish(MarkingBinaryReader reader)
	{
		CreationTime = reader.ReadInt32();
		ArcNameSize = reader.ReadInt16();
		UserNameSize = reader.ReadInt16();
	}
}
