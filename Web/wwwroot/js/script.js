// Преобразования номера месяца в название
function getMonthName(monthIndex) {
    const monthNames = [
        'Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь',
        'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'
    ];
    return monthNames[monthIndex];
}

// Функция для объединения даты и времени в UTC DateTimeOffset
function combineDateTime(date, time) {
    if (!date || !time) return null;
    const [year, month, day] = date.split('-');
    const [hours, minutes] = time.split(':');
    const localDateTime = new Date(year, month - 1, day, hours, minutes);
    return toUTC(localDateTime).toISOString();
}

// Получение смещения часового пояса клиента в минутах
const clientTimezoneOffset = new Date().getTimezoneOffset();

// Преобразование локальной даты в UTC
function toUTC(localDate) {
    return new Date(localDate.getTime() - clientTimezoneOffset * 60000);
}

// Преобразование UTC в локальную дату
function toLocal(utcDate) {
    return new Date(utcDate.getTime() + clientTimezoneOffset * 60000);
}

// Форматирование даты в строку для input[type="date"]
function formatDateForInput(date) {
    const localDate = toLocal(new Date(date));
    return localDate.toISOString().split('T')[0];
}

// Форматирование времени в строку для input[type="time"]
function formatTimeForInput(dateTimeOffset) {
    if (!dateTimeOffset) return '';
    const localDate = toLocal(new Date(dateTimeOffset));
    return `${String(localDate.getHours()).padStart(2, '0')}:${String(localDate.getMinutes()).padStart(2, '0')}`;
}

// Форматирование времени в строку для вывода
function formatDateForOutput(date) {
    const day = String(date.getDate()).padStart(2, '0');
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const year = date.getFullYear();
    return `${day}.${month}.${year}`;
}

// Преобразование статуса проекта для вывода
function translateStatus(status) {
    const statuses = {
        'Active': 'Активен',
        'Standby': 'Ожидание',
        'Done': 'Завершен',
        'Paid': 'Оплачен'
    };
    return statuses[status] || 'Неизвестный статус';
}


$(document).ready(function() {
    const contentSections = $('.content-section');
    const navLinks = $('.sidebar .nav-link');

    navLinks.on('click', function (event) {
        event.preventDefault();
        const target = $(this).attr('href');
        contentSections.hide();
        $(target).show();
        navLinks.removeClass('active');
        $(this).addClass('active');
    });
    
    navLinks.first().trigger('click');

    $('.close').on('click', function() {
        $(this).closest('.modal').fadeOut();
    });

    $(window).on('click', function(event) {
        if ($(event.target).hasClass('modal')) {
            $(event.target).fadeOut();
        }
    });
});