using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[Flags]
public enum SqlForXmlDirectivesType : short
{
	None = 0,
	BinaryBase64 = 1,
	Type = 2,
	Root = 4,
	XmlData = 8,
	XmlSchema = 0x10,
	ElementsXsiNil = 0x20,
	ElementsAbsent = 0x40,
	Elements = 0x80,
	IncludeNullValues = 0x100,
	WithoutArrayWrapper = 0x200,
	CommonDirectives = 7,
	AutoMode = 0xFF,
	RawMode = 0xFF,
	ExplicitMode = 0xF,
	PathMode = 0xE7,
	JsonMode = 0x304
}
