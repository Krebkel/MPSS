$(document).ready(function() {
    const contentSections = $('.content-section');
    const navLinks = $('.sidebar .nav-link');

    navLinks.on('click', function (event) {
        event.preventDefault();
        const target = $(this).attr('href');
        contentSections.hide();
        $(target).show();
        navLinks.removeClass('active');
        $(this).addClass('active');
    });
    

    navLinks.first().trigger('click');
});