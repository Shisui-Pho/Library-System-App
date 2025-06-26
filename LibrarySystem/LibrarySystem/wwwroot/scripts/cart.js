//Handle 'Add to cart' click button
$(function () {
    // Handle add-to-cart button
    $(document).on('click', '.ajax-cart-btn', function () {
        const button = $(this);
        const form = button.closest('form');
        const formData = form.serialize();

        $.post(form.attr('action'), formData, function (response) {
            $('.cart-count').text(response.cartCount);

            //Update side cart content
            $.get('/Cart/GetSideCartHtml', function (html) {
                $('#sideCartItems').html(html);
            });

            openSideCart();

        }).fail(function () {
            alert("Something went wrong. Try again.");
        });
    });

    //Handle remove item from side cart
    $(document).on('click', '.remove-from-cart', function () {
        const bookId = $(this).data('id');

        $.post('/Cart/RemoveFromSideCart', { bookId: bookId }, function (response) {
            $('.cart-count').text(response.cartCount);

            //Refresh the side cart from the server
            $.get('/Cart/GetSideCartHtml', function (html) {
                $('#sideCartItems').html(html);
            });
            openSideCart();
        }).fail(function () {
            alert("Something went wrong while removing the item.");
        });
    });
});

//Opens the side cart panel
function openSideCart() {
    document.getElementById("bookSideCart").classList.remove("side-cart-hidden");
}

//Closes the side cart panel
function closeSideCart() {
    document.getElementById("bookSideCart").classList.add("side-cart-hidden");
}