
using UnityEngine;
using System.Collections;

public class ShowDialogueStep : EventStep
{
	public ShowDialogueStep(GameEventType t) : base(t)
	{
		for(int i=0; i<this.message.Length; i++)
		{
			this.message[i] = "";
		}
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		string nm = "";
		SpeakerPortrait sp = null;
		if(this.show3)
		{
			nm = gameEvent.GetActorName(this.actorID);
		}
		else
		{
			nm = "";
		}
		
		string msg = "";
		if(GameHandler.GetLanguage() < this.message.Length)
		{
			msg = this.message[GameHandler.GetLanguage()];
		}
		else
		{
			msg = this.message[0];
		}
		if(this.show4)
		{
			sp = new SpeakerPortrait(this.scene, this.v2, this.show5);
		}
		
		GameHandler.GetLevelHandler().ShowDialogue(nm, msg, this.number, 
				gameEvent, this.next, this.show, this.time, this.show2, sp);
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		
		ht.Add("number", this.number.ToString());
		ht.Add("show3", this.show3.ToString());
		if(this.show)
		{
			ht.Add("show", this.show.ToString());
			ht.Add("show2", this.show2.ToString());
			ht.Add("time", this.time.ToString());
		}
		if(this.show3) ht.Add("actor", this.actorID.ToString());
		
		ArrayList subs = new ArrayList();
		for(int i=0; i<this.message.Length; i++)
		{
			Hashtable s = new Hashtable();
			s.Add(XMLHandler.NODE_NAME, "message");
			s.Add("id", i.ToString());
			s.Add(XMLHandler.CONTENT, this.message[i]);
			subs.Add(s);
		}
		if(this.show4)
		{
			ht.Add("show4", this.show4.ToString());
			ht.Add("show5", this.show5.ToString());
			
			Hashtable s = new Hashtable();
			s.Add(XMLHandler.NODE_NAME, "vector2");
			s.Add("x", this.v2.x.ToString());
			s.Add("y", this.v2.y.ToString());
			subs.Add(s);
			
			s = new Hashtable();
			s.Add(XMLHandler.NODE_NAME, "scene");
			s.Add(XMLHandler.CONTENT, this.scene);
			subs.Add(s);
		}
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class ShowChoiceStep : EventStep
{
	private int[] dummyNext = new int[0];
	
	public ShowChoiceStep(GameEventType t) : base(t)
	{
		for(int i=0; i<this.message.Length; i++)
		{
			this.message[i] = "";
		}
		for(int i=0; i<this.choiceNext.Length; i++)
		{
			this.choice.Add(new string[this.message.Length]);
			for(int j=0; j<((string[])this.choice[i]).Length; j++)
			{
				((string[])this.choice[i])[j] = "";
			}
			this.variableCondition[i] = new VariableCondition();
		}
	}
	
	public void AddChoice()
	{
		this.choiceNext = ArrayHelper.Add(0, this.choiceNext);
		this.addVariableCondition = ArrayHelper.Add(false, this.addVariableCondition);
		this.variableCondition = ArrayHelper.Add(new VariableCondition(), this.variableCondition);
		this.addItem = ArrayHelper.Add(false, this.addItem);
		this.itemChoiceType = ArrayHelper.Add(ItemDropType.ITEM, this.itemChoiceType);
		this.itemChoice = ArrayHelper.Add(0, this.itemChoice);
		this.itemChoiceQuantity = ArrayHelper.Add(1, this.itemChoiceQuantity);
		
		this.choice.Add(new string[this.message.Length]);
		for(int j=0; j<((string[])this.choice[this.choice.Count-1]).Length; j++)
		{
			((string[])this.choice[this.choice.Count-1])[j] = "";
		}
	}
	
	public void RemoveChoice(int index)
	{
		this.choiceNext = ArrayHelper.Remove(index, this.choiceNext);
		this.addVariableCondition = ArrayHelper.Remove(index, this.addVariableCondition);
		this.variableCondition = ArrayHelper.Remove(index, this.variableCondition);
		this.addItem = ArrayHelper.Remove(index, this.addItem);
		this.itemChoiceType = ArrayHelper.Remove(index, this.itemChoiceType);
		this.itemChoice = ArrayHelper.Remove(index, this.itemChoice);
		this.itemChoiceQuantity = ArrayHelper.Remove(index, this.itemChoiceQuantity);
		this.choice.RemoveAt(index);
	}
	
	public string GetChoiceText(int index, int lang)
	{
		string txt = "";
		if(lang < ((string[])this.choice[index]).Length) txt = ((string[])this.choice[index])[lang];
		else txt = ((string[])this.choice[index])[0];
		if(this.addItem[index])
		{
			txt = txt.Replace("%n", DataHolder.GetItemName(this.itemChoiceType[index], this.itemChoice[index]));
			txt = txt.Replace("%", this.itemChoiceQuantity[index].ToString());
		}
		return txt;
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		string nm = "";
		SpeakerPortrait sp = null;
		if(this.show3)
		{
			nm = gameEvent.GetActorName(this.actorID);
		}
		else
		{
			nm = "";
		}
		
		int lang = GameHandler.GetLanguage();
		string msg = "";
		if(lang < this.message.Length)
		{
			msg = this.message[lang];
		}
		else
		{
			msg = this.message[0];
		}
		
		string[] ch = new string[0];
		this.dummyNext = new int[0];
		for(int i=0; i<this.choice.Count; i++)
		{
			if(!this.addVariableCondition[i] || this.variableCondition[i].CheckVariables())
			{
				ch = ArrayHelper.Add(this.GetChoiceText(i, lang), ch);
				this.dummyNext = ArrayHelper.Add(this.choiceNext[i], this.dummyNext);
			}
		}
		
		if(this.show4)
		{
			sp = new SpeakerPortrait(this.scene, this.v2, this.show5);
		}
		
		GameHandler.GetLevelHandler().ShowChoice(nm, msg, ch, this.number, gameEvent, sp);
	}
	
	public override void ChoiceSelected(int index, GameEvent gameEvent)
	{
		if(index < this.dummyNext.Length)
		{
			if(this.addItem[index])
			{
				GameHandler.AddToInventory(this.itemChoiceType[index], 
						this.itemChoice[index], this.itemChoiceQuantity[index]);
			}
			gameEvent.StepFinished(this.dummyNext[index]);
		}
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("show3", this.show3.ToString());
		if(this.show3) ht.Add("actor", this.actorID.ToString());
		ht.Add("choices", this.choiceNext.Length.ToString());
		ht.Add("number", this.number.ToString());
		
		ArrayList subs = new ArrayList();
		for(int i=0; i<this.message.Length; i++)
		{
			Hashtable s = new Hashtable();
			s.Add(XMLHandler.NODE_NAME, "message");
			s.Add("id", i.ToString());
			s.Add(XMLHandler.CONTENT, this.message[i]);
			subs.Add(s);
		}
		for(int i=0; i<this.choiceNext.Length; i++)
		{
			Hashtable s = new Hashtable();
			s.Add(XMLHandler.NODE_NAME, "choice");
			s.Add("id", i.ToString());
			s.Add("next", this.choiceNext[i].ToString());
			ArrayList subs2 = new ArrayList();
			for(int j=0; j<((string[])this.choice[i]).Length; j++)
			{
				Hashtable ss = new Hashtable();
				ss.Add(XMLHandler.NODE_NAME, "choicetext");
				ss.Add("id", j.ToString());
				ss.Add(XMLHandler.CONTENT, ((string[])this.choice[i])[j]);
				subs2.Add(ss);
			}
			if(this.addVariableCondition[i])
			{
				subs2.Add(this.variableCondition[i].GetData("variablecondition"));
			}
			if(this.addItem[i])
			{
				Hashtable ss = HashtableHelper.GetTitleHashtable("item", this.itemChoice[i]);
				ss.Add("type", this.itemChoiceType[i].ToString());
				ss.Add("quantity", this.itemChoiceQuantity[i].ToString());
				subs2.Add(ss);
			}
			s.Add(XMLHandler.NODES, subs2);
			subs.Add(s);
		}
		if(this.show4)
		{
			ht.Add("show4", this.show4.ToString());
			ht.Add("show5", this.show5.ToString());
			
			Hashtable s = new Hashtable();
			s.Add(XMLHandler.NODE_NAME, "vector2");
			s.Add("x", this.v2.x.ToString());
			s.Add("y", this.v2.y.ToString());
			subs.Add(s);
			
			s = new Hashtable();
			s.Add(XMLHandler.NODE_NAME, "scene");
			s.Add(XMLHandler.CONTENT, this.scene);
			subs.Add(s);
		}
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}

public class TeleportChoiceStep : EventStep
{
	private int[] dummyNext = new int[0];
	
	public TeleportChoiceStep(GameEventType t) : base(t)
	{
		for(int i=0; i<this.message.Length; i++)
		{
			this.message[i] = "";
		}
		for(int i=0; i<this.choiceNext.Length; i++)
		{
			this.choice.Add(new string[this.message.Length]);
			for(int j=0; j<((string[])this.choice[i]).Length; j++)
			{
				((string[])this.choice[i])[j] = "";
			}
		}
	}
	
	public override void Execute(GameEvent gameEvent)
	{
		int[] ids = DataHolder.Teleports().GetTeleportIDs(this.show7);
		
		if(ids.Length == 0)
		{
			gameEvent.StepFinished(this.nextFail);
		}
		else
		{
			string[] ch = DataHolder.Teleports().GetNamesForIDs(ids);
			
			int lang = GameHandler.GetLanguage();
			if(this.show6)
			{
				ids = ArrayHelper.Add(this.choiceNext[0], ids);
				
				if(lang < ((string[])this.choice[0]).Length)
				{
					ch = ArrayHelper.Add(((string[])this.choice[0])[lang], ch);
				}
				else
				{
					ch = ArrayHelper.Add(((string[])this.choice[0])[0], ch);
				}
			}
			gameEvent.SetTeleportIDs(ids, this.show6);
			
			string nm = "";
			SpeakerPortrait sp = null;
			if(this.show3)
			{
				nm = gameEvent.GetActorName(this.actorID);
			}
			else
			{
				nm = "";
			}
			
			string msg = "";
			if(lang < this.message.Length)
			{
				msg = this.message[lang];
			}
			else
			{
				msg = this.message[0];
			}
			
			if(this.show4)
			{
				sp = new SpeakerPortrait(this.scene, this.v2, this.show5);
			}
			
			this.dummyNext = new int[ids.Length];
			for(int i=0; i<this.dummyNext.Length; i++) this.dummyNext[i] = i;
			GameHandler.GetLevelHandler().ShowChoice(nm, msg, ch, this.number, gameEvent, sp);
		}
	}
	
	public override void ChoiceSelected(int index, GameEvent gameEvent)
	{
		if(index < this.dummyNext.Length)
		{
			gameEvent.StepFinished(this.dummyNext[index]);
		}
	}
	
	public override Hashtable GetData()
	{
		Hashtable ht = base.GetData();
		ht.Add("show3", this.show3.ToString());
		if(this.show3) ht.Add("actor", this.actorID.ToString());
		ht.Add("number", this.number.ToString());
		ht.Add("nextfail", this.nextFail.ToString());
		
		ArrayList subs = new ArrayList();
		for(int i=0; i<this.message.Length; i++)
		{
			Hashtable s = new Hashtable();
			s.Add(XMLHandler.NODE_NAME, "message");
			s.Add("id", i.ToString());
			s.Add(XMLHandler.CONTENT, this.message[i]);
			subs.Add(s);
		}
		Hashtable s2 = new Hashtable();
		s2.Add(XMLHandler.NODE_NAME, "choice");
		s2.Add("id", "0");
		s2.Add("next", this.choiceNext[0].ToString());
		ArrayList subs2 = new ArrayList();
		for(int j=0; j<((string[])this.choice[0]).Length; j++)
		{
			Hashtable ss = new Hashtable();
			ss.Add(XMLHandler.NODE_NAME, "choicetext");
			ss.Add("id", j.ToString());
			ss.Add(XMLHandler.CONTENT, ((string[])this.choice[0])[j]);
			subs2.Add(ss);
		}
		s2.Add(XMLHandler.NODES, subs2);
		subs.Add(s2);
		
		if(this.show4)
		{
			ht.Add("show4", this.show4.ToString());
			ht.Add("show5", this.show5.ToString());
			
			Hashtable s = new Hashtable();
			s.Add(XMLHandler.NODE_NAME, "vector2");
			s.Add("x", this.v2.x.ToString());
			s.Add("y", this.v2.y.ToString());
			subs.Add(s);
			
			s = new Hashtable();
			s.Add(XMLHandler.NODE_NAME, "scene");
			s.Add(XMLHandler.CONTENT, this.scene);
			subs.Add(s);
		}
		ht.Add("show6", this.show6.ToString());
		ht.Add("show7", this.show7.ToString());
		ht.Add(XMLHandler.NODES, subs);
		return ht;
	}
}