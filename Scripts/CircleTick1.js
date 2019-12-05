//$('#toggle').click(function () {
//    $('.circle-loader').toggleClass('load-complete');
//    $('.checkmark').toggle();
//});

$(document).ready(function () {

    setTimeout(function () {
        $(".check").attr("class", "check check-complete");
        $(".fill").attr("class", "fill fill-complete");
    }, 3000);

    setTimeout(function () {
        $(".check").attr("class", "check check-complete success");
        $(".fill").attr("class", "fill fill-complete success");
        $(".path").attr("class", "path path-complete");
    }, 5000);

    setTimeout(function () {
        $('.main-cont').show();
    },7000)
    setTimeout(function () {
        $(".container2").show();
    }, 9000);
    setTimeout(function () {
        $(".container").show();
    }, 10000);

});