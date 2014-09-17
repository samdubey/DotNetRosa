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
namespace org.javarosa.core.util
{
	
	public class PrefixTree
	{
		private void  InitBlock()
		{
			if (finalized)
			{
				throw new System.SystemException("Can't get the strings from a finalized Prefix Tree");
			}
			//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
			Vector < String > v = new Vector < String >();
			root.decompose(v, "");
			return v;
		}
		//Sometimes the string optimizations here are basically useless 
		//due to wide availability of memory. It's easier in many cases
		//to simply keep using the framework, but just disable the actual
		//stemming/prefix ops
		internal bool disablePrefixing = false;
		
		private PrefixTreeNode root;
		
		internal int minimumPrefixLength;
		internal int minimumHeuristicLength;
		
		//Common delimeters which we'd prefer as prefix breaks rather than
		//maximum string space
		//UPGRADE_NOTE: Final was removed from the declaration of 'delimiters'. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1003'"
		private static readonly char[] delimiters = new char[]{'\\', '/', '.'};
		private const int delSacrifice = 3;
		internal bool finalized = false;
		
		public PrefixTree():this(0)
		{
		}
		
		public PrefixTree(int minimumPrefixLength)
		{
			InitBlock();
			root = new PrefixTreeNode(new char[0]);
			this.minimumPrefixLength = System.Math.Max(minimumPrefixLength++, 0);
			this.minimumHeuristicLength = System.Math.Max((int) (minimumPrefixLength / 2), 3);
		}
		
		public static int sharedPrefixLength(char[] a, int aStart, char[] b)
		{
			int len;
			int minLength = System.Math.Min(a.Length - aStart, b.Length);
			
			for (len = 0; len < minLength; len++)
			{
				if (a[len + aStart] != b[len])
					break;
			}
			
			return len;
		}
		
		public virtual PrefixTreeNode addString(System.String newString)
		{
			if (finalized)
			{
				throw new System.SystemException("Can't manipulate a finalized Prefix Tree");
			}
			
			if (disablePrefixing)
			{
				PrefixTreeNode newNode = new PrefixTreeNode(newString.ToCharArray());
				newNode.setTerminal();
				root.addChild(newNode);
				return newNode;
			}
			
			PrefixTreeNode current = root;
			
			char[] chars = newString.ToCharArray();
			int currentIndex = 0;
			
			while (currentIndex < chars.Length)
			{
				
				//The length of the string we've incorporated into the tree
				int len = 0;
				
				//The (potential) next node in the tree which prefixes the rest of the string
				PrefixTreeNode node = null;
				
				//TODO: This would be way faster if we sorted upon insertion....
				if (current.getChildren() != null)
				{
					//UPGRADE_TODO: Method 'java.util.Enumeration.hasMoreElements' was converted to 'System.Collections.IEnumerator.MoveNext' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationhasMoreElements'"
					for (System.Collections.IEnumerator e = current.getChildren().elements(); e.MoveNext(); )
					{
						//UPGRADE_TODO: Method 'java.util.Enumeration.nextElement' was converted to 'System.Collections.IEnumerator.Current' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javautilEnumerationnextElement'"
						node = (PrefixTreeNode) e.Current;
						
						char[] prefix = node.Prefix;
						//if(prefix.equals(s)) {
						if (ArrayUtilities.arraysEqual(prefix, 0, chars, currentIndex))
						{
							return node;
						}
						
						len = sharedPrefixLength(chars, currentIndex, prefix);
						if (len > minimumPrefixLength)
						{
							//See if we have any breaks which might make more heuristic sense than simply grabbing the biggest
							//difference
							//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
							for(char c: delimiters)
							{
								int sepLen = - 1;
								for (int i = currentIndex + len - 1; i >= currentIndex; i--)
								{
									if (chars[i] == c)
									{
										sepLen = i - currentIndex;
										break;
									}
								}
								if (sepLen != - 1 && len - sepLen < delSacrifice && sepLen > minimumHeuristicLength)
								{
									len = sepLen;
									break;
								}
							}
							
							break;
						}
						node = null;
					}
				}
				
				//If we didn't find anything that shared any common roots
				if (node == null)
				{
					//Create a placeholder for the rest of the string
					char[] newArray;
					if (currentIndex == 0)
					{
						newArray = chars;
					}
					else
					{
						newArray = new char[chars.Length - currentIndex];
						for (int i = 0; i < chars.Length - currentIndex; ++i)
						{
							newArray[i] = chars[i + currentIndex];
						}
					}
					node = new PrefixTreeNode(newArray);
					
					len = chars.Length - currentIndex;
					
					//Add this to the highest level prefix we've found
					current.addChild(node);
				}
				//Otherwise check to see if we are going to split the current prefix
				else if (len < node.Prefix.Length)
				{
					char[] newPrefix = new char[len];
					for (int i = 0; i < len; ++i)
					{
						newPrefix[i] = chars[currentIndex + i];
					}
					
					PrefixTreeNode interimNode = current.budChild(node, newPrefix, len);
					
					node = interimNode;
				}
				
				current = node;
				currentIndex = currentIndex + len;
			}
			
			current.setTerminal();
			return current;
		}
		
		//UPGRADE_ISSUE: The following fragment of code could not be parsed and was not converted. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1156'"
		public Vector < String > getStrings()
		
		public override System.String ToString()
		{
			return root.ToString();
		}
		public virtual void  seal()
		{
			//System.out.println(toString());
			root.seal();
			finalized = true;
		}
		
		public virtual void  clear()
		{
			finalized = false;
			root = new PrefixTreeNode(new char[0]);
		}
	}
}