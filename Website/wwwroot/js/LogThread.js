

console.log("Log Thread - INIT");


self.addEventListener('message', async function (e) {

    //make a log
    //NOTE: Log Disabled to reduce cost and data not as relevant anymore JUL2023
    //await addVisitor(e.data);

    //var message = e.data + ' to myself!';

    //self.postMessage(message);
    //self.close();

});

async function addVisitor(payloadXml) {

    //console.log("LogThread > Logging");

    //we always use stable api, event in beta mode (stable obvious)
    var response = await fetch("https://api.vedastro.org/addvisitor", {
        "headers": { "accept": "*/*", "Connection": "keep-alive" },
        "body": payloadXml,
        "method": "POST"
    });

    var responseText = await response?.text();

    return responseText ?? "";

}
