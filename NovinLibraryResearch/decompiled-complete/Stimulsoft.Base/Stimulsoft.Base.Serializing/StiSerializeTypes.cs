using System;

namespace Stimulsoft.Base.Serializing;

[Flags]
public enum StiSerializeTypes
{
	SerializeToAll = 0xF,
	SerializeToCode = 1,
	SerializeToDesigner = 2,
	SerializeToSaveLoad = 4,
	SerializeToDocument = 8,
	SerializeToAllExceptCode = 0xE,
	SerializeToAllExceptDocument = 7
}
