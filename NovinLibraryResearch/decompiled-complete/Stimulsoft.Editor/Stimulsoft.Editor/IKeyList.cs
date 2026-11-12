using System.Windows.Forms;

namespace Stimulsoft.Editor;

public interface IKeyList
{
	EventHandlers Handlers { get; }

	void Add(Keys keyData, KeyEventEx action, object param, int state, int leaveState);

	void Add(Keys keyData, KeyEvent action, int state, int leaveState);

	void Add(Keys keyData, KeyEventEx action, object param);

	void Add(Keys keyData, KeyEvent action);

	void AddNormal(Keys keyData, KeyEventEx action, object param);

	void AddNormal(Keys keyData, KeyEvent action);

	void Remove(Keys keyData, int state);

	void Remove(Keys keyData);

	void Clear();

	bool ExecuteKey(Keys keyData, ref int state);

	bool FindKey(Keys keyData, int state);
}
