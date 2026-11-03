using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using PacketParser.Entities;

namespace PacketParser.DepartmentApiModel;

public class WebResponseModel<T>
{
	public List<WebResponseModelDetail> Result { get; set; }

	public T Value { get; set; }

	public WebResponseModel<T> AddReturnedValue(Exception exception, string descriptions = "", [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		if (Result == null)
		{
			List<WebResponseModelDetail> list2 = (Result = new List<WebResponseModelDetail>());
		}
		Result.Add(new WebResponseModelDetail
		{
			Message = exception.Message,
			State = ReturnedState.Error
		});
		return this;
	}
}
