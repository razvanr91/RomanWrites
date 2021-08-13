let index = 0;
let tagValues = document.getElementById("TagValues");

function AddTag() {
    var tagEntry = document.getElementById("TagEntry");

    let newOption = new Option(tagEntry.value, tagEntry.value);

    tagValues.options[index++] = newOption;

    tagEntry.value = "";

    return true;
}

function DeleteTag() {
    let tagCount = 1;

    while (tagCount > 0) {
        let selectedIndex = tagValues.selectedIndex;
        if (selectedIndex >= 0) {
            tagValues.options[selectedIndex] = null;
            --tagCount;
        } else {
            tagCount = 0;
        }

        index--;
    }
}

$("form").on("submit", function () {
    $("#TagValues option").prop("selected", "selected");
})