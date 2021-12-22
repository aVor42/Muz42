function dragArtistPhoto(event) {
    event.preventDefault();
};

function dropArtistPhoto(event) {
    $('#input-artist-photo').files = event.dataTransfer.files;


    var selectedFile = event.dataTransfer.files[0];
    var reader = new FileReader();

    var imgtag = document.getElementById("img-artist-photo");
    imgtag.title = selectedFile.name;

    reader.onload = function (event) {
        imgtag.src = event.target.result;
    };

    reader.readAsDataURL(selectedFile);

    event.preventDefault();
}

function setArtistPhotoOnPage(file) {
    alert('Hello');
}