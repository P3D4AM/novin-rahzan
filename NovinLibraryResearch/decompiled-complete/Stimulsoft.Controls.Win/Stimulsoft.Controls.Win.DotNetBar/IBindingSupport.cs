using System.Collections;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public interface IBindingSupport
{
	bool AllowSelection { get; set; }

	int SelectedIndex { get; set; }

	BindingContext BindingContext { get; set; }

	IList ItemsCollection { get; }

	void BeginUpdate();

	void EndUpdate();

	void InvokeDataNodeCreated(DataItemVisualEventArgs e);
}
