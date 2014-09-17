/// <summary> </summary>
using System;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.model.data
{
	
	/// <summary> Uncast data values are those which are not assigned a particular
	/// data type. This is relevant when data is read before a datatype is
	/// available, or when it must be pulled from external instances.
	/// 
	/// In general, Uncast data should be used when a value is available
	/// in string form, and no adequate assumption can be made about the type
	/// of data being represented. This is preferable to making the assumption
	/// that data is a StringData object, since that will cause issues when
	/// select choices or other typed values are expected.
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public class UncastData : IAnswerData, System.ICloneable
	{
		virtual public System.String DisplayText
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.model.data.IAnswerData#getDisplayText()
			*/
			
			get
			{
				return value_Renamed;
			}
			
		}
		virtual public System.Object Value
		{
			/* (non-Javadoc)
			* @see org.javarosa.core.model.data.IAnswerData#getValue()
			*/
			
			get
			{
				return value_Renamed;
			}
			
			/* (non-Javadoc)
			* @see org.javarosa.core.model.data.IAnswerData#setValue(java.lang.Object)
			*/
			
			set
			{
				value_Renamed = ((System.String) value);
			}
			
		}
		/// <returns> The string representation of this data. This value should be
		/// castable into its appropriate data type.
		/// </returns>
		virtual public System.String String
		{
			get
			{
				return value_Renamed;
			}
			
		}
		internal System.String value_Renamed;
		
		public UncastData()
		{
		}
		
		public UncastData(System.String value_Renamed)
		{
			if (value_Renamed == null)
			{
				throw new System.NullReferenceException("Attempt to set Uncast Data value to null! IAnswerData objects should never have null values");
			}
			this.value_Renamed = value_Renamed;
		}
		
		//UPGRADE_ISSUE: The equivalent in .NET for method 'java.lang.Object.clone' returns a different type. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1224'"
		public virtual System.Object Clone()
		{
			return new UncastData(value_Renamed);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.externalizable.Externalizable#readExternal(java.io.DataInputStream, org.javarosa.core.util.externalizable.PrototypeFactory)
		*/
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			value_Renamed = ExtUtil.readString(in_Renamed);
		}
		
		/* (non-Javadoc)
		* @see org.javarosa.core.util.externalizable.Externalizable#writeExternal(java.io.DataOutputStream)
		*/
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.writeString(out_Renamed, value_Renamed);
		}
		
		public virtual UncastData uncast()
		{
			return this;
		}
		
		public virtual UncastData cast(UncastData data)
		{
			return new UncastData(data.value_Renamed);
		}
	}
}