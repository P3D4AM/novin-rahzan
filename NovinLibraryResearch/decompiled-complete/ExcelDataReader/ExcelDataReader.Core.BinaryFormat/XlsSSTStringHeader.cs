using System;

namespace ExcelDataReader.Core.BinaryFormat;

internal class XlsSSTStringHeader
{
	[Flags]
	public enum FormattedUnicodeStringFlags : byte
	{
		MultiByte = 1,
		HasExtendedString = 4,
		HasFormatting = 8
	}

	private readonly byte[] _bytes;

	private readonly uint _offset;

	public ushort CharacterCount => BitConverter.ToUInt16(_bytes, (int)_offset);

	public FormattedUnicodeStringFlags Flags => (FormattedUnicodeStringFlags)Buffer.GetByte(_bytes, (int)(_offset + 2));

	public bool HasExtString => (Flags & FormattedUnicodeStringFlags.HasExtendedString) == FormattedUnicodeStringFlags.HasExtendedString;

	public bool HasFormatting => (Flags & FormattedUnicodeStringFlags.HasFormatting) == FormattedUnicodeStringFlags.HasFormatting;

	public bool IsMultiByte => (Flags & FormattedUnicodeStringFlags.MultiByte) == FormattedUnicodeStringFlags.MultiByte;

	public ushort FormatCount
	{
		get
		{
			if (!HasFormatting)
			{
				return 0;
			}
			return BitConverter.ToUInt16(_bytes, (int)(_offset + 3));
		}
	}

	public uint ExtendedStringSize
	{
		get
		{
			if (!HasExtString)
			{
				return 0u;
			}
			return BitConverter.ToUInt32(_bytes, (int)_offset + (HasFormatting ? 5 : 3));
		}
	}

	public uint HeadSize => (uint)((HasFormatting ? 2 : 0) + (HasExtString ? 4 : 0) + 3);

	public uint TailSize => (uint)(HasFormatting ? (4 * FormatCount) : 0) + (HasExtString ? ExtendedStringSize : 0);

	public XlsSSTStringHeader(byte[] bytes, uint offset)
	{
		_bytes = bytes;
		_offset = offset;
	}
}
