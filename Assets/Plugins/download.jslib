mergeInto(LibraryManager.library, {
	DownloadFile: function(dataArray, length, fileName) {
		var bytes = new Uint8Array(Module.HEAPU8.buffer, dataArray, length);
		var blob = new Blob([bytes], { type: "image/png" });
		var link = document.createElement('a');
		link.href = window.URL.createObjectURL(blob);
		link.download = UTF8ToString(fileName);
		document.body.appendChild(link);
		link.click();
		document.body.removeChild(link);
	}
});