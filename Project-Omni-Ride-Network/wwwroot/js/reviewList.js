$(() => {
	getRatingList();
});

function getRatingList() {

	$.ajax({
		url: apiUrl,
		dataType: 'html',
		method: 'GET',
		data: {
			page: findGetParameter("page"),
			starFilter: null,
			sortNewest: true,
			sortByHighestStars: null

		},
		success: (res) => {
			$('#reviewList').html('').html(res);

			$('#nextPageBtn').prop("disabled", dataPage >= dataMaxPage);
			$('#prevPageBtn').prop("disabled", dataPage <= 1);
			$('#pageDisplay').text("Seite " + dataPage + " von " + dataMaxPage);
		},
		error: (err) => {
			console.log(err);
		}
	});
}

function findGetParameter(parameterName) {
	let result = null;
	let tmp = [];
	location.search.substr(1).split("&").forEach((item) => {
		tmp = item.split("=");
		if (tmp[0] === parameterName) result = decodeURIComponent(tmp[1]);
	});
	return result;
}

function getPage() {
	let currentPage = findGetParameter("page");
	if (currentPage != null) {
		return currentPage;
	} else {
		return 1;
	}
}

function switchPage(i) {
	let switchToPage = Number(getPage()) + i;

	if (switchToPage <= 0) {
		switchToPage = 1;
	}
	$('#page')[0].value = switchToPage;
}