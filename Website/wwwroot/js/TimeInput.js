//date input element
const dateInputId = '#DateInput';
const timeInputId = '#TimeInput';
const calendarPickerHolderId = '#CalendarPickerHolder';
const inputPopupEl = document.querySelector(dateInputId);
const timeInputElm = document.querySelector(timeInputId);

//date picker holder element
const calendarDatepickerPopupEl = document.querySelector(calendarPickerHolderId);

// CSS Selector
const calendar = new VanillaCalendar(calendarPickerHolderId, {
    // Options
    date: {
        today: new Date('2022-01-07'),
    },
    settings: {
        selection: {
            time: 24, // or 12
        },
        selected: {
            time: '03:44',
        },
    },
    actions: {
        changeTime(e, time, hours, minutes, keeping) {
            timeInputElm.value = `${hours}:${minutes}`;
        },
        clickDay(e, dates) {
            //if date selected, hide date picker
            if (dates[0]) {
                inputPopupEl.classList.remove('input_focus');//todo maybe not needed
                calendarDatepickerPopupEl.classList.add('visually-hidden');
            }

            //format the selected date for blazor
            const choppedTimeData = dates[0].split("-");
            var year = choppedTimeData[0];
            var month = choppedTimeData[1];
            var day = choppedTimeData[2];
            var formatted = `${day}/${month}/${year}`;
            inputPopupEl.value = formatted;
        },
        //clickMonth(e, month) { },
        //clickYear(e, year) { },
    },
});

//when module is loaded, calendar is initialized but not visible
//click event in blazor will make picker visible
calendar.init();

//handle clicks outside of picker
document.addEventListener('click', autoHidePicker, { capture: true });




//-------------------FUNCTIONS

export function togglePopup(e) {
    const input = e.target.closest(dateInputId);
    const calendar = e.target.closest(calendarPickerHolderId);

    var timeInput = (input && !input.classList.contains('input_focus'));
    if ( timeInput || calendar) {
        inputPopupEl.classList.add('input_focus');
        calendarDatepickerPopupEl.classList.remove('visually-hidden');
    } else {
        inputPopupEl.classList.remove('input_focus');
        calendarDatepickerPopupEl.classList.add('visually-hidden');
    }
};

//if click is outside picker & input then hide it
function autoHidePicker(e) {
    const dateInput = e.target.closest(dateInputId);
    const timeInput = e.target.closest(timeInputId);
    const pickerHolder = e.target.closest(calendarPickerHolderId);

    //if click is not on either inputs then hide picker
    if (!(dateInput || timeInput || pickerHolder)) {
        calendarDatepickerPopupEl.classList.add('visually-hidden');
    }
}

