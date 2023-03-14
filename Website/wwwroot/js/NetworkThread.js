
//TODO NEED TO MAKE WORK WITH CS JSFetchWrapper
//console.log("Log Thread JS Loaded");

////EVENT LISTENER
//self.addEventListener('message', async function (e) {

//    var method = e.data.method;
//    var callMeBack = e.data.callMeBack; //id of the instance
//    var url = e.data.url;
//    var payload = e.data.payloadXml;

//    var receivedRaw = await postWrapper(url, payload);

//    //todo call direct from here to blazor
//    var data = { "callMeBack": callMeBack, "payload": receivedRaw }
//    self.postMessage(data); //will end up in app.js glue function



//    //switch (method) {
//    //case "POST":
//    //    text = "Banana is good!";
//    //    break;
//    //case "Orange":
//    //    text = "I am not a fan of orange.";
//    //    break;
//    //case "Apple":
//    //    text = "How you like them apples?";
//    //    break;
//    //default:
//    //    text = "I have never heard of that fruit...";
//    //}


//    //var message = e.data + ' to myself!';
//    //var result = await addVisitor(e.data);
//    //console.log(result);

//    //var name = e.data.name;
//    //if (name === "addVisitor") {
//    //    var result = await addVisitor(e.data.UserId, e.data.VisitorId, e.data.Url, e.data.TimeStamp, e.data.TimeStampServer, e.data.Data);
//    //    console.log("DATA FROM LOG THREAD" + result);
//    //}

//    //self.postMessage(message);
//    //self.close();

//});


//async function postWrapper(url, payloadXml) {

//    console.log("NetworkThread > Sending POST request...");

//    var response = await fetch(url, {
//        "headers": { "accept": "*/*", "Connection": "keep-alive", "Content-Type": "text/plain" },
//        "body": payloadXml,
//        "method": "POST",
//        "mode": "no-cors"
//    });

//    var responseText = await response.text();

//    //show data 
//    console.log(responseText);

//    return responseText;
//}