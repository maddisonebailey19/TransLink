window.previewImage = function (picNumber, dataUrl) {
    const input = document.getElementById(`picUpload${picNumber}`);
    const preview = document.getElementById(`picPreview${picNumber}`);
    // previousElementSibling may be the + icon span
    const plusIcon = preview.previousElementSibling;

    // If caller provided a dataUrl from Blazor, use it directly
    if (dataUrl) {
        preview.src = dataUrl;
        preview.classList.remove('hidden');
        if (plusIcon) plusIcon.classList.add('hidden');
        return;
    }

    // Otherwise read the file from the input element (fallback)
    const file = input && input.files ? input.files[0] : null;
    if (!file) return;

    const reader = new FileReader();

    reader.onload = function (e) {
        preview.src = e.target.result;
        preview.classList.remove('hidden');
        if (plusIcon) plusIcon.classList.add('hidden');
    };

    reader.readAsDataURL(file);
};