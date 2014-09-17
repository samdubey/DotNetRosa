using System;
using Selection = org.javarosa.core.model.data.helper.Selection;
using TreeElement = org.javarosa.core.model.instance.TreeElement;
using Localizable = org.javarosa.core.services.locale.Localizable;
using Localizer = org.javarosa.core.services.locale.Localizer;
using DeserializationException = org.javarosa.core.util.externalizable.DeserializationException;
using ExtUtil = org.javarosa.core.util.externalizable.ExtUtil;
using Externalizable = org.javarosa.core.util.externalizable.Externalizable;
using PrototypeFactory = org.javarosa.core.util.externalizable.PrototypeFactory;
using FormEntryPrompt = org.javarosa.form.api.FormEntryPrompt;
using XFormParseException = org.javarosa.xform.parse.XFormParseException;
namespace org.javarosa.core.model
{
	
	public class SelectChoice : Localizable
	{
		virtual public int Index
		{
			get
			{
				if (index == - 1)
				{
					throw new System.SystemException("trying to access choice index before it has been set!");
				}
				
				return index;
			}
			
			set
			{
				this.index = value;
			}
			
		}
		virtual public System.String Value
		{
			get
			{
				return value_Renamed;
			}
			
		}
		virtual public bool Localizable
		{
			get
			{
				return isLocalizable_Renamed_Field;
			}
			
			set
			{
				this.isLocalizable_Renamed_Field = value;
			}
			
		}
		virtual public System.String TextID
		{
			get
			{
				return textID;
			}
			
			set
			{
				this.textID = value;
			}
			
		}
		
		private System.String labelInnerText;
		private System.String textID;
		private bool isLocalizable_Renamed_Field;
		private System.String value_Renamed;
		private int index = - 1;
		
		public TreeElement copyNode; //if this choice represents part of an <itemset>, and the itemset uses 'copy'
		//answer mode, this points to the node to be copied if this selection is chosen
		//this field only has meaning for dynamic choices, thus is unserialized
		
		//for deserialization only
		public SelectChoice()
		{
		}
		
		public SelectChoice(System.String labelID, System.String value_Renamed):this(labelID, null, value_Renamed, true)
		{
		}
		
		/// <summary> </summary>
		/// <param name="labelID">can be null
		/// </param>
		/// <param name="labelInnerText">can be null
		/// </param>
		/// <param name="value">should not be null
		/// </param>
		/// <param name="isLocalizable">
		/// </param>
		/// <throws>  XFormParseException if value is null </throws>
		public SelectChoice(System.String labelID, System.String labelInnerText, System.String value_Renamed, bool isLocalizable)
		{
			this.isLocalizable_Renamed_Field = isLocalizable;
			this.textID = labelID;
			this.labelInnerText = labelInnerText;
			if (value_Renamed != null)
			{
				this.value_Renamed = value_Renamed;
			}
			else
			{
				throw new XFormParseException("SelectChoice{id,innerText}:{" + labelID + "," + labelInnerText + "}, has null Value!");
			}
		}
		
		public SelectChoice(System.String labelOrID, System.String Value, bool isLocalizable):this(isLocalizable?labelOrID:null, isLocalizable?null:labelOrID, Value, isLocalizable)
		{
		}
		
		
		public virtual System.String getLabelInnerText()
		{
			return labelInnerText;
		}
		
		
		public virtual void  localeChanged(System.String locale, Localizer localizer)
		{
			//		if (captionLocalizable) {
			//			caption = localizer.getLocalizedText(captionID);
			//		}
		}
		
		//UPGRADE_TODO: Class 'java.io.DataInputStream' was converted to 'System.IO.BinaryReader' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataInputStream'"
		public virtual void  readExternal(System.IO.BinaryReader in_Renamed, PrototypeFactory pf)
		{
			isLocalizable_Renamed_Field = ExtUtil.readBool(in_Renamed);
			setLabelInnerText(ExtUtil.nullIfEmpty(ExtUtil.readString(in_Renamed)));
			TextID = ExtUtil.nullIfEmpty(ExtUtil.readString(in_Renamed));
			value_Renamed = ExtUtil.nullIfEmpty(ExtUtil.readString(in_Renamed));
			//index will be set by questiondef
		}
		
		//UPGRADE_TODO: Class 'java.io.DataOutputStream' was converted to 'System.IO.BinaryWriter' which has a different behavior. "ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?index='!DefaultContextWindowIndex'&keyword='jlca1073_javaioDataOutputStream'"
		public virtual void  writeExternal(System.IO.BinaryWriter out_Renamed)
		{
			ExtUtil.writeBool(out_Renamed, isLocalizable_Renamed_Field);
			ExtUtil.writeString(out_Renamed, ExtUtil.emptyIfNull(labelInnerText));
			ExtUtil.writeString(out_Renamed, ExtUtil.emptyIfNull(textID));
			ExtUtil.writeString(out_Renamed, ExtUtil.emptyIfNull(value_Renamed));
			//don't serialize index; it will be restored from questiondef
		}
		
		private void  setLabelInnerText(System.String labelInnerText)
		{
			this.labelInnerText = labelInnerText;
		}
		
		public virtual Selection selection()
		{
			return new Selection(this);
		}
		
		public override System.String ToString()
		{
			return ((textID != null && (System.Object) textID != (System.Object) "")?"{" + textID + "}":"") + (labelInnerText != null?labelInnerText:"") + " => " + value_Renamed;
		}
	}
}