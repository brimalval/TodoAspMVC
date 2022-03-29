(function (window) {
    function todoListAjaxInit() {
        // TODO : Ajax functions for todo tasks
        $('.page-control').submit(function (e) {
            const form = this;
            const id = form.dataset.id;
            $.ajax({
                type: "GET",
                url: form.action,
                data: $(form).serialize(),
                success: (data) => {
                    $('#todos-'+id).html(data);
                }
            });
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
        delete window.todoListAjaxInit;
    }
    window.todoListAjaxInit = todoListAjaxInit;
})(window)