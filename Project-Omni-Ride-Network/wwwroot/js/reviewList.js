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