using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataCollector {

	static bool m_reset = false;

	static string m_title = "115H-1P_dpr-3B_dp-20mins";

	static CSVWriter m_csv;

	static DataCollector(){

		if(m_reset) {
			PlayerPrefs.DeleteKey("trial"); 
			PlayerPrefs.Save();
		}

		if(!PlayerPrefs.HasKey("trial")){
			PlayerPrefs.SetInt("trial", 0);
			PlayerPrefs.Save();
		} else{
			PlayerPrefs.SetInt("trial", PlayerPrefs.GetInt("trial", 0) + 1);
		}

		m_csv = new CSVWriter(m_title + PlayerPrefs.GetInt("trial", 0));

		string[] headers = {"Time", "Creature", "Attack", "Defence", "Speed", "HP", "Fitness", "ColSums", "WeightSum", "\n"};
		m_csv.WriteCSVRow(headers);
	}

	public static void recordData(string[] m_record){
		m_csv.WriteCSVRow(m_record);
	}



}
