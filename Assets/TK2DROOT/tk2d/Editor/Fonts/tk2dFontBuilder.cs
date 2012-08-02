using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Xml;

namespace tk2dEditor.Font
{
	// Internal structures to fill and process
	public class Char
	{
		public int id = 0, x = 0, y = 0, width = 0, height = 0, xoffset = 0, yoffset = 0, xadvance = 0;
	
		public int texOffsetX, texOffsetY;
		public int texX, texY, texW, texH;
		public bool texFlipped;
		public bool texOverride;
	};
	
	public class Kerning
	{
		public int first = 0, second = 0, amount = 0;
	};
	
	public class Info
	{
		public int scaleW = 0, scaleH = 0;
		public int lineHeight = 0;
		
		public List<Char> chars = new List<Char>();
		public List<Kerning> kernings = new List<Kerning>();
	};

	public static class Builder
	{
		static int ReadIntAttribute(XmlNode node, string attribute)
		{
			return int.Parse(node.Attributes[attribute].Value, System.Globalization.NumberFormatInfo.InvariantInfo);
		}
		static float ReadFloatAttribute(XmlNode node, string attribute)
		{
			return float.Parse(node.Attributes[attribute].Value, System.Globalization.NumberFormatInfo.InvariantInfo);
		}
		static Vector2 ReadVector2Attributes(XmlNode node, string attributeX, string attributeY)
		{
			return new Vector2(ReadFloatAttribute(node, attributeX), ReadFloatAttribute(node, attributeY));
		}
		
		static Info ParseBMFontXml(XmlDocument doc)
		{
			Info fontInfo = new Info();
			
	        XmlNode nodeCommon = doc.SelectSingleNode("/font/common");
			fontInfo.scaleW = ReadIntAttribute(nodeCommon, "scaleW");
			fontInfo.scaleH = ReadIntAttribute(nodeCommon, "scaleH");
			fontInfo.lineHeight = ReadIntAttribute(nodeCommon, "lineHeight");
			int pages = ReadIntAttribute(nodeCommon, "pages");
			if (pages != 1)
			{
				EditorUtility.DisplayDialog("Fatal error", "Only one page supported in font. Please change the setting and re-export.", "Ok");
				return null;
			}
	
			foreach (XmlNode node in doc.SelectNodes(("/font/chars/char")))
			{
				Char thisChar = new Char();
				thisChar.id = ReadIntAttribute(node, "id");
	            thisChar.x = ReadIntAttribute(node, "x");
	            thisChar.y = ReadIntAttribute(node, "y");
	            thisChar.width = ReadIntAttribute(node, "width");
	            thisChar.height = ReadIntAttribute(node, "height");
	            thisChar.xoffset = ReadIntAttribute(node, "xoffset");
	            thisChar.yoffset = ReadIntAttribute(node, "yoffset");
	            thisChar.xadvance = ReadIntAttribute(node, "xadvance");
				
				thisChar.texOverride = false;
				
				fontInfo.chars.Add(thisChar);
			}
			
			foreach (XmlNode node in doc.SelectNodes("/font/kernings/kerning"))
			{
				Kerning thisKerning = new Kerning();
				thisKerning.first = ReadIntAttribute(node, "first");
				thisKerning.second = ReadIntAttribute(node, "second");
				thisKerning.amount = ReadIntAttribute(node, "amount");
				
				fontInfo.kernings.Add(thisKerning);
			}
	
			return fontInfo;
		}
		
		static string FindKeyValue(string[] tokens, string key)
		{
			string keyMatch = key + "=";
			for (int i = 0; i < tokens.Length; ++i)
			{
				if (tokens[i].Length > keyMatch.Length && tokens[i].Substring(0, keyMatch.Length) == keyMatch)
					return tokens[i].Substring(keyMatch.Length);
			}
			
			return "";
		}
		
		static Info ParseBMFontText(string path)
		{
			Info fontInfo = new Info();
			
			System.IO.FileInfo finfo = new System.IO.FileInfo(path);
			System.IO.StreamReader reader = finfo.OpenText();
			string line;
			while ((line = reader.ReadLine()) != null) 
			{
				string[] tokens = line.Split( ' ' );
				
				if (tokens[0] == "common")
				{
					fontInfo.lineHeight = int.Parse( FindKeyValue(tokens, "lineHeight") );
					fontInfo.scaleW = int.Parse( FindKeyValue(tokens, "scaleW") );
					fontInfo.scaleH = int.Parse( FindKeyValue(tokens, "scaleH") );
					int pages = int.Parse( FindKeyValue(tokens, "pages") );
					if (pages != 1)
					{
						EditorUtility.DisplayDialog("Fatal error", "Only one page supported in font. Please change the setting and re-export.", "Ok");
						return null;
					}
				}
				else if (tokens[0] == "char")
				{
					Char thisChar = new Char();
					thisChar.id = int.Parse(FindKeyValue(tokens, "id"));
					thisChar.x = int.Parse(FindKeyValue(tokens, "x"));
					thisChar.y = int.Parse(FindKeyValue(tokens, "y"));
					thisChar.width = int.Parse(FindKeyValue(tokens, "width"));
					thisChar.height = int.Parse(FindKeyValue(tokens, "height"));
					thisChar.xoffset = int.Parse(FindKeyValue(tokens, "xoffset"));
					thisChar.yoffset = int.Parse(FindKeyValue(tokens, "yoffset"));
					thisChar.xadvance = int.Parse(FindKeyValue(tokens, "xadvance"));
					fontInfo.chars.Add(thisChar);
				}
				else if (tokens[0] == "kerning")
				{
					Kerning thisKerning = new Kerning();
					thisKerning.first = int.Parse(FindKeyValue(tokens, "first"));
					thisKerning.second = int.Parse(FindKeyValue(tokens, "second"));
					thisKerning.amount = int.Parse(FindKeyValue(tokens, "amount"));
					fontInfo.kernings.Add(thisKerning);
				}
			}
			reader.Close();
			
			return fontInfo;
		}
		
		public static Info ParseBMFont(string path)
		{
			Info fontInfo = null;
			
			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(path);
				fontInfo = ParseBMFontXml(doc);
			}
			catch
			{
				fontInfo = ParseBMFontText(path);
			}
			
			if (fontInfo == null || fontInfo.chars.Count == 0)
				return null;
			
			return fontInfo;
		}
		
		public static bool BuildFont(Info fontInfo, tk2dFontData target, float scale, int charPadX, bool dupeCaps, bool flipTextureY, Texture2D gradientTexture, int gradientCount)
		{
			float texWidth = fontInfo.scaleW;
	        float texHeight = fontInfo.scaleH;
	        float lineHeight = fontInfo.lineHeight;
	
	        target.version = tk2dFontData.CURRENT_VERSION; 
	        target.lineHeight = lineHeight * scale;
	        target.texelSize = new Vector2(scale, scale);
			
			// Get number of characters (lastindex + 1)
			int maxCharId = 0;
			int maxUnicodeChar = 100000;
			foreach (var theChar in fontInfo.chars)
			{
				if (theChar.id > maxUnicodeChar)
				{
					// in most cases the font contains unwanted characters!
					Debug.LogError("Unicode character id exceeds allowed limit: " + theChar.id.ToString() + ". Skipping.");
					continue;
				}
				
				if (theChar.id > maxCharId) maxCharId = theChar.id;
			}
			
			// decide to use dictionary if necessary
			// 2048 is a conservative lower floor
			bool useDictionary = maxCharId > 2048;
			
			Dictionary<int, tk2dFontChar> charDict = (useDictionary)?new Dictionary<int, tk2dFontChar>():null;
			tk2dFontChar[] chars = (useDictionary)?null:new tk2dFontChar[maxCharId + 1];
			int minChar = 0x7fffffff;
			int maxCharWithinBounds = 0;
			int numLocalChars = 0;
			float largestWidth = 0.0f;
			foreach (var theChar in fontInfo.chars)
			{
				tk2dFontChar thisChar = new tk2dFontChar();
				int id = theChar.id;
	            int x = theChar.x;
	            int y = theChar.y;
	            int width = theChar.width;
	            int height = theChar.height;
	            int xoffset = theChar.xoffset;
	            int yoffset = theChar.yoffset;
	            int xadvance = theChar.xadvance + charPadX;
				
				// special case, if the width and height are zero, the origin doesn't need to be offset
				// handles problematic case highlighted here:
				// http://2dtoolkit.com/forum/index.php/topic,89.msg220.html
				if (width == 0 && height == 0)
				{
					xoffset = 0;
					yoffset = 0;
				}
	
	            // precompute required data
	            float px = xoffset * scale;
	            float py = (lineHeight - yoffset) * scale;
				
				if (theChar.texOverride)
				{
					int w = theChar.texW;
					int h = theChar.texH;
					if (theChar.texFlipped)
					{
						h = theChar.texW;
						w = theChar.texH;
					}
					
		            thisChar.p0 = new Vector3(px + theChar.texOffsetX * scale, py - theChar.texOffsetY * scale, 0);
		            thisChar.p1 = new Vector3(px + (theChar.texOffsetX + w) * scale, py - (theChar.texOffsetY + h) * scale, 0);
	
					thisChar.uv0 = new Vector2((theChar.texX) / texWidth, (theChar.texY + theChar.texH) / texHeight);
		            thisChar.uv1 = new Vector2((theChar.texX + theChar.texW) / texWidth, (theChar.texY) / texHeight);
					
					thisChar.flipped = theChar.texFlipped;
				}
				else
				{
		            thisChar.p0 = new Vector3(px, py, 0);
		            thisChar.p1 = new Vector3(px + width * scale, py - height * scale, 0);
					if (flipTextureY)
					{
			            thisChar.uv0 = new Vector2(x / texWidth, y / texHeight);
			            thisChar.uv1 = new Vector2(thisChar.uv0.x + width / texWidth, thisChar.uv0.y + height / texHeight);
					}
					else
					{
			            thisChar.uv0 = new Vector2(x / texWidth, 1.0f - y / texHeight);
			            thisChar.uv1 = new Vector2(thisChar.uv0.x + width / texWidth, thisChar.uv0.y - height / texHeight);
					}
					
					thisChar.flipped = false;
				}
	            thisChar.advance = xadvance * scale;
				largestWidth = Mathf.Max(thisChar.advance, largestWidth);
				
				// Needs gradient data
				if (gradientTexture != null)
				{
					// build it up assuming the first gradient
					float x0 = (float)(0.0f / gradientCount);
					float x1 = (float)(1.0f / gradientCount);
					float y0 = 1.0f;
					float y1 = 0.0f;
	
					// align to glyph if necessary
					
					thisChar.gradientUv = new Vector2[4];
					thisChar.gradientUv[0] = new Vector2(x0, y0);
					thisChar.gradientUv[1] = new Vector2(x1, y0);
					thisChar.gradientUv[2] = new Vector2(x0, y1);
					thisChar.gradientUv[3] = new Vector2(x1, y1);
				}
	
				if (id <= maxCharId)
				{
					maxCharWithinBounds = (id > maxCharWithinBounds) ? id : maxCharWithinBounds;
					minChar = (id < minChar) ? id : minChar;
					
					if (useDictionary)
						charDict[id] = thisChar;
					else
						chars[id] = thisChar;
					
					++numLocalChars;
				}
			}
			
			// duplicate capitals to lower case, or vice versa depending on which ones exist
	        if (dupeCaps)
	        {
	            for (int uc = 'A'; uc <= 'Z'; ++uc)
	            {
	                int lc = uc + ('a' - 'A');
					
					if (useDictionary)
					{
						if (charDict.ContainsKey(uc))
							charDict[lc] = charDict[uc];
						else if (charDict.ContainsKey(lc))
							charDict[uc] = charDict[lc];
					}
					else
					{
		                if (chars[lc] == null) chars[lc] = chars[uc];
		                else if (chars[uc] == null) chars[uc] = chars[lc];
					}
	            }
	        }
			
			// share null char, same pointer
			var nullChar = new tk2dFontChar();
			nullChar.gradientUv = new Vector2[4]; // this would be null otherwise
			
			target.largestWidth = largestWidth;
			if (useDictionary)
			{
				// guarantee at least the first 256 characters
				for (int i = 0; i < 256; ++i)
				{
					if (!charDict.ContainsKey(i))
						charDict[i] = nullChar;
				}
	
				target.chars = null;
				target.SetDictionary(charDict);
				target.useDictionary = true;
			}
			else
			{
				target.chars = new tk2dFontChar[maxCharId + 1];
				for (int i = 0; i <= maxCharId; ++i)
				{
					target.chars[i] = chars[i];
					if (target.chars[i] == null)
					{
						target.chars[i] = nullChar; // zero everything, null char
					}
				}
				
				target.charDict = null;
				target.useDictionary = false;
			}
			
			// kerning
			target.kerning = new tk2dFontKerning[fontInfo.kernings.Count];
			for (int i = 0; i < target.kerning.Length; ++i)
			{
				tk2dFontKerning kerning = new tk2dFontKerning();
				kerning.c0 = fontInfo.kernings[i].first;
				kerning.c1 = fontInfo.kernings[i].second;
				kerning.amount = fontInfo.kernings[i].amount * scale;
				target.kerning[i] = kerning;
			}
			
			return true;	
		}
	}
}

