using System;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

[Flags]
public enum SqlLoginOptionType
{
	None = 0,
	Hashed = 1,
	MustChange = 2,
	Sid = 4,
	DefaultDatabase = 8,
	DefaultLanguage = 0x10,
	Credential = 0x20,
	CheckExpiration = 0x40,
	CheckPolicy = 0x80,
	Name = 0x100,
	Password = 0x200,
	NoCredential = 0x400,
	Invalid = 0x8000,
	Unrecognized = 0x8001,
	InvalidValue = 0x8002
}
