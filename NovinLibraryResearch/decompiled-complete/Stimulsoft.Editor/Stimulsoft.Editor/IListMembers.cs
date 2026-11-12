using System.Collections;

namespace Stimulsoft.Editor;

public interface IListMembers : ICodeCompletionProvider, IList, ICollection, IEnumerable
{
	new IListMember this[int index] { get; }

	bool ShowQualifiers { get; set; }

	bool ShowResults { get; set; }

	bool ShowParams { get; set; }

	bool ShowHints { get; set; }

	IListMember CreateListMember();

	IListMember AddMember();

	IListMember InsertMember(int index);

	IListMember GetListMember(int index);

	void ResetShowQualifiers();

	void ResetShowResults();

	void ResetShowParams();

	void ResetShowHints();
}
