using System.Runtime.InteropServices;

namespace MSFlexGridLib;

[ComImport]
[CoClass(typeof(DataObjectFilesClass))]
[Guid("2334D2B3-713E-11CF-8AE5-00AA00C00905")]
public interface DataObjectFiles : IVBDataObjectFiles
{
}
