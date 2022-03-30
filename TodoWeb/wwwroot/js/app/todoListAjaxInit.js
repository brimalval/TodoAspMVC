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
        $('.page-control').submit(function (e) {
            const form = this;
            const id = form.dataset.id;
            const success = (data) => {
                $('#todos-'+id).html(data);
            };
            ajaxSubmit(form, "POST", success);
            return false;
        });
        $('.todo-creation-form').submit(function (e) {
            const form = this;
            const data = $(form).serialize();
            const success = (data) => {
                console.log(data);
            }
            ajaxSubmit(form, "POST", success);
            return false;
        });
        function findPageNumber(e) {
            return $(e).parents('.page-control').find('.page-number')[0]
        }
        $('.page-next').click(function(e) {
            if (!this.pageNumber) {
                this.pageNumber = findPageNumber(this);
            }
            this.pageNumber.stepUp();
        });
        $('.page-prev').click(function(e) {
            if (!this.pageNumber) {
                this.pageNumber = findPageNumber(this);
            }
            this.pageNumber.stepDown();
        });
        $('.page-number').change(function(e) {
            if (this.value < 1) {
                this.value = 1;
            }
            const pageControl = $(this).parents('.page-control');
            pageControl.submit();
        });
        $('.page-size').change(function(e) {
            if (!this.pageNumber) {
                this.pageNumber = findPageNumber(this);
            }
            this.pageNumber.value = 1;

            const pageControl = $(this).parents('.page-control');
            pageControl.submit();
        }); 
        $('.collapse-checkbox').change(function (e) {
            const parent = $(this).parent();
            parent.next().toggleClass('hidden');
            const icon = parent.find('.collapse-icon');
            console.log(icon);
            icon.toggleClass('rotate-90');
        });
        delete window.todoListAjaxInit;
    }
    window.todoListAjaxInit = todoListAjaxInit;
})(window)