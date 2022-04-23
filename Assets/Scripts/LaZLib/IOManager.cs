using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
using System;

public class IOManager : MonoBehaviour {

	
	
	public static string gameFolder{
		get{
			string dir = Application.dataPath;	//Application.persistentDataPath
			if (Application.platform == RuntimePlatform.OSXPlayer)	dir += "/../../";
			else if (Application.platform == RuntimePlatform.WindowsPlayer)	dir += "/../";

            else if (Application.platform == RuntimePlatform.Android) dir = Application.persistentDataPath;


            if (Application.isEditor)	dir += "/Data/";
			return dir;
		}
	}
	


	public static void SaveData(object data, string folder, string fname, bool inGameFolder = true, bool openSaveFolder = false){

		if (inGameFolder)
			folder = gameFolder + folder;

		if(!Directory.Exists(folder)){
			Directory.CreateDirectory(folder);
			Debug.Log("[IOManager] directory error: <no folder, create...>");
		}

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(folder + "/" + fname);

		bf.Serialize(file, data);
		file.Close();

		if(openSaveFolder) Application.OpenURL(folder);
	}
		


	public static object LoadData(string folder, string fname){

		bool a = Directory.Exists(gameFolder + folder);
		bool b = (a)? File.Exists(gameFolder + folder + "/" + fname) : false;

		if(a && b){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(gameFolder + folder + "/" + fname, FileMode.Open);

			object data = (object) bf.Deserialize(file);
			file.Close();

			return data;
		}
		else{
			Debug.Log("[IOManager] loading error: <no data>");
			return null;
		}
	}


	public static object LoadData(string path){

		if(File.Exists(path)){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(path, FileMode.Open);

			object data = (object) bf.Deserialize(file);
			file.Close();

			return data;
		}
		else{
			Debug.Log("[IOManager] loading error: <no data>");
			return null;
		}
	}






	//XML
	public static void SaveDataXML(object data, string folder, string fname, Type type = null, Type[] extraTypes = null, bool openSaveFolder = false){

		if(!Directory.Exists(gameFolder + folder)){
			Directory.CreateDirectory(gameFolder + folder);
			Debug.Log("[IOManager] directory error: <no folder, create...>");
		}

		XmlSerializer xml = new XmlSerializer((type == null)? data.GetType() : type, extraTypes);
		FileStream file = File.Create(gameFolder  + folder + "/" + fname);

		xml.Serialize(file, data);
		file.Close();

		Debug.Log("[IOManager] saving completed: <"+data.GetType().Name+">");
		if(openSaveFolder) Application.OpenURL(gameFolder + folder);
	}

	//XML
	public static object LoadDataXML(string path, Type type, Type[] extraTypes = null){

		if(File.Exists(path)){
			XmlSerializer xml = new XmlSerializer(type,extraTypes);
			FileStream file = File.Open(path, FileMode.Open);

			object data = (object) xml.Deserialize(file);
			file.Close();

			Debug.Log("[IOManager] loading completed: <"+type.Name+">");
			return data;
		}
		else{
			Debug.Log("[IOManager] loading error: <no data>");
			return null;
		}
	}


    //JSON
    public static void SaveDataJSON(object data, string folder, string fname, bool openSaveFolder = false)
    {

        if (!Directory.Exists(gameFolder + folder))
        {
            Directory.CreateDirectory(gameFolder + folder);
            Debug.Log("[IOManager] directory JSON error: <no folder, create...>");
        }

        string path = gameFolder + folder + "/" + fname;

        //FileStream file = File.Create(gameFolder + folder + "/" + fname);
        //xml.Serialize(file, data);
        //file.Close();

        string jsonData = JsonUtility.ToJson(data);

        File.WriteAllText(path, jsonData);


        Debug.Log("[IOManager] saving JSON completed: <" + data.GetType().Name + ">");
        if (openSaveFolder) Application.OpenURL(gameFolder + folder);
    }

    //JSON
    public static object LoadDataJSON(string path, Type type)
    {

        if (File.Exists(path))
        {
            
            //FileStream file = File.Open(path, FileMode.Open);
            //file.Close();

            string objectData = File.ReadAllText(path);
            

            object data = (object)JsonUtility.FromJson(objectData, type);

            Debug.Log("[IOManager] loading JSON completed: <" + type.Name + ">");
            return data;
        }
        else
        {
            Debug.Log("[IOManager] loading JSON error: <no data>");
            return null;
        }
    }




    public static void SaveTexture2DToPNG(Texture2D tex, string folder, string fname, bool openSaveFolder = false){
		byte[] bytesPng = tex.EncodeToPNG ();
		File.WriteAllBytes (gameFolder  + folder + "/" + fname, bytesPng);

		Debug.Log("[IOManager] Texture saving completed: <"+(gameFolder  + folder + "/" + fname)+">");
		if(openSaveFolder) Application.OpenURL(gameFolder + folder);
	}

	public static Texture2D LoadTexture2DFromPng(string folder, string fname){
		string url = gameFolder + folder + "/" + fname;

		if (!FileExist (folder, fname)) {
			Debug.Log("[IOManager] loading error: <file does not exist>");
			return null;
		}

		Texture2D tex;
		tex = new Texture2D(4, 4, TextureFormat.DXT1, false);

		byte[] binaryImageData = System.IO.File.ReadAllBytes(url);
		tex.LoadImage(binaryImageData);

		Debug.Log("[IOManager] Texture loading completed: <"+(gameFolder  + folder + "/" + fname)+">");

		return tex;
	}












	public static void SerializeINI(string name, string GamePath)
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		sb.AppendLine("name=" + name);
		System.IO.StreamWriter writer = new System.IO.StreamWriter(GamePath);
		writer.Write(sb.ToString());
		writer.Close();
	}

	public static string DeserializeINI(string name, string GamePath)
	{
		System.IO.StreamReader reader = new System.IO.StreamReader(GamePath);
		string line;
		while((line = reader.ReadLine()) != null)
		{
			string[] id_value = line.Split('=');
			switch (id_value[0])
			{
			case "name":
				name = id_value[1].ToString();
				break;
			}
		}
		reader.Close();
		return name;
	}








	//OPTIONAL
	public static bool FileExist(string folder, string name){
		return File.Exists(gameFolder + folder + "/" + name);
	}


	public static FileInfo[] FindFiles(string path, string filter = "*.*"){

		FileInfo[] entries = new FileInfo[0];

		if (path == "")
			return entries;

		DirectoryInfo info = new DirectoryInfo (path);
		//Debug.Log (path);
		if(!info.Exists){
			Debug.Log("[IOManager] searching error: <no folder>");
			return entries;
		}

		entries = info.GetFiles(filter);
		return entries;
	}
		



}
