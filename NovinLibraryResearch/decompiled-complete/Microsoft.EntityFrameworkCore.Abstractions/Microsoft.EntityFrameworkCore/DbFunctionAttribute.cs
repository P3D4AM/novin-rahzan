using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Microsoft.EntityFrameworkCore;

[AttributeUsage(AttributeTargets.Method)]
public class DbFunctionAttribute : Attribute
{
	private string _name;

	private string _schema;

	public virtual string Name
	{
		get
		{
			return _name;
		}
		[param: NotNull]
		set
		{
			Check.NotEmpty(value, "value");
			_name = value;
		}
	}

	public virtual string Schema
	{
		get
		{
			return _schema;
		}
		[param: CanBeNull]
		set
		{
			_schema = value;
		}
	}

	public DbFunctionAttribute()
	{
	}

	public DbFunctionAttribute([NotNull] string name, [CanBeNull] string schema = null)
	{
		Check.NotEmpty(name, "name");
		_name = name;
		_schema = schema;
	}
}
