function addModer(event) {
    let userId = $(event).attr('data-user-id');

    $.ajax({
        url: `/Account/AddModer?userId=${userId}`,
        type: 'PUT',
        success: function () {
            $(event).
                removeClass('btn-info').
                addClass('btn-secondary').
                attr('onclick', 'removeModer(this)').
                text('-');
        },
        error: function () {
            alert('Ошибка при добавлении модера');
        }
    })


}

function removeModer(event) {
    let userId = $(event).attr('data-user-id');


    $.ajax({
        url: `/Account/RemoveModer?moderId=${userId}`,
        type: 'PUT',
        success: function () {
            $(event).
                removeClass('btn-secondary').
                addClass('btn-info').
                attr('onclick', 'addModer(this)').
                text('+');
        },
        error: function () {
            alert('Ошибка при удалении модера');
        }
    })

}