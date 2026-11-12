using System.Runtime.InteropServices;

namespace ADODB;

[ComImport]
[Guid("B08400BD-F9D1-4D02-B856-71D5DBA123E9")]
[CoClass(typeof(CommandClass))]
public interface Command : _Command
{
}
