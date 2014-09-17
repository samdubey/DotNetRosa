using System;
using ILogger = org.javarosa.core.api.ILogger;
using FatalException = org.javarosa.core.log.FatalException;
using WrappedException = org.javarosa.core.log.WrappedException;
using JavaRosaPropertyRules = org.javarosa.core.services.properties.JavaRosaPropertyRules;
namespace org.javarosa.core.services
{
	
	public class Logger
	{
		private class AnonymousClassThread:SupportClass.ThreadClass
		{
			public AnonymousClassThread(org.javarosa.core.log.FatalException crashException)
			{
				InitBlock(crashException);
			}
			private void  InitBlock(org.javarosa.core.log.FatalException crashException)
			{
				this.crashException = crashException;
			}
			//UPGRADE_NOTE: Final variable crashException was copied into class AnonymousClassThread. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1023'"
			private org.javarosa.core.log.FatalException crashException;
			override public void  Run()
			{
				throw crashException;
			}
		}
		public static bool LoggingEnabled
		{
			get
			{
				bool enabled;
				bool problemReadingFlag = false;
				try
				{
					System.String flag = PropertyManager._().getSingularProperty(JavaRosaPropertyRules.LOGS_ENABLED);
					enabled = (flag == null || flag.Equals(JavaRosaPropertyRules.LOGS_ENABLED_YES));
				}
				catch (System.Exception e)
				{
					enabled = true; //default to true if problem
					problemReadingFlag = true;
				}
				
				if (problemReadingFlag)
				{
					logForce("log-error", "could not read 'logging enabled' flag");
				}
				
				return enabled;
			}
			
		}
		public const int MAX_MSG_LENGTH = 2048;
		
		private static ILogger logger;
		
		public static void  registerLogger(ILogger theLogger)
		{
			logger = theLogger;
		}
		
		public static ILogger _()
		{
			return logger;
		}
		
		/// <summary> Posts the given data to an existing Incident Log, if one has
		/// been registered and if logging is enabled on the device. 
		/// 
		/// NOTE: This method makes a best faith attempt to log the given
		/// data, but will not produce any output if such attempts fail.
		/// 
		/// </summary>
		/// <param name="type">The type of incident to be logged. 
		/// </param>
		/// <param name="message">A message describing the incident.
		/// </param>
		public static void  log(System.String type, System.String message)
		{
			if (LoggingEnabled)
			{
				logForce(type, message);
			}
		}
		
		protected internal static void  logForce(System.String type, System.String message)
		{
			System.Console.Error.WriteLine("logger> " + type + ": " + message);
			if (message.Length > MAX_MSG_LENGTH)
				System.Console.Error.WriteLine("  (message truncated)");
			
			message = message.Substring(0, (System.Math.Min(message.Length, MAX_MSG_LENGTH)) - (0));
			if (logger != null)
			{
				try
				{
					System.DateTime tempAux = System.DateTime.Now;
					//UPGRADE_NOTE: ref keyword was added to struct-type parameters. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1303'"
					logger.log(type, message, ref tempAux);
				}
				catch (System.SystemException e)
				{
					//do not catch exceptions here; if this fails, we want the exception to propogate
					System.Console.Error.WriteLine("exception when trying to write log message! " + WrappedException.printException(e));
					logger.panic();
					
					//be conservative for now
					//throw e;
				}
			}
		}
		
		public static void  exception(System.Exception e)
		{
			exception(null, e);
		}
		
		public static void  exception(System.String info, System.Exception e)
		{
			if (e is org.javarosa.core.io.StreamsUtil.DirectionalIOException)
				((org.javarosa.core.io.StreamsUtil.DirectionalIOException) e).printStackTrace();
			else
				SupportClass.WriteStackTrace(e, Console.Error);
			log("exception", (info != null?info + ": ":"") + WrappedException.printException(e));
		}
		
		public static void  die(System.String thread, System.Exception e)
		{
			//log exception
			exception("unhandled exception at top level", e);
			
			//print stacktrace
			if (e is org.javarosa.core.io.StreamsUtil.DirectionalIOException)
				((org.javarosa.core.io.StreamsUtil.DirectionalIOException) e).printStackTrace();
			else
				SupportClass.WriteStackTrace(e, Console.Error);
			
			//crash
			//UPGRADE_NOTE: Final was removed from the declaration of 'crashException '. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
			FatalException crashException = new FatalException("unhandled exception in " + thread, e);
			
			//depending on how the code was invoked, a straight 'throw' won't always reliably crash the app
			//throwing in a thread should work (at least on our nokias)
			new AnonymousClassThread(crashException).Start();
			
			//still do plain throw as a fallback
			try
			{
				//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
				System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * 3000));
			}
			catch (System.Threading.ThreadInterruptedException ie)
			{
			}
			throw crashException;
		}
		
		public static void  crashTest(System.String msg)
		{
			throw new FatalException(msg != null?msg:"shit has hit the fan");
		}
		
		public static void  halt()
		{
			if (logger != null)
			{
				logger.halt();
			}
		}
	}
}