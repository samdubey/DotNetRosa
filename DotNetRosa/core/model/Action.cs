/// <summary> </summary>
using System;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.model
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public class Action
	{
		//Some named events
		public const System.String EVENT_XFORMS_READY = "xforms-ready";
		
		public const System.String EVENT_XFORMS_REVALIDATE = "xforms-revalidate";
		
		public const System.String EVENT_JR_INSERT = "jr-insert";
		private System.String name;
		
		public Action()
		{
		}
		
		public Action(System.String name)
		{
			this.name = name;
		}
		
		/// <summary> Process actions that were triggered in the form. 
		/// 
		/// NOTE: Currently actions are only processed on nodes that are 
		/// WITHIN the context provided, if one is provided. This will
		/// need to get changed possibly for future action types.
		/// 
		/// </summary>
		/// <param name="model">
		/// </param>
		/// <param name="context">
		/// </param>
		public virtual void  processAction(FormDef model, TreeReference context)
		{
			//TODO: Big block of handlers for basic named action types
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			name = ExtUtil.readString(in_Renamed);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.writeString(out_Renamed, name);
		}
	}
}