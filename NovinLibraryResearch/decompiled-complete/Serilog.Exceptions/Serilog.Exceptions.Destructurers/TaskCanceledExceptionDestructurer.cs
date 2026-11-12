using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog.Exceptions.Core;

namespace Serilog.Exceptions.Destructurers;

public class TaskCanceledExceptionDestructurer : OperationCanceledExceptionDestructurer
{
	private static readonly Type[] TargetExceptionTypes = new Type[1] { typeof(TaskCanceledException) };

	public override Type[] TargetTypes => TargetExceptionTypes;

	public override void Destructure(Exception exception, IExceptionPropertiesBag propertiesBag, Func<Exception, IReadOnlyDictionary<string, object?>?> destructureException)
	{
		base.Destructure(exception, propertiesBag, destructureException);
		TaskCanceledException ex = (TaskCanceledException)exception;
		propertiesBag.AddProperty("Task", DestructureTask(ex.Task, destructureException));
	}

	internal static object? DestructureTask(Task? task, Func<Exception, IReadOnlyDictionary<string, object?>?> innerDestructure)
	{
		if (task == null)
		{
			return null;
		}
		string status = task.Status.ToString("G");
		string creationOptions = task.CreationOptions.ToString("F");
		if (task.IsFaulted && task.Exception != null)
		{
			return new
			{
				Id = task.Id,
				Status = status,
				CreationOptions = creationOptions,
				Exception = innerDestructure(task.Exception)
			};
		}
		return new
		{
			Id = task.Id,
			Status = status,
			CreationOptions = creationOptions
		};
	}
}
