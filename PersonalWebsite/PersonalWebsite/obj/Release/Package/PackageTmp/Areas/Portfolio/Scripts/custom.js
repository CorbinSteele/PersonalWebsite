function focusModal(tab, amount, duration) {
    var $tab = $(tab);
    var $modal = $('.modal');
    if ($modal.scrollTop() == amount)
        $tab.tab('show');
    else if (tab && $modal.scrollTop() < amount) {
        $tab.on('show.bs.tab', function (e) {
            $modal.animate(
                { scrollTop: amount },
                {
                    duration: duration,
                    start: function () { $tab.off('show.bs.tab'); }
                });
        });
        $tab.tab('show');
    }
    else {
        $modal.animate({ scrollTop: amount },
            {
                duration: duration,
                progress: function (_, __, remainingMS) {
                    if (remainingMS < 200)
                        $tab.tab('show');
                }
            });
    }
    return false;
}

function submitInput() {
    var currentTab = $('#jsExercisesTabContent > .active');
    window[currentTab.attr('data-action')]();
    currentTab.find('.jsOutput').trigger('shown.bs.collapse');
}

function resetInput() {
    var currentTab = $('#jsExercisesTabContent > .active');
    currentTab.find('textarea').val('');
    currentTab.find('.jsOutput').collapse('hide');
}

$(document).ready(function () {
    $(".btn").mouseup(function () {
        $(this).blur();
    });
    $('.auto-grow').on('shown.bs.collapse', function () { $(this).attr('rows', 0); $(this).attr('rows', this.scrollHeight / 20); });
    $('.nav-tabs > li > a ').on('click', function(e) { 
        $('#submitBtn, #resetBtn').attr('disabled', $(this).attr('href') == '#home');
    });
    $(".merged-textarea").each(function(_, mta) {
        var textareas = $(mta).find("textarea");
        var lastTextarea= textareas[textareas.length - 1];
        $(lastTextarea).scroll(function(e) {
            textareas.each(function (_, ta) {
                ta.scrollTop = lastTextarea.scrollTop;
            })
        })
    });
});