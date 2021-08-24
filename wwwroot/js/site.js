function DeleteBlog() {
    let confirmButton = document.getElementById("ConfirmBlogDelete");

    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success btn-sm rounded-pill',
            cancelButton: 'btn btn-danger btn-sm rounded-pill mx-2'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            swalWithBootstrapButtons.fire(
                'Deleted!',
                'The blog has been deleted.',
                'success'
            )
            setTimeout(() => { confirmButton.click(); }, 1000);

        } else if (
            /* Read more about handling dismissals below */
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'The comment is safe :)',
                'error'
            )
        }
    })
    
}

function DeleteComment() {
    let confirmDelete = document.getElementById("confirmDeleteButton");
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success btn-sm rounded-pill',
            cancelButton: 'btn btn-danger btn-sm rounded-pill mx-2'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: 'Are you sure?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            swalWithBootstrapButtons.fire(
                'Deleted!',
                'The comment has been deleted.',
                'success'
            )
            setTimeout(() => {confirmDelete.click();}, 1000);
            
        } else if (
            /* Read more about handling dismissals below */
            result.dismiss === Swal.DismissReason.cancel
        ) {
            swalWithBootstrapButtons.fire(
                'Cancelled',
                'The comment is safe :)',
                'error'
            )
        }
    })
}


function AddTag() {
    let tagList = document.getElementById("TagValues");
    let index = tagList.length;
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
    let tagList = document.getElementById("TagValues");
    let index = tagList.length;
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
    let tagList = document.getElementById("TagValues");
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