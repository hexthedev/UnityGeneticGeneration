using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataCollector {


	static bool m_reset = false;
	static bool m_is_open = false;
	public static bool Open{ get { return m_is_open;}}
	static string m_title;
	static CSVWriter m_csv;

	static int m_trails_complete = 0;

	public static int Trials{ get {return m_trails_complete;} set { m_trails_complete+= value; }}

	public static int Trial{ get {return PlayerPrefs.GetInt("trial", 0); } }


	static DataCollector(){


		PlayerPrefs.DeleteKey("trial"); 
		PlayerPrefs.Save();


		if(!PlayerPrefs.HasKey("trial")){
			PlayerPrefs.SetInt("trial", 0);
			PlayerPrefs.Save();
		} else{
			PlayerPrefs.SetInt("trial", PlayerPrefs.GetInt("trial", 0) + 1);
		}		



	}

	public static void setTitle(string x){
		m_title = x;
	}

	public static void startCSV(){
		m_csv = new CSVWriter(m_title + PlayerPrefs.GetInt("trial", 0));
		m_is_open = true;
	}

	public static void closeCSV(){
		m_is_open = false;
	}


	public static void recordData(string x){
		m_csv.WriteCSVData(x + ",");
	}

	public static void newRow(){
		m_csv.WriteCSVData("\n");
	}


}
