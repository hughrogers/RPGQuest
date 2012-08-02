
using UnityEngine;

public class MultiContent
{
	public LabelContent[] label;
	
	public int textPos = 0;
	public float xPos = 0;
	public float yPos = 0;
	public int currentColor = 0;
	public int shadowColor = -1;
	
	public string originalText = "";
	
	public GUIFont font;
	public int addNext = 0;
	
	public MultiContent(string text, GUIStyle textStyle, GUIStyle shadowStyle, TextPosition textPosition, bool scrollable)
	{
		this.originalText = text;
		this.label = new LabelContent[0];
		
		this.currentColor = 0;
		this.shadowColor = -1;
		int oldColor = 0;
		int oldShadowColor = -1;
		
		this.textPos = 0;
		int i = 0;
		int nl = 0;
		int fs = 0;
		int skip = 0;
		bool lineBreak = false;
		this.xPos = 0;
		this.yPos = 0;
		
		bool setNextX = false;
		bool setNextY = false;
		float nextX = 0;
		float nextY = 0;
		
		string addstring = "";
		bool addSpace = false;
		bool finished = false;
		
		Vector2 v = Vector2.zero;
		Color tCol;
		Color sCol;
		
		Texture2D icon = null;
		GUIContent part = null;
		
		while(!finished)
		{
			skip = 0;
			lineBreak = false;
			oldColor = this.currentColor;
			oldShadowColor = this.shadowColor;
			icon = null;
			part = null;
			
			tCol = DataHolder.Color(this.currentColor);
			if(this.shadowColor == -1 || this.shadowColor >= DataHolder.Colors().GetDataCount())
			{
				sCol = DataHolder.Color(1);
			}
			else
			{
				sCol = DataHolder.Color(this.shadowColor);
			}
			
			fs = text.IndexOf("#", this.textPos);
			nl = text.IndexOf("\n", this.textPos);
			
			if(fs != -1 && (nl == -1 || fs < nl))
			{
				i = fs;
			}
			else
			{
				i = nl;
			}
			
			if(i == -1)
			{
				i = text.Length - 1;
				finished = true;
			}
			
			if(setNextX)
			{
				this.xPos = nextX;
				setNextX = false;
			}
			if(setNextY)
			{
				this.yPos = nextY;
				setNextY = false;
			}
			
			if(!finished)
			{
				if (text[i].ToString() == "#")
				{
					if((text.Length - i) >= 1)
					{
						string nextChar = text[i+1].ToString();
						
						if(nextChar == "c" || nextChar == "s")
						{
							// set text color for the next part
							int nS = text.IndexOf("#", i+1);
							if(nS != -1)
							{
								string sub = text.Substring(i, nS-i+1);
								skip = sub.Length+1;
								
								try
								{
									if(nextChar == "c")
									{
										this.currentColor = int.Parse(sub.Substring(2, sub.Length-3));
									}
									else if(nextChar == "s")
									{
										this.shadowColor = int.Parse(sub.Substring(2, sub.Length-3));
									}
								}
								catch(System.Exception e)
								{
									Debug.Log(e.Message);
									if(nextChar == "c")
									{
										this.currentColor = 0;
									}
									else if(nextChar == "s")
									{
										this.shadowColor = -1;
									}
								}
							}
						}
						else if(nextChar == "!")
						{
							this.currentColor = 0;
							this.shadowColor = -1;
							skip = 3;
							if((text.Length - (i+skip-1)) >= 1 && (text.Substring(i+skip-1, 1) == "\n" || text.Substring(i+skip-1, 1) == "\r"))
							{
								lineBreak = true;
								skip += 1;
							}
						}
						else if(nextChar == "#")
						{
							i++;
							skip = 2;
						}
						else if(nextChar == "p")
						{
							nextChar = text[i+2].ToString();
							
							// set x or y position for the next text part
							int nS = text.IndexOf("#", i+2);
							if(nS != -1)
							{
								string sub = text.Substring(i, nS-i+1);
								skip = sub.Length+1;
								
								try
								{
									if(nextChar == "x")
									{
										nextX = float.Parse(sub.Substring(3, sub.Length-4));
										setNextX = true;
									}
									else if(nextChar == "y")
									{
										nextY = float.Parse(sub.Substring(3, sub.Length-4));
										setNextY = true;
									}
								}
								catch(System.Exception e)
								{
									Debug.Log(e.Message);
								}
							}
						}
						else if(nextChar == "i" && text[i+2].ToString() == "q") // icons
						{
							int nS = text.IndexOf("#", i+4);
							if(nS != -1)
							{
								string sub = text.Substring(i, nS-i+1);
								skip = sub.Length+1;
								sub = text.Substring(i+5, nS-(i+5));
								int index = int.Parse(sub);
								sub = text.Substring(i+3, 2);
								if(sub == "sv") icon = DataHolder.StatusValues().GetIcon(index);
								else if(sub == "se") icon = DataHolder.Effects().GetIcon(index);
								else if(sub == "el") icon = DataHolder.Elements().GetIcon(index);
								else if(sub == "st") icon = DataHolder.SkillTypes().GetIcon(index);
								else if(sub == "sk") icon = DataHolder.Skills().GetIcon(index);
								else if(sub == "it") icon = DataHolder.ItemTypes().GetIcon(index);
								else if(sub == "im") icon = DataHolder.Items().GetIcon(index);
								else if(sub == "ir") icon = DataHolder.ItemRecipes().GetIcon(index);
								else if(sub == "ep") icon = DataHolder.EquipmentParts().GetIcon(index);
								else if(sub == "wp") icon = DataHolder.Weapons().GetIcon(index);
								else if(sub == "am") icon = DataHolder.Armors().GetIcon(index);
								else if(sub == "cl") icon = DataHolder.Classes().GetIcon(index);
								else if(sub == "ch") icon = DataHolder.Characters().GetIcon(index);
								else if(sub == "en") icon = DataHolder.Enemies().GetIcon(index);
								else icon = null;
							}
						}
						else
						{
							skip = 2;
						}
					}
					else
					{
						skip = 1;
					}
				}
				else if((text.Length - i) >= 1 && (text.Substring(i, 1) == "\n" || text.Substring(i, 1) == "\r"))
				{
					lineBreak = true;
					skip = 2;
				}
				i--;
			}
			
			if((i-this.textPos+1) > 0)
			{
				if(addSpace) addstring = " "; else addstring = "";
				part = new GUIContent(addstring + text.Substring(this.textPos, i - this.textPos + 1));
				v = textStyle.CalcSize(part);
				
				if ((this.xPos+v.x) > textPosition.bounds.width)
				{
					lineBreak = true;
					icon = null;
					skip = 0;
					int tmp = i;
					while (this.xPos+v.x > textPosition.bounds.width)
					{
						// calculate how much letters to substract: overlength width / letter width
						//i -= (int)((this.xPos + v.x - textPosition.bounds.width) / (v.x / part.text.Length));
						i--;
						if((i-this.textPos+1) > 0)
						{
							part = new GUIContent(addstring + text.Substring(this.textPos, i - this.textPos + 1));
						}
						else
						{
							part = new GUIContent("");
						}
						v = textStyle.CalcSize(part);
						tmp = i+1;
					}
					
					while(" " != text[i].ToString() && i>(this.textPos+1))
					{
						i--;
					}
					if((i-this.textPos+1) > 0 && " " == text[i].ToString())
					{
						part = new GUIContent(addstring + text.Substring(this.textPos, i - this.textPos + 1));
					}
					else
					{
						i = tmp;
					}
					v = textStyle.CalcSize(part);
					this.currentColor = oldColor;
					this.shadowColor = oldShadowColor;
					finished = false;
				}
				this.label = ArrayHelper.Add(new LabelContent(part, new Rect(this.xPos, this.yPos, v.x, v.y), tCol, sCol), this.label);
				this.xPos += v.x;
				
				if(!lineBreak)
				{
					addSpace = part.text[part.text.Length-1].ToString() == " ";
				}
			}
			if(icon != null)
			{
				part = new GUIContent(icon);
				v = textStyle.CalcSize(part);
				this.xPos += textStyle.CalcSize(new GUIContent(".")).x;
				if(this.xPos+v.x <= textPosition.bounds.width)
				{
					this.label = ArrayHelper.Add(new LabelContent(part, new Rect(this.xPos, this.yPos, v.x, v.y), tCol, sCol), this.label);
					this.xPos += v.x;
					addSpace = true;
					icon = null;
				}
				else
				{
					lineBreak = true;
				}
			}
			if(lineBreak)
			{
				this.xPos = 0;
				this.yPos += v.y + textPosition.lineSpacing;
				addSpace = false;
				
				if(i+skip < text.Length && " " == text[i+skip].ToString())
				{
					i++;
				}
				if((this.yPos+v.y) > (textPosition.bounds.height) && !scrollable)
				{
					finished = true;
				}
				else if(icon != null)
				{
					part = new GUIContent(icon);
					v = textStyle.CalcSize(part);
					this.xPos += textStyle.CalcSize(new GUIContent(".")).x;
					this.label = ArrayHelper.Add(new LabelContent(part, new Rect(this.xPos, this.yPos, v.x, v.y), tCol, sCol), this.label);
					this.xPos += v.x;
					addSpace = true;
					icon = null;
				}
			}
			else if(finished)
			{
				this.xPos = 0;
				this.yPos += v.y + textPosition.lineSpacing;
			}
			
			this.textPos = i + skip;
		}
	}
	
	public MultiContent(string text, DialoguePosition dp)
	{
		this.font = DataHolder.Fonts().GetFont(dp.skin.font);
		
		GUIStyle shadowStyle = new GUIStyle(dp.skin.label);
		shadowStyle.normal.textColor = DataHolder.Color(1);
		GUIStyle textStyle = new GUIStyle(dp.skin.label);
		textStyle.wordWrap = false;
		TextPosition textPosition = new TextPosition(dp.boxBounds, dp.boxPadding, dp.lineSpacing);
		textPosition.bounds.width -= (dp.boxPadding.x + dp.boxPadding.z);
		textPosition.bounds.height -= (dp.boxPadding.y + dp.boxPadding.w);
		
		this.originalText = text;
		this.label = new LabelContent[0];
		
		this.currentColor = 0;
		this.shadowColor = -1;
		int oldColor = 0;
		int oldShadowColor = -1;
		
		this.textPos = 0;
		int i = 0;
		int nl = 0;
		int fs = 0;
		int skip = 0;
		bool lineBreak = false;
		this.xPos = 0;
		this.yPos = 0;
		
		bool setNextX = false;
		bool setNextY = false;
		float nextX = 0;
		float nextY = 0;
		
		string addstring = "";
		bool addSpace = false;
		bool finished = false;
		
		Vector2 v = Vector2.zero;
		Color tCol;
		Color sCol;
		
		Texture2D icon = null;
		GUIContent part = null;
		LabelContent nextLabel;
		
		while(!finished)
		{
			skip = 0;
			lineBreak = false;
			oldColor = this.currentColor;
			oldShadowColor = this.shadowColor;
			icon = null;
			part = null;
			
			tCol = DataHolder.Color(this.currentColor);
			if(this.shadowColor == -1 || this.shadowColor >= DataHolder.Colors().GetDataCount())
			{
				sCol = DataHolder.Color(1);
			}
			else
			{
				sCol = DataHolder.Color(this.shadowColor);
			}
			
			fs = text.IndexOf("#", this.textPos);
			nl = text.IndexOf("\n", this.textPos);
			
			if(fs != -1 && (nl == -1 || fs < nl))
			{
				i = fs;
			}
			else
			{
				i = nl;
			}
			
			if(i == -1)
			{
				i = text.Length - 1;
				finished = true;
			}
			
			if(setNextX)
			{
				this.xPos = nextX;
				setNextX = false;
			}
			if(setNextY)
			{
				this.yPos = nextY;
				setNextY = false;
			}
			
			if(!finished)
			{
				if (text[i].ToString() == "#")
				{
					if((text.Length - i) >= 1)
					{
						string nextChar = text[i+1].ToString();
						
						if(nextChar == "c" || nextChar == "s")
						{
							// set text color for the next part
							int nS = text.IndexOf("#", i+1);
							if(nS != -1)
							{
								string sub = text.Substring(i, nS-i+1);
								skip = sub.Length+1;
								
								try
								{
									if(nextChar == "c")
									{
										this.currentColor = int.Parse(sub.Substring(2, sub.Length-3));
									}
									else if(nextChar == "s")
									{
										this.shadowColor = int.Parse(sub.Substring(2, sub.Length-3));
									}
								}
								catch(System.Exception e)
								{
									Debug.Log(e.Message);
									if(nextChar == "c")
									{
										this.currentColor = 0;
									}
									else if(nextChar == "s")
									{
										this.shadowColor = -1;
									}
								}
							}
						}
						else if(nextChar == "!")
						{
							this.currentColor = 0;
							this.shadowColor = -1;
							skip = 3;
							if((text.Length - (i+skip-1)) >= 1 && (text.Substring(i+skip-1, 1) == "\n" || text.Substring(i+skip-1, 1) == "\r"))
							{
								lineBreak = true;
								skip += 1;
							}
						}
						else if(nextChar == "#")
						{
							i++;
							skip = 2;
						}
						else if(nextChar == "p")
						{
							nextChar = text[i+2].ToString();
							
							// set x or y position for the next text part
							int nS = text.IndexOf("#", i+2);
							if(nS != -1)
							{
								string sub = text.Substring(i, nS-i+1);
								skip = sub.Length+1;
								
								try
								{
									if(nextChar == "x")
									{
										nextX = float.Parse(sub.Substring(3, sub.Length-4));
										setNextX = true;
									}
									else if(nextChar == "y")
									{
										nextY = float.Parse(sub.Substring(3, sub.Length-4));
										setNextY = true;
									}
								}
								catch(System.Exception e)
								{
									Debug.Log(e.Message);
								}
							}
						}
						else if(nextChar == "i" && text[i+2].ToString() == "q") // icons
						{
							int nS = text.IndexOf("#", i+4);
							if(nS != -1)
							{
								string sub = text.Substring(i, nS-i+1);
								skip = sub.Length+1;
								sub = text.Substring(i+5, nS-(i+5));
								int index = int.Parse(sub);
								sub = text.Substring(i+3, 2);
								if(sub == "sv") icon = DataHolder.StatusValues().GetIcon(index);
								else if(sub == "se") icon = DataHolder.Effects().GetIcon(index);
								else if(sub == "el") icon = DataHolder.Elements().GetIcon(index);
								else if(sub == "st") icon = DataHolder.SkillTypes().GetIcon(index);
								else if(sub == "sk") icon = DataHolder.Skills().GetIcon(index);
								else if(sub == "it") icon = DataHolder.ItemTypes().GetIcon(index);
								else if(sub == "im") icon = DataHolder.Items().GetIcon(index);
								else if(sub == "ir") icon = DataHolder.ItemRecipes().GetIcon(index);
								else if(sub == "ep") icon = DataHolder.EquipmentParts().GetIcon(index);
								else if(sub == "wp") icon = DataHolder.Weapons().GetIcon(index);
								else if(sub == "am") icon = DataHolder.Armors().GetIcon(index);
								else if(sub == "cl") icon = DataHolder.Classes().GetIcon(index);
								else if(sub == "ch") icon = DataHolder.Characters().GetIcon(index);
								else if(sub == "en") icon = DataHolder.Enemies().GetIcon(index);
								else icon = null;
							}
						}
						else
						{
							skip = 2;
						}
					}
					else
					{
						skip = 1;
					}
				}
				else if((text.Length - i) >= 1 && (text.Substring(i, 1) == "\n" || text.Substring(i, 1) == "\r"))
				{
					lineBreak = true;
					skip = 2;
				}
				i--;
			}
			
			if((i-this.textPos+1) > 0)
			{
				if(addSpace) addstring = " "; else addstring = "";
				part = new GUIContent(addstring + text.Substring(this.textPos, i - this.textPos + 1));
				
				nextLabel = new LabelContent(part, this.xPos, this.yPos, tCol, sCol, font);
				v = new Vector2(nextLabel.bounds.width, nextLabel.bounds.height);
				
				if ((this.xPos+v.x) > textPosition.bounds.width)
				{
					i = this.textPos+font.GetFittingTextLength(part.text, textPosition.bounds.width-this.xPos);
					lineBreak = true;
					icon = null;
					skip = 0;
					int tmp = i;
					
					part = new GUIContent(addstring + text.Substring(this.textPos, i - this.textPos + 1));
					v = font.GetTextSize(part.text);
					
					while (this.xPos+v.x > textPosition.bounds.width)
					{
						i--;
						if((i-this.textPos+1) > 0)
						{
							part = new GUIContent(addstring + text.Substring(this.textPos, i - this.textPos + 1));
						}
						else
						{
							part = new GUIContent("");
						}
						v = font.GetTextSize(part.text);
						tmp = i+1;
					}
					
					while(" " != text[i].ToString() && i>(this.textPos+1))
					{
						i--;
					}
					if((i-this.textPos+1) > 0 && " " == text[i].ToString())
					{
						part = new GUIContent(addstring + text.Substring(this.textPos, i - this.textPos + 1));
					}
					else
					{
						i = tmp;
					}
					nextLabel = new LabelContent(part, this.xPos, this.yPos, tCol, sCol, font);
					v = new Vector2(nextLabel.bounds.width, nextLabel.bounds.height);
					this.currentColor = oldColor;
					this.shadowColor = oldShadowColor;
					finished = false;
				}
				this.label = ArrayHelper.Add(nextLabel, this.label);
				this.xPos += v.x;
				
				if(!lineBreak)
				{
					addSpace = part.text[part.text.Length-1].ToString() == " ";
				}
			}
			if(icon != null)
			{
				part = new GUIContent(icon);
				nextLabel = new LabelContent(part, this.xPos, this.yPos, tCol, sCol, font);
				v = new Vector2(nextLabel.bounds.width, nextLabel.bounds.height);
				//this.xPos += font.GetTextSize(".").x;
				if(this.xPos+v.x <= textPosition.bounds.width)
				{
					this.label = ArrayHelper.Add(nextLabel, this.label);
					this.xPos += v.x;
					addSpace = true;
					icon = null;
				}
				else
				{
					lineBreak = true;
				}
			}
			if(lineBreak)
			{
				this.xPos = 0;
				this.yPos += v.y + textPosition.lineSpacing;
				addSpace = false;
				
				if(i+skip < text.Length && " " == text[i+skip].ToString())
				{
					i++;
				}
				if((this.yPos+v.y) > (textPosition.bounds.height) && !dp.scrollable)
				{
					finished = true;
				}
				else if(icon != null)
				{
					part = new GUIContent(icon);
					nextLabel = new LabelContent(part, this.xPos, this.yPos, tCol, sCol, font);
					v = new Vector2(nextLabel.bounds.width, nextLabel.bounds.height);
					//this.xPos += font.GetTextSize(".").x;
					this.label = ArrayHelper.Add(nextLabel, this.label);
					this.xPos += v.x;
					addSpace = true;
					icon = null;
				}
			}
			else if(finished)
			{
				this.xPos = 0;
				this.yPos += v.y + textPosition.lineSpacing;
			}
			
			this.textPos = i + skip;
		}
	}
	
	public Texture2D AddNextLabel(Texture2D texture, DialoguePosition dp)
	{
		return this.AddLabel(texture, dp, this.addNext++);
	}
	
	public bool HasNextLabel()
	{
		return this.addNext < this.label.Length;
	}
	
	public Texture2D GetTexture(Texture2D texture, DialoguePosition dp)
	{
		float y = -1;
		float offsetX = 0;
		for(int i=0; i<this.label.Length; i++)
		{
			if(dp.oneline && dp.alignCenter && 
				(i == 0 || this.label[i].bounds.y != y))
			{
				y = this.label[i].bounds.y;
				offsetX = 0;
				for(int j=i; j<this.label.Length; j++)
				{
					if(y == this.label[i].bounds.y) offsetX += this.label[i].bounds.width;
					else break;
				}
				offsetX = (dp.contentBounds.width-offsetX)/2;
			}
			else if(!dp.oneline && this.label[i].bounds.y > dp.contentBounds.height)
			{
				break;
			}
			this.label[i].bounds.x += offsetX;
			texture = this.AddLabel(texture, dp, i);
			this.addNext = i+1;
		}
		return texture;
	}
	
	public Texture2D AddLabel(Texture2D texture, DialoguePosition dp, int i)
	{
		return this.label[i].AddTexture(this.font, texture, dp);
	}
	
	public Texture2D GetTexture(Texture2D texture, DialoguePosition dp, float offsetX, float offsetY)
	{
		for(int i=0; i<this.label.Length; i++)
		{
			this.label[i].bounds.x += offsetX;
			this.label[i].bounds.y += offsetY;
			texture = this.AddLabel(texture, dp, i);
			this.label[i].bounds.x -= offsetX;
			this.label[i].bounds.y -= offsetY;
			this.addNext = i+1;
		}
		return texture;
	}
}