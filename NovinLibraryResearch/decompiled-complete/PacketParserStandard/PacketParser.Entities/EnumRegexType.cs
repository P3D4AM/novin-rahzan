using System.ComponentModel;

namespace PacketParser.Entities;

public enum EnumRegexType
{
	[Description("Write on[\\s'\"]*([\\w\\.:()-]+)['\"\\s\\w,.]*|file[\\s'\"]*([\\w\\.:()-]+)[\\s'\".,]*")]
	WriteOnFileDetection,
	[Description("assembly[\\s'\"]*([\\w\\.:()-]+)['\"\\s\\w,.]*Version[\\s='\"]*([\\w\\.:()-]+)[\\s'\".,]*")]
	AssemblyDetection,
	[Description("file not found[\\s'\":]*([\\w\\.:()-]+)[\\s'\".,]*")]
	FileNotFoundDetection,
	[Description("font[\\s'\"]*([[\\s\\w-]+)[\\s'\".,]*")]
	FontDetection,
	[Description("in database[\\s'\"]*([\\w\\.:()-]+)[\\s'\".,]")]
	DatabaseDetection,
	[Description("object[\\s'\"]*([\\w\\.:()-]+)['\"\\s\\w,.]*because")]
	NotExistNotPermision
}
