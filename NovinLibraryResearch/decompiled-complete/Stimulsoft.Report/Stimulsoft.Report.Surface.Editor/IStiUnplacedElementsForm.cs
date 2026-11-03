using System.Windows.Forms;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Surface.Editor;

public interface IStiUnplacedElementsForm
{
	DialogResult ShowDialog();

	void RemoveElement(StiComponent component);
}
