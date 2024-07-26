namespace MatchMLPipeline;

public class NearestCentroidClassifier
{
    public int numClasses;
    public double[][] centroids;  // of each class

    public NearestCentroidClassifier(int numClasses)
    {
        this.numClasses = numClasses;
        this.centroids = new double[0][]; // keep compiler happy
    }

    public void Train(double[][] trainX, int[] trainY)
    {
        // compute centroid of each class
        int n = trainX.Length;
        int dim = trainX[0].Length;

        this.centroids = new double[this.numClasses][];
        for (int c = 0; c < numClasses; ++c)
            this.centroids[c] = new double[dim];

        double[][] sums = new double[this.numClasses][];
        for (int c = 0; c < numClasses; ++c)
            sums[c] = new double[dim];

        int[][] counts = new int[this.numClasses][];
        for (int c = 0; c < numClasses; ++c)
            counts[c] = new int[dim];

        for (int i = 0; i < n; ++i)
        {
            int c = trainY[i];
            for (int j = 0; j < dim; ++j)
            {
                sums[c][j] += trainX[i][j];
                ++counts[c][j];
            }
        }

        for (int c = 0; c < this.numClasses; ++c)
        for (int j = 0; j < dim; ++j)
            this.centroids[c][j] = sums[c][j] / counts[c][j];

        // // less efficient but more clear
        //for (int c = 0; c < this.numClasses; ++c)
        //{
        //  for (int j = 0; j < dim; ++j) // each col
        //  {
        //    double colSum = 0.0;
        //    int colCount = 0;
        //    for (int i = 0; i < n; ++i)  // each row
        //    {
        //      if (trainY[i] != c) continue;
        //      colSum += trainX[i][j];
        //      ++colCount;
        //    }
        //    this.means[c][j] = colSum / colCount;
        //  } // each col
        // } // each class

    } // Train

    // ------------------------------------------------------

    public int Predict(double[] x)
    {
        double[] distances = new double[this.numClasses];
        for (int c = 0; c < this.numClasses; ++c)
            distances[c] = EucDistance(x, this.centroids[c]);
        double smallestDist = distances[0];
        int result = 0;
        for (int c = 0; c < this.numClasses; ++c)
        {
            if (distances[c] < smallestDist)
            {
                smallestDist = distances[c];
                result = c;
            }
        }
        return result;
    }

    // ------------------------------------------------------

    public double[] PredictProbs(double[] x)
    {
        double[] probs = new double[this.numClasses];
        double[] distances = new double[this.numClasses];
        double[] invDists = new double[this.numClasses];

        double sum = 0.0;  // of inverse distances
        for (int c = 0; c < this.numClasses; ++c)
        {
            distances[c] = EucDistance(x, this.centroids[c]);
            if (distances[c] < 0.00000001)
                distances[c] = 0.00000001;  // avoid div by 0
            invDists[c] = 1.0 / distances[c];
            sum += invDists[c];
        }
        for (int c = 0; c < this.numClasses; ++c)
            probs[c] = invDists[c] / sum;
        return probs;  // pseudo-probabilities
    }

    // ------------------------------------------------------

    public double Accuracy(double[][] dataX, int[] dataY)
    {
        int nCorrect = 0;
        int nWrong = 0;
        int n = dataX.Length;
        for (int i = 0; i < n; ++i)
        {
            int c = this.Predict(dataX[i]);
            //Console.WriteLine("actual = " + dataY[i]);
            //Console.WriteLine("predicted = " + c);
            //Console.ReadLine();
            if (c == dataY[i])
                ++nCorrect;
            else
                ++nWrong;
        }
        return (nCorrect * 1.0) / (nCorrect + nWrong);
    }

    // ------------------------------------------------------

    private double EucDistance(double[] v1, double[] v2)
    {
        int dim = v1.Length;
        double sum = 0.0;
        for (int d = 0; d < dim; ++d)
            sum += (v1[d] - v2[d]) * (v1[d] - v2[d]);
        return Math.Sqrt(sum);
    }

    // ------------------------------------------------------

    public int[][] ConfusionMatrix(double[][] dataX,
        int[] dataY)
    {
        int n = this.numClasses;
        int[][] result = new int[n][];  // nxn
        for (int i = 0; i < n; ++i)
            result[i] = new int[n];

        for (int i = 0; i < dataX.Length; ++i)
        {
            double[] x = dataX[i];  // inputs
            int actualY = dataY[i];
            int predY = this.Predict(x);
            ++result[actualY][predY];
        }
        return result;
    }

    public void ShowConfusion(int[][] cm)
    {
        int n = cm.Length;
        int[] counts = new int[n];
        double[] accs = new double[n];
        for (int act = 0; act < n; ++act)
        {
            for (int pred = 0; pred < n; ++pred)
            {
                counts[act] += cm[act][pred];
            }
        }

        for (int act = 0; act < n; ++act)
        {
            accs[act] = (cm[act][act] * 1.0) / counts[act];
        }

        for (int i = 0; i < n; ++i)
        {
            Console.Write("actual " + i + ": ");
            for (int j = 0; j < n; ++j)
            {
                Console.Write(cm[i][j].ToString().
                    PadLeft(4) + " ");
            }
            Console.Write(" | " +
                          counts[i].ToString().PadLeft(4));
            Console.Write(" | " +
                          accs[i].ToString("F4").PadLeft(7));
            Console.WriteLine("");
        }
    }

}