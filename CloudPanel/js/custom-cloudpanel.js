jQuery(document).ready(function () {
    if (jQuery('.nav-horizontal').length === 0) {
        jQuery(".nav li a").each(function () {
            if (location.href.match($(this).attr("href"))) {
                $(this).parent().addClass("active");

                $(this).closest('ul').css("display", "block").closest('li').addClass("nav-active active");
            }
        });
    }
});