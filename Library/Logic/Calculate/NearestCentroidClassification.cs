using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VedAstro.Library
{
    public static class NearestCentroidClassificationTools
    {

        public static double[][] MatLoad(string fn,
  int[] usecols, char sep, string comment)
        {
            // count number of non-comment lines
            int nRows = 0;
            string line = "";
            FileStream ifs = new FileStream(fn, FileMode.Open);
            StreamReader sr = new StreamReader(ifs);
            while ((line = sr.ReadLine()) != null)
                if (line.StartsWith(comment) == false)
                    ++nRows;
            sr.Close(); ifs.Close(); // could reset fp

            // make result matrix
            int nCols = usecols.Length;
            double[][] result = new double[nRows][];
            for (int r = 0; r < nRows; ++r)
                result[r] = new double[nCols];

            line = "";
            string[] tokens = null;
            ifs = new FileStream(fn, FileMode.Open);
            sr = new StreamReader(ifs);

            int i = 0;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith(comment) == true)
                    continue;
                tokens = line.Split(sep);
                for (int j = 0; j < nCols; ++j)
                {
                    int k = usecols[j];  // into tokens
                    result[i][j] = double.Parse(tokens[k]);
                }
                ++i;
            }
            sr.Close(); ifs.Close();
            return result;
        }

        // ------------------------------------------------------

        public static int[] VecLoad(string fn, int usecol,
          string comment)
        {
            char dummySep = ',';
            double[][] tmp = MatLoad(fn, new int[] { usecol },
              dummySep, comment);
            int n = tmp.Length;
            int[] result = new int[n];
            for (int i = 0; i < n; ++i)
                result[i] = (int)tmp[i][0];
            return result;
        }

        // ------------------------------------------------------

        public static double[][] MatMinMaxValues(double[][] X)
        {
            // return min and max values for each column of X
            // mins on row[0] of result, maxs at row[1]

            int nRows = X.Length;
            int nCols = X[0].Length;

            double[][] result = new double[2][];
            for (int i = 0; i < 2; ++i)
                result[i] = new double[nCols];

            for (int j = 0; j < nCols; ++j)
            {
                double colMin = X[0][j];
                double colMax = X[0][j];

                for (int i = 0; i < nRows; ++i)
                {
                    if (X[i][j] < colMin)
                        colMin = X[i][j];
                    if (X[i][j] > colMax)
                        colMax = X[i][j];
                }
                result[0][j] = colMin;
                result[1][j] = colMax;
            }

            return result;
        } // MatMinMaxValues

        // ------------------------------------------------------

        public static double[][] MatNormalizeUsing(double[][] X, double[][] minsMaxs)
        {
            // return normalized X, using mins and maxs
            int nRows = X.Length;
            int nCols = X[0].Length;
            double[][] result = new double[nRows][];
            for (int i = 0; i < nRows; ++i)
                result[i] = new double[nCols];
            for (int j = 0; j < nCols; ++j)
                for (int i = 0; i < nRows; ++i)
                    result[i][j] =
                      (X[i][j] - minsMaxs[0][j]) /
                      (minsMaxs[1][j] - minsMaxs[0][j]);
            return result;
        } // MatMinMaxNormalize using


        public static double[][] MatNormalizeUsing2(double[][] X, double[][] minsMaxs)
        {
            // Validate input dimensions
            if (X.Length != minsMaxs.Length)
            {
                throw new ArgumentException("Input arrays X and minsMaxs must have the same number of rows.");
            }

            if (X[0].Length != minsMaxs[0].Length || X[0].Length != minsMaxs[1].Length)
            {
                throw new ArgumentException("Input arrays X, minsMaxs[0], and minsMaxs[1] must have the same number of columns.");
            }

            int nRows = X.Length;
            int nCols = X[0].Length;
            double[][] result = new double[nRows][];

            // Initialize result array
            for (int i = 0; i < nRows; i++)
            {
                result[i] = new double[nCols];
            }

            // Perform min-max normalization for each column
            for (int j = 0; j < nCols; j++)
            {
                double minValue = minsMaxs[0][j];
                double maxValue = minsMaxs[1][j];
                double range = maxValue - minValue;

                // Avoid division by zero
                if (range == 0)
                {
                    // Handle case where all values are the same (set all normalized values to 0)
                    for (int i = 0; i < nRows; i++)
                    {
                        result[i][j] = 0;
                    }
                }
                else
                {
                    for (int i = 0; i < nRows; i++)
                    {
                        result[i][j] = (X[i][j] - minValue) / range;
                    }
                }
            }

            return result;
        }        // ------------------------------------------------------

        public static double[] VecNormalizeUsing(double[] x,
          double[][] minsMaxs)
        {
            int dim = x.Length;
            double[] result = new double[dim];
            for (int j = 0; j < dim; ++j)
                result[j] =
                  (x[j] - minsMaxs[0][j]) /
                  (minsMaxs[1][j] - minsMaxs[0][j]);
            return result;
        }

        // ------------------------------------------------------

        /// <summary>
        /// Displays a matrix M with decimal precision dec, 
        /// width wid, and shows indices if showIndices is true.
        /// If numRows is less than the total number of rows in M, 
        /// an ellipsis (...) is displayed at the end.
        /// </summary>
        /// <param name="M">The 2D array of double values to be displayed</param>
        /// <param name="dec">The number of decimal places to display for each value</param>
        /// <param name="wid">The minimum width of each value to be displayed</param>
        /// <param name="numRows">The number of rows to display from the 2D array</param>
        /// <param name="showIndices">A boolean indicating whether to display row indices</param>
        public static void MatShow(double[][] M, int dec, int wid, int numRows, bool showIndices)
        {
            // Calculate a small value based on the decimal places (dec)
            double small = 1.0 / Math.Pow(10, dec);

            // Loop through each row of the 2D array (M)
            for (int i = 0; i < numRows; ++i)
            {
                // If showIndices is true, print the row index with padding
                if (showIndices == true)
                {
                    int pad = M.Length.ToString().Length;
                    Console.Write("[" + i.ToString().PadLeft(pad) + "]");
                }

                // Loop through each column of the current row
                for (int j = 0; j < M[0].Length; ++j)
                {
                    // Get the value at the current position
                    double v = M[i][j];

                    // If the absolute value of v is smaller than the small value, set it to 0
                    if (Math.Abs(v) < small) v = 0.0;

                    // Print the value with the specified decimal places (dec) and width (wid)
                    Console.Write(v.ToString("F" + dec).PadLeft(wid));
                }

                // Move to the next line
                Console.WriteLine("");
            }

            // If there are more rows in the array than the specified number of rows (numRows), print an ellipsis
            if (numRows < M.Length) Console.WriteLine("...");
        }

        // ------------------------------------------------------

        public static void VecShow(int[] vec, int wid)
        {
            int n = vec.Length;
            for (int i = 0; i < n; ++i)
            {
                if (i != 0 && i % 12 == 0) Console.WriteLine("");
                Console.Write(vec[i].ToString().PadLeft(wid));
            }
            Console.WriteLine("");
        }

        // ------------------------------------------------------

        public static void VecShow(int[] vec, int wid,
          int nItems)
        {
            //int n = vec.Length;
            for (int i = 0; i < nItems; ++i)
            {
                if (i != 0 && i % 12 == 0) Console.WriteLine("");
                Console.Write(vec[i].ToString().PadLeft(wid));
            }
            Console.WriteLine("");
        }

        // ------------------------------------------------------

        public static void VecShow(double[] vec, int decimals,
          int wid)
        {
            int n = vec.Length;
            for (int i = 0; i < n; ++i)
                Console.Write(vec[i].ToString("F" + decimals).
                  PadLeft(wid));
            Console.WriteLine("");
        }

        // ------------------------------------------------------

    }
}
