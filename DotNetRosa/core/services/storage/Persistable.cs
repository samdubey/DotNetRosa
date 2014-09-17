using System;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
namespace org.javarosa.core.services.storage
{
	
	/// <summary> A modest extension to Externalizable which identifies objects that have the concept of an internal 'record ID'</summary>
	public interface Persistable:Externalizable
	{
		int ID
		{
			get;
			
			set;
			
		}
	}
}