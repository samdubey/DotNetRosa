/// <summary> </summary>
using System;
namespace org.javarosa.core.util
{
	
	/// <summary> Directed A-cyclic (NOT ENFORCED) graph datatype.
	/// 
	/// Genericized with two types: An unique index value (representing the node) and a generic
	/// set of data to associate with that node
	/// 
	/// </summary>
	/// <author>  ctsims
	/// 
	/// </author>
	public class DAG
	{
		private void  InitBlock()
		{
			//TODO: This is a really unsafe datatype. Needs an absurd amount of updating for representation
			//invariance, synchronicity, cycle detection, etc.
			
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Hashtable < I, N > nodes;
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Hashtable < I, Vector < I >> edge;
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Hashtable < I, Vector < I >> inverse;;
			if (edgeList.containsKey(a))
			{
				edge = edgeList.get_Renamed(a);
			}
			else
			{
				//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
				edge = new Vector < I >();
			}
			edge.addElement(b);
			edgeList.put(a, edge);
			if (inverse.containsKey(index))
			{
				return inverse.get_Renamed(index);
			}
			else
			{
				return null;
			}
			if (!edge.containsKey(index))
			{
				return null;
			}
			else
			{
				return edge.get_Renamed(index);
			}
			//UPGRADE_NOTE: There is an untranslated Statement.  Please refer to original code. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1153'"
			//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
			for (System.Collections.IEnumerator en = nodes.keys(); en.MoveNext(); )
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
				I i = (I) en.Current;
				if (!inverse.containsKey(i))
				{
					roots.addElement(i);
				}
			}
			return roots;
		}
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		< I, N >
		
		public DAG()
		{
			InitBlock();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			nodes = new Hashtable < I, N >();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			edge = new Hashtable < I, Vector < I >>();
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			inverse = new Hashtable < I, Vector < I >>();
		}
		
		public virtual void  addNode(I i, N n)
		{
			nodes.put(i, n);
		}
		
		/// <summary> Connect Source -> Destination</summary>
		/// <param name="source">
		/// </param>
		/// <param name="destination">
		/// </param>
		public virtual void  setEdge(I source, I destination)
		{
			addToEdge(edge, source, destination);
			addToEdge(inverse, destination, source);
		}
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		private
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		void addToEdge(Hashtable < I, Vector < I >> edgeList, I a, I b)
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public Vector < I > getParents(I index)
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public Vector < I > getChildren(I index)
		
		public virtual N getNode(I index)
		{
			return nodes.get_Renamed(index);
		}
		
		//Is that the right name?
		/// <returns> Indices for all nodes in the graph which are not the target of
		/// any edges in the graph
		/// </returns>
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public Stack < I > getSources()
	}
}