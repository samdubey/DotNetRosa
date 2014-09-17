/// <summary> </summary>
using System;
using Action = org.javarosa.core.model.Action;
using FormDef = org.javarosa.core.model.FormDef;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.model.actions
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public class ActionWrapper:Action
	{
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		Vector < Action > listOfActions = new Vector < Action >();
		
		public ActionWrapper():base("action")
		{
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.Action#processAction(org.javarosa.core.model.FormDef)
		*/
		public override void  processAction(FormDef target, TreeReference context)
		{
			base.processAction(target, context);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.Action#readExternal(java.io.DataInputStream, org.javarosa.core.util.externalizable.PrototypeFactory)
		*/
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public override void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			base.readExternal(in_Renamed, pf);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.model.Action#writeExternal(java.io.DataOutputStream)
		*/
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public override void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			base.writeExternal(out_Renamed);
		}
	}
}