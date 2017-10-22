using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataCollector {

	static CSVWriter m_csv;

	static DataCollector(){

	/*	if(!PlayerPrefs.HasKey("trail")){
			PlayerPrefs.SetInt("trail", 0);
			PlayerPrefs.Save();
		} else{
			PlayerPrefs.SetInt("trail", PlayerPrefs.GetInt("trail", 0) + 1);
		}

		m_csv = new CSVWriter("Trial" + PlayerPrefs.GetInt("trail", 0));

		string[] headers = {"Creature", "Attack", "Defence", "Speed", "HP", "Fitness", "\n"};
		m_csv.WriteCSVRow(headers);*/
	}

	public static void recordData(PhysicalDNA p_dna, int p_creature, float p_fitness){
		//m_csv.WriteCSVRow(p_dna.getStatsCSV(p_creature, p_fitness));
	}



}
