
/*
      Customized JS for initializing Darkmode.js
 */

const options = {
    mixColor: '#fff', // default: '#fff'
    backgroundColor: '#fff',  // default: '#fff'
    buttonColorDark: '#100f2c',  // default: '#100f2c'
    buttonColorLight: '#fff', // default: '#fff'
    saveInCookies: true, // default: true,
    autoMatchOsTheme: false // default: true
}

window.DarkMode = new Darkmode(options);


/*
      Customized JS for handling nav bar
 */

$(function () {
    'use script'

    feather.replace();

    //const sb = new PerfectScrollbar('.sidebar-body', {
    //    suppressScrollX: true
    //});


   //on page load
   //set theme color
    localStorage.setItem('style', "one");
    localStorage.setItem('skin', "red");

    var skinColor = (localStorage.getItem('skin')) ? localStorage.getItem('skin') : 'base';
    var navStyle = (localStorage.getItem('style')) ? localStorage.getItem('style') : 'base';

    // skin
    $('body').attr('class', function (i, c) {
        return c.replace(/(^|\s)skin-\S+/g, '');
    });

    $('body').addClass('skin-' + skinColor);
    $('#navigationSkins a[data-skin=' + skinColor + ']').addClass('active').siblings().removeClass('active');

    //sidebar nav style
    $('.nav-sidebar').attr('class', function (i, c) {
        return c.replace(/(^|\s)style-\S+/g, '');
    });

    if (navStyle !== 'base') {
        $('.nav-sidebar').addClass('style-' + navStyle);
    }

    $('#navigationStyles a[data-style=' + navStyle + ']').addClass('active').siblings().removeClass('active');





    //Event handlers for final nav menu effect

    $('.sidebar').on('mouseenter mouseleave', function (e) {
        var isHover = (e.type === 'mouseenter') ? true : false;

        if ($('.sidebar').hasClass('minimized')) {
            if (isHover) {
                setTimeout(function () {
                    $('.sidebar').addClass('expand');
                }, 300);
            } else {
                $('.sidebar').removeClass('expand');
                $('.sidebar-body').scrollTop(0);
            }
        }
    });

    $('.search-body .form-control').on('focusin focusout', function (e) {
        $(this).parent().removeClass('onhover');

        if (e.type === 'focusin') {
            $(this).parent().addClass('onfocus');
        } else {
            $(this).parent().removeClass('onfocus');
        }
    });

    $('.search-body').on('mouseover mouseleave', function (e) {
        if (!$(this).hasClass('onfocus')) {
            $(this).toggleClass('onhover', e.type === 'mouseover');
        }
    });

    // single level menu
    $('.nav-sidebar > .nav-link').on('click', function (e) {
        e.preventDefault();

        // remove active siblings
        $(this).addClass('active').siblings().removeClass('active');

        // remove active siblings from other nav
        var ss = $(this).closest('.nav-sidebar').siblings('.nav-sidebar');
        var sg = $(this).closest('.nav-group').siblings('.nav-group');

        ss.find('.active').removeClass('active');
        ss.find('.show').removeClass('show');

        sg.find('.active').removeClass('active');
        sg.find('.show').removeClass('show');
    });

    // two level menu
    $('.nav-sidebar .nav-item').on('click', '.nav-link', function (e) {
        e.preventDefault();

        if ($(this).hasClass('with-sub')) {
            $(this).parent().toggleClass('show');
            $(this).parent().siblings().removeClass('show');
        } else {
            $(this).parent().addClass('active').siblings().removeClass('active');
            $(this).parent().siblings().find('.sub-link').removeClass('active');
        }

        var ss = $(this).closest('.nav-sidebar').siblings('.nav-sidebar');
        var sg = $(this).closest('.nav-group').siblings('.nav-group');

        ss.find('.active').removeClass('active');
        ss.find('.show').removeClass('show');

        sg.find('.active').removeClass('active');
        sg.find('.show').removeClass('show');

    });

    $('.nav-sub').on('click', '.sub-link', function (e) {
        e.preventDefault();

        $(this).addClass('active').siblings().removeClass('active');

        $(this).closest('.nav-item').addClass('active').siblings().removeClass('active');
        $(this).closest('.nav-item').siblings().find('.sub-link').removeClass('active');

        $(this).closest('.nav-sidebar').siblings().find('.active').removeClass('active');
        $(this).closest('.nav-group').siblings().find('.active').removeClass('active');
    });

    $('.nav-group-label').on('click', function () {
        $(this).closest('.nav-group').toggleClass('show');
        $(this).closest('.nav-group').siblings().removeClass('show');

    });

    // content menu
    $('#DesktopMenuBtn').on('click', function (e) {
        e.preventDefault();
        $('.sidebar').toggleClass('minimized');

        $('.sidebar-body').scrollTop(0);
    });

    // mobile menu
    $('#MobileMenuBtn').on('click', function (e) {
        e.preventDefault();
        $('body').toggleClass('sidebar-show');
    });
});
