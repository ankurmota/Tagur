function startSearchDownload(elem, id) {

	elem.disabled = true;
	elem.style.color = "#262352";

	var url = "/Home/UploadSearchImageToFolderAsync";

	$.post(url, { id: id }, function (data) {
		elem.style.color = "#FFFFFF";

		toastr.options = {
			"closeButton": false,
			"debug": false,
			"newestOnTop": false,
			"progressBar": false,
			"positionClass": "toast-top-right",
			"preventDuplicates": false,
			"onclick": null,
			"showDuration": "300",
			"hideDuration": "1000",
			"timeOut": "5000",
			"extendedTimeOut": "1000",
			"showEasing": "swing",
			"hideEasing": "linear",
			"showMethod": "fadeIn",
			"hideMethod": "fadeOut"
		}

		toastr.success("Your image has been tagged and uploaded to OneDrive successfully.")

	});


	return false;
}

function startDownload(elem, id) {

	elem.disabled = true;
	elem.style.color = "#262352";

	var url = "/Home/UploadImageToFolderAsync";

	$.post(url, { id: id }, function (data) {
		elem.style.color = "#FFFFFF";

		toastr.options = {
			"closeButton": false,
			"debug": false,
			"newestOnTop": false,
			"progressBar": false,
			"positionClass": "toast-top-right",
			"preventDuplicates": false,
			"onclick": null,
			"showDuration": "300",
			"hideDuration": "1000",
			"timeOut": "5000",
			"extendedTimeOut": "1000",
			"showEasing": "swing",
			"hideEasing": "linear",
			"showMethod": "fadeIn",
			"hideMethod": "fadeOut"
		}

		toastr.success("Your image has been tagged and uploaded to OneDrive successfully.")

	});


	return false;
}

function showTooltip(elem) {

	return false;
}

function testToast() {

	toastr.options = {
		"closeButton": false,
		"debug": false,
		"newestOnTop": false,
		"progressBar": false,
		"positionClass": "toast-top-right",
		"preventDuplicates": false,
		"onclick": null,
		"showDuration": "300",
		"hideDuration": "1000",
		"timeOut": "5000",
		"extendedTimeOut": "1000",
		"showEasing": "swing",
		"hideEasing": "linear",
		"showMethod": "fadeIn",
		"hideMethod": "fadeOut"
	}

	toastr.success("Your image has been tagged and uploaded to OneDrive successfully.")
}

