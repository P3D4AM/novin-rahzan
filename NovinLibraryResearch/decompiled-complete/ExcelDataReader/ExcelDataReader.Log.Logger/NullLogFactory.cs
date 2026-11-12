using System;
using System.Runtime.InteropServices;

namespace ExcelDataReader.Log.Logger;

[StructLayout(LayoutKind.Sequential, Size = 1)]
public struct NullLogFactory : ILogFactory, ILog
{
	public void Debug(string message, params object[] formatting)
	{
	}

	public void Info(string message, params object[] formatting)
	{
	}

	public void Warn(string message, params object[] formatting)
	{
	}

	public void Error(string message, params object[] formatting)
	{
	}

	public void Fatal(string message, params object[] formatting)
	{
	}

	public ILog Create(Type loggingType)
	{
		return this;
	}
}
