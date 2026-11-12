using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar.Validator;

internal class ErrorProviderWrapper : IErrorProvider
{
	private ErrorProvider _ErrorProvider;

	private int _IconPadding;

	public int IconPadding
	{
		get
		{
			return _IconPadding;
		}
		set
		{
			_IconPadding = value;
		}
	}

	public ErrorProviderWrapper(ErrorProvider errorProvider)
	{
		_ErrorProvider = errorProvider;
	}

	public ErrorProviderWrapper(ErrorProvider errorProvider, int iconPadding)
	{
		_ErrorProvider = errorProvider;
		_IconPadding = iconPadding;
	}

	public void SetError(Control control, string value)
	{
		_ErrorProvider.SetError(control, value);
		if (_IconPadding != 0)
		{
			_ErrorProvider.SetIconPadding(control, _IconPadding);
		}
	}

	public void ClearError(Control control)
	{
		_ErrorProvider.SetError(control, (string)null);
		if (_IconPadding != 0)
		{
			_ErrorProvider.SetIconPadding(control, 0);
		}
	}
}
