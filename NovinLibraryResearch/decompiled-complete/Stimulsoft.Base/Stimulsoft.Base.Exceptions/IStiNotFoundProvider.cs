using System.Windows.Forms;

namespace Stimulsoft.Base.Exceptions;

public interface IStiNotFoundProvider
{
	void Show(string namePackage);

	void Show(string namePackage, Form owner);
}
