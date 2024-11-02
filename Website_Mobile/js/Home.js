updateHistory();


//on each load of the page, shuffle the quick links so user can see more
shuffleChildren('#QuickLinksHolder');


//function to shuffle randomly the child divs in a parent
function shuffleChildren(parentSelector) {
    const parent = document.querySelector(parentSelector);
    if (!parent) return console.error('Parent element not found');

    // Get the child elements
    const children = Array.from(parent.children);

    // Shuffle function
    const shuffle = (array) => {
        for (let i = array.length - 1; i > 0; i--) {
            const j = Math.floor(Math.random() * (i + 1));
            [array[i], array[j]] = [array[j], array[i]];
        }
        return array;
    };

    // Shuffle the children array
    const shuffledChildren = shuffle(children);

    // Append the shuffled children back to the parent
    shuffledChildren.forEach(child => parent.appendChild(child));
}

