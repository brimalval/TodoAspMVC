// Convert a UTC date to a local date.
// Pass the ID of the date field.
(function () {
	var dateDisplayID;
	function adjustDateDisplay(displayID) {
		dateDisplayID = displayID;
		const utcDate = new Date($(dateDisplayID).text());
		const offsetInMinutes = utcDate.getTimezoneOffset();
		const offsetInMilliseconds = offsetInMinutes * 60 * 1000;
		const localDate = new Date(utcDate - offsetInMilliseconds);
		$(dateDisplayID).text(localDate.toLocaleString());
		delete window.adjustDateDisplay;
	}
	window.adjustDateDisplay = adjustDateDisplay;
})();