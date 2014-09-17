/// <summary> </summary>
using System;
namespace org.javarosa.core.util
{
	
	/// <summary> 
	/// Only use for J2ME Compatible Vectors 
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public class SizeBoundVector
	{
		private void  InitBlock()
		{
			
			int limit = - 1;
			int additional = 0;
			
			int badImageReferenceCount = 0;
			int badAudioReferenceCount = 0;
			int badVideoReferenceCount = 0;
		}
		virtual public int Additional
		{
			get
			{
				return additional;
			}
			
		}
		virtual public int BadImageReferenceCount
		{
			get
			{
				return badImageReferenceCount;
			}
			
		}
		virtual public int BadAudioReferenceCount
		{
			get
			{
				return badAudioReferenceCount;
			}
			
		}
		virtual public int BadVideoReferenceCount
		{
			get
			{
				return badVideoReferenceCount;
			}
			
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		< E > extends Vector < E >
		
		public SizeBoundVector(int sizeLimit)
		{
			InitBlock();
			this.limit = sizeLimit;
		}
		
		/* (non-Javadoc)
		* @see java.util.Vector#addElement(java.lang.Object)
		*/
		//UPGRADE_NOTE: Synchronized keyword was removed from method 'addElement'. Lock expression was added. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1027'"
		public virtual void  addElement(E obj)
		{
			lock (this)
			{
				if (this.size() == limit)
				{
					additional++;
					return ;
				}
				else
				{
					base.addElement(obj);
				}
			}
		}
		
		public virtual void  addBadImageReference()
		{
			badImageReferenceCount++;
		}
		public virtual void  addBadAudioReference()
		{
			badAudioReferenceCount++;
		}
		public virtual void  addBadVideoReference()
		{
			badVideoReferenceCount++;
		}
	}
}