using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Validator;

public interface IErrorProvider
{
	void SetError(Control control, string value);

	void ClearError(Control control);
}
