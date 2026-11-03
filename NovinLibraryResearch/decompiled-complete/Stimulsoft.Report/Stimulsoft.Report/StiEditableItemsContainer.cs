using System.Collections;
using Stimulsoft.Base.Serializing;

namespace Stimulsoft.Report;

public class StiEditableItemsContainer
{
	[StiSerializable(/*Could not decode attribute arguments.*/)]
	public ArrayList Items { get; } = new ArrayList();

}
