(function (window) {
    function todoListAjaxInit() {
        // TODO : Ajax functions for todo tasks
        function ajaxSubmit(form, successCallback, errorCallback) {
            // Default form behaviors
            successCallback ??= (data) => {
                toastr.success(data.message);
                refreshTodos(form);
            }
            errorCallback ??= (data) => {
                showErrors(data.responseJSON);
            }
            var data = $(form).serialize();
            $.ajax({
                type: form.method,
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

        function findListParent(el) {
            return $(el).parents('.list-parent');
        }

        function findPageControl(el) {
            const listParent = findListParent(el);
            return listParent.find('.page-control');
        }

        function findPageNumber(el) {
            const pageControl = findPageControl(el);
            return pageControl.find('.page-number');
        }

        function refreshTodos(el) {
            const pageControl = findPageControl(el);
            pageControl.submit();
        }

        $(".list-parent").on("submit", ".create-todo-form", function (e) {
            ajaxSubmit(this);
            return false;
        });

        $(".list-parent").on("submit", ".create-status-form", function (e) {
            ajaxSubmit(this);
            return false;
        });

        $(".list-parent").on('submit', '.delete-todo-form', function (e) {
            if (confirm("Are you sure you want to delete this task?")) {
                ajaxSubmit(this);
            }
            return false;
        });

        $(".list-parent").on('submit', '.edit-todo-form', function (e) {
            const form = this;
            const error = (data) => {
                showErrors(data.responseJSON.errors ?? []);
                const form = this;
                // Resetting input fields
                $(form).find('input, textarea').each(function (e) {
                    const inputField = this;
                    inputField.value = inputField.defaultValue;
                });
            };
            ajaxSubmit(form, null, error);
            return false;
        });

        $(".list-parent").on('change', '.edit-todo-form input, .edit-todo-form textarea', function (e) {
            if (this.defaultValue != this.value.trim()) {
                $(this).parents('form').submit();
            }
        });

        $(".list-parent").on('change', '.edit-todo-form select', function (e) {
            if (!this.options[this.selectedIndex].defaultSelected) {
                $(this).parents('form').submit();
            }
        });

        $(".list-parent").on('submit', '.page-control', function (e) {
            const form = this;
            const id = form.dataset.id;
            const success = (data) => {
                $('#todos-'+id).html(data);
            };
            ajaxSubmit(form, success);
            return false;
        });

        $(".list-parent").on('change', '.page-number', function(e) {
            if (this.value < 1) {
                this.value = 1;
            }
            refreshTodos(this);
        });

        $(".list-parent").on('click', '.page-next', function (e) {
            const pageControl = findPageControl(this);
            const pageNumber = pageControl.find('.page-number');
            pageNumber[0].stepUp();
        });

        $(".list-parent").on('click', '.page-prev', function(e) {
            const pageControl = findPageControl(this);
            const pageNumber = pageControl.find('.page-number');
            if (pageNumber.val() > 1) {
                pageNumber[0].stepDown();
            }
        });

        $(".list-parent").on('change', '.page-size', function (e) {
            const pageNumber = findPageNumber(this);
            pageNumber.value = 1;
            refreshTodos(this);
        }); 

        $(".list-parent").on('change', '.collapse-checkbox', function (e) {
            const listParent = findListParent(this);
            const targetForm = $(this).data('targetform');
            const form = listParent.find(targetForm);
            form.parent().toggleClass('hidden');
            const icon = $(this).parent().find('.collapse-icon');
            icon.toggleClass('-rotate-90');
        });
        delete window.todoListAjaxInit;
    }
    window.todoListAjaxInit = todoListAjaxInit;
})(window)