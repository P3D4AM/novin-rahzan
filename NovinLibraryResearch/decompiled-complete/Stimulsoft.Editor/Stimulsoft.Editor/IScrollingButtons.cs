using System.Collections;

namespace Stimulsoft.Editor;

public interface IScrollingButtons : IList, ICollection, IEnumerable
{
	void Assign(IScrollingButtons source);

	IScrollingButton GetScrollingButton(int index);

	int AddScrollingButton();

	int AddScrollingButton(string name, string description, int imageIndex);
}
