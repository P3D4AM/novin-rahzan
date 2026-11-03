using System;
using System.ComponentModel.DataAnnotations;

namespace PacketParser.DepartmentApiModel.RequestModel;

public class BaseRequestModel
{
	[Required]
	[Range(1, 1000, ErrorMessage = "حداکثر و حداقل شماره صفحه({0}) بین {1} تا {2}  می باشد")]
	public int Page { get; set; } = 1;


	[Range(1, 1000, ErrorMessage = "حداکثر و حداقل تعداد صفحات({0}) بین {1} تا {2}  می باشد")]
	[Required]
	public int PageSize { get; set; } = 200;


	[Required]
	[DataType(DataType.DateTime)]
	[Range(typeof(DateTime), "1/1/2000", "01/01/2099", ErrorMessage = "مقدار تاریخ {0} باید در بازه {1} - {2} باشد.")]
	public DateTime LastModified { get; set; } = DateTime.Now.AddYears(-1);

}
