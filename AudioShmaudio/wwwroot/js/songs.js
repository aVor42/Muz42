//Содержит id песен
let currentPlaylist = [];
// Содержит индекс айдишника из плейлиста
let currentSongIndex;
let currentVolume = 0.60;
let audio = new Audio();
let idIntervalTimeline = null;

// Возвращает строку формата мм:сс
function durationFromSeconds(seconds) {
    let minutes = Math.floor(seconds / 60);
    seconds = seconds % 60 >= 10 ? `${Math.floor(seconds % 60)}` : `0${Math.floor(seconds % 60)}`;
    return `${minutes}:${seconds}`
}

// Проиграть песню
function playSong() {
    audio.volume(currentVolume);
    audio.play();
}

// Поставить текущую песню на паузу
function pauseSong() {
    audio.pause();
}

// Задать громкость
function setVolume(volume) {
    currentVolume = volume / 100;
    audio.volume(currentVolume);
}

// new

// Вызывать при нажатии на плей
// Сохраняет все айдишники песен из списка,
// в котором была выбранная песня
function getCurrentPlaylist(button) {
    if ($(button).hasClass('player-button-play'))
        return;

    currentPlaylist = [];

    $('.songs-list').each((index, songList) => {
        if (songList.contains(button)) {
            let songLines = songList.children;

            for (let i = 0; i < songLines.length; ++i) {
                currentPlaylist.push(+songLines[i].getAttribute('data-song-id'));
            }

        }
    });
}

// Возвращает индекс песни в текущем плейлисте
function getPositionInPlaylist(songId) {
    return currentPlaylist.indexOf(songId);
}

// Меняет все отображаемые кнопки play (содержащие songId)
// на кнопки pause
function playToPauseButtons(songId) {

    let normalButton = $(`.normal-button-play[data-song-id="${songId}"]`);
    let reducedButton = $(`.reduced-button-play[data-song-id="${songId}"]`);
    let playerButton = $(`.player-button-play`);

    let artistId = normalButton.attr('data-artist-id');
    let songName = normalButton.attr('data-song-name');
    let artistName = normalButton.attr('data-artist-name');
    if (normalButton.length == 0) {
        artistId = reducedButton.attr('data-artist-id');
        songName = reducedButton.attr('data-song-name');
        artistName = reducedButton.attr('data-artist-name');
    }

    normalButton.parent().html('').
        append(getPauseButtonNormal(songId, artistId, songName, artistName));

    reducedButton.parent().html('').
        append(getPauseButtonReduce(songId, artistId, songName, artistName));

    playerButton.parent().html('').
        append(getPauseButtonPlayer(songId, artistId, songName, artistName));
}

// Меняет все отображаемые кнопки pause (содержащие songId)
// на кнопки play
function pauseToPlayButtons(songId) {

    let normalButton = $(`.normal-button-pause[data-song-id="${songId}"]`);
    let reducedButton = $(`.reduced-button-pause[data-song-id="${songId}"]`);
    let playerButton = $('.player-button-pause');

    let artistId = normalButton.attr('data-artist-id');
    let songName = normalButton.attr('data-song-name');
    let artistName = normalButton.attr('data-artist-name');
    if (normalButton.length == 0) {
        artistId = reducedButton.attr('data-artist-id');
        songName = reducedButton.attr('data-song-name');
        artistName = reducedButton.attr('data-artist-name');
    }

    normalButton.parent().html('').
        append(getPlayButtonNormal(songId, artistId, songName, artistName));

    reducedButton.parent().html('').
        append(getPlayButtonReduce(songId, artistId, songName, artistName));

    playerButton.parent().html('').
        append(getPlayButtonPlayer(songId, artistId, songName, artistName));
}

// Останавливает воспроизведение текущей песни и включает другую
function playOtherSong(currentSongId, nextSongId) {
    pauseSong();
    pauseToPlayButtons(currentSongId);
    stopMovingTimeline();

    $('.player-container').removeAttr('hidden');
    audio = new Howl({
        src: [`song/getaudio?audioId=${nextSongId}`],
        format: ['mp3']
    })
    audio.on('end', endPlaySong);

    audio.on('play', function () { setSongDuration(audio.duration()); });
    

    playSong();
    playToPauseButtons(nextSongId);
    moveTimeline();

    setPlayerImage(getArtistId());
    setPlayerSongName(getSongName(), getArtistName());
}

// Включает текущую песню
function playCurrentSong(currentSongId) {
    playSong();
    playToPauseButtons(currentSongId);
    moveTimeline()
}

// Начинает проигрывание следующей песни
function playNextSong() {
    let nextSongIndex = 0;
    if (currentPlaylist.length > currentSongIndex + 1) {
        nextSongIndex = currentSongIndex + 1;
    }
    playOtherSong(currentPlaylist[currentSongIndex], currentPlaylist[nextSongIndex]);
    currentSongIndex = nextSongIndex;
}

// Начинает проигрывание предыдущей песни
function playPrevSong() {
    let prevSongIndex = currentPlaylist.length - 1;
    if (currentSongIndex > 0) {
        prevSongIndex = currentSongIndex - 1;
    }
    playOtherSong(currentPlaylist[currentSongIndex], currentPlaylist[prevSongIndex]);
    currentSongIndex = prevSongIndex;
}


//-----------------------------------
//---   Обычное представление песен
//-----------------------------------
function getSongViewNormal(songId, artistId, songName, artistName, duration) {

    let button = currentPlaylist[currentSongIndex] != songId ?
        getPlayButtonNormal(songId) : getPauseButtonNormal(songId);
    button.
        attr('data-artist-id', artistId).
        attr('data-song-name', songName).
        attr('data-artist-name', artistName);

    let buttonSpan = $('<span>').
        addClass('play-pause-button').
        append(button);

    let artistRef = $('<span>').
        addClass('ref-artist').
        attr('data-artist-id', artistId).
        attr('onClick', 'refArtistClick(this)').
        text(artistName);

    let nameSpan = $('<span>').
        addClass('artist-song-name').
        append(artistRef).
        append(' - ' + songName);

    let durationSpan = $('<span>').
        addClass('song-duration').
        text(durationFromSeconds(duration));

    return $('<div>').
        addClass('songLine').
        attr('data-song-id', songId).
        append(buttonSpan).
        append(nameSpan).
        append(durationSpan);
}

function getPlayButtonNormal(songId, artistId, songName, artistName) {
    let playImg = $('<img>').
        addClass('img-play-button').
        attr('src', '/img/play_img.png');

    return $('<button>').
        addClass('button-play').
        addClass('normal-button-play').
        attr('data-song-id', songId).
        attr('data-artist-id', artistId).
        attr('data-song-name', songName).
        attr('data-artist-name', artistName).
        append(playImg).
        attr('onClick', 'playButtonClick(this)');
}

function getPauseButtonNormal(songId, artistId, songName, artistName) {
    let pauseImg = $('<img>').
        addClass('img-pause-button').
        attr('src', '/img/pause_img.png');

    return $('<button>').
        addClass('button-pause').
        addClass('normal-button-pause').
        attr('data-song-id', songId).
        attr('data-artist-id', artistId).
        attr('data-song-name', songName).
        attr('data-artist-name', artistName).
        append(pauseImg).
        attr('onClick', 'pauseButtonClick(this)');
}

function getAddSongIcon() {
    return $('<span>').
        addClass('icon-add-remove-song').
        append($('<img>').
            attr('src', '/img/plus.png').
            attr('onclick', 'addSongClick(this)').
            attr('width', 26).
            attr('height', 26));
}

function getRemoveSongIcon() {
    return $('<span>').
        addClass('icon-add-remove-song').
        append($('<img>').
            attr('src', '/img/x.png').
            attr('onclick', 'removeSongClick(this)').
            attr('width', 26).
            attr('height', 26));
}

//-----------------------------------
//---   Уменьшенное представление песен
//-----------------------------------
function getPlayButtonReduce(songId, artistId, songName, artistName) {
    let playImg = $('<img>').
        addClass('img-play-button-reduced').
        attr('src', '/img/play_img.png');

    return $('<button>').
        addClass('button-play').
        addClass('reduced-button-play').
        attr('data-song-id', songId).
        attr('data-artist-id', artistId).
        attr('data-song-name', songName).
        attr('data-artist-name', artistName).
        append(playImg).
        attr('onClick', 'playButtonClick(this)');
}

function getPauseButtonReduce(songId, artistId, songName, artistName) {
    let pauseImg = $('<img>').
        addClass('img-pause-button-reduced').
        attr('src', '/img/pause_img.png');

    return $('<button>').
        addClass('button-pause').
        addClass('reduced-button-pause').
        attr('data-song-id', songId).
        attr('data-artist-id', artistId).
        attr('data-song-name', songName).
        attr('data-artist-name', artistName).
        append(pauseImg).
        attr('onClick', 'pauseButtonClick(this)');
}

//-----------------------
//----  Кнопки плеера
//-----------------------

function getPlayButtonPlayer(songId, artistId, songName, artistName) {

    let playImg = $('<img>').
        addClass('img-player-button-play').
        attr('src', '/img/play_img.png');

    return $('<button>').
        addClass('button-play').
        addClass('player-button-play').
        attr('data-song-id', songId).
        attr('data-artist-id', artistId).
        attr('data-song-name', songName).
        attr('data-artist-name', artistName).
        attr('onClick', 'playButtonClick(this)').
        append(playImg);
}

function getPauseButtonPlayer(songId, artistId, songName, artistName) {
    let playImg = $('<img>').
        addClass('img-player-button-pause').
        attr('src', '/img/pause_img.png');

    return $('<button>').
        addClass('button-play').
        addClass('player-button-pause').
        attr('data-song-id', songId).
        attr('data-artist-id', artistId).
        attr('data-song-name', songName).
        attr('data-artist-name', artistName).
        attr('onClick', 'pauseButtonClick(this)').
        append(playImg);
}

function setSongDuration(seconds) {
    let time = durationFromSeconds(seconds);
    $('#player-song-duration').text(time);
}

function setCurrentDuration(seconds) {
    let time = durationFromSeconds(seconds);
    $('#player-song-duration-current').text(time);
}

function setPlayerImage(artistId) {
    $('.player-song-picture').children().
    attr('src', `/Artist/ArtistPhoto?id=${artistId}`)
}

function setPlayerSongName(songName, artistName) {
    $('#player-song-name').text(artistName + ' - ' + songName);
}

function getPlayerData(attr) {
    return $('.player-button-play').length > 0 ?
        $('.player-button-play').attr(attr) :
        $('.player-button-pause').attr(attr);
}

function getArtistId() {
    return getPlayerData('data-artist-id');
}

function getArtistName() {
    return getPlayerData('data-artist-name');
}

function getSongName() {
    return getPlayerData('data-song-name');
}

//-----------------------
//----  События
//-----------------------

// Нажатие на кнопку play
function playButtonClick(element) {
    let songId = $(element).data('song-id');
    let currentSongId = currentPlaylist[currentSongIndex];

    if (songId != currentSongId) {

        getCurrentPlaylist(element);
        currentSongIndex = getPositionInPlaylist(songId);
        playOtherSong(currentSongId, songId);

    } else {
        playCurrentSong(songId);
    }
}

// Нажатие на кнопку pause
function pauseButtonClick(element) {
    pauseSong();
    stopMovingTimeline();

    let songId = $(element).data('song-id');
    pauseToPlayButtons(songId);
}

// Вызывать при начале проигрывания песни
// Начинает движение таймлайна
function moveTimeline() {
    let elem = $("#timeline-slider");
    idIntervalTimeline = setInterval(frame, 5);
    function frame() {

        let value = audio.seek() * 1000 / audio.duration();
        elem.css('--value', value);
        elem.val(value);
        setCurrentDuration(audio.seek());
    }
}

// Вызывать при нажатии на таймлайн
// Останавливает движение таймлайна
function stopMovingTimeline() {
    if (idIntervalTimeline)
        clearInterval(idIntervalTimeline);
    idIntervalTimeline = null;
}

// Пользоваетель отпускает таймлайн
// таймлайна должен продолжить самостоятельное движение
function TimelineMouseUp(element) {
    let currentTime = ($(element).val() * audio.duration()) / 1000;
    setCurrentTime(currentTime);

    moveTimeline();
}

// Задать проигрываемой песне текущее время
function setCurrentTime(time) {

    audio.seek(time);
}

// Вызывается при завершении проигрывания песни
function endPlaySong() {

    $.post(`Song/IncreaseNumberOfListens?songId=${currentPlaylist[currentSongIndex]}`);

    playNextSong();
}

// Нажатие на название артиста
function refArtistClick(element){
    $.ajax({
        url: `/Artist/ArtistPage?id=${$(element).data('artist-id')}`,
        type: 'GET',
        success: function (data) {
            $('.main-content').html(data);
        },
        error: function () {
            alert('Ошибочка');
        }
    });
}

// Добавление песни в свой плейлист
function addSongClick(element) {
    $.ajax({
        url: `/Song/AddSongIntoPlaylist?songId=${$(element).parent().parent().data('song-id')}`,
        type: 'POST',
        success: function (data) {
            toggleAddRemoveSongIcon($(element).parent(), false);
        },
        error: function () {
            alert('Ошибочка');
        }
    });
}

// Удаление песни из своего плейлиста
function removeSongClick(element) {
    $.ajax({
        url: `/Song/DeleteSongFromPlaylist?songId=${$(element).parent().parent().data('song-id')}`,
        type: 'DELETE',
        success: function (data) {
            toggleAddRemoveSongIcon($(element).parent(), true);
        },
        error: function () {
            alert('Ошибочка');
        }
    });
}

// Меняет значёк добавить/удалить песню
function toggleAddRemoveSongIcon(element, isAdd) {
    let icon = isAdd ? getAddSongIcon() : getRemoveSongIcon();
    $(element).empty().append($(icon).html());
}