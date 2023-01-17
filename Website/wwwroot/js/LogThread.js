
console.log("Log Thread JS Loaded");



self.addEventListener('message', async function (e) {
    //make a log
    await addVisitor(e.data);

    //var message = e.data + ' to myself!';
    //var result = await addVisitor(e.data);
    //console.log(result);

    //var name = e.data.name;
    //if (name === "addVisitor") {
    //    var result = await addVisitor(e.data.UserId, e.data.VisitorId, e.data.Url, e.data.TimeStamp, e.data.TimeStampServer, e.data.Data);
    //    console.log("DATA FROM LOG THREAD" + result);
    //}

    //self.postMessage(message);
    //self.close();

});

async function addVisitor(payloadXml) {

    console.log("Sending data...");

    var response = await fetch("https://vedastroapi.azurewebsites.net/api/addvisitor", {
        "headers": {
            "accept": "*/*",
            "accept-language": "en-GB,en-US;q=0.9,en;q=0.8",
            "content-type": "plain/text; charset=utf-8",
            "sec-ch-ua": "\"Not?A_Brand\";v=\"8\", \"Chromium\";v=\"108\", \"Google Chrome\";v=\"108\"",
            "sec-ch-ua-mobile": "?0",
            "sec-ch-ua-platform": "\"Windows\"",
            "sec-fetch-dest": "empty",
            "sec-fetch-mode": "cors",
            "sec-fetch-site": "cross-site"
        },
        "referrer": "https://www.vedastro.org/",
        "referrerPolicy": "strict-origin-when-cross-origin",
        "body": payloadXml,
        "method": "POST",
        "mode": "cors",
        "credentials": "omit"
    });

    var responseText = await response.text();

    console.log("Sending complete.");

    return responseText;

}
