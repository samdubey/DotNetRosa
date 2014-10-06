/// <summary> </summary>
using System;
using ArrayUtilities = org.javarosa.core.util.ArrayUtilities;
using CacheTable = org.javarosa.core.util.CacheTable;
using DataUtil = org.javarosa.core.util.DataUtil;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapListPoly = org.javarosa.core.util.externalizable.ExtWrapListPoly;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
using XPathExpression = org.javarosa.xpath.expr.XPathExpression;
namespace org.javarosa.core.model.instance
{
	
	/// <author>  ctsims
	/// 
	/// </author>
	public class TreeReferenceLevel
	{
		private void  InitBlock()
		{
			TreeReferenceLevel.refs = refs;
			this.name = name;
			this.multiplicity = multiplicity;
			this.predicates = predicates;
			return new TreeReferenceLevel(name, multiplicity, xpe).intern();
			return this.predicates;
		}
		public const int MULT_UNINIT = - 16;
		
		private System.String name;
		private int multiplicity = MULT_UNINIT;
		
		private List< XPathExpression > predicates;
		
		
		private static CacheTable < TreeReferenceLevel > refs;
		
		
		public static
		
		void attachCacheTable(CacheTable < TreeReferenceLevel > refs)
		
		public TreeReferenceLevel()
		{
			InitBlock();
		}
		
		
		
		public TreeReferenceLevel(String name, int multiplicity, List< XPathExpression > predicates)
		
		public TreeReferenceLevel(System.String name, int multiplicity):this(name, multiplicity, null)
		{
		}
		
		
		public virtual int getMultiplicity()
		{
			return multiplicity;
		}
		
		public virtual System.String getName()
		{
			return name;
		}
		
		public virtual TreeReferenceLevel setMultiplicity(int mult)
		{
			return new TreeReferenceLevel(name, mult, predicates).intern();
		}
		
		public TreeReferenceLevel setPredicates;
		
		(List< XPathExpression > xpe)
		
		
		public List< XPathExpression > getPredicates()
		
		public virtual TreeReferenceLevel shallowCopy()
		{
			return new TreeReferenceLevel(name, multiplicity, ArrayUtilities.vectorCopy(predicates)).intern();
		}
		
		
		public virtual TreeReferenceLevel setName(System.String name)
		{
			return new TreeReferenceLevel(name, multiplicity, predicates).intern();
		}
		
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			name = ExtUtil.nullIfEmpty(ExtUtil.readString(in_Renamed));
			multiplicity = ExtUtil.readInt(in_Renamed);
			
			predicates = ExtUtil.nullIfEmpty((List< XPathExpression >) ExtUtil.read(in, new ExtWrapListPoly()));
		}
		
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.writeString(out_Renamed, ExtUtil.emptyIfNull(name));
			ExtUtil.writeNumeric(out_Renamed, multiplicity);
			ExtUtil.write(out_Renamed, new ExtWrapListPoly(ExtUtil.emptyIfNull(predicates)));
		}
		
		public override int GetHashCode()
		{
			int predPart = 0;
			if (predicates != null)
			{
				
				for(XPathExpression xpe: predicates)
				{
					predPart ^= xpe.hashCode();
				}
			}
			
			return name.GetHashCode() ^ multiplicity ^ predPart;
		}
		
		public  override bool Equals(System.Object o)
		{
			if (!(o is TreeReferenceLevel))
			{
				return false;
			}
			TreeReferenceLevel l = (TreeReferenceLevel) o;
			if (multiplicity != l.multiplicity)
			{
				return false;
			}
			if (name == null && l.name != null)
			{
				return false;
			}
			if (!name.Equals(l.name))
			{
				return false;
			}
			if (predicates == null && l.predicates == null)
			{
				return true;
			}
			
			if ((predicates == null && l.predicates != null) || (l.predicates == null && predicates != null))
			{
				return false;
			}
			if (predicates.size() != l.predicates.size())
			{
				return false;
			}
			for (int i = 0; i < predicates.size(); ++i)
			{
				if (!predicates.elementAt(i).equals(l.predicates.elementAt(i)))
				{
					return false;
				}
			}
			return true;
		}
		
		public static bool treeRefLevelInterningEnabled = true;
		public virtual TreeReferenceLevel intern()
		{
			if (!treeRefLevelInterningEnabled || refs == null)
			{
				return this;
			}
			else
			{
				return refs.intern(this);
			}
		}
	}
}