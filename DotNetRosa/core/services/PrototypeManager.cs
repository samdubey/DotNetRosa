using System;
using PrefixTree = org.javarosa.core.util.PrefixTree;
using CannotCreateObjectException = org.javarosa.core.util.externalizable.CannotCreateObjectException;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.services
{
	
	public class PrototypeManager
	{
		public static PrefixTree Prototypes
		{
			get
			{
				if (prototypes == null)
				{
					prototypes = new PrefixTree();
				}
				return prototypes;
			}
			
		}
		public static PrototypeFactory Default
		{
			get
			{
				if (staticDefault == null)
				{
					rebuild();
				}
				return staticDefault;
			}
			
		}
		private static PrefixTree prototypes;
		private static PrototypeFactory staticDefault;
		
		public static void  registerPrototype(System.String className)
		{
			Prototypes.addString(className);
			
			try
			{
				//UPGRADE_TODO: The differences in the format  of parameters for method 'java.lang.Class.forName'  may cause compilation errors.  "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1092'"
				PrototypeFactory.getInstance(System.Type.GetType(className));
			}
			//UPGRADE_NOTE: Exception 'java.lang.ClassNotFoundException' was converted to 'System.Exception' which has different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1100'"
			catch (System.Exception e)
			{
				throw new CannotCreateObjectException(className + ": not found");
			}
			rebuild();
		}
		
		public static void  registerPrototypes(System.String[] classNames)
		{
			for (int i = 0; i < classNames.Length; i++)
				registerPrototype(classNames[i]);
		}
		
		private static void  rebuild()
		{
			if (staticDefault == null)
			{
				staticDefault = new PrototypeFactory(Prototypes);
				return ;
			}
			lock (staticDefault)
			{
				staticDefault = new PrototypeFactory(Prototypes);
			}
		}
	}
}