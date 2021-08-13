let index = 0;

function AddTag() {
    var tagEntry = document.getElementById("TagEntry").value;

    let newOption = new Option(tagEntry, tagEntry);

    document.getElementById("TagValues").options[index++] = newOption;

    tagEntry = "";

    return true;
}

function DeleteTag() {

}