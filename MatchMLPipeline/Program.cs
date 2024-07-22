using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.Cuda;
using VedAstro.Library;

namespace MatchMLPipeline
{

    internal class NearestCentroidProgram
    {

        /// <summary>
        /// A simple 1D kernel using math functions.
        /// The <see cref="IntrinsicMath"/> class contains intrinsic math functions that -
        /// in contrast to the default .Net Math class - work on both floats and doubles. Note that
        /// the /// <see cref="IntrinsicMath"/> class is supported on all accelerators.
        /// The CompileUnitFlags.FastMath flag can be used during the creation of the compile unit
        /// to enable fast math intrinsics.
        /// Note that the full power of math functions on all accelerators is available via the
        /// Algorithms library (see ILGPU.Algorithms.Math sample).
        /// </summary>
        /// <param name="index">The current thread index.</param>
        /// <param name="dataView">The view pointing to our memory buffer.</param>
        /// <param name="constant">A uniform constant.</param>
        static void MathKernel(
            Index1D index,                  // The global thread index (1D in this case)
            ArrayView<float> singleView,    // A view of floats to store float results from GPUMath
            ArrayView<double> doubleView,   // A view of doubles to store double results from GPUMath
            ArrayView<double> doubleView2)  // A view of doubles to store double results from .Net Math
        {
            // Note the different returns type of GPUMath.Sqrt and Math.Sqrt.
            singleView[index] = IntrinsicMath.Abs(index);
            doubleView[index] = IntrinsicMath.Clamp(index, 0.0, 12.0);

            // Note that use can safely use functions from the Math class as long as they have a counterpart
            // in the IntrinsicMath class.
            doubleView2[index] = Math.Min(0.2, index);
        }

        /// <summary>
        /// Launches a simple math kernel.
        /// </summary>
        static void Main5()
        {
            // Create main context
            using var context = Context.CreateDefault();

            // For each available device...
            foreach (var device in context)
            {
                // Create accelerator for the given device
                using var accelerator = device.CreateAccelerator(context);
                Console.WriteLine($"Performing operations on {accelerator}");

                var kernel = accelerator.LoadAutoGroupedStreamKernel<
                    Index1D, ArrayView<float>, ArrayView<double>, ArrayView<double>>(MathKernel);

                using var buffer = accelerator.Allocate1D<float>(128);
                using var buffer2 = accelerator.Allocate1D<double>(128);
                using var buffer3 = accelerator.Allocate1D<double>(128);

                // Launch buffer.Length many threads
                kernel((int)buffer.Length, buffer.View, buffer2.View, buffer3.View);

                // Reads data from the GPU buffer into a new CPU array.
                // Implicitly calls accelerator.DefaultStream.Synchronize() to ensure
                // that the kernel and memory copy are completed first.
                var data = buffer.GetAsArray1D();
                var data2 = buffer2.GetAsArray1D();
                var data3 = buffer3.GetAsArray1D();
                for (int i = 0, e = data.Length; i < e; ++i)
                    Console.WriteLine($"Math results: {data[i]} (float) {data2[i]} (double [GPUMath]) {data3[i]} (double [.Net Math])");
            }
        }


        static void Main2(string[] args)
        {


            //DatasetFactory.AddPersonIdToMarriageInfoDataset();


            //DatasetFactory.AddPersonIdToNameEmbeddingsDataset();

            //DatasetFactory.CleanPersonList();

            //DatasetFactory.PrintDatasetHighDataCredibility<MarriageInfoDatasetEntity>(DatasetFactory.marriageInfoDatasetClient_LocalEmulator);
            
            //DatasetFactory.PrintDatasetHighDataCredibility<MarriageInfoDatasetEntity>(DatasetFactory.marriageInfoDatasetClient_LocalEmulator);

            DatasetFactory.GenerateAllRoddenAAMarriges();
            //DatasetFactory.PrintAllRoddenAAMarriges();

            //DatasetFactory.CleanDatasetFromCharacter("```");

            //DatasetFactory.CleanDatasetFromCharacter<MarriageInfoDatasetEntity>("```", DatasetFactory.marriageInfoDatasetClient_LocalEmulator);

            //var result = DatasetFactory.FillPersonNameEmbeddings().Result;

            //var result = DatasetFactory.FamousPersonNameLLMSearch("mrilyn monroe");

            //var result = DatasetFactory.GeneratePersonLifeDataset();

            //var result = DatasetFactory.GenerateMarriageKutaDataset();

            Console.WriteLine("\nEnd demo ");
            Console.ReadLine();
        } // Main


        static void Main(string[] args)
        {
            Console.WriteLine("\nBegin nearest " +
              "centroid classification demo ");

            // 1. load and normalize training data
            //Console.WriteLine("\nLoading penguin subset " + "train (30) and test (10) data ");
            //string trainFile = "..\\..\\..\\Data\\penguin_train_30.txt";
            //double[][] trainX = NearestCentroidClassification.MatLoad(trainFile, new int[] { 1, 2, 3, 4 }, ',', "#");
            //double[][] trainX = DatasetFactory.LoadMarriageInfoTrainingVectors();
            //Console.WriteLine("\nX training raw: ");
            //NearestCentroidClassificationTools.MatShow(trainX, 1, 9, 4, true);

            // get normalized X and mins-maxs
            //Console.WriteLine("\nNormalizing train X" + " using min-max ");
            //double[][] minsMaxs = NearestCentroidClassificationTools.MatMinMaxValues(trainX);
            //trainX = NearestCentroidClassification.MatNormalizeUsing(trainX, minsMaxs);
            //Console.WriteLine("Done ");
            //Console.WriteLine("\nX training normalized: ");
            //NearestCentroidClassification.MatShow(trainX, 4, 9, 4, true);

            // get the training data labels/classes/species
            //int[] trainY = NearestCentroidClassification.VecLoad(trainFile, 0, "#");
            //int[] trainY = DatasetFactory.GetMarriageTrainingDataLabels();
            //Console.WriteLine("\nY training: ");
            //NearestCentroidClassificationTools.VecShow(trainY, wid: 3);

            //// 2. load and normalize test data
            //Console.WriteLine("\nLoading and " + "normalizing test data ");
            //string testFile = "..\\..\\..\\Data\\penguin_test_10.txt";
            //double[][] testX = NearestCentroidClassification.MatLoad(testFile, new int[] { 1, 2, 3, 4 }, ',', "#");
            //testX = NearestCentroidClassification.MatNormalizeUsing(testX, minsMaxs);
            //int[] testY = NearestCentroidClassification.VecLoad(testFile, 0, "#");
            //Console.WriteLine("Done ");

            // 3. create and train classifier
            Console.WriteLine("\nCreating " + "NearestCentroidClassifier object ");
            int numClasses = 4;
            NearestCentroidClassifier ncc = new NearestCentroidClassifier(numClasses);
            ncc.LoadFromTable("marriagePredictMK1");
            //Console.WriteLine("Training the classifier ");
            //ncc.Train(trainX, trainY);
            //ncc.SaveToTable("marriagePredictMK1");
            //ncc.Train(trainX2, trainY2);
            Console.WriteLine("Done ");

            Console.WriteLine("\nClass centroids: ");
            NearestCentroidClassificationTools.MatShow(ncc.centroids, 4, 9, 3, true);

            // 4. evaluate model
            //Console.WriteLine("\nEvaluating model ");
            //double accTrain = ncc.Accuracy(trainX, trainY);
            //Console.WriteLine("Accuracy on train: " +
            //  accTrain.ToString("F4"));

            ////double accTest = ncc.Accuracy(testX, testY);
            ////Console.WriteLine("Accuracy on test: " +
            ////  accTest.ToString("F4"));

            //Console.WriteLine("\nConfusion matrix" +
            //  " for training data: ");
            //int[][] cm = ncc.ConfusionMatrix(trainX, trainY);
            //ncc.ShowConfusion(cm);

            // 5. use model
            Console.WriteLine("\nPredicting species" +
              " for x = 46.5, 17.9, 192, 3500");

            //string[] speciesNames = new string[] { "Adelie", "Chinstrap", "Gentoo" };
            string[] speciesNames = Enum.GetNames(typeof(DatasetFactory.Outcome));
            //double[] xRaw = { 46.5, 17.9, 192, 3500 };
            double[] xRaw = { 8.0, 0.0, 0.0, 8.0, 0.0, 0.0, 8.0, 0.0 };
            double[][] minsMaxs = new[] { new[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 }, new[] { 8.0, 8.0, 8.0, 8.0, 8.0, 8.0, 8.0, 8.0 } };
            //double[] xNorm = NearestCentroidClassificationTools.VecNormalizeUsing(xRaw, minsMaxs);
            Console.Write("Normalized x =");
            NearestCentroidClassificationTools.VecShow(xRaw, 4, 9);

            int lbl = ncc.Predict(xRaw);
            Console.WriteLine("predicted label/class = " + lbl);
            Console.WriteLine("predicted species = " + speciesNames[lbl]);

            double[] pseudoProbs = ncc.PredictProbs(xRaw);
            Console.WriteLine("\nprediction pseudo-probs = ");
            NearestCentroidClassificationTools.VecShow(pseudoProbs, 4, 9);

            // 6. TODO: consider saving model (centroids)

            Console.WriteLine("\nEnd demo ");
            Console.ReadLine();
        } // Main

        // ------------------------------------------------------


    }
}
