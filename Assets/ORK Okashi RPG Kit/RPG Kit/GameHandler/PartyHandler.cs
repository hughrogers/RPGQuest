
using System.Collections;
using UnityEngine;

public class PartyHandler
{
	private ArrayList battleParty = new ArrayList();
	private ArrayList lockedBattleParty = new ArrayList();
	private Character[] activeParty = new Character[0];
	private Character[] inactiveParty = new Character[0];
	
	private Character player;
	
	public PartyHandler()
	{
		
	}
	
	public void Tick()
	{
		for(int i=0; i<this.activeParty.Length; i++)
		{
			this.activeParty[i].Tick();
		}
	}
	
	public void DeleteBattleTextures()
	{
		DataHolder.BattleMenu().DeleteBattleTextures();
		for(int i=0; i<this.activeParty.Length; i++)
		{
			this.activeParty[i].battleMenu.DeleteBattleTextures();
		}
	}
	
	// party functions
	public bool HasJoinedParty(int cID)
	{
		bool has = false;
		for(int i=0; i<this.activeParty.Length; i++)
		{
			if(this.activeParty[i].realID == cID)
			{
				has = true;
				break;
			}
		}
		return has;
	}
	
	public bool HasLeftParty(int cID) 
	{
		bool has = false;
		for(int i=0; i<this.inactiveParty.Length; i++)
		{
			if(this.inactiveParty[i].realID == cID)
			{
				has = true;
				break;
			}
		}
		return has;
	}
	
	public Character GetPartyMember(int cID)
	{
		Character c = null;
		for(int i=0; i<this.activeParty.Length; i++)
		{
			if(this.activeParty[i].realID == cID)
			{
				c = this.activeParty[i];
				break;
			}
		}
		return c;
	}
	
	public Character GetInactiveMember(int cID)
	{
		Character c = null;
		for(int i=0; i<this.inactiveParty.Length; i++)
		{
			if(this.inactiveParty[i].realID == cID)
			{
				c = this.inactiveParty[i];
				break;
			}
		}
		return c;
	}
	
	public void JoinParty(int cID)
	{
		if(!this.HasJoinedParty(cID))
		{
			Character c = this.GetInactiveMember(cID);
			if(c == null)
			{
				c = DataHolder.Characters().GetCopy(cID);
				c.Init();
			}
			this.activeParty = ArrayHelper.Add(c, this.activeParty);
			
			this.JoinBattleParty(c.realID);
			if(this.player == null && "" != c.prefabName)
			{
				this.SetPlayer(c.realID);
			}
		}
	}
	
	public void LeaveParty(int cID)
	{
		Character c = this.GetPartyMember(cID);
		if(c != null)
		{
			this.LeaveBattleParty(cID);
			this.activeParty = ArrayHelper.Remove(c, this.activeParty);
			this.inactiveParty = ArrayHelper.Add(c, this.inactiveParty);
		}
	}
	
	public void RemoveFromParty(int cID)
	{
		Character c = this.GetPartyMember(cID);
		if(c != null)
		{
			this.LeaveBattleParty(cID);
			this.activeParty = ArrayHelper.Remove(c, this.activeParty);
		}
	}
	
	public int GetPartySize()
	{
		return this.activeParty.Length;
	}
	
	// battle party functions
	public void JoinBattleParty(int cID)
	{
		if(this.battleParty.Count < DataHolder.GameSettings().maxBattleParty &&
				!this.HasJoinedBattleParty(cID))
		{
			this.battleParty.Add(cID);
			if(DataHolder.GameSettings().spawnParty &&
				DataHolder.GameSettings().spawnOnlyBP && this.player != null)
			{
				this.SpawnParty(true);
			}
		}
	}
	
	public void LeaveBattleParty(int cID)
	{
		if(this.HasJoinedBattleParty(cID) && !this.IsLockedBattlePartyMember(cID))
		{
			this.battleParty.Remove(cID);
			if(DataHolder.GameSettings().spawnParty &&
				DataHolder.GameSettings().spawnOnlyBP)
			{
				this.DestroyInstance(cID);
			}
		}
	}
	
	public bool HasJoinedBattleParty(int cID)
	{
		bool has = false;
		for(int i=0; i<this.battleParty.Count; i++)
		{
			if((int)this.battleParty[i] == cID)
			{
				has = true;
				break;
			}
		}
		return has;
	}
	
	public void ChangeBattleParty(int oldID, int newID)
	{
		if(this.HasJoinedBattleParty(oldID) && !this.HasJoinedBattleParty(newID) &&
				!this.IsLockedBattlePartyMember(oldID))
		{
			this.battleParty[this.GetBattlePartyPosition(oldID)] = newID;
			if(DataHolder.GameSettings().spawnParty &&
				DataHolder.GameSettings().spawnOnlyBP)
			{
				if(this.activeParty[oldID].prefabInstance != null &&
					this.activeParty[newID].prefabInstance == null)
				{
					Vector3 pos = this.activeParty[oldID].prefabInstance.transform.position;
					Vector3 ea = this.activeParty[oldID].prefabInstance.transform.eulerAngles;
					this.activeParty[oldID].DestroyPrefab();
					
					this.activeParty[newID].CreatePrefabInstance();
					if(this.activeParty[newID].prefabInstance != null)
					{
						this.activeParty[newID].prefabInstance.transform.position = pos;
						this.activeParty[newID].prefabInstance.transform.eulerAngles = ea;
					}
				}
				else
				{
					this.DestroyInstance(oldID);
					this.SpawnParty(true);
				}
			}
		}
	}
	
	public int GetBattlePartyPosition(int cID)
	{
		int pos = -1;
		for(int i=0; i<this.battleParty.Count; i++)
		{
			if((int)this.battleParty[i] == cID)
			{
				pos = i;
				break;
			}
		}
		return pos;
	}
	
	public int GetBattlePartySize()
	{
		return this.battleParty.Count;
	}
	
	// lock battle party functions
	public void LockBattlePartyMember(int cID)
	{
		if(this.lockedBattleParty.Count < DataHolder.GameSettings().maxBattleParty &&
				this.HasJoinedBattleParty(cID) && !this.IsLockedBattlePartyMember(cID))
		{
			this.lockedBattleParty.Add(cID);
		}
	}
	
	public void UnlockBattlePartyMember(int cID)
	{
		if(this.IsLockedBattlePartyMember(cID))
		{
			this.lockedBattleParty.Remove(cID);
		}
	}
	
	public bool IsLockedBattlePartyMember(int cID)
	{
		bool has = false;
		for(int i=0; i<this.lockedBattleParty.Count; i++)
		{
			if((int)this.lockedBattleParty[i] == cID)
			{
				has = true;
				break;
			}
		}
		return has;
	}
	
	// get functions
	public Character GetCharacter(int cID)
	{
		Character c = this.GetPartyMember(cID);
		if(c == null)
		{
			c = this.GetInactiveMember(cID);
		}
		return c;
	}
	
	public Character GetCharacterOffset(int cID, int offset, bool onlyBP)
	{
		Character c = null;
		if(onlyBP)
		{
			int index = this.GetBattlePartyPosition(cID);
			index += offset;
			if(index < 0) index = this.battleParty.Count-1;
			else if(index >= this.battleParty.Count) index = 0;
			c = this.GetPartyMember((int)this.battleParty[index]);
		}
		else
		{
			int index = 0;
			for(int i=0; i<this.activeParty.Length; i++)
			{
				if(this.activeParty[i].realID == cID)
				{
					index = i+offset;
					break;
				}
			}
			if(index < 0) index = this.activeParty.Length-1;
			else if(index >= this.activeParty.Length) index = 0;
			c = this.activeParty[index];
		}
		return c;
	}
	
	public Character[] GetBattleParty()
	{
		Character[] cs = new Character[this.battleParty.Count];
		for(int i=0; i<this.battleParty.Count; i++)
		{
			cs[i] = this.GetPartyMember((int)this.battleParty[i]);
		}
		return cs;
	}
	
	public Character[] GetParty()
	{
		return this.activeParty;
	}
	
	public Character[] GetPartyWithoutBattle()
	{
		Character[] cs = new Character[this.activeParty.Length-this.battleParty.Count];
		int count = 0;
		for(int i=0; i<this.activeParty.Length; i++)
		{
			if(!this.HasJoinedBattleParty(this.activeParty[i].realID))
			{
				cs[count] = this.activeParty[i];
				count++;
			}
		}
		return cs;
	}
	
	public Character[] GetInactiveParty()
	{
		return this.inactiveParty;
	}
	
	// player functions
	public GameObject GetPlayer()
	{
		if(this.player != null) return this.player.prefabInstance;
		else return null;
	}
	
	public Transform GetPlayerTransform()
	{
		if(this.player != null && this.player.prefabInstance != null)
		{
			return this.player.prefabInstance.transform;
		}
		else return null;
	}
	
	public Character GetPlayerCharacter()
	{
		return this.player;
	}
	
	public bool IsPlayerCharacter(Combatant c)
	{
		return this.player == c;
	}
	
	public int GetPlayerID()
	{
		if(this.player != null) return this.player.realID;
		else return -1;
	}
	
	public void SetPlayer(int cID)
	{
		Character c = this.GetPartyMember(cID);
		if(c != null)
		{
			this.SetPlayer(c);
		}
	}
	
	public void SetPlayer(Character c)
	{
		GameHandler.GetLevelHandler().interactionList = new ArrayList();
		bool setPos = false;
		Vector3 pos = Vector3.zero;
		Vector3 ea = Vector3.zero;
		if(this.player != null && this.player.prefabInstance != null)
		{
			this.RemovePlayerComponents();
			if(c.prefabInstance == null)
			{
				pos = this.player.prefabInstance.transform.position;
				ea = this.player.prefabInstance.transform.eulerAngles;
				setPos = true;
				this.player.DestroyPrefab();
			}
		}
		this.player = c;
		if(this.player.prefabInstance == null)
		{
			this.player.CreatePrefabInstance();
			this.AddPlayerComponents();
			if(this.player.prefabInstance != null)
			{
				if(setPos)
				{
					this.player.prefabInstance.transform.position = pos;
					this.player.prefabInstance.transform.eulerAngles = ea;
				}
			}
		}
		else
		{
			this.AddPlayerComponents();
		}
		if(this.player.prefabInstance != null) GameObject.DontDestroyOnLoad(this.player.prefabInstance);
	}
	
	public void AddPlayerComponents()
	{
		if(this.player != null && this.player.prefabInstance != null)
		{
			DataHolder.GameSettings().playerControlSettings.AddPlayerControl(this.player.prefabInstance);
			for(int i=0; i<DataHolder.GameSettings().playerComponent.Length; i++)
			{
				Component comp = this.player.prefabInstance.GetComponent(
						DataHolder.GameSettings().playerComponent[i]);
				if(comp == null)
				{
					this.player.prefabInstance.AddComponent(
							DataHolder.GameSettings().playerComponent[i]);
				}
			}
		}
	}
	
	public void RemovePlayerComponents()
	{
		if(this.player != null && this.player.prefabInstance != null)
		{
			DataHolder.GameSettings().playerControlSettings.RemovePlayerControl(this.player.prefabInstance);
			for(int i=0; i<DataHolder.GameSettings().playerComponent.Length; i++)
			{
				Component comp = this.player.prefabInstance.GetComponent(
						DataHolder.GameSettings().playerComponent[i]);
				if(comp != null) GameObject.Destroy(comp);
			}
		}
	}
	
	public void SpawnPlayer(int spawnID)
	{
		SpawnPoint[] sp = (SpawnPoint[])UnityEngine.Object.FindObjectsOfType(typeof(SpawnPoint));
		for(int i=0; i<sp.Length; i++)
		{
			if(sp[i].ID == spawnID)
			{
				sp[i].SetOnGround();
				this.SpawnPlayer(sp[i].gameObject);
				break;
			}
		}
	}
	
	public void SpawnPlayer(GameObject spawnPoint)
	{
		if(this.player == null)
		{
			Debug.Log("Player object of GameHandler is null! Set a player before spawning it.");
		}
		else
		{
			if(this.player.prefabInstance == null) this.player.CreatePrefabInstance();
			if(this.player.prefabInstance != null)
			{
				this.AddPlayerComponents();
				this.player.prefabInstance.transform.position = spawnPoint.transform.position;
				Vector3 ea = this.player.prefabInstance.transform.eulerAngles;
				ea.y = spawnPoint.transform.eulerAngles.y;
				this.player.prefabInstance.transform.eulerAngles = ea;
				
				// spawning party
				if(DataHolder.GameSettings().spawnParty && 
					(!DataHolder.GameSettings().onlyInBattleArea ||
						(DataHolder.BattleSystem().IsRealTime() && GameHandler.IsInBattleArea())))
				{
					this.SpawnParty(false);
				}
			}
		}
	}
	
	public void RespawnParty()
	{
		Character[] party = new Character[0];
		if(DataHolder.GameSettings().spawnOnlyBP) party = this.GetBattleParty();
		else party = this.GetParty();
		for(int i=0; i<party.Length; i++)
		{
			if(!party[i].respawnFlag) party[i] = null;
		}
		this.SpawnParty(party);
	}
	
	public void SpawnParty(bool onlyNonCreated)
	{
		Character[] party = new Character[0];
		if(DataHolder.GameSettings().spawnOnlyBP) party = this.GetBattleParty();
		else party = this.GetParty();
		if(onlyNonCreated)
		{
			for(int i=0; i<party.Length; i++)
			{
				if(party[i].prefabInstance != null) party[i] = null;
			}
		}
		this.SpawnParty(party);
	}
	
	public void SpawnParty(Character[] party)
	{
		party = ArrayHelper.Remove(this.player, party);
		for(int i=0; i<party.Length; i++)
		{
			if(party[i] != this.player && party[i] != null)
			{
				if(party[i].prefabInstance == null) party[i].CreatePrefabInstance();
				if(party[i].prefabInstance != null)
				{
					party[i].respawnFlag = false;
					party[i].prefabInstance.transform.position = this.player.prefabInstance.transform.TransformPoint(
							0, 1, -DataHolder.GameSettings().spawnDistance);
					if(party.Length > 1)
					{
						if(party.Length <= 2)
						{
							party[i].prefabInstance.transform.RotateAround(
									this.player.prefabInstance.transform.position,
									Vector3.up, 45.0f-(90.0f*i));
						}
						else if(party.Length <= 6)
						{
							if(i%2 == 0)
							{
								party[i].prefabInstance.transform.RotateAround(
										this.player.prefabInstance.transform.position,
										Vector3.up, (180.0f/party.Length)*i);
							}
							else
							{
								party[i].prefabInstance.transform.RotateAround(
										this.player.prefabInstance.transform.position,
										Vector3.up, -(180.0f/party.Length)*i);
							}
						}
						else
						{
							if(i%2 == 0)
							{
								party[i].prefabInstance.transform.RotateAround(
										this.player.prefabInstance.transform.position,
										Vector3.up, (300.0f/(party.Length+1))*(i-1));
							}
							else
							{
								party[i].prefabInstance.transform.RotateAround(
										this.player.prefabInstance.transform.position,
										Vector3.up, -(300.0f/(party.Length+1))*i);
							}
						}
					}
					party[i].prefabInstance.transform.rotation = this.player.prefabInstance.transform.rotation;
				}
			}
		}
	}
	
	public void DestroySpawnedParty()
	{
		for(int i=0; i<this.activeParty.Length; i++)
		{
			if(this.activeParty[i] != this.player)
			{
				this.activeParty[i].DestroyPrefab();
			}
		}
	}
	
	public void DestroyPlayer()
	{
		if(this.player != null)
		{
			this.player.DestroyPrefab();
		}
	}
	
	public void DestroyInstances()
	{
		for(int i=0; i<this.activeParty.Length; i++)
		{
			this.activeParty[i].DestroyPrefab();
		}
		for(int i=0; i<this.inactiveParty.Length; i++)
		{
			this.inactiveParty[i].DestroyPrefab();
		}
	}
	
	public void DestroyInstance(int cID)
	{
		Character c = this.GetPartyMember(cID);
		if(c != null && c.prefabInstance != null)
		{
			c.DestroyPrefab();
		}
	}
	
	// save handling
	private static string BP = "bp";
	private static string LOCKEDBP = "lockedbp";
	public static string ACTIVEPARTY = "activeparty";
	private static string INACTIVEPARTY = "inactiveparty";
	public static string CHARACTER = "character";
	
	public Hashtable GetSaveData(Hashtable ht)
	{
		ArrayList s = new ArrayList();
		
		if(this.player != null) ht.Add("playerid", this.GetPlayerID().ToString());
		
		// active party
		ArrayList s2 = new ArrayList();
		Hashtable ht2 = HashtableHelper.GetTitleHashtable(PartyHandler.ACTIVEPARTY);
		ht2.Add("size", this.activeParty.Length.ToString());
		for(int i=0; i<this.activeParty.Length; i++)
		{
			s2.Add(this.activeParty[i].GetSaveData(
					HashtableHelper.GetTitleHashtable(PartyHandler.CHARACTER, i)));
		}
		if(s2.Count > 0) ht2.Add(XMLHandler.NODES, s2);
		s.Add(ht2);
		// inactive party
		s2 = new ArrayList();
		ht2 = HashtableHelper.GetTitleHashtable(PartyHandler.INACTIVEPARTY);
		ht2.Add("size", this.inactiveParty.Length.ToString());
		for(int i=0; i<this.inactiveParty.Length; i++)
		{
			s2.Add(this.inactiveParty[i].GetSaveData(
					HashtableHelper.GetTitleHashtable(PartyHandler.CHARACTER, i)));
		}
		if(s2.Count > 0) ht2.Add(XMLHandler.NODES, s2);
		s.Add(ht2);
		
		// battle party
		for(int i=0; i<this.battleParty.Count; i++)
		{
			s.Add(HashtableHelper.GetTitleHashtable(
					PartyHandler.BP, (int)this.battleParty[i]));
		}
		// locked battle party
		for(int i=0; i<this.lockedBattleParty.Count; i++)
		{
			s.Add(HashtableHelper.GetTitleHashtable(
					PartyHandler.LOCKEDBP, (int)this.lockedBattleParty[i]));
		}
		if(s.Count > 0) ht.Add(XMLHandler.NODES, s);
		return ht;
	}
	
	public void SetSaveData(Hashtable ht)
	{
		this.DestroyInstances();
		this.battleParty = new ArrayList();
		this.lockedBattleParty = new ArrayList();
		this.activeParty = new Character[0];
		this.inactiveParty = new Character[0];
		this.player = null;
		
		if(ht.ContainsKey(XMLHandler.NODES))
		{
			ArrayList s = ht[XMLHandler.NODES] as ArrayList;
			foreach(Hashtable ht2 in s)
			{
				if(ht2[XMLHandler.NODE_NAME] as string == PartyHandler.ACTIVEPARTY)
				{
					this.activeParty = new Character[int.Parse((string)ht2["size"])];
					if(ht2.ContainsKey(XMLHandler.NODES))
					{
						ArrayList s2 = ht2[XMLHandler.NODES] as ArrayList;
						foreach(Hashtable ht3 in s2)
						{
							if(ht3[XMLHandler.NODE_NAME] as string == PartyHandler.CHARACTER)
							{
								int i = int.Parse((string)ht3["id"]);
								this.activeParty[i] = DataHolder.Characters().GetCopy(int.Parse((string)ht3["realid"]));
								this.activeParty[i].SetSaveData(ht3);
							}
						}
					}
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == PartyHandler.INACTIVEPARTY)
				{
					this.inactiveParty = new Character[int.Parse((string)ht2["size"])];
					if(ht2.ContainsKey(XMLHandler.NODES))
					{
						ArrayList s2 = ht2[XMLHandler.NODES] as ArrayList;
						foreach(Hashtable ht3 in s2)
						{
							if(ht3[XMLHandler.NODE_NAME] as string == PartyHandler.CHARACTER)
							{
								int i = int.Parse((string)ht3["id"]);
								this.inactiveParty[i] = DataHolder.Characters().GetCopy(int.Parse((string)ht3["realid"]));
								this.inactiveParty[i].SetSaveData(ht3);
							}
						}
					}
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == PartyHandler.BP)
				{
					this.battleParty.Add(int.Parse((string)ht2["id"]));
				}
				else if(ht2[XMLHandler.NODE_NAME] as string == PartyHandler.LOCKEDBP)
				{
					this.lockedBattleParty.Add(int.Parse((string)ht2["id"]));
				}
			}
		}
		if(ht.ContainsKey("playerid")) this.SetPlayer(int.Parse((string)ht["playerid"]));
		this.AddPlayerComponents();
	}
}