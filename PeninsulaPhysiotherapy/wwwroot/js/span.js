function confirmDelete(id,isDeleteClicked) {
    var deleteSpan = document.getElementById("deleteSpan_${id}");
    var confirmDeleteSpan = document.getElementById("confirmDeleteSpan_${id}");
    if (isDeleteClicked) {
        deleteSpan.style = "display:none";
        confirmDeleteSpan.style = "display:show"
    } else {
        deleteSpan.style = "display:show";
        confirmDeleteSpan.style = "display:none"
    }
}