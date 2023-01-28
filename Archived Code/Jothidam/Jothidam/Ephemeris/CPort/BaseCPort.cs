using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwissEphNet.CPort
{
    abstract class BaseCPort
    {
        public BaseCPort(SwissEph se) {
            this.SE = se;
        }
        protected static T[] CreateArray<T>(int dim1) {
            var result = new T[dim1];
            for (int i = 0; i < dim1; i++) {
                result[i] = default(T);
            }
            return result;
        }
        protected static T[][] CreateArray<T>(int dim1, int dim2) {
            T[][] result = new T[dim1][];
            for (int i = 0; i < dim1; i++) {
                result[i] = new T[dim2];
                for (int j = 0; j < dim2; j++) {
                    result[i][j] = default(T);
                }
            }
            return result;
        }
        protected static T[][] CreateArray<T>(T[,] fromArray) {
            int dim1 = fromArray.GetLength(0), dim2 = fromArray.GetLength(1);
            T[][] result = CreateArray<T>(dim1, dim2);
            for (int i = 0; i < dim1; i++) {
                //result[i] = new T[dim2];
                for (int j = 0; j < dim2; j++) {
                    result[i][j] = fromArray[i, j];
                }
            }
            return result;
        }
        protected void trace(String format, params object[] args) {
            SE.Trace(format, args);
        }
        protected SwissEph SE { get; private set; }
    }
}
