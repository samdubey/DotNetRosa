/// <summary> </summary>
using System;
using TreeReferenceLevel = org.javarosa.core.model.instance.TreeReferenceLevel;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using XPathStep = org.javarosa.xpath.expr.XPathStep;
namespace org.javarosa.core.util
{
	
	/// <summary> J2ME suffers from major disparities in the effective use of memory. This
	/// class encompasses some hacks that sadly have to happen pertaining to high
	/// memory throughput actions.
	/// 
	/// This was implemented in a hurry, and urgently needs a major refactor to be less...
	/// hacky, static, and stupid.
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public class MemoryUtils
	{
		
		//These 3 are used to hold the profile of the heapspace, only relevant 
		//if you are doing deep memory profiling
		private static long[] memoryProfile;
		private static sbyte[][] memoryHolders;
		internal static int currentCount = 0;
		
		//Variables to keep track of the state of some of the internal 
		//interning options 
		//TODO: I think we can get rid of this, depending on fragmentation analysis
		internal static bool oldterning;
		internal static bool otrt;
		internal static bool oldxpath;
		public static void  stopTerning()
		{
			oldterning = ExtUtil.interning;
			otrt = TreeReferenceLevel.treeRefLevelInterningEnabled;
			oldxpath = XPathStep.XPathStepInterningEnabled;
			ExtUtil.interning = false;
			TreeReferenceLevel.treeRefLevelInterningEnabled = false;
			XPathStep.XPathStepInterningEnabled = false;
		}
		
		public static void  revertTerning()
		{
			ExtUtil.interning = oldterning;
			TreeReferenceLevel.treeRefLevelInterningEnabled = otrt;
			XPathStep.XPathStepInterningEnabled = oldxpath;
		}
		
		
		//Used once at the beginning of an execution to enable memory profiling for
		//this run through. If you get an error when you try to profile memory,
		//due to lack of space, you can increase the profile size.
		private const int MEMORY_PROFILE_SIZE = 5000;
		public static void  enableMemoryProfile()
		{
			memoryProfile = new long[MEMORY_PROFILE_SIZE * 2];
			memoryHolders = new sbyte[MEMORY_PROFILE_SIZE][];
		}
		
		//#if javarosa.memory.print
		//# private static boolean MEMORY_PRINT_ENABLED = true;
		//#else
		private static bool MEMORY_PRINT_ENABLED = false;
		//#endif
		
		/// <summary> Prints a memory test debug statement to stdout.
		/// Requires memory printing to be enabled, otherwise
		/// is a no-op
		/// </summary>
		public static void  printMemoryTest()
		{
			printMemoryTest(null);
		}
		
		/// <summary> Prints a memory test debug statement to stdout 
		/// with a tag to reference
		/// Requires memory printing to be enabled, otherwise
		/// is a no-op 
		/// </summary>
		/// <param name="tag">
		/// </param>
		public static void  printMemoryTest(System.String tag)
		{
			printMemoryTest(tag, - 1);
		}
		
		/// <summary> Prints a memory test debug statement to stdout
		/// with a tag to reference. After printing the message
		/// the app waits for Pause milliseconds to allow profiling
		/// 
		/// Requires memory printing to be enabled, otherwise
		/// is a no-op
		/// </summary>
		/// <param name="tag">
		/// </param>
		/// <param name="pause">
		/// </param>
		public static void  printMemoryTest(System.String tag, int pause)
		{
			if (!MEMORY_PRINT_ENABLED)
			{
				return ;
			}
			System.GC.Collect();
			System.Diagnostics.Process r = System.Diagnostics.Process.GetCurrentProcess();
			//UPGRADE_ISSUE: Method 'java.lang.Runtime.freeMemory' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangRuntimefreeMemory'"
			long free = r.freeMemory();
			//UPGRADE_ISSUE: Method 'java.lang.Runtime.totalMemory' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangRuntimetotalMemory'"
			long total = r.totalMemory();
			
			if (tag != null)
			{
				System.Console.Out.WriteLine("=== Memory Evaluation: " + tag + " ===");
			}
			
			System.Console.Out.WriteLine("Total: " + total + "\nFree: " + free);
			
			int chunk = 100;
			int lastSuccess = 100;
			
			//Some environments provide better or more accurate numbers for available memory than 
			//others. Just in case, we go through and allocate the largest contiguious block of
			//memory that is available to see what the actual upper bound is for what we can
			//use
			
			//The resolution is the smallest chunk of memory that we care about. We won't bother
			//trying to add resolution more bytes if we couldn't add resolution * 2 bytes.
			int resolution = 10000;
			
			while (true)
			{
				System.GC.Collect();
				try
				{
					int newAmount = lastSuccess + chunk;
					sbyte[] allocated = new sbyte[newAmount];
					lastSuccess = newAmount;
					//If we succeeded, keep trying a larger piece. 
					chunk = chunk * 10;
				}
				catch (System.OutOfMemoryException oom)
				{
					chunk = chunk / 2;
					if (chunk < resolution)
					{
						break;
					}
				}
			}
			
			
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			int availPercent = (int) System.Math.Floor((lastSuccess * 1.0 / total) * 100);
			//UPGRADE_WARNING: Data types in Visual C# might be different.  Verify the accuracy of narrowing conversions. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1042'"
			int fragmentation = (int) System.Math.Floor((lastSuccess * 1.0 / free) * 100);
			System.Console.Out.WriteLine("Usable Memory: " + lastSuccess + "\n" + availPercent + "% of available memory");
			System.Console.Out.WriteLine("Fragmentation: " + fragmentation + "%");
			
			if (pause != - 1)
			{
				try
				{
					//UPGRADE_TODO: Method 'java.lang.Thread.sleep' was converted to 'System.Threading.Thread.Sleep' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javalangThreadsleep_long'"
					System.Threading.Thread.Sleep(new System.TimeSpan((System.Int64) 10000 * pause));
				}
				catch (System.Threading.ThreadInterruptedException e)
				{
					// TODO Auto-generated catch block
					SupportClass.WriteStackTrace(e, Console.Error);
				}
			}
		}
		
		/// <summary> Experimental.
		/// 
		/// This method builds a profile of what the current memory allocation looks like in the current heap.
		/// 
		/// You must initialize the profiler once in your app (preferably immediately upon entering) to pre-allocate
		/// the space for the profile.
		/// </summary>
		public static void  profileMemory()
		{
			if (memoryProfile == null)
			{
				System.Console.Out.WriteLine("You must initialize the memory profiler before it can be used!");
				return ;
			}
			currentCount = 0;
			int chunkSize = 100000;
			long memoryAccountedFor = 0;
			bool succeeded = false;
			
			int threshold = 4;
			System.Diagnostics.Process r = System.Diagnostics.Process.GetCurrentProcess();
			
			System.GC.Collect();
			//UPGRADE_ISSUE: Method 'java.lang.Runtime.freeMemory' was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1000_javalangRuntimefreeMemory'"
			long memory = r.freeMemory();
			
			//Basically: We go through here and allocate arrays over and over, making them smaller and smaller 
			//until we reach the smallest unit we care about allocating. The parameters can be tuned depending
			//on the type of fragmentation you are concerned about. 
			while (true)
			{
				if (currentCount >= MEMORY_PROFILE_SIZE)
				{
					System.Console.Out.WriteLine("Memory profile is too small for this device's usage!");
					break;
				}
				if (chunkSize < threshold)
				{
					succeeded = true; break;
				}
				
				try
				{
					memoryHolders[currentCount] = new sbyte[chunkSize];
					memoryProfile[currentCount * 2] = (memoryHolders[currentCount].GetHashCode() & unchecked((int) 0x00000000ffffffffL));
					memoryProfile[(currentCount * 2) + 1] = chunkSize;
					currentCount++;
					memoryAccountedFor += chunkSize;
				}
				catch (System.OutOfMemoryException oom)
				{
					chunkSize = chunkSize - (chunkSize < 100?1:50);
				}
			}
			for (int i = 0; i < currentCount; ++i)
			{
				memoryHolders[i] = null;
			}
			System.GC.Collect();
			
			//For now, just print out the profile. Eventually we should compress it and output it in a useful format.
			if (succeeded)
			{
				System.Console.Out.WriteLine("Acquired memory profile for " + memoryAccountedFor + " of the " + memory + " available bytes, with " + currentCount + " traces");
				for (int i = 0; i < currentCount * 2; i += 2)
				{
					System.Console.Out.WriteLine("Address: " + memoryProfile[i] + " -> " + memoryProfile[i + 1]);
				}
			}
		}
	}
}