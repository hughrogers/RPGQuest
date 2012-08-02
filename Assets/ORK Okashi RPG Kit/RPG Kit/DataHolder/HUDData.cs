
using System.Collections;
using UnityEngine;

public class HUDData : BaseData
{
	public HUD[] hud = new HUD[0];
	
	public string resourcePath = "HUD/";
	
	// XML data
	private string filename = "huds";
	
	private static string HUDS = "huds";
	private static string HUD = "hud";
	private static string SKIN = "skin";
	private static string CONTROLTYPE = "controltype";
	private static string ELEMENT = "element";
	private static string TEXT = "text";

	public HUDData()
	{
		LoadData();
	}
	
	public void LoadData()
	{
		ArrayList data = XMLHandler.LoadXML(dir+filename);
		
		if(data.Count > 0)
		{
			foreach(Hashtable entry in data)
			{
				if(entry[XMLHandler.NODE_NAME] as string == HUDData.HUDS)
				{
					if(entry.ContainsKey(XMLHandler.NODES))
					{
						ArrayList subs = entry[XMLHandler.NODES] as ArrayList;
						name = new string[subs.Count];
						hud = new HUD[subs.Count];
						foreach(Hashtable val in subs)
						{
							if(val[XMLHandler.NODE_NAME] as string == HUDData.HUD)
							{
								int i = int.Parse((string)val["id"]);
								hud[i] = new HUD();
								
								if(val.ContainsKey("oninteraction")) hud[i].onInteraction = bool.Parse((string)val["oninteraction"]);
								if(val.ContainsKey("onlyone")) hud[i].onlyOne = bool.Parse((string)val["onlyone"]);
								if(val.ContainsKey("controls"))
								{
									int count = int.Parse((string)val["controls"]);
									hud[i].controlType = new bool[count];
								}
								if(val.ContainsKey("elements"))
								{
									int count = int.Parse((string)val["elements"]);
									hud[i].element = new HUDElement[count];
								}
								if(val.ContainsKey("box")) hud[i].showBox = bool.Parse((string)val["box"]);
								if(val.ContainsKey("x"))
								{
									hud[i].bounds.x = float.Parse((string)val["x"]);
									hud[i].bounds.y = float.Parse((string)val["y"]);
									hud[i].bounds.width = float.Parse((string)val["width"]);
									hud[i].bounds.height = float.Parse((string)val["height"]);
								}
								if(val.ContainsKey("ox"))
								{
									hud[i].offset.x = float.Parse((string)val["ox"]);
									hud[i].offset.y = float.Parse((string)val["oy"]);
								}
								// fading
								if(val.ContainsKey("fadeintime"))
								{
									hud[i].fadeIn = true;
									hud[i].fadeInTime = float.Parse((string)val["fadeintime"]);
									hud[i].fadeInInterpolation = (EaseType)System.Enum.Parse(typeof(EaseType), (string)val["fadeininterpolation"]);
								}
								if(val.ContainsKey("fadeouttime"))
								{
									hud[i].fadeOut = true;
									hud[i].fadeOutTime = float.Parse((string)val["fadeouttime"]);
									hud[i].fadeOutInterpolation = (EaseType)System.Enum.Parse(typeof(EaseType), (string)val["fadeoutinterpolation"]);
								}
								// moving
								if(val.ContainsKey("moveintime"))
								{
									hud[i].moveIn = true;
									hud[i].moveInStart.x = float.Parse((string)val["mix"]);
									hud[i].moveInStart.y = float.Parse((string)val["miy"]);
									hud[i].moveInTime = float.Parse((string)val["moveintime"]);
									hud[i].moveInInterpolation = (EaseType)System.Enum.Parse(typeof(EaseType), (string)val["moveininterpolation"]);
								}
								if(val.ContainsKey("moveouttime"))
								{
									hud[i].moveOut = true;
									hud[i].moveOutStart.x = float.Parse((string)val["mox"]);
									hud[i].moveOutStart.y = float.Parse((string)val["moy"]);
									hud[i].moveOutTime = float.Parse((string)val["moveouttime"]);
									hud[i].moveOutInterpolation = (EaseType)System.Enum.Parse(typeof(EaseType), (string)val["moveoutinterpolation"]);
								}
								if(val.ContainsKey("hudclick")) hud[i].hudClick = (HUDClick)System.Enum.Parse(typeof(HUDClick), (string)val["hudclick"]);
								if(val.ContainsKey("screenindex")) hud[i].screenIndex = int.Parse((string)val["screenindex"]);
								
								ArrayList s = val[XMLHandler.NODES] as ArrayList;
								foreach(Hashtable ht in s)
								{
									if(ht[XMLHandler.NODE_NAME] as string == HUDData.NAME)
									{
										name[i] = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == HUDData.SKIN)
									{
										hud[i].skinName = ht[XMLHandler.CONTENT] as string;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == HUDData.CONTROLTYPE)
									{
										int j = int.Parse((string)ht["id"]);
										hud[i].controlType[j] = true;
									}
									else if(ht[XMLHandler.NODE_NAME] as string == HUDData.ELEMENT)
									{
										int j = int.Parse((string)ht["id"]);
										hud[i].element[j] = new HUDElement();
										if(ht.ContainsKey("box")) hud[i].element[j].showBox = bool.Parse((string)ht["box"]);
										if(ht.ContainsKey("x"))
										{
											hud[i].element[j].bounds.x = float.Parse((string)ht["x"]);
											hud[i].element[j].bounds.y = float.Parse((string)ht["y"]);
											hud[i].element[j].bounds.width = float.Parse((string)ht["width"]);
											hud[i].element[j].bounds.height = float.Parse((string)ht["height"]);
										}
										if(ht.ContainsKey("type")) hud[i].element[j].type = (HUDElementType)System.Enum.Parse(typeof(HUDElementType), (string)ht["type"]);
										if(ht.ContainsKey("anchor")) hud[i].element[j].textAnchor = (TextAnchor)System.Enum.Parse(typeof(TextAnchor), (string)ht["anchor"]);
										
										if(HUDElementType.TEXT.Equals(hud[i].element[j].type))
										{
											hud[i].element[j].SetTextOptions(ht);
											if(ht.ContainsKey("langs")) hud[i].element[j].text = new string[int.Parse((string)ht["langs"])];
											
											if(ht.ContainsKey(XMLHandler.NODES))
											{
												ArrayList s2 = ht[XMLHandler.NODES] as ArrayList;
												foreach(Hashtable ht2 in s2)
												{
													if(ht2[XMLHandler.NODE_NAME] as string == HUDData.TEXT)
													{
														if(ht2.ContainsKey(XMLHandler.CONTENT)) 
														{
															hud[i].element[j].text[int.Parse((string)ht2["id"])] = ht2[XMLHandler.CONTENT] as string;
														}
													}
												}
											}
										}
										else if(HUDElementType.IMAGE.Equals(hud[i].element[j].type))
										{
											hud[i].element[j].SetImageOptions(ht);
										}
										else if(HUDElementType.NAME.Equals(hud[i].element[j].type))
										{
											hud[i].element[j].SetTextOptions(ht);
											if(ht.ContainsKey("contenttype")) hud[i].element[j].contentType = 
													(HUDContentType)System.Enum.Parse(typeof(HUDContentType), (string)ht["contenttype"]);
											if(ht.ContainsKey("nametype")) hud[i].element[j].nameType = 
													(HUDNameType)System.Enum.Parse(typeof(HUDNameType), (string)ht["nametype"]);
											if(HUDNameType.STATUS.Equals(hud[i].element[j].nameType) && ht.ContainsKey("statusid"))
											{
												hud[i].element[j].statusID = int.Parse((string)ht["statusid"]);
											}
										}
										else if(HUDElementType.STATUS.Equals(hud[i].element[j].type))
										{
											if(ht.ContainsKey("statusid")) hud[i].element[j].statusID = int.Parse((string)ht["statusid"]);
											if(ht.ContainsKey("displaytype")) hud[i].element[j].displayType = 
													(HUDDisplayType)System.Enum.Parse(typeof(HUDDisplayType), (string)ht["displaytype"]);
											
											if(HUDDisplayType.TEXT.Equals(hud[i].element[j].displayType))
											{
												if(ht.ContainsKey("showmax")) hud[i].element[j].showMax = bool.Parse((string)ht["showmax"]);
												if(hud[i].element[j].showMax && ht.ContainsKey(XMLHandler.CONTENT))
												{
													hud[i].element[j].divider = ht[XMLHandler.CONTENT] as string;
												}
												hud[i].element[j].SetTextOptions(ht);
											}
											else if(HUDDisplayType.BAR.Equals(hud[i].element[j].displayType))
											{
												if(ht.ContainsKey("useimage")) hud[i].element[j].useImage = bool.Parse((string)ht["useimage"]);
												if(hud[i].element[j].useImage)
												{
													hud[i].element[j].SetImageOptions(ht);
												}
												else if(ht.ContainsKey("barcolor"))
												{
													hud[i].element[j].barColor = int.Parse((string)ht["barcolor"]);
												}
											}
										}
										else if(HUDElementType.TIMEBAR.Equals(hud[i].element[j].type) ||
											HUDElementType.USED_TIMEBAR.Equals(hud[i].element[j].type) ||
											HUDElementType.CASTTIME.Equals(hud[i].element[j].type))
										{
											if(ht.ContainsKey("displaytype")) hud[i].element[j].displayType = 
													(HUDDisplayType)System.Enum.Parse(typeof(HUDDisplayType), (string)ht["displaytype"]);
											if(HUDDisplayType.TEXT.Equals(hud[i].element[j].displayType))
											{
												hud[i].element[j].SetTextOptions(ht);
											}
											else if(HUDDisplayType.BAR.Equals(hud[i].element[j].displayType))
											{
												if(ht.ContainsKey("useimage")) hud[i].element[j].useImage = bool.Parse((string)ht["useimage"]);
												if(hud[i].element[j].useImage)
												{
													hud[i].element[j].SetImageOptions(ht);
												}
												else if(ht.ContainsKey("barcolor"))
												{
													hud[i].element[j].barColor = int.Parse((string)ht["barcolor"]);
												}
											}
										}
										else if(HUDElementType.EFFECT.Equals(hud[i].element[j].type))
										{
											if(ht.ContainsKey("rows")) hud[i].element[j].rows = int.Parse((string)ht["rows"]);
											if(ht.ContainsKey("columns")) hud[i].element[j].columns = int.Parse((string)ht["columns"]);
											if(ht.ContainsKey("spacing")) hud[i].element[j].spacing = float.Parse((string)ht["spacing"]);
											if(ht.ContainsKey("contenttype")) hud[i].element[j].contentType = 
													(HUDContentType)System.Enum.Parse(typeof(HUDContentType), (string)ht["contenttype"]);
											if(HUDContentType.TEXT.Equals(hud[i].element[j].contentType) ||
												HUDContentType.BOTH.Equals(hud[i].element[j].contentType))
											{
												hud[i].element[j].SetTextOptions(ht);
											}
										}
										else if(HUDElementType.VARIABLE.Equals(hud[i].element[j].type))
										{
											if(ht.ContainsKey("varname")) hud[i].element[j].variableName = (string)ht["varname"];
											if(ht.ContainsKey("numbervar"))  hud[i].element[j].numberVariable = bool.Parse((string)ht["numbervar"]);
											if(ht.ContainsKey("asint"))  hud[i].element[j].asInt = bool.Parse((string)ht["asint"]);
											hud[i].element[j].SetTextOptions(ht);
											
											if(ht.ContainsKey("langs")) hud[i].element[j].text = new string[int.Parse((string)ht["langs"])];
											
											if(ht.ContainsKey(XMLHandler.NODES))
											{
												ArrayList s2 = ht[XMLHandler.NODES] as ArrayList;
												foreach(Hashtable ht2 in s2)
												{
													if(ht2[XMLHandler.NODE_NAME] as string == HUDData.TEXT)
													{
														if(ht2.ContainsKey(XMLHandler.CONTENT)) 
														{
															hud[i].element[j].text[int.Parse((string)ht2["id"])] = ht2[XMLHandler.CONTENT] as string;
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
	
	public void SaveData()
	{
		ArrayList data = new ArrayList();
		ArrayList subs = new ArrayList();
		Hashtable sv = new Hashtable();
		
		sv.Add(XMLHandler.NODE_NAME, HUDData.HUDS);
		
		if(name != null)
		{
			for(int i=0; i<name.Length; i++)
			{
				Hashtable ht = new Hashtable();
				ArrayList s = new ArrayList();
				
				ht.Add(XMLHandler.NODE_NAME, HUDData.HUD);
				ht.Add("id", i.ToString());
				ht.Add("controls", hud[i].controlType.Length.ToString());
				ht.Add("elements", hud[i].element.Length.ToString());
				ht.Add("box", hud[i].showBox.ToString());
				ht.Add("x", hud[i].bounds.x.ToString());
				ht.Add("y", hud[i].bounds.y.ToString());
				ht.Add("width", hud[i].bounds.width.ToString());
				ht.Add("height", hud[i].bounds.height.ToString());
				ht.Add("ox", hud[i].offset.x.ToString());
				ht.Add("oy", hud[i].offset.y.ToString());
				ht.Add("oninteraction", hud[i].onInteraction.ToString());
				ht.Add("onlyone", hud[i].onlyOne.ToString());
				ht.Add("hudclick", hud[i].hudClick.ToString());
				ht.Add("screenindex", hud[i].screenIndex.ToString());
				
				if(this.hud[i].fadeIn)
				{
					ht.Add("fadeintime", this.hud[i].fadeInTime.ToString());
					ht.Add("fadeininterpolation", this.hud[i].fadeInInterpolation.ToString());
				}
				if(this.hud[i].fadeOut)
				{
					ht.Add("fadeouttime", this.hud[i].fadeOutTime.ToString());
					ht.Add("fadeoutinterpolation", this.hud[i].fadeOutInterpolation.ToString());
				}
				if(this.hud[i].moveIn)
				{
					ht.Add("mix", this.hud[i].moveInStart.x.ToString());
					ht.Add("miy", this.hud[i].moveInStart.y.ToString());
					ht.Add("moveintime", this.hud[i].moveInTime.ToString());
					ht.Add("moveininterpolation", this.hud[i].moveInInterpolation.ToString());
				}
				if(this.hud[i].moveOut)
				{
					ht.Add("mox", this.hud[i].moveOutStart.x.ToString());
					ht.Add("moy", this.hud[i].moveOutStart.y.ToString());
					ht.Add("moveouttime", this.hud[i].moveOutTime.ToString());
					ht.Add("moveoutinterpolation", this.hud[i].moveOutInterpolation.ToString());
				}
				
				if(hud[i].skinName != null && "" != hud[i].skinName)
				{
					Hashtable ht2 = new Hashtable();
					ht2.Add(XMLHandler.NODE_NAME, HUDData.SKIN);
					ht2.Add(XMLHandler.CONTENT, hud[i].skinName);
					s.Add(ht2);
				}
				
				Hashtable n = new Hashtable();
				n.Add(XMLHandler.NODE_NAME, HUDData.NAME);
				n.Add(XMLHandler.CONTENT, name[i]);
				s.Add(n);
				
				for(int j=0; j<hud[i].controlType.Length; j++)
				{
					if(hud[i].controlType[j])
					{
						Hashtable ht2 = new Hashtable();
						ht2.Add(XMLHandler.NODE_NAME, HUDData.CONTROLTYPE);
						ht2.Add("id", j.ToString());
						s.Add(ht2);
					}
				}
				
				for(int j=0; j<hud[i].element.Length; j++)
				{
					Hashtable ht2 = new Hashtable();
					ht2.Add(XMLHandler.NODE_NAME, HUDData.ELEMENT);
					ht2.Add("id", j.ToString());
					ht2.Add("box", hud[i].element[j].showBox.ToString());
					ht2.Add("x", hud[i].element[j].bounds.x.ToString());
					ht2.Add("y", hud[i].element[j].bounds.y.ToString());
					ht2.Add("width", hud[i].element[j].bounds.width.ToString());
					ht2.Add("height", hud[i].element[j].bounds.height.ToString());
					ht2.Add("anchor", hud[i].element[j].textAnchor.ToString());
					ht2.Add("type", hud[i].element[j].type.ToString());
					
					if(HUDElementType.TEXT.Equals(hud[i].element[j].type))
					{
						ht2 = hud[i].element[j].GetTextOptions(ht2);
						ht2.Add("langs", hud[i].element[j].text.Length.ToString());
						ArrayList s2 = new ArrayList();
						for(int k=0; k<hud[i].element[j].text.Length; k++)
						{
							if("" != hud[i].element[j].text[k])
							{
								Hashtable ht3 = new Hashtable();
								ht3.Add(XMLHandler.NODE_NAME, HUDData.TEXT);
								ht3.Add("id", k.ToString());
								ht3.Add(XMLHandler.CONTENT, hud[i].element[j].text[k]);
								s2.Add(ht3);
							}
						}
						if(s2.Count > 0) ht2.Add(XMLHandler.NODES, s2);
					}
					else if(HUDElementType.IMAGE.Equals(hud[i].element[j].type))
					{
						ht2 = hud[i].element[j].GetImageOptions(ht2);
					}
					else if(HUDElementType.NAME.Equals(hud[i].element[j].type))
					{
						ht2 = hud[i].element[j].GetTextOptions(ht2);
						ht2.Add("contenttype", hud[i].element[j].contentType.ToString());
						ht2.Add("nametype", hud[i].element[j].nameType.ToString());
						if(HUDNameType.STATUS.Equals(hud[i].element[j].nameType))
						{
							ht2.Add("statusid", hud[i].element[j].statusID.ToString());
						}
					}
					else if(HUDElementType.STATUS.Equals(hud[i].element[j].type))
					{
						ht2.Add("statusid", hud[i].element[j].statusID.ToString());
						ht2.Add("displaytype", hud[i].element[j].displayType.ToString());
						if(HUDDisplayType.TEXT.Equals(hud[i].element[j].displayType))
						{
							ht2.Add("showmax", hud[i].element[j].showMax.ToString());
							if(hud[i].element[j].showMax && "" != hud[i].element[j].divider)
							{
								ht2.Add(XMLHandler.CONTENT, hud[i].element[j].divider);
							}
							ht2 = hud[i].element[j].GetTextOptions(ht2);
						}
						else if(HUDDisplayType.BAR.Equals(hud[i].element[j].displayType))
						{
							ht2.Add("useimage", hud[i].element[j].useImage.ToString());
							if(hud[i].element[j].useImage)
							{
								ht2 = hud[i].element[j].GetImageOptions(ht2);
							}
							else
							{
								ht2.Add("barcolor", hud[i].element[j].barColor.ToString());
							}
						}
					}
					else if(HUDElementType.TIMEBAR.Equals(hud[i].element[j].type) ||
						HUDElementType.USED_TIMEBAR.Equals(hud[i].element[j].type) ||
						HUDElementType.CASTTIME.Equals(hud[i].element[j].type))
					{
						ht2.Add("displaytype", hud[i].element[j].displayType.ToString());
						if(HUDDisplayType.TEXT.Equals(hud[i].element[j].displayType))
						{
							ht2 = hud[i].element[j].GetTextOptions(ht2);
						}
						else if(HUDDisplayType.BAR.Equals(hud[i].element[j].displayType))
						{
							ht2.Add("useimage", hud[i].element[j].useImage.ToString());
							if(hud[i].element[j].useImage)
							{
								ht2 = hud[i].element[j].GetImageOptions(ht2);
							}
							else
							{
								ht2.Add("barcolor", hud[i].element[j].barColor.ToString());
							}
						}
					}
					else if(HUDElementType.EFFECT.Equals(hud[i].element[j].type))
					{
						ht2.Add("rows", hud[i].element[j].rows.ToString());
						ht2.Add("columns", hud[i].element[j].columns.ToString());
						ht2.Add("spacing", hud[i].element[j].spacing.ToString());
						ht2.Add("contenttype", hud[i].element[j].contentType.ToString());
						if(HUDContentType.TEXT.Equals(hud[i].element[j].contentType) ||
							HUDContentType.BOTH.Equals(hud[i].element[j].contentType))
						{
							ht2 = hud[i].element[j].GetTextOptions(ht2);
						}
					}
					else if(HUDElementType.VARIABLE.Equals(hud[i].element[j].type))
					{
						ht2.Add("varname", hud[i].element[j].variableName.ToString());
						ht2.Add("numbervar", hud[i].element[j].numberVariable.ToString());
						ht2.Add("asint", hud[i].element[j].asInt.ToString());
						ht2 = hud[i].element[j].GetTextOptions(ht2);
						ht2.Add("langs", hud[i].element[j].text.Length.ToString());
						ArrayList s2 = new ArrayList();
						for(int k=0; k<hud[i].element[j].text.Length; k++)
						{
							if("" != hud[i].element[j].text[k])
							{
								Hashtable ht3 = new Hashtable();
								ht3.Add(XMLHandler.NODE_NAME, HUDData.TEXT);
								ht3.Add("id", k.ToString());
								ht3.Add(XMLHandler.CONTENT, hud[i].element[j].text[k]);
								s2.Add(ht3);
							}
						}
						if(s2.Count > 0) ht2.Add(XMLHandler.NODES, s2);
					}
					s.Add(ht2);
				}
				
				ht.Add(XMLHandler.NODES, s);
				subs.Add(ht);
			}
			sv.Add(XMLHandler.NODES, subs);
		}
		
		data.Add(sv);
		XMLHandler.SaveXML(dir, filename, data);
	}
	
	public void AddHUD(string n)
	{
		if(name == null)
		{
			name = new string[] {n};
			hud = new HUD[] {new HUD()};
		}
		else
		{
			name = ArrayHelper.Add(n, name);
			hud = ArrayHelper.Add(new HUD(), hud);
		}
	}
	
	public override void RemoveData(int index)
	{
		name = ArrayHelper.Remove(index, name);
		if(name.Length == 0) name = null;
		hud = ArrayHelper.Remove(index, hud);
	}
	
	public HUD GetCopy(int index)
	{
		HUD h = new HUD();
		h.realID = index;
		h.onInteraction = hud[index].onInteraction;
		h.skinName = hud[index].skinName;
		h.skin = hud[index].skin;
		h.hudClick = hud[index].hudClick;
		h.screenIndex = hud[index].screenIndex;
		h.showBox = hud[index].showBox;
		h.onlyOne = hud[index].onlyOne;
		h.bounds = new Rect(hud[index].bounds.x, hud[index].bounds.y, 
				hud[index].bounds.width, hud[index].bounds.height);
		h.offset = new Vector2(hud[index].offset.x, hud[index].offset.y);
		h.fadeIn = hud[index].fadeIn;
		h.fadeInTime = hud[index].fadeInTime;
		h.fadeInInterpolation = hud[index].fadeInInterpolation;
		h.fadeOut = hud[index].fadeOut;
		h.fadeOutTime = hud[index].fadeOutTime;
		h.fadeOutInterpolation = hud[index].fadeOutInterpolation;
		h.moveIn = hud[index].moveIn;
		h.moveInTime = hud[index].moveInTime;
		h.moveInInterpolation = hud[index].moveInInterpolation;
		h.moveInStart = new Vector2(hud[index].moveInStart.x, hud[index].moveInStart.y);
		h.moveOut = hud[index].moveOut;
		h.moveOutTime = hud[index].moveOutTime;
		h.moveOutInterpolation = hud[index].moveOutInterpolation;
		h.moveOutStart = new Vector2(hud[index].moveOutStart.x, hud[index].moveOutStart.y);
		
		h.controlType = new bool[hud[index].controlType.Length];
		for(int i=0; i<h.controlType.Length; i++)
		{
			h.controlType[i] = hud[index].controlType[i];
		}
		h.element = new HUDElement[hud[index].element.Length];
		for(int i=0; i<h.element.Length; i++)
		{
			h.element[i] = new HUDElement();
			h.element[i].showBox = hud[index].element[i].showBox;
			h.element[i].bounds = new Rect(hud[index].element[i].bounds.x, hud[index].element[i].bounds.y, 
					hud[index].element[i].bounds.width, hud[index].element[i].bounds.height);
			h.element[i].textAnchor = hud[index].element[i].textAnchor;
			h.element[i].type = hud[index].element[i].type;
			h.element[i].displayType = hud[index].element[i].displayType;
			h.element[i].nameType = hud[index].element[i].nameType;
			h.element[i].showShadow = hud[index].element[i].showShadow;
			h.element[i].textColor = hud[index].element[i].textColor;
			h.element[i].shadowColor = hud[index].element[i].shadowColor;
			h.element[i].shadowOffset = new Vector2(hud[index].element[i].shadowOffset.x, 
					hud[index].element[i].shadowOffset.y);
			h.element[i].divider = hud[index].element[i].divider;
			h.element[i].imageName = hud[index].element[i].imageName;
			h.element[i].texture = hud[index].element[i].texture;
			h.element[i].scaleMode = hud[index].element[i].scaleMode;
			h.element[i].alphaBlend = hud[index].element[i].alphaBlend;
			h.element[i].imageAspect = hud[index].element[i].imageAspect;
			h.element[i].statusID = hud[index].element[i].statusID;
			h.element[i].showMax = hud[index].element[i].showMax;
			h.element[i].useImage = hud[index].element[i].useImage;
			h.element[i].barColor = hud[index].element[i].barColor;
			h.element[i].rows = hud[index].element[i].rows;
			h.element[i].columns = hud[index].element[i].columns;
			h.element[i].spacing = hud[index].element[i].spacing;
			h.element[i].contentType = hud[index].element[i].contentType;
			h.element[i].variableName = hud[index].element[i].variableName;
			h.element[i].numberVariable = hud[index].element[i].numberVariable;
			h.element[i].asInt = hud[index].element[i].asInt;
			h.element[i].text = new string[hud[index].element[i].text.Length];
			for(int j=0; j<h.element[i].text.Length; j++)
			{
				h.element[i].text[j] = hud[index].element[i].text[j];
			}
		}
		return h;
	}
	
	public override void Copy(int index)
	{
		name = ArrayHelper.Add(name[index], name);
		hud = ArrayHelper.Add(this.GetCopy(index), hud);
	}
}