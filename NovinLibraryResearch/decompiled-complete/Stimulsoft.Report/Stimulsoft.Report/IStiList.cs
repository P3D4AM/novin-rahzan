using System;
using System.Collections;

namespace Stimulsoft.Report;

public interface IStiList : IEnumerable
{
	string ListName { get; }

	Type ListType { get; }

	int Count { get; }

	void AddElement(object value);

	string ToQueryString();

	string ToQueryString(string quotationMark);

	object[] ToObjectArray();

	void Clear();
}
