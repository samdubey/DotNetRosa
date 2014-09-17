/// <summary> </summary>
using System;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.reference
{
	
	/// <summary> A Root Translator is a simple reference factory which doesn't
	/// actually derive any specific references, but rather translates
	/// references from one prefix to another. This is useful for roots
	/// which don't describe any real raw accessor like "jr://media/",
	/// which could access a file reference (jr://file/) on one platform,
	/// but a resource reference (jr://resource/) on another.
	/// 
	/// Root Translators can be externalized and used as a dynamically 
	/// configured object.
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public class RootTranslator : ReferenceFactory
	{
		
		public System.String prefix;
		public System.String translatedPrefix;
		
		/// <summary> Serialization only!</summary>
		public RootTranslator()
		{
		}
		
		/// <summary> Creates a translator which will create references of the 
		/// type described by translatedPrefix whenever references of
		/// the type prefix are being derived.
		/// 
		/// </summary>
		/// <param name="prefix">
		/// </param>
		/// <param name="translatedPrefix">
		/// </param>
		public RootTranslator(System.String prefix, System.String translatedPrefix)
		{
			//TODO: Manage semantics of "ends with /" etc here?
			this.prefix = prefix;
			this.translatedPrefix = translatedPrefix;
		}
		
		/* (non-Javadoc)
		* @see org.commcare.reference.Root#derive(java.lang.String)
		*/
		public virtual Reference derive(System.String URI)
		{
			return ReferenceManager._().DeriveReference(translatedPrefix + URI.Substring(prefix.Length));
		}
		
		/* (non-Javadoc)
		* @see org.commcare.reference.Root#derive(java.lang.String, java.lang.String)
		*/
		public virtual Reference derive(System.String URI, System.String context)
		{
			return ReferenceManager._().DeriveReference(URI, translatedPrefix + context.Substring(prefix.Length));
		}
		
		/* (non-Javadoc)
		* @see org.commcare.reference.Root#derives(java.lang.String)
		*/
		public virtual bool derives(System.String URI)
		{
			if (URI.StartsWith(prefix))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			prefix = ExtUtil.readString(in_Renamed);
			translatedPrefix = ExtUtil.readString(in_Renamed);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.writeString(out_Renamed, prefix);
			ExtUtil.writeString(out_Renamed, translatedPrefix);
		}
	}
}