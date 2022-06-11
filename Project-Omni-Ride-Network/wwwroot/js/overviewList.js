const { param } = require("jquery");

$(() => {
	getVehicleList();

	$('#filterForm').change(() => {
		getVehicleList();
	});
});

function getVehicleList() {
	$.ajax({
		url: '@Url.Action("GetVehicleListView", "Api")',
		dataType: 'html',
		method: 'GET',
		data: {
			// TODO add parameters
			page: findGetParameter("page"),
			searchTxt: null,
			categoryFilter: null,
			brandFilter: null,
			modelFilter: null,
			typeFilter: null,
			minPrice: null,
			maxPrice: null

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