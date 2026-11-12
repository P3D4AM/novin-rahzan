using System;
using Microsoft.SqlServer.Management.SqlParser.Metadata;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataProvider;

internal sealed class DatePart : MetadataObjectBase, IDatePart, IMetadataObject
{
	private static class ListHelper
	{
		public static readonly DatePart[] Dateparts;

		static ListHelper()
		{
			Dateparts = new DatePart[40]
			{
				new DatePart("year"),
				new DatePart("yyyy"),
				new DatePart("yy"),
				new DatePart("quarter"),
				new DatePart("qq"),
				new DatePart("q"),
				new DatePart("month"),
				new DatePart("mm"),
				new DatePart("m"),
				new DatePart("dayofyear"),
				new DatePart("dy"),
				new DatePart("y"),
				new DatePart("day"),
				new DatePart("dd"),
				new DatePart("d"),
				new DatePart("week"),
				new DatePart("wk"),
				new DatePart("ww"),
				new DatePart("weekday"),
				new DatePart("dw"),
				new DatePart("w"),
				new DatePart("hour"),
				new DatePart("hh"),
				new DatePart("minute"),
				new DatePart("mi"),
				new DatePart("n"),
				new DatePart("second"),
				new DatePart("ss"),
				new DatePart("s"),
				new DatePart("millisecond"),
				new DatePart("ms"),
				new DatePart("microsecond"),
				new DatePart("mcs"),
				new DatePart("nanosecond"),
				new DatePart("ns"),
				new DatePart("tzoffset"),
				new DatePart("tz"),
				new DatePart("iso_week"),
				new DatePart("isowk"),
				new DatePart("isoww")
			};
		}
	}

	private DatePart(string name)
		: base(name)
	{
	}

	public override T Accept<T>(IMetadataObjectVisitor<T> visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		return visitor.Visit(this);
	}

	public static void PopulateCollection(DatePartCollection collection)
	{
		collection.AddRange(ListHelper.Dateparts);
	}
}
