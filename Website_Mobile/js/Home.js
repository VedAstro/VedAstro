updateHistory();

new PageTopNavbar("PageTopNavbar");
const links = [
    {
        url: 'Home',
        icon: 'ant-design:home-twotone',
        text: 'Home'
    },
    {
        url: 'MatchChecker',
        icon: 'bi:arrow-through-heart-fill',
        text: 'Match Checker'
    },
    {
        url: 'AIChat',
        icon: 'fluent-mdl2:chat-bot',
        text: 'AI Chat'
    },
    {
        url: 'LifePredictor',
        icon: 'gis:map-time',
        text: 'Life Predictor'
    },
    {
        url: 'MatchFinder',
        icon: 'game-icons:lovers',
        text: 'Match Finder'
    },
    {
        url: 'Horoscope',
        icon: 'fluent:book-star-20-filled',
        text: 'Horoscope'
    },
    {
        url: 'GoodTimeFinder',
        icon: 'svg-spinners:clock',
        text: 'Good Time Finder'
    },
    {
        url: 'APIBuilder',
        icon: 'mdi:cloud-tags',
        text: 'API Builder'
    },
    {
        url: 'Numerology',
        icon: 'fluent:text-number-format-20-filled',
        text: 'Numerology'
    },
    //{
    //    url: 'StarsAboveMe',
    //    icon: 'solar:moon-stars-bold',
    //    text: 'Stars Above Me'
    //}
];

new DesktopSidebar("DesktopSidebarHolder", links);
new PageFooter("PageFooter");



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

