(function (window) {

    var debounceOptions;
    function todoCheckerDebounce(options) {
        debounceOptions = options;
        // Keeps track of all the todo items that have changed.
        var updated = new Set();
        // Variable where the delayed Ajax request is stored.
        var debounce = null;
        $('.todo-checker').change(function() {
            var id = $(this).data("id");
            var checker = this;
            if (updated.has(id)) {
                $(checker).removeClass("loading");
                updated.delete(id);
            } else {
                $(checker).addClass("loading");
                updated.add(id);
            }

            // Resets the debounce timer.
            clearTimeout(debounce);
            // Starts a timer before sending an Ajax request.
            debounce = setTimeout(function() {
                if (updated.size == 0) {
                    return;
                }
                $.ajax({
                    type: "POST",
                    url: debounceOptions.url,
                    dataType: "json",
                    data: { 
                        idList: Array.from(updated),
                        __RequestVerificationToken: debounceOptions.token
                    },
                    complete: function() {
                        $('input.loading').each(function(idx, c) {
                            console.log(idx, c);
                            $(c).removeClass("loading");
                            var cId = $(c).data("id");
                            if (c.checked) {
                                $('#title-' + cId).addClass('line-through');
                            } else {
                                $('#title-' + cId).removeClass('line-through');
                            }
                        });
                    }
                });
                updated.clear();
            }, debounceOptions.debounceTime);
        });
        // Prevent access to the function after the first call.
        window.todoCheckerDebounce = null;
    }

    window.todoCheckerDebounce = todoCheckerDebounce;
})(window);