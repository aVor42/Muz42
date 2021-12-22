let interval;

function searchInputKeyDown(event) {
    clearInterval(interval);
    interval = setInterval(searchSongs, 1300);

    function searchSongs() {
        clearInterval(interval);
        let searchValue = $('#general-search-input').val();
        $.ajax({
            url: `/Song/GlobalSearchSongs?searchValue=${searchValue}`,
            type: 'GET',
            success: function (data) {
                $('.main-content').html(data);
            },
            error: function () {
                alert('Ошибочка');
            }
        });
    }

}
