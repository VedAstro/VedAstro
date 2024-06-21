using VedAstro.Library;

namespace MatchMLPipeline
{

    internal class NearestCentroidProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\nBegin nearest " +
              "centroid classification demo ");

            // 1. load and normalize training data
            Console.WriteLine("\nLoading penguin subset " + "train (30) and test (10) data ");
            string trainFile = "..\\..\\..\\Data\\penguin_train_30.txt";
            double[][] trainX = NearestCentroidClassification.MatLoad(trainFile, new int[] { 1, 2, 3, 4 }, ',', "#");
            Console.WriteLine("\nX training raw: ");
            NearestCentroidClassification.MatShow(trainX, 1, 9, 4, true);

            // get normalized X and mins-maxs
            Console.WriteLine("\nNormalizing train X" + " using min-max ");
            double[][] minsMaxs = NearestCentroidClassification.MatMinMaxValues(trainX);
            trainX = NearestCentroidClassification.MatNormalizeUsing(trainX, minsMaxs);
            Console.WriteLine("Done ");
            Console.WriteLine("\nX training normalized: ");
            NearestCentroidClassification.MatShow(trainX, 4, 9, 4, true);

            // get the training data labels/classes/species
            int[] trainY = NearestCentroidClassification.VecLoad(trainFile, 0, "#");
            Console.WriteLine("\nY training: ");
            NearestCentroidClassification.VecShow(trainY, wid: 3);

            // 2. load and normalize test data
            Console.WriteLine("\nLoading and " + "normalizing test data ");
            string testFile = "..\\..\\..\\Data\\penguin_test_10.txt";
            double[][] testX = NearestCentroidClassification.MatLoad(testFile, new int[] { 1, 2, 3, 4 }, ',', "#");
            testX = NearestCentroidClassification.MatNormalizeUsing(testX, minsMaxs);
            int[] testY = NearestCentroidClassification.VecLoad(testFile, 0, "#");
            Console.WriteLine("Done ");

            // 3. create and train classifier
            Console.WriteLine("\nCreating " + "NearestCentroidClassifier object ");
            int numClasses = 3;
            NearestCentroidClassifier ncc = new NearestCentroidClassifier(numClasses);
            Console.WriteLine("Training the classifier ");
            ncc.Train(trainX, trainY);
            Console.WriteLine("Done ");

            Console.WriteLine("\nClass centroids: ");
            NearestCentroidClassification.MatShow(ncc.centroids, 4, 9, 3, true);

            // 4. evaluate model
            Console.WriteLine("\nEvaluating model ");
            double accTrain = ncc.Accuracy(trainX, trainY);
            Console.WriteLine("Accuracy on train: " +
              accTrain.ToString("F4"));

            double accTest = ncc.Accuracy(testX, testY);
            Console.WriteLine("Accuracy on test: " +
              accTest.ToString("F4"));

            Console.WriteLine("\nConfusion matrix" +
              " for training data: ");
            int[][] cm = ncc.ConfusionMatrix(trainX, trainY);
            ncc.ShowConfusion(cm);

            // 5. use model
            Console.WriteLine("\nPredicting species" +
              " for x = 46.5, 17.9, 192, 3500");

            string[] speciesNames = new string[] { "Adelie", "Chinstrap", "Gentoo" };
            double[] xRaw = { 46.5, 17.9, 192, 3500 };
            double[] xNorm = NearestCentroidClassification.VecNormalizeUsing(xRaw, minsMaxs);
            Console.Write("Normalized x =");
            NearestCentroidClassification.VecShow(xNorm, 4, 9);

            int lbl = ncc.Predict(xNorm);
            Console.WriteLine("predicted label/class = " + lbl);
            Console.WriteLine("predicted species = " + speciesNames[lbl]);

            double[] pseudoProbs = ncc.PredictProbs(xNorm);
            Console.WriteLine("\nprediction pseudo-probs = ");
            NearestCentroidClassification.VecShow(pseudoProbs, 4, 9);

            // 6. TODO: consider saving model (centroids)

            Console.WriteLine("\nEnd demo ");
            Console.ReadLine();
        } // Main

        // ------------------------------------------------------


    } // Program


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

    } // class NearestCentroidClassifier
}
