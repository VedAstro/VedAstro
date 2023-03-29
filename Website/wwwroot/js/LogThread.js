
console.log("Log Thread JS Loaded");



self.addEventListener('message', async function (e) {
    //make a log
    await addVisitor(e.data);

    //var message = e.data + ' to myself!';

    //self.postMessage(message);
    //self.close();

});

async function addVisitor(payloadXml) {

    console.log("LogThread > Logging");
    var response = await fetch("https://api.vedastro.org/addvisitor", {
        "headers": { "accept": "*/*", "Connection": "keep-alive", "Content-Type": "text/plain" },
        "body": payloadXml,
        "method": "POST",
        "mode": "no-cors"
    });

    var responseText = await response.text();

    return responseText;

}
