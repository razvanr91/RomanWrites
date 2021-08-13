let tagList = document.getElementById("TagValues");
let index = tagList.length;


function AddTag() {
    var tagEntry = document.getElementById("TagEntry");

    let newOption = new Option(tagEntry.value, tagEntry.value);

    tagList.options[index++] = newOption;

    tagEntry.value = "";

    return true;
}

function DeleteTag() {
    let tagCount = 1;

    while (tagCount > 0) {
        let selectedIndex = tagList.selectedIndex;
        if (selectedIndex >= 0) {
            tagList.options[selectedIndex] = null;
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
