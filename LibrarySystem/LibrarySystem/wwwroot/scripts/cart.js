$(function () {
    $(document).on('click', '.ajax-cart-btn', function () {
        const button = $(this);
        const form = button.closest('form');
        const formData = form.serialize();

        $.post(form.attr('action'), formData, function (response) {
            $('.cart-count').text(response.cartCount);
            //button.removeClass('button-blue')
                //.addClass('button-red')
                //.text('Remove from cart')
                //.attr('data-action', 'remove');
        }).fail(function () {
            alert("Something went wrong. Try again.");
        });
    });
});