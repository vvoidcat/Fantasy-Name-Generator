using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAMEGEN.Core {
    public class Matrix {
        public bool isZeroed { get; private set; } = true;
        public int x { get; private set; } = 0;
        public int y { get; private set; } = 0;
        private double[,] matrix;

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

            if (value > 0) isZeroed = false;
            matrix[indexY, indexX] = value;
        }

        public void SetMatrix(double value) {
            if (y > 0 && x > 0 && value <= 0) isZeroed = true;

            for (int i = 0; i < y; i++) {
                for (int j = 0; j < x; j++) {
                    SetValueAtIndex(i, j, value);
                }
            }
        }

        public void IncrementValueAtIndex(int indexY, int indexX) {
            SetValueAtIndex(indexY, indexX, GetValueAtIndex(indexY, indexX) + 1.0f);
        }

        public void Normalize() {
            if (!isZeroed) {
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
}
