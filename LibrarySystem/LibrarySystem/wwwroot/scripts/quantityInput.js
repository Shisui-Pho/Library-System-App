var QtyInput = (function() {
    var $qtyInputs = $(".qty-input");

    if (!$qtyInputs.length) {
        return;
    }

    var $inputs = $qtyInputs.find(".product-qty");
    var $countBtn = $qtyInputs.find(".qty-count");
    var qtyMin = parseInt($inputs.attr("min"));
    var qtyMax = parseInt($inputs.attr("max"));


    $inputs.on('change', function () {
        var $this = $(this);
        var $minusBtn = $this.siblings(".qty-count--minus");
        var $addBtn = $this.siblings(".qty-count--add");
        var qty = parseInt($this.val());

        if (isNaN(qty) || qty <= qtyMin) {
            $this.val(qtyMin);
            $minusBtn.attr("disabled", true);
        }
        else {
            $minusBtn.attr("disabled", false);

            if (qty >= qtyMax) {
                $this.val(qtyMax);
                $addBtn.attr('disabled', true);
            } else {
                $this.val(qty);
                $addBtn.attr('disabled', false);
            }
        }
    });

    $countBtn.on('click', function () {
        var operator = this.dataset.action;
        var $this = $(this);
        var $input = $this.siblings(".product-qty");
        var qty = parseInt($input.val());
        var bookId = $input.data('id');

        if (operator == "add") {
            qty += 1;
            if (qty >= qtyMin + 1) {
                $this.siblings(".qty-count--minus").attr("disabled", false);
            }

            if (qty >= qtyMax) {
                $this.attr("disabled", true);
            }
        } else {
            qty = qty <= qtyMin ? qtyMin : (qty -= 1);

            if (qty == qtyMin) {
                $this.attr("disabled", true);
            }

            if (qty < qtyMax) {
                $this.siblings(".qty-count--add").attr("disabled", false);
            }
        }

        //trigger the ajex function
        $.post('/Cart/UpdateQuantity', { bookId: bookId, quantity: qty }, function (response) {
            $('.cart-count').text(response.cartCount);
            //If the items have have been removed
            if (qty == 0) {
                //Show only 'add to cart' button
                $('#addbookNumber' + String(bookId)).removeClass('add-cart-hidden')
                $('#updatebookNumber' + String(bookId)).addClass('qty-hidden')
                //Update quantity value to 1
                $('.product-qty').val('1');
            }

            //We need to show the updated side chart
            $.get('/Cart/GetSideCartHtml', function (html) {
                $('#bookSideCart').html(html);
            });
            openSideCart();
        }).fail(function () {
            alert('Something went wrong. Try again later.');
        })

        $input.val(qty);
    });
})();