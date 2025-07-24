$(function () {
    // Rating stars interaction
    const stars = document.querySelectorAll('#rating-stars .fa-star');
    const selectedRating = document.getElementById('selected-rating');

    stars.forEach(star => {
        star.addEventListener('click', function () {
            const value = parseInt(this.getAttribute('data-value'));

            // Update UI stars
            stars.forEach((s, index) => {
                if (index < value) {
                    s.classList.remove('muted-text');
                    s.classList.add('text-warning');
                    console.log('hit here');
                } else {
                    s.classList.remove('text-warning');
                    s.classList.add('muted-text');
                }
            });

            // Update rating display
            selectedRating.textContent = value.toFixed(1);
        });

        star.addEventListener('mouseover', function () {
            const value = parseInt(this.getAttribute('data-value'));
            stars.forEach((s, index) => {
                if (index < value) {
                    s.classList.add('text-warning');
                    s.classList.remove('muted-text');
                } else {
                    s.classList.remove('text-warning');
                    s.classList.add('muted-text');
                }
            });
        });

        star.addEventListener('mouseout', function () {
            const currentRating = parseFloat(selectedRating.textContent);

            stars.forEach((s, index) => {
                if (index < currentRating) {
                    s.classList.add('text-warning');
                } else {
                    s.classList.remove('text-warning');
                    s.classList.add('muted-text');
                }
            });
        });
    });

    // Add to cart button
    $(document).on('click', '#add-to-cart-btn', function () {
        const btn = this;
        const originalText = btn.innerHTML;

        btn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Adding...';
        btn.disabled = true;

        // Simulate API call
        setTimeout(() => {
            btn.innerHTML = '<i class="fas fa-check me-2"></i>Added to Cart!';
            btn.classList.remove('btn-primary');
            btn.classList.add('btn-success');

            // Reset after 2 seconds
            setTimeout(() => {
                btn.innerHTML = originalText;
                btn.classList.remove('btn-success');
                btn.classList.add('btn-primary');
                btn.disabled = false;
            }, 2000);
        }, 1000);
    });
        

    // Wishlist button
    $(document).on('click', '#wishlist-btn', function () {
        const btn = this;
        const icon = btn.querySelector('i');
        const isInWishlist = icon.classList.contains('fa-heart') &&
            !icon.classList.contains('text-danger');

        if (isInWishlist) {
            btn.innerHTML = '<i class="fas fa-heart text-danger me-2"></i> Remove from Wishlist';
        } else {
            btn.innerHTML = '<i class="fas fa-heart me-2"></i> Add to Wishlist';
        }

        // Toggle classes
        icon.classList.toggle('text-danger');

        // Simulate API call
        btn.disabled = true;
        setTimeout(() => {
            btn.disabled = false;
        }, 500);
    });

    // Submit review
    $(document).on('click', '#submit-review-btn', function () {
        const rating = parseFloat(selectedRating.textContent);
        const reviewText = document.getElementById('review-text').value;

        if (rating === 0) {
            alert('Please select a rating before submitting');
            return;
        }

        if (reviewText.trim() === '') {
            alert('Please enter your review text');
            return;
        }

        const btn = this;
        const originalText = btn.innerHTML;
        const bookId = $(btn).data('bookid');

        if (bookId == NaN || bookId == undefined) {
            bookId = parseInt(querySelector.getElementById('#bookId').value)
        }
        console.log(bookId);

        btn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Submitting...';
        btn.disabled = true;

        //Call post method
        //int bookId, int stars, string reviewText
        $.post('/BookReview/SubmitReview',
            { bookId: bookId, stars: rating, reviewText: reviewText },
            function (responseHtml) {
                //Replace the review section
                $('#reviewSection').html(responseHtml);

            }).fail(function () {
                alert("Something went wrong. Try again.");
            });

        setTimeout(() => {
            btn.innerHTML = '<i class="fas fa-check me-2"></i>Review Submitted!';
            btn.classList.remove('btn-primary');
            btn.classList.add('btn-success');

            // Reset after 3 seconds
            setTimeout(() => {
                btn.innerHTML = originalText;
                btn.classList.remove('btn-success');
                btn.classList.add('btn-primary');
                btn.disabled = false;

                // Clear form
                stars.forEach(s => {
                    s.classList.remove('text-warning');
                    s.classList.add('text-muted');
                });
                selectedRating.textContent = '0.0';
                document.getElementById('review-text').value = '';
            }, 3000);
        }, 1500);
    });


    //Like or dislike review
    $(document).on('click', '.review-feedback', function () {
        var message = 'Message was sent successfully';
        const btn = $(this);
        const reviewId = btn.data('id');
        const interactionValue = btn.data('liked')
        const returnUrl = encodeURIComponent(window.location.pathname + window.location.search);
        //trigger ajax post
        $.post('/BookReview/InteracWithComment', { commentId: reviewId, isLiked: interactionValue }, function (response) {
            // Update the UI elements
            if (response.success) {
                // Determine if it's a like or dislike
                const isLike = interactionValue === true || interactionValue === "true";

                // Update the clicked button
                btn.html(`<i class="fas fa-thumbs-${isLike ? 'up' : 'down'} me-1"></i> (${isLike ? response.likes : response.dislikes})`);

                // Update the sibling button (opposite action)
                const sibling = btn.siblings(`.review-feedback[data-liked="${!isLike}"][data-id="${reviewId}"]`);
                if (sibling.length > 0) {
                    sibling.html(`<i class="fas fa-thumbs-${!isLike ? 'up' : 'down'} me-1"></i> (${!isLike ? response.likes : response.dislikes})`);
                }
            }
        }).fail(function (xhr) {

            //console.log(returnUrl);
            alert('redirecting to login page');
            if (xhr.status === 401) {
                // Unauthorized: redirect to login page
                window.location.href = `/Account/Login?ReturnUrl=${returnUrl}`;
            } else {
                alert("Something went wrong. Try again.");
            }
        });
    });
});