using System.Collections;

namespace Stimulsoft.Editor;

public interface IStringList : ICollection, IEnumerable, IImport, IExport
{
	string this[int index] { get; }

	string Text { get; set; }

	string LineTerminator { get; set; }
}
