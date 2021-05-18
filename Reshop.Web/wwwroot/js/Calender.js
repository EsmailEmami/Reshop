let datePickers = $(".date-picker");

for (let i = 0; i < datePickers.length; i++) {
    let datePicker = datePickers[i];

    $(datePicker).persianDatepicker({
        inline: false,
        viewMode: "day",
        initialValue: true,
        autoClose: false,
        position: "auto",
        format: "YYYY/MM/DD",
        onlyTimePicker: false,
        onlySelectOnDate: false,
        calendarType: "persian",
        inputDelay: 800,
        observer: false,
        navigator: {
            enabled: true,
            scroll: {
                enabled: true,
            },
            text: {
                btnNextText: "<",
                btnPrevText: ">",
            },
        },
        toolbox: {
            enabled: true,
            calendarSwitch: {
                enabled: false,
                format: "MMMM",
            },
            todayButton: {
                enabled: true,
                text: {
                    fa: "امروز",
                    en: "Today",
                },
            },
            submitButton: {
                enabled: true,
                text: {
                    fa: "تایید",
                    en: "Submit",
                },
            },
            text: {
                btnToday: "امروز",
            },
        },
        timePicker: {
            enabled: false,
            step: 1,
            hour: {
                enabled: true,
                step: null,
            },
            minute: {
                enabled: true,
                step: null,
            },
            second: {
                enabled: false,
            },
            meridian: {
                enabled: false,
            },
        },
        dayPicker: {
            enabled: true,
            titleFormat: "YYYY MMMM",
        },
        monthPicker: {
            enabled: true,
            titleFormat: "YYYY",
        },
        yearPicker: {
            enabled: true,
            titleFormat: "YYYY",
        },
        responsive: true,
    });
}

let dateTimePickers = $(".date-time-picker");

for (let i = 0; i < dateTimePickers.length; i++) {
    let dateTimePicker = dateTimePickers[i];


    $(dateTimePicker).persianDatepicker({
        inline: false,
        viewMode: "day",
        initialValue: true,
        autoClose: false,
        position: "auto",
        format: "HH:mm YYYY/MM/DD",
        onlyTimePicker: false,
        onlySelectOnDate: false,
        calendarType: "persian",
        inputDelay: 800,
        observer: false,
        navigator: {
            enabled: true,
            scroll: {
                enabled: true,
            },
            text: {
                btnNextText: "<",
                btnPrevText: ">",
            },
        },
        toolbox: {
            enabled: true,
            calendarSwitch: {
                enabled: false,
                format: "MMMM",
            },
            todayButton: {
                enabled: true,
                text: {
                    fa: "امروز",
                    en: "Today",
                },
            },
            submitButton: {
                enabled: true,
                text: {
                    fa: "تایید",
                    en: "Submit",
                },
            },
            text: {
                btnToday: "امروز",
            },
        },
        timePicker: {
            enabled: true,
            step: 1,
            hour: {
                enabled: true,
                step: null,
            },
            minute: {
                enabled: true,
                step: null,
            },
            second: {
                enabled: false,
            },
            meridian: {
                enabled: false,
            },
        },
        dayPicker: {
            enabled: true,
            titleFormat: "YYYY MMMM",
        },
        monthPicker: {
            enabled: true,
            titleFormat: "YYYY",
        },
        yearPicker: {
            enabled: true,
            titleFormat: "YYYY",
        },
        responsive: true,
    });
}