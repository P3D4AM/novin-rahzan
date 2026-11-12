using System.Collections;

namespace Stimulsoft.Report.Web;

public class StiEmailOptions
{
	public string AddressFrom { get; set; } = string.Empty;


	public string AddressTo { get; set; } = string.Empty;


	public string Subject { get; set; } = string.Empty;


	public string ReplyTo { get; set; } = string.Empty;


	public string Body { get; set; } = string.Empty;


	public string Host { get; set; } = "localhost";


	public int Port { get; set; } = 25;


	public string UserName { get; set; } = string.Empty;


	public string Password { get; set; } = string.Empty;


	public bool EnableSsl { get; set; } = true;


	public ArrayList CC { get; } = new ArrayList();


	public ArrayList BCC { get; } = new ArrayList();

}
