using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization.Internals;

internal class XmlIndex : IXmlIndex, IIndex, IMetadataObject
{
	private readonly DeserializationModel model;

	private readonly bool disallowPageLocks;

	private readonly bool disallowRowLocks;

	private readonly byte fillFactor;

	private readonly bool ignoreDuplicateKeys;

	private readonly bool isDisabled;

	private readonly string name;

	private readonly bool padIndex;

	private readonly int parent;

	ITabular IIndex.Parent => model.GetObjectById<ITabular>(parent);

	IndexType IIndex.Type => IndexType.Xml;

	bool IIndex.DisallowPageLocks => disallowPageLocks;

	bool IIndex.DisallowRowLocks => disallowRowLocks;

	byte IIndex.FillFactor => fillFactor;

	bool IIndex.IgnoreDuplicateKeys => ignoreDuplicateKeys;

	bool IIndex.IsDisabled => isDisabled;

	bool IIndex.PadIndex => padIndex;

	string IMetadataObject.Name => name;

	public XmlIndex(DeserializationModel model, bool disallowPageLocks, bool disallowRowLocks, byte fillFactor, bool ignoreDuplicateKeys, bool isDisabled, string name, bool padIndex, int parent)
	{
		this.model = model;
		this.disallowPageLocks = disallowPageLocks;
		this.disallowRowLocks = disallowRowLocks;
		this.fillFactor = fillFactor;
		this.ignoreDuplicateKeys = ignoreDuplicateKeys;
		this.isDisabled = isDisabled;
		this.name = name;
		this.padIndex = padIndex;
		this.parent = parent;
	}

	public XmlIndex(DeserializationModel model, object[] values)
		: this(model, (bool)values[0], (bool)values[1], (byte)values[2], (bool)values[3], (bool)values[4], (string)values[5], (bool)values[6], (int)values[7])
	{
	}

	T IMetadataObject.Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}
}
