function addReview(url) {
	$.ajax({
		url: url,
		dataType: 'html',
		method: 'POST',
		data: {
			Comment: $('#commentArea').val(),
			Stars: currentRating
		},
		success: (res) => {
			$('#commentArea').html('');
			window.location.reload();
		},
		error: (err) => {
			console.log(err);
		}
	});

	// Don't submit the form
	return false;
}

const ratingStars = [...document.getElementsByClassName("editable-star")];
const ratingResult = document.querySelector(".rating__res");
printRatingResult(ratingResult);
var currentRating = 1;
function executeRating(stars, result) {
    let starClassActive = "rating-userstar editable-star checked";
    let starClassUnactive = "rating-userstar editable-star";
    let i;
    let m;
    stars.map((star) => {
        star.onclick = () => {
            m == m + 1;
            i = stars.indexOf(star);
            for (s of stars) {
                s.className = starClassUnactive;
            }
            printRatingResult(result, i + 1);
            for (i; i >= 0; --i) {
                stars[i].className = starClassActive;
            }
        };
    });
}
function printRatingResult(result, num = 1) {
    result.textContent = `${num}/5`;
    currentRating = num;
}
executeRating(ratingStars, ratingResult);