using System;
using System.Collections;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class CommandManager
{
	private static Hashtable _CommandBindings = new Hashtable();

	private static bool _UseReflection = true;

	private static bool _AutoUpdateLayout = true;

	public static bool UseReflection
	{
		get
		{
			return _UseReflection;
		}
		set
		{
			_UseReflection = value;
		}
	}

	public static bool AutoUpdateLayout
	{
		get
		{
			return _AutoUpdateLayout;
		}
		set
		{
			if (_AutoUpdateLayout != value)
			{
				_AutoUpdateLayout = value;
			}
		}
	}

	public static void RegisterCommand(ICommandSource commandSource, ICommand command)
	{
		if (commandSource == null)
		{
			throw new NullReferenceException("commandSource cannot be null");
		}
		if (command == null)
		{
			throw new NullReferenceException("command cannot be null");
		}
		ArrayList arrayList = null;
		if (_CommandBindings.Contains(command))
		{
			arrayList = (ArrayList)_CommandBindings[command];
			if (!arrayList.Contains(commandSource))
			{
				arrayList.Add(commandSource);
			}
		}
		else
		{
			arrayList = new ArrayList();
			arrayList.Add(commandSource);
			_CommandBindings.Add(command, arrayList);
		}
		command.CommandSourceRegistered(commandSource);
	}

	public static void UnRegisterCommandSource(ICommandSource commandSource, ICommand command)
	{
		if (commandSource == null)
		{
			throw new NullReferenceException("commandSource cannot be null");
		}
		if (command == null)
		{
			throw new NullReferenceException("command cannot be null");
		}
		if (_CommandBindings.Contains(command))
		{
			ArrayList arrayList = (ArrayList)_CommandBindings[command];
			if (arrayList.Contains(commandSource))
			{
				arrayList.Remove(commandSource);
			}
		}
		command.CommandSourceUnregistered(commandSource);
	}

	public static void UnRegisterCommand(ICommand command)
	{
		if (command == null)
		{
			throw new NullReferenceException("command cannot be null");
		}
		if (_CommandBindings.Contains(command))
		{
			_CommandBindings.Remove(command);
		}
	}

	public static ArrayList GetSubscribers(ICommand command)
	{
		if (command == null)
		{
			throw new NullReferenceException("command cannot be null");
		}
		ArrayList arrayList = null;
		if (_CommandBindings.Contains(command))
		{
			return (ArrayList)((ArrayList)_CommandBindings[command]).Clone();
		}
		return new ArrayList();
	}

	internal static void ExecuteCommand(ICommandSource commandSource)
	{
		if (commandSource == null)
		{
			throw new NullReferenceException("commandSource cannot be null");
		}
		ICommand command = commandSource.Command;
		if (command == null)
		{
			throw new NullReferenceException("commandSource.Command cannot be null");
		}
		command.Execute(commandSource);
	}

	internal static void ExecuteCommand(ICommandSource commandSource, ICommand command)
	{
		if (commandSource == null)
		{
			throw new NullReferenceException("commandSource cannot be null");
		}
		if (command == null)
		{
			throw new NullReferenceException("commandSource.Command cannot be null");
		}
		command.Execute(commandSource);
	}
}
