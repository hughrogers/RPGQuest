
using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class SecurityHandler
{
	private static byte[] dataBytes = ASCIIEncoding.ASCII.GetBytes("i4Qh7P2v");
	private static byte[] gameBytes = ASCIIEncoding.ASCII.GetBytes("k2nA0x1F");
	
	/*
	============================================================================
	Data handling functions
	============================================================================
	*/
	public static string SaveData(string text)
	{
		if(DataHolder.GameSettings().encryptData)
		{
			return "encrypted"+SecurityHandler.EncryptDES(text, SecurityHandler.dataBytes);
		}
		else return text;
	}
	
	public static string LoadData(string text)
	{
		if(text.StartsWith("encrypted"))
		{
			return SecurityHandler.DecryptDES(text.Substring(9), SecurityHandler.dataBytes);
		}
		else return text;
	}
	
	/*
	============================================================================
	Save game handling functions
	============================================================================
	*/
	public static string SaveGame(string text)
	{
		if(DataHolder.LoadSaveHUD().encryptData)
		{
			return "encrypted"+SecurityHandler.EncryptDES(text, SecurityHandler.gameBytes);
		}
		else return text;
	}
	
	public static string LoadGame(string text)
	{
		if(text.StartsWith("encrypted"))
		{
			return SecurityHandler.DecryptDES(text.Substring(9), SecurityHandler.gameBytes);
		}
		else return text;
	}
	
	/*
	============================================================================
	DES handling functions
	============================================================================
	*/
	public static string EncryptDES(string text, byte[] key)
	{
		if(text != null && text != "")
		{
			DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, 
					cryptoProvider.CreateEncryptor(key, key), CryptoStreamMode.Write);
			StreamWriter writer = new StreamWriter(cryptoStream);
			writer.Write(text);
			writer.Flush();
			cryptoStream.FlushFinalBlock();
			writer.Flush();
			text = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
		} 
		return text;
	}
	
	public static string DecryptDES(string text, byte[] key)
	{
		if(text != null && text != "")
		{
			DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
			MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(text));
			CryptoStream cryptoStream = new CryptoStream(memoryStream, 
					cryptoProvider.CreateDecryptor(key, key), CryptoStreamMode.Read);
			StreamReader reader = new StreamReader(cryptoStream);
			text = reader.ReadToEnd();
		}
		return text;
	}
}
