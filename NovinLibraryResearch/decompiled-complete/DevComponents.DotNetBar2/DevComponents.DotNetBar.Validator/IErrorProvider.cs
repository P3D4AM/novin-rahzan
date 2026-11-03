using System.Windows.Forms;

namespace DevComponents.DotNetBar.Validator;

public interface IErrorProvider
{
	void SetError(Control control, string value);

	void ClearError(Control control);
}
