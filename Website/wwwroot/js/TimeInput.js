//DESCRIPTION
//This file stores all code fo js date picker (VanillaCalendar)
//To use: first load file via blazor
//then call LoadCalendar
//make sure empty calendar div exists

//date input element
const hourInputId = '#HourInput';
const minuteInputId = '#MinuteInput';
const meridianInputId = '#MeridianInput';
const dateInputId = '#DateInput';
const monthInputId = '#MonthInput';
const yearInputId = '#YearInput';

const calendarPickerHolderId = '#CalendarPickerHolder';

const hourInputElm = document.querySelector(hourInputId);
const minuteInputElm = document.querySelector(minuteInputId);
const meridianInputElm = document.querySelector(meridianInputId);
const dateInputElm = document.querySelector(dateInputId);
const monthInputElm = document.querySelector(monthInputId);
const yearInputElm = document.querySelector(yearInputId);

//date picker holder element
const calendarDatepickerPopupEl = document.querySelector(calendarPickerHolderId);

//sets the input dates and initializes the calendar
export function LoadCalendar(hour12, minute, meridian, date, month, year) {
    // CSS Selector
    const calendar = new VanillaCalendar(calendarPickerHolderId, {
        // Options
        date: {
            //set the date to show when calendar opens
            today: new Date(`${year}-${month}-${date}`),
        },
        settings: {
            range: {
                min: '0001-01-01',
                max: '9999-01-01'
            },
            selection: {
                time: 12, //AM/PM format
            },
            selected: {
                //set the time to show when calendar opens
                time: `${hour12}:${minute} ${meridian}`,
            },
        },
        actions: {
            changeTime(e, time, hours, minutes, keeping) {
                hourInputElm.value = hours;
                minuteInputElm.value = minutes;
                meridianInputElm.value = keeping;
            },
            clickDay(e, dates) {
                //if date selected, hide date picker
                if (dates[0]) {
                    calendarDatepickerPopupEl.classList.add('visually-hidden');
                }

                //format the selected date for blazor
                const choppedTimeData = dates[0].split("-");
                var year = choppedTimeData[0];
                var month = choppedTimeData[1];
                var day = choppedTimeData[2];

                //inject the values into the text input
                dateInputElm.value = day;
                monthInputElm.value = month;
                yearInputElm.value = year;
            },
            //update year & month immediately even though not yet click date
            //allows user to change only month or year
            clickMonth(e, month) { monthInputElm.value = month; },
            clickYear(e, year) { yearInputElm.value = year; }
        },
    });

    //when module is loaded, calendar is initialized but not visible
    //click event in blazor will make picker visible
    calendar.init();

    //handle clicks outside of picker
    document.addEventListener('click', autoHidePicker, { capture: true });
}






//-------------------FUNCTIONS

export function togglePopup(e) {
    const input = e.target.closest(dateInputId);
    const calendar = e.target.closest(calendarPickerHolderId);

    var timeInput = (input && !input.classList.contains('input_focus'));
    if ( timeInput || calendar) {
        calendarDatepickerPopupEl.classList.remove('visually-hidden');
    } else {
        calendarDatepickerPopupEl.classList.add('visually-hidden');
    }
};

//if click is outside picker & input then hide it
function autoHidePicker(e) {

    //check if click was outside input
    const pickerHolder = e.target.closest(calendarPickerHolderId);
    const timeInput = e.target.closest("#TimeInputHolder");

    //if click is not on either inputs then hide picker
    if (!(timeInput || pickerHolder)) {
        calendarDatepickerPopupEl.classList.add('visually-hidden');
    }
}

