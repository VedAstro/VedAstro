namespace API
{
    public static class SkyChartAPI
    {

        /// <summary>
        /// Special method to heavily cache sky chart gif generation,
        /// code might be glued to durable, but speed increased with performance 
        /// </summary>
        //[Function(nameof(GetSkyChartGIFAsync))]
        //public static async Task<byte[]> GetSkyChartGIFAsync([OrchestrationTrigger] TaskOrchestrationContext context)
        //{
        //    var inputRaw = context.GetInput<string>();
        //    var inputParsed = JObject.Parse(inputRaw);
        //    var width = inputParsed["Width"].Value<double>();
        //    var height = inputParsed["Height"].Value<double>();
        //    var callerId = inputParsed["CallerId"].Value<string>();
        //    var time = Time.FromJson(inputParsed["Time"]);


        //    var generateFrames = new List<Task<string>>();

        //    //STAGE 1: get charts as SVG list frames
        //    var startTime = time.SubtractHours(Tools.DaysToHours(15));
        //    var endTime = time.AddHours(Tools.DaysToHours(15));
        //    var timeList = EventManager.GetTimeListFromRange(startTime, endTime, 24); //should be 30 frames
        //    foreach (var frameTime in timeList)
        //    {
        //        //place data needed to make chart into nice package
        //        var chartSpecs = new JObject();
        //        chartSpecs["Time"] = frameTime.ToJson();
        //        chartSpecs["Width"] = width;
        //        chartSpecs["Height"] = width;
        //        chartSpecs["CallerId"] = callerId;
        //        var chartSpecsStr = chartSpecs.ToString(Formatting.None); //convert to string else can't be parsed by JSON.NET

        //        //make ready calls to parallel generate 1 frame of sky chart, not executed here
        //        var subOrchestrationOptions = new SubOrchestrationOptions()
        //        {
        //            InstanceId = callerId
        //        };
        //        Task<string> chartSvgTask = context.CallActivityAsync<string>(nameof(SkyChartManager.GenerateChartAsync), chartSpecsStr, subOrchestrationOptions);
        //        generateFrames.Add(chartSvgTask);
        //    }

        //    //execute now (parallel)
        //    await Task.WhenAll(generateFrames);

        //    List<string> chartSvglist = new List<string>();
        //    foreach (var task in generateFrames)
        //    {
        //        //chart should be ready now
        //        chartSvglist.Add(task.Result);
        //    }


        //    //----------------------
        //    //STAGE 2: Convert SVG to PNG frames
            
        //    var pngFrameListByteTransparent = chartSvglist.Select(x => SvgConverter.Svg2Png(x, (int)width, (int)height)).ToList();
        //    var pngFrameLisWhite = pngFrameListByteTransparent.Select(x => SkyChartManager.TransparencyToWhite((Bitmap)x, ImageFormat.Png)).ToList();
        //    var pngFrameList = pngFrameLisWhite.Select(x => SkyChartManager.ByteArrayToImage(x)).ToList();


        //    //STAGE 3: Make GIF from PNGs
        //    AnimatedGifEncoder e = new AnimatedGifEncoder();
        //    // read file as memorystream
        //    var memStream = new MemoryStream();
        //    e.Start(memStream);
        //    e.SetDelay(700);
        //    //-1:no repeat,0:always repeat
        //    e.SetRepeat(0);
        //    foreach (var pngFrame in pngFrameList)
        //    {
        //        e.AddFrame(pngFrame);
        //    }

        //    var x = e.Output();

        //    e.Finish();

        //    //gif image as bytes
        //    var gifBytes = x.ToArray();

        //    return gifBytes;

        //    //await context.CallActivityAsync("F3", sum);


        //    //----------------



            
        //}
    }
}
