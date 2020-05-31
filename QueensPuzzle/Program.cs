using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;

namespace QueensPuzzle
{
    
    class Program
    {
        static int N = 22;
        static int [,] BTmatrix = new int[N,N];
        static int [,] BTmatrixAux;
        static int [,] Pmatrix = new int[N,N];
        private static bool btSolved = false;
        
        private static void InitializeMatrix(int[,] matrix) {
            for (var i = 0; i < matrix.GetLength(0); i++) {
                for (var j = 0; j < matrix.GetLength(1); j++) {
                    matrix[i,j] = 0;
                }
            }
        }

        private static void PrintMatrix(int[,] matrix)
        {
            Console.WriteLine();
            for (var i = 0; i < matrix.GetLength(0); i++) {
                for (var j = 0; j < matrix.GetLength(1); j++) {
                    Console.Write(matrix[i,j]+" ");
                }
                Console.WriteLine();
            }
            Console.Write("--------");
        }
        private static void CleanMatrix(int[,] matrix)
        {
            Console.WriteLine();
            for (var i = 0; i < matrix.GetLength(0); i++) {
                for (var j = 0; j < matrix.GetLength(1); j++) {
                    if (matrix[i, j] == 2)
                        matrix[i, j] = 0;
                }
            }
        }

        private static bool Aceptable(int i, int stage)
        {
            //rows
            for (var j = 0; j < i; j++) {
                if (BTmatrix[j, stage] == 1)
                    return false;
            }
            
            //columns
            for (var j = 0; j <= stage; j++) {
                //rows
                if (BTmatrix[i,j] == 1)
                    return false;
            }
            
            var h = i;
            //left diagonal
            for (var j = stage; j >= 0 && h>=0; j--)
            {
                if (BTmatrix[h, j] == 1)
                    return false;
                h--;
            }
            
            //right diagonal
            h = i;
            for (var j = stage; j >= 0 && h<BTmatrix.GetLength(1); j--)
            {
                if (BTmatrix[h, j] == 1)
                    return false;
                h++;
            }
            return true;
        }
        private static void Backtracking(int stage) {
            for (int i = 0; i < BTmatrix.GetLength(0); i++) {
                if (Aceptable(i, stage) && !btSolved)
                {
                    BTmatrix[i, stage] = 1;
                    if (stage == BTmatrix.GetLength(1) - 1) {
                        BTmatrixAux = (int[,]) BTmatrix.Clone();
                        btSolved = true;
                    }
                    else {
                        Backtracking(stage+1);
                    }
                    BTmatrix[i, stage] = 0;
                }
            }
        }

        private static List<int> AvaibleCells(int row)
        {
            var aux=new List<int>();
            for (var i = 0; i < Pmatrix.GetLength(0); i++) {
                if(Pmatrix[row,i]==0)
                    aux.Add(i);
            }
            return aux;
        }
        
        private static void BlockCells(int row, int column)
        {
            //Number 2 stands for cells that are being attackedd
            //rows
            for (var j = 0; j < Pmatrix.GetLength(0); j++) {
                if (Pmatrix[row, j] == 0)
                    Pmatrix[row, j] = 2;
            }
            
            //columns
            for (var j = 0; j < Pmatrix.GetLength(1); j++) {
                //rows
                if (Pmatrix[j, column] == 0)
                    Pmatrix[j, column] = 2;
            }
            
            var h = row;
            //left diagonal
            for (var j = column; j >= 0 && h<Pmatrix.GetLength(0); j--)
            {
                //Console.WriteLine("Row: {0} Column {1}",h,j);
                if (Pmatrix[h, j] == 0)
                    Pmatrix[h, j] = 2;
                h++;
            }
            
            //right diagonal
            h = row;
            for (var j = column; j <Pmatrix.GetLength(0) &&h<Pmatrix.GetLength(1); j++)
            {
                if (Pmatrix[h, j] == 0)
                    Pmatrix[h, j] = 2;
                h++;
            }
        }
        private static void Vegas()
        {
            var random= new Random();//Random number for the probabilistic algorithm
            //is not being attacked
            for (var row = 0; row < Pmatrix.GetLength(1); row++) {
                var availableCells = AvaibleCells(row);//List that is going to save the cells of each row that
                if (availableCells.Any()) { //Checks if the list has values  
                    var randomNumber = random.Next(0, availableCells.Count-1);
                    Pmatrix[row, availableCells[randomNumber]] = 1;//Put the queen in the matrix
                    BlockCells(row,availableCells[randomNumber]);   
                }
                else {//If the list is empty, there's no solution for this attempt
                    row = -1;
                    InitializeMatrix(Pmatrix);
                }
            }
            //PrintMatrix(Pmatrix);
        }
        
        static void Main(string[] args)
        {
            InitializeMatrix(BTmatrix);
            InitializeMatrix(Pmatrix);
            var watchBt = System.Diagnostics.Stopwatch.StartNew();
            Backtracking(0);
            watchBt.Stop();
            var bTelapsedMs = watchBt.ElapsedMilliseconds;
            var watchV = System.Diagnostics.Stopwatch.StartNew();
            Vegas();
            watchV.Stop();
            var velapsedMs = watchV.ElapsedMilliseconds;
            
            Console.WriteLine("Duracion Backtracking: {0} Duracion Vegas: {1}",bTelapsedMs,velapsedMs);
            CleanMatrix(Pmatrix);
            
        }
    }
}