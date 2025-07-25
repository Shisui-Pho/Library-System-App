//Handle 'Add to cart' click button
$(function () {
    $(document).on('click', '.ajax-cart-btn', function () {
        const button = $(this);
        const form = button.closest('form');
        const formData = form.serialize();
        const bookId = form.find('[name="CartItem.BookID"]').val();

        button.html('<i class="fas fa-spinner fa-spin me-2"></i>Adding...');
        button.prop('disabled', true);

        const div_tohide = '#' + button.data("origin");
        const div_to_show = '#' + button.data("complementary");

        $.post(form.attr('action'), formData, function (response) {

            setTimeout(() => {

                button.html('<i class="fas fa-check me-2"></i>Added to Cart!');
                button.removeClass('btn-primary').addClass('btn-success');

                //Execute after 2.1 seconds
                setTimeout(() => {
                    if (div_tohide && div_to_show) {
                        $(div_tohide).addClass('add-cart-hidden');
                        $(div_to_show).removeClass('qty-hidden');
                    }

                    $.get('/Cart/GetSideCartHtml', function (html) {
                        updateCorrespondingCartDetails(html, bookId);
                        if (window.location.pathname.toLowerCase().includes('books/bookdetails/')) {
                            button.html('<i class="fas fa-check me-2"></i>Added to Cart!');
                            button.removeClass('btn-primary').addClass('btn-success');
                        }
                    });
                    openSideCart();
                }, 2100);

                setTimeout(() => {
                    if (!window.location.pathname.toLowerCase().includes('books/bookdetails/')) {
                        console.log('potential ending')
                        button.removeClass('btn-success').addClass('btn-primary');
                        button.prop('disabled', false);
                    }
                }, 2000);
            }, 1000);
            $('.cart-count').text(response.cartCount);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            console.error("AJAX failed:", textStatus, errorThrown, jqXHR.responseText);
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



    // For checkout page
    $(function () {
        // Toggle between delivery and pickup sections
        $('.delivery-option').on('change', function () {
            if ($(this).val() === 'Delivery') {
                $('#deliveryAddressSection').removeClass('d-none');
                $('#pickupSection').addClass('d-none');
            } else {
                $('#deliveryAddressSection').addClass('d-none');
                $('#pickupSection').removeClass('d-none');
            }
        });

        // Populate cities when province is selected
        $('#pickupProvince').on('change', function () {
            const province = $(this).val();
            const $citySelect = $('#pickupCity');
            const $pointSelect = $('#pickupPoint');
            const $detailsCard = $('#pickupPointDetails');

            // Reset cities and points
            $citySelect.html('<option value="">Select City</option>');
            $pointSelect.html('<option value="">Select Pickup Point</option>');
            $pointSelect.prop('disabled', true);
            $detailsCard.addClass('d-none');
            $('#selectedPickupPointId').val('');

            if (province) {
                fetch(`/PickupPoint/GetCitiesForProvince?province=${encodeURIComponent(province)}`)
                    .then(response => response.json())
                    .then(cities => {
                        if (cities && cities.length > 0) {
                            $citySelect.prop('disabled', false);
                            $.each(cities, function (i, city) {
                                $citySelect.append(`<option value="${city}">${city}</option>`);
                            });
                        }
                    })
                    .catch(error => console.error('Error fetching cities:', error));
            } else {
                $citySelect.prop('disabled', true);
            }
        });

        // Populate pickup points when city is selected
        $('#pickupCity').on('change', function () {
            const province = $('#pickupProvince').val();
            const city = $(this).val();
            const $pointSelect = $('#pickupPoint');
            const $detailsCard = $('#pickupPointDetails');

            // Reset points
            $pointSelect.html('<option value="">Select Pickup Point</option>');
            $detailsCard.addClass('d-none');
            $('#selectedPickupPointId').val('');

            if (province && city) {
                fetch(`/PickupPoint/GetPickupPoints?province=${encodeURIComponent(province)}&city=${encodeURIComponent(city)}`)
                    .then(response => response.json())
                    .then(points => {
                        if (points && points.length > 0) {
                            $pointSelect.prop('disabled', false);
                            $.each(points, function (i, point) {
                                $pointSelect.append(`<option value="${point.id}">${point.name}</option>`);
                            });
                        }
                    })
                    .catch(error => console.error('Error fetching pickup points:', error));
            } else {
                $pointSelect.prop('disabled', true);
            }
        });

        // Show pickup point details when selected
        $('#pickupPoint').on('change', function () {
            const pointId = $(this).val();
            const $detailsCard = $('#pickupPointDetails');

            if (pointId) {
                fetch(`/PickupPoint/GetPickupPointDetails?id=${pointId}`)
                    .then(response => response.json())
                    .then(point => {
                        if (point) {
                            $('#pickupPointName').text(point.name);
                            $('#pickupPointAddress').text(point.address);
                            $('#pickupPointPhone').text(point.phone);

                            // Format hours
                            const opening = new Date(`2000-01-01T${point.openingTime}`);
                            const closing = new Date(`2000-01-01T${point.closingTime}`);
                            const hours = `${opening.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })} - ${closing.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })}`;
                            $('#pickupPointHours').text(hours);

                            $detailsCard.removeClass('d-none');
                            $('#selectedPickupPointId').val(pointId);
                        }
                    })
                    .catch(error => console.error('Error fetching pickup point details:', error));
            } else {
                $detailsCard.addClass('d-none');
                $('#selectedPickupPointId').val('');
            }
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

function updateCorrespondingCartDetails(html, bookId) {

    //Parse html to jquery html
    const parsedHtml = $(html);

    //Replace side cart
    $('#bookSideCart').html(parsedHtml);

    //Update other ui elements present in the form related to cart
    $('#book-cart-subtotal').html(parsedHtml.find('#sideCartTotal').html());

    //Update the other part of the pages
    $(`#cartItemPrice${bookId}`).html(parsedHtml.find(`#bookPrice${bookId}`).html());
}