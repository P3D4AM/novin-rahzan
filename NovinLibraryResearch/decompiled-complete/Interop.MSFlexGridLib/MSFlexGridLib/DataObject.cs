using System.Runtime.InteropServices;

namespace MSFlexGridLib;

[ComImport]
[CoClass(typeof(DataObjectClass))]
[Guid("2334D2B1-713E-11CF-8AE5-00AA00C00905")]
public interface DataObject : IVBDataObject
{
}
