
using UnityEngine;
using System.Collections;

public class TextureDrawer
{
	/*
	============================================================================
	Utility functions
	============================================================================
	*/
	public static int GetNextPowerOfTwo(float number)
	{
		return Mathf.NextPowerOfTwo((int)number);
	}
	
	public static int GetNextPowerOfTwo(int number)
	{
		return Mathf.NextPowerOfTwo(number);
	}
	
	public static Vector2 GetScaledSize(Texture2D texture, Rect bounds, ScaleMode mode)
	{
		return TextureDrawer.GetScaledSize(texture, new Vector2(bounds.width, bounds.height), mode);
	}
	
	public static Vector2 GetScaledSize(Texture2D texture, Vector2 size, ScaleMode mode)
	{
		Vector2 scaledSize = Vector2.zero;
		if(ScaleMode.StretchToFill.Equals(mode))
		{
			scaledSize.x = size.x;
			scaledSize.y = size.y;
		}
		else if(ScaleMode.ScaleAndCrop.Equals(mode))
		{
			Vector2 ratio = new Vector2(size.x/texture.width, size.y/texture.height);
			if(ratio.x > ratio.y)
			{
				scaledSize.x = texture.width*ratio.x;
				scaledSize.y = texture.height*ratio.x;
			}
			else
			{
				scaledSize.x = texture.width*ratio.y;
				scaledSize.y = texture.height*ratio.y;
			}
			if(scaledSize.x > size.x) scaledSize.x = size.x;
			if(scaledSize.y > size.y) scaledSize.y = size.y;
		}
		else if(ScaleMode.ScaleToFit.Equals(mode))
		{
			Vector2 ratio = new Vector2(size.x/texture.width, size.y/texture.height);
			if(ratio.x < ratio.y)
			{
				scaledSize.x = texture.width*ratio.x;
				scaledSize.y = texture.height*ratio.x;
			}
			else
			{
				scaledSize.x = texture.width*ratio.y;
				scaledSize.y = texture.height*ratio.y;
			}
		}
		return scaledSize;
	}
	
	public static Color[] GetScaledPixels(Texture2D texture, Vector2 size, ScaleMode mode)
	{
		return TextureDrawer.GetScaledPixels(texture, TextureDrawer.GetScaledSize(texture, size, mode), size, mode);
	}
	
	public static Color[] GetScaledPixels(Texture2D texture, Rect bounds, ScaleMode mode)
	{
		Vector2 size = new Vector2(bounds.width, bounds.height);
		return TextureDrawer.GetScaledPixels(texture, TextureDrawer.GetScaledSize(texture, size, mode), 
				size, mode);
	}
	
	public static Color[] GetScaledPixels(Texture2D texture, Vector2 scaledSize, Rect bounds, ScaleMode mode)
	{
		return TextureDrawer.GetScaledPixels(texture, scaledSize, new Vector2(bounds.width, bounds.height), mode);
	}
	
	public static Color[] GetScaledPixels(Texture2D texture, Vector2 scaledSize, Vector2 size, ScaleMode mode)
	{
		Color[] colors = null;
		if(ScaleMode.StretchToFill.Equals(mode) || ScaleMode.ScaleToFit.Equals(mode))
		{
			colors = TextureDrawer.GetScaledColors(texture.GetPixels(), texture.width, 
					texture.height, (int)scaledSize.x, (int)scaledSize.y);
		}
		else if(ScaleMode.ScaleAndCrop.Equals(mode))
		{
			Vector2 ratio = new Vector2(size.x/texture.width, size.y/texture.height);
			Vector2 tmpSize = Vector2.zero;
			if(ratio.x > ratio.y)
			{
				tmpSize.x = texture.width*ratio.x;
				tmpSize.y = texture.height*ratio.x;
			}
			else
			{
				tmpSize.x = texture.width*ratio.y;
				tmpSize.y = texture.height*ratio.y;
			}
			colors = TextureDrawer.GetScaledColors(texture.GetPixels(), texture.width, 
					texture.height, (int)tmpSize.x, (int)tmpSize.y);
			Texture2D tex = TextureDrawer.GetNewTexture((int)tmpSize.x, (int)tmpSize.y);
			tex.SetPixels(colors);
			colors = tex.GetPixels(0, (int)(tex.height-scaledSize.y), (int)scaledSize.x, (int)scaledSize.y);
		}
		return colors;
	}
	
	/*
	============================================================================
	Texture functions
	============================================================================
	*/
	public static Texture2D GetNewTexture(int width, int height)
	{
		Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
		texture.filterMode = DataHolder.GameSettings().guiFilterMode;
		texture.anisoLevel = 1;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.mipMapBias = DataHolder.GameSettings().mipMapBias;
		return texture;
	}
	public static Texture2D GetCleanTexture(int width, int height)
	{
		return TextureDrawer.SetPixelColors(TextureDrawer.GetNewTexture(width, height), 
				new Rect(0, 0, width, height), Color.clear);
	}
	
	public static Texture2D SetPixelColors(Texture2D texture, Rect bounds, Color c)
	{
		Color[] colors = new Color[(int)(bounds.width*bounds.height)];
		for(int i=0; i<colors.Length; i++) colors[i] = c;
		texture = TextureDrawer.SetPixels(texture, colors, (int)bounds.x, (int)(texture.height-bounds.y-bounds.height), 
				(int)bounds.width, (int)bounds.height);
		return texture;
	}
	
	public static Texture2D ClearTexture(Texture2D texture)
	{
		return TextureDrawer.SetPixelColors(texture, new Rect(0, 0, texture.width, texture.height), Color.clear);
	}
	
	public static Texture2D CopyTexture(Texture2D texture)
	{
		Texture2D newTexture = TextureDrawer.GetNewTexture(texture.width,texture.height);
		newTexture.SetPixels(texture.GetPixels());
		return texture;
	}
	
	public static Texture2D GetMaskTexture(int width, int height, Rect maskBounds)
	{
		return TextureDrawer.SetPixelColors(TextureDrawer.GetCleanTexture(width, height), maskBounds, new Color(1, 1, 1, 1));
	}
	
	/*public static Texture2D SetImageTexture(Texture2D texture, Rect bounds, Texture2D bgTex, RectOffset border)
	{
		int yPos = (int)(texture.height-bounds.y-bounds.height);
		
		Color[] colors = null;
		if(border.left > 0)
		{
			// bottom left
			texture.SetPixels((int)bounds.x, yPos, border.left, border.bottom, 
					TextureDrawer.GetSecuredPixels(bgTex, 0, 0, border.left, border.bottom));
			// left center
			colors = TextureDrawer.GetScaledColors(
					TextureDrawer.GetSecuredPixels(bgTex, 0, border.bottom, border.left, bgTex.height-border.vertical),
					border.left, bgTex.height-border.vertical, border.left, (int)(bounds.height-border.vertical));
			texture.SetPixels((int)bounds.x, yPos+border.bottom, border.left, (int)(bounds.height-border.vertical), colors);
			// top left
			texture.SetPixels((int)bounds.x, yPos+(int)(bounds.height-border.top), 
					border.left, border.top, 
					TextureDrawer.GetSecuredPixels(bgTex, 0, bgTex.height-border.top, border.left, border.top));
		}
		
		int hAdd = 0;
		if(border.right > 0) hAdd = 1;
		// bottom center
		colors = TextureDrawer.GetScaledColors(
				TextureDrawer.GetSecuredPixels(bgTex, border.left, 0, bgTex.width-border.horizontal, border.bottom),
				bgTex.width-border.horizontal, border.bottom, (int)(bounds.width-border.horizontal+hAdd), border.bottom);
		texture.SetPixels((int)(bounds.x+border.left), yPos, (int)(bounds.width-border.horizontal+hAdd), border.bottom, colors);
		// center center
		colors = TextureDrawer.GetScaledColors(
				TextureDrawer.GetSecuredPixels(bgTex, border.left, border.bottom, bgTex.width-border.horizontal, bgTex.height-border.vertical),
				bgTex.width-border.horizontal, bgTex.height-border.vertical, 
				(int)(bounds.width-border.horizontal+hAdd), (int)(bounds.height-border.vertical));
		texture.SetPixels((int)bounds.x+border.left, yPos+border.bottom, 
				(int)(bounds.width-border.horizontal+hAdd), (int)(bounds.height-border.vertical), colors);
		// top center
		colors = TextureDrawer.GetScaledColors(
				TextureDrawer.GetSecuredPixels(bgTex, border.left, bgTex.height-border.top, bgTex.width-border.horizontal, border.top),
				bgTex.width-border.horizontal, border.top, (int)(bounds.width-border.horizontal+hAdd), border.top);
		texture.SetPixels((int)bounds.x+border.left, yPos+(int)(bounds.height-border.top), (int)(bounds.width-border.horizontal+hAdd), border.top, colors);
		
		if(border.right > 0)
		{
			// bottom right
			texture.SetPixels((int)(bounds.x+bounds.width-border.right), yPos, 
					border.right, border.bottom, 
					TextureDrawer.GetSecuredPixels(bgTex, bgTex.width-border.right, 0, border.right, border.bottom));
			// right center
			colors = TextureDrawer.GetScaledColors(
					TextureDrawer.GetSecuredPixels(bgTex, bgTex.width-border.right, border.bottom, border.right, bgTex.height-border.vertical),
					border.right, bgTex.height-border.vertical, border.right, (int)(bounds.height-border.vertical));
			texture.SetPixels((int)(bounds.x+bounds.width-border.right), yPos+border.bottom, border.right, (int)(bounds.height-border.vertical), colors);
			// top right
			texture.SetPixels((int)(bounds.x+bounds.width-border.right), yPos+(int)(bounds.height-border.top), 
					border.right, border.top, 
					TextureDrawer.GetSecuredPixels(bgTex, bgTex.width-border.right, bgTex.height-border.top, border.right, border.top));
		}
		
		return texture;
	}
	
	public static Texture2D AddImageTexture(Texture2D texture, Rect bounds, Texture2D bgTex, RectOffset border)
	{
		int yPos = (int)(texture.height-bounds.y-bounds.height);
		
		Color[] colors = null;
		if(border.left > 0)
		{
			// bottom left
			texture.SetPixels((int)bounds.x, yPos, border.left, border.bottom, 
					TextureDrawer.PasteColors(
						TextureDrawer.GetSecuredPixels(bgTex, 0, 0, border.left, border.bottom),
						texture.GetPixels((int)bounds.x, yPos, border.left, border.bottom)));
			// left center
			colors = TextureDrawer.GetScaledColors(
					TextureDrawer.GetSecuredPixels(bgTex, 0, border.bottom, border.left, bgTex.height-border.vertical),
					border.left, bgTex.height-border.vertical, border.left, (int)(bounds.height-border.vertical));
			texture.SetPixels((int)bounds.x, yPos+border.bottom, border.left, (int)(bounds.height-border.vertical), 
					TextureDrawer.PasteColors(colors,
						texture.GetPixels((int)bounds.x, yPos+border.bottom, border.left, (int)(bounds.height-border.vertical))));
			// top left
			texture.SetPixels((int)bounds.x, yPos+(int)(bounds.height-border.top), border.left, border.top, 
					TextureDrawer.PasteColors(TextureDrawer.GetSecuredPixels(bgTex, 0, bgTex.height-border.top, border.left, border.top),
						texture.GetPixels((int)bounds.x, yPos+(int)(bounds.height-border.top), border.left, border.top)));
		}
		
		int hAdd = 0;
		//if(border.right > 0) hAdd = 1;
		// bottom center
		colors = TextureDrawer.GetScaledColors(
				TextureDrawer.GetSecuredPixels(bgTex, border.left, 0, bgTex.width-border.horizontal, border.bottom),
				bgTex.width-border.horizontal, border.bottom, (int)(bounds.width-border.horizontal+hAdd), border.bottom);
		texture.SetPixels((int)(bounds.x+border.left), yPos, (int)(bounds.width-border.horizontal+hAdd), border.bottom, 
				TextureDrawer.PasteColors(colors, 
					texture.GetPixels((int)(bounds.x+border.left), yPos, (int)(bounds.width-border.horizontal+hAdd), border.bottom)));
		
		// center center
		colors = TextureDrawer.GetScaledColors(
				TextureDrawer.GetSecuredPixels(bgTex, border.left, border.bottom, bgTex.width-border.horizontal, bgTex.height-border.vertical),
				bgTex.width-border.horizontal, bgTex.height-border.vertical, 
				(int)(bounds.width-border.horizontal+hAdd), (int)(bounds.height-border.vertical));
		texture.SetPixels((int)bounds.x+border.left, yPos+border.bottom, 
				(int)(bounds.width-border.horizontal+hAdd), (int)(bounds.height-border.vertical), 
				TextureDrawer.PasteColors(colors,
					texture.GetPixels((int)bounds.x+border.left, yPos+border.bottom, 
					(int)(bounds.width-border.horizontal+hAdd), (int)(bounds.height-border.vertical))));
		
		// top center
		colors = TextureDrawer.GetScaledColors(
				TextureDrawer.GetSecuredPixels(bgTex, border.left, bgTex.height-border.top, bgTex.width-border.horizontal, border.top),
				bgTex.width-border.horizontal, border.top, (int)(bounds.width-border.horizontal+hAdd), border.top);
		texture.SetPixels((int)bounds.x+border.left, yPos+(int)(bounds.height-border.top), (int)(bounds.width-border.horizontal+hAdd), border.top, 
				TextureDrawer.PasteColors(colors,
					texture.GetPixels((int)bounds.x+border.left, yPos+(int)(bounds.height-border.top), 
					(int)(bounds.width-border.horizontal+hAdd), border.top)));
		
		if(border.right > 0)
		{
			// bottom right
			texture.SetPixels((int)(bounds.x+bounds.width-border.right), yPos, border.right, border.bottom, 
					TextureDrawer.PasteColors(
						TextureDrawer.GetSecuredPixels(bgTex, bgTex.width-border.right, 0, border.right, border.bottom),
						texture.GetPixels((int)(bounds.x+bounds.width-border.right), yPos, border.right, border.bottom)));
			// right center
			colors = TextureDrawer.GetScaledColors(
					TextureDrawer.GetSecuredPixels(bgTex, bgTex.width-border.right, border.bottom, border.right, bgTex.height-border.vertical),
					border.right, bgTex.height-border.vertical, border.right, (int)(bounds.height-border.vertical));
			texture.SetPixels((int)(bounds.x+bounds.width-border.right), yPos+border.bottom, border.right, (int)(bounds.height-border.vertical), 
					TextureDrawer.PasteColors(colors,
						texture.GetPixels((int)(bounds.x+bounds.width-border.right), 
						yPos+border.bottom, border.right, (int)(bounds.height-border.vertical))));
			// top right
			texture.SetPixels((int)(bounds.x+bounds.width-border.right), yPos+(int)(bounds.height-border.top), border.right, border.top, 
					TextureDrawer.PasteColors(
						TextureDrawer.GetSecuredPixels(bgTex, bgTex.width-border.right, bgTex.height-border.top, border.right, border.top),
							texture.GetPixels((int)(bounds.x+bounds.width-border.right), yPos+(int)(bounds.height-border.top), border.right, border.top)));
		}
		
		return texture;
	}*/
	
	public static Texture2D SetImageTexture(Texture2D texture, Rect bounds, SkinBG bg)
	{
		int yPos = (int)(texture.height-bounds.y-bounds.height);
		
		Color[] colors = null;
		if(bg.border.left > 0)
		{
			// bottom left
			texture.SetPixels((int)bounds.x, yPos, bg.border.left, bg.border.bottom, bg.bl);
			// left center
			colors = TextureDrawer.GetScaledColors(
					bg.cl, bg.border.left, (int)(bg.size.y-bg.border.vertical), bg.border.left, (int)(bounds.height-bg.border.vertical));
			texture.SetPixels((int)bounds.x, yPos+bg.border.bottom, bg.border.left, (int)(bounds.height-bg.border.vertical), colors);
			// top left
			texture.SetPixels((int)bounds.x, yPos+(int)(bounds.height-bg.border.top), 
					bg.border.left, bg.border.top, bg.tl);
		}
		
		int hAdd = 0;
		if(bg.border.right > 0) hAdd = 1;
		// bottom center
		colors = TextureDrawer.GetScaledColors(
				bg.bc, (int)(bg.size.x-bg.border.horizontal), bg.border.bottom, (int)(bounds.width-bg.border.horizontal+hAdd), bg.border.bottom);
		texture.SetPixels((int)(bounds.x+bg.border.left), yPos, (int)(bounds.width-bg.border.horizontal+hAdd), bg.border.bottom, colors);
		// center center
		colors = TextureDrawer.GetScaledColors(
				bg.cc, (int)(bg.size.x-bg.border.horizontal), (int)(bg.size.y-bg.border.vertical), 
				(int)(bounds.width-bg.border.horizontal+hAdd), (int)(bounds.height-bg.border.vertical));
		texture.SetPixels((int)bounds.x+bg.border.left, yPos+bg.border.bottom, 
				(int)(bounds.width-bg.border.horizontal+hAdd), (int)(bounds.height-bg.border.vertical), colors);
		// top center
		colors = TextureDrawer.GetScaledColors(
				bg.tc, (int)(bg.size.x-bg.border.horizontal), bg.border.top, (int)(bounds.width-bg.border.horizontal+hAdd), bg.border.top);
		texture.SetPixels((int)bounds.x+bg.border.left, yPos+(int)(bounds.height-bg.border.top), (int)(bounds.width-bg.border.horizontal+hAdd), bg.border.top, colors);
		
		if(bg.border.right > 0)
		{
			// bottom right
			texture.SetPixels((int)(bounds.x+bounds.width-bg.border.right), yPos, 
					bg.border.right, bg.border.bottom, bg.br);
			// right center
			colors = TextureDrawer.GetScaledColors(
					bg.cr, bg.border.right, (int)(bg.size.y-bg.border.vertical), bg.border.right, (int)(bounds.height-bg.border.vertical));
			texture.SetPixels((int)(bounds.x+bounds.width-bg.border.right), yPos+bg.border.bottom, bg.border.right, (int)(bounds.height-bg.border.vertical), colors);
			// top right
			texture.SetPixels((int)(bounds.x+bounds.width-bg.border.right), yPos+(int)(bounds.height-bg.border.top), 
					bg.border.right, bg.border.top, 
					bg.tr);
		}
		
		return texture;
	}
	
	public static Texture2D AddImageTexture(Texture2D texture, Rect bounds, SkinBG bg)
	{
		int yPos = (int)(texture.height-bounds.y-bounds.height);
		
		Color[] colors = null;
		if(bg.border.left > 0)
		{
			// bottom left
			texture.SetPixels((int)bounds.x, yPos, bg.border.left, bg.border.bottom, 
					TextureDrawer.PasteColors(
						bg.bl, texture.GetPixels((int)bounds.x, yPos, bg.border.left, bg.border.bottom)));
			// left center
			colors = TextureDrawer.GetScaledColors(
					bg.cl, bg.border.left, (int)(bg.size.y-bg.border.vertical), bg.border.left, (int)(bounds.height-bg.border.vertical));
			texture.SetPixels((int)bounds.x, yPos+bg.border.bottom, bg.border.left, (int)(bounds.height-bg.border.vertical), 
					TextureDrawer.PasteColors(colors,
						texture.GetPixels((int)bounds.x, yPos+bg.border.bottom, bg.border.left, (int)(bounds.height-bg.border.vertical))));
			// top left
			texture.SetPixels((int)bounds.x, yPos+(int)(bounds.height-bg.border.top), bg.border.left, bg.border.top, 
					TextureDrawer.PasteColors(bg.tl, texture.GetPixels((int)bounds.x, yPos+(int)(bounds.height-bg.border.top), bg.border.left, bg.border.top)));
		}
		
		int hAdd = 0;
		//if(bg.border.right > 0) hAdd = 1;
		// bottom center
		colors = TextureDrawer.GetScaledColors(
				bg.bc, (int)(bg.size.x-bg.border.horizontal), bg.border.bottom, (int)(bounds.width-bg.border.horizontal+hAdd), bg.border.bottom);
		texture.SetPixels((int)(bounds.x+bg.border.left), yPos, (int)(bounds.width-bg.border.horizontal+hAdd), bg.border.bottom, 
				TextureDrawer.PasteColors(colors, 
					texture.GetPixels((int)(bounds.x+bg.border.left), yPos, (int)(bounds.width-bg.border.horizontal+hAdd), bg.border.bottom)));
		
		// center center
		colors = TextureDrawer.GetScaledColors(
				bg.cc, (int)(bg.size.x-bg.border.horizontal), (int)(bg.size.y-bg.border.vertical), 
				(int)(bounds.width-bg.border.horizontal+hAdd), (int)(bounds.height-bg.border.vertical));
		texture.SetPixels((int)bounds.x+bg.border.left, yPos+bg.border.bottom, 
				(int)(bounds.width-bg.border.horizontal+hAdd), (int)(bounds.height-bg.border.vertical), 
				TextureDrawer.PasteColors(colors,
					texture.GetPixels((int)bounds.x+bg.border.left, yPos+bg.border.bottom, 
					(int)(bounds.width-bg.border.horizontal+hAdd), (int)(bounds.height-bg.border.vertical))));
		
		// top center
		colors = TextureDrawer.GetScaledColors(
				bg.tc, (int)(bg.size.x-bg.border.horizontal), bg.border.top, (int)(bounds.width-bg.border.horizontal+hAdd), bg.border.top);
		texture.SetPixels((int)bounds.x+bg.border.left, yPos+(int)(bounds.height-bg.border.top), (int)(bounds.width-bg.border.horizontal+hAdd), bg.border.top, 
				TextureDrawer.PasteColors(colors,
					texture.GetPixels((int)bounds.x+bg.border.left, yPos+(int)(bounds.height-bg.border.top), 
					(int)(bounds.width-bg.border.horizontal+hAdd), bg.border.top)));
		
		if(bg.border.right > 0)
		{
			// bottom right
			texture.SetPixels((int)(bounds.x+bounds.width-bg.border.right), yPos, bg.border.right, bg.border.bottom, 
					TextureDrawer.PasteColors(
						bg.br, texture.GetPixels((int)(bounds.x+bounds.width-bg.border.right), yPos, bg.border.right, bg.border.bottom)));
			// right center
			colors = TextureDrawer.GetScaledColors(
					bg.cr, bg.border.right, (int)(bg.size.y-bg.border.vertical), bg.border.right, (int)(bounds.height-bg.border.vertical));
			texture.SetPixels((int)(bounds.x+bounds.width-bg.border.right), yPos+bg.border.bottom, bg.border.right, (int)(bounds.height-bg.border.vertical), 
					TextureDrawer.PasteColors(colors,
						texture.GetPixels((int)(bounds.x+bounds.width-bg.border.right), 
						yPos+bg.border.bottom, bg.border.right, (int)(bounds.height-bg.border.vertical))));
			// top right
			texture.SetPixels((int)(bounds.x+bounds.width-bg.border.right), yPos+(int)(bounds.height-bg.border.top), bg.border.right, bg.border.top, 
					TextureDrawer.PasteColors(
						bg.tr, texture.GetPixels((int)(bounds.x+bounds.width-bg.border.right), yPos+(int)(bounds.height-bg.border.top), bg.border.right, bg.border.top)));
		}
		
		return texture;
	}
	
	/*
	============================================================================
	Color functions
	============================================================================
	*/
	public static Color[] PasteColors(Color[] topPixels, Color[] bottomPixels)
	{
        for(int i=0; i < bottomPixels.Length; i++)
		{
			if(topPixels[i].a == 1 || bottomPixels[i].a == 0)
			{
				bottomPixels[i] = topPixels[i];
			}
			else if(topPixels[i].a > 0)
			{
				float hlp = bottomPixels[i].a;
				bottomPixels[i] = Color.Lerp(bottomPixels[i], topPixels[i], topPixels[i].a);
				bottomPixels[i].a += hlp/2;
			}
        }
        return bottomPixels;
    }
	
	public static Color[] ReplaceColors(Color[] colors, Color oldColor, Color newColor)
	{
		for(int i=0; i<colors.Length; i++)
		{
			if(colors[i].r == oldColor.r && 
				colors[i].g == oldColor.g && 
				colors[i].b == oldColor.b)
			{
				colors[i].r = newColor.r;
				colors[i].g = newColor.g;
				colors[i].b = newColor.b;
			}
		}
		return colors;
	}
	
	public static Color[] Colorize(Color[] colors, Color tint)
	{
		for(int i=0; i < colors.Length; i++)
		{       
            colors[i].r = colors[i].r - (1.0f - tint.r);
            colors[i].g = colors[i].g - (1.0f - tint.g);
            colors[i].b = colors[i].b - (1.0f - tint.b);
        }
        return colors;
    }
	
	public static Color[] ColorizeAlphas(float[] alphas, Color tint)
	{
		Color[] colors = new Color[alphas.Length];
		for(int i=0; i < colors.Length; i++)
		{
			colors[i] = tint;
			colors[i].a = alphas[i];
        }
        return colors;
    }
	
	public static Color[] GetScaledColors(Color[] colors, int oldWidth, int oldHeight, int newWidth, int newHeight)
	{
		Color[] newColors = null;
		if(GUIImageStretch.POINT.Equals(DataHolder.GameSettings().guiImageStretch))
		{
			newColors = TextureDrawer.GetScaledColorsPoint(colors, oldWidth, oldHeight, newWidth, newHeight);
		}
		else if(GUIImageStretch.BILINEAR.Equals(DataHolder.GameSettings().guiImageStretch))
		{
			newColors = TextureDrawer.GetScaledColorsBilinear(colors, oldWidth, oldHeight, newWidth, newHeight);
		}
		return newColors;
	}
	
	public static Color[] GetScaledColorsPoint(Color[] colors, int oldWidth, int oldHeight, int newWidth, int newHeight)
	{
		if(oldWidth < 1) oldWidth = 2;
		if(oldHeight < 1) oldHeight = 2;
		if(newWidth < 1) newWidth = 1;
		if(newHeight < 1) newHeight = 1;
		Color[] newColors = new Color[newWidth * newHeight];
		
		int x_ratio = (int)((oldWidth<<16)/newWidth) +1;
		int y_ratio = (int)((oldHeight<<16)/newHeight) +1;
		
		int x2, y2;
		for (int i=0;i<newHeight;i++)
		{
			for (int j=0;j<newWidth;j++)
			{
				x2 = ((j*x_ratio)>>16);
				y2 = ((i*y_ratio)>>16);
				newColors[(i*newWidth)+j] = colors[Mathf.Min((y2*oldWidth)+x2, colors.Length-1)];
			}
		}
		return newColors;
	}
	
	public static Color[] GetScaledColorsBilinear(Color[] colors, int oldWidth, int oldHeight, int newWidth, int newHeight)
	{
		bool wScale = oldWidth != newWidth;
		bool hScale = oldHeight != newHeight;
		if(oldWidth < 1) oldWidth = 2;
		if(oldHeight < 1) oldHeight = 2;
		if(newWidth < 1) newWidth = 1;
		if(newHeight < 1) newHeight = 1;
		Color[] newColors = new Color[newWidth*newHeight];
		Color a, b, c, d;
		int x, y, index;
		float x_ratio = ((float)(oldWidth-1))/newWidth;
		float y_ratio = ((float)(oldHeight-1))/newHeight;
		float x_diff, y_diff, blue, red, green;
		int offset = 0;
		for(int i=0; i<newHeight; i++)
		{
			for(int j=0; j<newWidth; j++)
			{
				if(wScale)
				{
					x = (int)(x_ratio * j);
					x_diff = (x_ratio * j) - x;
				}
				else
				{
					x = (int)Mathf.Ceil(x_ratio * j);
					x_diff = 0;
				}
				if(hScale)
				{
					y = (int)(y_ratio * i);
					y_diff = (y_ratio * i) - y;
				}
				else
				{
					y = (int)Mathf.Ceil(y_ratio * i);
					y_diff = 0;
				}
				
				index = (y*oldWidth+x);                
				a = colors[index];
				if(index+1 < colors.Length) b = colors[index+1];
				else b = a;
				if(index+oldWidth < colors.Length) c = colors[index+oldWidth];
				else c = a;
				if(index+oldWidth+1 < colors.Length) d = colors[index+oldWidth+1];
				else d = a;
				
				blue = a.b*(1-x_diff)*(1-y_diff) + b.b*(x_diff)*(1-y_diff) +
					   c.b*(y_diff)*(1-x_diff)   + d.b*(x_diff*y_diff);
				green = a.g*(1-x_diff)*(1-y_diff) + b.g*(x_diff)*(1-y_diff) +
						c.g*(y_diff)*(1-x_diff)   + d.g*(x_diff*y_diff);
				red = a.r*(1-x_diff)*(1-y_diff) + b.r*(x_diff)*(1-y_diff) +
					  c.r*(y_diff)*(1-x_diff)   + d.r*(x_diff*y_diff);

				newColors[offset++] = new Color(red, green, blue, colors[index].a);
			}
		}
		return newColors;
	}
	
	public static Color ColorLerpUnclamped(Color c1, Color c2, float value)
	{
		return new Color(c1.r + (c2.r - c1.r)*value,
			  c1.g + (c2.g - c1.g)*value,
			  c1.b + (c2.b - c1.b)*value,
			  c1.a + (c2.a - c1.a)*value);
	}
	
	public static Color[] GetSecuredPixels(Texture2D texture, int x, int y, int width, int height)
	{
		if(width < 1)
		{
			width = 2;
			if(x>0) x--;
		}
		if(height < 1)
		{
			height = 2;
			if(y>0) y--;
		}
		return texture.GetPixels(x, y, width, height);
	}
	
	public static Color[] MultiplyAlpha(Color[] colors, float alpha)
	{
		for(int i=0; i < colors.Length; i++)
		{
			colors[i].a *= alpha;
        }
        return colors;
    }
	
	public static Color[] DivideAlpha(Color[] colors, float alpha)
	{
		if(alpha > 0)
		{
			for(int i=0; i < colors.Length; i++)
			{
				colors[i].a /= alpha;
			}
		}
        return colors;
    }
	
	/*
	============================================================================
	Drawing functions
	============================================================================
	*/
	public static Texture2D AddTexture(Texture2D texture, Rect bounds, Texture2D addTex)
	{
		texture.SetPixels((int)bounds.x, (int)bounds.y, (int)bounds.width, (int)bounds.height, 
				TextureDrawer.PasteColors(addTex.GetPixels(), 
				texture.GetPixels((int)bounds.x, (int)bounds.y, (int)bounds.width, (int)bounds.height)));
		return texture;
	}
	
	public static Texture2D AddTexture(Texture2D texture, Rect bounds, Color[] colors)
	{
		/*texture.SetPixels((int)bounds.x, (int)bounds.y, (int)bounds.width, (int)bounds.height, 
				TextureDrawer.PasteColors(colors, 
				texture.GetPixels((int)bounds.x, (int)bounds.y, (int)bounds.width, (int)bounds.height)));*/
		return TextureDrawer.AddPixels(texture, colors, (int)bounds.x, (int)bounds.y, (int)bounds.width, (int)bounds.height);
	}
	
	public static Color[] GetPixels(Texture2D texture, int x, int y, int w, int h)
	{
		if(x < 0)
		{
			w += x;
			if(w < 0) w = 0;
			x = 0;
		}
		if(x+w > texture.width)
		{
			w += texture.width-(x+w);
			if(w < 0) w = 0;
		}
		if(y < 0)
		{
			h += y;
			if(h < 0) h = 0;
			y = 0;
		}
		if(y+h > texture.height)
		{
			h += texture.height-(y+h);
			if(h < 0) h = 0;
		}
		return texture.GetPixels(x, y, w, h);
	}
	
	public static Texture2D SetPixels(Texture2D texture, Color[] colors, Rect bounds)
	{
		return TextureDrawer.SetPixels(texture, colors, (int)bounds.x, (int)bounds.y, (int)bounds.width, (int)bounds.height);
	}
	
	public static Texture2D SetPixels(Texture2D texture, Color[] colors, int x, int y, int w, int h)
	{
		if(colors.Length > 0)
		{
			if(x < 0)
			{
				Texture2D tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
				tex.SetPixels(colors);
				w += x;
				if(w > 0)  colors = tex.GetPixels(-x, 0, w, h);
				else colors = new Color[0];
				x = 0;
				GameObject.Destroy(tex);
			}
			if(colors.Length > 0)
			{
				if(x+w > texture.width)
				{
					Texture2D tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
					tex.SetPixels(colors);
					w += texture.width-(x+w);
					if(w > 0) colors = tex.GetPixels(0, 0, w, h);
					else colors = new Color[0];
					GameObject.Destroy(tex);
				}
				if(colors.Length > 0)
				{
					if(y < 0)
					{
						Texture2D tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
						tex.SetPixels(colors);
						h += y;
						if(h > 0)  colors = tex.GetPixels(0, -y, w, h);
						else colors = new Color[0];
						y = 0;
						GameObject.Destroy(tex);
					}
					if(colors.Length > 0)
					{
						if(y+h > texture.height)
						{
							Texture2D tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
							tex.SetPixels(colors);
							h += texture.height-(y+h);
							if(h > 0) colors = tex.GetPixels(0, 0, w, h);
							else colors = new Color[0];
							GameObject.Destroy(tex);
						}
						if(colors.Length > 0) texture.SetPixels(x, y, w, h, colors);
					}
				}
			}
		}
		return texture;
	}
	
	public static Texture2D SetPixels(Texture2D texture, int sx, int sy, Texture2D texture2, int x, int y, int w, int h)
	{
		if(x < 0)
		{
			w += x;
			if(w < 0) w = 0;
			x = 0;
		}
		if(x+w > texture2.width)
		{
			w += texture2.width-(x+w);
			if(w < 0) w = 0;
		}
		if(y < 0)
		{
			h += y;
			if(h < 0) h = 0;
			y = 0;
		}
		if(y+h > texture2.height)
		{
			h += texture2.height-(y+h);
			if(h < 0) h = 0;
		}
		Color[] colors = texture2.GetPixels(x, y, w, h);
		if(colors.Length > 0)
		{
			if(sx < 0)
			{
				Texture2D tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
				tex.SetPixels(colors);
				w += sx;
				if(w > 0)  colors = tex.GetPixels(-sx, 0, w, h);
				else colors = new Color[0];
				sx = 0;
				GameObject.Destroy(tex);
			}
			if(colors.Length > 0)
			{
				if(sx+w > texture.width)
				{
					Texture2D tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
					tex.SetPixels(colors);
					w += texture.width-(sx+w);
					if(w > 0) colors = tex.GetPixels(0, 0, w, h);
					else colors = new Color[0];
					GameObject.Destroy(tex);
				}
				if(colors.Length > 0)
				{
					if(sy < 0)
					{
						Texture2D tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
						tex.SetPixels(colors);
						h += sy;
						if(h > 0)  colors = tex.GetPixels(0, -sy, w, h);
						else colors = new Color[0];
						sy = 0;
						GameObject.Destroy(tex);
					}
					if(colors.Length > 0)
					{
						if(sy+h > texture.height)
						{
							Texture2D tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
							tex.SetPixels(colors);
							h += texture.height-(sy+h);
							if(h > 0) colors = tex.GetPixels(0, 0, w, h);
							else colors = new Color[0];
							GameObject.Destroy(tex);
						}
						if(colors.Length > 0) texture.SetPixels(sx, sy, w, h, colors);
					}
				}
			}
		}
		return texture;
	}
	
	public static Texture2D AddPixels(Texture2D texture, Color[] colors, int x, int y, int w, int h)
	{
		if(colors.Length > 0)
		{
			if(x < 0)
			{
				Texture2D tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
				tex.SetPixels(colors);
				w += x;
				if(w > 0)  colors = tex.GetPixels(-x, 0, w, h);
				else colors = new Color[0];
				x = 0;
				GameObject.Destroy(tex);
			}
			if(colors.Length > 0)
			{
				if(x+w > texture.width)
				{
					Texture2D tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
					tex.SetPixels(colors);
					w += texture.width-(x+w);
					if(w > 0) colors = tex.GetPixels(0, 0, w, h);
					else colors = new Color[0];
					GameObject.Destroy(tex);
				}
				if(colors.Length > 0)
				{
					if(y < 0)
					{
						Texture2D tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
						tex.SetPixels(colors);
						h += y;
						if(h > 0)  colors = tex.GetPixels(0, -y, w, h);
						else colors = new Color[0];
						y = 0;
						GameObject.Destroy(tex);
					}
					if(colors.Length > 0)
					{
						if(y+h > texture.height)
						{
							Texture2D tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
							tex.SetPixels(colors);
							h += texture.height-(y+h);
							if(h > 0) colors = tex.GetPixels(0, 0, w, h);
							else colors = new Color[0];
							GameObject.Destroy(tex);
						}
						if(colors.Length > 0) texture.SetPixels(x, y, w, h, 
									TextureDrawer.PasteColors(colors, texture.GetPixels(x, y, w, h)));
					}
				}
			}
		}
		return texture;
	}
	
	/*public static Texture2D DrawBox(Texture2D texture, GUISkin skin, Rect bounds)
	{
		TextureDrawer.AddTexture(texture, bounds, TextureDrawer.GetImageTexture(bounds, skin.box.normal.background, skin.box.border));
		return texture;
	}*/
	
	/*
	============================================================================
	GUISkin texture functions
	============================================================================
	*/
	/*public static Texture2D GetBackground(DialoguePosition dp)
	{
		Texture2D texture = null;
		if(dp.isDragWindow && dp.IsFocused())
		{
			texture = TextureDrawer.GetImageTexture(dp.boxBounds, dp.skin.window.onNormal.background, dp.skin.window.border);
		}
		else if(dp.isDragWindow)
		{
			texture = TextureDrawer.GetImageTexture(dp.boxBounds, dp.skin.window.normal.background, dp.skin.window.border);
		}
		else if(dp.showBox)
		{
			texture = TextureDrawer.GetImageTexture(dp.boxBounds, dp.skin.box.normal.background, dp.skin.box.border);
		}
		else
		{
			texture = TextureDrawer.GetCleanTexture((int)dp.boxBounds.width, (int)dp.boxBounds.height);
		}
		return texture;
	}*/
}
