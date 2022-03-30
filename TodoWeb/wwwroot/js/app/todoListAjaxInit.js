(function (window) {
    function todoListAjaxInit() {
        // TODO : Ajax functions for todo tasks
        function ajaxSubmit(form, type, successCallback, errorCallback) {
            var data = $(form).serialize();
            $.ajax({
                type: type,
                url: form.action,
                data: data,
                success: successCallback,
                error: errorCallback
            });
        }
        $('.list-parent').each(function() {
            const listParent = $(this);
            const pageControl = listParent.find('.page-control');
            const createTodoForm = listParent.find('.todo-creation-form');

            const creationCallback = function (e) {
                const form = this;
                const success = (data) => {
                    refreshTodos();
                }
                ajaxSubmit(form, "POST", success);
                return false;
            }
            $(document).on('submit', '.todo-creation-form', creationCallback);

            const deletionCallback = function (e) {
                const form = this;
                const success = (data) => {
                    refreshTodos();
                }
                if (confirm("Are you sure you want to delete this task?")) {
                    ajaxSubmit(form, "POST", success);
                }
                return false;
            }
            $(document).on('submit', '.delete-todo-form', deletionCallback);

            pageControl.submit(function (e) {
                const form = this;
                const id = form.dataset.id;
                const success = (data) => {
                    $('#todos-'+id).html(data);
                };
                ajaxSubmit(form, "GET", success);
                return false;
            });

            function refreshTodos() {
                pageControl.submit();
            }

            const pageNumber = pageControl.find('.page-number');
            pageNumber.change(function(e) {
                if (this.value < 1) {
                    this.value = 1;
                }
                refreshTodos();
            });

            const nextPageBtn = pageControl.find('.page-next');
            nextPageBtn.click(function(e) {
                pageNumber[0].stepUp();
            });

            const prevPageBtn = pageControl.find('.page-prev');
            prevPageBtn.click(function(e) {
                if (pageNumber.val() > 1) {
                    pageNumber[0].stepDown();
                }
            });

            const pageSize = pageControl.find('.page-size');
            pageSize.change(function(e) {
                pageNumber.value = 1;
                refreshTodos();
            }); 

            const collapseCheckbox = listParent.find('.collapse-checkbox');
            collapseCheckbox.change(function (e) {
                createTodoForm.parent().toggleClass('hidden');
                const icon = $(this).parent().find('.collapse-icon');
                icon.toggleClass('rotate-90');
            });
        });
        delete window.todoListAjaxInit;
    }
    window.todoListAjaxInit = todoListAjaxInit;
})(window)