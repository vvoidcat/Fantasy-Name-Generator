﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public class Matrix {
        public int x { get; private set; }
        public int y { get; private set; }
        double[,] matrix;

        public Matrix(int lenY, int lenX) {
            if (lenX <= 0 || lenY <= 0) throw new ArgumentException("init size values should be above 0");

            y = lenY;
            x = lenX;
            matrix = new double[y, x];
            SetMatrix(0.0f);
        }

        public bool Contains(int indexY, int indexX) {
            return (indexX >= 0 && indexX < x && indexY >= 0 && indexY < y) ? true : false;
        }

        public double GetValueAtIndex(int indexY, int indexX) {
            if (!Contains(indexY, indexX)) throw new IndexOutOfRangeException("index out of range");
            return matrix[indexY, indexX];
        }

        public void SetValueAtIndex(int indexY, int indexX, double value) {
            if (!Contains(indexY, indexX)) throw new IndexOutOfRangeException("index out of range");
            matrix[indexY, indexX] = value;
        }

        public void SetMatrix(double value) {
            for (int i = 0; i < y; i++) {
                for (int j = 0; j < x; j++) {
                    SetValueAtIndex(i, j, value);

                    Console.WriteLine(matrix[i, j]);
                }
            }
        }

        public void IncrementValueAtIndex(int indexY, int indexX) {
            if (!Contains(indexY, indexX)) throw new IndexOutOfRangeException("index out of range");
            matrix[indexY, indexX] += 1.0f;
        }

        public void NormalizeMatrix() {
            for (int i = 0; i < y; i++) {
                double max = 1.0f;
                double sum = 0.0f;

                for (int j = 0; j < x; j++) {
                    sum += matrix[i, j];
                }

                if (sum > 0.0f) {
                    double norm = max / sum;

                    for (int j = 0; j < x; j++) {
                        matrix[i, j] = matrix[i, j] * norm;
                    }
                }
            }
        }
    }
}
