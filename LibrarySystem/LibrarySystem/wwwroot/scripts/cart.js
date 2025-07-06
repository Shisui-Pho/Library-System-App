//Handle 'Add to cart' click button
$(function () {
    // Handle add-to-cart button
    $(document).on('click', '.ajax-cart-btn', function () {
        const button = $(this);
        const form = button.closest('form');
        const formData = form.serialize();

        //For hiding and unhiding
        const div_tohide = '#' + button.data("origin");
        const div_to_show = '#' + button.data("complementary");
        console.log('comming\n\n\n', div_tohide, '\n', div_to_show);
        $.post(form.attr('action'), formData, function (response) {
            $('.cart-count').text(response.cartCount);
            console.log('\n\n\n', div_tohide, '\n', div_to_show);
            if (div_tohide != null & div_to_show != null) {
                $(div_tohide).addClass('add-cart-hidden')
                $(div_to_show).removeClass('qty-hidden')
            }

            $.get('/Cart/GetSideCartHtml', function (html) {
                $('#bookSideCart').html(html);
            });

            openSideCart();

        }).fail(function () {
            alert("Something went wrong. Try again.");
        });
    });

    //Handle remove item from side cart
    $(document).on('click', '.remove-from-cart', function () {
        const bookId = $(this).data('id');
        const div_tohide = '#' + $(this).data("origin");
        const div_to_show = '#' + $(this).data("complementary");

        $.post('/Cart/RemoveFromSideCart', { bookId: bookId }, function (response) {
            $('.cart-count').text(response.cartCount);

            //Unhide controlls
            if (div_tohide != null & div_to_show != null) {
                $(div_to_show).removeClass('add-cart-hidden')
                $(div_tohide).addClass('qty-hidden')
            }

            //Refresh the side cart from the server
            $.get('/Cart/GetSideCartHtml', function (html) {
                $('#bookSideCart').html(html);
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