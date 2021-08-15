let tagList = document.getElementById("TagValues");
let index = tagList.length;


function AddTag() {
    var tagEntry = document.getElementById("TagEntry");

    let searchResult = search(tagEntry.value);

    if (searchResult != null) {
        // Trigger sweet alert for empty or duplicate input
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            html: `${searchResult}`
        })

    } else {
        // Create new tag
        let newOption = new Option(tagEntry.value, tagEntry.value);

        tagList.options[index++] = newOption;
    }

    tagEntry.value = "";

    return true;

}

function DeleteTag() {
    let tagCount = 1;

    if (tagList.selectedIndex === -1) {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            html: `Please select an option to delete`
        });
        return true;
    }

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

// Function to detect if the input is empty or the tag is a duplicate
function search(str) {
    if (str == "") {
        return "You have not entered any text";
    }

    if (tagList) {
        let options = tagList.options;
        for (let i = 0; i < options.length; i++) {
            if (options[i].value === str) {
                return `<strong>#${str}</strong> already exists in the list.`;
            }
        }
    }
}