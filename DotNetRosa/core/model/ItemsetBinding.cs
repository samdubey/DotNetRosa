using System;
using IConditionExpr = org.javarosa.core.model.condition.IConditionExpr;
using FormInstance = org.javarosa.core.model.instance.FormInstance;
using TreeReference = org.javarosa.core.model.instance.TreeReference;
using RestoreUtils = org.javarosa.core.model.util.restorable.RestoreUtils;
using Localizable = org.javarosa.core.services.locale.Localizable;
using Localizer = org.javarosa.core.services.locale.Localizer;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using ExtWrapNullable = org.javarosa.core.util.externalizable.ExtWrapNullable;
using ExtWrapTagged = org.javarosa.core.util.externalizable.ExtWrapTagged;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
namespace org.javarosa.core.model
{
	
	public class ItemsetBinding : Localizable
	{
		public ItemsetBinding()
		{
			InitBlock();
		}
		private void  InitBlock()
		{
			return choices;
			if (this.choices != null)
			{
				System.Console.Out.WriteLine("warning: previous choices not cleared out");
				clearChoices();
			}
			this.choices = choices;
			
			//init localization
			if (localizer != null)
			{
				System.String curLocale = localizer.getLocale();
				if (curLocale != null)
				{
					localeChanged(curLocale, localizer);
				}
			}
		}
		virtual public IConditionExpr RelativeValue
		{
			get
			{
				TreeReference relRef = null;
				
				if (copyRef == null)
				{
					relRef = valueRef; //must be absolute in this case
				}
				else if (valueRef != null)
				{
					relRef = valueRef.relativize(copyRef);
				}
				
				return relRef != null?RestoreUtils.xfFact.refToPathExpr(relRef):null;
			}
			
		}
		
		/// <summary> note that storing both the ref and expr for everything is kind of redundant, but we're forced
		/// to since it's nearly impossible to convert between the two w/o having access to the underlying
		/// xform/xpath classes, which we don't from the core model project
		/// </summary>
		
		public TreeReference nodesetRef; //absolute ref of itemset source nodes
		public IConditionExpr nodesetExpr; //path expression for source nodes; may be relative, may contain predicates
		public TreeReference contextRef; //context ref for nodesetExpr; ref of the control parent (group/formdef) of itemset question
		//note: this is only here because its currently impossible to both (a) get a form control's parent, and (b)
		//convert expressions into refs while preserving predicates. once these are fixed, this field can go away
		
		public TreeReference labelRef; //absolute ref of label
		public IConditionExpr labelExpr; //path expression for label; may be relative, no predicates  
		public bool labelIsItext; //if true, content of 'label' is an itext id
		
		public bool copyMode; //true = copy subtree; false = copy string value
		public TreeReference copyRef; //absolute ref to copy
		public TreeReference valueRef; //absolute ref to value
		public IConditionExpr valueExpr; //path expression for value; may be relative, no predicates (must be relative if copy mode)
		
		private TreeReference destRef; //ref that identifies the repeated nodes resulting from this itemset
		//not serialized -- set by QuestionDef.setDynamicChoices()
		
		private List< SelectChoice > choices; //dynamic choices -- not serialized, obviously
		
		
		public List< SelectChoice > getChoices()
		
		
		public
		
		void setChoices(List< SelectChoice > choices, Localizer localizer)
		
		public virtual void  clearChoices()
		{
			this.choices = null;
		}
		
		public virtual void  localeChanged(System.String locale, Localizer localizer)
		{
			if (choices != null)
			{
				for (int i = 0; i < choices.size(); i++)
				{
					choices.elementAt(i).localeChanged(locale, localizer);
				}
			}
		}
		
		public virtual void  setDestRef(QuestionDef q)
		{
			destRef = FormInstance.unpackReference(q.Bind).Clone();
			if (copyMode)
			{
				destRef.add(copyRef.NameLast, TreeReference.INDEX_UNBOUND);
			}
		}
		
		public virtual TreeReference getDestRef()
		{
			return destRef;
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			nodesetRef = (TreeReference) ExtUtil.read(in_Renamed, typeof(TreeReference), pf);
			nodesetExpr = (IConditionExpr) ExtUtil.read(in_Renamed, new ExtWrapTagged(), pf);
			contextRef = (TreeReference) ExtUtil.read(in_Renamed, typeof(TreeReference), pf);
			labelRef = (TreeReference) ExtUtil.read(in_Renamed, typeof(TreeReference), pf);
			labelExpr = (IConditionExpr) ExtUtil.read(in_Renamed, new ExtWrapTagged(), pf);
			valueRef = (TreeReference) ExtUtil.read(in_Renamed, new ExtWrapNullable(typeof(TreeReference)), pf);
			valueExpr = (IConditionExpr) ExtUtil.read(in_Renamed, new ExtWrapNullable(new ExtWrapTagged()), pf);
			copyRef = (TreeReference) ExtUtil.read(in_Renamed, new ExtWrapNullable(typeof(TreeReference)), pf);
			labelIsItext = ExtUtil.readBool(in_Renamed);
			copyMode = ExtUtil.readBool(in_Renamed);
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.write(out_Renamed, nodesetRef);
			ExtUtil.write(out_Renamed, new ExtWrapTagged(nodesetExpr));
			ExtUtil.write(out_Renamed, contextRef);
			ExtUtil.write(out_Renamed, labelRef);
			ExtUtil.write(out_Renamed, new ExtWrapTagged(labelExpr));
			ExtUtil.write(out_Renamed, new ExtWrapNullable(valueRef));
			ExtUtil.write(out_Renamed, new ExtWrapNullable(valueExpr == null?null:new ExtWrapTagged(valueExpr)));
			ExtUtil.write(out_Renamed, new ExtWrapNullable(copyRef));
			ExtUtil.writeBool(out_Renamed, labelIsItext);
			ExtUtil.writeBool(out_Renamed, copyMode);
		}
	}
}