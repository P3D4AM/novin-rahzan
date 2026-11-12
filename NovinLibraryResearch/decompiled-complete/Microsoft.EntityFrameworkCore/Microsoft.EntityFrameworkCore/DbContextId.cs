using System;
using System.Globalization;

namespace Microsoft.EntityFrameworkCore;

public readonly struct DbContextId
{
	public Guid InstanceId { get; }

	public int Lease { get; }

	public bool Equals(DbContextId other)
	{
		if (InstanceId == other.InstanceId)
		{
			return Lease == other.Lease;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is DbContextId other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(InstanceId, Lease);
	}

	public static bool operator ==(DbContextId left, DbContextId right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(DbContextId left, DbContextId right)
	{
		return !left.Equals(right);
	}

	public DbContextId(Guid id, int lease)
	{
		InstanceId = id;
		Lease = lease;
	}

	public override string ToString()
	{
		return InstanceId.ToString() + ":" + Lease.ToString(CultureInfo.InvariantCulture);
	}
}
