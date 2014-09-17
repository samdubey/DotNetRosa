using System;
namespace org.javarosa.core.util
{
	
	/// <summary> 
	/// Only use for J2ME Compatible Vectors
	/// 
	/// A SizeBoundVector that enforces that all member items be unique. You must
	/// implement the .equals() method of class E
	/// 
	/// </summary>
	/// <author>  wspride
	/// 
	/// </author>
	public class SizeBoundUniqueVector
	{
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		< E > extends SizeBoundVector < E >
		
		public SizeBoundUniqueVector(int sizeLimit):base(sizeLimit)
		{
		}
		
		/* (non-Javadoc)
		* @see java.util.Vector#addElement(java.lang.Object)
		*/
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'addElement'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public virtual void  addElement(E obj)
		{
			lock (this)
			{
				add(obj);
			}
		}
		
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'add'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public virtual bool add(E obj)
		{
			lock (this)
			{
				if (this.size() == limit)
				{
					additional++;
					return true;
				}
				else if (this.contains(obj))
				{
					return false;
				}
				else
				{
					base.addElement(obj);
					return true;
				}
			}
		}
	}
}