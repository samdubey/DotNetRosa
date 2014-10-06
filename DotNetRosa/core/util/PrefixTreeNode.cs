/*
* Copyright (C) 2009 JavaRosa
*
* Licensed under the Apache License, Version 2.0 (the "License"); you may not
* use this file except in compliance with the License. You may obtain a copy of
* the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
* WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
* License for the specific language governing permissions and limitations under
* the License.
*/
using System;
using System.Collections.Generic;
namespace org.javarosa.core.util
{
	
	public class PrefixTreeNode
	{
		private void  InitBlock()
		{
			System.String stem = s + new System.String(prefix);
			
			if (terminal)
			{
				v.addElement(stem);
			}
			
			if (children != null)
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
				for (System.Collections.IEnumerator e = children.elements(); e.MoveNext(); )
				{
					//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
					((PrefixTreeNode) e.Current).decompose(v, stem);
				}
			}
			return children;
		}
		virtual public char[] Prefix
		{
			get
			{
				return prefix;
			}
			
		}
		private char[] prefix;
		private bool terminal;
		
		private List< PrefixTreeNode > children;
		private PrefixTreeNode parent;
		
		public PrefixTreeNode(char[] prefix)
		{
			InitBlock();
			this.prefix = prefix;
			this.terminal = false;
		}
		
		
		public	void decompose(List< String > v, String s);


        public List<PrefixTreeNode> getChildren();
		
		public  override bool Equals(System.Object o)
		{
			//uh... is this right?
			return (o is PrefixTreeNode?prefix == ((PrefixTreeNode) o).prefix || ArrayUtilities.arraysEqual(prefix, 0, ((PrefixTreeNode) o).prefix, 0):false);
		}
		
		public override System.String ToString()
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("{");
			sb.Append(prefix);
			if (terminal)
				sb.Append("*");
			if (children != null)
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
				for (System.Collections.IEnumerator e = children.elements(); e.MoveNext(); )
				{
					//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
					sb.Append(((PrefixTreeNode) e.Current).ToString());
				}
			}
			sb.Append("}");
			return sb.ToString();
		}
		
		public virtual System.String render()
		{
			System.Text.StringBuilder temp = new System.Text.StringBuilder();
			return render(temp);
		}
		
		public virtual System.String render(System.Text.StringBuilder buffer)
		{
			if (parent != null)
			{
				parent.render(buffer);
			}
			buffer.Append(this.prefix);
			return buffer.ToString();
		}
		
		public virtual void  seal()
		{
			if (children != null)
			{
				//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
				for (System.Collections.IEnumerator e = children.elements(); e.MoveNext(); )
				{
					//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
					((PrefixTreeNode) e.Current).seal();
				}
			}
			this.children = null;
		}
		
		public virtual void  addChild(PrefixTreeNode node)
		{
			if (children == null)
			{
				
				children = new List< PrefixTreeNode >();
			}
			children.addElement(node);
			node.parent = this;
		}
		
		public virtual void  setTerminal()
		{
			//This node is now terminal (we can use this fact to clean things up)
			terminal = true;
		}
		
		public virtual PrefixTreeNode budChild(PrefixTreeNode node, char[] subPrefix, int subPrefixLen)
		{
			//make a new child for the subprefix
			PrefixTreeNode newChild = new PrefixTreeNode(subPrefix);
			
			//remove the child from our tree (we'll re-add it later)
			this.children.removeElement(node);
			node.parent = null;
			
			//cut out the middle part of the prefix (which is now this node's domain)
			char[] old = node.prefix;
			node.prefix = new char[old.Length - subPrefixLen];
			for (int i = 0; i < old.Length - subPrefixLen; ++i)
			{
				node.prefix[i] = old[subPrefixLen + i];
			}
			
			//replace the old child with the new one, and put it in the proper order
			this.addChild(newChild);
			newChild.addChild(node);
			
			return newChild;
		}
		//UPGRADE_NOTE: The following method implementation was automatically added to preserve functionality. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1306'"
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}