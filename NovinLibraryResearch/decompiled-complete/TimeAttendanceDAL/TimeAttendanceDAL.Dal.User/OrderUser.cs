using System;
using System.Windows.Forms;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.User;

[Serializable]
[DoNotPruneType]
public class OrderUser
{
	public string UCName { get; set; } = "";


	public UserControl UC { get; set; }

	public short Order { get; set; }

	public bool Visible { get; set; }

	public OrderUser(string pUcName, UserControl pUc, short pOrder, bool pVisible)
	{
		UCName = pUcName;
		UC = pUc;
		Order = pOrder;
		Visible = pVisible;
	}
}
