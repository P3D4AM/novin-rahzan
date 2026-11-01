using System.Windows.Forms;

namespace TimeAttendanceUI.Forms.Checkout;

public interface ICheckout
{
	bool TopLevel { get; set; }

	bool AutoScroll { get; set; }

	void FillCheckout();

	ICheckout NextPage(bool fillCheckout);

	void Show(IWin32Window owner);
}
