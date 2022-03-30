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

        function showErrors(errors) {
            var message = "";
            $.each(errors, function (key, error) {
                message += error + "\n";
            });
            toastr.error(message);
        }

        function findListParent(e) {
            return $(e).parents('.list-parent');
        }

        function findPageControl(e) {
            const listParent = findListParent(e);
            return listParent.find('.page-control');
        }

        function findPageNumber(e) {
            const pageControl = findPageControl(e);
            return pageControl.find('.page-number');
        }

        function refreshTodos(e) {
            const pageControl = findPageControl(e);
            pageControl.submit();
        }

        $(document).on('submit', '.todo-creation-form', function (e) {
            const form = this;
            const success = (data) => {
                toastr.success("Task successfully created!");
                refreshTodos(this);
            }
            const error = (data) => {
                showErrors(data.responseJSON.errors);
            }
            ajaxSubmit(form, "POST", success, error);
            return false;
        });

        $(document).on('submit', '.delete-todo-form', function (e) {
            const form = this;
            const success = (data) => {
                toastr.success("Task successfully deleted!");
                refreshTodos(this);
            }
            const error = (data) => {
                showErrors(data.responseJSON.errors);
            }
            if (confirm("Are you sure you want to delete this task?")) {
                ajaxSubmit(form, "POST", success, error);
            }
            return false;
        });

        $(document).on('submit', '.edit-todo-form', function (e) {
            const form = this;
            const success = (data) => {
                console.log(data);
            };
            ajaxSubmit(form, "POST", success);
            return false;
        });

        $(document).on('change', '.edit-todo-form input, .edit-todo-form select', function (e) {
            console.log(this);
        });

        $(document).on('submit', '.page-control', function (e) {
            const form = this;
            const id = form.dataset.id;
            const success = (data) => {
                $('#todos-'+id).html(data);
            };
            ajaxSubmit(form, "GET", success);
            return false;
        });

        $(document).on('change', '.page-number', function(e) {
            if (this.value < 1) {
                this.value = 1;
            }
            refreshTodos(this);
        });

        $(document).on('click', '.page-next', function (e) {
            const pageControl = findPageControl(this);
            const pageNumber = pageControl.find('.page-number');
            pageNumber[0].stepUp();
        });

        $(document).on('click', '.page-prev', function(e) {
            const pageControl = findPageControl(this);
            const pageNumber = pageControl.find('.page-number');
            if (pageNumber.val() > 1) {
                pageNumber[0].stepDown();
            }
        });

        $(document).on('change', '.page-size', function (e) {
            const pageNumber = findPageNumber(this);
            pageNumber.value = 1;
            refreshTodos(this);
        }); 

        $(document).on('change', '.collapse-checkbox', function (e) {
            const listParent = findListParent(this);
            const createTodoForm = listParent.find('.todo-creation-form');
            createTodoForm.parent().toggleClass('hidden');
            const icon = $(this).parent().find('.collapse-icon');
            icon.toggleClass('rotate-90');
        });
        delete window.todoListAjaxInit;
    }
    window.todoListAjaxInit = todoListAjaxInit;
})(window)