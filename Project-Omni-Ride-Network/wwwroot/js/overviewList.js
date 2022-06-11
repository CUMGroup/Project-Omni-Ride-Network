$(() => {
	getVehicleList();
	$('#filterForm').change(getVehicleList);
});

function getVehicleList() {
	$.ajax({
		url: apiUrl,
		dataType: 'html',
		method: 'GET',
		data: {
			page: findGetParameter("page"),
			searchTxt: $('#searchFilter').val(),
			categoryFilter: $('#categoryFilter').val(),
			brandFilter: $('#brandFilter').val(),
			modelFilter: $('#modelFilter').val(),
			typeFilter: $('#typeFilter').val(),
			minPrice: $('#minPriceFilter').val(),
			maxPrice: $('#maxPriceFilter').val()

		},
		success: (res) => {
			$('#listVehicles').html('').html(res);
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