var ImageManager = {
    preview: function (fileInput, loadSRCName, addLocationID) {
        debugger;
        if (fileInput.files && fileInput.files[0]) {
            var reader = new FileReader();
            reader.onloadend = function (e) {
                $('#' + loadSRCName + '').attr('src', "#");
                $('#' + loadSRCName + '').attr('src', e.target.result);
                $("#" + addLocationID + "Path").val("exist");
            }
            reader.readAsDataURL(fileInput.files[0]);
        }
    },
    preview: function (fileInput, loadSRCName) {
        debugger;
        if (fileInput.files && fileInput.files[0]) {
            var reader = new FileReader();
            reader.onloadend = function (e) {
                $('#' + loadSRCName + '').attr('src', "#");
                $('#' + loadSRCName + '').attr('src', e.target.result);
            }
            reader.readAsDataURL(fileInput.files[0]);
        }
    },

    RemoveContent: function (e, removeID) {//PreviewResult1Image
        $("#Preview" + removeID + "").attr('src', "#");//Result1ImageDetails
        //$("#" + removeID + "Details").val("");
        $("#" + removeID + "").val("");

        e.wrap('<form>').closest('form').get(0).reset();
        e.unwrap();

        // Prevent form submission
        e.stopPropagation();
        e.preventDefault();
    },

    ShowLargeImage: function (imageID) {
        // Get the modal
        var modal = document.getElementById('myModal');

        // Get the image and insert it inside the modal - use its "alt" text as a caption
        var img = document.getElementById(imageID);
        var modalImg = document.getElementById("img01");
        var captionText = document.getElementById("caption");
        img.onclick = function () {
            modal.style.display = "block";
            modalImg.src = this.src;
            captionText.innerHTML = this.alt;
        }

        // Get the <span> element that closes the modal
        var span = document.getElementsByClassName("close")[0];

        // When the user clicks on <span> (x), close the modal
        span.onclick = function () {
            modal.style.display = "none";
        }
        //$("#myModal").modal('show');
    }
}