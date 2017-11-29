using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVWriter {

	private static string m_base_path = Application.dataPath + "/Evo_data/";

	private string m_file_path;

	public CSVWriter(string p_file_name){
		m_file_path = m_base_path + p_file_name + ".csv";

		if(!File.Exists(m_file_path)) createNewFile();
	}

	private void createNewFile(){
				
		if(!Directory.Exists(m_base_path)){
			Directory.CreateDirectory(m_base_path);
		}

		if(!File.Exists(m_file_path)){
			File.Create(m_file_path).Close();
		}		
	}

	public void WriteCSVData(string data){
		File.AppendAllText(m_file_path, data);
	}

	public void WriteCSVRow(string[] data){

		string data_row = "";

		for(int i = 0; i<data.Length; i++){
			if(i == data.Length-1){
				data_row += data[i];
				break;
			}

			data_row += data[i] + ",";
		}

		File.AppendAllText(m_file_path, data_row);
	}



}
