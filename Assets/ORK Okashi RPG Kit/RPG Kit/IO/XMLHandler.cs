
using System.Collections;
using System.IO;
using UnityEngine;

public class XMLHandler
{
	// XML strings
	private static string OPEN = "<";
	private static string CLOSE = ">";
	private static string SPACE = " ";
	private static string EQUALS = "=";
	private static string QUOTE = "\"";
	private static string SLASH = "/";
	private static string QUESTION = "?";
	
	private static string COMMENT_OPEN = "<!--";
	private static string COMMENT_CLOSE = "-->";
	private static string CDATA_OPEN = "<![CDATA[";
	private static string CDATA_CLOSE = "]]>";
	
	// data strings
	public static string NODE_NAME = "_node_name";
	public static string CONTENT = "_content";
	public static string NODES = "_nodes";
	
	public static string XML_HEADER = "?xml";
	
	public static ArrayList LoadXML(string file)
	{
		if(Application.isEditor)
		{
			file = "Assets/Resources/"+file+".xml";
			if(File.Exists(file))
			{
				StreamReader sr = new StreamReader(file);
				string content = sr.ReadToEnd();
				sr.Close();
				return XMLHandler.ParseXML(SecurityHandler.LoadData(content));
			}
			else
			{
				return new ArrayList();
			}
		}
		else
		{
			TextAsset textAsset = (TextAsset)Resources.Load(file, typeof(TextAsset));
			if(textAsset)
			{
				return XMLHandler.ParseXML(SecurityHandler.LoadData(textAsset.text));
			}
			else
			{
				return new ArrayList();
			}
		}
	}
	
	public static ArrayList ParseXML(string text)
	{
		ArrayList result = new ArrayList();
		bool finished = false;
		int pos = 0;
		int i = 0;
		int i2 = 0;
		int j = 0;
		bool noVals = false;
		string sub = "";
		
		try
		{
			while (!finished)
			{
				noVals = false;
				i = text.IndexOf(XMLHandler.OPEN, pos);
				
				if(i == -1)
				{
					finished = true;
				}
				else
				{
					// comments are ignored
					if(i == text.IndexOf(XMLHandler.COMMENT_OPEN, pos))
					{
						i = text.IndexOf(XMLHandler.COMMENT_CLOSE, i);
						if(i == -1)
						{
							finished = true;
						}
						else
						{
							i += XMLHandler.COMMENT_CLOSE.Length-1;
						}
					}
					// regular node
					else
					{
						Hashtable ht = new Hashtable();
						
						int cl = text.IndexOf(XMLHandler.CLOSE, i);
						i2 = text.IndexOf(XMLHandler.SPACE, i);
						if(i2 == -1 || cl < i2)
						{
							i2 = cl;
							if(text[cl-1].ToString() == XMLHandler.SLASH || text[cl-1].ToString() == XMLHandler.QUESTION)
							{
								i2--;
							}
							
							noVals = true;
						}
						string name = text.Substring(i+1, i2-i-1);
						ht.Add(XMLHandler.NODE_NAME, name);
						i = i2;
						if(!noVals)
						{
							// first get node values
							string[] vals = text.Substring(i+1, cl-i-1).Split(XMLHandler.SPACE[0]);
							foreach(string s in vals)
							{
								j = s.IndexOf(XMLHandler.EQUALS);
								if(j != -1)
								{
									string key = s.Substring(0, j);
									j = s.IndexOf(XMLHandler.QUOTE, 0);
									string val = s.Substring(j+1, s.IndexOf(XMLHandler.QUOTE, j+1)-j-1);
									ht.Add(key, val);
								}
							}
						}
						
						// check for node closing
						if(text[cl-1].ToString() != XMLHandler.SLASH && text[cl-1].ToString() != XMLHandler.QUESTION)
						{
							string closeNode = XMLHandler.CloseNode(name);
							i2 = text.IndexOf(closeNode, cl);
							if(i2 == -1)
							{
								Debug.Log("Error: Unclosed node ("+name+")");
								finished = true;
							}
							else
							{
								sub = text.Substring(cl+1, i2-cl-1);
								j = 0;
								while(j < sub.Length)
								{
									if(sub[j].ToString() == XMLHandler.OPEN)
									{
										// CDATA
										if(j == sub.IndexOf(XMLHandler.CDATA_OPEN))
										{
											i = sub.IndexOf(XMLHandler.CDATA_CLOSE, j);
											j += XMLHandler.CDATA_OPEN.Length;
											ht.Add(XMLHandler.CONTENT, sub.Substring(j, i-j));
											
											if(i == -1)
											{
												finished = true;
											}
										}
										// subnodes
										else
										{
											ht.Add(XMLHandler.NODES, XMLHandler.ParseXML(sub));
										}
										break;
									}
									else if(sub[j].ToString() != "\n" && sub[j].ToString() != "\t" && 
										sub[j].ToString() != XMLHandler.SPACE && sub[j].ToString() != "\r")
									{
										ht.Add(XMLHandler.CONTENT, sub.Substring(j, sub.Length-j));
										break;
									}
									j++;
								}
								cl = i2 + closeNode.Length-1;
							}
						}
						
						result.Add(ht);
						i = cl;
					}
				}
				
				pos = i;
			}
		}
		catch (System.Exception e)
		{
			Debug.Log("Error: XML couldn't be parsed: "+e.Message);
		}
		
		return result;
	}
	
	public static void SaveXML(string dir, string file, ArrayList data)
	{
		dir = "Assets/Resources/"+dir;
		if(!Directory.Exists(dir))
		{
			Directory.CreateDirectory(dir);
		}
		XMLHandler.SaveXML(dir+file, data);
	}
	
	public static void SaveXML(string file, ArrayList data)
	{
		string result = SecurityHandler.SaveData(XMLHandler.ParseArrayList(data));
		StreamWriter sw = new StreamWriter(file+".xml");
		sw.Write(result);
		sw.Flush();
		sw.Close();
	}
	
	public static string ParseArrayList(ArrayList data)
	{
		string result = "";
		string node = "";
		string key = "";
		
		try
		{
			foreach(Hashtable entry in data)
			{
				key = entry[XMLHandler.NODE_NAME] as string;
				node = XMLHandler.OpenNode(key, entry);
				node += XMLHandler.AddContent(entry);
				
				if(key == XMLHandler.XML_HEADER)
				{
					result = node + "\n" + result;
				}
				else
				{
					result += node + XMLHandler.CloseNode(key) + "\n";
				}
			}
		}
		catch(System.Exception e)
		{
			Debug.Log("Error: ArrayList couldn't be parsed: "+e.Message);
		}
		return result;
	}
	
	public static string OpenNode(string name, Hashtable data)
	{
		string result = XMLHandler.OPEN+name;
		
		foreach(DictionaryEntry entry in data)
		{
			if(entry.Key as string != XMLHandler.CONTENT && 
				entry.Key as string != XMLHandler.NODES && 
				entry.Key as string != XMLHandler.NODE_NAME)
			{
				result += XMLHandler.SPACE+entry.Key+XMLHandler.EQUALS+XMLHandler.QUOTE+entry.Value+XMLHandler.QUOTE;
			}
		}
		
		if(name[0].ToString() == XMLHandler.QUESTION)
		{
			result += XMLHandler.QUESTION;
		}
		
		return result+XMLHandler.CLOSE;
	}
	
	public static string AddContent(Hashtable data)
	{
		string result = "";
		
		foreach(DictionaryEntry entry in data)
		{
			if(entry.Key as string == XMLHandler.CONTENT)
			{
				result += XMLHandler.CDATA_OPEN+entry.Value+XMLHandler.CDATA_CLOSE;
			}
			else if(entry.Key as string == XMLHandler.NODES)
			{
				result += "\n" + XMLHandler.ParseArrayList((ArrayList)entry.Value);
			}
		}
		
		return result;
	}
	
	public static string CloseNode(string name)
	{
		return XMLHandler.OPEN+XMLHandler.SLASH+name+XMLHandler.CLOSE;
	}
}