using System;
using System.IO;
using UnityEngine;

public static class Configuration
{
	const string ConfigName = "config.dat";
	static float hoverTime = 0.5f;
	static int highScore;

	public static int HighScore { get => highScore; set => highScore = value; }
	public static float MouseHoverTime { get => hoverTime; set => hoverTime = value; }

	public static void Load()
	{
		string path = string.Format("{0}{1}ATEyeGaze{1}config.dat", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.DirectorySeparatorChar);
		if (!File.Exists(path))
		{
			hoverTime = 0.5f;
			highScore = 0;
			return;
		}

		BinaryReader br = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read));
		hoverTime = br.ReadInt32() * 0.01f;
		highScore = br.ReadInt32();
		br.Close();
	}

	public static void Save()
	{
		string path = string.Format("{0}{1}ATEyeGaze", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Path.DirectorySeparatorChar);
		if (!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		path += Path.DirectorySeparatorChar + ConfigName;

		BinaryWriter bw = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate, FileAccess.Write));
		bw.Write((int)(hoverTime * 100));
		bw.Write(highScore);
		bw.Close();
	}
}
