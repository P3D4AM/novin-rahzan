using System.Windows.Forms;

namespace Stimulsoft.Editor;

public interface IGotoLineDialog
{
	DialogResult Execute(object sender, int lines, ref int line);
}
