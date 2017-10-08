﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Calc;

public class Matrix {

	float[ , ] m_matrix;

	public Matrix(float[,] p_matrix){
		m_matrix = (float[,]) p_matrix.Clone();
	}

	public Matrix(int p_width, int p_height){
		m_matrix = new float[p_width,p_height];
		randomPopulate();
	}	

	private void randomPopulate(){

		for(int x = 0; x<m_matrix.GetLength(0); x++){
			for(int y = 0; y<m_matrix.GetLength(1); y++){
				m_matrix[x,y] = Random.Range(0f, 1f);
			} 
		}
	}

	public Matrix clone(){
		return new Matrix((float[,])m_matrix.Clone());
	}

	public int numRows(){
		return m_matrix.GetLength(1);
	}

	public int numColumns(){
		return m_matrix.GetLength(0);
	}

	public void activate(DActivationFunction[] p_activators){
		
		if(numRows() > 1){
			Debug.LogError("Only single row vectors can be activated");
		}

		if(numColumns() != p_activators.Length){
			Debug.LogError("Bad number of activators");
		}

		for(int i = 0; i<numColumns(); i++){
			m_matrix[i, 0] = p_activators[i]( m_matrix[i, 0] );
		}
	}

	public float get(int col, int row){
		return m_matrix[col, row];
	}

	//STATIC
	public static bool isSameSize(Matrix p_matrix1, Matrix p_matrix2){
		return p_matrix1.m_matrix.GetLength(0) == p_matrix2.m_matrix.GetLength(0) && p_matrix1.m_matrix.GetLength(1) == p_matrix2.m_matrix.GetLength(1);
	}

	public static float[,] crossover(Matrix p_matrix1, Matrix p_matrix2){      
		if(!isSameSize(p_matrix1, p_matrix2)){
			Debug.LogError("CANNOT CROSSOVER MATRIX, NOT SAME SIZE");
		}
		
		float[,] to_return = new float[ p_matrix1.m_matrix.GetLength(0), p_matrix2.m_matrix.GetLength(1) ];

		for(int i = 0; i<to_return.GetLength(0);i++){
			for(int j = 0; j<to_return.GetLength(1);j++){
				to_return[i,j] = BoolCalc.random() ? p_matrix1.m_matrix[i,j] : p_matrix2.m_matrix[i,j];
			}
		}

		return to_return;
	}

	//OVERRIDES
	public override string ToString(){

		string to_return = "";

		for(int i = 0; i<m_matrix.GetLength(1); i++){
			to_return += "[";

			for(int j = 0; j<m_matrix.GetLength(0); j++){
				to_return += m_matrix[j,i] + " ";
			}

			to_return += "]\n";
		}

		return to_return;
	}

	//OPERATORS
	public static Matrix operator* (Matrix p_matrix1, Matrix p_matrix2){
		int[] inputSize = {p_matrix1.m_matrix.GetLength(0), p_matrix1.m_matrix.GetLength(1)};
		int[] outputSize = {p_matrix2.m_matrix.GetLength(0), p_matrix2.m_matrix.GetLength(1)};

		if(inputSize[0] != outputSize[1]){
			Debug.LogError("Invalid Matrix multiplication");
		}

		float[ , ] result = new float[outputSize[0] , inputSize[1]];

		for(int y = 0; y < inputSize[1]; y++){
			for(int x = 0; x < outputSize[0]; x++){
				for(int i = 0; i<inputSize[0]; i++){
					result[x, y] += p_matrix1.m_matrix[i, y] * p_matrix2.m_matrix[x,i];
				}
			}
		}
		
		return new Matrix(result);
	}

}
