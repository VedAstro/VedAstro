

//█░░░█ █░░█ █░░█ 　 █▀▀▄ █▀▀█ 　 █░░█ █▀▀█ █░░█ 　 █▀▀█ █▀▀ █░█ 　 █░░░█ █░░█ █▀▀█ ▀▀█▀▀ 　 ░▀░ █▀▀ 
//█▄█▄█ █▀▀█ █▄▄█ 　 █░░█ █░░█ 　 █▄▄█ █░░█ █░░█ 　 █▄▄█ ▀▀█ █▀▄ 　 █▄█▄█ █▀▀█ █▄▄█ ░░█░░ 　 ▀█▀ ▀▀█ 
//░▀░▀░ ▀░░▀ ▄▄▄█ 　 ▀▀▀░ ▀▀▀▀ 　 ▄▄▄█ ▀▀▀▀ ░▀▀▀ 　 ▀░░▀ ▀▀▀ ▀░▀ 　 ░▀░▀░ ▀░░▀ ▀░░▀ ░░▀░░ 　 ▀▀▀ ▀▀▀ 

//█▀▀▀ █▀▀█ █▀▀█ █▀▀▄ ▀█ 
//█░▀█ █░░█ █░░█ █░░█ █▀ 
//▀▀▀▀ ▀▀▀▀ ▀▀▀▀ ▀▀▀░ ▄░ 

//▀▀█▀▀ █░░█ █▀▀ █▀▀█ █▀▀ 　 ░▀░ █▀▀ 　 █▀▀█ █▀▀▄ █░░ █░░█ 　 █▀▀█ █▀▀▄ █▀▀ 　 █░░░█ █░░█ █▀▀█ 　 █▀▀ █▀▀█ █▀▀▄ 
//░░█░░ █▀▀█ █▀▀ █▄▄▀ █▀▀ 　 ▀█▀ ▀▀█ 　 █░░█ █░░█ █░░ █▄▄█ 　 █░░█ █░░█ █▀▀ 　 █▄█▄█ █▀▀█ █░░█ 　 █░░ █▄▄█ █░░█ 
//░░▀░░ ▀░░▀ ▀▀▀ ▀░▀▀ ▀▀▀ 　 ▀▀▀ ▀▀▀ 　 ▀▀▀▀ ▀░░▀ ▀▀▀ ▄▄▄█ 　 ▀▀▀▀ ▀░░▀ ▀▀▀ 　 ░▀░▀░ ▀░░▀ ▀▀▀▀ 　 ▀▀▀ ▀░░▀ ▀░░▀ 

//█▀▀▄ █▀▀█ 　 █▀▀▀ █▀▀█ █▀▀█ █▀▀▄ 
//█░░█ █░░█ 　 █░▀█ █░░█ █░░█ █░░█ 
//▀▀▀░ ▀▀▀▀ 　 ▀▀▀▀ ▀▀▀▀ ▀▀▀▀ ▀▀▀░


using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using static VedAstro.Library.EventsChartFactory;

namespace VedAstro.Library
{

    public class ChartOptions(List<AlgorithmFuncs> SelectedAlgorithm)
    {
        public static ChartOptions Empty = new ChartOptions(new List<AlgorithmFuncs>());

        /// <summary>
        /// List of selected algorithms to be used to make summary row
        /// </summary>
        public List<AlgorithmFuncs> SelectedAlgorithm { get; init; } = SelectedAlgorithm;

        public static ChartOptions FromJson(JToken summaryOptionsJson)
        {
            //name list of selected methods
            var algorithmList = summaryOptionsJson["SelectedAlgorithm"];//todo summary algorithm list 

            List<AlgorithmFuncs> selectedAlgorithm = new List<AlgorithmFuncs>();
            foreach (JToken token in algorithmList)
            {
                // Get the method info
                MethodInfo methodInfo = typeof(Algorithm).GetMethod(token.ToString());
                // Create delegate
                AlgorithmFuncs myDelegate = (AlgorithmFuncs)Delegate.CreateDelegate(typeof(AlgorithmFuncs), methodInfo);
                //add to list
                selectedAlgorithm.Add(myDelegate);
            }

            //create new instance
            var tobeReturned = new ChartOptions(selectedAlgorithm);

            return tobeReturned;

        }


        /// <summary>
        /// Parses from "GetGeneralScore,GocharaAshtakvargaBindu"
        /// </summary>
        public static ChartOptions FromUrl(string summaryOptionsUrl)
        {
            //name list of selected methods
            var algorithmList = summaryOptionsUrl.Split(',').ToList();

            List<AlgorithmFuncs> selectedAlgorithms = new List<AlgorithmFuncs>();
            foreach (string algoName in algorithmList)
            {
                // Get the method info
                MethodInfo methodInfo = typeof(Algorithm).GetMethod(algoName);
                // Create delegate
                AlgorithmFuncs myDelegate = (AlgorithmFuncs)Delegate.CreateDelegate(typeof(AlgorithmFuncs), methodInfo);
                //add to list
                selectedAlgorithms.Add(myDelegate);
            }

            //create new instance
            var tobeReturned = new ChartOptions(selectedAlgorithms);

            return tobeReturned;

        }

        public JObject ToJson()
        {
            //create names of algorithm as string list
            var nameList = SelectedAlgorithm.Select(algorithm => algorithm.Method.Name).ToList();

            //convert to json format array
            JArray jArray = new JArray(nameList);

            var temp = new JObject();
            temp[nameof(SelectedAlgorithm)] = jArray;

            return temp;
        }


        /// <summary>
        /// returns list selected algo as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //get names of funcs
            var nameList = SelectedAlgorithm.Select(al => al.Method.Name).ToList();

            return Tools.ListToString(nameList, "-");
        }
    }
}
