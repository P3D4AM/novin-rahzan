using System.Collections;
using System.Collections.Generic;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.Binder;
using Microsoft.SqlServer.Management.SqlParser.Metadata;
using Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

namespace Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

public abstract class SqlMultipartIdentifier : SqlCodeObject, IEnumerable<SqlIdentifier>, IEnumerable
{
	public abstract int Count { get; }

	public abstract SqlIdentifier this[int index] { get; }

	internal sealed override IMetadataCollection<IMetadataObject> GetCompletionList(LexLocation caretLocation, CompletionContext completionContext)
	{
		IMetadataCollection<IMetadataObject> result = null;
		LexLocation position = base.Position;
		if (completionContext != 0 && (caretLocation.startTokenNumber != caretLocation.endTokenNumber || caretLocation.endTokenNumber != position.endTokenNumber || this[Count - 1].IsMissing))
		{
			result = ((caretLocation.startTokenNumber != position.startTokenNumber) ? RetrieveCompletionListItems(caretLocation, this, completionContext) : RetrieveCompletionListItems(completionContext));
		}
		return result;
	}

	internal sealed override string GetQuickInfoText(LexLocation cursorLocation, IMetadataDisplayInfoProvider displayInfoProvider)
	{
		IMetadataObject metadataObject = null;
		BatchBindingContext batch = BindingContext.Batch;
		if (batch != null)
		{
			SqlCodeObject sqlCodeObject = FindChildContainingLocation(ref cursorLocation);
			metadataObject = batch.GetBoundObject(sqlCodeObject);
			if (metadataObject == null)
			{
				IDatabaseObject databaseObject = null;
				for (int num = Count - 1; num >= 0; num--)
				{
					SqlIdentifier sqlIdentifier = this[num];
					if (databaseObject != null)
					{
						databaseObject = databaseObject.Parent;
						if (sqlCodeObject == sqlIdentifier)
						{
							metadataObject = databaseObject;
							break;
						}
					}
					else
					{
						if (sqlIdentifier == sqlCodeObject)
						{
							break;
						}
						IMetadataObject boundObject = batch.GetBoundObject(sqlIdentifier);
						if (boundObject != null)
						{
							databaseObject = boundObject as IDatabaseObject;
						}
					}
				}
			}
		}
		if (metadataObject == null)
		{
			return null;
		}
		return displayInfoProvider.GetDescription(metadataObject);
	}

	public abstract IEnumerator<SqlIdentifier> GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
