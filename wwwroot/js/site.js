let index = 0;

function AddTag() {
    var tagEntry = document.getElementById("TagEntry");

    let newOption = new Option(tagEntry.value, tagEntry.value);

    document.getElementById("TagValues").options[index++] = newOption;

    tagEntry.value = "";

    return true;
}

function DeleteTag() {

}