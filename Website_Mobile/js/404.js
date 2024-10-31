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
