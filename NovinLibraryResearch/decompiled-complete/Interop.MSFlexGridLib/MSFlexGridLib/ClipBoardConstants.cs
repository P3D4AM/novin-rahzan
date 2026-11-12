using System.Runtime.InteropServices;

namespace MSFlexGridLib;

[Guid("D8898462-742F-11CF-8AEA-00AA00C00905")]
public enum ClipBoardConstants
{
	flexCFText = 1,
	flexCFBitmap = 2,
	flexCFMetafile = 3,
	flexCFDIB = 8,
	flexCFPalette = 9,
	flexCFEMetafile = 14,
	flexCFFiles = 15,
	flexCFRTF = -16639
}
