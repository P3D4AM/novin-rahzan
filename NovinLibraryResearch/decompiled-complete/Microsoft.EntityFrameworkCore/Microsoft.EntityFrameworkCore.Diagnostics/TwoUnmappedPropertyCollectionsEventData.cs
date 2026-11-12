using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;

namespace Microsoft.EntityFrameworkCore.Diagnostics;

public class TwoUnmappedPropertyCollectionsEventData : EventData
{
	public virtual IEnumerable<Tuple<MemberInfo, Type>> FirstPropertyCollection { get; }

	public virtual IEnumerable<Tuple<MemberInfo, Type>> SecondPropertyCollection { get; }

	public TwoUnmappedPropertyCollectionsEventData([JetBrains.Annotations.NotNull] EventDefinitionBase eventDefinition, [JetBrains.Annotations.NotNull] Func<EventDefinitionBase, EventData, string> messageGenerator, [JetBrains.Annotations.NotNull] IEnumerable<Tuple<MemberInfo, Type>> firstPropertyCollection, [JetBrains.Annotations.NotNull] IEnumerable<Tuple<MemberInfo, Type>> secondPropertyCollection)
		: base(eventDefinition, messageGenerator)
	{
		FirstPropertyCollection = firstPropertyCollection;
		SecondPropertyCollection = secondPropertyCollection;
	}
}
