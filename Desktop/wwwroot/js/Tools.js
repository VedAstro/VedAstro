

console.log(`TOOLS - INIT`);

//prints a message to console for developers to see
export function printConsoleMessage() {
    $.get("https://vedastro.org/data/ConsoleGreeting.txt") 
        .done((result) => {
            console.log(result);
        });
}