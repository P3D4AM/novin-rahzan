using System;
using System.Collections;

namespace DevComponents.DotNetBar;

public class CommandManager
{
	private static Hashtable _૪ = new Hashtable();

	private static bool _૫ = true;

	private static bool _૬ = true;

	public static bool UseReflection
	{
		get
		{
			return _૫;
		}
		set
		{
			_૫ = value;
		}
	}

	public static bool AutoUpdateLayout
	{
		get
		{
			return _૬;
		}
		set
		{
			if (_૬ != value)
			{
				_૬ = value;
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
		if (_૪.Contains(command))
		{
			arrayList = (ArrayList)_૪[command];
			if (!arrayList.Contains(commandSource))
			{
				arrayList.Add(commandSource);
			}
		}
		else
		{
			arrayList = new ArrayList();
			arrayList.Add(commandSource);
			_૪.Add(command, arrayList);
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
		if (_૪.Contains(command))
		{
			ArrayList arrayList = (ArrayList)_૪[command];
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
		if (_૪.Contains(command))
		{
			_૪.Remove(command);
		}
	}

	public static ArrayList GetSubscribers(ICommand command)
	{
		if (command == null)
		{
			throw new NullReferenceException("command cannot be null");
		}
		ArrayList arrayList = null;
		if (_૪.Contains(command))
		{
			return (ArrayList)((ArrayList)_૪[command]).Clone();
		}
		return new ArrayList();
	}

	internal static void _૭(ICommandSource _૮)
	{
		if (_૮ == null)
		{
			throw new NullReferenceException("commandSource cannot be null");
		}
		ICommand command = _૮.Command;
		if (command == null)
		{
			throw new NullReferenceException("commandSource.Command cannot be null");
		}
		command.Execute(_૮);
	}

	internal static void _૭(ICommandSource _૮, ICommand _૯)
	{
		if (_૮ == null)
		{
			throw new NullReferenceException("commandSource cannot be null");
		}
		if (_૯ == null)
		{
			throw new NullReferenceException("commandSource.Command cannot be null");
		}
		_૯.Execute(_૮);
	}
}
