// Write your Javascript code.

; (function ($) {

    'use strict';

    $('.alert[data-auto-dismiss]').each(function (index, element) {
        var $element = $(element),
            timeout = $element.data('auto-dismiss') || 5000;

        setTimeout(function () {
            //$element.alert('close');
            $element.fadeTo(1000, 500).slideUp(500, function () {
                $element.alert('close');
            });
        }, timeout);
    });

})(jQuery);
