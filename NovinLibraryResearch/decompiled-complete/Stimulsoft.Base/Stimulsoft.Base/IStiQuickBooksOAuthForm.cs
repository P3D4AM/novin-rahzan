using System.Windows.Forms;

namespace Stimulsoft.Base;

internal interface IStiQuickBooksOAuthForm
{
	string AuthorizationCode { get; set; }

	string RealmId { get; set; }

	string Error { get; set; }

	DialogResult ShowDialog();
}
