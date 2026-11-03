using System.Collections.Generic;

namespace PacketParser.DepartmentApiModel;

public class PagedResponseModel<T>
{
	public int CurrentPage { get; set; }

	public int Pages { get; set; }

	public int TotalItemCount { get; set; }

	public List<T> Results { get; set; }

	public string Message { get; set; }
}
