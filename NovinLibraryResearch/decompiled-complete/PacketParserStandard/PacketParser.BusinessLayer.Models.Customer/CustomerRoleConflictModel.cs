using System.Collections.Generic;

namespace PacketParser.BusinessLayer.Models.Customer;

public class CustomerRoleConflictModel
{
	public string WordPressName { get; set; }

	public List<string> ConflictName { get; set; }

	public string Names => string.Join(" , ", ConflictName);

	public int Count { get; set; }
}
